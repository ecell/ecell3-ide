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
using Ecell.Events;

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
        private ToolStripButton m_stopButton;
        /// <summary>
        /// Button to stop simulation.
        /// </summary>
        private ToolStripButton m_resetButton;
        /// <summary>
        /// Button to step simulation.
        /// </summary>
        private ToolStripButton m_stepButton;
        /// <summary>
        /// Button to go foward by one step.
        /// </summary>
        private ToolStripButton m_fowardButton;
        /// <summary>
        /// Button to go back by one step.
        /// </summary>
        private ToolStripButton m_backButton;
        /// <summary>
        /// Separator
        /// </summary>
        private ToolStripSeparator m_separator;
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
        private ToolStripMenuItem menuStopSim;
        /// <summary>
        /// the menu strip for [Stop ...]
        /// </summary>
        private ToolStripMenuItem menuResetSim;
        /// <summary>
        /// the menu strip for [Step ...]
        /// </summary>
        private ToolStripMenuItem menuStepSim;
        /// <summary>
        /// the menu strip for [Setup ...]
        /// </summary>
        private ToolStripMenuItem menuSetupSim;
        /// <summary>
        /// The menu strip for loading the stepping model.
        /// </summary>
        private ToolStripMenuItem menuSteppingModel;
        /// <summary>
        /// The dictionary of the saving id and menu strip.
        /// </summary>
        private Dictionary<int, ToolStripMenuItem> menuSteppingModelDic = new Dictionary<int, ToolStripMenuItem>();
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
        /// The flag whether simulation is stepping.
        /// </summary>
        private bool m_isStepping = false;
        /// <summary>
        /// The flag whether simulation is suspending.
        /// </summary>
        private bool m_isSuspend = false;
        /// <summary>
        /// The current simulation time.
        /// </summary>
        private double m_time = 0.0;
        /// <summary>
        /// The simulation time for the loaded model.
        /// </summary>
        private double m_loadedModel = 0.0;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Simulation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulation));
            menuRunSim = new ToolStripMenuItem();
            menuRunSim.Name = "MenuItemRunSimulation";
            menuRunSim.Size = new Size(96, 22);
            menuRunSim.Image = (Image)resources.GetObject("media_play_green");
            menuRunSim.Text = MessageResources.MenuItemRun;
            menuRunSim.Tag = 5;
            menuRunSim.Enabled = false;
            menuRunSim.Click += new EventHandler(this.RunSimulation);

            menuStopSim = new ToolStripMenuItem();
            menuStopSim.Name = "MenuItemSuspendSimulation";
            menuStopSim.Size = new Size(96, 22);
            menuStopSim.Text = MessageResources.MenuItemSuspend;
            menuStopSim.Tag = 6;
            menuStopSim.Image = (Image)resources.GetObject("media_pause"); 
            menuStopSim.Enabled = false;
            menuStopSim.Click += new EventHandler(this.StopSimulation);

            menuResetSim = new ToolStripMenuItem();
            menuResetSim.Name = "MenuItemStopSimulation";
            menuResetSim.Size = new Size(96, 22);
            menuResetSim.Image = (Image)resources.GetObject("media_stop_red");
            menuResetSim.Text = MessageResources.MenuItemStop;
            menuResetSim.Tag = 7;
            menuResetSim.Enabled = false;
            menuResetSim.Click += new EventHandler(this.ResetSimulation);

            menuStepSim = new ToolStripMenuItem();
            menuStepSim.Name = "MenuItemStepSimulation";
            menuStepSim.Size = new Size(96, 22);
            menuStepSim.Image = (Image)resources.GetObject("media_step_forward");
            menuStepSim.Text = MessageResources.MenuItemStep;
            menuStepSim.Tag = 8;
            menuStepSim.Enabled = false;
            menuStepSim.Click += new EventHandler(this.Step);

            ToolStripSeparator sep1 = new ToolStripSeparator();
            sep1.Tag = 9;

            menuSteppingModel = new ToolStripMenuItem();
            menuSteppingModel.Name = "menuSteppingModel";
            menuSteppingModel.Size = new Size(96, 22);
            menuSteppingModel.Text = MessageResources.MenuItemSteppingModel;
            menuSteppingModel.Tag = 10;
            menuSteppingModel.Enabled = false;

            for (int i = 1; i <= 10; i++)
            {
                ToolStripMenuItem tmpMenu = new ToolStripMenuItem();
                tmpMenu.Name = "menuSteppingModel" + i.ToString();
                tmpMenu.Size = new Size(96, 22);
                tmpMenu.Text = "";
                tmpMenu.Tag = i;
                tmpMenu.Enabled = false;
                tmpMenu.Visible = false;
                tmpMenu.Click += new EventHandler(ClickLoadingSteppingModel);
                menuSteppingModelDic.Add(i, tmpMenu);
                menuSteppingModel.DropDownItems.Add(tmpMenu);
            }


            MenuItemRun = new ToolStripMenuItem();
            MenuItemRun.Name = "MenuItemRun";
            MenuItemRun.Size = new Size(36, 20);
            MenuItemRun.Text = "Run";
            MenuItemRun.DropDownItems.AddRange(new ToolStripItem[] {
                menuRunSim,
                menuStopSim,
                menuResetSim,
                menuStepSim,
                sep1,
                menuSteppingModel
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
            m_paramsCombo.ToolTipText = MessageResources.MenuToolTipParamCombo;

            m_runButton = new ToolStripButton();
            m_runButton.Image = (Image)resources.GetObject("media_play_green");
            m_runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_runButton.Tag = 2;
            m_runButton.Name = "RunSimulation";
            m_runButton.Size = new System.Drawing.Size(23, 22);
            m_runButton.Text = "";
            m_runButton.ToolTipText = MessageResources.MenuToolTipRun;
            m_runButton.Click += new System.EventHandler(this.RunSimulation);

            m_stopButton = new ToolStripButton();
            m_stopButton.Image = (Image)resources.GetObject("media_pause");
            m_stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_stopButton.Name = "StopSimulation";
            m_stopButton.Size = new System.Drawing.Size(23, 22);
            m_stopButton.Tag = 3;
            m_stopButton.Text = "";
            m_stopButton.ToolTipText = MessageResources.MenuToolTipSuspend;
            m_stopButton.Click += new System.EventHandler(this.StopSimulation);

            m_resetButton = new ToolStripButton();
            m_resetButton.Image = (Image)resources.GetObject("media_stop_red");
            m_resetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_resetButton.Name = "ResetSimulation";
            m_resetButton.Size = new System.Drawing.Size(23, 22);
            m_resetButton.Text = "";
            m_resetButton.Tag = 4;
            m_resetButton.ToolTipText = MessageResources.MenuToolTipReset;
            m_resetButton.Click += new System.EventHandler(this.ResetSimulation);

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
            m_stepButton.ToolTipText = MessageResources.MenuToolTipStep;
            m_stepButton.Click += new System.EventHandler(this.Step);

            m_stepText = new ToolStripTextBox();
            m_stepText.Name = "StepText";
            m_stepText.Size = new System.Drawing.Size(60, 25);
            m_stepText.Text = "1";
            m_stepText.Tag = 10;
            m_stepText.TextBoxTextAlign = HorizontalAlignment.Right;
            m_stepText.ToolTipText = MessageResources.MenuToolTipStepText;

            m_stepUnitCombo = new ToolStripComboBox();
            m_stepUnitCombo.Name = "StepCourse";
            m_stepUnitCombo.Size = new System.Drawing.Size(50, 25);
            m_stepUnitCombo.AutoSize = false;
            m_stepUnitCombo.ToolTipText = MessageResources.MenuToolTipStepUnitCombo;
            m_stepUnitCombo.Tag = 11;
            m_stepUnitCombo.Items.Add("Step");
            m_stepUnitCombo.Items.Add("Sec");
            m_stepUnitCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            m_stepUnitCombo.SelectedIndex = 0;
            m_stepUnitCombo.SelectedIndexChanged += new EventHandler(m_stepUnitCombo_SelectedIndexChanged);

            m_separator = new ToolStripSeparator();
            m_separator.Visible = false;
            m_separator.Tag = 12;

            m_backButton = new ToolStripButton();
            m_backButton.Image = (Image)resources.GetObject("nav_left_blue");
            m_backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_backButton.Name = "BackStep";
            m_backButton.Size = new System.Drawing.Size(23, 22);
            m_backButton.Tag = 13;
            m_backButton.Text = "";
            m_backButton.Enabled = false;
            m_backButton.Visible = false;
            m_backButton.ToolTipText = MessageResources.MenuToolTipBack;
            m_backButton.Click += new System.EventHandler(this.StepBackButtonClicked);

            m_fowardButton = new ToolStripButton();
            m_fowardButton.Image = (Image)resources.GetObject("nav_right_blue");
            m_fowardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            m_fowardButton.Name = "ForwardStep";
            m_fowardButton.Size = new System.Drawing.Size(23, 22);
            m_fowardButton.Tag = 14;
            m_fowardButton.Text = "";
            m_fowardButton.Enabled = false;
            m_fowardButton.Visible = false;
            m_fowardButton.ToolTipText = MessageResources.MenuToolTipFoward;
            m_fowardButton.Click += new System.EventHandler(this.StepForwardButtonClicked);

            ButtonList = new ToolStrip();

            ButtonList.Items.AddRange( new ToolStripItem[] {
                m_paramsCombo,
                m_runButton,
                m_stopButton,
                m_resetButton,
                timeLabel,
                m_timeText,
                secLabel,
                sep,
                m_stepButton,
                m_stepText,
                m_stepUnitCombo,
                m_separator,
                m_backButton,
                m_fowardButton
            });
            ButtonList.Location = new Point(400, 0);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the flag whether stepping model is saved.
        /// </summary>
        public bool IsSaveSteppingModel
        {
            set
            {
                this.m_separator.Visible = value;
                this.m_backButton.Visible = value;
                this.m_fowardButton.Visible = value;
            }
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Initialize this plugin.       
        /// </summary>
        public override void Initialize()
        {
            m_env.DataManager.DisplayFormatEvent += new DisplayFormatChangedEventHandler(DataManager_DisplayFormatEvent);
            m_env.DataManager.SteppingModelEvent += new SteppingModelChangedEventHandler(DataManager_SteppingModelEvent);
            m_env.DataManager.ApplySteppingModelEvent += new ApplySteppingModelEnvetHandler(DataManager_ApplySteppingModelEvent);
        }


        /// <summary>
        /// Get the menu list for Simulation
        /// </summary>
        /// <returns>MenuItemRun and MenuItemSetup.</returns>
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
            int aindex = m_paramsCombo.SelectedIndex;
            if (m_paramsCombo.Items.Contains(parameterID))
            {
                int index = m_paramsCombo.Items.IndexOf(parameterID);
                m_paramsCombo.Items.RemoveAt(index);
                if (aindex == index)
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
                m_timeText.Text =  time.ToString(m_env.DataManager.DisplayStringFormat);
            m_time = time;
        }

        /// <summary>
        /// Get the property setting in the common setting dialog.
        /// </summary>
        /// <returns>the list of property items.</returns>
        public override List<IPropertyItem> GetPropertySettings()
        {
            PropertyNode node = new PropertyNode(MessageResources.NameSimulation);
            node.Nodes.Add(new PropertyNode(new SimulationConfigurationPage(m_env.DataManager, this)));

            List<IPropertyItem> nodeList = new List<IPropertyItem>();
            nodeList.Add(node);

            return nodeList;
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">project status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            bool isUninitialized = type == ProjectStatus.Uninitialized;
            bool isLoaded = type == ProjectStatus.Loaded;
            bool isRunning = type == ProjectStatus.Running;
            bool isStepping = type == ProjectStatus.Stepping;
            bool isSuspended = type == ProjectStatus.Suspended;

            // Show toolbar when the model is loaded.
            this.ButtonList.Visible = !isUninitialized;

            // Set Menu Enabled.
            menuRunSim.Enabled = isLoaded || isSuspended;
            menuStopSim.Enabled = isRunning || isStepping;
            menuResetSim.Enabled = isSuspended;
            menuStepSim.Enabled = isLoaded || isSuspended;
            menuSetupSim.Enabled = isLoaded;

            menuRunSim.Checked = isRunning;
            menuStopSim.Checked = isSuspended;
            menuStepSim.Checked = isStepping;

            // Set Button Enabled.
            m_runButton.Enabled = isLoaded || isSuspended;
            m_stopButton.Enabled = isRunning || isStepping;
            m_resetButton.Enabled = isSuspended;
            m_stepButton.Enabled = isLoaded || isSuspended;
            m_fowardButton.Enabled = false;
            m_backButton.Enabled = !isRunning && !isStepping && !isLoaded && m_dManager.SaveTime.Count > 1;

            m_runButton.Checked = isRunning;
            m_stopButton.Checked = isSuspended;
            m_stepButton.Checked = isStepping;

            // Set ComboBox for Params.
            m_timeText.Enabled = isLoaded || isStepping || isRunning || isSuspended;
            if (isUninitialized || isLoaded)
            {
                m_timeText.Text = "0";
                m_time = 0.0;
            }
            m_stepText.Enabled = isLoaded || isSuspended;
            m_stepUnitCombo.Enabled = isLoaded || isSuspended;
            m_paramsCombo.Enabled = isLoaded;

            m_type = type;
            m_loadedModel = 0.0;
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

        /// <summary>
        /// Show the simulation setting dialog with the set ID.
        /// </summary>
        /// <param name="viewParamID">the simulation parameter ID.</param>
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
                                if (sps.Name.Equals(Constants.defaultSimParam))
                                    m_dManager.ImportSimulationParameter(pmsp.ModelID, sps.Name, pairs);
                                else
                                    m_dManager.UpdateInitialCondition(sps.Name, pmsp.ModelID, pairs);
                            }
                        }

                        foreach (String key in delList)
                        {
                            m_dManager.DeleteSimulationParameter(key);
                        }

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
        /// Event when display format is changed.
        /// </summary>
        /// <param name="o">DataManager.</param>
        /// <param name="e">DisplayFormatEventArgs</param>
        private void DataManager_DisplayFormatEvent(object o, Ecell.Events.DisplayFormatEventArgs e)
        {
            m_timeText.Text = m_time.ToString(m_env.DataManager.DisplayStringFormat);
        }

        /// <summary>
        /// Event when the stepping model is changed.
        /// </summary>
        /// <param name="o">DataManager.</param>
        /// <param name="e">SteppingModelEventArgs</param>
        private void DataManager_SteppingModelEvent(object o, SteppingModelEventArgs e)
        {
            for (int i = 1; i <= 10; i++)
            {
                menuSteppingModelDic[i].Visible = false;
                menuSteppingModelDic[i].Enabled = false;
            }

            foreach (int id in m_dManager.SaveTime.Keys)
            {
                menuSteppingModelDic[id].Visible = true;
                menuSteppingModelDic[id].Enabled = true;
                menuSteppingModelDic[id].Text = m_dManager.SaveTime[id].ToString();
            }
            if (m_dManager.SaveTime.Count > 1)
            {
                menuSteppingModel.Enabled = true;
                m_backButton.Enabled = true;
                m_fowardButton.Enabled = false;
            }
            else
            {
                menuSteppingModel.Enabled = false;
                m_backButton.Enabled = false;
                m_fowardButton.Enabled = false;
            }
        }
        
        /// <summary>
        /// Event when the stepping model is applied.
        /// </summary>
        /// <param name="o">DataManager</param>
        /// <param name="e">SteppingModelEventArgs</param>
        private void DataManager_ApplySteppingModelEvent(object o, SteppingModelEventArgs e)
        {
            if (m_type == ProjectStatus.Uninitialized || m_type == ProjectStatus.Loading)
                return;
            if (e.ApplyTime < 0.0)
            {                
                m_timeText.Text = DataManager.CurrentProject.Simulator.GetCurrentTime().ToString(m_env.DataManager.DisplayStringFormat);
            }
            else
            {
                m_timeText.Text = e.ApplyTime.ToString(m_env.DataManager.DisplayStringFormat);
            }
            int index = m_dManager.SaveTime.Count;
            int count = 0;
            foreach (int id in m_dManager.SaveTime.Keys)
            {
                if (m_dManager.SaveTime[id] == e.ApplyTime)
                {
                    if (count == 0)
                        m_fowardButton.Enabled = false;
                    else
                        m_fowardButton.Enabled = true;

                    if (count == index - 1)
                        m_backButton.Enabled = false;
                    else
                        m_backButton.Enabled = true;
                }
                count++;
            }
        }

        /// <summary>
        /// The action of [Simulation] menu click.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void SetupSimulation(object sender, EventArgs e)
        {
            ShowSetupSimulationDialog(null);
        }

        /// <summary>
        /// The action of [run ...] menu click.
        /// start simulation after you click run button.
        /// </summary>
        /// <param name="sender">object(ToolStripButton)</param>
        /// <param name="e">EventArgs</param>
        private void RunSimulation(object sender, EventArgs e)
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
            catch (Exception ex)
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
        private void StopSimulation(object sender, EventArgs e)
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
        private void Step(object sender, EventArgs e)
        {
            if (m_type == ProjectStatus.Running) return;
            if (m_type == ProjectStatus.Uninitialized) return;
            ProjectStatus preType = m_type;
            try
            {
                if (m_stepUnitCombo.Text == "Step")
                {
                    int stepCount = Convert.ToInt32(m_stepText.Text);
                    if (stepCount <= 0)
                        throw new EcellException(MessageResources.ErrInvalidValue);
                }
                else
                {
                    double timeCount = Convert.ToDouble(m_stepText.Text);
                    if (timeCount <= 0)
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
                    m_dManager.StartStepSimulation(stepCount, true);
                }
                else
                {
                    double timeCount = Convert.ToDouble(m_stepText.Text);
                    if (timeCount < 0) return;
                    // m_dManager.SimulationStartKeepSetting(timeCount); 
                    // m_dManager.SimulationStart(timeCount);
                    m_dManager.StartStepSimulation(timeCount, true);
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
        private void ResetSimulation(object sender, EventArgs e)
        {
            m_stepText.Control.Update();

            if (m_type != ProjectStatus.Running &&
                    m_type != ProjectStatus.Suspended &&
                    m_type != ProjectStatus.Stepping)
                return;

            if (!Util.ShowYesNoDialog(MessageResources.ConfirmReset)) return;
            ProjectStatus preType = m_type;
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
            m_pManager.ChangeStatus(ProjectStatus.Loaded);
        }

        /// <summary>
        /// Back button for the stepping model is clickend.
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">EventArgs.</param>
        private void StepBackButtonClicked(object sender, EventArgs e)
        {
            foreach (int ind in m_dManager.SaveTime.Keys)
            {                
                if (m_dManager.SaveTime[ind] == m_loadedModel)
                {
                    m_dManager.LoadSteppingModel(ind + 1);
                    m_loadedModel = m_dManager.SaveTime[ind + 1];
                    return;
                }
            }
            m_dManager.LoadSteppingModel(1);
            m_loadedModel = m_dManager.SaveTime[1];
        }

        /// <summary>
        /// Foward button for the stepping model is clicked.
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">EventArgs.</param>
        private void StepForwardButtonClicked(object sender, EventArgs e)
        {
            foreach (int ind in m_dManager.SaveTime.Keys)
            {
                if (m_dManager.SaveTime[ind] == m_loadedModel)
                {
                    m_dManager.LoadSteppingModel(ind - 1);
                    m_loadedModel = m_dManager.SaveTime[ind - 1];
                    return;
                }
            }
        }

        /// <summary>
        /// Event when ParamComboBox is changed.
        /// </summary>
        /// <param name="sender">ToolStripComboBox</param>
        /// <param name="e">EventArgs</param>
        private void ParameterSelectedIndexChanged(object sender, EventArgs e)
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

        /// <summary>
        /// Event when ComboBox of step unit is changed.
        /// </summary>
        /// <param name="sender">ComboBox.</param>
        /// <param name="e">EventArgs</param>
        private void m_stepUnitCombo_SelectedIndexChanged(object sender, EventArgs e)
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

        /// <summary>
        /// Event when ToolStripMenuItem for loading the stepping model is clicked.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ClickLoadingSteppingModel(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            foreach (int ind in menuSteppingModelDic.Keys)
            {
                if (menu.Equals(menuSteppingModelDic[ind]))
                {
                    m_dManager.LoadSteppingModel(ind);
                    m_loadedModel = m_dManager.SaveTime[ind];
                    return;
                }
            }
            
        }
        #endregion
    }
}
