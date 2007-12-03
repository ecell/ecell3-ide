using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Util;
using System.Drawing;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using System.Windows.Forms;
using EcellLib.PathwayWindow.Nodes;
using System.ComponentModel;

namespace EcellLib.PathwayWindow.Handler
{
    public class ResizeHandler
    {

        /// <summary>
        /// Half of width of a ResizeHandle
        /// </summary>
        protected readonly float HALF_WIDTH = 10;

        #region Field
        /// <summary>
        /// m_resideHandles contains a list of ResizeHandle for resizing a system.
        /// </summary>
        protected PNodeList m_resizeHandles = new PNodeList();

        /// <summary>
        /// CanvasControl.
        /// </summary>
        protected CanvasControl m_canvas = null;
        
        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        /// <summary>
        /// Used to save upper left point of a system
        /// </summary>
        protected PointF m_upperLeftPoint;

        /// <summary>
        /// Used to save upper right point of a system
        /// </summary>
        protected PointF m_upperRightPoint;

        /// <summary>
        /// Used to save lower right point of a system
        /// </summary>
        protected PointF m_lowerRightPoint;

        /// <summary>
        /// Used to save lower left point of a system
        /// </summary>
        protected PointF m_lowerLeftPoint;
        #endregion

        #region Constructor
        public ResizeHandler(CanvasControl canvas)
        {
            this.m_canvas = canvas;
            // Preparing system handlers
            for (int m = 0; m < 8; m++)
            {
                ResizeHandle handle = new ResizeHandle();
                handle.Brush = Brushes.DarkOrange;
                handle.Pen = new Pen(Brushes.DarkOliveGreen, 1);

                handle.AddRectangle(-1 * HALF_WIDTH,
                                    -1 * HALF_WIDTH,
                                    HALF_WIDTH * 2f,
                                    HALF_WIDTH * 2f);
                m_resizeHandles.Add(handle);
            }

            m_resizeHandles[0].Tag = MovingRestriction.NoRestriction;
            m_resizeHandles[0].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNWSE);
            m_resizeHandles[0].MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
            m_resizeHandles[0].MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
            m_resizeHandles[0].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeNW);
            m_resizeHandles[0].MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);

            m_resizeHandles[1].Tag = MovingRestriction.Vertical;
            m_resizeHandles[1].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNS);
            m_resizeHandles[1].MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
            m_resizeHandles[1].MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
            m_resizeHandles[1].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeN);
            m_resizeHandles[1].MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);

            m_resizeHandles[2].Tag = MovingRestriction.NoRestriction;
            m_resizeHandles[2].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNESW);
            m_resizeHandles[2].MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
            m_resizeHandles[2].MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
            m_resizeHandles[2].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeNE);
            m_resizeHandles[2].MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);

            m_resizeHandles[3].Tag = MovingRestriction.Horizontal;
            m_resizeHandles[3].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeWE);
            m_resizeHandles[3].MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
            m_resizeHandles[3].MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
            m_resizeHandles[3].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeE);
            m_resizeHandles[3].MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);

            m_resizeHandles[4].Tag = MovingRestriction.NoRestriction;
            m_resizeHandles[4].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNWSE);
            m_resizeHandles[4].MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
            m_resizeHandles[4].MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
            m_resizeHandles[4].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeSE);
            m_resizeHandles[4].MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);

            m_resizeHandles[5].Tag = MovingRestriction.Vertical;
            m_resizeHandles[5].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNS);
            m_resizeHandles[5].MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
            m_resizeHandles[5].MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
            m_resizeHandles[5].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeS);
            m_resizeHandles[5].MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);

            m_resizeHandles[6].Tag = MovingRestriction.NoRestriction;
            m_resizeHandles[6].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeNESW);
            m_resizeHandles[6].MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
            m_resizeHandles[6].MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
            m_resizeHandles[6].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeSW);
            m_resizeHandles[6].MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);

            m_resizeHandles[7].Tag = MovingRestriction.Horizontal;
            m_resizeHandles[7].MouseEnter += new PInputEventHandler(ResizeHandle_CursorSizeWE);
            m_resizeHandles[7].MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
            m_resizeHandles[7].MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
            m_resizeHandles[7].MouseDrag += new PInputEventHandler(ResizeHandle_ResizeW);
            m_resizeHandles[7].MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);
        }
        #endregion

        #region Method
        /// <summary>
        /// Show resize handles for resizing system.
        /// </summary>
        public void ShowResizeHandles()
        {
            foreach (PNode node in m_resizeHandles)
            {
                m_canvas.ControlLayer.AddChild(node);
            }
        }

        /// <summary>
        /// Hide resize handles for resizing system.
        /// </summary>
        public void HideResizeHandles()
        {
            foreach (PNode node in m_resizeHandles)
                if (node.Parent == m_canvas.ControlLayer)
                    m_canvas.ControlLayer.RemoveChild(node);
        }

        /// <summary>
        /// Reset reside handles' positions.
        /// </summary>
        public void UpdateResizeHandlePositions()
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;

            PPathwaySystem system = m_canvas.Systems[systemName];
            PointF gP = system.PointF;

            float halfThickness = PPathwaySystem.HALF_THICKNESS;
            m_resizeHandles[0].SetOffset(gP.X + halfThickness, gP.Y + halfThickness);
            m_resizeHandles[1].SetOffset(gP.X + system.Width / 2f, gP.Y + halfThickness);
            m_resizeHandles[2].SetOffset(gP.X + system.Width - halfThickness, gP.Y + halfThickness);
            m_resizeHandles[3].SetOffset(gP.X + system.Width - halfThickness, gP.Y + system.Height / 2f);
            m_resizeHandles[4].SetOffset(gP.X + system.Width - halfThickness, gP.Y + system.Height - halfThickness);
            m_resizeHandles[5].SetOffset(gP.X + system.Width / 2f, gP.Y + system.Height - halfThickness);
            m_resizeHandles[6].SetOffset(gP.X + halfThickness, gP.Y + system.Height - halfThickness);
            m_resizeHandles[7].SetOffset(gP.X + halfThickness, gP.Y + system.Height / 2f);
        }

        /// <summary>
        /// Reset resize handles' positions except one fixedHandle
        /// </summary>
        /// <param name="fixedHandle">this ResizeHandle must not be updated</param>
        private void UpdateResizeHandlePositions(PNode fixedHandle)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            PointF gP = system.PointF;

            float halfOuterRadius = PPathwaySystem.OUTER_RADIUS / 2f;
            float halfThickness = (PPathwaySystem.OUTER_RADIUS - PPathwaySystem.INNER_RADIUS) / 2;
            if (m_resizeHandles[0] != fixedHandle)
                m_resizeHandles[0].SetOffset(gP.X + halfThickness, gP.Y + halfThickness);
            if (m_resizeHandles[1] != fixedHandle)
                m_resizeHandles[1].SetOffset(gP.X + system.Width / 2f, gP.Y + halfThickness);
            if (m_resizeHandles[2] != fixedHandle)
                m_resizeHandles[2].SetOffset(gP.X + system.Width - halfThickness, gP.Y + halfThickness);
            if (m_resizeHandles[3] != fixedHandle)
                m_resizeHandles[3].SetOffset(gP.X + system.Width - halfThickness, gP.Y + system.Height / 2f);
            if (m_resizeHandles[4] != fixedHandle)
                m_resizeHandles[4].SetOffset(gP.X + system.Width - halfThickness, gP.Y + system.Height - halfThickness);
            if (m_resizeHandles[5] != fixedHandle)
                m_resizeHandles[5].SetOffset(gP.X + system.Width / 2f, gP.Y + system.Height - halfThickness);
            if (m_resizeHandles[6] != fixedHandle)
                m_resizeHandles[6].SetOffset(gP.X + halfThickness, gP.Y + system.Height - halfThickness);
            if (m_resizeHandles[7] != fixedHandle)
                m_resizeHandles[7].SetOffset(gP.X + halfThickness, gP.Y + system.Height / 2f);
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
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            // If selected system overlaps another, reset system region.
            if (m_canvas.DoesSystemOverlaps(system.GlobalBounds, systemName))
            {
                m_canvas.ResetSystemResize(system);
                return;
            }
            system.Refresh();

            List<PPathwayObject> objList = m_canvas.GetAllObjects();
            // Select PathwayObjects being moved into current system.
            Dictionary<string, PPathwayObject> currentDict = new Dictionary<string, PPathwayObject>();
            // Select PathwayObjects being moved to upper system.
            Dictionary<string, PPathwayObject> beforeDict = new Dictionary<string, PPathwayObject>();
            foreach (PPathwayObject obj in objList)
            {
                if (system.Rect.Contains(obj.Rect))
                {
                    if (!obj.EcellObject.parentSystemID.StartsWith(systemName) && !obj.EcellObject.key.Equals(systemName))
                        currentDict.Add(obj.EcellObject.type + ":" + obj.EcellObject.key, obj);
                }
                else
                {
                    if (obj.EcellObject.parentSystemID.StartsWith(systemName) && !obj.EcellObject.key.Equals(systemName))
                        beforeDict.Add(obj.EcellObject.type + ":" + obj.EcellObject.key, obj);
                }
            }

            // If ID duplication could occurred, system resizing will be aborted
            foreach (PPathwayObject obj in currentDict.Values)
            {
                // Check duplicated object.
                if (obj is PPathwaySystem && !m_canvas.Systems.ContainsKey(systemName + "/" + obj.EcellObject.name))
                    continue;
                else if (obj is PPathwayProcess && !m_canvas.Processes.ContainsKey(systemName + ":" + obj.EcellObject.name))
                    continue;
                else if (obj is PPathwayVariable && !m_canvas.Variables.ContainsKey(systemName + ":" + obj.EcellObject.name))
                    continue;
                // If duplicated object exists.
                m_canvas.ResetSystemResize(system);
                MessageBox.Show(m_resources.GetString("ErrSameObj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string parentKey = system.EcellObject.parentSystemID;
            foreach (PPathwayObject obj in beforeDict.Values)
            {
                // Check duplicated object.
                if (obj is PPathwaySystem && !m_canvas.Systems.ContainsKey(parentKey + "/" + obj.EcellObject.name))
                    continue;
                else if (obj is PPathwayProcess && !m_canvas.Processes.ContainsKey(parentKey + ":" + obj.EcellObject.name))
                    continue;
                else if (obj is PPathwayVariable && !m_canvas.Variables.ContainsKey(parentKey + ":" + obj.EcellObject.name))
                    continue;
                // If duplicated object exists.
                m_canvas.ResetSystemResize(system);
                MessageBox.Show(m_resources.GetString("ErrSameObj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Move objects.
            foreach (PPathwayObject obj in currentDict.Values)
            {
                string oldKey = obj.EcellObject.key;
                string oldSyskey = obj.EcellObject.parentSystemID;
                string newKey = obj.EcellObject.key.Replace(obj.EcellObject.parentSystemID, systemName);
                // Set node change
                m_canvas.PathwayControl.NotifyDataChanged(oldKey, newKey, obj, true, true);
            }
            foreach (PPathwayObject obj in beforeDict.Values)
            {
                string oldKey = obj.EcellObject.key;
                string newKey = obj.EcellObject.key.Replace(systemName, parentKey);
                // Set node change
                m_canvas.PathwayControl.NotifyDataChanged(oldKey, newKey, obj, true, true);
            }

            // Fire DataChanged for child in system.!
            UpdateResizeHandlePositions();
            m_canvas.ResetSelectedObjects();
            m_canvas.ClearSurroundState();

            // Update systems
            m_canvas.PathwayControl.NotifyDataChanged(
                system.EcellObject.key,
                system.EcellObject.key,
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
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            system.MemorizePosition();
            m_upperLeftPoint = system.PointF;
            m_upperRightPoint = new PointF(system.X + system.Width, system.Y);
            m_lowerRightPoint = new PointF(system.X + system.Width, system.Y + system.Height);
            m_lowerLeftPoint = new PointF(system.X, system.Y + system.Height);
        }

        /// <summary>
        /// Called when the mouse is off a resize handle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_MouseLeave(object sender, PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Called when the NorthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeNW(object sender, PInputEventArgs e)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            float X = e.PickedNode.X + e.PickedNode.OffsetX + HALF_WIDTH - PPathwaySystem.HALF_THICKNESS;
            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + HALF_WIDTH - PPathwaySystem.HALF_THICKNESS;
            float width = m_lowerRightPoint.X - X;
            float height = m_lowerRightPoint.Y - Y;
            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                system.X = X;
                system.Y = Y;
                system.Width = width;
                system.Height = height;

                m_canvas.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                if (width <= PPathwaySystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();
                }
                if (height <= PPathwaySystem.MIN_Y_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                }
            }
        }

        /// <summary>
        /// Called when the North resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeN(object sender, PInputEventArgs e)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + HALF_WIDTH - PPathwaySystem.HALF_THICKNESS;
            float height = m_lowerRightPoint.Y - Y;

            if (height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.Y = Y - offsetToL.Y;
                system.Height = height;
                m_canvas.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
            }
        }

        /// <summary>
        /// Called when the NorthEast resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeNE(object sender, PInputEventArgs e)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + HALF_WIDTH - PPathwaySystem.HALF_THICKNESS;
            float width = e.PickedNode.X + e.PickedNode.OffsetX + HALF_WIDTH + PPathwaySystem.HALF_THICKNESS
                               - system.X - system.Offset.X;
            float height = m_lowerLeftPoint.Y - Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.Y = Y - offsetToL.Y;
                system.Width = width;
                system.Height = height;
                m_canvas.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();

                if (width <= PPathwaySystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();
                }
                if (height <= PPathwaySystem.MIN_Y_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                }
            }
        }

        /// <summary>
        /// Called when the East resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeE(object sender, PInputEventArgs e)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            float width = e.PickedNode.X + e.PickedNode.OffsetX + HALF_WIDTH + PPathwaySystem.HALF_THICKNESS
                              - system.X - system.Offset.X;
            if (width > PPathwaySystem.MIN_X_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                system.Width = width;
                m_canvas.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
            }
        }

        /// <summary>
        /// Called when the SouthEast resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeSE(object sender, PInputEventArgs e)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            float width = e.PickedNode.X + e.PickedNode.OffsetX + HALF_WIDTH + PPathwaySystem.HALF_THICKNESS
                               - system.X - system.Offset.X;
            float height = e.PickedNode.Y + e.PickedNode.OffsetY + HALF_WIDTH + PPathwaySystem.HALF_THICKNESS
                                - system.Y - system.Offset.Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.Width = width;
                system.Height = height;
                m_canvas.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();

                if (width <= PPathwaySystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                }
                if (height <= PPathwaySystem.MIN_Y_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                }
            }
        }

        /// <summary>
        /// Called when the South resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeS(object sender, PInputEventArgs e)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            float height = e.PickedNode.Y + e.PickedNode.OffsetY + HALF_WIDTH + PPathwaySystem.HALF_THICKNESS
                                 - system.Y - system.Offset.Y;

            if (height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                system.Height = height;
                m_canvas.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();
            }
        }

        /// <summary>
        /// Called when the SouthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeSW(object sender, PInputEventArgs e)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            float X = e.PickedNode.X + e.PickedNode.OffsetX + HALF_WIDTH - PPathwaySystem.HALF_THICKNESS;
            float width = m_upperRightPoint.X - e.PickedNode.X - e.PickedNode.OffsetX - HALF_WIDTH + PPathwaySystem.HALF_THICKNESS;
            float height = e.PickedNode.Y + e.PickedNode.OffsetY + HALF_WIDTH + PPathwaySystem.HALF_THICKNESS
                               - system.Y - system.Offset.Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.X = X - offsetToL.X;
                system.Width = width;
                system.Height = height;
                m_canvas.ValidateSystem(system);
                system.Refresh();

                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();

                if (width <= PPathwaySystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                }
                if (height <= PPathwaySystem.MIN_Y_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                }
            }
        }

        /// <summary>
        /// Called when the West resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_ResizeW(object sender, PInputEventArgs e)
        {
            string systemName = m_canvas.SelectedSystemName;
            if (systemName == null || !m_canvas.Systems.ContainsKey(systemName))
                return;
            PPathwaySystem system = m_canvas.Systems[systemName];
            m_canvas.RefreshSurroundState();

            float X = e.PickedNode.X + e.PickedNode.OffsetX + HALF_WIDTH - PPathwaySystem.HALF_THICKNESS;
            float width = m_lowerRightPoint.X - X;

            if (width > PPathwaySystem.MIN_X_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.X = X - offsetToL.X;
                system.Width = width;
                m_canvas.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
            }
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_CursorSizeNWSE(object sender, PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNWSE;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_CursorSizeNS(object sender, PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNS;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_CursorSizeNESW(object sender, PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNESW;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ResizeHandle_CursorSizeWE(object sender, PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeWE;
        }
        #endregion
    }
}
