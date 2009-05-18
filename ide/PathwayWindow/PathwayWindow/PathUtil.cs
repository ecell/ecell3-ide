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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow
{
    /// <summary>
    /// Utilities for PathwayWindow.
    /// </summary>
    public class PathUtil
    {
        /// <summary>
        /// Get bounds to focus on a object.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static RectangleF GetFocusBound(RectangleF node, RectangleF camera)
        {
            if(node.Width > camera.Width || node.Height > camera.Height)
            {
                return node;
            }
            else
            {
                float x = node.X + node.Width / 2f - camera.Width / 2f;
                float y = node.Y + node.Height / 2f - camera.Height / 2f;
                return new RectangleF(x, y, camera.Width, camera.Height);
            }
        }

        /// <summary>
        /// Create new EcellReference's name.
        /// </summary>
        /// <param name="refList"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        public static string GetNewReferenceName(List<EcellReference> refList, int coefficient)
        {
            string baseName = null;

            if(coefficient == 0)
            {
                baseName = "C";
            }
            else if (0 < coefficient)
            {
                baseName = "P";
            }
            else
            {
                baseName = "S";
            }

            string newName = null;
            int i = 0;

            while(null == newName)
            {
                newName = baseName + i;

                foreach(EcellReference reference in refList)
                {
                    if (null != reference.Name && reference.Name.Equals(newName))
                        newName = null;
                }

                i++;
            }

            return newName;

        }

        /// <summary>
        /// Set PathwayLayout to EcellObject. 
        /// </summary>
        /// <param name="eo"></param>
        /// <param name="obj"></param>
        public static void SetLayout(EcellObject eo, PPathwayObject obj)
        {
            eo.Layer = obj.Layer.Name;
            eo.X = obj.X + obj.OffsetX;
            eo.Y = obj.Y + obj.OffsetY;
            eo.Width = obj.Width;
            eo.Height = obj.Height;
            eo.OffsetX = 0f;
            eo.OffsetY = 0f;
            eo.isFixed = false;
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
        /// Create RectangleF which has point1 and point2 as it's opposing corner
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static RectangleF GetRectangle(PointF point1, PointF point2)
        {
            RectangleF rect = new RectangleF(
                Math.Min(point1.X,point2.X),
                Math.Min(point1.Y,point2.Y),
                Math.Abs(point1.X - point2.X),
                Math.Abs(point1.Y - point2.Y));
            return rect;
        }

        /// <summary>
        /// Get key moved to another system.
        /// </summary>
        /// <param name="originalKey"></param>
        /// <param name="originalSystemKey"></param>
        /// <param name="newSystemKey"></param>
        /// <returns></returns>
        public static string GetMovedKey(string originalKey, string originalSystemKey, string newSystemKey)
        {
            if (null == originalKey || null == originalSystemKey || null == newSystemKey)
                return null;
            string newKey;
            if (originalSystemKey.Equals(Constants.delimiterPath) && !newSystemKey.Equals(Constants.delimiterPath))
                newKey = newSystemKey + originalKey.Replace("/:", ":");
            else if (!originalSystemKey.Equals(Constants.delimiterPath) && newSystemKey.Equals(Constants.delimiterPath))
                newKey = originalKey.Replace(originalSystemKey, Constants.delimiterPath);
            else
                newKey = originalKey.Replace(originalSystemKey, newSystemKey).Replace("/:", Constants.delimiterColon);
            return newKey.Replace("//", Constants.delimiterPath);
        }
    }
}
