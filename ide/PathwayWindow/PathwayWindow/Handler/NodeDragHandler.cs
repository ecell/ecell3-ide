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

using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Nodes;
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.ComponentModel;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Exceptions;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// EcventHandler when the node object is dragged.
    /// </summary>
    public class NodeDragHandler : ObjectDragHandler
    {
        #region Fields
        /// <summary>
        /// the related CanvasControl.
        /// </summary>
        private CanvasControl m_canvas;

        /// <summary>
        /// the delta of moving.
        /// </summary>
        private SizeF m_movingDelta = new SizeF(0, 0);
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
        /// check whether this event is mouse event.
        /// </summary>
        /// <param name="e">event.</param>
        /// <returns>mouse event is true.</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.IsMouseEvent
               && (e.Button == MouseButtons.Left || e.IsMouseEnterOrMouseLeave);
        }

        /// <summary>
        /// event on down the mouse button in canvas.
        /// </summary>
        /// <param name="sender">PathwayView.</param>
        /// <param name="e">PInputEventArgs.</param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);
            if (!(e.PickedNode is PPathwayObject))
                return;
            PPathwayObject obj = e.PickedNode as PPathwayObject;

            if (obj is PPathwayNode)
            {
                foreach (PPathwayObject node in m_canvas.SelectedNodes)
                    node.MemorizePosition();
            }
            else if (obj is PPathwaySystem)
                obj.MemorizePosition();

            //e.Canvas.BackColor = Color.Silver;
            //SetBackToDefault();
            //SetShadeWithoutSystem(m_canvas.GetSurroundingSystemKey(obj.PointF));
        }

        /// <summary>
        /// Set background color of pathway view to default color;
        /// </summary>
        private void SetBackToDefault()
        {
            m_canvas.PCanvas.BackColor = Color.White;
            foreach (PPathwaySystem system in m_canvas.Systems.Values)
                system.BackgroundBrush = null;
        }

        /// <summary>
        /// Set background color of pathway view without given systemName to shading color
        /// </summary>
        /// <param name="systemName">Only this system's color remain default state.</param>
        private void SetShadeWithoutSystem(string systemName)
        {
            if (systemName != null && m_canvas.Systems.ContainsKey(systemName))
            {
                m_canvas.PCanvas.BackColor = Color.Silver;
                m_canvas.Systems[systemName].BackgroundBrush = Brushes.White;

                foreach (PPathwaySystem system in m_canvas.Systems.Values)
                    if (!system.EcellObject.Key.Equals(systemName))
                        system.BackgroundBrush = Brushes.Silver;
            }
            else
            {
                m_canvas.PCanvas.BackColor = Color.White;
                foreach (PPathwaySystem system in m_canvas.Systems.Values)
                    system.BackgroundBrush = Brushes.Silver;
            }
        }

        /// <summary>
        /// event on drag PNode.
        /// </summary>
        /// <param name="sender">PathwayView.</param>
        /// <param name="e">PInputEventArgs.</param>
        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            //e.Canvas.BackColor = Color.Silver;
            //SetBackToDefault();
            if (!(e.PickedNode is PPathwayObject))
                return;

            base.OnDrag(sender, e);
            PointF offset = e.PickedNode.Offset;
            foreach (PPathwayObject obj in m_canvas.SelectedNodes)
            {
                obj.Offset = offset;
                // Move Nodes.
                if (obj is PPathwaySystem)
                {
                    PPathwaySystem system = (PPathwaySystem)obj;
                    // Change color if the system overlaps other system
                    if (m_canvas.DoesSystemOverlaps(system) || !IsInsideRoot(system.Rect))
                        system.IsInvalid = true;
                    else
                        system.IsInvalid = false;
                    foreach (PPathwayObject child in m_canvas.GetAllObjectUnder(system.EcellObject.Key))
                    {
                        child.Offset = offset;
                    }
                }
                obj.Offset = offset;
                obj.Refresh();
            }
        }

        /// <summary>
        /// Whether given rectangle is inside the root system or not.
        /// </summary>
        /// <param name="rectF">a rectangle to be checked.</param>
        /// <returns>True, if the given rectangle is inside the root system.
        ///          False, if the given rectangle is outside the root system.
        /// </returns>
        private bool IsInsideRoot(RectangleF rectF)
        {
            RectangleF rootRect = m_canvas.Systems["/"].Rect;
            if (rootRect.Contains(rectF))
                return true;
            else
                return false;
        }
        /// <summary>
        /// event on end to drag PNode in PathwayView.
        /// </summary>
        /// <param name="sender">Pathwayview.</param>
        /// <param name="e">PInputEventArgs.</param>
        protected override void OnEndDrag(object sender, PInputEventArgs e)
        {
            base.OnEndDrag(sender, e);

            if (e.PickedNode.OffsetX == 0 && e.PickedNode.OffsetY == 0)
                return;

            m_canvas.NotifyMoveObjects();

        }
    }
}
