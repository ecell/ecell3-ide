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
using System.Threading;
using System.Windows.Forms;

using EcellLib;
using EcellLib.SessionManager;
using ZedGraph;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Analysis window for robust analysis.
    /// </summary>
    public partial class RobustAnalysis : EcellDockContent
    {
        /// <summary>
        /// The dictionary of the logging data to be observed.
        /// </summary>
        private Dictionary<string, EcellData> m_observList = new Dictionary<string, EcellData>();
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
        /// The flag whether the analysis is running.
        /// </summary>
        private bool m_isRunning = false;
        /// <summary>
        /// SessionManager to manage the analysis session.
        /// </summary>
        private SessionManager.SessionManager m_manager;
        /// <summary>
        /// Timer to update the status of jobs.
        /// </summary>
        private System.Windows.Forms.Timer m_timer;
        /// <summary>
        /// The max number of input data to be executed FFT.
        /// </summary>
        public const int MaxSize = 2097152;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RobustAnalysis()
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

            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Interval = 5000;
            m_timer.Tick += new EventHandler(FireTimer);

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

            InitializeData();
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
        /// get/set the status of robust analysis.
        /// </summary>
        public bool IsRunning
        {
            get { return this.m_isRunning; }
            set { this.m_isRunning = value; }
        }
        #endregion

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

            m_paramList.Clear();
            m_observList.Clear();
        }

        /// <summary>
        /// Clear the entries in result data.
        /// </summary>
        private void ClearResult()
        {
            RAResultGridView.Rows.Clear();
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();
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
        /// Execute the robust analysis.
        /// Robust analysis include the simulation execution of parameter and 
        /// judgement of simulation results.
        /// </summary>
        public void ExecuteAnalysis()
        {
            String tmpDir = m_manager.TmpRootDir;
            int num = Convert.ToInt32(RASampleNumText.Text);
            double simTime = Convert.ToDouble(RASimTimeText.Text);
            int maxSize = Convert.ToInt32(RMAMaxData.Text);
            if (maxSize > RobustAnalysis.MaxSize)
            {
                string errmes = Analysis.s_resources.GetString("ErrOverMax") + "[" + RobustAnalysis.MaxSize + "]";
                MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string model = "";
            List<string> modelList = DataManager.GetDataManager().GetModelList();
            if (modelList.Count > 0) model = modelList[0];

            List<ParameterRange> paramList = ExtractParameter();
            if (paramList == null) return;
            List<SaveLoggerProperty> saveList = GetObservedDataList();
            if (saveList == null) return;

            m_manager.SetParameterRange(paramList);
            m_manager.SetLoggerData(saveList);
            if (RARandomCheck.Checked == true)
            {
                m_manager.RunSimParameterRange(tmpDir, model, num, simTime, false);
            }
            else
            {
                m_manager.RunSimParameterMatrix(tmpDir, model, simTime, false);
            }
            m_isRunning = true;
            m_timer.Enabled = true;
            m_timer.Start();
        }

        /// <summary>
        /// Update the status of session at intervals while program is running.
        /// </summary>
        /// <param name="sender">Timer.</param>
        /// <param name="e">EventArgs.</param>
        void FireTimer(object sender, EventArgs e)
        {
            if (!m_manager.IsFinished())
            {
                if (m_isRunning == false)
                {
                    m_manager.StopRunningJobs();
                    m_timer.Enabled = false;
                    m_timer.Stop();
                }
                return;
            }
            m_isRunning = false;
            m_timer.Enabled = false;
            m_timer.Stop();

            if (m_manager.IsError())
            {
                String mes = Analysis.s_resources.GetString("ErrFindErrorJob");
                DialogResult res = MessageBox.Show(mes, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Cancel)
                {
                    return;
                }
            }
            JudgeResult();
            String finMes = Analysis.s_resources.GetString("FinishRAnalysis");
            MessageBox.Show(finMes, "Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Stop the robust analysis.
        /// </summary>
        public void StopAnalysis()
        {
            m_manager.StopRunningJobs();
            m_isRunning = false;
        }

        /// <summary>
        /// Judge the robustness from the simulation result.
        /// </summary>
        private void JudgeResult()
        {
            string xPath = "";
            string yPath = "";
            double xmax = 0.0;
            double xmin = 0.0;
            double ymax = 0.0;
            double ymin = 0.0;
            RAXComboBox.Items.Clear();
            RAYComboBox.Items.Clear();
            List<ParameterRange> pList = ExtractParameter();
            if (pList == null) return;
            int count = 0;
            foreach (ParameterRange r in pList)
            {
                RAXComboBox.Items.Add(r.FullPath);
                RAYComboBox.Items.Add(r.FullPath);
                if (count == 0) {
                    RAXComboBox.SelectedText = r.FullPath;
                    xPath = r.FullPath;
                    xmax = r.Max;
                    xmin = r.Min;
                }
                if (count == 1) {
                    RAYComboBox.SelectedText = r.FullPath;
                    yPath = r.FullPath;
                    ymax = r.Max;
                    ymin = r.Min;
                }
                count++;
            }
            m_zCnt.GraphPane.XAxis.Scale.Max = xmax;
            m_zCnt.GraphPane.XAxis.Scale.Min = xmin;
            m_zCnt.GraphPane.YAxis.Scale.Max = ymax;
            m_zCnt.GraphPane.YAxis.Scale.Min = ymin;
           
            List<JudgementParam> judgeList = ExtractObserved();
            foreach (int jobid in m_manager.SessionList.Keys)
            {
                if (m_manager.SessionList[jobid].Status != JobStatus.FINISHED)
                    continue;
                double x = m_manager.ParameterDic[jobid].GetParameter(xPath);
                double y = m_manager.ParameterDic[jobid].GetParameter(yPath);
                bool isOK = true;
                foreach (JudgementParam p in judgeList)
                {
                    Dictionary<double, double> logList = 
                        m_manager.SessionList[jobid].GetLogData(p.Path);

                    double simTime = Convert.ToDouble(RASimTimeText.Text);
                    double winSize = Convert.ToDouble(RAWinSizeText.Text);
                    if (simTime > winSize)
                    {
                        Dictionary<double, double> tmpList = new Dictionary<double, double>();
                        foreach (double t in logList.Keys)
                        {
                            if (simTime - winSize > t) continue;
                            tmpList.Add(t, logList[t]);
                        }
                        logList.Clear();

                        foreach (double t in tmpList.Keys)
                        {
                            logList.Add(t, tmpList[t]);
                        }
                    }

                    bool rJudge = JudgeResultByRange(logList, p.Max, p.Min, p.Difference);
                    bool pJudge = JudgeResultByFFT(logList, p.Rate);
                    if (rJudge == false || pJudge == false)
                    {
                        isOK = false;
                        break;
                    }
                }
                AddJudgementData(jobid, x, y, isOK);
            }
        }

        /// <summary>
        /// Judge the range of log data for target property.
        /// </summary>
        /// <param name="resDic">log data.</param>
        /// <param name="max">the maximum data of log data.</param>
        /// <param name="min">the minimum data of log data.</param>
        /// <param name="diff">the difference of log data.</param>
        /// <returns>if all condition is OK, return true.</returns>
        private bool JudgeResultByRange(Dictionary<double, double> resDic, double max, double min, double diff)
        {
            bool isFirst = true;
            double minValue = 0.0;
            double maxValue = 0.0;
            foreach (double time in resDic.Keys)
            {
                if (resDic[time] > max || resDic[time] < min) return false;
                if (isFirst)
                {
                    isFirst = false;
                    minValue = resDic[time];
                    maxValue = resDic[time];
                    continue;
                }
                if (minValue > resDic[time]) minValue = resDic[time];
                if (maxValue < resDic[time]) maxValue = resDic[time];
            }

            if (maxValue - minValue > diff) return false;
            return true;
        }

        /// <summary>
        /// judge the FFT result for log data.
        /// </summary>
        /// <param name="resDic">log data.</param>
        /// <param name="rate">FFT rate.</param>
        /// <returns>if all judgement is ok, return true.</returns>
        private bool JudgeResultByFFT(Dictionary<double, double> resDic, double rate)
        {
            double maxFreq = Convert.ToDouble(RAMaxFreqText.Text);
            double minFreq = Convert.ToDouble(RAMinFreqText.Text);
            int maxSize = Convert.ToInt32(RMAMaxData.Text);

            ComplexFourierTransformation cft = new ComplexFourierTransformation();
            int size = 4;
            int divide = 1;
            while (true)
            {
                if (resDic.Count <= size) break;
                if (size >= maxSize)
                {
                    divide = resDic.Count / maxSize + 1;
                    break;
                }
                size = size * 2;
            }
            double[] data = new double[size * 2];
            int i = 0;
            foreach(double d in resDic.Keys)
            {
                if (i % divide != 0) continue;
                data[i*2] = resDic[d];
                data[i*2 + 1] = 0.0;
                i++;
            }

            cft.Convention = TransformationConvention.Matlab; // so we can check MATLAB consistency
            cft.TransformForward(data);

            int count = 0;
            for (int j = 0; j < size; j++)
            {
                double d = Math.Sqrt(data[j * 2] * data[j * 2] + data[j * 2 + 1] * data[j * 2 + 1]);
                if (maxFreq > d && minFreq < d) count++;
            }
            if ((double)count / (double)resDic.Count > rate)
                return true;

            return false;
        }

        /// <summary>
        /// Extract the judgement condition from DataGridView.
        /// </summary>
        /// <returns>the list of judgement condition.</returns>
        private List<JudgementParam> ExtractObserved()
        {
            List<JudgementParam> resList = new List<JudgementParam>();

            for (int i = 0; i < RAObservGridView.Rows.Count; i++)
            {
                string path = RAObservGridView[0, i].Value.ToString();
                double max = Convert.ToDouble(RAObservGridView[1, i].Value);
                double min = Convert.ToDouble(RAObservGridView[2, i].Value);
                double diff = Convert.ToDouble(RAObservGridView[3, i].Value);
                double rate = Convert.ToDouble(RAObservGridView[4, i].Value);

                JudgementParam p = new JudgementParam(path, max, min, diff, rate);
                resList.Add(p);
            }

            return resList;
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

            if (resList.Count < 2)
            {
                String mes = Analysis.s_resources.GetString("ErrParamProp");
                MessageBox.Show(mes, "ERRPR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return resList;
        }

        /// <summary>
        /// Get the list of observed property to judge for analysis.
        /// If there are any problems, this function return null. 
        /// </summary>
        /// <returns>the list of observed property.</returns>
        public List<SaveLoggerProperty> GetObservedDataList()
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
        /// Remove the entry of parameter value.
        /// </summary>
        /// <param name="key">the key of parameter value.</param>
        public void RemoveParamEntry(string key)
        {
            if (m_paramList.ContainsKey(key)) m_paramList.Remove(key);
            else return;

            for (int i = 0; i < RAParamGridView.Rows.Count; i++)
            {
                String pData = RAParamGridView[0, i].Value.ToString();
                if (pData.Equals(key))
                {
                    EcellObject obj = RAParamGridView.Rows[i].Tag as EcellObject;
                    if (obj == null) continue;
                    RAParamGridView.Rows.RemoveAt(i);
                    m_paramList.Remove(key);

                    foreach (EcellData d in obj.Value)
                    {
                        if (key.Equals(d.EntityPath))
                        {
                            d.Committed = true;
                        }
                        break;
                    }
                    DataManager dManager = DataManager.GetDataManager();
                    dManager.DataChanged(obj.modelID, obj.key, obj.type, obj);
                    break;
                }
            }
        }

        /// <summary>
        /// Remove the entry of observed value.
        /// </summary>
        /// <param name="key">the key of observed value.</param>
        public void RemoveObservEntry(string key)
        {
            if (m_observList.ContainsKey(key)) m_observList.Remove(key);
            else return;

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
            AssignParamPopupMenu(r);
            RAParamGridView.Rows.Add(r);
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
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="jobid">the jobidof this parameters.</param>
        /// <param name="x">the value of parameter.</param>
        /// <param name="y">the value of parameter.</param>
        /// <param name="isOK">the flag whether this parameter is robustness.</param>
        public void AddJudgementData(int jobid, double x, double y, bool isOK)
        {
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
                SymbolType.TriangleDown);

            line.Line.Width = 3;
            line.AddPoint(new PointPair(x, y));

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
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
                m_parent.CloseRobustWindow();
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
            dManager.DataChanged(t.modelID, t.key, t.type, t);
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
        private void DragDropObserv(object sender, DragEventArgs e)
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
                    m_observList.Add(d.EntityPath, d);
                }
            }
        }

        /// <summary>
        /// Event to enter in the observe DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragEnterObserv(object sender, DragEventArgs e)
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
        #endregion

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
    }

    /// <summary>
    /// Parameter to judge the robustness.
    /// </summary>
    public class JudgementParam
    {
        private string m_path = "";
        private double m_max = 0.0;
        private double m_min = 0.0;
        private double m_difference = 0.0;
        private double m_rate = 0.0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public JudgementParam()
        {
        }

        /// <summary>
        /// Constructor with initial parameters.
        /// </summary>
        /// <param name="path">entry path to judge the logger data.</param>
        /// <param name="max">the maximum value of logger data.</param>
        /// <param name="min">the minimum value of logger data.</param>
        /// <param name="diff">the difference value of logger data.</param>
        /// <param name="rate">the rate of FFT.</param>
        public JudgementParam(string path, double max, double min, double diff, double rate)
        {
            m_path = path;
            m_max = max;
            m_min = min;
            m_difference = diff;
            m_rate = rate;
        }

        /// <summary>
        /// get/set the entry path to use by the judgement of robustness.
        /// </summary>
        public string Path
        {
            get { return this.m_path; }
            set { this.m_path = value; }
        }

        /// <summary>
        /// get/set the maximum value of logger data.
        /// </summary>
        public double Max
        {
            get { return this.m_max; }
            set { this.m_max = value; }
        }

        /// <summary>
        /// get/set the minimum value of logger data.
        /// </summary>
        public double Min
        {
            get { return this.m_min; }
            set { this.m_min = value; }
        }

        /// <summary>
        /// get/set the differnce value of logger data.
        /// </summary>
        public double Difference
        {
            get { return this.m_difference; }
            set { this.m_difference = value; }
        }

        /// <summary>
        /// get/set the rate of FFT.
        /// </summary>
        public double Rate
        {
            get { return this.m_rate; }
            set { this.m_rate = value; }
        }
    }
}