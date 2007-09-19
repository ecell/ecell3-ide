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

namespace EcellLib.DistributeLayout
{
    /// <summary>
    /// Layout algorithm to distribute nodes evenly spaced
    /// </summary>
    public class DistributeLayout : ILayoutAlgorithm
    {
        enum Direction { Horizontally, Vertically }

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
                             List<EcellObject> systemList,
                             List<EcellObject> nodeList)
        {
        }
        /// <summary>
        /// Execute layout.
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

            foreach (NodeElement element in nodeElements)
                if (!element.Fixed)
                    newNodeElements.Add(element);

            nodeElements = newNodeElements;

            if (nodeElements.Count <= 2)
                return false;

            Direction dir;
            switch(subNum)
            {
                case 0:
                    dir = Direction.Horizontally;
                    break;
                case 1:
                    dir = Direction.Vertically;
                    break;
                default:
                    dir = Direction.Horizontally;
                    break;
            }

            bool isFirst = true;
            float min = 0;
            float max = 0;
            SortedDictionary<float, List<NodeElement>> posDict = new SortedDictionary<float, List<NodeElement>>();

            foreach(NodeElement nodeEle in nodeElements)
            {
                if (isFirst)
                {
                    switch (dir)
                    {
                        case Direction.Horizontally:
                            min = nodeEle.X;
                            max = nodeEle.X;
                            this.AddIntoDictionary(posDict, nodeEle.X, nodeEle);
                            break;
                        case Direction.Vertically:
                            min = nodeEle.Y;
                            max = nodeEle.Y;
                            this.AddIntoDictionary(posDict, nodeEle.Y, nodeEle);
                            break;
                    }

                    isFirst = false;
                }
                else
                {
                    switch(dir)
                    {
                        case Direction.Horizontally:
                            if (nodeEle.X < min)
                                min = nodeEle.X;
                            else if (max < nodeEle.X)
                                max = nodeEle.X;
                            this.AddIntoDictionary(posDict, nodeEle.X, nodeEle);
                            break;
                        case Direction.Vertically:
                            if (nodeEle.Y < min)
                                min = nodeEle.Y;
                            else if (max < nodeEle.Y)
                                max = nodeEle.Y;
                            this.AddIntoDictionary(posDict, nodeEle.Y, nodeEle);
                            break;
                    }
                }
            }

            float increment = (max - min) / ((float)nodeElements.Count - 1);

            float count = 0;

            foreach(KeyValuePair<float, List<NodeElement>> pair in posDict)
            {
                foreach(NodeElement node in pair.Value)
                {
                    switch(dir)
                    {
                        case Direction.Horizontally:
                            node.X = min + increment * count;
                            break;
                        case Direction.Vertically:
                            node.Y = min + increment * count;
                            break;
                    }
                    count = count + 1;
                }
            }

            return true;
        }

        private void AddIntoDictionary(SortedDictionary<float, List<NodeElement>> dict,
                                       float posValue,
                                       NodeElement nodeEle)
        {
            if(dict == null)
                return;
            if (dict.ContainsKey(posValue))
                dict[posValue].Add(nodeEle);
            else
            {
                List<NodeElement> newDict = new List<NodeElement>();
                newDict.Add(nodeEle);
                dict.Add(posValue,newDict);
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

        public string GetMenuText()
        {
            ComponentResourceManager crm = new ComponentResourceManager(typeof(DistributeLayout));
            return crm.GetString("MenuItemDistribute");
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
            ComponentResourceManager crm = new ComponentResourceManager(typeof(DistributeLayout));
            return crm.GetString("ToolTip");
        }

        /// <summary>
        /// Get a list of name for sub menus.
        /// </summary>
        /// <returns>a list of name</returns>
        public List<string> GetSubCommands()
        {
            ComponentResourceManager crm = new ComponentResourceManager(typeof(DistributeLayout));
            List<string> subCommands = new List<string>();
            subCommands.Add(crm.GetString("MenuItemSubHorizontally"));
            subCommands.Add(crm.GetString("MenuItemSubVertically"));

            return subCommands;
        }
    }
}
