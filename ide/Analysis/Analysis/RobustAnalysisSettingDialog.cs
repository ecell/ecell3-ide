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

using Ecell.IDE;
using Ecell.Objects;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Setting Dialog for robust analysis.
    /// </summary>
    public partial class RobustAnalysisSettingDialog : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// Plugin object.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// Parameter object for robust analysis.
        /// </summary>
        private RobustAnalysisParameter m_param;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="owner">Plugin object.</param>
        public RobustAnalysisSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
        }
        #endregion

        /// <summary>
        /// Get the robust analysis parameter set in this form.
        /// </summary>
        /// <returns>the parameter of robust analysis.</returns>
        public RobustAnalysisParameter GetParameter()
        {
            return m_param;
        }

        /// <summary>
        /// Set the robust analysis parameter.
        /// </summary>
        /// <param name="p">the parameter of robust analysis.</param>
        public void SetParameter(RobustAnalysisParameter p)
        {
            m_param = p;
            robustAnalysisMaxSampleTextBox.Text = Convert.ToString(p.SampleNum);
            robustAnalysisSimulationTimeTextBox.Text = Convert.ToString(p.SimulationTime);
            robustAnalysisMaxSampleTextBox.Text = Convert.ToString(p.MaxData);
            robustAnalysisMaxFrequencyTextBox.Text = Convert.ToString(p.MaxFreq);
            robustAnalysisMinFrequencyTextBox.Text = Convert.ToString(p.MinFreq);
            robustAnalysisWindowSizeTextBox.Text = Convert.ToString(p.WinSize);
            if (p.IsRandomCheck) robustAnalysisRandomRadioButton.Checked = true;
            else robustAnalysisMatrixRadioButton.Checked = true;
        }

        /// <summary>
        /// Remove the parameter data.
        /// </summary>
        /// <param name="data">the removed parameter data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            foreach (DataGridViewRow r in robustAnalysisParameterDataGrid.Rows)
            {
                string fullPN = r.Cells[paramFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    robustAnalysisParameterDataGrid.Rows.Remove(r);
                    return;
                }
            }
        }

        /// <summary>
        /// Remove the observed data.
        /// </summary>
        /// <param name="data">the removed observed data.</param>
        public void RemoveObservedData(EcellObservedData data)
        {
            foreach (DataGridViewRow r in robustAnalysisObservedDataGrid.Rows)
            {
                string fullPN = r.Cells[observedFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    robustAnalysisObservedDataGrid.Rows.Remove(r);
                    return;
                }
            }
        }

        /// <summary>
        /// Set the parameter data.
        /// </summary>
        /// <param name="data">the set parameter data.</param>
        public void SetParameterData(EcellParameterData data)
        {
            foreach (DataGridViewRow r1 in robustAnalysisParameterDataGrid.Rows)
            {
                string fullPN = r1.Cells[paramFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    // max
                    int index = dataGridViewTextBoxColumn2.Index;
                    r1.Cells[index].Value = data.Max;

                    // min
                    index = dataGridViewTextBoxColumn3.Index;
                    r1.Cells[index].Value = data.Min;

                    // step
                    index = dataGridViewTextBoxColumn4.Index;
                    r1.Cells[index].Value = data.Step;

                    return;
                }
            }
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

            if (robustAnalysisParameterDataGrid.ColumnCount >= 4)
            {
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = data.Step;
                r.Cells.Add(c4);
            }

            r.Tag = data.Copy();

            robustAnalysisParameterDataGrid.Rows.Add(r);
        }

        /// <summary>
        /// Set the observed data.
        /// </summary>
        /// <param name="data">the set observed data.</param>
        public void SetObservedData(EcellObservedData data)
        {
            foreach (DataGridViewRow r1 in robustAnalysisObservedDataGrid.Rows)
            {
                string fullPN = r1.Cells[observedFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    // max
                    int index = dataGridViewTextBoxColumn6.Index;
                    r1.Cells[index].Value = data.Max;

                    // min
                    index = dataGridViewTextBoxColumn7.Index;
                    r1.Cells[index].Value = data.Min;

                    // differ
                    index = dataGridViewTextBoxColumn8.Index;
                    r1.Cells[index].Value = data.Differ;

                    // rate
                    index = dataGridViewTextBoxColumn9.Index;
                    r1.Cells[index].Value = data.Rate;

                    return;
                }
            }

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

            r.Tag = data.Copy();
            robustAnalysisObservedDataGrid.Rows.Add(r);
        }

        /// <summary>
        /// Get the list of observed data.
        /// </summary>
        /// <returns>The list of EcellObservedData.</returns>
        public List<EcellObservedData> GetObservedDataList()
        {
            List<EcellObservedData> result = new List<EcellObservedData>();
            for (int i = 0; i < robustAnalysisObservedDataGrid.Rows.Count; i++)
            {
                EcellObservedData data = robustAnalysisObservedDataGrid.Rows[i].Tag as EcellObservedData;
                result.Add(data);
            }
            return result;
        }

        /// <summary>
        /// Get the list of parameter data.
        /// </summary>
        /// <returns>The list of EcellParameterData.</returns>
        public List<EcellParameterData> GetParameterDataList()
        {
            List<EcellParameterData> result = new List<EcellParameterData>();
            for (int i = 0; i < robustAnalysisParameterDataGrid.Rows.Count; i++)
            {
                EcellParameterData data = robustAnalysisParameterDataGrid.Rows[i].Tag as EcellParameterData;
                result.Add(data);
            }
            return result;
        }


        #region Events
        /// <summary>
        /// Event when CheckBox of random is changed.
        /// If CheckBox of random is true, TextBox input the number of sample is active.
        /// </summary>
        /// <param name="sender">CheckBox.</param>
        /// <param name="e">EventArgs.</param>
        private void ChangeRARandomCheck(object sender, EventArgs e)
        {
            if (robustAnalysisRandomRadioButton.Checked == true)
            {
                if (robustAnalysisMatrixRadioButton.Checked == true)
                {
                    robustAnalysisMatrixRadioButton.Checked = false;
                    robustAnalysisMaxSampleTextBox.ReadOnly = false;
                }
            }
            else
            {
                if (robustAnalysisMatrixRadioButton.Checked == false)
                {
                    robustAnalysisMatrixRadioButton.Checked = true;
                    robustAnalysisMaxSampleTextBox.ReadOnly = true;
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
            if (robustAnalysisMatrixRadioButton.Checked == true)
            {
                robustAnalysisRandomRadioButton.Checked = false;
                robustAnalysisSampleNumberTextBox.ReadOnly = true;
                m_param.IsRandomCheck = false;
            }
            else
            {
                robustAnalysisRandomRadioButton.Checked = true;
                robustAnalysisSampleNumberTextBox.ReadOnly = false;
                m_param.IsRandomCheck = true;
            }
        }
        /// <summary>
        /// The event to load the form.
        /// </summary>
        /// <param name="sender">RobustAnalysisSettingDialog.</param>
        /// <param name="e">EventArgs</param>
        private void FormLoad(object sender, EventArgs e)
        {
            robustToolTip.SetToolTip(robustAnalysisSimulationTimeTextBox, 
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
            robustToolTip.SetToolTip(robustAnalysisWindowSizeTextBox, 
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
            robustToolTip.SetToolTip(robustAnalysisMaxSampleTextBox,
                String.Format(MessageResources.CommonToolTipRange, 0, 2097152));
            robustToolTip.SetToolTip(robustAnalysisMaxFrequencyTextBox,
                String.Format(MessageResources.CommonToolTipMoreThanUpper, 0.0, MessageResources.NameMinFrequency));
            robustToolTip.SetToolTip(robustAnalysisMinFrequencyTextBox, 
                String.Format(MessageResources.CommonToolTipMoreThanLower, 0.0, MessageResources.NameMaxFrequency));
            robustToolTip.SetToolTip(robustAnalysisSampleNumberTextBox,
                String.Format(MessageResources.CommonToolTipIntMoreThan, 0));
        }

        /// <summary>
        /// Validating the value of simulation time.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void SimulationTime_Validating(object sender, CancelEventArgs e)
        {
            string text = robustAnalysisSimulationTimeTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameSimulationTime));
                robustAnalysisSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                robustAnalysisSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            m_param.SimulationTime = dummy;
        }

        /// <summary>
        /// Validating the value of window size.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void WindowSize_Validating(object sender, CancelEventArgs e)
        {
            string text = robustAnalysisWindowSizeTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameWindowSize));
                robustAnalysisWindowSizeTextBox.Text = Convert.ToString(m_param.WinSize);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameWindowSize));
                robustAnalysisWindowSizeTextBox.Text = Convert.ToString(m_param.WinSize);
                e.Cancel = true;
                return;
            }
            m_param.WinSize = dummy;
        }
        /// <summary>
        /// Validating the value of sample number.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void SampleNumber_Validating(object sender, CancelEventArgs e)
        {
            string text = robustAnalysisSampleNumberTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameSampleNum));
                robustAnalysisSampleNumberTextBox.Text = Convert.ToString(m_param.SampleNum);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSampleNum));
                robustAnalysisSampleNumberTextBox.Text = Convert.ToString(m_param.SampleNum);
                e.Cancel = true;
                return;
            }
            m_param.SampleNum = dummy;
        }

        /// <summary>
        /// Validating the value of max input for FFT.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void MaxInput_Validating(object sender, CancelEventArgs e)
        {
            string text = robustAnalysisMaxSampleTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxSample));
                robustAnalysisMaxSampleTextBox.Text = Convert.ToString(m_param.MaxData);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxSample));
                robustAnalysisMaxSampleTextBox.Text = Convert.ToString(m_param.MaxData);
                e.Cancel = true;
                return;
            }
            m_param.MaxData = dummy;
        }

        /// <summary>
        /// Validating the value of max frequency for FFT.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void MaxFrequency_Validating(object sender, CancelEventArgs e)
        {
            string text = robustAnalysisMaxFrequencyTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxFrequency));
                robustAnalysisMaxFrequencyTextBox.Text = Convert.ToString(m_param.MaxFreq);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxFrequency));
                robustAnalysisMaxFrequencyTextBox.Text = Convert.ToString(m_param.MaxFreq);
                e.Cancel = true;
                return;
            }
            m_param.MaxFreq = dummy;
        }

        /// <summary>
        /// Validating the value of min frequency of FFT.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void MinFrequency_Validating(object sender, CancelEventArgs e)
        {
            string text = robustAnalysisMinFrequencyTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMinFrequency));
                robustAnalysisMinFrequencyTextBox.Text = Convert.ToString(m_param.MinFreq);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMinFrequency));
                robustAnalysisMinFrequencyTextBox.Text = Convert.ToString(m_param.MinFreq);
                e.Cancel = true;
                return;
            }
            m_param.MinFreq = dummy;
        }

        /// <summary>
        /// Change the property of data on DataGridView of the observed data.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void ObservedDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            EcellObservedData data = robustAnalysisObservedDataGrid.Rows[e.RowIndex].Tag as EcellObservedData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value == null ||
                !double.TryParse(robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
            {
                isCorrect = false;
            }

            if (isCorrect)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        data.Max = dummy;
                        break;
                    case 2:
                        data.Min = dummy;
                        break;
                    case 3:
                        data.Differ = dummy;
                        break;
                    case 4:
                        data.Rate = dummy;
                        break;
                }
                try
                {
                    m_owner.NotifyObservedDataChanged(data);
                }
                catch (Exception)
                {
                    isCorrect = false;
                }
            }
            if (!isCorrect)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue,
                        MessageResources.NameObservedData));
                switch (e.ColumnIndex)
                {
                    case 1:
                        robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Max;
                        break;
                    case 2:
                        robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Min;
                        break;
                    case 3:
                        robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Differ;
                        break;
                    case 4:
                        robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Rate;
                        break;
                }
            }
        }

        /// <summary>
        /// Change the property of data on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void ParameterDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            EcellParameterData data = robustAnalysisParameterDataGrid.Rows[e.RowIndex].Tag as EcellParameterData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (robustAnalysisParameterDataGrid[e.ColumnIndex, e.RowIndex].Value == null ||
                !double.TryParse(robustAnalysisParameterDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
            {
                isCorrect = false;
            }

            if (isCorrect)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        data.Max = dummy;
                        break;
                    case 2:
                        data.Min = dummy;
                        break;
                    case 3:
                        data.Step = dummy;
                        break;
                }
                try
                {
                    m_owner.NotifyParameterDataChanged(data);
                }
                catch (Exception)
                {
                    isCorrect = false;
                }
            }
            if (!isCorrect)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue,
                    MessageResources.NameParameterData));
                switch (e.ColumnIndex)
                {
                    case 1:
                        robustAnalysisParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Max;
                        break;
                    case 2:
                        robustAnalysisParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Min;
                        break;
                    case 3:
                        robustAnalysisParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Step;
                        break;
                }
            }

        }

        /// <summary>
        /// Click the button to execute the analysis.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void ExecuteButtonClick(object sender, EventArgs e)
        {
            executeButton.Enabled = false;
            if (m_param.SimulationTime <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                executeButton.Enabled = true;
                return;
            }
            if (m_param.WinSize <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameWindowSize));
                executeButton.Enabled = true;
                return;
            }

            if (m_param.MaxData <= 0 || m_param.MaxData > 2097152)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxSample));
                executeButton.Enabled = true;
                return;
            }

            if (m_param.SampleNum <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSampleNum));
                executeButton.Enabled = true;
                return;
            }

            if (m_param.MinFreq <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMinFrequency));
                executeButton.Enabled = true;
                return;
            }
            if (m_param.MaxFreq <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxFrequency));
                executeButton.Enabled = true;
                return;
            }
            if (m_param.MaxFreq < m_param.MinFreq)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger, MessageResources.NameMaxFrequency, MessageResources.NameMinFrequency));
                executeButton.Enabled = true;
                return;
            }

            List<EcellParameterData> plist = GetParameterDataList();
            foreach (EcellParameterData p in plist)
            {
                if (p.Max < p.Min || p.Step < 0.0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameParameterData));
                    executeButton.Enabled = true;
                    return;
                }
            }

            List<EcellObservedData> olist = GetObservedDataList();
            foreach (EcellObservedData o in olist)
            {
                if (o.Max < o.Min || o.Rate < 0.0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameObservedData));
                    executeButton.Enabled = true;
                    return;
                }
            }
            m_owner.ExecuteRobustAnalysis();
            executeButton.Enabled = true;
        }

        /// <summary>
        /// Opening the ContextMenuStrip of DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">ContextMenuStrip</param>
        /// <param name="e">CancelEventArgs</param>
        private void ParamContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (robustAnalysisParameterDataGrid.SelectedCells.Count <= 0)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Opening the ContextMenuStrip of DataGridView of the observed data.
        /// </summary>
        /// <param name="sender">ContextMenuStrip.</param>
        /// <param name="e">CancelEventArgs</param>
        private void ObservedContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (robustAnalysisObservedDataGrid.SelectedCells.Count <= 0)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Click the delete menu on DataGridView of the observed data.
        /// </summary>
        /// <param name="sender">MenuToolStripItem</param>
        /// <param name="e">EventArgs</param>
        private void DeleteRObservedClick(object sender, EventArgs e)
        {            
            List<string> delList = new List<string>();
            foreach (DataGridViewCell c in robustAnalysisObservedDataGrid.SelectedCells)
            {
                string fullPN = robustAnalysisObservedDataGrid.Rows[c.RowIndex].Cells[observedFullPNColumn.Index].Value.ToString();
                if (!delList.Contains(fullPN))
                    delList.Add(fullPN);
            }

            foreach (string r in delList)
            {
                m_owner.Environment.DataManager.RemoveObservedData(
                    new EcellObservedData(r, 0.0));
            }
        }

        /// <summary>
        /// Click the delete menu on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">MenuToolStripItem</param>
        /// <param name="e">EventArgs</param>
        private void DeleteRParamClick(object sender, EventArgs e)
        {            
            List<string> delList = new List<string>();
            foreach (DataGridViewCell c in robustAnalysisParameterDataGrid.SelectedCells)
            {
                string fullPN = robustAnalysisParameterDataGrid.Rows[c.RowIndex].Cells[paramFullPNColumn.Index].Value.ToString();
                if (!delList.Contains(fullPN))
                    delList.Add(fullPN);
            }

            foreach (string r in delList)
            {
                m_owner.Environment.DataManager.RemoveParameterData(
                    new EcellParameterData(r, 0.0));
            }
        }

        /// <summary>
        /// Enter the drag object on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void ParamDataDragEnter(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Drop the parameter data on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DragEventArgs</param>
        private void ParamDataDragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            foreach (EcellDragEntry ent in dobj.Entries)
            {
                EcellObject t = m_owner.DataManager.GetEcellObject(dobj.ModelID, ent.Key, ent.Type);
                foreach (EcellData d in t.Value)
                {
                    if (d.EntityPath.Equals(ent.Path) && d.Settable && d.Value.IsDouble)
                    {
                        EcellParameterData data =
                            new EcellParameterData(d.EntityPath, Double.Parse(d.Value.ToString()));
                        m_owner.Environment.DataManager.SetParameterData(data);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Enter the drag object on DataGridView of the observed data.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void ObservedDragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Drop the parameter data on DataGridView of the observed data.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DragEventArgs</param>
        private void ObservedDragEnter(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            foreach (EcellDragEntry ent in dobj.Entries)
            {
                EcellObject t = m_owner.DataManager.GetEcellObject(dobj.ModelID, ent.Key, ent.Type);
                foreach (EcellData d in t.Value)
                {
                    if (d.EntityPath.Equals(ent.Path) && d.Logable && d.Value.IsDouble)
                    {
                        EcellObservedData data =
                            new EcellObservedData(d.EntityPath, Double.Parse(d.Value.ToString()));
                        m_owner.Environment.DataManager.SetObservedData(data);
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
