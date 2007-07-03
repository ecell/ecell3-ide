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
using EcellLib.PathwayWindow.Element;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Default layout algorithm.
    /// Nodes will be layouted from an upper-left corner to right.
    /// </summary>
    
    public class DefaultLayoutAlgorithm : ILayoutAlgorithm
    {
        /// <summary>
        /// Execute a layout.
        /// </summary>
        /// <param name="subCommandNum">An index of sub command which was clicked on subMenu.
        /// Sub command which is in subCommandNum position in the list returned by GetSubCommands() [0 origin]
        /// If layout name itself was clicked, subCommandNum = -1.
        /// </param>
        /// <param name="layoutSystem">Whether systems should be layouted or not</param>
        /// <param name="systemElements">Systems</param>
        /// <param name="nodeElements">Nodes (Variables, Processes)</param>
        /// <returns>Whether layout is completed or aborted</returns>
        public bool DoLayout(int subCommandNum, 
                             bool layoutSystem,
                             List<SystemElement> systemElements,
                             List<NodeElement> nodeElements)
        {
            Dictionary<string, SystemElement> sysDict = new Dictionary<string,SystemElement>();
            if(systemElements != null && nodeElements != null)
            {
                foreach (SystemElement sysEle in systemElements)
                {
                    sysDict.Add(sysEle.Key, sysEle);
                }
                foreach(NodeElement nodeEle in nodeElements)
                {
                    if (sysDict.ContainsKey(nodeEle.ParentSystemID))
                        sysDict[nodeEle.ParentSystemID].ChildNodeNum += 1;
                }
                foreach(SystemElement sysEle in systemElements)
                {
                    if (sysDict.ContainsKey(sysEle.ParentSystemID))
                        sysDict[sysEle.ParentSystemID].ChildSystemNum += 1;
                }
                Dictionary<string, SystemContainer> containerDict = new Dictionary<string, SystemContainer>();
                SystemContainer rootContainer = null;
                foreach (SystemElement sysEle in systemElements)
                {
                    SystemContainer container = new SystemContainer();
                    container.Self = sysEle;
                    if (sysEle.ChildNodeNum != 0)
                    {
                        SystemContainer childDummyContainer = new SystemContainer();
                        childDummyContainer.ChildNodeNum = sysEle.ChildNodeNum;
                        childDummyContainer.IsDummyContainer = true;
                        container.AddChildContainer(childDummyContainer);
                    }
                    if (sysEle.Key.Equals("/"))
                        rootContainer = container;
                    containerDict.Add(sysEle.Key,container);
                }
                foreach(SystemContainer sysCon in containerDict.Values)
                {
                    if (containerDict.ContainsKey(sysCon.Self.ParentSystemID))
                    {
                        containerDict[sysCon.Self.ParentSystemID].AddChildContainer(sysCon);
                    }
                }
                // Settle system coordinates
                rootContainer.ArrangeAllCoordinates(0, 0);

                // Settle node coordinates
                foreach (NodeElement nodeEle in nodeElements)
                {
                    PointF nodePoint = containerDict[nodeEle.ParentSystemID].RequestNodePosition();
                    nodeEle.X = nodePoint.X;
                    nodeEle.Y = nodePoint.Y;
                }
            }
            return true;
        }

        /// <summary>
        /// Get LayoutType of this layout algorithm.
        /// </summary>
        /// <returns></returns>
        public LayoutType GetLayoutType()
        {
            return LayoutType.Whole;
        }

        /// <summary>
        /// Get a name of this layout algorithm.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return "Default Layout";
        }

        /// <summary>
        /// Get a tooltip of this layout algorithm.
        /// </summary>
        /// <returns></returns>
        public string GetToolTipText()
        {
            return "Default Layout";
        }

        /// <summary>
        /// Get a list of name ofsub menus.
        /// </summary>
        /// <returns>a list of name of sub menus</returns>
        public List<string> GetSubCommands()
        {
            return new List<string>();
        }

        /// <summary>
        /// A class for containing all information for one system.
        /// </summary>
        class SystemContainer
        {
            #region Fields
            private float m_defaultRootSize = 300;
            private float m_x;
            private float m_y;
            private float m_width;
            private float m_height;

            private float m_outlineWidth = 10;
            private float m_margin = 40;
            private float m_squarePerNode = 9600;
            private bool m_isDummyContainer = false;
            private int m_childNodeNum;
            private SystemElement m_self;
            private List<SystemContainer> m_childSystems;

            private int m_pointNumSqrt = 0;
            private int m_consumedPointNum = 0;
            #endregion

            #region Contructor
            public SystemContainer()
            {
                m_childSystems = new List<SystemContainer>();
            }
            public SystemContainer(float outlineWidth, float margin)
            {
                m_childSystems = new List<SystemContainer>();
                m_outlineWidth = outlineWidth;
                m_margin = margin;
            }
            #endregion

            #region Accessors
            public int ChildNodeNum
            {
                get { return m_childNodeNum; }
                set
                {
                    m_childNodeNum = value;
                    m_pointNumSqrt = 0;
                    m_consumedPointNum = 0;
                    do{
                        m_pointNumSqrt++;
                    }while(m_pointNumSqrt * m_pointNumSqrt < m_childNodeNum);
                }
            }
            public bool IsDummyContainer
            {
                get { return m_isDummyContainer; }
                set { m_isDummyContainer = value; }
            }
            public SystemElement Self
            {
                get { return m_self; }
                set
                {
                    m_self = value;
                    m_x = value.X;
                    m_y = value.Y;
                    m_width = value.Width;
                    m_height = value.Height;
                }
            }
            public List<SystemContainer> ChildSystems
            {
                get { return m_childSystems; }
                set { m_childSystems = value; }
            }
            public float X
            {
                get { return m_x; }
                set
                {
                    m_x = value;
                    if (m_self != null)
                        m_self.X = value;
                }
            }
            public float Y
            {
                get { return m_y; }
                set
                {
                    m_y = value;
                    if (m_self != null)
                        m_self.Y = value;
                }
            }
            public float Width
            {
                get { return m_width; }
                set
                {
                    m_width = value;
                    if (m_self != null)
                        m_self.Width = value;
                }
            }
            public float Height
            {
                get { return m_height; }
                set
                {
                    m_height = value;
                    if (m_self != null)
                        m_self.Height = value;
                }
            }
            #endregion

            #region Methods
            public void AddChildContainer(SystemContainer childCon)
            {
                m_childSystems.Add(childCon);
            }
            
            public void ArrangeAllCoordinates(float x, float y)
            {
                this.ArrangeRegion();
                this.SettleCoordinates(x, y);
            }

            public void ArrangeRegion()
            {
                float width = 2 * m_outlineWidth;
                float height = 2 * m_outlineWidth;
                if (m_self.Key.Equals("/"))
                {
                    width = m_defaultRootSize;
                    height = m_defaultRootSize;
                }
                if (m_childSystems.Count != 0)
                {
                    float maxHeight = 0;
                    foreach (SystemContainer container in m_childSystems)
                    {
                        container.ArrangeRegion();
                        width += m_margin;
                        width += container.Width;
                        if (container.Height > maxHeight)
                        {
                            maxHeight = container.Height;
                        }
                    }
                    height += maxHeight + m_margin;
                }
                else
                {
                    float length = (float)Math.Sqrt(m_childNodeNum * m_squarePerNode);
                    width += length;
                    height += length;
                }
                width += m_margin;
                height += m_margin;
                this.Width = width;
                this.Height = height;
            }
            public void CancelAllNodePosition()
            {
                m_consumedPointNum = 0;
            }
            public PointF RequestNodePosition()
            {
                if(!m_isDummyContainer)
                {
                    foreach(SystemContainer sysCont in m_childSystems)
                    {
                        if(sysCont.IsDummyContainer)
                        {
                            return sysCont.RequestNodePosition();
                        }
                    }
                    return new PointF();
                }
                int newPointNum = m_consumedPointNum + 1;
                int gridX = 0;
                int gridY = 0;
                for (int i = 0; i < m_pointNumSqrt; i++ )
                {
                    gridY++;
                    if (newPointNum <= m_pointNumSqrt)
                    {
                        gridX = newPointNum;
                        break;
                    }
                    else
                    {
                        newPointNum -= m_pointNumSqrt;
                    }                    
                }
                m_consumedPointNum++;
                if(m_consumedPointNum >= m_pointNumSqrt * m_pointNumSqrt)
                {
                    m_consumedPointNum = 0;
                }

                PointF returnPoint = new Point();

                returnPoint.X = this.m_x + gridX * (this.m_width / (m_pointNumSqrt + 1));
                returnPoint.Y = this.m_y + gridY * (this.m_height / (m_pointNumSqrt + 1));

                return returnPoint;
            }
            public void SettleCoordinates(float x, float y)
            {
                this.X = x;
                this.Y = y;
                if(m_childSystems.Count == 0)
                    return;
                x += m_outlineWidth + m_margin;
                y += m_outlineWidth + m_margin;
                foreach(SystemContainer container in m_childSystems)
                {
                    container.SettleCoordinates(x,y);
                    x += container.Width + m_margin;
                }
            }
            #endregion
        }
    }
}