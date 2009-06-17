﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
            m_obj.Refresh();

            string systemName = m_obj.EcellObject.Key;
            List<PPathwayObject> objList = m_canvas.GetAllEntities();
            // Select PathwayObjects being moved into current system.
            Dictionary<string, PPathwayObject> currentDict = new Dictionary<string, PPathwayObject>();
            // Select PathwayObjects being moved to upper system.
            Dictionary<string, PPathwayObject> beforeDict = new Dictionary<string, PPathwayObject>();
            RectangleF rect = m_obj.Rect;
            foreach (PPathwayObject obj in objList)
            {
                if (rect.Contains(obj.Rect))
                {
                    if (!obj.EcellObject.ParentSystemID.StartsWith(systemName) && !obj.EcellObject.Key.Equals(systemName))
                        currentDict.Add(obj.EcellObject.Type + ":" + obj.EcellObject.Key, obj);
                }
                else
                {
                    if (obj.EcellObject.ParentSystemID.StartsWith(systemName) && !obj.EcellObject.Key.Equals(systemName))
                        beforeDict.Add(obj.EcellObject.Type + ":" + obj.EcellObject.Key, obj);
                }
            }

            // If ID duplication could occurred, system resizing will be aborted
            foreach (PPathwayObject obj in currentDict.Values)
            {
                // Check duplicated object.
                if (obj is PPathwayText)
                    continue;
                if (obj is PPathwaySystem && !m_canvas.Systems.ContainsKey(systemName + Constants.delimiterPath + obj.EcellObject.LocalID))
                    continue;
                else if (obj is PPathwayProcess && !m_canvas.Processes.ContainsKey(systemName + Constants.delimiterColon + obj.EcellObject.LocalID))
                    continue;
                else if (obj is PPathwayVariable && !m_canvas.Variables.ContainsKey(systemName + Constants.delimiterColon + obj.EcellObject.LocalID))
                    continue;
                // If duplicated object exists.
                ResetSystemResize();
                Util.ShowErrorDialog(string.Format(
                        MessageResources.ErrAlrExist,
                        new object[] { obj.EcellObject.LocalID }));
                return;
            }
            string parentKey = m_obj.EcellObject.ParentSystemID;
            foreach (PPathwayObject obj in beforeDict.Values)
            {
                // Check duplicated object.
                if (obj is PPathwayText)
                    continue;
                // Check Outroot
                if(string.IsNullOrEmpty(parentKey))
                {
                    ResetSystemResize();
                    Util.ShowErrorDialog(string.Format(
                            MessageResources.ErrOutRoot,
                            new object[] { obj.EcellObject.LocalID }));
                    return;

                }
                // Check duplilcated key
                if (obj is PPathwaySystem && !m_canvas.Systems.ContainsKey(parentKey + Constants.delimiterPath + obj.EcellObject.LocalID))
                    continue;
                else if (obj is PPathwayProcess && !m_canvas.Processes.ContainsKey(parentKey + Constants.delimiterColon + obj.EcellObject.LocalID))
                    continue;
                else if (obj is PPathwayVariable && !m_canvas.Variables.ContainsKey(parentKey + Constants.delimiterColon + obj.EcellObject.LocalID))
                    continue;
                // If duplicated object exists.
                ResetSystemResize();
                Util.ShowErrorDialog(string.Format(
                        MessageResources.ErrAlrExist,
                        new object[] { obj.EcellObject.LocalID }));
                return;
            }

            // Move objects.
            try
            {
                foreach (PPathwayObject obj in currentDict.Values)
                {
                    if (obj is PPathwayText)
                        continue;
                    string oldKey = obj.EcellObject.Key;
                    string newKey = PathUtil.GetMovedKey(oldKey, parentKey, systemName);
                    // Set node change
                    m_canvas.Control.NotifyDataChanged(oldKey, newKey, obj, true, false);
                }
                foreach (PPathwayObject obj in beforeDict.Values)
                {
                    if (obj is PPathwayText)
                        continue;
                    string oldKey = obj.EcellObject.Key;
                    string newKey = PathUtil.GetMovedKey(oldKey, systemName, parentKey);
                    // Set node change
                    m_canvas.Control.NotifyDataChanged(oldKey, newKey, obj, true, false);
                }

                // Fire DataChanged for child in system.!
                Update();
                m_canvas.NotifyResetSelect();
                ClearSurroundState();

                base.ResizeHandle_MouseUp(sender, e);
            }
            catch (Exception)
            {
                ResetSystemResize();
            }
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
