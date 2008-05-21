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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using EcellLib.Objects;
using EcellLib.PathwayWindow.Handler;

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayNode for E-cell system.
    /// </summary>
    public class PPathwaySystem : PPathwayObject
    {
        #region Static readonly fields
        /// <summary>
        /// default width
        /// </summary>
        public const float DEFAULT_WIDTH = 500;
        /// <summary>
        /// default height
        /// </summary>
        public const float DEFAULT_HEIGHT = 500;
        /// <summary>
        /// When new system will be added by other plugin, it will be positioned this length away from
        /// parent system boundary.
        /// </summary>
        public const float SYSTEM_MARGIN = 60;

        /// <summary>
        /// minimum width
        /// </summary>
        public const float MIN_X_LENGTH = 80;
        /// <summary>
        /// minimum height
        /// </summary>
        public const float MIN_Y_LENGTH = 80;
        /// <summary>
        /// An outer radius of round-shaped corner of a system.
        /// </summary>
        public const float OUTER_RADIUS = 20f;

        /// <summary>
        /// An inner radius of round-shaped corner of a system.
        /// </summary>
        public const float INNER_RADIUS = 10f;
        /// <summary>
        /// Thickness of system.
        /// </summary>
        public const float HALF_THICKNESS = (OUTER_RADIUS - INNER_RADIUS) / 2f;
        /// <summary>
        /// Margin between lower hem and PText for a name of a system.
        /// </summary>
        public const float TEXT_LOWER_MARGIN = 20f;
        #endregion

        #region Fields
        /// <summary>
        /// Brush for drawing back ground.
        /// </summary>
        protected Brush m_backBrush = null; //Brushes.White;

        /// <summary>
        /// ResizeHandler for resizing a system.
        /// </summary>
        protected SystemResizeHandler m_resizeHandler;

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_ecellobj.
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return base.m_ecellObj; }
            set {
                if (value.Width < DEFAULT_WIDTH)
                    base.Width = DEFAULT_WIDTH;
                else
                    base.Width = value.Width;
                if (value.Height < DEFAULT_HEIGHT)
                    base.Height = DEFAULT_HEIGHT;
                else
                    base.Height = value.Height;

                base.EcellObject = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override CanvasControl Canvas
        {
            get
            {
                return base.Canvas;
            }
            set
            {
                base.Canvas = value;
                m_resizeHandler.Canvas = value;
            }
        }

        /// <summary>
        /// get/set the flag whether display this system with highlight.
        /// </summary>
        public override bool IsHighLighted
        {
            get { return this.m_isSelected; }
            set
            {
                this.m_isSelected = value;
                if (value)
                {
                    this.Brush = m_highLightBrush;
                    this.m_resizeHandler.ShowResizeHandles();
                }
                else
                {
                    this.Brush = m_fillBrush;
                    this.m_resizeHandler.HideResizeHandles();
                }
            }
        }

        /// <summary>
        /// Accessor for m_backBrush.
        /// </summary>
        public virtual Brush BackgroundBrush
        {
            get { return m_backBrush; }
            set { m_backBrush = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for PPathwaySystem.
        /// </summary>
        public PPathwaySystem()
        {
            base.Width = DEFAULT_WIDTH;
            base.Height = DEFAULT_HEIGHT;
            this.m_resizeHandler = new SystemResizeHandler(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create object of PPathwayObject.
        /// </summary>
        /// <returns>new instance</returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwaySystem();
        }

        /// <summary>
        /// Add child to this system.
        /// </summary>
        /// <param name="child">child</param>
        public override void AddChild(PNode child)
        {
            base.AddChild(child);
        }

        /// <summary>
        /// Refresh graphical contents of this object.
        /// ex) Edges of a process can be refreshed by using this.
        /// </summary>
        public override void Refresh()
        {
            if (m_canvas == null)
                return;
            foreach (PPathwayObject obj in m_canvas.GetAllObjectUnder(m_ecellObj.Key))
            {
                obj.MoveToFront();
                if (obj is PPathwayVariable)
                    ((PPathwayVariable)obj).Refresh();
            }
            base.Refresh();
        }

        /// <summary>
        /// Refresh View
        /// </summary>
        public override void RefreshView()
        {
            base.m_path = m_figure.CreatePath(X, Y, Width, Height);
            base.RefreshView();
            m_resizeHandler.UpdateResizeHandle();
        }
        /// <summary>
        /// Refresh Text contents of this object.
        /// </summary>
        protected override void RefreshText()
        {
            base.RefreshText();
            base.m_pText.CenterBoundsOnPoint(base.X + base.Width / 2, base.Y + base.Height - TEXT_LOWER_MARGIN);
        }

        /// <summary>
        /// Make space for child rectangle.
        /// Extend current space to contain given rectangle.
        /// </summary>
        /// <param name="obj">The child object.</param>
        /// <param name="isRecorded">is recorded or not.</param>
        public void MakeSpace(PPathwayObject obj, bool isRecorded)
        {
            // Offset position of given object.
            if (obj.X <= base.X + base.Offset.X + SYSTEM_MARGIN)
                obj.X = base.X + base.Offset.X + SYSTEM_MARGIN;
            if (obj.Y <= base.Y + base.Offset.Y + SYSTEM_MARGIN)
                obj.Y = base.Y + base.Offset.Y + SYSTEM_MARGIN;
            // Enlarge this system
            if (base.X + base.Width < obj.X + obj.Width + SYSTEM_MARGIN)
                base.Width = obj.X + obj.Width + SYSTEM_MARGIN - base.X;
            if (base.Y + base.Height < obj.Y + obj.Height + SYSTEM_MARGIN)
                base.Height = obj.Y + obj.Height + SYSTEM_MARGIN - base.Y;

            // Move child nodes position.
            foreach (PPathwayObject child in m_canvas.GetAllObjectUnder(m_ecellObj.Key))
            {
                if (child.EcellObject.Key.StartsWith(obj.EcellObject.Key))
                    continue;
                if (!obj.Rect.Contains(child.Rect) && !obj.Rect.IntersectsWith(child.Rect))
                    continue;
                child.PointF = m_canvas.GetVacantPoint(m_ecellObj.Key, child.Rect);
                m_canvas.Control.NotifyDataChanged(child.EcellObject.Key, child.EcellObject.Key, child, isRecorded, false);
            }

            // Make parent system create space for this system.
            if (m_parentSystem != null)
                m_parentSystem.MakeSpace(this, isRecorded);
            m_canvas.Control.NotifyDataChanged(m_ecellObj.Key, m_ecellObj.Key, this, isRecorded, false);
            this.Refresh();
        }

        /// <summary>
        /// the event sequence of selecting the PNode of system in PathwayEditor.
        /// </summary>
        /// <param name="e">PInputEventArgs</param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            if (m_canvas == null)
                return;
            m_canvas.NotifySelectChanged(this);
            base.OnMouseDown(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDrag(PInputEventArgs e)
        {
            base.OnMouseDrag(e);
            m_resizeHandler.UpdateResizeHandle();
        }
        /// <summary>
        /// Check if this PSystem's region overlaps given rectangle
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if each rectangle overlaps other rectangle
        /// (doesn't contain whole rectangle)</returns>
        public virtual bool Overlaps(RectangleF rect)
        {
            if (this.Rect.Contains(rect) && rect.Contains(this.Rect))
                return true;
            else if (this.Rect.IntersectsWith(rect) && !(this.Rect.Contains(rect) || rect.Contains(this.Rect)))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Check if this PSystem's region overlaps given rectangle
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if each rectangle overlaps other rectangle
        /// (doesn't contain whole rectangle)</returns>
        public virtual bool Contains(RectangleF rect)
        {
            if (this.Rect.Contains(rect) && rect.Contains(this.Rect))
                return true;
            else if (this.Rect.IntersectsWith(rect))
                return true;
            else if (this.Rect.Contains(rect) || rect.Contains(this.Rect))
                return true;
            else
                return false;
        }
        #endregion
    }    
}