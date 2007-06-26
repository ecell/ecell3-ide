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
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using EcellLib;
using EcellLib.PathwayWindow.Element;

namespace EcellLib.PathwayWindow
{
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

        #region Accessors
        public Dictionary<string, Brush> BrushDic
        {
            get { return m_brushDic; }
            set { this.m_brushDic = value; }
        }
        #endregion

        /// <summary>
        /// This method checks whether an EcellReference list contain a key with same coefficient or not.
        /// </summary>
        /// <param name="list">list of EcellReference to be checked</param>
        /// <param name="key">key of Entity</param>
        /// <param name="coefficient">coefficient of reference</param>
        /// <returns>true if a list contains a key. false if a list doesn't contain a key</returns>
        public static bool CheckReferenceListContainsEntity(List<EcellReference> list, string key, int coefficient)
        {
            // null check
            if (null == list || null == key)
                return false;

            bool contains = false;

            foreach(EcellReference reference in list)
            {
                if (reference.fullID.EndsWith(key))
                {
                    if (reference.coefficient * coefficient >= 0)
                        contains = true;
                }
            }

            return contains;
        }

        /// <summary>
        /// Create new instance of EcellReference which has same information of an argument EcellReference
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static EcellReference CopyEcellReference(EcellReference reference)
        {
            EcellReference newRef = new EcellReference();
            newRef.coefficient = reference.coefficient;
            newRef.fullID = reference.fullID;
            newRef.isAccessor = reference.isAccessor;
            newRef.name = reference.name;
            return newRef;
        }

        /// <summary>
        /// Get an environment variable.
        /// If such an variable that has a key doesn't exist, registry will be searched.
        /// </summary>
        /// <param name="key">a key for an environment variable</param>
        /// <returns>
        ///    a environment variable, if a key has a value.
        ///    null, if a key is null or '' or doesn't have a value.
        /// </returns>
        public static string GetEnvironmentVariable4DirPath(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey subkey = regkey.OpenSubKey(EcellLib.Util.s_registryEnvKey);
            string dirName = (string)subkey.GetValue(key);            
            if (string.IsNullOrEmpty(dirName) || !Directory.Exists(dirName))
            {
                subkey.Close();
                subkey = regkey.OpenSubKey(EcellLib.Util.s_registrySWKey);
                dirName = (string)subkey.GetValue(key);
            }
            subkey.Close();
            regkey.Close();

            return dirName;
        }

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

