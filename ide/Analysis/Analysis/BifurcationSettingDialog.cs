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
    public partial class BifurcationSettingDialog : Form
    {
        private Analysis m_owner;
        private BifurcationAnalysisParameter m_param;

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

            if (bifurcationParameterDataGrid.ColumnCount >= 4)
            {
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = data.Step;
                r.Cells.Add(c4);
            }

            r.Tag = data.Copy();

            bifurcationParameterDataGrid.Rows.Add(r);
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
            bifurcationObservedDataGrid.Rows.Add(r);
        }

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

        private void FormLoad(object sender, EventArgs e)
        {
            bifurcationToolTip.SetToolTip(bifurcationSimulationTimeTextBox, MessageResources.ToolTipSimulationTime);
            bifurcationToolTip.SetToolTip(bifurcationWindowSizeTextBox, MessageResources.ToolTipWindowSize);
            bifurcationToolTip.SetToolTip(bifurcationMaxFrequencyTextBox, MessageResources.ToolTipMaxFFT);
            bifurcationToolTip.SetToolTip(bifurcationMinFrequencyTextBox, MessageResources.ToolTipMinFFT);
            bifurcationToolTip.SetToolTip(groupBox3, MessageResources.ToolTipParameterGrid);
            bifurcationToolTip.SetToolTip(groupBox4, MessageResources.ToolTipObservedGrid);
            bifurcationToolTip.SetToolTip(bifurcationMaxInputTextBox, MessageResources.ToolTipMaxInputSize);            
        }

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
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                bifurcationWindowSizeTextBox.Text = Convert.ToString(m_param.WindowSize);
                e.Cancel = true;
                return;
            }
            m_param.WindowSize = dummy;
        }

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
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                bifurcationSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            m_param.SimulationTime = dummy;
        }

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
            if (!Int32.TryParse(text, out dummy) || dummy <= 0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                bifurcationMaxInputTextBox.Text = Convert.ToString(m_param.MaxInput);
                e.Cancel = true;
                return;
            }
            m_param.MaxInput = dummy;
        }

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
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0 || dummy < m_param.MinFreq)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                bifurcationMaxFrequencyTextBox.Text = Convert.ToString(m_param.MaxFreq);
                e.Cancel = true;
                return;
            }
            m_param.MaxFreq = dummy;
        }

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
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0 || dummy > m_param.MaxFreq)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                bifurcationMinFrequencyTextBox.Text = Convert.ToString(m_param.MinFreq);
                e.Cancel = true;
                return;
            }
            m_param.MinFreq = dummy;
        }
    }
}