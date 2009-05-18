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
using System.Drawing;
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;

namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    /// <summary>
    /// FigureBase for an ellipse.
    /// </summary>
    public class EllipseFigure : FigureBase
    {
        /// <summary>
        /// Figure type.
        /// </summary>
        public new const string TYPE = "Ellipse";

        #region Constructors
        /// <summary>
        /// Constructor with no params.
        /// </summary>
        public EllipseFigure()
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
        public EllipseFigure(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height, TYPE);
        }
        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public EllipseFigure(float[] vars)
        {
            if (vars.Length >= 4)
                Initialize(vars[0], vars[1], vars[2], vars[3], TYPE);
            else
                Initialize(0, 0, 1, 1, TYPE);
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
            RectangleF rect = new RectangleF(x, y, width, height);
            path.AddEllipse(rect);
            return path;
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
            float dx = innerPoint.X - outerPoint.X;
            float dy = innerPoint.Y - outerPoint.Y;
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
            else if (Math.Pow((dx / a), 2) + Math.Pow((dy / b), 2) < 1)
            {
                x = innerPoint.X;
                y = innerPoint.Y;
            }
            else
            {
                float delta = dy / dx;
                float xx = b * b / ((delta * delta) + (b * b) / (a * a));
                float x1 = innerPoint.X - (float)Math.Sqrt(xx);
                float x2 = innerPoint.X + (float)Math.Sqrt(xx);
                x = (outerPoint.X <= innerPoint.X) ? x1 : x2;
                float yy = b * b / (1 + (b * b) / (a * a) * (dx * dx) / (dy * dy));
                float y1 = innerPoint.Y - (float)Math.Sqrt(yy);
                float y2 = innerPoint.Y + (float)Math.Sqrt(yy);
                y = (outerPoint.Y <= innerPoint.Y) ? y1 : y2;
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
            string obj = SVGUtil.Ellipse(rect, lineBrush, fillBrush);
            return obj;
        }

        #endregion

    }
}