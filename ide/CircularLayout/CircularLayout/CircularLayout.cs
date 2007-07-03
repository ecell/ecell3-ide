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
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace EcellLib.CircularLayout
{
    /// <summary>
    /// Layout algorithm to layout nodes on a circle
    /// </summary>
    public class CircularLayout : ILayoutAlgorithm
    {
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
            // A rectangle area in which all nodes to be layouted exist.
            foreach(NodeElement element in nodeElements)
                if (!element.Fixed)
                    newNodeElements.Add(element);

            RectangleF rect = GetSurroundingRect(newNodeElements);

            // If arguments are invalid, show message and return.
            if(!Validate(newNodeElements.Count, rect))
                return false;

            RectangleF square = GetMaximumSquare(rect);

            // Number of layouts to be layouted.
            int numLayout = newNodeElements.Count;
            
            newNodeElements = null; // delete reference for memory usage

            // first numLayout elements are NodeElements to be layouted.
            // And rest are NodeElement which are connected with layouted nodes.
            List<NodeElement> orderedList = GetRelatedNodes(nodeElements);

            // Coordinates of the points on the circle.
            // circlePoints[0] is the coordinates of the east point on the circle.
            // circlePoints[1] is the coordinates of the next anticlockwise point from circlePoints[0], and so on.
            PointF[] circlePoints = CalcCirclePoints(new PointF(square.X + square.Width / 2, square.Y + square.Height / 2),
                                                    square.Width / 2, numLayout);

            // Distance between the points one the circle.
            float[] iDistance = GetInsideDistanceMatrix(circlePoints);

            // Distances between the points on the circle and the points not on the circle.
            float[,] bDistance = GetBetweenDistanceMatrix(circlePoints, orderedList);

            // Relation between the points on the circle and the points not on the circle.
            bool[,] relation = GetRelationMatrix(orderedList);

            // Settle positions with which the sum of edges lengthes become shortest.
            int[] pos = GetBestPositions(numLayout, relation, iDistance, bDistance);

            // Nodes are moved, at last.
            MoveNodeElement(pos, orderedList, circlePoints);

            return true;
        }

        #region Inner use

        /// <summary>
        /// Calculate the coordinates of the points which are equally spaced on a circle.
        /// </summary>
        /// <param name="center">the center point of a circle</param>
        /// <param name="radius">the radius of a circle</param>
        /// <param name="num">the number of points on a circles</param>
        /// <returns>circle points</returns>
        private PointF[] CalcCirclePoints(PointF center, float radius, int num)
        {
            PointF[] circlePoints = new PointF[num];

            float spaceR = (float)Math.PI * 2f / num; // space between nodes in radian

            for (int i = 0; i < num; i++ )
            {
                float x = center.X + radius * (float)Math.Cos(spaceR * i);
                float y = center.Y - radius * (float)Math.Sin(spaceR * i);
                circlePoints[i] = new PointF(x,y);
            }
            
            return circlePoints;
        }

        /// <summary>
        /// Calculate best positions for nodes on the circle.
        /// Best means that the sum of edge lengthes becomes smallest.
        /// </summary>
        /// <param name="num">the number of circle points</param>
        /// <param name="relation">relation table. true if two nodes are connected</param>
        /// <param name="insideDistance">array, which contains distance between nodes on the circle.</param>
        /// <param name="betweenDistance">m * n array contains distance between nodes on the circle and nodes not on the circle</param>
        /// <returns>position array, means which nodes comes to which position on the circle. 
        /// </returns>
        private int[] GetBestPositions(int num, bool[,] relation, float[] insideDistance, float[,] betweenDistance)
        {
            int[] position = new int[num];
            for (int i = 0; i < num; i++ )
            {
                position[i] = i;
            }

            Random ran = new Random();

            float initialT = 200;
            float t = initialT;
            int kmax = 100;
            int k = 0;

            while(k < kmax)
            {
                for (int i = 0; i < num; i++ )
                {
                    DirectionEnergyDiff neighbor = GetNeighbor(i, position, num, relation, insideDistance, betweenDistance);

                    float currentT = initialT * (kmax - k) / kmax;

                    if (ran.NextDouble() < GetProbability(neighbor.EnergyDifference, currentT))                    
                        InterchangePosition(i, neighbor.Direction, position);                    
                }

                k++;
            }

            return position;
        }

        /// <summary>
        /// This method is used by GetBestPosition() only.
        /// Position of a node, which is indicated by argument 'index' will be moved by argument 'by'
        /// </summary>
        /// <param name="index">A node is at this index in argument 'position'</param>
        /// <param name="by">moved by this argument</param>
        /// <param name="position">position array. the same as in GetBestPosition() method</param>
        private void InterchangePosition(int index, int by, int[] position)
        {
            int currentPos = position[index];
            int newPos = currentPos + by;

            if(newPos < 0)
                newPos = position.Length - Math.Abs(newPos % position.Length);
            else
                newPos = newPos % position.Length;

            for (int i = 0; i < position.Length; i++)
                if (position[i] == newPos)
                    position[i] = currentPos;
            
            position[index] = newPos;
        }

        /// <summary>
        /// At first, a node on the circle will be moved to next clockwise position and anticlockwise position and
        ///  energy will be calculated.
        /// Position with lower energy (clockwise or anticlockwise) will be returned with its energy.
        /// Energy, here, means the sum of lengthes of edges of a node and its neighbors.
        /// </summary>
        /// <param name="index">means which node will be moved</param>
        /// <param name="position">means which node will be at which position on the circle</param>
        /// <param name="num">the number of nodes on the circle</param>
        /// <param name="relation">means relationship between nodes. true if two nodes are connected</param>
        /// <param name="insideDistance">means distance between two nodes on the circle</param>
        /// <param name="betweenDistance">means distance between a node on the circle and a node not on the circle</param>
        /// <returns>which position has lower energy</returns>
        private DirectionEnergyDiff GetNeighbor(int index, int[] position, int num, bool[,] relation, float[] insideDistance, float[,] betweenDistance)
        {
            int allNum = relation.GetUpperBound(0) + 1; // The number of all related nodes.

            int currentPos = position[index];
            int plusPos = (currentPos == num - 1) ? 0 : currentPos + 1;
            int minusPos = (currentPos == 0) ? num - 1 : currentPos - 1;

            List<int> plusInsideNeighbor = new List<int>(); // neighbor nodes on the circle of a node on plus-one position now.
            List<int> plusOutsideNeighbor = new List<int>(); // neighbor nodes not on the circle of a node on plus-one position now.
            List<int> currentInsideNeighbor = new List<int>(); // neighbor nodes on the circle of a node on current position now.
            List<int> currentOutsideNeighbor = new List<int>(); // neighbor nodes not on the circle of a node on current position now.
            List<int> minusInsideNeighbor = new List<int>(); // neighbor nodes on the circle of a node on minus-one position now.
            List<int> minusOutsideNeighbor = new List<int>(); // neighbor nodes not on the circle of a node on minus-one position now.

            // Search neighbor nodes of a node currently on plus-one position
            for (int i = 0; i < num; i++ )
            {
                if (position[i] == plusPos)
                {
                    for (int j = 0; j < allNum; j++)
                        if (relation[i, j] && i != j)
                            if (j < num)
                                plusInsideNeighbor.Add(j);
                            else
                                plusOutsideNeighbor.Add(j);
                    continue;
                }
            }

            // Search neighbor nodes of a node currently on a position indicated by index
            for (int j = 0; j < allNum; j++ )
                if (relation[index, j] && index != j)
                    if (j < num)
                        currentInsideNeighbor.Add(j);
                    else
                        currentOutsideNeighbor.Add(j);
            
            // Search neighbor nodes of a node currently on minus-one position
            for (int i = 0; i < num; i++ )
            {
                if(position[i] == minusPos)
                {
                    for (int j = 0; j < allNum; j++ )
                        if (relation[i, j] && i != j)
                            if (j < num)
                                minusInsideNeighbor.Add(j);
                            else
                                minusOutsideNeighbor.Add(j);
                    continue;
                }
            }

            float currentE = 0; // Current energy.
            currentE += CalcEnergy(plusPos, num, plusInsideNeighbor, plusOutsideNeighbor, insideDistance, betweenDistance);
            currentE += CalcEnergy(currentPos, num, currentInsideNeighbor, currentOutsideNeighbor, insideDistance, betweenDistance);
            currentE += CalcEnergy(minusPos, num, minusInsideNeighbor, minusOutsideNeighbor, insideDistance, betweenDistance);

            float plusE = 0; // Energy when the node indicated by index were moved anticlockwise by one.
            plusE += CalcEnergy(plusPos, num, currentInsideNeighbor, currentOutsideNeighbor, insideDistance, betweenDistance);
            plusE += CalcEnergy(currentPos, num, plusInsideNeighbor, plusOutsideNeighbor, insideDistance, betweenDistance);
            plusE += CalcEnergy(minusPos, num, minusInsideNeighbor, minusOutsideNeighbor, insideDistance, betweenDistance);

            float minusE = 0; // Energy when the node indicated by index were moved clockwise by one.
            minusE += CalcEnergy(plusPos, num, plusInsideNeighbor, plusOutsideNeighbor, insideDistance, betweenDistance);
            minusE += CalcEnergy(currentPos, num, minusInsideNeighbor, minusOutsideNeighbor, insideDistance, betweenDistance);
            minusE += CalcEnergy(minusPos, num, currentInsideNeighbor, currentOutsideNeighbor, insideDistance, betweenDistance);

            if(plusE < minusE)
                return new DirectionEnergyDiff(1, plusE - currentE);
            else
                return new DirectionEnergyDiff(-1, minusE - currentE);
        }

        /// <summary>
        /// Calculation energy about a node indicated by pos argument.
        /// This is only used by GetNeighbor() method. For detail, see this method.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="numLayout"></param>
        /// <param name="insideNeighbor"></param>
        /// <param name="outsideNeighbor"></param>
        /// <param name="insideDistance"></param>
        /// <param name="betweenDistance"></param>
        /// <returns></returns>
        private float CalcEnergy(int pos, int numLayout, List<int> insideNeighbor, List<int> outsideNeighbor, float[] insideDistance, float[,] betweenDistance)
        {
            float energy = 0;

            foreach(int neighbor in insideNeighbor)
                energy += insideDistance[Math.Abs(neighbor - pos)];
            
            foreach(int neighbor in outsideNeighbor)
                energy += betweenDistance[pos, neighbor - numLayout];

            return energy;
        }

        private double GetProbability(float diff, float currentT)
        {
            if (diff < 0)
                return 1;
            else
                return Math.Exp((double)(-1 * diff) / currentT);
        }

        /// <summary>
        /// Get distance matrix between circle points and points which are not on the circle.
        /// When m is the number of circle points, and n is the number of points no on the circle,
        /// matrix float[m,n] will be returned.
        /// </summary>
        /// <param name="circlePoints">Coordinates of the circle points</param>
        /// <param name="nodeElements">NodeElement of points</param>
        /// <returns></returns>
        private float[,] GetBetweenDistanceMatrix(PointF[] circlePoints, List<NodeElement> nodeElements)
        {
            int inP = circlePoints.Length;
            int outP = nodeElements.Count - inP;

            float[,] matrix = new float[inP, outP];

            // Fill in matrix with distance.
            int count = 0;
            foreach(NodeElement node in nodeElements)
            {
                if (count >= circlePoints.Length)
                {
                    PointF point = new PointF(node.X, node.Y);

                    for (int i = 0; i < circlePoints.Length; i++ )
                        matrix[i,count -  circlePoints.Length] =  PathUtil.GetDistance(point, circlePoints[i]);                    
                }

                count++;
            }

            return matrix;
        }

        /// <summary>
        /// Get relation matrix between the nodes.
        /// return[m,n] means whether connection exists between m'th node and n'th node of relatedNodes
        /// </summary>
        /// <param name="relatedNodes">Relation between these nodes will be searched.</param>
        /// <returns>relation matrix. true if two nodes connected, false if not.</returns>
        private bool[,] GetRelationMatrix(List<NodeElement> relatedNodes)
        {
            bool[,] relateMat = new bool[relatedNodes.Count, relatedNodes.Count];

            // Initialize
            for (int m = 0; m < relatedNodes.Count; m++)
                for (int n = 0; n < relatedNodes.Count; n++)
                    relateMat[m, n] = false;

            Dictionary<string, int> varDict = new Dictionary<string, int>();

            int count = 0;
            foreach(NodeElement node in relatedNodes)
            {
                if (node is VariableElement)
                    varDict.Add(node.Key, count);                

                count++;
            }

            count = 0;
            foreach(NodeElement node in relatedNodes)
            {
                if(node is ProcessElement)
                {
                    foreach(string varKey in ((ProcessElement)node).Edges.Keys)
                    {
                        if(!string.IsNullOrEmpty(varKey) && varDict.ContainsKey(varKey))
                        {
                            relateMat[count, varDict[varKey]] = true;
                            relateMat[varDict[varKey], count] = true;
                        }
                    }
                }

                count++;
            }
            return relateMat;
        }

        /// <summary>
        /// Get relation matrix between the points on the circle and the points not on the circle.
        /// returnarray[m, n] means the relation the m'th node on the circle and the n'th node out of the circle.
        /// </summary>
        /// <param name="num">The number of circle points</param>
        /// <param name="relatedNodes">First num'th elements of relatedNodes are on the circle, the rest are not.</param>
        /// <returns>true if nodes are connected, false if nodes are not connected.</returns>
        private bool[,] GetBetweenRelationMatrix(int num, List<NodeElement> relatedNodes)
        {
            bool[,] relateMat = new bool[num, relatedNodes.Count - num];

            for(int m = 0; m < num; m++)
                for(int n = 0; n < relatedNodes.Count - num; n++)
                    relateMat[m,n] = false;

            Dictionary<string, int> inDict = new Dictionary<string, int>();
            Dictionary<string, int> outDict = new Dictionary<string, int>();
            
            int count = 0;
            foreach(NodeElement node in relatedNodes)
            {
                if(node is VariableElement)
                {
                    if (count < num)
                        inDict.Add(node.Key, count);
                    else
                        outDict.Add(node.Key, count - num);
                }

                count++;
            }

            count = 0;
            foreach (NodeElement node in relatedNodes)
            {
                if(node is ProcessElement)
                {
                    ProcessElement pro = (ProcessElement)node;

                    if (count < num)
                    {
                        foreach (string key in pro.Edges.Keys)
                        {
                            if (!string.IsNullOrEmpty(key) && outDict.ContainsKey(key))
                                relateMat[count, outDict[key]] = true;
                        }
                    }
                    else
                    {
                        foreach(string key in pro.Edges.Keys)
                        {
                            if(!string.IsNullOrEmpty(key) && inDict.ContainsKey(key))
                                relateMat[inDict[key], count - num] = true;
                        }
                    }
                }
                
                count++;
            }

            return relateMat;
        }

        /// <summary>
        /// Get distance array for circle points.
        /// Number i element of an array have distance between two nodes which have i - 1 nodes between them.
        /// </summary>
        /// <returns>distance array</returns>
        private float[] GetInsideDistanceMatrix(PointF[] circlePoints)
        {
            float[] distMat = new float[circlePoints.Length];

            float half = ((float)circlePoints.Length) / 2f;

            for (int i = 0; i < circlePoints.Length; i++ )
            {
                if(i <= half)
                {
                    distMat[i] = PathUtil.GetDistance(circlePoints[0], circlePoints[i]);
                }
                else
                {
                    distMat[i] = distMat[ circlePoints.Length - i ];
                }
            }
             
            return distMat;
        }

        /// <summary>
        /// Get a maximum square within the given rectangle area.
        /// The center of returned squre and given rectangle will be the same.
        /// </summary>
        /// <param name="rect">the given rectangle area.</param>
        /// <returns>square</returns>
        private RectangleF GetMaximumSquare(RectangleF rect)
        {
            if (rect.Width > rect.Height)
            {
                float midX = rect.X + rect.Width / 2;
                float leftX = midX - rect.Height / 2;
                float rightX = midX + rect.Height / 2;
                return new RectangleF(leftX, rect.Y, rightX - leftX, rect.Height);
            }
            else
            {
                float midY = rect.Y + rect.Height / 2;
                float upperY = midY - rect.Width / 2;
                float lowerY = midY + rect.Width / 2;
                return new RectangleF(rect.X, upperY, rect.Width, lowerY - upperY);
            }
        }

        /// <summary>
        /// Get nodes related to layouting.
        /// When the number of nodes is m, first m'th elements of return list contains nodes on the circle.
        /// And the rest of the list contains nodes which are connected with nodes on the circle.
        /// </summary>
        /// <param name="nodeElements"></param>
        /// <returns></returns>
        private List<NodeElement> GetRelatedNodes(List<NodeElement> nodeElements)
        {
            List<NodeElement> returnNodes = new List<NodeElement>();

            Dictionary<string, NodeElement> inDict = new Dictionary<string, NodeElement>();
            Dictionary<string, NodeElement> outDict = new Dictionary<string, NodeElement>();

            foreach(NodeElement node in nodeElements)
                if (!node.Fixed)
                {
                    returnNodes.Add(node);
                    if (node is VariableElement)
                        inDict.Add(node.Key, node);
                }
                else if (node is VariableElement)
                    outDict.Add(node.Key, node);

            Dictionary<string, NodeElement> relatedOutDict = new Dictionary<string,NodeElement>();

            foreach(NodeElement node in nodeElements)
            {
                if (node is ProcessElement)
                {
                    ProcessElement pro = (ProcessElement)node;

                    if (!pro.Fixed)
                    {
                        foreach (string key in pro.Edges.Keys)
                        {
                            string unique = key + ":variable";
                            if (outDict.ContainsKey(key) && !relatedOutDict.ContainsKey(unique))
                            {
                                returnNodes.Add(outDict[key]);
                                relatedOutDict.Add(unique, outDict[key]);
                            }
                        }
                    }
                    else
                    {
                        foreach(string key in pro.Edges.Keys)
                        {
                            string unique = pro.Key + ":process";
                            if(inDict.ContainsKey(key) && !relatedOutDict.ContainsKey(unique))
                            {
                                returnNodes.Add(pro);
                                relatedOutDict.Add(unique, pro);
                            }
                        }
                    }
                }
            }

            return returnNodes;
        }

        /// <summary>
        /// Get a rectangle surrounded by given nodes.
        /// </summary>
        /// <param name="nodeElements">rectangle, which will be surrounded by these nodes will be returned.</param>
        /// <returns>surrounded rectangle</returns>
        private RectangleF GetSurroundingRect(List<NodeElement> nodeElements)
        {
            float minX = 0;
            float maxX = 0;
            float minY = 0;
            float maxY = 0;
            bool isFirst = true;
            foreach (NodeElement element in nodeElements)
                if (!element.Fixed)
                {
                    if (isFirst)
                    {
                        minX = element.X;
                        maxX = element.X;
                        minY = element.Y;
                        maxY = element.Y;
                        isFirst = false;
                    }
                    else
                    {
                        if (element.X < minX)
                            minX = element.X;

                        if (maxX < element.X)
                            maxX = element.X;

                        if (element.Y < minY)
                            minY = element.Y;

                        if (maxY < element.Y)
                            maxY = element.Y;
                    }
                }
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Move each NodeElements positions to the point on the circle
        /// </summary>
        /// <param name="positions">positions[m] indicates m'th element of relatedNodes comes to which point of circlePoints</param>
        /// <param name="relatedNodes">first positions.Length elements of relatedNodes are on the circle</param>
        /// <param name="circlePoints">Coordinates of each circle points</param>
        private void MoveNodeElement(int[] positions, List<NodeElement> relatedNodes, PointF[] circlePoints)
        {
            int count = 0;
            foreach(NodeElement node in relatedNodes)
            {
                PointF coords = circlePoints[positions[count]];

                node.X = coords.X;
                node.Y = coords.Y;

                count++;
                if(count >= positions.Length)
                    return;
            }
        }

        /// <summary>
        /// Check if arguments are valid or not.
        /// </summary>
        /// <param name="num">number of nodes to be layouted</param>
        /// <param name="rect">nodes are layouted within this rectangle</param>
        /// <returns>valid or not</returns>
        private bool Validate(int num, RectangleF rect)
        {
            if (num < 3)
            {
                ComponentResourceManager crm = new ComponentResourceManager(typeof(CircularLayout));
                MessageBox.Show(crm.GetString("MsgLessNode"),
                     "Layout Error",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                return false;
            }

            if (rect.Width == 0 || rect.Height == 0)
            {
                ComponentResourceManager crm = new ComponentResourceManager(typeof(CircularLayout));
                MessageBox.Show(crm.GetString("MsgSelectRect"),
                    "Layout Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        #endregion

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
            ComponentResourceManager crm = new ComponentResourceManager(typeof(CircularLayout));
            return crm.GetString("MenuItemCircular");
        }

        /// <summary>
        /// Get a name of this layout
        /// </summary>
        /// <returns>a name of this layout</returns>
        public string GetName()
        {
            return "Circular";
        }

        /// <summary>
        /// Get a tooltip of this layout
        /// </summary>
        /// <returns>tooltip</returns>
        public string GetToolTipText()
        {
            ComponentResourceManager crm = new ComponentResourceManager(typeof(CircularLayout));
            return crm.GetString("ToolTip");
        }

        /// <summary>
        /// Get a list of names for submenu.
        /// </summary>
        /// <returns>a list of name of sub commands</returns>
        public List<string> GetSubCommands()
        {
            return null;
        }

        #region Inner class
        class DirectionEnergyDiff
        {
            private int m_direction;
            private float m_energyDiff;

            public int Direction
            {
                get { return m_direction; }
                set { m_direction = value; }
            }

            public float EnergyDifference
            {
                get { return m_energyDiff; }
                set { m_energyDiff = value; }
            }

            public DirectionEnergyDiff(int dir, float diff)
            {
                m_direction = dir;
                m_energyDiff = diff;
            }
        }
        #endregion
    }
}