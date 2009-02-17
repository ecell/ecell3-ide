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
using System.Drawing;
using System.IO;
using System.Text;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using Ecell.IDE.Plugins.PathwayWindow.Components;

namespace Ecell.IDE.Plugins.PathwayWindow.Graphic
{
    /// <summary>
    /// SVGExPorter
    /// </summary>
    public class GraphicsExporter
    {
        private const string XML_HEADER = "<?xml version=\"1.0\"?>";
        private const string SVG_FOOTER = "</svg>";
        /// <summary>
        /// Export SVG file.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="filename"></param>
        public static void ExportSVG(CanvasControl canvas, string filename)
        {
            // Start writing SVG format.
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(XML_HEADER);
            writer.WriteLine(CreateSVGHeader(canvas));
            // Create brushes.
            foreach (ComponentSetting setting in canvas.Control.ComponentManager.ComponentSettings)
                writer.WriteLine(GetGradationBrush(setting));
            // Create SVG objects.
            foreach (PNode node in canvas.PCanvas.Root.ChildrenReference)
            {
                if (!(node is PPathwayLayer) || !node.Visible)
                    continue;
                PPathwayLayer layer = (PPathwayLayer)node;
                foreach (PPathwayObject obj in layer.GetNodes())
                    writer.WriteLine(CreateSVGObject(obj));
            }
            // Close SVG file.
            writer.WriteLine(SVG_FOOTER);
            writer.Flush();
            writer.Close();
        }

        private static string CreateSVGObject(PPathwayObject obj)
        {
            string svgObj = "<!--" + obj.EcellObject.Key + "-->\n";
            // Check Visibility.
            if (!obj.Visible)
                return svgObj;

            // Create object brush.
            string textBrush = BrushManager.ParseBrushToString(obj.Setting.TextBrush);
            string lineBrush = BrushManager.ParseBrushToString(obj.Setting.LineBrush);
            string fillBrush = "url(#" + obj.Setting.Name + ")";

            // Create Process
            if (obj is PPathwayProcess)
            {
                PPathwayProcess process = (PPathwayProcess)obj;
                foreach (PPathwayLine line in process.Relations)
                    if (line.Visible)
                        svgObj += CreateSVGLine(line);
                if (process.ViewMode)
                {
                    svgObj += SVGUtil.Ellipse(process.Rect, lineBrush, fillBrush);
                    return svgObj;
                }
            }

            svgObj += obj.Figure.CreateSVGObject(obj.Rect, lineBrush, fillBrush);
            
            // Create NodeText
            if (obj.ShowingID)
            {
                PText text = obj.PText;
                PointF textPos = new PointF(text.X, text.Y + SVGUtil.SVG_FONT_SIZE);
                if (obj is PPathwayText)
                    svgObj += SVGUtil.Text(textPos, text.Text, textBrush, "", text.Font.Size);
                else
                    svgObj += SVGUtil.Text(textPos, text.Text, textBrush);
            }

            // Create Node Parameter.
            if (obj is PPathwayNode)
            {
                PText propText = ((PPathwayNode)obj).PPropertyText;
                if (propText.Visible && !string.IsNullOrEmpty(propText.Text))
                {
                    PointF pos = new PointF(propText.X, propText.Y + SVGUtil.SVG_FONT_SIZE);
                    svgObj += SVGUtil.Text(pos, propText.Text, BrushManager.ParseBrushToString(propText.TextBrush), "", SVGUtil.SVG_FONT_SIZE);
                }
            }
            return svgObj;
        }

        private static string CreateSVGLine(PPathwayLine line)
        {
            string obj = "";
            string brush = BrushManager.ParseBrushToString(line.Brush);
            string width = line.Pen.Width.ToString();
            PointF proPoint = line.ProPoint;
            PointF varPoint = line.VarPoint;
            switch (line.Info.LineType)
            {
                case LineType.Solid:
                case LineType.Unknown:
                    obj += SVGUtil.Line(proPoint, varPoint, brush, width);
                    break;
                case LineType.Dashed:
                    obj += SVGUtil.DashedLine(proPoint, varPoint, brush, width);
                    break;
            }
            switch (line.Info.Direction)
            {
                case EdgeDirection.Bidirection:
                    obj += SVGUtil.Polygon(PPathwayLine.GetArrowPoints(proPoint, varPoint), brush, width);
                    obj += SVGUtil.Polygon(PPathwayLine.GetArrowPoints(varPoint, proPoint), brush, width);
                    break;
                case EdgeDirection.Inward:
                    obj += SVGUtil.Polygon(PPathwayLine.GetArrowPoints(proPoint, varPoint), brush, width);
                    break;
                case EdgeDirection.Outward:
                    obj += SVGUtil.Polygon(PPathwayLine.GetArrowPoints(varPoint, proPoint), brush, width);
                    break;
                case EdgeDirection.None:
                    break;
            }
            return obj;
        }

        private static string CreateSVGHeader(CanvasControl canvas)
        {
            PPathwaySystem system = canvas.Systems[Constants.delimiterPath];
            RectangleF rect = system.Rect;
            rect.X -= 50f;
            rect.Y -= 50f;
            rect.Width += 100f;
            rect.Height += 100f;
            float viewWidth = rect.Width * 0.7f;
            float viewHeight = rect.Height * 0.7f;
            string header = "<svg xmlns=\"http://www.w3.org/2000/svg\""
            + " width=\"" + viewWidth.ToString() + "\""
            + " height=\"" + viewHeight.ToString() + "\""
            + " viewBox=\""
            + rect.X.ToString() + " "
            + rect.Y.ToString() + " "
            + rect.Width.ToString() + " "
            + rect.Height.ToString() + "\">\n";
            // Set BackGround
            string brush = BrushManager.ParseBrushToString(canvas.BackGroundBrush);
            header += SVGUtil.Rectangle(rect, brush, brush);
            return header;
        }

        private static string GetGradationBrush(ComponentSetting setting)
        {
            string fillBrush = BrushManager.ParseBrushToString(setting.FillBrush);
            string centerBrush = BrushManager.ParseBrushToString(setting.FillBrush);
            if (setting.IsGradation)
                centerBrush = BrushManager.ParseBrushToString(setting.CenterBrush);
            string brush = SVGUtil.GradationBrush(
                setting.Name,
                centerBrush,
                fillBrush);
            return brush;
        }
    }
}
