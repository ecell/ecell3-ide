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
using System.ComponentModel;
using System.Drawing;
using EcellLib.Layout;

namespace EcellLib.AlignLayout
{
    /// <summary>
    /// Layout algorithm to align nodes
    /// </summary>
    public class AlignLayout : LayoutBase, ILayoutAlgorithm
    {
        enum Alignment {
            /// <summary>
            /// Align vertically at left side.
            /// </summary>
            Left,
            /// <summary>
            /// Align vertically at right side.
            /// </summary>
            Right,
            /// <summary>
            /// Align horizontally at upper side.
            /// </summary>
            Upper,
            /// <summary>
            /// Align horizontally at lower side.
            /// </summary>
            Lower
        }
        /// <summary>
        /// Execute layout
        /// </summary>
        private ComponentResourceManager m_crm = new ComponentResourceManager(typeof(AlignLayout));

        /// <summary>
        /// Execute layout
        /// </summary>
        /// <param name="subNum">
        /// An index of sub command which was clicked on subMenu.
        /// Sub command which is in subCommandNum position in the list returned by GetSubCommands() [0 origin]
        /// If layout name itself was clicked, subCommandNum = -1.
        /// </param>
        /// <param name="layoutSystem">Whether systems should be layouted or not</param>
        /// <param name="systemList">Systems (can null)</param>
        /// <param name="nodeList">Nodes (Variables, Processes)</param>
        /// <returns>Whether layout is completed or aborted</returns>
        public bool DoLayout(int subNum,
                             bool layoutSystem,
                             List<EcellObject> systemList,
                             List<EcellObject> nodeList)
        {
            // Error check.
            if (nodeList == null || nodeList.Count <= 1)
                return false;
            nodeList = GetSelectedObject(nodeList);
            RectangleF rect = GetSurroundingRect(nodeList);

            Alignment align = GetAlignment(subNum);
            float alignValue = GetAlignValue(rect, align); // Set all nodes X or Y to this position

            // Set x or y coordinate to alignValue
            foreach (EcellObject node in nodeList)
            {
                switch (align)
                {
                    case Alignment.Left:
                    case Alignment.Right:
                        node.X = alignValue;
                        break;
                    case Alignment.Upper:
                    case Alignment.Lower:
                        node.Y = alignValue;
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Get LayoutType of this layout
        /// </summary>
        /// <returns>LayoutType of this layout</returns>
        public LayoutType GetLayoutType()
        {
            return LayoutType.Selected;
        }

        /// <summary>
        /// Get menu name of this algorithm
        /// </summary>
        /// <returns>menu name of this algorithm</returns>
        public string GetMenuText()
        {
            return m_crm.GetString("MenuItemAlign");
        }

        /// <summary>
        /// Get a name of this layout
        /// </summary>
        /// <returns>a name of this layout</returns>
        public string GetName()
        {
            return "Align";
        }

        /// <summary>
        /// Get a tooltip of this layout
        /// </summary>
        /// <returns>tooltip</returns>
        public string GetToolTipText()
        {
            return m_crm.GetString("ToolTip");
        }

        /// <summary>
        /// Get a list of names for submenu.
        /// </summary>
        /// <returns>a list of name of sub commands</returns>
        public List<string> GetSubCommands()
        {
            List<string> subCommands = new List<string>();
            subCommands.Add(m_crm.GetString("MenuItemSubLeft"));
            subCommands.Add(m_crm.GetString("MenuItemSubRight"));
            subCommands.Add(m_crm.GetString("MenuItemSubUpper"));
            subCommands.Add(m_crm.GetString("MenuItemSubLower"));

            return subCommands;
        }

        #region Private Methods
        /// <summary>
        /// Get aligned value.
        /// </summary>
        /// <param name="rect">List</param>
        /// <param name="align">Alignment</param>
        /// <returns>float</returns>
        private float GetAlignValue(RectangleF rect, Alignment align)
        {
            switch (align)
            {
                case Alignment.Left:
                default:
                    return rect.X;
                case Alignment.Right:
                    return rect.X + rect.Width;
                case Alignment.Upper:
                    return rect.Y;
                case Alignment.Lower:
                    return rect.Y + rect.Height;
            }
        }

        /// <summary>
        /// Get Alignment direction.
        /// </summary>
        /// <param name="subNum">Number of Layout</param>
        /// <returns>Alignment</returns>
        private Alignment GetAlignment(int subNum)
        {
            switch (subNum)
            {
                case 0:default:
                    return Alignment.Left;
                case 1:
                    return Alignment.Right;
                case 2:
                    return Alignment.Upper;
                case 3:
                    return Alignment.Lower;
            }
        }
        #endregion
    }
}
