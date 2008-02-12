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
using EcellLib.PathwayWindow.Figure;
using System.Drawing;
using EcellLib.PathwayWindow;

namespace EcellLib.PathwayWindow.Figure
{
    /// <summary>
    /// FigureBase for a rectangle
    /// </summary>
    public class RectangleFigure : FigureBase
    {
        #region Constructors
        /// <summary>
        /// Constructor with no params.
        /// </summary>
        public RectangleFigure()
        {
            Initialize(0, 0, 1, 1);
        }

        /// <summary>
        /// Constructor with params.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RectangleFigure(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height);

        }

        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public RectangleFigure(float[] vars)
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
            m_type = "Rectangle";
            RectangleF rect = new RectangleF(x, y, width, height);
            m_gp.AddRectangle(rect);
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// Get contact point for this figure.
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF outerPoint, PointF innerPoint)
        {
            // Transform the coordinate system as the center of this rectangle is the original point
            // and this recntangle's width is 2.
            float a = m_width / 2;
            float b = m_height / 2;

            float x1 = innerPoint.X - a;
            float x2 = innerPoint.X + a;
            float y1 = innerPoint.Y - b;
            float y2 = innerPoint.Y + b;

            float x = 0;
            float y = 0;

            if (outerPoint.X <= x1)
                x = x1;
            else if (outerPoint.X >= x2)
                x = x2;
            else
                x = outerPoint.X;

            if (outerPoint.Y <= y1)
                y = y1;
            else if (outerPoint.Y >= y2)
                y = y2;
            else
                y = outerPoint.Y;

            return new PointF(x, y);
        }
        
        #endregion
    }
}
