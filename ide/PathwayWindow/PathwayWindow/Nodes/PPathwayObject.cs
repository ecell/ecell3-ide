using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo;
using EcellLib;
using System.Drawing.Drawing2D;
using System.Drawing;
using UMD.HCIL.Piccolo.Util;
using System.Runtime.Serialization;

namespace EcellLib.PathwayWindow
{
    public abstract class PPathwayObject : PNode
    {

        #region Abstract Methods
        public abstract void Delete();
        public abstract bool HighLighted(bool highlight);
        public abstract void Initialize();
        public abstract void DataChanged(EcellObject ecellObj);
        public abstract void DataDeleted();
        public abstract void SelectChanged();
        public abstract void Start();
        public abstract void Change();
        public abstract void Stop();
        public abstract void End();
        public abstract List<PathwayElement> GetElements();
        public abstract PPathwayObject CreateNewObject();
        #endregion

        #region Fields
        /// <summary>
        /// On this CanvasViewComponentSet this PPathwayObject is drawn.
        /// </summary>
        protected CanvasViewComponentSet m_set;
        /// <summary>
        /// EcellObject for this object.
        /// </summary>
        protected EcellObject m_ecellObj;
        /// <summary>
        /// The name of this instance;
        /// </summary>
        protected string m_name;
        /// <summary>
        /// The ID of ComponentSetting from which this object was created
        /// </summary>
        protected string m_csId;
        /// <summary>
        /// Object will be painted with this Brush when object is not selected.
        /// </summary>
        protected Brush m_normalBrush = Brushes.White;
        /// <summary>
        /// Object will be painted with this Brush when object is selected.
        /// </summary>
        protected Brush m_highLightBrush = Brushes.Gold;
        /// <summary>
        /// Whether this object is highlighted or not.
        /// </summary>
        protected bool m_isSelected = false;
                
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

        protected static GraphicsPath TEMP_PATH = new GraphicsPath();

        protected static Region TEMP_REGION = new Region();
        protected static PMatrix TEMP_MATRIX = new PMatrix();
        protected static Pen DEFAULT_PEN = Pens.Black;
        protected const FillMode DEFAULT_FILLMODE = FillMode.Alternate;

        protected PLayer m_layer;

        protected PathwayView m_pathwayView;

        protected GraphicsPath path;
        [NonSerialized]
        protected GraphicsPath resizePath;
        [NonSerialized]
        protected Pen pen;
        [NonSerialized]
        protected bool updatingBoundsFromPath;
        protected PathPickMode pickMode = PathPickMode.Fast;
        #endregion

        #region Accessors
        public EcellObject ECellObject
        {
            get { return this.m_ecellObj; }
            set { this.m_ecellObj = value; }
        }
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush NormalBrush
        {
            get { return this.m_normalBrush; }
            set{ this.m_normalBrush = value; }
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
        /// Accessor for m_isighLighted.
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
                    this.Brush = m_normalBrush;
            }
        }
        public virtual PLayer Layer
        {
            get { return this.m_layer; }
            set { this.m_layer = value; }
        }
        public virtual PathwayView PathwayView
        {
            get { return this.m_pathwayView; }
            set { this.m_pathwayView = value; }
        }
        public virtual string CsID
        {
            get { return this.m_csId; }
            set { this.m_csId = value; }
        }
        public virtual List<PPathwayObject> ChildObjectList
        {
            get
            {
                List<PPathwayObject> returnList =  new List<PPathwayObject>();
                
                foreach(PNode node in ChildrenReference)
                {
                    if (node is PPathwayObject)
                        returnList.Add((PPathwayObject)node);
                }
                return returnList;
            }
        }
        public virtual CanvasViewComponentSet CanvasViewComponentSet
        {
            get { return m_set; }
            set { m_set = value; }
        }
        #endregion

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
            get { return pen; }
            set
            {
                Pen old = pen;
                pen = value;
                UpdateBoundsFromPath();
                InvalidatePaint();
                FirePropertyChangedEvent(PROPERTY_KEY_PEN, PROPERTY_CODE_PEN, old, pen);
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
            resizePath = new GraphicsPath();
            resizePath.AddPath(path, false);
        }

