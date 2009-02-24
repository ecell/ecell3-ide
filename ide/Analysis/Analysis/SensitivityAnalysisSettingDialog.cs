using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SensitivityAnalysisSettingDialog : Form
    {
        private Analysis m_owner;
        private SensitivityAnalysisParameter m_param;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public SensitivityAnalysisSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
        }

        /// <summary>
        /// Get the sensitivity analysis parameter set in this form.
        /// </summary>
        /// <returns>the parameter of sensitivity analysis.</returns>
        public SensitivityAnalysisParameter GetParameter()
        {
            return m_param;
        }

        /// <summary>
        /// Set the parameter of sensitivity analysis to this form.
        /// </summary>
        /// <param name="p">the parameter of sensitivity analysis.</param>
        public void SetParameter(SensitivityAnalysisParameter p)
        {
            sensitivityStepTextBox.Text = Convert.ToString(p.Step);
            sensitivityRelativePerturbationTextBox.Text = Convert.ToString(p.RelativePerturbation);
            sensitivityAbsolutePerturbationTextBox.Text = Convert.ToString(p.AbsolutePerturbation);
            m_param = p;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            sensitivityToolTip.SetToolTip(sensitivityStepTextBox, MessageResources.ToolTipStep);
            sensitivityToolTip.SetToolTip(sensitivityAbsolutePerturbationTextBox, MessageResources.ToolTipAbsolutePert);
            sensitivityToolTip.SetToolTip(sensitivityRelativePerturbationTextBox, MessageResources.ToolTipRelativePert);
        }

        private void Step_Validating(object sender, CancelEventArgs e)
        {
            string text = sensitivityStepTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameStepNum));
                sensitivityStepTextBox.Text = Convert.ToString(m_param.Step);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameStepNum));
                sensitivityStepTextBox.Text = Convert.ToString(m_param.Step);
                e.Cancel = true;
                return;
            }
            m_param.Step = dummy;
        }

        private void AbsolutePerturbation_Validating(object sender, CancelEventArgs e)
        {
            string text = sensitivityAbsolutePerturbationTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameAbsolutePert));
                sensitivityAbsolutePerturbationTextBox.Text = Convert.ToString(m_param.AbsolutePerturbation);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameStepNum));
                sensitivityAbsolutePerturbationTextBox.Text = Convert.ToString(m_param.AbsolutePerturbation);
                e.Cancel = true;
                return;
            }
            m_param.AbsolutePerturbation = dummy;
        }

        private void RelativePerturbation_Validating(object sender, CancelEventArgs e)
        {
            string text = sensitivityRelativePerturbationTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameRelativePert));
                sensitivityRelativePerturbationTextBox.Text = Convert.ToString(m_param.RelativePerturbation);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRelativePert));
                sensitivityRelativePerturbationTextBox.Text = Convert.ToString(m_param.RelativePerturbation);
                e.Cancel = true;
                return;
            }
            m_param.RelativePerturbation = dummy;
        }

        private void SensitivityAnalysisSettingDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (m_param.Step <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameStepNum));
                e.Cancel = true;
                return;
            }

            if (m_param.RelativePerturbation <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRelativePert));
                e.Cancel = true;
                return;
            }

            if (m_param.AbsolutePerturbation <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameAbsolutePert));
                e.Cancel = true;
                return;
            }
        }
    }
}