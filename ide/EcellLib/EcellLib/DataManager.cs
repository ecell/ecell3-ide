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

using Python.Runtime;

using EcellCoreLib;
using Ecell.Objects;
using Ecell.Logging;
using Ecell.Logger;
using Ecell.Exceptions;
using Ecell.SBML;
using Ecell.Action;
using Ecell.Plugin;
using Ecell.Events;
using Ecell.Job;

namespace Ecell
{
    /// <summary>
    /// EventHandler when object is added.
    /// </summary>
    /// <param name="o">DataManager</param>
    /// <param name="e">DisplayFormatEventArgs</param>
    public delegate void DisplayFormatChangedEventHandler(object o, DisplayFormatEventArgs e);
    /// <summary>
    /// EventHandler when the stepping model is changed.
    /// </summary>
    /// <param name="o">DataManager</param>
    /// <param name="e">SteppingModelEventArgs</param>
    public delegate void SteppingModelChangedEventHandler(object o, SteppingModelEventArgs e);
    /// <summary>
    /// EventHandler when the stepping model is applied.
    /// </summary>
    /// <param name="o">DataManager</param>
    /// <param name="e">SteppingModelEventArgs</param>
    public delegate void ApplySteppingModelEnvetHandler(object o, SteppingModelEventArgs e);
    /// <summary>
    /// EventHandler when the simulator is reloaded.
    /// </summary>
    /// <param name="o">DataManager</param>
    /// <param name="e">EventArgs</param>
    public delegate void ReloadSimulatorEventHandler(object o, EventArgs e);

    /// <summary>
    /// Manages data of projects, models, and so on.
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// EventHandler when display format of value is changed.
        /// </summary>
        public event DisplayFormatChangedEventHandler DisplayFormatEvent;
        /// <summary>
        /// EventHandler when the stepping model is changed.
        /// </summary>
        public event SteppingModelChangedEventHandler SteppingModelEvent;
        /// <summary>
        /// EventHandler when the stepping model is applied.
        /// </summary>
        public event ApplySteppingModelEnvetHandler ApplySteppingModelEvent;
        /// <summary>
        /// EventHandler when the simulator is reloaded.
        /// </summary>
        public event ReloadSimulatorEventHandler ReloadSimulatorEvent;

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
        /// The remain step in stepping.
        /// </summary>
        private int m_remainStep = 0;
        /// <summary>
        /// The remain time in stepping.
        /// </summary>
        private double m_remainTime = 0.0;
        /// <summary>
        /// The time limit of the simulation
        /// </summary>
        private double m_simulationTimeLimit = -1.0;
        /// <summary>
        /// The wait time between steps.
        /// </summary>
        private int m_waitTime = 0;
        /// <summary>
        /// Display format of value.
        /// </summary>
        private ValueDataFormat m_format = ValueDataFormat.Normal;
        /// <summary>
        /// The flag whether this step is saving.
        /// </summary>
        private bool m_isSaveStep = false;
        /// <summary>
        /// The dictionary of the save data and time.
        /// </summary>
        private Dictionary<int, double> m_saveTimeDic = new Dictionary<int, double>();
        /// <summary>
        /// The dictionary of the loading stepping data.
        /// </summary>
        private Dictionary<string, EcellValue> m_steppingData = null;
        /// <summary>
        /// The list of deleted parameter IDs.
        /// </summary>
        private List<string> m_deleteParameterList = new List<string>();
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
        /// get / set StepCount.
        /// </summary>
        public int StepCount
        {
            get { return this.m_defaultStepCount; }
            set { this.m_defaultStepCount = value; }
        }

        /// <summary>
        /// get / set WaitTime.
        /// </summary>
        public int WaitTime
        {
            get { return this.m_waitTime; }
            set { this.m_waitTime = value; }
        }

        /// <summary>
        /// get / set the dispaly format of value.
        /// </summary>
        public ValueDataFormat DisplayFormat
        {
            get { return this.m_format; }
            set 
            {
                if (m_format == value) 
                    return;

                this.m_format = value;
                if (DisplayFormatEvent != null)
                    DisplayFormatEvent(this, 
                        new DisplayFormatEventArgs(Util.GetDisplayFormat(this.m_format)));
            }
        }

        /// <summary>
        /// get / set the display format string.
        /// </summary>
        public string DisplayStringFormat
        {
            get
            {
                switch (this.m_format)
                {
                    case ValueDataFormat.Exponential1:
                        return "e1";
                    case ValueDataFormat.Exponential2:
                        return "e2";
                    case ValueDataFormat.Exponential3:
                        return "e3";
                    case ValueDataFormat.Exponential4:
                        return "e4";
                    case ValueDataFormat.Exponential5:
                        return "e5";
                    default:
                        return "G";
                }
            }
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
        public string DefaultDir
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

        /// <summary>
        /// get / set the default time.
        /// </summary>
        public double DefaultTime
        {
            get { return this.m_defaultTime; }
            set { this.m_defaultTime = value; }
        }

        /// <summary>
        /// get / set the flag whether the step is saving.
        /// </summary>
        public bool IsSaveStep
        {
            get { return this.m_isSaveStep; }
            set 
            { 
                this.m_isSaveStep = value;
                this.m_steppingData = null;
                if (!value && ApplySteppingModelEvent != null)
                    ApplySteppingModelEvent(this, new SteppingModelEventArgs(-1.0));
            }
        }

        /// <summary>
        /// get / set the dictionary of the save index and time.
        /// </summary>
        public Dictionary<int, double> SaveTime
        {
            get { return this.m_saveTimeDic; }
            set { this.m_saveTimeDic = value; }
        }

        #endregion

        #region Method for File I/O.
        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="fileName">the script file name.</param>
        public void SaveScript(string fileName)
        {
            try
            {
                ScriptWriter writer = new ScriptWriter(m_currentProject);
                writer.SaveScript(fileName);
                m_env.Console.WriteLine(string.Format(MessageResources.InfoSaveScript, fileName));
                m_env.Console.Flush();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new EcellException(string.Format(MessageResources.ErrSaveScript, fileName), ex);
            }
        }

        /// <summary>
        /// Compile the dm source file.
        /// </summary>
        /// <param name="fileName">the source file name.</param>
        public void ExecuteScript(string fileName)
        {
            int i = 0;
            PythonEngine.InitExt();

            string scriptFile = fileName;
            m_env.Console.WriteLine(string.Format(MessageResources.InfoExecScript, fileName));
            m_env.Console.Flush();
         
            i = PythonEngine.RunSimpleString("import sys");
            i = PythonEngine.RunSimpleString("import getopt");
            i = PythonEngine.RunSimpleString("import code");
            i = PythonEngine.RunSimpleString("import os");
            i = PythonEngine.RunSimpleString("from EcellIDE import *");

            i = PythonEngine.RunSimpleString("aSession = Session()");

            i = PythonEngine.RunSimpleString("aContext = { 'self': aSession }");
            i = PythonEngine.RunSimpleString("aKeyList = list ( aSession.__dict__.keys() + aSession.__class__.__dict__.keys() )");
            i = PythonEngine.RunSimpleString("aDict = {}");
            string ddd = "for aKey in aKeyList:\n" +
                            "    aDict[ aKey ] = getattr (aSession, aKey)";
            i = PythonEngine.RunSimpleString(ddd);
            i = PythonEngine.RunSimpleString("aContext.update( aDict )");
            string res = fileName;
            res = res.Replace("\\", "\\\\");
            i = PythonEngine.RunSimpleString("execfile('" + res + "', aContext)");            

            //PythonEngine m_engine = new PythonEngine();

            //m_engine.AddToPath(Directory.GetCurrentDirectory());
            //m_engine.AddToPath(Util.GetAnalysisDir());

            //MemoryStream standardOutput = new MemoryStream();
            //m_engine.SetStandardOutput(standardOutput);
            //m_engine.Execute("from EcellIDE import *");
            //m_engine.Execute("import time");
            //m_engine.Execute("import System.Threading");
            //m_engine.Execute("session=Session()");
            //m_engine.ExecuteFile(scriptFile);            
            //string stdOut = ASCIIEncoding.ASCII.GetString(standardOutput.ToArray());

            //m_env.Console.WriteLine(stdOut);
            //m_env.Console.Flush();
            //m_engine = null;
        }

        /// <summary>
        /// LoadSBML
        /// </summary>
        /// <param name="filename">the SBML file name.</param>
        public void LoadSBML(string filename)
        {
            WrappedSimulator sim = null;
            try
            {
                // Load model
                sim = new WrappedSimulator(Util.GetDMDirs());
                EcellModel model = SBML2EML.Convert(filename);
                EmlReader.InitializeModel(model, sim);
                // Save eml.
                string dir = Util.GetTmpDir();
                string modelFileName = Path.GetFileNameWithoutExtension(filename) + Constants.FileExtEML;
                modelFileName = Path.Combine(dir, modelFileName);
                EmlWriter.Create(modelFileName, model.Children, false);
                LoadProject(modelFileName);

            }
            catch (Exception e)
            {
                m_env.Console.WriteLine("Failed to convert SBML:" + filename);
                m_env.Console.WriteLine(e.ToString());
                throw new EcellException("Failed to convert SBML.", e);
            }
            finally
            {
                // 20090727
                if (sim != null)
                    sim.Dispose();
            }

        }

        /// <summary>
        /// Export the current model to the file.
        /// </summary>
        /// <param name="filename">the SBML file.</param>
        public void ExportSBML(string filename)
        {
            EcellModel model = m_currentProject.Model;
            model.Children.Clear();
            model.Children.AddRange(m_currentProject.SystemDic[model.ModelID]);

            Trace.WriteLine("Export SBML: " + filename);
            EML2SBML.SaveSBML(model, filename);
        }

        /// <summary>
        /// Import DM from the set directory.
        /// </summary>
        /// <param name="path">the loading directory.</param>
        public void ImportDM(string path)
        {
            // Copy DMs.
            if (m_currentProject == null || m_currentProject.Info.ProjectType != ProjectType.Project)
                return;
            string dmDir = Path.Combine(m_currentProject.Info.ProjectPath, Constants.DMDirName);
            if (!Directory.Exists(dmDir))
                Directory.CreateDirectory(dmDir);
            Util.CopyDirectory(path, dmDir, true);

            m_currentProject.SetDMList();
            m_env.DMDescriptorKeeper.Load(m_currentProject.GetDMDirs());
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
                throw new EcellException(string.Format(MessageResources.ErrLoadPrj,
                    new object[] { filename }), ex);
            }
        }

