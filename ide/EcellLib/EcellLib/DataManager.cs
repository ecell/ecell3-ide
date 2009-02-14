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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using IronPython.Hosting;
using IronPython.Runtime;

using EcellCoreLib;
using Ecell.Objects;
using Ecell.Logging;
using Ecell.Exceptions;

namespace Ecell
{
    /// <summary>
    /// Manages data of projects, models, and so on.
    /// </summary>
    public class DataManager
    {
        #region Fields
        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        private ApplicationEnvironment m_env;
        /// <summary>
        /// The current project
        /// </summary>
        private Project m_currentProject = null;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isStepStepping = false;
        /// <summary>
        /// ObservedDatas
        /// </summary>
        private Dictionary<string, EcellObservedData> m_observedList;
        /// <summary>
        /// ParameterDatas
        /// </summary>
        private Dictionary<string, EcellParameterData> m_parameterList;
        /// <summary>
        /// Logger list.
        /// </summary>
        private List<string> m_loggerEntry = new List<string>();

        /// <summary>
        /// The default directory
        /// </summary>
        private string m_defaultDir = null;

        /// <summary>
        /// The default count of the step 
        /// </summary>
        private int m_defaultStepCount = 10;
        /// <summary>
        /// The default time 
        /// </summary>
        private double m_defaultTime = 10.0;
        /// <summary>
        /// 
        /// </summary>
        private int m_remainStep = 0;
        /// <summary>
        /// 
        /// </summary>
        private double m_remainTime = 0.0;
        /// <summary>
        /// The time limit of the simulation
        /// </summary>
        private double m_simulationTimeLimit = -1.0;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isTimeStepping = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates the new "DataManager" instance with no argument.
        /// </summary>
        public DataManager(ApplicationEnvironment env)
        {
            this.m_env = env;
            this.m_observedList = new Dictionary<string, EcellObservedData>();
            this.m_parameterList = new Dictionary<string, EcellParameterData>();
            SetDefaultDir();
        }

        #endregion

        #region Accessors
        /// <summary>
        /// get / set StepCount
        /// </summary>
        public int StepCount
        {
            get { return this.m_defaultStepCount; }
            set { this.m_defaultStepCount = value; }
        }

        /// <summary>
        /// get CurrentProjectID
        /// </summary>
        public string CurrentProjectID
        {
            get
            {
                if (m_currentProject == null)
                    return null;
                return m_currentProject.Info.Name;
            }
        }

        /// <summary>
        /// get CurrentProject
        /// </summary>
        public Project CurrentProject
        {
            get
            {
                return m_currentProject;
            }
        }

        /// <summary>
        /// get the default directory.
        /// </summary>
        public String DefaultDir
        {
            get { return this.m_defaultDir; }
        }

        /// <summary>
        /// get / set whether simulation time have limit.
        /// </summary>
        public double SimulationTimeLimit
        {
            get { return this.m_simulationTimeLimit; }
            set { this.m_simulationTimeLimit = value; }
        }

        /// <summary>
        /// Associated Enviroment
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
        }

        /// <summary>
        /// Checks whether the simulator is running.
        /// </summary>
        /// <returns>true if the simulator is running; false otherwise</returns>
        public bool IsActive
        {
            get
            {
                bool runningFlag = (m_currentProject != null && m_currentProject.SimulationStatus == SimulationStatus.Run);
                return runningFlag;
            }
        }
        #endregion

        #region Method for File I/O.
        /// <summary>
        /// Save the user action to the set file.
        /// </summary>
        /// <param name="fileName">saved file name.</param>
        public void SaveUserAction(string fileName)
        {
            try
            {
                m_env.ActionManager.SaveActionFile(fileName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new EcellException(String.Format(MessageResources.ErrSaveAct, fileName), ex);
            }
        }

        /// <summary>
        /// Load the user action from the set file.
        /// </summary>
        /// <param name="filenName">saved file name.</param>
        public void LoadUserActionFile(string filenName)
        {
            try
            {
                CloseProject();
                m_env.ActionManager.LoadActionFile(filenName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new EcellException(String.Format(MessageResources.ErrLoadFile,
                    new object[] { filenName }), ex);
            }
        }

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveScript(string fileName)
        {
            try
            {
                ScriptWriter writer = new ScriptWriter(m_currentProject);
                writer.SaveScript(fileName);
                m_env.Console.WriteLine(String.Format(MessageResources.InfoSaveScript, fileName));
                m_env.Console.Flush();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new EcellException(String.Format(MessageResources.ErrSaveScript, fileName), ex);
            }
        }

        /// <summary>
        /// Compile the dm source file.
        /// </summary>
        /// <param name="fileName">the source file name.</param>
        public void ExecuteScript(string fileName)
        {
            PythonEngine engine = new PythonEngine();

            engine.AddToPath(Directory.GetCurrentDirectory());
            engine.AddToPath(Util.GetAnalysisDir());
            string scriptFile = fileName;

            try
            {
                MemoryStream standardOutput = new MemoryStream();
                engine.SetStandardOutput(standardOutput);
                engine.Execute("from EcellIDE import *");
                engine.Execute("import time");
                engine.Execute("import System.Threading");
                engine.Execute("session=Session()");
                engine.ExecuteFile(scriptFile);
                string stdOut = ASCIIEncoding.ASCII.GetString(standardOutput.ToArray());

                m_env.Console.WriteLine(stdOut);
                m_env.Console.WriteLine(String.Format(MessageResources.InfoExecScript, fileName));
                m_env.Console.Flush();
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLoadFile,
                    new object[] { scriptFile }));
            }
        }

        #endregion

