using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.Analysis
{
    public partial class SensitivityAnalysisSettingDialog : Form
    {
        private Analysis m_owner;
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
            SensitivityAnalysisParameter p = new SensitivityAnalysisParameter();
            p.Step = Convert.ToInt32(sensitivityStepTextBox.Text);
            p.RelativePerturbation = Convert.ToDouble(sensitivityRelativePerturbationTextBox.Text);
            p.AbsolutePerturbation = Convert.ToDouble(sensitivityAbsolutePerturbationTextBox.Text);

            return p;
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
        }

        private void FormLoad(object sender, EventArgs e)
        {
            sensitivityToolTip.SetToolTip(sensitivityStepTextBox, MessageResources.ToolTipStep);
            sensitivityToolTip.SetToolTip(sensitivityAbsolutePerturbationTextBox, MessageResources.ToolTipAbsolutePert);
            sensitivityToolTip.SetToolTip(sensitivityRelativePerturbationTextBox, MessageResources.ToolTipRelativePert);
        }



    }
}