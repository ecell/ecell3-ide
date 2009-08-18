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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using Ecell.Objects;
using System.Xml;
using Ecell.Exceptions;
using System.Windows.Forms;

namespace Ecell.Plugin
{
    /// <summary>
    /// Common algorithm of Layout algorithm.
    /// </summary>
    public abstract class LayoutBase : IEcellPlugin, ILayoutAlgorithm, IMenuStripProvider
    {
        /// <summary>
        /// 
        /// </summary>
        protected ApplicationEnvironment m_env;

        /// <summary>
        /// 
        /// </summary>
        protected ILayoutPanel m_panel = null;

        /// <summary>
        /// 
        /// </summary>
        protected int m_subIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            set { m_env = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILayoutPanel Panel
        {
            get { return m_panel; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SubIndex
        {
            get { return m_subIndex; }
            set { m_subIndex = value; }
        }

        /// <summary>
        /// Returns list of selected EcellObjects.
        /// </summary>
        /// <param name="objList">List of EcellObject</param>
        /// <returns>Selected EcellObject List</returns>
        public static List<EcellObject> GetSelectedObject(List<EcellObject> objList)
        {
            List<EcellObject> returnList = new List<EcellObject>();
            foreach (EcellObject eo in objList)
                if (eo.isFixed)
                    returnList.Add(eo);

            return returnList;
        }

        /// <summary>
        /// Returns list of selected EcellObjects.
        /// </summary>
        /// <param name="objList">List of EcellObject</param>
        /// <returns>Selected EcellObject List</returns>
        public static List<EcellObject> GetRelatedObject(List<EcellObject> objList)
        {
            List<EcellObject> returnList = new List<EcellObject>();
            foreach (EcellObject eo in objList)
                if (eo.isFixed)
                    returnList.Add(eo);

            return returnList;
        }

        /// <summary>
        /// Get a rectangle surrounded by given nodes.
        /// </summary>
        /// <param name="nodeList">rectangle, which will be surrounded by these nodes will be returned.</param>
        /// <returns>surrounded rectangle</returns>
        public static RectangleF GetSurroundingRect(List<EcellObject> nodeList)
        {
            float minX = nodeList[0].CenterPointF.X;
            float maxX = nodeList[0].CenterPointF.X;
            float minY = nodeList[0].CenterPointF.Y;
            float maxY = nodeList[0].CenterPointF.Y;
            foreach (EcellObject node in nodeList)
            {
                if (node.CenterPointF.X < minX)
                    minX = node.CenterPointF.X;
                else if (maxX < node.CenterPointF.X)
                    maxX = node.CenterPointF.X;
                if (node.CenterPointF.Y < minY)
                    minY = node.CenterPointF.Y;
                else if (maxY < node.CenterPointF.Y)
                    maxY = node.CenterPointF.Y;
            }
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="pointC"></param>
        /// <param name="pointD"></param>
        /// <returns></returns>
        public static bool DoesIntersect(PointF pointA, PointF pointB, PointF pointC, PointF pointD)
        {
            PointF point;
            return DoesIntersect(pointA, pointB, pointC, pointD, out point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="pointC"></param>
        /// <param name="pointD"></param>
        /// <param name="pointIntersection"></param>
        /// <returns></returns>
        public static bool DoesIntersect(PointF pointA, PointF pointB, PointF pointC, PointF pointD, out PointF pointIntersection)
        {
            pointIntersection = new PointF();
            float gradient = (pointB.X - pointA.X) * (pointD.Y - pointC.Y) - (pointB.Y - pointA.Y) * (pointD.X - pointC.X);
            if (0 == gradient)
                return false;

            PointF vectorAC = new PointF(pointC.X - pointA.X, pointC.Y - pointA.Y);
            float dR = ((pointD.Y - pointC.Y) * vectorAC.X - (pointD.X - pointC.X) * vectorAC.Y) / gradient;
            float dS = ((pointB.Y - pointA.Y) * vectorAC.X - (pointB.X - pointA.X) * vectorAC.Y) / gradient;
            if (dR < 0 || dR > 1 || dS < 0 || dS > 1)
                return false;

            pointIntersection = new PointF(pointA.X + dR * (pointB.X - pointA.X), pointA.Y + dR * (pointB.Y - pointA.Y));
            if (pointA.X == pointB.X)
                pointIntersection.X = pointA.X; 
            if (pointC.X == pointD.X)
                pointIntersection.X = pointC.X;
            if (pointA.Y == pointB.Y)
                pointIntersection.Y = pointA.Y;
            if (pointC.Y == pointD.Y)
                pointIntersection.Y = pointC.Y;

            return true;
        }

        /// <summary>
        /// Set Grid position.
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="systemList"></param>
        /// <param name="margin">distance between</param>
        public void SetGrid(List<EcellObject> nodeList, List<EcellObject> systemList, float margin)
        {
            Dictionary<string, List<PointF>> gridDic = GetLayoutGrid(systemList, margin);
            foreach (EcellObject entity in nodeList)
            {
                // Get nearest point
                List<PointF> grids = gridDic[entity.ParentSystemID];
                PointF point = entity.PointF;
                PointF newPos = point;
                float dist = GetDistance(point, grids[0]);
                foreach (PointF grid in grids)
                {
                    float temp = GetDistance(point, grid);
                    if (dist < temp)
                        continue;
                    // set grid.
                    newPos = grid;
                    dist = temp;
                }
                grids.Remove(newPos);
                entity.PointF = newPos;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public float GetDistance(PointF point1, PointF point2)
        {
            float dist = (float)Math.Sqrt(Math.Pow((double)(point1.X - point2.X), 2d) + Math.Pow((double)(point1.Y - point2.Y), 2d));
            return dist;

        }

        /// <summary>
        /// Create LayoutGrid with
        /// </summary>
        /// <param name="systemList"></param>
        /// <returns></returns>
        public static Dictionary<string, List<PointF>> GetLayoutGrid(List<EcellObject> systemList, float margin)
        {
            Dictionary<string, List<PointF>> gridDic = new Dictionary<string, List<PointF>>();
            // Set Grid
            foreach (EcellObject system in systemList)
            {
                List<PointF> list = new List<PointF>();
                for (int x = (int)(system.X + margin /2f); x < system.Layout.Right; )
                {
                    for (int y = (int)(system.Y + margin / 2f); y < system.Layout.Bottom; )
                    {
                        list.Add(new PointF((float)x,(float)y));
                        y = y + (int)margin;
                    }
                    x = x + (int)margin;
                }
                gridDic.Add(system.Key, list);
            }
            // Remove duplicated point.
            foreach (EcellObject system in systemList)
            {
                if (!gridDic.ContainsKey(system.ParentSystemID))
                    continue;

                List<PointF> list = gridDic[system.ParentSystemID];
                List<PointF> temp = new List<PointF>();
                RectangleF rect = system.Rect;
                foreach (PointF grid in list)
                {
                    if (rect.Contains(grid))
                        temp.Add(grid);
                }
                foreach (PointF grid in temp)
                {
                    list.Remove(grid);
                }
            }
            return gridDic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="pointC"></param>
        /// <param name="pointD"></param>
        /// <returns></returns>
        public static PointF GetIntersectingPoint(PointF pointA, PointF pointB, PointF pointC, PointF pointD)
        {
            PointF point;
            if (!DoesIntersect(pointA, pointB, pointC, pointD, out point))
                throw new EcellException("Two lines do not intersect.");

            return point;
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetPluginName();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetVersionString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public virtual void ChangeStatus(ProjectStatus status)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, Delegate> GetPublicDelegate()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual List<IPropertyItem> GetPropertySettings()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual XmlNode GetPluginStatus()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public virtual void SetPluginStatus(XmlNode status)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subCommandNum"></param>
        /// <param name="layoutSystem"></param>
        /// <param name="systemList"></param>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        public abstract bool DoLayout(int subCommandNum,
                      bool layoutSystem,
                      List<EcellObject> systemList,
                      List<EcellObject> nodeList);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract LayoutType GetLayoutType();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetLayoutName();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<ToolStripMenuItem> GetMenuStripItems();

    }
}
