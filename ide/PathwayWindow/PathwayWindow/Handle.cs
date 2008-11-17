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
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.

using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Event;

namespace Ecell.IDE.Plugins.PathwayWindow
{
    /// <summary>
    /// Attached to tag of ToolStripMenuItem
    /// </summary>
    public class Handle
    {
        #region Fields
        /// <summary>
        /// This handle's mode
        /// </summary>
        private Mode m_mode;

        /// <summary>
        /// EventHandler
        /// </summary>
        private PBasicInputEventHandler m_handler = null;

        /// <summary>
        /// ComponentType
        /// </summary>
        private ComponentType m_cType;

        /// <summary>
        /// Zooming rate of canvas
        /// </summary>
        private float m_zoomingRate;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for mode of this handle.
        /// </summary>
        public Mode Mode
        {
            get { return this.m_mode; }
        }

        /// <summary>
        /// EventHandler
        /// </summary>
        public PBasicInputEventHandler EventHandler
        {
            get { return m_handler; }
            set { m_handler = value; }
        }

        /// <summary>
        /// Accessor for component setting's ID
        /// </summary>
        public ComponentType ComponentType
        {
            get { return this.m_cType; }
        }

        /// <summary>
        /// Zooming rate of canvas
        /// </summary>
        public float ZoomingRate
        {
            get { return m_zoomingRate; }
            set { m_zoomingRate = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mode">Mode of this handle (select, pan, etc.)</param>
        /// <param name="handler">PBasicInputEventHandler</param>
        public Handle(Mode mode, PBasicInputEventHandler handler)
        {
            this.m_mode = mode;
            this.m_handler = handler;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mode">Mode of this handle (select, pan, etc.)</param>
        /// <param name="handler">PBasicInputEventHandler</param>
        /// <param name="csID">ID of component setting</param>
        public Handle(Mode mode, PBasicInputEventHandler handler, ComponentType csID)
        {
            this.m_mode = mode;
            this.m_cType = csID;
            this.m_handler = handler;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mode">Mode of this handle (select, pan, etc.)</param>
        /// <param name="zoomingRate">Zooming rate</param>
        public Handle(Mode mode, float zoomingRate)
        {
            this.m_mode = mode;
            this.m_zoomingRate = zoomingRate;
        }
        #endregion
    }
}