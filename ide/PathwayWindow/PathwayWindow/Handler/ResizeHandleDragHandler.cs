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
using System.Drawing;
using System.Text;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.Handler;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Drag handler for resize handles of system.
    /// </summary>
    public class ResizeHandleDragHandler : PDragEventHandler
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ResizeHandleDragHandler()
        {
        }

        /// <summary>
        /// Called when dragging of a resize handle started.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartDrag(object sender, PInputEventArgs e)
        {
            base.OnStartDrag(sender, e);
            e.Handled = true;
            e.PickedNode.MoveToFront();
        }

        /// <summary>
        /// Called when resize handles are dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            base.OnDrag(sender, e);
            if (!(e.PickedNode is ResizeHandle))
                return;

            ResizeHandle handle = (ResizeHandle)e.PickedNode;
            SizeF size = e.GetDeltaRelativeTo(base.DraggedNode);
            size = base.DraggedNode.LocalToParent(size);

            if (handle.Restriction == MovingRestriction.Horizontal)
                base.DraggedNode.OffsetBy(size.Width, 0);
            else if (handle.Restriction == MovingRestriction.Vertical)
                base.DraggedNode.OffsetBy(0, size.Height);
            else
                base.DraggedNode.OffsetBy(size.Width, size.Height);
        }
    }
}
