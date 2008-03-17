﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
using EcellLib.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo;

namespace EcellLib.PathwayWindow.Graphic
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
            foreach (ComponentSetting setting in canvas.PathwayControl.ComponentManager.ComponentSettings)
                writer.WriteLine(GetGradationBrush(setting));
            // Create SVG objects.
            foreach (PNode node in canvas.PathwayCanvas.Root.ChildrenReference)
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
            float left = system.X - 50f;
            float top = system.Y - 50f;
            float width = system.Width + 100f;
            float height = system.Height + 100f;
            float viewWidth = width * 0.7f;
            float viewHeight = height * 0.7f;
            string header = "<svg xmlns=\"http://www.w3.org/2000/svg\""
            + " width=\"" + viewWidth.ToString() + "\""
            + " height=\"" + viewHeight.ToString() + "\""
            + " viewBox=\"" 
            + left.ToString() + " "
            + top.ToString() + " "
            + width.ToString() + " "
            + height.ToString() + "\">";
            return header;
        }

        private static string GetGradationBrush(ComponentSetting setting)
        {
            string brush = SVGUtil.GradationBrush(
                setting.Name,
                BrushManager.ParseBrushToString(setting.CenterBrush),
                BrushManager.ParseBrushToString(setting.FillBrush));
            return brush;
        }
    }
}