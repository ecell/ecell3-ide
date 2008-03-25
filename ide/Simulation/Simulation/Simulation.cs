//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

using EcellLib;

namespace EcellLib.Simulation
{
    /// <summary>
    /// Plugin class to manager simulation.
    /// </summary>
    public class Simulation : PluginBase
    {
        #region Fields
        /// <summary>
        /// The flag whether m_paramsCombo is changed.
        /// </summary>
        private bool m_isChanged = false;
        /// <summary>
        /// TextBox of displaying simulation time.
        /// </summary>
        private ToolStripTextBox m_timeText = null;
        /// <summary>
        /// TextBoxt to set the step interval.
        /// </summary>
        private ToolStripTextBox m_stepText = null;
        /// <summary>
        /// ComboBox to set the parameter set of simulation.
        /// </summary>
        private ToolStripComboBox m_paramsCombo = null;
        /// <summary>
        /// ComboBox to set the unit of step.
        /// </summary>
        private ToolStripComboBox m_stepUnitCombo = null;
        /// <summary>
        /// Window for set the the parameter of simulation
        /// </summary>
        private SimulationSetup m_win;
        /// <summary>
        /// the menu strip for [Run ...]
        /// </summary>
        private ToolStripMenuItem m_runSim;
        /// <summary>
        /// the menu strip for [Suspend ... ]
        /// </summary>
        private ToolStripMenuItem m_suspendSim;
        /// <summary>
        /// the menu strip for [Stop ...]
        /// </summary>
        private ToolStripMenuItem m_stopSim;
        /// <summary>
        /// the menu strip for [Setup ...]
        /// </summary>
        private ToolStripMenuItem m_setupSim;
        /// <summary>
        /// system status.
        /// </summary>
        private ProjectStatus m_type;
        /// <summary>
        /// ResourceManager for NewParameterWindow.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResSimulation));
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get manustripts for Simulation
        /// [Run]   -> [Run ...]
        ///            [Suspend ...]
        ///            [Stop ...]
        /// [Setup] -> [Simulation]
        /// </summary>
        /// <returns>MenuStripItems</returns>
        public override List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulation));
            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_runSim = new ToolStripMenuItem();
            m_runSim.Name = "MenuItemRunSimulation";
            m_runSim.Size = new Size(96, 22);
            m_runSim.Image = (Image)resources.GetObject("media_play_green");
            m_runSim.Text = Simulation.s_resources.GetString("MenuItemRun");
            m_runSim.Enabled = false;
            m_runSim.Click += new EventHandler(this.RunSimulation);

            m_suspendSim = new ToolStripMenuItem();
            m_suspendSim.Name = "MenuItemSuspendSimulation";
            m_suspendSim.Size = new Size(96, 22);
            m_suspendSim.Text = Simulation.s_resources.GetString("MenuItemSuspend");
            m_suspendSim.Image = (Image)resources.GetObject("media_pause"); 
            m_suspendSim.Enabled = false;
            m_suspendSim.Click += new EventHandler(this.SuspendSimulation);

            m_stopSim = new ToolStripMenuItem();
            m_stopSim.Name = "MenuItemStopSimulation";
            m_stopSim.Size = new Size(96, 22);
            m_stopSim.Image = (Image)resources.GetObject("media_stop_red");
            m_stopSim.Text = Simulation.s_resources.GetString("MenuItemStop");
            m_stopSim.Enabled = false;
            m_stopSim.Click += new EventHandler(this.ResetSimulation);

            ToolStripMenuItem run = new ToolStripMenuItem();
            run.DropDownItems.AddRange(new ToolStripItem[] {
                m_runSim,
                m_suspendSim,
                m_stopSim
            });
            run.Name = "MenuItemRun";
            run.Size = new Size(36, 20);
            run.Text = "Run";
            tmp.Add(run);

            m_setupSim = new ToolStripMenuItem();
            m_setupSim.Name = "MenuItemSetupSimulation";
            m_setupSim.Size = new Size(96, 22);
            m_setupSim.Text = Simulation.s_resources.GetString("MenuItemSetupSim");
            m_setupSim.Tag = 10;
            m_setupSim.Enabled = false;
            m_setupSim.Click += new EventHandler(this.SetupSimulation);

            ToolStripMenuItem setup = new ToolStripMenuItem();
            setup.DropDownItems.AddRange(new ToolStripItem[] {
                m_setupSim
            });
            setup.Name = "MenuItemSetup";
            setup.Size = new Size(36, 20);
            setup.Text = "Setup";
            tmp.Add(setup);

            return tmp;
        }

        /// <summary>
        /// Get toolbar buttons for Simulation.
        /// </summary>
        /// <returns>List of ToolStripItem.</returns>
        public override List<ToolStripItem> GetToolBarMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulation));
            List<ToolStripItem> list = new List<ToolStripItem>();

            m_paramsCombo = new ToolStripComboBox();
            m_paramsCombo.Name = "SimulationParameter";
            m_paramsCombo.Size = new System.Drawing.Size(150, 25);
            m_paramsCombo.AutoSize = false;
            m_paramsCombo.SelectedIndexChanged += new EventHandler(ParameterSelectedIndexChanged);
            m_paramsCombo.Tag = 1;
            list.Add(m_paramsCombo);

            ToolStripButton button1 = new ToolStripButton();
            button1.Image = (Image)resources.GetObject("media_play_green");
            button1.ImageTransparentColor = System.Drawing.Color.Magenta;
            button1.Tag = 2;
            button1.Name = "RunSimulation";
            button1.Size = new System.Drawing.Size(23, 22);
            button1.Text = "";
            button1.ToolTipText = Simulation.s_resources.GetString("ToolTipRun");
            button1.Click += new System.EventHandler(this.RunSimulation);
            list.Add(button1);

            ToolStripButton button3 = new ToolStripButton();
            button3.Image = (Image)resources.GetObject("media_pause");
            button3.ImageTransparentColor = System.Drawing.Color.Magenta;
            button3.Name = "SuspendSimulation";
            button3.Size = new System.Drawing.Size(23, 22);
            button3.Tag = 3;
            button3.Text = "";
            button3.ToolTipText = Simulation.s_resources.GetString("ToolTipSuspend");
            button3.Click += new System.EventHandler(this.SuspendSimulation);
            list.Add(button3);

            ToolStripButton button2 = new ToolStripButton();
            button2.Image = (Image)resources.GetObject("media_stop_red");
            button2.ImageTransparentColor = System.Drawing.Color.Magenta;
            button2.Name = "StopSimulation";
            button2.Size = new System.Drawing.Size(23, 22);
            button2.Text = "";
            button2.Tag = 4;
            button2.ToolTipText = Simulation.s_resources.GetString("ToolTipStop");
            button2.Click += new System.EventHandler(this.ResetSimulation);
            list.Add(button2);

            ToolStripLabel label1 = new ToolStripLabel();
            label1.Name = "TimeLabel";
            label1.Size = new System.Drawing.Size(81, 22);
            label1.Text = " Time: ";
            label1.Tag = 5;
            list.Add(label1);

            m_timeText = new ToolStripTextBox();
            m_timeText.Name = "TimeText";
            m_timeText.Size = new System.Drawing.Size(80, 25);
            m_timeText.Text = "0";
            m_timeText.ReadOnly = true;
            m_timeText.Tag = 6;
            m_timeText.TextBoxTextAlign =  HorizontalAlignment.Right;
            list.Add(m_timeText);

            ToolStripLabel label2 = new ToolStripLabel();
            label2.Name = "SecLabel";
            label2.Size = new System.Drawing.Size(50, 22);
            label2.Text = "sec";
            label2.Tag = 7;
            list.Add(label2);

            ToolStripSeparator sep = new ToolStripSeparator();
            sep.Tag = 8;
            list.Add(sep);

            ToolStripButton button4 = new ToolStripButton();
            button4.Image = (Image)resources.GetObject("media_step_forward");
            button4.ImageTransparentColor = System.Drawing.Color.Magenta;
            button4.Name = "StepSimulation";
            button4.Size = new System.Drawing.Size(23, 22);
            button4.Text = "";
            button4.Tag = 9;
            button4.ToolTipText = Simulation.s_resources.GetString("ToolTipStep");
            button4.Click += new System.EventHandler(this.Step);
            list.Add(button4);

            m_stepText = new ToolStripTextBox();
            m_stepText.Name = "StepText";
            m_stepText.Size = new System.Drawing.Size(60, 25);
            m_stepText.Text = "1";
            m_stepText.Tag = 10;
            m_stepText.TextBoxTextAlign = HorizontalAlignment.Right;
            list.Add(m_stepText);

            m_stepUnitCombo = new ToolStripComboBox();
            m_stepUnitCombo.Name = "StepCourse";
            m_stepUnitCombo.Size = new System.Drawing.Size(50, 25);
            m_stepUnitCombo.AutoSize = false;
            m_stepUnitCombo.Tag = 11;
            m_stepUnitCombo.Items.Add("Step");
            m_stepUnitCombo.Items.Add("Sec");
            m_stepUnitCombo.SelectedText = "Step";
            list.Add(m_stepUnitCombo);

            return list;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            if (type == Constants.xpathParameters && key != Constants.xpathParameters)
            {
                SetupSimulation(new Button(), new EventArgs());
            }
        }

        /// <summary>
        /// The event sequence when the simulation parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID.</param>
        public override void ParameterAdd(string projectID, string parameterID)
        {
            if (!m_paramsCombo.Items.Contains(parameterID))
                m_paramsCombo.Items.Add(parameterID);
        }

        /// <summary>
        /// The event sequence when the simulation parameter is deleted.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public override void ParameterDelete(string projectID, string parameterID)
        {
            if (m_paramsCombo.Items.Contains(parameterID))
                m_paramsCombo.Items.Remove(parameterID);
        }


        /// <summary>
        /// The event sequence when the simulation parameter is set.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public override void ParameterSet(string projectID, string parameterID)
        {
            if (m_isChanged) return;
            if (parameterID == null) return;
            if (!m_paramsCombo.Items.Contains(parameterID))
                m_paramsCombo.Items.Add(parameterID);
            int ind = 0;
            foreach (Object obj in m_paramsCombo.Items)
            {
                if (obj.ToString().Equals(parameterID))
                    m_paramsCombo.SelectedIndex = ind;
                ind++;
            }
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            m_paramsCombo.Items.Clear();
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public override void AdvancedTime(double time)
        {
            if (m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping)
            m_timeText.Text = time.ToString();
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Uninitialized)
            {
                m_runSim.Enabled = false;
                m_stopSim.Enabled = false;
                m_suspendSim.Enabled = false;
                m_setupSim.Enabled = false;
            }
            else if (type == ProjectStatus.Loaded)
            {
                m_runSim.Enabled = true;
                m_stopSim.Enabled = false;
                m_suspendSim.Enabled = false;
                m_setupSim.Enabled = true;
                m_timeText.Text = "0";
                m_timeText.ForeColor = Color.Black;
                m_paramsCombo.Enabled = true;
                m_stepUnitCombo.Enabled = true;
                m_stepText.Enabled = true;
            }
            else if (type == ProjectStatus.Stepping)
            {
                m_runSim.Enabled = true;
                m_stopSim.Enabled = false;
                m_suspendSim.Enabled = false;
                m_setupSim.Enabled = true;
            }
            else if (type == ProjectStatus.Running)
            {
                m_runSim.Enabled = false;
                m_stopSim.Enabled = true;
                m_suspendSim.Enabled = true;
                m_setupSim.Enabled = true;
                m_paramsCombo.Enabled = false;
                m_stepText.Enabled = false;
                m_stepUnitCombo.Enabled = false;
                m_timeText.ForeColor = Color.Black;
            }
            else if (type == ProjectStatus.Suspended)
            {
                m_runSim.Enabled = true;
                m_stopSim.Enabled = true;
                m_suspendSim.Enabled = false;
                m_setupSim.Enabled = true;
                m_timeText.ForeColor = Color.Gray;
            }
            else
            {
                m_runSim.Enabled = false;
                m_stopSim.Enabled = false;
                m_suspendSim.Enabled = false;
                m_setupSim.Enabled = false;
            }
            m_type = type;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"Simulation"</returns>
        public override string GetPluginName()
        {
            return "Simulation";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        #region Event
        /// <summary>
        /// The action of [Simulation] menu click.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void SetupSimulation(object sender, EventArgs e)
        {
            if (m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Running)
            {
                String mes = Simulation.s_resources.GetString("ConfirmSetup");
                DialogResult r = MessageBox.Show(mes,
                    "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (r != DialogResult.OK) return;
                ResetSimulation(sender, e);
            }

            m_win = new SimulationSetup();
            m_win.ShowDialog();
        }

        /// <summary>
        /// The action of [run ...] menu click.
        /// start simulation after you click run button.
        /// </summary>
        /// <param name="sender">object(ToolStripButton)</param>
        /// <param name="e">EventArgs</param>
        public void RunSimulation(object sender, EventArgs e)
        {
            if (m_type == ProjectStatus.Running || m_type == ProjectStatus.Uninitialized) return;
            ProjectStatus preType = m_type;
            m_pManager.ChangeStatus(ProjectStatus.Running);
            try
            {
                m_dManager.SimulationStart(0.0, 0);
            }
            catch (Exception ex)
            {
                String errmes = Simulation.s_resources.GetString("ErrRunning");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (m_type != ProjectStatus.Uninitialized)
                    m_pManager.ChangeStatus(preType);
            }

        }

        /// <summary>
        /// The action of [suspend ...] menu click.
        /// suspend simulation after you click suspend button.
        /// </summary>
        /// <param name="sender">object(ToolStripButton)</param>
        /// <param name="e">EventArgs</param>
        public void SuspendSimulation(object sender, EventArgs e)
        {
            if (m_type != ProjectStatus.Running && m_type != ProjectStatus.Stepping)
                return;
            ProjectStatus preType = m_type;            
            m_pManager.ChangeStatus(ProjectStatus.Suspended);
            try
            {
                m_dManager.SimulationSuspend();
            }
            catch (Exception ex)
            {
                String errmes = Simulation.s_resources.GetString("ErrSuspend");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_pManager.ChangeStatus(preType);
            }
        }


        /// <summary>
        /// The action of [Step ...] menu click.
        /// </summary>
        /// <param name="sender">object(ToolStripButton)</param>
        /// <param name="e">EventArgs</param>
        public void Step(object sender, EventArgs e)
        {
            if (m_type == ProjectStatus.Running) return;
            if (m_type == ProjectStatus.Uninitialized) return;
            ProjectStatus preType = m_type;
            m_type = ProjectStatus.Running;
            try
            {
                if (m_stepUnitCombo.Text == "Step")
                {
                    int stepCount = Convert.ToInt32(m_stepText.Text);
                    if (stepCount < 0) return;
                    m_dManager.SimulationStartKeepSetting(stepCount); // m_dManager.SimulationStart(stepCount);
                }
                else
                {
                    double timeCount = Convert.ToDouble(m_stepText.Text);
                    if (timeCount < 0) return;
                    m_dManager.SimulationStartKeepSetting(timeCount); // m_dManager.SimulationStart(timeCount);
                }
                m_pManager.ChangeStatus(ProjectStatus.Stepping);
            }
            catch (Exception ex)
            {
                String errmes = Simulation.s_resources.GetString("ErrStep");
                MessageBox.Show(errmes + "\n\n" + ex,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_pManager.ChangeStatus(preType);                
            }
        }


        /// <summary>
        /// The action of [stop ...] menu click.
        /// stop simulation after you click stop button.
        /// </summary>
        /// <param name="sender">object(ToolStripButton)</param>
        /// <param name="e">EventArgs</param>
        public void ResetSimulation(object sender, EventArgs e)
        {
            if (m_type != ProjectStatus.Running &&
                    m_type != ProjectStatus.Suspended &&
                    m_type != ProjectStatus.Stepping)
                return;
            ProjectStatus preType = m_type;
            m_pManager.ChangeStatus(ProjectStatus.Loaded);
            try
            {
                m_dManager.SimulationStop();
            }
            catch (Exception ex)
            {
                String errmes = Simulation.s_resources.GetString("ErrReset");
                MessageBox.Show(errmes + "\n\n" + ex,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_pManager.ChangeStatus(preType);
            }
        }

        /// <summary>
        /// Event when ParamComboBox is changed.
        /// </summary>
        /// <param name="sender">ToolStripComboBox</param>
        /// <param name="e">EventArgs</param>
        void ParameterSelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_paramsCombo.Text != "")
            {
                m_isChanged = true;
                m_dManager.SetSimulationParameter(m_paramsCombo.Text,false,false);
                m_isChanged = false;
            }
        }
        #endregion
    }
}
