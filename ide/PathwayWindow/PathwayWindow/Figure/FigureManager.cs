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
using System.Text;

namespace EcellLib.PathwayWindow.Figure
{
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
            switch (type)
            {
                case EllipseFigure.TYPE:
                    return new EllipseFigure(StringToFloats(args));
                case RectangleFigure.TYPE:
                    return new RectangleFigure(StringToFloats(args));
                case RoundedRectangle.TYPE:
                    return new RoundedRectangle(StringToFloats(args));
                case SystemRectangle.TYPE:
                    return new SystemRectangle(StringToFloats(args));
                default:
                    return new FigureBase(StringToFloats(args));
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
            list.Add(RoundedRectangle.TYPE);
            list.Add(SystemRectangle.TYPE);
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

    public enum FigureType
    {
        FigureBase = 0,
        EllipseFigure = 1,
        RectangleFigure = 2,
        RoundedRectangle = 3,
        SystemRectangle = 4
    }
}
