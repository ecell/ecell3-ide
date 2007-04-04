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
using System.Collections;
using System.Xml.Serialization;

namespace EcellLib.PathwayWindow.Element
{
    /// <summary>
    /// Utility classs for serializing a list of PathwayElements to XML in bulk.
    /// </summary>
    [Serializable]
    public class PathwayElements
    {
        #region Fields
        /// <summary>
        /// An array of PathwayElement.
        /// </summary>
        protected PathwayElement[] m_elements;
        #endregion

        #region Accessor
        /// <summary>
        /// Accessor for m_elements.
        /// </summary>
        [XmlArrayItem(Type = typeof(AttributeElement)),
        XmlArrayItem(Type = typeof(CanvasElement)),
        XmlArrayItem(Type = typeof(ComponentElement)),
        XmlArrayItem(Type = typeof(LayerElement)),
        XmlArrayItem(Type = typeof(NodeElement)),
        XmlArrayItem(Type = typeof(PathwayElement)),
        XmlArrayItem(Type = typeof(ProcessElement)),
        XmlArrayItem(Type = typeof(SystemElement)),
        XmlArrayItem(Type = typeof(VariableElement))]
        public PathwayElement[] Elements
        {
            get { return m_elements; }
            set { m_elements = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// A constructor
        /// </summary>
        public PathwayElements()
        {
        }
        #endregion
    }
}