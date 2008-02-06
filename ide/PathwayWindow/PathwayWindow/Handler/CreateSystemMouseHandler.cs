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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.Piccolo.Util;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.UIComponent;

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// For creating a system.
    /// When [Create System] menu (rectangle on it) is selected in the toolbar, mouse event will be
    /// dealed with this handler.
    /// </summary>
    class CreateSystemMouseHandler : PPathwayInputEventHandler
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        protected CanvasControl m_canvas;
        
        /// <summary>
        /// 
        /// </summary>
        protected PointF m_startPoint;

        /// <summary>
        /// 
        /// </summary>
        protected PPath m_selectedPath;

        /// <summary>
        /// Rectangle, surrounded by mouse.
        /// </summary>
        protected RectangleF m_rect;

        /// <summary>
        /// A system, which surrounds the position where the mouse was pressed down.
        /// </summary>
        protected string m_surSystem;

        /// <summary>
        /// m_invalidPen is used for writing rectangle in which new system will be created.
        /// If surrounding area is too small to create new system, this pen will be used.
        /// Otherwise, m_validPen will be used.
        /// </summary>
        protected Pen m_invalidPen = new Pen(Brushes.Black,1);

        /// <summary>
        /// m_validPen is used for writing rectangle in which new system will be created.
        /// If surrounding area has enough size for creating new system, this pen will be used.
        /// Otherwise, m_invalidPen will be used.
        /// </summary>
        protected Pen m_validPen = new Pen(Brushes.Blue, 5);

        /// <summary>
        /// m_overlapPen is used for writing rectangle in which new system will be created.
        /// If surrounding area has enough size for creating new system but is overlapping other
        /// systems, m_overlapPen will be used.
        /// </summary>
        protected Pen m_overlapPen = new Pen(Brushes.Red, 5);

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control">PathwayControl instance</param>
        public CreateSystemMouseHandler(PathwayControl control)
        {
            this.m_con = control;
        }

        /// <summary>
        /// Get the flag whether system accept this action.
        /// </summary>
        /// <param name="e">PInputEventArgs</param>
        /// <returns>The judgement whether this action is acceped.</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.Button != MouseButtons.Right;
        }

        bool m_isNode = false;
        /// <summary>
        /// Called when the mouse is down on the pathway canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {            
            base.OnMouseDown(sender, e);
            m_canvas = m_con.CanvasDictionary[e.Canvas.Name];

            if (e.PickedNode is PCamera)
            {
                m_surSystem = m_canvas.GetSurroundingSystemKey(e.Position);

                if (string.IsNullOrEmpty(m_surSystem))
                {
                    MessageBox.Show(m_resources.GetString("ErrOutRoot"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                m_startPoint = e.Position;
                m_selectedPath = new PPath();
                e.Canvas.Layer.AddChild(m_selectedPath);
                m_isNode = false;
            }
            else
            {
                m_isNode = true;
            }
        }
        
        /// <summary>
        /// Called when the mouse is being dragged
        /// </summary>
        /// <param name="sender">event sender. maybe, PLayer</param>
        /// <param name="e">event information</param>
        public override void OnMouseDrag(object sender, PInputEventArgs e)
        {
            base.OnMouseDrag(sender, e);
            if (m_isNode)
                return;
            if (m_selectedPath == null)
                return;


            m_selectedPath.Reset();
            
            RectangleF rect = PathUtil.GetRectangle(m_startPoint, e.Position);
            m_selectedPath.AddRectangle(rect.X, rect.Y, rect.Width, rect.Height);
            if (rect.Width < PPathwaySystem.MIN_X_LENGTH || rect.Height < PPathwaySystem.MIN_Y_LENGTH)
            {
                // When mouse surrounding region is smaller than minimum.
                m_selectedPath.Pen = m_invalidPen;
            }
            else if (m_canvas.DoesSystemOverlaps(rect))
            {
                // When mouse surrounding region overlaps other system
                m_selectedPath.Pen = m_overlapPen;
                m_selectedPath.CloseFigure();
                m_selectedPath.AddLine(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                m_selectedPath.CloseFigure();
                m_selectedPath.AddLine(rect.X, rect.Y + rect.Height, rect.X + rect.Width, rect.Y);
            }
            else
            {
                // When a system could be created successfully if mouse is up.
                m_selectedPath.Pen = m_validPen;
            }
            
            PNodeList newlySelectedList = new PNodeList();
            m_canvas.ResetSelectedObjects();
            foreach (PLayer layer in m_canvas.Layers.Values)
            {
                PNodeList list = new PNodeList();
                layer.FindIntersectingNodes(rect, list);
                newlySelectedList.AddRange(list);
            }

            foreach(PNode node in newlySelectedList)
            {
                if (!(node is PPathwayNode))
                    continue;
                m_canvas.AddSelectedNode((PPathwayNode)node);
            }
        }

        public override void OnMouseUp(object sender, PInputEventArgs e)
        {
            if (m_isNode) return;
            base.OnMouseUp(sender, e);

            if (m_startPoint == PointF.Empty || m_selectedPath == null)
            {
                m_startPoint = PointF.Empty;
                return;
            }

            m_selectedPath.Reset();
            if(m_selectedPath != null && m_selectedPath.Parent != null)
            {
                m_selectedPath.Parent.RemoveChild(m_selectedPath);
            }
            m_rect = PathUtil.GetRectangle(m_startPoint, e.Position);

            if (m_rect.Width >= PPathwaySystem.MIN_X_LENGTH && m_rect.Height >= PPathwaySystem.MIN_Y_LENGTH)
            {
                if (m_canvas.DoesSystemOverlaps(m_rect))
                {
                    MessageBox.Show(m_resources.GetString("ErrOverSystem"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Stop);
                    return;
                }

                List<PPathwayObject> newlySelectedList = new List<PPathwayObject>();

                foreach (PLayer layer in m_canvas.Layers.Values)
                {
                    PNodeList list = new PNodeList();
                    layer.FindIntersectingNodes(m_rect, list);
                    foreach (PNode node in list)
                        if (node is PPathwayObject)
                            newlySelectedList.Add((PPathwayObject)node);
                }
                EcellObject eo = m_con.CreateDefaultObject(m_canvas.ModelID, m_surSystem, EcellObject.SYSTEM, false);

                eo.X = m_rect.X;
                eo.Y = m_rect.Y;
                eo.Width = m_rect.Width;
                eo.Height = m_rect.Height;

                m_con.NotifyDataAdd(eo, false);
                TransferNodeToByCreate(eo.Key);
            }
            else
            {
                m_canvas.ResetSelectedObjects();
            }
            m_startPoint = PointF.Empty;
        }
        /// <summary>
        /// Transfer an object from one System/Layer to other System/Layer.
        /// </summary>
        /// <param name="systemName">The name of the system to which object is transfered. If null, obj is
        /// transfered to layer itself</param>
        public void TransferNodeToByCreate(string systemName)
        {
            PPathwaySystem system = m_canvas.Systems[systemName];
            string newKey = null;
            foreach (PPathwayObject obj in m_canvas.GetSystemList())
            {
                if (obj == system || !system.Rect.Contains(obj.Rect))
                    continue;
                if (obj.EcellObject.ParentSystemID.StartsWith(systemName))
                    continue;

                newKey = PathUtil.GetMovedKey(obj.EcellObject.Key, system.EcellObject.ParentSystemID, systemName);
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    newKey,
                    obj,
                    true,
                    false);
            }
            foreach (PPathwayObject obj in m_canvas.GetNodeList())
            {
                if (obj.EcellObject.ParentSystemID.StartsWith(systemName) || !system.Rect.Contains(obj.Rect))
                    continue;

                newKey = PathUtil.GetMovedKey(obj.EcellObject.Key, system.EcellObject.ParentSystemID, systemName);
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    newKey,
                    obj,
                    true,
                    false);
            }
            m_con.NotifyDataChanged(systemName, systemName, system, true, true);

        }
    }
}