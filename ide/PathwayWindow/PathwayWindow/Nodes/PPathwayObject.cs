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
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.Serialization;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using Ecell;
using System.ComponentModel;
using UMD.HCIL.Piccolo.Event;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Figure;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayObject is a super class for all component of PCanvas.
    /// </summary>
    public abstract class PPathwayObject : PNode
    {
        #region Enums
        /// <summary>
        /// Represents the types of picking modes for a PProcess object.
        /// </summary>
        public enum PathPickMode
        {
            /// <summary>
            /// Faster Picking.  Paths are picked in local coordinates.
            /// </summary>
            Fast,

            /// <summary>
            /// Slower and more accurate picking.  Paths are picked in canvas
            /// coordinates.
            /// </summary>
            Accurate
        }
        #endregion

        #region Constant
        /// <summary>
        /// Font size of node object.
        /// </summary>
        protected const int FONT_SIZE = 10;

        /// <summary>
        /// A bit field that identifies a PenChanged event.
        /// </summary>
        /// <remarks>
        /// This field is used to indicate whether PenChanged events should be forwarded to
        /// a node's parent.
        /// <seealso cref="UMD.HCIL.Piccolo.Event.PPropertyEventArgs">PPropertyEventArgs</seealso>.
        /// <seealso cref="UMD.HCIL.Piccolo.PNode.PropertyChangeParentMask">PropertyChangeParentMask</seealso>.
        /// </remarks>
        public const int PROPERTY_CODE_PEN = 1 << 15;

        /// <summary>
        /// A bit field that identifies a PathChanged event.
        /// </summary>
        /// <remarks>
        /// This field is used to indicate whether PathChanged events should be forwarded to
        /// a node's parent.
        /// <seealso cref="UMD.HCIL.Piccolo.Event.PPropertyEventArgs">PPropertyEventArgs</seealso>.
        /// <seealso cref="UMD.HCIL.Piccolo.PNode.PropertyChangeParentMask">PropertyChangeParentMask</seealso>.
        /// </remarks>
        public const int PROPERTY_CODE_PATH = 1 << 16;

        /// <summary>
        /// The key that identifies a change in this node's <see cref="Pen">Pen</see>.
        /// </summary>
        /// <remarks>
        /// In a property change event both the old and new value will be set correctly
        /// to Pen objects.
        /// </remarks>
        protected static readonly object PROPERTY_KEY_PEN = new object();

        /// <summary>
        /// The key that identifies a change in this node's Path.
        /// </summary>
        /// <remarks>
        /// In a property change event the new value will be a reference to this node's path, but old
        /// value will always be null.
        /// </remarks>
        protected static readonly object PROPERTY_KEY_PATH = new object();

        #endregion

        #region Fields
        /// <summary>
        /// From this ComponentSetting, this object was created.
        /// </summary>
        protected ComponentSetting m_setting;
        /// <summary>
        /// Figure
        /// </summary>
        protected IFigure m_figure;
        /// <summary>
        /// On this CanvasViewComponentSet this PPathwayObject is drawn.
        /// </summary>
        protected CanvasControl m_canvas;
        /// <summary>
        /// EcellObject for this object.
        /// </summary>
        protected EcellObject m_ecellObj;
        /// <summary>
        /// PText for showing this object's ID.
        /// </summary>
        protected PText m_pText;
        /// <summary>
        /// Object will be painted with this Brush when object is not selected.
        /// </summary>
        protected Brush m_fillBrush = Brushes.White;
        /// <summary>
        /// Object will be painted with this Brush when object is not selected.
        /// </summary>
        protected Brush m_lineBrush = Brushes.Black;
        /// <summary>
        /// Object will be painted with this Brush when object is selected.
        /// </summary>
        protected Brush m_highLightBrush = Brushes.Gold;
        /// <summary>
        /// Object will be painted with this Brush when object is in invalid state.
        /// </summary>
        protected Brush m_invalidBrush = Brushes.Red;
        /// <summary>
        /// Whether this object is highlighted or not.
        /// </summary>
        protected bool m_isSelected = false;
        
        /// <summary>
        /// Whether this object is in invalid state or not.
        /// </summary>
        protected bool m_isInvalid = false;

        /// <summary>
        /// tempolary GraphicsPath.
        /// </summary>
        protected static GraphicsPath m_tempPath = new GraphicsPath();

        /// <summary>
        /// tempolary region.
        /// </summary>
        protected static Region m_tempRegion = new Region();

        /// <summary>
        /// tempolary matrix.
        /// </summary>
        protected static PMatrix TEMP_MATRIX = new PMatrix();

        /// <summary>
        /// default Pen is Black.
        /// </summary>
        protected static Pen DEFAULT_PEN = Pens.Black;

        /// <summary>
        /// this node belong the layer.
        /// </summary>
        protected PPathwayLayer m_layer;

        /// <summary>
        /// GraphicsPath.
        /// </summary>
        protected GraphicsPath m_path;

        /// <summary>
        /// Pen written this node.
        /// </summary>
        [NonSerialized]
        protected Pen m_pen;

        /// <summary>
        /// the flag whether bound from path.
        /// </summary>
        [NonSerialized]
        protected bool m_updatingBoundsFromPath;

        /// <summary>
        /// mode to pick this node.
        /// </summary>
        protected PathPickMode pickMode = PathPickMode.Fast;

        /// <summary>
        /// For memorizing a position before the start of a dragging.
        /// When the dragging failed, this object will be returned to this position.
        /// </summary>
        protected float m_originalX = 0;

        /// <summary>
        /// For memorizing a position before the start of a dragging.
        /// When the dragging failed, this object will be returned to this position.
        /// </summary>
        protected float m_originalY = 0;

        /// <summary>
        /// For memorizing a position before the start of a dragging.
        /// When the dragging failed, this object will be returned to this position.
        /// </summary>
        protected float m_originalOffsetX = 0;

        /// <summary>
        /// For memorizing a position before the start of a dragging.
        /// When the dragging failed, this object will be returned to this position.
        /// </summary>
        protected float m_originalOffsetY = 0;

        /// <summary>
        /// For memorizing a width and a height before the start of a dragging.
        /// When the dragging failed, this object will be set to this width
        /// </summary>
        protected float m_originalWidth = 0;

        /// <summary>
        /// For memorizing a width and a height before the start of a dragging.
        /// When the dragging failed, this object will be set to this width
        /// </summary>
        protected float m_originalHeight = 0;

        /// <summary>
        /// Whether this node is showing ID or not.
        /// </summary>
        protected bool m_showingId = false;

        /// <summary>
        /// Is Edit Mode or not.
        /// </summary>
        protected bool m_isViewMode = false;

        /// <summary>
        /// Parent object.
        /// </summary>
        protected PPathwaySystem m_parentSystem;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        protected ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResources));

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for EcellObject.
        /// </summary>
        public virtual EcellObject EcellObject
        {
            get {
                return this.m_ecellObj;
            }
            set
            {
                this.m_ecellObj = value;
                if (m_ecellObj.IsPosSet)
                {
                    this.CenterPointF = m_ecellObj.CenterPointF;
                    base.OffsetX = m_ecellObj.OffsetX;
                    base.OffsetY = m_ecellObj.OffsetY;
                }
                MemorizePosition();
                Refresh();
            }
        }
        /// <summary>
        /// Accessor for X coordinate.
        /// </summary>
        public PointF PointF
        {
            get { return new PointF(base.X + base.OffsetX, base.Y + base.OffsetY); }
            set
            {
                base.X = value.X;
                base.Y = value.Y;
            }
        }

        /// <summary>
        /// Accessor for X coordinate.
        /// </summary>
        public PointF CenterPointF
        {
            get { return new PointF(base.X + base.OffsetX + base.Width / 2f, base.Y + base.OffsetY + base.Height / 2f); }
            set
            {
                base.X = value.X - base.Width / 2f;
                base.Y = value.Y - base.Height / 2f;
            }
        }

        /// <summary>
        /// Accessor for m_setting.
        /// </summary>
        public ComponentSetting Setting
        {
            get { return this.m_setting; }
            set 
            {
                if (m_setting != null)
                {
                    this.m_setting.PropertyChange -= m_setting_PropertyChange;
                }
                if (value != null)
                {
                    this.m_setting = value;
                    this.m_setting.PropertyChange += new EventHandler(m_setting_PropertyChange);
                    RefreshSettings();
                }
            }
        }
        /// <summary>
        /// Accessor for m_figure.
        /// </summary>
        public IFigure Figure
        {
            get { return this.m_figure; }
            set
            {
                this.m_figure = value;
                MemorizePosition();
                m_path.Reset();
                AddPath(m_figure.GraphicsPath, false);
                ResetPosition();
                RefreshView();
            }
        }
        /// <summary>
        /// Accessor for Text.
        /// </summary>
        public PText PText
        {
            get
            {
                RefreshText();
                return m_pText;
            }
        }
        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush FillBrush
        {
            get { return this.m_fillBrush; }
            set
            {
                this.m_fillBrush = value;
                base.Brush = m_fillBrush;
            }
        }
        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush LineBrush
        {
            get { return this.m_lineBrush; }
            set {
                this.m_lineBrush = value;
                Pen = new Pen(value, 0);
            }
        }
        /// <summary>
        /// Accessor for m_highLightBrush.
        /// </summary>
        public Brush HighLightBrush
        {
            get { return this.m_highLightBrush; }
            set { this.m_highLightBrush = value; }
        }
        /// <summary>
        /// Accessor for m_isHighLighted.
        /// </summary>
        public virtual bool IsHighLighted
        {
            get { return this.m_isSelected; }
            set
            {
                this.m_isSelected = value;
                if (value)
                    this.Brush = m_highLightBrush;
                else
                    this.Brush = m_fillBrush;
            }
        }

        /// <summary>
        /// get/set m_isViewMode.
        /// </summary>
        public virtual bool ViewMode
        {
            get { return m_isViewMode; }
            set
            {
                m_isViewMode = value;
                RefreshView();
                SetTextVisiblity();
            }
        }

        /// <summary>
        /// Accessor for m_isInvalid.
        /// </summary>
        public virtual bool IsInvalid
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
        /// Accessor for m_layer.
        /// </summary>
        public virtual PPathwayLayer Layer
        {
            get { return this.m_layer; }
            set { this.m_layer = value; }
        }
        /// <summary>
        /// Accessor for an instance of CanvasViewComponentSet which this instance belongs.
        /// </summary>
        public virtual CanvasControl Canvas
        {
            get { return m_canvas; }
            set {
                m_canvas = value;

            }
        }
        /// <summary>
        /// Accessor for m_parentObject.
        /// </summary>
        public virtual PPathwaySystem ParentObject
        {
            get { return m_parentSystem; }
            set 
            { 
                m_parentSystem = value; 
            }
        }
        /// <summary>
        /// Gets or sets the pen used when rendering this node.
        /// </summary>
        /// <value>The pen used when rendering this node.</value>
        public virtual Pen Pen
        {
            get { return m_pen; }
            set
            {
                Pen old = m_pen;
                m_pen = value;
                UpdateBoundsFromPath();
                InvalidatePaint();
                FirePropertyChangedEvent(PROPERTY_KEY_PEN, PROPERTY_CODE_PEN, old, m_pen);
            }
        }
        /// <summary>
        /// get/set whether is shown ID.
        /// </summary>
        public bool ShowingID
        {
            get { return m_showingId; }
            set { m_showingId = value;
            SetTextVisiblity();
            }
        }
        /// <summary>
        /// See <see cref="GraphicsPath.PathData">GraphicsPath.PathData</see>.
        /// </summary>
        public virtual GraphicsPath Path
        {
            get { return m_path; }
        }
        /// <summary>
        /// acessor for a rectangle of this system.
        /// </summary>
        public virtual RectangleF Rect
        {
            get
            {
                return new RectangleF(base.X + this.OffsetX,
                                      base.Y + this.OffsetY,
                                      base.Width,
                                      base.Height);
            }
            set
            {
                base.X = value.X;
                base.Y = value.Y;
                base.Width = value.Width;
                base.Height = value.Height;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayObject()
        {
            m_pen = DEFAULT_PEN;
            m_path = new GraphicsPath();
            m_pText = new PText();
            m_pText.Pickable = false;
            m_pText.Font = new Font("Gothics", FONT_SIZE, FontStyle.Bold);
            this.AddChild(m_pText);
        }
        #endregion

        #region Methods
        #region Abstract Methods
        /// <summary>
        /// Create new instance of this object.
        /// </summary>
        /// <returns></returns>
        public abstract PPathwayObject CreateNewObject();
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Refresh graphical contents of this object.
        /// ex) Edges of a process can be refreshed by using this.
        /// </summary>
        public virtual void Refresh()
        {
            RefreshText();
        }

        /// <summary>
        /// Change View Mode.
        /// </summary>
        public virtual void RefreshView()
        {
            SetFillBrush();
            Refresh();
        }

        /// <summary>
        /// Get a suitable label for the specified object.
        /// </summary>
        protected static string GetLabelFor(EcellObject obj)
        {
            StringBuilder retval = new StringBuilder();
            if (obj is EcellSystem)
            {
                retval.Append(string.Format("{0} (size={1})",
                    obj.Name, ((EcellSystem)obj).SizeInVolume));
            }
            else
            {
                retval.Append(string.Format("{0}", obj.Name));
            }
            if (obj.Logged)
            {
                retval.Append("*");
            }
            return retval.ToString();
        } 

        /// <summary>
        /// Refresh Text contents of this object.
        /// </summary>
        protected virtual void RefreshText()
        {
            if (this.m_ecellObj != null)
                this.m_pText.Text = GetLabelFor(this.m_ecellObj);
            this.m_pText.CenterBoundsOnPoint(base.X + base.Width / 2, base.Y + base.Height / 2);
            this.m_pText.MoveToFront();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetTextVisiblity()
        {
            if (m_showingId)
                m_pText.Visible = true;
            else
                m_pText.Visible = false;
        }

        /// <summary>
        /// Create SVG object.
        /// </summary>
        /// <returns></returns>
        public virtual string CreateSVGObject()
        {
            string svgObj = "<!--" + this.m_ecellObj.Key + "-->\n";
            if (!base.Visible)
                return svgObj;
            // Create object
            string textBrush = BrushManager.ParseBrushToString(m_setting.TextBrush);
            string lineBrush = BrushManager.ParseBrushToString(m_setting.LineBrush);
            string fillBrush = "url(#" + m_setting.Name + ")";
            svgObj += m_figure.CreateSVGObject(this.Rect, lineBrush, fillBrush);
            // Create Text
            if (m_showingId)
            {
                PointF textPos = new PointF(m_pText.X, m_pText.Y + SVGUtil.SVG_FONT_SIZE);
                svgObj += SVGUtil.Text(textPos, m_pText.Text, textBrush);
            }
            return svgObj;
        }
        #endregion

        /// <summary>
        /// Memorize a current position for returning to this position in the future in neccessary.
        /// </summary>
        public void MemorizePosition()
        {
            this.m_originalX = base.X;
            this.m_originalY = base.Y;
            this.m_originalOffsetX = base.OffsetX;
            this.m_originalOffsetY = base.OffsetY;
            this.m_originalWidth = this.Width;
            this.m_originalHeight = this.Height;
        }
        /// <summary>
        /// Reset a current position.
        /// </summary>
        public void ResetPosition()
        {
            base.X = this.m_originalX;
            base.Y = this.m_originalY;
            base.OffsetX = this.m_originalOffsetX;
            base.OffsetY = this.m_originalOffsetY;
            base.Width = this.m_originalWidth;
            base.Height = this.m_originalHeight;
            RefreshView();
            // if system refresh children.
            if (m_ecellObj == null || !(m_ecellObj is EcellSystem))
                return;
            foreach (PPathwayObject child in m_canvas.GetAllObjectUnder(m_ecellObj.Key))
            {
                child.ResetPosition();
            }
        }
        /// <summary>
        /// Set FillBrush
        /// </summary>
        private void SetFillBrush()
        {
            if (m_setting.IsGradation)
            {
                PathGradientBrush pthGrBrush = new PathGradientBrush(m_path);
                pthGrBrush.CenterColor = BrushManager.ParseBrushToColor(m_setting.CenterBrush);
                pthGrBrush.SurroundColors = new Color[] { BrushManager.ParseBrushToColor(m_setting.FillBrush) };
                this.FillBrush = pthGrBrush;
            }
            else
            {
                this.FillBrush = m_setting.FillBrush;
            }
        }
        /// <summary>
        /// Refresh ComponentSetting.
        /// </summary>
        protected virtual void RefreshSettings()
        {
            this.PText.TextBrush = m_setting.TextBrush;
            this.LineBrush = m_setting.LineBrush;
            this.Figure = m_setting.Figure;
        }

        #region Methods to control Bounds
        //****************************************************************
        // Bounds - Methods for manipulating/updating the bounds of a
        // PProcess.
        //****************************************************************

        /// <summary>
        /// Overridden.  Set the bounds of this path.
        /// </summary>
        /// <param name="x">The new x-coordinate of the bounds/</param>
        /// <param name="y">The new y-coordinate of the bounds.</param>
        /// <param name="width">The new width of the bounds.</param>
        /// <param name="height">The new height of the bounds.</param>
        /// <returns>True if the bounds have changed; otherwise, false.</returns>
        /// <remarks>
        /// This works by scaling the path to fit into the specified bounds.  This normally
        /// works well, but if the specified base bounds get too small then it is impossible
        /// to expand the path shape again since all its numbers have tended to zero, so
        /// application code may need to take this into consideration.
        /// </remarks>
        protected override void InternalUpdateBounds(float x, float y, float width, float height)
        {
            if (m_updatingBoundsFromPath)
                return;

            RectangleF pathBounds = m_path.GetBounds();

            if (m_pen != null && m_path.PointCount > 0)
            {
                try
                {
                    m_tempPath.Reset();
                    m_tempPath.AddPath(m_path, false);

                    m_tempPath.Widen(m_pen);
                    RectangleF penPathBounds = m_tempPath.GetBounds();

                    float strokeOutset = Math.Max(penPathBounds.Width - pathBounds.Width,
                        penPathBounds.Height - pathBounds.Height);

                    x += strokeOutset / 2;
                    y += strokeOutset / 2;
                    width -= strokeOutset;
                    height -= strokeOutset;
                }
                catch (OutOfMemoryException ex)
                {
                    Trace.WriteLine(ex);
                    // Catch the case where the path is a single point
                }
            }

            float scaleX = (width == 0 || pathBounds.Width == 0) ? 1 : width / pathBounds.Width;
            float scaleY = (height == 0 || pathBounds.Height == 0) ? 1 : height / pathBounds.Height;

            TEMP_MATRIX.Reset();
            TEMP_MATRIX.TranslateBy(x, y);
            TEMP_MATRIX.ScaleBy(scaleX, scaleY);
            TEMP_MATRIX.TranslateBy(-pathBounds.X, -pathBounds.Y);

            m_path.Transform(TEMP_MATRIX.MatrixReference);
        }

        /// <summary>
        /// Returns true if this path intersects the given rectangle.
        /// </summary>
        /// <remarks>
        /// This method first checks if the interior of the path intersects with the rectangle.
        /// If not, the method then checks if the path bounding the pen stroke intersects with
        /// the rectangle.  If either of these cases are true, this method returns true.
        /// <para>
        /// <b>Performance Note</b>:  For some paths, this method can be very slow.  This is due
        /// to the implementation of IsVisible.  The problem usually occurs when many lines are
        /// joined at very steep angles.  
        /// </para>
        /// </remarks>
        /// <param name="bounds">The rectangle to check for intersection.</param>
        /// <returns>True if this path intersects the given rectangle; otherwise, false.</returns>
        public override bool Intersects(RectangleF bounds)
        {
            // Call intersects with the identity matrix.
            PMatrix matrix = new PMatrix();
            bool isIntersects = false;
            if (base.Intersects(bounds))
            {
                // Transform the bounds.
                if (!matrix.IsIdentity) bounds = matrix.Transform(bounds);

                // Set the temp region to the transformed path.
                SetTempRegion(m_path, matrix, false);

                if (Brush != null && m_tempRegion.IsVisible(bounds))
                {
                    isIntersects = true;
                }
                else if (m_pen != null)
                {
                    // Set the temp region to the transformed, widened path.
                    SetTempRegion(m_path, matrix, true);
                    isIntersects = m_tempRegion.IsVisible(bounds);
                }
            }
            return isIntersects;
        }

        /// <summary>
        /// Sets the temp region to the transformed path, widening the path if
        /// requested to do so.
        /// </summary>
        private void SetTempRegion(GraphicsPath path, PMatrix matrix, bool widen)
        {
            m_tempPath.Reset();

            if (path.PointCount > 0)
            {
                m_tempPath.AddPath(path, false);

                if (widen)
                {
                    m_tempPath.Widen(m_pen, matrix.MatrixReference);
                }
                else
                {
                    m_tempPath.Transform(matrix.MatrixReference);
                }
            }

            m_tempRegion.MakeInfinite();
            m_tempRegion.Intersect(m_tempPath);
        }
        #endregion

        #region Painting
        //****************************************************************
        // Painting - Methods for painting a PPathwayObject.
        //****************************************************************
        /// <summary>
        /// Overridden.  See <see cref="PNode.Paint">PNode.Paint</see>.
        /// </summary>
        protected override void Paint(PPaintContext paintContext)
        {
            Brush b = this.Brush;
            Graphics g = paintContext.Graphics;

            if (b != null)
            {
                g.FillPath(b, m_path);
            }

            if (m_pen != null)
            {
                g.DrawPath(m_pen, m_path);
            }
        }
        #endregion

        #region Path Support
        //****************************************************************
        // Path Support - Methods for manipulating the underlying path.
        // See System.Drawing.Drawing2D.GraphicsPath documentation for
        // more information on using these methods.
        //****************************************************************

        /// <summary>
        /// See <see cref="GraphicsPath.AddPath(GraphicsPath, bool)">GraphicsPath.AddPath</see>.
        /// </summary>
        public virtual void AddPath(GraphicsPath path, bool connect)
        {
            this.m_path.AddPath(path, connect);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// This method is called to update the bounds whenever the underlying path changes.
        /// </summary>
        public virtual void UpdateBoundsFromPath()
        {
            m_updatingBoundsFromPath = true;
            if (m_path == null || m_path.PointCount == 0)
            {
                ResetBounds();
            }
            else
            {
                try
                {
                    m_tempPath.Reset();
                    m_tempPath.AddPath(m_path, false);
                    if (m_pen != null && m_tempPath.PointCount > 0)
                        m_tempPath.Widen(m_pen);
                    RectangleF b = m_tempPath.GetBounds();
                    SetBounds(b.X, b.Y, b.Width, b.Height);
                }
                catch (OutOfMemoryException ex)
                {
                    Trace.WriteLine(ex);
                    //Catch the case where the path is a single point
                }
            }
            m_updatingBoundsFromPath = false;
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Eventhandler on ComponentSetting.PropertyChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_setting_PropertyChange(object sender, EventArgs e)
        {
            RefreshSettings();
        }

        /// <summary>
        /// event on mouse drag on this node.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDrag(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDrag(e);
            Refresh();
        }

        /// <summary>
        /// Called when the mouse leaves this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_canvas == null)
                return;
            m_canvas.FocusNode = this;
        }

        #endregion

        #endregion
    }
}
