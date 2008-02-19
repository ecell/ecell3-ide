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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EcellLib.PathwayWindow.Figure
{
    class SystemRectangle : FigureBase
    {

        /// <summary>
        /// COnstructor without params.
        /// </summary>
        public SystemRectangle()
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
        public SystemRectangle(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height);
        }

        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public SystemRectangle(float[] vars)
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
        protected override void Initialize(float x, float y, float width, float height)
        {
            m_type = "SystemRectangle";
            base.SetBounds(x, y, width, height);
            m_gp = CreatePath(x, y, width, height);

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

            float marginX1 = 20f;
            float marginY1 = 20f;
            path.AddArc(x, y, marginX1 * 2, marginY1 * 2, 180, 90);
            path.AddLine(x + marginX1, y, x + width - marginX1, y);
            path.AddArc(x + width - marginX1 * 2, y, marginX1 * 2, marginY1 * 2, 270, 90);
            path.AddLine(x + width, y + marginY1, x + width, y + height - marginY1);
            path.AddArc(x + width - marginX1 * 2, y + height - marginY1 * 2, marginX1 * 2, marginY1 * 2, 0, 90);
            path.AddLine(x + marginX1, y + height, x + width - marginX1, y + height);
            path.AddArc(x, y + height - marginY1 * 2, marginX1 * 2, marginY1 * 2, 90, 90);
            path.AddLine(x, y + marginY1, x, y + height - marginY1);
            path.CloseFigure();

            float marginX2 = 10f;
            float marginY2 = 10f;
            path.AddArc(x + marginX2, y + marginY2, marginX2 * 2, marginY2 * 2, 180, 90);
            path.AddLine(x + marginX1, y + marginY2, x + width - marginX1, y + marginY2);
            path.AddArc(x + width - marginX1 - marginX2, y + marginY2, marginX2 * 2, marginY2 * 2, 270, 90);
            path.AddLine(x + width - marginX2, y + marginY1, x + width - marginX2, y + height - marginY1);
            path.AddArc(x + width - marginX1 - marginX2, y + height - marginY1 - marginY2, marginX2 * 2, marginY2 * 2, 0, 90);
            path.AddLine(x + width - marginX1, y + height - marginY2, x + marginX1, y + height - marginY2);
            path.AddArc(x + marginX2, y + height - marginY1 - marginY2, marginX2 * 2, marginY2 * 2, 90, 90);
            path.AddLine(x + marginX2, y + height - marginY1, x + marginX2, y + marginY1);
            return path;
        }

    }
}
