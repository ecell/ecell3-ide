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
using System.Drawing;
using System.ComponentModel;
using EcellLib.Layout;
using EcellLib.Objects;

namespace EcellLib.DistributeLayout
{
    /// <summary>
    /// Layout algorithm to distribute nodes evenly spaced
    /// </summary>
    public partial class DistributeLayout : LayoutBase, ILayoutAlgorithm
    {
        enum Direction { Horizontally, Vertically }

        public DistributeLayout()
        {
        }

        /// <summary>
        /// Execute layout
        /// </summary>
        /// <param name="subNum">
        /// An index of sub command which was clicked on subMenu.
        /// Sub command which is in subCommandNum position in the list returned by GetSubCommands() [0 origin]
        /// If layout name itself was clicked, subCommandNum = -1.
        /// </param>
        /// <param name="layoutSystem">Whether systems should be layouted or not</param>
        /// <param name="systemList">Systems</param>
        /// <param name="nodeList">Nodes (Variables, Processes)</param>
        /// <returns>Whether layout is completed or aborted</returns>
        public bool DoLayout(int subNum,
                             bool layoutSystem,
                             List<EcellObject> systemList,
                             List<EcellObject> nodeList)
        {
            if (nodeList == null || nodeList.Count <= 1)
                return false;
            nodeList = GetSelectedObject(nodeList);
            if (nodeList.Count <= 2)
                return false;

            RectangleF rect = GetSurroundingRect(nodeList);

            Direction dir = GetDirection(subNum);

            SortedDictionary<float, List<EcellObject>> posDict = new SortedDictionary<float, List<EcellObject>>();
            foreach (EcellObject node in nodeList)
                this.AddIntoDictionary(posDict, (Direction.Horizontally == dir) ? node.X : node.Y, node);

            float increment = (Direction.Horizontally == dir) ? rect.Width / (float)(nodeList.Count - 1) : rect.Height / (float)(nodeList.Count - 1);
            float count = 0;

            foreach (KeyValuePair<float, List<EcellObject>> pair in posDict)
            {
                foreach (EcellObject node in pair.Value)
                {
                    switch (dir)
                    {
                        case Direction.Horizontally:
                            node.X = rect.X + increment * count;
                            break;
                        case Direction.Vertically:
                            node.Y = rect.Y + increment * count;
                            break;
                    }
                    count = count + 1;
                }
            }

            return true;
        }

        /// <summary>
        /// Get Direction.
        /// </summary>
        private Direction GetDirection(int subNum)
        {
            switch (subNum)
            {
                case 0:
                default:
                    return Direction.Horizontally;
                case 1:
                    return Direction.Vertically;
            }
        }

        private void AddIntoDictionary(SortedDictionary<float, List<EcellObject>> dict,
                                       float posValue,
                                       EcellObject node)
        {
            if (dict == null)
                return;
            if (dict.ContainsKey(posValue))
                dict[posValue].Add(node);
            else
            {
                List<EcellObject> newDict = new List<EcellObject>();
                newDict.Add(node);
                dict.Add(posValue, newDict);
            }
        }

        /// <summary>
        /// Get LayoutType of this layout algorithm.
        /// </summary>
        /// <returns>LayoutType of this layout algorithm</returns>
        public LayoutType GetLayoutType()
        {
            return LayoutType.Selected;
        }

        /// <summary>
        /// Get the menu text of this layout.
        /// </summary>
        /// <returns></returns>
        public string GetMenuText()
        {
            return MessageResDistributeLayout.MenuItemDistribute;
        }

        /// <summary>
        /// Get a name of this layout algorithm.
        /// </summary>
        /// <returns>a name of this layout algorithm</returns>
        public string GetName()
        {
            return "Distribute";
        }

        /// <summary>
        /// Get a tooltip of this layout algorithm.
        /// </summary>
        /// <returns>a tooltip of this layout</returns>
        public string GetToolTipText()
        {
            return MessageResDistributeLayout.ToolTip;
        }

        /// <summary>
        /// Get a list of name for sub menus.
        /// </summary>
        /// <returns>a list of name</returns>
        public List<string> GetSubCommands()
        {
            List<string> subCommands = new List<string>();
            subCommands.Add(MessageResDistributeLayout.MenuItemSubHorizontally);
            subCommands.Add(MessageResDistributeLayout.MenuItemSubVertically);

            return subCommands;
        }
    }
}
