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
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using EcellLib.SessionManager;
using WeifenLuo.WinFormsUI.Docking;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Plugin class to manage the result and parameter of analysis.
    /// </summary>
    public class Analysis : IEcellPlugin
    {
        #region Fields
        /// <summary>
        /// MenuItem to display the window for robust analysis.
        /// </summary>
        private ToolStripMenuItem m_robustAnalysisWinItem;
        /// <summary>
        /// MenuItem to execute robust analysis.
        /// </summary>
        private ToolStripMenuItem m_robustAnalysisItem;
        /// <summary>
        /// MenuItem to execute parameter estimation.
        /// </summary>
        private ToolStripMenuItem m_parameterEstimationItem;
        /// <summary>
        /// MenuItem to execute sensitivity analysis.
        /// </summary>
        private ToolStripMenuItem m_sensitivityAnalysisItem;
        /// <summary>
        /// MenuItem to execute bifurcation analysis.
        /// </summary>
        private ToolStripMenuItem m_bifurcationAnalysisItem;
        /// <summary>
        /// Window to analysis the robustness of model.
        /// </summary>
        private AnalysisWindow m_win = null;
        /// <summary>
        /// SessionManager.
        /// </summary>
        private SessionManager.SessionManager m_manager = SessionManager.SessionManager.GetManager();
        /// <summary>
        /// ResourceManager for AnalysisTemplate.
        /// </summary>
        static public ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResAnalysis));
        /// <summary>
        /// Robust Analysis Class.
        /// </summary>
        private RobustAnalysis m_robustAnalysis;
        /// <summary>
        /// Parameter Estimation Class.
        /// </summary>
        private ParameterEstimation m_parameterEstimation;
        /// <summary>
        /// Bifurcation Analysis Calss.
        /// </summary>
        private BifurcationAnalysis m_bifurcationAnalysis;
        /// <summary>
        /// Sensitivity Analysis Class.
        /// </summary>
        private SensitivityAnalysis m_sensitivityAnalysis;
        #endregion

        /// <summary>
        /// Window is null when window is closed.
        /// </summary>
        public void CloseAnalysisWindow()
        {
            m_win = null;
            m_robustAnalysis = null;
            m_robustAnalysisItem.Enabled = false;
        }

        /// <summary>
        /// Stop the robust analysis.
        /// </summary>
        public void StopRobustAnalysis()
        {
            m_robustAnalysis = null;
        }

        /// <summary>
        /// Stop the parameter estimation.
        /// </summary>
        public void StopParameterEstimation()
        {
            m_parameterEstimation = null;
        }

        /// <summary>
        /// Stop the sensitivity analysis.
        /// </summary>
        public void StopSensitivityAnalysis()
        {
            m_sensitivityAnalysis = null;
        }

        /// <summary>
        /// Stop the bifurcation analysis.
        /// </summary>
        public void StopBifurcationAnalysis()
        {
            m_bifurcationAnalysis = null;
        }

        #region Events
        /// <summary>
        /// Event when this form of robust analysis is shown.
        /// </summary>
        /// <param name="sender">RobustAnalysis.</param>
        /// <param name="e">EventArgs.</param>
        private void ShowRobustAnalysisWindow(object sender, EventArgs e)
        {
            if (m_win == null)
            {
                m_win = new AnalysisWindow();
                m_win.Control = this;
                DockPanel panel = PluginManager.GetPluginManager().DockPanel;
                m_win.DockHandler.DockPanel = panel;
                m_win.DockHandler.FloatPane = panel.DockPaneFactory.CreateDockPane(m_win, DockState.Float, true);
                FloatWindow fw = panel.FloatWindowFactory.CreateFloatWindow(
                                    panel,
                                    m_win.FloatPane,
                                    new Rectangle(m_win.Left, m_win.Top, m_win.Width, m_win.Height));
                m_win.Pane.DockTo(fw);
                m_win.Show();
            }
            else
            {
                m_win.Activate();
            }
            m_robustAnalysisItem.Enabled = true;
        }

        /// <summary>
        /// Event when the menu to execute robust analysis is clicked.
        /// This program execute the program of robust analysis.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs.</param>
        private void ExecuteRobustAnalysis(object sender, EventArgs e)
        {
            if (m_win == null) return;
            if (m_robustAnalysis != null && m_robustAnalysis.IsRunning)
            {
                string mes = Analysis.s_resources.GetString("ConfirmStopAnalysis");
                DialogResult res = MessageBox.Show(mes, "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    m_robustAnalysis.StopAnalysis();
                }
                return;
            }           
            m_robustAnalysis = new RobustAnalysis();
            m_robustAnalysis.Control = this;
            m_robustAnalysis.ExecuteAnalysis();
        }

        /// <summary>
        /// Event when the menu to execute parameter estimation is clicked.
        /// This program execute the program of parameter estimation.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs.</param>
        private void ExecuteParameterEstimation(object sender, EventArgs e)
        {
            if (m_win == null) return;
            if (m_parameterEstimation != null && m_parameterEstimation.IsRunning)
            {
                string mes = Analysis.s_resources.GetString("ConfirmStopAnalysis");
                DialogResult res = MessageBox.Show(mes, "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    m_parameterEstimation.StopAnalysis();
                }
                return;
            }  
            m_parameterEstimation = new ParameterEstimation();
            m_parameterEstimation.Control = this;
            m_parameterEstimation.ExecuteAnalysis();
        }

        /// <summary>
        /// Event when the menu to execute sensitivity analysis is clicked.
        /// This program execute the program of sensitivity analysis.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs.</param>
        private void ExecuteSensitivityAnalysis(object sender, EventArgs e)
        {
            if (m_win == null) return;
            if (m_sensitivityAnalysis != null && m_sensitivityAnalysis.IsRunning)
            {
                string mes = Analysis.s_resources.GetString("ConfirmStopAnalysis");
                DialogResult res = MessageBox.Show(mes, "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    m_sensitivityAnalysis.StopAnalysis();
                }
                return;
            }
            m_sensitivityAnalysis = new SensitivityAnalysis();
            m_sensitivityAnalysis.Control = this;
            m_sensitivityAnalysis.ExecuteAnalysis();
        }

        /// <summary>
        /// Event when the menu to execute bifurcation analysis is clicked.
        /// This program execute the program of bifurcation analysis.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs.</param>
        private void ExecuteBifurcationAnalysis(object sender, EventArgs e)
        {
            if (m_win == null) return;
            if (m_bifurcationAnalysis != null && m_bifurcationAnalysis.IsRunning)
            {
                string mes = Analysis.s_resources.GetString("ConfirmStopAnalysis");
                DialogResult res = MessageBox.Show(mes, "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    m_bifurcationAnalysis.StopAnalysis();
                }
                return;
            }
            m_bifurcationAnalysis = new BifurcationAnalysis();
            m_bifurcationAnalysis.Control = this;
            m_bifurcationAnalysis.ExecuteAnalysis();
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for Analysis plugin.
        /// </summary>
        /// <returns>the list of menu.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResAnalysis));
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();

            m_robustAnalysisWinItem = new ToolStripMenuItem();
            m_robustAnalysisWinItem.Text = resources.GetString("MenuItemAnalysisWindow");
            m_robustAnalysisWinItem.ToolTipText = resources.GetString("MenuItemAnalysisWindow");
            m_robustAnalysisWinItem.Tag = 50;
            m_robustAnalysisWinItem.Click += new EventHandler(ShowRobustAnalysisWindow);

            ToolStripMenuItem viewMenu = new ToolStripMenuItem();
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { m_robustAnalysisWinItem });
            viewMenu.Text = "View";
            viewMenu.Name = "MenuItemView";

            list.Add(viewMenu);

            m_robustAnalysisItem = new ToolStripMenuItem();
            m_robustAnalysisItem.Text = resources.GetString("MenuItemRobustAnalysis");
            m_robustAnalysisItem.ToolTipText = resources.GetString("MenuItemRobustAnalysis");
            m_robustAnalysisItem.Tag = 50;
            m_robustAnalysisItem.Enabled = false;
            m_robustAnalysisItem.Click += new EventHandler(ExecuteRobustAnalysis);

            m_parameterEstimationItem = new ToolStripMenuItem();
            m_parameterEstimationItem.Text = resources.GetString("MenuItemParameterEstimation");
            m_parameterEstimationItem.ToolTipText = resources.GetString("MenuItemParameterEstimation");
            m_parameterEstimationItem.Tag = 60;
            m_parameterEstimationItem.Enabled = false;
            m_parameterEstimationItem.Click += new EventHandler(ExecuteParameterEstimation);

            m_sensitivityAnalysisItem = new ToolStripMenuItem();
            m_sensitivityAnalysisItem.Text = resources.GetString("MenuItemSensitivityAnalysis");
            m_sensitivityAnalysisItem.ToolTipText = resources.GetString("MenuItemSensitivityAnalysis");
            m_sensitivityAnalysisItem.Tag = 70;
            m_sensitivityAnalysisItem.Enabled = false;
            m_sensitivityAnalysisItem.Click += new EventHandler(ExecuteSensitivityAnalysis);

            m_bifurcationAnalysisItem = new ToolStripMenuItem();
            m_bifurcationAnalysisItem.Text = resources.GetString("MenuItemBifurcationAnalysis");
            m_bifurcationAnalysisItem.ToolTipText = resources.GetString("MenuItemBifurcationAnalysis");
            m_bifurcationAnalysisItem.Tag = 80;
            m_bifurcationAnalysisItem.Enabled = false;
            m_bifurcationAnalysisItem.Click += new EventHandler(ExecuteBifurcationAnalysis);

            ToolStripMenuItem analysisMenu = new ToolStripMenuItem();
            analysisMenu.DropDownItems.AddRange(new ToolStripItem[] { m_robustAnalysisItem, m_parameterEstimationItem, 
                m_sensitivityAnalysisItem, m_bifurcationAnalysisItem });
            analysisMenu.Text = "Analysis";
            analysisMenu.Name = "MenuItemAnalysis";

            list.Add(analysisMenu);

            return list;
        }

        /// <summary>
        /// Get toolbar buttons for Analysis plugin.
        /// </summary>
        /// <returns>the list of ToolBarMenu.</returns>
        public List<System.Windows.Forms.ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Called by PluginManager for getting UseControl.
        /// </summary>
        /// <returns>nothing.</returns>
        public List<EcellDockContent> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(ProjectStatus type)
        {
            if (ProjectStatus.Loaded == type)
            {
                m_robustAnalysisWinItem.Enabled = true;
                m_parameterEstimationItem.Enabled = true;
                m_sensitivityAnalysisItem.Enabled = true;
                m_bifurcationAnalysisItem.Enabled = true;
            }
            else
            {
                m_robustAnalysisWinItem.Enabled = false;
                m_parameterEstimationItem.Enabled = false;
                m_sensitivityAnalysisItem.Enabled = false;
                m_bifurcationAnalysisItem.Enabled = false;
            }
        }

        /// <summary>
        /// Change availability of undo/redo function
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Called by PluginManager for newly added EcellObjects on the core.
        /// </summary>
        /// <param name="data">List of EcellObjects to be added</param>
        public void DataAdd(List<EcellObject> data)
        {
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
            if (m_win != null)
                m_win.DataChanged(modelID, key, type, data);
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
        /// The event sequence when the simulation parameter is set.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterSet(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true.</returns>
        public List<String> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            return names;
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false.</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="log">Changed value of object.</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> log)
        {
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public void LoggerAdd(string modelID, string type, string key, string path)
        {
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print(string name)
        {
            return null;
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public void SaveModel(string modelID, string directory)
        {
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
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
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"PathwayWindow"</returns> 
        public string GetPluginName()
        {
            return "Analysis";
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
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
        }
        #endregion
    }


    /// <summary>
    /// Enum of type of the estimation formulators.
    /// </summary>
    public enum EstimationFormulatorType
    {
        /// <summary>
        /// Use max value at the finished simulation time.
        /// </summary>
        Max = 0,
        /// <summary>
        /// Use min value at the finished simulation time.
        /// </summary>
        Min = 1,
        /// <summary>
        /// Use neally 0 at the finished simulation time.
        /// </summary>
        EqualZero = 2,
        /// <summary>
        /// Use max value while simulation is executed.
        /// </summary>
        SumMax = 3,
        /// <summary>
        /// Use min value while simulation is executed.
        /// </summary>
        SumMin = 4,
        /// <summary>
        /// Use neally 0 while simulation is executed.
        /// </summary>
        SumEqualZero = 5
    }
}
