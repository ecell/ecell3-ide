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
        }

        /// <summary>
        /// Get the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <returns>the parameter set of bifurcation analysis.</returns>
        public BifurcationAnalysisParameter GetParameter()
        {
            BifurcationAnalysisParameter p = new BifurcationAnalysisParameter();
            p.SimulationTime = Convert.ToDouble(bifurcationSimulationTimeTextBox.Text);
            p.WindowSize = Convert.ToDouble(bifurcationWindowSizeTextBox.Text);
            p.MaxInput = Convert.ToInt32(bifurcationMaxInputTextBox.Text);
            p.MaxFreq = Convert.ToDouble(bifurcationMaxFrequencyTextBox.Text);
            p.MinFreq = Convert.ToDouble(bifurcationMinFrequencyTextBox.Text);

            return p;
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

            r.Tag = data;

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

            r.Tag = data;
            bifurcationObservedDataGrid.Rows.Add(r);
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
    }
}