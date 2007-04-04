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
    /// LayerElement is an element class for a layer.
    /// </summary>
    [Serializable]
    public class LayerElement : PathwayElement
    {
        #region Fields
        /// <summary>
        /// Canvas ID, of which canvas this layer belongs to.
        /// </summary>
        protected string m_canvasId;

        /// <summary>
        /// Layer ID of this layer.
        /// </summary>
        protected string m_layerId;

        /// <summary>
        /// Z-Order of this layer
        /// </summary>
        protected int m_zorder;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_canvasId.
        /// </summary>
        public string CanvasID
        {
            get { return m_canvasId; }
            set { m_canvasId = value; }
        }

        /// <summary>
        /// Accessor for m_layerId.
        /// </summary>
        public string LayerID
        {
            get { return m_layerId; }
            set { m_layerId = value; }
        }

        /// <summary>
        /// Accessor for m_zorder.
        /// </summary>
        public int ZOrder
        {
            get { return m_zorder; }
            set { m_zorder = value; }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public LayerElement()
        {
            base.m_elementType = PathwayElement.ElementType.Layer;
        }

        /// <summary>
        /// Return literal expression of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string returnStr = base.ToString();

            returnStr += " [CanvasID = " + m_canvasId;
            returnStr += ", LayerID = " + m_layerId;
            returnStr += ", ZOrder = " + m_zorder + "]";

            return returnStr;
        }
    }
}
