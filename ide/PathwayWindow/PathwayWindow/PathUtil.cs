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

        #region Fields
        private static PathUtil m_util = null;
        private Dictionary<string, Brush> m_brushDic = null;
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
                    if (null != reference.name && reference.name.Equals(newName))
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
                newKey = newSystemKey + originalKey;
            else if (!originalSystemKey.Equals("/") && newSystemKey.Equals("/"))
                newKey = originalKey.Replace(originalSystemKey, "");
            else
                newKey = originalKey.Replace(originalSystemKey, newSystemKey);
            return newKey;
        }
    }

    public class BrushManager
    {
        /// <summary>
        /// Brush Dictionary.
        /// </summary>
        private static Dictionary<string, Brush> m_brushDic = null;

        /// <summary>
        /// Parse argument string into Brush
        /// For example, when an argument is "white", Brushes.White will be returned.
        /// </summary>
        /// <param name="color">A color you want</param>
        /// <returns>Brush, which has a color indicated by argument.
        ///          Null will be returned when an argument can't be parsed.
        /// </returns>
        public static Brush ParseStringToBrush(string color)
        {
            if (string.IsNullOrEmpty(color))
                return null;
            if (m_brushDic == null)
                m_brushDic = CreateBrushDictionary();

            color = color.ToLower();
            Brush brush = null;
            foreach (KeyValuePair<string, Brush> pair in m_brushDic)
            {
                string key = pair.Key.ToLower();
                if (key.Equals(color))
                    brush = pair.Value;
            }
            return brush;
        }

        /// <summary>
        /// Parse Brush into string.
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static string ParseBrushToString(Brush brush)
        {
            if (brush == null)
                return null;
            if (m_brushDic == null)
                m_brushDic = CreateBrushDictionary();

            string key = null;
            foreach(KeyValuePair<string, Brush> pair in m_brushDic)
            {
                if(pair.Value.Equals(brush))
                    key = pair.Key;
            }
            return key;
        }
        /// <summary>
        /// Create a dictionary (key: name of brush, value: Brush object)
        /// </summary>
        public static Dictionary<string, Brush> CreateBrushDictionary()
        {
            Dictionary<string, Brush> brushDic = new Dictionary<string, Brush>();
            brushDic.Add("AliceBlue", Brushes.AliceBlue);
            brushDic.Add("AntiqueWhite", Brushes.AntiqueWhite);
            brushDic.Add("Aqua", Brushes.Aqua);
            brushDic.Add("Aquamarine", Brushes.Aquamarine);
            brushDic.Add("Azure", Brushes.Azure);
            brushDic.Add("Beige", Brushes.Beige);
            brushDic.Add("Bisque", Brushes.Bisque);
            brushDic.Add("Black", Brushes.Black);
            brushDic.Add("BlanchedAlmond", Brushes.BlanchedAlmond);
            brushDic.Add("Blue", Brushes.Blue);
            brushDic.Add("BlueViolet", Brushes.BlueViolet);
            brushDic.Add("Brown", Brushes.Brown);
            brushDic.Add("BurlyWood", Brushes.BurlyWood);
            brushDic.Add("CadetBlue", Brushes.CadetBlue);
            brushDic.Add("Chartreuse", Brushes.Chartreuse);
            brushDic.Add("Chocolate", Brushes.Chocolate);
            brushDic.Add("Coral", Brushes.Coral);
            brushDic.Add("CornflowerBlue", Brushes.CornflowerBlue);
            brushDic.Add("Cornsilk", Brushes.Cornsilk);
            brushDic.Add("Crimson", Brushes.Crimson);
            brushDic.Add("Cyan", Brushes.Cyan);
            brushDic.Add("DarkBlue", Brushes.DarkBlue);
            brushDic.Add("DarkCyan", Brushes.DarkCyan);
            brushDic.Add("DarkGoldenrod", Brushes.DarkGoldenrod);
            brushDic.Add("DarkGray", Brushes.DarkGray);
            brushDic.Add("DarkGreen", Brushes.DarkGreen);
            brushDic.Add("DarkKhaki", Brushes.DarkKhaki);
            brushDic.Add("DarkMagenta", Brushes.DarkMagenta);
            brushDic.Add("DarkOliveGreen", Brushes.DarkOliveGreen);
            brushDic.Add("DarkOrange", Brushes.DarkOrange);
            brushDic.Add("DarkOrchid", Brushes.DarkOrchid);
            brushDic.Add("DarkRed", Brushes.DarkRed);
            brushDic.Add("DarkSalmon", Brushes.DarkSalmon);
            brushDic.Add("DarkSeaGreen", Brushes.DarkSeaGreen);
            brushDic.Add("DarkSlateBlue", Brushes.DarkSlateBlue);
            brushDic.Add("DarkSlateGray", Brushes.DarkSlateGray);
            brushDic.Add("DarkTurquoise", Brushes.DarkTurquoise);
            brushDic.Add("DarkViolet", Brushes.DarkViolet);
            brushDic.Add("DeepPink", Brushes.DeepPink);
            brushDic.Add("DeepSkyBlue", Brushes.DeepSkyBlue);
            brushDic.Add("DimGray", Brushes.DimGray);
            brushDic.Add("DodgerBlue", Brushes.DodgerBlue);
            brushDic.Add("Firebrick", Brushes.Firebrick);
            brushDic.Add("FloralWhite", Brushes.FloralWhite);
            brushDic.Add("ForestGreen", Brushes.ForestGreen);
            brushDic.Add("Fuchsia", Brushes.Fuchsia);
            brushDic.Add("Gainsboro", Brushes.Gainsboro);
            brushDic.Add("GhostWhite", Brushes.GhostWhite);
            brushDic.Add("Gold", Brushes.Gold);
            brushDic.Add("Goldenrod", Brushes.Goldenrod);
            brushDic.Add("Gray", Brushes.Gray);
            brushDic.Add("Green", Brushes.Green);
            brushDic.Add("GreenYellow", Brushes.GreenYellow);
            brushDic.Add("Honeydew", Brushes.Honeydew);
            brushDic.Add("HotPink", Brushes.HotPink);
            brushDic.Add("IndianRed", Brushes.IndianRed);
            brushDic.Add("Indigo", Brushes.Indigo);
            brushDic.Add("Ivory", Brushes.Ivory);
            brushDic.Add("Khaki", Brushes.Khaki);
            brushDic.Add("Lavender", Brushes.Lavender);
            brushDic.Add("LavenderBlush", Brushes.LavenderBlush);
            brushDic.Add("LawnGreen", Brushes.LawnGreen);
            brushDic.Add("LemonChiffon", Brushes.LemonChiffon);
            brushDic.Add("LightBlue", Brushes.LightBlue);
            brushDic.Add("LightCoral", Brushes.LightCoral);
            brushDic.Add("LightCyan", Brushes.LightCyan);
            brushDic.Add("LightGoldenrodYellow", Brushes.LightGoldenrodYellow);
            brushDic.Add("LightGray", Brushes.LightGray);
            brushDic.Add("LightGreen", Brushes.LightGreen);
            brushDic.Add("LightPink", Brushes.LightPink);
            brushDic.Add("LightSalmon", Brushes.LightSalmon);
            brushDic.Add("LightSeaGreen", Brushes.LightSeaGreen);
            brushDic.Add("LightSkyBlue", Brushes.LightSkyBlue);
            brushDic.Add("LightSlateGray", Brushes.LightSlateGray);
            brushDic.Add("LightSteelBlue", Brushes.LightSteelBlue);
            brushDic.Add("LightYellow", Brushes.LightYellow);
            brushDic.Add("Lime", Brushes.Lime);
            brushDic.Add("LimeGreen", Brushes.LimeGreen);
            brushDic.Add("Linen", Brushes.Linen);
            brushDic.Add("Magenta", Brushes.Magenta);
            brushDic.Add("Maroon", Brushes.Maroon);
            brushDic.Add("MediumAquamarine", Brushes.MediumAquamarine);
            brushDic.Add("MediumBlue", Brushes.MediumBlue);
            brushDic.Add("MediumOrchid", Brushes.MediumOrchid);
            brushDic.Add("MediumPurple", Brushes.MediumPurple);
            brushDic.Add("MediumSeaGreen", Brushes.MediumSeaGreen);
            brushDic.Add("MediumSlateBlue", Brushes.MediumSlateBlue);
            brushDic.Add("MediumSpringGreen", Brushes.MediumSpringGreen);
            brushDic.Add("MediumTurquoise", Brushes.MediumTurquoise);
            brushDic.Add("MediumVioletRed", Brushes.MediumVioletRed);
            brushDic.Add("MidnightBlue", Brushes.MidnightBlue);
            brushDic.Add("MintCream", Brushes.MintCream);
            brushDic.Add("MistyRose", Brushes.MistyRose);
            brushDic.Add("Moccasin", Brushes.Moccasin);
            brushDic.Add("NavajoWhite", Brushes.NavajoWhite);
            brushDic.Add("Navy", Brushes.Navy);
            brushDic.Add("OldLace", Brushes.OldLace);
            brushDic.Add("Olive", Brushes.Olive);
            brushDic.Add("OliveDrab", Brushes.OliveDrab);
            brushDic.Add("Orange", Brushes.Orange);
            brushDic.Add("OrangeRed", Brushes.OrangeRed);
            brushDic.Add("Orchid", Brushes.Orchid);
            brushDic.Add("PaleGoldenrod", Brushes.PaleGoldenrod);
            brushDic.Add("PaleGreen", Brushes.PaleGreen);
            brushDic.Add("PaleTurquoise", Brushes.PaleTurquoise);
            brushDic.Add("PaleVioletRed", Brushes.PaleVioletRed);
            brushDic.Add("PapayaWhip", Brushes.PapayaWhip);
            brushDic.Add("PeachPuff", Brushes.PeachPuff);
            brushDic.Add("Peru", Brushes.Peru);
            brushDic.Add("Pink", Brushes.Pink);
            brushDic.Add("Plum", Brushes.Plum);
            brushDic.Add("PowderBlue", Brushes.PowderBlue);
            brushDic.Add("Purple", Brushes.Purple);
            brushDic.Add("Red", Brushes.Red);
            brushDic.Add("RosyBrown", Brushes.RosyBrown);
            brushDic.Add("RoyalBlue", Brushes.RoyalBlue);
            brushDic.Add("SaddleBrown", Brushes.SaddleBrown);
            brushDic.Add("Salmon", Brushes.Salmon);
            brushDic.Add("SandyBrown", Brushes.SandyBrown);
            brushDic.Add("SeaGreen", Brushes.SeaGreen);
            brushDic.Add("SeaShell", Brushes.SeaShell);
            brushDic.Add("Sienna", Brushes.Sienna);
            brushDic.Add("Silver", Brushes.Silver);
            brushDic.Add("SkyBlue", Brushes.SkyBlue);
            brushDic.Add("SlateBlue", Brushes.SlateBlue);
            brushDic.Add("SlateGray", Brushes.SlateGray);
            brushDic.Add("Snow", Brushes.Snow);
            brushDic.Add("SpringGreen", Brushes.SpringGreen);
            brushDic.Add("SteelBlue", Brushes.SteelBlue);
            brushDic.Add("Tan", Brushes.Tan);
            brushDic.Add("Teal", Brushes.Teal);
            brushDic.Add("Thistle", Brushes.Thistle);
            brushDic.Add("Tomato", Brushes.Tomato);
            brushDic.Add("Transparent", Brushes.Transparent);
            brushDic.Add("Turquoise", Brushes.Turquoise);
            brushDic.Add("Violet", Brushes.Violet);
            brushDic.Add("Wheat", Brushes.Wheat);
            brushDic.Add("White", Brushes.White);
            brushDic.Add("WhiteSmoke", Brushes.WhiteSmoke);
            brushDic.Add("Yellow", Brushes.Yellow);
            brushDic.Add("YellowGreen", Brushes.YellowGreen);

            return brushDic;
        }
    }
}
