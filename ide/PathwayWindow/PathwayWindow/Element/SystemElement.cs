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
using EcellLib.PathwayWindow;

namespace EcellLib.PathwayWindow.Element
{
    /// <summary>
    /// SystemElement is an element class for a system.
    /// </summary>
    [Serializable]
    public class SystemElement : ComponentElement
    {
        #region Enum
        /// <summary>
        /// Relationship between two systems.
        /// </summary>
        public enum Relation {Superior, Equal, Inferior, NoRelation};
        #endregion

        #region Public static readonly
        /// <summary>
        /// An outer radius of round-shaped corner of a system.
        /// </summary>
        public static readonly float OUTER_RADIUS = 20f;

        /// <summary>
        /// An inner radius of round-shaped corner of a system.
        /// </summary>
        public static readonly float INNER_RADIUS = 10f;

        /// <summary>
        /// Margin between lower hem and PText for a name of a system.
        /// </summary>
        public static readonly float TEXT_LOWER_MARGIN = 20f;
        #endregion

        #region Fields
        /// <summary>
        /// X offset of a system.
        /// </summary>
        protected float m_offsetX;

        /// <summary>
        /// Y offset of a system.
        /// </summary>
        protected float m_offsetY;

        /// <summary>
        /// Width of a system.
        /// </summary>
        protected float m_width;

        /// <summary>
        /// Height of a system.
        /// </summary>
        protected float m_height;

        /// <summary>
        /// The number of child nodes (variables, processes) below this system.
        /// </summary>
        protected int m_childNodeNum;

        /// <summary>
        /// The number of child systems below this system.
        /// </summary>
        protected int m_childSystemNum;

        /// <summary>
        /// Half of system thickness
        /// </summary>
        protected float m_halfThickness = 0;
        #endregion

        #region Accessors
        public float OffsetX
        {
            get { return m_offsetX; }
            set { m_offsetX = value; }
        }
        public float OffsetY
        {
            get { return m_offsetY; }
            set { m_offsetY = value; }
        }
        public float Width
        {
            get { return m_width; }
            set { m_width = value; }
        }
        public float Height
        {
            get { return m_height; }
            set { m_height = value; }
        }
        public int ChildNodeNum
        {
            get { return m_childNodeNum; }
            set { m_childNodeNum = value; }
        }
        public int ChildSystemNum
        {
            get { return m_childSystemNum; }
            set { m_childSystemNum = value; }
        }
        public float HalfThickness
        {
            get
            {
                if (m_halfThickness == 0)
                    m_halfThickness = (OUTER_RADIUS - INNER_RADIUS) / 2f;
                return m_halfThickness;
            }
        }
        public float TextCenterX
        {
            get
            {
                return m_x + m_offsetX + m_width / 2;
            }
        }
        public float TextCenterY
        {
            get
            {
                return m_y + m_offsetY + m_height - TEXT_LOWER_MARGIN;
            }
        }
        public float TextLowerMargin
        {
            get
            {
                return TEXT_LOWER_MARGIN;
            }
        }
        #endregion

        #region Constructors
        public SystemElement()
        {
            base.m_elementType = PathwayElement.ElementType.System;
            this.m_type = "System";
            m_childNodeNum = 0;
            m_childSystemNum = 0;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Check hierarchical relation between two systems.
        /// </summary>
        /// <param name="system">a system which you want to check relation to this object.</param>
        /// <returns>
        /// This method returns
        ///    Relation.Superior: when an argument is superior to this object.
        ///    Relation.Equal: when an arugment is equal to this object.
        ///    Relation.Inferior: when an argument is inferior to this object.
        ///    Relation.NoRelation: when an argument has no relation with this object.
        /// </returns>
        public Relation CheckRelation(string system)
        {
            if (system == null || system.Equals(""))
                return Relation.NoRelation;
            else if (m_key.StartsWith(system))
                return Relation.Superior;
            else if (system.StartsWith(m_key))
                return Relation.Inferior;
            else
                return Relation.Equal;
        }
        
        /// <summary>
        /// Literal expression of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string returnStr = base.ToString();

            returnStr += " [CanvasID = " + m_canvasId;
            returnStr += ", LayerID = " + m_layerId;
            returnStr += ", ModelID = " + m_modelId;
            returnStr += ", Key = " + m_key;
            returnStr += ", Type = " + m_type;
            returnStr += ", X = " + m_x;
            returnStr += ", Y = " + m_y;
            returnStr += ", Width = " + m_width;
            returnStr += ", Height = " + m_height;
            returnStr += ", CsId = " + m_csId;
            returnStr += ", Optional = " + m_optional + "]";

            return returnStr;
        }
        #endregion
    }
}