        public static PointF[] GetArrowPoints(PointF arrowApex,
                                              PointF guidePoint,
                                              float arrowRadianA,
                                              float arrowRadianB,
                                              float arrowLength)
        {
            guidePoint.X = guidePoint.X - arrowApex.X;
            guidePoint.Y = guidePoint.Y - arrowApex.Y;

            float factor = PathUtil.GetDistance(guidePoint, new Point(0,0));
            if(factor == 0)
                return new PointF[] { arrowApex, arrowApex, arrowApex };
            guidePoint.X = guidePoint.X / factor;
            float guideRadian = (float)Math.Acos(guidePoint.X);
            if (guidePoint.Y < 0)
                guideRadian = 6.283f - guideRadian;

            arrowRadianA += guideRadian;
            arrowRadianB += guideRadian;

            PointF arrowPointA = new PointF((float)Math.Cos(arrowRadianA), (float)Math.Sin(arrowRadianA));
            PointF arrowPointB = new PointF((float)Math.Cos(arrowRadianB), (float)Math.Sin(arrowRadianB));

            arrowPointA.X = arrowPointA.X * arrowLength + arrowApex.X;
            arrowPointA.Y = arrowPointA.Y * arrowLength + arrowApex.Y;
            arrowPointB.X = arrowPointB.X * arrowLength + arrowApex.X;
            arrowPointB.Y = arrowPointB.Y * arrowLength + arrowApex.Y;

            return new PointF[] { arrowApex, arrowPointA, arrowPointB };
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

        public static string RemovePath(string absolutePath)
        {
            if (absolutePath == null || absolutePath.Equals("/"))
                return "";
            else if (absolutePath.Contains(":"))
            {
                return m_preColonRegex.Replace(absolutePath,"");
            }
            else
            {
                return m_preSlashRegex.Replace(absolutePath,"");
            }
        }

        /// <summary>
        /// Convert Ecell FullID to key
        ///  ex) When argument is ":/CELL/CYTOPLASM:P", a return value will be "/CELL/CYTOPLASM:P"
        /// </summary>
        /// <param name="fullID">Ecell FullID</param>
        /// <returns>Ecell key</returns>
        public static string FullID2Key(string fullID)
        {
            if (fullID == null)
                return null;
            else
                return m_headColonRegex.Replace(fullID, "");
        }

        /// <summary>
        /// Convert Ecell key to FullID
        ///  ex) When argument is "/CELL/CYTOPLASM:P", a return value will be ":/CELL/CYTOPLASM:P"
        /// </summary>
        /// <param name="key">Ecell key</param>
        /// <returns>Ecell FullID</returns>
        public static string Key2FullID(string key)
        {
            return ":" + key;
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
        /// Parse argument string into Brush
        /// For example, when an argument is "white", Brushes.White will be returned.
        /// </summary>
        /// <param name="color">A color you want</param>
        /// <returns>Brush, which has a color indicated by argument.
        ///          Null will be returned when an argument can't be parsed.
        /// </returns>
        public static Brush GetBrushFromString(string color)
        {
            if(color == null || color.Equals(""))
            {
                return null;
            }
            color = color.ToLower();
            
            if(m_util == null)
            {
                m_util = new PathUtil();
            }

            if(m_util.BrushDic == null)
            {
                m_util.createBrushDictionary();
            }

            Dictionary<string, Brush> brushDic = m_util.BrushDic;

            if (brushDic.ContainsKey(color))
            {
                return brushDic[color];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Serialize a list of PathwayElements into a file.
        /// </summary>
        /// <param name="fileName">Serialized into this file</param>
        /// <param name="list">PathwayElement objects, which is going to be serialized</param>
        public static void SerializeElements(string fileName, List<PathwayElement> list)
        {
            if (fileName == null)
                return;
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                foreach(PathwayElement element in list)
                {
                    formatter.Serialize(stream, element);
                }
            }
        }

        /// <summary>
        /// Deserialize a list of PathwayElements from a file
        /// </summary>
        /// <param name="fileName">Deserialized from this file</param>
        /// <returns>Deserialized list of PathwayElements</returns>
        public static List<PathwayElement> DeserializeElements(string fileName)
        {
            List<PathwayElement> list = new List<PathwayElement>();
            if (fileName == null || !File.Exists(fileName))
                return list;
            BinaryFormatter formatter = new BinaryFormatter();            
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                while (stream.Length != stream.Position)
                {
                    list.Add((PathwayElement)formatter.Deserialize(stream));
                }
            }
            return list;
        }

        public void createBrushDictionary()
        {
            m_brushDic = new Dictionary<string, Brush>();
            m_brushDic.Add("aliceblue",Brushes.AliceBlue);
            m_brushDic.Add("antiquewhite", Brushes.AntiqueWhite);
            m_brushDic.Add("aqua", Brushes.Aqua);
            m_brushDic.Add("aquamarine", Brushes.Aquamarine);
            m_brushDic.Add("azure", Brushes.Azure);
            m_brushDic.Add("beige", Brushes.Beige);
            m_brushDic.Add("bisque", Brushes.Bisque);
            m_brushDic.Add("black", Brushes.Black);
            m_brushDic.Add("blanchedalmond", Brushes.BlanchedAlmond);
            m_brushDic.Add("blue", Brushes.Blue);
            m_brushDic.Add("blueviolet", Brushes.BlueViolet);
            m_brushDic.Add("brown", Brushes.Brown);
            m_brushDic.Add("burlywood", Brushes.BurlyWood);
            m_brushDic.Add("cadetblue", Brushes.CadetBlue);
            m_brushDic.Add("chartreuse", Brushes.Chartreuse);
            m_brushDic.Add("chocolate", Brushes.Chocolate);
            m_brushDic.Add("coral", Brushes.Coral);
            m_brushDic.Add("cornflowerblue", Brushes.CornflowerBlue);
            m_brushDic.Add("cornsilk", Brushes.Cornsilk);
            m_brushDic.Add("crimson", Brushes.Crimson);
            m_brushDic.Add("cyan", Brushes.Cyan);
            m_brushDic.Add("darkblue", Brushes.DarkBlue);
            m_brushDic.Add("darkcyan", Brushes.DarkCyan);
            m_brushDic.Add("darkgoldenrod", Brushes.DarkGoldenrod);
            m_brushDic.Add("darkgray", Brushes.DarkGray);
            m_brushDic.Add("darkgreen", Brushes.DarkGreen);
            m_brushDic.Add("darkkhaki", Brushes.DarkKhaki);
            m_brushDic.Add("darkmagenta", Brushes.DarkMagenta);
            m_brushDic.Add("darkolivegreen", Brushes.DarkOliveGreen);
            m_brushDic.Add("darkorange", Brushes.DarkOrange);
            m_brushDic.Add("darkorchid", Brushes.DarkOrchid);
            m_brushDic.Add("darkred", Brushes.DarkRed);
            m_brushDic.Add("darksalmon", Brushes.DarkSalmon);
            m_brushDic.Add("darkseagreen", Brushes.DarkSeaGreen);
            m_brushDic.Add("darkslateblue", Brushes.DarkSlateBlue);
            m_brushDic.Add("darkslategray", Brushes.DarkSlateGray);
            m_brushDic.Add("darkturquoise", Brushes.DarkTurquoise);
            m_brushDic.Add("darkviolet", Brushes.DarkViolet);
            m_brushDic.Add("deeppink", Brushes.DeepPink);
            m_brushDic.Add("deepskyblue", Brushes.DeepSkyBlue);
            m_brushDic.Add("dimgray", Brushes.DimGray);
            m_brushDic.Add("dodgerblue", Brushes.DodgerBlue);
            m_brushDic.Add("firebrick", Brushes.Firebrick);
            m_brushDic.Add("floralwhite", Brushes.FloralWhite);
            m_brushDic.Add("forestgreen", Brushes.ForestGreen);
            m_brushDic.Add("fuchsia", Brushes.Fuchsia);
            m_brushDic.Add("gainsboro", Brushes.Gainsboro);
            m_brushDic.Add("ghostwhite", Brushes.GhostWhite);
            m_brushDic.Add("gold", Brushes.Gold);
            m_brushDic.Add("goldenrod", Brushes.Goldenrod);
            m_brushDic.Add("gray", Brushes.Gray);
            m_brushDic.Add("green", Brushes.Green);
            m_brushDic.Add("greenyellow", Brushes.GreenYellow);
            m_brushDic.Add("honeydew", Brushes.Honeydew);
            m_brushDic.Add("hotpink", Brushes.HotPink);
            m_brushDic.Add("indianred", Brushes.IndianRed);
            m_brushDic.Add("indigo", Brushes.Indigo);
            m_brushDic.Add("ivory", Brushes.Ivory);
            m_brushDic.Add("khaki", Brushes.Khaki);
            m_brushDic.Add("lavender", Brushes.Lavender);
            m_brushDic.Add("lavenderblush", Brushes.LavenderBlush);
            m_brushDic.Add("lawngreen", Brushes.LawnGreen);
            m_brushDic.Add("lemonchiffon", Brushes.LemonChiffon);
            m_brushDic.Add("lightblue", Brushes.LightBlue);
            m_brushDic.Add("lightcoral", Brushes.LightCoral);
            m_brushDic.Add("lightcyan", Brushes.LightCyan);
            m_brushDic.Add("lightgoldenrodyellow", Brushes.LightGoldenrodYellow);
            m_brushDic.Add("lightgray", Brushes.LightGray);
            m_brushDic.Add("lightgreen", Brushes.LightGreen);
            m_brushDic.Add("lightpink", Brushes.LightPink);
            m_brushDic.Add("lightsalmon", Brushes.LightSalmon);
            m_brushDic.Add("lightseagreen", Brushes.LightSeaGreen);
            m_brushDic.Add("lightskyblue", Brushes.LightSkyBlue);
            m_brushDic.Add("lightslategray", Brushes.LightSlateGray);
            m_brushDic.Add("lightsteelblue", Brushes.LightSteelBlue);
            m_brushDic.Add("lightyellow", Brushes.LightYellow);
            m_brushDic.Add("lime", Brushes.Lime);
            m_brushDic.Add("limegreen", Brushes.LimeGreen);
            m_brushDic.Add("linen", Brushes.Linen);
            m_brushDic.Add("magenta", Brushes.Magenta);
            m_brushDic.Add("maroon", Brushes.Maroon);
            m_brushDic.Add("mediumaquamarine", Brushes.MediumAquamarine);
            m_brushDic.Add("mediumblue", Brushes.MediumBlue);
            m_brushDic.Add("mediumorchid", Brushes.MediumOrchid);
            m_brushDic.Add("mediumpurple", Brushes.MediumPurple);
            m_brushDic.Add("mediumseagreen", Brushes.MediumSeaGreen);
            m_brushDic.Add("mediumslateblue", Brushes.MediumSlateBlue);
            m_brushDic.Add("mediumspringgreen", Brushes.MediumSpringGreen);
            m_brushDic.Add("mediumturquoise", Brushes.MediumTurquoise);
            m_brushDic.Add("mediumvioletred", Brushes.MediumVioletRed);
            m_brushDic.Add("midnightblue", Brushes.MidnightBlue);
            m_brushDic.Add("mintcream", Brushes.MintCream);
            m_brushDic.Add("mistyrose", Brushes.MistyRose);
            m_brushDic.Add("moccasin", Brushes.Moccasin);
            m_brushDic.Add("navajowhite", Brushes.NavajoWhite);
            m_brushDic.Add("navy", Brushes.Navy);
            m_brushDic.Add("oldlace", Brushes.OldLace);
            m_brushDic.Add("olive", Brushes.Olive);
            m_brushDic.Add("olivedrab", Brushes.OliveDrab);
            m_brushDic.Add("orange", Brushes.Orange);
            m_brushDic.Add("orangered", Brushes.OrangeRed);
            m_brushDic.Add("orchid", Brushes.Orchid);
            m_brushDic.Add("palegoldenrod", Brushes.PaleGoldenrod);
            m_brushDic.Add("palegreen", Brushes.PaleGreen);
            m_brushDic.Add("paleturquoise", Brushes.PaleTurquoise);
            m_brushDic.Add("palevioletred", Brushes.PaleVioletRed);
            m_brushDic.Add("papayawhip", Brushes.PapayaWhip);
            m_brushDic.Add("peachpuff", Brushes.PeachPuff);
            m_brushDic.Add("peru", Brushes.Peru);
            m_brushDic.Add("pink", Brushes.Pink);
            m_brushDic.Add("plum", Brushes.Plum);
            m_brushDic.Add("powderblue", Brushes.PowderBlue);
            m_brushDic.Add("purple", Brushes.Purple);
            m_brushDic.Add("red", Brushes.Red);
            m_brushDic.Add("rosybrown", Brushes.RosyBrown);
            m_brushDic.Add("royalblue", Brushes.RoyalBlue);
            m_brushDic.Add("saddlebrown", Brushes.SaddleBrown);
            m_brushDic.Add("salmon", Brushes.Salmon);
            m_brushDic.Add("sandybrown", Brushes.SandyBrown);
            m_brushDic.Add("seagreen", Brushes.SeaGreen);
            m_brushDic.Add("seashell", Brushes.SeaShell);
            m_brushDic.Add("sienna", Brushes.Sienna);
            m_brushDic.Add("silver", Brushes.Silver);
            m_brushDic.Add("skyblue", Brushes.SkyBlue);
            m_brushDic.Add("slateblue", Brushes.SlateBlue);
            m_brushDic.Add("slategray", Brushes.SlateGray);
            m_brushDic.Add("snow", Brushes.Snow);
            m_brushDic.Add("springgreen", Brushes.SpringGreen);
            m_brushDic.Add("steelblue", Brushes.SteelBlue);
            m_brushDic.Add("tan", Brushes.Tan);
            m_brushDic.Add("teal", Brushes.Teal);
            m_brushDic.Add("thistle", Brushes.Thistle);
            m_brushDic.Add("tomato", Brushes.Tomato);
            m_brushDic.Add("transparent", Brushes.Transparent);
            m_brushDic.Add("turquoise", Brushes.Turquoise);
            m_brushDic.Add("violet", Brushes.Violet);
            m_brushDic.Add("wheat", Brushes.Wheat);
            m_brushDic.Add("white", Brushes.White);
            m_brushDic.Add("whitesmoke", Brushes.WhiteSmoke);
            m_brushDic.Add("yellow", Brushes.Yellow);
            m_brushDic.Add("yellowgreen", Brushes.YellowGreen);
        }
    }
}
