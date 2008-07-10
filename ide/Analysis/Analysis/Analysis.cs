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

using Ecell.Job;
using WeifenLuo.WinFormsUI.Docking;
using Ecell.Plugin;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.Analysis
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
        /// <summary>
        /// For to display the result of analysis.
        /// </summary>
        private AnalysisResultWindow m_rWin = null;
        /// <summary>
        /// ResourceManager for AnalysisTemplate.
        /// </summary>
        static public ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResources));
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
        /// <summary>
        /// The parameter data for bifurcation analysis.
        /// </summary>
        private BifurcationAnalysisParameter m_bifurcateParameter;
        /// <summary>
        /// The parameter data for parameter estimation.
        /// </summary>
        private ParameterEstimationParameter m_estimationParameter;
        /// <summary>
        /// The parameter data for robust analysis.
        /// </summary>
        private RobustAnalysisParameter m_robustParameter;
        /// <summary>
        /// The parameter data for sensitivity analysis.
        /// </summary>
        private SensitivityAnalysisParameter m_sensitivityParameter;
        /// <summary>
        /// The dictionary of the data to be set by random.
        /// </summary>
        private Dictionary<string, EcellData> m_paramList = new Dictionary<string, EcellData>();
        private List<string> m_headerList = new List<string>();
        private Dictionary<string, List<double>> m_cccResult = new Dictionary<string, List<double>>();
        private Dictionary<string, List<double>> m_fccResult = new Dictionary<string, List<double>>();
        private string m_currentAnalysus = null;
        #endregion

        /// <summary>
        /// get the JobManager.
        /// </summary>
        public IJobManager JobManager
        {
            get { return m_env.JobManager; }
        }

        /// <summary>
        /// constructor.
        /// </summary>
        public Analysis()
        {
            m_win = null;
            m_bifurcateParameter = new BifurcationAnalysisParameter();
            m_estimationParameter = new ParameterEstimationParameter();
            m_robustParameter = new RobustAnalysisParameter();
            m_sensitivityParameter = new SensitivityAnalysisParameter();
        }

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
            m_currentAnalysus = null;
            StopBifurcationAnalysis();
            StopParameterEstimation();
            StopRobustAnalysis();
            StopSensitivityAnalysis();
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
        /// Set the header string of sensitivity matrix.
        /// </summary>
        /// <param name="headerList">the list of activity.</param>
        public void SetSensitivityHeader(List<string> headerList)
        {
            m_headerList.Clear();
            m_cccResult.Clear();
            m_fccResult.Clear();
            foreach (string d in headerList)
                m_headerList.Add(d);
            if (m_rWin != null)
                m_rWin.SetSensitivityHeader(headerList);
        }

        /// <summary>
        /// Create the the row data of analysis result for variable.
        /// </summary>
        /// <param name="name">the property name of parameter.</param>
        /// <param name="result">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfCCC(string name, List<double> result)
        {
            m_cccResult.Add(name, result);
            if (m_rWin != null)
                m_rWin.AddSensitivityDataOfCCC(name, result);
        }

        /// <summary>
        /// Create the row data of analysis result for process
        /// </summary>
        /// <param name="name">the property name of parameter.</param>
        /// <param name="result">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfFCC(string name, List<double> result)
        {
            m_fccResult.Add(name, result);
            if (m_rWin != null)
                m_rWin.AddSensitivityDataOfFCC(name, result);
        }

        /// <summary>
        ///  Update the color of result by using the result value.
        /// </summary>
        public void UpdateResultColor()
        {
            if (m_rWin != null)
                m_rWin.UpdateResultColor();
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

        /// <summary>
        /// Get the list of observed property to judge for analysis.
        /// If there are any problems, this function return null. 
        /// </summary>
        /// <returns>the list of observed property.</returns>
        public List<SaveLoggerProperty> GetPEObservedDataList()
        {
            List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();
            
            String dir = m_env.JobManager.TmpDir;
            double start = 0.0;
            double end = m_estimationParameter.SimulationTime;
            string formulator = m_estimationParameter.EstimationFormulator;
            string[] ele = formulator.Split(new char[] { '+', '-', '*' });
            for (int i = 0; i < ele.Length; i++)
            {
                string element = ele[i].Replace(" ", "");
                if (element.StartsWith("Variable") ||
                    element.StartsWith("Process"))
                    resList.Add(new SaveLoggerProperty(element, start, end, dir));
            }
            return resList;
        }

        /// <summary>
        /// Get the list of observed property to judge for analysis.
        /// If there are any problems, this function return null. 
        /// </summary>
        /// <returns>the list of observed property.</returns>
        public List<SaveLoggerProperty> GetRAObservedDataList()
        {
            List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();
            List<EcellObservedData> obsList = m_env.DataManager.GetObservedData();

            foreach (EcellObservedData data in obsList)
            {
                string dir = m_env.JobManager.TmpDir;
                string path = data.Key;
                double start = 0.0;
                double end = m_robustParameter.SimulationTime;

                SaveLoggerProperty p = new SaveLoggerProperty(path, start, end, dir);
                resList.Add(p);
            }

            if (resList.Count < 1)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoSet,
                    new object[] { MessageResources.NameObservedData }));
                return null;
            }

            return resList;
        }

        /// <summary>
        /// Get the list of observed property to judge for analysis.
        /// If there are any problems, this function return null. 
        /// </summary>
        /// <returns>the list of observed property.</returns>
        public List<SaveLoggerProperty> GetBAObservedDataList()
        {
            List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();
            List<EcellObservedData> obsList = m_env.DataManager.GetObservedData();

            foreach (EcellObservedData data in obsList)
            {
                String dir = m_env.JobManager.TmpDir;
                string path = data.Key;
                double start = 0.0;
                double end = m_bifurcateParameter.SimulationTime;

                SaveLoggerProperty p = new SaveLoggerProperty(path, start, end, dir);
                resList.Add(p);
            }

            if (resList.Count < 1)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoSet,
                    new object[] { MessageResources.NameObservedData }));
                return null;
            }

            return resList;
        }

        /// <summary>
        /// Set the estimated parameter.
        /// </summary>
        /// <param name="param">the execution parameter.</param>
        /// <param name="result">the estimated value.</param>
        /// <param name="generation">the generation.</param>
        public void AddEstimateParameter(ExecuteParameter param, double result, int generation)
        {
            if (m_rWin != null)
            {
                m_rWin.AddEstimateParameter(param, result, generation);
            }
        }

        #region Events
        /// <summary>
        /// Event when this form of robust analysis is shown.
        /// </summary>
        /// <param name="sender">RobustAnalysis.</param>
        /// <param name="e">EventArgs.</param>
        private void ShowAnalysisWindow(object sender, EventArgs e)
        {
            if (m_win == null)
            {
                m_win = new AnalysisWindow(this);
                DockPanel panel = m_env.PluginManager.DockPanel;
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
        }

        /// <summary>
        /// Event when the menu to execute robust analysis is clicked.
        /// This program execute the program of robust analysis.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs.</param>
        private void ExecuteRobustAnalysis(object sender, EventArgs e)
        {
            if (m_robustAnalysis != null && m_robustAnalysis.IsRunning)
            {
                if (Util.ShowYesNoDialog(MessageResources.ConfirmStopAnalysis))


                {
                    m_robustAnalysis.StopAnalysis();
                }
                return;
            }
            m_currentAnalysus = "RobustAnalysis";
            m_robustAnalysis = new RobustAnalysis(this);
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
            if (m_parameterEstimation != null && m_parameterEstimation.IsRunning)
            {
                if (Util.ShowYesNoDialog(MessageResources.ConfirmStopAnalysis))


                {
                    m_parameterEstimation.StopAnalysis();
                }
                return;
            }
            m_currentAnalysus = "ParameterEstimation";
            m_parameterEstimation = new ParameterEstimation(this);
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
            if (m_sensitivityAnalysis != null && m_sensitivityAnalysis.IsRunning)
            {
                if (Util.ShowYesNoDialog(MessageResources.ConfirmStopAnalysis))


                {
                    m_sensitivityAnalysis.StopAnalysis();
                }
                return;
            }
            m_currentAnalysus = "SensitivityAnalysis";
            m_sensitivityAnalysis = new SensitivityAnalysis(this);
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
            if (m_bifurcationAnalysis != null && m_bifurcationAnalysis.IsRunning)
            {
                if (Util.ShowYesNoDialog(MessageResources.ConfirmStopAnalysis))


                {
                    m_bifurcationAnalysis.StopAnalysis();
                }
                return;
            }
            m_currentAnalysus = "BifurcationAnalysis";
            m_bifurcationAnalysis = new BifurcationAnalysis(this);
            m_bifurcationAnalysis.ExecuteAnalysis();
        }

        /// <summary>
        /// Stop the all process of analysis.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
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
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for Analysis plugin.
        /// </summary>
        /// <returns>the list of menu.</returns>
        public override List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResources));
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();

            m_showAnalysisSetupItem = new ToolStripMenuItem();
            m_showAnalysisSetupItem.Text = MessageResources.MenuItemAnalysisWindow;
            m_showAnalysisSetupItem.ToolTipText = MessageResources.MenuItemAnalysisWindow;
            m_showAnalysisSetupItem.Tag = 10;
            m_showAnalysisSetupItem.Click += new EventHandler(ShowAnalysisWindow);

            ToolStripSeparator sep1 = new ToolStripSeparator();
            sep1.Tag = 20;

            m_robustAnalysisItem = new ToolStripMenuItem();
            m_robustAnalysisItem.Text = MessageResources.MenuItemRobustAnalysis;
            m_robustAnalysisItem.ToolTipText = MessageResources.MenuItemRobustAnalysis;
            m_robustAnalysisItem.Tag = 50;
            m_robustAnalysisItem.Enabled = false;
            m_robustAnalysisItem.Click += new EventHandler(ExecuteRobustAnalysis);

            m_parameterEstimationItem = new ToolStripMenuItem();
            m_parameterEstimationItem.Text = MessageResources.MenuItemParameterEstimation;
            m_parameterEstimationItem.ToolTipText = MessageResources.MenuItemParameterEstimation;
            m_parameterEstimationItem.Tag = 60;
            m_parameterEstimationItem.Enabled = false;
            m_parameterEstimationItem.Click += new EventHandler(ExecuteParameterEstimation);

            m_sensitivityAnalysisItem = new ToolStripMenuItem();
            m_sensitivityAnalysisItem.Text = MessageResources.MenuItemSensitivityAnalysis;
            m_sensitivityAnalysisItem.ToolTipText = MessageResources.MenuItemSensitivityAnalysis;
            m_sensitivityAnalysisItem.Tag = 70;
            m_sensitivityAnalysisItem.Enabled = false;
            m_sensitivityAnalysisItem.Click += new EventHandler(ExecuteSensitivityAnalysis);

            m_bifurcationAnalysisItem = new ToolStripMenuItem();
            m_bifurcationAnalysisItem.Text = MessageResources.MenuItemBifurcationAnalysis;
            m_bifurcationAnalysisItem.ToolTipText = MessageResources.MenuItemBifurcationAnalysis;
            m_bifurcationAnalysisItem.Tag = 80;
            m_bifurcationAnalysisItem.Enabled = false;
            m_bifurcationAnalysisItem.Click += new EventHandler(ExecuteBifurcationAnalysis);

            ToolStripMenuItem stopAnalysisItem = new ToolStripMenuItem();
            stopAnalysisItem.Text = MessageResources.MenuItemStopAnalysis;
            stopAnalysisItem.ToolTipText = MessageResources.MenuItemStopAnalysis;
            stopAnalysisItem.Tag = 90;
            stopAnalysisItem.Enabled = true;
            stopAnalysisItem.Click += new EventHandler(StopAnalysis);

            ToolStripSeparator sep2 = new ToolStripSeparator();
            sep2.Tag = 120;

            ToolStripMenuItem saveAnalysisResultItem = new ToolStripMenuItem();
            saveAnalysisResultItem.Text = MessageResources.MenuItemSaveAnalysisResult;
            saveAnalysisResultItem.ToolTipText = MessageResources.MenuItemSaveAnalysisResult;
            saveAnalysisResultItem.Tag = 150;
            saveAnalysisResultItem.Enabled = true;
            saveAnalysisResultItem.Click += new EventHandler(SaveAnalysisResult);

            ToolStripMenuItem loadAnalysisResultItem = new ToolStripMenuItem();
            loadAnalysisResultItem.Text = MessageResources.MenuItemLoadAnalysisResult;
            loadAnalysisResultItem.ToolTipText = MessageResources.MenuItemLoadAnalysisResult;
            loadAnalysisResultItem.Tag = 160;
            loadAnalysisResultItem.Enabled = true;
            loadAnalysisResultItem.Click += new EventHandler(LoadAnalysisResult);

            ToolStripMenuItem analysisMenu = new ToolStripMenuItem();
            analysisMenu.DropDownItems.AddRange(new ToolStripItem[] { 
                m_showAnalysisSetupItem, sep1, m_robustAnalysisItem, m_parameterEstimationItem, 
                m_sensitivityAnalysisItem, m_bifurcationAnalysisItem, stopAnalysisItem, sep2,
                saveAnalysisResultItem, loadAnalysisResultItem
            });
            analysisMenu.Text = "Analysis";
            analysisMenu.Name = "MenuItemAnalysis";

            list.Add(analysisMenu);

            return list;
        }

        private void SaveAnalysisResult(object sender, EventArgs e)
        {
            if (m_currentAnalysus == null) return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = Constants.FileExtCSV;
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            if (m_currentAnalysus.Equals("RobustAnalysis"))
                m_rWin.SaveRobustAnalysisResult(dialog.FileName);
            else if (m_currentAnalysus.Equals("BifurcationAnalysis"))
                m_rWin.SaveBifurcationResult(dialog.FileName);
            else if (m_currentAnalysus.Equals("ParameterEstimation"))
                m_rWin.SaveParameterEstimationResult(dialog.FileName);
            else if (m_currentAnalysus.Equals("SensitivityAnalysis"))
                m_rWin.SaveSensitivityAnalysisResult(dialog.FileName);
        }

        private void LoadAnalysisResult(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = Constants.FileExtCSV;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_rWin.LoadResultFile(dialog.FileName);
            }
        }

        /// <summary>
        /// Called by PluginManager for getting UseControl.
        /// </summary>
        /// <returns>nothing.</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            m_rWin = new AnalysisResultWindow(this);
            return new EcellDockContent[] { m_rWin };
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
                m_robustAnalysisItem.Enabled = true;
                m_parameterEstimationItem.Enabled = true;
                m_sensitivityAnalysisItem.Enabled = true;
                m_bifurcationAnalysisItem.Enabled = true;                
            }
            else
            {
                m_showAnalysisSetupItem.Enabled = false;
                m_robustAnalysisItem.Enabled = false;
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
                            if (d.EntityPath == null) continue;
                            if (!m_env.DataManager.IsContainsParameterData(d.EntityPath)) continue;
                            m_paramList.Add(d.EntityPath, d);
                            if (m_win != null)
                                m_win.AddParameterEntry(child, d);
                        }
                        
                    }
                }
                if (obj.Value == null) continue;

                foreach (EcellData d in obj.Value)
                {
                    if (d.EntityPath == null) continue;
                    if (!m_env.DataManager.IsContainsParameterData(d.EntityPath)) continue;
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
                if (d.EntityPath == null) continue;
                if (!m_env.DataManager.IsContainsParameterData(d.EntityPath))
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
        /// The event sequence when the user add and change the parameter data.
        /// </summary>
        /// <param name="data">The parameter data.</param>
        public override void SetParameterData(EcellParameterData data)
        {
            if (m_win != null)
                m_win.SetParameterData(data);
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed parameter data.</param>
        public override void RemoveParameterData(EcellParameterData data)
        {
            if (m_win != null)
                m_win.RemoveParameterData(data);
        }

        /// <summary>
        /// The event sequence when the user set and change the observed data.
        /// </summary>
        /// <param name="data">The observed data.</param>
        public override void SetObservedData(EcellObservedData data)
        {
            if (m_win != null)
                m_win.SetObservedData(data);
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public override void RemoveObservedData(EcellObservedData data)
        {
            if (m_win != null)
                m_win.RemoveObservedData(data);
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
