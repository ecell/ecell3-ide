using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Nodes;

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

        /// <summary>
        /// Override SettOffset.
        /// Restriction of moving was inplemented.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void SetOffset(float x, float y)
        {
            if (m_isMoved)
            {
                float deltaX = x - m_prevX;
                float deltaY = y - m_prevY;
                if ((0 < deltaX && !m_ableToMoveXPlus) || (deltaX < 0 && !m_ableToMoveXMinus))
                    x = m_prevX;
                if ((0 < deltaY && !m_ableToMoveYPlus) || (deltaY < 0 && !m_ableToMoveYMinus))
                    y = m_prevY;
            }
            else
                m_isMoved = true;
            
            base.SetOffset(x, y);

            m_prevX = x;
            m_prevY = y;
        }
    }
}
