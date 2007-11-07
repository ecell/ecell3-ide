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

using SessionManager;

using WeifenLuo.WinFormsUI.Docking;
using ZedGraph;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Analysis window for robust analysis.
    /// </summary>
    public partial class RobustAnalysis : DockContent
    {
        private Dictionary<string, EcellData> m_observList = new Dictionary<string, EcellData>();
        private Dictionary<string, EcellData> m_paramList = new Dictionary<string, EcellData>();
        private ZedGraphControl m_zCnt = null;
        private ContextMenuStrip m_cntMenu = null;
        private Analysis m_parent = null;
        private bool m_isRunning = false;
        private SessionManager.SessionManager m_manager;
        private ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResAnalysis));
        private System.Windows.Forms.Timer m_timer;


        /// <summary>
        /// Constructor.
        /// </summary>
        public RobustAnalysis()
        {
            InitializeComponent();
            m_cntMenu = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = m_resources.GetString("ReflectMenuText");
            it.Click += new EventHandler(ReflectMenuClick);
            m_cntMenu.Items.AddRange(new ToolStripItem[] { it });
            RAResultGridView.ContextMenuStrip = m_cntMenu;

            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Interval = 5000;
            m_timer.Tick += new EventHandler(UpdateTimeFire);

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

            InitializeData();
            string d = Util.GetTmpDir();
            d.ToString();

            this.FormClosed += new FormClosedEventHandler(RobustAnalysis_FormClosed);
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
                    SearchAndAddEntry(sObj);
                    if (sObj.M_instances != null)
                    {
                        foreach (EcellObject obj in sObj.M_instances)
                        {
                            SearchAndAddEntry(obj);
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
        private void SearchAndAddEntry(EcellObject obj)
        {
            if (obj.M_value == null) return;
            foreach (EcellData d in obj.M_value)
            {
                if (d.M_isCommit) continue;
                if (m_paramList.ContainsKey(d.M_entityPath)) continue;
                DataGridViewRow r = new DataGridViewRow();
                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = d.M_entityPath;
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
                m_paramList.Add(d.M_entityPath, d);
            }
        }

        /// <summary>
        /// Execute the robust analysis.
        /// Robust analysis include the simulation execution of parameter and 
        /// judgement of simulation results.
        /// </summary>
        public void Execute()
        {
            String tmpDir = m_manager.TmpRootDir;
            int num = Convert.ToInt32(RASampleNumText.Text);
            double simTime = Convert.ToDouble(RASimTimeText.Text);

            string model = "";
            List<string> modelList = DataManager.GetDataManager().GetModelList();
            if (modelList.Count > 0) model = modelList[0];

            List<ParameterRange> paramList = GetParamPropList();
            if (paramList == null) return; 
            List<SaveLoggerProperty> saveList = GetObservedPropList();
            if (saveList == null) return;

            m_manager.SetParameterRange(paramList);
            m_manager.SetLoggerData(saveList);
            m_manager.RunSimParameterRange(tmpDir, model, num, simTime, false);
            m_isRunning = true;

            m_timer.Enabled = true;
            m_timer.Start();

        }

        /// <summary>
        /// Update the status of session at intervals while program is running.
        /// </summary>
        /// <param name="sender">Timer.</param>
        /// <param name="e">EventArgs.</param>
        void UpdateTimeFire(object sender, EventArgs e)
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
                String mes = m_resources.GetString("ErrFindErrorJob");
                DialogResult res = MessageBox.Show(mes, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Cancel)
                {
                    return;
                }
            }
            Judgement();
            String finMes = m_resources.GetString("FinishRAnalysis");
            MessageBox.Show(finMes, "Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Stop the robust analysis.
        /// </summary>
        public void Stop()
        {
            m_manager.StopRunningJobs();
            m_isRunning = false;
        }

        /// <summary>
        /// Judge the robustness from the simulation result.
        /// </summary>
        private void Judgement()
        {
            string xPath = "";
            string yPath = "";
            double xmax = 0.0;
            double xmin = 0.0;
            double ymax = 0.0;
            double ymin = 0.0;
            RAXComboBox.Items.Clear();
            RAYComboBox.Items.Clear();
            List<ParameterRange> pList = GetParamPropList();
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
           

            List<JudgementParam> judgeList = ExtractJudgement();
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

                    bool rJudge = JudgementRange(logList, p.Max, p.Min, p.Difference);
                    if (rJudge == false)
                    {
                        isOK = false;
                        break;
                    }
                    bool pJudge = JudgementFFT(logList, p.Rate);
                    if (pJudge == false)
                    {
                        isOK = false;
                        break;
                    }                
                }
                AddJudgeData(jobid, x, y, isOK);
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
        private bool JudgementRange(Dictionary<double, double> resDic, double max, double min, double diff)
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
        private bool JudgementFFT(Dictionary<double, double> resDic, double rate)
        {
            double maxFreq = Convert.ToDouble(RAMaxFreqText.Text);
            double minFreq = Convert.ToDouble(RAMinFreqText.Text);
            ComplexFourierTransformation cft = new ComplexFourierTransformation();
            int size = 4;
            while (true)
            {
                if (resDic.Count <= size) break;
                size = size * 2;
            }
            double[] data = new double[size * 2];
            int i = 0;
            foreach(double d in resDic.Keys)
            {
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
        private List<JudgementParam> ExtractJudgement()
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
        public List<ParameterRange> GetParamPropList()
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
                String mes = m_resources.GetString("ErrParamProp");
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
        public List<SaveLoggerProperty> GetObservedPropList()
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
                String mes = m_resources.GetString("ErrObservProp");
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

                    foreach (EcellData d in obj.M_value)
                    {
                        if (key.Equals(d.M_entityPath))
                        {
                            d.M_isCommit = true;
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
            if (obj.M_value == null) return;
            foreach (EcellData d in obj.M_value)
            {
                if (m_paramList.ContainsKey(d.M_entityPath))
                {
                    if (!d.M_isCommit) continue;
                    m_paramList.Remove(d.M_entityPath);
                    for (int i = 0; i < RAParamGridView.Rows.Count; i++)
                    {
                        String pData = RAParamGridView[0, i].Value.ToString();
                        if (pData.Equals(d.M_entityPath))
                        {
                            RAParamGridView.Rows.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (d.M_isCommit) continue;
                DataGridViewRow r = new DataGridViewRow();
                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = d.M_entityPath;
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
                RAParamGridView.Rows.Add(r); m_paramList.Add(d.M_entityPath, d);
            }
        }

        /// <summary>
        /// Redraw the result table and graph when axis data is changed.
        /// </summary>
        public void AxisIndexChanged()
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
        public void AddJudgeData(int jobid, double x, double y, bool isOK)
        {
            LineItem line = null;
            if (isOK)
            {
                DataGridViewRow r = new DataGridViewRow();
                DataGridViewCheckBoxCell c0 = new DataGridViewCheckBoxCell();
                c0.Value = true;
                r.Cells.Add(c0);

                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = Convert.ToString(x);
                r.Cells.Add(c1);

                DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                c2.Value = Convert.ToString(y);
                r.Cells.Add(c2);

                r.Tag = jobid;
                RAResultGridView.Rows.Add(r);


                line = m_zCnt.GraphPane.AddCurve(
                    "Result",
                    new PointPairList(),
                    Color.Blue,
                    SymbolType.TriangleDown);
            }
            else
            {
                DataGridViewRow r = new DataGridViewRow();
                DataGridViewCheckBoxCell c0 = new DataGridViewCheckBoxCell();
                c0.Value = false;
                r.Cells.Add(c0);

                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = Convert.ToString(x);
                c1.Style.BackColor = Color.Silver;
                r.Cells.Add(c1);

                DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                c2.Value = Convert.ToString(y);
                c2.Style.BackColor = Color.Silver;
                r.Cells.Add(c2);

                r.Tag = jobid;
                RAResultGridView.Rows.Add(r);


                line = m_zCnt.GraphPane.AddCurve(
                    "Result",
                    new PointPairList(),
                    Color.Red,
                    SymbolType.TriangleDown);
            }
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
        void RobustAnalysis_FormClosed(object sender, FormClosedEventArgs e)
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
        private void YIndexChanged(object sender, EventArgs e)
        {
            AxisIndexChanged();
        }

        /// <summary>
        /// Event to change the X axis.
        /// </summary>
        /// <param name="sender">ComboBox.</param>
        /// <param name="e">EventArgs.</param>
        private void XIndexChanged(object sender, EventArgs e)
        {
            AxisIndexChanged();
        }

        /// <summary>
        /// Reflect the parameter condition to the model property.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ReflectMenuClick(object sender, EventArgs e)
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
                    foreach (EcellData d in obj.M_value)
                    {
                        if (d.M_entityPath.Equals(path))
                        {
                            d.M_value = new EcellValue(param);
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
        private void ParamDragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            DataManager dManager = DataManager.GetDataManager();
            EcellObject t = dManager.GetEcellObject(dobj.ModelID, dobj.Key, dobj.Type);
            foreach (EcellData d in t.M_value)
            {
                if (d.M_entityPath.Equals(dobj.Path))
                {
                    if (d.Max == 0.0 && d.Min == 0.0)
                    {
                        d.Max = Convert.ToDouble(d.M_value.ToString()) * 1.5;
                        d.Min = Convert.ToDouble(d.M_value.ToString()) * 0.5;
                    }
                    d.M_isCommit = false;
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
        private void ParamDragEnter(object sender, DragEventArgs e)
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
        private void ObservDragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            DataManager dManager = DataManager.GetDataManager();
            EcellObject t = dManager.GetEcellObject(dobj.ModelID, dobj.Key, dobj.Type);
            foreach (EcellData d in t.M_value)
            {
                if (d.M_entityPath.Equals(dobj.Path))
                {
                    DataGridViewRow r = new DataGridViewRow();
                    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                    c1.Value = d.M_entityPath;
                    r.Cells.Add(c1);
                    DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                    if (d.Max == 0.0)
                    {
                        c2.Value = Convert.ToDouble(d.M_value.ToString()) * 1.5;
                    }
                    else
                    {
                        c2.Value = d.Max;
                    }
                    r.Cells.Add(c2);
                    DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
                    if (d.Min == 0.0)
                    {
                        c3.Value = Convert.ToDouble(d.M_value.ToString()) * 0.5;
                    }
                    else
                    {
                        c3.Value = d.Min;
                    }
                    r.Cells.Add(c3);
                    DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                    if (d.Max == 0.0 && d.Min == 0.0)
                    {
                        c4.Value = Convert.ToDouble(d.M_value.ToString());
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
                    m_observList.Add(d.M_entityPath, d);
                }
            }
        }

        /// <summary>
        /// Event to enter in the observe DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void ObservDragEnter(object sender, DragEventArgs e)
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
                String mes = m_resources.GetString("ConfirmClose");

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