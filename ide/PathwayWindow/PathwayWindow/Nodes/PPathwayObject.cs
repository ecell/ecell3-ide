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
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.Serialization;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib;
using System.ComponentModel;
using UMD.HCIL.Piccolo.Event;
using EcellLib.PathwayWindow.Resources;

namespace EcellLib.PathwayWindow.Nodes
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

        #region Fields
        /// <summary>
        /// Font size of node object.
        /// </summary>
        protected static readonly int m_nodeTextFontSize = 10;
        /// <summary>
        /// From this ComponentSetting, this object was created.
        /// </summary>
        protected ComponentSetting m_setting;
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
        /// The ID of ComponentSetting from which this object was created
        /// </summary>
        protected string m_csId;
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
        /// This object.Pickable before freeze() method called.
        /// </summary>
        protected bool m_isPickableBeforeFreeze = false;

        /// <summary>
        /// The key that identifies a change in this node's <see cref="Pen">Pen</see>.
        /// </summary>
        /// <remarks>
        /// In a property change event both the old and new value will be set correctly
        /// to Pen objects.
        /// </remarks>
        protected static readonly object PROPERTY_KEY_PEN = new object();

        /// <summary>
        /// A bit field that identifies a <see cref="PenChanged">PenChanged</see> event.
        /// </summary>
        /// <remarks>
        /// This field is used to indicate whether PenChanged events should be forwarded to
        /// a node's parent.
        /// <seealso cref="UMD.HCIL.Piccolo.Event.PPropertyEventArgs">PPropertyEventArgs</seealso>.
        /// <seealso cref="UMD.HCIL.Piccolo.PNode.PropertyChangeParentMask">PropertyChangeParentMask</seealso>.
        /// </remarks>
        public const int PROPERTY_CODE_PEN = 1 << 15;

        /// <summary>
        /// The key that identifies a change in this node's <see cref="PathReference">Path</see>.
        /// </summary>
        /// <remarks>
        /// In a property change event the new value will be a reference to this node's path, but old
        /// value will always be null.
        /// </remarks>
        protected static readonly object PROPERTY_KEY_PATH = new object();

        /// <summary>
        /// A bit field that identifies a <see cref="PathChanged">PathChanged</see> event.
        /// </summary>
        /// <remarks>
        /// This field is used to indicate whether PathChanged events should be forwarded to
        /// a node's parent.
        /// <seealso cref="UMD.HCIL.Piccolo.Event.PPropertyEventArgs">PPropertyEventArgs</seealso>.
        /// <seealso cref="UMD.HCIL.Piccolo.PNode.PropertyChangeParentMask">PropertyChangeParentMask</seealso>.
        /// </remarks>
        public const int PROPERTY_CODE_PATH = 1 << 16;

        /// <summary>
        /// tempolary GraphicsPath.
        /// </summary>
        protected static GraphicsPath TEMP_PATH = new GraphicsPath();

        /// <summary>
        /// tempolary region.
        /// </summary>
        protected static Region TEMP_REGION = new Region();

        /// <summary>
        /// tempolary matrix.
        /// </summary>
        protected static PMatrix TEMP_MATRIX = new PMatrix();

        /// <summary>
        /// default Pen is Black.
        /// </summary>
        protected static Pen DEFAULT_PEN = Pens.Black;

        /// <summary>
        /// FillMode of this Node.
        /// </summary>
        protected const FillMode DEFAULT_FILLMODE = FillMode.Alternate;

        /// <summary>
        /// this node belong the layer.
        /// </summary>
        protected PPathwayLayer m_layer;

        /// <summary>
        /// GraphicsPath.
        /// </summary>
        protected GraphicsPath m_path;

        /// <summary>
        /// GraphicsPath for resize.
        /// </summary>
        [NonSerialized]
        protected GraphicsPath m_resizePath;

        /// <summary>
        /// Pen written this node.
        /// </summary>
        [NonSerialized]
        protected Pen m_pen;

        /// <summary>
        /// the flag whether bound from path.
        /// </summary>
        [NonSerialized]
        protected bool updatingBoundsFromPath;

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
        /// Canvas ID.
        /// </summary>
        protected string m_canvasID = null;

        /// <summary>
        /// Parent object.
        /// </summary>
        protected PPathwayObject m_parentObject;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        protected ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

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
                    base.X = m_ecellObj.X;
                    base.Y = m_ecellObj.Y;
                    base.OffsetX = m_ecellObj.OffsetX;
                    base.OffsetY = m_ecellObj.OffsetY;
                }
                MemorizePosition();
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
            set { this.m_setting = value; }
        }

        /// <summary>
        /// Accessor for Text.
        /// </summary>
        public PText Text
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
            set {
                this.m_fillBrush = value;
                base.Brush = value;
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
                Pen = new Pen(value, 1);
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
                if (m_isViewMode == value)
                    return;
                m_isViewMode = value;
                ChangeViewMode(value);
                SetTextVisiblity();
            }
        }

        /// <summary>
        /// Accessor for m_isInvalid.
        /// </summary>
        public virtual bool IsInvalid
        {
            get { return this.m_isInvalid; }
            set { this.m_isInvalid = value; }
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
        /// Accessor for m_csId.
        /// </summary>
        public virtual string CsID
        {
            get { return this.m_csId; }
            set { this.m_csId = value; }
        }
        /// <summary>
        /// Accessor for an instance of CanvasViewComponentSet which this instance belongs.
        /// </summary>
        public virtual CanvasControl CanvasControl
        {
            get { return m_canvas; }
            set {
                m_canvas = value;
            }
        }
        /// <summary>
        /// Accessor for m_parentObject.
        /// </summary>
        public virtual PPathwayObject ParentObject
        {
            get { return m_parentObject; }
            set { m_parentObject = value; }
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
            m_pText.Font = new Font("Gothics", m_nodeTextFontSize, System.Drawing.FontStyle.Bold);
            this.AddChild(m_pText);
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Delete
        /// </summary>
        public abstract void Delete();
        /// <summary>
        /// Highlighted
        /// </summary>
        /// <param name="highlight"></param>
        /// <returns></returns>
        public abstract bool HighLighted(bool highlight);
        /// <summary>
        /// Initialize
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// DataChanged
        /// </summary>
        /// <param name="ecellObj"></param>
        public abstract void DataChanged(EcellObject ecellObj);
        /// <summary>
        /// DataDeleted
        /// </summary>
        public abstract void DataDeleted();
        /// <summary>
        /// SelectChanged
        /// </summary>
        public abstract void SelectChanged();
        /// <summary>
        /// Start
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Change
        /// </summary>
        public abstract void Change();
        /// <summary>
        /// Stop
        /// </summary>
        public abstract void Stop();
        /// <summary>
        /// End
        /// </summary>
        public abstract void End();

        /// <summary>
        /// Create new instance of this object.
        /// </summary>
        /// <returns></returns>
        public abstract PPathwayObject CreateNewObject();
        #endregion

        #region Pen
        //****************************************************************
        // Pen - Methods for changing the pen used when rendering the
        // PProcess.
        //****************************************************************

        /// <summary>
        /// Occurs when there is a change in this node's <see cref="Pen">Pen</see>.
        /// </summary>
        /// <remarks>
        /// When a user attaches an event handler to the PenChanged Event as in
        /// PenChanged += new PPropertyEventHandler(aHandler),
        /// the add method adds the handler to the delegate for the event
        /// (keyed by PROPERTY_KEY_PEN in the Events list).
        /// When a user removes an event handler from the PenChanged event as in 
        /// PenChanged -= new PPropertyEventHandler(aHandler),
        /// the remove method removes the handler from the delegate for the event
        /// (keyed by PROPERTY_KEY_PEN in the Events list).
        /// </remarks>
        public virtual event PPropertyEventHandler PenChanged
        {
            add { HandlerList.AddHandler(PROPERTY_KEY_PEN, value); }
            remove { HandlerList.RemoveHandler(PROPERTY_KEY_PEN, value); }
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
        #endregion

        #region Picking Mode
        /// <summary>
        /// Gets or sets the mode used to pick this node.
        /// <seealso cref="PathPickMode">PathPickMode</seealso>
        /// </summary>
        /// <value>The mode used to pick this node.</value>
        public virtual PathPickMode PickMode
        {
            get { return pickMode; }
            set
            {
                this.pickMode = value;
            }
        }
        #endregion

        #region Bounds
        //****************************************************************
        // Bounds - Methods for manipulating/updating the bounds of a
        // PProcess.
        //****************************************************************

        /// <summary>
        /// Overridden.  See <see cref="PNode.StartResizeBounds">PNode.StartResizeBounds</see>.
        /// </summary>
        public override void StartResizeBounds()
        {
            m_resizePath = new GraphicsPath();
            m_resizePath.AddPath(m_path, false);
        }

        /// <summary>
        /// Overridden.  See <see cref="PNode.EndResizeBounds">PNode.EndResizeBounds</see>.
        /// </summary>
        public override void EndResizeBounds()
        {
            m_resizePath = null;
        }

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
            if (updatingBoundsFromPath || m_path == null)
            {
                return;
            }

            if (m_resizePath != null)
            {
                m_path.Reset();
                m_path.AddPath(m_resizePath, false);
            }

            RectangleF pathBounds = m_path.GetBounds();

            if (m_pen != null && m_path.PointCount > 0)
            {
                try
                {
                    TEMP_PATH.Reset();
                    TEMP_PATH.AddPath(m_path, false);

                    TEMP_PATH.Widen(m_pen);
                    RectangleF penPathBounds = TEMP_PATH.GetBounds();

                    float strokeOutset = Math.Max(penPathBounds.Width - pathBounds.Width,
                        penPathBounds.Height - pathBounds.Height);

                    x += strokeOutset / 2;
                    y += strokeOutset / 2;
                    width -= strokeOutset;
                    height -= strokeOutset;
                }
                catch (OutOfMemoryException)
                {
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
            return Intersects(bounds, new PMatrix());
        }

        /// <summary>
        /// Overridden.  Performs picking in canvas coordinates if <see cref="PickMode">PickMode</see>
        /// is false.
        /// </summary>
        /// <remarks>
        /// Due to the implementation of the GraphicsPath object, picking in canvas coordinates
        /// is more accurate, but will introduce a significant performance hit.
        /// </remarks>
        protected override bool PickAfterChildren(PPickPath pickPath)
        {
            if (pickMode == PathPickMode.Fast)
            {
                return base.PickAfterChildren(pickPath);
            }
            else
            {
                return Intersects(pickPath.PickBounds, pickPath.GetPathTransformTo(this));
            }
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
        /// <param name="matrix">
        /// A matrix object that specifies a transform to apply to the path and bounds before
        /// checking for an intersection.
        /// </param>
        /// <returns>True if this path intersects the given rectangle; otherwise, false.</returns>
        public virtual bool Intersects(RectangleF bounds, PMatrix matrix)
        {
            if (base.Intersects(bounds))
            {
                // Transform the bounds.
                if (!matrix.IsIdentity) bounds = matrix.Transform(bounds);

                // Set the temp region to the transformed path.
                SetTempRegion(m_path, matrix, false);

                if (Brush != null && TEMP_REGION.IsVisible(bounds))
                {
                    return true;
                }
                else if (m_pen != null)
                {
                    // Set the temp region to the transformed, widened path.
                    SetTempRegion(m_path, matrix, true);
                    return TEMP_REGION.IsVisible(bounds);
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the temp region to the transformed path, widening the path if
        /// requested to do so.
        /// </summary>
        private void SetTempRegion(GraphicsPath path, PMatrix matrix, bool widen)
        {
            TEMP_PATH.Reset();

            if (path.PointCount > 0)
            {
                TEMP_PATH.AddPath(path, false);

                if (widen)
                {
                    TEMP_PATH.Widen(m_pen, matrix.MatrixReference);
                }
                else
                {
                    TEMP_PATH.Transform(matrix.MatrixReference);
                }
            }

            TEMP_REGION.MakeInfinite();
            TEMP_REGION.Intersect(TEMP_PATH);
        }

        /// <summary>
        /// This method is called to update the bounds whenever the underlying path changes.
        /// </summary>
        public virtual void UpdateBoundsFromPath()
        {
            updatingBoundsFromPath = true;
            if (m_path == null || m_path.PointCount == 0)
            {
                ResetBounds();
            }
            else
            {
                try
                {
                    TEMP_PATH.Reset();
                    TEMP_PATH.AddPath(m_path, false);
                    if (m_pen != null && TEMP_PATH.PointCount > 0) TEMP_PATH.Widen(m_pen);
                    RectangleF b = TEMP_PATH.GetBounds();
                    SetBounds(b.X, b.Y, b.Width, b.Height);
                }
                catch (OutOfMemoryException)
                {
                    //Catch the case where the path is a single point
                }
            }
            updatingBoundsFromPath = false;
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
        /// Occurs when there is a change in this node's <see cref="Pen">Pen</see>.
        /// </summary>
        /// <remarks>
        /// When a user attaches an event handler to the PathChanged Event as in
        /// PathChanged += new PPropertyEventHandler(aHandler),
        /// the add method adds the handler to the delegate for the event
        /// (keyed by PROPERTY_KEY_PATH in the Events list).
        /// When a user removes an event handler from the PathChanged event as in 
        /// PathChanged -= new PPropertyEventHandler(aHandler),
        /// the remove method removes the handler from the delegate for the event
        /// (keyed by PROPERTY_KEY_PATH in the Events list).
        /// </remarks>
        public virtual event PPropertyEventHandler PathChanged
        {
            add { HandlerList.AddHandler(PROPERTY_KEY_PATH, value); }
            remove { HandlerList.RemoveHandler(PROPERTY_KEY_PATH, value); }
        }

        /// <summary>
        /// Gets a reference to the underlying path object.
        /// </summary>
        /// <value>The underlying path object.</value>
        public virtual GraphicsPath PathReference
        {
            get { return m_path; }
        }

        /// <summary>
        /// See <see cref="GraphicsPath.FillMode">GraphicsPath.FillMode</see>.
        /// </summary>
        public virtual FillMode FillMode
        {
            get { return m_path.FillMode; }
            set
            {
                m_path.FillMode = value;
                InvalidatePaint();
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
        public virtual PathData PathData
        {
            get { return m_path.PathData; }
        }

        /// <summary>
        /// See <see cref="GraphicsPath.PointCount">GraphicsPath.PointCount</see>.
        /// </summary>
        public virtual int PointCount
        {
            get { return m_path.PointCount; }
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
        }
        /// <summary>
        /// See <see cref="GraphicsPath.AddArc(float, float, float, float, float, float)">
        /// GraphicsPath.AddArc</see>.
        /// </summary>
        public virtual void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            m_path.AddArc(x, y, width, height, startAngle, sweepAngle);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddBezier(float, float, float, float, float, float, float, float)">
        /// GraphicsPath.AddBezier</see>.
        /// </summary>
        public virtual void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            m_path.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddClosedCurve(PointF[])">GraphicsPath.AddClosedCurve</see>.
        /// </summary>
        public virtual void AddClosedCurve(PointF[] points)
        {
            m_path.AddClosedCurve(points);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddCurve(PointF[])">GraphicsPath.AddCurve</see>.
        /// </summary>
        public virtual void AddCurve(PointF[] points)
        {
            m_path.AddCurve(points);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddEllipse(float, float, float, float)">
        /// GraphicsPath.AddEllipse</see>.
        /// </summary>
        public virtual void AddEllipse(float x, float y, float width, float height)
        {
            m_path.AddEllipse(x, y, width, height);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddLine(float, float, float, float)">GraphicsPath.AddLine</see>.
        /// </summary>
        public virtual void AddLine(float x1, float y1, float x2, float y2)
        {
            m_path.AddLine(x1, y1, x2, y2);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

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
        /// See <see cref="GraphicsPath.AddPolygon(PointF[])">GraphicsPath.AddPolygon</see>.
        /// </summary>
        public virtual void AddPolygon(PointF[] points)
        {
            m_path.AddPolygon(points);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddRectangle(RectangleF)">
        /// GraphicsPath.AddRectangle</see>.
        /// </summary>
        public virtual void AddRectangle(float x, float y, float width, float height)
        {
            m_path.AddRectangle(new RectangleF(x, y, width, height));
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.CloseFigure">GraphicsPath.CloseFigure</see>.
        /// </summary>
        public virtual void CloseFigure()
        {
            m_path.CloseFigure();
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.CloseAllFigures">GraphicsPath.CloseAllFigures</see>.
        /// </summary>
        public virtual void CloseAllFigures()
        {
            m_path.CloseAllFigures();
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.Reset">GraphicsPath.Reset</see>.
        /// </summary>
        public virtual void Reset()
        {
            m_path.Reset();
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, m_path);
            UpdateBoundsFromPath();
            InvalidatePaint();
            RefreshText();
        }

        /// <summary>
        /// Cancel offsets of this object's all parent.
        /// </summary>
        public virtual void CancelAllParentOffsets()
        {
            PNode dummyParent = null;
            do
            {
                if (dummyParent == null)
                    dummyParent = this.Parent;
                else
                    dummyParent = dummyParent.Parent;
                this.X += dummyParent.OffsetX;
                this.Y += dummyParent.OffsetY;
                
            }while(dummyParent != this.Root);
        }

        #endregion

        #region Messaging between subclasses
        /// <summary>
        /// Notify children about movement.
        /// </summary>
        public virtual void NotifyMovement()
        {
            foreach (PPathwayObject obj in m_canvas.GetAllObjectUnder(this.EcellObject.key))
            {
                obj.NotifyMovement();
            }
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Memorize a current position for returning to this position in the future in neccessary.
        /// </summary>
        public virtual void MemorizePosition()
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
        public virtual void ResetPosition()
        {
            base.X = this.m_originalX;
            base.Y = this.m_originalY;
            base.OffsetX = this.m_originalOffsetX;
            base.OffsetY = this.m_originalOffsetY;
            base.Width = this.m_originalWidth;
            base.Height = this.m_originalHeight;
            RefreshText();
            foreach (PPathwayObject child in m_canvas.GetAllObjectUnder(this.EcellObject.key))
            {
                child.ResetPosition();
            }
        }

        /// <summary>
        /// Refresh graphical contents of this object.
        /// ex) Edges of a process can be refreshed by using this.
        /// </summary>
        public virtual void Refresh()
        {
            RefreshText();
        }
        /// <summary>
        /// Refresh Text contents of this object.
        /// </summary>
        public virtual void RefreshText()
        {
            if (this.m_ecellObj != null)
                this.m_pText.Text = this.m_ecellObj.Text;
            
            this.m_pText.CenterBoundsOnPoint(base.X + base.Width / 2, base.Y + base.Height / 2);
            this.m_pText.MoveToFront();
        }
        
        /// <summary>
        /// start to move this Node by drag.
        /// </summary>
        public virtual void MoveStart()
        {
        }

        /// <summary>
        /// end to move this Node by drag.
        /// </summary>
        public virtual void MoveEnd()
        {
        }

        /// <summary>
        /// Change View Mode.
        /// </summary>
        public virtual void ChangeViewMode(bool isViewMode)
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// Make this object freezed.
        /// </summary>
        public virtual void Freeze()
        {
            m_isPickableBeforeFreeze = this.Pickable;
            this.Pickable = false;
        }

        /// <summary>
        /// Reset freeze status.
        /// </summary>
        public virtual void Unfreeze()
        {
            this.Pickable = m_isPickableBeforeFreeze;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetSetting()
        {
            LineBrush = m_setting.LineBrush;
            FillBrush = m_setting.FillBrush;
        }

        protected virtual void SetTextVisiblity()
        {
            if (m_showingId)
                m_pText.Visible = true;
            else
                m_pText.Visible = false;
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Called when the mouse enters this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseEnter(PInputEventArgs e)
        {
            base.OnMouseEnter(e);
            if (m_canvas == null)
                return;
            m_canvas.FocusNode = this;
        }

        /// <summary>
        /// Called when the mouse leaves this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseLeave(PInputEventArgs e)
        {
            base.OnMouseLeave(e);
            if (m_canvas == null)
                return;
            m_canvas.FocusNode = null;
        }

        /// <summary>
        /// Called when the mouse leaves this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseUp(PInputEventArgs e)
        {
            base.OnMouseUp(e);
            if (m_canvas == null)
                return;
            m_canvas.FocusNode = this;
        }

        /// <summary>
        /// event on double click this system.
        /// if there are system outside this system,
        /// it fire event at outside system.
        /// </summary>
        /// <param name="e"></param>
        public override void OnDoubleClick(PInputEventArgs e)
        {
            if (m_ecellObj == null)
                return;
            PropertyEditor.Show(m_ecellObj);
        }

        #endregion

        #region Serialization
        //****************************************************************
        // Serialization - Nodes conditionally serialize their parent.
        // This means that only the parents that were unconditionally
        // (using GetObjectData) serialized by someone else will be restored
        // when the node is deserialized.
        //****************************************************************
        /// <summary>
        /// Write this PProcess and all of its descendent nodes to the given SerializationInfo.
        /// </summary>
        /// <param name="info">The SerializationInfo to write to.</param>
        /// <param name="context">The streaming context of this serialization operation.</param>
        /// <remarks>
        /// This node's parent is written out conditionally, that is it will only be written out
        /// if someone else writes it out unconditionally.
        /// </remarks>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            PUtil.WritePen(m_pen, info);
        }

        #endregion

    }
}
