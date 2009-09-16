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
using System.Drawing;
using System.Drawing.Drawing2D;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Event;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Graph object for Pathway
    /// </summary>
    public class PPathwayGraph : PPathwayNode
    {
        #region Fields
        /// <summary>
        /// Default Graph size.
        /// </summary>
        public const float GRAPH_SIZE = 120f;
        /// <summary>
        /// Max number of plots.
        /// </summary>
        public const int MAX_COUNT = 100;
        /// <summary>
        /// List of Values.
        /// </summary>
        private List<Plot> m_values = new List<Plot>();
        /// <summary>
        /// Panel object.
        /// </summary>
        private PPathwayNode m_panel = null;
        /// <summary>
        /// 
        /// </summary>
        private PPathwayNode m_graph = null;
        /// <summary>
        /// 
        /// </summary>
        private string m_entityPath = null;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public PText PText
        {
            get { return m_pText; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get { return m_pText.Text; }
            set { m_pText.Text = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EntityPath
        {
            get { return m_entityPath; }
            set { m_entityPath = value;}
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Plot> Plots
        {
            get { return m_values; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal PPathwayNode Panel
        {
            get { return m_panel; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal PPathwayNode Graph
        {
            get { return m_graph; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayGraph()
        {
            // Allow Drag event.
            this.AddInputEventListener(new PDragEventHandler());

            // Draw Rect
            this.Brush = Brushes.LightBlue;
            this.Pen = new Pen(Brushes.Black);
            this.Width = GRAPH_SIZE + 20;
            this.Height = GRAPH_SIZE + 30;
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0, 0, GRAPH_SIZE + 20, GRAPH_SIZE + 30));
            this.AddPath(path, false);

            // Draw Graph Panel
            this.m_panel = new PPathwayNode();
            this.m_panel.Pickable = false;
            this.m_panel.Brush = Brushes.White;
            this.m_panel.Pen = new Pen(Brushes.Black);
            path = new GraphicsPath();
            path.AddRectangle(new RectangleF(20, 10, GRAPH_SIZE, GRAPH_SIZE));
            this.m_panel.AddPath(path, false);
            this.AddChild(m_panel);

            // Draw Graph
            this.m_graph = new PPathwayNode();
            this.m_graph.Pickable = false;
            this.m_graph.Pen = new Pen(Brushes.Red);
            this.AddChild(m_graph);

            // Draw Title
            m_pText = new PText();
            m_pText.Text = "Title";
            m_pText.Pickable = false;
            this.AddChild(m_pText);

            Refresh();
        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        public void SetValue(double value, double time)
        {
            if (m_values.Count > 0 && time == m_values[m_values.Count - 1].X)
                return;
            if (double.IsNaN(value))
                value = 0;
            m_values.Add(new Plot(time,value));
            if (m_values.Count > MAX_COUNT)
                m_values.RemoveAt(0);

            DrawGraph();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawGraph()
        {
            // Set max and min X
            double maxX = m_values[0].X;
            double minX = m_values[0].X;
            // Set max and min Y
            double maxY = m_values[0].Y;
            double minY = m_values[0].Y;
            foreach (Plot plot in m_values)
            {
                if (plot.X > maxX)
                    maxX = plot.X;
                else if (plot.X < minX)
                    minX = plot.X;
                if (plot.Y > maxY)
                    maxY = plot.Y;
                else if (plot.Y < minY)
                    minY = plot.Y;
            }
            // set rate
            double rateX = GRAPH_SIZE / (maxX - minX) * m_values.Count / MAX_COUNT;
            if (maxX == minX)
                rateX = 1;
            double rateY = GRAPH_SIZE / (maxY - minY);
            if (maxY == minY)
                rateY = 1;

            // create plots
            int i = 0;
            List<PointF> plots = new List<PointF>();
            foreach (Plot val in m_values)
            {
                PointF plot = new PointF();
                // X
                plot.X = m_panel.X + (float)((val.X - minX) * rateX);
                // Y 
                plot.Y = m_panel.Y + GRAPH_SIZE - (float)((val.Y - minY) * rateY);
                plots.Add(plot);
                i++;
            }
            if (plots.Count < 2)
                return;

            GraphicsPath path = new GraphicsPath();
            path.AddLines(plots.ToArray());
            m_graph.AddPath(path, false);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            m_panel.X = this.X + 10;
            m_panel.Y = this.Y + 20;
            m_pText.X = this.X + 20;
            m_pText.Y = this.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            m_values.Clear();
            m_panel.Reset();
        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public struct Plot
    {
        /// <summary>
        /// 
        /// </summary>
        public double X;
        /// <summary>
        /// 
        /// </summary>
        public double Y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Plot(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
