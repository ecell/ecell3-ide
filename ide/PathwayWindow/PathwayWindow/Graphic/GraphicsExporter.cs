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
                foreach(PPathwayObject obj in layer.NodeList)
                    writer.WriteLine(obj.CreateSVGObject());
            }
            // Close SVG file.
            writer.WriteLine(SVG_FOOTER);
            writer.Flush();
            writer.Close();
        }

        private static string CreateSVGHeader(CanvasControl canvas)
        {
            PPathwaySystem system = canvas.Systems["/"];
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
