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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Nodes;
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.ComponentModel;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Exceptions;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// EcventHandler when the node object is dragged.
    /// </summary>
    public class NodeDragHandler : PDragEventHandler
    {
        #region Fields
        /// <summary>
        /// the related CanvasControl.
        /// </summary>
        private CanvasControl m_canvas;
        #endregion

        /// <summary>
        /// constructor with initial parameters.
        /// </summary>
        /// <param name="canvas">canvas control.</param>
        public NodeDragHandler(CanvasControl canvas)
        {
            m_canvas = canvas;
        }

        /// <summary>
        /// check whether this event is accepted.
        /// </summary>
        /// <param name="e">event.</param>
        /// <returns>mouse event is true.</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.IsMouseEvent
               && e.Button == MouseButtons.Left;
        }

        /// <summary>
        /// event on start drag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartDrag(object sender, PInputEventArgs e)
        {
            base.OnStartDrag(sender, e);
        }

        /// <summary>
        /// event on drag PNode.
        /// </summary>
        /// <param name="sender">PathwayView.</param>
        /// <param name="e">PInputEventArgs.</param>
        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            base.OnDrag(sender, e);
            // Move objects
            PointF offset = e.PickedNode.Offset;
            if (offset.X == 0 && offset.Y == 0)
                return;

            m_canvas.MoveSelectedObjects(offset);
        }

        /// <summary>
        /// event on end to drag PNode in PathwayView.
        /// </summary>
        /// <param name="sender">Pathwayview.</param>
        /// <param name="e">PInputEventArgs.</param>
        protected override void OnEndDrag(object sender, PInputEventArgs e)
        {
            base.OnEndDrag(sender, e);
            m_canvas.NotifyMoveObjects();
        }
    }
}
