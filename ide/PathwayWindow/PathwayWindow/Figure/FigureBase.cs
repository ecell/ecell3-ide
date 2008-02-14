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
        /// GraphicsPath
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

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public FigureBase()
        {
            m_gp = new GraphicsPath();
        }
        
        #endregion

        #region Methods
        /// <summary>
        /// Accessor for resized m_gp for being used as ToolBox item.
        /// </summary>
        public GraphicsPath TransformedPath
        {
            get
            {
                float width = 32;
                float height = 32;
                GraphicsPath transPath = (GraphicsPath)m_gp.Clone();
                RectangleF rect = m_gp.GetBounds();
                Matrix matrix = new Matrix();

                matrix.Translate(-1f * (rect.X + rect.Width / 2f),
                                 -1f * (rect.Y + rect.Height / 2f));

                transPath.Transform(matrix);

                matrix = new Matrix();
                if (rect.Width > rect.Height)
                {
                    matrix.Scale(30f / rect.Width, 30f / rect.Width);
                    matrix.Translate(rect.Width / 2f, rect.Width / 2f);
                }
                else
                {
                    matrix.Scale(30f / rect.Height, 30f / rect.Height);
                    matrix.Translate(rect.Height / 2f, rect.Height / 2f);
                }

                transPath.Transform(matrix);
                return transPath;
            }
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
                    return new RoundCornerRectangle(StringToFloats(args));
                default:
                    return null;
            }
        }

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

        #endregion

    }
}