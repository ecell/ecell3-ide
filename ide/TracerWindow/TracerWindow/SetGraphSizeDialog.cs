//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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

namespace Ecell.IDE.Plugins.TracerWindow
{
    public partial class SetGraphSizeDialog : Form
    {
        #region Fields
        private bool m_isDefault;
        private YAxisSettings m_setting;
        #endregion

        #region Accessors
        public bool IsDefault
        {
            get { return m_isDefault; }
            set { m_isDefault = value; }
        }

        public YAxisSettings Settings
        {
            get { return m_setting; }
            set { this.m_setting = value; }
        }
        #endregion

        public SetGraphSizeDialog(bool isDefault, YAxisSettings setting)
        {
            InitializeComponent();
            m_isDefault = isDefault;
            m_setting = setting;

            defaultCheckBox.Checked = m_isDefault;
            yMaxAutoCheckBox.Checked = m_setting.IsAutoMaxY;
            yMinAutoCheckBox.Checked = m_setting.IsAutoMinY;
            yMaxTextBox.Text = m_setting.YMax.ToString();
            yMinTextBox.Text = m_setting.YMin.ToString();
            y2MaxAutoCheckBox.Checked = m_setting.IsAutoMaxY2;
            y2MinAutoCheckBox.Checked = m_setting.IsAutoMinY2;
            y2MaxTextBox.Text = m_setting.Y2Max.ToString();
            y2MinTextBox.Text = m_setting.Y2Min.ToString();

            yMaxAutoCheckBox.Enabled = !m_isDefault;
            yMinAutoCheckBox.Enabled = !m_isDefault;
            yMaxTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMaxY;
            yMinTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMinY;
            y2MaxAutoCheckBox.Enabled = !m_isDefault;
            y2MinAutoCheckBox.Enabled = !m_isDefault;
            y2MaxTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMaxY2;
            y2MinTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMinY2;
        }

        #region Events
        private void DefaultCheckChanged(object sender, EventArgs e)
        {
            m_isDefault = defaultCheckBox.Checked;

            yMaxAutoCheckBox.Enabled = !m_isDefault;
            yMinAutoCheckBox.Enabled = !m_isDefault;
            y2MaxAutoCheckBox.Enabled = !m_isDefault;
            y2MinAutoCheckBox.Enabled = !m_isDefault;
            yMaxTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMaxY;
            yMinTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMinY;
            y2MaxTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMaxY2;
            y2MinTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMinY2;
        }

        private void MaxAutoCheckChanged(object sender, EventArgs e)
        {
            m_setting.IsAutoMaxY = yMaxAutoCheckBox.Checked;

            yMaxTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMaxY;
        }

        private void MinAutoCheckChanged(object sender, EventArgs e)
        {
            m_setting.IsAutoMinY = yMinAutoCheckBox.Checked;

            yMinTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMinY;
        }

        private void MaxAuto2CheckChanged(object sender, EventArgs e)
        {
            m_setting.IsAutoMaxY2 = y2MaxAutoCheckBox.Checked;

            y2MaxTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMaxY2;
        }

        private void MinAuto2CheckChanged(object sender, EventArgs e)
        {
            m_setting.IsAutoMinY2 = y2MinAutoCheckBox.Checked;

            y2MinTextBox.Enabled = !m_isDefault && !m_setting.IsAutoMinY2;
        }


        private void YMaxValidating(object sender, CancelEventArgs e)
        {
            string value = yMaxTextBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, yMaxAutoCheckBox.Text));
                e.Cancel = true;
                yMaxTextBox.Text = m_setting.YMax.ToString();
                return;
            }

            double dummy = 0.0;
            if (!Double.TryParse(value, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                yMaxTextBox.Text = m_setting.YMax.ToString();
                return;
            }
            m_setting.YMax = dummy;
        }

        private void YMinValidating(object sender, CancelEventArgs e)
        {
            string value = yMinTextBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, yMinAutoCheckBox.Text));
                e.Cancel = true;
                yMinTextBox.Text = m_setting.YMin.ToString();
                return;
            }

            double dummy = 0.0;
            if (!Double.TryParse(value, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                yMinTextBox.Text = m_setting.YMin.ToString();
                return;
            }
            m_setting.YMin = dummy;
        }

        private void Y2MaxValidating(object sender, CancelEventArgs e)
        {
            string value = y2MaxTextBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, y2MaxAutoCheckBox.Text));
                e.Cancel = true;
                y2MaxTextBox.Text = m_setting.Y2Max.ToString();
                return;
            }

            double dummy = 0.0;
            if (!Double.TryParse(value, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                y2MaxTextBox.Text = m_setting.Y2Max.ToString();
                return;
            }
            m_setting.Y2Max = dummy;
        }

        private void Y2MinValidating(object sender, CancelEventArgs e)
        {
            string value = y2MinTextBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, y2MinAutoCheckBox.Text));
                e.Cancel = true;
                y2MinTextBox.Text = m_setting.Y2Min.ToString();
                return;
            }

            double dummy = 0.0;
            if (!Double.TryParse(value, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                y2MinTextBox.Text = m_setting.Y2Min.ToString();
                return;
            }
            m_setting.Y2Min = dummy;
        }

        private void GraphSizeDialogClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (m_setting.YMax < m_setting.YMin)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                return;
            }
            if (m_setting.Y2Max < m_setting.Y2Min)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                return;
            }
        }
        #endregion
    }
}
