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
        /// m_text (TextBox of displaying simulation time)
        /// </summary>
        private ToolStripTextBox m_text = null;
        /// <summary>
        /// the step interval setting text box.
        /// </summary>
        private ToolStripTextBox m_text1 = null;
        /// <summary>
        /// the unit of step interval.
        /// </summary>
        private ToolStripComboBox m_combo1 = null;
        /// <summary>
        /// DataManager.
        /// </summary>
        private DataManager m_dManager = null;
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
        private PluginManager m_pManager;
        /// <summary>
        /// ResourceManager for NewParameterWindow.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResSimulation));
        #endregion

        /// <summary>
        /// Constructor for Simulation.
        /// </summary>
        public Simulation()
        {
            m_dManager = DataManager.GetDataManager();
            m_pManager = PluginManager.GetPluginManager();
        }

        #region PluginBase
        /// <summary>
        /// Get manustripts for Simulation
        /// [Run]   -> [Run ...]
        ///            [Suspend ...]
        ///            [Stop ...]
        /// [Setup] -> [Simulation]
        /// </summary>
        /// <returns>MenuStripItems</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulation));
            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_runSim = new ToolStripMenuItem();
            m_runSim.Name = "MenuItemRunSimulation";
            m_runSim.Size = new Size(96, 22);
            m_runSim.Image = (Image)resources.GetObject("media_play_green");
            m_runSim.Text = Simulation.s_resources.GetString("MenuItemRun");
//            resources.ApplyResources(m_runSim, "MenuItemRun");
            m_runSim.Enabled = false;
            m_runSim.Click += new EventHandler(this.RunSimulation);

            m_suspendSim = new ToolStripMenuItem();
            m_suspendSim.Name = "MenuItemSuspendSimulation";
            m_suspendSim.Size = new Size(96, 22);
//            m_suspendSim.Text = "Suspend ...";
            m_suspendSim.Text = Simulation.s_resources.GetString("MenuItemSuspend");
            m_suspendSim.Image = (Image)resources.GetObject("media_pause"); 
//            resources.ApplyResources(m_suspendSim, "MenuItemSuspend");
            m_suspendSim.Enabled = false;
            m_suspendSim.Click += new EventHandler(this.SuspendSimulation);

            m_stopSim = new ToolStripMenuItem();
            m_stopSim.Name = "MenuItemStopSimulation";
            m_stopSim.Size = new Size(96, 22);
//            m_stopSim.Text = "Stop ...";
            m_stopSim.Image = (Image)resources.GetObject("media_stop_red");
            m_stopSim.Text = Simulation.s_resources.GetString("MenuItemStop");
            //            resources.ApplyResources(m_stopSim, "MenuItemStop");
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
//            m_setupSim.Text = "Simulation";
//            resources.ApplyResources(m_setupSim, "MenuItemSetupSim");
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
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulation));

            List<ToolStripItem> list = new List<ToolStripItem>();
            ToolStripButton button1 = new ToolStripButton();
//            button1.Image = (Image)resources.GetObject("green3");
            button1.Image = (Image)resources.GetObject("media_play_green");
            button1.ImageTransparentColor = System.Drawing.Color.Magenta;
            button1.Tag = 0;
            button1.Name = "RunSimulation";
            
            button1.Size = new System.Drawing.Size(23, 22);
            button1.Text = "";
            button1.ToolTipText = "Run";
            button1.Click += new System.EventHandler(this.RunSimulation);
            list.Add(button1);

            ToolStripButton button3 = new ToolStripButton();
//            button3.Image = (Image)resources.GetObject("blue1");
            button3.Image = (Image)resources.GetObject("media_pause");
            button3.ImageTransparentColor = System.Drawing.Color.Magenta;
            button3.Name = "SuspendSimulation";
            button3.Size = new System.Drawing.Size(23, 22);
            button3.Tag = 2;
            button3.Text = "";
            button3.ToolTipText = "Suspend";
            button3.Click += new System.EventHandler(this.SuspendSimulation);
            list.Add(button3);

            ToolStripButton button2 = new ToolStripButton();
