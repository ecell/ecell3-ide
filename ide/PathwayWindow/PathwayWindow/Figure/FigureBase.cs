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
using System.Drawing.Drawing2D;

namespace EcellLib.PathwayWindow.Figure
{
    /// <summary>
    /// Concrete classe which extended FigureBase stands for 
    /// </summary>
    public abstract class FigureBase
    {
        #region Fields
        /// <summary>
        /// X coordinate of this figure.
        /// </summary>
        protected float m_x = 0;

        /// <summary>
        /// Y coordinate of this figure.
        /// </summary>
        protected float m_y = 0;

        /// <summary>
        /// Width of this figure.
        /// </summary>
        protected float m_width = 0;

        /// <summary>
        /// Height of this figure.
        /// </summary>
        protected float m_height = 0;

        /// <summary>
        /// Figure type.
        /// </summary>
        protected string m_type = "";

        /// <summary>
        /// 
        /// </summary>
        protected GraphicsPath m_gp = null;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_gp.
        /// </summary>
        public GraphicsPath GraphicsPath
        {
            get { return m_gp; }
        }

        /// <summary>
        /// type string.
        /// </summary>
        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// type string.
        /// </summary>
        public string Coordinates
        {
            get
            {
                string coordinates = m_x.ToString() + "," + m_y.ToString() + "," + m_width + "," + m_height.ToString();
                return coordinates;
            }
        }

        /// <summary>
        /// Accessor for resized m_gp for being used as ToolBox item.
        /// </summary>
        public GraphicsPath TransformedPath
        {
            get
            {
                GraphicsPath transPath = (GraphicsPath)m_gp.Clone();
                RectangleF rect = m_gp.GetBounds();
                Matrix matrix = new Matrix();

                matrix.Translate(-1f * (rect.X + rect.Width / 2f),
                                 -1f * (rect.Y + rect.Height / 2f));

                transPath.Transform(matrix);

                matrix = new Matrix();
                if (rect.Width > rect.Height)
                {
                    matrix.Scale(240f / rect.Width, 240f / rect.Width);
                    matrix.Translate(128f * rect.Width / 256f, 128f * rect.Width / 256f);
                }
                else
                {
                    matrix.Scale(240f / rect.Height, 240f / rect.Height);
                    matrix.Translate(128f * rect.Height / 256f, 128f * rect.Height / 256f);
                }

                transPath.Transform(matrix);
                return transPath;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public FigureBase()
        {
            m_gp = new GraphicsPath();
        }

        /// <summary>
        /// Create figure.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static FigureBase CreateFigure(string type, string args)
        {
            switch (type)
            {
                case "Ellipse":
                    return new EllipseFigure(StringToFloats(args));
                case "Rectangle":
                    return new RectangleFigure(StringToFloats(args));
                case "RoundCornerRectangle":
                    return new EllipseFigure(StringToFloats(args));
                default:
                    return null;
            }
        }
        #endregion

        /// <summary>
        /// Change string to float array.
        /// </summary>
        /// <param name="argString"></param>
        /// <returns></returns>
        protected static float[] StringToFloats(string argString)
        {
            string[] args = argString.Split(new Char[] { ',', ' ' });
            float[] values = new float[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                values[i] = float.Parse(args[i]);
            }
            return values;
        }

        /// <summary>
        /// Return a contact point between an outer point and an inner point.
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public abstract PointF GetContactPoint(PointF outerPoint, PointF innerPoint);

        #region GraphicsPath
        /// <summary>
        /// Add arc.
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddArc(float[] vars)
        {
            if (vars.Length < 6)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddArc(vars[0],
                            vars[1],
                            vars[2],
                            vars[3],
                            vars[4],
                            vars[5]);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add bezier curve.
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddBezier(float[] vars)
        {
            if (vars.Length < 8)
                return ErrorType.Error_LessArgs;

            try
            {
                PointF p1 = new PointF(vars[0], vars[1]);
                PointF p2 = new PointF(vars[2], vars[3]);
                PointF p3 = new PointF(vars[4], vars[5]);
                PointF p4 = new PointF(vars[6], vars[7]);
                m_gp.AddBezier(p1, p2, p3, p4);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add bezier curves
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddBeziers(float[] vars)
        {
            if (vars.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = vars.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(vars[m], vars[m + 1]);
                m_gp.AddBeziers(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add closed curve
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddClosedCurve(float[] vars)
        {
            if (vars.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = vars.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(vars[m], vars[m + 1]);
                m_gp.AddClosedCurve(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add curve
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddCurve(float[] vars)
        {
            if (vars.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = vars.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(vars[m], vars[m + 1]);
                m_gp.AddCurve(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add ellipse
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddEllipse(float[] vars)
        {
            if (vars.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                RectangleF rect = new RectangleF(vars[0],
                                                  vars[1],
                                                  vars[2],
                                                  vars[3]);
                m_gp.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add line
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddLine(float[] vars)
        {
            if (vars.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddLine(vars[0],
                             vars[1],
                             vars[2],
                             vars[3]);
                m_gp.CloseFigure();
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add lines
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddLines(float[] vars)
        {
            if (vars.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = vars.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(vars[m], vars[m + 1]);
                m_gp.AddLines(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add pie
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddPie(float[] vars)
        {
            if (vars.Length < 6)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddPie(vars[0],
                            vars[1],
                            vars[2],
                            vars[3],
                            vars[4],
                            vars[5]);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add polygon
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddPolygon(float[] vars)
        {
            if (vars.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = vars.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(vars[m], vars[m + 1]);
                m_gp.AddPolygon(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add rectangle
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddRectangle(float[] vars)
        {
            if (vars.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                RectangleF rect = new RectangleF(vars[0],
                                vars[1],
                                vars[2],
                                vars[3]);
                m_gp.AddRectangle(rect);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add RoundCornerRectangle
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        protected ErrorType AddRoundCornerRectangle(float[] vars)
        {
            if (vars.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                RectangleF rect = new RectangleF(vars[0],
                                vars[1],
                                vars[2],
                                vars[3]);
                m_gp.AddRectangle(rect);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        #endregion
    }

    #region Enums
    /// <summary>
    /// Type of errors which are returned by methods of this class.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// not an error.
        /// </summary>
        No_Error,
        /// <summary>
        /// figure is null
        /// </summary>
        Error_FigureNull,
        /// <summary>
        /// argument is null
        /// </summary>
        Error_ArgsNull,
        /// <summary>
        /// a figure doesn't exist
        /// </summary>
        Error_NoSuchFigure,
        /// <summary>
        /// some argument is lost
        /// </summary>
        Error_LessArgs,
        /// <summary>
        /// format is illegal
        /// </summary>
        Error_IllegalFormat
    };
    #endregion
}