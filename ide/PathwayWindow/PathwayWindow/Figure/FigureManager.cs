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
//
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    /// <summary>
    /// FigureManager
    /// </summary>
    public class FigureManager
    {
        private static ImageList figureIcons = null;
        /// <summary>
        /// FigureIcons for FigureComboBox.
        /// </summary>
        public static ImageList FigureIcons
        {
            get
            {
                if (figureIcons == null)
                    figureIcons = CreateFigureIcons();
                return figureIcons;
            }
        }

        /// <summary>
        /// FigureManager to create figure.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IFigure CreateFigure(string type, string args)
        {
            float[] values = StringToFloats(args);
            if (values.Length < 4)
                values = new float[] { 0,0,1,1};

            return CreateFigure(type, values[0], values[1], values[2], values[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static IFigure CreateFigure(string type, float x, float y, float width, float height)
        {
            switch (type)
            {
                case EllipseFigure.TYPE:
                    return new EllipseFigure(x, y, width, height);
                case RectangleFigure.TYPE:
                    return new RectangleFigure(x, y, width, height);
                case DiamondFigure.TYPE:
                    return new DiamondFigure(x, y, width, height);
                case TriangleFigure.TYPE:
                    return new TriangleFigure(x, y, width, height);
                case RoundedRectangle.TYPE:
                    return new RoundedRectangle(x, y, width, height);
                case SystemRectangle.TYPE:
                    return new SystemRectangle(x, y, width, height);
                case SystemEllipse.TYPE:
                    return new SystemEllipse(x, y, width, height);
                default:
                    return new FigureBase(x, y, width, height);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetFigureList(string type)
        {
            if (type.Equals(Constants.xpathSystem))
                return FigureManager.GetSystemFigures();
            else
                return FigureManager.GetEntityFigures();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<string> GetSystemFigures()
        {
            List<string> list = new List<string>();
            list.Add(SystemRectangle.TYPE);
            list.Add(SystemEllipse.TYPE);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<string> GetEntityFigures()
        {
            List<string> list = new List<string>();
            list.Add(EllipseFigure.TYPE);
            list.Add(RectangleFigure.TYPE);
            list.Add(DiamondFigure.TYPE);
            list.Add(TriangleFigure.TYPE);
            list.Add(RoundedRectangle.TYPE);
            return list;
        }

        /// <summary>
        /// Get Figure list.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFigureList()
        {
            List<string> list = new List<string>();
            list.AddRange(GetEntityFigures());
            list.AddRange(GetSystemFigures());
            return list;
        }

        /// <summary>
        /// Create a list of Figure icons.
        /// </summary>
        /// <returns></returns>
        private static ImageList CreateFigureIcons()
        {
            ImageList list = new ImageList();
            foreach (string type in GetFigureList())
            {
                IFigure figure = CreateFigure(type, 0, 0, 150, 150);
                GraphicsPath gp = figure.GraphicsPath;
                Image image = new Bitmap(160, 160);
                System.Drawing.Graphics gra = System.Drawing.Graphics.FromImage(image);
                gra.FillPath(Brushes.Orange, gp);
                gra.DrawPath(new Pen(Brushes.Orange, 0), gp);

                Image icon = new Bitmap(image, 16, 16);
                list.Images.Add(type, image);
            }
            return list;
        }

        /// <summary>
        /// Change string to float array.
        /// </summary>
        /// <param name="argString"></param>
        /// <returns></returns>
        private static float[] StringToFloats(string argString)
        {
            string[] args = argString.Split(new Char[] { ',', ' ' });
            float[] values = new float[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                values[i] = float.Parse(args[i]);
            }
            return values;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum FigureType
    {
        /// <summary>
        /// 
        /// </summary>
        FigureBase = 0,
        /// <summary>
        /// 
        /// </summary>
        EllipseFigure = 1,
        /// <summary>
        /// 
        /// </summary>
        RectangleFigure = 2,
        /// <summary>
        /// 
        /// </summary>
        RoundedRectangle = 3,
        /// <summary>
        /// 
        /// </summary>
        SystemRectangle = 4
    }
}