//            button2.Image = (Image)resources.GetObject("red1");
            button2.Image = (Image)resources.GetObject("media_stop_red");
            button2.ImageTransparentColor = System.Drawing.Color.Magenta;
            button2.Name = "StopSimulation";
            button2.Size = new System.Drawing.Size(23, 22);
            button2.Text = "";
            button2.Tag = 3;
            button2.ToolTipText = "Reset";
            button2.Click += new System.EventHandler(this.ResetSimulation);
            list.Add(button2);

            ToolStripLabel label1 = new ToolStripLabel();
            label1.Name = "TimeLabel";
            label1.Size = new System.Drawing.Size(81, 22);
            label1.Text = " Time: ";
            label1.Tag = 4;
            list.Add(label1);

            m_text = new ToolStripTextBox();
            m_text.Name = "TimeText";
            m_text.Size = new System.Drawing.Size(80, 25);
            m_text.Text = "0";
            m_text.ReadOnly = true;
            m_text.Tag = 5;
            m_text.TextBoxTextAlign =  HorizontalAlignment.Right;
            list.Add(m_text);

            ToolStripLabel label2 = new ToolStripLabel();
            label2.Name = "SecLabel";
            label2.Size = new System.Drawing.Size(50, 22);
            label2.Text = "sec";
            label2.Tag = 6;
            list.Add(label2);

            ToolStripSeparator sep = new ToolStripSeparator();
            sep.Tag = 7;
            list.Add(sep);

            ToolStripButton button4 = new ToolStripButton();
            button4.Image = (Image)resources.GetObject("media_step_forward");
            button4.ImageTransparentColor = System.Drawing.Color.Magenta;
            button4.Name = "StepSimulation";
            button4.Size = new System.Drawing.Size(23, 22);
            button4.Text = "";
            button4.Tag = 8;
            button4.ToolTipText = "Step";
            button4.Click += new System.EventHandler(this.Step);
            list.Add(button4);

            m_text1 = new ToolStripTextBox();
            m_text1.Name = "StepText";
            m_text1.Size = new System.Drawing.Size(60, 25);
            m_text1.Text = "1";
            m_text1.Tag = 9;
            m_text1.TextBoxTextAlign = HorizontalAlignment.Right;
            list.Add(m_text1);

            m_combo1 = new ToolStripComboBox();
            m_combo1.Name = "StepCourse";
            m_combo1.Size = new System.Drawing.Size(50, 25);
            m_combo1.AutoSize = false;
            m_combo1.Tag = 10;
            m_combo1.Items.Add("Step");
            m_combo1.Items.Add("Sec");
            m_combo1.SelectedText = "Step";
            list.Add(m_combo1);


            return list;
        }

        /// <summary>
        /// Get the windows form for Simulation.
        /// </summary>
        /// <returns>null</returns>
        public List<EcellDockContent> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            if (type == Constants.xpathParameters && key != Constants.xpathParameters)
            {
                SetupSimulation(new Button(), new EventArgs());
            }
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            // not implement
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public void LoggerAdd(string modelID, string key, string type, string path)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID.</param>
        public void ParameterAdd(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is deleted.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterDelete(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="data">Changed value of object.</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            // nothing
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
            if (m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping)
            m_text.Text = time.ToString();
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(ProjectStatus type)
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
                m_text.Text = "0";
                m_text.ForeColor = Color.Black;
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
                m_text.ForeColor = Color.Black;
                m_text.BackColor = m_text.BackColor;
            }
            else if (type == ProjectStatus.Suspended)
            {
                m_runSim.Enabled = true;
                m_stopSim.Enabled = true;
                m_suspendSim.Enabled = false;
                m_setupSim.Enabled = true;
                m_text.ForeColor = Color.Gray;
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
        /// Change availability of undo/redo status
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        /// <summary>
        /// Save the selected model to directory.
        /// </summary>
        /// <param name="modelID">selected model.</param>
        /// <param name="directory">output directory.</param>
        public void SaveModel(string modelID, string directory)
        {
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print()
        {
            return null;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"Simulation"</returns>
        public string GetPluginName()
        {
            return "Simulation";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>false</returns>
        public bool IsEnablePrint()
        {
            return false;
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
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
                if (m_combo1.Text == "Step")
                {
                    int stepCount = Convert.ToInt32(m_text1.Text);
                    if (stepCount < 0) return;
                    m_dManager.SimulationStartKeepSetting(stepCount); // m_dManager.SimulationStart(stepCount);
                }
                else
                {
                    double timeCount = Convert.ToDouble(m_text1.Text);
                    if (timeCount < 0) return;
                    m_dManager.SimulationStartKeepSetting(timeCount); // m_dManager.SimulationStart(timeCount);
                    m_pManager.ChangeStatus(ProjectStatus.Stepping);
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
        #endregion
    }
}
