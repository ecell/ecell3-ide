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
    public class SimulationConfigurationPage : PropertyDialogPage
    {
        #region Fields
        /// <summary>
        /// TextBox to input the step count.
        /// </summary>
        private TextBox stepCountTextBox;
        /// <summary>
        /// TextBox to input the wait time.
        /// </summary>
        private TextBox waitTimeTextBox;
        /// <summary>
        /// DataManager
        /// </summary>
        private DataManager m_manager;
        /// <summary>
        /// The number of step.
        /// </summary>
        private int m_stepCount;
        /// <summary>
        /// CheckBox to input the flag.
        /// </summary>
        private CheckBox saveCheckBox;
        /// <summary>
        /// The wait time.
        /// </summary>
        private int m_waitTime;
        /// <summary>
        /// Owner object.
        /// </summary>
        private Simulation m_owner;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="manager">DataManager object.</param>
        /// <param name="sim">Simulator</param>
        public SimulationConfigurationPage(DataManager manager, Simulation sim)
        {
            InitializeComponent();
            m_manager = manager;
            m_stepCount = m_manager.StepCount;
            m_waitTime = m_manager.WaitTime;

            stepCountTextBox.Text = m_stepCount.ToString();
            waitTimeTextBox.Text = m_waitTime.ToString();
            saveCheckBox.Checked = manager.IsSaveStep;
            //if (manager.Environment.PluginManager.Status == ProjectStatus.Loaded ||
            //    manager.Environment.PluginManager.Status == ProjectStatus.Uninitialized)
            //    saveCheckBox.Enabled = true;
            //else
            //    saveCheckBox.Enabled = false;
            m_owner = sim;
        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationConfigurationPage));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.stepCountTextBox = new System.Windows.Forms.TextBox();
            this.waitTimeTextBox = new System.Windows.Forms.TextBox();
            this.saveCheckBox = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
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
            // saveCheckBox
            // 
            resources.ApplyResources(this.saveCheckBox, "saveCheckBox");
            this.saveCheckBox.Name = "saveCheckBox";
            this.saveCheckBox.UseVisualStyleBackColor = true;
            // 
            // SimulationConfigurationPage
            // 
            this.Controls.Add(this.saveCheckBox);
            this.Controls.Add(label4);
            this.Controls.Add(this.waitTimeTextBox);
            this.Controls.Add(this.stepCountTextBox);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Name = "SimulationConfigurationPage";
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
            m_manager.IsSaveStep = saveCheckBox.Checked;

            m_owner.IsSaveSteppingModel = m_manager.IsSaveStep;
        }

        /// <summary>
        /// Close this control.
        /// </summary>
        public override void PropertyDialogClosing()
        {
            if (m_stepCount <= 0)
            {
                throw new EcellException(MessageResources.ErrInvalidValue);
            }
            if (m_waitTime < 0)
            {
                throw new EcellException(MessageResources.ErrInvalidValue);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Validate the Text Box in step count.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void stepCountTextBox_Validating(object sender, CancelEventArgs e)
        {
            string text = stepCountTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameStepNum));
                stepCountTextBox.Text = Convert.ToString(m_stepCount);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameStepNum));
                stepCountTextBox.Text = Convert.ToString(m_stepCount);
                e.Cancel = true;
                return;
            }
            m_stepCount = dummy;
        }

        /// <summary>
        /// Validate the text box in wait time.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void waitTimeTextBox_Validating(object sender, CancelEventArgs e)
        {
            string text = waitTimeTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameWaitTime));
                waitTimeTextBox.Text = Convert.ToString(m_waitTime);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameWaitTime));
                waitTimeTextBox.Text = Convert.ToString(m_waitTime);
                e.Cancel = true;
                return;
            }
            m_waitTime = dummy;
        }
        #endregion
    }
}
