//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
// written by Sachio Nohara<nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

using ZedGraph;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// GraphResultWindow
    /// </summary>
    public partial class GraphResultWindow : UserControl
    {
        #region Fields
        /// <summary>
        /// Graph control to display the matrix of analysis result.
        /// </summary>
        private ZedGraphControl m_zCnt = null;
        /// <summary>
        /// The line information of dot plot.
        /// </summary>
        private LineItem m_line;
        private int x_index = 0;
        private int y_index = 0;
        private Analysis m_owner;
        private AnalysisResultWindow m_win;
        private string m_groupName;
        #endregion

        public string GroupName
        {
            get { return this.m_groupName; }
            set { this.m_groupName = value; }
        }


        #region Constructor
        /// <summary>
        /// Constructors
        /// </summary>
        public GraphResultWindow(Analysis owner, AnalysisResultWindow win)
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
            m_zCnt.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(ZedControlContextMenuBuilder);
            RAAnalysisTableLayout.Controls.Add(m_zCnt, 0, 0);
            m_zCnt.AxisChange();
            m_zCnt.Refresh();

            m_owner = owner;
            m_win = win;
        }
        #endregion

        /// <summary>
        /// Draw the result point on graph view,
        /// </summary>
        private void DrawLine(List<PointF> list)
        {
            LineItem line = null;
            Color drawColor = Color.Blue;
            Color styleColor = Color.White;

            line = m_zCnt.GraphPane.AddCurve(
                "Result",
                new PointPairList(),
                drawColor,
                SymbolType.Circle);

            Fill f = new Fill(drawColor);
            line.Symbol.Fill = f;

            line.Line.Width = 5;
            foreach (PointF p in list)
            {
                line.AddPoint(new PointPair(p.X, p.Y));
            }

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Draw the result point on graph view,
        /// </summary>
        /// <param name="x">the position of X.</param>
        /// <param name="y">the position of Y.</param>
        /// <param name="isOK">The flag whether this data is ok.</param>
        private void DrawPoint(double x, double y, bool isOK)
        {
            LineItem line = null;
            Color drawColor = Color.Blue;
            Color styleColor = Color.White;
            if (!isOK)
            {
                drawColor = Color.Red;
                styleColor = Color.Silver;
            }

            line = m_zCnt.GraphPane.AddCurve(
                "Result",
                new PointPairList(),
                drawColor,
                SymbolType.Circle);

            Fill f = new Fill(drawColor);
            line.Symbol.Fill = f;

            line.Line.Width = 5;
            line.AddPoint(new PointPair(x, y));

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="jobid">the jobidof this parameters.</param>
        /// <param name="x">the value of parameter.</param>
        /// <param name="y">the value of parameter.</param>
        /// <param name="isOK">the flag whether this parameter is robustness.</param>
        public void AddJudgementData(int jobid, double x, double y, bool isOK)
        {
            RAXComboBox.Enabled = true;
            RAYComboBox.Enabled = true;

            DrawPoint(x, y, isOK);
        }

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="list">the values of parameter.[List[PointF]]</param>
        public void AddJudgementDataForBifurcation(List<PointF> list)
        {
            RAXComboBox.Enabled = false;
            RAYComboBox.Enabled = false;

            DrawLine(list);
        }

        /// <summary>
        /// Add the judgement data of parameter estimation into graph.
        /// </summary>
        /// <param name="x">the number of generation.</param>
        /// <param name="y">the value of estimation.</param>
        public void AddEstimationData(int x, double y)
        {
            RAXComboBox.Enabled = false;
            RAYComboBox.Enabled = false;

            if (m_line == null)
            {
                m_line = m_zCnt.GraphPane.AddCurve(
                        "Result",
                        new PointPairList(),
                        Color.Blue,
                        SymbolType.Circle);

                Fill f = new Fill(Color.Blue);
                m_line.Symbol.Fill = f;
            }
            m_line.AddPoint(new PointPair(x, y));

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Clear the entries in result data.
        /// </summary>
        public void ClearResult()
        {
            RAXComboBox.Items.Clear();
            RAYComboBox.Items.Clear();

            m_line = null;
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();
        }

        /// <summary>
        /// Set the graph size of result.
        /// </summary>
        /// <param name="xmax">Max value of X axis.</param>
        /// <param name="xmin">Min value of X axis.</param>
        /// <param name="ymax">Max value of Y axis.</param>
        /// <param name="ymin">Min value of Y axis.</param>
        /// <param name="isAutoX">The flag whether X axis is auto scale.</param>
        /// <param name="isAutoY">The flag whether Y axis is auto scale.</param>
        public void SetResultGraphSize(double xmax, double xmin, double ymax, double ymin,
            bool isAutoX, bool isAutoY)
        {
            m_zCnt.GraphPane.XAxis.Scale.Max = xmax;
            m_zCnt.GraphPane.XAxis.Scale.Min = xmin;
            m_zCnt.GraphPane.YAxis.Scale.Max = ymax;
            m_zCnt.GraphPane.YAxis.Scale.Min = ymin;
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = isAutoX;
            m_zCnt.GraphPane.YAxis.Scale.MaxAuto = isAutoY;
        }

        /// <summary>
        /// Redraw the result table and graph when axis data is changed.
        /// </summary>
        public void ChangeAxisIndex()
        {
            if (RAXComboBox.Text.Equals(RAYComboBox.Text))
                throw new IgnoreException("");

            string xPath = RAXComboBox.Text;
            string yPath = RAYComboBox.Text;

            m_line = null;
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();

            foreach (int jobid in m_win.JobList.Keys)
            {
                Job.Job j = m_owner.JobManager.GroupDic[m_groupName].GetJob(jobid);
                double xd = j.ExecParam.GetParameter(xPath);
                double yd = j.ExecParam.GetParameter(yPath);

                DrawPoint(xd, yd, m_win.JobList[jobid]);
            }
            m_zCnt.GraphPane.XAxis.Scale.MinAuto = true;
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = true;
            m_zCnt.GraphPane.YAxis.Scale.MinAuto = true;
            m_zCnt.GraphPane.YAxis.Scale.MaxAuto = true;
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Set the parameter entry to display the result.
        /// </summary>
        /// <param name="name">the parameter name.</param>
        /// <param name="isX">the flag whether this parameter is default parameter at X axis.</param>
        /// <param name="isY">the flag whether this parameter is default parameter at Y axis.</param>
        public void SetResultEntryBox(string name, bool isX, bool isY)
        {
            RAXComboBox.Items.Add(name);
            RAYComboBox.Items.Add(name);

            if (isX)
            {
                for (int i = 0; i < RAXComboBox.Items.Count; i++)
                {
                    if (RAXComboBox.Items[i].Equals(name))
                    {
                        RAXComboBox.SelectedIndex = i;
                    }
                }
            }
            if (isY)
            {
                for (int i = 0; i < RAYComboBox.Items.Count; i++)
                {
                    if (RAYComboBox.Items[i].Equals(name))
                    {
                        RAYComboBox.SelectedIndex = i;
                    }
                }
            }
        }

        /// <summary>
        /// Save the result of parameter estimation to the file.
        /// </summary>
        /// <param name="writer">the save file name.</param>
        public void SaveParameterEstimationResult(string fileName, StreamWriter writer)
        {
            List<string> list = new List<string>();
            list.Add("Generation");
            list.Add("Estimation");
            string metaFile = fileName + ".meta";
            AnalysisFile.AnalysisResultMetaFile.CreateGenerationData(metaFile, "ParameterEstimation", list);
//            writer.WriteLine("#PARAMETER");
//            writer.WriteLine("#GENERATION");
            for (int i = 0; i < m_line.Points.Count; i++)
            {
                writer.WriteLine(m_line[i].X + "," + m_line[i].Y);
            }
        }

        /// <summary>
        /// Save the result of robust analysis to the file.
        /// </summary>
        /// <param name="writer">the save file name.</param>
        public void SaveRobustAnalysisResult(string fileName, StreamWriter writer)
        {
//            writer.WriteLine("#ROBUST");
            List<string> paramList = new List<string>();
            for (int ind = 0; ind < RAXComboBox.Items.Count; ind++)
            {
                string name = RAXComboBox.Items[ind] as string;
//                writer.Write("," + name);
                paramList.Add(name);
            }
//            writer.WriteLine("");
            string metaFile = fileName + ".meta";
            AnalysisFile.AnalysisResultMetaFile.CreatePlotMetaFile(metaFile, "RobustAnalysis", paramList);

            foreach (int jobid in m_win.JobList.Keys)
            {
                Job.Job j = m_owner.JobManager.GroupDic[m_groupName].GetJob(jobid);
                writer.Write(m_win.JobList[jobid]);
                foreach (string param in paramList)
                {
                    double data = j.ExecParam.GetParameter(param);
                    writer.Write("," + data);
                }
                writer.WriteLine("");
            }

        }

        /// <summary>
        /// Save the result of bifurcation analysis to the file.
        /// </summary>
        /// <param name="writer">the save file name.</param>
        public void SaveBifurcationResult(string fileName, StreamWriter writer)
        {
            //            writer.WriteLine("#BIFURCATION");
            List<string> paramList = new List<string>();
            for (int ind = 0; ind < RAXComboBox.Items.Count; ind++)
            {
                string name = RAXComboBox.Items[ind] as string;
                paramList.Add(name);
            }
            string metaFile = fileName + ".meta";
            AnalysisFile.AnalysisResultMetaFile.CreatePlotMetaFile(metaFile, "BifurcationAnalysis", paramList);

            foreach (LineItem c in m_zCnt.GraphPane.CurveList)
            {
                for (int i = 0; i < c.Points.Count; i++)
                {
                    writer.WriteLine(c[i].X + "," + c[i].Y);
                }
                writer.WriteLine("");
            }
        }

        /// <summary>
        /// Set graph setting before the data is added.
        /// </summary>
        public void PreGraphSet()
        {
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = true;
            m_zCnt.GraphPane.YAxis.Scale.MaxAuto = true;
        }

        /// <summary>
        /// Set graph setting after the data is added.
        /// </summary>
        public void PostGraphSet()
        {
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = false;
            m_zCnt.GraphPane.YAxis.Scale.MaxAuto = false;
        }

        #region Events
        /// <summary>
        /// Event to change the index of selected data.
        /// </summary>
        /// <param name="sender">object(ComboBox).</param>
        /// <param name="e">EventArgs.</param>
        private void XSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAxisIndex();
                x_index = RAXComboBox.SelectedIndex;
            }
            catch (IgnoreException)
            {
                RAXComboBox.SelectedIndex = x_index;
            }
        }

        /// <summary>
        /// Event to change the index of selected data.
        /// </summary>
        /// <param name="sender">object(ComboBox).</param>
        /// <param name="e">EventArgs.</param>
        private void YSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAxisIndex();
                y_index = RAYComboBox.SelectedIndex;
            }
            catch (IgnoreException)
            {
                RAYComboBox.SelectedIndex = y_index;
            }
        }

        // ZedGraphでContextMenuを表示するたびに作り直しているので、
        // このイベントでも毎回メニューの削除、追加をする必要がある
        private void ZedControlContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            foreach (ToolStripMenuItem m in menuStrip.Items)
            {
                if (m.Name.Contains("copy"))
                {
                    menuStrip.Items.Remove(m);
                    break;
                }
            }
            foreach (ToolStripMenuItem m in menuStrip.Items)
            {
                if (m.Name.Contains("save_as"))
                {
                    menuStrip.Items.Remove(m);
                    break;
                }
            }
            foreach (ToolStripMenuItem m in menuStrip.Items)
            {
                if (m.Name.Contains("set_default"))
                {
                    menuStrip.Items.Remove(m);
                    break;
                }
            }
        }
        #endregion

    }
}
