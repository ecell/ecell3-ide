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

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// default handler.
    /// </summary>
    public class DefaultMouseHandler : PPathwayInputEventHandler
    {
        /// <summary>
        /// The drag start point
        /// </summary>
        private PointF m_startPoint;

        /// <summary>
        /// Rectangle dashed-line area for surrouding object.
        /// </summary>
        private PPath m_selectedPath;

        /// <summary>
        /// Flag to show dragged state.
        /// </summary>
        private bool m_isDragged = false;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="control">The control of PathwayView.</param>
        public DefaultMouseHandler(PathwayControl control)
            : base(control)
        {
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
            m_startPoint = e.Position;
            m_con.MousePosition = e.Position;

            if (!(e.PickedNode is PCamera))
                return;

            m_isDragged = true;
            CanvasControl canvas = m_con.Canvas;
            canvas.NotifyMoveObjects(true);
            canvas.NotifyResetSelect();
            if (e.Button == MouseButtons.Left)
            {
                m_selectedPath = new PPath();
                canvas.ControlLayer.AddChild(m_selectedPath);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                m_con.Canvas.PCanvas.Cursor = new Cursor(new MemoryStream(PathwayResource.move));
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
            if (m_selectedPath != null)
                m_selectedPath.RemoveFromParent();
            m_selectedPath = null;
            m_isDragged = false;
            m_con.Canvas.NotifyMoveObjects(true);
            m_con.Canvas.PCanvas.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Called when the mouse is dragged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDrag(object sender, PInputEventArgs e)
        {
            base.OnMouseDrag(sender, e);
            if (e.Button == MouseButtons.Middle)
            {
                PCamera camera = m_con.Canvas.PCanvas.Camera;
                camera.TranslateViewBy(e.Delta.Width, e.Delta.Height);
                camera.Canvas.Refresh();
                return;
            }
            if (m_selectedPath == null || !m_isDragged)
                return;

            CanvasControl canvas = m_con.Canvas;
            m_selectedPath.Reset();
            RectangleF rect = PathUtil.GetRectangle(m_startPoint, e.Position);
            m_selectedPath.AddRectangle(rect.X, rect.Y, rect.Width, rect.Height);

            // Select object.
            List<PPathwayObject> newlySelectedList = new List<PPathwayObject>();
            foreach (PPathwayObject obj in canvas.GetSurroundedObject(rect))
            {
                if (!obj.Visible)
                    continue;
                newlySelectedList.Add(obj);
            }

            // Add/Remove select for each object.
            List<PPathwayObject> objlist = new List<PPathwayObject>();
            objlist.AddRange(canvas.SelectedNodes);
            foreach (PPathwayObject obj in objlist)
            {
                if (!newlySelectedList.Contains(obj))
                    canvas.NotifyRemoveSelect(obj);
            }
            foreach (PNode obj in newlySelectedList)
            {
                if (!(obj is PPathwayObject))
                    continue;
                PPathwayObject node = (PPathwayObject)obj;
                if(!canvas.SelectedNodes.Contains(node))
                    canvas.NotifyAddSelect(node);
            }
        }

        /// <summary>
        /// Get the flag whether system accept this events.
        /// </summary>
        /// <param name="e">Target events.</param>
        /// <returns>The judgement whether this event is accepted.</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return true;
        }
    }
}