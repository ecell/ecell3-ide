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

using System.Drawing;
using System.IO;
using Ecell.IDE.Plugins.PathwayWindow.Components;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using System.Collections.Generic;

namespace Ecell.IDE.Plugins.PathwayWindow.Graphics
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
            foreach (ComponentSetting setting in canvas.Control.ComponentManager.GetAllSettings())
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
            // Set key
            string key;
            if (obj is PPathwayAlias)
                key = ((PPathwayAlias)obj).Variable.EcellObject.Key;
            else
                key = obj.EcellObject.Key;

            string svgObj = "<!--" + key + "-->\n";
            // Check Visibility.
            if (!obj.Visible)
                return svgObj;

            // Create object brush.
            string textBrush = BrushManager.ParseBrushToString(obj.Setting.TextBrush);
            string lineBrush = BrushManager.ParseBrushToString(obj.Setting.LineBrush);
            string fillBrush = "url(#" + obj.Setting.Name + ")";

            // Create Edges
            if (obj is PPathwayProcess)
            {
                PPathwayProcess process = (PPathwayProcess)obj;
                foreach (PPathwayEdge line in process.Relations)
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
            if (obj is PPathwayEntity)
            {
                svgObj += CreateSVGProperties((PPathwayEntity)obj);
                svgObj += CreateSVGGraph((PPathwayEntity)obj);
            }
            return svgObj;
        }

        private static string CreateSVGGraph(PPathwayEntity entity)
        {
            string svgObj = "";
            PPathwayGraph graph = entity.Graph;
            if (graph == null || !graph.Visible)
                return svgObj;
            string pen = BrushManager.ParseBrushToString(graph.Pen.Brush);
            string fill = BrushManager.ParseBrushToString(graph.Brush);
            svgObj += SVGUtil.Rectangle(graph.Rect, pen, fill);
            //
            fill = BrushManager.ParseBrushToString(graph.Panel.Brush);
            svgObj += SVGUtil.Rectangle(graph.Panel.Rect, pen, fill);
            //
            pen = BrushManager.ParseBrushToString(graph.Graph.Brush);
            svgObj += SVGUtil.Polygon(graph.Graph.Path.PathPoints, pen, "1");

            return svgObj;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string CreateSVGProperties(PPathwayEntity entity)
        {
            string svgObj = "";
            float margin = 2f;
            PPathwayProperties properties = entity.Property;
            if (!properties.Visible)
                return svgObj;

            foreach (PPathwayProperty property in entity.Property.Properties)
            {
                if (!property.Visible)
                    continue;
                string pen = BrushManager.ParseBrushToString(Brushes.Black);
                string fill = BrushManager.ParseBrushToString(property.Brush);
                svgObj += SVGUtil.Rectangle(property.Rect, pen, fill);
                svgObj += SVGUtil.Line(new PointF(property.value.X, property.value.Y), new PointF(property.value.X, property.value.Y + property.Height), pen, "1");
                svgObj += SVGUtil.Text(new PointF(property.label.X + margin, property.label.Y + SVGUtil.SVG_FONT_SIZE + margin), property.Label, pen, "", SVGUtil.SVG_FONT_SIZE);
                svgObj += SVGUtil.Text(new PointF(property.value.X + margin, property.value.Y + SVGUtil.SVG_FONT_SIZE + margin), property.Value, pen, "", SVGUtil.SVG_FONT_SIZE);
            }
            return svgObj;
        }

        private static string CreateSVGLine(PPathwayEdge line)
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
                    obj += SVGUtil.Polygon(PPathwayEdge.GetArrowPoints(proPoint, varPoint), brush, width);
                    obj += SVGUtil.Polygon(PPathwayEdge.GetArrowPoints(varPoint, proPoint), brush, width);
                    break;
                case EdgeDirection.Inward:
                    obj += SVGUtil.Polygon(PPathwayEdge.GetArrowPoints(proPoint, varPoint), brush, width);
                    break;
                case EdgeDirection.Outward:
                    obj += SVGUtil.Polygon(PPathwayEdge.GetArrowPoints(varPoint, proPoint), brush, width);
                    break;
                case EdgeDirection.None:
                    break;
            }
            return obj;
        }

        private static string CreateSVGHeader(CanvasControl canvas)
        {
            // Set root system's rect.
            PPathwaySystem system = canvas.Systems[Constants.delimiterPath];
            RectangleF rect = system.Rect;

            // Update position
            List<PPathwayObject> list = canvas.GetAllObjects();
            float top = rect.Y;
            float left = rect.X;
            float right = rect.X;
            float bottom = rect.Y;
            foreach (PPathwayObject obj in list)
            {
                //
                if (obj.Top < top)
                    top = obj.Top;
                //
                if (obj.Left < left)
                    left = obj.Left;
                //
                float tmpRight = obj.Right;
                if (obj is PPathwayEntity && ((PPathwayEntity)obj).Property.Visible)
                    tmpRight = ((PPathwayEntity)obj).Property.Right;
                if (tmpRight > right)
                    right = tmpRight;
                // 
                if (obj.Bottom > bottom)
                    bottom = obj.Bottom;
            }
            // Set Top position.
            rect.Y = top;
            // Set Left position.
            rect.X = left;
            // Set Right position.
            rect.Width = right - rect.X;
            // Set Bottom position.
            rect.Height = bottom - rect.Y;

            // Set Margin
            rect.X -= 50f;
            rect.Y -= 50f;
            rect.Width += 100f;
            rect.Height += 100f;


            // Create SVG canvas.
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
            // Set BackGround brush
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
