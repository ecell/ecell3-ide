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
            RobustAnalysisParameter p = new RobustAnalysisParameter();
            p.SampleNum = Convert.ToInt32(robustAnalysisMaxSampleTextBox.Text);
            p.SimulationTime = Convert.ToDouble(robustAnalysisSimulationTimeTextBox.Text);
            p.IsRandomCheck = robustAnalysisRandomCheckBox.Checked;
            p.MaxData = Convert.ToInt32(robustAnalysisMaxSampleTextBox.Text);
            p.MaxFreq = Convert.ToDouble(robustAnalysisMaxFrequencyTextBox.Text);
            p.MinFreq = Convert.ToDouble(robustAnalysisMinFrequencyTextBox.Text);
            p.WinSize = Convert.ToDouble(robustAnalysisWindowSizeTextBox.Text);

            return p;
        }

        /// <summary>
        /// Set the robust analysis parameter.
        /// </summary>
        /// <param name="p">the parameter of robust analysis.</param>
        public void SetParameter(RobustAnalysisParameter p)
        {
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

            r.Tag = data;

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

            r.Tag = data;
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
                if (robustAnalysisMatrixCheckBox.Checked == true)
                {
                    robustAnalysisMatrixCheckBox.Checked = false;
                    robustAnalysisMaxSampleTextBox.ReadOnly = false;
                }
            }
            else
            {
                if (robustAnalysisMatrixCheckBox.Checked == false)
                {
                    robustAnalysisMatrixCheckBox.Checked = true;
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

    }
}