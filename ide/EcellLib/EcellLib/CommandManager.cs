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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using IronPython.Hosting;
using IronPython.Runtime;
using EcellCoreLib;
using EcellLib.Objects;

namespace EcellLib
{
    /// <summary>
    /// Wraps the "DataManager" for the "IronPython".
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// The "IronPythonConsole.exe"
        /// </summary>
        private const string s_consoleExe = "IronPythonConsole.exe";
        /// <summary>
        /// The model ID
        /// </summary>
        private static string s_modelID = null;
        /// <summary>
        /// The singleton object.
        /// This object is used when the data is exchanged among IDE and script.
        /// </summary>
        public static CommandManager s_instance;

        /// <summary>
        /// get DataManager.
        /// </summary>
        public DataManager DataManager
        {
            get { return m_env.DataManager; }
        }


        private ApplicationEnvironment m_env;

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="env">the environment object.</param>
        public CommandManager(ApplicationEnvironment env)
        {
            m_env = env;
            s_instance = this;
        }

        /// <summary>
        /// Get the singleton object.
        /// </summary>
        /// <returns>CommandManager.</returns>
        public static CommandManager GetInstance()
        {
            if (s_instance == null)
                new ApplicationEnvironment();

            return s_instance;
        }

        /// <summary>
        /// Creates the entity of the full ID.
        /// </summary>
        /// <param name="l_fullID">the created full ID</param>
        /// <returns>the created entity</returns>
        public EntityStub CreateEntityStub(string l_fullID)
        {
            return new EntityStub(this, l_fullID);
        }

        /// <summary>
        /// Creates the logger of the full PN.
        /// </summary>
        /// <param name="l_fullPN">The logged full PN</param>
        public void CreateLogger(string l_fullPN)
        {
            string[] l_fullIDs = l_fullPN.Split(Constants.delimiterColon.ToCharArray());
            if (l_fullIDs.Length != 4)
            {
                throw new Exception(MessageResLib.ErrInvalidID);
            }
            List<EcellObject> l_systemObjectList
                    = m_env.DataManager.GetData(s_modelID, l_fullIDs[1]);

            if (l_systemObjectList == null || l_systemObjectList.Count <= 0)
            {
                throw new Exception(String.Format(MessageResLib.ErrFindEnt,
                    new object[] { l_fullPN }));
            }
            //
            // Searchs the fullID
            //
            string l_changedKey = null;
            string l_changedType = null;
            EcellObject l_changedObject = null;
            foreach (EcellObject l_systemObject in l_systemObjectList)
            {
                if (!l_systemObject.Type.Equals(Constants.xpathSystem))
                {
                    continue;
                }
                if (l_fullIDs[0].Equals(Constants.xpathSystem))
                {
                    if (l_systemObject.Key.Equals(l_fullIDs[2]))
                    {
                        if (l_systemObject.Value == null || l_systemObject.Value.Count <= 0)
                        {
                            continue;
                        }
                        foreach (EcellData l_systemData in l_systemObject.Value)
                        {
                            if (l_systemData.Logable && l_systemData.Name.Equals(l_fullIDs[3]))
                            {
                                l_systemData.Logged = true;
                                l_changedKey = l_fullIDs[2];
                                l_changedType = l_fullIDs[0];
                                l_changedObject = l_systemObject;
                                break;
                            }
                        }
                    }
                }
                else if (l_fullIDs[0].Equals(Constants.xpathProcess) || l_fullIDs[0].Equals(Constants.xpathVariable))
                {
                    if (l_systemObject.Children == null || l_systemObject.Children.Count <= 0)
                    {
                        continue;
                    }
                    foreach (EcellObject l_childObject in l_systemObject.Children)
                    {
                        if (l_childObject.Type.Equals(l_fullIDs[0])
                                && l_childObject.Key.Equals(l_fullIDs[1] + Constants.delimiterColon + l_fullIDs[2]))
                        {
                            if (l_childObject.Value == null || l_childObject.Value.Count <= 0)
                            {
                                continue;
                            }
                            foreach (EcellData l_childData in l_childObject.Value)
                            {
                                if (l_childData.Logable && l_childData.Name.Equals(l_fullIDs[3]))
                                {
                                    l_childData.Logged = true;
                                    l_changedKey = l_fullIDs[1]
                                            + Constants.delimiterColon + l_fullIDs[2];
                                    l_changedType = l_fullIDs[0];
                                    l_changedObject = l_childObject;
                                    break;
                                }
                            }
                        }
                        if (l_changedKey != null && l_changedType != null && l_changedObject != null)
                        {
                            break;
                        }
                    }
                }
                if (l_changedKey != null && l_changedType != null && l_changedObject != null)
                {
                    break;
                }
            }
            if (l_changedKey != null && l_changedType != null && l_changedObject != null)
            {
                m_env.DataManager.DataChanged(
                        s_modelID, l_changedKey, l_changedType, l_changedObject);
                m_env.PluginManager.LoggerAdd(
                        s_modelID, l_changedType, l_changedKey, l_fullPN);
            }
            else
            {
                throw new Exception(String.Format(MessageResLib.ErrFindEnt,
                    new object[] { l_fullPN }));
            }
        }

        /// <summary>
        /// Creates the logger policy with some parameters.
        /// </summary>
        /// <param name="l_savedStepCount">The saved step count</param>
        /// <param name="l_savedInterval">The saved interval</param>
        /// <param name="l_diskFullAction">The action if the HDD is full</param>
        /// <param name="l_maxDiskSpace">The limit of the usable HDD</param>
        public void CreateLoggerPolicy(
                int l_savedStepCount,
                double l_savedInterval,
                int l_diskFullAction,
                int l_maxDiskSpace)
        {
            LoggerPolicy l_loggerPolicy
                    = new LoggerPolicy(l_savedStepCount, l_savedInterval, l_diskFullAction, l_maxDiskSpace);
            m_env.DataManager.SetLoggerPolicy(m_env.DataManager.GetCurrentSimulationParameterID(),
                    ref l_loggerPolicy);
        }

        /// <summary>
        /// Creates the logger stub of the full PN.
        /// </summary>
        /// <param name="l_fullPN">The logged full PN</param>
        /// <returns>the created logger stub</returns>
        public LoggerStub CreateLoggerStub(string l_fullPN)
        {
            return new LoggerStub(this, l_fullPN);
        }

        /// <summary>
        /// Creates the model of the model ID.
        /// </summary>
        /// <param name="l_modelID">the model ID</param>
        public void CreateModel(string l_modelID)
        {
            List<EcellObject> l_list = new List<EcellObject>();
            l_list.Add(EcellObject.CreateObject(l_modelID, null, Constants.xpathModel, null, null));
            m_env.DataManager.DataAdd(l_list);
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
            s_modelID = l_modelID;
        }

        /// <summary>
        /// Creates the project.
        /// </summary>
        /// <param name="l_projectID">The project name</param>
        /// <param name="l_comment">The comment of the project</param>
        public void CreateProject(string l_projectID, string l_comment)
        {
            m_env.DataManager.CreateProject(l_projectID, l_comment, null, new List<string>());
        }

        /// <summary>
        /// Creates the simulation parameter stub.
        /// </summary>
        /// <param name="l_parameterID">the simulation parameter ID</param>
        /// <returns>the simulation parameter stub</returns>
        public SimulationParameterStub CreateSimulationParameterStub(string l_parameterID)
        {
            return new SimulationParameterStub(this, l_parameterID);
        }

