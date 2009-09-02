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
        private List<double> m_values = new List<double>();
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
        public List<double> Plots
        {
            get { return m_values; }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public PPathwayGraph(PPathwayEntity entity)
            : this()
        {
            m_pText.Text = entity.EcellObject.LocalID;
            // set parameter.
            if(entity is PPathwayProcess)
                this.EntityPath = entity.EcellObject.FullID + ":MolarActivity";
            else if(entity is PPathwayVariable)
                this.EntityPath = entity.EcellObject.FullID + ":MolarConc";
        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(double value)
        {
            if (double.IsNaN(value))
                value = 0;
            m_values.Add(value);
            if (m_values.Count > MAX_COUNT)
                m_values.RemoveAt(0);

            DrawGraph();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawGraph()
        {
            // Set max and min
            double max = m_values[0];
            double min = m_values[0];
            foreach (double val in m_values)
            {
                if (val > max)
                    max = val;
                if (val < min)
                    min = val;
            }
            double rate = GRAPH_SIZE / (max - min);
            if (max == min)
                rate = 1;

            // create plots
            int i = 0;
            float x = (float)(GRAPH_SIZE / MAX_COUNT);
            List<PointF> plots = new List<PointF>();
            foreach (double val in m_values)
            {
                PointF plot = new PointF();
                plot.X = m_panel.X + x * i;
                plot.Y = m_panel.Y + GRAPH_SIZE - (float)((val - min) * rate);
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
}
