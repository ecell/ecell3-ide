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
using EcellLib.PathwayWindow;
using EcellLib.PathwayWindow.Element;
using System.ComponentModel;

namespace EcellLib.AlignLayout
{
    /// <summary>
    /// Layout algorithm to align nodes
    /// </summary>
    public class AlignLayout : ILayoutAlgorithm
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
            Lower }

        /// <summary>
        /// Execute layout
        /// </summary>
        /// <param name="subNum">
        /// An index of sub command which was clicked on subMenu.
        /// Sub command which is in subCommandNum position in the list returned by GetSubCommands() [0 origin]
        /// If layout name itself was clicked, subCommandNum = -1.
        /// </param>
        /// <param name="layoutSystem">Whether systems should be layouted or not</param>
        /// <param name="systemElements">Systems</param>
        /// <param name="nodeElements">Nodes (Variables, Processes)</param>
        /// <returns>Whether layout is completed or aborted</returns>
        public bool DoLayout(int subNum,
                             bool layoutSystem,
                             List<SystemElement> systemElements,
                             List<NodeElement> nodeElements)
        {
            List<NodeElement> newNodeElements = new List<NodeElement>();

            foreach(NodeElement element in nodeElements)
                if (!element.Fixed)
                    newNodeElements.Add(element);

            nodeElements = newNodeElements;

            if(nodeElements.Count <= 1)
                return false;

            Alignment align;
            switch (subNum)
            {
                case 0:
                    align = Alignment.Left;
                    break;
                case 1:
                    align = Alignment.Right;
                    break;
                case 2:
                    align = Alignment.Upper;
                    break;
                case 3:
                    align = Alignment.Lower;
                    break;
                default:
                    align = Alignment.Left;
                    break;
            }

            bool isFirst = true;
            float alignValue = 0; // Set all nodes X or Y to this position

            // Settle alignValue
            foreach(NodeElement node in nodeElements)
            {
                if(isFirst)
                {
                    switch (align)
                    {
                        case Alignment.Left:
                            alignValue = node.X;
                            break;
                        case Alignment.Right:
                            alignValue = node.X;
                            break;
                        case Alignment.Upper:
                            alignValue = node.Y;
                            break;
                        case Alignment.Lower:
                            alignValue = node.Y;
                            break;
                    }
                    isFirst = false;
                }
                else
                {
                    switch (align)
                    {
                        case Alignment.Left:
                            if (node.X < alignValue)
                                alignValue = node.X;
                            break;
                        case Alignment.Right:
                            if (alignValue < node.X)
                                alignValue = node.X;
                            break;
                        case Alignment.Upper:
                            if (node.Y < alignValue)
                                alignValue = node.Y;
                            break;
                        case Alignment.Lower:
                            if (alignValue < node.Y)
                                alignValue = node.Y;
                            break;
                    }
                }
            }

            // Set x or y coordinate to alignValue
            foreach(NodeElement node in nodeElements)
            {
                switch(align)
                {
                    case Alignment.Left:
                        node.X = alignValue;
                        break;
                    case Alignment.Right:
                        node.X = alignValue;
                        break;
                    case Alignment.Upper:
                        node.Y = alignValue;
                        break;
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
            ComponentResourceManager crm = new ComponentResourceManager(typeof(AlignLayout));
            return crm.GetString("MenuItemAlign");
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
            ComponentResourceManager crm = new ComponentResourceManager(typeof(AlignLayout));
            return crm.GetString("ToolTip");
        }

        /// <summary>
        /// Get a list of names for submenu.
        /// </summary>
        /// <returns>a list of name of sub commands</returns>
        public List<string> GetSubCommands()
        {
            ComponentResourceManager crm = new ComponentResourceManager(typeof(AlignLayout));
            List<string> subCommands = new List<string>();
            subCommands.Add(crm.GetString("MenuItemSubLeft"));
            subCommands.Add(crm.GetString("MenuItemSubRight"));
            subCommands.Add(crm.GetString("MenuItemSubUpper"));
            subCommands.Add(crm.GetString("MenuItemSubLower"));

            return subCommands;
        }
    }
}