        #region Method for Load Project.
        /// <summary>
        /// Load project from project file.
        /// </summary>
        /// <param name="filename">Project file or EML file.</param>
        public void LoadProject(string filename)
        {
            try
            {
                ProjectInfo info = ProjectInfoLoader.Load(filename);
                LoadProject(info);
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrLoadPrj,
                    new object[] { filename }), ex);
            }
        }

        /// <summary>
        /// LoadProject
        /// </summary>
        /// <param name="info"></param>
        public void LoadProject(ProjectInfo info)
        {
            List<EcellObject> passList = new List<EcellObject>();
            string[] parameters = new string[0];
            string message = null;
            string projectID = null;
            Project project = null;

            try
            {
                if (info == null)
                    throw new EcellException(MessageResources.ErrLoadPrj);
                if (m_currentProject != null)
                    CloseProject();


                // Initializes.
                projectID = info.Name;
                message = "[" + projectID + "]";
                project = new Project(info);

                // If this project is not model.
                if (project.Info.ProjectType != ProjectType.Model)
                    project.Info.FindModels();
                // If this project is Template.
                if (project.Info.ProjectType == ProjectType.Template)
                    project.CopyDMDirs(info.DMDirList);

                // Set current project.
                m_currentProject = project;
                m_env.PluginManager.ParameterSet(projectID, project.Info.SimulationParam);

                // Create EcellProject.
                List<EcellData> ecellDataList = new List<EcellData>();
                ecellDataList.Add(new EcellData(Constants.textComment, new EcellValue(project.Info.Comment), null));
                passList.Add(EcellObject.CreateObject(projectID, "", Constants.xpathProject, "", ecellDataList));

                // Loads the model.
                m_env.PluginManager.ChangeStatus(ProjectStatus.Loading);
                LoadModel(project.Info.Models);

                // Prepare datas.
                foreach (EcellObject model in m_currentProject.ModelList)
                    passList.Add(model);
                foreach (string storedModelID in m_currentProject.SystemDic.Keys)
                {
                    passList.AddRange(m_currentProject.SystemDic[storedModelID]);
                }

                // Loads the simulation parameter.
                string simulationDirName = null;
                if (project.Info.ProjectPath != null)
                    simulationDirName = Path.Combine(project.Info.ProjectPath, Constants.xpathParameters);
                if (Directory.Exists(simulationDirName))
                {
                    parameters = Directory.GetFileSystemEntries(
                        simulationDirName,
                        Constants.delimiterWildcard + Constants.FileExtXML);
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (string parameter in parameters)
                        {
                            string fileName = Path.GetFileName(parameter);
                            if (fileName.IndexOf(Constants.delimiterUnderbar) != 0)
                            {
                                SimulationParameter simParam = LoadSimulationParameter(parameter);
                                SetSimulationParameter(simParam);
                            }
                        }
                    }
                }
                m_env.Console.WriteLine(string.Format(MessageResources.InfoLoadPrj, projectID));
                m_env.Console.Flush();
                Trace.WriteLine(string.Format(MessageResources.InfoLoadPrj, projectID));
            }
            catch (Exception ex)
            {
                passList = null;
                CloseProject();
                throw new EcellException(String.Format(MessageResources.ErrLoadPrj, projectID), ex);
            }
            finally
            {
                if (m_currentProject != null)
                {
                    if (passList != null && passList.Count > 0)
                    {
                        this.m_env.PluginManager.DataAdd(passList);
                    }
                    foreach (string paramID in this.GetSimulationParameterIDs())
                    {
                        this.m_env.PluginManager.ParameterAdd(projectID, paramID);
                    }

                    m_env.ActionManager.AddAction(new LoadProjectAction(projectID, project.Info.ProjectFile));
                    m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
                }
            }
        }

        /// <summary>
        /// Loads the eml formatted file and returns the model ID.
        /// </summary>
        /// <param name="files">The eml formatted file name</param>
        /// <returns>The model ID</returns>
        private void LoadModel(List<string> files)
        {
            string modelID = null;
            try
            {
                // To load
                if (m_currentProject.Simulator == null)
                {
                    m_currentProject.SetDMList();
                    m_currentProject.Simulator = m_currentProject.CreateSimulatorInstance();
                }
                foreach (string filename in files)
                {
                    // Load model
                    EcellObject modelObj = null;
                    try
                    {
                        modelObj = EmlReader.Parse(filename, m_currentProject.Simulator);
                    }
                    catch (EcellException e)
                    {
                        throw new EcellException(string.Format(MessageResources.ErrLoadModel, filename), e);
                    }
                    catch (Exception e)
                    {
                        string msg = string.Format(MessageResources.ErrLoadModel, filename) + "\n" + e.Message;
                        Util.ShowErrorDialog(msg);
                        continue;
                    }

                    // If file is not Eml, return.
                    if (modelObj.Children == null || modelObj.Children.Count <= 0)
                        continue;
                    // If this project is template.
                    if (m_currentProject.Info.ProjectType == ProjectType.Template)
                        modelObj.ModelID = m_currentProject.Info.Name;
                    modelID = modelObj.ModelID;

                    // Initialize
                    try
                    {
                        m_currentProject.Simulator.Initialize();
                    }
                    catch (Exception e)
                    {
                        // Error Message
                        // [VariableReference [S0] not found in this Process]
                        // MichaelisUniUniFluxprocess
                        // DecayFluxProcess
                        // [Only first or second order scheme is allowed]
                        // PingPongBiBiFluxProcess
                        // TauLeapProcess
                        Util.ShowWarningDialog(MessageResources.WarnInvalidData + "\n" + e.Message);
                    }

                    // Sets initial conditions.
                    m_currentProject.Initialize(modelObj.ModelID);
                    InitializeModel(modelObj);

                    Trace.WriteLine(String.Format(MessageResources.InfoLoadModel, modelID));
                    m_env.Console.WriteLine(String.Format(MessageResources.InfoLoadModel, modelID));
                    m_env.Console.Flush();
                }

                // If this project has no model.
                if (m_currentProject.ModelList.Count <= 0)
                    throw new EcellException(string.Format(MessageResources.ErrNoSet, "Model"));

                // Stores the "LoggerPolicy"
                string simParam = m_currentProject.Info.SimulationParam;
                if (!m_currentProject.LoggerPolicyDic.ContainsKey(simParam))
                {
                    m_currentProject.LoggerPolicyDic[simParam] = new LoggerPolicy();
                }
            }
            catch (Exception ex)
            {
                throw new EcellException(string.Format(MessageResources.ErrLoadModel, files.ToString()), ex);
            }
        }

        /// <summary>
        /// InitializeModel
        /// </summary>
        /// <param name="ecellObject"></param>
        private void InitializeModel(EcellObject ecellObject)
        {
            // Sets the "EcellObject".
            string modelID = ecellObject.ModelID;
            string simParam = m_currentProject.Info.SimulationParam;
            if (ecellObject.Type.Equals(Constants.xpathModel))
            {
                m_currentProject.ModelList.Add((EcellModel)ecellObject);
                DataStorer.DataStored(
                    m_currentProject.Simulator,
                    m_env,
                    ecellObject,
                    m_currentProject.InitialCondition[simParam][modelID]);
            }
            else if (ecellObject.Type.Equals(Constants.xpathSystem))
            {
                if (!m_currentProject.SystemDic.ContainsKey(modelID))
                {
                    m_currentProject.SystemDic[modelID]
                            = new List<EcellObject>();
                }
                m_currentProject.SystemDic[modelID].Add(ecellObject);
            }
            else if (ecellObject.Type.Equals(Constants.xpathStepper))
            {
                if (!m_currentProject.StepperDic.ContainsKey(simParam))
                {
                    m_currentProject.StepperDic[simParam] = new Dictionary<string, List<EcellObject>>();
                }
                if (!m_currentProject.StepperDic[simParam].ContainsKey(modelID))
                {
                    m_currentProject.StepperDic[simParam][modelID] = new List<EcellObject>();
                }
                m_currentProject.StepperDic[simParam][modelID].Add(ecellObject);
            }
            foreach (EcellObject childEcellObject in ecellObject.Children)
            {
                InitializeModel(childEcellObject);
            }
        }
        #endregion

        #region Method to manage project.
        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="comment"></param>
        /// <param name="modelID"></param>
        public void CreateNewProject(string projectID, string comment, string modelID)
        {
            CreateNewProject(projectID, comment, modelID, new List<string>());
        }

        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="comment"></param>
        /// <param name="modelID"></param>
        /// <param name="setDirList"></param>
        public void CreateNewProject(string projectID, string comment, string modelID, List<string> setDirList)
        {
            try
            {
                CreateProject(projectID, comment, modelID);
                m_currentProject.CopyDMDirs(setDirList);

                EcellObject model = EcellObject.CreateObject(modelID, "", Constants.xpathModel, "", new List<EcellData>());
                DataAdd(model, false, false);
                foreach (string paramID in GetSimulationParameterIDs())
                {
                    m_env.PluginManager.ParameterAdd(projectID, paramID);
                }
                m_env.PluginManager.ParameterSet(CurrentProjectID, GetCurrentSimulationParameterID());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(string.Format(MessageResources.ErrCrePrj, modelID) + "\n" + ex.Message);
                CloseProject();
            }

        }

        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="projectID">The "Project" ID</param>
        /// <param name="comment">The comment</param>
        /// <param name="projectPath">The project directory path to load the dm of this project.</param>
        private void CreateProject(string projectID, string comment, string projectPath)
        {
            Project prj = null;
            try
            {
                //
                // Closes the current project.
                //
                if (m_currentProject != null)
                {
                    this.CloseProject();
                }
                //
                // Initialize
                //
                ProjectInfo info = new ProjectInfo(projectID, comment, DateTime.Now.ToString(), Constants.defaultSimParam);
                prj = new Project(info);
                m_currentProject = prj;
                if (projectPath != null)
                    m_currentProject.Info.ProjectPath = projectPath;

                //
                // 4 PluginManager
                //
                List<EcellData> ecellDataList = new List<EcellData>();
                ecellDataList.Add(new EcellData(Constants.textComment, new EcellValue(comment), null));
                EcellObject ecellObject
                        = EcellObject.CreateObject(projectID, "", Constants.xpathProject, "", ecellDataList);
                List<EcellObject> ecellObjectList = new List<EcellObject>();
                ecellObjectList.Add(ecellObject);
                m_env.PluginManager.DataAdd(ecellObjectList);
                m_env.ActionManager.AddAction(new NewProjectAction(projectID, comment, projectPath));
                m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);

                m_env.Console.WriteLine(string.Format(MessageResources.InfoCrePrj, projectID));
                m_env.Console.Flush();
                Trace.WriteLine(string.Format(MessageResources.InfoCrePrj, projectID));
            }
            catch (Exception ex)
            {
                m_currentProject = null;
                string message = string.Format(
                        MessageResources.ErrCrePrj,
                        projectID);
                Trace.WriteLine(message);
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Create new revision of current project.
        /// </summary>
        public void CreateNewRevision()
        {
            if (m_currentProject == null)
                return;
            SaveProject();

            string sourceDir = m_currentProject.Info.ProjectPath;
            string revNo = Util.GetRevNo(sourceDir);
            string targetDir = Path.Combine(sourceDir, revNo);
            foreach (string dir in Util.IgnoredDirList)
            {
                string tempdir = Path.Combine(sourceDir, dir);
                if (Directory.Exists(tempdir))
                    Util.CopyDirectory(tempdir, Path.Combine(targetDir, dir), false);
            }
            string[] files = Directory.GetFiles(sourceDir, "project.*");
            foreach (string file in files)
                Util.CopyFile(file, targetDir);
            m_env.Console.WriteLine(String.Format(MessageResources.InfoNewRev,
                m_currentProject.Info.Name, revNo));
        }

        /// <summary>
        /// Saves the model using the model ID.
        /// </summary>
        /// <param name="modelID">The saved model ID</param>
        internal void SaveModel(string modelID)
        {
            List<EcellObject> storedList = new List<EcellObject>();
            string message = null;
            try
            {
                message = String.Format(MessageResources.InfoSaveModel,
                    new object[] { modelID });
                //
                // Initializes
                //
                Debug.Assert(!String.IsNullOrEmpty(modelID));
                SetDefaultDir();

                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + m_currentProject.Info.Name))
                {
                    m_currentProject.Save();
                }
                // Set Model dir and filename.
                string modelDirName = Path.Combine(m_currentProject.Info.ProjectPath, Constants.xpathModel);
                if (!Directory.Exists(modelDirName))
                    Directory.CreateDirectory(modelDirName);
                string modelFileName
                    = modelDirName + Constants.delimiterPath + modelID + Constants.delimiterPeriod + Constants.xpathEml;

                // Picks the "Stepper" up.
                List<EcellObject> stepperList
                    = m_currentProject.StepperDic[m_currentProject.Info.SimulationParam][modelID];
                Debug.Assert(stepperList != null && stepperList.Count > 0);
                storedList.AddRange(stepperList);

                // Picks the "System" up.
                List<EcellObject> systemList = m_currentProject.SystemDic[modelID];
                Debug.Assert(systemList != null && systemList.Count > 0);
                storedList.AddRange(systemList);

                // Save eml.
                EmlWriter.Create(modelFileName, storedList, true);
                // Save Leml.
                EcellModel model = (EcellModel)m_currentProject.Model;
                model.Children = storedList;
                string leml = modelFileName.Replace(Constants.FileExtEML, Constants.FileExtLEML);
                Leml.SaveLEML(model, leml);

                Trace.WriteLine("Save Model: " + message);
                m_env.PluginManager.SaveModel(modelID, modelDirName);
            }
            catch (Exception ex)
            {
                storedList = null;
                message = string.Format(MessageResources.ErrSaveModel, modelID);
                Trace.WriteLine(message);
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Save current project.
        /// </summary>
        public void SaveProject()
        {
            SetDefaultDir();
            try
            {
                m_currentProject.Save();
                List<string> modelList = m_currentProject.GetSavableModel();
                List<string> paramList = m_currentProject.GetSavableSimulationParameter();
                List<string> logList = m_currentProject.GetSavableSimulationResult();

                foreach (EcellObject model in m_currentProject.ModelList)
                {
                    SaveModel(model.ModelID);
                }
                foreach (string name in paramList)
                {
                    SaveSimulationParameter(name);
                }

                SaveSimulationResult();
                SaveSimulationResultDelegate dlg = m_env.PluginManager.GetDelegate("SaveSimulationResult") as SaveSimulationResultDelegate;
                if (dlg != null)
                    dlg(logList);

                m_env.Console.WriteLine(String.Format(MessageResources.InfoSavePrj, m_currentProject.Info.Name));
                m_env.Console.Flush();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new EcellException(string.Format(MessageResources.ErrSavePrj, m_currentProject.Info.Name), ex);
            }
        }

        /// <summary>
        /// Sets the directory name to "m_defaultDir"
        /// </summary>
        private void SetDefaultDir()
        {
            this.m_defaultDir = Util.GetBaseDir();
        }

        /// <summary>
        /// Exports the models to ths designated file.
        /// </summary>
        /// <param name="modelIDList">The list of the model ID</param>
        /// <param name="fileName">The designated file</param>
        public void ExportModel(List<string> modelIDList, string fileName)
        {
            string message = null;
            try
            {
                message = "[" + fileName + "]";
                //
                // Initializes.
                //
                if (modelIDList == null || modelIDList.Count <= 0)
                {
                    return;
                }
                else if (fileName == null || fileName.Length <= 0)
                {
                    return;
                }
                //
                // Checks the parent directory.
                //
                string parentPath = Path.GetDirectoryName(fileName);
                if (parentPath != null && parentPath.Length > 0 && !Directory.Exists(parentPath))
                {
                    Directory.CreateDirectory(parentPath);
                }
                //
                // Searchs the "Stepper" & the "System".
                //
                List<EcellObject> storedStepperList = new List<EcellObject>();
                List<EcellObject> storedSystemList = new List<EcellObject>();

                Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
                Dictionary<string, List<EcellObject>> stepperDic = m_currentProject.StepperDic[m_currentProject.Info.SimulationParam];

                foreach (string modelID in modelIDList)
                {
                    storedStepperList.AddRange(stepperDic[modelID]);
                    storedSystemList.AddRange(sysDic[modelID]);
                }
                Debug.Assert(storedStepperList != null && storedStepperList.Count > 0);
                Debug.Assert(storedSystemList != null && storedSystemList.Count > 0);

                //
                // Exports.
                //
                storedStepperList.AddRange(storedSystemList);
                EmlWriter.Create(fileName, storedStepperList, false);
                Trace.WriteLine("Export Model: " + message);
            }
            catch (Exception ex)
            {
                throw new EcellException(string.Format(MessageResources.ErrCreFile, fileName), ex);
            }
        }

        /// <summary>
        /// Closes project without confirming save or no save.
        /// </summary>
        public void CloseProject()
        {
            try
            {
                if (this.m_currentProject != null)
                {
                    string msg = string.Format(MessageResources.InfoClose, m_currentProject.Info.Name);
                    Trace.WriteLine(msg);
                    m_env.Console.WriteLine(msg);
                    m_env.Console.Flush();
                    this.m_currentProject.Close();
                    this.m_currentProject.Simulator.Dispose();
                    this.m_currentProject = null;
                }

                this.m_env.PluginManager.AdvancedTime(0);
                this.m_env.PluginManager.Clear();
                this.m_env.ActionManager.Clear();
                this.m_env.ReportManager.Clear();
                this.m_parameterList.Clear();
                this.m_observedList.Clear();
                this.m_loggerEntry.Clear();

                m_env.PluginManager.ChangeStatus(ProjectStatus.Uninitialized);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                string errmes = string.Format(MessageResources.ErrClosePrj, "");
                throw new EcellException(errmes, ex);
            }
        }
        #endregion

        #region Method for DataAdd
        /// <summary>
        /// Adds the list of "EcellObject".
        /// </summary>
        /// <param name="ecellObjectList">The list of "EcellObject"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DataAdd(List<EcellObject> ecellObjectList, bool isRecorded, bool isAnchor)
        {
            int i = 0;
            int max = ecellObjectList.Count;
            foreach (EcellObject obj in ecellObjectList)
            {
                i++;
                DataAdd(obj, isRecorded, isAnchor && (i == max));
            }
        }

        /// <summary>
        /// Adds the "EcellObject".
        /// </summary>
        /// <param name="ecellObject"></param>
        public void DataAdd(EcellObject ecellObject)
        {
            DataAdd(ecellObject, true, true);
        }

        /// <summary>
        /// Adds the "EcellObject".
        /// </summary>
        /// <param name="ecellObject">The "EcellObject"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DataAdd(EcellObject ecellObject, bool isRecorded, bool isAnchor)
        {
            if (!ecellObject.IsUsable)
                return;

            List<EcellObject> usableList = new List<EcellObject>();
            string type = null;

            bool isUndoable = true; // Whether DataAdd action is undoable or not
            try
            {
                type = ecellObject.Type;
                ConfirmAnalysisReset("add", type);
                ConfirmReset("add", type);

                if (type.Equals(Constants.xpathProcess))
                {
                    DataAdd4Entity(ecellObject, true);
                    usableList.Add(ecellObject);
                }
                else if (type.Equals(Constants.xpathVariable))
                {
                    DataAdd4Entity(ecellObject, true);
                    usableList.Add(ecellObject);
                }
                else if (type.Equals(Constants.xpathText))
                {
                    DataAdd4Entity(ecellObject, true);
                    usableList.Add(ecellObject);
                }
                else if (type.Equals(Constants.xpathSystem))
                {
                    DataAdd4System(ecellObject, true);
                    if ("/".Equals(ecellObject.Key))
                        isUndoable = false;
                    usableList.Add(ecellObject);
                }
                else if (type.Equals(Constants.xpathStepper))
                {
                    // this.DataAdd4Stepper(ecellObject);
                    // usableList.Add(ecellObject);
                }
                else if (type.Equals(Constants.xpathModel))
                {
                    isUndoable = false;
                    DataAdd4Model(ecellObject, usableList);
                }
            }
            catch (IgnoreException)
            {
                // CancelしたときにIgnoreExceptionが発生するが無視しないと
                // Cancelしているのにエラーダイアログが表示されてしまう
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                usableList.Clear();
                throw new EcellException(
                   String.Format(MessageResources.ErrAdd,
                    new object[] { type, ecellObject.Key }), ex);
            }
            finally
            {
                if (usableList.Count > 0)
                {
                    if (isRecorded)
                    {
                        foreach (EcellObject obj in usableList)
                        {
                            m_env.ActionManager.AddAction(new DataAddAction(obj, isUndoable, false));
                        }
                    }
                    m_env.PluginManager.DataAdd(usableList);
                    if (type.Equals(EcellObject.SYSTEM))
                        m_env.PluginManager.RaiseRefreshEvent();
                    if (isAnchor)
                        m_env.ActionManager.AddAction(new AnchorAction());
                }
            }
        }

        /// <summary>
        /// Adds the "Model"
        /// </summary>
        /// <param name="ecellObject">The "Model"</param>
        /// <param name="usableList">The list of the added "EcellObject"</param>
        private void DataAdd4Model(EcellObject ecellObject, List<EcellObject> usableList)
        {
            List<EcellModel> modelList = m_currentProject.ModelList;

            string modelID = ecellObject.ModelID;

            foreach (EcellObject model in modelList)
            {
                Debug.Assert(!model.ModelID.Equals(modelID));
            }
            //
            // Sets the "Model".
            //
            modelList.Add((EcellModel)ecellObject);
            usableList.Add(ecellObject);
            //
            // Sets the root "System".
            //
            if (m_currentProject.SystemDic == null)
                m_currentProject.SystemDic = new Dictionary<string, List<EcellObject>>();
            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
            if (!sysDic.ContainsKey(modelID))
                sysDic[modelID] = new List<EcellObject>();

            Dictionary<string, EcellObject> dic = GetDefaultSystem(modelID);
            Debug.Assert(dic != null);
            sysDic[modelID].Add(dic[Constants.xpathSystem]);
            usableList.Add(dic[Constants.xpathSystem]);
            //
            // Sets the default parameter.
            //
            m_currentProject.Initialize(modelID);
            foreach (string simParam in m_currentProject.InitialCondition.Keys)
            {
                // Sets initial conditions.
                m_currentProject.StepperDic[simParam] = new Dictionary<string, List<EcellObject>>();
                m_currentProject.StepperDic[simParam][modelID] = new List<EcellObject>();
                m_currentProject.StepperDic[simParam][modelID].Add(dic[Constants.xpathStepper]);
                m_currentProject.LoggerPolicyDic[simParam] = new LoggerPolicy();
            }
            //
            // Messages
            //
            string message = String.Format(MessageResources.InfoAdd,
                new object[] { ecellObject.Type, modelID });
            MessageCreateEntity(EcellObject.MODEL, message);
            MessageCreateEntity(EcellObject.SYSTEM, message);
        }

        /// <summary>
        /// Returns the dictionary of the default "System" and the "Stepper".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <returns>The dictionary of the default "System" and the "Stepper"</returns>
        private Dictionary<string, EcellObject> GetDefaultSystem(string modelID)
        {
            Dictionary<string, EcellObject> dic = new Dictionary<string, EcellObject>();
            EcellObject systemEcellObject = null;
            EcellObject stepperEcellObject = null;
            WrappedSimulator simulator = m_currentProject.Simulator;
            BuildDefaultSimulator(simulator, null, null);
            systemEcellObject
                    = EcellObject.CreateObject(
                        modelID,
                        Constants.delimiterPath,
                        Constants.xpathSystem,
                        Constants.xpathSystem,
                        null);
            DataStorer.DataStored4System(
                    simulator,
                    systemEcellObject,
                    new Dictionary<string, double>());
            stepperEcellObject
                    = EcellObject.CreateObject(
                        modelID,
                        Constants.textKey,
                        Constants.xpathStepper,
                        "",
                        null);
            DataStorer.DataStored4Stepper(simulator, m_env, stepperEcellObject);
            dic[Constants.xpathSystem] = systemEcellObject;
            dic[Constants.xpathStepper] = stepperEcellObject;
            return dic;
        }

        /// <summary>
        /// Adds the "Stepper".
        /// </summary>
        /// <param name="ecellObject">The "Stepper"</param>
        private void DataAdd4Stepper(EcellObject ecellObject)
        {
            // Bypasses now.
        }

        /// <summary>
        /// Adds the "System".
        /// </summary>
        /// <param name="system">The System</param>
        /// <param name="messageFlag">The flag of the messages</param>
        private void DataAdd4System(EcellObject system, bool messageFlag)
        {
            string modelID = system.ModelID;
            string type = system.Type;

            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;

            if (!sysDic.ContainsKey(modelID))
                sysDic[modelID] = new List<EcellObject>();
            // Check duplicated system.
            foreach (EcellObject sys in sysDic[modelID])
            {
                if (!sys.Key.Equals(system.Key))
                    continue;
                throw new EcellException(String.Format(MessageResources.ErrAdd,
                    new object[] { type, system.Key }));
            }
            CheckEntityPath(system);
            m_currentProject.AddSystem(system);

            // Show Message.
            if (messageFlag)
            {
                MessageCreateEntity(EcellObject.SYSTEM,
                    String.Format(MessageResources.InfoAdd,
                    new object[] { type, system.Key }));
            }
        }

        /// <summary>
        /// Adds the "Process" or the "Variable".
        /// </summary>
        /// <param name="entity">The "Variable"</param>
        /// <param name="messageFlag">The flag of the messages</param>
        private void DataAdd4Entity(EcellObject entity, bool messageFlag)
        {
            string modelID = entity.ModelID;
            string key = entity.Key;
            string type = entity.Type;
            string systemKey = entity.ParentSystemID;

            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
            Debug.Assert(sysDic != null && sysDic.Count > 0);
            Debug.Assert(sysDic.ContainsKey(modelID));

            // Add object.
            bool findFlag = false;
            foreach (EcellObject system in sysDic[modelID])
            {
                if (!system.ModelID.Equals(modelID) || !system.Key.Equals(systemKey))
                    continue;
                if (system.Children == null)
                    system.Children = new List<EcellObject>();
                // Check duplicated object.
                foreach (EcellObject child in system.Children)
                {
                    if (!child.Key.Equals(key) || !child.Type.Equals(type))
                        continue;
                    throw new EcellException(
                        string.Format(
                            MessageResources.ErrExistObj,
                            new object[] { key }
                        )
                    );
                }
                // Set object.
                CheckEntityPath(entity);
                system.Children.Add(entity.Clone());
                findFlag = true;
                break;
            }

            if (messageFlag)
            {
                MessageCreateEntity(type, String.Format(MessageResources.InfoAdd,
                    new object[] { type, entity.Key }));
            }
            Debug.Assert(findFlag);

            if (entity.Value == null || entity.Value.Count <= 0)
                return;
            if (entity is EcellText)
                return;

            // Set Simulation param
            m_currentProject.AddSimulationParameter(entity);
        }
        #endregion

        #region Method for DataChanged
        /// <summary>
        /// Changes the "EcellObject".
        /// </summary>
        /// <param name="ecellObjectList">The changed "EcellObject"</param>
        public void DataChanged(List<EcellObject> ecellObjectList)
        {
            DataChanged(ecellObjectList, true, true);
        }

        /// <summary>
        /// Changes the "EcellObject".
        /// </summary>
        /// <param name="ecellObjectList">The changed "EcellObject"</param>
        /// <param name="isRecorded">The flag whether this action is recorded.</param>
        /// <param name="isAnchor">The flag whether this action is anchor.</param>
        public void DataChanged(List<EcellObject> ecellObjectList, bool isRecorded, bool isAnchor)
        {
            int i = 0;
            int max = ecellObjectList.Count;
            foreach (EcellObject obj in ecellObjectList)
            {
                i++;
                DataChanged(obj.ModelID, obj.Key, obj.Type, obj, isRecorded, isAnchor && (i == max));
            }
        }

        /// <summary>
        /// Changes the "EcellObject".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key</param>
        /// <param name="type">The type of the "EcellObject"</param>
        /// <param name="ecellObject">The changed "EcellObject"</param>
        public void DataChanged(string modelID, string key, string type, EcellObject ecellObject)
        {
            DataChanged(modelID, key, type, ecellObject, true, true);
        }

        /// <summary>
        /// Changes the "EcellObject".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key</param>
        /// <param name="type">The type of the "EcellObject"</param>
        /// <param name="ecellObject">The changed "EcellObject"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DataChanged(
            string modelID,
            string key,
            string type,
            EcellObject ecellObject,
            bool isRecorded,
            bool isAnchor)
        {
            Debug.Assert(!String.IsNullOrEmpty(modelID));
            Debug.Assert(!String.IsNullOrEmpty(key));
            Debug.Assert(!String.IsNullOrEmpty(type));

            // DataChange for simulation.
            try
            {
                if (m_env.PluginManager.Status == ProjectStatus.Analysis)
                {
                    EcellObject obj = GetEcellObject(modelID, key, type);
                    if (!key.Equals(ecellObject.Key) ||
                        !obj.Classname.Equals(ecellObject.Classname) ||
                        obj.Value.Count != ecellObject.Value.Count ||
                        (obj is EcellProcess && Util.DoesVariableReferenceChange(obj, ecellObject)))
                    {
                        ConfirmAnalysisReset("change", type);
                    }
                }

                // StatusCheck
                if (m_currentProject.SimulationStatus == SimulationStatus.Run ||
                    m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    EcellObject obj = GetEcellObject(modelID, key, type);
                    // Confirm Reset.
                    if (!key.Equals(ecellObject.Key) ||
                        !obj.Classname.Equals(ecellObject.Classname) ||
                        obj.Value.Count != ecellObject.Value.Count ||
                        (obj is EcellProcess && Util.DoesVariableReferenceChange(obj, ecellObject)))
                    {
                        ConfirmReset("change", type);
                    }
                    // DataChange for parameter.
                    else if (ecellObject.Type == EcellObject.PROCESS ||
                            ecellObject.Type == EcellObject.VARIABLE ||
                        ecellObject.Type == EcellObject.SYSTEM)
                    {
                        foreach (EcellData d in obj.Value)
                        {
                            foreach (EcellData d1 in ecellObject.Value)
                            {
                                if (!d.Name.Equals(d1.Name))
                                    continue;
                                if (!d.Value.ToString().Equals(d1.Value.ToString()))
                                {
                                    m_currentProject.Simulator.SetEntityProperty(d1.EntityPath, d1.Value.Value);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (IgnoreException)
            {
                // CancelしたときにIgnoreExceptionが発生するが無視しないと
                // Cancelしているのにエラーダイアログが表示されてしまう
                return;
            }

            // Check Duplication Error.
            if (key != ecellObject.Key &&
                GetEcellObject(ecellObject.ModelID, ecellObject.Key, ecellObject.Type) != null)
            {
                throw new EcellException(string.Format(MessageResources.ErrExistObj, ecellObject.Key));
            }

            try
            {
                // Record action
                EcellObject oldObj = GetEcellObject(modelID, key, type);

                // Searches the "System".
                List<EcellObject> systemList = m_currentProject.SystemDic[modelID];
                Debug.Assert(systemList != null && systemList.Count > 0);

                // Checks the EcellObject
                CheckEntityPath(ecellObject);

                // Record Action.
                if (isRecorded)
                    this.m_env.ActionManager.AddAction(new DataChangeAction(modelID, type, oldObj, ecellObject, false));
                // 4 System & Entity
                if (ecellObject.Type.Equals(Constants.xpathModel))
                {
                    DataChanged4Model(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }
                else if (ecellObject.Type.Equals(Constants.xpathSystem))
                {
                    DataChanged4System(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }
                else if (ecellObject.Type.Equals(Constants.xpathProcess))
                {
                    DataChanged4Entity(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }
                else if (ecellObject.Type.Equals(Constants.xpathText))
                {
                    DataChanged4Entity(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }
                else if (ecellObject.Type.Equals(Constants.xpathVariable))
                {
                    DataChanged4Entity(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }

                if (key != ecellObject.Key)
                    CheckParameterObservedData(oldObj, ecellObject.Key);

                //if (!oldObj.IsPosSet)
                //    m_env.PluginManager.SetPosition(oldObj);
                // Set Event Anchor.
                if (isAnchor)
                    this.m_env.ActionManager.AddAction(new AnchorAction());
            }
            catch (Exception ex)
            {
                throw new EcellException(
                    string.Format(
                    MessageResources.ErrSetProp,
                    new object[] { key }),
                    ex);
            }
        }

        /// <summary>
        /// Changes the "Model".
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="ecellObject"></param>
        /// <param name="isRecorded"></param>
        /// <param name="isAnchor"></param>
        private void DataChanged4Model(string modelID, string key, string type, EcellObject ecellObject, bool isRecorded, bool isAnchor)
        {
            if (modelID.Equals(ecellObject.ModelID))
            {
                m_currentProject.ModelList[0].Layers = ((EcellModel)ecellObject).Layers;
                m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
                return;
            }

            // ToDo: モデル名が変更された場合の処理（各オブジェクトのモデルIDの変更とDataChangedEventの実行）を記述する。
            // Caution! 現時点ではモデル名の変更はできません。
            throw new Exception("The method to change ModelID is not implemented.");
        }

        /// <summary>
        /// Changes the "Variable" or the "Process".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key</param>
        /// <param name="type">The type</param>
        /// <param name="ecellObject">The changed "Variable" or the "Process"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        private void DataChanged4Entity(
            string modelID, string key, string type, EcellObject ecellObject, bool isRecorded, bool isAnchor)
        {
            // Get changed node.
            EcellObject oldNode = m_currentProject.GetEcellObject(modelID, type, key);
            EcellObject oldSystem = m_currentProject.GetSystem(modelID, Util.GetSuperSystemPath(key));
            Debug.Assert(oldNode != null);
            Debug.Assert(oldSystem != null);

            this.CheckDifferences(oldNode, ecellObject, null);
            if (modelID.Equals(ecellObject.ModelID)
                && key.Equals(ecellObject.Key)
                && type.Equals(ecellObject.Type))
            {
                oldSystem.Children.Remove(oldNode);
                oldSystem.Children.Add(ecellObject);
                m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
                return;
            }

            // Get parent system.
            // Add new object.
            DataAdd4Entity(ecellObject.Clone(), false);
            m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
            if (type.Equals(Constants.xpathVariable))
            {
                List<EcellObject> list = CheckVariableReferenceChanges(key, ecellObject.Key);
                DataChanged(list, false, false);
            }
            // Deletes the old object.
            DataDelete4Node(modelID, key, type, false, true, false);
            return;
        }

        /// <summary>
        /// Changes the "System".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key</param>
        /// <param name="type">The type</param>
        /// <param name="ecellObject">The changed "System"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        private void DataChanged4System(string modelID, string key, string type, EcellObject ecellObject, bool isRecorded, bool isAnchor)
        {
            m_currentProject.SortSystems();
            List<EcellObject> systemList = m_currentProject.SystemDic[modelID];

            // When system path is not modified.
            if (modelID.Equals(ecellObject.ModelID) && key.Equals(ecellObject.Key))
            {
                // Changes some properties.
                for (int i = 0; i < systemList.Count; i++)
                {
                    if (!systemList[i].Key.Equals(key))
                        continue;

                    CheckDifferences(systemList[i], ecellObject, null);
                    systemList[i] = ecellObject.Clone();
                    m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
                    break;
                }
                return;
            }

            // Check Root
            if (key == Constants.delimiterPath)
                throw new EcellException(MessageResources.ErrChangeRoot);

            // Changes the object.
            Dictionary<string, string> variableKeyDic = new Dictionary<string, string>();
            Dictionary<string, string> oldKeyDic = new Dictionary<string, string>();
            List<EcellObject> tempList = new List<EcellObject>();
            tempList.AddRange(systemList);
            foreach (EcellObject system in tempList)
            {
                if (!system.Key.Equals(key) && !system.Key.StartsWith(key + Constants.delimiterPath))
                    continue;

                // Adds the new "System" object.
                string newKey = ecellObject.Key + system.Key.Substring(key.Length);
                oldKeyDic.Add(newKey, system.Key);
                EcellSystem newSystem
                    = (EcellSystem)EcellObject.CreateObject(modelID, newKey, system.Type, system.Classname, system.Value);
                if (newSystem.Key == ecellObject.Key)
                    newSystem.SetPosition(ecellObject);
                else
                    newSystem.SetPosition(system);
                CheckEntityPath(newSystem);
                CheckDifferences(system, newSystem, null);
                CheckParameterObservedData(system, newSystem.Key);
                m_currentProject.AddSystem(newSystem);
                m_currentProject.DeleteSystem(system);

                // Change Child
                foreach (EcellObject child in system.Children)
                {
                    EcellObject copy = child.Clone();
                    copy.ParentSystemID = newKey;
                    CheckEntityPath(copy);
                    m_currentProject.AddEntity(copy);
                    CheckParameterObservedData(child, copy.Key);

                    oldKeyDic.Add(copy.Key, child.Key);
                    if (copy.Type.Equals(Constants.xpathVariable))
                        variableKeyDic.Add(child.Key, copy.Key);
                }
            }

            ecellObject = m_currentProject.GetSystem(modelID, ecellObject.Key);
            m_env.PluginManager.DataChanged(modelID, key, ecellObject.Type, ecellObject);
            // Checks all processes.
            m_currentProject.SortSystems();
            List<EcellObject> processList = CheckVariableReferenceChanges(variableKeyDic);
            DataChanged(processList, true, false);
        }

        /// <summary>
        /// SetPosition
        /// </summary>
        /// <param name="eo"></param>
        public void SetPosition(EcellObject eo)
        {
            EcellObject oldNode = m_currentProject.GetEcellObject(eo.ModelID, eo.Type, eo.Key);
            oldNode.SetPosition(eo);
            // not implement.
            m_env.PluginManager.SetPosition(oldNode.Clone());
        }

        #endregion

        #region Method for DataDelete
        /// <summary>
        /// Deletes the "EcellObject".
        /// </summary>
        /// <param name="eo"></param>
        public void DataDelete(EcellObject eo)
        {
            DataDelete(eo.ModelID, eo.Key, eo.Type, true, true);
        }

        /// <summary>
        /// Deletes the "EcellObject".
        /// </summary>
        /// <param name="eo"></param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DataDelete(EcellObject eo, bool isRecorded, bool isAnchor)
        {
            DataDelete(eo.ModelID, eo.Key, eo.Type, isRecorded, isAnchor);
        }

        /// <summary>
        /// Deletes the "EcellObject" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key of the "EcellObject"</param>
        /// <param name="type">The type of the "EcellObject"</param>
        public void DataDelete(string modelID, string key, string type)
        {
            DataDelete(modelID, key, type, true, true);
        }

        /// <summary>
        /// Deletes the "EcellObject" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key of the "EcellObject"</param>
        /// <param name="type">The type of the "EcellObject"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DataDelete(string modelID, string key, string type, bool isRecorded, bool isAnchor)
        {
            try
            {
                ConfirmAnalysisReset("delete", type);
                ConfirmReset("delete", type);
            }
            catch (IgnoreException)
            {
                // CancelしたときにIgnoreExceptionが発生するが無視しないと
                // Cancelしているのにエラーダイアログが表示されてしまう
                return;
            }

            // Check root
            if (key.Equals("/"))
            {
                Util.ShowErrorDialog(MessageResources.ErrDelRoot);
                return;
            }
            // Check Model
            if (string.IsNullOrEmpty(modelID))
                return;
            // Check Object;
            EcellObject deleteObj = GetEcellObject(modelID, key, type);
            if (deleteObj == null)
                return;

            try
            {
                foreach (EcellData data in deleteObj.Value)
                {
                    if (GetParameterData(data.EntityPath) != null)
                    {
                        RemoveParameterData(new EcellParameterData(data.EntityPath, 0.0));
                    }
                    if (GetObservedData(data.EntityPath) != null)
                    {
                        RemoveObservedData(new EcellObservedData(data.EntityPath, 0.0));
                    }
                }

                if (string.IsNullOrEmpty(key))
                {
                    DataDelete4Model(modelID);
                }
                else if (key.Contains(":"))
                { // not system
                    DataDelete4Node(modelID, key, type, true, isRecorded, false);
                }
                else
                { // system
                    DataDelete4System(modelID, key, true, isRecorded);
                }
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrDelete,
                    new object[] { key }), ex);
            }
            finally
            {
                m_env.PluginManager.DataDelete(modelID, key, type);
                if (isRecorded)
                    m_env.ActionManager.AddAction(new DataDeleteAction(modelID, key, type, deleteObj, false));
                if (type.Equals(EcellObject.SYSTEM))
                    m_env.PluginManager.RaiseRefreshEvent();
                if (isAnchor)
                    this.m_env.ActionManager.AddAction(new AnchorAction());
            }
        }

        /// <summary>
        /// Deletes the "Model" using the model ID.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        private void DataDelete4Model(string modelID)
        {
            string message = "[" + modelID + "]";
            //
            // Delete the "Model".
            //
            bool isDelete = false;
            foreach (EcellObject obj in m_currentProject.ModelList)
            {
                if (obj.ModelID == modelID)
                {
                    m_currentProject.ModelList.Remove((EcellModel)obj);
                    isDelete = true;
                    break;
                }
            }
            Debug.Assert(isDelete);

            // Deletes "System"s.
            if (m_currentProject.SystemDic.ContainsKey(modelID))
            {
                m_currentProject.SystemDic.Remove(modelID);
            }
            // Deletes "Stepper"s.
            foreach (string param in m_currentProject.StepperDic.Keys)
            {
                if (m_currentProject.StepperDic[param].ContainsKey(modelID))
                {
                    m_currentProject.StepperDic[param].Remove(modelID);
                }
            }
            MessageDeleteEntity(EcellObject.MODEL, message);
        }

        /// <summary>
        /// Deletes the "System" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="model">The model ID</param>
        /// <param name="key">The key of the "EcellObject"</param>
        /// <param name="messageFlag">The flag of the messages</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        private void DataDelete4System(string model, string key, bool messageFlag, bool isRecorded)
        {
            List<EcellObject> sysList = m_currentProject.SystemDic[model];

            string message = "[" + model + "][" + key + "]";
            // List up systems for delete.
            List<EcellObject> delList = new List<EcellObject>();
            foreach (EcellObject sys in sysList)
            {
                if (sys.Key.Equals(key) || sys.Key.StartsWith(key + "/"))
                    delList.Add(sys);
            }

            // Delete system
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> initialCondition = this.m_currentProject.InitialCondition;
            Dictionary<string, string> varDic = new Dictionary<string, string>();
            for (int i = delList.Count - 1; i >= 0; i--)
            {
                EcellObject sys = delList[i];
                m_currentProject.DeleteSystem(sys);
                // Record deletion of child system. 
                if (!sys.Key.Equals(key))
                    m_env.ActionManager.AddAction(new DataDeleteAction(sys.ModelID, sys.Key, sys.Type, sys, false));
                if (messageFlag)
                    MessageDeleteEntity(EcellObject.SYSTEM, message);

                // Check VariableReferences to delete
                foreach (EcellObject child in sys.Children)
                {
                    if (!child.Type.Equals(Constants.xpathVariable))
                        continue;
                    varDic.Add(child.Key, null);
                }
            }

            // Update VariableReferences.
            List<EcellObject> processlist = CheckVariableReferenceChanges(varDic);
            DataChanged(processlist, isRecorded, false);
        }

        /// <summary>
        /// Deletes the "Process" or the "Variable" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="model">The model ID</param>
        /// <param name="key">The key of the "EcellObject"</param>
        /// <param name="type">The type of the "EcellObject"</param>
        /// <param name="messageFlag">The flag of the message</param>
        /// <param name="isRecorded">The flag whether this action is recorded.</param>
        /// <param name="isAnchor">The flag whether this action is anchor.</param>
        private void DataDelete4Node(
            string model,
            string key,
            string type,
            bool messageFlag,
            bool isRecorded,
            bool isAnchor)
        {
            if (!m_currentProject.SystemDic.ContainsKey(model))
                return;

            // Show Message.
            string message = "[" + model + "][" + key + "]";
            if (messageFlag)
                MessageDeleteEntity(type, message);

            // Delete node.
            EcellObject node = GetEcellObject(model, key, type);
            if (node != null)
                m_currentProject.DeleteEntity(node);

            // Update VariableReference.
            if (node is EcellVariable)
            {
                List<EcellObject> processList = CheckVariableReferenceChanges(key, null);
                DataChanged(processList, isRecorded, false);
            }
        }

        /// <summary>
        /// Move the component to the upper system, when system is deleted.
        /// </summary>
        /// <param name="modelID">modelID of deleted system.</param>
        /// <param name="sysKey">key of deleted system.</param>
        public void DataMerge(string modelID, string sysKey)
        {
            // Check system.
            EcellObject system = GetEcellObject(modelID, sysKey, EcellObject.SYSTEM);
            if (system == null)
                throw new EcellException(String.Format(MessageResources.ErrFindEnt, new object[] { sysKey }));
            // CheckRoot
            if (system.Key.Equals("/"))
                throw new EcellException(MessageResources.ErrDelRoot);
            // Confirm system merge.
            if (!Util.ShowYesNoDialog(MessageResources.ConfirmMerge))
                return;

            // Get objects under this system.
            string parentSysKey = system.ParentSystemID;
            List<EcellObject> eoList = new List<EcellObject>(); ;
            foreach (EcellObject sys in m_currentProject.SystemDic[modelID])
            {
                if (!sys.Key.StartsWith(sysKey + "/") && !sys.Key.Equals(sysKey))
                    continue;
                // Add System
                if (!sys.Key.Equals(sysKey))
                {
                    eoList.Add(sys.Clone());
                    continue;
                }
                // Add Nodes
                foreach (EcellObject node in sys.Children)
                {
                    if (node.Key.EndsWith(":SIZE"))
                        continue;
                    eoList.Add(node.Clone());
                }
            }

            // Check and replace object keys.
            Dictionary<string, string> varDic = new Dictionary<string, string>();
            List<EcellProcess> processList = new List<EcellProcess>();
            string oldKey;
            string newKey;
            foreach (EcellObject eo in eoList)
            {
                // Check Duplication
                oldKey = eo.Key;
                newKey = Util.GetMovedKey(oldKey, sysKey, parentSysKey);
                if (GetEcellObject(modelID, newKey, eo.Type) != null)
                {
                    throw new EcellException(String.Format(MessageResources.ErrExistObj,
                        new object[] { newKey }));
                }
                CheckParameterObservedData(eo, newKey);
                eo.Key = newKey;
                // Set varDic
                if (eo is EcellVariable)
                    varDic.Add(oldKey, newKey);
                else if (eo is EcellProcess)
                    processList.Add((EcellProcess)eo);

                // Check process
                List<EcellObject> tempList = new List<EcellObject>();
                if (!(eo is EcellSystem))
                    continue;
                foreach (EcellObject child in eo.Children)
                {
                    oldKey = child.Key;
                    newKey = Util.GetMovedKey(oldKey, sysKey, parentSysKey);
                    CheckParameterObservedData(child, newKey);
                    child.Key = newKey;

                    // Set varDic
                    if (child is EcellVariable)
                    {
                        varDic.Add(oldKey, newKey);
                    }
                    else if (child is EcellProcess)
                    {
                        processList.Add((EcellProcess)child);
                        tempList.Add(child);
                    }
                }
                // Sort children.
                foreach (EcellObject child in tempList)
                {
                    eo.Children.Remove(child);
                    eo.Children.Add(child);
                }
            }
            // Update VariableReferences for new processes.
            CheckVariableReferenceChanges(varDic, processList);

            // Get Processes who's VariableReference will be removed in DataDelete Event.
            processList.Clear();
            foreach (EcellObject sys in m_currentProject.SystemDic[modelID])
            {
                if (sys.Key.StartsWith(sysKey + "/"))
                    continue;
                if (sys.Key.Equals(sysKey))
                    continue;

                // Add Nodes
                foreach (EcellObject node in sys.Children)
                {
                    if (!(node is EcellProcess))
                        continue;
                    processList.Add((EcellProcess)node.Clone());
                }
            }
            List<EcellObject> updatingProcesses = CheckVariableReferenceChanges(varDic, processList);

            // Remove system.
            DataDelete(system, true, false);

            bool isRecorded = (updatingProcesses.Count <= 0);
            // Move systems and nodes under merged system.
            DataAdd(eoList, true, false);

            // Update VariableReferences
            DataChanged(updatingProcesses, true, false);

            EcellObject parent = GetEcellObject(modelID, parentSysKey, EcellObject.SYSTEM);
            DataChanged(modelID, parent.Key, parent.Type, parent, true, true);

            m_env.PluginManager.RaiseRefreshEvent();
        }

        #endregion

        #region Method for DataCheck
        /// <summary>
        /// Checks differences between the source "EcellObject" and the destination.
        /// </summary>
        /// <param name="src">The source "EcellObject"</param>
        /// <param name="dest">The  destination "EcellObject"</param>
        /// <param name="parameterID">The simulation parameter ID</param>
        private bool CheckDifferences(EcellObject src, EcellObject dest, string parameterID)
        {
            bool updated = false;
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> initialCondition = this.m_currentProject.InitialCondition;

            // Set Message
            string message = null;
            if (string.IsNullOrEmpty(parameterID))
                message = "[" + src.ModelID + "][" + src.Key + "]";
            else
                message = "[" + parameterID + "][" + src.ModelID + "][" + src.Key + "]";

            // Check Class change.
            if (!src.Classname.Equals(dest.Classname))
            {
                updated = true;
                MessageUpdateData(Constants.xpathClassName, message, src.Classname, dest.Classname);
            }
            // Check Key change.
            if (!src.Key.Equals(dest.Key))
            {
                updated = true;
                MessageUpdateData(Constants.xpathKey, message, src.Key, dest.Key);
            }

            // Changes a className and not change a key.
            if (!src.Classname.Equals(dest.Classname) && src.Key.Equals(dest.Key))
            {
                foreach (EcellData srcEcellData in src.Value)
                {
                    // Changes the logger.
                    if (srcEcellData.Logged)
                    {
                        MessageDeleteEntity("Logger", message + "[" + srcEcellData.Name + "]");
                    }
                    // Changes the initial parameter.
                    if (!src.Type.Equals(Constants.xpathSystem)
                        && !src.Type.Equals(Constants.xpathProcess)
                        && !src.Type.Equals(Constants.xpathVariable))
                        continue;
                    if (!srcEcellData.IsInitialized())
                        continue;

                    if (string.IsNullOrEmpty(parameterID))
                    {
                        foreach (string keyParameterID in initialCondition.Keys)
                        {
                            Dictionary<string, double> condition = initialCondition[keyParameterID][src.ModelID];
                            if (condition.ContainsKey(srcEcellData.EntityPath))
                            {
                                condition.Remove(srcEcellData.EntityPath);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, double> condition = initialCondition[parameterID][src.ModelID];
                        if (condition.ContainsKey(srcEcellData.EntityPath))
                        {
                            condition.Remove(srcEcellData.EntityPath);
                        }
                    }
                }
                foreach (EcellData destEcellData in dest.Value)
                {
                    // Changes the initial parameter.
                    if (!dest.Type.Equals(Constants.xpathSystem)
                        && !dest.Type.Equals(Constants.xpathProcess)
                        && !dest.Type.Equals(Constants.xpathVariable))
                        continue;
                    if (!destEcellData.IsInitialized())
                        continue;

                    // GetValue
                    EcellValue value = destEcellData.Value;
                    double temp = 0;
                    if (value.IsDouble)
                        temp = (double)value;
                    else if (value.IsInt)
                        temp = (double)value;
                    else
                        continue;

                    if (!string.IsNullOrEmpty(parameterID))
                    {
                        initialCondition[parameterID][dest.ModelID][destEcellData.EntityPath] = temp;
                    }
                    else
                    {
                        foreach (string keyParameterID in initialCondition.Keys)
                        {
                            initialCondition[keyParameterID][dest.ModelID][destEcellData.EntityPath] = temp;
                        }
                    }
                }
            }
            else
            {
                foreach (EcellData srcEcellData in src.Value)
                {
                    foreach (EcellData destEcellData in dest.Value)
                    {
                        if (!srcEcellData.Name.Equals(destEcellData.Name) ||
                            !srcEcellData.EntityPath.Equals(destEcellData.EntityPath))
                            continue;

                        if (!srcEcellData.Logged && destEcellData.Logged)
                        {
                            MessageCreateEntity("Logger", message + "[" + srcEcellData.Name + "]");

                            WrappedSimulator simulator = m_currentProject.Simulator;
                            LoggerPolicy loggerPolicy = this.GetCurrentLoggerPolicy();
                            CreateLogger(srcEcellData.EntityPath, false, simulator, loggerPolicy);
                            updated = true;
                        }
                        else if (srcEcellData.Logged && !destEcellData.Logged)
                        {
                            MessageDeleteEntity("Logger", message + "[" + srcEcellData.Name + "]");
                            updated = true;
                        }
                        if (!srcEcellData.Value.ToString()
                                .Equals(destEcellData.Value.ToString()))
                        {
                            Trace.WriteLine(
                                "Update Data: " + message
                                    + "[" + srcEcellData.Name + "]"
                                    + System.Environment.NewLine
                                    + "\t[" + srcEcellData.Value.ToString()
                                    + "]->[" + destEcellData.Value.ToString() + "]");
                            updated = true;
                        }
                        //
                        // Changes the initial parameter.
                        //
                        if (!src.Type.Equals(Constants.xpathSystem)
                            && !src.Type.Equals(Constants.xpathProcess)
                            && !src.Type.Equals(Constants.xpathVariable))
                            continue;

                        EcellValue value = destEcellData.Value;
                        if (!srcEcellData.IsInitialized()
                            || srcEcellData.Value.Equals(value))
                            continue;

                        // GetValue
                        double temp = 0;
                        if (value.IsDouble)
                            temp = (double)value;
                        else if (value.IsInt)
                            temp = (double)value;
                        else
                            continue;

                        if (!string.IsNullOrEmpty(parameterID))
                        {
                            Dictionary<string, double> condition = initialCondition[parameterID][src.ModelID];
                            if (!condition.ContainsKey(srcEcellData.EntityPath))
                                continue;
                            condition[srcEcellData.EntityPath] = temp;
                        }
                        else
                        {
                            foreach (string keyParameterID in initialCondition.Keys)
                            {
                                Dictionary<string, double> condition = initialCondition[keyParameterID][src.ModelID];

                                if (!condition.ContainsKey(srcEcellData.EntityPath))
                                    continue;
                                condition[srcEcellData.EntityPath] = temp;
                            }
                        }
                        break;
                    }
                }
            }
            return updated;
        }

        /// <summary>
        /// Checks differences between the key and the entity path.
        /// </summary>
        /// <param name="ecellObject">The checked "EcellObject"</param>
        private static void CheckEntityPath(EcellObject ecellObject)
        {
            if (ecellObject.Key == null)
            {
                return;
            }
            if (ecellObject.Type.Equals(Constants.xpathSystem))
            {
                string entityPath = null;
                string parentPath = ecellObject.ParentSystemID;
                string childPath = ecellObject.LocalID;
                entityPath = ecellObject.Type + Constants.delimiterColon
                    + parentPath + Constants.delimiterColon
                    + childPath + Constants.delimiterColon;
                if (ecellObject.Value != null && ecellObject.Value.Count > 0)
                {
                    for (int i = 0; i < ecellObject.Value.Count; i++)
                    {
                        if (!ecellObject.Value[i].EntityPath.Equals(
                            entityPath + ecellObject.Value[i].Name))
                        {
                            ecellObject.Value[i].EntityPath
                                = entityPath + ecellObject.Value[i].Name;
                        }
                    }
                }
                if (ecellObject.Children != null && ecellObject.Children.Count > 0)
                {
                    for (int i = 0; i < ecellObject.Children.Count; i++)
                    {
                        CheckEntityPath(ecellObject.Children[i]);
                    }
                }
            }
            else if (ecellObject.Type.Equals(Constants.xpathProcess) || ecellObject.Type.Equals(Constants.xpathVariable))
            {
                string entityPath
                    = ecellObject.Type + Constants.delimiterColon
                    + ecellObject.Key + Constants.delimiterColon;
                if (ecellObject.Value != null && ecellObject.Value.Count > 0)
                {
                    for (int i = 0; i < ecellObject.Value.Count; i++)
                    {
                        if (!ecellObject.Value[i].EntityPath.Equals(
                            entityPath + ecellObject.Value[i].Name))
                        {
                            ecellObject.Value[i].EntityPath
                                = entityPath + ecellObject.Value[i].Name;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Process list to update VariableReference.
        /// </summary>
        /// <param name="oldKey"></param>
        /// <param name="newKey"></param>
        /// <returns></returns>
        private List<EcellObject> CheckVariableReferenceChanges(string oldKey, string newKey)
        {
            Dictionary<string, string> varDic = new Dictionary<string, string>();
            varDic.Add(oldKey, newKey);
            return CheckVariableReferenceChanges(varDic);
        }

        /// <summary>
        /// Get Process list to update VariableReference.
        /// </summary>
        /// <param name="variableDic">Dictionary of VariableReference changed."Dictionary[oldKey,newKey]"</param>
        /// <returns></returns>
        private List<EcellObject> CheckVariableReferenceChanges(Dictionary<string, string> variableDic)
        {
            // Get ProcessList
            List<EcellProcess> processList = new List<EcellProcess>();
            foreach (EcellObject system in m_currentProject.SystemList)
            {
                if (system.Children == null || system.Children.Count <= 0)
                    continue;

                foreach (EcellObject child in system.Children)
                {
                    if (!(child is EcellProcess))
                        continue;
                    processList.Add((EcellProcess)child.Clone());
                }
            }
            // Check VariableReference
            List<EcellObject> returnList = CheckVariableReferenceChanges(variableDic, processList);
            return returnList;
        }

        /// <summary>
        /// CheckVariableReferenceChanges.
        /// </summary>
        /// <param name="variableDic"></param>
        /// <param name="processList"></param>
        /// <returns></returns>
        private static List<EcellObject> CheckVariableReferenceChanges(Dictionary<string, string> variableDic, List<EcellProcess> processList)
        {
            List<EcellObject> returnList = new List<EcellObject>();
            bool changedFlag = false;
            foreach (EcellProcess process in processList)
            {
                changedFlag = false;
                List<EcellReference> refList = process.ReferenceList;
                List<EcellReference> newList = new List<EcellReference>();
                foreach (EcellReference er in refList)
                {
                    // in case er isn't changed
                    if (!variableDic.ContainsKey(er.Key))
                    {
                        newList.Add(er);
                        continue;
                    }
                    changedFlag = true;
                    string refKey = variableDic[er.Key];
                    // in case er is removed.
                    if (refKey == null)
                        continue;
                    // in case er is changed.
                    er.Key = refKey;
                    newList.Add(er);
                }
                if (!changedFlag)
                    continue;
                process.ReferenceList = newList;
                returnList.Add(process);
            }
            return returnList;
        }

        /// <summary>
        /// ConfirmReset
        /// </summary>
        /// <param name="action"></param>
        /// <param name="type"></param>
        private void ConfirmReset(string action, string type)
        {
            if (m_currentProject.SimulationStatus == SimulationStatus.Wait)
                return;
            if (EcellObject.TEXT.Equals(type))
                return;

            if (!Util.ShowOKCancelDialog(MessageResources.ConfirmReset))
            {
                throw new IgnoreException("Can't " + action + " the object.");
            }
            SimulationStop();
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }

        /// <summary>
        /// ConfirmAnalysisReset
        /// </summary>
        /// <param name="action"></param>
        /// <param name="type"></param>
        private void ConfirmAnalysisReset(string action, string type)
        {
            if (m_env.PluginManager.Status != ProjectStatus.Analysis)
                return;
            if (EcellObject.TEXT.Equals(type))
                return;

            if (!Util.ShowOKCancelDialog(MessageResources.ConfirmAnalysisReset))
            {
                throw new IgnoreException("Can't " + action + " the object.");
            }
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }

        /// <summary>
        /// Check parameters.
        /// </summary>
        /// <param name="oldObj"></param>
        /// <param name="newkey"></param>
        private void CheckParameterObservedData(EcellObject oldObj, string newkey)
        {
            foreach (EcellData data in oldObj.Value)
            {
                EcellParameterData param = GetParameterData(data.EntityPath);
                if (param != null)
                {
                    SetParameterData(new EcellParameterData(data.EntityPath.Replace(oldObj.Key, newkey),
                        param.Max, param.Min, param.Step));
                }
                EcellObservedData observed = GetObservedData(data.EntityPath);
                if (observed != null)
                {
                    SetObservedData(new EcellObservedData(data.EntityPath.Replace(oldObj.Key, newkey),
                        observed.Max, observed.Min, observed.Differ, observed.Rate));
                }
            }
        }

        #endregion

        #region Create Default Object.
        /// <summary>
        /// Create the default object(Process, Variable and System).
        /// </summary>
        /// <param name="modelID">the model ID of created object.</param>
        /// <param name="key">the system path of parent object.</param>
        /// <param name="type">the type of created object.</param>
        /// <returns>the create object.</returns>
        public EcellObject CreateDefaultObject(string modelID, string key, string type)
        {
            EcellObject obj = null;
            try
            {
                if (type.Equals(Constants.xpathSystem))
                {
                    obj = CreateDefaultSystem(modelID, key);
                }
                else if (type.Equals(Constants.xpathProcess))
                {
                    obj = CreateDefaultProcess(modelID, key);
                }
                else if (type.Equals(Constants.xpathVariable))
                {
                    obj = CreateDefaultVariable(modelID, key);
                }
                else if (type.Equals(Constants.xpathText))
                {
                    obj = CreateDefaultText(modelID);
                }
                return obj;
            }
            catch (IgnoreException)
            {
                return null;
            }
            catch (EcellException ex)
            {
                String message = String.Format(MessageResources.ErrAdd,
                    new object[] { type, key });
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Create the text with temporary ID.
        /// </summary>
        /// <param name="modelID">model ID of created object.</param>
        /// <returns></returns>
        private EcellObject CreateDefaultText(string modelID)
        {
            string nodeKey = GetTemporaryID(modelID, EcellObject.TEXT, "/");
            EcellText text = new EcellText(modelID, nodeKey, EcellObject.TEXT, EcellObject.TEXT, new List<EcellData>());
            return text;
        }

        /// <summary>
        /// Create the process with temporary ID.
        /// </summary>
        /// <param name="modelID">model ID of created object.</param>
        /// <param name="key">the key of parent system object.</param>
        /// <returns>the create object.</returns>
        private EcellObject CreateDefaultProcess(string modelID, string key)
        {
            String tmpID = GetTemporaryID(modelID, Constants.xpathProcess, key);

            // Get Default StepperID.
            EcellObject sysobj = GetEcellObject(modelID, key, Constants.xpathSystem);
            if (sysobj == null)
                return null;
            String stepperID = "";
            foreach (EcellData d in sysobj.Value)
            {
                if (!d.Name.Equals(Constants.xpathStepperID))
                    continue;
                stepperID = d.Value.ToString();
            }

            // Set Parameters.
            Dictionary<string, EcellData> list = GetProcessProperty(Constants.DefaultProcessName);
            List<EcellData> data = new List<EcellData>();
            foreach (EcellData d in list.Values)
            {
                if (d.Name.Equals(Constants.xpathStepperID))
                {
                    d.Value = new EcellValue(stepperID);
                }
                if (d.Name.Equals(Constants.xpathVRL))
                {
                    d.Value = new EcellValue(new List<EcellValue>());
                }
                if (d.Name.Equals(Constants.xpathK))
                {
                    d.Value = new EcellValue(1.0);
                }
                data.Add(d);
            }

            EcellObject obj = EcellObject.CreateObject(modelID, tmpID,
                Constants.xpathProcess, Constants.DefaultProcessName, data);
            return obj;
        }

        /// <summary>
        /// Create the variable with temporary ID.
        /// </summary>
        /// <param name="modelID">model ID of created object.</param>
        /// <param name="key">the key of parent system object.</param>
        /// <returns>the create object.</returns>
        private EcellObject CreateDefaultVariable(string modelID, string key)
        {
            String tmpID = GetTemporaryID(modelID, Constants.xpathVariable, key);

            Dictionary<string, EcellData> list = GetVariableProperty();
            List<EcellData> data = new List<EcellData>();
            foreach (EcellData d in list.Values)
            {
                data.Add(d);
            }
            EcellObject obj = EcellObject.CreateObject(modelID, tmpID,
                Constants.xpathVariable, Constants.xpathVariable, data);
            return obj;
        }

        /// <summary>
        /// Create the system with temporary ID.
        /// </summary>
        /// <param name="modelID">model ID of created object.</param>
        /// <param name="key">the key of parent system object.</param>
        /// <returns>the create object.</returns>
        private EcellObject CreateDefaultSystem(string modelID, string key)
        {
            String tmpID = GetTemporaryID(modelID, Constants.xpathSystem, key);

            EcellObject sysobj = GetEcellObject(modelID, key, Constants.xpathSystem);
            if (sysobj == null) return null;
            String stepperID = "";
            foreach (EcellData d in sysobj.Value)
            {
                if (!d.Name.Equals(Constants.xpathStepperID)) continue;
                stepperID = d.Value.ToString();
            }

            Dictionary<string, EcellData> list = this.GetSystemProperty();
            List<EcellData> data = new List<EcellData>();
            foreach (EcellData d in list.Values)
            {
                if (d.Name.Equals(Constants.xpathStepperID))
                {
                    d.Value = new EcellValue(stepperID);
                }
                data.Add(d);
            }
            EcellObject obj = EcellObject.CreateObject(modelID, tmpID,
                Constants.xpathSystem, Constants.xpathSystem, data);
            return obj;
        }

        /// <summary>
        /// Get the temporary id in projects.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="type">object type.</param>
        /// <param name="systemID">ID of parent system.</param>
        /// <returns>the temporary id.</returns>
        public string GetTemporaryID(string modelID, string type, string systemID)
        {
            return m_currentProject.GetTemporaryID(modelID, type, systemID);
        }

        /// <summary>
        /// Get a copied key in project.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="type">object type.</param>
        /// <param name="key">ID of parent system.</param>
        /// <returns>the copied id.</returns>
        public string GetCopiedID(string modelID, string type, string key)
        {
            return m_currentProject.GetCopiedID(modelID, type, key);
        }

        /// <summary>
        /// Returns the list of the "System" property. 
        /// </summary>
        /// <returns>The dictionary of the "System" property</returns>
        public Dictionary<string, EcellData> GetSystemProperty()
        {
            Dictionary<string, EcellData> dic = new Dictionary<string, EcellData>();
            WrappedSimulator sim = m_currentProject.CreateSimulatorInstance();
            BuildDefaultSimulator(sim, null, null);
            ArrayList list = new ArrayList();
            list.Clear();
            list.Add("");
            sim.LoadEntityProperty(
                Constants.xpathSystem + Constants.delimiterColon +
                Constants.delimiterColon +
                Constants.delimiterPath + Constants.delimiterColon +
                Constants.xpathName,
                list
                );
            EcellObject dummyEcellObject = EcellObject.CreateObject(
                "",
                Constants.delimiterPath,
                EcellObject.SYSTEM,
                EcellObject.SYSTEM,
                null);
            DataStorer.DataStored4System(
                sim,
                dummyEcellObject,
                new Dictionary<string, double>());
            SetPropertyList(dummyEcellObject, dic);
            return dic;
        }

        /// <summary>
        /// Returns the list of the "Variable" property. 
        /// </summary>
        /// <returns>The dictionary of the "Variable" property</returns>
        public Dictionary<string, EcellData> GetVariableProperty()
        {
            Dictionary<string, EcellData> dic = new Dictionary<string, EcellData>();
            WrappedSimulator simulator = null;
            EcellObject dummyEcellObject = null;
            try
            {
                simulator = m_currentProject.CreateSimulatorInstance();
                BuildDefaultSimulator(simulator, null, null);
                dummyEcellObject = EcellObject.CreateObject(
                    "",
                    Constants.delimiterPath + Constants.delimiterColon + Constants.xpathSize.ToUpper(),
                    EcellObject.VARIABLE,
                    EcellObject.VARIABLE,
                    null
                    );
                DataStorer.DataStored4Variable(
                        simulator,
                        dummyEcellObject,
                        new Dictionary<string, double>());
                SetPropertyList(dummyEcellObject, dic);
            }
            finally
            {
                simulator = null;
                dummyEcellObject = null;
            }
            return dic;
        }

        /// <summary>
        /// Returns the list of the "Process" property. 
        /// </summary>
        /// <param name="dmName">The DM name</param>
        /// <returns>The dictionary of the "Process" property</returns>
        public Dictionary<string, EcellData> GetProcessProperty(string dmName)
        {
            Dictionary<string, EcellData> dic = new Dictionary<string, EcellData>();
            try
            {
                string key = Constants.delimiterPath + Constants.delimiterColon + "tmp";
                WrappedSimulator sim = m_currentProject.CreateSimulatorInstance();
                sim.CreateStepper("DAEStepper", "temporaryStepper");
                sim.SetEntityProperty("System::/:StepperID", "temporaryStepper");
                sim.CreateEntity(dmName,
                    Constants.xpathProcess + Constants.delimiterColon + key);
                EcellObject dummyEcellObject = EcellObject.CreateObject("", key, EcellObject.PROCESS, dmName, null);
                DataStorer.DataStored4Process(
                        sim,
                        m_env,
                        dummyEcellObject,
                        new Dictionary<string, double>());
                SetPropertyList(dummyEcellObject, dic);
            }
            catch (Exception ex)
            {
                throw new EcellException(
                    String.Format(MessageResources.ErrGetProp,
                    new object[] { dmName }), ex);
            }
            return dic;
        }

        /// <summary>
        /// Returns the list of the "Stepper" property. 
        /// </summary>
        /// <param name="dmName">The DM name</param>
        /// <returns>The dictionary of the "Stepper" property</returns>
        public List<EcellData> GetStepperProperty(string dmName)
        {
            List<EcellData> list;
            EcellObject dummyEcellObject = null;
            try
            {
                WrappedSimulator sim = m_currentProject.CreateSimulatorInstance();
                sim.CreateStepper(dmName, Constants.textKey);
                dummyEcellObject = EcellObject.CreateObject("", Constants.textKey, EcellObject.STEPPER, dmName, null);
                DataStorer.DataStored4Stepper(sim, m_env, dummyEcellObject);
                list = dummyEcellObject.Value;
            }
            finally
            {
                dummyEcellObject = null;
            }
            return list;
        }

        /// <summary>
        /// Sets the property list.
        /// </summary>
        /// <param name="ecellObject">The "EcellObject"</param>
        /// <param name="dic">The dictionary of "EcellData"</param>
        private static void SetPropertyList(EcellObject ecellObject, Dictionary<string, EcellData> dic)
        {
            if (ecellObject.Value == null || ecellObject.Value.Count <= 0)
            {
                return;
            }
            foreach (EcellData ecellData in ecellObject.Value)
            {
                if (ecellData.Name.Equals(EcellProcess.VARIABLEREFERENCELIST))
                {
                    ecellData.Value = new EcellValue(new List<object>());
                }
                dic[ecellData.Name] = ecellData;
            }
        }

        /// <summary>
        /// Creates the dummy simulator 4 property lists.
        /// </summary>
        /// <param name="simulator">The dummy simulator</param>
        /// <param name="defaultProcess">The dm name of "Process"</param>
        /// <param name="defaultStepper">The dm name of "Stepper"</param>
        private static void BuildDefaultSimulator(
                WrappedSimulator simulator, string defaultProcess, string defaultStepper)
        {
            try
            {
                // Set DefaultProcess if null
                if (defaultProcess == null)
                    defaultProcess = Constants.DefaultProcessName;
                // Set DefaultStepper if null
                if (defaultStepper == null)
                    defaultStepper = Constants.DefaultStepperName;

                //
                simulator.CreateStepper(defaultStepper, Constants.textKey);
                simulator.CreateEntity(
                    Constants.xpathVariable,
                    Constants.xpathVariable + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper()
                    );
                simulator.CreateEntity(
                    defaultProcess,
                    Constants.xpathProcess + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper()
                );
                simulator.LoadEntityProperty(
                    Constants.xpathSystem +  "::/:" + Constants.xpathStepperID,
                    new string[] { Constants.textKey }
                );
                simulator.LoadEntityProperty(
                    Util.BuildFullPN(
                        Constants.xpathVariable,
                        Constants.delimiterPath,
                        Constants.xpathSize.ToUpper(),
                        Constants.xpathValue
                    ),
                    new string[] { "0.1" }
                );
                simulator.Initialize();
            }
            catch (Exception ex)
            {
                throw new EcellException(
                    MessageResources.ErrCombiStepProc, ex);
            }
        }

        #endregion

        #region Method for Get EcellObject
        /// <summary>
        /// Returns the list of a "EcellObject" from "EcellCoreLib" using a model ID and a key .
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key</param>
        /// <returns>The list of a "EcellObject"</returns>
        public List<EcellObject> GetData(string modelID, string key)
        {
            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
            List<EcellObject> ecellObjectList = new List<EcellObject>();
            try
            {
                // Returns all stored "EcellObject".
                if (string.IsNullOrEmpty(modelID))
                {
                    // Searches the model.
                    foreach (EcellObject model in m_currentProject.ModelList)
                        ecellObjectList.Add(model);
                    // Searches the "System".
                    m_currentProject.SortSystems();
                    foreach (List<EcellObject> systemList in sysDic.Values)
                        ecellObjectList.AddRange(systemList);
                }
                // Searches the model.
                else if (string.IsNullOrEmpty(key))
                {
                    foreach (EcellObject model in m_currentProject.ModelList)
                    {
                        if (!model.ModelID.Equals(modelID))
                            continue;
                        ecellObjectList.Add(model.Clone());
                        break;
                    }
                    ecellObjectList.AddRange(sysDic[modelID]);
                }
                // Searches the "System".
                else
                {
                    foreach (EcellObject system in sysDic[modelID])
                    {
                        if (!key.Equals(system.Key))
                            continue;
                        ecellObjectList.Add(system.Clone());
                        break;
                    }
                }
                return ecellObjectList;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// Get EcellObject from DataManager.
        /// </summary>
        /// <param name="modelId">the modelId of EcellObject.</param>
        /// <param name="key">the key of EcellObject.</param>
        /// <param name="type">the type of EcellObject.</param>
        /// <returns>EcellObject</returns>
        public EcellObject GetEcellObject(string modelId, string key, string type)
        {
            EcellObject obj = m_currentProject.GetEcellObject(modelId, type, key);
            if (obj == null)
                return obj;
            else
                return obj.Clone();
        }

        /// <summary>
        /// Is data exists in the system or not.
        /// data is identified by modelID, key and type.
        /// </summary>
        /// <param name="modelID">modelID of deleted system.</param>
        /// <param name="key">key of deleted system.</param>
        /// <param name="type">type of deleted system.</param>
        /// <returns>true if the key exists; false otherwise</returns>
        internal bool IsDataExists(string modelID, string key, string type)
        {
            EcellObject obj = GetEcellObject(modelID, key, type);
            return (obj != null);
        }

        /// <summary>
        /// get model list loaded by this system now.
        /// </summary>
        /// <returns></returns>
        public List<string> GetModelList()
        {
            List<EcellModel> objList = m_currentProject.ModelList;
            List<string> list = new List<string>();

            if (objList != null)
            {
                foreach (EcellObject obj in objList)
                {
                    list.Add(obj.ModelID);
                }
            }

            return list;
        }

        /// <summary>
        /// Get the list of system in model.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <returns>the list of system.</returns>
        public List<string> GetSystemList(string modelID)
        {
            List<string> systemList = new List<string>();
            foreach (EcellObject system in m_currentProject.SystemDic[modelID])
            {
                systemList.Add(system.Key);
            }
            return systemList;
        }

        /// <summary>
        /// Returns the entity name list of the model.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="entityName">The entity name</param>
        /// <returns></returns>
        public List<string> GetEntityList(string modelID, string entityName)
        {
            List<string> entityList = new List<string>();
            try
            {
                foreach (EcellObject system in m_currentProject.SystemDic[modelID])
                {
                    if (entityName.Equals(Constants.xpathSystem))
                    {
                        string parentPath = system.ParentSystemID;
                        string childPath = system.LocalID;
                        entityList.Add(
                            Constants.xpathSystem + Constants.delimiterColon
                            + parentPath + Constants.delimiterColon + childPath);
                    }
                    else
                    {
                        if (system.Children == null || system.Children.Count <= 0)
                            continue;
                        foreach (EcellObject entity in system.Children)
                        {
                            if (!entity.Type.Equals(entityName))
                                continue;
                            entityList.Add(entity.Type + Constants.delimiterColon + entity.Key);
                        }
                    }
                }
                return entityList;
            }
            catch (Exception ex)
            {
                entityList.Clear();
                entityList = null;
                throw new EcellException(String.Format(MessageResources.ErrFindEnt,
                    new object[] { entityName }), ex);
            }
        }

        #endregion

        #region Method for Stepper
        /// <summary>
        /// Adds the new "Stepper"
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="stepper">The "Stepper"</param>
        public void AddStepperID(string parameterID, EcellObject stepper)
        {
            AddStepperID(parameterID, stepper, true);
        }

        /// <summary>
        /// Adds the new "Stepper"
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="stepper">The "Stepper"</param>
        /// <param name="isRecorded">Whether this action is recorded</param>
        public void AddStepperID(string parameterID, EcellObject stepper, bool isRecorded)
        {
            string message = null;
            Dictionary<string, List<EcellObject>> stepperDic = null;
            try
            {
                // Get stepperDic
                message = "[" + parameterID + "][" + stepper.ModelID + "][" + stepper.Key + "]";
                if (stepper == null || string.IsNullOrEmpty(parameterID) || string.IsNullOrEmpty(stepper.ModelID))
                    throw new EcellException();
                if (!m_currentProject.StepperDic.ContainsKey(parameterID))
                    m_currentProject.StepperDic[parameterID] = new Dictionary<string, List<EcellObject>>();
                stepperDic = m_currentProject.StepperDic[parameterID];
                if (!stepperDic.ContainsKey(stepper.ModelID))
                    throw new EcellException();

                // Check duplication.
                foreach (EcellObject storedStepper in stepperDic[stepper.ModelID])
                {
                    if (!stepper.Key.Equals(storedStepper.Key))
                        continue;
                    throw new EcellException(
                        string.Format(MessageResources.ErrExistStepper,
                            new object[] { stepper.Key }
                        )
                    );
                }
                // Set Stteper.
                stepperDic[stepper.ModelID].Add(stepper);
                if (m_currentProject.Info.SimulationParam.Equals(parameterID))
                {
                    List<EcellObject> stepperList = new List<EcellObject>();
                    stepperList.Add(stepper);
                    m_env.PluginManager.DataAdd(stepperList);
                }
                MessageCreateEntity(Constants.xpathStepper, message);
                if (isRecorded)
                    m_env.ActionManager.AddAction(new AddStepperAction(parameterID, stepper));
            }
            catch (Exception ex)
            {
                message = String.Format(MessageResources.ErrNotCreStepper,
                    new object[] { stepper.Key });
                Trace.WriteLine(message);
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Updates the "Stepper".
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="stepperList">The list of the "Stepper"</param>
        public void UpdateStepperID(string parameterID, List<EcellObject> stepperList)
        {
            UpdateStepperID(parameterID, stepperList, true);
        }

        /// <summary>
        /// Updates the "Stepper".
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="stepperList">The list of the "Stepper"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        public void UpdateStepperID(string parameterID, List<EcellObject> stepperList, bool isRecorded)
        {
            if (stepperList.Count == 0)
                return;

            string message = null;

            try
            {
                List<EcellObject> addedStepperList = new List<EcellObject>();
                List<EcellObject> removedStepperList = new List<EcellObject>();
                List<EcellObject> updatedStepperList = new List<EcellObject>();
                List<EcellObject> oldStepperList = new List<EcellObject>();
                Dictionary<string, List<EcellObject>> perParameterStepperListDic = m_currentProject.StepperDic[parameterID];
                foreach (EcellObject model in m_currentProject.ModelList)
                {
                    List<EcellObject> remainingStepperList = new List<EcellObject>();
                    foreach (EcellObject stepper in perParameterStepperListDic[model.ModelID])
                    {
                        bool removed = true;
                        oldStepperList.Add(stepper.Clone());
                        foreach (EcellObject newStepper in stepperList)
                        {
                            if (stepper.Key == newStepper.Key)
                            {
                                remainingStepperList.Add(stepper);
                                removed = false;
                                break;
                            }
                        }
                        if (removed)
                            removedStepperList.Add(stepper);
                    }
                    perParameterStepperListDic[model.ModelID] = remainingStepperList;
                }
                foreach (EcellObject stepper in stepperList)
                {
                    message = stepper.Key;

                    List<EcellObject> perModelStepperList = perParameterStepperListDic[stepper.ModelID];
                    bool added = true;
                    foreach (EcellObject oldStepper in perModelStepperList.ToArray())
                    {
                        if (oldStepper.Key.Equals(stepper.Key))
                        {
                            added = false;
                            perModelStepperList.Remove(oldStepper);
                            perModelStepperList.Add(stepper);
                            updatedStepperList.Add(stepper);
                            break;
                        }
                    }
                    if (added)
                    {
                        addedStepperList.Add(stepper);
                        perModelStepperList.Add(stepper);
                    }
                    if (isRecorded && addedStepperList.Count + removedStepperList.Count + updatedStepperList.Count > 0)
                        m_env.ActionManager.AddAction(new UpdateStepperAction(parameterID, stepperList, oldStepperList));
                }
            }
            catch (Exception ex)
            {
                throw new EcellException(MessageResources.ErrSetSimParam, ex);
            }
        }

        /// <summary>
        /// Deletes the "Stepper".
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="stepper">The "Stepper"</param>
        public void DeleteStepperID(string parameterID, EcellObject stepper)
        {
            DeleteStepperID(parameterID, stepper, true);
        }

        /// <summary>
        /// Deletes the "Stepper".
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="stepper">The "Stepper"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        public void DeleteStepperID(string parameterID, EcellObject stepper, bool isRecorded)
        {
            try
            {
                int point = -1;
                List<EcellObject> storedStepperList
                    = m_currentProject.StepperDic[parameterID][stepper.ModelID];
                for (int i = 0; i < storedStepperList.Count; i++)
                {
                    if (storedStepperList[i].Key.Equals(stepper.Key))
                    {
                        point = i;
                        break;
                    }
                }
                if (point != -1)
                {
                    storedStepperList.RemoveAt(point);
                    Trace.WriteLine(String.Format(MessageResources.InfoDel,
                        new object[] { stepper.Type, stepper.Key }));
                }
                if (isRecorded)
                    m_env.ActionManager.AddAction(new DeleteStepperAction(parameterID, stepper));
                if (m_currentProject.Info.SimulationParam.Equals(parameterID))
                {
                    m_env.PluginManager.DataDelete(stepper.ModelID, stepper.Key, stepper.Type);
                }
            }
            catch (Exception ex)
            {
                String errmes = String.Format(MessageResources.ErrDelete,
                    new object[] { stepper.Key });
                Trace.WriteLine(errmes);
                throw new EcellException(errmes, ex);
            }
        }

        /// <summary>
        /// Returns the list of the "Stepper" with the parameter ID.
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="modelID"> model ID</param>
        /// <returns>The list of the "Stepper"</returns>
        public List<EcellObject> GetStepper(string parameterID, string modelID)
        {
            List<EcellObject> returnedStepper = new List<EcellObject>();
            Debug.Assert(!string.IsNullOrEmpty(modelID));
            if (string.IsNullOrEmpty(parameterID))
                parameterID = m_currentProject.Info.SimulationParam;
            if (string.IsNullOrEmpty(parameterID))
                throw new EcellException(String.Format(MessageResources.ErrNoSet,
                    new object[] { MessageResources.NameSimParam }));

            List<EcellObject> tempList = m_currentProject.StepperDic[parameterID][modelID];
            foreach (EcellObject stepper in tempList)
            {
                // DataStored4Stepper(simulator, stepper);
                returnedStepper.Add(stepper.Clone());
            }
            return returnedStepper;
        }

        #endregion

        #region Method for ObservedData
        /// <summary>
        /// Check whether this key is included in observed data.
        /// </summary>
        /// <param name="key">the key of data.</param>
        /// <returns>if this key is included, return true.</returns>
        public bool IsContainsObservedData(string key)
        {
            return m_observedList.ContainsKey(key);
        }

        /// <summary>
        /// The event sequence when the user set and change the observed data.
        /// </summary>
        /// <param name="data">The observed data.</param>
        public void SetObservedData(EcellObservedData data)
        {
            if (m_observedList.ContainsKey(data.Key))
            {
                m_observedList[data.Key] = data;
            }
            else
                m_observedList.Add(data.Key, data);
            m_env.PluginManager.SetObservedData(data);
        }

        /// <summary>
        /// Get the observed data from the key, if the observed data does not exist, return null.
        /// </summary>
        /// <param name="key">the key of observed data.</param>
        /// <returns>the observed data.</returns>
        public EcellObservedData GetObservedData(string key)
        {
            if (m_observedList.ContainsKey(key))
                return m_observedList[key];
            return null;
        }

        /// <summary>
        /// Get the list of all observed data.
        /// </summary>
        /// <returns>the list of observed data.</returns>
        public List<EcellObservedData> GetObservedData()
        {
            List<EcellObservedData> resList = new List<EcellObservedData>();
            foreach (string key in m_observedList.Keys)
                resList.Add(m_observedList[key]);
            return resList;
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public void RemoveObservedData(EcellObservedData data)
        {
            if (m_observedList.ContainsKey(data.Key))
                m_observedList.Remove(data.Key);
            m_env.PluginManager.RemoveObservedData(data);
        }
        #endregion

        #region Method for ParameterData
        /// <summary>
        /// Check whether this key is included in parameter data.
        /// </summary>
        /// <param name="key">the key of data.</param>
        /// <returns>if this key is included, return true.</returns>
        public bool IsContainsParameterData(string key)
        {
            return m_parameterList.ContainsKey(key);
        }

        /// <summary>
        /// The event sequence when the user set and change the prameter data.
        /// </summary>
        /// <param name="data">The observed data.</param>
        public void SetParameterData(EcellParameterData data)
        {
            if (m_parameterList.ContainsKey(data.Key))
            {
                m_parameterList[data.Key] = data;
            }
            else
                m_parameterList.Add(data.Key, data);
            m_env.PluginManager.SetParameterData(data);
        }

        /// <summary>
        /// Get the parameter data from the key. if the parameter data does not exist, return null.
        /// </summary>
        /// <param name="key">the key of parameter data.</param>
        /// <returns>the parameter data.</returns>
        public EcellParameterData GetParameterData(string key)
        {
            if (m_parameterList.ContainsKey(key))
                return m_parameterList[key];
            return null;
        }

        /// <summary>
        /// Get the list of all parameter data.
        /// </summary>
        /// <returns>the list of parameter data.</returns>
        public List<EcellParameterData> GetParameterData()
        {
            List<EcellParameterData> resList = new List<EcellParameterData>();
            foreach (string key in m_parameterList.Keys)
                resList.Add(m_parameterList[key]);
            return resList;
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            if (m_parameterList.ContainsKey(data.Key))
                m_parameterList.Remove(data.Key);
            m_env.PluginManager.RemoveParameterData(data);
        }

        #endregion

        #region Method for Simulation Execution
        /// <summary>
        /// Start simulation.
        /// </summary>
        /// <param name="time"></param>
        public void StartSimulation(double time)
        {
            if (m_isTimeStepping && m_remainTime > 0.0)
            {
                StartStepSimulation(m_remainTime);
                return;
            }
            else if (m_isStepStepping && m_remainStep > 0)
            {
                StartStepSimulation(m_remainStep);
                return;
            }

            try
            {
                int i = 0;
                if (m_currentProject.SimulationStatus != SimulationStatus.Suspended)
                {
                    m_currentProject.SimulationStatus = SimulationStatus.Run;
                    this.Initialize(true);
                    m_env.LogManager.Append(new ApplicationLogEntry(
                            MessageType.Information,
                            MessageResources.SimulationStarted,
                            this));
                }
                else
                {
                    m_currentProject.SimulationStatus = SimulationStatus.Run;
                    m_env.LogManager.Append(new ApplicationLogEntry(
                            MessageType.Information,
                            MessageResources.SimulationRestarted,
                            this));
                }
                while (m_currentProject.SimulationStatus == SimulationStatus.Run)
                {
                    if (i == 1000)
                    {
                        Thread.Sleep(1);
                        i = 0;
                    }
                    m_currentProject.Simulator.Step(m_defaultStepCount);
                    Application.DoEvents();
                    double currentTime = m_currentProject.Simulator.GetCurrentTime();
                    this.m_env.PluginManager.AdvancedTime(currentTime);
                }
            }
            catch (WrappedException ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new SimulationException(MessageResources.ErrRunSim, ex);
            }
            catch (Exception)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new SimulationException(MessageResources.ErrSuspend, new WrappedException());
            }

        }

        /// <summary>
        /// Step Simulation with time.
        /// </summary>
        /// <param name="time"></param>
        public void StartStepSimulation(double time)
        {
            try
            {
                if (m_currentProject.SimulationStatus != SimulationStatus.Suspended)
                {
                    this.Initialize(true);
                }
                double cTime = m_currentProject.Simulator.GetCurrentTime();
                double stoppedTime;
                if (m_isTimeStepping && m_remainTime > 0.0)
                {

                }
                else
                {
                    m_isTimeStepping = true;
                    m_remainTime = time;
                }
                stoppedTime = cTime + m_remainTime;

                m_currentProject.SimulationStatus = SimulationStatus.Run;
                while (m_currentProject.SimulationStatus == SimulationStatus.Run)
                {
                    lock (m_currentProject.Simulator)
                    {
                        if (m_remainTime < m_defaultTime)
                        {
                            m_currentProject.Simulator.Run(m_remainTime);
                            // 一時停止したときに同じ時間が再度実行される
                            if (cTime != m_currentProject.Simulator.GetCurrentTime())
                                this.m_remainTime = 0.0;
                        }
                        else
                        {
                            m_currentProject.Simulator.Run(m_defaultTime);
                            // 一時停止したときに同じ時間が再度実行される
                            if (cTime != m_currentProject.Simulator.GetCurrentTime())
                                this.m_remainTime = this.m_remainTime - m_defaultTime;
                        }
                    }
                    Application.DoEvents();
                    double currentTime = m_currentProject.Simulator.GetCurrentTime();
                    this.m_env.PluginManager.AdvancedTime(currentTime);
                    if (m_remainTime == 0.0)
                    {
                        m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                        m_isTimeStepping = false;
                        break;
                    }
                }
                if (m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    double aTime = m_currentProject.Simulator.GetCurrentTime();
                    m_remainTime = stoppedTime - aTime;
                }
            }
            catch (WrappedException ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new SimulationException(MessageResources.ErrRunSim, ex);
            }
            catch (Exception)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new SimulationException(MessageResources.ErrSuspend, new WrappedException());
            }
        }

        /// <summary>
        /// Step simulation with step.
        /// </summary>
        /// <param name="step"></param>
        public void StartStepSimulation(int step)
        {
            try
            {
                if (m_currentProject.SimulationStatus != SimulationStatus.Suspended)
                {
                    this.Initialize(true);
                }
                double cTime = m_currentProject.Simulator.GetCurrentTime();
                double stoppedTime;
                if (m_isStepStepping && m_remainStep > 0)
                {
                    // nothing.
                }
                else
                {
                    m_isStepStepping = true;
                    m_remainStep = step;
                }
                stoppedTime = cTime + m_remainTime;

                m_currentProject.SimulationStatus = SimulationStatus.Run;
                while (m_currentProject.SimulationStatus == SimulationStatus.Run)
                {
                    lock (m_currentProject.Simulator)
                    {
                        if (m_remainStep < m_defaultStepCount)
                        {
                            m_currentProject.Simulator.Step(m_remainStep);
                            this.m_remainStep = 0;
                        }
                        else
                        {
                            m_currentProject.Simulator.Step(m_defaultStepCount);
                            this.m_remainStep = this.m_remainStep - m_defaultStepCount;
                        }
                    }
                    Application.DoEvents();
                    double currentTime = m_currentProject.Simulator.GetCurrentTime();
                    this.m_env.PluginManager.AdvancedTime(currentTime);
                    if (m_remainStep == 0)
                    {
                        m_isStepStepping = false;
                        m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                        break;
                    }
                }
            }
            catch (WrappedException ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new SimulationException(MessageResources.ErrRunSim, ex);
            }
            catch (Exception)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new SimulationException(MessageResources.ErrSuspend, new WrappedException());
            }
        }

        /// <summary>
        /// Suspends the simulation.
        /// </summary>
        public void SimulationSuspend()
        {
            try
            {
                //m_currentProject.Simulator.Suspend();
                m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                m_env.LogManager.Append(new ApplicationLogEntry(
                    MessageType.Information,
                    String.Format(MessageResources.InfoSuspend, m_currentProject.Simulator.GetCurrentTime()),
                    this));
                m_env.Console.WriteLine(String.Format(MessageResources.InfoSuspend, m_currentProject.Simulator.GetCurrentTime()));
                m_env.Console.Flush();
            }
            catch (WrappedException ex)
            {
                throw new SimulationException(MessageResources.ErrSuspendSim, ex);
            }
        }

        /// <summary>
        /// Stops this simulation.
        /// </summary>
        public void SimulationStop()
        {
            try
            {
                Debug.Assert(m_currentProject.Simulator != null);

                lock (m_currentProject.Simulator)
                {
                    m_currentProject.Simulator.Stop();
                }
                m_env.LogManager.Append(new ApplicationLogEntry(
                    MessageType.Information,
                    String.Format(MessageResources.InfoResetSim, m_currentProject.Simulator.GetCurrentTime()),
                    this));
                m_env.Console.WriteLine(String.Format(MessageResources.InfoResetSim, m_currentProject.Simulator.GetCurrentTime()));
                m_env.Console.Flush();

                m_isTimeStepping = false;
                m_isStepStepping = false;
                m_remainTime = 0.0;
                m_remainStep = 0;
            }
            catch (WrappedException ex)
            {
                throw new SimulationException(MessageResources.ErrResetSim, ex);
            }
            finally
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
            }
        }

        /// <summary>
        /// Returns the current simulation time.
        /// </summary>
        /// <returns>The current simulation time</returns>
        public double GetCurrentSimulationTime()
        {
            if (m_currentProject.Simulator != null)
            {
                return m_currentProject.Simulator.GetCurrentTime();
            }
            else
            {
                return double.NaN;
            }
        }
        #endregion

        #region Method for SimulationLog
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="interval"></param>
        /// <param name="fullID"></param>
        /// <returns></returns>
        public LogData GetLogData(double startTime, double endTime, double interval, string fullID)
        {
            try
            {
                // Initialize
                if (m_currentProject.Simulator == null)
                    return null;
                if (m_currentProject.LogableEntityPathDic == null ||
                    m_currentProject.LogableEntityPathDic.Count == 0)
                    return null;
                // GetLogData
                return this.GetUniqueLogData(startTime, endTime, interval, fullID);
            }
            catch (Exception ex)
            {
                throw new EcellException(MessageResources.ErrGetLogData, ex);
            }
        }

        /// <summary>
        /// Returns the list of the "LogData".
        /// </summary>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="interval">The interval</param>
        /// <returns>The list of the "LogData"</returns>
        public List<LogData> GetLogData(double startTime, double endTime, double interval)
        {
            List<LogData> logDataList = new List<LogData>();
            try
            {
                // Initialize
                if (m_currentProject.Simulator == null)
                    return null;
                if (m_currentProject.LogableEntityPathDic == null ||
                    m_currentProject.LogableEntityPathDic.Count == 0)
                    return null;

                IList<string> loggerList = m_currentProject.Simulator.GetLoggerList();
                foreach (string logger in loggerList)
                {
                    logDataList.Add(
                            this.GetUniqueLogData(startTime, endTime, interval, logger));
                }
                return logDataList;
            }
            catch (Exception ex)
            {
                logDataList = null;
                throw new EcellException(MessageResources.ErrGetLogData, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="interval"></param>
        /// <param name="fullID"></param>
        /// <returns></returns>
        private LogData GetUniqueLogData(
                double startTime,
                double endTime,
                double interval,
                string fullID)
        {
            if (startTime < 0.0)
            {
                startTime = 0.0;
            }
            if (endTime <= 0.0)
            {
                endTime = m_currentProject.Simulator.GetCurrentTime();
            }
            if (this.m_simulationTimeLimit > 0.0 && endTime > this.m_simulationTimeLimit)
            {
                endTime = this.m_simulationTimeLimit;
            }
            if (startTime > endTime)
            {
                double tmpTime = startTime;
                startTime = endTime;
                endTime = tmpTime;
            }
            WrappedDataPointVector dataPointVector = null;
            if (interval <= 0.0)
            {
                lock (m_currentProject.Simulator)
                {
                    dataPointVector
                        = m_currentProject.Simulator.GetLoggerData(
                            fullID,
                            startTime,
                            endTime
                            );
                }
            }
            else
            {
                lock (m_currentProject.Simulator)
                {
                    dataPointVector
                        = m_currentProject.Simulator.GetLoggerData(
                            fullID,
                            startTime,
                            endTime,
                            interval
                            );
                }
            }
            List<LogValue> logValueList = new List<LogValue>();
            double lastTime = -1.0;
            for (int i = 0; i < dataPointVector.GetArraySize(); i++)
            {
                if (lastTime == dataPointVector.GetTime(i))
                {
                    continue;
                }
                LogValue logValue = new LogValue(
                    dataPointVector.GetTime(i),
                    dataPointVector.GetValue(i),
                    dataPointVector.GetAvg(i),
                    dataPointVector.GetMin(i),
                    dataPointVector.GetMax(i)
                    );
                logValueList.Add(logValue);
                lastTime = dataPointVector.GetTime(i);
            }
            string modelID = null;
            if (m_currentProject.LogableEntityPathDic.ContainsKey(fullID))
            {
                modelID = m_currentProject.LogableEntityPathDic[fullID];
            }
            string key = null;
            string type = null;
            string propName = null;
            Util.ParseFullPN(fullID, out type, out key, out propName);
            if (logValueList.Count == 1 && logValueList[0].time == 0.0)
            {
                LogValue logValue =
                    new LogValue(
                       endTime,
                       logValueList[0].value,
                       logValueList[0].avg,
                       logValueList[0].min,
                       logValueList[0].max
                       );
                logValueList.Add(logValue);
            }
            LogData logData = new LogData(
                modelID,
                key,
                type,
                propName,
                logValueList
                );
            dataPointVector.Dispose();
            return logData;
        }

        /// <summary>
        /// Returns the list of the registred logger.
        /// </summary>
        /// <returns></returns>
        public List<string> GetLoggerList()
        {
            List<string> loggerList = new List<string>();
            try
            {
                IList<string> polymorphList = m_currentProject.Simulator.GetLoggerList();
                foreach (string polymorph in polymorphList)
                {
                    loggerList.Add(polymorph);
                }
                return loggerList;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// Saves the simulation result.
        /// </summary>
        public void SaveSimulationResult()
        {
            try
            {
                SaveSimulationResult(null, 0.0, GetCurrentSimulationTime(), null, GetLoggerList());
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrSavePrj,
                    new object[] { m_currentProject.Info.Name }), ex);
            }
        }

        /// <summary>
        /// Saves the simulation result.
        /// </summary>
        /// <param name="savedDirName">The saved directory name</param>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="savedType">The saved type (ECD or Binary)</param>
        /// <param name="fullIDList">The list of the saved fullID</param>
        public void SaveSimulationResult(
            string savedDirName,
            double startTime,
            double endTime,
            string savedType,
            List<string> fullIDList
            )
        {
            string errMsg = "";
            if (fullIDList == null || fullIDList.Count <= 0)
                return;

            string message = null;
            try
            {
                if (endTime == 0.0) endTime = m_currentProject.Simulator.GetCurrentTime();
                string projectID = m_currentProject.Info.Name;
                string simParam = m_currentProject.Info.SimulationParam;
                message = "[" + projectID + "][" + simParam + "]";

                SaveType saveFileType = SaveType.ECD;
                if (savedType.Equals("csv") || savedType.Equals("CSV"))
                    saveFileType = SaveType.CSV;

                //
                // Initializes.
                //
                string simulationDirName = null;
                if (!string.IsNullOrEmpty(savedDirName))
                {
                    simulationDirName = savedDirName;
                }
                else
                {
                    SetDefaultDir();

                    if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + projectID))
                    {
                        m_currentProject.Save();
                    }
                    simulationDirName = GetSimulationResultSaveDirectory();
                }
                if (!Directory.Exists(simulationDirName))
                {
                    Directory.CreateDirectory(simulationDirName);
                }

                double reloadInterval = m_currentProject.LoggerPolicy.ReloadInterval;
                double percent = 0.0;

                foreach (string key in fullIDList)
                {
                    try
                    {
                        double basePercent = percent;
                        double hitCount = 100.0 / (double)fullIDList.Count;
                        Ecd ecd = new Ecd();
                        double cTime = startTime;
                        while (cTime < endTime)
                        {
                            double nextTime = cTime + reloadInterval * 100000;
                            if (reloadInterval == 0.0) nextTime = endTime;
                            if (nextTime > endTime) nextTime = endTime;
                            LogData l = this.GetLogData(cTime, nextTime, reloadInterval, key);
                            if (cTime == startTime)
                            {
                                ecd.Create(simulationDirName, l, saveFileType,
                                cTime, nextTime);
                            }
                            else
                            {
                                ecd.Append(simulationDirName, l, saveFileType,
                                    cTime, nextTime);
                            }
                            m_env.ReportManager.SetProgress((int)(basePercent + hitCount * (nextTime / endTime)));
                            Application.DoEvents();
                            cTime = nextTime;
                        }
                        ecd.Close();
                        message = "[" + key + "]";
                        Trace.WriteLine("Save Simulation Result: " + message);
                    }
                    catch (Exception)
                    {
                        if (string.IsNullOrEmpty(errMsg))
                        {
                            errMsg = key;
                        }
                        else
                        {
                            errMsg = errMsg + "," + key;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(errMsg))
                {
                    throw new Exception(errMsg);
                }
            }
            catch (Exception ex)
            {
                message = MessageResources.ErrSaveLog;
                Trace.WriteLine(message);
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Load SimulationResult
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public LogData LoadSimulationResult(string fileName)
        {
            return Ecd.LoadSavedLogData(fileName);
        }

        /// <summary>
        /// Get Directory to Save SimulationResult.
        /// </summary>
        /// <returns></returns>
        public string GetSimulationResultSaveDirectory()
        {
            return this.m_defaultDir + "\\" +
                        m_currentProject.Info.Name + "\\" + Constants.xpathParameters +
                        "\\" + m_currentProject.Info.SimulationParam;
        }

        #endregion

        #region Method to Initialize Simulation
        /// <summary>
        /// Initialize the simulator before it starts.
        /// </summary>
        public void Initialize(bool flag)
        {
            string simParam = m_currentProject.Info.SimulationParam;
            Dictionary<string, List<EcellObject>> stepperList = m_currentProject.StepperDic[simParam];
            WrappedSimulator simulator = null;
            Dictionary<string, Dictionary<string, double>> initialCondition = m_currentProject.InitialCondition[simParam];

            m_currentProject.Simulator = m_currentProject.CreateSimulatorInstance();
            simulator = m_currentProject.Simulator;
            //
            // Loads steppers on the simulator.
            //
            List<EcellObject> newStepperList = new List<EcellObject>();
            List<string> modelIDList = new List<string>();
            Dictionary<string, Dictionary<string, object>> setStepperPropertyDic
                = new Dictionary<string, Dictionary<string, object>>();
            foreach (string modelID in stepperList.Keys)
            {
                newStepperList.AddRange(stepperList[modelID]);
                modelIDList.Add(modelID);
            }
            if (newStepperList.Count > 0)
                LoadStepper(
                simulator,
                newStepperList,
                setStepperPropertyDic);

            //
            // Loads systems on the simulator.
            //
            List<string> allLoggerList = new List<string>();
            List<EcellObject> systemList = new List<EcellObject>();
            m_currentProject.LogableEntityPathDic = new Dictionary<string, string>();
            Dictionary<string, object> setSystemPropertyDic = new Dictionary<string, object>();
            foreach (string modelID in modelIDList)
            {
                List<string> loggerList = new List<string>();
                if (flag)
                {
                    LoadSystem(
                        simulator,
                        m_currentProject.SystemDic[modelID],
                        loggerList,
                        initialCondition[modelID],
                        setSystemPropertyDic);
                }
                else
                {
                    LoadSystem(
                        simulator,
                        m_currentProject.SystemDic[modelID],
                        loggerList,
                        null,
                        setSystemPropertyDic);
                }
                foreach (string logger in loggerList)
                {
                    m_currentProject.LogableEntityPathDic[logger] = modelID;
                }
                allLoggerList.AddRange(loggerList);
            }

            //
            // Initializes
            //
            simulator.Initialize();
            //
            // Sets the "Settable" and "Not Savable" properties
            //
            foreach (string key in setStepperPropertyDic.Keys)
            {
                foreach (string path in setStepperPropertyDic[key].Keys)
                {
                    simulator.SetStepperProperty(key, path, setStepperPropertyDic[key][path]);
                }
            }
            foreach (string path in setSystemPropertyDic.Keys)
            {
                try
                {
                    EcellValue storedEcellValue = new EcellValue(simulator.GetEntityProperty(path));
                    EcellValue newEcellValue = new EcellValue(setSystemPropertyDic[path]);
                    if (storedEcellValue.Type.Equals(newEcellValue.Type)
                        && storedEcellValue.Value.Equals(newEcellValue.Value))
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    // do nothing
                }
                simulator.SetEntityProperty(path, setSystemPropertyDic[path]);
            }
            //
            // Set the initial condition property.
            //
            foreach (string modelID in modelIDList)
            {
                foreach (string fullPN in initialCondition[modelID].Keys)
                {
                    EcellValue storedValue = new EcellValue(simulator.GetEntityProperty(fullPN));
                    double initialValue = initialCondition[modelID][fullPN];
                    object newValue = null;
                    if (storedValue.IsInt)
                    {
                        int initialValueInt = Convert.ToInt32(initialValue);
                        if (storedValue.Value.Equals(initialValueInt))
                        {
                            continue;
                        }
                        newValue = initialValueInt;
                    }
                    else
                    {
                        if (storedValue.Value.Equals(initialValue))
                        {
                            continue;
                        }
                        newValue = initialValue;
                    }
                    simulator.SetEntityProperty(fullPN, newValue);
                }
            }
            //
            // Reinitializes
            //
            // this.m_simulatorDic[m_currentProject.Name].Initialize();
            //
            // Creates the "Logger" only after the initialization.
            //
            m_loggerEntry.Clear();
            if (allLoggerList != null && allLoggerList.Count > 0)
            {
                LoggerPolicy loggerPolicy = this.GetCurrentLoggerPolicy();
                foreach (string logger in allLoggerList)
                {
                    CreateLogger(logger, true, simulator, loggerPolicy);
                }
            }
        }

        /// <summary>
        /// Loads the "Stepper" 2 the "EcellCoreLib".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="stepperList">The list of the "Stepper"</param>
        /// <param name="setStepperDic"></param>
        private static void LoadStepper(
            WrappedSimulator simulator,
            List<EcellObject> stepperList,
            Dictionary<string, Dictionary<string, object>> setStepperDic)
        {
            Debug.Assert(stepperList != null && stepperList.Count > 0);

            foreach (EcellObject stepper in stepperList)
            {
                if (stepper == null)
                    continue;

                simulator.CreateStepper(stepper.Classname, stepper.Key);

                // 4 property
                if (stepper.Value == null || stepper.Value.Count <= 0)
                    continue;

                foreach (EcellData ecellData in stepper.Value)
                {
                    if (ecellData.Name == null || ecellData.Name.Length <= 0 || ecellData.Value == null)
                        continue;
                    else if (!ecellData.Value.IsDouble && !ecellData.Value.IsInt)
                        continue;

                    // 4 MaxStepInterval == Double.MaxValue
                    EcellValue value = ecellData.Value;
                    try
                    {
                        if ((double)value == Double.PositiveInfinity || (double)value == Double.MaxValue)
                            continue;
                        XmlConvert.ToDouble(value);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                        continue;
                    }

                    if (value.IsDouble
                        && (Double.IsInfinity((double)value) || Double.IsNaN((double)value)))
                        continue;

                    if (ecellData.Saveable)
                    {
                        simulator.LoadStepperProperty(
                            stepper.Key,
                            ecellData.Name,
                            value.Value);
                    }
                    else if (ecellData.Settable)
                    {
                        if (!setStepperDic.ContainsKey(stepper.Key))
                        {
                            setStepperDic[stepper.Key] = new Dictionary<string, object>();
                        }
                        setStepperDic[stepper.Key][ecellData.Name] = value.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Loads the "System" 2 the "ECellCoreLib".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="systemList">The list of "System"</param>
        /// <param name="loggerList">The list of the "Logger"</param>
        /// <param name="initialCondition">The dictionary of initial condition.</param>
        /// <param name="setPropertyDic">The dictionary of simulation library.</param>
        private static void LoadSystem(
            WrappedSimulator simulator,
            List<EcellObject> systemList,
            List<string> loggerList,
            Dictionary<string, double> initialCondition,
            Dictionary<string, object> setPropertyDic)
        {
            Debug.Assert(systemList != null && systemList.Count > 0);

            bool existSystem = false;
            Dictionary<string, object> processPropertyDic = new Dictionary<string, object>();

            foreach (EcellObject system in systemList)
            {
                if (system == null)
                    continue;

                existSystem = true;
                string parentPath = system.ParentSystemID;
                string childPath = system.LocalID;
                if (!system.Key.Equals(Constants.delimiterPath))
                {
                    simulator.CreateEntity(
                        system.Classname,
                        system.Classname + Constants.delimiterColon
                            + parentPath + Constants.delimiterColon + childPath);
                }
                // 4 property
                if (system.Value == null || system.Value.Count <= 0)
                    continue;

                foreach (EcellData ecellData in system.Value)
                {
                    if (ecellData.Name == null || ecellData.Name.Length <= 0
                        || ecellData.Value == null)
                    {
                        continue;
                    }
                    EcellValue value = ecellData.Value;
                    if (ecellData.Saveable)
                    {
                        simulator.LoadEntityProperty(
                            ecellData.EntityPath,
                            value.Value);
                    }
                    else if (ecellData.Settable)
                    {
                        setPropertyDic[ecellData.EntityPath] = value.Value;
                    }
                    if (ecellData.Logged)
                    {
                        loggerList.Add(ecellData.EntityPath);
                    }
                }
                // 4 children
                if (system.Children == null || system.Children.Count <= 0)
                    continue;
                LoadEntity(
                    simulator,
                    system.Children,
                    loggerList,
                    processPropertyDic,
                    initialCondition,
                    setPropertyDic);
            }
            if (processPropertyDic.Count > 0)
            {
                // The "VariableReferenceList" is previously loaded. 
                string[] keys = null;
                processPropertyDic.Keys.CopyTo(keys = new string[processPropertyDic.Keys.Count], 0);
                foreach (string entityPath in keys)
                {
                    if (entityPath.EndsWith(Constants.xpathVRL))
                    {
                        simulator.LoadEntityProperty(entityPath, processPropertyDic[entityPath]);
                        processPropertyDic.Remove(entityPath);
                    }
                }
                foreach (string entityPath in processPropertyDic.Keys)
                {
                    if (!entityPath.EndsWith("Fixed"))
                    {
                        simulator.LoadEntityProperty(entityPath, processPropertyDic[entityPath]);
                    }
                }
            }
            Debug.Assert(existSystem);
        }

        /// <summary>
        /// Loads the "Process" and the "Variable" to the "EcellCoreLib".
        /// </summary>
        /// <param name="entityList">The list of the "Process" and the "Variable"</param>
        /// <param name="simulator">The simulator</param>
        /// <param name="loggerList">The list of the "Logger"</param>
        /// <param name="processPropertyDic">The dictionary of the process property</param>
        /// <param name="initialCondition">The dictionary of the initial condition</param>
        /// <param name="setPropertyDic"></param>
        private static void LoadEntity(
            WrappedSimulator simulator,
            List<EcellObject> entityList,
            List<string> loggerList,
            Dictionary<string, object> processPropertyDic,
            Dictionary<string, double> initialCondition,
            Dictionary<string, object> setPropertyDic)
        {
            if (entityList == null || entityList.Count <= 0)
                return;

            foreach (EcellObject entity in entityList)
            {
                if (entity is EcellText)
                    continue;
                simulator.CreateEntity(
                    entity.Classname,
                    entity.Type + Constants.delimiterColon + entity.Key);
                if (entity.Value == null || entity.Value.Count <= 0)
                    continue;

                foreach (EcellData ecellData in entity.Value)
                {
                    EcellValue value = ecellData.Value;
                    if (string.IsNullOrEmpty(ecellData.Name)
                            || value == null
                            || (value.IsString && ((string)value).Length == 0))
                    {
                        continue;
                    }

                    if (ecellData.Logged)
                    {
                        loggerList.Add(ecellData.EntityPath);
                    }

                    if (value.IsDouble
                        && (Double.IsInfinity((double)value) || Double.IsNaN((double)value)))
                    {
                        continue;
                    }
                    if (ecellData.Saveable)
                    {
                        if (ecellData.EntityPath.EndsWith(Constants.xpathVRL))
                        {
                            processPropertyDic[ecellData.EntityPath] = value.Value;
                        }
                        else
                        {
                            if (ecellData.EntityPath.EndsWith("FluxDistributionList"))
                                continue;
                            simulator.LoadEntityProperty(
                                ecellData.EntityPath,
                                value.Value);
                        }
                    }
                    else if (ecellData.Settable)
                    {
                        setPropertyDic[ecellData.EntityPath] = value.Value;
                    }
                }
            }
        }

        #endregion

        #region Method for SimulationParameter
        /// <summary>
        /// Creates the new simulation parameter.
        /// </summary>
        /// <param name="parameterID">The new parameter ID</param>
        /// <returns>The new parameter</returns>
        public void CreateSimulationParameter(string parameterID)
        {
            CreateSimulationParameter(parameterID, true, true);
        }

        /// <summary>
        /// Creates the new simulation parameter.
        /// </summary>
        /// <param name="parameterID">The new parameter ID</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>        
        public void CreateSimulationParameter(string parameterID, bool isRecorded, bool isAnchor)
        {
            try
            {
                string message = null;

                message = "[" + parameterID + "]";
                //
                // 4 Stepper
                //
                if (m_currentProject.StepperDic.ContainsKey(parameterID))
                {
                    throw new EcellException(
                        String.Format(MessageResources.ErrExistObj,
                        new object[] { parameterID }));
                }

                m_currentProject.LoggerPolicyDic[parameterID] = new LoggerPolicy();

                Dictionary<string, List<EcellObject>> newStepperListSets = new Dictionary<string, List<EcellObject>>();
                Dictionary<string, Dictionary<string, double>> newInitialCondSets = new Dictionary<string, Dictionary<string, double>>();
                foreach (EcellObject model in m_currentProject.ModelList)
                {
                    newInitialCondSets[model.ModelID] = new Dictionary<string, double>();
                    newStepperListSets[model.ModelID] = new List<EcellObject>();
                }
                m_currentProject.StepperDic[parameterID] = newStepperListSets;
                m_currentProject.InitialCondition[parameterID] = newInitialCondSets;

                // Notify that a new parameter set is created.
                m_env.PluginManager.ParameterAdd(m_currentProject.Info.Name, parameterID);

                m_env.Console.WriteLine(String.Format(MessageResources.InfoCreSim, parameterID));
                m_env.Console.Flush();
                Trace.WriteLine(String.Format(MessageResources.InfoCreSim,
                    new object[] { parameterID }));
                //if (isRecorded)
                //    m_env.ActionManager.AddAction(new NewSimParamAction(parameterID, isAnchor));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                string message = String.Format(MessageResources.ErrCreSimParam,
                    new object[] { parameterID });
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Loads the simulation parameter.
        /// </summary>
        /// <param name="fileName">The simulation parameter file name</param>
        /// <returns></returns>
        public SimulationParameter LoadSimulationParameter(string fileName)
        {
            string message = null;
            SimulationParameter simParam = null;
            string projectID = m_currentProject.Info.Name;
            try
            {
                message = "[" + fileName + "]";
                // Initializes
                Debug.Assert(!string.IsNullOrEmpty(fileName));
                // Parses the simulation parameter.
                simParam = SimulationParameterReader.Parse(fileName, m_currentProject.Simulator);
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrLoadSimParam,
                    fileName), ex);
            }
            Trace.WriteLine("Load Simulation Parameter: " + message);
            return simParam;
        }

        /// <summary>
        /// Returns the current simulation parameter ID.
        /// </summary>
        /// <returns>The current simulation parameter ID</returns>
        public string GetCurrentSimulationParameterID()
        {
            return m_currentProject.Info.SimulationParam;
        }

        /// <summary>
        /// Returns the list of the parameter ID with the model ID.
        /// </summary>
        /// <returns>The list of parameter ID</returns>
        public List<string> GetSimulationParameterIDs()
        {
            if (m_currentProject == null ||
                m_currentProject.StepperDic == null)
                return new List<string>();

            return new List<string>(m_currentProject.StepperDic.Keys);
        }

        /// <summary>
        /// Sets the parameter of the simulator.
        /// </summary>
        /// <param name="parameterID">the set parameter ID</param>
        public void SetSimulationParameter(string parameterID)
        {
            SetSimulationParameter(parameterID, true, true);
        }

        /// <summary>
        /// Sets the parameter of the simulator.
        /// </summary>
        /// <param name="parameterID">the set parameter ID</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void SetSimulationParameter(string parameterID, bool isRecorded, bool isAnchor)
        {
            string message = null;
            try
            {
                message = "[" + parameterID + "]";
                string oldParameterID = m_currentProject.Info.SimulationParam;
                if (oldParameterID != parameterID)
                {
                    foreach (string modelID in m_currentProject.StepperDic[oldParameterID].Keys)
                    {
                        if (!m_currentProject.StepperDic[parameterID].ContainsKey(modelID))
                            continue;

                        List<EcellObject> currentList
                            = m_currentProject.StepperDic[oldParameterID][modelID];
                        List<EcellObject> newList
                            = m_currentProject.StepperDic[parameterID][modelID];
                        foreach (EcellObject current in currentList)
                        {
                            foreach (EcellObject newObj in newList)
                            {
                                if (!current.Classname.Equals(newObj.Classname))
                                    continue;

                                foreach (EcellData currentData in current.Value)
                                {
                                    foreach (EcellData newData in newObj.Value)
                                    {
                                        if (currentData.Name.Equals(newData.Name)
                                            && currentData.EntityPath.Equals(newData.EntityPath))
                                        {
                                            newData.Gettable = currentData.Gettable;
                                            newData.Loadable = currentData.Loadable;
                                            newData.Saveable = currentData.Saveable;
                                            newData.Settable = currentData.Settable;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    m_currentProject.Info.SimulationParam = parameterID;
                    this.Initialize(true);
                    foreach (string modelID
                        in m_currentProject.StepperDic[oldParameterID].Keys)
                    {
                        foreach (EcellObject old
                            in m_currentProject.StepperDic[oldParameterID][modelID])
                        {
                            List<EcellData> delList = new List<EcellData>();
                            foreach (EcellData oldData in old.Value)
                            {
                                if (oldData.Gettable
                                    && !oldData.Loadable
                                    && !oldData.Saveable
                                    && !oldData.Settable)
                                {
                                    delList.Add(oldData);
                                }
                            }
                            foreach (EcellData del in delList)
                            {
                                old.Value.Remove(del);
                            }
                        }
                    }
                }
                m_env.PluginManager.ParameterSet(m_currentProject.Info.Name, parameterID);
                m_env.LogManager.Append(new ApplicationLogEntry(
                    MessageType.Information,
                    string.Format(MessageResources.InfoSimParamSet, parameterID),
                    this));
                if (isRecorded)
                    m_env.ActionManager.AddAction(new SetSimParamAction(parameterID, oldParameterID, isAnchor));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new EcellException(MessageResources.ErrSetSimParam, ex);
            }
        }

        /// <summary>
        /// Sets the parameter of the simulator.
        /// </summary>
        /// <param name="simParam">the set parameter ID</param>
        internal void SetSimulationParameter(SimulationParameter simParam)
        {
            try
            {
                string simParamID = simParam.ID;
                // Stores the simulation parameter.
                if (!m_currentProject.Info.SimulationParam.Equals(simParamID))
                {
                    if (!m_currentProject.StepperDic.ContainsKey(simParamID))
                    {
                        m_currentProject.StepperDic[simParamID]
                            = new Dictionary<string, List<EcellObject>>();
                    }
                    foreach (EcellObject stepper in simParam.Steppers)
                    {
                        if (!m_currentProject.StepperDic[simParamID]
                            .ContainsKey(stepper.ModelID))
                        {
                            m_currentProject.StepperDic[simParamID][stepper.ModelID]
                                = new List<EcellObject>();
                        }
                        foreach (EcellData data in stepper.Value)
                        {
                            data.Value = GetEcellValue(data);
                        }
                        m_currentProject.StepperDic[simParamID][stepper.ModelID].Add(stepper);
                    }
                }
                else
                {
                    foreach (EcellObject stepper in simParam.Steppers)
                    {
                        bool matchFlag = false;
                        if (!m_currentProject.StepperDic[simParamID].ContainsKey(stepper.ModelID))
                        {
                            m_currentProject.StepperDic[simParamID][stepper.ModelID]
                                = new List<EcellObject>();
                        }
                        for (int j = 0;
                            j < m_currentProject.StepperDic[simParamID][stepper.ModelID].Count;
                            j++)
                        {
                            EcellObject storedStepper
                                = m_currentProject.StepperDic[simParamID][stepper.ModelID][j];
                            if (!storedStepper.Classname.Equals(stepper.Classname)
                                || !storedStepper.Key.Equals(stepper.Key)
                                || !storedStepper.ModelID.Equals(stepper.ModelID)
                                || !storedStepper.Type.Equals(stepper.Type))
                                continue;

                            List<EcellData> newDataList = new List<EcellData>();
                            foreach (EcellData storedData in storedStepper.Value)
                            {
                                bool existFlag = false;
                                foreach (EcellData newData in stepper.Value)
                                {
                                    if (!storedData.Name.Equals(newData.Name)
                                        || !storedData.EntityPath.Equals(newData.EntityPath))
                                        continue;

                                    if (storedData.Value.IsDouble)
                                    {
                                        // XXX: canonicalize the value
                                        newData.Value = GetEcellValue(newData);
                                    }
                                    newData.Gettable = storedData.Gettable;
                                    newData.Loadable = storedData.Loadable;
                                    newData.Saveable = storedData.Saveable;
                                    newData.Settable = storedData.Settable;
                                    newDataList.Add(newData);
                                    existFlag = true;
                                    break;
                                }
                                if (!existFlag)
                                {
                                    newDataList.Add(storedData);
                                }
                            }
                            m_currentProject.StepperDic[simParamID][stepper.ModelID][j]
                                = EcellObject.CreateObject(
                                    stepper.ModelID,
                                    stepper.Key,
                                    stepper.Type,
                                    stepper.Classname,
                                    newDataList);
                            matchFlag = true;
                            break;
                        }
                        if (!matchFlag)
                        {
                            m_currentProject.StepperDic[simParamID][stepper.ModelID]
                                .Add(stepper);
                        }
                    }
                }
                m_currentProject.LoggerPolicyDic[simParamID] = simParam.LoggerPolicy;
                m_currentProject.InitialCondition[simParamID] = simParam.InitialConditions;
                m_env.Console.WriteLine(String.Format(MessageResources.InfoLoadSim, simParamID));
                m_env.Console.Flush();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// GetEcellValue
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static EcellValue GetEcellValue(EcellData data)
        {
            double value = 0.0;
            try
            {
                // Get new value.
                string newValue = data.Value.ToString();
                if (newValue.Equals(Double.PositiveInfinity.ToString()))
                    value = Double.PositiveInfinity;
                else if (newValue.Equals(Double.MaxValue.ToString()))
                    value = Double.MaxValue;
                else
                    value = XmlConvert.ToDouble(newValue);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                value = Double.PositiveInfinity;
            }
            return new EcellValue(value);
        }

        /// <summary>
        /// Deletes the parameter.
        /// </summary>
        /// <param name="parameterID"></param>
        public void DeleteSimulationParameter(string parameterID)
        {
            DeleteSimulationParameter(parameterID, true, true);
        }

        /// <summary>
        /// Deletes the parameter.
        /// </summary>
        /// <param name="parameterID"></param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DeleteSimulationParameter(string parameterID, bool isRecorded, bool isAnchor)
        {
            string message = null;

            if (m_currentProject.StepperDic.Keys.Count <= 1)
            {
                throw new EcellException(String.Format(MessageResources.ErrDelParam));
            }

            try
            {
                Debug.Assert(!String.IsNullOrEmpty(parameterID));
                if (m_currentProject.SimulationStatus == SimulationStatus.Run ||
                    m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    if (parameterID.Equals(m_currentProject.Info.SimulationParam))
                    {
                        if (Util.ShowYesNoDialog(
                            String.Format(MessageResources.InfoDeleteSim,
                            parameterID)) == false)
                            return;
                        SimulationStop();
                        m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
                    }
                }
                message = "[" + parameterID + "]";
                //
                // Initializes.
                //

                this.SetDefaultDir();
                if (string.IsNullOrEmpty(m_defaultDir))
                {
                    throw new EcellException(String.Format(MessageResources.ErrNoSet,
                        new object[] { MessageResources.NameWorkDir }));
                }

                Debug.Assert(m_currentProject.StepperDic.ContainsKey(parameterID));
                m_currentProject.StepperDic.Remove(parameterID);
                string simulationDirName
                        = this.m_defaultDir + Constants.delimiterPath
                        + m_currentProject.Info.Name + Constants.delimiterPath + Constants.xpathParameters;
                string pattern
                        = "_????_??_??_??_??_??_" + parameterID + Constants.FileExtXML;
                if (Directory.Exists(simulationDirName))
                {
                    foreach (string fileName in Directory.GetFiles(simulationDirName, pattern))
                    {
                        File.Delete(fileName);
                    }
                    string simulationFileName
                            = simulationDirName + Constants.delimiterPath + parameterID + Constants.FileExtXML;
                    File.Delete(simulationFileName);
                }
                m_currentProject.LoggerPolicyDic.Remove(parameterID);
                Trace.WriteLine(m_currentProject.Info.SimulationParam + ":" + parameterID);
                if (m_currentProject.Info.SimulationParam == parameterID)
                {
                    foreach (string key in m_currentProject.StepperDic.Keys)
                    {
                        m_currentProject.Info.SimulationParam = key;
                        m_env.PluginManager.ParameterSet(m_currentProject.Info.Name, key);
                        break;
                    }
                }
                m_env.PluginManager.ParameterDelete(m_currentProject.Info.Name, parameterID);
                m_env.Console.WriteLine(String.Format(MessageResources.InfoRemoveSim, parameterID));
                m_env.Console.Flush();
                MessageDeleteEntity("Simulation Parameter", message);

                //if (isRecorded)
                //    m_env.ActionManager.AddAction(new DeleteSimParamAction(parameterID, isAnchor));
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrDelete,
                    new object[] { parameterID }), ex);
            }
        }

        /// <summary>
        /// Copy SimulationParameter.
        /// </summary>
        /// <param name="newParameterID"></param>
        /// <param name="srcParameterID"></param>
        public void CopySimulationParameter(string newParameterID, string srcParameterID)
        {
            try
            {
                string message = null;

                message = "[" + newParameterID + "]";
                //
                // 4 Stepper
                //
                if (m_currentProject.StepperDic.ContainsKey(newParameterID))
                {
                    throw new EcellException(
                        String.Format(MessageResources.ErrExistObj,
                        new object[] { newParameterID }));
                }

                m_currentProject.LoggerPolicyDic[newParameterID] =
                    new LoggerPolicy(m_currentProject.LoggerPolicyDic[srcParameterID]);


                Dictionary<string, List<EcellObject>> newStepperListSets = new Dictionary<string, List<EcellObject>>();
                Dictionary<string, Dictionary<string, double>> newInitialCondSets = new Dictionary<string, Dictionary<string, double>>();
                foreach (string name in m_currentProject.StepperDic[srcParameterID].Keys)
                {
                    List<EcellObject> tmpList = new List<EcellObject>();
                    foreach (EcellObject sObj in m_currentProject.StepperDic[srcParameterID][name])
                    {
                        tmpList.Add(sObj.Clone());
                    }
                    newStepperListSets.Add(name, tmpList);
                }
                foreach (string name in m_currentProject.InitialCondition[srcParameterID].Keys)
                {
                    Dictionary<string, double> tmpDic = new Dictionary<string, double>();
                    foreach (string path in m_currentProject.InitialCondition[srcParameterID][name].Keys)
                    {
                        tmpDic.Add(path, m_currentProject.InitialCondition[srcParameterID][name][path]);
                    }
                    newInitialCondSets.Add(name, tmpDic);
                }

                m_currentProject.StepperDic[newParameterID] = newStepperListSets;
                m_currentProject.InitialCondition[newParameterID] = newInitialCondSets;

                m_env.PluginManager.ParameterAdd(m_currentProject.Info.Name, newParameterID);
                m_env.Console.WriteLine(String.Format(MessageResources.InfoCreSim, newParameterID));
                m_env.Console.Flush();

                Trace.WriteLine(String.Format(MessageResources.InfoCreSim,
                    new object[] { newParameterID }));
                //まだcopyがない
                //m_env.ActionManager.AddAction(new NewSimParamAction(parameterID, isAnchor));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                string message = String.Format(MessageResources.ErrCreSimParam,
                    new object[] { newParameterID });
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Saves the simulation parameter.
        /// </summary>
        /// <param name="paramID">The simulation parameter ID</param>
        internal void SaveSimulationParameter(string paramID)
        {
            string message = null;
            try
            {
                message = "[" + paramID + "]";
                //
                // Initializes.
                //
                Debug.Assert(!String.IsNullOrEmpty(paramID));
                SetDefaultDir();

                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + m_currentProject.Info.Name))
                {
                    m_currentProject.Save();
                }
                string simulationDirName = Path.Combine(m_currentProject.Info.ProjectPath, Constants.xpathParameters);

                if (!Directory.Exists(simulationDirName))
                {
                    Directory.CreateDirectory(simulationDirName);
                }
                string simulationFileName
                    = simulationDirName + Constants.delimiterPath + paramID + Constants.FileExtXML;
                //
                // Picks the "Stepper" up.
                //
                List<EcellObject> stepperList = new List<EcellObject>();
                foreach (string modelID in m_currentProject.StepperDic[paramID].Keys)
                {
                    stepperList.AddRange(m_currentProject.StepperDic[paramID][modelID]);
                }
                Debug.Assert(stepperList != null && stepperList.Count > 0);

                //
                // Picks the "LoggerPolicy" up.
                //
                LoggerPolicy loggerPolicy = m_currentProject.LoggerPolicyDic[paramID];
                //
                // Picks the "InitialCondition" up.
                //
                Dictionary<string, Dictionary<string, double>> initialCondition
                        = this.m_currentProject.InitialCondition[paramID];
                //
                // Creates.
                //
                SimulationParameterWriter.Create(simulationFileName,
                    new SimulationParameter(
                        stepperList,
                        initialCondition,
                        loggerPolicy,
                        paramID));
                Trace.WriteLine("Save Simulation Parameter: " + message);
            }
            catch (Exception ex)
            {
                message = String.Format(MessageResources.ErrSavePrj,
                    new object[] { m_currentProject.Info.Name });
                Trace.WriteLine(message);
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Returns the initial condition.
        /// </summary>
        /// <param name="paremterID">The parameter ID</param>
        /// <param name="modelID">The model ID</param>
        /// <returns>The initial condition</returns>
        public Dictionary<string, double>
                GetInitialCondition(string paremterID, string modelID)
        {
            return this.m_currentProject.InitialCondition[paremterID][modelID];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterID"></param>
        /// <param name="modelID"></param>
        /// <param name="initialList"></param>
        public void UpdateInitialCondition(
                string parameterID, string modelID, Dictionary<string, double> initialList)
        {
            if (string.IsNullOrEmpty(parameterID))
                parameterID = m_currentProject.Info.SimulationParam;

            Dictionary<string, double> parameters = this.m_currentProject.InitialCondition[parameterID][modelID];
            foreach (string key in initialList.Keys)
            {
                if (parameters.ContainsKey(key))
                    parameters.Remove(key);

                parameters[key] = initialList[key];
            }
            Trace.WriteLine("Update Initial Condition: " + "(parameterName=" + parameterID + ", modelID=" + modelID + ")");
        }

        /// <summary>
        /// Returns the next event.
        /// </summary>
        /// <returns>The current simulation time, The stepper</returns>
        public ArrayList GetNextEvent()
        {
            try
            {
                ArrayList list = new ArrayList(2);
                EcellCoreLib.EventDescriptor desc = m_currentProject.Simulator.GetNextEvent();
                list.Add(desc.Time);
                list.Add(desc.StepperID);
                return list;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        #endregion

        #region Mehtod for properties
        /// <summary>
        /// Check whether this dm is able to add the property.
        /// </summary>
        /// <param name="dmName">dm Name.</param>
        /// <returns>if this dm is enable to add property, return true.</returns>
        public bool IsEnableAddProperty(string dmName)
        {
            bool isEnable = true;
            try
            {
                string path = Constants.xpathProcess + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper();
                WrappedSimulator sim = m_currentProject.CreateSimulatorInstance();
                sim.CreateEntity(
                    dmName,
                    path);

                string fullPath = path + Constants.delimiterColon + "CheckProperty";
                EcellValue newValue = new EcellValue(0.01);
                sim.SetEntityProperty(fullPath, newValue.Value);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return false;
            }
            return isEnable;
        }

        /// <summary>
        /// Get the EcellValue from fullPath.
        /// </summary>
        /// <param name="fullPN"></param>
        /// <returns></returns>
        public EcellValue GetEntityProperty(string fullPN)
        {
            try
            {
                if (m_currentProject.Simulator == null)
                {
                    return null;
                }
                EcellValue value
                    = new EcellValue(m_currentProject.Simulator.GetEntityProperty(fullPN));
                return value;
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrPropData,
                    new object[] { fullPN }), ex);
            }
        }

        /// <summary>
        /// Set the value to the full Path.
        /// </summary>
        /// <param name="fullPN">set full path.</param>
        /// <param name="value">set value.</param>
        public void SetEntityProperty(string fullPN, string value)
        {
            if (m_currentProject.Simulator == null
                || this.GetCurrentSimulationTime() <= 0.0)
            {
                return;
            }
            EcellValue storedValue
                = new EcellValue(m_currentProject.Simulator.GetEntityProperty(fullPN));
            EcellValue newValue = null;
            if (storedValue.IsDouble)
            {
                newValue = new EcellValue(XmlConvert.ToDouble(value));
            }
            else if (storedValue.IsInt)
            {
                newValue = new EcellValue(XmlConvert.ToInt32(value));
            }
            else if (storedValue.IsList)
            {
                // newValue = new EcellValue(value);
                return;
            }
            else
            {
                newValue = new EcellValue(value);
            }
            m_currentProject.Simulator.LoadEntityProperty(
                fullPN,
                newValue.Value);
        }

        /// <summary>
        /// Get the current value with fullPath.
        /// This method is used to get numerical value of parameter while simulating.
        /// </summary>
        /// <param name="fullPN"></param>
        /// <returns></returns>
        public double GetPropertyValue(string fullPN)
        {
            try
            {
                return (double)m_currentProject.Simulator.GetEntityProperty(fullPN);
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrPropData, new object[] { fullPN }), ex);
            }
        }

        #endregion

        #region Mehtod for Logger
        /// <summary>
        /// Returns the current logger policy.
        /// </summary>
        /// <returns>The current logger policy</returns>
        private LoggerPolicy GetCurrentLoggerPolicy()
        {
            string simParam = m_currentProject.Info.SimulationParam;
            return m_currentProject.LoggerPolicyDic[simParam];
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <param name="fullPathID"></param>
        /// <param name="isInitalize"></param>
        /// <param name="sim"></param>
        /// <param name="loggerPolicy"></param>
        private void CreateLogger(string fullPathID, bool isInitalize, WrappedSimulator sim, LoggerPolicy loggerPolicy)
        {
            if (m_loggerEntry.Contains(fullPathID)) return;

            if (m_currentProject.SimulationStatus == SimulationStatus.Run ||
                m_currentProject.SimulationStatus == SimulationStatus.Suspended ||
                isInitalize)
            {
                sim.CreateLogger(fullPathID,
                    loggerPolicy.ReloadStepCount,
                    loggerPolicy.ReloadInterval,
                    Convert.ToBoolean((int)loggerPolicy.DiskFullAction),
                    loggerPolicy.MaxDiskSpace);
            }
            m_loggerEntry.Add(fullPathID);
        }

        /// <summary>
        /// Sets the "LoggerPolicy".
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="loggerPolicy">The "LoggerPolicy"</param>
        public void SetLoggerPolicy(string parameterID, LoggerPolicy loggerPolicy)
        {
            m_currentProject.LoggerPolicyDic[parameterID] = loggerPolicy;
        }

        /// <summary>
        /// Returns the "LoggerPolicy".
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <returns>The "LoggerPolicy"</returns>
        public LoggerPolicy GetLoggerPolicy(string parameterID)
        {
            return m_currentProject.LoggerPolicyDic[parameterID];
        }

        /// <summary>
        /// GetLogDataList
        /// </summary>
        /// <returns></returns>
        public List<string> GetLogDataList()
        {
            List<string> result = new List<string>();
            string topDir = GetParameterDir();
            if (!Directory.Exists(topDir)) return result;

            string[] pdirs = Directory.GetDirectories(topDir);
            for (int i = 0; i < pdirs.Length; i++)
            {
                string paramdir = pdirs[i];
                string paramName = Path.GetFileName(paramdir);
                string[] pfiles = Directory.GetFiles(paramdir, Constants.xpathProcess + "*");
                string[] vfiles = Directory.GetFiles(paramdir, Constants.xpathVariable + "*");
                for (int j = 0; j < pfiles.Length; j++)
                {
                    string logdata = paramName + Path.PathSeparator
                        + Path.GetFileName(pfiles[j])
                        + Path.PathSeparator + pfiles[j];
                    result.Add(logdata);
                }

                for (int j = 0; j < vfiles.Length; j++)
                {
                    string logdata = paramName + Path.PathSeparator
                        + Path.GetFileName(vfiles[j])
                        + Path.PathSeparator + vfiles[j];
                    result.Add(logdata);
                }
            }
            return result;
        }

        /// <summary>
        /// GetParameterDir
        /// </summary>
        /// <returns></returns>
        private string GetParameterDir()
        {
            return Path.Combine(Path.Combine(this.m_defaultDir, m_currentProject.Info.Name), Constants.ParameterDirName);
        }

        #endregion

        #region Methof for DM
        /// <summary>
        /// Get the dm file and the source file for dm in the directory of current project.
        /// </summary>
        /// <returns>The list of dm in the directory of current project.</returns>
        public List<string> GetDMNameList()
        {
            List<string> resultList = new List<string>();
            // Get DM directory for this project.
            string path = null;
            if (m_currentProject.Info.ProjectPath != null)
                path = Path.Combine(m_currentProject.Info.ProjectPath, Constants.DMDirName);
            if (!Directory.Exists(path))
                return resultList;

            // Add DMs in DM directory.
            string[] files = Directory.GetFiles(path, "*" + Constants.FileExtDM);
            for (int i = 0; i < files.Length; i++)
            {
                string name = Path.GetFileNameWithoutExtension(files[i]);
                if (!resultList.Contains(name))
                    resultList.Add(name);
            }

            // 
            // Get DM sources.
            files = Directory.GetFiles(path, "*" + Constants.FileExtSource);
            for (int i = 0; i < files.Length; i++)
            {
                string name = Path.GetFileNameWithoutExtension(files[i]);
                if (!resultList.Contains(name))
                    resultList.Add(name);
            }
            return resultList;
        }

        /// <summary>
        /// Get the DM directory of current project.
        /// </summary>
        /// <returns></returns>
        public string GetDMDir()
        {
            return Path.Combine(m_currentProject.Info.ProjectPath, Constants.DMDirName);
        }

        /// <summary>
        /// GetDMFileName
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public string GetDMFileName(string indexName)
        {
            string path = Path.Combine(m_currentProject.Info.ProjectPath, Constants.DMDirName);
            path = Path.Combine(path, indexName + Constants.FileExtSource);
            if (!File.Exists(path))
                return null;
            return path;
        }

        #endregion

        #region Send Message
        /// <summary>
        /// Message on CreateEntity
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void MessageCreateEntity(string type, string message)
        {
            Trace.WriteLine("Create " + type + ": " + message);
        }
        /// <summary>
        /// Message on DeleteEntity
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void MessageDeleteEntity(string type, string message)
        {
            Trace.WriteLine("Delete " + type + ": " + message);
        }
        /// <summary>
        /// Message on UpdateData
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public void MessageUpdateData(string type, string message, string src, string dest)
        {
            Trace.WriteLine(
                "Update Data: " + message + "[" + type + "]" + System.Environment.NewLine
                    + "\t[" + src + "]->[" + dest + "]");
        }
        #endregion
    }
}
