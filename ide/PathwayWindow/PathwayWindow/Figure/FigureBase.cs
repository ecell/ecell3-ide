//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using System.Drawing;
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.Graphics;

namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    /// <summary>
    /// Concrete classe which extended FigureBase stands for 
    /// </summary>
    public class FigureBase : IFigure
    {
        #region Fields
        /// <summary>
        /// X coordinate of this figure.
        /// </summary>
        protected float m_x = 0;

        /// <summary>
        /// Y coordinate of this figure.
        /// </summary>
        protected float m_y = 0;

        /// <summary>
        /// Width of this figure.
        /// </summary>
        protected float m_width = 0;

        /// <summary>
        /// Height of this figure.
        /// </summary>
        protected float m_height = 0;

        /// <summary>
        /// Figure type.
        /// </summary>
        protected string m_type;
        /// <summary>
        /// Figure type.
        /// </summary>
        public const string TYPE = "FigureBase";
        /// <summary>
        /// GraphicsPath
        /// </summary>
        protected GraphicsPath m_gp = null;
        #endregion

        #region Accessors
        /// <summary>
        /// type string.
        /// </summary>
        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// X coodinate.
        /// </summary>
        public float X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        /// <summary>
        /// Y coodinate.
        /// </summary>
        public float Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// Width.
        /// </summary>
        public float Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        /// Height.
        /// </summary>
        public float Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        /// Contact points of edge. 
        /// </summary>
        public virtual PointF[] ContactPoints
        {
            get
            {
                PointF[] points =
                {
                    new PointF(this.X, this.Y),
                    new PointF(this.X, this.Y + this.Height /2f),
                    new PointF(this.X, this.Y + this.Height),
                    new PointF(this.X + this.Width /2f, this.Y),
                    new PointF(this.X + this.Width /2f, this.Y + this.Height),
                    new PointF(this.X + this.Width, this.Y),
                    new PointF(this.X + this.Width, this.Y + this.Height /2f),
                    new PointF(this.X + this.Width, this.Y + this.Height)
                };
                return points;
            }
        }

        /// <summary>
        /// Coordinates string.
        /// </summary>
        public string Coordinates
        {
            get
            {
                string coordinates = m_x + "," + m_y + "," + m_width + "," + m_height;
                return coordinates;
            }
        }

        /// <summary>
        /// Accessor for m_gp.
        /// </summary>
        public GraphicsPath GraphicsPath
        {
            get { return m_gp; }
        }

        /// <summary>
        /// Create new GraphicsPath for the icon image.
        /// </summary>
        public GraphicsPath IconPath
        {
            get
            {
                GraphicsPath iconPath = (GraphicsPath)m_gp.Clone();
                RectangleF rect = m_gp.GetBounds();
                Matrix matrix = new Matrix();
                float scale = 14f;

                matrix.Translate(-1f * (rect.X + rect.Width / 2f),
                                 -1f * (rect.Y + rect.Height / 2f));

                iconPath.Transform(matrix);

                matrix = new Matrix();
                if (rect.Width > rect.Height)
                {
                    matrix.Scale(scale / rect.Width, scale / rect.Width);
                    matrix.Translate(rect.Width / 2f, rect.Width / 2f);
                }
                else
                {
                    matrix.Scale(scale / rect.Height, scale / rect.Height);
                    matrix.Translate(rect.Height / 2f, rect.Height / 2f);
                }

                iconPath.Transform(matrix);
                return iconPath;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public FigureBase()
        {
            Initialize(0, 0, 1, 1, TYPE);
        }

        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public FigureBase(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height, TYPE);
        }
        /// <summary>
        /// Initialize this figure.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="type"></param>
        protected void Initialize(float x, float y, float width, float height, string type)
        {
            m_type = type;
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
            m_gp = CreatePath(x, y, width, height);
        }

        #endregion

        #region Virtual Methods
        /// <summary>
        /// Create new GraphicsPath
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual GraphicsPath CreatePath(float x, float y, float width, float height)
        {
            return new GraphicsPath();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public GraphicsPath CreatePath(RectangleF rect)
        {
            return CreatePath(rect.X, rect.Y, rect.Width, rect.Height);
        }

        ///// <summary>
        ///// Return a contact point between an outer point and an inner point.
        ///// </summary>
        ///// <param name="outerPoint"></param>
        ///// <param name="innerPoint"></param>
        ///// <returns></returns>
        //public virtual PointF GetContactPoint(PointF outerPoint, PointF innerPoint)
        //{
        //    // Transform the coordinate system as the center of this rectangle is the original point
        //    // and this recntangle's width is 2.
        //    float a = m_width / 2;
        //    float b = m_height / 2;

        //    float x1 = innerPoint.X - a;
        //    float x2 = innerPoint.X + a;
        //    float y1 = innerPoint.Y - b;
        //    float y2 = innerPoint.Y + b;

        //    float x = 0;
        //    float y = 0;

        //    if (outerPoint.X <= x1)
        //        x = x1;
        //    else if (outerPoint.X >= x2)
        //        x = x2;
        //    else
        //        x = outerPoint.X;

        //    if (outerPoint.Y <= y1)
        //        y = y1;
        //    else if (outerPoint.Y >= y2)
        //        y = y2;
        //    else
        //        y = outerPoint.Y;

        //    // if outerPoint is inside of this rect.
        //    if (x == outerPoint.X && y == outerPoint.Y)
        //        return innerPoint;

        //    return new PointF(x, y);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public virtual PointF GetContactPoint(PointF outerPoint, PointF innerPoint)
        {
            PointF relationalPosition = new PointF(outerPoint.X - innerPoint.X + this.Width / 2f, outerPoint.Y - innerPoint.Y + this.Height / 2f);
            PointF relationalContact = GetNearestPoint(relationalPosition, ContactPoints);
            PointF contact = new PointF(relationalContact.X + innerPoint.X - this.Width / 2f, relationalContact.Y + innerPoint.Y - this.Height / 2f);
            return contact;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public PointF GetNearestPoint(PointF pos, PointF[] points)
        {
            PointF ans = points[0];
            foreach (PointF point in points)
            {
                if (GetDistance(point, pos) < GetDistance(ans, pos))
                    ans = point;
            }
            return ans;
        }

        /// <summary>
        /// Get distance between two points
        /// </summary>
        /// <param name="point1">point 1</param>
        /// <param name="point2">point 2</param>
        /// <returns>Distance between point 1 and point 2</returns>
        public static float GetDistance(PointF point1, PointF point2)
        {
            float dist = (float)Math.Sqrt(Math.Pow((double)(point1.X - point2.X), 2d) + Math.Pow((double)(point1.Y - point2.Y), 2d));
            return dist;
        }

        /// <summary>
        /// Create SVG object.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="lineBrush"></param>
        /// <param name="fillBrush"></param>
        /// <returns></returns>
        public virtual string CreateSVGObject(RectangleF rect, string lineBrush, string fillBrush)
        {
            string obj = "<!--FigureBase-->\n";
            obj += SVGUtil.Rectangle(rect, lineBrush, fillBrush);
            return obj;
        }


        #endregion


        #region ICloneable ÉÅÉìÉo
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IFigure Clone()
        {
            return FigureManager.CreateFigure(m_type, m_x, m_y, m_width, m_height);
        }
        #endregion
    }
}