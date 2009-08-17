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
using System.IO;

using Ecell.Job;
using WeifenLuo.WinFormsUI.Docking;
using Ecell.Plugin;
using Ecell.Objects;

using Ecell.IDE.Plugins.Analysis.AnalysisFile;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Plugin class to manage the result and parameter of analysis.
    /// </summary>
    public class Analysis : PluginBase, IAnalysis, IRasterizable
    {
        #region Fields
        /// <summary>
        /// For to display the result of analysis.
        /// </summary>
        private AnalysisResultWindow m_rWin = null;
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
        /// The dictionary of the parameter data.
        /// </summary>
        private Dictionary<string, EcellData> m_paramList = new Dictionary<string, EcellData>();
        /// <summary>
        /// The dictionary of the observed data.
        /// </summary>
        private Dictionary<string, EcellData> m_observedList = new Dictionary<string, EcellData>();
        /// <summary>
        /// The list of header of CCC and FCC.
        /// </summary>
        private List<string> m_headerList = new List<string>();
        /// <summary>
        /// The pain to set the parameter of bifurcation analysis.
        /// </summary>
        private BifurcationSettingDialog m_bifurcationDialog;
        /// <summary>
        /// The pain to set the parameter of robust analysis.
        /// </summary>
        private RobustAnalysisSettingDialog m_robustDialog;
        /// <summary>
        /// The pain to set the parameter of sensitivity analysis.
        /// </summary>
        private SensitivityAnalysisSettingDialog m_sensitivityDialog;
        /// <summary>
        /// The pain to set the parameter of parameter estimation.
        /// </summary>
        private ParameterEstimationSettingDialog m_estimationDialog;
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
            InitializeComponent();
        }

        /// <summary>
        /// Initialize Component.
        /// </summary>
        private void InitializeComponent()
        {
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
            return m_sensitivityParameter;
        }

        /// <summary>
        /// Close the analysis result window.
        /// </summary>
        public void CloseAnalysisResultWindow()
        {
            m_rWin = null;
        }

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="list">the values of parameter. [List[PointF]]</param>
        public void AddJudgementDataForBifurcation(List<PointF> list)
        {
            if (m_rWin != null)
            {
                m_rWin.AddJudgementDataForBifurcation(list);
            }
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
            {
                m_rWin.AddJudgementData(jobid, x, y, isOK);
            }
        }

        /// <summary>
        /// Add the judgement data of parameter estimation into graph.
        /// </summary>
        /// <param name="x">the number of generation.</param>
        /// <param name="y">the value of estimation.</param>
        public void AddEstimationData(int x, double y)
        {
            if (m_rWin != null)
            {
                m_rWin.AddEstimationData(x, y);
            }
        }

        /// <summary>
        /// Set the header string of sensitivity matrix.
        /// </summary>
        /// <param name="headerList">the list of activity.</param>
        public void SetSensitivityHeader(List<string> headerList)
        {
            m_headerList.Clear();
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
            if (m_rWin != null)
            {
                m_rWin.AddSensitivityDataOfCCC(name, result);
            }
        }

        /// <summary>
        /// Create the row data of analysis result for process
        /// </summary>
        /// <param name="name">the property name of parameter.</param>
        /// <param name="result">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfFCC(string name, List<double> result)
        {
            if (m_rWin != null)
            {
                m_rWin.AddSensitivityDataOfFCC(name, result);
            }
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
            {                
                m_rWin.SetResultEntryBox(name, isX, isY);
            }
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
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSetNumber,
                    new object[] { MessageResources.NameObservedData, 2 }));
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
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSetNumber,
                    new object[] { MessageResources.NameObservedData, 2 }));
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

        /// <summary>
        /// Notify to change the parameter data to DataManager.
        /// </summary>
        /// <param name="data">the changed parameter data.</param>
        public void NotifyParameterDataChanged(EcellParameterData data)
        {
            DataManager.SetParameterData(data);
        }

        /// <summary>
        /// Notify to change the observed data to DataManager.
        /// </summary>
        /// <param name="data">the changed observed data.</param>
        public void NotifyObservedDataChanged(EcellObservedData data)
        {
            DataManager.SetObservedData(data);
        }

        /// <summary>
        /// This program execute the program of sensitivity analysis.
        /// </summary>
        public void ExecuteSensitivityAnalysis()
        {
            if (m_env.PluginManager.Status == ProjectStatus.Uninitialized)
                return;
            m_sensitivityParameter = m_sensitivityDialog.GetParameter();
            ShowGridStatusDialog();
            string modelName = m_env.DataManager.CurrentProject.Model.ModelID;
            List<EcellObject> sysObj = m_env.DataManager.CurrentProject.SystemDic[modelName];
            List<EcellObject> stepperObj = m_env.DataManager.CurrentProject.StepperDic[modelName];
            JobGroup g = m_env.JobManager.CreateJobGroup(SensitivityAnalysis.s_analysisName, sysObj, stepperObj);
            SensitivityAnalysis sensitivityAnalysis = new SensitivityAnalysis(this);
            sensitivityAnalysis.Group = g;
            sensitivityAnalysis.AnalysisParameter = m_sensitivityParameter;
            sensitivityAnalysis.ExecuteAnalysis();
        }

        /// <summary>
        /// This program execute the program of bifurcation analysis.
        /// </summary>
        public void ExecuteBifurcationAnalysis()
        {
            if (m_env.PluginManager.Status == ProjectStatus.Uninitialized)
                return;
            m_bifurcateParameter = m_bifurcationDialog.GetParameter();
            ShowGridStatusDialog();
            string modelName = m_env.DataManager.CurrentProject.Model.ModelID;
            List<EcellObject> sysObj = m_env.DataManager.CurrentProject.SystemDic[modelName];
            List<EcellObject> stepperObj = m_env.DataManager.CurrentProject.StepperDic[modelName];
            JobGroup g = m_env.JobManager.CreateJobGroup(BifurcationAnalysis.s_analysisName, sysObj, stepperObj);
            BifurcationAnalysis bifurcationAnalysis = new BifurcationAnalysis(this);
            bifurcationAnalysis.Group = g;
            bifurcationAnalysis.AnalysisParameter = m_bifurcateParameter;
            bifurcationAnalysis.ExecuteAnalysis();
        }

        /// <summary>
        /// This program execute the program of robust analysis.
        /// </summary>
        public void ExecuteRobustAnalysis()
        {
            if (m_env.PluginManager.Status == ProjectStatus.Uninitialized)
                return;
            m_robustParameter = m_robustDialog.GetParameter();
            ShowGridStatusDialog();
            string modelName = m_env.DataManager.CurrentProject.Model.ModelID;
            List<EcellObject> sysObj = m_env.DataManager.CurrentProject.SystemDic[modelName];
            List<EcellObject> stepperObj = m_env.DataManager.CurrentProject.StepperDic[modelName];
            JobGroup g = m_env.JobManager.CreateJobGroup(RobustAnalysis.s_analysisName, sysObj, stepperObj);
            RobustAnalysis robustAnalysis = new RobustAnalysis(this);
            robustAnalysis.Group = g;
            robustAnalysis.AnalysisParameter = m_robustParameter;
            robustAnalysis.ExecuteAnalysis();
        }



        /// <summary>
        /// This program execute the program of parameter estimation.
        /// </summary>
        public void ExecuteParameterEstimation()
        {
            if (m_env.PluginManager.Status == ProjectStatus.Uninitialized)
                return;
            m_estimationParameter = m_estimationDialog.GetParameter();
            ShowGridStatusDialog();
            string modelName = m_env.DataManager.CurrentProject.Model.ModelID;
            List<EcellObject> sysObj = m_env.DataManager.CurrentProject.SystemDic[modelName];
            List<EcellObject> stepperObj = m_env.DataManager.CurrentProject.StepperDic[modelName];
            JobGroup g = m_env.JobManager.CreateJobGroup(ParameterEstimation.s_analysisName, sysObj, stepperObj);
            ParameterEstimation parameterEstimation = new ParameterEstimation(this);
            parameterEstimation.Group = g;
            parameterEstimation.AnalysisParameter = m_estimationParameter;
            parameterEstimation.ExecuteAnalysis();
        }

        /// <summary>
        /// Show the dialog of grid status.
        /// </summary>
        private void ShowGridStatusDialog()
        {
            ShowDialogDelegate dlg = m_env.PluginManager.GetDelegate(Constants.delegateShowGridDialog) as ShowDialogDelegate;
            if (dlg != null)
                dlg();
        }

        /// <summary>
        /// Activate the result window.
        /// </summary>
        /// <param name="groupName">the group name.</param>
        /// <param name="isGraphWindow">the flag whether graph window is visible.</param>
        /// <param name="isParameterWindow">the flag whether sensitivity window is visible.</param>
        /// <param name="isSensitivityWindow">the flag whether parameter window is visible.</param>
        public void ActivateResultWindow(string groupName, bool isGraphWindow, bool isSensitivityWindow, bool isParameterWindow)
        {
            if (isGraphWindow)
            {
                m_rWin.GraphWindow.GroupName = groupName;
                m_rWin.GraphContent.Activate();
            }
            if (isSensitivityWindow)
            {
                m_rWin.SensitivityWindow.GroupName = groupName;
                m_rWin.SensitivityAnalysisContent.Activate();                
            }
            if (isParameterWindow)
            {
                m_rWin.ParameterEstimationWindow.GroupName = groupName;
                m_rWin.ParameterEsitmationContent.Activate();
            }
        }        


        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for Analysis plugin.
        /// </summary>
        /// <returns>the list of menu.</returns>
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();

            return list;
        }

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            m_env.JobManager.AnalysisDic[ParameterEstimation.s_analysisName] = new ParameterEstimation(this);
            m_env.JobManager.AnalysisDic[BifurcationAnalysis.s_analysisName] = new BifurcationAnalysis(this);
            m_env.JobManager.AnalysisDic[SensitivityAnalysis.s_analysisName] = new SensitivityAnalysis(this);
            m_env.JobManager.AnalysisDic[RobustAnalysis.s_analysisName] = new RobustAnalysis(this);

            m_bifurcateParameter = new BifurcationAnalysisParameter();
            m_estimationParameter = new ParameterEstimationParameter();
            m_sensitivityParameter = new SensitivityAnalysisParameter();
            m_robustParameter = new RobustAnalysisParameter();

            m_rWin = new AnalysisResultWindow(this);
            m_bifurcationDialog = new BifurcationSettingDialog(this);            
            m_bifurcationDialog.SetParameter(m_bifurcateParameter);
            m_bifurcationDialog.ContentType = DockContentType.ANALYSIS;

            m_robustDialog = new RobustAnalysisSettingDialog(this);
            m_robustDialog.SetParameter(m_robustParameter);
            m_robustDialog.ContentType = DockContentType.ANALYSIS;

            m_sensitivityDialog = new SensitivityAnalysisSettingDialog(this);
            m_sensitivityDialog.SetParameter(m_sensitivityParameter);
            m_sensitivityDialog.ContentType = DockContentType.ANALYSIS;

            m_estimationDialog = new ParameterEstimationSettingDialog(this);
            m_estimationDialog.SetParameter(m_estimationParameter);
            m_estimationDialog.ContentType = DockContentType.ANALYSIS;
        }

        /// <summary>
        /// Called by PluginManager for getting UseControl.
        /// </summary>
        /// <returns>nothing.</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            List<EcellDockContent> result = new List<EcellDockContent>();
            IEnumerable<EcellDockContent> list = m_rWin.GetWindowsForms();

            foreach (EcellDockContent c in list)
            {
                result.Add(c);
            }
            result.Add(m_bifurcationDialog);
            result.Add(m_robustDialog);
            result.Add(m_sensitivityDialog);
            result.Add(m_estimationDialog);

            return result;
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            m_estimationDialog.ChangeStatus(type);
            m_robustDialog.ChangeStatus(type);
            m_sensitivityDialog.ChangeStatus(type);
            m_bifurcationDialog.ChangeStatus(type);
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            m_paramList.Clear();
            m_observedList.Clear();
            ClearResult();
            m_estimationDialog.Clear();
            m_bifurcationDialog.Clear();
            m_robustDialog.Clear();
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public override void DataAdd(List<EcellObject> data)
        {           
            foreach (EcellObject obj in data)
            {
                if (obj.Type == Constants.xpathProject)
                {                    
                    continue;
                }
                if (obj.Children != null)
                {
                    foreach (EcellObject child in obj.Children)
                    {
                        if (child.Value == null)
                            continue;
                        foreach (EcellData d in child.Value)
                        {
                            if (d.EntityPath == null)
                                continue;
                            if (m_env.DataManager.IsContainsParameterData(d.EntityPath))
                            {
                                m_paramList[d.EntityPath] = d;
                            }
                            if (m_env.DataManager.IsContainsObservedData(d.EntityPath))
                            {
                                m_observedList[d.EntityPath] = d;
                            }
                        }
                        
                    }
                }
                if (obj.Value == null)
                    continue;
                foreach (EcellData d in obj.Value)
                {
                    if (d.EntityPath == null)
                        continue;
                    if (m_env.DataManager.IsContainsParameterData(d.EntityPath))
                    {
                        m_paramList[d.EntityPath] = d;
                    }
                    if (m_env.DataManager.IsContainsObservedData(d.EntityPath))
                    {
                        m_observedList[d.EntityPath] = d;
                    }
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
                    }
                }
                else
                {
                    if (!m_paramList.ContainsKey(d.EntityPath))
                    {
                        m_paramList.Add(d.EntityPath, d);
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
            }
        }        

        /// <summary>
        /// The event sequence when the user add and change the parameter data.
        /// </summary>
        /// <param name="data">The parameter data.</param>
        public void SetParameterData(EcellParameterData data)
        {
            if (!m_paramList.ContainsKey(data.Key))
                m_paramList.Add(data.Key, null);
            m_bifurcationDialog.SetParameterData(data);
            m_robustDialog.SetParameterData(data);
            m_estimationDialog.SetParameterData(data);
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed parameter data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            m_paramList.Remove(data.Key);
            m_bifurcationDialog.RemoveParameterData(data);
            m_robustDialog.RemoveParameterData(data);
            m_estimationDialog.RemoveParameterData(data);
        }

        /// <summary>
        /// The event sequence when the user set and change the observed data.
        /// </summary>
        /// <param name="data">The observed data.</param>
        public void SetObservedData(EcellObservedData data)
        {
            if (!m_observedList.ContainsKey(data.Key))
                m_observedList.Add(data.Key, null);
            m_bifurcationDialog.SetObservedData(data);
            m_robustDialog.SetObservedData(data);
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public void RemoveObservedData(EcellObservedData data)
        {
            m_observedList.Remove(data.Key);
            m_bifurcationDialog.RemoveObservedData(data);
            m_robustDialog.RemoveObservedData(data);
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public override void SaveModel(string modelID, string directory)
        {
            // test code
            //string path = Path.Combine(directory, "bifurcation.param");
            //BifurcationAnlaysisParameterFile f = new BifurcationAnlaysisParameterFile(m_env, path, modelID);
            //f.Parameter = m_bifurcateParameter;
            //f.Write();
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
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public IEnumerable<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            if (!m_rWin.GraphContent.IsHidden)
                names.Add(m_rWin.GraphContent.Text);
            return names;
        }

        /// <summary>
        /// get whether this plugin is enbale to print directly.
        /// </summary>
        /// <returns></returns>
        public bool IsDirect()
        {
            return true;
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print(string name)
        {
            if (m_rWin.GraphContent.Text.Equals(name))
                m_rWin.GraphWindow.Print();
            return null;
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
