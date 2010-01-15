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
//

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Base object for Pathway.
    /// </summary>
    public class PPathwayNode: PNode, IDisposable
    {
        #region Constant
        /// <summary>
        /// Font size of node object.
        /// </summary>
        protected const int FONT_SIZE = 10;

        /// <summary>
        /// default Pen is Black.
        /// </summary>
        protected static readonly Pen DEFAULT_PEN = Pens.Black;

        #endregion

        #region Fields
        /// <summary>
        /// GraphicsPath.
        /// </summary>
        [NonSerialized]
        protected GraphicsPath m_path = new GraphicsPath();

        /// <summary>
        /// Pen written this node.
        /// </summary>
        [NonSerialized]
        protected Pen m_pen = DEFAULT_PEN;

        /// <summary>
        /// PText for showing this object's ID.
        /// </summary>
        protected PText m_pText = new PText();
        #endregion

        #region Properties
        #region Properties for Layout
        /// <summary>
        /// 
        /// </summary>
        public float Top
        {
            get { return base.Y + base.OffsetY; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Bottom
        {
            get { return base.Y + base.OffsetY + base.Height; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Left
        {
            get { return base.X + base.OffsetX; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Right
        {
            get { return base.X + base.OffsetX + base.Width; }
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

        /// <summary>
        /// Accessor for X coordinate.
        /// </summary>
        public PointF CenterPointF
        {
            get { return new PointF(base.X + base.OffsetX + base.Width / 2f, base.Y + base.OffsetY + base.Height / 2f); }
            set
            {
                base.X = value.X - base.Width / 2f - base.OffsetX;
                base.Y = value.Y - base.Height / 2f - base.OffsetY;
            }
        }
        #endregion

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
        /// Accessor for Text.
        /// </summary>
        public string Text
        {
            get { return m_pText.Text; }
            set
            {
                m_pText.Text = value;
                RefreshText();
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayNode()
        {
            m_pText.Pickable = false;
            m_pText.Font = new Font("Arial", FONT_SIZE, FontStyle.Bold);
            this.AddChild(m_pText);
        }
        
        #endregion
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
        protected virtual void RefreshText()
        {
            this.m_pText.CenterBoundsOnPoint(base.X + base.Width / 2, base.Y + base.Height / 2);
            this.m_pText.MoveToFront();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPoint"></param>
        /// <param name="contactPoint"></param>
        /// <returns></returns>
        public static double GetDistance(PointF refPoint, PointF contactPoint)
        {
            return Math.Sqrt(Math.Pow((double)(refPoint.X - contactPoint.X), 2) + Math.Pow((double)(refPoint.Y - contactPoint.Y), 2));
        }


        #region IDisposable メンバ
        /// <summary>
        /// Event on Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }

        #endregion

        #region Merged from PPath
        /// <summary>
        /// tempolary GraphicsPath.
        /// </summary>
        [NonSerialized]
        protected static GraphicsPath m_tempPath = new GraphicsPath();

        /// <summary>
        /// tempolary region.
        /// </summary>
        [NonSerialized]
        protected static Region m_tempRegion = new Region();

        /// <summary>
        /// tempolary matrix.
        /// </summary>
        [NonSerialized]
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
        /// See <see cref="GraphicsPath.AddRectangle(RectangleF)">GraphicsPath.AddRectangle</see>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void AddRect(float x, float y, float width, float height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(x, y, width, height));
            AddPath(path, false);
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
                    Debug.WriteLine(ex);
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
                    Debug.WriteLine(ex);
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
            System.Drawing.Graphics g = paintContext.Graphics;

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
}