        /// <summary>
        /// LoadProject
        /// </summary>
        /// <param name="info">the load project information.</param>
        public void LoadProject(ProjectInfo info)
        {
            List<EcellObject> passList = new List<EcellObject>();
            string message = null;
            string projectID = null;
            Project project = null;

            try
            {
                // Check Current.
                if (info == null)
                    throw new EcellException(MessageResources.ErrLoadPrj);
                if (m_currentProject != null)
                    CloseProject();

                // Create project.
                projectID = info.Name;
                message = "[" + projectID + "]";
                project = new Project(info, m_env);
                m_env.PluginManager.ChangeStatus(ProjectStatus.Loading);

                // Create EcellProject.
                List<EcellData> ecellDataList = new List<EcellData>();
                ecellDataList.Add(new EcellData(Constants.textComment, new EcellValue(project.Info.Comment), null));
                passList.Add(EcellObject.CreateObject(projectID, "", Constants.xpathProject, "", ecellDataList));

                // Load DMs.
                m_env.DMDescriptorKeeper.Load(project.GetDMDirs());

                // Prepare datas.
                project.LoadModel();
                foreach (EcellObject model in project.ModelList)
                {
                    Trace.WriteLine(string.Format(MessageResources.InfoLoadModel, model.ModelID));
                    m_env.Console.WriteLine(string.Format(MessageResources.InfoLoadModel, model.ModelID));
                    m_env.Console.Flush();
                    passList.Add(model);
                }
                foreach (string storedModelID in project.SystemDic.Keys)
                {
                    passList.AddRange(project.StepperDic[storedModelID]);
                    passList.AddRange(project.SystemDic[storedModelID]);
                }

                // Set current project.
                m_currentProject = project;

                // Load analysis directory.
                LoadAnalysisDirectory(project);

                // Load SimulationParameters.
                LoadSimulationParameters(project);
                m_env.PluginManager.ParameterSet(projectID, project.Info.SimulationParam);

                ClearSteppingModel();
            }
            catch (Exception ex)
            {
                passList = null;
                CloseProject();
                throw new EcellException(string.Format(MessageResources.ErrLoadPrj, projectID), ex);
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

                    // Send Message.
                    m_env.Console.WriteLine(string.Format(MessageResources.InfoLoadPrj, projectID));
                    m_env.Console.Flush();

                    m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
                }
            }
        }

        /// <summary>
        /// Load the analysis directory.
        /// </summary>
        /// <param name="project">the project object.</param>
        private void LoadAnalysisDirectory(Project project)
        {
            string path = project.GetAnalysisDirectory();
            if (path == null || !Directory.Exists(path))
                return;

            string[] dirs = Directory.GetDirectories(path);
            for (int i = 0; i < dirs.Length; i++)
            {
                DirectoryInfo d = new DirectoryInfo(dirs[i]);
                string groupName = d.Name;
                string[] ele = groupName.Split(new char[] { '_' });
                if (ele.Length != 2) continue;
                string analysisName = ele[0];
                string date = ele[1];
               
                if (!m_env.JobManager.AnalysisDic.ContainsKey(analysisName))
                    continue;

                string modelDir = dirs[i] + "/" + Constants.ModelDirName;
                string logDir = dirs[i] + "/" + Constants.LogDirName;

                // load model
                string modelFile = modelDir + "/" + date + ".eml";                
                ProjectInfo info = ProjectInfoLoader.Load(modelFile);
                string projectID = info.Name;
                Project aproject = new Project(info, m_env);
                aproject.LoadModel();

                List<EcellObject> systemObjList = aproject.SystemDic[aproject.Model.ModelID];
                List<EcellObject> stepperObjList = aproject.StepperDic[aproject.Model.ModelID];

                // create job group and analysis.
                JobGroup g = m_env.JobManager.CreateJobGroup(analysisName, date, systemObjList, stepperObjList);
                IAnalysisModule analysis = m_env.JobManager.AnalysisDic[analysisName].CreateNewInstance(g);


                // load analysis parameters.
                analysis.LoadAnalysisInfo(modelDir);
                g.LoadJobEntry(logDir);
                g.IsSaved = true;
                g.TopDir = dirs[i];
                g.UpdateStatus();
                m_env.JobManager.Update();
            }
        }

        /// <summary>
        /// Loads the simulation parameters.
        /// </summary>
        /// <param name="project">the project object.</param>
        private void LoadSimulationParameters(Project project)
        {
            // Check directory.
            string simulationDirName = null;
            if (project.Info.ProjectPath != null)
                simulationDirName = Path.Combine(project.Info.ProjectPath, Constants.xpathParameters);
            if (!Directory.Exists(simulationDirName))
                return;

            // Get Parameter files.
            string[] parameters = Directory.GetFileSystemEntries(
                simulationDirName,
                Constants.delimiterWildcard + Constants.FileExtXML);
            if (parameters == null || parameters.Length <= 0)
                return;

            // Load parameters.
            foreach (string parameter in parameters)
            {
                // Check filename.
                string fileName = Path.GetFileName(parameter);
                if (fileName.IndexOf(Constants.delimiterUnderbar) == 0)
                    continue;
                // Load parameter.
                SimulationParameter simParam = LoadSimulationParameter(parameter, project);
                // Set parameter.
                SetSimulationParameter(simParam);
            }
        }
        #endregion

        #region Method to manage project.
        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="projectID">the new project ID</param>
        /// <param name="comment">the new project comment.</param>
        public void CreateNewProject(string projectID, string comment)
        {
            CreateNewProject(projectID, comment, new List<string>());
        }

        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="projectID">the new project ID.</param>
        /// <param name="comment">the new project comment.</param>
        /// <param name="setDirList">the dm directory.</param>
        public void CreateNewProject(string projectID, string comment, List<string> setDirList)
        {
            try
            {
                CreateProject(projectID, comment);
                m_currentProject.Info.DMDirList.AddRange(setDirList);

                EcellObject model = EcellObject.CreateObject(projectID, "", Constants.xpathModel, "", new List<EcellData>());
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
                Util.ShowErrorDialog(string.Format(MessageResources.ErrCrePrj, projectID) + "\n" + ex.Message);
                CloseProject();
            }

        }

        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="projectID">The "Project" ID</param>
        /// <param name="comment">The comment</param>
        private void CreateProject(string projectID, string comment)
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
                prj = new Project(info, m_env);
                m_currentProject = prj;

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
                m_env.ActionManager.AddAction(new NewProjectAction(projectID, comment));
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
            m_env.Console.WriteLine(string.Format(MessageResources.InfoNewRev,
                m_currentProject.Info.Name, revNo));
            // 
            string revision = Path.Combine(targetDir,Constants.fileProjectXML);
            if (!File.Exists(revision))
                throw new EcellException(string.Format(MessageResources.ErrCreRevision, revNo));
            ProjectInfo info = ProjectInfoLoader.Load(revision);
            info.ProjectType = ProjectType.Revision;
            info.ProjectPath = targetDir;
            info.Save();

        }

        /// <summary>
        /// Load Revision.
        /// </summary>
        /// <param name="revision">the revision string.</param>
        public void LoadRevision(string revision)
        {
            if (m_currentProject == null || m_currentProject.Info.ProjectPath == null)
                return;

            // Load Current Project.
            string oldDir = m_currentProject.Info.ProjectPath;
            string filename = Path.Combine(oldDir, Constants.fileProjectXML);
            if (revision.Equals(Constants.xpathCurrent))
            {
                LoadProject(filename);
                return;
            }

            // Load previous revision.
            string revDir = Path.Combine(oldDir, revision);
            filename = Path.Combine(revDir, Constants.fileProjectXML);
            ProjectInfo info = ProjectInfoLoader.Load(filename);
            info.Name = m_currentProject.Info.Name + "_" + revision;
            info.ProjectType = ProjectType.Revision;

            LoadProject(info);
            m_currentProject.Info.ProjectPath = oldDir;
        }

        /// <summary>
        /// Save the stepping model to the set file name.
        /// </summary>
        /// <param name="fileName">the wrote file name</param>
        private void SaveSteppingModelInfo(string fileName)
        {
            string modelID = m_currentProject.Model.ModelID;
            Encoding enc = Encoding.Unicode;
            File.WriteAllText(fileName, "", Encoding.Unicode);

            // Picks the "Stepper" up.
            List<EcellObject> stepperList
                = m_currentProject.StepperDic[modelID];
            foreach (EcellObject obj in stepperList)
            {
                foreach (EcellData d in obj.Value)
                {
                    if (!d.Gettable || !d.Value.IsDouble)
                        continue;
                    double value = GetPropertyValue4Stepper(obj.Key, d.Name);
                    File.AppendAllText(fileName,
                        d.EntityPath + "," + value.ToString() + "\n",
                        enc);                  
                }
            }


            // Picks the "System" up.
            List<EcellObject> systemList = m_currentProject.SystemDic[modelID];
            Debug.Assert(systemList != null && systemList.Count > 0);

            foreach (EcellObject obj in systemList)
            {
                EcellObject sysObj = obj;
                foreach (EcellData d in sysObj.Value)
                {
                    if (!d.Gettable || !d.Value.IsDouble)
                        continue;
                    double v = GetPropertyValue(d.EntityPath);
                    File.AppendAllText(fileName,
                        d.EntityPath + "," + v.ToString() + "\n",
                        enc);
                }
 
                foreach (EcellObject cobj in sysObj.Children)
                {
                    foreach (EcellData d in cobj.Value)
                    {
                        if (!d.Gettable || !d.Value.IsDouble)
                            continue;
                        double v = GetPropertyValue(d.EntityPath);
                        File.AppendAllText(fileName, 
                            d.EntityPath + "," + v.ToString() + "\n", 
                            enc);
                    }
                }
            }
        }


        /// <summary>
        /// Load the stepping model from file.
        /// </summary>
        /// <param name="fileName">the stepping model file.</param>
        private void LoadSteppingModelInfo(string fileName)
        {
            m_steppingData = new Dictionary<string, EcellValue>();
            string line;
            string[] ele;
            StreamReader reader;
            reader = new StreamReader(fileName, Encoding.Unicode);
            while ((line = reader.ReadLine()) != null)
            {
                ele = line.Split(new char[] { ',' });
                string entPath = ele[0];
                string data = ele[1];
                if (entPath.StartsWith(Constants.xpathStepper))
                {
                    ele = entPath.Split(new char[] { ':' });
                    string key = ele[2];
                    string name = ele[3];
                    EcellValue storedValue
                            = new EcellValue(m_currentProject.Simulator.GetStepperProperty(key, name));
                    if (storedValue.IsDouble)
                        m_steppingData.Add(entPath, new EcellValue(Double.Parse(data)));
                    else if (storedValue.IsInt)
                        m_steppingData.Add(entPath, new EcellValue(Int32.Parse(data)));
                    else
                        m_steppingData.Add(entPath, new EcellValue(data));
                }
                else
                {
                    EcellValue storedValue
                            = new EcellValue(m_currentProject.Simulator.GetEntityProperty(entPath));
                    if (storedValue.IsDouble)
                        m_steppingData.Add(entPath, new EcellValue(Double.Parse(data)));
                    else if (storedValue.IsInt)
                        m_steppingData.Add(entPath, new EcellValue(Int32.Parse(data)));
                    else
                        m_steppingData.Add(entPath, new EcellValue(data));
                }
            }
            reader.Close();
        }


        /// <summary>
        /// Write EML File.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="modelFileName">the model FileName</param>
        /// <param name="isProjectSave">the flag whether project is saved.</param>
        private void SaveEmlFile(string modelID, string modelFileName, bool isProjectSave)
        {
            List<EcellObject> storedList = new List<EcellObject>();
            // Picks the "Stepper" up.
            List<EcellObject> stepperList
                = m_currentProject.StepperDic[modelID];
            Debug.Assert(stepperList != null && stepperList.Count > 0);
            foreach (EcellObject obj in stepperList)
            {
                foreach (EcellData d in obj.Value)
                {
                    if (!d.Gettable || !d.Value.IsDouble)
                        continue;
                    if (m_env.PluginManager.Status == ProjectStatus.Running ||
                        m_env.PluginManager.Status == ProjectStatus.Stepping ||
                        m_env.PluginManager.Status == ProjectStatus.Suspended)
                    {
                        double v = GetPropertyValue4Stepper(obj.Key, d.Name);
                        d.Value = new EcellValue(v);
                    }
                }
            }
            storedList.AddRange(stepperList);

            // Picks the "System" up.
            List<EcellObject> systemList = m_currentProject.SystemDic[modelID];
            Debug.Assert(systemList != null && systemList.Count > 0);
            if (this.CurrentProject.SimulationStatus != SimulationStatus.Wait)
            {
                foreach (EcellObject obj in systemList)
                {
                    //                        EcellObject sysObj = obj.Clone();
                    EcellObject sysObj = obj;
                    if (sysObj.Children == null)
                    {
                        storedList.Add(sysObj);
                        continue;
                    }
                    foreach (EcellObject cobj in sysObj.Children)
                    {
                        foreach (EcellData d in cobj.Value)
                        {
                            if (!d.Gettable || !d.Value.IsDouble)
                                continue;
                            if (m_env.PluginManager.Status == ProjectStatus.Running ||
                                m_env.PluginManager.Status == ProjectStatus.Stepping ||
                                m_env.PluginManager.Status == ProjectStatus.Suspended)
                            {
                                EcellValue v = GetEntityProperty(d.EntityPath);
                                d.Value = v;
                            }
                        }
                    }
                    storedList.Add(sysObj);
                }
            }
            else
            {
                storedList.AddRange(systemList);
            }

            // Save eml.
            EmlWriter.Create(modelFileName, storedList, isProjectSave);
        }

        /// <summary>
        /// Saves the model using the model ID.
        /// </summary>
        /// <param name="modelID">The saved model ID</param>
        internal void SaveModel(string modelID)
        {
            string message = null;
            try
            {
                message = string.Format(MessageResources.InfoSaveModel,
                    new object[] { modelID });
                //
                // Initializes
                //
                Debug.Assert(!string.IsNullOrEmpty(modelID));
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

                SaveEmlFile(modelID, modelFileName, true);

                // Save Leml.
                EcellModel model = (EcellModel)m_currentProject.Model;
                model.Children = m_currentProject.SystemDic[modelID];

                string leml = Path.GetDirectoryName(modelFileName) + Path.DirectorySeparatorChar +
                            Path.GetFileNameWithoutExtension(modelFileName) + Constants.FileExtLEML;
                Leml.SaveLEML(m_env, model, leml);

                Trace.WriteLine("Save Model: " + message);
                m_env.PluginManager.SaveModel(modelID, modelDirName);
            }
            catch (Exception ex)
            {
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
            try
            {
                // Set project dir.
                SetDefaultDir();
                // Save ProjectInfo.
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
                foreach (string name in m_deleteParameterList)
                {
                    string simulationDirName
                        = Path.Combine(Path.Combine(m_defaultDir, m_currentProject.Info.Name), Constants.xpathParameters);
                    if (Directory.Exists(simulationDirName))
                    {
                        string pattern = "_????_??_??_??_??_??_" + name + Constants.FileExtXML;
                        foreach (string fileName in Directory.GetFiles(simulationDirName, pattern))
                        {                            
                            File.Delete(fileName);
                        }
                        string simulationFileName
                                = Path.Combine(simulationDirName, name + Constants.FileExtXML);
                        if (File.Exists(simulationFileName))
                            File.Delete(simulationFileName);
                    }
                }

                SaveSimulationResult();
                SaveSimulationResultDelegate dlg = 
                    m_env.PluginManager.GetDelegate(Constants.delegateSaveSimulationResult) as SaveSimulationResultDelegate;
                if (dlg != null)
                    dlg(logList);

                EcellObject project = EcellObject.CreateObject(m_currentProject.Info.Name, "", Constants.xpathProject, "", new List<EcellData>());
                project.SetEcellValue(Constants.textComment, new EcellValue(m_currentProject.Info.Comment));
                m_env.PluginManager.DataChanged(project.ModelID, project.Key,project.ModelID, project);
                m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
                m_env.Console.WriteLine(string.Format(MessageResources.InfoSavePrj, m_currentProject.Info.Name));
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
        /// Export the simulation parameter to the file.
        /// </summary>
        /// <param name="modelID">the modelID to export simulation parameter.</param>
        /// <param name="parameterID">the parameter ID.</param>
        /// <param name="fileName">the fileName</param>
        public void ExportSimulationParameter(string modelID, string parameterID, string fileName)
        {
            if (m_currentProject == null)
                return;

            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(fileName, false, Encoding.Unicode);
                foreach (EcellObject sysobj in m_currentProject.SystemDic[modelID])
                {
                    foreach (EcellObject child in sysobj.Children)
                    {
                        foreach (EcellData data in child.Value)
                        {
                            if (data.Settable && data.Value.IsDouble)
                            {
                                if (!parameterID.Equals(Constants.defaultSimParam) &&
                                    m_currentProject.InitialCondition[parameterID][modelID].ContainsKey(data.EntityPath))
                                    writer.WriteLine(data.EntityPath + "," + m_currentProject.InitialCondition[parameterID][modelID][data.EntityPath].ToString());
                                else
                                    writer.WriteLine(data.EntityPath + "," + data.Value.Value.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Import the simulation parameter from the file.
        /// </summary>
        /// <param name="modelID">the modelID to import the simulation parameter.</param>
        /// <param name="parameterID">the parameter ID.</param>
        /// <param name="param">the list of initial parameters.</param>
        public void ImportSimulationParameter(string modelID, string parameterID,
            Dictionary<string, double> param)
        {
            if (m_currentProject == null || param == null)
                return;

            Dictionary<string, EcellObject> updateObjs = new Dictionary<string, EcellObject>();
            if (!m_currentProject.InitialCondition.ContainsKey(parameterID))
            {
                CopySimulationParameter(parameterID, Constants.defaultSimParam);
            }

            foreach (string fullPN in param.Keys)
            {
                double value = param[fullPN];

                string type, key, propName;
                double orgValue;
                Util.ParseFullPN(fullPN, out type, out key, out propName);
                EcellObject obj = m_currentProject.GetEcellObject(modelID, type, key, true);
                if (obj == null)
                    continue;
                EcellData d = obj.GetEcellData(propName);
                if (!d.Settable || !d.Value.IsDouble)
                    continue;
                if (!Double.TryParse(d.Value.ToString(), out orgValue))
                    continue;
                if (orgValue == value)
                {
                    if (!parameterID.Equals(Constants.defaultSimParam) &&
                        m_currentProject.InitialCondition[parameterID][modelID].ContainsKey(fullPN))
                        m_currentProject.InitialCondition[parameterID][modelID].Remove(fullPN);
                    continue;
                }
                if (parameterID.Equals(Constants.defaultSimParam))
                {
                    d.Value = new EcellValue(value);
                    updateObjs[key] = obj;
                }
                else
                {
                    m_currentProject.InitialCondition[parameterID][modelID][fullPN] = value;
                }
            }
            if (updateObjs.Count > 0)
            {
                foreach (EcellObject obj in updateObjs.Values)
                    DataChanged(obj.ModelID, obj.Key, obj.Type, obj, true, false);
                this.m_env.ActionManager.AddAction(new AnchorAction());
            }
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
                foreach (string modelID in modelIDList)
                {
                    SaveEmlFile(modelID, fileName, false);
                }
                ////
                //// Searchs the "Stepper" & the "System".
                ////
                //List<EcellObject> storedStepperList = new List<EcellObject>();
                //List<EcellObject> storedSystemList = new List<EcellObject>();

                //Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
                //Dictionary<string, List<EcellObject>> stepperDic = m_currentProject.StepperDic;

                //foreach (string modelID in modelIDList)
                //{
                //    storedStepperList.AddRange(stepperDic[modelID]);
                //    storedSystemList.AddRange(sysDic[modelID]);
                //}
                //Debug.Assert(storedStepperList != null && storedStepperList.Count > 0);
                //Debug.Assert(storedSystemList != null && storedSystemList.Count > 0);

                ////
                //// Exports.
                ////
                //storedStepperList.AddRange(storedSystemList);
                //EmlWriter.Create(fileName, storedStepperList, false);
                //Trace.WriteLine("Export Model: " + message);
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
                    this.m_currentProject = null;
                }

                this.m_env.PluginManager.AdvancedTime(0);
                this.m_env.PluginManager.Clear();
                this.m_env.ActionManager.Clear();
                this.m_env.ReportManager.Clear();
                this.m_env.LoggerManager.Clear();
                this.m_env.JobManager.Clear();
                this.m_parameterList.Clear();
                this.m_observedList.Clear();
                this.m_loggerEntry.Clear();

                m_env.PluginManager.ChangeStatus(ProjectStatus.Uninitialized);
                this.ClearSteppingModel();
                m_deleteParameterList.Clear();
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
        /// <param name="ecellObject">the added object.</param>
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
                ConfirmReset("add", type);

                if (type.Equals(Constants.xpathProcess) || type.Equals(Constants.xpathVariable) || type.Equals(Constants.xpathText))
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
                    DataAdd4Stepper(ecellObject);
                    usableList.Add(ecellObject);
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
                   string.Format(MessageResources.ErrAdd,
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
                            m_env.ActionManager.AddAction(new DataAddAction(obj, isUndoable));
                        }
                    }
                    m_env.PluginManager.DataAdd(usableList);
                    if (type.Equals(EcellObject.SYSTEM))
                        m_env.PluginManager.RaiseRefreshEvent();
                    if (isRecorded && isAnchor)
                        this.m_env.ActionManager.AddAction(new AnchorAction());
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
            //
            // Sets the "Model".
            //
            modelList.Add((EcellModel)ecellObject);
            usableList.Add(ecellObject);
            //
            // Sets the root "System".
            //
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
            m_currentProject.SetSimParams(modelID);
            foreach (string simParam in m_currentProject.InitialCondition.Keys)
            {
                // Sets initial conditions.
                m_currentProject.StepperDic[modelID] = new List<EcellObject>();
                m_currentProject.StepperDic[modelID].Add(dic[Constants.xpathStepper]);
                usableList.Add(dic[Constants.xpathStepper]);
                m_currentProject.LoggerPolicyDic[simParam] = new LoggerPolicy();
            }
            //
            // Messages
            //
            string message = string.Format(MessageResources.InfoAdd,
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
            DataStorer.DataStored4Stepper(simulator, m_env.DMDescriptorKeeper, stepperEcellObject);
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
            AddStepperID(ecellObject);
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

            // Check duplicated system.
            foreach (EcellObject sys in sysDic[modelID])
            {
                if (!sys.Key.Equals(system.Key))
                    continue;
                throw new EcellException(string.Format(MessageResources.ErrAdd,
                    new object[] { type, system.Key }));
            }
            CheckEntityPath(system);
            m_currentProject.AddSystem(system);

            SetLogger(system);

            // Show Message.
            if (messageFlag)
            {
                MessageCreateEntity(EcellObject.SYSTEM,
                    string.Format(MessageResources.InfoAdd,
                    new object[] { type, system.Key }));
            }
        }

        private void SetLogger(EcellObject obj)
        {
            foreach (EcellData d in obj.Value)
            {
                if (d.Logged)
                {
                    m_env.LoggerManager.AddLoggerEntry(
                        obj.ModelID, obj.Key, obj.Type, d.EntityPath);
                }
            }
            foreach (EcellObject child in obj.Children)
            {
                SetLogger(child);
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

            // Check Stepper
            if (entity is EcellProcess)
            {
                EcellProcess process = (EcellProcess)entity;
                string stepperID = process.StepperID;
                EcellObject stepper = null;
                foreach (EcellObject s in GetStepper(modelID))
                {
                    if (s.Key.Equals(stepperID))
                        stepper = s;
                }
                if (stepper == null)
                    process.StepperID = GetStepper(entity.ModelID)[0].Key;
            }

            // Add object.
            bool findFlag = false;
            foreach (EcellObject system in sysDic[modelID])
            {
                if (!system.ModelID.Equals(modelID) || !system.Key.Equals(systemKey))
                    continue;
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
                SetLogger(entity);

                findFlag = true;
                break;
            }

            if (messageFlag)
            {
                MessageCreateEntity(type, string.Format(MessageResources.InfoAdd,
                    new object[] { type, entity.Key }));
            }
            Debug.Assert(findFlag);
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
            Debug.Assert(!string.IsNullOrEmpty(modelID));
            Debug.Assert(!string.IsNullOrEmpty(key));
            Debug.Assert(!string.IsNullOrEmpty(type));

            // DataChange for simulation.
            try
            {
                // StatusCheck
                if (m_currentProject.SimulationStatus == SimulationStatus.Run ||
                    m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    EcellObject obj = GetEcellObject(modelID, key, type);
                    // Confirm Reset.
                    if (!key.Equals(ecellObject.Key) ||
                        !obj.Classname.Equals(ecellObject.Classname) ||
                        obj.Value.Count != ecellObject.Value.Count ||
                        (obj is EcellProcess && Util.DoesVariableReferenceChange(obj, ecellObject)) ||
                        (obj is EcellProcess && Util.DoesExpressionChange(obj, ecellObject)) ||
                        (obj is EcellProcess && Util.DoesActivityChange(obj, ecellObject)) ||
                        (obj is EcellSystem && ((EcellSystem)obj).SizeInVolume != ((EcellSystem)ecellObject).SizeInVolume))
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
            if (key != ecellObject.Key && type != EcellObject.MODEL &&
                GetEcellObject(ecellObject.ModelID, ecellObject.Key, ecellObject.Type) != null)
            {
                throw new EcellException(string.Format(MessageResources.ErrExistObj, ecellObject.Key));
            }

            try
            {
                EcellObject oldObj = GetEcellObject(modelID, key, type);
                // Checks the EcellObject
                CheckEntityPath(ecellObject);
                // Record action
                if (isRecorded)
                    this.m_env.ActionManager.AddAction(new DataChangeAction(CurrentProject.Info.SimulationParam, oldObj, ecellObject));

                // 4 System & Entity
                if (ecellObject.Type.Equals(Constants.xpathModel))
                {
                    DataChanged4Model(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }
                else if (ecellObject.Type.Equals(Constants.xpathSystem))
                {
                    DataChanged4System(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }
                else if (ecellObject.Type.Equals(Constants.xpathProcess) || ecellObject.Type.Equals(Constants.xpathText))
                {
                    DataChanged4Entity(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }
                else if (ecellObject.Type.Equals(Constants.xpathVariable))
                {
                    UpdatePropertyForDataChanged(ecellObject, null);
                    DataChanged4Entity(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }
                else if (ecellObject.Type.Equals(Constants.xpathStepper))
                {
                    DataChanged4Stepper(modelID, key, type, ecellObject, isRecorded, isAnchor);
                }

                if (key != ecellObject.Key)
                {
                    CheckParameterObservedData(oldObj, ecellObject.Key);
                }
                if (IsCheckNecessary(oldObj, ecellObject))
                {
                    CheckDeleteParameterData(oldObj, ecellObject);
                }
                CheckLoggerData(oldObj, ecellObject);                

                //if (!oldObj.IsPosSet)
                //    m_env.PluginManager.SetPosition(oldObj);
                // Set Event Anchor.
                // Record Action.
                if (isRecorded && isAnchor)
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
        /// Check whether the properties of this object is added or deleted.
        /// </summary>
        /// <param name="oldObj">the old object.</param>
        /// <param name="newObj">the new object.</param>
        /// <returns>if changed, return true.</returns>
        private bool IsCheckNecessary(EcellObject oldObj, EcellObject newObj)
        {
            if (oldObj.Value.Count > newObj.Value.Count)
                return true;

            foreach (EcellData d in oldObj.Value)
            {
                if (newObj.GetEcellData(d.Name) == null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Changes the "Model".
        /// </summary>
        /// <param name="modelID">the model ID of original object.</param>
        /// <param name="key">the key of original object.</param>
        /// <param name="type">the type of original object.</param>
        /// <param name="ecellObject">the changed object.</param>
        /// <param name="isRecorded">the flag whether this action is recorded.</param>
        /// <param name="isAnchor">the flag whether this action is anchor.</param>
        private void DataChanged4Model(string modelID, string key, string type, EcellObject ecellObject, bool isRecorded, bool isAnchor)
        {
            if (!modelID.Equals(ecellObject.ModelID))
            {
                // ToDo: モデル名が変更された場合の処理（各オブジェクトのモデルIDの変更とDataChangedEventの実行）を記述する。
                // Caution! 現時点ではモデル名の変更はできません。
                throw new Exception("The method to change ModelID is not implemented.");
            }

            m_currentProject.ModelList[0].Layers = ((EcellModel)ecellObject).Layers;
            m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
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
            EcellObject oldNode = m_currentProject.GetEcellObject(modelID, type, key, false);
            Debug.Assert(oldNode != null);

            string paramId = m_currentProject.Info.SimulationParam;
            if (Constants.defaultSimParam.Equals(paramId))
                paramId = null;

            CheckDifferences(oldNode, ecellObject, paramId);
            if (key.Equals(ecellObject.Key))
            {
                if (m_currentProject.Info.SimulationParam.Equals(Constants.defaultSimParam) ||
                    !oldNode.Classname.Equals(ecellObject.Classname) || oldNode.Value.Count != ecellObject.Value.Count)
                {
                    EcellObject oldSystem = m_currentProject.GetEcellObject(modelID, Constants.xpathSystem, Util.GetSuperSystemPath(key), true);
                    Debug.Assert(oldSystem != null);
                    oldSystem.Children.Remove(oldNode);
                    oldSystem.Children.Add(ecellObject);
                }
                else
                {
                    m_currentProject.DeleteInitialCondition(oldNode);
                    m_currentProject.SetInitialCondition(ecellObject);
                }
                m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
                return;
            }

            // Get parent system.
            // Add new object.
            DataAdd4Entity(ecellObject.Clone(), false);
            m_currentProject.UpdateInitialCondition(oldNode, ecellObject);
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

        private void CheckMassCalc(EcellObject src, EcellObject dest)
        {
            EcellProcess process = (EcellProcess)dest;
            List<EcellReference> list = process.ReferenceList;
            if (dest.Classname == EcellProcess.MASSCALCULATIONPROCESS)
            {
                foreach (EcellReference er in list)
                {
                    er.Name = "1";
                    er.Coefficient = 0;
                }
            }
            else if (src.Classname == EcellProcess.MASSCALCULATIONPROCESS)
            {
                int i = 0;
                foreach (EcellReference er in list)
                {
                    er.Name = "C" + i.ToString();
                    i++;
                }

            }
            process.ReferenceList = list;
        }

        private void DataChanged4Stepper(
            string modelID, string key, string type, EcellObject ecellObject, bool isRecorded, bool isAnchor)
        {
            UpdateStepperID(key, ecellObject, isRecorded);
            m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);

            if (key.Equals(ecellObject.Key)) return;
            foreach (EcellObject sysObj in m_currentProject.SystemDic[modelID])
            {
                EcellData data = sysObj.GetEcellData(Constants.xpathStepperID);
                if (data != null && key.Equals(data.Value.Value.ToString()))
                {
                    data.Value = new EcellValue(ecellObject.Key);
                    m_env.PluginManager.DataChanged(
                        sysObj.ModelID, sysObj.Key, sysObj.Type, sysObj);
                }
                if (sysObj.Children == null) continue;
                foreach (EcellObject obj in sysObj.Children)
                {
                    EcellData cdata = obj.GetEcellData(Constants.xpathStepperID);
                    if (cdata != null && key.Equals(cdata.Value.Value.ToString()))
                    {
                        cdata.Value = new EcellValue(ecellObject.Key);
                        m_env.PluginManager.DataChanged(
                            obj.ModelID, obj.Key, obj.Type, obj);
                    }
                }
            }
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

            string paramId = m_currentProject.Info.SimulationParam;
            if (Constants.defaultSimParam.Equals(paramId))
                paramId = null;

            // When system path is not modified.
            if (modelID.Equals(ecellObject.ModelID) && key.Equals(ecellObject.Key))
            {
                // Changes some properties.
                for (int i = 0; i < systemList.Count; i++)
                {
                    if (!systemList[i].Key.Equals(key))
                        continue;                   

                    CheckDifferences(systemList[i], ecellObject, paramId);

                    if (m_currentProject.Info.SimulationParam.Equals(Constants.defaultSimParam))
                    {
                        systemList[i] = ecellObject.Clone();
                    }
                    else
                    {
                        m_currentProject.DeleteInitialCondition(systemList[i]);
                        m_currentProject.SetInitialCondition(ecellObject);
                    }

                    m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
                    return;
                }
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
                CheckDifferences(system, newSystem, paramId);
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
                    CheckLogger(child, copy.Key);

                    oldKeyDic.Add(copy.Key, child.Key);
                    if (copy.Type.Equals(Constants.xpathVariable))
                        variableKeyDic.Add(child.Key, copy.Key);
                }
            }

            ecellObject = m_currentProject.GetEcellObject(modelID, Constants.xpathSystem, ecellObject.Key, false);
            m_env.PluginManager.DataChanged(modelID, key, ecellObject.Type, ecellObject);
            // Checks all processes.
            m_currentProject.SortSystems();
            List<EcellObject> processList = CheckVariableReferenceChanges(variableKeyDic);
            DataChanged(processList, true, false);
        }

        /// <summary>
        /// SetPosition
        /// </summary>
        /// <param name="eo">ths position changed object.</param>
        public void SetPosition(EcellObject eo)
        {
            EcellObject oldNode = m_currentProject.GetEcellObject(eo.ModelID, eo.Type, eo.Key, false);
            oldNode.SetPosition(eo);
            // not implement.
//            m_env.PluginManager.SetPosition(oldNode.Clone());            
            m_env.PluginManager.SetPosition(oldNode);
        }

        #endregion

        #region Method for DataDelete
        /// <summary>
        /// Deletes the "EcellObject".
        /// </summary>
        /// <param name="eo">the deleted object.</param>
        public void DataDelete(EcellObject eo)
        {
            DataDelete(eo.ModelID, eo.Key, eo.Type, true, true);
        }

        /// <summary>
        /// Deletes the "EcellObject".
        /// </summary>
        /// <param name="eo">the deleted object.</param>
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
                ConfirmReset("delete", type);
            }
            catch (IgnoreException)
            {
                // CancelしたときにIgnoreExceptionが発生するが無視しないと
                // Cancelしているのにエラーダイアログが表示されてしまう
                return;
            }

            // Check Model
            if (string.IsNullOrEmpty(modelID))
                return;
            if (type.Equals(EcellObject.MODEL))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrDelete, EcellObject.MODEL));
                return;
            }
            // Check root
            if (key.Equals("/"))
            {
                Util.ShowErrorDialog(MessageResources.ErrDelRoot);
                return;
            }

            // Check Object;
            EcellObject deleteObj = GetEcellObject(modelID, key, type);
            if (deleteObj == null)
                return;

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
                //                    DataDelete4Model(modelID);
            }
            else if (key.Contains(":") || type.Equals(Constants.xpathStepper))
            { // not system
                m_env.LoggerManager.NodeRemoved(deleteObj);
                DataDelete4Node(modelID, key, type, true, isRecorded, false);
            }
            else
            { // system
                m_env.LoggerManager.SystemRemoved(deleteObj);
                DataDelete4System(modelID, key, true, isRecorded);
            }
            m_env.PluginManager.DataDelete(modelID, key, type);
            if (isRecorded)
                m_env.ActionManager.AddAction(new DataDeleteAction(deleteObj));
            if (type.Equals(EcellObject.SYSTEM))
                m_env.PluginManager.RaiseRefreshEvent();
            if (isRecorded && isAnchor)
                this.m_env.ActionManager.AddAction(new AnchorAction());
        }

        ///// <summary>
        ///// Deletes the "Model" using the model ID.
        ///// </summary>
        ///// <param name="modelID">The model ID</param>
        //private void DataDelete4Model(string modelID)
        //{
        //    string message = "[" + modelID + "]";
        //    //
        //    // Delete the "Model".
        //    //
        //    bool isDelete = false;
        //    foreach (EcellObject obj in m_currentProject.ModelList)
        //    {
        //        if (obj.ModelID == modelID)
        //        {
        //            m_currentProject.ModelList.Remove((EcellModel)obj);
        //            isDelete = true;
        //            break;
        //        }
        //    }
        //    Debug.Assert(isDelete);

        //    // Deletes "System"s.
        //    if (m_currentProject.SystemDic.ContainsKey(modelID))
        //    {
        //        m_currentProject.SystemDic.Remove(modelID);
        //    }
        //    // Deletes "Stepper"s.
        //    if (m_currentProject.StepperDic.ContainsKey(modelID))
        //    {
        //        m_currentProject.StepperDic.Remove(modelID);
        //    }
        //    MessageDeleteEntity(EcellObject.MODEL, message);
        //}

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
                    m_env.ActionManager.AddAction(new DataDeleteAction(sys));
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
            // Show Message.
            string message = "[" + model + "][" + key + "]";
            if (messageFlag)
                MessageDeleteEntity(type, message);

            // Delete node.
            EcellObject node = GetEcellObject(model, key, type);
            if (node != null)
            {
                m_env.LoggerManager.NodeRemoved(node);
                m_currentProject.DeleteInitialCondition(node);
                m_currentProject.DeleteEntity(node);
            }

            // Update VariableReference.
            if (node is EcellVariable)
            {
                List<EcellObject> processList = CheckVariableReferenceChanges(key, null);
                DataChanged(processList, isRecorded, false);
            }

            if (node is EcellStepper)
            {
                DeleteStepperID(node);
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
                throw new EcellException(string.Format(MessageResources.ErrFindEnt, new object[] { sysKey }));
            // CheckRoot
            if (system.Key.Equals("/"))
                throw new EcellException(MessageResources.ErrDelRoot);

            try
            {
                ConfirmReset("merge", sysKey);
            }
            catch (IgnoreException)
            {
                // CancelしたときにIgnoreExceptionが発生するが無視しないと
                // Cancelしているのにエラーダイアログが表示されてしまう
                return;
            }

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
                    throw new EcellException(string.Format(MessageResources.ErrExistObj,
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
                CheckMassCalc(src, dest);
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
                        //foreach (string keyParameterID in initialCondition.Keys)
                        //{
                        //    Dictionary<string, double> condition = initialCondition[keyParameterID][src.ModelID];
                        //    if (condition.ContainsKey(srcEcellData.EntityPath))
                        //    {
                        //        condition.Remove(srcEcellData.EntityPath);
                        //    }
                        //}
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
                        //foreach (string keyParameterID in initialCondition.Keys)
                        //{
                        //    initialCondition[keyParameterID][dest.ModelID][destEcellData.EntityPath] = temp;
                        //}
                    }
                }
            }
            else
            {
                foreach (EcellData srcEcellData in src.Value)
                {
                    bool isHit = false;
                    foreach (EcellData destEcellData in dest.Value)
                    {
                        if (!srcEcellData.Name.Equals(destEcellData.Name) ||
                            !srcEcellData.EntityPath.Equals(destEcellData.EntityPath))
                            continue;
                        isHit = true;

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
                        if (srcEcellData.Value != destEcellData.Value &&
                            !srcEcellData.Value.ToString()
                                .Equals(destEcellData.Value.ToString()))
                        {
                            string srcdata = "";
                            string destdata = "";
                            if (srcEcellData.Value != null)
                                srcdata = srcEcellData.Value.ToString();
                            if (destEcellData.Value != null)
                                destdata = destEcellData.Value.ToString();

                            Trace.WriteLine(
                                "Update Data: " + message
                                    + "[" + srcEcellData.Name + "]"
                                    + System.Environment.NewLine
                                    + "\t[" + srcdata
                                    + "]->[" + destdata + "]");
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
                            //foreach (string keyParameterID in initialCondition.Keys)
                            //{
                            //    Dictionary<string, double> condition = initialCondition[keyParameterID][src.ModelID];

                            //    if (!condition.ContainsKey(srcEcellData.EntityPath))
                            //        continue;
                            //    condition[srcEcellData.EntityPath] = temp;
                            //}
                        }
                        break;
                    }
                    if (isHit == false)
                    {

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
            if (ecellObject.Type.Equals(Constants.xpathSystem))
            {
                string entityPath = ecellObject.FullID + Constants.delimiterColon;
                if (ecellObject.Value != null && ecellObject.Value.Count > 0)
                {
                    foreach (EcellData data in ecellObject.Value)
                    {
                        if (!data.EntityPath.Equals(entityPath + data.Name))
                        {
                            data.EntityPath = entityPath + data.Name;
                        }
                    }
                }
                if (ecellObject.Children != null && ecellObject.Children.Count > 0)
                {
                    foreach (EcellObject child in ecellObject.Children)
                    {
                        CheckEntityPath(child);
                    }
                }
            }
            else if (ecellObject.Type.Equals(Constants.xpathProcess) || 
                ecellObject.Type.Equals(Constants.xpathVariable) || 
                ecellObject.Type.Equals(Constants.xpathStepper))
            {
                string entityPath = ecellObject.FullID + Constants.delimiterColon;
                if (ecellObject.Value != null && ecellObject.Value.Count > 0)
                {
                    foreach (EcellData data in ecellObject.Value)
                    {
                        if (!data.EntityPath.Equals(entityPath + data.Name))
                        {
                            data.EntityPath = entityPath + data.Name;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Process list to update VariableReference.
        /// </summary>
        /// <param name="oldKey">the old key of object.</param>
        /// <param name="newKey">the new key of object.</param>
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
        /// <param name="variableDic">the list of Variable.</param>
        /// <param name="processList">the list of Process.</param>
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
        /// <param name="action">the action string.</param>
        /// <param name="type">the changed type.</param>
        public void ConfirmReset(string action, string type)
        {
            if (m_currentProject == null)
                return;
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


        private void CheckLoggerData(EcellObject oldObj, EcellObject newObj)
        {
            List<LoggerEntry> entList = m_env.LoggerManager.GetLoggerEntryForObject(oldObj.Key, oldObj.Type);

            foreach (EcellData nd in newObj.Value)
            {
                EcellData od = oldObj.GetEcellData(nd.Name);
                if (od != null && od.Logged == nd.Logged)
                    continue;
                if (nd.Logged)
                {
                    m_env.LoggerManager.AddLoggerEntry(newObj.ModelID, newObj.Key, newObj.Type, nd.EntityPath);
                }
                else
                {
                    if (od != null)
                        m_env.LoggerManager.LoggerRemoved(new LoggerEntry(newObj.ModelID, newObj.Key, newObj.Type, nd.EntityPath));
                }
            }
        }        

        /// <summary>
        /// Check the deleted property in paramater or observed data.
        /// </summary>
        /// <param name="oldObj">the old object.</param>
        /// <param name="newObj">the new object.</param>
        private void CheckDeleteParameterData(EcellObject oldObj, EcellObject newObj)
        {           
            List<string> delList = new List<string>();
            foreach (EcellData oldData in oldObj.Value)
            {
                if (newObj.GetEcellData(oldData.Name) == null)
                    delList.Add(oldData.EntityPath);
            }

            foreach (string entPath in delList)
            {
                EcellParameterData p = GetParameterData(entPath);
                if (p != null)
                    RemoveParameterData(p);
                EcellObservedData o = GetObservedData(entPath);
                if (o != null )
                    RemoveObservedData(o);
            }


            foreach (string paramID in m_currentProject.InitialCondition.Keys)
            {
                foreach (string modelID in m_currentProject.InitialCondition[paramID].Keys)
                {
                    foreach (string entPath in delList)
                    {
                        if (m_currentProject.InitialCondition[paramID][modelID].ContainsKey(entPath))
                            m_currentProject.InitialCondition[paramID][modelID].Remove(entPath);
                    }
                }
            }
        }

        /// <summary>
        /// Check parameters.
        /// </summary>
        /// <param name="oldObj">the old object.</param>
        /// <param name="newkey">the changed new object.</param>
        private void CheckParameterObservedData(EcellObject oldObj, string newkey)
        {
            foreach (EcellData data in oldObj.Value)
            {
                EcellParameterData param = GetParameterData(data.EntityPath);
                if (param != null)
                {
                    RemoveParameterData(new EcellParameterData(data.EntityPath, 0.0));
                    SetParameterData(new EcellParameterData(data.EntityPath.Replace(oldObj.Key, newkey),
                        param.Max, param.Min, param.Step));
                }
                EcellObservedData observed = GetObservedData(data.EntityPath);
                if (observed != null)
                {
                    RemoveObservedData(new EcellObservedData(data.EntityPath, 0.0));
                    SetObservedData(new EcellObservedData(data.EntityPath.Replace(oldObj.Key, newkey),
                        observed.Max, observed.Min, observed.Differ, observed.Rate));
                }
            }
        }

        private void CheckLogger(EcellObject oldObj, string newkey)
        {
            foreach (EcellData data in oldObj.Value)
            {
                if (data.Logged)
                {
                    LoggerEntry ent = m_env.LoggerManager.GetLoggerEntryForFullPN(data.EntityPath);                    
                    ent.FullPN = data.EntityPath.Replace(oldObj.Key, newkey);
                    ent.ID = newkey;
                    m_env.LoggerManager.LoggerChanged(data.EntityPath, ent);
                }
            }
        }

        #endregion

        #region Create Default Object.
        /// <summary>
        /// Create the default object(Process, Variable and System).
        /// </summary>
        /// <param name="modelID">the model ID of created object.</param>
        /// <param name="systemKey">the system path of parent object.</param>
        /// <param name="type">the type of created object.</param>
        /// <returns>the create object.</returns>
        public EcellObject CreateDefaultObject(string modelID, string systemKey, string type)
        {
            EcellObject obj = null;
            if (type.Equals(Constants.xpathSystem))
            {
                obj = CreateDefaultSystem(modelID, systemKey);
            }
            else if (type.Equals(Constants.xpathProcess))
            {
                obj = CreateDefaultProcess(modelID, systemKey);
            }
            else if (type.Equals(Constants.xpathVariable))
            {
                obj = CreateDefaultVariable(modelID, systemKey);
            }
            else if (type.Equals(Constants.xpathText))
            {
                obj = CreateDefaultText(modelID);
            }
            else if (type.Equals(Constants.xpathStepper))
            {                
                obj = CreateDefaultStepper(modelID, GetTemporaryID(modelID, type, ""));
            }
            return obj;
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

        private EcellObject CreateDefaultStepper(string modelID, string key)
        {
            List<EcellData> data = GetStepperProperty(Constants.DefaultStepperName);
            EcellObject obj = EcellObject.CreateObject(modelID, key,
                    Constants.xpathStepper, Constants.DefaultStepperName, data);
            return obj;
        }

        /// <summary>
        /// Create the process with temporary ID.
        /// </summary>
        /// <param name="modelID">model ID of created object.</param>
        /// <param name="key">the key of parent system object.</param>
        /// <returns>the create object.</returns>
        private EcellObject CreateDefaultProcess(string modelID, string key)
        {
            string tmpID = GetTemporaryID(modelID, Constants.xpathProcess, key);

            // Get Default StepperID.
            EcellObject sysobj = GetEcellObject(modelID, key, Constants.xpathSystem);
            if (sysobj == null)
                return null;
            string stepperID = "";
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
            string tmpID = GetTemporaryID(modelID, Constants.xpathVariable, key);

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
            string tmpID = GetTemporaryID(modelID, Constants.xpathSystem, key);

            EcellObject sysobj = GetEcellObject(modelID, key, Constants.xpathSystem);
            if (sysobj == null)
                return null;
            string stepperID = "";
            foreach (EcellData d in sysobj.Value)
            {
                if (!d.Name.Equals(Constants.xpathStepperID))
                    continue;
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
            // 20090727
            sim.Dispose();
            return dic;
        }

        /// <summary>
        /// Update the property when DataChanged is executed,
        /// </summary>
        /// <param name="variable">the variable object.</param>
        /// <param name="updateData">the update data.</param>
        public void UpdatePropertyForDataChanged(EcellObject variable, EcellData updateData)
        {
            if (variable.Key.EndsWith(EcellSystem.SIZE)) return;
            string variablePath = Constants.delimiterPath + Constants.delimiterColon + "V0";
            Dictionary<string, EcellData> dic = new Dictionary<string, EcellData>();
            string sysID = variable.ParentSystemID;
            EcellObject sys = GetEcellObject(variable.ModelID, sysID, Constants.xpathSystem);           
//            EcellData updateData = null;
            EcellObject org = GetEcellObject(variable.ModelID, variable.Key, variable.Type);
            if (org == null) return;

            if (updateData == null)
            {
                foreach (EcellData orgdata in org.Value)
                {
                    if (!orgdata.Settable) continue;
                    updateData = variable.GetEcellData(orgdata.Name);
                    if (updateData == null) continue;
                    if (!updateData.Value.ToString().Equals(orgdata.Value.ToString()))
                    {
                        break;
                    }
                    updateData = null;
                }
            }
            if (updateData == null) return;

            string sizePath = Constants.delimiterPath + Constants.delimiterColon + Constants.xpathSize.ToUpper();
            string sizeValuePath = Constants.xpathVariable + Constants.delimiterColon + sizePath +
                Constants.delimiterColon + Constants.xpathValue;
            WrappedSimulator sim = null;
            EcellObject dummySizeObject = null;
            EcellObject variableObject = null;
            try
            {
                sim = m_currentProject.CreateSimulatorInstance();
                BuildDefaultSimulator(sim, null, null);
                dummySizeObject = EcellObject.CreateObject(
                    "",
                    sizePath,
                    EcellObject.VARIABLE,
                    EcellObject.VARIABLE,
                    null
                    );
                sim.SetEntityProperty(sizeValuePath, ((EcellSystem)sys).SizeInVolume);
                sim.CreateEntity(Constants.xpathVariable, Constants.xpathVariable + Constants.delimiterColon + variablePath);
                foreach (EcellData data in variable.Value)
                {
                    if (!data.Settable) continue;
                    if (data.Name.Equals(updateData.Name)) continue;
                    sim.SetEntityProperty(
                        Constants.xpathVariable + Constants.delimiterColon + variablePath +
                        Constants.delimiterColon + data.Name, data.Value.Value);
                }
                sim.SetEntityProperty(
                    Constants.xpathVariable + Constants.delimiterColon + variablePath +
                    Constants.delimiterColon + updateData.Name, updateData.Value.Value);

                variableObject = EcellObject.CreateObject(
                    "",
                    variablePath,
                    EcellObject.VARIABLE,
                    EcellObject.VARIABLE,
                    null
                    );
                DataStorer.DataStored4Variable(
                    sim,
                    variableObject,
                    new Dictionary<string, double>());

                SetPropertyList(variableObject, dic);
                foreach (string name in dic.Keys)
                {
                    EcellData tmp = variable.GetEcellData(name);
                    tmp.Value = dic[name].Value;
                }
            }
            finally
            {
                // 20090727
                sim.Dispose();
                sim = null;
                variableObject = null;
            }
        }

        /// <summary>
        /// Returns the list of the "Variable" property. 
        /// </summary>
        /// <returns>The dictionary of the "Variable" property</returns>
        public Dictionary<string, EcellData> GetVariableProperty()
        {
            Dictionary<string, EcellData> dic = new Dictionary<string, EcellData>();
            WrappedSimulator sim = null;
            EcellObject dummyEcellObject = null;
            try
            {
                sim = m_currentProject.CreateSimulatorInstance();
                BuildDefaultSimulator(sim, null, null);
                dummyEcellObject = EcellObject.CreateObject(
                    "",
                    Constants.delimiterPath + Constants.delimiterColon + Constants.xpathSize.ToUpper(),
                    EcellObject.VARIABLE,
                    EcellObject.VARIABLE,
                    null
                    );
                DataStorer.DataStored4Variable(
                        sim,
                        dummyEcellObject,
                        new Dictionary<string, double>());
                SetPropertyList(dummyEcellObject, dic);
                // 20090727
                sim.Dispose();
            }
            finally
            {
                sim = null;
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
                WrappedSimulator sim = m_currentProject.CreateSimulatorInstance();
                string key = Constants.delimiterPath + Constants.delimiterColon + "tmp";
                //sim.CreateStepper("ODEStepper", "temporaryStepper");
                //sim.SetEntityProperty("System::/:StepperID", "temporaryStepper");
                sim.CreateEntity(dmName,
                    Constants.xpathProcess + Constants.delimiterColon + key);
                EcellObject dummyEcellObject = EcellObject.CreateObject("", key, EcellObject.PROCESS, dmName, new List<EcellData>());
                DataStorer.DataStored4Process(
                        sim,
                        m_env.DMDescriptorKeeper,
                        dummyEcellObject,
                        new Dictionary<string, double>());
                //string key = Constants.delimiterPath + Constants.delimiterColon + Constants.xpathSize.ToUpper();
                //BuildDefaultSimulator(sim, dmName, "ODEStepper");
                //EcellObject dummyEcellObject = EcellObject.CreateObject("",
                //    key, EcellObject.PROCESS, dmName, null);
                //DataStorer.DataStored4Process(
                //        sim,
                //        m_env.DMDescriptorKeeper,
                //        dummyEcellObject,
                //        new Dictionary<string, double>());
                SetPropertyList(dummyEcellObject, dic);
                // 20090727
                sim.Dispose();
            }
            catch (Exception ex)
            {
                throw new EcellException(
                    string.Format(MessageResources.ErrGetProp,
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
                DataStorer.DataStored4Stepper(sim, m_env.DMDescriptorKeeper, dummyEcellObject);
                list = dummyEcellObject.Value;
                // 20090727
                sim.Dispose();
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
            if (m_currentProject == null)
                return null;
            EcellObject obj = m_currentProject.GetEcellObject(modelId, type, key, false);
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
        public bool IsDataExists(string modelID, string key, string type)
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
        /// <param name="type">The entity name</param>
        /// <returns></returns>
        public List<string> GetEntityList(string modelID, string type)
        {
            List<string> entityList = new List<string>();
            try
            {
                foreach (EcellObject system in m_currentProject.SystemDic[modelID])
                {
                    if (type.Equals(Constants.xpathSystem))
                    {
                        entityList.Add(system.FullID);
                    }
                    else
                    {
                        if (system.Children == null || system.Children.Count <= 0)
                            continue;
                        foreach (EcellObject entity in system.Children)
                        {
                            if (!entity.Type.Equals(type))
                                continue;
                            entityList.Add(entity.FullID);
                        }
                    }
                }
                return entityList;
            }
            catch (Exception ex)
            {
                entityList.Clear();
                entityList = null;
                throw new EcellException(string.Format(MessageResources.ErrFindEnt,
                    new object[] { type }), ex);
            }
        }

        #endregion

        #region Method for Stepper
        /// <summary>
        /// Adds the new "Stepper"
        /// </summary>
        /// <param name="stepper">The "Stepper"</param>
        public void AddStepperID(EcellObject stepper)
        {
            AddStepperID(stepper, true);
        }

        /// <summary>
        /// Adds the new "Stepper"
        /// </summary>
        /// <param name="stepper">The "Stepper"</param>
        /// <param name="isRecorded">Whether this action is recorded</param>
        public void AddStepperID(EcellObject stepper, bool isRecorded)
        {
            // Check parameters.
            if (stepper == null || string.IsNullOrEmpty(stepper.ModelID))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, ""));

            string message = null;
            try
            {
                CheckEntityPath(stepper);

                // Get stepperDic
                message = "[" + stepper.ModelID + "][" + stepper.Key + "]";
                Dictionary<string, List<EcellObject>> stepperDic = m_currentProject.StepperDic;
                if (!stepperDic.ContainsKey(stepper.ModelID))
                    throw new EcellException();

                // Check duplication.
                foreach (EcellObject storedStepper in stepperDic[stepper.ModelID])
                {
                    if (!stepper.Key.Equals(storedStepper.Key))
                        continue;
                    throw new EcellException(string.Format(MessageResources.ErrExistStepper, stepper.Key));
                }
                // Set Stteper.
                stepperDic[stepper.ModelID].Add(stepper);
            }
            catch (Exception ex)
            {
                message = string.Format(MessageResources.ErrNotCreStepper, stepper.Key);
                Trace.WriteLine(message);
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Updates the "Stepper".
        /// </summary>
        /// <param name="orgStepperID">The parameter ID</param>
        /// <param name="newStepper">The list of the "Stepper"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        public void UpdateStepperID(string orgStepperID, EcellObject newStepper, bool isRecorded)
        {
            EcellObject oldStepepr = null;
            Dictionary<string, List<EcellObject>> perParameterStepperListDic = m_currentProject.StepperDic;
            if (m_currentProject.Info.SimulationParam.Equals(Constants.defaultSimParam) ||
                !newStepper.Key.Equals(orgStepperID))
            {
                foreach (EcellObject model in m_currentProject.ModelList)
                {
                    foreach (EcellObject obj in perParameterStepperListDic[model.ModelID])
                    {
                        if (obj.Key.Equals(orgStepperID))
                        {
                            oldStepepr = obj;
                            perParameterStepperListDic[model.ModelID].Remove(obj);
                            break;
                        }
                    }
                    perParameterStepperListDic[model.ModelID].Add(newStepper);
                }
                //Debug.Assert(oldStepepr != null);
                //m_env.ActionManager.AddAction(
                //    new ChangeStepperAction(newStepper.Key, orgStepperID, newStepper, oldStepepr));
            }
            else
            {
                foreach (EcellObject model in m_currentProject.ModelList)
                {
                    foreach (EcellObject obj in perParameterStepperListDic[model.ModelID])
                    {
                        if (obj.Key.Equals(orgStepperID))
                        {
                            oldStepepr = obj;
                            break;
                        }
                    }
                }
                m_currentProject.DeleteInitialCondition(oldStepepr);
                m_currentProject.SetInitialCondition(newStepper);
            }
        }

        /// <summary>
        /// Deletes the "Stepper".
        /// </summary>
        /// <param name="stepper">The "Stepper"</param>
        public void DeleteStepperID(EcellObject stepper)
        {
            DeleteStepperID(stepper, true);
        }

        /// <summary>
        /// Deletes the "Stepper".
        /// </summary>
        /// <param name="stepper">The "Stepper"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        public void DeleteStepperID(EcellObject stepper, bool isRecorded)
        {
            int point = -1;
            List<EcellObject> storedStepperList
                = m_currentProject.StepperDic[stepper.ModelID];
            if (storedStepperList.Count <= 1)
            {
                throw new EcellException(MessageResources.ErrDelStep);
            }
            if (m_currentProject.IsUsedStepper(stepper.Key))
            {
                throw new EcellException(string.Format(MessageResources.ErrStepperStillInUse, stepper.Key));
            }


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
                Trace.WriteLine(string.Format(MessageResources.InfoDel,
                    new object[] { stepper.Type, stepper.Key }));
            }
        }

        /// <summary>
        /// Returns the list of the "Stepper" with the parameter ID.
        /// </summary>
        /// <param name="modelID"> model ID</param>
        /// <returns>The list of the "Stepper"</returns>
        public List<EcellObject> GetStepper(string modelID)
        {
            List<EcellObject> returnedStepper = new List<EcellObject>();
            Debug.Assert(!string.IsNullOrEmpty(modelID));

            List<EcellObject> tempList = m_currentProject.StepperDic[modelID];
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
        /// Save the stepping model.
        /// </summary>
        private void SaveSteppingModel()
        {
            string tmpDir = Util.GetTmpDir();
            string prevPath = null;
            for (int i = 10; i >= 1; i--)
            {
                string path = tmpDir + "\\stepping" + i + ".tmp";
                if (prevPath != null)
                {
                    if (File.Exists(path))
                    {
                        File.Move(path, prevPath);
                        if (m_saveTimeDic.ContainsKey(i))
                            m_saveTimeDic[i + 1] = m_saveTimeDic[i];
                    }
                }
                if (File.Exists(path))
                    File.Delete(path);
                prevPath = path;
            }
            string savePath = tmpDir + "\\stepping1.tmp";
            m_saveTimeDic[1] = GetCurrentSimulationTime();
            SaveSteppingModelInfo(savePath);
            if (SteppingModelEvent != null)
                SteppingModelEvent(this, new SteppingModelEventArgs(m_saveTimeDic[1]));
        }

        /// <summary>
        /// Load the stepping model.
        /// </summary>
        /// <param name="id">the ID of stepping model.</param>
        public void LoadSteppingModel(int id)
        {
            string tmpDir = Util.GetTmpDir();
            string path = tmpDir + "\\stepping" + id + ".tmp";

            LoadSteppingModelInfo(path);
            if (ApplySteppingModelEvent != null)
                ApplySteppingModelEvent(this, new SteppingModelEventArgs(m_saveTimeDic[id]));
        }

        /// <summary>
        /// Clear the stepping model.
        /// </summary>
        private void ClearSteppingModel()
        {
            if (!m_isSaveStep)
                return;
            string tmpDir = Util.GetTmpDir();

            string[] files = Directory.GetFiles(tmpDir, "stepping*.tmp");
            for (int i = 0; i < files.Length; i++)
            {
                if (File.Exists(files[i]))
                    File.Delete(files[i]);
            }
            m_saveTimeDic.Clear();
            if (SteppingModelEvent != null)
                SteppingModelEvent(this, new SteppingModelEventArgs(0.0));
        }

        /// <summary>
        /// Start simulation.
        /// </summary>
        /// <param name="time">the simulation time.</param>
        public void StartSimulation(double time)
        {
            m_steppingData = null;
            //if (m_isTimeStepping && m_remainTime > 0.0)
            //{
            //    StartStepSimulation(m_remainTime, false);
            //    return;
            //}
            //else if (m_isStepStepping && m_remainStep > 0)
            //{
            //    StartStepSimulation(m_remainStep, false);
            //    return;
            //}

            try
            {
                string msg;
                if (m_currentProject.SimulationStatus != SimulationStatus.Suspended)
                {
                    this.Initialize(true);
                    msg = MessageResources.SimulationStarted;
                }
                else
                {
                    msg = MessageResources.SimulationRestarted;
                }
                m_currentProject.SimulationStatus = SimulationStatus.Run;
                m_env.Console.WriteLine(MessageResources.InfoStartSim);
                m_env.Console.Flush();
                m_env.LogManager.Append(new ApplicationLogEntry(
                        MessageType.Information,
                        msg,
                        this));

                while (m_currentProject != null && m_currentProject.SimulationStatus == SimulationStatus.Run)
                {
                    m_currentProject.Simulator.Step(m_defaultStepCount);
                    Application.DoEvents();
                    double currentTime = m_currentProject.Simulator.GetCurrentTime();
                    this.m_env.PluginManager.AdvancedTime(currentTime);
                    if (m_waitTime > 0 && m_currentProject.SimulationStatus == SimulationStatus.Run)
                    {
                        for (int i = 0; i < m_waitTime; i++)
                        {
                            Thread.Sleep(1000);
                            Application.DoEvents();
                        }
                    }
                }
                if (m_isSaveStep)
                {
                    SaveSteppingModel();
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
        /// <param name="time">step time</param>
        /// <param name="isDirect">the flag whether this function is called.</param>
        public void StartStepSimulation(double time, bool isDirect)
        {
            m_steppingData = null;
            try
            {
                if (m_currentProject.SimulationStatus != SimulationStatus.Suspended)
                {
                    this.Initialize(true);
                }
                double cTime = m_currentProject.Simulator.GetCurrentTime();
                double stoppedTime;
//                if (!(m_isTimeStepping && m_remainTime > 0.0) || isDirect)
//                {
                    m_remainTime = time;
//                }
                stoppedTime = cTime + m_remainTime;
                m_remainStep = 0;

                m_currentProject.SimulationStatus = SimulationStatus.Run;
                m_env.Console.WriteLine(string.Format(MessageResources.InfoStartStepSim, "t:" + time.ToString()));
                m_env.Console.Flush();
                while (m_currentProject.SimulationStatus == SimulationStatus.Run)
                {
                    lock (m_currentProject.Simulator)
                    {
                        if (m_remainTime < m_defaultTime)
                        {
                            m_currentProject.Simulator.Run(m_remainTime);
                            m_remainTime = 0.0;
                        }
                        else
                        {
                            m_currentProject.Simulator.Run(m_defaultTime);
                            m_remainTime = m_remainTime - m_defaultTime;
                        }
                    }
                    Application.DoEvents();
                    double currentTime = m_currentProject.Simulator.GetCurrentTime();
                    this.m_env.PluginManager.AdvancedTime(currentTime);
                    if (m_remainTime == 0.0)
                    {
                        m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                        break;
                    }
                }
                if (m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    double aTime = m_currentProject.Simulator.GetCurrentTime();
                    m_remainTime = stoppedTime - aTime;
                }
                if (m_isSaveStep)
                {
                    SaveSteppingModel();
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
        /// <param name="step">step count</param>
        /// <param name="isDirect">the flag whether this function is called.</param>
        public void StartStepSimulation(int step, bool isDirect)
        {
            m_steppingData = null;
            try
            {
                if (m_currentProject.SimulationStatus != SimulationStatus.Suspended)
                {
                    this.Initialize(true);
                }
                double cTime = m_currentProject.Simulator.GetCurrentTime();
                double stoppedTime;
//                if (!(m_isStepStepping && m_remainStep > 0) || isDirect)
//                {
                    m_remainStep = step;
//                }
                stoppedTime = cTime + m_remainTime;
                m_remainTime = 0.0;

                m_currentProject.SimulationStatus = SimulationStatus.Run;
                m_env.Console.WriteLine(string.Format(MessageResources.InfoStartStepSim, "step:" + step.ToString()));
                m_env.Console.Flush();
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
                        m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                        break;
                    }
                }
                if (m_isSaveStep)
                {
                    SaveSteppingModel();
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
                    string.Format(MessageResources.InfoSuspend, m_currentProject.Simulator.GetCurrentTime()),
                    this));
                m_env.Console.WriteLine(string.Format(MessageResources.InfoSuspend, "t:" + m_currentProject.Simulator.GetCurrentTime()));
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
                    string.Format(MessageResources.InfoResetSim, m_currentProject.Simulator.GetCurrentTime()),
                    this));
                m_env.Console.WriteLine(string.Format(MessageResources.InfoResetSim, "t:"+m_currentProject.Simulator.GetCurrentTime()));
                m_env.Console.Flush();

                m_remainTime = 0.0;
                m_remainStep = 0;
                m_steppingData = null;
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
        /// Get log data from the current simulator.
        /// </summary>
        /// <param name="startTime">The start time of log file.</param>
        /// <param name="endTime">The end time of log file.</param>
        /// <param name="interval">The interval of log file.</param>
        /// <param name="fullID">The FullID fo log data.</param>
        /// <returns>The list of log data.</returns>
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
                
//                IList<string> loggerList = m_currentProject.Simulator.GetLoggerList();
                List<string> loggerList = GetLoggerList();
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
        /// Get the uniqueue log data from the current simulator.
        /// </summary>
        /// <param name="startTime">The start time of log file.</param>
        /// <param name="endTime">The endt time of log file.</param>
        /// <param name="interval">The interval of log file.</param>
        /// <param name="fullID">The FullID of log data.</param>
        /// <returns>The list of log data.</returns>
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

            List<string> loggerList = m_env.LoggerManager.GetLoggerList();
            return loggerList;

            //try
            //{
            //    IList<string> polymorphList = m_currentProject.Simulator.GetLoggerList();
            //    foreach (string polymorph in polymorphList)
            //    {
            //        loggerList.Add(polymorph);
            //    }
            //    return loggerList;
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //    return null;
            //}
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
                throw new EcellException(string.Format(MessageResources.ErrSavePrj,
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
                if (savedType != null && (savedType.Equals("csv") || savedType.Equals("CSV")))
                    saveFileType = SaveType.CSV;

                // Initializes.
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
        /// <param name="fileName">the simulation result file name.</param>
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
            Dictionary<string, List<EcellObject>> stepperList = m_currentProject.StepperDic;
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
                //foreach (EcellObject stepper in stepperList[modelID])
                //{
                //    if (m_currentProject.IsUsedStepper(stepper.Key))
                //    {
                //        newStepperList.Add(stepper);
                //    }
                //}
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
            m_currentProject.LogableEntityPathDic.Clear();
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
                    EcellValue storedValue = null;
                    if (fullPN.StartsWith(Constants.xpathStepper))
                    {
                        string name;
                        string type;
                        string propName;
                        Util.ParseFullPN(fullPN, out type, out name, out propName);
                        storedValue = new EcellValue(simulator.GetStepperProperty(name, propName));
                    }
                    else
                    {
                        storedValue = new EcellValue(simulator.GetEntityProperty(fullPN));
                    }
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
                    if (fullPN.StartsWith(Constants.xpathStepper))
                    {
                        string name;
                        string type;
                        string propName;
                        Util.ParseFullPN(fullPN, out type, out name, out propName);
                        simulator.SetStepperProperty(name, propName, newValue);
                    }
                    else
                    {
                        simulator.SetEntityProperty(fullPN, newValue);
                    }
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
            ClearSteppingModel();
        }

        /// <summary>
        /// Loads the "Stepper" 2 the "EcellCoreLib".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="stepperList">The list of the "Stepper"</param>
        /// <param name="setStepperDic">The list of stepper.</param>
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
        /// <param name="setPropertyDic">The dictionary of property.</param>
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
                if (m_currentProject.InitialCondition.ContainsKey(parameterID))
                {
                    throw new EcellException(
                        string.Format(MessageResources.ErrExistObj,
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
//                m_currentProject.StepperDic = newStepperListSets;
                m_currentProject.InitialCondition[parameterID] = newInitialCondSets;

                // Notify that a new parameter set is created.
                m_env.PluginManager.ParameterAdd(m_currentProject.Info.Name, parameterID);

                m_env.Console.WriteLine(string.Format(MessageResources.InfoCreSim, parameterID));
                m_env.Console.Flush();
                Trace.WriteLine(string.Format(MessageResources.InfoCreSim,
                    new object[] { parameterID }));
                //if (isRecorded)
                //    m_env.ActionManager.AddAction(new NewSimParamAction(parameterID, isAnchor));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                string message = string.Format(MessageResources.ErrCreSimParam,
                    new object[] { parameterID });
                throw new EcellException(message, ex);
            }
        }

        /// <summary>
        /// Loads the simulation parameter.
        /// </summary>
        /// <param name="fileName">The simulation parameter file name</param>
        /// <param name="project">The project</param>
        /// <returns></returns>
        private SimulationParameter LoadSimulationParameter(string fileName, Project project)
        {
            string message = null;
            SimulationParameter simParam = null;
            string projectID = project.Info.Name;
            try
            {
                message = "[" + fileName + "]";
                // Initializes
                Debug.Assert(!string.IsNullOrEmpty(fileName));
                // Parses the simulation parameter.
                WrappedSimulator simulator = project.CreateSimulatorInstance();
                simParam = SimulationParameterReader.Parse(fileName, simulator);
                //20090623
                //simulator.Dispose();
            }
            catch (Exception ex)
            {
                throw new EcellException(string.Format(MessageResources.ErrLoadSimParam,
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
                m_currentProject.InitialCondition.Keys == null)
                return new List<string>();

            return new List<string>(m_currentProject.InitialCondition.Keys);
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
                    try
                    {
                        ConfirmReset("change simulation parameter set", Constants.xpathSimulation);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    string modelID = m_currentProject.Model.ModelID;
                    if (parameterID.Equals(Constants.defaultSimParam))
                    {
                        foreach (string entityPath in m_currentProject.InitialCondition[oldParameterID][modelID].Keys)
                        {
                            string type;
                            string key;
                            string propName;
                            Util.ParseFullPN(entityPath, out type, out key, out propName);
                            EcellObject obj = m_currentProject.GetEcellObject(modelID, type, key, true);
                            if (obj == null) continue;
                            m_env.PluginManager.DataChanged(modelID, key, type, obj);
                            m_currentProject.Info.SimulationParam = parameterID;
                        }
                    }
                    else
                    {
                        m_currentProject.Info.SimulationParam = parameterID;
                        foreach (string entityPath in m_currentProject.InitialCondition[parameterID][modelID].Keys)
                        {
                            string type;
                            string key;
                            string propName;
                            Util.ParseFullPN(entityPath, out type, out key, out propName);
                            EcellObject obj = GetEcellObject(modelID, key, type);
                            if (obj == null) continue;
                            EcellData d = obj.GetEcellData(propName);
                            d.Value = new EcellValue(m_currentProject.InitialCondition[parameterID][modelID][entityPath]);
                            m_env.PluginManager.DataChanged(modelID, key, type, obj);
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
                m_currentProject.LoggerPolicyDic[simParamID] = simParam.LoggerPolicy;
                m_currentProject.InitialCondition[simParamID] = simParam.InitialConditions;
                m_env.Console.WriteLine(string.Format(MessageResources.InfoLoadSim, simParamID));
                m_env.Console.Flush();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Deletes the parameter.
        /// </summary>
        /// <param name="parameterID">the deleted simulation parameter ID.</param>
        public void DeleteSimulationParameter(string parameterID)
        {
            DeleteSimulationParameter(parameterID, true, true);
        }

        /// <summary>
        /// Deletes the parameter.
        /// </summary>
        /// <param name="parameterID">the deleted simulation parameter ID.</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DeleteSimulationParameter(string parameterID, bool isRecorded, bool isAnchor)
        {
            string message = null;


            try
            {
                if (m_currentProject.InitialCondition.Keys.Count <= 1)
                {
                    throw new EcellException(string.Format(MessageResources.ErrDelParam));
                }
                if (parameterID.Equals(Constants.defaultSimParam))
                {
                    throw new EcellException(string.Format(MessageResources.ErrDelParam));
                }

                if (m_currentProject.SimulationStatus == SimulationStatus.Run ||
                    m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    if (parameterID.Equals(m_currentProject.Info.SimulationParam))
                    {
                        if (Util.ShowYesNoDialog(
                            string.Format(MessageResources.InfoDeleteSim,
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
                //m_currentProject.StepperDic.Remove(parameterID);

                m_currentProject.LoggerPolicyDic.Remove(parameterID);
                m_currentProject.InitialCondition.Remove(parameterID);
                Trace.WriteLine(m_currentProject.Info.SimulationParam + ":" + parameterID);
                if (m_currentProject.Info.SimulationParam == parameterID)
                {
                    foreach (string key in m_currentProject.InitialCondition.Keys)
                    {
                        m_currentProject.Info.SimulationParam = key;
                        m_env.PluginManager.ParameterSet(m_currentProject.Info.Name, key);
                        break;
                    }
                }
                m_env.PluginManager.ParameterDelete(m_currentProject.Info.Name, parameterID);
                m_env.Console.WriteLine(string.Format(MessageResources.InfoRemoveSim, parameterID));
                m_env.Console.Flush();
                MessageDeleteEntity("Simulation Parameter", message);
                m_deleteParameterList.Add(parameterID);

                //if (isRecorded)
                //    m_env.ActionManager.AddAction(new DeleteSimParamAction(parameterID, isAnchor));
            }
            catch (Exception ex)
            {
                throw new EcellException(string.Format(MessageResources.ErrDelete,
                    new object[] { parameterID }), ex);
            }
        }

        /// <summary>
        /// Copy SimulationParameter.
        /// </summary>
        /// <param name="newParameterID">the dst simulation parameter ID.</param>
        /// <param name="srcParameterID">the src simulation parameter ID.</param>
        public void CopySimulationParameter(string newParameterID, string srcParameterID)
        {
            try
            {
                string message = null;

                message = "[" + newParameterID + "]";
                //
                // 4 Stepper
                //
                if (m_currentProject.InitialCondition.ContainsKey(newParameterID))
                {
                    throw new EcellException(
                        string.Format(MessageResources.ErrExistObj,
                        new object[] { newParameterID }));
                }

                m_currentProject.LoggerPolicyDic[newParameterID] =
                    new LoggerPolicy(m_currentProject.LoggerPolicyDic[srcParameterID]);


                Dictionary<string, Dictionary<string, double>> newInitialCondSets = new Dictionary<string, Dictionary<string, double>>();
                foreach (string name in m_currentProject.InitialCondition[srcParameterID].Keys)
                {
                    Dictionary<string, double> tmpDic = new Dictionary<string, double>();
                    foreach (string path in m_currentProject.InitialCondition[srcParameterID][name].Keys)
                    {
                        tmpDic.Add(path, m_currentProject.InitialCondition[srcParameterID][name][path]);
                    }
                    newInitialCondSets.Add(name, tmpDic);
                }

                m_currentProject.InitialCondition[newParameterID] = newInitialCondSets;

                m_env.PluginManager.ParameterAdd(m_currentProject.Info.Name, newParameterID);
                m_env.Console.WriteLine(string.Format(MessageResources.InfoCreSim, newParameterID));
                m_env.Console.Flush();

                Trace.WriteLine(string.Format(MessageResources.InfoCreSim,
                    new object[] { newParameterID }));
                //まだcopyがない
                //m_env.ActionManager.AddAction(new NewSimParamAction(parameterID, isAnchor));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                string message = string.Format(MessageResources.ErrCreSimParam,
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
                Debug.Assert(!string.IsNullOrEmpty(paramID));
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
                foreach (string modelID in m_currentProject.StepperDic.Keys)
                {
                    stepperList.AddRange(m_currentProject.StepperDic[modelID]);
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
                message = string.Format(MessageResources.ErrSavePrj,
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
        /// Update the initial conditions.
        /// </summary>
        /// <param name="parameterID">the parameter ID.</param>
        /// <param name="modelID">the model ID.</param>
        /// <param name="initialList">the list of initial data.</param>
        public void UpdateInitialCondition(
                string parameterID, string modelID, Dictionary<string, double> initialList)
        {
            if (string.IsNullOrEmpty(parameterID))
                parameterID = m_currentProject.Info.SimulationParam;

            Dictionary<string, double> parameters = this.m_currentProject.InitialCondition[parameterID][modelID];
            foreach (string key in initialList.Keys)
            {
                if (parameters.ContainsKey(key) && parameters[key] == initialList[key])
                    continue;
                parameters[key] = initialList[key];
                string type;
                string id;
                string propName;
                Util.ParseFullPN(key, out type, out id, out propName);
                EcellObject obj = m_currentProject.GetEcellObject(modelID, type, id, parameterID);
                m_env.PluginManager.DataChanged(modelID, id, type, obj);
            }
            List<string> delList = new List<string>();
            foreach (string key in parameters.Keys)
            {
                if (initialList.ContainsKey(key))
                    continue;
                delList.Add(key);
            }
            foreach (string key in delList)
            {
                parameters.Remove(key);
                string type;
                string id;
                string propName;
                Util.ParseFullPN(key, out type, out id, out propName);
                EcellObject obj = m_currentProject.GetEcellObject(modelID, type, id, parameterID);
                m_env.PluginManager.DataChanged(modelID, id, type, obj);
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
                DMDescriptor desc = m_env.DMDescriptorKeeper.GetDMDescriptor(EcellObject.PROCESS, dmName);
                isEnable = desc.CanHaveDynamicProperties;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                isEnable = false;
            }
            return isEnable;
        }

        /// <summary>
        /// Get the EcellValue from fullPath.
        /// </summary>
        /// <param name="fullPN">the FullID to get the property.</param>
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
                throw new EcellException(string.Format(MessageResources.ErrPropData, fullPN), ex);
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
        /// Set the stepper property to the current simulator.
        /// </summary>
        /// <param name="key">the key of property.</param>
        /// <param name="name">the name of property.</param>
        /// <param name="value">the set data.</param>
        public void SetStepperProperty(string key, string name, string value)
        {
            if (m_currentProject.Simulator == null
                || this.GetCurrentSimulationTime() <= 0.0)
            {
                return;
            }
            EcellValue storedValue
                = new EcellValue(m_currentProject.Simulator.GetStepperProperty(key, name));
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
            m_currentProject.Simulator.LoadStepperProperty(key, name, newValue.Value);
        }

        /// <summary>
        /// Get the current value with fullPath.
        /// This method is used to get numerical value of parameter while simulating.
        /// </summary>
        /// <param name="fullPN">the FullPN to get proerty.</param>
        /// <returns></returns>
        public double GetPropertyValue(string fullPN)
        {
            try
            {
                if (m_steppingData != null && m_steppingData.ContainsKey(fullPN))
                {
                    return (double)m_steppingData[fullPN];
                }                
                return (double)m_currentProject.Simulator.GetEntityProperty(fullPN);
            }
            catch (Exception ex)
            {
                throw new EcellException(string.Format(MessageResources.ErrPropData, new object[] { fullPN }), ex);
            }
        }

        /// <summary>
        /// Get the current value with name of stepper.
        /// </summary>
        /// <param name="stepperID">StepperID.</param>
        /// <param name="name">parameter name.</param>
        /// <returns></returns>
        public double GetPropertyValue4Stepper(string stepperID, string name)
        {
            try
            {
                if (m_steppingData != null)
                {
                    string fullPN = Constants.xpathStepper + "::" + stepperID + ":" + name;
                    return (double)m_steppingData[fullPN];
                }
                return (double)m_currentProject.Simulator.GetStepperProperty(stepperID, name);
            }
            catch (Exception ex)
            {
                throw new EcellException(string.Format(MessageResources.ErrPropData, name), ex);
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
        /// <param name="fullPathID">The FullPathID to set the logger.</param>
        /// <param name="isInitalize">The flag whether this logger is initialized.</param>
        /// <param name="sim">The current simulator.</param>
        /// <param name="loggerPolicy">The current logger policy.</param>
        private void CreateLogger(string fullPathID, bool isInitalize, WrappedSimulator sim, LoggerPolicy loggerPolicy)
        {
            if (m_loggerEntry.Contains(fullPathID))
                return;

            if (m_currentProject.SimulationStatus == SimulationStatus.Run ||
                m_currentProject.SimulationStatus == SimulationStatus.Suspended ||
                isInitalize)
            {
                sim.CreateLogger(fullPathID,
                    loggerPolicy.ReloadStepCount,
                    loggerPolicy.ReloadInterval,
                    Convert.ToBoolean((int)loggerPolicy.DiskFullAction),
                    loggerPolicy.MaxDiskSpace);
                m_currentProject.LogableEntityPathDic[fullPathID] = m_currentProject.Model.FullID;
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
            if (!Directory.Exists(topDir))
                return result;

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

        /// <summary>
        /// Unload the current simulator.
        /// </summary>
        public void UnloadSimulator()
        {
            if (m_currentProject != null)
                m_currentProject.UnloadSimulator();
        }

        /// <summary>
        /// Reload the current simulator.
        /// </summary>
        public void ReloadSimulator()
        {
            m_currentProject.ReloadSimulator();
            if (ReloadSimulatorEvent != null)
                ReloadSimulatorEvent(this, new EventArgs());
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

            // Get project DMs
            if (Directory.Exists(path))
            {
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
            }

            // Get Extra DMs
            foreach (string dmdir in m_currentProject.Info.DMDirList)
            {
                string[] extraDMs = Directory.GetFiles(dmdir, "*" + Constants.FileExtDM);
                for (int i = 0; i < extraDMs.Length; i++)
                {
                    string name = Path.GetFileNameWithoutExtension(extraDMs[i]);
                    if (!resultList.Contains(name))
                        resultList.Add(name);
                }
            }
            return resultList;
        }

        /// <summary>
        /// Get the DM directory of current project.
        /// </summary>
        /// <returns></returns>
        public string GetDMDir()
        {
            string prjPath = m_currentProject.Info.ProjectPath;
            // Return null when this project is not saved.
            if (string.IsNullOrEmpty(prjPath))
                return null;
            // Return project path.
            return Path.Combine(m_currentProject.Info.ProjectPath, Constants.DMDirName);
        }

        /// <summary>
        /// Get the source file path of DM.
        /// </summary>
        /// <param name="indexName">Index name of DM.</param>
        /// <returns>the source file path.</returns>
        public string GetDMSourceFileName(string indexName)
        {
            if (m_currentProject.Info.ProjectPath == null)
                return null;
            string path = Path.Combine(m_currentProject.Info.ProjectPath, Constants.DMDirName);
            path = Path.Combine(path, indexName + Constants.FileExtSource);
            if (!File.Exists(path))
                path = null;
            return path;
        }

        /// <summary>
        /// Get the DLL file path of DM.
        /// </summary>
        /// <param name="indexName">Index name of DM.</param>
        /// <returns>the DLL file path.</returns>
        public string GetDMDLLFileName(string indexName)
        {
            if (m_currentProject.Info.ProjectPath == null)
                return null;
            string path = Path.Combine(m_currentProject.Info.ProjectPath, Constants.DMDirName);
            path = Path.Combine(path, indexName + Constants.FileExtDM);
            if (!File.Exists(path))
                path = null;
            return path;
        }

        #endregion

        #region Send Message
        /// <summary>
        /// Message on CreateEntity
        /// </summary>
        /// <param name="type">the object type.</param>
        /// <param name="message">the create message.</param>
        public void MessageCreateEntity(string type, string message)
        {
            Trace.WriteLine("Create " + type + ": " + message);
        }
        /// <summary>
        /// Message on DeleteEntity
        /// </summary>
        /// <param name="type">the object type.</param>
        /// <param name="message">the delete message.</param>
        public void MessageDeleteEntity(string type, string message)
        {
            Trace.WriteLine("Delete " + type + ": " + message);
        }
        /// <summary>
        /// Message on UpdateData
        /// </summary>
        /// <param name="type">the object type.</param>
        /// <param name="message">the message string.</param>
        /// <param name="src">the src object.</param>
        /// <param name="dest">the dst object.</param>
        public void MessageUpdateData(string type, string message, string src, string dest)
        {
            Trace.WriteLine(
                "Update Data: " + message + "[" + type + "]" + System.Environment.NewLine
                    + "\t[" + src + "]->[" + dest + "]");
        }
        #endregion
    }
}
