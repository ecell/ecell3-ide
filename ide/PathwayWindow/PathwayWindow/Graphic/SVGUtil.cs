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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EcellLib.PathwayWindow.Graphic
{
    /// <summary>
    /// SVGUtil
    /// This class is to create SVG object.
    /// </summary>
    public class SVGUtil
    {
        /// <summary>
        /// Create Rectangle object.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="lineBrush"></param>
        /// <param name="fillBrush"></param>
        /// <returns></returns>
        public static string Rectangle(RectangleF rect, string lineBrush, string fillBrush)
        {
            string obj = "<rect x=\"" + rect.X.ToString()
            + "\" y=\"" + rect.Y.ToString()
            + "\" width=\"" + rect.Width.ToString()
            + "\" height=\"" + rect.Height.ToString()
            + "\" fill=\"" + fillBrush
            + "\" stroke=\"" + lineBrush
            + "\" stroke-width=\"1\"/>\n";
            return obj;
        }
        /// <summary>
        /// Create RoundedRectangle object.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="lineBrush"></param>
        /// <param name="fillBrush"></param>
        /// <returns></returns>
        public static string RoundedRectangle(RectangleF rect, string lineBrush, string fillBrush)
        {
            float rx = rect.Width * 0.1f;
            float ry = rect.Height * 0.1f;
            return RoundedRectangle(rect, lineBrush, fillBrush, rx, ry);
        }
        /// <summary>
        /// Create RoundedRectangle object.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="lineBrush"></param>
        /// <param name="fillBrush"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <returns></returns>
        public static string RoundedRectangle(RectangleF rect, string lineBrush, string fillBrush, float rx, float ry)
        {
            string obj = "<rect x=\"" + rect.X.ToString()
            + "\" y=\"" + rect.Y.ToString()
            + "\" rx=\"" + rx.ToString()
            + "\" ry=\"" + ry.ToString()
            + "\" width=\"" + rect.Width.ToString()
            + "\" height=\"" + rect.Height.ToString()
            + "\" fill=\"" + fillBrush
            + "\" stroke=\"" + lineBrush
            + "\" stroke-width=\"1\"/>\n";
            return obj;
        }
        /// <summary>
        /// Create Ellipse object.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="lineBrush"></param>
        /// <param name="fillBrush"></param>
        /// <returns></returns>
        public static string Ellipse(RectangleF rect, string lineBrush, string fillBrush)
        {
            float rx = rect.Width / 2f;
            float ry = rect.Height / 2f;
            float cx = rect.X + rx;
            float cy = rect.Y + ry;
            string obj = "<ellipse cx=\"" + cx.ToString()
            + "\" cy=\"" + cy.ToString()
            + "\" rx=\"" + rx.ToString()
            + "\" ry=\"" + ry.ToString()
            + "\" fill=\"" + fillBrush
            + "\" stroke=\"" + lineBrush
            + "\" stroke-width=\"1\"/>\n";
            return obj;
        }
        /// <summary>
        /// Create Line object.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static string Line(PointF start, PointF end, string brush)
        {
            string obj = "<line x1=\"" + start.X.ToString()
            + "\" y1=\"" + start.Y.ToString()
            + "\" x2=\"" + end.X.ToString()
            + "\" y2=\"" + end.Y.ToString()
            + "\" stroke=\"" + brush
            + "\" stroke-width=\"1\"/>\n";
            return obj;
        }
        /// <summary>
        /// Create DashedLine object.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static string DashedLine(PointF start, PointF end, string brush)
        {
            string obj = "<line x1=\"" + start.X.ToString()
            + "\" y1=\"" + start.Y.ToString()
            + "\" x2=\"" + end.X.ToString()
            + "\" y2=\"" + end.Y.ToString()
            + "\" stroke-dasharray=\"5"
            + "\" stroke=\"" + brush
            + "\" stroke-width=\"1\"/>\n";
            return obj;
        }
        /// <summary>
        /// Create Poligon object.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static string Polygon(PointF[] points, string brush)
        {
            string obj = "<polygon stroke=\"" + brush
            + "\" stroke-width=\"1"
            + "\" fill=\"" + brush
            + "\" points=\"";
            for ( int i = 0; i < points.Length; i++)
            {
                PointF point = points[i];
                obj += point.X.ToString() + " " + point.Y.ToString();
                if (i != points.Length - 1)
                    obj += ",";
            }
            obj += "\"/>\n";
            return obj;
        }
        /// <summary>
        /// Create Text object.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="text"></param>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static string Text(PointF point, string text, string brush)
        {
            string obj = "<text x=\"" + point.X.ToString()
            + "\" y=\"" + point.Y.ToString()
            + "\" font-family=\"MS-Gothic"
            + "\" font-size=\"16"
            + "\" font-weight=\"bold"
            + "\" fill=\"" + brush
            + "\">" + text + "</text>\n";
            return obj;
        }
        /// <summary>
        /// Create GradationBrush object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="centerBrush"></param>
        /// <param name="roundBrush"></param>
        /// <returns></returns>
        public static string GradationBrush(string name, string centerBrush, string roundBrush)
        {
            string obj = "<radialGradient id=\"" + name + "\">\n";
            obj += "<stop offset=\"0%\" style=\"stop-color: " + centerBrush + "\"/>\n";
            obj += "<stop offset=\"100%\" style=\"stop-color: " + roundBrush + "\"/>\n";
            obj += "</radialGradient>\n";
            return obj;
        }
    }
}
