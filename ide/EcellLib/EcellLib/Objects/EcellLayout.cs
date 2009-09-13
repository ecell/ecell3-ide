﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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

namespace Ecell.Objects
{
    /// <summary>
    /// EcellLayout
    /// This class contains 4 float variable and 1 string variable which show the layout of EcellObject.
    /// </summary>
    [Serializable]
    public class EcellLayout : ICloneable
    {
        #region Fields
        /// <summary>
        /// RectangleF
        /// </summary>
        private RectangleF m_rect;
        /// <summary>
        /// Offset
        /// </summary>
        private PointF m_offset;
        /// <summary>
        /// Layer
        /// </summary>
        private string m_layer;
        /// <summary>
        /// Figure string.
        /// </summary>
        private string m_figure;
	    #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public EcellLayout()
            : this(new Rectangle())
        {
        }
        /// <summary>
        /// Constructor with RectangleF.
        /// </summary>
        /// <param name="rect">Rectangle object.</param>
        public EcellLayout(RectangleF rect)
        {
            m_rect = rect;
            m_offset = PointF.Empty;
            m_layer = "";
            m_figure = "";
        }

        /// <summary>
        /// Constructor with x, y, width, and height.
        /// </summary>
        /// <param name="x">X of layout region</param>
        /// <param name="y">Y of layout region</param>
        /// <param name="width">width of layout region</param>
        /// <param name="height">height of layout region</param>
        public EcellLayout(float x, float y, float width, float height)
            : this(new RectangleF(x, y, width, height))
        {
        }

        /// <summary>
        /// Constructor with PointF.
        /// </summary>
        /// <param name="location">the location.</param>
        public EcellLayout(PointF location)
            : this(RectangleF.Empty)
        {
            m_rect.Location = location;
        }


        #endregion

        #region Accessors
        /// <summary>
        /// get empty EcellLayout.
        /// </summary>
        public static EcellLayout Empty
        {
            get { return new EcellLayout(); }
        }

        /// <summary>
        /// get/set the Figure property.
        /// </summary>
        public string Figure
        {
            get { return m_figure; }
            set { m_figure = value; }
        }

        /// <summary>
        /// get/set the layer property.
        /// </summary>
        public string Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public RectangleF Rect
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public PointF Location
        {
            get { return m_rect.Location; }
            set { m_rect.Location = value; }
        }

        /// <summary>
        /// X coordinate
        /// </summary>
        public float X
        {
            get { return m_rect.X; }
            set { m_rect.X = value; }
        }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public float Y
        {
            get { return m_rect.Y; }
            set { m_rect.Y = value; }
        }

        /// <summary>
        /// Size
        /// </summary>
        public SizeF Size
        {
            get { return m_rect.Size; }
            set { m_rect.Size = value; }
        }

        /// <summary>
        /// Width
        /// </summary>
        public float Width
        {
            get { return m_rect.Width; }
            set { m_rect.Width = value; }
        }

        /// <summary>
        /// Height
        /// </summary>
        public float Height
        {
            get { return m_rect.Height; }
            set { m_rect.Height = value; }
        }

        /// <summary>
        /// Offset
        /// </summary>
        public PointF Offset
        {
            get { return m_offset; }
            set { m_offset = value; }
        }

        /// <summary>
        /// X offset
        /// </summary>
        public float OffsetX
        {
            get { return m_offset.X; }
            set { m_offset.X = value; }
        }

        /// <summary>
        /// Y offset
        /// </summary>
        public float OffsetY
        {
            get { return m_offset.Y; }
            set { m_offset.Y = value; }
        }

        /// <summary>
        /// Accessor for Center.
        /// </summary>
        public PointF Center
        {
            get { return new PointF(CenterX, CenterY); }
            set
            {
                CenterX = value.X;
                CenterY = value.Y;
            }
        }

        /// <summary>
        /// Accessor for CenterX.
        /// </summary>
        public float CenterX
        {
            get { return m_rect.X + m_rect.Width / 2f; }
            set { m_rect.X = value - m_rect.Width / 2f; }
        }

        /// <summary>
        /// Accessor for CenterY.
        /// </summary>
        public float CenterY
        {
            get { return m_rect.Y + m_rect.Height / 2f; }
            set { m_rect.Y = value - m_rect.Height / 2f; }
        }

        /// <summary>
        /// Top
        /// </summary>
        public float Top 
        {
            get { return m_rect.Top;}
        }

        /// <summary>
        /// Bottom
        /// </summary>
        public float Bottom 
        {
            get { return m_rect.Bottom; } 
        }

        /// <summary>
        /// Left
        /// </summary>
        public float Left 
        {
            get { return m_rect.Left; }
        }

        /// <summary>
        /// Right
        /// </summary>
        public float Right
        {
            get { return m_rect.Right; }
        }

        /// <summary>
        /// IsEmpty
        /// </summary>
        public bool IsEmpty
        {
            get { return this.Location.IsEmpty && m_rect.IsEmpty && m_offset.IsEmpty; }
        }
        #endregion

        #region Method
        /// <summary>
        /// Check whether this point contains this object.
        /// </summary>
        /// <param name="pt">the target point.</param>
        /// <returns>Return true if this point contains this object.</returns>
        public bool Contains(PointF pt)
        {
            return m_rect.Contains(pt);
        }
        /// <summary>
        /// Check whether this rectangle contains this object
        /// </summary>
        /// <param name="rect">the target rectangle</param>
        /// <returns>Return true if this rectangle contains this object.</returns>
        public bool Contains(RectangleF rect)
        {
            return m_rect.Contains(rect);
        }
        #endregion

        #region Inherited Method
        /// <summary>
        /// Equals override function.
        /// </summary>
        /// <param name="obj">the src object.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        { 
            if(!(obj is EcellLayout))
                return false;
            EcellLayout layout = (EcellLayout)obj;
            return (layout.Rect == m_rect) && (layout.Offset == m_offset) && (layout.Layer == m_layer);
        }
        /// <summary>
        /// GetHashCode override function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = m_rect.GetHashCode() ^ m_offset.GetHashCode();
            if(m_layer != null)
                hash = hash ^ m_layer.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Get information of this struct.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Width + ", " + Height + ", " + OffsetX + ", " + OffsetY + ", " + Layer + ")";
        }
	    #endregion

        #region ICloneable メンバ
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public EcellLayout Clone()
        {
            EcellLayout layout = new EcellLayout();
            layout.Rect = m_rect;
            layout.Offset = m_offset;
            layout.Layer = m_layer;
            layout.Figure = m_figure;
            return layout;
        }
        #endregion
    }
}
