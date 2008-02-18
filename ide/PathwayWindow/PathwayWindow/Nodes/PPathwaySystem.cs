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
        public static readonly float DEFAULT_WIDTH = 500;
        /// <summary>
        /// default height
        /// </summary>
        public static readonly float DEFAULT_HEIGHT = 500;
        /// <summary>
        /// When new system will be added by other plugin, it will be positioned this length away from
        /// parent system boundary.
        /// </summary>
        public static readonly float SYSTEM_MARGIN = 60;

        /// <summary>
        /// minimum width
        /// </summary>
        public static readonly float MIN_X_LENGTH = 80;
        /// <summary>
        /// minimum height
        /// </summary>
        public static readonly float MIN_Y_LENGTH = 80;
        /// <summary>
        /// An outer radius of round-shaped corner of a system.
        /// </summary>
        public static readonly float OUTER_RADIUS = 20f;

        /// <summary>
        /// An inner radius of round-shaped corner of a system.
        /// </summary>
        public static readonly float INNER_RADIUS = 10f;
        /// <summary>
        /// Thickness of system.
        /// </summary>
        public static readonly float HALF_THICKNESS = (OUTER_RADIUS - INNER_RADIUS) / 2f;
        /// <summary>
        /// Margin between lower hem and PText for a name of a system.
        /// </summary>
        public static readonly float TEXT_LOWER_MARGIN = 20f;
        #endregion

        #region Fields
        /// <summary>
        /// Indicate whether this PSystem is valid or not.
        /// </summary>
        protected bool m_valid = true;

        /// <summary>
        /// Brush for drawing back ground.
        /// </summary>
        protected Brush m_backBrush = null; //Brushes.White;

        /// <summary>
        /// the flag whether this system is changed.
        /// </summary>
        protected bool m_isChanged = true;
        /// <summary>
        /// graphics of system.
        /// </summary>
        protected Graphics m_g;
        /// <summary>
        /// position of x before moving.
        /// </summary>
        protected float m_prevX = 0;
        /// <summary>
        /// position of y before moving.
        /// </summary>
        protected float m_prevY = 0;
        /// <summary>
        /// GraphicPath of background.
        /// </summary>
        protected GraphicsPath m_backGp;
        /// <summary>
        /// GraohicsPath of outline.
        /// </summary>
        protected GraphicsPath m_outlineGp;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_ecellobj.
        /// </summary>
        public new EcellSystem EcellObject
        {
            get { return (EcellSystem)base.m_ecellObj; }
            set {
                base.Width = value.Width;
                base.Height = value.Height;
                base.EcellObject = value;
                this.Refresh();
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
                }
                else
                {
                    this.Brush = m_fillBrush;
                }
                m_isChanged = true;
            }
        }

        /// <summary>
        /// get/set the flag whether this system is valid.
        /// </summary>
        public override bool IsInvalid
        {
            get { return this.m_isInvalid; }
            set
            {
                this.m_isInvalid = value;
                if (value)
                {
                    this.Brush = m_invalidBrush;
                }
                else if (m_isSelected)
                    this.Brush = m_highLightBrush;
                else
                    this.Brush = m_fillBrush;
            }
        }
        
        /// <summary>
        /// get/set Pen.
        /// </summary>
        public override Pen Pen
        {
            get
            {
                return base.Pen;
            }
            set
            {
                base.m_pen = value;
                this.Repaint();
            }
        }

        /// <summary>
        /// Accessor for width.
        /// </summary>
        public override float Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                m_isChanged = true;
            }
        }

        /// <summary>
        /// Accessor for height.
        /// </summary>
        public override float Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
                m_isChanged = true;
            }
        }

        /// <summary>
        /// get/set the flag whether this system is valid.
        /// </summary>
        public bool Valid
        {
            get
            {
                return m_valid;
            }
            set
            {
                this.m_valid = value;
                if (this.m_valid)
                {
                    if (this.m_isSelected)
                        this.Brush = m_highLightBrush;
                    else
                        this.Brush = m_fillBrush;
                }
                else
                    this.Brush = m_invalidBrush;
                m_isChanged = true;
            }
        }
        /// <summary>
        /// Accessor for x coordinates of memorized position.
        /// </summary>
        public virtual float OriginalX
        {
            get { return m_originalX; }
        }

        /// <summary>
        /// Accessor for y coordinates of memorized position.
        /// </summary>
        public virtual float OriginalY
        {
            get { return m_originalY; }
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
        /// convert the position at canvas to the position of system.
        /// </summary>
        /// <param name="canvasPos">the position at canvas.</param>
        /// <returns>the position of system.</returns>
        public virtual PointF CanvasPos2SystemPos(PointF canvasPos)
        {      
            canvasPos.X -= base.OffsetX;
            canvasPos.Y -= base.OffsetY;
            if (base.Parent is PLayer)
                return canvasPos;
            PNode dummyParent = null;
            do
            {
                if (dummyParent == null)
                    dummyParent = base.Parent;
                else
                    dummyParent = dummyParent.Parent;
                canvasPos.X -= dummyParent.OffsetX;
                canvasPos.Y -= dummyParent.OffsetY;
            } while (!(dummyParent.Parent is PLayer));
            return canvasPos;
        }

        /// <summary>
        /// convert the position of system to the position at canvas.
        /// </summary>
        /// <param name="systemPos">the position of system.</param>
        /// <returns>the position at canvas.</returns>
        public virtual PointF SystemPos2CanvasPos(PointF systemPos)
        {
            PNode dummyParent = null;
            systemPos.X += base.OffsetX;
            systemPos.Y += base.OffsetY;
            do
            {
                if (dummyParent == null)
                    dummyParent = this.Parent;
                else
                    dummyParent = dummyParent.Parent;
                systemPos.X += dummyParent.OffsetX;
                systemPos.Y += dummyParent.OffsetY;
            } while (!(dummyParent.Parent is PLayer));
            return systemPos;
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
                if (obj is PPathwayVariable)
                    ((PPathwayVariable)obj).Refresh();
            }
        }
        /// <summary>
        /// Refresh Text contents of this object.
        /// </summary>
        public override void RefreshText()
        {
            base.RefreshText();
            base.m_pText.CenterBoundsOnPoint(base.X + base.Width / 2, base.Y + base.Height - TEXT_LOWER_MARGIN);
            base.m_pText.MoveToFront();
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
                m_canvas.PathwayControl.NotifyDataChanged(child.EcellObject.Key, child.EcellObject.Key, child, isRecorded, false);
            }

            // Make parent system create space for this system.
            if (m_parentSystem != null)
                m_parentSystem.MakeSpace(this, isRecorded);
            m_canvas.PathwayControl.NotifyDataChanged(m_ecellObj.Key, m_ecellObj.Key, this, isRecorded, false);
            this.Refresh();
        }

        /// <summary>
        /// the event sequence of selecting the PNode of system in PathwayEditor.
        /// </summary>
        /// <param name="e">PInputEventArgs</param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_canvas == null)
                return;

            m_canvas.ResetSelectedObjects();
            m_canvas.NotifySelectChanged(this);
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
            return this.Rect.IntersectsWith(rect) && !(this.Rect.Contains(rect) || rect.Contains(this.Rect));
        }

        /// <summary>
        /// Return to memorized position.
        /// </summary>
        public void ReturnToMemorizedPosition()
        {
            base.X = base.m_originalX;
            base.Y = base.m_originalY;
            base.OffsetX = base.m_originalOffsetX;
            base.OffsetY = base.m_originalOffsetY;
            //this.SystemWidth = this.m_originalWidth;
            //this.SystemHeight = this.m_originalHeight;
        }
        #endregion
    }    
}