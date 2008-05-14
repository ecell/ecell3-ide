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
using EcellLib.Objects;

namespace EcellLib.Layout
{
    /// <summary>
    /// Common algorithm of Layout algorithm.
    /// </summary>
    public class LayoutBase
    {
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
            float minX = nodeList[0].X;
            float maxX = nodeList[0].X;
            float minY = nodeList[0].Y;
            float maxY = nodeList[0].Y;
            foreach (EcellObject node in nodeList)
            {
                if (node.X < minX)
                    minX = node.X;
                else if (maxX < node.X)
                    maxX = node.X;
                if (node.Y < minY)
                    minY = node.Y;
                else if (maxY < node.Y)
                    maxY = node.Y;
            }
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

    }
}
