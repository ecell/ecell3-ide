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
using EcellLib.Job;
using ZedGraph;
using Formulator;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;
using EcellLib.Objects;

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
        private Dictionary<string, EcellObservedData> m_observList = new Dictionary<string, EcellObservedData>();
        /// <summary>
        /// The dictionary of the parameter data.
        /// </summary>
        private Dictionary<string, EcellParameterData> m_parameterList = new Dictionary<string, EcellParameterData>();
        /// <summary>
        /// The parent plugin include this form.
        /// </summary>
        private Analysis m_owner = null;
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
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public AnalysisWindow(Analysis owner)
        {
            this.m_owner = owner;
            InitializeComponent();

            RARandomCheck.Checked = true;
            RAMatrixCheck.Checked = false;

            this.FormClosed += new FormClosedEventHandler(CloseRobustAnalysisForm);

            InitializeData();
            this.Text = MessageResAnalysis.AnalysisWindow;
            this.TabText = this.Text;
        }

        #region Commons
        /// <summary>
        /// Refresh the value of parameters.
        /// </summary>
        public void InitializeData()
        {
            ClearEntry();
            DataManager manager = m_owner.DataManager;

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

            m_observList.Clear();
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

        ///// <summary>
        ///// Get the list of property to set the initial value for analysis.
        ///// If there are any problems, this function return null.
        ///// </summary>
        ///// <returns>the list of parameter property.</returns>
        //public List<ParameterRange> ExtractParameterForRobustAnalysis()
        //{
        //    List<ParameterRange> resList = new List<ParameterRange>();

        //    for (int i = 0; i < RAParamGridView.Rows.Count; i++)
        //    {
        //        string path = RAParamGridView[0, i].Value.ToString();
        //        double max = Convert.ToDouble(RAParamGridView[1, i].Value);
        //        double min = Convert.ToDouble(RAParamGridView[2, i].Value);
        //        double step = Convert.ToDouble(RAParamGridView[3, i].Value);

        //        if (min > max) continue;
        //        ParameterRange p = new ParameterRange(path, min, max, step);
        //        resList.Add(p);
        //    }

        //    return resList;
        //}

        /// <summary>
        /// Add the parameter data.
        /// </summary>
        /// <param name="obj">object include the parameter data.</param>
        /// <param name="d">the parameter data.</param>
        public void AddParameterEntry(EcellObject obj, EcellData d)
        {
            if (!m_owner.DataManager.IsContainsParameterData(d.EntityPath)) return;
            EcellParameterData data = m_owner.DataManager.GetParameterData(d.EntityPath);

            SetParameterData(data);
        }

        /// <summary>
        /// Add the parameter data.
        /// </summary>
        /// <param name="obj">object include the parameter data.</param>
        /// <param name="d">the parameter data.</param>
        public void AddObservedEntry(EcellObject obj, EcellData d)
        {
            if (!m_owner.DataManager.IsContainsObservedData(d.EntityPath)) return;
            EcellObservedData data = m_owner.DataManager.GetObservedData(d.EntityPath);

            SetObservedData(data);
        }

        /// <summary>
        /// Add the parameter data.
        /// </summary>
        /// <param name="key">the entry key of entry data.</param>
        public void RemoveParameterEntry(string key)
        {
            RemoveRobustAnalysisParameterEntry(key);
            RemoveParameterEstimateEntry(key);
            RemoveBifurcationAnalysisParameterEntry(key);
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
                AddParameterEntry(obj, d);
                AddObservedEntry(obj, d);
            }
        }

        /// <summary>
        /// The event sequence when the user add and change the parameter data.
        /// </summary>
        /// <param name="data">The parameter data.</param>
        public void SetParameterData(EcellParameterData data)
        {
            if (m_parameterList.ContainsKey(data.Key))
            {
                EditParameterRow(RAParamGridView, data);
                EditParameterRow(BAParameterGridView, data);
                m_parameterList[data.Key] = data;

                return;
            }

            AddParameterRow(RAParamGridView, data);
            AddParameterRow(BAParameterGridView, data);
            AddParameterRow(PEParamGridView, data);
            m_parameterList.Add(data.Key, data);
        }

        /// <summary>
        /// Add the row of parameter data to DataGridView.
        /// </summary>
        /// <param name="v">DataGridView to add the row.</param>
        /// <param name="data">the parameter data.</param>
        private void AddParameterRow(DataGridView v, EcellParameterData data)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = data.Key;
            r.Cells.Add(c1);

            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = data.Max;
            r.Cells.Add(c2);

            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = data.Min;
            r.Cells.Add(c3);

            if (v.ColumnCount >= 4)
            {
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = data.Step;
                r.Cells.Add(c4);
            }

            r.Tag = data;
            v.Rows.Add(r);
        }

        /// <summary>
        /// Edit the parameter data in DataGridView.
        /// </summary>
        /// <param name="v">DataGridView.</param>
        /// <param name="data">the edited data.</param>
        private void EditParameterRow(DataGridView v, EcellParameterData data)
        {
            foreach (DataGridViewRow r in v.Rows)
            {
                if (r.Tag == null) continue;
                EcellParameterData obj = r.Tag as EcellParameterData;
                if (obj == null) continue;

                if (!obj.Equals(data)) continue;
                r.Cells[1].Value = data.Max;
                r.Cells[2].Value = data.Min;
                if (r.Cells.Count >= 4)
                    r.Cells[3].Value = data.Step;

                r.Tag = data;

                return;
            }
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed parameter data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            RemoveParameterRow(RAParamGridView, data);
            RemoveParameterRow(BAParameterGridView, data);
            RemoveParameterRow(PEParamGridView, data);
        }

        private void RemoveParameterRow(DataGridView v, EcellParameterData data)
        {
            foreach (DataGridViewRow r in v.Rows)
            {
                if (r.Tag == null) continue;
                EcellParameterData obj = r.Tag as EcellParameterData;
                if (obj == null) continue;

                if (!obj.Equals(data)) continue;
                v.Rows.Remove(r);
                return;
            }
        }

        /// <summary>
        /// The event sequence when the user set and change the observed data.
        /// </summary>
        /// <param name="data">The observed data.</param>
        public void SetObservedData(EcellObservedData data)
        {
            if (m_observList.ContainsKey(data.Key))
            {
                EditObservedRow(RAObservGridView, data);
                EditObservedRow(BAObservedGridView, data);
                m_observList[data.Key] = data;

                return;
            }

            AddObservedRow(RAObservGridView, data);
            AddObservedRow(BAObservedGridView, data);
            m_observList.Add(data.Key, data);
        }

        /// <summary>
        /// Add the row of observed data to DataGridView.
        /// </summary>
        /// <param name="v">DataGridView.</param>
        /// <param name="data">the observed data.</param>
        private void AddObservedRow(DataGridView v, EcellObservedData data)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = data.Key;
            r.Cells.Add(c1);

            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = data.Max;
            r.Cells.Add(c2);

            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = data.Min;
            r.Cells.Add(c3);

            DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
            c4.Value = data.Differ;
            r.Cells.Add(c4);

            DataGridViewTextBoxCell c5 = new DataGridViewTextBoxCell();
            c5.Value = data.Rate;
            r.Cells.Add(c5);

            r.Tag = data;
            v.Rows.Add(r);
        }

        /// <summary>
        /// Edit the observed data in DataGridView.
        /// </summary>
        /// <param name="v">DataGridView.</param>
        /// <param name="data">the observed data.</param>
        private void EditObservedRow(DataGridView v, EcellObservedData data)
        {
            foreach (DataGridViewRow r in v.Rows)
            {
                if (r.Tag == null) continue;
                EcellObservedData obj = r.Tag as EcellObservedData;
                if (obj == null) continue;

                if (!obj.Equals(data)) continue;
                r.Cells[1].Value = data.Max;
                r.Cells[2].Value = data.Min;
                r.Cells[3].Value = data.Differ;
                r.Cells[4].Value = data.Rate;

                r.Tag = data;

                return;
            }
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public void RemoveObservedData(EcellObservedData data)
        {
            RemoveObservedRow(RAObservGridView, data);
            RemoveObservedRow(BAObservedGridView, data);
        }

        /// <summary>
        /// The event sequence when the data entry is removed.
        /// </summary>
        /// <param name="v">the data entry in DataGridView.</param>
        /// <param name="data">the observed data.</param>
        private void RemoveObservedRow(DataGridView v, EcellObservedData data)
        {
            foreach (DataGridViewRow r in v.Rows)
            {
                if (r.Tag == null) continue;
                EcellObservedData obj = r.Tag as EcellObservedData;
                if (obj == null) continue;

                if (!obj.Equals(data)) continue;
                v.Rows.Remove(r);
                return;
            }
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
        /// Extract the observed data from the row data.
        /// </summary>
        /// <param name="r">The row of data.</param>
        /// <returns>the observed data.</returns>
        private EcellObservedData ExtractObservedDataFromRow(DataGridViewRow r)
        {
            string key = Convert.ToString(r.Cells[0].Value.ToString());
            double max = Convert.ToDouble(r.Cells[1].Value.ToString());
            double min = Convert.ToDouble(r.Cells[2].Value.ToString());
            double diff = Convert.ToDouble(r.Cells[3].Value.ToString());
            double rate = Convert.ToDouble(r.Cells[4].Value.ToString());

            return new EcellObservedData(key, max, min, diff, rate);
        }

        /// <summary>
        /// Extract the parameter data from the row data.
        /// </summary>
        /// <param name="r">The row of data.</param>
        /// <returns>the parameter data.</returns>
        private EcellParameterData ExtractParameterDataFromRow(DataGridViewRow r)
        {
            string key = Convert.ToString(r.Cells[0].Value.ToString());
            double max = Convert.ToDouble(r.Cells[1].Value.ToString());
            double min = Convert.ToDouble(r.Cells[2].Value.ToString());
            double step = 0.0;
            if (r.Cells.Count >= 4)
                step = Convert.ToDouble(r.Cells[3].Value.ToString());

            return new EcellParameterData(key, max, min, step);
        }

        ///// <summary>
        ///// Extract the judgement condition from DataGridView.
        ///// </summary>
        ///// <returns>the list of judgement condition.</returns>
        //public List<AnalysisJudgementParam> ExtractBifurcationObserved()
        //{
        //    List<AnalysisJudgementParam> resList = new List<AnalysisJudgementParam>();

        //    for (int i = 0; i < BAObservedGridView.Rows.Count; i++)
        //    {
        //        string path = BAObservedGridView[0, i].Value.ToString();
        //        double max = Convert.ToDouble(BAObservedGridView[1, i].Value);
        //        double min = Convert.ToDouble(BAObservedGridView[2, i].Value);
        //        double diff = Convert.ToDouble(BAObservedGridView[3, i].Value);
        //        double rate = Convert.ToDouble(BAObservedGridView[4, i].Value);

        //        AnalysisJudgementParam p = new AnalysisJudgementParam(path, max, min, diff, rate);
        //        resList.Add(p);
        //    }

        //    return resList;
        //}

        ///// <summary>
        ///// Get the list of property to set the initial value for analysis.
        ///// If there are any problems, this function return null.
        ///// </summary>
        ///// <returns>the list of parameter property.</returns>
        //public List<EcellParameterData> ExtractParameterForBifurcation()
        //{
        //    List<EcellParameterData> resList = new List<EcellParameterData>();

        //    for (int i = 0; i < BAParameterGridView.Rows.Count; i++)
        //    {
        //        string path = BAParameterGridView[0, i].Value.ToString();
        //        double max = Convert.ToDouble(BAParameterGridView[1, i].Value);
        //        double min = Convert.ToDouble(BAParameterGridView[2, i].Value);
        //        double step = Convert.ToDouble(BAParameterGridView[3, i].Value);

        //        if (step <= 0.0)
        //            step = (max - min) / 10.0;

        //        if (min > max) continue;
        //        EcellParameterData p = new EcellParameterData(path, min, max, step);
        //        resList.Add(p);
        //    }

        //    return resList;
        //}

        ///// <summary>
        ///// Get the list of observed property to judge for analysis.
        ///// If there are any problems, this function return null. 
        ///// </summary>
        ///// <returns>the list of observed property.</returns>
        //public List<SaveLoggerProperty> GetBifurcationObservedDataList()
        //{
        //    SessionManager.SessionManager manager = m_owner.SessionManager;
        //    List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();

        //    for (int i = 0; i < BAObservedGridView.Rows.Count; i++)
        //    {
        //        String dir = manager.TmpDir;
        //        string path = BAObservedGridView[0, i].Value.ToString();
        //        double start = 0.0;
        //        double end = Convert.ToDouble(BASimTimeText.Text);
        //        SaveLoggerProperty p = new SaveLoggerProperty(path, start, end, dir);

        //        resList.Add(p);
        //    }

        //    if (resList.Count < 1)
        //    {
        //        String mes = Analysis.s_resources.GetString("ErrObservProp");
        //        MessageBox.Show(mes, "ERRPR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }

        //    return resList;
        //}
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

        ///// <summary>
        ///// Get the list of property to set the initial value for analysis.
        ///// If there are any problems, this function return null.
        ///// </summary>
        ///// <returns>the list of parameter property.</returns>
        //public List<EcellParameterData> ExtractParameterForParameterEstimation()
        //{
        //    List<EcellParameterData> resList = new List<EcellParameterData>();

        //    for (int i = 0; i < PEParamGridView.Rows.Count; i++)
        //    {
        //        string path = PEParamGridView[0, i].Value.ToString();
        //        double max = Convert.ToDouble(PEParamGridView[1, i].Value);
        //        double min = Convert.ToDouble(PEParamGridView[2, i].Value);
        //        double step = 0.0;

        //        if (min > max) continue;
        //        EcellParameterData p = new EcellParameterData(path, min, max, step);
        //        resList.Add(p);
        //    }

        //    return resList;
        //}

        ///// <summary>
        ///// Get the list of observed property to judge for analysis.
        ///// If there are any problems, this function return null. 
        ///// </summary>
        ///// <returns>the list of observed property.</returns>
        //public List<SaveLoggerProperty> GetParameterObservedDataList()
        //{
        //    SessionManager.SessionManager manager = m_owner.SessionManager;
        //    List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();

        //    String dir = manager.TmpDir;
        //    double start = 0.0;
        //    double end = Convert.ToDouble(RASimTimeText.Text);
        //    string formulator = PEEstmationFormula.Text;
        //    string[] ele = formulator.Split(new char[] { '+', '-', '*' });
        //    for (int i = 0; i < ele.Length; i++)
        //    {
        //        string element = ele[i].Replace(" ", "");
        //        if (element.StartsWith("Variable") ||
        //            element.StartsWith("Process"))
        //            resList.Add(new SaveLoggerProperty(element, start, end, dir));
        //    }
        //    return resList;
        //}


        #endregion

        /// <summary>
        /// Remove the entry of parameter value.
        /// </summary>
        /// <param name="key">the key of parameter value.</param>
        public void RemoveBifurcationAnalysisParameterEntry(string key)
        {
            for (int i = 0; i < BAParameterGridView.Rows.Count; i++)
            {
                string pData = BAParameterGridView[0, i].Value.ToString();
                if (!pData.Equals(key))
                    continue;
                EcellObject obj = BAParameterGridView.Rows[i].Tag as EcellObject;
                if (obj == null)
                    continue;

                BAParameterGridView.Rows.RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove the entry of parameter value.
        /// </summary>
        /// <param name="key">the key of parameter value.</param>
        public void RemoveParameterEstimateEntry(string key)
        {
            for (int i = 0; i < PEParamGridView.Rows.Count; i++)
            {
                string pData = PEParamGridView[0, i].Value.ToString();
                if (!pData.Equals(key))
                    continue;
                EcellObject obj = PEParamGridView.Rows[i].Tag as EcellObject;
                if (obj == null)
                    continue;

                PEParamGridView.Rows.RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove the entry of parameter value.
        /// </summary>
        /// <param name="key">the key of parameter value.</param>
        public void RemoveRobustAnalysisParameterEntry(string key)
        {
            for (int i = 0; i < RAParamGridView.Rows.Count; i++)
            {
                string pData = RAParamGridView[0, i].Value.ToString();
                if (!pData.Equals(key))
                    continue;
                EcellObject obj = RAParamGridView.Rows[i].Tag as EcellObject;
                if (obj == null)
                    continue;

                RAParamGridView.Rows.RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove the entry of observed value.
        /// </summary>
        /// <param name="key">the key of observed value.</param>
        public void RemoveObservEntry(string key)
        {
            EcellObservedData data = new EcellObservedData(key, 0.0);
            m_owner.DataManager.RemoveObservedData(data);
        }

        #region Events
        /// <summary>
        /// Event to close this window.
        /// </summary>
        /// <param name="sender">This form.</param>
        /// <param name="e">FormClosedEventArgs</param>
        void CloseRobustAnalysisForm(object sender, FormClosedEventArgs e)
        {
            if (m_owner != null)
            {
                m_owner.CloseAnalysisWindow();
            }
            m_owner = null;
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
            object obj = e.Data.GetData(typeof(EcellLib.Objects.EcellDragObject));
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            try
            {
                DataManager dManager = m_owner.DataManager;
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
                        dManager.SetParameterData(new EcellParameterData(d.EntityPath, d.Max, d.Min, d.Step));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// Event to enter in the parameter DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragEnterParam(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData(typeof(EcellLib.Objects.EcellDragObject));
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Event to enter in the observe DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void DragEnterObservedData(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData(typeof(EcellLib.Objects.EcellDragObject));
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
        private void DragDropObservedData(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData(typeof(EcellLib.Objects.EcellDragObject));
            
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            try
            {
                DataManager dManager = m_owner.DataManager;
                EcellObject t = dManager.GetEcellObject(dobj.ModelID, dobj.Key, dobj.Type);
                foreach (EcellData d in t.Value)
                {
                    if (d.EntityPath.Equals(dobj.Path))
                    {
                        EcellObservedData data = new EcellObservedData(d.EntityPath, d.Value.CastToDouble());
                        m_owner.DataManager.SetObservedData(data);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
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
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && m.WParam.ToInt32() == SC_CLOSE)
            {

                if (Util.ShowOKCancelDialog(MessageResAnalysis.ConfirmClose))


                {
                    this.Dispose();
                }
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
            DataManager manager = m_owner.DataManager;
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

        /// <summary>
        /// The event sequence when cancel button is clicked.
        /// </summary>
        /// <param name="sender">object(Button).</param>
        /// <param name="e">EventArgs.</param>
        private void AECancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The event sequence when ok button is clicked.
        /// </summary>
        /// <param name="sender">object(Button).</param>
        /// <param name="e">EventArgs.</param>
        private void AEOKButtonClicked(object sender, EventArgs e)
        {
            m_owner.SetBifurcationAnalysisParameter(GetBifurcationAnalysisPrameter());
            m_owner.SetParameterEstimationParameter(GetParameterEstimationParameter());
            m_owner.SetRobustAnalysisParameter(GetRobustAnalysisParameter());
            m_owner.SetSensitivityAnalysisParameter(GetSensitivityAnalysisParameter());

            this.Close();
        }

        /// <summary>
        /// The event sequence when user finish to edit the cell of parameter data.
        /// </summary>
        /// <param name="sender">object(DataGridView).</param>
        /// <param name="e">DataGridViewEcellEventArgs.</param>
        private void ParameterCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView v = sender as DataGridView;
            if (v == null) return;
            DataGridViewRow r = v.Rows[e.RowIndex];
            if (r.Tag == null) return;
            EcellParameterData obj = r.Tag as EcellParameterData;
            if (obj == null) return;

            EcellParameterData p = ExtractParameterDataFromRow(r);
            m_owner.DataManager.SetParameterData(p);
        }

        /// <summary>
        /// The event sequence when user finish to edit the cell of observed data.
        /// </summary>
        /// <param name="sender">object(DataGridView).</param>
        /// <param name="e">DataGridViewEcellEventArgs.</param>
        private void ObserbedCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView v = sender  as DataGridView;
            if (v == null) return;
            DataGridViewRow r = v.Rows[e.RowIndex];
            if (r.Tag == null) return;
            EcellObservedData obj = r.Tag as EcellObservedData;
            if (obj == null) return;

            EcellObservedData p = ExtractObservedDataFromRow(r);
            m_owner.DataManager.SetObservedData(p);
        }
        #endregion
    }

}
