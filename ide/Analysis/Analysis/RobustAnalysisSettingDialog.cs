using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ecell.IDE;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.Analysis
{
    public partial class RobustAnalysisSettingDialog : Form
    {
        private Analysis m_owner;
        private RobustAnalysisParameter m_param;
        public RobustAnalysisSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
        }

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
            if (p.IsRandomCheck) robustAnalysisRandomCheckBox.Checked = true;
            else robustAnalysisMatrixCheckBox.Checked = true;
        }

        public void SetParameterDataList(Dictionary<string, EcellData> dic)
        {
            foreach (string key in dic.Keys)
            {
                EcellParameterData d = m_owner.DataManager.GetParameterData(key);
                SetParameterData(d);
            }
        }

        public void SetObservedDataList(Dictionary<string, EcellData> dic)
        {
            foreach (string key in dic.Keys)
            {
                EcellObservedData d = m_owner.DataManager.GetObservedData(key);
                SetObservedData(d);
            }
        }

        public void SetParameterData(EcellParameterData data)
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

            if (robustAnalysisParameterDataGrid.ColumnCount >= 4)
            {
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = data.Step;
                r.Cells.Add(c4);
            }

            r.Tag = data.Copy();

            robustAnalysisParameterDataGrid.Rows.Add(r);
        }

        public void SetObservedData(EcellObservedData data)
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

            r.Tag = data.Copy();
            robustAnalysisObservedDataGrid.Rows.Add(r);
        }

        /// <summary>
        /// Event when CheckBox of random is changed.
        /// If CheckBox of random is true, TextBox input the number of sample is active.
        /// </summary>
        /// <param name="sender">CheckBox.</param>
        /// <param name="e">EventArgs.</param>
        private void ChangeRARandomCheck(object sender, EventArgs e)
        {
            if (robustAnalysisRandomCheckBox.Checked == true)
            {
                robustAnalysisMatrixCheckBox.Checked = false;
                robustAnalysisSampleNumberTextBox.ReadOnly = false;
                m_param.IsRandomCheck = false;
            }
            else
            {
                robustAnalysisMatrixCheckBox.Checked = true;
                robustAnalysisSampleNumberTextBox.ReadOnly = true;
                m_param.IsRandomCheck = true;
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
            if (robustAnalysisMatrixCheckBox.Checked == true)
            {
                if (robustAnalysisRandomCheckBox.Checked == true)
                {
                    robustAnalysisRandomCheckBox.Checked = false;
                    robustAnalysisMaxSampleTextBox.ReadOnly = true;
                }
            }
            else
            {
                if (robustAnalysisRandomCheckBox.Checked == false)
                {
                    robustAnalysisRandomCheckBox.Checked = true;
                    robustAnalysisMaxSampleTextBox.ReadOnly = false;
                    
                }
            }
        }

        private void FormLoad(object sender, EventArgs e)
        {
            robustToolTip.SetToolTip(robustAnalysisSimulationTimeTextBox, MessageResources.ToolTipSimulationTime);
            robustToolTip.SetToolTip(robustAnalysisWindowSizeTextBox, MessageResources.ToolTipWindowSize);
            robustToolTip.SetToolTip(robustAnalysisMaxSampleTextBox, MessageResources.ToolTipMaxInputSize);
            robustToolTip.SetToolTip(robustAnalysisMaxFrequencyTextBox, MessageResources.ToolTipMaxFFT);
            robustToolTip.SetToolTip(robustAnalysisMinFrequencyTextBox, MessageResources.ToolTipMinFFT);
            robustToolTip.SetToolTip(robustAnalysisSampleNumberTextBox, MessageResources.ToolTipSampleNumber);
            robustToolTip.SetToolTip(groupBox4, MessageResources.ToolTipParameterGrid);
        }

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
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                robustAnalysisSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            m_param.SimulationTime = dummy;
        }

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
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                robustAnalysisWindowSizeTextBox.Text = Convert.ToString(m_param.WinSize);
                e.Cancel = true;
                return;
            }
            m_param.WinSize = dummy;
        }

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
            if (!Int32.TryParse(text, out dummy) || dummy <= 0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                robustAnalysisSampleNumberTextBox.Text = Convert.ToString(m_param.SampleNum);
                e.Cancel = true;
                return;
            }
            m_param.SampleNum = dummy;
        }

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
            if (!Int32.TryParse(text, out dummy) || dummy <= 0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                robustAnalysisMaxSampleTextBox.Text = Convert.ToString(m_param.MaxData);
                e.Cancel = true;
                return;
            }
            m_param.MaxData = dummy;
        }

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
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0 || dummy < m_param.MinFreq)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                robustAnalysisMaxFrequencyTextBox.Text = Convert.ToString(m_param.MaxFreq);
                e.Cancel = true;
                return;
            }
            m_param.MaxFreq = dummy;
        }

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
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0 || dummy > m_param.MaxFreq)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                robustAnalysisMinFrequencyTextBox.Text = Convert.ToString(m_param.MinFreq);
                e.Cancel = true;
                return;
            }
            m_param.MinFreq = dummy;
        }

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

        private void ObservedDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            EcellObservedData data = robustAnalysisObservedDataGrid.Rows[e.RowIndex].Tag as EcellObservedData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (!double.TryParse(robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
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
            }
            else
            {
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

        private void ParameterDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            EcellParameterData data = robustAnalysisParameterDataGrid.Rows[e.RowIndex].Tag as EcellParameterData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (!double.TryParse(robustAnalysisParameterDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
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
            }
            else
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Max;
                        break;
                    case 2:
                        robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Min;
                        break;
                    case 3:
                        robustAnalysisObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Step;
                        break;
                }
            }
        }
    }
}