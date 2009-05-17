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
    public struct EcellLayout : ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Aliases = "Aliases";

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
	    #endregion

        #region Constructors
        /// <summary>
        /// Constructor with RectangleF.
        /// </summary>
        /// <param name="rect"></param>
        public EcellLayout(RectangleF rect)
        {
            m_rect = rect;
            m_offset = PointF.Empty;
            m_layer = "";
        }

        /// <summary>
        /// Constructor with x, y, width, and height.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public EcellLayout(float x, float y, float width, float height)
            : this(new RectangleF(x, y, width, height))
        {
        }

        /// <summary>
        /// Constructor with PointF.
        /// </summary>
        /// <param name="location"></param>
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static List<EcellLayout> ConvertFromEcellValue(EcellValue value)
        {
            List<EcellLayout> aliases = new List<EcellLayout>();
            if (value == null || !value.IsList)
                return aliases;

            List<object> list = (List<object>)value.Value;
            foreach (object obj in list)
            {
                List<object> aliasObj = (List<object>)obj;
                EcellLayout alias = new EcellLayout();
                alias.X = (float)(double)aliasObj[0];
                alias.Y = (float)(double)aliasObj[1];
                alias.Width = (float)(double)aliasObj[2];
                alias.Height = (float)(double)aliasObj[3];
                alias.OffsetX = (float)(double)aliasObj[4];
                alias.OffsetY = (float)(double)aliasObj[5];
                alias.Layer = (string)aliasObj[6];
                aliases.Add(alias);
            }
            return aliases;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aliases"></param>
        /// <returns></returns>
        public static EcellValue ConvertToEcellValue(List<EcellLayout> aliases)
        {
            List<object> list = new List<object>();
            foreach (EcellLayout alias in aliases)
            {
                List<object> aliasObj = new List<object>();
                aliasObj.Add((double)alias.X);
                aliasObj.Add((double)alias.Y);
                aliasObj.Add((double)alias.Width);
                aliasObj.Add((double)alias.Height);
                aliasObj.Add((double)alias.OffsetX);
                aliasObj.Add((double)alias.OffsetY);
                aliasObj.Add(alias.Layer);
                list.Add(aliasObj);
            }
            return new EcellValue(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool Contains(PointF pt)
        {
            return m_rect.Contains(pt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool Contains(RectangleF rect)
        {
            return m_rect.Contains(rect);
        }
        #endregion

        #region Inherited Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        { 
            if(!(obj is EcellLayout))
                return false;
            EcellLayout layout = (EcellLayout)obj;
            return (layout.Rect == m_rect) && (layout.Offset == m_offset) && (layout.Layer == m_layer);
        }
        /// <summary>
        /// GetHashCode
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
            return layout;
        }
        #endregion
    }
}
