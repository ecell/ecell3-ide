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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ecell.IDE;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// TracerConfigDialog
    /// </summary>
    public class TracerConfigurationPage : PropertyDialogPage
    {
        #region Fields
        private int m_plotNum;
        private double m_redrawInterval;
        private TextBox intervalTextBox;
        private TextBox numberTextBox;
        private Label label2;
        private TextBox YMinTextBox;
        private TextBox Y2MinTextBox;
        private CheckBox YMaxCheckBox;
        private CheckBox YMinCheckBox;
        private TextBox YMaxTextBox;
        private CheckBox Y2MaxCheckBox;
        private TextBox Y2MaxTextBox;
        private CheckBox Y2MinCheckBox;
        private TracerWindow m_owner;
        private YAxisSettings m_settings;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="plotNum"></param>
        /// <param name="redrawInt"></param>
        public TracerConfigurationPage(TracerWindow owner, int plotNum, double redrawInt, YAxisSettings setting)
        {
            InitializeComponent();
            m_owner = owner;
            m_plotNum = plotNum;
            m_redrawInterval = redrawInt;
            m_settings = setting;

            numberTextBox.Text = plotNum.ToString();
            intervalTextBox.Text = redrawInt.ToString();

            YMaxCheckBox.Checked = setting.IsAutoMaxY;
            YMinCheckBox.Checked = setting.IsAutoMinY;
            Y2MaxCheckBox.Checked = setting.IsAutoMaxY2;
            Y2MinCheckBox.Checked = setting.IsAutoMinY2;

            YMaxTextBox.Text = setting.YMax.ToString();
            YMinTextBox.Text = setting.YMin.ToString();
            Y2MaxTextBox.Text = setting.Y2Max.ToString();
            Y2MinTextBox.Text = setting.Y2Min.ToString();

            YMaxTextBox.Enabled = !setting.IsAutoMaxY;
            YMinTextBox.Enabled = !setting.IsAutoMinY;
            Y2MaxTextBox.Enabled = !setting.IsAutoMaxY2;
            Y2MinTextBox.Enabled = !setting.IsAutoMinY2;
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.Label label4;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TracerConfigurationPage));
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.numberTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.YMinTextBox = new System.Windows.Forms.TextBox();
            this.Y2MinTextBox = new System.Windows.Forms.TextBox();
            this.YMaxCheckBox = new System.Windows.Forms.CheckBox();
            this.YMinCheckBox = new System.Windows.Forms.CheckBox();
            this.YMaxTextBox = new System.Windows.Forms.TextBox();
            this.Y2MaxCheckBox = new System.Windows.Forms.CheckBox();
            this.Y2MaxTextBox = new System.Windows.Forms.TextBox();
            this.Y2MinCheckBox = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // intervalTextBox
            // 
            resources.ApplyResources(this.intervalTextBox, "intervalTextBox");
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.RedrawInterval_Validating);
            // 
            // numberTextBox
            // 
            resources.ApplyResources(this.numberTextBox, "numberTextBox");
            this.numberTextBox.Name = "numberTextBox";
            this.numberTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.PlotNumber_Validating);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // YMinTextBox
            // 
            resources.ApplyResources(this.YMinTextBox, "YMinTextBox");
            this.YMinTextBox.Name = "YMinTextBox";
            this.YMinTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.YMinValidating);
            // 
            // Y2MinTextBox
            // 
            resources.ApplyResources(this.Y2MinTextBox, "Y2MinTextBox");
            this.Y2MinTextBox.Name = "Y2MinTextBox";
            this.Y2MinTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Y2MinValidating);
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // YMaxCheckBox
            // 
            resources.ApplyResources(this.YMaxCheckBox, "YMaxCheckBox");
            this.YMaxCheckBox.Name = "YMaxCheckBox";
            this.YMaxCheckBox.UseVisualStyleBackColor = true;
            this.YMaxCheckBox.CheckedChanged += new System.EventHandler(this.YMaxAutoCheckedChanged);
            // 
            // YMinCheckBox
            // 
            resources.ApplyResources(this.YMinCheckBox, "YMinCheckBox");
            this.YMinCheckBox.Name = "YMinCheckBox";
            this.YMinCheckBox.UseVisualStyleBackColor = true;
            this.YMinCheckBox.CheckedChanged += new System.EventHandler(this.YMinAutoCheckedChanged);
            // 
            // YMaxTextBox
            // 
            resources.ApplyResources(this.YMaxTextBox, "YMaxTextBox");
            this.YMaxTextBox.Name = "YMaxTextBox";
            this.YMaxTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.YMaxValidating);
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // Y2MaxCheckBox
            // 
            resources.ApplyResources(this.Y2MaxCheckBox, "Y2MaxCheckBox");
            this.Y2MaxCheckBox.Name = "Y2MaxCheckBox";
            this.Y2MaxCheckBox.UseVisualStyleBackColor = true;
            this.Y2MaxCheckBox.CheckedChanged += new System.EventHandler(this.Y2MaxCheckedChanged);
            // 
            // Y2MaxTextBox
            // 
            resources.ApplyResources(this.Y2MaxTextBox, "Y2MaxTextBox");
            this.Y2MaxTextBox.Name = "Y2MaxTextBox";
            this.Y2MaxTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Y2MaxValidating);
            // 
            // Y2MinCheckBox
            // 
            resources.ApplyResources(this.Y2MinCheckBox, "Y2MinCheckBox");
            this.Y2MinCheckBox.Name = "Y2MinCheckBox";
            this.Y2MinCheckBox.UseVisualStyleBackColor = true;
            this.Y2MinCheckBox.CheckedChanged += new System.EventHandler(this.Y2MinCheckedChanged);
            // 
            // TracerConfigurationPage
            // 
            this.Controls.Add(this.Y2MinCheckBox);
            this.Controls.Add(this.Y2MaxTextBox);
            this.Controls.Add(this.Y2MaxCheckBox);
            this.Controls.Add(label7);
            this.Controls.Add(label6);
            this.Controls.Add(this.YMaxTextBox);
            this.Controls.Add(this.YMinCheckBox);
            this.Controls.Add(this.YMaxCheckBox);
            this.Controls.Add(label5);
            this.Controls.Add(this.Y2MinTextBox);
            this.Controls.Add(this.YMinTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(this.intervalTextBox);
            this.Controls.Add(label1);
            this.Controls.Add(this.numberTextBox);
            this.Name = "TracerConfigurationPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Override
        /// <summary>
        /// Apply the changed property.
        /// </summary>
        public override void ApplyChange()
        {
            m_owner.PlotNumber = m_plotNum;
            m_owner.RedrawInterval = m_redrawInterval;
            m_owner.Settings = m_settings;
        }

        /// <summary>
        /// Close this control.
        /// </summary>
        public override void PropertyDialogClosing()
        {
            if (m_redrawInterval <= 0.0 || m_redrawInterval > 3600.0)
            {
                throw new EcellException(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRedrawInterval));
            }
            if (m_plotNum < 0)
            {
                throw new EcellException(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePlotNumber));
            }
            if (m_settings.YMax < m_settings.YMin)
            {
                throw new EcellException(MessageResources.ErrInvalidValue);
            }
            if (m_settings.Y2Max < m_settings.Y2Min)
            {
                throw new EcellException(MessageResources.ErrInvalidValue);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Validate the input data in PlotNumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlotNumber_Validating(object sender, CancelEventArgs e)
        {
            string text = numberTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NamePlotNumber));
                numberTextBox.Text = Convert.ToString(m_plotNum);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePlotNumber));
                numberTextBox.Text = Convert.ToString(m_plotNum);
                e.Cancel = true;
                return;
            }
            m_plotNum = dummy;
        }

        /// <summary>
        /// Validate the input data in the RedrawInterval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedrawInterval_Validating(object sender, CancelEventArgs e)
        {
            string text = intervalTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameRedrawInterval));
                intervalTextBox.Text = Convert.ToString(m_redrawInterval);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRedrawInterval));
                intervalTextBox.Text = Convert.ToString(m_redrawInterval);
                e.Cancel = true;
                return;
            }
            m_redrawInterval = dummy;
        }

        private void YMaxAutoCheckedChanged(object sender, EventArgs e)
        {
            m_settings.IsAutoMaxY = YMaxCheckBox.Checked;

            YMaxTextBox.Enabled = !m_settings.IsAutoMaxY;
        }

        private void YMinAutoCheckedChanged(object sender, EventArgs e)
        {
            m_settings.IsAutoMinY = YMinCheckBox.Checked;

            YMinTextBox.Enabled = !m_settings.IsAutoMinY;
        }

        private void Y2MaxCheckedChanged(object sender, EventArgs e)
        {
            m_settings.IsAutoMaxY2 = Y2MaxCheckBox.Checked;

            Y2MaxTextBox.Enabled = !m_settings.IsAutoMaxY2;
        }

        private void Y2MinCheckedChanged(object sender, EventArgs e)
        {
            m_settings.IsAutoMinY2 = Y2MinCheckBox.Checked;

            Y2MinTextBox.Enabled = !m_settings.IsAutoMinY2;
        }

        private void YMaxValidating(object sender, CancelEventArgs e)
        {
            string value = YMaxTextBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, YMaxCheckBox.Text));
                e.Cancel = true;
                YMaxTextBox.Text = m_settings.YMax.ToString();
                return;
            }

            double dummy = 0.0;
            if (!Double.TryParse(value, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                YMaxTextBox.Text = m_settings.YMax.ToString();
                return;
            }
            m_settings.YMax = dummy;
        }

        private void YMinValidating(object sender, CancelEventArgs e)
        {
            string value = YMinTextBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, YMinCheckBox.Text));
                e.Cancel = true;
                YMinTextBox.Text = m_settings.YMin.ToString();
                return;
            }

            double dummy = 0.0;
            if (!Double.TryParse(value, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                YMinTextBox.Text = m_settings.YMin.ToString();
                return;
            }
            m_settings.YMin = dummy;
        }

        private void Y2MaxValidating(object sender, CancelEventArgs e)
        {
            string value = Y2MaxTextBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, Y2MaxCheckBox.Text));
                e.Cancel = true;
                Y2MaxTextBox.Text = m_settings.Y2Max.ToString();
                return;
            }

            double dummy = 0.0;
            if (!Double.TryParse(value, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                Y2MaxTextBox.Text = m_settings.Y2Max.ToString();
                return;
            }
            m_settings.Y2Max = dummy;
        }

        private void Y2MinValidating(object sender, CancelEventArgs e)
        {
            string value = Y2MinTextBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, Y2MinCheckBox.Text));
                e.Cancel = true;
                Y2MinTextBox.Text = m_settings.Y2Min.ToString();
                return;
            }

            double dummy = 0.0;
            if (!Double.TryParse(value, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                e.Cancel = true;
                Y2MinTextBox.Text = m_settings.Y2Min.ToString();
                return;
            }
            m_settings.Y2Min = dummy;
        }
        #endregion
    }
}
