//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Form to display the parameter of Parameter Estimation.
    /// </summary>
    public partial class ParameterEstimationAdvancedSettingDialog : Form
    {
        private bool m_isCancel = false;
        private SimplexCrossoverParameter m_param;
        /// <summary>
        /// Constructor.
        /// </summary>
        public ParameterEstimationAdvancedSettingDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the parameter of Parameter Estimation.
        /// </summary>
        /// <param name="param"></param>
        public void SetParameter(SimplexCrossoverParameter param)
        {
            PEMTextBox.Text = Convert.ToString(param.M);
            PEUpsilonTextBox.Text =  Convert.ToString(param.Upsilon);
            PEM0TextBox.Text =  Convert.ToString(param.Initial);
            PEKTextBox.Text =  Convert.ToString(param.K);
            PEMaxRateTextBox.Text = Convert.ToString(param.Max);
            m_param = param;
        }

        /// <summary>
        /// Get the parameter of Parameter Estimation.
        /// </summary>
        /// <returns></returns>
        public SimplexCrossoverParameter GetParam()
        {
            return m_param;
        }

        #region Events
        private void M_Validating(object sender, CancelEventArgs e)
        {
            string text = PEMTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameM));
                PEMTextBox.Text = Convert.ToString(m_param.M);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameM));
                PEMTextBox.Text = Convert.ToString(m_param.M);
                e.Cancel = true;
                return;
            }
            m_param.M = dummy;
        }

        private void Upsilon_Validating(object sender, CancelEventArgs e)
        {
            string text = PEUpsilonTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameUpsilon));
                PEUpsilonTextBox.Text = Convert.ToString(m_param.Upsilon);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameUpsilon));
                PEUpsilonTextBox.Text = Convert.ToString(m_param.Upsilon);
                e.Cancel = true;
                return;
            }
            m_param.Upsilon = dummy;
        }

        private void M0_Validating(object sender, CancelEventArgs e)
        {
            string text = PEM0TextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameM0));
                PEM0TextBox.Text = Convert.ToString(m_param.Initial);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameM0));
                PEM0TextBox.Text = Convert.ToString(m_param.Initial);
                e.Cancel = true;
                return;
            }
            m_param.Initial = dummy;
        }

        private void K_Validating(object sender, CancelEventArgs e)
        {
            string text = PEKTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameK));
                PEKTextBox.Text = Convert.ToString(m_param.K);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameK));
                PEKTextBox.Text = Convert.ToString(m_param.K);
                e.Cancel = true;
                return;
            }
            m_param.K = dummy;
        }

        private void MaxRate_Validating(object sender, CancelEventArgs e)
        {
            string text = PEMaxRateTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxRate));
                PEMaxRateTextBox.Text = Convert.ToString(m_param.Max);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxRate));
                PEMaxRateTextBox.Text = Convert.ToString(m_param.Max);
                e.Cancel = true;
                return;
            }
            m_param.Max = dummy;
        }

        #endregion

        private void ParameterEstimationAdvancedSettingDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (m_param.Max <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxRate));
                e.Cancel = true;
                return;
            }
            if (m_param.K <= 1.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameK));
                e.Cancel = true;
                return;
            }
            if (m_param.Initial <= 1.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameM0));
                e.Cancel = true;
                return;
            }
            if (m_param.Upsilon <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameUpsilon));
                e.Cancel = true;
                return;
            }
            if (m_param.M <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameM));
                e.Cancel = true;
                return;
            }
        }
    }
}