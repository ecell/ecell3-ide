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
        public const float GRAPH_SIZE = 150f;
        /// <summary>
        /// Max number of plots.
        /// </summary>
        public const int MAX_COUNT = 20;
        /// <summary>
        /// List of Values.
        /// </summary>
        private List<double> m_values = new List<double>();
        /// <summary>
        /// Graph object.
        /// </summary>
        private PPathwayNode m_graph = null;
        /// <summary>
        /// 
        /// </summary>
        private PText m_text = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayGraph()
        {
            this.Brush = Brushes.LightBlue;
            this.Pen = new Pen(Brushes.Black);
            this.Width = GRAPH_SIZE;
            this.Height = GRAPH_SIZE;
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0, 0, GRAPH_SIZE, GRAPH_SIZE));
            this.AddPath(path, false);

            this.m_graph = new PPathwayNode();
            this.m_graph.Pickable = false;
            this.m_graph.Brush = Brushes.White;
            this.m_graph.Pen = new Pen(Brushes.Black);
            path = new GraphicsPath();
            path.AddRectangle(new RectangleF(20, 20, GRAPH_SIZE-30, GRAPH_SIZE-30));
            this.m_graph.AddPath(path, false);

            this.AddChild(m_graph);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public PPathwayGraph(PPathwayEntity entity)
            : this()
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(double value)
        {
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
            double rate = 150d / (max - min);

            // create plots
            int i = 0;
            float x = 7.5f;
            List<PointF> plots = new List<PointF>();
            foreach (double val in m_values)
            {
                PointF plot = new PointF();
                plot.X = 3.75f + x * i;
                plot.Y = GRAPH_SIZE - (float)((val - min) * rate);
                plots.Add(plot);
            }
            GraphicsPath path = new GraphicsPath();
            path.AddBeziers(plots.ToArray());
            m_graph.AddPath(path, false);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            m_values.Clear();
            m_graph.Reset();
        }
        #endregion

    }
}
