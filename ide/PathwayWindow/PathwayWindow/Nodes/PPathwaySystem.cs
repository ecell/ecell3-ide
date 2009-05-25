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
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.Objects;
using UMD.HCIL.Piccolo;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
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
        public const float MIN_WIDTH = 80;
        /// <summary>
        /// minimum height
        /// </summary>
        public const float MIN_HEIGHT = 80;
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

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_ecellobj.
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return base.m_ecellObj; }
            set {
                if (value.Width < MIN_WIDTH)
                    base.Width = MIN_WIDTH;
                else
                    base.Width = value.Width;
                if (value.Height < MIN_HEIGHT)
                    base.Height = MIN_HEIGHT;
                else
                    base.Height = value.Height;

                base.EcellObject = value;
            }
        }
        /// <summary>
        /// Offset
        /// </summary>
        public override PointF Offset
        {
            get
            {
                return base.Offset;
            }
            set
            {
                base.Offset = value;
                m_resizeHandler.Update();
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
        public override bool Selected
        {
            get { return this.m_selected; }
            set
            {
                this.m_selected = value;
                if (value)
                {
                    this.Brush = m_highLightBrush;
                }
                else
                {
                    this.Brush = m_setting.CreateBrush(m_path);
                }
                RaiseHightLightChanged();
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
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            m_resizeHandler.Hide();
            base.Dispose();
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
                obj.Refresh();
            }
            base.Refresh();
        }

        /// <summary>
        /// Refresh View
        /// </summary>
        public override void RefreshView()
        {
            SetNewPath();
            base.RefreshView();
            m_resizeHandler.Update();
        }

        private void SetNewPath()
        {
            if (m_path.PathPoints == null || m_path.PathPoints.Length < 1)
                return;
            float minX = m_path.PathPoints[0].X;
            float minY = m_path.PathPoints[0].Y;
            float maxX = minX;
            float maxY = minY;
            foreach (PointF point in m_path.PathPoints)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }
            base.m_path = m_figure.CreatePath(minX, minY, maxX - minX, maxY - minY);
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