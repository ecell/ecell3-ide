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
    public class SystemEllipse : FigureBase
    {
        /// <summary>
        /// Figure type.
        /// </summary>
        public new const string TYPE = "SystemEllipse";

        private const float R = 10;
        /// <summary>
        /// Constructor without params.
        /// </summary>
        public SystemEllipse()
        {
            Initialize(0, 0, 1, 1, TYPE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public SystemEllipse(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height, TYPE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public override GraphicsPath CreatePath(float x, float y, float width, float height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(x, y, width, height);
            float innerwidth = width - R * 2;
            float innerheight = height - R * 2;

            if (width > R * 2f && height > R * 2)
            {
                path.AddArc(x + R, y + R, innerwidth, innerheight, 0, 360);
                //path.AddArc(x + R, y + R, innerwidth, innerheight, 180, 360);
            }
            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="lineBrush"></param>
        /// <param name="fillBrush"></param>
        /// <returns></returns>
        public override string CreateSVGObject(RectangleF rect, string lineBrush, string fillBrush)
        {
            string svgObj =  SVGUtil.SystemEllipse(rect, lineBrush, fillBrush, R);
            return svgObj;
        }

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

    }
}