        /// <summary>
        /// Creates the stepper stub.
        /// </summary>
        /// <param name="l_ID">the created stepper ID</param>
        /// <returns>the created stepper stub</returns>
        public StepperStub CreateStepperStub(string l_ID)
        {
            return new StepperStub(this, l_ID);
        }

        /// <summary>
        /// Creates the stepper stub.
        /// </summary>
        /// <param name="l_parameterID">the simulation parameter ID</param>
        /// <param name="l_ID">the created stepper ID</param>
        /// <returns>the created stepper stub</returns>
        public StepperStub CreateStepperStub(string l_parameterID, string l_ID)
        {
            return new StepperStub(l_parameterID, l_ID);
        }

        /// <summary>
        /// Deletes the default stepper.
        /// </summary>
        public void DeleteDefaultStepperStub()
        {
            CommandManager.StepperStub l_defaultStepper
                = this.CreateStepperStub("DefaultParameter", "DefaultStepper");
            l_defaultStepper.Create("FixedODE1Stepper");
            l_defaultStepper.Delete();
        }

        /// <summary>
        /// Deletes the logger of the full PN.
        /// </summary>
        /// <param name="l_fullPN">the full PN</param>
        public void DeleteLogger(string l_fullPN)
        {
            string[] l_fullPNDivs = l_fullPN.Split(Constants.delimiterColon.ToCharArray());
            if (l_fullPNDivs.Length != 4)
            {
                throw new Exception(MessageResLib.ErrInvalidID);
            }
            List<EcellObject> l_systemObjectList
                    = m_env.DataManager.GetData(s_modelID, l_fullPNDivs[1]);
            if (l_systemObjectList == null || l_systemObjectList.Count <= 0)
            {
                throw new Exception(String.Format(MessageResLib.ErrFindEnt,
                    new object[] { l_fullPN }));
            }
            //
            // Searchs the fullID
            //
            string l_changedKey = null;
            string l_changedType = null;
            EcellObject l_changedObject = null;
            foreach (EcellObject l_systemObject in l_systemObjectList)
            {
                if (!l_systemObject.Type.Equals(Constants.xpathSystem))
                {
                    continue;
                }
                if (l_fullPNDivs[0].Equals(Constants.xpathSystem))
                {
                    if (l_systemObject.Key.Equals(l_fullPNDivs[2]))
                    {
                        if (l_systemObject.Value == null || l_systemObject.Value.Count <= 0)
                        {
                            continue;
                        }
                        foreach (EcellData l_systemValue in l_systemObject.Value)
                        {
                            if (l_systemValue.Logable && l_systemValue.Name.Equals(l_fullPNDivs[3]))
                            {
                                l_systemValue.Logged = false;
                                l_changedKey = l_fullPNDivs[2];
                                l_changedType = l_fullPNDivs[0];
                                l_changedObject = l_systemObject;
                                break;
                            }
                        }
                    }
                }
                else if (l_fullPNDivs[0].Equals(Constants.xpathProcess)
                        || l_fullPNDivs[0].Equals(Constants.xpathVariable))
                {
                    if (l_systemObject.Children == null || l_systemObject.Children.Count <= 0)
                    {
                        continue;
                    }
                    foreach (EcellObject l_childObject in l_systemObject.Children)
                    {
                        if (l_childObject.Type.Equals(l_fullPNDivs[0])
                                && l_childObject.Key.Equals(l_fullPNDivs[1]
                                + Constants.delimiterColon + l_fullPNDivs[2]))
                        {
                            if (l_childObject.Value == null || l_childObject.Value.Count <= 0)
                            {
                                continue;
                            }
                            foreach (EcellData l_childValue in l_childObject.Value)
                            {
                                if (l_childValue.Logable && l_childValue.Name.Equals(l_fullPNDivs[3]))
                                {
                                    l_childValue.Logged = false;
                                    l_changedKey = l_fullPNDivs[1]
                                            + Constants.delimiterColon + l_fullPNDivs[2];
                                    l_changedType = l_fullPNDivs[0];
                                    l_changedObject = l_childObject;
                                    break;
                                }
                            }
                        }
                        if (l_changedKey != null && l_changedType != null && l_changedObject != null)
                        {
                            break;
                        }
                    }
                }
                if (l_changedKey != null && l_changedType != null && l_changedObject != null)
                {
                    break;
                }
            }
            if (l_changedKey != null && l_changedType != null && l_changedObject != null)
            {
                m_env.DataManager.DataChanged(s_modelID, l_changedKey, l_changedType, l_changedObject);
            }
            else
            {
                throw new Exception(String.Format(MessageResLib.ErrFindEnt,
                    new object[] { l_fullPN }));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_execFileName"></param>
        /// <param name="l_argument"></param>
        public void Exec(string l_execFileName, string l_argument)
        {
            try
            {
                Process execProcess = new Process();
                execProcess.StartInfo.Arguments = l_argument;
                execProcess.StartInfo.CreateNoWindow = true;
                execProcess.StartInfo.FileName = l_execFileName;
                execProcess.StartInfo.UseShellExecute = false;
                execProcess.Start();
                execProcess.WaitForExit();
            }
            catch (Exception l_ex)
            {
                throw new Exception(String.Format(MessageResLib.ErrLoadFile,
                    new object[] { l_execFileName }), l_ex);
            }
        }

        /// <summary>
        /// Returns the current project ID.
        /// </summary>
        /// <returns>the current project ID</returns>
        public string GetCurrentProjectID()
        {
            return m_env.DataManager.CurrentProjectID;
        }

        /// <summary>
        /// Returns the current simulation parameter.
        /// </summary>
        /// <returns>the current simulaton parameter</returns>
        public string GetCurrentSimulationParameterID()
        {
            return m_env.DataManager.GetCurrentSimulationParameterID();
        }

        /// <summary>
        /// Returns the current time of the simulator.
        /// </summary>
        /// <returns>The current time of the simulator</returns>
        public double GetCurrentSimulationTime()
        {
            return m_env.DataManager.GetCurrentSimulationTime();
        }

        /// <summary>
        /// Returns the entity list.
        /// </summary>
        /// <param name="l_entityName">The entity name</param>
        /// <param name="l_systemPath">The system path</param>
        /// <returns>The entity list</returns>
        public List<string> GetEntityList(string l_entityName, string l_systemPath)
        {
            if (l_entityName == null || l_entityName.Length <= 0
                    || l_systemPath == null || l_systemPath.Length <= 0)
            {
                return null;
            }
            if (l_entityName.Equals(Constants.xpathSystem))
            {
                List<string> l_list = new List<string>();
                int depth = l_systemPath.Split(Constants.delimiterPath.ToCharArray()).Length;
                if (l_systemPath.Equals(Constants.delimiterPath))
                {
                    foreach (string l_system in m_env.DataManager.GetSystemList(s_modelID))
                    {
                        if (l_systemPath.Equals(l_system))
                        {
                            continue;
                        }
                        else if (depth == l_system.Split(Constants.delimiterPath.ToCharArray()).Length)
                        {
                            l_list.Add(l_system.Replace(Constants.delimiterPath, ""));
                        }
                    }
                }
                else
                {
                    foreach (string l_system in m_env.DataManager.GetSystemList(s_modelID))
                    {
                        if (l_systemPath.Equals(l_system))
                        {
                            continue;
                        }
                        else if ((depth + 1) == l_system.Split(Constants.delimiterPath.ToCharArray()).Length)
                        {
                            l_list.Add(l_system.Replace(l_systemPath, "").Replace(Constants.delimiterPath, ""));
                        }
                    }
                }
                return l_list;
            }
            else if (l_entityName.Equals(Constants.xpathProcess) || l_entityName.Equals(Constants.xpathVariable))
            {
                List<string> l_list = new List<string>();
                foreach (EcellObject l_parent in m_env.DataManager.GetData(s_modelID, l_systemPath))
                {
                    if (l_parent.Children == null || l_parent.Children.Count <= 0)
                    {
                        continue;
                    }
                    foreach (EcellObject l_child in l_parent.Children)
                    {
                        if (l_child.Type.Equals(l_entityName))
                        {
                            l_list.Add(l_child.Key.Split(Constants.delimiterColon.ToCharArray())[1]);
                        }
                    }
                }
                return l_list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the entity property.
        /// </summary>
        /// <param name="l_fullPN">The full PN</param>
        /// <returns>The entity property of the full PN</returns>
        public EcellValue GetEntityProperty(string l_fullPN)
        {
            if (this.GetCurrentSimulationTime() <= 0.0)
            {
                return this.GetEntityPropertyFromDic(l_fullPN);
            }
            else
            {
                EcellValue v = this.GetEntityPropertyFromSimulator(l_fullPN);
                return this.GetEntityPropertyFromSimulator(l_fullPN);
            }
        }

        /// <summary>
        /// Returns the entity property of the dic.
        /// </summary>
        /// <param name="l_fullPN">The full PN</param>
        /// <returns>The entity property of the full PN</returns>
        private EcellValue GetEntityPropertyFromDic(string l_fullPN)
        {
            if (l_fullPN.IndexOf(Constants.delimiterColon) < 0)
            {
                return null;
            }
            string[] pathElements = l_fullPN.Split(Constants.delimiterColon.ToCharArray());
            if (pathElements[0].Equals(Constants.xpathSystem))
            {
                EcellObject l_system
                    = (m_env.DataManager.GetData(
                        s_modelID, pathElements[1] + Constants.delimiterColon + pathElements[2]))[0];
                foreach (EcellData l_systemProperty in l_system.Value)
                {
                    if (l_systemProperty.Name.Equals(pathElements[3]))
                    {
                        return l_systemProperty.Value;
                    }
                }
            }
            else
            {
                EcellObject l_system
                    = (m_env.DataManager.GetData(
                        s_modelID, pathElements[1]))[0];
                foreach (EcellObject l_entity in l_system.Children)
                {
                    if (l_entity.Type.Equals(pathElements[0])
                        && l_entity.Key.Equals(pathElements[1] + Constants.delimiterColon + pathElements[2]))
                    {
                        foreach (EcellData l_entityProperty in l_entity.Value)
                        {
                            if (l_entityProperty.Name.Equals(pathElements[3]))
                            {
                                return l_entityProperty.Value;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the entity property of the simulator.
        /// </summary>
        /// <param name="l_fullPN">The full PN</param>
        /// <returns>The entity property of the full PN</returns>
        private EcellValue GetEntityPropertyFromSimulator(string l_fullPN)
        {
            return m_env.DataManager.GetEntityProperty(l_fullPN);
        }

        /// <summary>
        /// Returns the logger list.
        /// </summary>
        /// <returns>The logger list</returns>
        public List<string> GetLoggerList()
        {
            List<string> l_list = new List<string>();
            foreach (string l_systemPath in m_env.DataManager.GetSystemList(s_modelID))
            {
                foreach (EcellObject l_system in m_env.DataManager.GetData(s_modelID, l_systemPath))
                {
                    if (l_system.Value != null && l_system.Value.Count > 0)
                    {
                        foreach (EcellData l_data in l_system.Value)
                        {
                            if (l_data.Logged)
                            {
                                l_list.Add(l_data.EntityPath);
                            }
                        }
                    }
                    if (l_system.Children != null && l_system.Children.Count > 0)
                    {
                        foreach (EcellObject l_entity in l_system.Children)
                        {
                            if (l_entity.Value != null && l_entity.Value.Count > 0)
                            {
                                foreach (EcellData l_data in l_entity.Value)
                                {
                                    if (l_data.Logged)
                                    {
                                        l_list.Add(l_data.EntityPath);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return l_list;
        }

        /// <summary>
        /// Returns the process list.
        /// </summary>
        /// <returns>The process list</returns>
        public List<string> GetProcessList()
        {
            return m_env.DataManager.GetEntityList(s_modelID, Constants.xpathProcess);
        }

        /// <summary>
        /// Returns the list of the simulation parameter ID.
        /// </summary>
        /// <returns>the list of the simulation parameter ID</returns>
        public List<string> GetSimulationParameterIDList()
        {
            return m_env.DataManager.GetSimulationParameterIDs();
        }

        /// <summary>
        /// Returns the stepper list of the current simulation parameter.
        /// </summary>
        /// <returns>The stepper list of the current simulation parameter</returns>
        public List<string> GetStepperList()
        {
            return this.GetStepperList(this.GetCurrentSimulationParameterID());
        }

        /// <summary>
        /// Returns the stepper list.
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <returns>The stepper list</returns>
        public List<string> GetStepperList(string l_parameterID)
        {
            List<string> l_list = new List<string>();
            foreach (EcellObject l_stepper in m_env.DataManager.GetStepper(l_parameterID, s_modelID))
            {
                l_list.Add(l_stepper.Key);
            }
            return l_list;
        }

        /// <summary>
        /// Returns the selected log data.
        /// </summary>
        /// <param name="l_startTime">The start time of the logger</param>
        /// <param name="l_endTime">The end time of the logger</param>
        /// <param name="l_fullPN">The logged full PN</param>
        /// <returns></returns>
        public List<LogValue> GetLogData(string l_fullPN, double l_startTime, double l_endTime)
        {
            double l_interval
                    = m_env.DataManager
                            .GetLoggerPolicy(m_env.DataManager.GetCurrentSimulationParameterID())
                            .m_reloadInterval;
            return m_env.DataManager
                    .GetLogData(l_startTime, l_endTime, l_interval, l_fullPN).logValueList;
        }

        /// <summary>
        /// Returns the logger policy.
        /// </summary>
        /// <returns>the logger policy</returns>
        public LoggerPolicy GetLoggerPolicy()
        {
            return m_env.DataManager.GetLoggerPolicy(
                    m_env.DataManager.GetCurrentSimulationParameterID());
        }

        /// <summary>
        /// Returns the next event.
        /// </summary>
        /// <returns>The next event</returns>
        public ArrayList GetNextEvent()
        {
            return m_env.DataManager.GetNextEvent();
        }

        /// <summary>
        /// Returns the variable list.
        /// </summary>
        /// <returns>The variable list</returns>
        public List<string> GetVariableList()
        {
            return m_env.DataManager.GetEntityList(s_modelID, Constants.xpathVariable);
        }

        /// <summary>
        /// Initializes the simulation.
        /// </summary>
        public void Initialize()
        {
            m_env.DataManager.Initialize(true);
        }

        /// <summary>
        /// Checks whether the simulator is running.
        /// </summary>
        /// <returns>true if the simulator is running; false otherwise</returns>
        public bool IsActive()
        {
            return m_env.DataManager.IsActive();
        }

        /// <summary>
        /// Activates the "IronPythonConsole.exe".
        /// </summary>
        public void Interact()
        {
            Process l_process = new Process();
            l_process.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\" + s_consoleExe;
            l_process.Start();
        }

        /// <summary>
        /// Loads the model.
        /// </summary>
        /// <param name="l_fileName">The "EML" file name</param>
        public void LoadModel(string l_fileName)
        {
            if (m_env.DataManager.CurrentProjectID == null)
            {
                String modelDir = Path.GetDirectoryName(l_fileName);
                if (modelDir.EndsWith(Constants.xpathModel))
                {
                    modelDir = modelDir.Substring(0, modelDir.Length - 5);
                }
                m_env.DataManager.CreateProject(Constants.defaultPrjID, DateTime.Now.ToString(), modelDir, new List<string>());
            }
            s_modelID = m_env.DataManager.LoadModel(l_fileName, false);
            m_env.PluginManager.LoadData(s_modelID);
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }

        /// <summary>
        /// Sends to the "Message" window the message.
        /// </summary>
        /// <param name="l_message">The message</param>
        public void Message(string l_message)
        {
            m_env.PluginManager.Message(Constants.messageSimulation, l_message);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            m_env.DataManager.CloseProject(null);
        }

        /// <summary>
        /// Runs the simulator.
        /// </summary>
        /// <param name="l_interval">The time limit of the simulator</param>
        public void Run(double l_interval)
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Running);
            m_env.DataManager.SimulationStartKeepSetting(l_interval);
        }

        /// <summary>
        /// Runs the simulator.
        /// </summary>
        /// <param name="l_interval">The time limit of the simulator</param>
        public void RunNotSuspend(double l_interval)
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Running);
            m_env.DataManager.SimulationStart(l_interval, 0);
        }

        /// <summary>
        /// Saves the selected log data.
        /// </summary>
        /// <param name="l_savedDirName">The saved directory name</param>
        /// <param name="l_startTime">The start time</param>
        /// <param name="l_endTime">The end time</param>
        /// <param name="l_fullID">The logged full ID</param>
        public void SaveLoggerData(string l_fullID, string l_savedDirName, double l_startTime, double l_endTime)
        {
            List<string> l_fullIDList = new List<string>();
            l_fullIDList.Add(l_fullID);
            m_env.DataManager
                    .SaveSimulationResult(l_savedDirName, l_startTime, l_endTime, Constants.xpathCsv, l_fullIDList);
        }

        /// <summary>
        /// Saves the model.
        /// </summary>
        /// <param name="l_modelID">The saved model ID</param>
        public void SaveModel(string l_modelID)
        {
            m_env.DataManager.SaveModel(l_modelID);
        }

        /// <summary>
        /// Sets the entity property.
        /// </summary>
        /// <param name="l_fullPN">The full PN</param>
        /// <param name="l_value">The property value</param>
        public void SetEntityProperty(string l_fullPN, string l_value)
        {
            m_env.DataManager.SetEntityProperty(l_fullPN, l_value);
        }

        /// <summary>
        /// Steps the simulator.
        /// </summary>
        /// <param name="l_count">The step limit of the simulator</param>
        public void Step(int l_count)
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Running);
            m_env.DataManager.SimulationStartKeepSetting(l_count);
        }

        /// <summary>
        /// Steps the simulator.
        /// </summary>
        /// <param name="l_count">The step limit of the simulator</param>
        public void StepNotSuspend(int l_count)
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Running);
            m_env.DataManager.SimulationStart(l_count, 0);
        }

        /// <summary>
        /// Stops the simulator.
        /// </summary>
        public void Stop()
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
            m_env.DataManager.SimulationStop();
        }

        /// <summary>
        /// Suspends the simulator.
        /// </summary>
        public void Suspend()
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Suspended);
            m_env.DataManager.SimulationSuspend();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_type">data type to set initial parameter.</param>
        /// <param name="l_initialDic">the dictionary of initial parameter.</param>
        public void UpdateInitialCondition(string l_type, Dictionary<string, double> l_initialDic)
        {
            if (l_type == null || l_type.Length <= 0
                || l_initialDic == null || l_initialDic.Count <= 0)
            {
                return;
            }
            m_env.DataManager.UpdateInitialCondition(null, s_modelID, l_type, l_initialDic);
        }


        /// <summary>
        /// Operates the entity.
        /// </summary>
        public class EntityStub
        {
            /// <summary>
            /// the entity
            /// </summary>
            private EcellObject m_ecellObject = null;
            /// <summary>
            /// the full ID
            /// </summary>
            private string m_fullID = null;

            /// <summary>
            /// CommandManager instance associated to this object.
            /// </summary>
            private CommandManager m_cManager = null;

            /// <summary>
            /// Creates the new "EntityStub" instance with the full ID.
            /// </summary>
            /// <param name="cManager">CommandManager instance to associate</param>
            /// <param name="l_fullID">the full ID</param>
            public EntityStub(CommandManager cManager, string l_fullID)
            {
                this.m_cManager = cManager;
                this.m_fullID = l_fullID;
            }

            /// <summary>
            /// Creates the entity stub.
            /// </summary>
            /// <param name="l_className">the class name</param>
            public void Create(string l_className)
            {
                Debug.Assert(!String.IsNullOrEmpty(s_modelID));

                //
                // Already get
                //
                if (this.m_ecellObject != null)
                {
                    return;
                }
                //
                // Refines the full ID
                //
                string l_key = null;
                string l_type = null;
                string l_systemKey = null;
                this.RefinedFullID(ref l_key, ref l_type, ref l_systemKey);
                //
                // Searches the loaded "EcellObject".
                //
                foreach (EcellObject l_system
                        in m_cManager.DataManager.GetData(
                            CommandManager.s_modelID, l_systemKey))
                {
                    if (l_type.Equals(Constants.xpathSystem))
                    {
                        this.m_ecellObject = l_system;
                        return;
                    }
                    else
                    {
                        if (l_system.Children != null && l_system.Children.Count > 0)
                        {
                            foreach (EcellObject l_entity in l_system.Children)
                            {
                                if (l_entity.Type.Equals(l_type) && l_entity.Key.Equals(l_key))
                                {
                                    this.m_ecellObject = l_entity;
                                    return;
                                }
                            }
                        }
                    }
                }
                //
                // Creates a new "EcellObject".
                //
                //
                // Checks whether the class exists.
                //
                this.Create(l_key, l_type, l_className);
                //
                // Adds the "EcellObject" to the "DataManager". 
                //
                List<EcellObject> l_list = new List<EcellObject>();
                l_list.Add(this.m_ecellObject);
                m_cManager.DataManager.DataAdd(l_list);
            }

            /// <summary>
            /// Creates the entity stub with some parameters.
            /// </summary>
            /// <param name="l_key">the "System" path of the entity.</param>
            /// <param name="l_type">the type of entity</param>
            /// <param name="l_className">the class name</param>
            private void Create(string l_key, string l_type, string l_className)
            {
                List<string> l_entityList = null;
                if (l_type.Equals(Constants.xpathSystem))
                {
                    l_entityList = m_cManager.DataManager.GetSystemList();
                }
                else if (l_type.Equals(Constants.xpathProcess))
                {
                    l_entityList = m_cManager.DataManager.GetProcessList();
                }
                else if (l_type.Equals(Constants.xpathVariable))
                {
                    l_entityList = m_cManager.DataManager.GetVariableList();
                }

                if (l_entityList != null && l_entityList.Count > 0)
                {
                    foreach (string l_entity in l_entityList)
                    {
                        if (l_className.Equals(l_entity))
                        {
                            List<EcellData> l_propertyList = new List<EcellData>();
                            if (l_type.Equals(Constants.xpathSystem))
                            {
                                Dictionary<string, EcellData> l_propertyDic
                                        = m_cManager.DataManager.GetSystemProperty();
                                foreach (string l_property in m_cManager.DataManager.GetSystemProperty().Keys)
                                {
                                    EcellData l_ecellData = l_propertyDic[l_property];
                                    l_ecellData.EntityPath
                                            = l_type + Constants.delimiterColon
                                            + l_key + Constants.delimiterColon + l_property;
                                    l_propertyList.Add(l_ecellData);
                                }
                            }
                            else if (l_type.Equals(Constants.xpathProcess))
                            {
                                Dictionary<string, EcellData> l_propertyDic
                                        = m_cManager.DataManager.GetProcessProperty(l_className);
                                foreach (string l_property in l_propertyDic.Keys)
                                {
                                    EcellData l_ecellData = l_propertyDic[l_property];
                                    l_ecellData.EntityPath
                                            = l_type + Constants.delimiterColon
                                            + l_key + Constants.delimiterColon + l_property;
                                    l_propertyList.Add(l_ecellData);
                                }
                            }
                            else
                            {
                                Dictionary<string, EcellData> l_propertyDic
                                        = m_cManager.DataManager.GetVariableProperty();
                                foreach (string l_property in m_cManager.DataManager.GetVariableProperty().Keys)
                                {
                                    EcellData l_ecellData = l_propertyDic[l_property];
                                    l_ecellData.EntityPath
                                            = l_type + Constants.delimiterColon
                                            + l_key + Constants.delimiterColon + l_property;
                                    l_propertyList.Add(l_ecellData);
                                }
                            }
                            this.m_ecellObject = EcellObject.CreateObject(
                                    CommandManager.s_modelID, l_key, l_type, l_className, l_propertyList);
                            return;
                        }
                    }
                }
            }

            /// <summary>
            /// Deletes the entity.
            /// </summary>
            public void Delete()
            {
                string l_key = null;
                string l_type = null;
                string l_systemKey = null;
                this.RefinedFullID(ref l_key, ref l_type, ref l_systemKey);
                m_cManager.DataManager.DataDelete(CommandManager.s_modelID, l_key, l_type);
                this.m_ecellObject = null;
                this.m_fullID = null;
            }

            /// <summary>
            /// Tests whether the entity is already loaded.
            /// </summary>
            /// <returns>true if the entity is loaded; false otherwise</returns>
            public bool Exists()
            {
                return m_cManager.DataManager.Exists(CommandManager.s_modelID, this.m_fullID);
            }

            /// <summary>
            /// Returns the class name.
            /// </summary>
            /// <returns>the class name</returns>
            public string GetClassName()
            {
                if (this.m_ecellObject != null)
                {
                    return this.m_ecellObject.Classname;
                }
                return null;
            }

            /// <summary>
            /// Returns the full ID.
            /// </summary>
            /// <returns>the full ID</returns>
            public string GetName()
            {
                return this.m_fullID;
            }

            /// <summary>
            /// Returns the value of the property.
            /// </summary>
            /// <param name="l_propertyName">the property name</param>
            /// <returns>the value of the property</returns>
            public string GetProperty(string l_propertyName)
            {
                if (this.m_ecellObject != null)
                {
                    if (this.m_ecellObject.Value != null && this.m_ecellObject.Value.Count > 0)
                    {
                        foreach (EcellData l_data in this.m_ecellObject.Value)
                        {
                            if (l_data.Name.Equals(l_propertyName))
                            {
                                return l_data.Value.ToString();
                            }
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// Returns the attributes of the property.
            /// </summary>
            /// <param name="l_propertyName">the property name</param>
            /// <returns>the attributes of the property(Settable, Gettable, Loadable, Savable)</returns>
            public List<bool> GetPropertyAttributes(string l_propertyName)
            {
                if (this.m_ecellObject != null)
                {
                    if (this.m_ecellObject.Value != null && this.m_ecellObject.Value.Count > 0)
                    {
                        foreach (EcellData l_data in this.m_ecellObject.Value)
                        {
                            if (l_data.Name.Equals(l_propertyName))
                            {
                                List<bool> l_list = new List<bool>();
                                l_list.Add(l_data.Settable);
                                l_list.Add(l_data.Gettable);
                                l_list.Add(l_data.Loadable);
                                l_list.Add(l_data.Saveable);
                                return l_list;
                            }
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// Returns the list of the property.
            /// </summary>
            /// <returns>the list of the property</returns>
            public List<string> GetPropertyList()
            {
                if (this.m_ecellObject != null)
                {
                    if (this.m_ecellObject.Value != null && this.m_ecellObject.Value.Count > 0)
                    {
                        List<string> l_list = new List<string>();
                        foreach (EcellData l_data in this.m_ecellObject.Value)
                        {
                            l_list.Add(l_data.Name);
                        }
                        l_list.Sort();
                        return l_list;
                    }
                }
                return null;
            }

            /// <summary>
            /// Refines the information of the EcellObject.
            /// </summary>
            private void RefinedEcellObject()
            {
                //
                // Refines the full ID
                //
                string l_key = null;
                string l_type = null;
                string l_systemKey = null;
                this.RefinedFullID(ref l_key, ref l_type, ref l_systemKey);
                //
                // Searches the loaded "EcellObject".
                //
                foreach (EcellObject l_system
                        in m_cManager.DataManager.GetData(CommandManager.s_modelID, l_systemKey))
                {
                    if (l_type.Equals(Constants.xpathSystem))
                    {
                        this.m_ecellObject = l_system;
                        return;
                    }
                    else
                    {
                        if (l_system.Children != null && l_system.Children.Count > 0)
                        {
                            foreach (EcellObject l_entity in l_system.Children)
                            {
                                if (l_entity.Type.Equals(l_type) && l_entity.Key.Equals(l_key))
                                {
                                    this.m_ecellObject = l_entity;
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Refines the information of the full ID.
            /// </summary>
            /// <param name="l_key">the system path of the entity</param>
            /// <param name="l_type">the type of the entity</param>
            /// <param name="l_systemKey">the system path of the entity</param>
            private void RefinedFullID(ref string l_key, ref string l_type, ref string l_systemKey)
            {
                string[] l_infos = this.m_fullID.Split(Constants.delimiterColon.ToCharArray());

                l_key = null;
                l_type = l_infos[0];
                l_systemKey = null;
                if (l_infos[1].Equals("") && l_infos[2].Equals(Constants.delimiterPath))
                {
                    l_key = Constants.delimiterPath;
                    l_systemKey = Constants.delimiterPath;
                }
                else
                {
                    if (l_infos[0].Equals(Constants.xpathSystem))
                    {
                        l_key = l_infos[1] + Constants.delimiterPath + l_infos[2];
                        l_systemKey = l_infos[1] + Constants.delimiterPath + l_infos[2];
                    }
                    else
                    {
                        l_key = l_infos[1] + Constants.delimiterColon + l_infos[2];
                        l_systemKey = l_infos[1];
                    }
                    l_key = l_key.Replace(Constants.delimiterPath + Constants.delimiterPath, Constants.delimiterPath);
                    l_systemKey = l_systemKey.Replace(Constants.delimiterPath + Constants.delimiterPath, Constants.delimiterPath);
                }
            }

            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="l_propertyName">the property</param>
            /// <param name="l_value">the value</param>
            public void SetProperty(string l_propertyName, string l_value)
            {
                //
                // Get a current EcellObject.
                //
                this.RefinedEcellObject();
                //
                // Set.
                //
                if (this.m_ecellObject != null)
                {
                    if (this.m_ecellObject.Value != null && this.m_ecellObject.Value.Count > 0)
                    {
                        bool l_findFlag = false;
                        for (int i = 0; i < this.m_ecellObject.Value.Count; i++)
                        {
                            EcellData l_data = this.m_ecellObject.Value[i];
                            if (l_data.Name.Equals(l_propertyName))
                            {
                                if (!l_data.Settable)
                                {
                                    throw new Exception(String.Format(MessageResLib.ErrSetProp,
                                        new object[] { l_propertyName }));
                                }
                                else if (l_propertyName.Equals(Constants.xpathVRL))
                                {
                                    l_data.Value = EcellValue.ToVariableReferenceList(l_value);
                                    //
                                    // Exchange ":.:" for ":[path]:".
                                    //
                                    string l_path = this.m_fullID.Split(Constants.delimiterColon.ToCharArray())[1];
                                    for (int j = 0; j < l_data.Value.CastToList().Count; j++)
                                    {
                                        string[] l_IDs
                                            = l_data.Value.CastToList()[j].CastToList()[1].CastToString()
                                                .Split(Constants.delimiterColon.ToCharArray());
                                        if (l_IDs[1].Equals(Constants.delimiterPeriod))
                                        {
                                            l_IDs[1] = l_path;
                                        }
                                        string l_ID = null;
                                        foreach (string l_IDElement in l_IDs)
                                        {
                                            l_ID = l_ID + Constants.delimiterColon + l_IDElement;
                                        }
                                        l_data.Value.CastToList()[j].CastToList()[1]
                                            = new EcellValue(l_ID.Substring(1));
                                    }
                                    l_findFlag = true;
                                }
                                else if (l_data.Value.IsDouble())
                                {
                                    l_data.Value = new EcellValue(XmlConvert.ToDouble(l_value));
                                    l_findFlag = true;
                                }
                                else if (l_data.Value.IsInt())
                                {
                                    l_data.Value = new EcellValue(Convert.ToInt32(l_value));
                                    l_findFlag = true;
                                }
                                else if (l_data.Value.IsString())
                                {
                                    l_data.Value = new EcellValue(l_value);
                                    l_findFlag = true;
                                }
                            }
                        }
                        if (!l_findFlag)
                        {
                            EcellData l_new
                                = new EcellData(
                                    l_propertyName,
                                    new EcellValue(Convert.ToDouble(l_value)),
                                    this.m_fullID + Constants.delimiterColon + l_propertyName);
                            l_new.Logable = true;
                            this.m_ecellObject.Value.Add(l_new);
                            // throw new Exception("The property named [" + l_propertyName + "]" + "isn't found.");
                        }
                        m_cManager.DataManager.DataChanged(
                                this.m_ecellObject.ModelID,
                                this.m_ecellObject.Key,
                                this.m_ecellObject.Type,
                                this.m_ecellObject);
                    }
                }
            }
        }


        /// <summary>
        /// Operates the logger.
        /// </summary>
        public class LoggerStub
        {
            /// <summary>
            ///  The creator of this object
            /// </summary>
            private CommandManager m_cManager;

            /// <summary>
            /// the full PN
            /// </summary>
            private string m_fullPN = null;

            /// <summary>
            /// the flug of the load
            /// </summary>
            private bool m_isExist = false;

            /// <summary>
            /// Creates the new "LoggerStub" instance with the full PN.
            /// </summary>
            /// <param name="cManager">the CommandManager assciated this object.</param>
            /// <param name="l_fullPN">the full PN</param>
            public LoggerStub(CommandManager cManager, string l_fullPN)
            {
                this.m_cManager = cManager;
                this.m_fullPN = l_fullPN;
            }

            /// <summary>
            /// Creates the logger.
            /// </summary>
            public void Create()
            {
                if (!this.m_isExist)
                {
                    m_cManager.CreateLogger(this.m_fullPN);
                    this.m_isExist = true;
                }
            }

            /// <summary>
            /// Deletes the logger.
            /// </summary>
            public void Delete()
            {
                m_cManager.DeleteLogger(this.m_fullPN);
                this.m_isExist = false;
            }

            /// <summary>
            /// Tests whether the logger is already loaded.
            /// </summary>
            /// <returns>true if the logger is loaded; false otherwise</returns>
            public bool Exists()
            {
                return this.m_isExist;
            }

            /// <summary>
            /// Returns the log data from the start time to the end time.
            /// </summary>
            /// <returns>the log data from the start time to the end time</returns>
            public List<LogValue> GetData(double l_startTime, double l_endTime)
            {
                return m_cManager.GetLogData(this.m_fullPN, l_startTime, l_endTime);
            }

            /// <summary>
            /// Returns the end time of the logger.
            /// </summary>
            /// <returns>the end time of the logger</returns>
            public double GetEndTime()
            {
                List<LogValue> l_logDataList =
                        m_cManager.GetLogData(
                                this.m_fullPN,
                                0.0,
                                m_cManager.GetCurrentSimulationTime());
                return l_logDataList[l_logDataList.Count - 1].time;
            }

            /// <summary>
            /// Returns the logger policy of the logger.
            /// </summary>
            /// <returns>the logger policy</returns>
            public LoggerPolicy GetLoggerPolicy()
            {
                return m_cManager.GetLoggerPolicy();
            }

            /// <summary>
            /// Returns the full PN.
            /// </summary>
            /// <returns>the full PN</returns>
            public string GetName()
            {
                return this.m_fullPN;
            }

            /// <summary>
            /// Returns the size of the logger.
            /// </summary>
            /// <returns>the size of the logger</returns>
            public int GetSize()
            {
                List<LogValue> l_logDataList =
                        m_cManager.GetLogData(
                            this.m_fullPN,
                            0.0,
                            m_cManager.GetCurrentSimulationTime());
                return l_logDataList.Count;
            }

            /// <summary>
            /// Returns the start time of the logger.
            /// </summary>
            /// <returns>the start time of the logger</returns>
            public double GetStartTime()
            {
                List<LogValue> l_logDataList =
                    m_cManager.GetLogData(
                        this.m_fullPN, 0.0,
                        m_cManager.GetCurrentSimulationTime());
                return l_logDataList[0].time;
            }

            /// <summary>
            /// Sets the logger policy of the logger.
            /// </summary>
            /// <param name="l_savedStepCount">the saved step count</param>
            /// <param name="l_savedInterval">the saved interval</param>
            /// <param name="l_diskFullAction">the HDD full action</param>
            /// <param name="l_maxDiskSpace">the max HDD space</param>
            public void SetLoggerPolicy(
                    int l_savedStepCount,
                    double l_savedInterval,
                    int l_diskFullAction,
                    int l_maxDiskSpace)
            {
                m_cManager.CreateLoggerPolicy(
                        l_savedStepCount, l_savedInterval, l_diskFullAction, l_maxDiskSpace);
            }
        }


        /// <summary>
        /// Operates the simulation parameter.
        /// </summary>
        public class SimulationParameterStub
        {
            /// <summary>
            /// CommandManager
            /// </summary>
            private CommandManager m_cManager;
            /// <summary>
            /// the logger policy belong to this
            /// </summary>
            private LoggerPolicy m_loggerPolicy;
            /// <summary>
            /// the simulation parameter ID
            /// </summary>
            private string m_parameterID = null;
            /// <summary>
            /// the list of the stepper belong to this
            /// </summary>
            private List<EcellObject> m_stepperList = null;
            /// <summary>
            /// the initial condition belong to this
            /// </summary>
            private Dictionary<string, Dictionary<string, double>> m_initialCondition = null;

            /// <summary>
            /// Creates the simulation parameter stub with the simulation parameter ID.
            /// </summary>
            /// <param name="cManager">the CommandManager associated this object.</param>
            /// <param name="l_parameterID">the simulation parameter ID</param>
            public SimulationParameterStub(CommandManager cManager, string l_parameterID)
            {
                this.m_cManager = cManager;
                this.m_parameterID = l_parameterID;
            }

            /// <summary>
            /// Creates the simulation parameter ID.
            /// </summary>
            public void Create()
            {
                Debug.Assert(!String.IsNullOrEmpty(CommandManager.s_modelID));
                //
                // Already get
                //
                if (this.m_stepperList != null)
                {
                    return;
                }
                //
                // Searches the simulation parameter.
                // 
                bool l_existFlag = false;
                foreach (string l_parameterID in m_cManager.GetSimulationParameterIDList())
                {
                    if (this.m_parameterID.Equals(l_parameterID))
                    {
                        l_existFlag = true;
                        break;
                    }
                }
                if (!l_existFlag)
                {
                    m_cManager.DataManager.CreateSimulationParameter(this.m_parameterID);
                }
                //
                // Searches the loaded "Stepper".
                //
                this.m_stepperList
                        = m_cManager.DataManager.GetStepper(this.m_parameterID, CommandManager.s_modelID);
                //
                // Searches the loaded "LoggerPolicy".
                //
                this.m_loggerPolicy = m_cManager.DataManager.GetLoggerPolicy(this.m_parameterID);
                //
                // Searches the loaded "InitialCondition".
                //
                this.m_initialCondition
                        = m_cManager.DataManager.GetInitialCondition(
                                this.m_parameterID, CommandManager.s_modelID);
            }

            /// <summary>
            /// Creates the stepper stub.
            /// </summary>
            /// <param name="l_stepperID">the stepper ID</param>
            /// <returns>the created stepper stub</returns>
            public StepperStub CreateStepperStub(string l_stepperID)
            {
                return new StepperStub(this.m_parameterID, l_stepperID);
            }

            /// <summary>
            /// Delates the simulation parameter stub.
            /// </summary>
            public void Delete()
            {
                m_cManager.DataManager.DeleteSimulationParameter(this.m_parameterID);
                this.m_parameterID = null;
                this.m_loggerPolicy = new LoggerPolicy();
                this.m_stepperList = null;
                this.m_initialCondition = null;
            }

            /// <summary>
            /// Tests whether the simulation parameter is already loaded.
            /// </summary>
            /// <returns>true if the simulation parameter is loaded; false otherwise</returns>
            public bool Exists()
            {
                foreach (string l_parameterID in m_cManager.DataManager.GetSimulationParameterIDs())
                {
                    if (l_parameterID.Equals(this.m_parameterID))
                    {
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// Returns the logger policy.
            /// </summary>
            /// <returns>the logger policy</returns>
            public LoggerPolicy GetLoggerPolicy()
            {
                return this.m_loggerPolicy;
            }

            /// <summary>
            /// Returns the initial condition of the type.
            /// </summary>
            /// <param name="l_type">the type of the entity</param>
            /// <returns>the initial condition</returns>
            public Dictionary<string, double> GetInitialCondition(string l_type)
            {
                if (l_type.Equals(Constants.xpathSystem))
                {
                    return this.m_initialCondition[Constants.xpathSystem];
                }
                else if (l_type.Equals(Constants.xpathProcess))
                {
                    return this.m_initialCondition[Constants.xpathProcess];
                }
                else if (l_type.Equals(Constants.xpathVariable))
                {
                    return this.m_initialCondition[Constants.xpathVariable];
                }
                return new Dictionary<string, double>();
            }

            /// <summary>
            /// Returns the simulation parameter ID.
            /// </summary>
            /// <returns>the simulation parameter ID</returns>
            public string GetSimulationParameterID()
            {
                return this.m_parameterID;
            }

            /// <summary>
            /// Returns the list of the stepper ID.
            /// </summary>
            /// <returns>the list of the stepper ID</returns>
            public List<string> GetStepperIDList()
            {
                List<string> l_stepperIDList = new List<string>();
                foreach (EcellObject l_ecellObject in this.m_stepperList)
                {
                    l_stepperIDList.Add(l_ecellObject.Key);
                }

                return l_stepperIDList;
            }

            /// <summary>
            /// Sets the logger policy of the logger.
            /// </summary>
            /// <param name="l_savedStepCount">the saved step count</param>
            /// <param name="l_savedInterval">the saved interval</param>
            /// <param name="l_diskFullAction">the HDD full action</param>
            /// <param name="l_maxDiskSpace">the max HDD space</param>
            public void SetLoggerPolicy(
                    int l_savedStepCount,
                    double l_savedInterval,
                    int l_diskFullAction,
                    int l_maxDiskSpace)
            {
                LoggerPolicy l_loggerPolicy
                        = new LoggerPolicy(l_savedStepCount, l_savedInterval, l_diskFullAction, l_maxDiskSpace);
                m_cManager.DataManager.SetLoggerPolicy(this.m_parameterID, ref l_loggerPolicy);
            }
        }


        /// <summary>
        /// Operates the stepper.
        /// </summary>
        public class StepperStub
        {
            /// <summary>
            /// the stepper
            /// </summary>
            private EcellObject m_stepper = null;
            /// <summary>
            /// the stepper ID
            /// </summary>
            private string m_ID = null;
            /// <summary>
            /// the simulation parameter ID the stepper belongs to
            /// </summary>
            private string m_parameterID = null;

            /// <summary>
            /// CommandManager instance associated to this object.
            /// </summary>
            private CommandManager m_cManager = null;

            /// <summary>
            /// Creates the stepper stub with no argument.
            /// </summary>
            private StepperStub()
            {
            }

            /// <summary>
            /// Creates the stepper stub with the current simulation parameter and the stepper ID.
            /// </summary>
            public StepperStub(CommandManager cManager, string l_ID)
            {
                this.m_cManager = cManager;
                this.m_ID = l_ID;
            }

            /// <summary>
            /// Creates the stepper stub with the simulation parameter ID and the stepper ID.
            /// </summary>
            /// <param name="l_parameterID">the simulation parameter</param>
            /// <param name="l_ID">the stepper ID</param>
            public StepperStub(string l_parameterID, string l_ID)
            {
                this.m_ID = l_ID;
                this.m_parameterID = l_parameterID;
            }

            /// <summary>
            /// Creates the stepper stub.
            /// </summary>
            /// <param name="l_className">the class name</param>
            public void Create(string l_className)
            {
                Debug.Assert(!String.IsNullOrEmpty(CommandManager.s_modelID));
                //
                // Already get
                //
                if (this.m_stepper != null)
                {
                    return;
                }
                //
                // Sets the default parameter ID if the parameter ID is "null".
                //
                if (this.m_parameterID == null)
                {
                    this.m_parameterID = m_cManager.DataManager.GetCurrentSimulationParameterID();
                }
                //
                // Searches the simulation parameter.
                // 
                bool l_existFlag = false;
                foreach (string l_parameterID in m_cManager.GetSimulationParameterIDList())
                {
                    if (this.m_parameterID.Equals(l_parameterID))
                    {
                        l_existFlag = true;
                        break;
                    }
                }
                if (!l_existFlag)
                {
                    m_cManager.DataManager.CreateSimulationParameter(this.m_parameterID);
                }
                //
                // Searches the loaded "Stepper".
                //
                foreach (EcellObject l_stepper
                        in m_cManager.DataManager.GetStepper(this.m_parameterID, CommandManager.s_modelID))
                {
                    if (l_stepper.Key.Equals(this.m_ID) && l_stepper.Classname.Equals(l_className))
                    {
                        this.m_stepper = l_stepper;
                        return;
                    }
                }
                //
                // Creates a new "Stepper".
                //
                //
                // Checks whether the class exists.
                //
                this.Create(this.m_ID, l_className);
                //
                // Adds the "EcellObject" to the "DataManager". 
                //
                m_cManager.DataManager.AddStepperID(this.m_parameterID, this.m_stepper);
            }

            /// <summary>
            /// Creates the stepper stub with the key and the class name.
            /// </summary>
            /// <param name="l_key">the key</param>
            /// <param name="l_className">the class name</param>
            private void Create(string l_key, string l_className)
            {
                List<string> l_entityList = m_cManager.DataManager.GetStepperList();
                if (l_entityList != null && l_entityList.Count > 0)
                {
                    foreach (string l_entity in l_entityList)
                    {
                        if (l_className.Equals(l_entity))
                        {
                            List<EcellData> l_propertyList = new List<EcellData>();
                            foreach (string l_property in m_cManager.DataManager.GetStepperProperty(l_className).Keys)
                            {
                                l_propertyList.Add(m_cManager.DataManager.GetStepperProperty(l_className)[l_property]);
                            }
                            this.m_stepper = EcellObject.CreateObject(
                                    CommandManager.s_modelID,
                                    l_key,
                                    Constants.xpathStepper,
                                    l_className,
                                    l_propertyList);
                            return;
                        }
                    }
                }
            }

            /// <summary>
            /// Delates the stepper stub.
            /// </summary>
            public void Delete()
            {
                m_cManager.DataManager.DeleteStepperID(this.m_parameterID, this.m_stepper);
                this.m_ID = null;
                this.m_parameterID = null;
                this.m_stepper = null;
            }

            /// <summary>
            /// Tests whether the stepper is already loaded.
            /// </summary>
            /// <returns>true if the stepper is loaded; false otherwise</returns>
            public bool Exists()
            {
                foreach (EcellObject l_stepper
                        in m_cManager.DataManager.GetStepper(this.m_parameterID, CommandManager.s_modelID))
                {
                    if (l_stepper.Key.Equals(this.m_ID))
                    {
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// Returns the class name.
            /// </summary>
            /// <returns>the class name</returns>
            public string GetClassName()
            {
                if (this.m_stepper != null)
                {
                    return this.m_stepper.Classname;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Returns the stepper ID.
            /// </summary>
            /// <returns>the stepper ID</returns>
            public string GetName()
            {
                if (this.m_ID != null)
                {
                    return this.m_ID;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Returns the simulation parameter ID the stepper belongs to.
            /// </summary>
            /// <returns>the simulation parameter</returns>
            public string GetSimulationParameterID()
            {
                if (this.m_parameterID != null)
                {
                    return this.m_parameterID;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Returns the value of the property.
            /// </summary>
            /// <param name="l_propertyName">the property</param>
            /// <returns>the value of the property</returns>
            public string GetProperty(string l_propertyName)
            {
                if (this.m_stepper != null)
                {
                    if (this.m_stepper.Value != null && this.m_stepper.Value.Count > 0)
                    {
                        foreach (EcellData l_data in this.m_stepper.Value)
                        {
                            if (l_data.Name.Equals(l_propertyName))
                            {
                                return l_data.Value.ToString();
                            }
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// Returns the attributes of the property.
            /// </summary>
            /// <param name="l_propertyName">the property</param>
            /// <returns>the attributes of the property(Settable, Gettable, Loadable, Savable)</returns>
            public List<bool> GetPropertyAttributes(string l_propertyName)
            {
                if (this.m_stepper != null)
                {
                    if (this.m_stepper.Value != null && this.m_stepper.Value.Count > 0)
                    {
                        foreach (EcellData l_data in this.m_stepper.Value)
                        {
                            if (l_data.Name.Equals(l_propertyName))
                            {
                                List<bool> l_list = new List<bool>();
                                l_list.Add(l_data.Settable);
                                l_list.Add(l_data.Gettable);
                                l_list.Add(l_data.Loadable);
                                l_list.Add(l_data.Saveable);
                                return l_list;
                            }
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// Returns the list of the property.
            /// </summary>
            /// <returns>the list of the property</returns>
            public List<string> GetPropertyList()
            {
                if (this.m_stepper != null)
                {
                    if (this.m_stepper.Value != null && this.m_stepper.Value.Count > 0)
                    {
                        List<string> l_list = new List<string>();
                        foreach (EcellData l_data in this.m_stepper.Value)
                        {
                            l_list.Add(l_data.Name);
                        }
                        l_list.Sort();
                        return l_list;
                    }
                }
                return null;
            }

            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="l_propertyName">the property</param>
            /// <param name="l_value">the value</param>
            public void SetProperty(string l_propertyName, string l_value)
            {
                if (this.m_stepper != null)
                {
                    if (this.m_stepper.Value != null && this.m_stepper.Value.Count > 0)
                    {
                        bool l_findFlag = false;
                        for (int i = 0; i < this.m_stepper.Value.Count; i++)
                        {
                            EcellData l_data = this.m_stepper.Value[i];
                            if (l_data.Name.Equals(l_propertyName))
                            {
                                if (!l_data.Settable)
                                {
                                    throw new Exception(String.Format(MessageResLib.ErrSetProp,
                                        new object[] { l_propertyName }));
                                }
                                else if (l_data.Value.IsDouble())
                                {
                                    l_data.Value = new EcellValue(XmlConvert.ToDouble(l_value));
                                    l_findFlag = true;
                                }
                                else if (l_data.Value.IsInt())
                                {
                                    l_data.Value = new EcellValue(Convert.ToInt32(l_value));
                                    l_findFlag = true;
                                }
                                else if (l_data.Value.IsString())
                                {
                                    l_data.Value = new EcellValue(l_value);
                                    l_findFlag = true;
                                }
                            }
                        }
                        Debug.Assert(l_findFlag);
                        m_cManager.DataManager.DataChanged(
                                this.m_stepper.ModelID,
                                this.m_stepper.Key,
                                this.m_stepper.Type,
                                this.m_stepper,
                                false,
                                false);                    
                    }
                }
            }
        }
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
