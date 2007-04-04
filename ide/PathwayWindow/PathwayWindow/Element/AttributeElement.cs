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

namespace EcellLib.PathwayWindow.Element
{
    /// <summary>
    /// AttributeElement is an element class for an attribute.(SIZE variable, etc.)
    /// </summary>
    [Serializable]
    public class AttributeElement : VariableElement
    {
        #region Fields
        /// <summary>
        /// Enumeration for type of attribute.
        /// </summary>
        public enum AttributeType { Size };

        /// <summary>
        /// Which type of attribute.
        /// </summary>
        protected AttributeType m_attributeType;

        /// <summary>
        /// ModelID of the object to which this attribute belongs.
        /// </summary>
        protected string m_targetModelId;

        /// <summary>
        /// Key of the object to which this attribute belongs.
        /// </summary>
        protected string m_targetKey;

        /// <summary>
        /// value of the attribute
        /// </summary>
        protected string m_value;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_attributeType.
        /// </summary>
        public AttributeType Attribute
        {
            get { return m_attributeType; }
            set { m_attributeType = value; }
        }

        /// <summary>
        /// Accessor for key.
        /// Extended for setting m_targetKey based on key value.
        /// </summary>
        public override string Key
        {
            get { return base.m_key; }
            set
            {
                base.m_key = value;
                if(m_key != null)
                    this.m_targetKey = PathUtil.GetParentSystemId(m_key);
            }
        }

        /// <summary>
        /// Accessor for m_targetModelId.
        /// </summary>
        public string TargetModelID
        {
            get { return m_targetModelId; }
            set { m_targetModelId = value; }
        }

        /// <summary>
        /// Accessor for m_targetKey.
        /// </summary>
        public string TargetKey
        {
            get { return m_targetKey; }
        }

        /// <summary>
        /// Accessor for m_value.
        /// </summary>
        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public AttributeElement()
        {
            base.m_elementType = PathwayElement.ElementType.Attribute;
            this.m_type = "variable";
        }
        #endregion
    }
}