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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Ecell.Plugin;
using Ecell.Objects;
using System.Reflection;

namespace Ecell.IDE.Plugins.CircularLayout
{
    /// <summary>
    /// Layout algorithm to layout nodes on a circle
    /// </summary>
    public class CircularLayout : LayoutBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CircularLayout()
        {
            m_panel = new CircularLayoutPanel(this);
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"MessageWindow"</returns>
        public override string GetPluginName()
        {
            return "CircularLayout";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override string GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
        public override bool DoLayout(int subNum,
                             bool layoutSystem,
                             List<EcellObject> systemList,
                             List<EcellObject> nodeList)
        {
            // Set selected node list.
            List<EcellObject> selectedList = GetSelectedObject(nodeList);
            // Error check.
            if (selectedList.Count <= 2)
                return false;

            // Number of layouts to be layouted.
            int nodeNum = selectedList.Count;
            // Get region.
            RectangleF rect = GetSurroundingRect(selectedList);

            // If arguments are invalid, show message and return.
            if (!Validate(nodeNum, rect))
                return false;

            // Coordinates of the points on the circle.
            // circlePoints[0] is the coordinates of the east point on the circle.
            // circlePoints[1] is the coordinates of the next clockwise point from circlePoints[0], and so on.
            PointF[] circlePoints = GetCirclePoints(rect, nodeNum);

            // Distance between the points one the circle.
            float[] iDistance = GetInsideDistanceMatrix(circlePoints);

            // first numLayout elements are NodeElements to be layouted.
            // And rest are NodeElement which are connected with layouted nodes.
            // List<EcellObject> orderedList = GetRelatedNodes(selectedList);

            //// Relation between the points on the circle and the points not on the circle.
            bool[,] relation = GetRelationMatrix(selectedList);

            // Distances between the points on the circle and the points not on the circle.
            float[,] bDistance = GetBetweenDistanceMatrix(circlePoints, selectedList);

            //// Settle positions with which the sum of edges lengthes become shortest.
            int[] pos = GetBestPositions(nodeNum, relation, iDistance, bDistance);

            //// Nodes are moved, at last.
            MoveNodeElement(pos, selectedList, circlePoints);

            return true;
        }


        #region Inner use

        private void SortNodes(List<EcellObject> nodeList)
        {
            bool[,] relation = GetRelationMatrix(nodeList);
        }
        /// <summary>
        /// Calculate the coordinates of the points which are equally spaced on a circle.
        /// </summary>
        /// <param name="rect">the region of circle</param>
        /// <param name="nodeNum">the number of points on a circles</param>
        /// <returns>circle points</returns>
        private PointF[] GetCirclePoints(RectangleF rect, int nodeNum)
        {
            float rx = rect.Width / 2;
            float ry = rect.Height / 2;
            float cx = rect.X + rx;
            float cy = rect.Y + ry;
            float rad = (float)Math.PI * 2f / nodeNum; // space between nodes in radian

            PointF[] circlePoints = new PointF[nodeNum];
            for (int i = 0; i < nodeNum; i++)
            {
                circlePoints[i].X = cx + rx * (float)Math.Cos(rad * i);
                circlePoints[i].Y = cy - ry * (float)Math.Sin(rad * i);
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
        /// <param name="nodeList">NodeElement of points</param>
        /// <returns></returns>
        private float[,] GetBetweenDistanceMatrix(PointF[] circlePoints, List<EcellObject> nodeList)
        {
            int inP = circlePoints.Length;
            int outP = nodeList.Count - inP;

            float[,] matrix = new float[inP, outP];

            // Fill in matrix with distance.
            int count = 0;
            foreach (EcellObject node in nodeList)
            {
                if (count >= circlePoints.Length)
                {
                    PointF point = new PointF(node.X, node.Y);

                    for (int i = 0; i < circlePoints.Length; i++)
                        matrix[i, count - circlePoints.Length] = GetDistance(point, circlePoints[i]);
                }

                count++;
            }

            return matrix;
        }

        /// <summary>
        /// Get distance between two points
        /// </summary>
        /// <param name="point1">point 1</param>
        /// <param name="point2">point 2</param>
        /// <returns>Distance between point 1 and point 2</returns>
        public static float GetDistance(PointF point1, PointF point2)
        {
            double x = point2.X - point1.X;
            double y = point2.Y - point1.Y;
            return (float)Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// Get relation matrix between the nodes.
        /// return[m,n] means whether connection exists between m'th node and n'th node of relatedNodes
        /// </summary>
        /// <param name="nodeList">Relation between these nodes will be searched.</param>
        /// <returns>relation matrix. true if two nodes connected, false if not.</returns>
        private bool[,] GetRelationMatrix(List<EcellObject> nodeList)
        {
            bool[,] relationMatrix = new bool[nodeList.Count, nodeList.Count];

            Dictionary<string, int> varDict = new Dictionary<string, int>();

            int count = 0;
            foreach (EcellObject node in nodeList)
            {
                if (node is EcellVariable)
                    varDict.Add(node.Key, count);                
                count++;
            }

            count = 0;
            foreach (EcellObject node in nodeList)
            {
                if(node is EcellProcess)
                {
                    foreach (EcellReference er in ((EcellProcess)node).ReferenceList)
                    {
                        if (varDict.ContainsKey(er.Key))
                        {
                            relationMatrix[count, varDict[er.Key]] = true;
                            relationMatrix[varDict[er.Key], count] = true;
                        }
                    }
                }
                count++;
            }
            return relationMatrix;
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
                    distMat[i] = GetDistance(circlePoints[0], circlePoints[i]);
                }
                else
                {
                    distMat[i] = distMat[ circlePoints.Length - i ];
                }
            }
             
            return distMat;
        }

        /// <summary>
        /// Get nodes related to layouting.
        /// When the number of nodes is m, first m'th elements of return list contains nodes on the circle.
        /// And the rest of the list contains nodes which are connected with nodes on the circle.
        /// </summary>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        private List<EcellObject> GetRelatedNodes(List<EcellObject> nodeList)
        {
            List<EcellObject> returnNodes = new List<EcellObject>();

            Dictionary<string, EcellObject> inDict = new Dictionary<string, EcellObject>();
            Dictionary<string, EcellObject> outDict = new Dictionary<string, EcellObject>();

            foreach (EcellObject node in nodeList)
                if (node.isFixed)
                {
                    returnNodes.Add(node);
                    if (node is EcellVariable)
                        inDict.Add(node.Key, node);
                }
                else if (node is EcellVariable)
                    outDict.Add(node.Key, node);

            Dictionary<string, EcellObject> relatedOutDict = new Dictionary<string, EcellObject>();

            foreach (EcellObject node in nodeList)
            {
                if (node is EcellProcess)
                {
                    EcellProcess pro = (EcellProcess)node;

                    if (pro.isFixed)
                    {
                        foreach (EcellReference er in pro.ReferenceList)
                        {
                            string unique = er.Key + ":variable";
                            if (outDict.ContainsKey(er.Key) && !relatedOutDict.ContainsKey(unique))
                            {
                                returnNodes.Add(outDict[er.Key]);
                                relatedOutDict.Add(unique, outDict[er.Key]);
                            }
                        }
                    }
                    else
                    {
                        foreach (EcellReference er in pro.ReferenceList)
                        {
                            string unique = er.Key + ":process";
                            if (inDict.ContainsKey(er.Key) && !relatedOutDict.ContainsKey(unique))
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
        /// Move each NodeElements positions to the point on the circle
        /// </summary>
        /// <param name="positions">positions[m] indicates m'th element of relatedNodes comes to which point of circlePoints</param>
        /// <param name="relatedNodes">first positions.Length elements of relatedNodes are on the circle</param>
        /// <param name="circlePoints">Coordinates of each circle points</param>
        private void MoveNodeElement(int[] positions, List<EcellObject> relatedNodes, PointF[] circlePoints)
        {
            int count = 0;
            foreach (EcellObject node in relatedNodes)
            {
                PointF coords = circlePoints[positions[count]];

                node.X = coords.X;
                node.Y = coords.Y;

                count++;
                if (count >= positions.Length)
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
                Util.ShowErrorDialog(MessageResCircularLayout.MsgLessNode);
                return false;
            }

            if (rect.Width == 0 || rect.Height == 0)
            {
                Util.ShowErrorDialog(MessageResCircularLayout.MsgSelectRect);
                return false;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// Get LayoutType of this layout
        /// </summary>
        /// <returns>LayoutType of this layout</returns>
        public override LayoutType GetLayoutType()
        {
            return LayoutType.Selected;
        }

        /// <summary>
        /// Get a name of this layout
        /// </summary>
        /// <returns>a name of this layout</returns>
        public override string GetLayoutName()
        {
            return "Circular";
        }

        /// <summary>
        /// Return MenuStrips for Ecell IDE's MainMenu.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            ToolStripMenuItem layoutMenu = new ToolStripMenuItem();
            layoutMenu.Name = MenuConstants.MenuItemLayout;

            ToolStripMenuItem algoMenu = new ToolStripMenuItem(
                MessageResCircularLayout.MenuItemCircular,
                null,
                new EventHandler(delegate(object o, EventArgs e)
                {
                    m_env.PluginManager.DiagramEditor.InitiateLayout(this, 0);
                })
            );

            algoMenu.ToolTipText = MessageResCircularLayout.ToolTip;
            layoutMenu.DropDownItems.Add(algoMenu);
            return new ToolStripMenuItem[] { layoutMenu };
        }

        #region Inner class
        class DirectionEnergyDiff
        {
            private int m_direction;
            private float m_energyDiff;

            public DirectionEnergyDiff(int dir, float diff)
            {
                m_direction = dir;
                m_energyDiff = diff;
            }

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
        }
        #endregion

    }
}
