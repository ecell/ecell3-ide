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

using System.Drawing;
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using System;

namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    /// <summary>
    /// 
    /// </summary>
    public class TriangleFigure : FigureBase
    {
                /// <summary>
        /// Figure type.
        /// </summary>
        public new const string TYPE = "Triangle";


        #region Constructors
        /// <summary>
        /// Constructor with no params.
        /// </summary>
        public TriangleFigure()
        {
            Initialize(0, 0, 1, 1, TYPE);
        }
        /// <summary>
        /// Constructor with params
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public TriangleFigure(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height, TYPE);
        }

        /// <summary>
        /// Create GraphicsPath
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public override GraphicsPath CreatePath(float x, float y, float width, float height)
        {
            GraphicsPath path = new GraphicsPath();
            PointF[] points = GetPoints(x, y, width, height);

            path.AddPolygon(points);
            return path;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private static PointF[] GetPoints(RectangleF rect)
        {
            return GetPoints(rect.X, rect.Y, rect.Width, rect.Height);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static PointF[] GetPoints(float x, float y, float width, float height)
        {
            float halfX = x + width / 2f;
            PointF point1 = new PointF(halfX, y);
            PointF point2 = new PointF(x, y + height);
            PointF point3 = new PointF(x + width, y + height);
            PointF[] points = { point1, point2, point3 };
            return points;
        }
        #endregion

        #region Inherited
        /// <summary>
        /// Get contact point for this figure.
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF outerPoint, PointF innerPoint)
        {
            // Transform the coordinate system as the center of this ellipse is the original point
            // and this ellipse's radius is 1.
            float dx = outerPoint.X - innerPoint.X;
            float dy = outerPoint.Y - innerPoint.Y;
            float a = m_width / 2;
            float b = m_height / 2;
            float x = 0;
            float y = 0;

            if (dx == 0)
            {
                x = innerPoint.X;
                float y1 = innerPoint.Y - b;
                float y2 = innerPoint.Y + b;
                y = (outerPoint.Y <= innerPoint.Y) ? y1 : y2;
            }
            else if (dy == 0)
            {
                y = innerPoint.Y;
                float x1 = innerPoint.X - a;
                float x2 = innerPoint.X + a;
                x = (outerPoint.X <= innerPoint.X) ? x1 : x2;
            }
            else if (Math.Abs(dx) < b / (b / a + Math.Abs(dy / dx)))
            {
                x = innerPoint.X;
                y = innerPoint.Y;
            }
            else
            {
                float xx = Math.Sign(dx) * b / (b / a + Math.Abs(dy / dx));
                float yy = Math.Sign(dy) * Math.Abs(dy / dx * xx);
                x = innerPoint.X + xx;
                y = innerPoint.Y + yy;
            }
            return new PointF(x, y);

        }

        /// <summary>
        /// Create SVG object.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="lineBrush"></param>
        /// <param name="fillBrush"></param>
        /// <returns></returns>
        public override string CreateSVGObject(RectangleF rect, string lineBrush, string fillBrush)
        {
            string obj = SVGUtil.Polygon(GetPoints(rect), lineBrush, fillBrush);
            return obj;
        }
        #endregion
    }
}
