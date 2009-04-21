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
    /// <summary>
    /// 
    /// </summary>
    public partial class RobustAnalysisSettingDialog : Form
    {
        private Analysis m_owner;
        private RobustAnalysisParameter m_param;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
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
            if (p.IsRandomCheck) robustAnalysisRandomRadioButton.Checked = true;
            else robustAnalysisMatrixRadioButton.Checked = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        public void SetParameterDataList(Dictionary<string, EcellData> dic)
        {
            foreach (string key in dic.Keys)
            {
                EcellParameterData d = m_owner.DataManager.GetParameterData(key);
                SetParameterData(d);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        public void SetObservedDataList(Dictionary<string, EcellData> dic)
        {
            foreach (string key in dic.Keys)
            {
                EcellObservedData d = m_owner.DataManager.GetObservedData(key);
                SetObservedData(d);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            }
            else
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            }
            else
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

        private void RobustAnalysisSettingDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (m_param.SimulationTime <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                e.Cancel = true;
                return;
            }
            if (m_param.WinSize <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameWindowSize));
                e.Cancel = true;
                return;
            }

            if (m_param.MaxData <= 0 || m_param.MaxData > 2097152)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxSample));
                e.Cancel = true;
                return;
            }

            if (m_param.SampleNum <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSampleNum));
                e.Cancel = true;
                return;
            }
            
            if (m_param.MinFreq <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMinFrequency));
                e.Cancel = true;
                return;
            }
            if (m_param.MaxFreq <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxFrequency));
                e.Cancel = true;
                return;
            }
            if (m_param.MaxFreq < m_param.MinFreq)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger, MessageResources.NameMaxFrequency, MessageResources.NameMinFrequency));
                e.Cancel = true;
                return;
            }

            List<EcellParameterData> plist = GetParameterDataList();
            foreach (EcellParameterData p in plist)
            {
                if (p.Max < p.Min || p.Step < 0.0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameParameterData));
                    e.Cancel = true;
                    return;
                }
            }

            List<EcellObservedData> olist = GetObservedDataList();
            foreach (EcellObservedData o in olist)
            {
                if (o.Max < o.Min || o.Rate < 0.0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameObservedData));
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
