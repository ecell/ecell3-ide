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
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using EcellLib.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using EcellLib.PathwayWindow.Resources;

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// ResizeHandler
    /// </summary>
    public class SystemResizeHandler
    {

        /// <summary>
        /// Half of width of a ResizeHandle
        /// </summary>
        protected readonly float HALF_WIDTH = 10;

        #region Field
        /// <summary>
        /// CanvasControl.
        /// </summary>
        protected CanvasControl m_canvas = null;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources;

        /// <summary>
        /// m_resideHandles contains a list of ResizeHandle for resizing a system.
        /// </summary>
        protected List<ResizeHandle> m_resizeHandles = new List<ResizeHandle>();

        /// <summary>
        /// List of PNodes, which are currently surrounded by the system.
        /// </summary>
        PNodeList m_surroundedBySystem = null;

        /// <summary>
        /// Cursor
        /// </summary>
        private Cursor m_cursor;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas"></param>
        public SystemResizeHandler(CanvasControl canvas)
        {
            this.m_canvas = canvas;
            this.m_resources = m_canvas.Control.Resources;
            // Preparing system resize handlers
            // position of each handle is shown below.
            //  0 | 1 | 2
            // -----------
            //  7 |   | 3
            // -----------
            //  6 | 5 | 4
            for (int m = 0; m < 8; m++)
            {
                ResizeHandle handle = new ResizeHandle();
                handle.MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
                handle.MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
                handle.MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);
                m_resizeHandles.Add(handle);
            }

            m_resizeHandles[0].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNWSE);
            m_resizeHandles[0].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeNW);

            m_resizeHandles[1].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNS);
            m_resizeHandles[1].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeN);

            m_resizeHandles[2].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNESW);
            m_resizeHandles[2].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeNE);

            m_resizeHandles[3].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeWE);
            m_resizeHandles[3].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeE);

            m_resizeHandles[4].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNWSE);
            m_resizeHandles[4].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeSE);

            m_resizeHandles[5].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNS);
            m_resizeHandles[5].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeS);

            m_resizeHandles[6].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNESW);
            m_resizeHandles[6].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeSW);

            m_resizeHandles[7].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeWE);
            m_resizeHandles[7].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeW);
        }
        #endregion

        #region Method
        /// <summary>
        /// Show resize handles for resizing system.
        /// </summary>
        public void ShowResizeHandles()
        {
            UpdateResizeHandlePositions();
            foreach (PNode node in m_resizeHandles)
                m_canvas.ControlLayer.AddChild(node);
        }

        /// <summary>
        /// Hide resize handles for resizing system.
        /// </summary>
        public void HideResizeHandles()
        {
            foreach (PNode handle in m_resizeHandles)
                if (handle.Parent == m_canvas.ControlLayer)
                    m_canvas.ControlLayer.RemoveChild(handle);
        }

        /// <summary>
        /// Reset reside handles' positions.
        /// </summary>
        public void UpdateResizeHandlePositions()
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;

            PointF gP = new PointF(system.X + system.OffsetX, system.Y + system.OffsetY);

            m_resizeHandles[0].SetOffset(gP.X, gP.Y);
            m_resizeHandles[1].SetOffset(gP.X + system.Width / 2f, gP.Y);
            m_resizeHandles[2].SetOffset(gP.X + system.Width, gP.Y);
            m_resizeHandles[3].SetOffset(gP.X + system.Width, gP.Y + system.Height / 2f);
            m_resizeHandles[4].SetOffset(gP.X + system.Width, gP.Y + system.Height);
            m_resizeHandles[5].SetOffset(gP.X + system.Width / 2f, gP.Y + system.Height);
            m_resizeHandles[6].SetOffset(gP.X, gP.Y + system.Height);
            m_resizeHandles[7].SetOffset(gP.X, gP.Y + system.Height / 2f);
        }

        /// <summary>
        /// Reset System Resize.
        /// </summary>
        /// <param name="system"></param>
        private void ResetSystemResize(PPathwaySystem system)
        {
            // Resizing is aborted
            system.ResetPosition();
            system.RefreshView();
            ValidateSystem(system);
            UpdateResizeHandlePositions();
            m_canvas.ResetSelectedObjects();
            ClearSurroundState();
        }

        /// <summary>
        /// Validate a system. According to result, system.Valid will be changed.
        /// </summary>
        /// <param name="system">System to be validated</param>
        private void ValidateSystem(PPathwaySystem system)
        {
            if (m_canvas.DoesSystemOverlaps(system))
                system.IsInvalid = true;
            else
                system.IsInvalid = false;
        }

        /// <summary>
        /// Highlights objects currently surrounded by the selected system.
        /// </summary>
        private void RefreshSurroundState()
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;

            ClearSurroundState();
            m_surroundedBySystem = new PNodeList();
            foreach (PLayer layer in m_canvas.Layers.Values)
            {
                PNodeList list = new PNodeList();
                layer.FindIntersectingNodes(system.Rect, list);
                m_surroundedBySystem.AddRange(list);
            }
            foreach (PNode node in m_surroundedBySystem)
            {
                if (node is PPathwayObject)
                    ((PPathwayObject)node).IsHighLighted = true;
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
            foreach (PNode node in m_surroundedBySystem)
            {
                if (node is PPathwayObject)
                    ((PPathwayObject)node).IsHighLighted = false;
            }
            m_surroundedBySystem = null;
        }
        #endregion

        #region EventHandler for ResizeHandle
        /// <summary>
        /// Called when the mouse is up on one of resize handles for a system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_MouseUp(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            // If selected system overlaps another, reset system region.
            if (m_canvas.DoesSystemOverlaps(system))
            {
                ResetSystemResize(system);
                return;
            }
            system.Refresh();

            string systemName = system.EcellObject.Key;
            List<PPathwayObject> objList = m_canvas.GetAllObjects();
            // Select PathwayObjects being moved into current system.
            Dictionary<string, PPathwayObject> currentDict = new Dictionary<string, PPathwayObject>();
            // Select PathwayObjects being moved to upper system.
            Dictionary<string, PPathwayObject> beforeDict = new Dictionary<string, PPathwayObject>();
            foreach (PPathwayObject obj in objList)
            {
                if (system.Rect.Contains(obj.Rect))
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
                if (obj is PPathwaySystem && !m_canvas.Systems.ContainsKey(systemName + "/" + obj.EcellObject.Name))
                    continue;
                else if (obj is PPathwayProcess && !m_canvas.Processes.ContainsKey(systemName + ":" + obj.EcellObject.Name))
                    continue;
                else if (obj is PPathwayVariable && !m_canvas.Variables.ContainsKey(systemName + ":" + obj.EcellObject.Name))
                    continue;
                // If duplicated object exists.
                ResetSystemResize(system);
                Util.__showErrorDialog(m_resources.GetString(MessageConstants.ErrSameObj));
                return;
            }
            string parentKey = system.EcellObject.ParentSystemID;
            foreach (PPathwayObject obj in beforeDict.Values)
            {
                // Check duplicated object.
                if (obj is PPathwaySystem && !m_canvas.Systems.ContainsKey(parentKey + "/" + obj.EcellObject.Name))
                    continue;
                else if (obj is PPathwayProcess && !m_canvas.Processes.ContainsKey(parentKey + ":" + obj.EcellObject.Name))
                    continue;
                else if (obj is PPathwayVariable && !m_canvas.Variables.ContainsKey(parentKey + ":" + obj.EcellObject.Name))
                    continue;
                // If duplicated object exists.
                ResetSystemResize(system);
                Util.__showErrorDialog(m_resources.GetString(MessageConstants.ErrSameObj));
                return;
            }

            // Move objects.
            foreach (PPathwayObject obj in currentDict.Values)
            {
                string oldKey = obj.EcellObject.Key;
                string newKey = PathUtil.GetMovedKey(oldKey, parentKey, systemName);
                // Set node change
                m_canvas.Control.NotifyDataChanged(oldKey, newKey, obj, true, false);
            }
            foreach (PPathwayObject obj in beforeDict.Values)
            {
                string oldKey = obj.EcellObject.Key;
                string newKey = PathUtil.GetMovedKey(oldKey, systemName, parentKey);
                // Set node change
                m_canvas.Control.NotifyDataChanged(oldKey, newKey, obj, true, false);
            }

            // Fire DataChanged for child in system.!
            UpdateResizeHandlePositions();
            m_canvas.ResetSelectedObjects();
            ClearSurroundState();

            // Update systems
            m_canvas.Control.NotifyDataChanged(
                system.EcellObject.Key,
                system.EcellObject.Key,
                system,
                true,
                true);
        }

        /// <summary>
        /// Called when the mouse is down on one of resize handles for a system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_MouseDown(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            system.MemorizePosition();
        }

        /// <summary>
        /// Called when the mouse is off a resize handle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_MouseLeave(object sender, PInputEventArgs e)
        {
            e.Canvas.Cursor = m_cursor;
        }

        /// <summary>
        /// Resize system
        /// </summary>
        /// <param name="system"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void ResizeSystem(PPathwaySystem system, float x, float y, float width, float height)
        {
            system.X = x;
            system.Y = y;
            system.Width = width;
            system.Height = height;
            system.RefreshView();
            ValidateSystem(system);
            UpdateResizeHandlePositions();
        }

        /// <summary>
        /// Called when the NorthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeNW(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            ResizeHandle handle = (ResizeHandle)e.PickedNode;
            float x = handle.OffsetX;
            float y = handle.OffsetY;
            float width = system.X + system.Width - x;
            float height = system.Y + system.Height - y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ResizeSystem(system, x, y, width, height);
            }
            else
            {
                UpdateResizeHandlePositions();
            }
        }

        /// <summary>
        /// Called when the North resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeN(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            ResizeHandle handle = (ResizeHandle)e.PickedNode;
            float y = handle.OffsetY;
            float height = system.Y + system.Height - y;

            handle.OffsetX = system.X + system.Width / 2f;
            if (height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ResizeSystem(system, system.X, y, system.Width, height);
            }
            else
            {
                UpdateResizeHandlePositions();
            }
        }

        /// <summary>
        /// Called when the NorthEast resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeNE(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            ResizeHandle handle = (ResizeHandle)e.PickedNode;
            float y = handle.OffsetY;
            float width = handle.OffsetX - system.X;
            float height = system.Y + system.Height - y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ResizeSystem(system, system.X, y, width, height);
            }
            else
            {
                UpdateResizeHandlePositions();
            }
        }

        /// <summary>
        /// Called when the East resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeE(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            PPath handle = (PPath)e.PickedNode;
            float width = handle.OffsetX - system.X;

            handle.OffsetY = system.Y + system.Height / 2f;
            if (width > PPathwaySystem.MIN_X_LENGTH)
            {
                ResizeSystem(system, system.X, system.Y, width, system.Height);
            }
            else
            {
                UpdateResizeHandlePositions();
            }
        }

        /// <summary>
        /// Called when the SouthEast resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeSE(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            PPath handle = (PPath)e.PickedNode;
            float width = handle.OffsetX - system.X;
            float height = handle.OffsetY - system.Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ResizeSystem(system, system.X, system.Y, width, height);
            }
            else
            {
                UpdateResizeHandlePositions();
            }
        }

        /// <summary>
        /// Called when the South resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeS(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            PPath handle = (PPath)e.PickedNode;
            float height = handle.OffsetY - system.Y;

            handle.OffsetX = system.X + system.Width / 2f;
            if (height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ResizeSystem(system, system.X, system.Y, system.Width, height);
            }
            else
            {
                UpdateResizeHandlePositions();
            }
        }

        /// <summary>
        /// Called when the SouthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeSW(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            PPath handle = (PPath)e.PickedNode;
            float x = handle.OffsetX;
            float width = system.X + system.Width - handle.OffsetX;
            float height = handle.OffsetY - system.Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ResizeSystem(system, x, system.Y, width, height);
            }
            else
            {
                UpdateResizeHandlePositions();
            }
        }

        /// <summary>
        /// Called when the West resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeW(object sender, PInputEventArgs e)
        {
            PPathwaySystem system = m_canvas.SelectedSystem;
            if (system == null)
                return;
            RefreshSurroundState();

            PPath handle = (PPath)e.PickedNode;
            float x = handle.OffsetX;
            float width = system.X + system.Width - x;

            handle.OffsetY = system.Y + system.Height / 2f;
            if (width > PPathwaySystem.MIN_X_LENGTH)
            {
                ResizeSystem(system, x, system.Y, width, system.Height);
            }
            else
            {
                UpdateResizeHandlePositions();
            }
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_CursorSizeNWSE(object sender, PInputEventArgs e)
        {
            m_cursor = e.Canvas.Cursor;
            e.Canvas.Cursor = Cursors.SizeNWSE;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_CursorSizeNS(object sender, PInputEventArgs e)
        {
            m_cursor = e.Canvas.Cursor;
            e.Canvas.Cursor = Cursors.SizeNS;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_CursorSizeNESW(object sender, PInputEventArgs e)
        {
            m_cursor = e.Canvas.Cursor;
            e.Canvas.Cursor = Cursors.SizeNESW;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_CursorSizeWE(object sender, PInputEventArgs e)
        {
            m_cursor = e.Canvas.Cursor;
            e.Canvas.Cursor = Cursors.SizeWE;
        }
        #endregion

        #region Inner Class
        /// <summary>
        /// ResizeHandle
        /// </summary>
        protected class ResizeHandle : PPath
        {
            /// <summary>
            /// Half of width of a ResizeHandle
            /// </summary>
            protected readonly float HALF_WIDTH = 10;
            /// <summary>
            /// Constructor
            /// </summary>
            public ResizeHandle()
            {
                this.AddInputEventListener(new ObjectDragHandler());
                this.Brush = Brushes.DarkOrange;
                this.Pen = new Pen(Brushes.DarkOliveGreen, 0);
                this.AddRectangle(-1 * HALF_WIDTH,
                                    -1 * HALF_WIDTH,
                                    HALF_WIDTH * 2f,
                                    HALF_WIDTH * 2f);
            }
        }
        #endregion
    }
}
