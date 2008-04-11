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
using EcellLib.Plugin;
using EcellLib.Objects;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Plugin class to manage the result and parameter of analysis.
    /// </summary>
    public class Analysis : PluginBase
    {
        #region Fields
        /// <summary>
        /// MenuItem to display the window for robust analysis.
        /// </summary>
        private ToolStripMenuItem m_showAnalysisSetupItem;
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
        private AnalysisResultWindow m_rWin = null;
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

        private BifurcationAnalysisParameter m_bifurcateParameter;
        private ParameterEstimationParameter m_estimationParameter;
        private RobustAnalysisParameter m_robustParameter;
        private SensitivityAnalysisParameter m_sensitivityParameter;
        /// <summary>
        /// The dictionary of the data to be set by random.
        /// </summary>
        private Dictionary<string, EcellData> m_paramList = new Dictionary<string, EcellData>();

        #endregion

        /// <summary>
        /// Set the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <param name="p">the parameter set of bifurcation analysis.</param>
        public void SetBifurcationAnalysisParameter(BifurcationAnalysisParameter p)
        {
            m_bifurcateParameter = p;
        }

        /// <summary>
        /// Get the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <returns>the parameter set of bifurcation analysis.</returns>
        public BifurcationAnalysisParameter GetBifurcationAnalysisPrameter()
        {
            if (m_win != null)
                m_bifurcateParameter = m_win.GetBifurcationAnalysisPrameter();

            return m_bifurcateParameter;
        }

        /// <summary>
        /// Set the parameter of parameter estimation.
        /// </summary>
        /// <param name="p">the parameter of parameter estimation.</param>
        public void SetParameterEstimationParameter(ParameterEstimationParameter p)
        {
            m_estimationParameter = p;
        }

        /// <summary>
        /// Get the parameter of parameter estimation.
        /// </summary>
        /// <returns>the parameter of parameter estimation.</returns>
        public ParameterEstimationParameter GetParameterEstimationParameter()
        {
            if (m_win != null)
                m_estimationParameter = m_win.GetParameterEstimationParameter();

            return m_estimationParameter;
        }

        /// <summary>
        /// Set the robust analysis parameter.
        /// </summary>
        /// <param name="p">the parameter of robust analysis.</param>
        public void SetRobustAnalysisParameter(RobustAnalysisParameter p)
        {
            m_robustParameter = p;
        }

        /// <summary>
        /// Get the robust analysis parameter set in this form.
        /// </summary>
        /// <returns>the parameter of robust analysis.</returns>
        public RobustAnalysisParameter GetRobustAnalysisParameter()
        {
            if (m_win != null)
                m_robustParameter = m_win.GetRobustAnalysisParameter();

            return m_robustParameter;
        }

        /// <summary>
        /// Set the parameter of sensitivity analysis to this form.
        /// </summary>
        /// <param name="p">the parameter of sensitivity analysis.</param>
        public void SetSensitivityAnalysisParameter(SensitivityAnalysisParameter p)
        {
            m_sensitivityParameter = p;
        }

        /// <summary>
        /// Get the sensitivity analysis parameter set in this form.
        /// </summary>
        /// <returns>the parameter of sensitivity analysis.</returns>
        public SensitivityAnalysisParameter GetSensitivityAnalysisParameter()
        {
            if (m_win != null)
                m_sensitivityParameter = m_win.GetSensitivityAnalysisParameter();

            return m_sensitivityParameter;
        }

        /// <summary>
        /// Window is null when window is closed.
        /// </summary>
        public void CloseAnalysisWindow()
        {
            m_win = null;
            StopBifurcationAnalysis();
            StopParameterEstimation();
            StopRobustAnalysis();
            StopSensitivityAnalysis();

            m_bifurcateParameter = new BifurcationAnalysisParameter();
            m_estimationParameter = new ParameterEstimationParameter();
            m_robustParameter = new RobustAnalysisParameter();
            m_sensitivityParameter = new SensitivityAnalysisParameter();
        }

        /// <summary>
        /// Close the analysis result window.
        /// </summary>
        public void CloseAnalysisResultWindow()
        {
            m_rWin = null;
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

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="x">the value of parameter.</param>
        /// <param name="y">the value of parameter.</param>
        public void AddJudgementDataForBifurcation(double x, double y)
        {
            if (m_rWin != null)
                m_rWin.AddJudgementDataForBifurcation(x, y);
        }

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="jobid">the jobidof this parameters.</param>
        /// <param name="x">the value of parameter.</param>
        /// <param name="y">the value of parameter.</param>
        /// <param name="isOK">the flag whether this parameter is robustness.</param>
        public void AddJudgementData(int jobid, double x, double y, bool isOK)
        {
            if (m_rWin != null)
                m_rWin.AddJudgementData(jobid, x, y, isOK);
        }

        /// <summary>
        /// Add the judgement data of parameter estimation into graph.
        /// </summary>
        /// <param name="x">the number of generation.</param>
        /// <param name="y">the value of estimation.</param>
        public void AddEstimationData(int x, double y)
        {
            if (m_rWin != null)
                m_rWin.AddEstimationData(x, y);
        }

        /// <summary>
        /// Clear the result of analysis.
        /// </summary>
        public void ClearResult()
        {
            if (m_rWin != null)
                m_rWin.ClearResult();
        }

        /// <summary>
        /// Set the parameter entry to display the result.
        /// </summary>
        /// <param name="name">the parameter name.</param>
        /// <param name="isX">the flag whether this parameter is default parameter at X axis.</param>
        /// <param name="isY">the flag whether this parameter is default parameter at Y axis.</param>
        public void SetResultEntryBox(string name, bool isX, bool isY)
        {
            if (m_rWin != null)
                m_rWin.SetResultEntryBox(name, isX, isY);
        }

        /// <summary>
        /// Set the graph size of result.
        /// </summary>
        /// <param name="xmax">Max value of X axis.</param>
        /// <param name="xmin">Min value of X axis.</param>
        /// <param name="ymax">Max value of Y axis.</param>
        /// <param name="ymin">Min value of Y axis.</param>
        /// <param name="isAutoX">The flag whether X axis is auto scale.</param>
        /// <param name="isAutoY">The flag whether Y axis is auto scale.</param>
        public void SetResultGraphSize(double xmax, double xmin, double ymax, double ymin,
            bool isAutoX, bool isAutoY)
        {
            if (m_rWin != null)
                m_rWin.SetResultGraphSize(xmax, xmin, ymax, ymin, isAutoX, isAutoY);
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

        private void StopAnalysis(object sender, EventArgs e)
        {
            if (m_bifurcationAnalysis != null)
                m_bifurcationAnalysis.StopAnalysis();
            if (m_parameterEstimation != null)
                m_parameterEstimation.StopAnalysis();
            if (m_robustAnalysis != null)
                m_robustAnalysis.StopAnalysis();
            if (m_sensitivityAnalysis != null)
                m_sensitivityAnalysis.StopAnalysis();
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
        public override List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResAnalysis));
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();

            m_showAnalysisSetupItem = new ToolStripMenuItem();
            m_showAnalysisSetupItem.Text = resources.GetString("MenuItemAnalysisWindow");
            m_showAnalysisSetupItem.ToolTipText = resources.GetString("MenuItemAnalysisWindow");
            m_showAnalysisSetupItem.Tag = 50;
            m_showAnalysisSetupItem.Click += new EventHandler(ShowRobustAnalysisWindow);

            ToolStripMenuItem setupMenu = new ToolStripMenuItem();
            setupMenu.DropDownItems.AddRange(new ToolStripItem[] { m_showAnalysisSetupItem });
            setupMenu.Text = "Setup";
            setupMenu.Name = "MenuItemSetup";

            list.Add(setupMenu);

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

            ToolStripMenuItem stopAnalysisItem = new ToolStripMenuItem();
            m_bifurcationAnalysisItem.Text = resources.GetString("MenuItemStopAnalysis");
            m_bifurcationAnalysisItem.ToolTipText = resources.GetString("MenuItemStopAnalysis");
            m_bifurcationAnalysisItem.Tag = 80;
            m_bifurcationAnalysisItem.Enabled = true;
            m_bifurcationAnalysisItem.Click += new EventHandler(StopAnalysis);


            ToolStripMenuItem analysisMenu = new ToolStripMenuItem();
            analysisMenu.DropDownItems.AddRange(new ToolStripItem[] { m_robustAnalysisItem, m_parameterEstimationItem, 
                m_sensitivityAnalysisItem, m_bifurcationAnalysisItem });
            analysisMenu.Text = "Analysis";
            analysisMenu.Name = "MenuItemAnalysis";

            list.Add(analysisMenu);

            return list;
        }

        /// <summary>
        /// Called by PluginManager for getting UseControl.
        /// </summary>
        /// <returns>nothing.</returns>
        public override List<EcellDockContent> GetWindowsForms()
        {
            List<EcellDockContent> list = new List<EcellDockContent>();
            m_rWin = new AnalysisResultWindow();
            list.Add(m_rWin);
            return list;
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            if (ProjectStatus.Loaded == type)
            {
                m_showAnalysisSetupItem.Enabled = true;
                m_parameterEstimationItem.Enabled = true;
                m_sensitivityAnalysisItem.Enabled = true;
                m_bifurcationAnalysisItem.Enabled = true;
            }
            else
            {
                m_showAnalysisSetupItem.Enabled = false;
                m_parameterEstimationItem.Enabled = false;
                m_sensitivityAnalysisItem.Enabled = false;
                m_bifurcationAnalysisItem.Enabled = false;
            }
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            m_paramList.Clear();
            if (m_win != null)
                m_win.Clear();
            ClearResult();
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            foreach (EcellObject obj in data)
            {
                if (obj.Children != null)
                {
                    foreach (EcellObject child in obj.Children)
                    {
                        if (child.Value == null) continue;

                        foreach (EcellData d in child.Value)
                        {
                            if (d.Committed) continue;
                            m_paramList.Add(d.EntityPath, d);
                            if (m_win != null)
                                m_win.AddParameterEntry(child, d);
                        }
                        
                    }
                }
                if (obj.Value == null) continue;

                foreach (EcellData d in obj.Value)
                {
                    if (d.Committed) continue;
                    m_paramList.Add(d.EntityPath, d);
                    if (m_win != null)
                        m_win.AddParameterEntry(obj, d);
                }
            }
        }

        /// <summary>
        ///  The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="obj">Changed value of object.</param>
        public override void DataChanged(string modelID, string key, string type, EcellObject obj)
        {
            if (obj.Value == null) return;
            foreach (EcellData d in obj.Value)
            {
                if (d.Committed)
                {
                    if (m_paramList.ContainsKey(d.EntityPath))
                    {
                        m_paramList.Remove(d.EntityPath);
                        if (m_win != null)
                            m_win.RemoveParameterEntry(d.EntityPath);
                    }
                }
                else
                {
                    if (!m_paramList.ContainsKey(d.EntityPath))
                    {
                        m_paramList.Add(d.EntityPath, d);
                        if (m_win != null)
                            m_win.AddParameterEntry(obj, d);
                    }
                }
            }
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            List<string> delList = new List<string>();
            foreach (string data in m_paramList.Keys)
            {
                int pos = data.IndexOf(':');
                string p = data.Substring(pos + 1);
                if (p.StartsWith(key))
                    delList.Add(data);
            }
            foreach (string data in delList)
            {
                m_paramList.Remove(data);
                if (m_win != null)
                    m_win.RemoveParameterEntry(data);
            }
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public override void SaveModel(string modelID, string directory)
        {
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"PathwayWindow"</returns> 
        public override string GetPluginName()
        {
            return "Analysis";
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
