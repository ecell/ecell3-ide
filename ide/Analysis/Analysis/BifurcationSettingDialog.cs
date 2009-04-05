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
    public partial class BifurcationSettingDialog : Form
    {
        private Analysis m_owner;
        private BifurcationAnalysisParameter m_param;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public BifurcationSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
        }

        /// <summary>
        /// Set the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <param name="p">the parameter set of bifurcation analysis.</param>
        public void SetParameter(BifurcationAnalysisParameter p)
        {            
            bifurcationSimulationTimeTextBox.Text = Convert.ToString(p.SimulationTime);
            bifurcationWindowSizeTextBox.Text = Convert.ToString(p.WindowSize);
            bifurcationMaxInputTextBox.Text = Convert.ToString(p.MaxInput);
            bifurcationMaxFrequencyTextBox.Text = Convert.ToString(p.MaxFreq);
            bifurcationMinFrequencyTextBox.Text = Convert.ToString(p.MinFreq);
            m_param = p;
        }

        /// <summary>
        /// Get the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <returns>the parameter set of bifurcation analysis.</returns>
        public BifurcationAnalysisParameter GetParameter()
        {
            return m_param;
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

            if (bifurcationParameterDataGrid.ColumnCount >= 4)
            {
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = data.Step;
                r.Cells.Add(c4);
            }

            r.Tag = data.Copy();

            bifurcationParameterDataGrid.Rows.Add(r);
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
            bifurcationObservedDataGrid.Rows.Add(r);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EcellObservedData> GetObservedDataList()
        {
            List<EcellObservedData> result = new List<EcellObservedData>();
            for (int i = 0; i < bifurcationObservedDataGrid.Rows.Count; i++)
            {
                EcellObservedData data = bifurcationObservedDataGrid.Rows[i].Tag as EcellObservedData;
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
            for (int i = 0; i < bifurcationParameterDataGrid.Rows.Count; i++)
            {
                EcellParameterData data = bifurcationParameterDataGrid.Rows[i].Tag as EcellParameterData;
                result.Add(data);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLoad(object sender, EventArgs e)
        {
            bifurcationToolTip.SetToolTip(bifurcationSimulationTimeTextBox,
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
            bifurcationToolTip.SetToolTip(bifurcationWindowSizeTextBox, 
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
            bifurcationToolTip.SetToolTip(bifurcationMaxFrequencyTextBox,
                String.Format(MessageResources.CommonToolTipMoreThanUpper, 0.0, MessageResources.NameMinFrequency));
            bifurcationToolTip.SetToolTip(bifurcationMinFrequencyTextBox,
                String.Format(MessageResources.CommonToolTipMoreThanLower, 0.0, MessageResources.NameMaxFrequency));
            bifurcationToolTip.SetToolTip(bifurcationMaxInputTextBox,
                String.Format(MessageResources.CommonToolTipRange, 0, 2097152));     
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowSize_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationWindowSizeTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameWindowSize));
                bifurcationWindowSizeTextBox.Text = Convert.ToString(m_param.WindowSize);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameWindowSize));
                bifurcationWindowSizeTextBox.Text = Convert.ToString(m_param.WindowSize);
                e.Cancel = true;
                return;
            }
            m_param.WindowSize = dummy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SimulationTime_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationSimulationTimeTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameSimulationTime));
                bifurcationSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                bifurcationSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
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
        private void MaxInput_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationMaxInputTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxSample));
                bifurcationMaxInputTextBox.Text = Convert.ToString(m_param.MaxInput);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxSample));
                bifurcationMaxInputTextBox.Text = Convert.ToString(m_param.MaxInput);
                e.Cancel = true;
                return;
            }
            m_param.MaxInput = dummy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxFrequency_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationMaxFrequencyTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxFrequency));
                bifurcationMaxFrequencyTextBox.Text = Convert.ToString(m_param.MaxFreq);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxFrequency));
                bifurcationMaxFrequencyTextBox.Text = Convert.ToString(m_param.MaxFreq);
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
            string text = bifurcationMinFrequencyTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMinFrequency));
                bifurcationMinFrequencyTextBox.Text = Convert.ToString(m_param.MinFreq);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMinFrequency));
                bifurcationMinFrequencyTextBox.Text = Convert.ToString(m_param.MinFreq);
                e.Cancel = true;
                return;
            }
            m_param.MinFreq = dummy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObservedDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            EcellObservedData data = bifurcationObservedDataGrid.Rows[e.RowIndex].Tag as EcellObservedData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value == null ||
                !double.TryParse(bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
            {
                isCorrect = false;
            }
            if (isCorrect && e.ColumnIndex == 4)
            {
                isCorrect = false;
            }
            if (isCorrect && e.ColumnIndex == 1)
            {
                isCorrect = false;
            }
            if (isCorrect && e.ColumnIndex == 2)
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
                        bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Max;
                        break;
                    case 2:
                        bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Min;
                        break;
                    case 3:
                        bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Differ;
                        break;
                    case 4:
                        bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Rate;
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
            EcellParameterData data = bifurcationParameterDataGrid.Rows[e.RowIndex].Tag as EcellParameterData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value == null ||
                !double.TryParse(bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
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
                        bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Max;
                        break;
                    case 2:
                        bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Min;
                        break;
                    case 3:
                        bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Step;
                        break;
                }
            }

        }

        private void BifurcationSettingDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (m_param.SimulationTime <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                e.Cancel = true;
                return;
            }
            if (m_param.WindowSize <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameWindowSize));
                e.Cancel = true;
                return;
            }

            if (m_param.MaxInput <= 0 || m_param.MaxInput > 2097152)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxSample));
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
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameParameterData));
                    e.Cancel = true;
                    return;
                }
            }
        }

    }
}