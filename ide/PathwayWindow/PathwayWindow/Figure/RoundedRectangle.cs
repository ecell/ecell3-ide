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
    class RoundedRectangle : FigureBase
    {
        /// <summary>
        /// Figure type.
        /// </summary>
        public new const string TYPE = "RoundedRectangle";

        /// <summary>
        /// Constructor without params.
        /// </summary>
        public RoundedRectangle()
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
        public RoundedRectangle(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height);
        }

        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public RoundedRectangle(float[] vars)
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
            m_type = TYPE;
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
            float marginX = width * 0.05f;
            float marginY = height * 0.05f;
            path.AddArc(x, y, marginX * 2, marginY * 2, 180, 90);
            path.AddLine(x + marginX, y, x + width - marginX, y);
            path.AddArc(x + width - marginX * 2, y, marginX * 2, marginY * 2, 270, 90);
            path.AddLine(x + width, y + marginY, x + width, y + height - marginY);
            path.AddArc(x + width - marginX * 2, y + height - marginY * 2, marginX * 2, marginY * 2, 0, 90);
            path.AddLine(x + marginX, y + height, x + width - marginX, y + height);
            path.AddArc(x, y + height - marginY * 2, marginX * 2, marginY * 2, 90, 90);
            path.AddLine(x, y + marginY, x, y + height - marginY);
            return path;
        }
    }
}
