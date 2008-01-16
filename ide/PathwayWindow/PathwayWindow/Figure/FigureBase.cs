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
    /// Concrete classe which extended FigureBase stands for 
    /// </summary>
    public abstract class FigureBase
    {
        /// <summary>
        /// X coordinate of this figure.
        /// </summary>
        protected float m_x;

        /// <summary>
        /// Y coordinate of this figure.
        /// </summary>
        protected float m_y;

        /// <summary>
        /// Width of this figure.
        /// </summary>
        protected float m_width;

        /// <summary>
        /// Height of this figure.
        /// </summary>
        protected float m_height;

        /// <summary>
        /// 
        /// </summary>
        protected string m_type;

        /// <summary>
        /// type string.
        /// </summary>
        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// type string.
        /// </summary>
        public string Coordinates 
        {
            get{
                string coordinates = m_x.ToString() + "," + m_y.ToString() + "," + m_width + "," + m_height.ToString();
                return coordinates;
            }
        }

        /// <summary>
        /// Return a contact point between an outer point and an inner point.
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public abstract PointF GetContactPoint(PointF outerPoint, PointF innerPoint);
    }
}