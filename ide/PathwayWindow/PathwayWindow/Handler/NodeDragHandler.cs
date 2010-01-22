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
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System.Drawing;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Event;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// EcventHandler when the node object is dragged.
    /// </summary>
    public class NodeDragHandler : PDragEventHandler
    {
        #region Fields
        /// <summary>
        /// the related CanvasControl.
        /// </summary>
        private CanvasControl m_canvas;
        #endregion

        /// <summary>
        /// constructor with initial parameters.
        /// </summary>
        /// <param name="canvas">canvas control.</param>
        public NodeDragHandler(CanvasControl canvas)
        {
            m_canvas = canvas;
        }

        /// <summary>
        /// event on start drag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            base.OnMouseDown(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartDrag(object sender, PInputEventArgs e)
        {
            if(m_canvas.GetPickedObject(e.Position) == null)
                return;
            base.OnStartDrag(sender, e);
            PointF offset = e.PickedNode.Offset;
            if (offset == PointF.Empty)
                return;
            PPathwayObject obj = (PPathwayObject)e.PickedNode;
            if (!obj.Selected)
                m_canvas.NotifyAddSelect(obj);
            m_canvas.MoveSelectedObjects(offset);
        }

        /// <summary>
        /// event on drag PNode.
        /// </summary>
        /// <param name="sender">PathwayView.</param>
        /// <param name="e">PInputEventArgs.</param>
        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            base.OnDrag(sender, e);
            // Move objects
            PointF offset = e.PickedNode.Offset;
            if (offset == PointF.Empty)
                return;
            PointF canvasPos = e.CanvasPosition;
            RectangleF canvasRectF = e.Canvas.Bounds;
            PPathwayObject obj = (PPathwayObject)e.PickedNode;
            if (!canvasRectF.Contains(canvasPos))
                offset = PointF.Empty;
            else if (!obj.Rect.Contains(e.Position))
                offset = new PointF(e.Position.X - obj.CenterPointF.X, e.Position.Y - obj.CenterPointF.Y);
            if (!obj.Selected)
                m_canvas.NotifyAddSelect(obj);
            m_canvas.MoveSelectedObjects(offset);
        }

        /// <summary>
        /// event on end of dragging PNode in PathwayView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnEndDrag(object sender, PInputEventArgs e)
        {
            base.OnEndDrag(sender, e);
            m_canvas.NotifyMoveObjects(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(object sender, PInputEventArgs e)
        {
            base.OnMouseUp(sender, e);
            m_canvas.NotifyMoveObjects(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return true;
        }
    }
}
