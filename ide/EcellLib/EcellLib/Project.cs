﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using EcellCoreLib;
using Ecell.Objects;
using Ecell.Exceptions;
using Ecell.Reporting;
using System.Text.RegularExpressions;

namespace Ecell
{
    /// <summary>
    /// Stores the project information.
    /// </summary>
    public class Project
    {
        #region Field
        /// <summary>
        /// Project Path.
        /// </summary>
        private ProjectInfo m_info;
        /// <summary>
        /// ApplicationEnvironment
        /// </summary>
        private ApplicationEnvironment m_env;
        /// <summary>
        /// The list of the DM with Object type (stepper, system, process, variable).
        /// </summary>
        private Dictionary<string, List<string>> m_dmDic = null;
        /// <summary>
        /// The ModelList of this project
        /// </summary>
        private List<EcellModel> m_modelList = null;
        /// <summary>
        /// The Simulator of this project.
        /// </summary>
        private WrappedSimulator m_simulator = null;
        /// <summary>
        /// The dictionary of the "LoggerPolicy" with the parameter ID
        /// </summary>
        private Dictionary<string, LoggerPolicy> m_loggerPolicyDic = null;
        /// <summary>
        /// The dictionary of the logable entity path
        /// </summary>
        private Dictionary<string, string> m_logableEntityPathDic = null;
        /// <summary>
        /// The dictionary of the "System" with the model ID 
        /// </summary>
        private Dictionary<string, List<EcellObject>> m_systemDic = null;
        /// <summary>
        /// The dictionary of the "InitialCondition" with
        ///     the parameter ID, the model ID, the data type and the full ID
        /// </summary>
        private Dictionary<string, Dictionary<string, Dictionary<string, double>>> m_initialCondition = null;
        /// <summary>
        /// The dictionary of the "Stepper" with the parameter ID and the model ID
        /// </summary>
        private Dictionary<string, List<EcellObject>> m_stepperDic = null;

        /// <summary>
        /// The executed flag of Simulator.
        /// </summary>
        private SimulationStatus m_simulationStatus = 0;

        #endregion

        #region Accessor
        /// <summary>
        /// The ProjectInfo
        /// </summary>
        public ProjectInfo Info
        {
            get { return m_info; }
        }
        /// <summary>
        /// The list of the DM filepath.
        /// </summary>
        public Dictionary<string, List<string>> DmDic
        {
            get { return m_dmDic; }
        }

        /// <summary>
        /// Returns the list of the "Stepper" DM.
        /// </summary>
        public List<string> StepperDmList
        {
            get
            {
                SetDMList();
                List<string> stepperList = new List<string>();
                WrappedSimulator sim = CreateSimulatorInstance();
                foreach (DMInfo dmInfo in sim.GetDMInfo())
                {
                    if (dmInfo.TypeName == Constants.xpathStepper)
                    {
                        stepperList.Add(dmInfo.ModuleName);
                    }
                }
                if (m_dmDic != null)
                {
                    foreach (string name in m_dmDic[Constants.xpathStepper])
                    {
                        if (!stepperList.Contains(name))
                            stepperList.Add(name);
                    }
                }
                stepperList.Sort();
                // 20090727
                sim.Dispose();
                return stepperList;
            }
        }

        /// <summary>
        /// Returns the list of the "System" DM.
        /// </summary>
        public List<string> SystemDmList
        {
            get
            {
                SetDMList();
                return m_dmDic[Constants.xpathSystem];
            }
        }

        /// <summary>
        /// Returns the list of the "Variable" DM.
        /// </summary>
        public List<string> VariableDmList
        {
            get
            {
                SetDMList();
                return m_dmDic[Constants.xpathVariable];
            }
        }

        /// <summary>
        /// Returns the list of the "Process" DM.
        /// </summary>
        public List<string> ProcessDmList
        {
            get
            {
                SetDMList();
                return m_dmDic[Constants.xpathProcess];
            }
        }

        /// <summary>
        /// The List of the Model
        /// </summary>
        public List<EcellModel> ModelList
        {
            get { return m_modelList; }
        }

        /// <summary>
        /// Current Model
        /// </summary>
        public EcellModel Model
        {
            get { return m_modelList[0]; }
        }

