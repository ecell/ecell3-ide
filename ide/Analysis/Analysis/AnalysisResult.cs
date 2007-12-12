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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ZedGraph;

namespace EcellLib.Analysis
{
    public partial class AnalysisResult : Form
    {

        #region Fields
        /// <summary>
        /// UserControl to plot the data.
        /// </summary>
        private ZedGraphControl m_zCnt;
        #endregion

        /// <summary>
        /// Constructor for AnalysisResult.
        /// </summary>
        public AnalysisResult()
        {
            InitializeComponent();

            m_zCnt = new ZedGraphControl();
            m_zCnt.Dock = DockStyle.Fill;
            m_zCnt.GraphPane.Title.Text = "";
            m_zCnt.GraphPane.XAxis.Title.Text = "X";
            m_zCnt.GraphPane.YAxis.Title.Text = "Y";
            m_zCnt.GraphPane.Legend.IsVisible = false;
            m_zCnt.GraphPane.XAxis.Scale.Max = 100;
            m_zCnt.GraphPane.XAxis.Scale.Min = 0;
            m_zCnt.GraphPane.YAxis.Scale.Max = 100;
            m_zCnt.GraphPane.YAxis.Scale.Min = 0;

            tableLayoutPanel1.Controls.Add(m_zCnt, 0, 0);
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Set the information on x axis in plot region.
        /// </summary>
        /// <param name="tmpStr">Text on X axis.</param>
        /// <param name="max">Max value on X axis.</param>
        /// <param name="min">Min value on X axis.</param>
        public void ChangeXAxis(String tmpStr, double max, double min)
        {
            m_zCnt.GraphPane.XAxis.Title.Text = tmpStr;
            dataGridView1.Columns[0].HeaderText = tmpStr;
            m_zCnt.GraphPane.XAxis.Scale.Max = max;
            m_zCnt.GraphPane.XAxis.Scale.Min = min;
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Set the information on y axis in plot region.
        /// </summary>
        /// <param name="tmpStr">Text on Y axis.</param>
        /// <param name="max">Max value on Y axis.</param>
        /// <param name="min">Min value on Y axis.</param>
        public void ChangeYAxis(String tmpStr, double max, double min)
        {
            m_zCnt.GraphPane.YAxis.Title.Text = tmpStr;
            dataGridView1.Columns[1].HeaderText = tmpStr;
            m_zCnt.GraphPane.YAxis.Scale.Max = max;
            m_zCnt.GraphPane.YAxis.Scale.Min = min;
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Clear all data on the plot area and DataGrid region.
        /// </summary>
        public void Clear()
        {
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();
            dataGridView1.Rows.Clear();
        }

        /// <summary>
        /// Add the OK data to the plot area and DataGrid region.
        /// </summary>
        /// <param name="x">The value of X axis.</param>
        /// <param name="y">The value of Y axis.</param>
        public void DataAdd(Double x, Double y)
        {
            DataGridViewRow r = new DataGridViewRow();
            
            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = x.ToString();
            r.Cells.Add(c3);
            DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
            c4.Value = y.ToString();
            r.Cells.Add(c4);

            dataGridView1.Rows.Add(r);

            LineItem i = m_zCnt.GraphPane.AddCurve("AAA", new PointPairList(), Color.Red, SymbolType.TriangleDown);
            i.Line.Width = 3;

            i.AddPoint(new PointPair(x, y));

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Add the NG data to the plot area.
        /// </summary>
        /// <param name="x">The value of X axis.</param>
        /// <param name="y">The value of Y axis.</param>
        public void NGDataAdd(Double x, Double y)
        {
            LineItem i = m_zCnt.GraphPane.AddCurve("NG", new PointPairList(), Color.Blue, SymbolType.TriangleDown);
            i.Line.Width = 3;

            i.AddPoint(new PointPair(x, y));

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        # region Events
        /// <summary>
        /// The Event sequence when the close button was clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs</param>
        private void CloseButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}