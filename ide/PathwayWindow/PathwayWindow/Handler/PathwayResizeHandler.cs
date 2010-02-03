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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// PathwayResizeHandler
    /// </summary>
    public class PathwayResizeHandler
    {
        #region Fields
        /// <summary>
        /// CanvasControl.
        /// </summary>
        protected CanvasControl m_canvas = null;

        /// <summary>
        /// m_resideHandles contains a list of ResizeHandle for resizing a system.
        /// </summary>
        protected List<ResizeHandle> m_resizeHandles = new List<ResizeHandle>();

        /// <summary>
        /// Selected object
        /// </summary>
        protected PPathwayObject m_obj = null;

        /// <summary>
        /// Cursor
        /// </summary>
        protected Cursor m_cursor;
        /// <summary>
        /// 
        /// </summary>
        protected float m_minWidth = 60;
        /// <summary>
        /// 
        /// </summary>
        protected float m_minHeight = 40;
        #endregion

        #region Accessor
        /// <summary>
        /// Accessor for an instance of CanvasViewComponentSet which this instance belongs.
        /// </summary>
        public virtual CanvasControl Canvas
        {
            get { return m_canvas; }
            set
            {
                m_canvas = value;
            }
        }

        /// <summary>
        /// Accessor for Minimum Width of Object.
        /// </summary>
        public virtual float MinWidth
        {
            get { return m_minWidth; }
            set { m_minWidth = value; }
        }

        /// <summary>
        /// Accessor for Minimum Height of Object.
        /// </summary>
        public virtual float MinHeight
        {
            get { return m_minHeight; }
            set { m_minHeight = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public PathwayResizeHandler(PPathwayObject obj)
        {
            this.m_obj = obj;
            this.m_obj.VisibleChanged += new PPropertyEventHandler(Object_VisibleChanged);
            this.m_obj.HighLightChanged += new PPropertyEventHandler(Object_VisibleChanged);
            this.m_canvas = obj.Canvas;

            for (int i = 0; i < 8; i++)
            {
                ResizeHandle handle = new ResizeHandle();
                handle.MouseEnter += new PInputEventHandler(ResizeHandle_MouseEnter);
                handle.MouseLeave += new PInputEventHandler(ResizeHandle_MouseLeave);
                handle.MouseDown += new PInputEventHandler(ResizeHandle_MouseDown);
                handle.MouseDrag += new PInputEventHandler(ResizeHandle_MouseDrag);
                handle.MouseUp += new PInputEventHandler(ResizeHandle_MouseUp);
                handle.Cursor = GetCursor(i);
                handle.HandlePosition = i;
                m_resizeHandles.Add(handle);
            }
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

        #region public Methods
        /// <summary>
        /// Show resize handles for resizing system.
        /// </summary>
        public void Show()
        {
            Update();
            foreach (PNode node in m_resizeHandles)
                m_canvas.ControlLayer.AddChild(node);
        }

        /// <summary>
        /// Hide resize handles for resizing system.
        /// </summary>
        public void Hide()
        {
            foreach (PNode handle in m_resizeHandles)
                if (handle.Parent == m_canvas.ControlLayer)
                    m_canvas.ControlLayer.RemoveChild(handle);
        }

        /// <summary>
        /// Reset reside handles' positions.
        /// </summary>
        public void Update()
        {
            PointF gp = m_obj.PointF;
            float x = gp.X;
            float y = gp.Y;
            float width = m_obj.Width;
            float height = m_obj.Height;
            float half_width = width / 2f;
            float half_height = height / 2f;

            m_resizeHandles[ResizeHandle.NW].OffsetX = x;
            m_resizeHandles[ResizeHandle.NW].OffsetY = y;
            m_resizeHandles[ResizeHandle.N].OffsetX = x + half_width;
            m_resizeHandles[ResizeHandle.N].OffsetY = y;
            m_resizeHandles[ResizeHandle.NE].OffsetX = x + width;
            m_resizeHandles[ResizeHandle.NE].OffsetY = y;
            m_resizeHandles[ResizeHandle.E].OffsetX = x + width;
            m_resizeHandles[ResizeHandle.E].OffsetY = y + half_height;
            m_resizeHandles[ResizeHandle.SE].OffsetX = x + width;
            m_resizeHandles[ResizeHandle.SE].OffsetY = y + height;
            m_resizeHandles[ResizeHandle.S].OffsetX = x + half_width;
            m_resizeHandles[ResizeHandle.S].OffsetY = y + height;
            m_resizeHandles[ResizeHandle.SW].OffsetX = x;
            m_resizeHandles[ResizeHandle.SW].OffsetY = y + height;
            m_resizeHandles[ResizeHandle.W].OffsetX = x;
            m_resizeHandles[ResizeHandle.W].OffsetY = y + half_height;
        }

        /// <summary>
        /// ResizeObject
        /// </summary>
        protected virtual void ResizeObject(float x, float y, float width, float height)
        {
            if (width >= m_minWidth && height >= m_minHeight)
            {
                m_obj.X = x;
                m_obj.Y = y;
                m_obj.Width = width;
                m_obj.Height = height;
                m_obj.RefreshView();
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ResizeHandle_MouseEnter(object sender, PInputEventArgs e)
        {
            m_cursor = e.Canvas.Cursor;
            ResizeHandle handle = (ResizeHandle)e.PickedNode;
            e.Canvas.Cursor = handle.Cursor;
        }

        /// <summary>
        /// Called when the mouse is off a resize handle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ResizeHandle_MouseLeave(object sender, PInputEventArgs e)
        {
            e.Canvas.Cursor = m_cursor;
        }

        /// <summary>
        /// Called when the mouse is down on one of resize handles for a system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ResizeHandle_MouseDown(object sender, PInputEventArgs e)
        {
            m_obj.MemorizePosition();
        }

        /// <summary>
        /// Called when the mouse is up on one of resize handles for a system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ResizeHandle_MouseUp(object sender, PInputEventArgs e)
        {
            m_obj.Changed = true;
            m_canvas.NotifyMoveObjects(true);
            m_obj.Changed = false;
        }

        /// <summary>
        /// Called when the NorthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ResizeHandle_MouseDrag(object sender, PInputEventArgs e)
        {
            ResizeHandle handle = (ResizeHandle)e.PickedNode;
            int pos = handle.HandlePosition;

            // Set Position
            float x = m_obj.X;
            float y = m_obj.Y;
            float width = m_obj.Width;
            float height = m_obj.Height;

            // Set x and width
            if (pos == ResizeHandle.NW || pos == ResizeHandle.W || pos == ResizeHandle.SW)
            {
                x = handle.OffsetX;
                width = m_obj.X + m_obj.Width - handle.OffsetX;
            }
            else if (pos == ResizeHandle.NE || pos == ResizeHandle.E || pos == ResizeHandle.SE)
            {
                width = handle.OffsetX - m_obj.X;
            }
            // Set y and height
            if (pos == ResizeHandle.NW || pos == ResizeHandle.N || pos == ResizeHandle.NE)
            {
                y = handle.OffsetY;
                height = m_obj.Y + m_obj.Height - handle.OffsetY;
            }
            else if (pos == ResizeHandle.SW || pos == ResizeHandle.S || pos == ResizeHandle.SE)
            {
                height = handle.OffsetY - m_obj.Y;
            }

            // Reset Handle position
            if (pos == ResizeHandle.N || pos == ResizeHandle.S)
                handle.OffsetX = m_obj.X + m_obj.Width / 2f;
            else if (pos == ResizeHandle.E || pos == ResizeHandle.W)
                handle.OffsetY = m_obj.Y + m_obj.Height / 2f;
            ResizeObject(x, y, width, height);
            Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Object_VisibleChanged(object sender, PPropertyEventArgs e)
        {
            if (m_obj.Visible && m_obj.Selected)
                Show();
            else
                Hide();
        }

        #endregion

        #region Inner Class
        /// <summary>
        /// ResizeHandle
        /// </summary>
        protected class ResizeHandle : PPath
        {
            #region Constants
            // Preparing system resize handlers
            // position of each handle is shown below.
            //  0 | 1 | 2
            // -----------
            //  7 |   | 3
            // -----------
            //  6 | 5 | 4
            /// <summary>
            /// NW
            /// </summary>
            public const int NW = 0;
            /// <summary>
            /// N
            /// </summary>
            public const int N = 1;
            /// <summary>
            /// NE
            /// </summary>
            public const int NE = 2;
            /// <summary>
            /// E
            /// </summary>
            public const int E = 3;
            /// <summary>
            /// SE
            /// </summary>
            public const int SE = 4;
            /// <summary>
            /// S
            /// </summary>
            public const int S = 5;
            /// <summary>
            /// SW
            /// </summary>
            public const int SW = 6;
            /// <summary>
            /// W
            /// </summary>
            public const int W = 7;
            /// <summary>
            /// Half of width of a ResizeHandle
            /// </summary>
            protected readonly float HALF_WIDTH = 5;

            #endregion
            /// <summary>
            /// Mouse cursor.
            /// </summary>
            private Cursor m_cursor = null;
            /// <summary>
            /// HandlePosition
            /// </summary>
            private int m_handlePosition;
            /// <summary>
            /// 
            /// </summary>
            public int HandlePosition
            {
                get { return m_handlePosition; }
                set { m_handlePosition = value; }
            }

            /// <summary>
            /// Mouse cursor.
            /// </summary>
            public Cursor Cursor
            {
                get { return m_cursor; }
                set { m_cursor = value; }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            internal ResizeHandle()
            {
                this.AddInputEventListener(new PResizeHandleDragEventhandler());
                this.Brush = Brushes.DarkOrange;
                this.Pen = new Pen(Brushes.DarkOliveGreen, 0);
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(new RectangleF(-1 * HALF_WIDTH,
                                    -1 * HALF_WIDTH,
                                    HALF_WIDTH * 2f,
                                    HALF_WIDTH * 2f));
                this.AddPath(path, false);
            }
        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public class PResizeHandleDragEventhandler : PDragEventHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.Button == MouseButtons.Left;
        }
    }
}
