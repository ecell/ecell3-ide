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
using UMD.HCIL.Piccolo.Event;
using System.IO;
using EcellLib.PathwayWindow.Resources;
using System.ComponentModel;
using System.Windows.Forms;
using EcellLib.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo;

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// PPathwayPanEventHandler
    /// </summary>
    class PPathwayPanEventHandler : PPanEventHandler, IPathwayEventHandler
    {
        /// <summary>
        /// PathwayControl
        /// </summary>
        protected PathwayControl m_con;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        protected ComponentResourceManager m_resources;
        
        #region Constructors
        /// <summary>
        /// Constructor with PathwayView.
        /// </summary>
        protected PPathwayPanEventHandler()
        {
        }
        /// <summary>
        /// Constructor with PathwayView.
        /// </summary>
        /// <param name="control">The control of PathwayView.</param>
        public PPathwayPanEventHandler(PathwayControl control)
        {
            this.m_con = control;
            this.m_resources = control.Resources;
        }
        #endregion

        #region IPathwayEventHandler
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            m_con.Canvas.PathwayCanvas.Cursor = new Cursor(new MemoryStream(PathwayResource.move));
            m_con.Canvas.ResetSelectedObjects();
            PCanvas canvas = m_con.Canvas.PathwayCanvas;
            canvas.Camera.Pickable = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            m_con.Canvas.PathwayCanvas.Cursor = Cursors.Arrow;
            PCanvas canvas = m_con.Canvas.PathwayCanvas;
            canvas.Camera.Pickable = true;
        }
        #endregion
    }
}
