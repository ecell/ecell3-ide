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

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.PathwayWindow.Graphics
{
    /// <summary>
    /// BrushManager
    /// </summary>
    public class BrushManager
    {
        /// <summary>
        /// Brush Dictionary.
        /// </summary>
        private static Dictionary<string, Brush> m_brushDic = null;
        /// <summary>
        /// Brush Dictionary.
        /// </summary>
        private static Dictionary<string, Color> m_colorDic = null;
        /// <summary>
        /// Brush name list.
        /// </summary>
        private static List<string> m_nameList = null;
        /// <summary>
        /// ImageList of brush colors.
        /// </summary>
        private static ImageList m_imageList = null;

        /// <summary>
        /// Returns a list of Brush names.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetBrushNameList()
        {
            if (m_brushDic == null)
                m_brushDic = CreateBrushDictionary();
            if (m_nameList != null)
                return m_nameList;

            m_nameList = new List<string>();
            foreach (string key in m_brushDic.Keys)
                m_nameList.Add(key);
            return m_nameList;
        }

        /// <summary>
        /// Returns a list of Brush ImageList.
        /// </summary>
        /// <returns></returns>
        public static ImageList BrushImageList
        {
            get
            {
                if (m_brushDic == null)
                    m_brushDic = CreateBrushDictionary();
                if (m_imageList != null)
                    return m_imageList;

                m_imageList = new ImageList();
                foreach (string key in m_brushDic.Keys)
                {
                    Brush brush = m_brushDic[key];
                    Image image = new Bitmap(16, 16);
                    System.Drawing.Graphics gra = System.Drawing.Graphics.FromImage(image);
                    gra.FillRectangle(brush, 0, 0, 15, 13);
                    gra.DrawRectangle(new Pen(Brushes.Black), 0, 0, 15, 13);
                    m_imageList.Images.Add(key, image);
                }
                return m_imageList;
            }
        }

        /// <summary>
        /// Parse argument string into Brush
        /// For example, when an argument is "white", Brushes.White will be returned.
        /// </summary>
        /// <param name="color">A name of color you want</param>
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
            foreach (KeyValuePair<string, Brush> pair in m_brushDic)
            {
                if (pair.Value.Equals(brush))
                    key = pair.Key;
            }
            return key;
        }

        /// <summary>
        /// Parse Brush into string.
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static Color ParseBrushToColor(Brush brush)
        {
            if(brush is SolidBrush)
                return ((SolidBrush)brush).Color;

            Color color = Color.Black;
            if (brush == null)
                return color;

            string brushName = ParseBrushToString(brush);

            if (m_colorDic == null)
                m_colorDic = CreateColorDictionary();

            foreach (KeyValuePair<string, Color> pair in m_colorDic)
            {
                if (pair.Key.Equals(brushName))
                    color = pair.Value;
            }
            return color;
        }

        /// <summary>
        /// Create GradientBrush.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="centerBrush"></param>
        /// <param name="fillBrush"></param>
        /// <returns></returns>
        public static PathGradientBrush CreateGradientBrush(GraphicsPath path, Brush centerBrush, Brush fillBrush)
        {
            PathGradientBrush pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = ParseBrushToColor(centerBrush);
            pthGrBrush.SurroundColors = new Color[] { ParseBrushToColor(fillBrush) };
            return pthGrBrush;
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
            //brushDic.Add("Transparent", Brushes.Transparent);
            brushDic.Add("Turquoise", Brushes.Turquoise);
            brushDic.Add("Violet", Brushes.Violet);
            brushDic.Add("Wheat", Brushes.Wheat);
            brushDic.Add("White", Brushes.White);
            brushDic.Add("WhiteSmoke", Brushes.WhiteSmoke);
            brushDic.Add("Yellow", Brushes.Yellow);
            brushDic.Add("YellowGreen", Brushes.YellowGreen);

            return brushDic;
        }
        /// <summary>
        /// Create a dictionary (key: name of brush, value: Brush object)
        /// </summary>
        public static Dictionary<string, Color> CreateColorDictionary()
        {
            Dictionary<string, Color> colorDic = new Dictionary<string, Color>();
            colorDic.Add("AliceBlue", Color.AliceBlue);
            colorDic.Add("AntiqueWhite", Color.AntiqueWhite);
            colorDic.Add("Aqua", Color.Aqua);
            colorDic.Add("Aquamarine", Color.Aquamarine);
            colorDic.Add("Azure", Color.Azure);
            colorDic.Add("Beige", Color.Beige);
            colorDic.Add("Bisque", Color.Bisque);
            colorDic.Add("Black", Color.Black);
            colorDic.Add("BlanchedAlmond", Color.BlanchedAlmond);
            colorDic.Add("Blue", Color.Blue);
            colorDic.Add("BlueViolet", Color.BlueViolet);
            colorDic.Add("Brown", Color.Brown);
            colorDic.Add("BurlyWood", Color.BurlyWood);
            colorDic.Add("CadetBlue", Color.CadetBlue);
            colorDic.Add("Chartreuse", Color.Chartreuse);
            colorDic.Add("Chocolate", Color.Chocolate);
            colorDic.Add("Coral", Color.Coral);
            colorDic.Add("CornflowerBlue", Color.CornflowerBlue);
            colorDic.Add("Cornsilk", Color.Cornsilk);
            colorDic.Add("Crimson", Color.Crimson);
            colorDic.Add("Cyan", Color.Cyan);
            colorDic.Add("DarkBlue", Color.DarkBlue);
            colorDic.Add("DarkCyan", Color.DarkCyan);
            colorDic.Add("DarkGoldenrod", Color.DarkGoldenrod);
            colorDic.Add("DarkGray", Color.DarkGray);
            colorDic.Add("DarkGreen", Color.DarkGreen);
            colorDic.Add("DarkKhaki", Color.DarkKhaki);
            colorDic.Add("DarkMagenta", Color.DarkMagenta);
            colorDic.Add("DarkOliveGreen", Color.DarkOliveGreen);
            colorDic.Add("DarkOrange", Color.DarkOrange);
            colorDic.Add("DarkOrchid", Color.DarkOrchid);
            colorDic.Add("DarkRed", Color.DarkRed);
            colorDic.Add("DarkSalmon", Color.DarkSalmon);
            colorDic.Add("DarkSeaGreen", Color.DarkSeaGreen);
            colorDic.Add("DarkSlateBlue", Color.DarkSlateBlue);
            colorDic.Add("DarkSlateGray", Color.DarkSlateGray);
            colorDic.Add("DarkTurquoise", Color.DarkTurquoise);
            colorDic.Add("DarkViolet", Color.DarkViolet);
            colorDic.Add("DeepPink", Color.DeepPink);
            colorDic.Add("DeepSkyBlue", Color.DeepSkyBlue);
            colorDic.Add("DimGray", Color.DimGray);
            colorDic.Add("DodgerBlue", Color.DodgerBlue);
            colorDic.Add("Firebrick", Color.Firebrick);
            colorDic.Add("FloralWhite", Color.FloralWhite);
            colorDic.Add("ForestGreen", Color.ForestGreen);
            colorDic.Add("Fuchsia", Color.Fuchsia);
            colorDic.Add("Gainsboro", Color.Gainsboro);
            colorDic.Add("GhostWhite", Color.GhostWhite);
            colorDic.Add("Gold", Color.Gold);
            colorDic.Add("Goldenrod", Color.Goldenrod);
            colorDic.Add("Gray", Color.Gray);
            colorDic.Add("Green", Color.Green);
            colorDic.Add("GreenYellow", Color.GreenYellow);
            colorDic.Add("Honeydew", Color.Honeydew);
            colorDic.Add("HotPink", Color.HotPink);
            colorDic.Add("IndianRed", Color.IndianRed);
            colorDic.Add("Indigo", Color.Indigo);
            colorDic.Add("Ivory", Color.Ivory);
            colorDic.Add("Khaki", Color.Khaki);
            colorDic.Add("Lavender", Color.Lavender);
            colorDic.Add("LavenderBlush", Color.LavenderBlush);
            colorDic.Add("LawnGreen", Color.LawnGreen);
            colorDic.Add("LemonChiffon", Color.LemonChiffon);
            colorDic.Add("LightBlue", Color.LightBlue);
            colorDic.Add("LightCoral", Color.LightCoral);
            colorDic.Add("LightCyan", Color.LightCyan);
            colorDic.Add("LightGoldenrodYellow", Color.LightGoldenrodYellow);
            colorDic.Add("LightGray", Color.LightGray);
            colorDic.Add("LightGreen", Color.LightGreen);
            colorDic.Add("LightPink", Color.LightPink);
            colorDic.Add("LightSalmon", Color.LightSalmon);
            colorDic.Add("LightSeaGreen", Color.LightSeaGreen);
            colorDic.Add("LightSkyBlue", Color.LightSkyBlue);
            colorDic.Add("LightSlateGray", Color.LightSlateGray);
            colorDic.Add("LightSteelBlue", Color.LightSteelBlue);
            colorDic.Add("LightYellow", Color.LightYellow);
            colorDic.Add("Lime", Color.Lime);
            colorDic.Add("LimeGreen", Color.LimeGreen);
            colorDic.Add("Linen", Color.Linen);
            colorDic.Add("Magenta", Color.Magenta);
            colorDic.Add("Maroon", Color.Maroon);
            colorDic.Add("MediumAquamarine", Color.MediumAquamarine);
            colorDic.Add("MediumBlue", Color.MediumBlue);
            colorDic.Add("MediumOrchid", Color.MediumOrchid);
            colorDic.Add("MediumPurple", Color.MediumPurple);
            colorDic.Add("MediumSeaGreen", Color.MediumSeaGreen);
            colorDic.Add("MediumSlateBlue", Color.MediumSlateBlue);
            colorDic.Add("MediumSpringGreen", Color.MediumSpringGreen);
            colorDic.Add("MediumTurquoise", Color.MediumTurquoise);
            colorDic.Add("MediumVioletRed", Color.MediumVioletRed);
            colorDic.Add("MidnightBlue", Color.MidnightBlue);
            colorDic.Add("MintCream", Color.MintCream);
            colorDic.Add("MistyRose", Color.MistyRose);
            colorDic.Add("Moccasin", Color.Moccasin);
            colorDic.Add("NavajoWhite", Color.NavajoWhite);
            colorDic.Add("Navy", Color.Navy);
            colorDic.Add("OldLace", Color.OldLace);
            colorDic.Add("Olive", Color.Olive);
            colorDic.Add("OliveDrab", Color.OliveDrab);
            colorDic.Add("Orange", Color.Orange);
            colorDic.Add("OrangeRed", Color.OrangeRed);
            colorDic.Add("Orchid", Color.Orchid);
            colorDic.Add("PaleGoldenrod", Color.PaleGoldenrod);
            colorDic.Add("PaleGreen", Color.PaleGreen);
            colorDic.Add("PaleTurquoise", Color.PaleTurquoise);
            colorDic.Add("PaleVioletRed", Color.PaleVioletRed);
            colorDic.Add("PapayaWhip", Color.PapayaWhip);
            colorDic.Add("PeachPuff", Color.PeachPuff);
            colorDic.Add("Peru", Color.Peru);
            colorDic.Add("Pink", Color.Pink);
            colorDic.Add("Plum", Color.Plum);
            colorDic.Add("PowderBlue", Color.PowderBlue);
            colorDic.Add("Purple", Color.Purple);
            colorDic.Add("Red", Color.Red);
            colorDic.Add("RosyBrown", Color.RosyBrown);
            colorDic.Add("RoyalBlue", Color.RoyalBlue);
            colorDic.Add("SaddleBrown", Color.SaddleBrown);
            colorDic.Add("Salmon", Color.Salmon);
            colorDic.Add("SandyBrown", Color.SandyBrown);
            colorDic.Add("SeaGreen", Color.SeaGreen);
            colorDic.Add("SeaShell", Color.SeaShell);
            colorDic.Add("Sienna", Color.Sienna);
            colorDic.Add("Silver", Color.Silver);
            colorDic.Add("SkyBlue", Color.SkyBlue);
            colorDic.Add("SlateBlue", Color.SlateBlue);
            colorDic.Add("SlateGray", Color.SlateGray);
            colorDic.Add("Snow", Color.Snow);
            colorDic.Add("SpringGreen", Color.SpringGreen);
            colorDic.Add("SteelBlue", Color.SteelBlue);
            colorDic.Add("Tan", Color.Tan);
            colorDic.Add("Teal", Color.Teal);
            colorDic.Add("Thistle", Color.Thistle);
            colorDic.Add("Tomato", Color.Tomato);
            //colorDic.Add("Transparent", Color.Transparent);
            colorDic.Add("Turquoise", Color.Turquoise);
            colorDic.Add("Violet", Color.Violet);
            colorDic.Add("Wheat", Color.Wheat);
            colorDic.Add("White", Color.White);
            colorDic.Add("WhiteSmoke", Color.WhiteSmoke);
            colorDic.Add("Yellow", Color.Yellow);
            colorDic.Add("YellowGreen", Color.YellowGreen);

            return colorDic;
        }
    }
}
