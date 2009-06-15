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

namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    /// <summary>
    /// FigureManager
    /// </summary>
    public class FigureManager
    {
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
        /// Get Figure list.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFigureList()
        {
            List<string> list = new List<string>();
            list.Add(EllipseFigure.TYPE);
            list.Add(RectangleFigure.TYPE);
            list.Add(DiamondFigure.TYPE);
            list.Add(RoundedRectangle.TYPE);
            list.Add(SystemRectangle.TYPE);
            list.Add(SystemEllipse.TYPE);
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
