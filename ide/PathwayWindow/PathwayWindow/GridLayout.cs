using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using EcellLib.PathwayWindow;
using EcellLib.PathwayWindow.Element;
using System.Windows.Forms;
using System.ComponentModel;
using PathwayWindow;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Layout algorithm to layout nodes on grid.
    /// </summary>
    public class GridLayout : ILayoutAlgorithm
    {
        #region fields
        /// <summary>
        ///             |         |
        ///   LeftUpper |  Upper  |   RightUpper
        ///             |         |
        /// -----------------------------------
        ///             |         |
        ///   Left      |  *Node* |   Right
        ///             |         |
        /// ------------------------------------
        ///             |         |
        ///   LeftLower |  Lower  |   RightLower
        ///             |         |
        /// </summary>
        private enum Direction
        {
            LeftUpper,
            Upper,
            RightUpper,
            Right,
            RightLower,
            Lower,
            LeftLower,
            Left,
            None
        };

        /// <summary>
        /// natural length between connected nodes
        /// </summary>
        private static float m_naturalLength = 4;

        /// <summary>
        /// margin for a system. a node can't be put within this length from system's bound
        /// </summary>
        private static float m_defSystemMargin = 70;

        /// <summary>
        /// initial temperature (for simulated annealing)
        /// See http://en.wikipedia.org/wiki/Simulated_annealing for detail.
        /// </summary>
        private static float m_initialT = 60;

        /// <summary>
        /// At most, a step of simulated annealing repeats this time.
        /// See http://en.wikipedia.org/wiki/Simulated_annealing for detail.
        /// </summary>
        private static int m_kmax = 250;

        private static float m_defGridDistance = 60;
        #endregion

        #region inherited from ILayoutAlgorithm
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
            // Prepare the progress bar
            ProgressDialog form = new ProgressDialog();
            form.Bar.Minimum = 1;
            form.Bar.Maximum = m_kmax;
            form.Bar.Step = 1;
            form.Bar.Value = 1;
            form.Show();

            // At first, all systems layout will be settled
            if (layoutSystem)
                DoSystemLayout(systemElements, nodeElements);

            // Next, nodes within one system will be layouted, for each systems.
            foreach (SystemElement sysEle in systemElements)
            {
                List<NodeElement> nodesOfTheSystem = new List<NodeElement>();
                foreach (NodeElement nodeEle in nodeElements)
                {
                    if (!string.IsNullOrEmpty(sysEle.Key)
                        && !string.IsNullOrEmpty(nodeEle.ParentSystemID)
                        && sysEle.Key.Equals(nodeEle.ParentSystemID))
                    {
                        nodesOfTheSystem.Add(nodeEle);
                    }
                }

                List<SystemElement> childSystems = new List<SystemElement>();
                foreach (SystemElement eachSys in systemElements)
                {
                    if (eachSys.ParentSystemID.Equals(sysEle.Key))
                        childSystems.Add(eachSys);
                }

                if (nodesOfTheSystem.Count > 1)
                    DoNodeLayout(sysEle, childSystems, nodesOfTheSystem, form);
            }
            form.Dispose();

            return true;
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
        /// <param name="systemElements">Systems</param>
        /// <param name="nodeElements">Nodes (Variables, Processes)</param>
        /// <returns>Whether layout is completed or aborted</returns>
        public bool DoLayout(int subNum,
                             bool layoutSystem,
                             List<EcellObject> systemList,
                             List<EcellObject> nodeList)
        {
            return false;
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
        /// Get menu name of this algorithm
        /// </summary>
        /// <returns>menu name of this algorithm</returns>
        public string GetMenuText()
        {
            ComponentResourceManager crm = new ComponentResourceManager(typeof(GridLayout));
            return crm.GetString("MenuItemGrid");
        }

        /// <summary>
        /// Get a name of this layout algorithm.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return "Grid";
        }

        /// <summary>
        /// Get a tooltip of this layout algorithm.
        /// </summary>
        /// <returns></returns>
        public string GetToolTipText()
        {
            ComponentResourceManager crm = new ComponentResourceManager(typeof(GridLayout));
            return crm.GetString("ToolTip");
        }

        /// <summary>
        /// Get a list of name of sub menus.
        /// </summary>
        /// <returns>a list of name of sub menus</returns>
        public List<string> GetSubCommands()
        {
            return null;
        }
        #endregion

        #region methods for internal use
        /// <summary>
        /// Formally, an energy between two nodes will be calculated like below.
        ///   1       K
        ///  --- * ------- * (d - m_naturalLength)^2 
        ///   2     Hop^2
        /// 
        /// But K / 2 is a constant, so I use the following energy formula
        /// ( (d - m_naturalLength) / Hop )^2
        /// </summary>
        /// <param name="relationMatrix"></param>
        ///     This matrix contains node relation.
        ///     If relationMatrix[1,3] is true, nodeElements[1] and nodeElements[3] are connected in a pathway. 
        /// <param name="nodeIndex">A node of nodeElements at nodeIndex position, will be moved</param>
        /// <param name="nodeElements">An array of all nodes</param>
        /// <returns></returns>
        private float CalculateEnergy(int[,] relationMatrix, int nodeIndex, List<NodeElement> nodeElements)
        {
            NodeElement targetNode = nodeElements[nodeIndex];

            float energy = 0;

            int count = 0;

            foreach (NodeElement nodeEle in nodeElements)
            {
                float hop = (float)relationMatrix[nodeIndex, count];
                if (hop > 0)
                {
                    float d = (float)Math.Sqrt(Math.Pow(nodeEle.X - targetNode.X, 2) + Math.Pow(nodeEle.Y - targetNode.Y, 2));
                    energy += (float)Math.Pow(Math.Abs((d - m_naturalLength * Math.Sqrt(hop)) / hop), 2);
                }

                count++;
            }
            return energy;
        }

        /// <summary>
        /// A relation matrix contains node relations on the pathway.
        /// relationMatrix[i,j] = 3 means that a shortest path length between node i and node j is 3.
        /// In other words, there are three edges between node i and node j at shortest.
        /// If two nodes have totally no relation (if start from one node, never arrive another node), relationMatrix[i,j] = -1
        /// relationMatrix[i,i] = 0 means that length between a node and the same node is zero, off course
        /// </summary>
        /// <param name="nodeElements">relation of these nodes will be checked</param>
        /// <returns>relationMatrix, described above</returns>
        private int[,] CreateRelationMatrix(List<NodeElement> nodeElements)
        {
            int[,] relationMatrix = new int[nodeElements.Count, nodeElements.Count];

            // Initialize matrix
            for (int i = 0; i < nodeElements.Count; i++)
                for (int j = 0; j < nodeElements.Count; j++)
                {
                    if (i == j)
                        relationMatrix[i, j] = 0;
                    else
                        relationMatrix[i, j] = -1;
                }

            // that's all for this case
            if (nodeElements.Count <= 1)
                return relationMatrix;

            // using nodeElements, let lengths of neighboring nodes be 1
            Dictionary<string, int> keyDict = new Dictionary<string, int>();
            int num = 0;
            foreach (NodeElement nodeEle in nodeElements)
            {
                if (nodeEle is VariableElement)
                    keyDict.Add(nodeEle.Key, num);
                num++;
            }
            num = 0;

            foreach (NodeElement nodeEle in nodeElements)
            {
                if (nodeEle is ProcessElement)
                {
                    foreach (string key in ((ProcessElement)nodeEle).Edges.Keys)
                    {
                        if (string.IsNullOrEmpty(key))
                            continue;

                        try
                        {
                            int pos = keyDict[key];
                            relationMatrix[num, pos] = 1;
                            relationMatrix[pos, num] = 1;
                        }
                        catch (KeyNotFoundException) { }
                    }
                }
                num++;
            }

            for (int i = 0; i < nodeElements.Count; i++)
            {
                bool toBeDone = false;
                bool[] already = new bool[nodeElements.Count];
                for (int j = 0; j < nodeElements.Count; j++)
                {
                    already[j] = false;

                    if (relationMatrix[i, j] == -1)
                        toBeDone = true;
                    else if (relationMatrix[i, j] == 0)
                        already[j] = true;
                }
                if (!toBeDone)
                    continue;

                List<int> presentNodes = new List<int>();
                presentNodes.Add(i);

                int hopNum = 0;

                do
                {
                    hopNum++;
                    List<int> newNodes = new List<int>();

                    while (presentNodes.Count != 0)
                    {
                        int current = presentNodes[0];
                        presentNodes.Remove(current);

                        for (int j = 0; j < nodeElements.Count; j++)
                        {
                            if (relationMatrix[current, j] == 1 && already[j] == false)
                            {
                                newNodes.Add(j);
                                already[j] = true;
                            }
                        }
                    }

                    foreach (int newone in newNodes)
                    {
                        if (relationMatrix[i, newone] != -1)
                            continue;
                        relationMatrix[i, newone] = hopNum;
                        relationMatrix[newone, i] = hopNum;
                    }

                    presentNodes = newNodes;
                }
                while (presentNodes.Count != 0);
            }

            return relationMatrix;
        }

        private void DoSystemLayout(List<SystemElement> systemElements, List<NodeElement> nodeElements)
        {
            Dictionary<string, SystemElement> sysDict = new Dictionary<string, SystemElement>();
            if (systemElements != null && nodeElements != null)
            {
                foreach (SystemElement sysEle in systemElements)
                {
                    sysDict.Add(sysEle.Key, sysEle);
                }
                foreach (NodeElement nodeEle in nodeElements)
                {
                    if (sysDict.ContainsKey(nodeEle.ParentSystemID))
                        sysDict[nodeEle.ParentSystemID].ChildNodeNum += 1;
                }
                foreach (SystemElement sysEle in systemElements)
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
                    containerDict.Add(sysEle.Key, container);
                }
                foreach (SystemContainer sysCon in containerDict.Values)
                {
                    if (containerDict.ContainsKey(sysCon.Self.ParentSystemID))
                    {
                        containerDict[sysCon.Self.ParentSystemID].AddChildContainer(sysCon);
                    }
                }
                // Settle system coordinates
                rootContainer.ArrangeAllCoordinates(0, 0);
            }
        }

        /// <summary>
        /// Layout nodes within the system
        /// </summary>
        /// <param name="sysElement">nodes must be within this sytem</param>
        /// <param name="childSystems">child systems of this system</param>
        /// <param name="nodeElements">nodes, to be layouted</param>
        /// <param name="dialog">a progress bar</param>
        private void DoNodeLayout(SystemElement sysElement, List<SystemElement> childSystems, List<NodeElement> nodeElements, ProgressDialog dialog)
        {
            int[,] relationMatrix = CreateRelationMatrix(nodeElements);

            // For convenience, simulated annealing procedure will be calculated using
            // virtual grid coordinate system :positionMatrix[x,y] (0 <= x <= maxX, 0 <= y <= maxY)
            // gridDistance means physical distance in Piccolo between neighboring two nodes in grid coordinate system
            // An initial value of gridDistance is m_defGridDistance. gridDistance will be decreased until numbers of
            // position on grid coorindate system become sufficient to contain all nodes.
            bool[,] positionMatrix = new bool[1, 1];
            float gridDistance = m_defGridDistance;
            float systemMargin = m_defSystemMargin;
            bool posUnsettled = true;
            int maxX = 0;
            int maxY = 0;
            while (posUnsettled)
            {
                maxX = (int)((sysElement.X + sysElement.Width - systemMargin) / gridDistance)
                           - (int)((sysElement.X + systemMargin) / gridDistance) - 1;
                maxY = (int)((sysElement.Y + sysElement.Height - systemMargin) / gridDistance)
                           - (int)((sysElement.Y + systemMargin) / gridDistance) - 1;

                if (maxX < 0 || maxY < 0)
                {
                    gridDistance = gridDistance / 2f;
                    systemMargin = systemMargin / 2f;
                    continue;
                }

                positionMatrix = new bool[maxX + 1, maxY + 1];

                // Initialize position matrix
                for (int i = 0; i < maxX + 1; i++)
                    for (int j = 0; j < maxY + 1; j++)
                        positionMatrix[i, j] = false;

                foreach (SystemElement childElement in childSystems)
                {
                    int leftX = (int)((childElement.X + childElement.OffsetX) / gridDistance) - (int)((sysElement.X) / gridDistance) - 1;
                    int rightX = (int)((childElement.X + childElement.OffsetX + childElement.Width) / gridDistance) - (int)((sysElement.X) / gridDistance) - 1;
                    int upperY = (int)((childElement.Y + childElement.OffsetY) / gridDistance) - (int)((sysElement.Y) / gridDistance) - 1;
                    int lowerY = (int)((childElement.Y + childElement.OffsetY + childElement.Height) / gridDistance) - (int)((sysElement.Y) / gridDistance) - 1;
                    for (int i = leftX; i < rightX; i++)
                        for (int j = upperY; j < lowerY; j++)
                        {
                            if (0 <= i && i <= maxX && 0 <= j && j <= maxY)
                                positionMatrix[i, j] = true;
                        }
                }

                // Count the number of vacant points
                int numOfVacantPoint = 0;

                for (int i = 0; i < maxX + 1; i++)
                    for (int j = 0; j < maxY + 1; j++)
                        if (positionMatrix[i, j] == false)
                            numOfVacantPoint++;

                // Check if there is enough space for nodes
                if (nodeElements.Count * 2 < numOfVacantPoint)
                    posUnsettled = false;
                else
                {
                    gridDistance = gridDistance / 2f;
                    systemMargin = systemMargin / 2f;
                }
            }

            // Layout nodes randomly in the system
            RandomizeLayout(maxX, maxY, nodeElements, positionMatrix);

            // Create random number generator
            Random ran = new Random();

            // Repeat simulated annealing steps
            float t = m_initialT;
            int k = 0;

            while (k < m_kmax)
            {
                int nodeIndex = 0;
                foreach (NodeElement nodeEle in nodeElements)
                {
                    DirectionEnergyDiff neighbor = GetNeighbor(relationMatrix, nodeIndex, nodeElements, maxX, maxY, positionMatrix);

                    float currentT = m_initialT * (m_kmax - k) / m_kmax;

                    if (ran.NextDouble() < GetProbability(neighbor.EnergyDifference, currentT))
                        MoveNode(nodeEle, neighbor.Direction, positionMatrix);

                    nodeIndex++;
                }
                dialog.Bar.PerformStep();
                k++;
            }

            // Transform coordinate system from virtual grid coordinate to piccolo coordinate
            float baseX = ((int)((sysElement.X + systemMargin) / gridDistance) + 1) * gridDistance;
            float baseY = ((int)((sysElement.Y + systemMargin) / gridDistance) + 1) * gridDistance;

            foreach (NodeElement nodeEle in nodeElements)
            {
                nodeEle.X = nodeEle.X * gridDistance + baseX;
                nodeEle.Y = nodeEle.Y * gridDistance + baseY;
            }
        }

        /// <summary>
        /// In the figure below, N means a node and number means a position into one of which
        /// a node is going to be move.  
        ///  1 | 2 | 3
        /// ---+---+---
        ///  4 | N | 5
        /// ---+---+---
        ///  6 | 7 | 8
        /// At first, an energy will be calculated when a node is in N.
        /// Next, an energy will be calculated when a node is in 1.
        /// Also, energies wil be calculated when a node is in 2 - 8
        /// Energy differences will be calculated when a node is moved from N to each of neighboring positions (1 - 8)
        /// At last, one neighboring position of 1 - 8 which has lowest energy difference will be selected.
        /// </summary>
        /// <param name="relationMatrix">
        ///     This matrix contains node relation.
        ///     If relationMatrix[1,3] is true, nodeElements[1] and nodeElements[3] are connected in a pathway. 
        /// </param>
        /// <param name="nodeIndex">a node of nodeElements at nodeIndex position, will be moved</param>
        /// <param name="nodeElements">an array of all nodes</param>
        /// <param name="maxX">node will be put between 0 &lt;= x &lt;= maxX</param>
        /// <param name="maxY">node will be put between 0 &lt;= y &lt;= maxY</param>
        /// <param name="posMatrix">a matrix of positions in grid coordinate system</param>
        /// <returns></returns>
        private DirectionEnergyDiff GetNeighbor(int[,] relationMatrix, int nodeIndex, List<NodeElement> nodeElements, int maxX, int maxY, bool[,] posMatrix)
        {
            int presentX = (int)nodeElements[nodeIndex].X;
            int presentY = (int)nodeElements[nodeIndex].Y;

            GridLayout.Direction lowestEnergyDir = Direction.None;
            float lowestEnergy = 0;
            float energyAtN = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);

            // Move to leftupper
            if (presentX != 0 && presentY != 0 && !posMatrix[presentX - 1, presentY - 1])
            {
                MoveNode(nodeElements[nodeIndex], Direction.LeftUpper, null);
                float energyWhenMoved = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);
                MoveNode(nodeElements[nodeIndex], Direction.RightLower, null);

                if ((lowestEnergyDir == Direction.None) || (energyWhenMoved < lowestEnergy))
                {
                    lowestEnergyDir = Direction.LeftUpper;
                    lowestEnergy = energyWhenMoved;
                }
            }

            // Move to upper
            if (presentY != 0 && !posMatrix[presentX, presentY - 1])
            {
                MoveNode(nodeElements[nodeIndex], Direction.Upper, null);
                float energyWhenMoved = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);
                MoveNode(nodeElements[nodeIndex], Direction.Lower, null);

                if ((lowestEnergyDir == Direction.None) || (energyWhenMoved < lowestEnergy))
                {
                    lowestEnergyDir = Direction.Upper;
                    lowestEnergy = energyWhenMoved;
                }
            }

            // Move to rightupper
            if (presentX != maxX && presentY != 0 && !posMatrix[presentX + 1, presentY - 1])
            {
                MoveNode(nodeElements[nodeIndex], Direction.RightUpper, null);
                float energyWhenMoved = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);
                MoveNode(nodeElements[nodeIndex], Direction.LeftLower, null);

                if ((lowestEnergyDir == Direction.None) || (energyWhenMoved < lowestEnergy))
                {
                    lowestEnergyDir = Direction.RightUpper;
                    lowestEnergy = energyWhenMoved;
                }
            }

            // Move to right
            if (presentX != maxX && !posMatrix[presentX + 1, presentY])
            {
                MoveNode(nodeElements[nodeIndex], Direction.Right, null);
                float energyWhenMoved = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);
                MoveNode(nodeElements[nodeIndex], Direction.Left, null);

                if ((lowestEnergyDir == Direction.None) || (energyWhenMoved < lowestEnergy))
                {
                    lowestEnergyDir = Direction.Right;
                    lowestEnergy = energyWhenMoved;
                }
            }

            // Move to rightlower
            if (presentX != maxX && presentY != maxY && !posMatrix[presentX + 1, presentY + 1])
            {
                MoveNode(nodeElements[nodeIndex], Direction.RightLower, null);
                float energyWhenMoved = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);
                MoveNode(nodeElements[nodeIndex], Direction.LeftUpper, null);

                if ((lowestEnergyDir == Direction.None) || (energyWhenMoved < lowestEnergy))
                {
                    lowestEnergyDir = Direction.RightLower;
                    lowestEnergy = energyWhenMoved;
                }
            }

            // Move to lower
            if (presentY != maxY && !posMatrix[presentX, presentY + 1])
            {
                MoveNode(nodeElements[nodeIndex], Direction.Lower, null);
                float energyWhenMoved = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);
                MoveNode(nodeElements[nodeIndex], Direction.Upper, null);

                if ((lowestEnergyDir == Direction.None) || (energyWhenMoved < lowestEnergy))
                {
                    lowestEnergyDir = Direction.Lower;
                    lowestEnergy = energyWhenMoved;
                }
            }

            // Move to leftlower
            if (presentX != 0 && presentY != maxY && !posMatrix[presentX - 1, presentY + 1])
            {
                MoveNode(nodeElements[nodeIndex], Direction.LeftLower, null);
                float energyWhenMoved = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);
                MoveNode(nodeElements[nodeIndex], Direction.RightUpper, null);

                if ((lowestEnergyDir == Direction.None) || (energyWhenMoved < lowestEnergy))
                {
                    lowestEnergyDir = Direction.LeftLower;
                    lowestEnergy = energyWhenMoved;
                }
            }

            // Move to left
            if (presentX != 0 && !posMatrix[presentX - 1, presentY])
            {
                MoveNode(nodeElements[nodeIndex], Direction.Left, null);
                float energyWhenMoved = CalculateEnergy(relationMatrix, nodeIndex, nodeElements);
                MoveNode(nodeElements[nodeIndex], Direction.Right, null);

                if ((lowestEnergyDir == Direction.None) || (energyWhenMoved < lowestEnergy))
                {
                    lowestEnergyDir = Direction.Left;
                    lowestEnergy = energyWhenMoved;
                }
            }

            if (lowestEnergyDir != Direction.None)
                return new DirectionEnergyDiff(lowestEnergyDir, lowestEnergy - energyAtN);
            else
                return new DirectionEnergyDiff(Direction.None, 0);
        }

        /// <summary>
        /// Calculate transition probability.
        /// See http://en.wikipedia.org/wiki/Simulated_annealing for detail
        /// </summary>
        /// <param name="difference">energy difference</param>
        /// <param name="temperature">temperature</param>
        /// <returns></returns>
        private double GetProbability(float difference, float temperature)
        {
            if (difference < 0)
                return 1;
            else
            {
                double p = Math.Exp((double)((-1 * difference) / temperature));
                return p;
            }
        }

        /// <summary>
        /// Move the position of a node to the direction.
        /// </summary>
        /// <param name="nodeElement">a node, to be moved.</param>
        /// <param name="direction">direction, to be moved to.</param>
        /// <param name="posMatrix">a matrix of position in grid coordinate system</param>
        private void MoveNode(NodeElement nodeElement, GridLayout.Direction direction, bool[,] posMatrix)
        {
            if (posMatrix != null)
                posMatrix[(int)nodeElement.X, (int)nodeElement.Y] = false;
            switch (direction)
            {
                case Direction.LeftUpper:
                    nodeElement.X = nodeElement.X - 1;
                    nodeElement.Y = nodeElement.Y - 1;
                    break;
                case Direction.Upper:
                    nodeElement.Y = nodeElement.Y - 1;
                    break;
                case Direction.RightUpper:
                    nodeElement.X = nodeElement.X + 1;
                    nodeElement.Y = nodeElement.Y - 1;
                    break;
                case Direction.Right:
                    nodeElement.X = nodeElement.X + 1;
                    break;
                case Direction.RightLower:
                    nodeElement.X = nodeElement.X + 1;
                    nodeElement.Y = nodeElement.Y + 1;
                    break;
                case Direction.Lower:
                    nodeElement.Y = nodeElement.Y + 1;
                    break;
                case Direction.LeftLower:
                    nodeElement.X = nodeElement.X - 1;
                    nodeElement.Y = nodeElement.Y + 1;
                    break;
                case Direction.Left:
                    nodeElement.X = nodeElement.X - 1;
                    break;
            }
            if (posMatrix != null)
                posMatrix[(int)nodeElement.X, (int)nodeElement.Y] = true;
        }

        /// <summary>
        /// Put nodes in random positions
        /// </summary>
        /// <param name="maxX">node will be put between 0 &lt;= x &lt;= maxX</param>
        /// <param name="maxY">node will be put between 0 &lt;= y &lt;= maxY</param>
        /// <param name="nodeElements">nodes, which will be layouted</param>
        /// <param name="posMatrix">matrix of position in grid coordinate system</param>
        /// <returns>false, if no enough space for nodes. otherwise, return true.</returns>
        private bool RandomizeLayout(int maxX, int maxY, List<NodeElement> nodeElements, bool[,] posMatrix)
        {
            // When no enough vacant positions, return with false
            if ((maxX + 1) * (maxY + 1) < nodeElements.Count)
                return false;

            // Initialize reservedPoints to true.
            /*
            for(int i = 0; i < maxX + 1; i++ )
                for(int j = 0; j < maxY + 1; j++)
                    posMatrix[i,j] = false;
            
             */
            // Constansiate a random number generator.
            Random ran = new Random();

            // In each loop, a node will be assigned a position
            foreach (NodeElement nodeEle in nodeElements)
            {
                int x = -1;
                int y = -1;

                // do until a vacant position will be found
                do
                {
                    x = ran.Next(maxX + 1);
                    y = ran.Next(maxY + 1);
                }
                while (posMatrix[x, y]);

                nodeEle.X = x;
                nodeEle.Y = y;

                posMatrix[x, y] = true;
            }

            return true;
        }

        #endregion

        #region Inner class
        class DirectionEnergyDiff
        {
            #region Fields
            private GridLayout.Direction m_direction;
            private float m_energyDiff;
            #endregion

            #region Accessors
            public GridLayout.Direction Direction
            {
                get { return m_direction; }
                set { m_direction = value; }
            }
            public float EnergyDifference
            {
                get { return m_energyDiff; }
                set { m_energyDiff = value; }
            }
            #endregion

            #region Constructor
            public DirectionEnergyDiff(GridLayout.Direction dir, float ene)
            {
                this.m_direction = dir;
                this.m_energyDiff = ene;
            }
            #endregion
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
            private float m_squarePerNode = 20000;//25000;
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
                    do
                    {
                        m_pointNumSqrt++;
                    } while (m_pointNumSqrt * m_pointNumSqrt < m_childNodeNum);
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
                if (m_self != null && m_self.Key.Equals("/") && m_childSystems.Count == 0)
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
                if (!m_isDummyContainer)
                {
                    foreach (SystemContainer sysCont in m_childSystems)
                    {
                        if (sysCont.IsDummyContainer)
                        {
                            return sysCont.RequestNodePosition();
                        }
                    }
                    return new PointF();
                }
                int newPointNum = m_consumedPointNum + 1;
                int gridX = 0;
                int gridY = 0;
                for (int i = 0; i < m_pointNumSqrt; i++)
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
                if (m_consumedPointNum >= m_pointNumSqrt * m_pointNumSqrt)
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
                if (m_childSystems.Count == 0)
                    return;
                x += m_outlineWidth + m_margin;
                y += m_outlineWidth + m_margin;
                foreach (SystemContainer container in m_childSystems)
                {
                    container.SettleCoordinates(x, y);
                    x += container.Width + m_margin;
                }
            }
        #endregion
        }
    }
}
