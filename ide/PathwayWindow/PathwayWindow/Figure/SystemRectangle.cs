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
using Ecell.IDE.Plugins.PathwayWindow.Graphic;

namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemRectangle : FigureBase
    {
        /// <summary>
        /// Figure type.
        /// </summary>
        public new const string TYPE = "SystemRectangle";

        private const float rOut = 20f;
        private const float rIn = 10f;

        /// <summary>
        /// COnstructor without params.
        /// </summary>
        public SystemRectangle()
        {
            Initialize(0, 0, 1, 1, TYPE);
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
            Initialize(x, y, width, height, TYPE);
        }

        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public SystemRectangle(float[] vars)
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

            path.AddArc(x, y, rOut * 2, rOut * 2, 180, 90);
            path.AddLine(x + rOut, y, x + width - rOut, y);
            path.AddArc(x + width - rOut * 2, y, rOut * 2, rOut * 2, 270, 90);
            path.AddLine(x + width, y + rOut, x + width, y + height - rOut);
            path.AddArc(x + width - rOut * 2, y + height - rOut * 2, rOut * 2, rOut * 2, 0, 90);
            path.AddLine(x + rOut, y + height, x + width - rOut, y + height);
            path.AddArc(x, y + height - rOut * 2, rOut * 2, rOut * 2, 90, 90);
            path.AddLine(x, y + rOut, x, y + height - rOut);
            path.CloseFigure();

            path.AddArc(x + rIn, y + rIn, rIn * 2, rIn * 2, 180, 90);
            path.AddLine(x + rOut, y + rIn, x + width - rOut, y + rIn);
            path.AddArc(x + width - rOut - rIn, y + rIn, rIn * 2, rIn * 2, 270, 90);
            path.AddLine(x + width - rIn, y + rOut, x + width - rIn, y + height - rOut);
            path.AddArc(x + width - rOut - rIn, y + height - rOut - rIn, rIn * 2, rIn * 2, 0, 90);
            path.AddLine(x + width - rOut, y + height - rIn, x + rOut, y + height - rIn);
            path.AddArc(x + rIn, y + height - rOut - rIn, rIn * 2, rIn * 2, 90, 90);
            path.AddLine(x + rIn, y + height - rOut, x + rIn, y + rOut);
            return path;
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
            string obj = SVGUtil.SystemRectangle(rect, lineBrush, fillBrush, rOut, rIn);
            return obj;
        }

    }
}
