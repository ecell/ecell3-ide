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
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EcellLib.PathwayWindow.Element
{
    /// <summary>
    /// NodeElement is an element class for a node, such as variable, process.
    /// </summary>
    [Serializable]
    public class NodeElement : ComponentElement
    {
        /// <summary>
        /// For layout use only.
        /// If this value is true, this node will not be moved by layout.
        /// </summary>
        private bool m_isFixed = false;

        /// <summary>
        /// A constructor.
        /// </summary>
        public NodeElement()
        {
        }

        /// <summary>
        /// Accessor foor fixation of this element.
        /// </summary>
        [XmlIgnore]
        public bool Fixed
        {
            get { return m_isFixed; }
            set { m_isFixed = value; }
        }

        /// <summary>
        /// Literal expression of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string returnStr = base.ToString();

            returnStr += " [CanvasID = " + base.m_canvasId;
            returnStr += ", LayerID = " + base.m_layerId;
            returnStr += ", ModelID = " + base.m_modelId;
            returnStr += ", Key = " + base.m_key;
            returnStr += ", Type = " + base.m_type;
            returnStr += ", X = " + base.m_x;
            returnStr += ", Y = " + base.m_y;
            returnStr += ", Fixed = " + m_isFixed;
            returnStr += ", CsId = " + base.m_csId;
            returnStr += ", Optional = " + base.m_optional + "]";

            return returnStr;
        }
    }
}