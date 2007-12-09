//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX.Nodes;
using EcellLib.PathwayWindow.Nodes;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// default handler.
    /// </summary>
    public class DefaultMouseHandler : PBasicInputEventHandler
    {
        /// <summary>
        /// The PathwayView instance.
        /// </summary>
        private PathwayControl m_con;

        /// <summary>
        /// The drag start point
        /// </summary>
        private PointF m_startPoint;

        /// <summary>
        /// Rectangle dashed-line area for surrouding object.
        /// </summary>
        private PPath m_selectedPath;

        /// <summary>
        /// One PPathwayObject of currently selected objects.
        /// </summary>
        private PPathwayObject m_lastSelectedObj;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="control">The control of PathwayView.</param>
        public DefaultMouseHandler(PathwayControl control)
        {
            this.m_con = control;
        }
        
        /// <summary>
        /// Called when the mouse is down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);
            // set mouse position
            CanvasControl canvas = m_con.CanvasDictionary[e.Canvas.Name];
            m_startPoint = e.Position;
            m_con.MousePosition = e.Position;
            if (e.PickedNode is PCamera)
            {
                canvas.ClickedNode = null;
                canvas.ResetSelectedObjects();
            }
            if (e.Button == MouseButtons.Left)
            {
                canvas.ResetSelectedObjects();
                m_selectedPath = new PPath();
                canvas.ControlLayer.AddChild(m_selectedPath);
            }
        }

        /// <summary>
        /// Called when the mouse is up.
        /// When this method is called, a sequence for selecting objects finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(object sender, PInputEventArgs e)
        {
            base.OnMouseUp(sender, e);
            if (m_selectedPath != null && m_selectedPath.Parent != null)
                m_selectedPath.Parent.RemoveChild(m_selectedPath);
            m_selectedPath = null;
            m_lastSelectedObj = null;
        }

        /// <summary>
        /// Called when the mouse is dragged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDrag(object sender, PInputEventArgs e)
        {
            base.OnMouseDrag(sender, e);
            if (m_selectedPath == null)
                return;

            CanvasControl canvas = m_con.CanvasDictionary[e.Canvas.Name];
            m_selectedPath.Reset();
            RectangleF rect = PathUtil.GetRectangle(m_startPoint, e.Position);
            m_selectedPath.AddRectangle(rect.X, rect.Y, rect.Width, rect.Height);

            canvas.ResetSelectedObjects();
            PNodeList newlySelectedList = new PNodeList();

            foreach (PLayer layer in canvas.Layers.Values)
            {
                if (!layer.Visible)
                    continue;
                PNodeList list = new PNodeList();
                layer.FindIntersectingNodes(rect, list);
                newlySelectedList.AddRange(list);
            }

            bool isAlreadySelected = false;
            PPathwayNode lastNode = null;

            foreach (PNode node in newlySelectedList)
            {
                if (node is PPathwayNode)
                {
                    lastNode = (PPathwayNode)node;
                    canvas.NotifyAddSelect(
                        lastNode.EcellObject.key,
                        lastNode.EcellObject.type,
                        true);
                }
                if (node == m_lastSelectedObj)
                    isAlreadySelected = true;
            }
            if (!isAlreadySelected && lastNode != null)
            {
                canvas.NotifySelectChanged(lastNode.EcellObject.key, lastNode.EcellObject.type);
                m_lastSelectedObj = lastNode;
            }
        }
    }
}