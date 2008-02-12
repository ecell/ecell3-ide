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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EcellLib.PathwayWindow.Figure
{
    /// <summary>
    /// FigureBase for an ellipse.
    /// </summary>
    public class EllipseFigure : FigureBase
    {
        #region Constructors
        /// <summary>
        /// Constructor with no params.
        /// </summary>
        public EllipseFigure()
        {
            Initialize(0, 0, 1, 1);
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
            Initialize(x, y, width, height);
        }
        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public EllipseFigure(float[] vars)
        {
            if (vars.Length >= 4)
                Initialize(vars[0], vars[1], vars[2], vars[3]);
            else
                Initialize(0, 0, 1, 1);
        }

        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void Initialize(float x, float y, float width, float height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
            m_type = "Ellipse";
            RectangleF rect = new RectangleF(x, y, width, height);
            m_gp.AddEllipse(rect);
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
                y = (Math.Abs(y1 - outerPoint.Y) <= Math.Abs(y2 - outerPoint.Y)) ? y1 : y2;
            }
            else if (dy == 0)
            {
                y = innerPoint.Y;
                float x1 = innerPoint.X - a;
                float x2 = innerPoint.X + a;
                x = (Math.Abs(x1 - outerPoint.X) <= Math.Abs(x2 - outerPoint.X)) ? x1 : x2;
            }
            else
            {
                float delta = dy / dx;
                float xx = b * b / ((delta * delta) + (b * b) / (a * a));
                float x1 = innerPoint.X - (float)Math.Sqrt(xx);
                float x2 = innerPoint.X + (float)Math.Sqrt(xx);
                x = (Math.Abs(x1 - outerPoint.X) <= Math.Abs(x2 - outerPoint.X)) ? x1 : x2;
                y = delta * (x - innerPoint.X) + innerPoint.Y;
            }
            return new PointF(x, y);

        }
        #endregion

    }
}