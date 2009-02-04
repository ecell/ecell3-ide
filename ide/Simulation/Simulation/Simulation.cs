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
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

using Ecell;
using Ecell.Objects;
using Ecell.Plugin;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.Simulation
{
    /// <summary>
    /// Plugin class to manager simulation.
    /// </summary>
    public class Simulation : PluginBase
    {
        #region Fields
        #region Buttons
        /// <summary>
        /// ToolButtons.
        /// </summary>
        private ToolStrip ButtonList;
        /// <summary>
        /// Button to start simulation.
        /// </summary>
        private ToolStripButton m_runButton;
        /// <summary>
        /// Button to suspend simulation.
        /// </summary>
        private ToolStripButton m_suspendButton;
        /// <summary>
        /// Button to stop simulation.
        /// </summary>
        private ToolStripButton m_stopButton;
        /// <summary>
        /// Button to step simulation.
        /// </summary>
        private ToolStripButton m_stepButton;
        /// <summary>
        /// TextBox of displaying simulation time.
        /// </summary>
        private ToolStripTextBox m_timeText;
        /// <summary>
        /// TextBoxt to set the step interval.
        /// </summary>
        private ToolStripTextBox m_stepText;
        /// <summary>
        /// ComboBox to set the parameter set of simulation.
        /// </summary>
        private ToolStripComboBox m_paramsCombo;
        /// <summary>
        /// ComboBox to set the unit of step.
        /// </summary>
        private ToolStripComboBox m_stepUnitCombo;
        #endregion

        #region Menus
        /// <summary>
        /// Simulation Menu.
        /// </summary>
        private ToolStripMenuItem MenuItemRun;
        /// <summary>
        /// Setup Menu.
        /// </summary>
        private ToolStripMenuItem MenuItemSetup;
        /// <summary>
        /// the menu strip for [Run ...]
        /// </summary>
        private ToolStripMenuItem menuRunSim;
        /// <summary>
        /// the menu strip for [Suspend ... ]
        /// </summary>
        private ToolStripMenuItem menuSuspendSim;
        /// <summary>
        /// the menu strip for [Stop ...]
        /// </summary>
        private ToolStripMenuItem menuStopSim;
        /// <summary>
        /// the menu strip for [Step ...]
        /// </summary>
        private ToolStripMenuItem menuStepSim;
        /// <summary>
        /// the menu strip for [Setup ...]
        /// </summary>
        private ToolStripMenuItem menuSetupSim;
        #endregion

        /// <summary>
        /// system status.
        /// </summary>
        private ProjectStatus m_type;
        /// <summary>
        /// The flag whether m_paramsCombo is changed.
        /// </summary>
        private bool m_isChanged = false;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isStepping = false;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isSuspend = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Simulation()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulation));
            menuRunSim = new ToolStripMenuItem();
            menuRunSim.Name = "MenuItemRunSimulation";
            menuRunSim.Size = new Size(96, 22);
            menuRunSim.Image = (Image)resources.GetObject("media_play_green");
            menuRunSim.Text = MessageResources.MenuItemRun;
            menuRunSim.Tag = 10;
            menuRunSim.Enabled = false;
            menuRunSim.Click += new EventHandler(this.RunSimulation);

            menuSuspendSim = new ToolStripMenuItem();
            menuSuspendSim.Name = "MenuItemSuspendSimulation";
            menuSuspendSim.Size = new Size(96, 22);
            menuSuspendSim.Text = MessageResources.MenuItemSuspend;
            menuSuspendSim.Tag = 20;
            menuSuspendSim.Image = (Image)resources.GetObject("media_pause"); 
            menuSuspendSim.Enabled = false;
            menuSuspendSim.Click += new EventHandler(this.SuspendSimulation);

            menuStopSim = new ToolStripMenuItem();
            menuStopSim.Name = "MenuItemStopSimulation";
            menuStopSim.Size = new Size(96, 22);
            menuStopSim.Image = (Image)resources.GetObject("media_stop_red");
            menuStopSim.Text = MessageResources.MenuItemStop;
            menuStopSim.Tag = 30;
            menuStopSim.Enabled = false;
            menuStopSim.Click += new EventHandler(this.ResetSimulation);

            menuStepSim = new ToolStripMenuItem();
            menuStepSim.Name = "MenuItemStepSimulation";
            menuStepSim.Size = new Size(96, 22);
            menuStepSim.Image = (Image)resources.GetObject("media_step_forward");
            menuStepSim.Text = MessageResources.MenuItemStep;
            menuStepSim.Tag = 30;
            menuStepSim.Enabled = false;
            menuStepSim.Click += new EventHandler(this.Step);

            MenuItemRun = new ToolStripMenuItem();
            MenuItemRun.Name = "MenuItemRun";
            MenuItemRun.Size = new Size(36, 20);
            MenuItemRun.Text = "Run";
            MenuItemRun.DropDownItems.AddRange(new ToolStripItem[] {
                menuRunSim,
                menuSuspendSim,
                menuStopSim,
                menuStepSim
            });

            menuSetupSim = new ToolStripMenuItem();
            menuSetupSim.Name = "MenuItemSetupSimulation";
            menuSetupSim.Size = new Size(96, 22);
            menuSetupSim.Text = MessageResources.MenuItemSetupSim;
            menuSetupSim.Tag = 10;
            menuSetupSim.Enabled = false;
            menuSetupSim.Click += new EventHandler(this.SetupSimulation);

            MenuItemSetup = new ToolStripMenuItem();
            MenuItemSetup.Name = "MenuItemSetup";
            MenuItemSetup.Size = new Size(36, 20);
            MenuItemSetup.Text = "Setup";
            MenuItemSetup.DropDownItems.AddRange(new ToolStripItem[] {
                menuSetupSim
            });

            m_paramsCombo = new ToolStripComboBox();
            m_paramsCombo.Name = "SimulationParameter";
            m_paramsCombo.Size = new System.Drawing.Size(150, 25);
            m_paramsCombo.AutoSize = false;
            m_paramsCombo.MaxLength = 0;
            m_paramsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            m_paramsCombo.SelectedIndexChanged += new EventHandler(ParameterSelectedIndexChanged);
            m_paramsCombo.Tag = 1;

            m_runButton = new ToolStripButton();
            m_runButton.Image = (Image)resources.GetObject("media_play_green");
            m_runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_runButton.Tag = 2;
            m_runButton.Name = "RunSimulation";
            m_runButton.Size = new System.Drawing.Size(23, 22);
            m_runButton.Text = "";
            m_runButton.ToolTipText = MessageResources.ToolTipRun;
            m_runButton.Click += new System.EventHandler(this.RunSimulation);

            m_suspendButton = new ToolStripButton();
            m_suspendButton.Image = (Image)resources.GetObject("media_pause");
            m_suspendButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_suspendButton.Name = "SuspendSimulation";
            m_suspendButton.Size = new System.Drawing.Size(23, 22);
            m_suspendButton.Tag = 3;
            m_suspendButton.Text = "";
            m_suspendButton.ToolTipText = MessageResources.ToolTipSuspend;
            m_suspendButton.Click += new System.EventHandler(this.SuspendSimulation);

            m_stopButton = new ToolStripButton();
            m_stopButton.Image = (Image)resources.GetObject("media_stop_red");
            m_stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_stopButton.Name = "StopSimulation";
            m_stopButton.Size = new System.Drawing.Size(23, 22);
            m_stopButton.Text = "";
            m_stopButton.Tag = 4;
            m_stopButton.ToolTipText = MessageResources.ToolTipStop;
            m_stopButton.Click += new System.EventHandler(this.ResetSimulation);

            ToolStripLabel timeLabel = new ToolStripLabel();
            timeLabel.Name = "TimeLabel";
            timeLabel.Size = new System.Drawing.Size(81, 22);
            timeLabel.Text = " Time: ";
            timeLabel.Tag = 5;

            m_timeText = new ToolStripTextBox();
            m_timeText.Name = "TimeText";
            m_timeText.Size = new System.Drawing.Size(80, 25);
            m_timeText.Text = "0";
            m_timeText.ReadOnly = true;
            m_timeText.Tag = 6;
            m_timeText.TextBoxTextAlign = HorizontalAlignment.Right;

            ToolStripLabel secLabel = new ToolStripLabel();
            secLabel.Name = "SecLabel";
            secLabel.Size = new System.Drawing.Size(50, 22);
            secLabel.Text = "sec";
            secLabel.Tag = 7;

            ToolStripSeparator sep = new ToolStripSeparator();
            sep.Tag = 8;

            m_stepButton = new ToolStripButton();
            m_stepButton.Image = (Image)resources.GetObject("media_step_forward");
            m_stepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_stepButton.Name = "StepSimulation";
            m_stepButton.Size = new System.Drawing.Size(23, 22);
            m_stepButton.Text = "";
            m_stepButton.Tag = 9;
            m_stepButton.ToolTipText = MessageResources.ToolTipStep;
            m_stepButton.Click += new System.EventHandler(this.Step);

            m_stepText = new ToolStripTextBox();
            m_stepText.Name = "StepText";
            m_stepText.Size = new System.Drawing.Size(60, 25);
            m_stepText.Text = "1";
            m_stepText.Tag = 10;
            m_stepText.TextBoxTextAlign = HorizontalAlignment.Right;

            m_stepUnitCombo = new ToolStripComboBox();
            m_stepUnitCombo.Name = "StepCourse";
            m_stepUnitCombo.Size = new System.Drawing.Size(50, 25);
            m_stepUnitCombo.AutoSize = false;
            m_stepUnitCombo.Tag = 11;
            m_stepUnitCombo.Items.Add("Step");
            m_stepUnitCombo.Items.Add("Sec");
            m_stepUnitCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            m_stepUnitCombo.SelectedIndex = 0;
            m_stepUnitCombo.SelectedIndexChanged += new EventHandler(m_stepUnitCombo_SelectedIndexChanged);

            ButtonList = new ToolStrip();

            ButtonList.Items.AddRange( new ToolStripItem[] {
                m_paramsCombo,
                m_runButton,
                m_suspendButton,
                m_stopButton,
                timeLabel,
                m_timeText,
                secLabel,
                sep,
                m_stepButton,
                m_stepText,
                m_stepUnitCombo});
            ButtonList.Location = new Point(400, 0);

        }
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
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            List<ToolStripMenuItem> menuList = new List<ToolStripMenuItem>();
            menuList.Add(MenuItemRun);
            menuList.Add(MenuItemSetup);
            return menuList;
        }

        /// <summary>
        /// Get toolbar buttons for Simulation.
        /// </summary>
        /// <returns>List of ToolStripItem.</returns>
        public override ToolStrip GetToolBarMenuStrip()
        {
            return this.ButtonList;
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
                ShowSetupSimulationDialog(key);
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
            {
                int index = m_paramsCombo.Items.IndexOf(parameterID);
                m_paramsCombo.Items.RemoveAt(index);
                m_paramsCombo.SelectedIndex = 0;
            }
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
            bool isUninitialized = type == ProjectStatus.Uninitialized;
            bool isLoaded = type == ProjectStatus.Loaded;
            bool isRunning = type == ProjectStatus.Running;
            bool isStepping = type == ProjectStatus.Stepping;
            bool isSuspended = type == ProjectStatus.Suspended;

            // Set Menu Enabled.
            menuRunSim.Enabled = isLoaded || isSuspended;
            menuSuspendSim.Enabled = isRunning || isStepping;
            menuStopSim.Enabled = isStepping || isRunning || isSuspended;
            menuStepSim.Enabled = isLoaded || isStepping || isSuspended;
            menuSetupSim.Enabled = isLoaded || isStepping || isRunning || isSuspended;

            menuRunSim.Checked = isRunning;
            menuSuspendSim.Checked = isSuspended;

            // Set Button Enabled.
            m_runButton.Enabled = isLoaded || isSuspended;
            m_suspendButton.Enabled = isRunning || isStepping;
            m_stopButton.Enabled = isStepping || isRunning || isSuspended;
            m_stepButton.Enabled = isLoaded || isStepping || isSuspended;

            m_runButton.Checked = isRunning;
            m_suspendButton.Checked = isSuspended;

            // Set ComboBox for Params.
            m_timeText.Enabled = isLoaded || isStepping || isRunning || isSuspended;
            if (isUninitialized || isLoaded)
                m_timeText.Text = "0";
            m_stepText.Enabled = isLoaded || (isSuspended && !m_isStepping);
            m_stepUnitCombo.Enabled = isLoaded || (isSuspended && !m_isStepping);
            m_paramsCombo.Enabled = isLoaded;

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

        private void ShowSetupSimulationDialog(string viewParamID)
        {
            Dictionary<string, SimulationParameterSet> sim = new Dictionary<string, SimulationParameterSet>();
            foreach (string paramID in m_dManager.GetSimulationParameterIDs())
            {
                SimulationParameterSet sps = new SimulationParameterSet(paramID);
                foreach (string modelID in m_dManager.GetModelList())
                {
                    PerModelSimulationParameter pmsp = new PerModelSimulationParameter(modelID);
                    foreach (KeyValuePair<string, double> pair in 
                        m_dManager.GetInitialCondition(paramID, modelID))
                    {
                        pmsp.InitialConditions.Add(
                            KeyValuePairConverter<string, double>.Convert(pair));
                    }

                    foreach (EcellObject stepper in m_dManager.GetStepper(paramID, modelID))
                    {
                        StepperConfiguration sc = new StepperConfiguration();
                        sc.Name = stepper.Key;
                        sc.ClassName = stepper.Classname;
                        foreach (EcellData prop in stepper.Value)
                        {
                            if (prop.Value.IsList || !prop.Settable)
                                continue;
                            sc.Properties.Add(new MutableKeyValuePair<string, string>(
                                prop.Name, prop.Value.Value.ToString()));
                        }
                        pmsp.Steppers.Add(sc);
                    }
                    sps.PerModelSimulationParameters.Add(pmsp);
                }
                sps.LoggerPolicy = (LoggerPolicy)m_dManager.GetLoggerPolicy(paramID).Clone();
                sim.Add(sps.Name, sps);
            }

            SimulationConfigurationDialog win = new SimulationConfigurationDialog(this, sim.Values);
            if (viewParamID != null)
                win.ChangeParameterID(sim[viewParamID]);
            using (win)
            {
                List<string> delList = new List<string>();
                DialogResult r = win.ShowDialog();
                if (r != DialogResult.OK)
                    return;
                foreach (SimulationParameterSet sps in sim.Values)
                {
                    bool deleted = true;
                    foreach (SimulationParameterSet newSps in win.Result)
                    {
                        if (newSps.Name == sps.Name)
                        {
                            deleted = false;
                            break;
                        }
                    }

                    if (deleted)
                    {
                        delList.Add(sps.Name);
//                        m_dManager.DeleteSimulationParameter(sps.Name);
                    }
                }

                try
                {
                    foreach (SimulationParameterSet sps in win.Result)
                    {
                        if (!sim.ContainsKey(sps.Name))
                            m_dManager.CreateSimulationParameter(sps.Name);

                        m_dManager.SetLoggerPolicy(sps.Name, sps.LoggerPolicy);


                        List<EcellObject> steppers = new List<EcellObject>();
                        foreach (PerModelSimulationParameter pmsp in sps.PerModelSimulationParameters)
                        {
                            {
                                Dictionary<string, double> pairs = new Dictionary<string, double>(pmsp.InitialConditions.Count);
                                foreach (MutableKeyValuePair<string, double> pair in pmsp.InitialConditions)
                                    pairs.Add(pair.Key, pair.Value);
                                m_dManager.UpdateInitialCondition(sps.Name, pmsp.ModelID, pairs);
                            }
                            foreach (StepperConfiguration sc in pmsp.Steppers)
                            {
                                Dictionary<string, EcellData> propDict = m_dManager.GetStepperProperty(sc.ClassName);
                                foreach (MutableKeyValuePair<string, string> pair in sc.Properties)
                                {
                                    EcellData d = propDict[pair.Key];
                                    if (d.Value.IsDouble)
                                    {
                                        d.Value = new EcellValue(Convert.ToDouble(pair.Value));
                                    }
                                    else if (d.Value.IsInt)
                                    {
                                        d.Value = new EcellValue(Convert.ToInt32(pair.Value));
                                    }
                                    else if (d.Value.IsString)
                                    {
                                        d.Value = new EcellValue(pair.Value);
                                    }
                                    Trace.WriteLine(d.Name + ":" + d.Value.Value);
                                }
                                steppers.Add(EcellObject.CreateObject(pmsp.ModelID, sc.Name, Constants.xpathStepper, sc.ClassName, new List<EcellData>(propDict.Values)));
                            }
                        }

                        foreach (String key in delList)
                        {
                            m_dManager.DeleteSimulationParameter(key);
                        }

                        m_dManager.UpdateStepperID(sps.Name, steppers);
                        m_env.PluginManager.ParameterUpdate(
                            m_env.DataManager.CurrentProjectID, sps.Name);
                    }
                    m_env.DataManager.SetSimulationParameter(win.CurrentParameterID);
                }
                catch (Exception ex)
                {
                    Util.ShowErrorDialog(ex.Message);
                }            
            }
        }

        #region Event
        /// <summary>
        /// The action of [Simulation] menu click.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void SetupSimulation(object sender, EventArgs e)
        {
            ShowSetupSimulationDialog(null);
        }

        /// <summary>
        /// The action of [run ...] menu click.
        /// start simulation after you click run button.
        /// </summary>
        /// <param name="sender">object(ToolStripButton)</param>
        /// <param name="e">EventArgs</param>
        public void RunSimulation(object sender, EventArgs e)
        {
            if (m_type == ProjectStatus.Running || m_type == ProjectStatus.Uninitialized) 
                return;

            try
            {
                m_isSuspend = false;
                if (m_isStepping)
                {
                    m_pManager.ChangeStatus(ProjectStatus.Stepping);
                }
                else
                {
                    m_pManager.ChangeStatus(ProjectStatus.Running);
                }
//                m_dManager.SimulationStart(0.0, 0);
                m_dManager.StartSimulation(0.0);
            }
            catch (SimulationException ex)
            {
                m_env.Console.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
                if (m_type != ProjectStatus.Uninitialized)
                    m_pManager.ChangeStatus(ProjectStatus.Loaded);
            }
            if (m_isStepping && !m_isSuspend)
            {
                m_isStepping = false;
                if (m_type == ProjectStatus.Stepping)
                    m_pManager.ChangeStatus(ProjectStatus.Suspended);
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
            try
            {
                m_dManager.SimulationSuspend();
                m_pManager.ChangeStatus(ProjectStatus.Suspended);
            }
            catch (SimulationException ex)
            {
                Util.ShowErrorDialog(ex.Message);
                m_pManager.ChangeStatus(preType);
            }
            m_isSuspend = true;
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
            try
            {
                if (m_stepUnitCombo.Text == "Step")
                {
                    int stepCount = Convert.ToInt32(m_stepText.Text);
                    if (stepCount < 0)
                        throw new EcellException(MessageResources.ErrInvalidValue);
                }
                else
                {
                    double timeCount = Convert.ToDouble(m_stepText.Text);
                    if (timeCount < 0)
                        throw new EcellException(MessageResources.ErrInvalidValue);
                }
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                if (m_stepUnitCombo.Text == "Step")
                {
                    m_stepText.Text = "1";
                }
                else
                {
                    m_stepText.Text = "1.0";
                }
                return;
            }

            try
            {
                m_pManager.ChangeStatus(ProjectStatus.Stepping); 
                m_isStepping = true;
                m_isSuspend = false;
                if (m_stepUnitCombo.Text == "Step")
                {
                    int stepCount = Convert.ToInt32(m_stepText.Text);
                    if (stepCount < 0) return;
                    // m_dManager.SimulationStartKeepSetting(stepCount); 
                    // m_dManager.SimulationStart(stepCount);
                    m_dManager.StartStepSimulation(stepCount);
                }
                else
                {
                    double timeCount = Convert.ToDouble(m_stepText.Text);
                    if (timeCount < 0) return;
                    // m_dManager.SimulationStartKeepSetting(timeCount); 
                    // m_dManager.SimulationStart(timeCount);
                    m_dManager.StartStepSimulation(timeCount);
                }
            }
            catch (SimulationException ex)
            {
                Util.ShowErrorDialog(ex.Message);
                m_pManager.ChangeStatus(preType);
            }
            if (!m_isSuspend)
            {
                m_isStepping = false;
                if (m_type == ProjectStatus.Stepping)
                    m_pManager.ChangeStatus(ProjectStatus.Suspended);
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

            if (!Util.ShowYesNoDialog(MessageResources.ConfirmReset)) return;
            ProjectStatus preType = m_type;
            m_pManager.ChangeStatus(ProjectStatus.Loaded);
            try
            {
                m_isSuspend = false;
                m_dManager.SimulationStop();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
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
            string preParam = m_dManager.CurrentProject.Info.SimulationParam;
            try
            {
                if (m_paramsCombo.Text != "")
                {
                    m_isChanged = true;
                    m_dManager.SetSimulationParameter(m_paramsCombo.Text, false, false);
                    m_isChanged = false;
                }
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
                m_paramsCombo.Text = preParam;
            }
        }

        void m_stepUnitCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_stepUnitCombo.Text.Equals("Step"))
            {
                m_stepText.Text = "1";
            }
            else
            {
                m_stepText.Text = "1.0";
            }
        }
        #endregion
    }
}
