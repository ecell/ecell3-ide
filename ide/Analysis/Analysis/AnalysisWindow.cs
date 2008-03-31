//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
using System.Threading;
using System.Windows.Forms;

using EcellLib;
using EcellLib.SessionManager;
using ZedGraph;
using Formulator;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Form to display the setting and result of analysis.
    /// </summary>
    public partial class AnalysisWindow : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// The dictionary of the logging data to be observed.
        /// </summary>
        private Dictionary<string, EcellData> m_robustObservList = new Dictionary<string, EcellData>();
        /// <summary>
        /// The dictionary of the logging data to be observed.
        /// </summary>
        private Dictionary<string, EcellData> m_bifurcationObservList = new Dictionary<string, EcellData>();
        /// <summary>
        /// The dictionary of the data to be set by random.
        /// </summary>
        private Dictionary<string, EcellData> m_paramList = new Dictionary<string, EcellData>();
        /// <summary>
        /// Graph control to display the matrix of analysis result.
        /// </summary>
        private ZedGraphControl m_zCnt = null;
        /// <summary>
        /// Popup menu to be displayed in DataGridView.
        /// </summary>
        private ContextMenuStrip m_cntMenu = null;
        /// <summary>
        /// The parent plugin include this form.
        /// </summary>
        private Analysis m_parent = null;
        /// <summary>
        /// SessionManager to manage the analysis session.
        /// </summary>
        private SessionManager.SessionManager m_manager;
        /// <summary>
        /// The max number of input data to be executed FFT.
        /// </summary>
        public const int MaxSize = 2097152;
        /// <summary>
        /// The parameter of estimation parameter
        /// </summary>
        private SimplexCrossoverParameter m_peParam = new SimplexCrossoverParameter();
        /// <summary>
        /// The form to set the advanced parameter of parameter estimation.
        /// </summary>
        private PEAdvancedWindow m_awin;
        /// <summary>
        /// The form to set the estimation formulator.
        /// </summary>
        private FormulatorWindow m_fwin;
        /// <summary>
        /// The user control to set the estimation formulator.
        /// </summary>
        private FormulatorControl m_fcnt;
        /// <summary>
        /// The form to display the setting and result of analysis.
        /// </summary>
        private static AnalysisWindow s_win = null;
        /// <summary>
        /// The line information of dot plot.
        /// </summary>
        private LineItem m_line;
        private Color m_headerColor;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public AnalysisWindow()
        {
            InitializeComponent();
            m_cntMenu = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = Analysis.s_resources.GetString("ReflectMenuText");
            it.Click += new EventHandler(ClickReflectMenu);
            m_cntMenu.Items.AddRange(new ToolStripItem[] { it });
            RAResultGridView.ContextMenuStrip = m_cntMenu;
            RARandomCheck.Checked = true;
            RAMatrixCheck.Checked = false;

            ContextMenuStrip peCntMenu = new ContextMenuStrip();
            ToolStripMenuItem peit = new ToolStripMenuItem();
            peit.Text = Analysis.s_resources.GetString("ReflectMenuText");
            peit.Click += new EventHandler(ClickPEReflectMenu);
            peCntMenu.Items.AddRange(new ToolStripItem[] { peit });
            PEEstimateView.ContextMenuStrip = peCntMenu;

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
            RAAnalysisTableLayout.Controls.Add(m_zCnt, 0, 0);
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
            m_manager = SessionManager.SessionManager.GetManager();
            this.FormClosed += new FormClosedEventHandler(CloseRobustAnalysisForm);
            m_line = null;

            InitializeData();
            s_win = this;
            m_headerColor = Color.LightCyan;
        }

        #region accessor
        /// <summary>
        /// get/set the controller of this window.
        /// </summary>
        public Analysis Control
        {
            get { return this.m_parent; }
            set { this.m_parent = value; }
        }

        /// <summary>
        /// get the AnalysisWindow.
        /// </summary>
        /// <returns></returns>
        static public AnalysisWindow GetWindow()
        {
            return AnalysisWindow.s_win;
        }
        #endregion

        #region Commons
        /// <summary>
        /// Refresh the value of parameters.
        /// </summary>
        public void InitializeData()
        {
            ClearEntry();
            DataManager manager = DataManager.GetDataManager();

            List<string> mList = manager.GetModelList();
            foreach (string modelName in mList)
            {
                List<EcellObject> oList = manager.GetData(modelName, null);
                foreach (EcellObject sObj in oList)
                {
                    SearchAndAddParamEntry(sObj);
                    if (sObj.Children != null)
                    {
                        foreach (EcellObject obj in sObj.Children)
                        {
                            SearchAndAddParamEntry(obj);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clear the entries.
        /// </summary>
        public void Clear()
        {
            ClearEntry();
            ClearResult();
        }

        /// <summary>
        /// Clear the entries in observed data and parameter data.
        /// </summary>
        private void ClearEntry()
        {
            RAObservGridView.Rows.Clear();
            RAParamGridView.Rows.Clear();
            PEParamGridView.Rows.Clear();

            BAParameterGridView.Rows.Clear();
            BAObservedGridView.Rows.Clear();

            m_paramList.Clear();
            m_robustObservList.Clear();
            m_bifurcationObservList.Clear();
        }

        /// <summary>
        /// Clear the entries in result data.
        /// </summary>
        public void ClearResult()
        {
            RAXComboBox.Items.Clear();
            RAYComboBox.Items.Clear();
            RAResultGridView.Rows.Clear();

            PEEstimateView.Rows.Clear();
            SACCCGridView.Rows.Clear();
            SAFCCGridView.Rows.Clear();

            m_line = null;
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();
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
            if (isX) RAXComboBox.SelectedText = name;
            if (isY) RAYComboBox.SelectedText = name;
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
        /// Get the estimation type.
        /// </summary>
        /// <returns>Estimation type.</returns>
        private EstimationFormulatorType GetFormulatorType()
        {
            EstimationFormulatorType type = EstimationFormulatorType.Max;
            if (PEEstimationCombo.SelectedIndex == 1)
            {
                type = EstimationFormulatorType.SumMax;
            }
            else if (PEEstimationCombo.SelectedIndex == 2)
            {
                type = EstimationFormulatorType.Min;
            }
            else if (PEEstimationCombo.SelectedIndex == 3)
            {
                type = EstimationFormulatorType.SumMin;
            }
            else if (PEEstimationCombo.SelectedIndex == 4)
            {
                type = EstimationFormulatorType.EqualZero;
            }
            else if (PEEstimationCombo.SelectedIndex == 5)
            {
                type = EstimationFormulatorType.SumEqualZero;
            }
            return type;
        }

        /// <summary>
        /// Get the list of property to set the initial value for analysis.
        /// If there are any problems, this function return null.
        /// </summary>
        /// <returns>the list of parameter property.</returns>
        public List<ParameterRange> ExtractParameter()
        {
            List<ParameterRange> resList = new List<ParameterRange>();

            for (int i = 0; i < RAParamGridView.Rows.Count; i++)
            {
                string path = RAParamGridView[0, i].Value.ToString();
                double max = Convert.ToDouble(RAParamGridView[1, i].Value);
                double min = Convert.ToDouble(RAParamGridView[2, i].Value);
                double step = Convert.ToDouble(RAParamGridView[3, i].Value);

                if (min > max) continue;
                ParameterRange p = new ParameterRange(path, min, max, step);
                resList.Add(p);
            }

            return resList;
        }

        /// <summary>
        ///  The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="obj">Changed value of object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject obj)
        {
            if (obj.Value == null) return;
            foreach (EcellData d in obj.Value)
            {
                if (m_paramList.ContainsKey(d.EntityPath))
                {
                    if (!d.Committed) continue;
                    for (int i = 0; i < RAParamGridView.Rows.Count; i++)
                    {
                        String pData = RAParamGridView[0, i].Value.ToString();
                        if (pData.Equals(d.EntityPath))
                        {
                            m_paramList.Remove(d.EntityPath);
                            RAParamGridView.Rows.RemoveAt(i);
                            break;
                        }
                    }
                }
                AddParameterEntry(obj, d);
            }
        }


        /// <summary>
        /// Add the parameter data.
        /// </summary>
        /// <param name="obj">object include the parameter data.</param>
        /// <param name="d">the parameter data.</param>
        private void AddParameterEntry(EcellObject obj, EcellData d)
        {
            if (d.Committed) return;
            AddRobustAnalysisParameterEntry(obj, d);
            AddParameterEstimateEntry(obj, d);
            AddBifurcationAnalysisParameterEntry(obj, d);
            m_paramList.Add(d.EntityPath, d);
        }

        /// <summary>
        /// Redraw the result table and graph when axis data is changed.
        /// </summary>
        public void ChangeAxisIndex()
        {
            if (RAXComboBox.Text.Equals(RAYComboBox.Text))
                return;

            string xPath = RAXComboBox.Text;
            string yPath = RAYComboBox.Text;

            foreach (DataGridViewRow r in RAResultGridView.Rows)
            {
                if (r.Tag == null) continue;
                int jobid = (int)r.Tag;

                r.Cells[1].Value = m_manager.ParameterDic[jobid].ParamDic[xPath];
                r.Cells[2].Value = m_manager.ParameterDic[jobid].ParamDic[yPath];
            }
        }


        /// <summary>
        /// Add the parameter entry into Parameter DataGridView.
        /// If this parameter alrady exists, system don't insert the entry.
        /// </summary>
        /// <param name="obj">parameter object.</param>
        private void SearchAndAddParamEntry(EcellObject obj)
        {
            if (obj.Value == null) return;
            foreach (EcellData d in obj.Value)
            {
                if (m_paramList.ContainsKey(d.EntityPath)) continue;
                AddParameterEntry(obj, d);
            }
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
            RAResultGridView.Enabled = false;

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
            RAResultGridView.Enabled = true;

            LineItem line = null;
            Color drawColor = Color.Blue;
            Color styleColor = Color.White;
            if (!isOK)
            {
                drawColor = Color.Red;
                styleColor = Color.Silver;
            }

            DataGridViewRow r = new DataGridViewRow();
            DataGridViewCheckBoxCell c0 = new DataGridViewCheckBoxCell();
            c0.Value = isOK;
            r.Cells.Add(c0);

            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = Convert.ToString(x);
            c1.Style.BackColor = styleColor;
            r.Cells.Add(c1);

            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = Convert.ToString(y);
            c2.Style.BackColor = styleColor;
            r.Cells.Add(c2);

            r.Tag = jobid;
            RAResultGridView.Rows.Add(r);
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
        #endregion


        #region SensitivityAnalysis
        /// <summary>
        /// Get the sensitivity analysis parameter set in this form.
        /// </summary>
        /// <returns>the parameter of sensitivity analysis.</returns>
        public SensitivityAnalysisParameter GetSensitivityAnalysisParameter()
        {
            SensitivityAnalysisParameter p = new SensitivityAnalysisParameter();
            p.Step = Convert.ToInt32(SAStepTextBox.Text);
            p.RelativePerturbation = Convert.ToDouble(SARelativePertTextBox.Text);
            p.AbsolutePerturbation = Convert.ToDouble(SAAbsolutePertTextBox.Text);

            return p;
        }

        /// <summary>
        /// Set the parameter of sensitivity analysis to this form.
        /// </summary>
        /// <param name="p">the parameter of sensitivity analysis.</param>
        public void SetSensitivityAnalysis(SensitivityAnalysisParameter p)
        {
            SAStepTextBox.Text = Convert.ToString(p.Step);
            SARelativePertTextBox.Text = Convert.ToString(p.RelativePerturbation);
            SAAbsolutePertTextBox.Text = Convert.ToString(p.AbsolutePerturbation);
        }

        /// <summary>
        /// Create the header of sensitivity matrix.
        /// </summary>
        /// <param name="gridView">DataGridView.</param>
        /// <param name="data">Header List.</param>
        private void CreateSensitivityHeader(DataGridView gridView, List<string> data)
        {
            DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
            gridView.Columns.Add(c);
            foreach (string key in data)
            {
                c = new DataGridViewTextBoxColumn();
                c.Name = key;
                c.HeaderText = key;
                gridView.Columns.Add(c);
            }

            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Style.BackColor = m_headerColor;
            r.Cells.Add(c1);
            c1.ReadOnly = true;

            foreach (string key in data)
            {
                c1 = new DataGridViewTextBoxCell();
                c1.Style.BackColor = m_headerColor;
                c1.Value = key;
                r.Cells.Add(c1);
                c1.ReadOnly = true;
            }
            gridView.Rows.Add(r);
        }

        /// <summary>
        /// Set the header string of sensitivity matrix.
        /// </summary>
        /// <param name="activityList">the list of activity.</param>
        public void SetSensitivityHeader(List<string> activityList)
        {
            SACCCGridView.Columns.Clear();
            SACCCGridView.Rows.Clear();
            SAFCCGridView.Columns.Clear();
            SAFCCGridView.Rows.Clear();

            RAXComboBox.Enabled = false;
            RAYComboBox.Enabled = false;
            RAResultGridView.Enabled = false;

            CreateSensitivityHeader(SACCCGridView, activityList);
            CreateSensitivityHeader(SAFCCGridView, activityList);
        }

        /// <summary>
        /// Create the the row data of analysis result for variable.
        /// </summary>
        /// <param name="key">the property name of parameter.</param>
        /// <param name="sensList">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfCCC(string key, List<double> sensList)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
            c.Value = key;
            c.Style.BackColor = m_headerColor;
            r.Cells.Add(c);
            c.ReadOnly = true;

            foreach (double d in sensList)
            {
                c = new DataGridViewTextBoxCell();
                c.Value = d;
                c.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                r.Cells.Add(c);
                c.ReadOnly = true;
            }
            SACCCGridView.Rows.Add(r);
        }

        /// <summary>
        /// Create the row data of analysis result for process
        /// </summary>
        /// <param name="key">the property name of parameter.</param>
        /// <param name="sensList">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfFCC(string key, List<double> sensList)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
            c.Value = key;
            c.Style.BackColor = m_headerColor;
            r.Cells.Add(c);
            c.ReadOnly = true;

            foreach (double d in sensList)
            {
                c = new DataGridViewTextBoxCell();
                c.Value = d;
                r.Cells.Add(c);
                c.ReadOnly = true;
            }
            SAFCCGridView.Rows.Add(r);
        }
        #endregion

        #region BifurcationAnalysis
        /// <summary>
        /// Get the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <returns>the parameter set of bifurcation analysis.</returns>
        public BifurcationAnalysisParameter GetBifurcationAnalysisPrameter()
        {
            BifurcationAnalysisParameter p = new BifurcationAnalysisParameter();
            p.SimulationTime = Convert.ToDouble(BASimTimeText.Text);
            p.WindowSize = Convert.ToDouble(BAWinSizeText.Text);
            p.MaxInput = Convert.ToInt32(BAMaxInputText.Text);
            p.MaxFreq = Convert.ToDouble(BAMaxFreqText.Text);
            p.MinFreq = Convert.ToDouble(BAMinFreqText.Text);

            return p;
        }

        /// <summary>
        /// Set the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <param name="p">the parameter set of bifurcation analysis.</param>
        public void SetBifurcationAnalysisParameter(BifurcationAnalysisParameter p)
        {
            BASimTimeText.Text = Convert.ToString(p.SimulationTime);
            BAWinSizeText.Text = Convert.ToString(p.WindowSize);
            BAMaxInputText.Text = Convert.ToString(p.MaxInput);
            BAMaxFreqText.Text = Convert.ToString(p.MaxFreq);
            BAMinFreqText.Text = Convert.ToString(p.MinFreq);
        }


        /// <summary>
        /// Add the parameter entry to use at the bifurcation analysis.
        /// </summary>
        /// <param name="obj">object include the parameter data.</param>
        /// <param name="d">the parameter data.</param>
        private void AddBifurcationAnalysisParameterEntry(EcellObject obj, EcellData d)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = d.EntityPath;
            r.Cells.Add(c1);
            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = d.Max;
            r.Cells.Add(c2);
            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = d.Min;
            r.Cells.Add(c3);
            DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
            c4.Value = d.Step;
            r.Cells.Add(c4);
            r.Tag = obj;

            BAParameterGridView.Rows.Add(r);
        }

        /// <summary>
        /// Extract the judgement condition from DataGridView.
        /// </summary>
        /// <returns>the list of judgement condition.</returns>
        public List<AnalysisJudgementParam> ExtractBifurcationObserved()
        {
            List<AnalysisJudgementParam> resList = new List<AnalysisJudgementParam>();

            for (int i = 0; i < BAObservedGridView.Rows.Count; i++)
            {
                string path = BAObservedGridView[0, i].Value.ToString();
                double max = Convert.ToDouble(BAObservedGridView[1, i].Value);
                double min = Convert.ToDouble(BAObservedGridView[2, i].Value);
                double diff = Convert.ToDouble(BAObservedGridView[3, i].Value);
                double rate = Convert.ToDouble(BAObservedGridView[4, i].Value);

                AnalysisJudgementParam p = new AnalysisJudgementParam(path, max, min, diff, rate);
                resList.Add(p);
            }

            return resList;
        }

        /// <summary>
        /// Get the list of observed property to judge for analysis.
        /// If there are any problems, this function return null. 
        /// </summary>
        /// <returns>the list of observed property.</returns>
        public List<SaveLoggerProperty> GetBifurcationObservedDataList()
        {
            SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();
            List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();

            for (int i = 0; i < BAObservedGridView.Rows.Count; i++)
            {
                String dir = manager.TmpDir;
                string path = BAObservedGridView[0, i].Value.ToString();
                double start = 0.0;
                double end = Convert.ToDouble(BASimTimeText.Text);
                SaveLoggerProperty p = new SaveLoggerProperty(path, start, end, dir);

                resList.Add(p);
            }

            if (resList.Count < 1)
            {
                String mes = Analysis.s_resources.GetString("ErrObservProp");
                MessageBox.Show(mes, "ERRPR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return resList;
        }
        #endregion

        #region RobustAnalysis
        /// <summary>
        /// Get the robust analysis parameter set in this form.
        /// </summary>
        /// <returns>the parameter of robust analysis.</returns>
        public RobustAnalysisParameter GetRobustAnalysisParameter()
        {
            RobustAnalysisParameter p = new RobustAnalysisParameter();
            p.SampleNum = Convert.ToInt32(RASampleNumText.Text);
            p.SimulationTime = Convert.ToDouble(RASimTimeText.Text);
            p.IsRandomCheck = RARandomCheck.Checked;
            p.MaxData = Convert.ToInt32(RMAMaxData.Text);
            p.MaxFreq = Convert.ToDouble(RAMaxFreqText.Text);
            p.MinFreq = Convert.ToDouble(RAMinFreqText.Text);
            p.WinSize = Convert.ToDouble(RAWinSizeText.Text);

            return p;
        }

        /// <summary>
        /// Set the robust analysis parameter.
        /// </summary>
        /// <param name="p">the parameter of robust analysis.</param>
        public void SetRobustAnalysisParameter(RobustAnalysisParameter p)
        {
            RASampleNumText.Text = Convert.ToString(p.SampleNum);
            RASimTimeText.Text = Convert.ToString(p.SimulationTime);
            RMAMaxData.Text = Convert.ToString(p.MaxData);
            RAMaxFreqText.Text = Convert.ToString(p.MaxFreq);
            RAMinFreqText.Text = Convert.ToString(p.MinFreq);
            RAWinSizeText.Text = Convert.ToString(p.WinSize);
            if (p.IsRandomCheck) RARandomCheck.Checked = true;
            else RAMatrixCheck.Checked = true;
        }

        /// <summary>
        /// Extract the judgement condition from DataGridView.
        /// </summary>
        /// <returns>the list of judgement condition.</returns>
        public List<AnalysisJudgementParam> ExtractObserved()
        {
            List<AnalysisJudgementParam> resList = new List<AnalysisJudgementParam>();

            for (int i = 0; i < RAObservGridView.Rows.Count; i++)
            {
                string path = RAObservGridView[0, i].Value.ToString();
                double max = Convert.ToDouble(RAObservGridView[1, i].Value);
                double min = Convert.ToDouble(RAObservGridView[2, i].Value);
                double diff = Convert.ToDouble(RAObservGridView[3, i].Value);
                double rate = Convert.ToDouble(RAObservGridView[4, i].Value);

                AnalysisJudgementParam p = new AnalysisJudgementParam(path, max, min, diff, rate);
                resList.Add(p);
            }

            return resList;
        }

        /// <summary>
        /// Get the list of observed property to judge for analysis.
        /// If there are any problems, this function return null. 
        /// </summary>
        /// <returns>the list of observed property.</returns>
        public List<SaveLoggerProperty> GetRobustObservedDataList()
        {
            SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();
            List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();

            for (int i = 0; i < RAObservGridView.Rows.Count; i++)
            {
                String dir = manager.TmpDir;
                string path = RAObservGridView[0, i].Value.ToString();
                double start = 0.0;
                double end = Convert.ToDouble(RASimTimeText.Text);
                SaveLoggerProperty p = new SaveLoggerProperty(path, start, end, dir);

                resList.Add(p);
            }

            if (resList.Count < 1)
            {
                String mes = Analysis.s_resources.GetString("ErrObservProp");
                MessageBox.Show(mes, "ERRPR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return resList;
        }

        /// <summary>
        /// Add the parameter entry to use at the robust analysis.
        /// </summary>
        /// <param name="obj">object include the parameter data.</param>
        /// <param name="d">the parameter data.</param>
        private void AddRobustAnalysisParameterEntry(EcellObject obj, EcellData d)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = d.EntityPath;
            r.Cells.Add(c1);
            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = d.Max;
            r.Cells.Add(c2);
            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = d.Min;
            r.Cells.Add(c3);
            DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
            c4.Value = d.Step;
            r.Cells.Add(c4);
            r.Tag = obj;

            RAParamGridView.Rows.Add(r);
        }

        #endregion

        #region ParameterEstimation
        /// <summary>
        /// Get the parameter of parameter estimation.
        /// </summary>
        /// <returns>the parameter of parameter estimation.</returns>
        public ParameterEstimationParameter GetParameterEstimationParameter()
        {
            string estForm = PEEstmationFormula.Text;
            double simTime = Convert.ToDouble(PESimulationText.Text);
            int popNum = Convert.ToInt32(PEPopulationText.Text);
            int genNum = Convert.ToInt32(PEGenerationText.Text);
            EstimationFormulatorType type = GetFormulatorType();

            return new ParameterEstimationParameter(estForm, simTime, popNum, genNum, type, m_peParam);
        }

        /// <summary>
        /// Set the parameter of parameter estimation.
        /// </summary>
        /// <param name="param">the parameter of parameter estimation.</param>
        public void SetParameterEstimationParameter(ParameterEstimationParameter param)
        {
            PESimulationText.Text = Convert.ToString(param.SimulationTime);
            PEPopulationText.Text = Convert.ToString(param.Population);
            PEGenerationText.Text = Convert.ToString(param.Generation);
            m_peParam = param.Param;
        }

        /// <summary>
        /// Get the list of observed property to judge for analysis.
        /// If there are any problems, this function return null. 
        /// </summary>
        /// <returns>the list of observed property.</returns>
        public List<SaveLoggerProperty> GetParameterObservedDataList()
        {
            SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();
            List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();

            String dir = manager.TmpDir;
            double start = 0.0;
            double end = Convert.ToDouble(RASimTimeText.Text);
            string formulator = PEEstmationFormula.Text;
            string[] ele = formulator.Split(new char[] { '+', '-', '*' });
            for (int i = 0; i < ele.Length; i++)
            {
                string element = ele[i].Replace(" ", "");
                if (element.StartsWith("Variable") ||
                    element.StartsWith("Process"))
                    resList.Add(new SaveLoggerProperty(element, start, end, dir));
            }
            return resList;
        }

        /// <summary>
        /// Add the parameter entry to use at parameter estimation.
        /// </summary>
        /// <param name="obj">object include the parameter data.</param>
        /// <param name="d">the parameter data.</param>
        private void AddParameterEstimateEntry(EcellObject obj, EcellData d)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = d.EntityPath;
            r.Cells.Add(c1);
            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = d.Max;
            r.Cells.Add(c2);
            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = d.Min;
            r.Cells.Add(c3);
            r.Tag = obj;

            PEParamGridView.Rows.Add(r);
        }



        /// <summary>
        /// Set the estimated parameter.
        /// </summary>
        /// <param name="param">the execution parameter.</param>
        /// <param name="result">the estimated value.</param>
        /// <param name="generation">the generation.</param>
        public void AddEstimateParameter(ExecuteParameter param, double result, int generation)
        {
            PEEstimateView.Rows.Clear();
            foreach (string key in param.ParamDic.Keys)
            {
                DataGridViewRow r = new DataGridViewRow();

                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = Convert.ToString(key);
                r.Cells.Add(c1);

                DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                c2.Value = Convert.ToString(param.ParamDic[key]);
                r.Cells.Add(c2);

                PEEstimateView.Rows.Add(r);
            }
            PEEstimationValue.Text = Convert.ToString(result);
            PEGenerateValue.Text = Convert.ToString(generation);
        }
        #endregion


        /// <summary>
        /// Remove the entry of parameter value.
        /// </summary>
        /// <param name="key">the key of parameter value.</param>
        public void RemoveParamEntry(string key)
        {
            if (m_paramList.ContainsKey(key))
                m_paramList.Remove(key);
            else
                return;

            for (int i = 0; i < RAParamGridView.Rows.Count; i++)
            {
                string pData = RAParamGridView[0, i].Value.ToString();
                if (!pData.Equals(key))
                    continue;
                EcellObject obj = RAParamGridView.Rows[i].Tag as EcellObject;
                if (obj == null)
                    continue;

                RAParamGridView.Rows.RemoveAt(i);
                m_paramList.Remove(key);

                foreach (EcellData d in obj.Value)
                {
                    if (!key.Equals(d.EntityPath))
                        continue;

                    d.Committed = true;
                }
                DataManager dManager = DataManager.GetDataManager();
                dManager.DataChanged(obj.ModelID, obj.Key, obj.Type, obj);
            }
        }

        /// <summary>
        /// Remove the entry of observed value.
        /// </summary>
        /// <param name="key">the key of observed value.</param>
        public void RemoveObservEntry(string key)
        {
            if (m_robustObservList.ContainsKey(key))
                m_robustObservList.Remove(key);
            else
                return;

            for (int i = 0; i < RAObservGridView.Rows.Count; i++)
            {
                String ind = RAObservGridView[0, i].Value.ToString();
                if (ind.Equals(key))
                {
                    RAObservGridView.Rows.RemoveAt(i);
                    continue;
                }
            }

        }


        #region Events
        /// <summary>
        /// Event to close this window.
        /// </summary>
        /// <param name="sender">This form.</param>
        /// <param name="e">FormClosedEventArgs</param>
        void CloseRobustAnalysisForm(object sender, FormClosedEventArgs e)
        {
            if (m_parent != null)
            {
                m_parent.CloseAnalysisWindow();
            }
            m_parent = null;
        }

        /// <summary>
        /// Event to change the Y axis.
        /// </summary>
        /// <param name="sender">ComboBox.</param>
        /// <param name="e">EventArgs.</param>
        private void ChangeYIndex(object sender, EventArgs e)
        {
            ChangeAxisIndex();
        }

        /// <summary>
        /// Event to change the X axis.
        /// </summary>
        /// <param name="sender">ComboBox.</param>
        /// <param name="e">EventArgs.</param>
        private void ChangeXIndex(object sender, EventArgs e)
        {
            ChangeAxisIndex();
        }

        /// <summary>
        /// Reflect the parameter condition to the model property.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ClickReflectMenu(object sender, EventArgs e)
        {
            DataManager manager = DataManager.GetDataManager();
            foreach (DataGridViewRow r in RAResultGridView.SelectedRows)
            {
                if (r.Tag == null) continue;
                int jobid = (int)r.Tag;
                foreach (string path in m_manager.ParameterDic[jobid].ParamDic.Keys)
                {
                    double param = m_manager.ParameterDic[jobid].ParamDic[path];
                    String[] ele = path.Split(new char[] { ':' });
                    String objId = ele[1] + ":" + ele[2];
                    List<string> modelList = manager.GetModelList();
                    EcellObject obj = manager.GetEcellObject(modelList[0], objId, ele[0]);
                    if (obj == null) continue;
                    foreach (EcellData d in obj.Value)
                    {
                        if (d.EntityPath.Equals(path))
                        {
                            d.Value = new EcellValue(param);
                            d.Committed = true;
                            break;
                        }
                    }
                    manager.DataChanged(modelList[0], objId, ele[0], obj);
                }
                break;
            }
        }

        /// <summary>
        /// Reflect the parameter condition to the model property.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ClickPEReflectMenu(object sender, EventArgs e)
        {
            DataManager manager = DataManager.GetDataManager();
            foreach (DataGridViewRow r in PEEstimateView.Rows)
            {
                string path = Convert.ToString(r.Cells[0].Value);
                double v = Convert.ToDouble(r.Cells[1].Value);
                String[] ele = path.Split(new char[] { ':' });
                String objId = ele[1] + ":" + ele[2];
                List<string> modelList = manager.GetModelList();
                EcellObject obj = manager.GetEcellObject(modelList[0], objId, ele[0]);
                if (obj == null) continue;
                foreach (EcellData d in obj.Value)
                {
                    if (d.EntityPath.Equals(path))
                    {
                        d.Value = new EcellValue(v);
                        d.Committed = true;
                        break;
                    }
                }
                manager.DataChanged(modelList[0], objId, ele[0], obj);
            }
        }

        /// <summary>
        /// Show the popup menu on the parameter DataGridView.
        /// </summary>
        /// <param name="r">The row of popup menu.</param>
        private void AssignParamPopupMenu(DataGridViewRow r)
        {
            ContextMenuStrip contextStrip = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = "Delete ";
            it.ShortcutKeys = Keys.Control | Keys.D;
            it.Click += new EventHandler(DeleteParamItem);
            it.Tag = r;
            contextStrip.Items.AddRange(new ToolStripItem[] { it });
            r.ContextMenuStrip = contextStrip;
        }

        /// <summary>
        /// Show the popup menu on the observe DataGridView.
        /// </summary>
        /// <param name="r">The row of popup menu.</param>
        private void AssignObservPopupMenu(DataGridViewRow r)
        {
            ContextMenuStrip contextStrip = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = "Delete ";
            it.ShortcutKeys = Keys.Control | Keys.D;
            it.Click += new EventHandler(DeleteObservItem);
            it.Tag = r;
            contextStrip.Items.AddRange(new ToolStripItem[] { it });
            r.ContextMenuStrip = contextStrip;
        }

        /// <summary>
        /// Event to drop the object on the parameter DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragDropParam(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            DataManager dManager = DataManager.GetDataManager();
            EcellObject t = dManager.GetEcellObject(dobj.ModelID, dobj.Key, dobj.Type);
            foreach (EcellData d in t.Value)
            {
                if (d.EntityPath.Equals(dobj.Path))
                {
                    if (d.Max == 0.0 && d.Min == 0.0)
                    {
                        d.Max = Convert.ToDouble(d.Value.ToString()) * 1.5;
                        d.Min = Convert.ToDouble(d.Value.ToString()) * 0.5;
                    }
                    d.Committed = false;
                    break;
                }
            }
            dManager.DataChanged(t.ModelID, t.Key, t.Type, t);
        }

        /// <summary>
        /// Event to enter in the parameter DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragEnterParam(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Event to drop the object on the observe DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragDropObservForRobust(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            DataManager dManager = DataManager.GetDataManager();
            EcellObject t = dManager.GetEcellObject(dobj.ModelID, dobj.Key, dobj.Type);
            foreach (EcellData d in t.Value)
            {
                if (d.EntityPath.Equals(dobj.Path))
                {
                    DataGridViewRow r = new DataGridViewRow();
                    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                    c1.Value = d.EntityPath;
                    r.Cells.Add(c1);
                    DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                    if (d.Max == 0.0)
                    {
                        c2.Value = Convert.ToDouble(d.Value.ToString()) * 1.5;
                    }
                    else
                    {
                        c2.Value = d.Max;
                    }
                    r.Cells.Add(c2);
                    DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
                    if (d.Min == 0.0)
                    {
                        c3.Value = Convert.ToDouble(d.Value.ToString()) * 0.5;
                    }
                    else
                    {
                        c3.Value = d.Min;
                    }
                    r.Cells.Add(c3);
                    DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                    if (d.Max == 0.0 && d.Min == 0.0)
                    {
                        c4.Value = Convert.ToDouble(d.Value.ToString());
                    }
                    else
                    {
                        c4.Value = d.Max - d.Min;
                    }
                    r.Cells.Add(c4);
                    DataGridViewTextBoxCell c5 = new DataGridViewTextBoxCell();
                    c5.Value = 0.5;
                    r.Cells.Add(c5);
                    r.Tag = t;
                    AssignObservPopupMenu(r);
                    RAObservGridView.Rows.Add(r);
                    m_robustObservList.Add(d.EntityPath, d);
                }
            }
        }

        /// <summary>
        /// Event to enter in the observe DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragEnterObservForRobust(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Event to drop the object on the observe DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragDropObservForBifurcation(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            DataManager dManager = DataManager.GetDataManager();
            EcellObject t = dManager.GetEcellObject(dobj.ModelID, dobj.Key, dobj.Type);
            foreach (EcellData d in t.Value)
            {
                if (d.EntityPath.Equals(dobj.Path))
                {
                    DataGridViewRow r = new DataGridViewRow();
                    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                    c1.Value = d.EntityPath;
                    r.Cells.Add(c1);
                    DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                    if (d.Max == 0.0)
                    {
                        c2.Value = Convert.ToDouble(d.Value.ToString()) * 1.5;
                    }
                    else
                    {
                        c2.Value = d.Max;
                    }
                    r.Cells.Add(c2);
                    DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
                    if (d.Min == 0.0)
                    {
                        c3.Value = Convert.ToDouble(d.Value.ToString()) * 0.5;
                    }
                    else
                    {
                        c3.Value = d.Min;
                    }
                    r.Cells.Add(c3);
                    DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                    if (d.Max == 0.0 && d.Min == 0.0)
                    {
                        c4.Value = Convert.ToDouble(d.Value.ToString());
                    }
                    else
                    {
                        c4.Value = d.Max - d.Min;
                    }
                    r.Cells.Add(c4);
                    DataGridViewTextBoxCell c5 = new DataGridViewTextBoxCell();
                    c5.Value = 0.5;
                    r.Cells.Add(c5);
                    r.Tag = t;
//                    AssignObservPopupMenu(r);
                    BAObservedGridView.Rows.Add(r);
                    m_bifurcationObservList.Add(d.EntityPath, d);
                }
            }
        }

        /// <summary>
        /// Event to enter in the observe DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragEnterObservForBifurcation(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }


        /// <summary>
        /// Event to delete the item on the observe DataGridView.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs</param>
        private void DeleteObservItem(object sender, EventArgs e)
        {
            DataGridViewRow r = ((ToolStripMenuItem)sender).Tag as DataGridViewRow;
            if (r == null) return;

            string key = r.Cells[0].Value.ToString();
            RemoveObservEntry(key);
        }

        /// <summary>
        /// Event to delete the item on the parameter DataGridView.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void DeleteParamItem(object sender, EventArgs e)
        {
            DataGridViewRow r = ((ToolStripMenuItem)sender).Tag as DataGridViewRow;
            if (r == null) return;

            string key = r.Cells[0].Value.ToString();
            RemoveParamEntry(key);
        }

        /// <summary>
        /// Event when CheckBox of random is changed.
        /// If CheckBox of random is true, TextBox input the number of sample is active.
        /// </summary>
        /// <param name="sender">CheckBox.</param>
        /// <param name="e">EventArgs.</param>
        private void ChangeRARandomCheck(object sender, EventArgs e)
        {
            if (RARandomCheck.Checked == true)
            {
                if (RAMatrixCheck.Checked == true)
                {
                    RAMatrixCheck.Checked = false;
                    RASampleNumText.ReadOnly = false;
                }
            }
            else
            {
                if (RAMatrixCheck.Checked == false)
                {
                    RAMatrixCheck.Checked = true;
                    RASampleNumText.ReadOnly = true;
                }
            }
        }

        /// <summary>
        /// Event when CheckBox of matrix is changed.
        /// If CheckBox of matrix is true, TextBox input the number of sample is not active.
        /// </summary>
        /// <param name="sender">CheckBox.</param>
        /// <param name="e">EventArgs.</param>
        private void ChangeRAMatrixCheck(object sender, EventArgs e)
        {
            if (RAMatrixCheck.Checked == true)
            {
                if (RARandomCheck.Checked == true)
                {
                    RARandomCheck.Checked = false;
                    RASampleNumText.ReadOnly = true;
                }
            }
            else
            {
                if (RARandomCheck.Checked == false)
                {
                    RARandomCheck.Checked = true;
                    RASampleNumText.ReadOnly = false;
                }
            }
        }

        /// <summary>
        /// Process when user click close button on Window.
        /// </summary>
        /// <param name="m">Message</param>
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && m.WParam.ToInt32() == SC_CLOSE)
            {
                String mes = Analysis.s_resources.GetString("ConfirmClose");

                DialogResult res = MessageBox.Show(mes,
                    "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.OK) this.Dispose();
                return;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// The event sequence when the parameter estimation setting window is shown..
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void PEAdvancedButtonClicked(object sender, EventArgs e)
        {
            m_awin = new PEAdvancedWindow();
            m_peParam = new SimplexCrossoverParameter();
            m_awin.SetParameter(m_peParam);

            m_awin.PEAApplyButton.Click += new EventHandler(PEAApplyButtonClicked);

            m_awin.ShowDialog();
        }

        /// <summary>
        /// The event sequence when apply button in the parameter estimation setting 
        /// window is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        void PEAApplyButtonClicked(object sender, EventArgs e)
        {
            m_peParam = m_awin.GetParam();
            m_awin.Close();
        }

        /// <summary>
        /// The event sequence when the formulator window is shown.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void PEFormulaButtonClicked(object sender, EventArgs e)
        {
            DataManager manager = DataManager.GetDataManager();
            m_fwin = new FormulatorWindow();
            m_fcnt = new FormulatorControl();
            m_fwin.tableLayoutPanel.Controls.Add(m_fcnt, 0, 0);
            m_fcnt.Dock = DockStyle.Fill;
            m_fcnt.IsExpression = false;

            List<string> list = new List<string>();
            List<string> mlist = manager.GetModelList();
            List<EcellObject> objList = manager.GetData(mlist[0], null);
            if (objList != null)
            {
                foreach (EcellObject obj in objList)
                {
                    if (obj is EcellSystem)
                    {
                        if (obj.Children == null) continue;
                        foreach (EcellObject data in obj.Children)
                        {
                            if (data is EcellProcess)
                            {
                                list.Add("Process:" + data.Key + ":Activity");
                            }
                            else if (data is EcellVariable)
                            {
                                list.Add("Variable:" + data.Key + ":MolarConc");
                            }
                        }
                    }
                }
            }
            m_fcnt.AddReserveString(list);
            m_fcnt.ImportFormulate(PEEstmationFormula.Text);

            m_fwin.FApplyButton.Click += new EventHandler(FApplyButtonClicked);
            m_fwin.FCloseButton.Click += new EventHandler(m_fwin.CancelButtonClick);
            m_fwin.ShowDialog();
        }

        /// <summary>
        /// The event sequence when apply button in the  formulator window 
        /// to set estimation formulator is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        void FApplyButtonClicked(object sender, EventArgs e)
        {
            string ext = m_fcnt.ExportFormulate();
            PEEstmationFormula.Text = ext;
            m_fwin.Close();
        }
        #endregion
    }

}