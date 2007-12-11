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
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Event;

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// Resize handle for resizing system size
    /// </summary>
    public class ResizeHandle : PPath
    {
        /// <summary>
        /// Whether this node is able to move plus x direction or not.
        /// </summary>
        private bool m_ableToMoveXPlus = true;
        /// <summary>
        /// Whether this node is able to move minus x direction or not.
        /// </summary>
        private bool m_ableToMoveXMinus = true;
        /// <summary>
        /// Whether this node is able to move plus y direction or not.
        /// </summary>
        private bool m_ableToMoveYPlus = true;
        /// <summary>
        /// Whether this node is able to move minus y direction or not.
        /// </summary>
        private bool m_ableToMoveYMinus = true;

        private bool m_isMoved = false;

        private float m_prevX = 0;

        private float m_prevY = 0;
        /// <summary>
        /// MovingRestriction
        /// </summary>
        private MovingRestriction m_restriction = MovingRestriction.NoRestriction;

        #region Accessor
        /// <summary>
        /// MovingRestriction
        /// </summary>
        public MovingRestriction Restriction
        {
            get { return m_restriction; }
            set { m_restriction = value; }
        }
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        public ResizeHandle()
        {
            this.AddInputEventListener(new ResizeHandleDragHandler());
        }

        /// <summary>
        /// Free restriction of move of this node
        /// </summary>
        public void FreeMoveRestriction()
        {
            m_ableToMoveXPlus = true;
            m_ableToMoveXMinus = true;
            m_ableToMoveYPlus = true;
            m_ableToMoveYMinus = true;
        }

        /// <summary>
        /// Prohibit this node to move in plus x direction.
        /// </summary>
        public void ProhibitMovingToXPlus()
        {
            m_ableToMoveXPlus = false;
        }

        /// <summary>
        /// Prohibit this node to move in minus x direction.
        /// </summary>
        public void ProhibitMovingToXMinus()
        {
            m_ableToMoveXMinus = false;
        }

        /// <summary>
        /// Prohibit this node to move in plus y direction.
        /// </summary>
        public void ProhibitMovingToYPlus()
        {
            m_ableToMoveYPlus = false;
        }

        /// <summary>
        /// Prohibit this node to move in minus y direction.
        /// </summary>
        public void ProhibitMovingToYMinus()
        {
            m_ableToMoveYMinus = false;
        }

        ///// <summary>
        ///// Override SettOffset.
        ///// Restriction of moving was inplemented.
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public override void SetOffset(float x, float y)
        //{
        //    if (m_isMoved)
        //    {
        //        float deltaX = x - m_prevX;
        //        float deltaY = y - m_prevY;
        //        if ((0 < deltaX && !m_ableToMoveXPlus) || (deltaX < 0 && !m_ableToMoveXMinus))
        //            x = m_prevX;
        //        if ((0 < deltaY && !m_ableToMoveYPlus) || (deltaY < 0 && !m_ableToMoveYMinus))
        //            y = m_prevY;
        //    }
        //    else
        //        m_isMoved = true;
            
        //    base.SetOffset(x, y);

        //    m_prevX = x;
        //    m_prevY = y;
        //}
    }
}
