//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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

using Ecell.Plugin;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Setting Dialog for sensitivity analysis.
    /// </summary>
    public partial class SensitivityAnalysisSettingDialog : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// Plugin object.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// Parameter object for sensitivity analysis.
        /// </summary>
        private SensitivityAnalysisParameter m_param;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="owner">Plugin object.</param>
        public SensitivityAnalysisSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
        }
        #endregion

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

        /// <summary>
        /// Change the status of project.
        /// </summary>
        /// <param name="status">the status of project.</param>
        public void ChangeStatus(ProjectStatus status)
        {
            if (status == ProjectStatus.Loaded)
                executeButton.Enabled = true;
            else
                executeButton.Enabled = false;
        }

        #region Events
        /// <summary>
        /// The event to load the form.
        /// </summary>
        /// <param name="sender">SensitivityAnalysisSettingDialog.</param>
        /// <param name="e">EventArgs</param>
        private void FormLoad(object sender, EventArgs e)
        {
            sensitivityToolTip.SetToolTip(sensitivityStepTextBox,
                String.Format(MessageResources.CommonToolTipIntMoreThan, 0));
            sensitivityToolTip.SetToolTip(sensitivityAbsolutePerturbationTextBox, 
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
            sensitivityToolTip.SetToolTip(sensitivityRelativePerturbationTextBox,
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
        }

        /// <summary>
        /// Validating the value of step count.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
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

        /// <summary>
        /// Validating the value of absolute perturbation.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
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

        /// <summary>
        /// Validating the value of relative perturbation.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
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

        /// <summary>
        /// Click the button to execute the analysis.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void ExecuteButtonClick(object sender, EventArgs e)
        {
            executeButton.Enabled = false;
            System.Threading.Thread.Sleep(1000);
            if (m_param.Step <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameStepNum));
                executeButton.Enabled = true;
                return;
            }

            if (m_param.RelativePerturbation <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRelativePert));
                executeButton.Enabled = true;
                return;
            }

            if (m_param.AbsolutePerturbation <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameAbsolutePert));
                executeButton.Enabled = true;
                return;
            }
            m_owner.ExecuteSensitivityAnalysis();
            executeButton.Enabled = true;
        }
        #endregion
    }
}