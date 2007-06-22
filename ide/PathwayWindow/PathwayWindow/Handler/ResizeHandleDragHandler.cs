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
using System.Drawing;
using System.Text;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib.PathwayWindow.Node;
using PathwayWindow.UIComponent;

namespace EcellLib.PathwayWindow
{
    public class ResizeHandleDragHandler : PDragEventHandler
    {
        #region Fields
        public enum MovingRestriction {Vertical, Horizontal, NoRestriction}

        private MovingRestriction m_restrict;

        private CanvasView m_set;
        private List<PEcellSystem> m_systemList;
        #endregion

        #region Accessors
        public MovingRestriction Restrict
        {
            get { return this.m_restrict; }
            set { this.m_restrict = value; }
        }
        #endregion

        public ResizeHandleDragHandler(CanvasView set)
        {
            m_set = set;
        }

        protected override void OnStartDrag(object sender, PInputEventArgs e)
        {
            base.OnStartDrag(sender, e);
            if (e.PickedNode is PEcellSystem)
                m_systemList = m_set.GetSystem(((PEcellSystem)e.PickedNode).Name);
            e.Handled = true;
            if (!(e.PickedNode is PEcellComposite) ||
                e.PickedNode.ChildrenCount != 1 ||
                !(e.PickedNode.ChildrenReference[0] is PSystem))
                e.PickedNode.MoveToFront();
        }

        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            if (e.PickedNode is ResizeHandle)
            {
                SizeF s = e.GetDeltaRelativeTo(base.DraggedNode);
                s = base.DraggedNode.LocalToParent(s);

                ResizeHandleDragHandler.MovingRestriction restrict = (ResizeHandleDragHandler.MovingRestriction)((PPath)e.PickedNode).Tag;

                if (restrict == ResizeHandleDragHandler.MovingRestriction.Horizontal)
                    base.DraggedNode.OffsetBy(s.Width, 0);
                else if (restrict == ResizeHandleDragHandler.MovingRestriction.Vertical)
                    base.DraggedNode.OffsetBy(0, s.Height);
                else
                    base.DraggedNode.OffsetBy(s.Width, s.Height);
            }
            else
            {
                base.OnDrag(sender, e);
            }
        }
    }
}
