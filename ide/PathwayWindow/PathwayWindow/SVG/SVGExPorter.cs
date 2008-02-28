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
using EcellLib.PathwayWindow.Nodes;

namespace EcellLib.PathwayWindow.SVG
{
    /// <summary>
    /// SVGExPorter
    /// </summary>
    public class SVGExporter
    {
        private const string XML_HEADER = "<?xml version=\"1.0\"?>";
        private const string SVG_HEADER = "<svg xmlns=\"http://www.w3.org/2000/svg\">";
        private const string SVG_FOOTER = "</svg>";
        /// <summary>
        /// Export SVG file.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="filename"></param>
        public static void Export(CanvasControl canvas, string filename)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(XML_HEADER);
            writer.WriteLine(SVG_HEADER);
            foreach (ComponentSetting setting in canvas.PathwayControl.ComponentManager.ComponentSettings)
            {
                writer.WriteLine(GetGradationBrush(setting));
            }
            writer.WriteLine(SVG_FOOTER);
            foreach (PPathwayObject obj in canvas.GetAllObjects())
            {
            }
            writer.Flush();
            writer.Close();

        }
        private static string CreateSystemObject(PPathwayObject system)
        {
            string obj = "";
            string textBrush = BrushManager.ParseBrushToString(system.PText.Brush);
            string lineBrush = BrushManager.ParseBrushToString(system.LineBrush);
            string fillBrush = GetBrushName(system.Setting.Name);
            PointF textPos = new PointF(system.PText.X, system.PText.Y + system.PText.Height);
            obj += SVGUtil.RoundedRectangle(system.Rect, lineBrush, fillBrush);
            obj += SVGUtil.RoundedRectangle(system.Rect, lineBrush, "white");
            obj += SVGUtil.Text(textPos, system.PText.Text, textBrush);
            return obj;
        }

        private static string CreateProcessObject(PPathwayObject Process)
        {
            string obj = "";
            string textBrush = BrushManager.ParseBrushToString(Process.PText.Brush);
            string lineBrush = BrushManager.ParseBrushToString(Process.LineBrush);
            string fillBrush = GetBrushName(Process.Setting.Name);
            PointF textPos = new PointF(Process.PText.X, Process.PText.Y + Process.PText.Height);
            obj += SVGUtil.RoundedRectangle(Process.Rect, lineBrush, fillBrush) + "\n";
            obj += SVGUtil.Text(textPos, Process.PText.Text, textBrush) + "\n";
            return obj;
        }

        private static string CreateVariableObject(PPathwayObject variable)
        {
            string obj = "";
            string textBrush = BrushManager.ParseBrushToString(variable.PText.Brush);
            string lineBrush = BrushManager.ParseBrushToString(variable.LineBrush);
            string fillBrush = GetBrushName(variable.Setting.Name);
            PointF textPos = new PointF(variable.PText.X, variable.PText.Y + variable.PText.Height);
            obj += SVGUtil.Ellipse(variable.Rect, lineBrush, fillBrush);
            obj += SVGUtil.Text(textPos, variable.PText.Text, textBrush);
            return obj;
        }

        private static string GetBrushName(string settingName)
        {
            string gradationBrush = "url(#" + settingName + ")";
            return gradationBrush;
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
