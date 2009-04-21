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

using Ecell.Exceptions;
using Ecell.IDE;

namespace Ecell.IDE.Plugins.Simulation
{
    /// <summary>
    /// SimulationConfigDialog
    /// </summary>
    public class SimulationConfigDialog : PropertyDialogPage
    {
        #region Fields
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox stepCountTextBox;
        private TextBox waitTimeTextBox;
        private DataManager m_manager;
        private int m_stepCount;
        private int m_waitTime;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="manager"></param>
        public SimulationConfigDialog(DataManager manager)
        {
            InitializeComponent();
            m_manager = manager;
            m_stepCount = m_manager.StepCount;
            m_waitTime = m_manager.WaitTime;

            stepCountTextBox.Text = m_stepCount.ToString();
            waitTimeTextBox.Text = m_waitTime.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationConfigDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.stepCountTextBox = new System.Windows.Forms.TextBox();
            this.waitTimeTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // stepCountTextBox
            // 
            resources.ApplyResources(this.stepCountTextBox, "stepCountTextBox");
            this.stepCountTextBox.Name = "stepCountTextBox";
            this.stepCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.stepCountTextBox_Validating);
            // 
            // waitTimeTextBox
            // 
            resources.ApplyResources(this.waitTimeTextBox, "waitTimeTextBox");
            this.waitTimeTextBox.Name = "waitTimeTextBox";
            this.waitTimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.waitTimeTextBox_Validating);
            // 
            // SimulationConfigDialog
            // 
            this.Controls.Add(this.waitTimeTextBox);
            this.Controls.Add(this.stepCountTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SimulationConfigDialog";
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
            m_manager.StepCount = m_stepCount;
            m_manager.WaitTime = m_waitTime;
        }

        /// <summary>
        /// Close this control.
        /// </summary>
        public override void PropertyDialogClosing()
        {
            if (m_stepCount <= 0)
            {
                throw new EcellException(String.Format(MessageResources.ErrInvalidValue, label1.Text));
            }
            if (m_waitTime < 0)
            {
                throw new EcellException(String.Format(MessageResources.ErrInvalidValue, label2.Text));
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Validate the Text Box in step count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stepCountTextBox_Validating(object sender, CancelEventArgs e)
        {
            string text = stepCountTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, label1.Text));
                stepCountTextBox.Text = Convert.ToString(m_stepCount);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, label1.Text));
                stepCountTextBox.Text = Convert.ToString(m_stepCount);
                e.Cancel = true;
                return;
            }
            m_stepCount = dummy;
        }

        /// <summary>
        /// Validate the text box in wait time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waitTimeTextBox_Validating(object sender, CancelEventArgs e)
        {
            string text = waitTimeTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, label2.Text));
                waitTimeTextBox.Text = Convert.ToString(m_waitTime);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, label2.Text));
                waitTimeTextBox.Text = Convert.ToString(m_waitTime);
                e.Cancel = true;
                return;
            }
            m_waitTime = dummy;
        }
        #endregion
    }
}
