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
using Ecell.IDE.Plugins.PathwayWindow.Graphics;

namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    /// <summary>
    /// 
    /// </summary>
    public class RoundedRectangle : FigureBase
    {
        private const float roundness = 0.1f;
        /// <summary>
        /// Figure type.
        /// </summary>
        public new const string TYPE = "RoundedRectangle";

        /// <summary>
        /// Constructor without params.
        /// </summary>
        public RoundedRectangle()
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
        public RoundedRectangle(float x, float y, float width, float height)
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
            float rx = width * roundness;
            float ry = height * roundness;
            path.AddArc(x, y, rx * 2, ry * 2, 180, 90);
            path.AddLine(x + rx, y, x + width - rx, y);
            path.AddArc(x + width - rx * 2, y, rx * 2, ry * 2, 270, 90);
            path.AddLine(x + width, y + ry, x + width, y + height - ry);
            path.AddArc(x + width - rx * 2, y + height - ry * 2, rx * 2, ry * 2, 0, 90);
            path.AddLine(x + rx, y + height, x + width - rx, y + height);
            path.AddArc(x, y + height - ry * 2, rx * 2, ry * 2, 90, 90);
            path.AddLine(x, y + ry, x, y + height - ry);
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
            float rx = rect.Width * roundness;
            float ry = rect.Height * roundness;
            string obj = SVGUtil.RoundedRectangle(rect, lineBrush, fillBrush, rx, ry);
            return obj;
        }

    }
}
