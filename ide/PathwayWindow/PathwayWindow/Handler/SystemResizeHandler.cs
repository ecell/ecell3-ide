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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo.Event;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// ResizeHandler
    /// </summary>
    public class SystemResizeHandler : PathwayResizeHandler
    {
        #region Field
        /// <summary>
        /// List of PNodes, which are currently surrounded by the system.
        /// </summary>
        protected List<PPathwayObject> m_surroundedBySystem = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public SystemResizeHandler(PPathwayObject obj)
            : base(obj)
        {
            m_minWidth = PPathwaySystem.MIN_WIDTH;
            m_minHeight = PPathwaySystem.MIN_HEIGHT;

        }
        /// <summary>
        /// GetCursor
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private Cursor GetCursor(int pos)
        {
            if (pos == ResizeHandle.NW || pos == ResizeHandle.SE)
                return Cursors.SizeNWSE;
            else if (pos == ResizeHandle.N || pos == ResizeHandle.S)
                return Cursors.SizeNS;
            else if (pos == ResizeHandle.NE || pos == ResizeHandle.SW)
                return Cursors.SizeNESW;
            else if (pos == ResizeHandle.E || pos == ResizeHandle.W)
                return Cursors.SizeWE;
            else
                return Cursors.Arrow;
        }
        #endregion

        #region Method

        /// <summary>
        /// Reset System Resize.
        /// </summary>
        private void ResetSystemResize()
        {
            // Resizing is aborted
            m_obj.ResetPosition();
            ValidateSystem();
            m_obj.RefreshView();
            Update();
            m_canvas.NotifyResetSelect();
            ClearSurroundState();
        }

        /// <summary>
        /// Validate a system. According to result, system.Valid will be changed.
        /// </summary>
        private void ValidateSystem()
        {
            if (m_canvas.DoesSystemOverlaps((PPathwaySystem)m_obj))
                m_obj.Invalid = true;
            else
                m_obj.Invalid = false;
        }

        /// <summary>
        /// Highlights objects currently surrounded by the selected system.
        /// </summary>
        private void RefreshSurroundState()
        {
            ClearSurroundState();
            RectangleF rect = m_obj.Rect;
            m_surroundedBySystem = m_canvas.GetSurroundedObject(rect);
            if (m_surroundedBySystem.Contains(m_obj))
                m_surroundedBySystem.Remove(m_obj);
            foreach (PPathwayObject obj in m_surroundedBySystem)
            {
                obj.Selected = true;
            }
        }

        /// <summary>
        /// Turn off highlight for previously surrounded by system objects, and clear resources for managing
        /// surrounding state.
        /// </summary>
        private void ClearSurroundState()
        {
            if (m_surroundedBySystem == null)
                return;
            foreach (PPathwayObject obj in m_surroundedBySystem)
            {
                obj.Selected = false;
            }
            m_surroundedBySystem = null;
        }
        #endregion

        #region EventHandler for ResizeHandle
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ResizeHandle_MouseDown(object sender, PInputEventArgs e)
        {
            // If selected system overlaps another, reset system region.
            if (m_canvas.DoesSystemOverlaps((PPathwaySystem)m_obj))
            {
                ResetSystemResize();
                return;
            }
            base.ResizeHandle_MouseDown(sender, e);
        }
        /// <summary>
        /// Called when the mouse is up on one of resize handles for a system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ResizeHandle_MouseUp(object sender, PInputEventArgs e)
        {
            RefreshSurroundState();

            // If selected system overlaps another, reset system region.
            if (m_canvas.DoesSystemOverlaps((PPathwaySystem)m_obj))
            {
                ResetSystemResize();
                return;
            }
            base.ResizeHandle_MouseUp(sender, e);
        }

        /// <summary>
        /// Called when the NorthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ResizeHandle_MouseDrag(object sender, PInputEventArgs e)
        {
            RefreshSurroundState();
            base.ResizeHandle_MouseDrag(sender, e);
            ValidateSystem();

            if (e.Modifiers != Keys.Shift)
                return;

            // Resize object under
            float x = m_obj.X;
            float y = m_obj.Y;
            float width = m_obj.Width;
            float height = m_obj.Height;
            float oldX = m_obj.EcellObject.X;
            float oldY = m_obj.EcellObject.Y;
            float oldWidth = m_obj.EcellObject.Width;
            float oldHeight = m_obj.EcellObject.Height;
            float xx = width / oldWidth;
            float yy = height / oldHeight;

            List<PPathwayObject> list = m_canvas.GetAllObjectUnder(m_obj.EcellObject.Key);
            foreach (PPathwayObject obj in list)
            {
                float newX = x + (obj.X - oldX) * xx;
                float newY = y + (obj.Y - oldY) * yy;
                obj.OffsetX = newX - obj.X;
                obj.OffsetY = newY - obj.Y;
                if (obj is PPathwaySystem)
                {
                    obj.Width = obj.Width * xx;
                    obj.Height = obj.Height * yy;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_LostFocus(object sender, PInputEventArgs e)
        {
            ResetSystemResize();
        }
        #endregion
    }
}
