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
            this.Reset();
            foreach (PPathwayObject obj in m_canvas.GetAllObjectUnder(m_ecellObj.key))
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
            base.m_pText.Text = m_ecellObj.Text;
            base.m_pText.CenterBoundsOnPoint(base.X + base.Width / 2, base.Y + base.Height - TEXT_LOWER_MARGIN);
            base.m_pText.MoveToFront();
        }

        /// <summary>
        /// reset the view object of system in canvas.
        /// </summary>
        public override void Reset()
        {
            float prevX = X;
            float prevY = Y;
            float prevWidth = base.Width;
            float prevHeight = base.Height;

            base.Reset();
            base.Width = prevWidth;
            base.Height = prevHeight;
            float thickness = OUTER_RADIUS - INNER_RADIUS;
            float outerDiameter = OUTER_RADIUS * 2;
            float innerDiameter = INNER_RADIUS * 2;
            float horizontalRectWidth = base.Width - 2f * OUTER_RADIUS;
            float verticalRectHeight = base.Height - 2f * OUTER_RADIUS;
            GraphicsPath gp = new GraphicsPath();
            gp.AddPie(0, 0, outerDiameter, outerDiameter, 180, 90);
            gp.AddPie(thickness, thickness, innerDiameter, innerDiameter, 180, 90);
            gp.AddRectangle(new RectangleF(OUTER_RADIUS, 0, base.Width - outerDiameter, thickness));
            gp.AddPie(base.Width - outerDiameter, 0, outerDiameter, outerDiameter, 270, 90);
            gp.AddPie(base.Width - outerDiameter + thickness, thickness, innerDiameter, innerDiameter, 270, 90);
            gp.AddRectangle(new RectangleF(base.Width - thickness, OUTER_RADIUS, thickness, verticalRectHeight));
            gp.AddPie(base.Width - outerDiameter, base.Height - outerDiameter, outerDiameter, outerDiameter, 0, 90);
            gp.AddPie(base.Width - outerDiameter + thickness,
                      base.Height - outerDiameter + thickness, innerDiameter, innerDiameter, 0, 90);
            gp.AddRectangle(new RectangleF(OUTER_RADIUS, base.Height - thickness, horizontalRectWidth, thickness));
            gp.AddPie(0, base.Height - outerDiameter, outerDiameter, outerDiameter, 90, 90);
            gp.AddPie(thickness, base.Height - outerDiameter + thickness, innerDiameter, innerDiameter, 90, 90);
            gp.AddRectangle(new RectangleF(0, OUTER_RADIUS, thickness, verticalRectHeight));
            
            AddPath(gp,false);
            X = prevX;
            Y = prevY;

            SetGraphicsPath();
            RefreshText();
        }

        /// <summary>
        /// Make space for child rectangle.
        /// Extend current space to contain given rectangle.
        /// </summary>
        /// <param name="obj">The child object.</param>
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
            foreach (PPathwayObject child in m_canvas.GetAllObjectUnder(m_ecellObj.key))
            {
                if (child.EcellObject.key.StartsWith(obj.EcellObject.key))
                    continue;
                if (!obj.Rect.Contains(child.Rect) && !obj.Rect.IntersectsWith(child.Rect))
                    continue;
                child.PointF = m_canvas.GetVacantPoint(m_ecellObj.key, child.Rect);
                m_canvas.PathwayControl.NotifyDataChanged(child.EcellObject.key, child.EcellObject.key, child, isRecorded, false);
            }

            // Make parent system create space for this system.
            if (null != m_parentObject && m_parentObject is PPathwaySystem)
                ((PPathwaySystem)m_parentObject).MakeSpace(this, isRecorded);
            m_canvas.PathwayControl.NotifyDataChanged(m_ecellObj.key, m_ecellObj.key, this, isRecorded, false);
            this.Refresh();
        }

        /// <summary>
        /// event on paint this object.
        /// </summary>
        /// <param name="paintContext">PPaintContext</param>
        protected override void Paint(PPaintContext paintContext)
        {
            if(m_prevX != (this.X + this.OffsetX) || m_prevY != (this.Y + this.OffsetY))
            {
                m_isChanged = true;
            }
            
            if (m_isChanged)
            {
                this.SetGraphicsPath();
                m_isChanged = false;
            }

            this.Brush = m_fillBrush;
            if (m_fillBrush != null)
                paintContext.Graphics.FillPath(m_fillBrush, base.m_path);
            if(m_backBrush != null)
                paintContext.Graphics.FillPath(m_backBrush, m_backGp);
            Pen pen = (IsHighLighted) ? new Pen(m_highLightBrush, 1) : new Pen(m_lineBrush, 1);
            paintContext.Graphics.DrawPath(pen, m_outlineGp);            
        }

        /// <summary>
        /// the event sequence of selecting the PNode of system in PathwayEditor.
        /// </summary>
        /// <param name="sender">PPathwaySystem</param>
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
        /// set GraphicsPath.
        /// </summary>
        public void SetGraphicsPath()
        {
            float thickness = OUTER_RADIUS - INNER_RADIUS;
            float outerDiameter = OUTER_RADIUS * 2;
            float innerDiameter = INNER_RADIUS * 2;
            float horizontalRectWidth = base.Width - 2f * OUTER_RADIUS;
            float verticalRectHeight = base.Height - 2f * OUTER_RADIUS;

            m_backGp = new GraphicsPath();
            m_backGp.FillMode = FillMode.Alternate;
            m_backGp.AddRectangle(new RectangleF(X + thickness,
                                               Y + thickness,
                                               base.Width - 2 * thickness,
                                               base.Height - 2 * thickness));
            m_backGp.AddRectangle(new RectangleF(X + thickness,
                                               Y + thickness,
                                               INNER_RADIUS,
                                               INNER_RADIUS));
            m_backGp.AddRectangle(new RectangleF(X + base.Width - OUTER_RADIUS,
                                               Y + thickness,
                                               INNER_RADIUS,
                                               INNER_RADIUS));
            m_backGp.AddRectangle(new RectangleF(X + base.Width - OUTER_RADIUS,
                                               Y + base.Height - OUTER_RADIUS,
                                               INNER_RADIUS,
                                               INNER_RADIUS));
            m_backGp.AddRectangle(new RectangleF(X + thickness,
                                               Y + base.Height - OUTER_RADIUS,
                                               INNER_RADIUS,
                                               INNER_RADIUS));
            m_backGp.AddPie(X + thickness, Y + thickness, innerDiameter, innerDiameter, 180, 90);
            m_backGp.AddPie(X + base.Width - OUTER_RADIUS - INNER_RADIUS, Y + thickness,
                           innerDiameter, innerDiameter, 270, 90);
            m_backGp.AddPie(X + base.Width - OUTER_RADIUS - INNER_RADIUS,
                            Y + base.Height - OUTER_RADIUS - INNER_RADIUS,
                            innerDiameter, innerDiameter, 0, 90);
            m_backGp.AddPie(X + thickness, Y + base.Height - OUTER_RADIUS - INNER_RADIUS,
                           innerDiameter, innerDiameter, 90, 90);


            m_outlineGp = new GraphicsPath();
            m_outlineGp.FillMode = FillMode.Alternate;
            m_outlineGp.AddArc(X, Y, outerDiameter, outerDiameter, 180, 90);
            m_outlineGp.AddArc(X + base.Width - outerDiameter, Y, outerDiameter, outerDiameter, 270, 90);
            m_outlineGp.AddArc(X + base.Width - outerDiameter, Y + base.Height - outerDiameter,
                      outerDiameter, outerDiameter, 0, 90);
            m_outlineGp.AddArc(X, Y + base.Height - outerDiameter, outerDiameter, outerDiameter, 90, 90);
            m_outlineGp.AddLine(X, Y + OUTER_RADIUS, X, Y + base.Height - OUTER_RADIUS);
            m_outlineGp.CloseFigure();
            m_outlineGp.AddArc(X + thickness, Y + thickness, innerDiameter, innerDiameter, 180, 90);
            m_outlineGp.AddArc(X + base.Width - OUTER_RADIUS - INNER_RADIUS,
                      Y + thickness, innerDiameter, innerDiameter, 270, 90);
            m_outlineGp.AddArc(X + base.Width - OUTER_RADIUS - INNER_RADIUS,
                      Y + base.Height - OUTER_RADIUS - INNER_RADIUS,
                      innerDiameter, innerDiameter, 0, 90);
            m_outlineGp.AddArc(X + thickness, Y + base.Height - OUTER_RADIUS - INNER_RADIUS, innerDiameter, innerDiameter, 90, 90);
            m_outlineGp.AddLine(X + thickness, Y + OUTER_RADIUS, X + thickness, Y + base.Height - OUTER_RADIUS);

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

        /// <summary>
        /// start to move this Node by drag.
        /// </summary>
        public override void MoveStart()
        {
            foreach (PPathwayObject obj in m_canvas.GetAllObjectUnder(m_ecellObj.key))
            {
                obj.MoveStart();
            }
        }

        /// <summary>
        /// end to move this Node by drag.
        /// </summary>
        public override void MoveEnd()
        {
            foreach (PPathwayObject obj in m_canvas.GetAllObjectUnder(m_ecellObj.key))
            {
                obj.MoveEnd();
            }
        }
        #endregion

        #region nouse
        /// <summary>
        /// Delete this object.
        /// </summary>
        public override void Delete()
        {
        }

        /// <summary>
        /// Change whether this object is high light.
        /// </summary>
        /// <param name="highlight">the flag whether this object is high light.</param>
        /// <returns></returns>
        public override bool HighLighted(bool highlight)
        {
            return true;
        }

        /// <summary>
        /// Initialize this object.
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// The properties of this object is changed.
        /// </summary>
        /// <param name="ecellObj"></param>
        public override void DataChanged(EcellObject ecellObj)
        {
        }

        /// <summary>
        /// This object is deleted.
        /// </summary>
        public override void DataDeleted()
        {
        }

        /// <summary>
        /// This object is selected.
        /// </summary>
        public override void SelectChanged()
        {
        }

        /// <summary>
        /// The simulation start.
        /// </summary>
        public override void Start()
        {
        }

        /// <summary>
        /// The property of this object is changed.
        /// </summary>
        public override void Change()
        {
        }

        /// <summary>
        /// The simulation stop.
        /// </summary>
        public override void Stop()
        {
        }

        /// <summary>
        /// End to edit this object.
        /// </summary>
        public override void End()
        {
        }
        #endregion
    }    
}