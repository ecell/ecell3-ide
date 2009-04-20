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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.Objects;
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Event;
using System.Diagnostics;
using UMD.HCIL.Piccolo.Util;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Line
    /// </summary>
    public class PPathwayLine : PNode, IDisposable
    {
        #region Constants
        /// <summary>
        ///  Arrow design settings
        /// </summary>
        internal const float ARROW_DEGREE = 18f / 360f;
        /// <summary>
        /// pi
        /// </summary>
        internal const float PI2 = (float)Math.PI * 2;
        /// <summary>
        ///  Arrow design settings
        /// radian = x / 360 * 2pi, x = ARROW_DEGREE, 2pi = 6.283
        /// </summary>
        internal const float ARROW_RADIAN_A = ARROW_DEGREE * PI2;

        /// <summary>
        ///  Arrow design settings
        /// </summary>
        internal const float ARROW_RADIAN_B = PI2 * (1 - ARROW_DEGREE);

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        internal const float ARROW_LENGTH = 15;

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        internal const float LINE_WIDTH = 2;

        /// <summary>
        /// 
        /// </summary>
        internal static readonly Brush DefaultEdgeBrush = Brushes.Black;

        /// <summary>
        /// 
        /// </summary>
        internal static readonly Brush SelectedBrush = Brushes.Yellow;
        #endregion

        #region Fields
        /// <summary>
        /// On this CanvasViewComponentSet this PPathwayObject is drawn.
        /// </summary>
        private CanvasControl m_canvas;

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
        /// this line stands for this EdgeInfo.
        /// </summary>
        private EdgeInfo m_edgeInfo;

        /// <summary>
        /// Coordinate of the variable side end point in global coordinate system.
        /// </summary>
        private PointF m_varPoint;

        /// <summary>
        /// Coordinate of the process side end point in global coordinate system.
        /// </summary>
        private PointF m_proPoint;
        /// <summary>
        /// 
        /// </summary>
        private PPathwayVariable m_variable;
        /// <summary>
        /// 
        /// </summary>
        private PPathwayProcess m_process;

        /// <summary>
        /// 
        /// </summary>
        private bool m_selected = false;

        /// <summary>
        /// 
        /// </summary>
        private float m_width = LINE_WIDTH;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_edgeInfo.
        /// </summary>
        public EdgeInfo Info
        {
            get { return m_edgeInfo; }
            set { this.m_edgeInfo = value; }
        }
        
        /// <summary>
        /// See <see cref="GraphicsPath.PathData">GraphicsPath.PathData</see>.
        /// </summary>
        public virtual GraphicsPath Path
        {
            get { return m_path; }
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
                m_pen = value;
                AddPen(value);
            }
        }
        /// <summary>
        /// Accessor for m_varPoint.
        /// </summary>
        public PointF VarPoint
        {
            get { return m_varPoint; }
            set { this.m_varPoint = value; }
        }

        /// <summary>
        /// Accessor for m_proPoint.
        /// </summary>
        public PointF ProPoint
        {
            get { return m_proPoint; }
            set { this.m_proPoint = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float EdgeWidth
        {
            get { return m_width; }
            set
            {
                this.m_width = value;
                this.Pen.Width = m_width;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush EdgeBrush
        {
            get { return this.Brush; }
            set
            {
                this.Brush = value;
                this.Pen.Brush = this.Brush;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Selected
        {
            get { return m_selected; }
            set
            {
                m_selected = value;
                if (value)
                {
                    this.EdgeBrush = LineHandler.LINE_BRUSH;
                }
                else
                {
                    this.EdgeBrush = m_canvas.Control.Animation.EdgeBrush;
                }
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayLine(CanvasControl canvas)
            : this(canvas, new EdgeInfo())
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="edgeInfo"></param>
        public PPathwayLine(CanvasControl canvas, EdgeInfo edgeInfo)
        {
            m_canvas = canvas;
            m_edgeInfo = edgeInfo;
            m_path = new GraphicsPath();
            
            SetEdge(DefaultEdgeBrush, m_width);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="edgeInfo"></param>
        /// <param name="process"></param>
        /// <param name="variable"></param>
        public PPathwayLine(CanvasControl canvas, EdgeInfo edgeInfo, PPathwayProcess process, PPathwayVariable variable)
            : this(canvas, edgeInfo)
        {
            m_variable = variable;
            m_process = process;
            m_variable.Relations.Add(this);
            m_process.Relations.Add(this);

            SetEdge(canvas.Control.Animation.EdgeBrush, m_width);
            base.Pickable = (variable.Visible && process.Visible);
            base.Visible = (variable.Visible && process.Visible);

            m_varPoint = variable.GetContactPoint(process.CenterPointF);
            m_proPoint = process.GetContactPoint(m_varPoint);
            this.DrawLine();
            
        }

        /// <summary>
        /// Refresh Line.
        /// </summary>
        public void Refresh()
        {
            if (m_variable == null || m_process == null)
                return;
            m_varPoint = m_variable.GetContactPoint(m_process.CenterPointF);
            m_proPoint = m_process.GetContactPoint(m_varPoint);
            DrawLine();
            this.Visible = m_process.Visible && m_variable.Visible;
            this.Pickable = this.Visible;
        }

        /// <summary>
        /// Visible change
        /// </summary>
        public void VisibleChange()
        {
            bool visible = m_process.Visible && m_variable.Visible;
            this.Visible = visible;
            this.Pickable = visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="width"></param>
        public void SetEdge(Brush brush, float width)
        {
            this.Brush = brush;
            this.m_width = width;
            this.Pen = new Pen(brush, width);
        }

        /// <summary>
        /// Draw Line.
        /// </summary>
        public void DrawLine()
        {
            if (m_proPoint == m_varPoint)
                return;

            //Set Pen
            this.Pen = new Pen(Brush, m_width);

            //Set line
            GraphicsPath path = new GraphicsPath();
            SetLine(path);
            //Set Arrow
            SetArrow(path);
            this.AddPath(path, false);
        }

        /// <summary>
        /// Set line
        /// </summary>
        /// <param name="path"></param>
        private void SetLine(GraphicsPath path)
        {
            try
            {
                if (m_edgeInfo.LineType == LineType.Dashed)
                    AddDashedLine(path, m_proPoint.X, m_proPoint.Y, m_varPoint.X, m_varPoint.Y);
                else
                    path.AddLine(m_proPoint.X, m_proPoint.Y, m_varPoint.X, m_varPoint.Y);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Set Arrow.
        /// </summary>
        /// <param name="path"></param>
        private void SetArrow(GraphicsPath path)
        {
            if (m_process != null && m_process.ViewMode && m_edgeInfo.Coefficient != 1 && !m_edgeInfo.IsEndNode)
                return;
            switch (this.m_edgeInfo.Direction)
            {
                case EdgeDirection.Bidirection:
                    path.AddPolygon(GetArrowPoints(m_proPoint, m_varPoint));
                    path.AddPolygon(GetArrowPoints(m_varPoint, m_proPoint));
                    break;
                case EdgeDirection.Inward:
                    path.AddPolygon(GetArrowPoints(m_proPoint, m_varPoint));
                    break;
                case EdgeDirection.Outward:
                    path.AddPolygon(GetArrowPoints(m_varPoint, m_proPoint));
                    break;
                case EdgeDirection.None:
                    break;
            }
        }

        /// <summary>
        /// add the dash line to PPath.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="startX">the position of start.</param>
        /// <param name="startY">the position of start.</param>
        /// <param name="endX">the position of end.</param>
        /// <param name="endY">the position of end.</param>
        private void AddDashedLine(GraphicsPath path, float startX, float startY, float endX, float endY)
        {
            float repeatNum = (float)Math.Sqrt(Math.Pow(endX - startX, 2) + Math.Pow(endY - startY, 2)) / 8f;

            float xMovement = (endX - startX) / repeatNum;
            float yMovement = (endY - startY) / repeatNum;
            float xFragment = xMovement * 0.6f;
            float yFragment = yMovement * 0.6f;

            float presentX = startX;
            float presentY = startY;
            for (int i = 0; i < repeatNum; i++)
            {
                path.AddLine(presentX, presentY, presentX + xFragment, presentY + yFragment);
                path.CloseFigure();

                presentX += xMovement;
                presentY += yMovement;
            }
        }

        /// <summary>
        /// Get coordinates of an arrow head.
        /// </summary>
        /// <param name="arrowApex">an apex of an arrow</param>
        /// <param name="guidePoint">an arrow line goes direction from arrowApex to guidePoint</param>
        /// <returns></returns>
        internal static PointF[] GetArrowPoints(PointF arrowApex, PointF guidePoint)
        {
            guidePoint.X = guidePoint.X - arrowApex.X;
            guidePoint.Y = guidePoint.Y - arrowApex.Y;

            float factor = PathUtil.GetDistance(guidePoint, new Point(0, 0));
            if (factor == 0)
                return new PointF[] { arrowApex, arrowApex, arrowApex };
            guidePoint.X = guidePoint.X / factor;
            float guideRadian = (float)Math.Acos(guidePoint.X);
            if (guidePoint.Y < 0)
                guideRadian = PI2 - guideRadian;

            PointF arrowPointA = new PointF((float)Math.Cos(ARROW_RADIAN_A + guideRadian), (float)Math.Sin(ARROW_RADIAN_A + guideRadian));
            PointF arrowPointB = new PointF((float)Math.Cos(ARROW_RADIAN_B + guideRadian), (float)Math.Sin(ARROW_RADIAN_B + guideRadian));

            arrowPointA.X = arrowPointA.X * ARROW_LENGTH + arrowApex.X;
            arrowPointA.Y = arrowPointA.Y * ARROW_LENGTH + arrowApex.Y;
            arrowPointB.X = arrowPointB.X * ARROW_LENGTH + arrowApex.X;
            arrowPointB.Y = arrowPointB.Y * ARROW_LENGTH + arrowApex.Y;

            return new PointF[] { arrowApex, arrowPointA, arrowPointB };
        }

        #region EventHandlers
        /// <summary>
        /// Called when the mouse up.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_canvas == null)
                return;
            m_canvas.NotifyResetSelect();
            m_canvas.FocusNode = this;
            m_canvas.LineHandler.AddSelectedLine(this);
        }
        #endregion


        #region IDisposable ÉÅÉìÉo
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }

        #endregion



        #region Merged from PPath
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
        /// the flag whether bound from path.
        /// </summary>
        [NonSerialized]
        protected bool m_updatingBoundsFromPath;

        #region Path Support
        //****************************************************************
        // Path Support - Methods for manipulating the underlying path.
        // See System.Drawing.Drawing2D.GraphicsPath documentation for
        // more information on using these methods.
        //****************************************************************

        /// <summary>
        /// See <see cref="GraphicsPath.Reset">GraphicsPath.Reset</see>.
        /// </summary>
        public virtual void Reset()
        {
            m_path.Reset();
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// See <see cref="GraphicsPath.AddPath(GraphicsPath, bool)">GraphicsPath.AddPath</see>.
        /// </summary>
        public virtual void AddPath(GraphicsPath path, bool connect)
        {
            m_path.Reset();
            m_path.AddPath(path, connect);
            FirePropertyChangedEvent(new object(), PPath.PROPERTY_CODE_PATH, null, path);
            UpdateBoundsFromPath();
            InvalidatePaint();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pen"></param>
        public virtual void AddPen(Pen pen)
        {
            UpdateBoundsFromPath();
            InvalidatePaint();
            FirePropertyChangedEvent(new object(), PPath.PROPERTY_CODE_PEN, null, pen);
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
        #endregion
    }

    #region Enum
    /// <summary>
    /// Enumeration for a direction of a edge
    /// </summary>
    public enum EdgeDirection
    {
        /// <summary>
        /// Outward direction
        /// </summary>
        Outward,
        /// <summary>
        /// Inward direction
        /// </summary>
        Inward,
        /// <summary>
        /// Outward and inward direction
        /// </summary>
        Bidirection,
        /// <summary>
        /// An edge has no direction
        /// </summary>
        None
    }
    /// <summary>
    /// Enumeration of a type of a line.
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        Unknown,
        /// <summary>
        /// Solid line
        /// </summary>
        Solid,
        /// <summary>
        /// Dashed line
        /// </summary>
        Dashed
    }
    #endregion

    /// <summary>
    /// EdgeInfo contains all information for one edge.
    /// </summary>
    public class EdgeInfo
    {

        #region Fields

        /// <summary>
        /// Key of a process, an owner of this edge.
        /// </summary>
        protected string m_proKey;

        /// <summary>
        /// Key of a variable with which a process has an edge.
        /// </summary>
        protected string m_varKey;

        private List<EcellReference> m_refList = null;

        /// <summary>
        /// Direction of this edge.
        /// </summary>
        protected EdgeDirection m_direction = EdgeDirection.None;

        /// <summary>
        /// Type of a line of this edge.
        /// </summary>
        protected LineType m_type = LineType.Unknown;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EdgeInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="list"></param>
        /// <param name="er"></param>
        public EdgeInfo(string processKey, List<EcellReference> list, EcellReference er)
        {
            m_refList = list;
            bool bidir = CheckBidir(list, er);

            m_proKey = processKey;
            m_varKey = er.Key;
            // Set Relation
            int l_coef = er.Coefficient;
            if (bidir)
            {
                m_direction = EdgeDirection.Bidirection;
                m_type = LineType.Solid;
            }
            else if (l_coef < 0)
            {
                m_direction = EdgeDirection.Inward;
                m_type = LineType.Solid;
            }
            else if (l_coef == 0)
            {
                m_direction = EdgeDirection.None;
                m_type = LineType.Dashed;
            }
            else
            {
                m_direction = EdgeDirection.Outward;
                m_type = LineType.Solid;
            }

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processKey">The key of process.</param>
        /// <param name="er">The reference of EcellObject.</param>
        public EdgeInfo(string processKey, EcellReference er)
        {
            m_proKey = processKey;
            // Set Relation
            int coef = er.Coefficient;
            if (coef < 0)
            {
                m_direction = EdgeDirection.Inward;
                m_type = LineType.Solid;
            }
            else if (coef == 0)
            {
                m_direction = EdgeDirection.None;
                m_type = LineType.Dashed;
            }
            else
            {
                m_direction = EdgeDirection.Outward;
                m_type = LineType.Solid;
            }
            m_varKey = er.Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="varKey"></param>
        /// <param name="direction"></param>
        public EdgeInfo(string processKey, string varKey, EdgeDirection direction)
        {
            m_proKey = processKey;
            // Set Relation
            if (direction == EdgeDirection.Inward)
            {
                m_direction = EdgeDirection.Inward;
                m_type = LineType.Solid;
            }
            else if (direction == EdgeDirection.None)
            {
                m_direction = EdgeDirection.None;
                m_type = LineType.Dashed;
            }
            else if (direction == EdgeDirection.Outward)
            {
                m_direction = EdgeDirection.Outward;
                m_type = LineType.Solid;
            }
            else
            {
                m_direction = EdgeDirection.Bidirection;
                m_type = LineType.Solid;
            }
            m_varKey = varKey;
        }

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string EdgeKey
        {
            get { return m_varKey + ":" + m_direction.ToString(); }
        }
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string VariableKey
        {
            get { return m_varKey; }
            set { m_varKey = value; }
        }
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string ProcessKey
        {
            get { return m_proKey; }
            set { m_proKey = value; }
        }
        /// <summary>
        /// Accessor for m_direction.
        /// </summary>
        public EdgeDirection Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }
        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        public LineType LineType
        {
            get { return m_type; }
            set { m_type = value; }
        }
        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        public int Coefficient
        {
            get
            {
                int coefficient = 0;
                switch (this.m_direction)
                {
                    case EdgeDirection.Inward:
                        coefficient = -1;
                        break;
                    case EdgeDirection.None:
                        coefficient = 0;
                        break;
                    case EdgeDirection.Outward:
                        coefficient = 1;
                        break;
                }
                return coefficient;
            }
        }

        /// <summary>
        /// IsEndNode
        /// </summary>
        public bool IsEndNode
        {
            get
            {
                return CheckEndNode(m_refList);
            }
        }
        #endregion

        private static bool CheckBidir(List<EcellReference> list, EcellReference er)
        {
            bool bidir = false;
            foreach (EcellReference er1 in list)
            {
                if (er.Key.Equals(er1.Key) &&
                    er.Coefficient != 0 &&
                    er.Coefficient == -1 * er1.Coefficient)
                {
                    bidir = true;
                    break;
                }
            }
            return bidir;
        }

        private static bool CheckEndNode(List<EcellReference> list)
        {
            bool isEndNode = true;
            foreach (EcellReference er in list)
            {
                if (er.Coefficient != 1)
                    continue;
                isEndNode = false;
                break;
            }
            return isEndNode;
        }

    }
}