        /// <summary>
        /// The dictionary of the "Systems" with the model ID 
        /// </summary>
        public Dictionary<string, List<EcellObject>> SystemDic
        {
            get { return m_systemDic; }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<EcellObject> SystemList
        {
            get 
            {
                return m_systemDic[m_modelList[0].ModelID]; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<EcellObject> ProcessList
        {
            get
            {
                List<EcellObject> list = new List<EcellObject>();
                foreach (EcellObject system in this.SystemList)
                {
                    foreach (EcellObject obj in system.Children)
                    {
                        if (!(obj is EcellProcess))
                            continue;
                        list.Add(obj);
                    }
                }
                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<EcellObject> VariableList
        {
            get
            {
                List<EcellObject> list = new List<EcellObject>();
                foreach (EcellObject system in this.SystemList)
                {
                    foreach (EcellObject obj in system.Children)
                    {
                        if (!(obj is EcellVariable))
                            continue;
                        else if (obj.LocalID.Equals(EcellSystem.SIZE))
                            continue;
                        list.Add(obj);
                    }
                }
                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<EcellObject> TextList
        {
            get
            {
                List<EcellObject> list = new List<EcellObject>();
                foreach (EcellObject system in this.SystemList)
                {
                    foreach (EcellObject obj in system.Children)
                    {
                        if (!(obj is EcellText))
                            continue;
                        else if (obj.LocalID.Equals(EcellSystem.SIZE))
                            continue;
                        list.Add(obj);
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// The Simulator of this project.
        /// </summary>
        public WrappedSimulator Simulator
        {
            get { return m_simulator; }
            set {
                // 20090623
                //if (m_simulator != null)
                //    m_simulator.Dispose();
                m_simulator = value; 
            }
        }

        /// <summary>
        /// The executed flag of Simulator.
        /// </summary>
        public SimulationStatus SimulationStatus
        {
            get { return m_simulationStatus; }
            set { m_simulationStatus = value; }
        }

        /// <summary>
        /// The dictionary of the logable entity path
        /// </summary>
        public Dictionary<string, string> LogableEntityPathDic
        {
            get { return m_logableEntityPathDic; }
            set { m_logableEntityPathDic = value; }
        }

        /// <summary>
        /// The dictionary of the "InitialCondition" with
        ///     the parameter ID, the model ID, the data type and the full ID
        /// </summary>
        public Dictionary<string, Dictionary<string, Dictionary<string, double>>> InitialCondition
        {
            get { return m_initialCondition; }
        }

        /// <summary>
        /// The dictionary of the "LoggerPolicy" with the parameter ID
        /// </summary>
        public Dictionary<string, LoggerPolicy> LoggerPolicyDic
        {
            get { return m_loggerPolicyDic; }
        }

        /// <summary>
        /// Current LoggerPolicy.
        /// </summary>
        public LoggerPolicy LoggerPolicy
        {
            get { return m_loggerPolicyDic[m_info.SimulationParam];}
        }

        /// <summary>
        /// The dictionary of the "Stepper" with the parameter ID and the model ID
        /// </summary>
        public Dictionary<string, List<EcellObject>> StepperDic
        {
            get { return m_stepperDic; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Creates the new "Project" instance with ProjectInfo.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="env"></param>
        public Project(ProjectInfo info, ApplicationEnvironment env)
        {
            if (info == null)
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "ProjectInfo"));
            if (env == null)
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "ApplicationEnvironment"));

            m_info = info;
            m_env = env;
            m_loggerPolicyDic = new Dictionary<string, LoggerPolicy>();
            m_stepperDic = new Dictionary<string, List<EcellObject>>();
            m_modelList = new List<EcellModel>();
            m_systemDic = new Dictionary<string, List<EcellObject>>();
            m_logableEntityPathDic = new Dictionary<string, string>();

            // Loads the model.
            if (info.ProjectType != ProjectType.Model)
                info.FindModels();

            SetDMList();
            m_simulator = CreateSimulatorInstance();
        }

        public void UnloadSimulator()
        {
            if (m_simulator != null)
            {
                m_simulator.Dispose();
                m_simulator = null;
            }
        }

        public void ReloadSimulator()
        {
            m_simulator = CreateSimulatorInstance();
        }

        /// <summary>
        /// Loads the eml formatted file and returns the model ID.
        /// </summary>
        public void LoadModel()
        {
            try
            {
                foreach (string filename in m_info.Models)
                {
                    // Load model
                    string modelID = null;
                    EcellModel modelObj = null;
                    try
                    {
                        modelObj = EmlReader.Parse(filename, m_simulator);
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
                    if (!string.IsNullOrEmpty(modelObj.ErrMsg) && m_env.PluginManager.DockPanel != null)
                    {
                        Util.ShowWarningDialog(MessageResources.WarnLoadDM);
                        m_env.Console.Write(modelObj.ErrMsg);
                        m_env.Console.Flush();
                    }

                    // If this project is template.
                    if (m_info.ProjectType == ProjectType.Template)
                        modelObj.ModelID = m_info.Name;
                    modelID = modelObj.ModelID;

                    // Initialize
                    try
                    {
                        m_simulator.Initialize();
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
                        if (m_env.PluginManager.DockPanel != null)
                            Util.ShowWarningDialog(MessageResources.WarnInvalidData + "\n" + e.Message);
                        Trace.WriteLine(e.ToString());
                    }

                    // Sets initial conditions.
                    SetSimParams(modelID);
                    InitializeModel(modelObj);

                    try
                    {
                        string leml = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar +
                            Path.GetFileNameWithoutExtension(filename) + Constants.FileExtLEML;
                        Leml.LoadLEML(m_env, modelObj, leml);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.StackTrace);
                    }
                }

                // If this project has no model.
                if (m_modelList.Count <= 0)
                    throw new EcellException(string.Format(MessageResources.ErrNoSet, "Model"));

                // Stores the "LoggerPolicy"
                string simParam = m_info.SimulationParam;
                if (!m_loggerPolicyDic.ContainsKey(simParam))
                {
                    m_loggerPolicyDic[simParam] = new LoggerPolicy();
                }
            }
            catch (Exception ex)
            {
                throw new EcellException(string.Format(MessageResources.ErrLoadModel, ""), ex);
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

            string simParam = m_info.SimulationParam;
            if (ecellObject.Type.Equals(Constants.xpathModel))
            {
                m_modelList.Add((EcellModel)ecellObject);
                DataStorer.DataStored(
                    m_simulator,
                    m_env.DMDescriptorKeeper,
                    ecellObject,
                    m_initialCondition[simParam][modelID]);
            }
            else if (ecellObject.Type.Equals(Constants.xpathSystem))
            {
                if (!m_systemDic.ContainsKey(modelID))
                {
                    m_systemDic[modelID]
                            = new List<EcellObject>();
                }
                m_systemDic[modelID].Add(ecellObject);
            }
            else if (ecellObject.Type.Equals(Constants.xpathStepper))
            {
                if (!m_stepperDic.ContainsKey(modelID))
                {
                    m_stepperDic[modelID] = new List<EcellObject>();
                }
                m_stepperDic[modelID].Add(ecellObject);
            }
            foreach (EcellObject childEcellObject in ecellObject.Children)
            {
                InitializeModel(childEcellObject);
            }
        }

        #endregion

        #region Methods for DM
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetDMDirs()
        {
            List<string> list = new List<string>();
            foreach (string name in m_info.DMDirList)
            {
                if (list.Contains(name)) continue;
                list.Add(name);
            }
            foreach (string name in Util.GetDMDirs(m_info.ProjectPath))
            {
                if (list.Contains(name)) continue;
                list.Add(name);
            }

            return list.ToArray();
        }
        /// <summary>
        /// Initialize DM Dictionary.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<string>> GetDmDic()
        {
            // Initialize
            Dictionary<string, List<string>> dmDic = new Dictionary<string, List<string>>();
            // 4 Process
            dmDic.Add(Constants.xpathProcess, new List<string>());
            // 4 Stepper
            dmDic.Add(Constants.xpathStepper, new List<string>());
            // 4 System
            List<string> systemList = new List<string>();
            systemList.Add(Constants.xpathSystem);
            dmDic.Add(Constants.xpathSystem, systemList);
            // 4 Variable
            List<string> variableList = new List<string>();
            variableList.Add(Constants.xpathVariable);
            dmDic.Add(Constants.xpathVariable, variableList);

            // Searches the DM paths
            string[] dmPathArray = GetDMDirs();
            if (dmPathArray == null)
            {
                throw new EcellException("ErrFindDmDir");
            }

            // Set DM
            foreach (string dmPath in dmPathArray)
            {
                if (!Directory.Exists(dmPath))
                {
                    continue;
                }
                // 4 Process
                string[] processDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathProcess + Constants.FileExtDM
                    );
                foreach (string processDM in processDMArray)
                {
                    dmDic[Constants.xpathProcess].Add(Path.GetFileNameWithoutExtension(processDM));
                }
                // 4 Stepper
                string[] stepperDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathStepper + Constants.FileExtDM
                    );
                foreach (string stepperDM in stepperDMArray)
                {
                    dmDic[Constants.xpathStepper].Add(Path.GetFileNameWithoutExtension(stepperDM));
                }
                // 4 System
                string[] systemDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathSystem + Constants.FileExtDM
                    );
                foreach (string systemDM in systemDMArray)
                {
                    dmDic[Constants.xpathSystem].Add(Path.GetFileNameWithoutExtension(systemDM));
                }
                // 4 Variable
                string[] variableDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathVariable + Constants.FileExtDM
                    );
                foreach (string variableDM in variableDMArray)
                {
                    dmDic[Constants.xpathVariable].Add(Path.GetFileNameWithoutExtension(variableDM));
                }
            }

            return dmDic;
        }

        /// <summary>
        /// Sets the list of DMs.
        /// </summary>
        public void SetDMList()
        {
            // Initialize
            this.m_dmDic = GetDmDic();
        }

        /// <summary>
        /// Create a new WrappedSimulator instance.
        /// </summary>
        internal WrappedSimulator CreateSimulatorInstance()
        {
            string[] dmPaths = GetDMDirs();
            Trace.WriteLine("Creating simulator (dmpath=" + string.Join(";", dmPaths) + ")");
            return new WrappedSimulator(dmPaths);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get analysis path in project.
        /// </summary>
        /// <returns></returns>
        public string GetAnalysisDirectory()
        {
            string path = null;
            if (this.Info.ProjectPath != null)
                path = Path.Combine(this.Info.ProjectPath, Constants.AnalysisDirName);
            else
                return path;
            if (!Directory.Exists(path))
                return null;
            return path;
        }

        /// <summary>
        /// Initialize objects.
        /// </summary>
        public void SetSimParams(string modelID)
        {
            // Checks the modelID.
            if (string.IsNullOrEmpty(modelID))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "modelID"));

            m_initialCondition = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            m_initialCondition[m_info.SimulationParam] = new Dictionary<string, Dictionary<string, double>>();
            m_initialCondition[m_info.SimulationParam][modelID] = new Dictionary<string, double>();
        }

        /// <summary>
        /// SortSystems
        /// </summary>
        public void SortSystems()
        {
            SortedDictionary<string, EcellObject> tempDic = new SortedDictionary<string, EcellObject>();
            List<EcellObject> systemList = null;
            foreach (KeyValuePair<string, List<EcellObject>> systemDic in m_systemDic)
            {
                tempDic.Clear();
                systemList = systemDic.Value;
                foreach (EcellObject system in systemList)
                    tempDic.Add(system.Key, system);
                systemList.Clear();
                foreach (EcellObject system in tempDic.Values)
                    systemList.Add(system);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            // Dispose simulator.
            this.m_simulator.Dispose();
            this.m_simulator = null;
        }

        /// <summary>
        /// Check whether this stepper is used.
        /// </summary>
        /// <param name="stepperID">the checked stepper ID.</param>
        /// <returns>if stepper is used, return true.</returns>
        public bool IsUsedStepper(string stepperID)
        {
            foreach (EcellObject sysobj in m_systemDic[Model.ModelID])
            {
                EcellData d = sysobj.GetEcellData(Constants.xpathStepperID);
                if (d != null) 
                {
                    if (stepperID.Equals(d.Value.ToString()))
                        return true;
                }
                if (sysobj.Children == null)
                    continue;
                foreach (EcellObject child in sysobj.Children)
                {
                    if (!(child is EcellProcess))
                        continue;
                    EcellData d1 = child.GetEcellData(Constants.xpathStepperID);
                    if (d1 == null) continue;
                    if (stepperID.Equals(d1.Value.ToString()))
                        return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Check whether this stepper class is used.
        /// </summary>
        /// <param name="classname">the stepper class name.</param>
        /// <returns>if stepper is used, return truue.</returns>
        public bool IsUsedStepperClass(string classname)
        {
            foreach (EcellObject stepper in m_stepperDic[Model.ModelID])
            {
                if (stepper.Classname.Equals(classname))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether this process class is used.
        /// </summary>
        /// <param name="classname">the process class name.</param>
        /// <returns>if process is used, return truue.</returns>
        public bool IsUsedProcessClass(string classname)
        {
            foreach (EcellObject sysobj in m_systemDic[Model.ModelID])
            {
                if (sysobj.Children == null)
                    continue;
                foreach (EcellObject child in sysobj.Children)
                {
                    if (!(child is EcellProcess))
                        continue;
                    if (child.Classname.Equals(classname))
                        return true;
                }
            }
            return false;
        }

        #region Methods for Save
        /// <summary>
        /// 
        /// </summary>
        public void Save(ProjectType status)
        {
            // get old path
            string oldPath = m_info.ProjectPath;
            // set new path
            m_info.ProjectPath = Path.Combine(Util.GetBaseDir(), m_info.Name);

            // Delete old project before overwrite.
            if (m_info.ProjectType == ProjectType.Revision)
            {
                string projectFile = Path.Combine(m_info.ProjectPath, Constants.fileProjectXML);
                if (File.Exists(projectFile))
                {
                    ProjectInfo info = ProjectInfoLoader.Load(projectFile);
                    if (!info.Name.Equals(m_info.Name))
                        Directory.Delete(m_info.ProjectPath, true);
                }
            }
            else if (m_info.ProjectType != ProjectType.Project)
            {
                // new project.
                if (Util.IsExistProject(m_info.Name))
                {
                    Directory.Delete(m_info.ProjectPath, true);
                }
            }
            else
            {
                // renamed project.
                string oldDir = Path.GetDirectoryName(m_info.ProjectFile).Replace("\\", "/");
                string newDir = m_info.ProjectPath.Replace("\\", "/");
                if (Util.IsExistProject(m_info.Name) && !Path.Equals(oldDir, newDir))
                    Directory.Delete(m_info.ProjectPath, true);
            }

            // Save ProjectInfo
            m_info.Save(status);

            // Copy DMs.
            string dmDir = Path.Combine(m_info.ProjectPath, Constants.DMDirName);
            if (!Directory.Exists(dmDir))
                Directory.CreateDirectory(dmDir);
            foreach (string dir in m_info.DMDirList)
            {
                DirectoryInfo d1 = new DirectoryInfo(dir);
                DirectoryInfo d2 = new DirectoryInfo(dmDir);
                if (d1.FullName.Equals(d2.FullName))
                    continue;
                Util.CopyDirectory(dir, dmDir, true);
            }
            m_info.DMDirList.Clear();

            // Check oldpath.
            if (string.IsNullOrEmpty(oldPath))
                return;
            oldPath = oldPath.Replace("\\", "/");
            m_info.ProjectPath = m_info.ProjectPath.Replace("\\", "/");
            if (Path.Equals(m_info.ProjectPath, oldPath))
                return;

            // Copy Revisions.
            string[] revisions = Directory.GetDirectories(oldPath, "Revision*");
            foreach (string revision in revisions)
            {
                string newDir = Path.Combine(m_info.ProjectPath, Path.GetFileName(revision));
                Util.CopyDirectory(revision, newDir, true);
            }

            // Copy Logs.
            string paramDir = Path.Combine(oldPath, Constants.ParameterDirName);
            if(!(Directory.Exists(paramDir)))
                return;
            string[] logs = Directory.GetDirectories(paramDir, "*");
            paramDir = Path.Combine(m_info.ProjectPath, Constants.ParameterDirName);
            foreach (string log in logs)
            {
                string newDir = Path.Combine(paramDir, Path.GetFileName(log));
                Util.CopyDirectory(log, newDir, true);
            }
        }

        /// <summary>
        /// Returns the savable model ID.
        /// </summary>
        /// <returns>The savable model ID</returns>
        internal List<string> GetSavableModel()
        {
            List<string> modelIDList = new List<string>();
            foreach (EcellObject model in m_modelList)
            {
                modelIDList.Add(model.ModelID);
            }
            return modelIDList;
        }

        /// <summary>
        /// Returns the savable simulation parameter ID.
        /// </summary>
        /// <returns>The savable simulation parameter ID</returns>
        internal List<string> GetSavableSimulationParameter()
        {
            Debug.Assert(m_loggerPolicyDic != null);
            List<string> prmIDList = new List<string>();
            foreach (string prmID in m_loggerPolicyDic.Keys)
            {
                prmIDList.Add(prmID);
            }
            return prmIDList;
        }

        /// <summary>
        /// Returns the savable simulation result.
        /// </summary>
        /// <returns>The savable simulation result</returns>
        internal List<string> GetSavableSimulationResult()
        {
            List<string> list = new List<string>();
            list.Add(Constants.xpathParameters + Constants.xpathResult);
            return list;
        }
        #endregion

        #region Getter
        /// <summary>
        /// Get a temporary id in this project.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="type">object type.</param>
        /// <param name="systemID">ID of parent system.</param>
        /// <returns>the temporary id.</returns>
        public string GetTemporaryID(string modelID, string type, string systemID)
        {
            // Set Preface
            string localID;
            string key;
            if (type.Equals(EcellObject.PROCESS))
            {
                localID = GetTemporaryID(ProcessList, "P");
                key = systemID + ":" + localID;
            }
            else if (type.Equals(EcellObject.VARIABLE))
            {
                localID = GetTemporaryID(VariableList, "V");
                key = systemID + ":" + localID;
            }
            else if (type.Equals(EcellObject.TEXT))
            {
                localID = GetTemporaryID(TextList, "Text");
                key = systemID + ":" + localID;
            }
            else if (type.Equals(EcellObject.SYSTEM))
            {
                if (systemID == null || systemID == "/")
                    systemID = "";
                localID = GetTemporaryID(SystemList, "S");
                key = systemID + "/" + localID;
            }
            else if (type.Equals(EcellObject.STEPPER))
            {
                key = GetTemporaryID(StepperDic[modelID], "Stepper");
            }
            else
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "type"));
            }
            return key;
        }

        /// <summary>
        /// Get a copied key in this project.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="type">object type.</param>
        /// <param name="key">ID of parent system.</param>
        /// <returns>the copied id.</returns>
        public string GetCopiedID(string modelID, string type, string key)
        {
            // Get LocalID
            string oldLocalID;
            string systemID;
            if (type.Equals(EcellObject.STEPPER))
            {
                oldLocalID = key;
                systemID = "";
            }
            else
            {
                Util.ParseKey(key, out systemID, out oldLocalID);
            }

            if(oldLocalID.Contains("_copy"))
                oldLocalID = oldLocalID.Substring(0, oldLocalID.IndexOf("_copy"));
            oldLocalID = oldLocalID + "_copy";

            // Set Preface
            string localID;
            string newKey;
            if (type.Equals(EcellObject.PROCESS))
            {
                localID = GetTemporaryID(ProcessList, oldLocalID);
                newKey = systemID + ":" + localID;
            }
            else if (type.Equals(EcellObject.VARIABLE))
            {
                localID = GetTemporaryID(VariableList, oldLocalID);
                newKey = systemID + ":" + localID;
            }
            else if (type.Equals(EcellObject.TEXT))
            {
                localID = GetTemporaryID(TextList, oldLocalID);
                newKey = "/:" + localID;
            }
            else if (type.Equals(EcellObject.SYSTEM))
            {
                if (systemID == null || systemID == "/")
                    systemID = "";
                localID = GetTemporaryID(SystemList, oldLocalID);
                newKey = systemID + "/" + localID;
            }
            else if (type.Equals(EcellObject.STEPPER))
            {
                newKey = GetTemporaryID(m_stepperDic[modelID], oldLocalID);
            }
            else
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "type"));
            }
            return newKey;
        }

        /// <summary>
        /// GetTemporaryID
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pref"></param>
        /// <returns></returns>
        private static string GetTemporaryID(List<EcellObject> list, string pref)
        {
            int i = 0, max = 0;
            string tempID;
            foreach (EcellObject obj in list)
            {
                i = Util.ParseTemporaryID(obj.LocalID);
                tempID = pref + i.ToString();

                if (!tempID.Equals(obj.LocalID))
                    continue;
                if (i + 1 > max)
                    max = i + 1;
            }
            return pref + max.ToString();
        }

        /// <summary>
        /// Get EcellObject of this project.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public EcellObject GetEcellObject(string model, string type, string key, bool isDefault)
        {
            if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(type))
                return null;
            if (type.Equals(EcellObject.STEPPER))
                return GetStepper(model, key, isDefault);
            if (type.Equals(EcellObject.MODEL))
                return m_modelList[0];
            if (type.Equals(EcellObject.SYSTEM))
                return GetSystem(model, key, isDefault);
            else 
                return GetEntity(model, key, type, isDefault);
        }

        public EcellObject GetEcellObject(string model, string type, string key, string simParam)
        {
            EcellObject obj = GetEcellObject(model, type, key, true);
            obj = obj.Clone();
            foreach (EcellData data in obj.Value)
            {
                if (InitialCondition[simParam][model].ContainsKey(data.EntityPath))
                {
                    data.Value = new EcellValue(InitialCondition[simParam][model][data.EntityPath]);
                }
            }
            return obj;
        }

        /// <summary>
        /// Get System.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private EcellObject GetSystem(string model, string key, bool isDefault)
        {
            // Check systemList
            if(m_systemDic == null || !m_systemDic.ContainsKey(model))
                return null;
            List<EcellObject> systemList = m_systemDic[model];

            EcellObject system = null;
            foreach (EcellObject sys in systemList)
            {
                if (!sys.Key.Equals(key))
                    continue;
                system = sys;
                break;
            }

            if (!isDefault && !Info.SimulationParam.Equals(Constants.defaultSimParam))
            {
                string simParam = Info.SimulationParam;
                system = system.Clone();
                foreach (EcellData data in system.Value)
                {
                    if (InitialCondition[simParam][model].ContainsKey(data.EntityPath))
                    {
                        data.Value = new EcellValue(InitialCondition[simParam][model][data.EntityPath]);
                    }
                }
            }
            return system;
        }

        /// <summary>
        /// Get Entity.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private EcellObject GetEntity(string model, string key, string type, bool isDefault)
        {
            EcellObject system = GetSystem(model, Util.GetSuperSystemPath(key),isDefault);
            if (system == null || system.Children == null || system.Children.Count <= 0)
                return null;

            EcellObject entity = null;
            foreach (EcellObject child in system.Children)
            {
                if (!child.Type.Equals(type) || !child.Key.Equals(key))
                    continue;
                entity = child;
                break;
            }

            if (entity != null && !isDefault && !Info.SimulationParam.Equals(Constants.defaultSimParam))
            {
                string simParam = Info.SimulationParam;               
                entity = entity.Clone();
                foreach (EcellData data in entity.Value)
                {
                    if (InitialCondition[simParam][model].ContainsKey(data.EntityPath))
                    {
                        data.Value = new EcellValue(InitialCondition[simParam][model][data.EntityPath]);
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// Get Stepper.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private EcellObject GetStepper(string model, string key, bool isDefault)
        {
            EcellObject stepper = null;
            List<EcellObject> list = m_stepperDic[model];
            foreach (EcellObject eo in list)
            {
                if (eo.Key == key)
                    stepper = eo;
            }
            if (!isDefault && !Info.SimulationParam.Equals(Constants.defaultSimParam))
            {
                string simParam = Info.SimulationParam;
                stepper = stepper.Clone();
                foreach (EcellData data in stepper.Value)
                {
                    if (InitialCondition[simParam][model].ContainsKey(data.EntityPath))
                    {
                        data.Value = new EcellValue(InitialCondition[simParam][model][data.EntityPath]);
                    }
                }
            }
            return stepper;
        }

        /// <summary>
        /// Get a list of Revisions
        /// </summary>
        /// <returns></returns>
        public List<string> GetRevisions()
        {
            List<string> list = new List<string>();
            if (m_info.ProjectPath == null)
                return list;

            // Get Revision names.
            string revDir = Path.Combine(Util.GetBaseDir(), m_info.Name);
            string[] temp = Directory.GetDirectories(m_info.ProjectPath, "Revision*");
            foreach (string revision in temp)
            {
                list.Add(Path.GetFileName(revision));
            }
            return list;
        }
        #endregion

        #region Add Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="system"></param>
        public void AddSystem(EcellObject system)
        {
            m_systemDic[system.ModelID].Add(system);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(EcellObject entity)
        {
            EcellObject system = GetSystem(entity.ModelID, entity.ParentSystemID, true);
            system.Children.Add(entity);
        }
        #endregion

        #region Delete Object
        /// <summary>
        /// Delete System.
        /// </summary>
        /// <param name="system"></param>
        public void DeleteSystem(EcellObject system)
        {
            string type, ekey, param;
            m_systemDic[system.ModelID].Remove(system);
            // Delete Simulation parameter.
            foreach (string keyParamID in m_initialCondition.Keys)
            {
                foreach (string delModel in m_initialCondition[keyParamID].Keys)
                {
                    Debug.Assert(system.Type == Constants.xpathSystem);
                    string delKey = system.Key;
                    List<String> delKeyList = new List<string>();
                    foreach (string entKey in m_initialCondition[keyParamID][delModel].Keys)
                    {
                        Util.ParseFullPN(entKey, out type, out ekey, out param);
                        if (ekey.Equals(delKey) || ekey.StartsWith(delKey + "/") || ekey.StartsWith(delKey + ":"))
                            delKeyList.Add(entKey);
                    }
                    foreach (string entKey in delKeyList)
                    {
                        m_initialCondition[keyParamID][delModel].Remove(entKey);
                    }
                }
            }
        }
        /// <summary>
        /// Delete Entity.
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEntity(EcellObject entity)
        {
            // set param
            string model = entity.ModelID;
            string key = entity.Key;
            string sysKey = entity.ParentSystemID;
            string type = entity.Type;
            // delete entity
            foreach (EcellObject system in m_systemDic[entity.ModelID])
            {
                if (!system.Key.Equals(sysKey))
                    continue;
                foreach (EcellObject child in system.Children)
                {
                    if (!child.Key.Equals(key) || !child.Type.Equals(type))
                        continue;
                    system.Children.Remove(child);
                    break;
                }
            }

            // Delete Simulation parameter.
            foreach (string keyParameterID in m_initialCondition.Keys)
            {
                Dictionary<string, double> condition = m_initialCondition[keyParameterID][model];
                foreach (EcellData data in entity.Value)
                {
                    if (!data.Settable)
                        continue;
                    if (!condition.ContainsKey(data.EntityPath))
                        continue;
                    condition.Remove(data.EntityPath);
                }
            }

        }
        #endregion

        #region InitialCondition

        public void SetInitialCondition(EcellObject newobj)
        {
            EcellObject defaultObj = GetEcellObject(newobj.ModelID,
                newobj.Type, newobj.Key, true);

            foreach (EcellData newData in newobj.Value)
            {
                if (!newData.Settable || (!newData.Value.IsDouble && !newData.Value.IsInt))
                {
                    defaultObj.RemoveEcellValue(newData.Name);
                    defaultObj.Value.Add(newData);
                    continue;
                }
                EcellData defaultData = defaultObj.GetEcellData(newData.Name);
                if (defaultData == null)
                    continue;
                double newProp = Convert.ToDouble(newData.Value.Value.ToString());
                double defaultProp = Convert.ToDouble(defaultData.Value.Value.ToString());
                if (newProp != defaultProp)
                {
                    InitialCondition[Info.SimulationParam][newobj.ModelID][newData.EntityPath] = newProp;
                }
                else
                {
                    InitialCondition[Info.SimulationParam][newobj.ModelID].Remove(newData.EntityPath);
                }
            }
        }

        public void UpdateInitialCondition(EcellObject oldObj, EcellObject newObj)
        {
            string modelID = oldObj.ModelID;
            foreach (EcellData oldData in oldObj.Value)
            {
                EcellData newData = newObj.GetEcellData(oldData.Name);
                if (newData == null)
                    continue;
                string oldPath = oldData.EntityPath;
                string newPath = newData.EntityPath;
                foreach (string simParam in InitialCondition.Keys)
                {
                    if (InitialCondition[simParam][modelID].ContainsKey(oldPath))
                    {
                        InitialCondition[simParam][modelID][newPath] =
                            InitialCondition[simParam][modelID][oldPath];
                        InitialCondition[simParam][modelID].Remove(oldPath);
                    }
                }
            }
        }

        public void DeleteInitialCondition(EcellObject obj)
        {
            string modelID = obj.ModelID;
            foreach (String simParam in InitialCondition.Keys)
            {
                foreach (EcellData d in obj.Value)
                {
                    if (InitialCondition[simParam][modelID].ContainsKey(d.EntityPath))
                        InitialCondition[simParam][modelID].Remove(d.EntityPath);
                }
            }
        }

        public void DeleteInitialCondition(string simParam, String entityPath)
        {
            string modelID = Model.ModelID;
            if (InitialCondition[simParam][modelID].ContainsKey(entityPath))
                InitialCondition[simParam][modelID].Remove(entityPath);
        }

        public void SetInitialCondition(string simParam, string entityPath, double data)
        {
            string modelID = Model.ModelID;

            InitialCondition[simParam][modelID][entityPath] = data;
        }
        #endregion

        #endregion

    }
}
