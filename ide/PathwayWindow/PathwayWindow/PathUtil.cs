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
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using EcellLib;
using EcellLib.Objects;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Utilities for PathwayWindow.
    /// </summary>
    public class PathUtil
    {
        #region Readonlys
        private static readonly Regex m_headColonRegex = new Regex("^\\w*?:");
        private static readonly Regex m_preSlashRegex = new Regex("^.*/");
        private static readonly Regex m_postSlashRegex = new Regex("/\\w*$");
        private static readonly Regex m_postColonRegex = new Regex(":\\w*$");
        private static readonly Regex m_preColonRegex = new Regex("^.*:");
        #endregion

        /// <summary>
        /// Get bounds to focus on a object.
        /// </summary>
        /// <returns></returns>
        public static RectangleF GetFocusBound(RectangleF focusObj, float size)
        {
            float centerX = focusObj.X + focusObj.Width / 2f;
            float centerY = focusObj.Y + focusObj.Height / 2f;

            return new RectangleF(centerX - size / 2f, centerY - size / 2f, size, size);
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
        /// Get parent system's key from child node's key
        ///  ex) When an argument is "/CELL/CYTOPLASM", a return value is "/CELL"
        /// </summary>
        /// <param name="childId">Child node's key that you want to know it's parent</param>
        /// <returns>Parent system's key. "" will be returned when the argument is null or the root system</returns>
        public static string GetParentSystemId(string childId)
        {
            if (childId == null || childId.Equals("/"))
                return "";
            else if(childId.Contains(":"))
            {
                return m_postColonRegex.Replace(childId, "");
            }
            else
            {
                string returnStr = m_postSlashRegex.Replace(childId, "");
                if (returnStr.Equals(""))
                    return "/";
                else
                    return returnStr;
            }
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
            float x = 0;
            float y = 0;

            if (point1.X < point2.X)
            {
                x = point1.X;
            }
            else
            {
                x = point2.X;
            }
            if(point1.Y < point2.Y)
            {
                y = point1.Y;
            }
            else
            {
                y = point2.Y;
            }

            return new RectangleF(x,y,Math.Abs(point1.X - point2.X),Math.Abs(point1.Y - point2.Y));
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
            if (originalSystemKey.Equals("/") && !newSystemKey.Equals("/"))
                newKey = newSystemKey + originalKey.Substring(1);
            else if (!originalSystemKey.Equals("/") && newSystemKey.Equals("/"))
                newKey = originalKey.Replace(originalSystemKey, "/");
            else
                newKey = originalKey.Replace(originalSystemKey, newSystemKey);
            return newKey.Replace("//","/");
        }
    }
}