        /// <summary>
        /// Overridden.  See <see cref="PNode.EndResizeBounds">PNode.EndResizeBounds</see>.
        /// </summary>
        public override void EndResizeBounds()
        {
            resizePath = null;
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
            if (updatingBoundsFromPath || path == null)
            {
                return;
            }

            if (resizePath != null)
            {
                path.Reset();
                path.AddPath(resizePath, false);
            }

            RectangleF pathBounds = path.GetBounds();

            if (pen != null && path.PointCount > 0)
            {
                try
                {
                    TEMP_PATH.Reset();
                    TEMP_PATH.AddPath(path, false);

                    TEMP_PATH.Widen(pen);
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

            path.Transform(TEMP_MATRIX.MatrixReference);
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
        /// joined at very steep angles.  See <see cref="PProcess">PProcess Overview</see> for workarounds.
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
        /// joined at very steep angles.  See <see cref="PProcess">PProcess Overview</see> for workarounds.
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
                SetTempRegion(path, matrix, false);

                if (Brush != null && TEMP_REGION.IsVisible(bounds))
                {
                    return true;
                }
                else if (pen != null)
                {
                    // Set the temp region to the transformed, widened path.
                    SetTempRegion(path, matrix, true);
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
                    TEMP_PATH.Widen(pen, matrix.MatrixReference);
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
            if (path == null || path.PointCount == 0)
            {
                ResetBounds();
            }
            else
            {
                try
                {
                    TEMP_PATH.Reset();
                    TEMP_PATH.AddPath(path, false);
                    if (pen != null && TEMP_PATH.PointCount > 0) TEMP_PATH.Widen(pen);
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

        #region Constructor
        public PPathwayObject()
        {
            pen = DEFAULT_PEN;
            path = new GraphicsPath();
        }
        #endregion
        
        #region Painting
        //****************************************************************
        // Painting - Methods for painting a PProcess.
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
                g.FillPath(b, path);
            }

            if (pen != null)
            {
                g.DrawPath(pen, path);
            }

            //g.DrawString("X=" + this.X + "\nY=" + this.Y + "\nOffset=" + this.Offset, new Font("Arial", 6), new SolidBrush(Color.Black), new Point((int)(this.bounds.X + 5), (int)(this.bounds.Y + 10)));

        }

        protected override void PaintAfterChildren(PPaintContext paintContext)
        {
            base.PaintAfterChildren(paintContext);
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
            get { return path; }
        }

        /// <summary>
        /// See <see cref="GraphicsPath.FillMode">GraphicsPath.FillMode</see>.
        /// </summary>
        public virtual FillMode FillMode
        {
            get { return path.FillMode; }
            set
            {
                path.FillMode = value;
                InvalidatePaint();
            }
        }

        /// <summary>
        /// See <see cref="GraphicsPath.PathData">GraphicsPath.PathData</see>.
        /// </summary>
        public virtual PathData PathData
        {
            get { return path.PathData; }
        }

        /// <summary>
        /// See <see cref="GraphicsPath.PointCount">GraphicsPath.PointCount</see>.
        /// </summary>
        public virtual int PointCount
        {
            get { return path.PointCount; }
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddArc(float, float, float, float, float, float)">
        /// GraphicsPath.AddArc</see>.
        /// </summary>
        public virtual void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            path.AddArc(x, y, width, height, startAngle, sweepAngle);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddBezier(float, float, float, float, float, float, float, float)">
        /// GraphicsPath.AddBezier</see>.
        /// </summary>
        public virtual void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            path.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddClosedCurve(PointF[])">GraphicsPath.AddClosedCurve</see>.
        /// </summary>
        public virtual void AddClosedCurve(PointF[] points)
        {
            path.AddClosedCurve(points);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddCurve(PointF[])">GraphicsPath.AddCurve</see>.
        /// </summary>
        public virtual void AddCurve(PointF[] points)
        {
            path.AddCurve(points);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddEllipse(float, float, float, float)">
        /// GraphicsPath.AddEllipse</see>.
        /// </summary>
        public virtual void AddEllipse(float x, float y, float width, float height)
        {
            path.AddEllipse(x, y, width, height);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddLine(float, float, float, float)">GraphicsPath.AddLine</see>.
        /// </summary>
        public virtual void AddLine(float x1, float y1, float x2, float y2)
        {
            path.AddLine(x1, y1, x2, y2);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddPath(GraphicsPath, bool)">GraphicsPath.AddPath</see>.
        /// </summary>
        public virtual void AddPath(GraphicsPath path, bool connect)
        {
            this.path.AddPath(path, connect);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddPolygon(PointF[])">GraphicsPath.AddPolygon</see>.
        /// </summary>
        public virtual void AddPolygon(PointF[] points)
        {
            path.AddPolygon(points);
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddRectangle(RectangleF)">
        /// GraphicsPath.AddRectangle</see>.
        /// </summary>
        public virtual void AddRectangle(float x, float y, float width, float height)
        {
            path.AddRectangle(new RectangleF(x, y, width, height));
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.CloseFigure">GraphicsPath.CloseFigure</see>.
        /// </summary>
        public virtual void CloseFigure()
        {
            path.CloseFigure();
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.CloseAllFigures">GraphicsPath.CloseAllFigures</see>.
        /// </summary>
        public virtual void CloseAllFigures()
        {
            path.CloseAllFigures();
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.Reset">GraphicsPath.Reset</see>.
        /// </summary>
        public virtual void Reset()
        {
            path.Reset();
            FirePropertyChangedEvent(PROPERTY_KEY_PATH, PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
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
        /// Read this PProcess and all of its descendent nodes from the given SerializationInfo.
        /// </summary>
        /// <param name="info">The SerializationInfo to read from.</param>
        /// <param name="context">The StreamingContext of this serialization operation.</param>
        /// <remarks>
        /// This constructor is required for Deserialization.
        /// </remarks>
        /*
        protected PProcess(SerializationInfo info, StreamingContext context)
            :
            base(info, context)
        {

            pen = PUtil.ReadPen(info);
        }
        */
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

            PUtil.WritePen(pen, info);
        }

        #endregion

    }
}
