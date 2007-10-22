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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;

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
        protected Brush m_backBrush = Brushes.White;

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
        /// <summary>
        /// Child object list.
        /// </summary>
        protected List<PPathwayObject> m_childList = new List<PPathwayObject>();
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_ecellobj.
        /// </summary>
        public new EcellSystem EcellObject
        {
            get { return (EcellSystem)base.m_ecellObj; }
            set {
                base.EcellObject = value;
                this.Name = value.key;
                this.Reset();
                this.RefreshText();
            }
        }
        /// <summary>
        /// Accessor for m_ecellobj.
        /// </summary>
        public List<PPathwayObject> ChildObjectList
        {
            get { return this.m_childList; }
            set { this.m_childList = value; }
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
                    this.Brush = m_normalBrush;
                    if(m_control != null)
                    {
                        //HideResizeHandles();
                    }
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
                    this.Brush = m_normalBrush;
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
                base.pen = value;
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
                        this.Brush = m_normalBrush;
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

        /// <summary>
        /// get the position of center for the text.
        /// </summary>
        public new float TextCenterX
        {
            get
            {
                //return this.X + this.OffsetToLayer.X + Width / 2f;
                return this.X + Width / 2f;
            }
        }

        /// <summary>
        /// get the position of center for the text.
        /// </summary>
        public new float TextCenterY
        {
            get
            {
                return this.Y + this.Height - TEXT_LOWER_MARGIN;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for PEcellSystem.
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
        /// set the rectangle of system at canvas.
        /// </summary>
        /// <param name="x">the position of system.</param>
        /// <param name="y">the position of system.</param>
        /// <param name="width">the width of system.</param>
        /// <param name="height">the height of system.</param>
        public void SetRect(float x, float y, float width, float height)
        {
            base.X = x;
            base.Y = y;
            base.Width = width;
            base.Height = height;
            Reset();
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
        }

        /// <summary>
        /// Make space for child rectangle.
        /// Extend current space to contain given rectangle.
        /// </summary>
        /// <param name="rect"></param>
        public void MakeSpace(PPathwayObject obj)
        {
            // Offset position of given object.
            if (obj.X < this.X + this.Offset.X + SYSTEM_MARGIN)
                obj.X = this.X + this.Offset.X + SYSTEM_MARGIN;
            if (obj.Y < this.Y + this.Offset.Y + SYSTEM_MARGIN)
                obj.Y = this.Y + this.Offset.Y + SYSTEM_MARGIN;
            // Enlarge this system
            if (this.X + this.Width < obj.X + obj.Width + SYSTEM_MARGIN)
                this.Width = obj.X + obj.Width + SYSTEM_MARGIN - this.X;
            if (this.Y + this.Height < obj.Y + obj.Height + SYSTEM_MARGIN)
                this.Height = obj.Y + obj.Height + SYSTEM_MARGIN - this.Y;

            // Make parent system create space for this system.
            if (null != this.Parent && this.Parent is PPathwaySystem)
                ((PPathwaySystem)this.Parent).MakeSpace(this);

            // Move child nodes position.
            if (this.ChildObjectList == null)
                return;
            foreach(PPathwayObject child in this.ChildObjectList)
                if (obj.Rect.Contains(child.Rect) || obj.Rect.IntersectsWith(child.Rect))
                    child.X += obj.Width;
            m_control.NotifyDataChanged(this.EcellObject.key, this.EcellObject.key, this, false);

        }

        /// <summary>
        /// event on paint this object.
        /// </summary>
        /// <param name="paintContext">PPaintContext</param>
        protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
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
            Brush b = this.Brush;
            if (b != null)
                paintContext.Graphics.FillPath(b, base.path);
            
            if(m_backBrush != null)
                paintContext.Graphics.FillPath(m_backBrush, m_backGp);
            
            Pen p = null;
            if (IsHighLighted)
                p = new Pen(Brushes.DarkOrange, 1);
            else
                p = new Pen(Brushes.Blue, 1);
            paintContext.Graphics.DrawPath( p, m_outlineGp );            
            
        }

        /// <summary>
        /// event on double click this system.
        /// if there are system outside this system,
        /// it fire event at outside system.
        /// </summary>
        /// <param name="e"></param>
        public override void OnDoubleClick(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (!(e.PickedNode is PPathwaySystem)) return;
            if (EcellObject == null)
                return;
            PPathwaySystem p = (PPathwaySystem)e.PickedNode;
            if (!p.EcellObject.key.Equals(EcellObject.key)) return;

            PropertyEditor editor = new PropertyEditor();
            editor.layoutPanel.SuspendLayout();
            editor.SetCurrentObject(EcellObject);
            editor.SetDataType(EcellObject.type);
            editor.PEApplyButton.Click += new EventHandler(editor.UpdateProperty);
            editor.LayoutPropertyEditor();
            editor.layoutPanel.ResumeLayout(false);
            editor.ShowDialog();
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
        /// Refresh graphical contents of this object.
        /// ex) Edges of a process can be refreshed by using this.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.Reset();
            this.RefreshText();
            foreach (PPathwayObject obj in this.ChildObjectList)
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
            base.m_pText.Text = this.EcellObject.Text;
            base.m_pText.CenterBoundsOnPoint(TextCenterX, TextCenterY);
            base.m_pText.MoveToFront();
        }

        /// <summary>
        /// start to move this Node by drag.
        /// </summary>
        public override void MoveStart()
        {
            PNodeList tmplist = new PNodeList();
            foreach (PNode p in this.ChildrenReference)
            {
                if (p is PPathwayProcess)
                    tmplist.Add(p);
            }
            foreach (PNode p in tmplist)
            {
                if (p is PPathwayObject)
                {
                    ((PPathwayObject)p).MoveStart();
                }
                else if (p is PPath)
                {
                    int ind = this.IndexOfChild(p);
                    this.RemoveChild(ind);
                }
            }
        }

        /// <summary>
        /// end to move this Node by drag.
        /// </summary>
        public override void MoveEnd()
        {
            PNodeList tmplist = new PNodeList();
            foreach (PNode p in this.ChildrenReference)
            {
                if (p is PPathwayProcess)
                    tmplist.Add(p);
            }
            foreach (PNode p in tmplist)
            {
                if (p is PPathwayObject)
                {
                    ((PPathwayObject)p).MoveEnd();
                }
            }
        }
        #endregion

        #region nouse
        public override void Delete()
        {
        }
        public override bool HighLighted(bool highlight)
        {
            return true;
        }
        public override void Initialize()
        {
        }
        public override void DataChanged(EcellObject ecellObj)
        {
        }
        public override void DataDeleted()
        {
        }
        public override void SelectChanged()
        {
        }
        public override void Start()
        {
        }
        public override void Change()
        {
        }
        public override void Stop()
        {
        }
        public override void End()
        {
        }
        #endregion
    }    
}