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
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell
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
        /// Creates the entity of the full ID.
        /// </summary>
        /// <param name="fullID">the created full ID</param>
        /// <returns>the created entity</returns>
        public EntityStub CreateEntityStub(string fullID)
        {
            s_modelID = m_env.DataManager.CurrentProject.ModelList[0].ModelID;
            return new EntityStub(this, fullID);
        }

        /// <summary>
        /// Creates the logger of the full PN.
        /// </summary>
        /// <param name="fullPN">The logged full PN</param>
        public void CreateLogger(string fullPN)
        {
            // FullID "":"systemID":"processName":"coefficient"
            string[] fullIDs = fullPN.Split(Constants.delimiterColon.ToCharArray());
            if (fullIDs.Length != 4)
            {
                throw new EcellException(MessageResources.ErrInvalidID);
            }
            List<EcellObject> systemObjectList
                    = m_env.DataManager.GetData(s_modelID, fullIDs[1]);

            if (systemObjectList == null || systemObjectList.Count <= 0)
            {
                throw new EcellException(String.Format(MessageResources.ErrFindEnt,
                    new object[] { fullPN }));
            }
            //
            // Searchs the fullID
            //
            string changedKey = null;
            string changedType = null;
            EcellObject changedObject = null;
            foreach (EcellObject systemObject in systemObjectList)
            {
                if (!systemObject.Type.Equals(Constants.xpathSystem))
                {
                    continue;
                }
                if (fullIDs[0].Equals(Constants.xpathSystem))
                {
                    if (systemObject.Key.Equals(fullIDs[2]))
                    {
                        if (systemObject.Value == null || systemObject.Value.Count <= 0)
                        {
                            continue;
                        }
                        foreach (EcellData systemData in systemObject.Value)
                        {
                            if (systemData.Logable && systemData.Name.Equals(fullIDs[3]))
                            {
                                systemData.Logged = true;
                                changedKey = fullIDs[2];
                                changedType = fullIDs[0];
                                changedObject = systemObject;
                                break;
                            }
                        }
                    }
                }
                else if (fullIDs[0].Equals(Constants.xpathProcess) || fullIDs[0].Equals(Constants.xpathVariable))
                {
                    if (systemObject.Children == null || systemObject.Children.Count <= 0)
                    {
                        continue;
                    }
                    foreach (EcellObject childObject in systemObject.Children)
                    {
                        if (childObject.Type.Equals(fullIDs[0])
                                && childObject.Key.Equals(fullIDs[1] + Constants.delimiterColon + fullIDs[2]))
                        {
                            if (childObject.Value == null || childObject.Value.Count <= 0)
                            {
                                continue;
                            }
                            foreach (EcellData childData in childObject.Value)
                            {
                                if (childData.Logable && childData.Name.Equals(fullIDs[3]))
                                {
                                    childData.Logged = true;
                                    changedKey = fullIDs[1]
                                            + Constants.delimiterColon + fullIDs[2];
                                    changedType = fullIDs[0];
                                    changedObject = childObject;
                                    break;
                                }
                            }
                        }
                        if (changedKey != null && changedType != null && changedObject != null)
                        {
                            break;
                        }
                    }
                }
                if (changedKey != null && changedType != null && changedObject != null)
                {
                    break;
                }
            }
            if (changedKey != null && changedType != null && changedObject != null)
            {
                m_env.DataManager.DataChanged(
                        s_modelID, changedKey, changedType, changedObject);
                m_env.PluginManager.LoggerAdd(
                        s_modelID, changedKey, changedType, fullPN);
            }
            else
            {
                throw new EcellException(String.Format(MessageResources.ErrFindEnt,
                    new object[] { fullPN }));
            }
        }

        /// <summary>
        /// Creates the logger policy with some parameters.
        /// </summary>
        /// <param name="savedStepCount">The saved step count</param>
        /// <param name="savedInterval">The saved interval</param>
        /// <param name="diskFullAction">The action if the HDD is full</param>
        /// <param name="maxDiskSpace">The limit of the usable HDD</param>
        public void CreateLoggerPolicy(
                int savedStepCount,
                double savedInterval,
                DiskFullAction diskFullAction,
                int maxDiskSpace)
        {
            LoggerPolicy loggerPolicy
                    = new LoggerPolicy(savedStepCount, savedInterval, diskFullAction, maxDiskSpace);
            m_env.DataManager.SetLoggerPolicy(m_env.DataManager.GetCurrentSimulationParameterID(), loggerPolicy);
        }

        /// <summary>
        /// Creates the logger stub of the full PN.
        /// </summary>
        /// <param name="fullPN">The logged full PN</param>
        /// <returns>the created logger stub</returns>
        public LoggerStub CreateLoggerStub(string fullPN)
        {
            return new LoggerStub(this, fullPN);
        }

        /// <summary>
        /// Creates the model of the model ID.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        public void CreateModel(string modelID)
        {
            EcellObject model = EcellObject.CreateObject(modelID, "", Constants.xpathModel, "", new List<EcellData>());
            m_env.DataManager.DataAdd(model);
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
            s_modelID = modelID;
        }

        /// <summary>
        /// Creates the project.
        /// </summary>
        /// <param name="projectID">The project name</param>
        /// <param name="comment">The comment of the project</param>
        public void CreateProject(string projectID, string comment)
        {
            m_env.DataManager.CreateNewProject(projectID, comment, new List<string>());
        }

        /// <summary>
        /// Creates the simulation parameter stub.
        /// </summary>
        /// <param name="parameterID">the simulation parameter ID</param>
        /// <returns>the simulation parameter stub</returns>
        public SimulationParameterStub CreateSimulationParameterStub(string parameterID)
        {
            return new SimulationParameterStub(this, parameterID);
        }

        /// <summary>
        /// Creates the stepper stub.
        /// </summary>
        /// <param name="ID">the created stepper ID</param>
        /// <returns>the created stepper stub</returns>
        public StepperStub CreateStepperStub(string ID)
        {
            return new StepperStub(this, ID);
        }

        /// <summary>
        /// Creates the stepper stub.
        /// </summary>
        /// <param name="parameterID">the simulation parameter ID</param>
        /// <param name="ID">the created stepper ID</param>
        /// <returns>the created stepper stub</returns>
        public StepperStub CreateStepperStub(string parameterID, string ID)
        {
            return new StepperStub(parameterID, ID);
        }

        /// <summary>
        /// Deletes the default stepper.
        /// </summary>
        public void DeleteDefaultStepperStub()
        {
            CommandManager.StepperStub defaultStepper
                = this.CreateStepperStub("DefaultParameter", "DefaultStepper");
            defaultStepper.Create("FixedODE1Stepper");
            defaultStepper.Delete();
        }

        /// <summary>
        /// Deletes the logger of the full PN.
        /// </summary>
        /// <param name="fullPN">the full PN</param>
        public void DeleteLogger(string fullPN)
        {
            string[] fullPNDivs = fullPN.Split(Constants.delimiterColon.ToCharArray());
            if (fullPNDivs.Length != 4)
            {
                throw new EcellException(MessageResources.ErrInvalidID);
            }
            List<EcellObject> systemObjectList
                    = m_env.DataManager.GetData(s_modelID, fullPNDivs[1]);
            if (systemObjectList == null || systemObjectList.Count <= 0)
            {
                throw new EcellException(String.Format(MessageResources.ErrFindEnt,
                    new object[] { fullPN }));
            }
            //
            // Searchs the fullID
            //
            string changedKey = null;
            string changedType = null;
            EcellObject changedObject = null;
            foreach (EcellObject systemObject in systemObjectList)
            {
                if (!systemObject.Type.Equals(Constants.xpathSystem))
                {
                    continue;
                }
                if (fullPNDivs[0].Equals(Constants.xpathSystem))
                {
                    if (systemObject.Key.Equals(fullPNDivs[2]))
                    {
                        if (systemObject.Value == null || systemObject.Value.Count <= 0)
                        {
                            continue;
                        }
                        foreach (EcellData systemValue in systemObject.Value)
                        {
                            if (systemValue.Logable && systemValue.Name.Equals(fullPNDivs[3]))
                            {
                                systemValue.Logged = false;
                                changedKey = fullPNDivs[2];
                                changedType = fullPNDivs[0];
                                changedObject = systemObject;
                                break;
                            }
                        }
                    }
                }
                else if (fullPNDivs[0].Equals(Constants.xpathProcess)
                        || fullPNDivs[0].Equals(Constants.xpathVariable))
                {
                    if (systemObject.Children == null || systemObject.Children.Count <= 0)
                    {
                        continue;
                    }
                    foreach (EcellObject childObject in systemObject.Children)
                    {
                        if (childObject.Type.Equals(fullPNDivs[0])
                                && childObject.Key.Equals(fullPNDivs[1]
                                + Constants.delimiterColon + fullPNDivs[2]))
                        {
                            if (childObject.Value == null || childObject.Value.Count <= 0)
                            {
                                continue;
                            }
                            foreach (EcellData childValue in childObject.Value)
                            {
                                if (childValue.Logable && childValue.Name.Equals(fullPNDivs[3]))
                                {
                                    childValue.Logged = false;
                                    changedKey = fullPNDivs[1]
                                            + Constants.delimiterColon + fullPNDivs[2];
                                    changedType = fullPNDivs[0];
                                    changedObject = childObject;
                                    break;
                                }
                            }
                        }
                        if (changedKey != null && changedType != null && changedObject != null)
                        {
                            break;
                        }
                    }
                }
                if (changedKey != null && changedType != null && changedObject != null)
                {
                    break;
                }
            }
            if (changedKey != null && changedType != null && changedObject != null)
            {
                m_env.DataManager.DataChanged(s_modelID, changedKey, changedType, changedObject);
            }
            else
            {
                throw new EcellException(String.Format(MessageResources.ErrFindEnt,
                    new object[] { fullPN }));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execFileName"></param>
        /// <param name="argument"></param>
        public void Exec(string execFileName, string argument)
        {
            try
            {
                Process execProcess = new Process();
                execProcess.StartInfo.Arguments = argument;
                execProcess.StartInfo.CreateNoWindow = true;
                execProcess.StartInfo.FileName = execFileName;
                execProcess.StartInfo.UseShellExecute = false;
                execProcess.Start();
                execProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrLoadFile,
                    new object[] { execFileName }), ex);
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
        /// <param name="entityName">The entity name</param>
        /// <param name="systemPath">The system path</param>
        /// <returns>The entity list</returns>
        public List<string> GetEntityList(string entityName, string systemPath)
        {
            if (entityName == null || entityName.Length <= 0
                    || systemPath == null || systemPath.Length <= 0)
            {
                return null;
            }
            if (entityName.Equals(Constants.xpathSystem))
            {
                List<string> list = new List<string>();
                int depth = systemPath.Split(Constants.delimiterPath.ToCharArray()).Length;
                if (systemPath.Equals(Constants.delimiterPath))
                {
                    foreach (string system in m_env.DataManager.GetSystemList(s_modelID))
                    {
                        if (systemPath.Equals(system))
                        {
                            continue;
                        }
                        else if (depth == system.Split(Constants.delimiterPath.ToCharArray()).Length)
                        {
                            list.Add(system.Replace(Constants.delimiterPath, ""));
                        }
                    }
                }
                else
                {
                    foreach (string system in m_env.DataManager.GetSystemList(s_modelID))
                    {
                        if (systemPath.Equals(system))
                        {
                            continue;
                        }
                        else if ((depth + 1) == system.Split(Constants.delimiterPath.ToCharArray()).Length)
                        {
                            list.Add(system.Replace(systemPath, "").Replace(Constants.delimiterPath, ""));
                        }
                    }
                }
                return list;
            }
            else if (entityName.Equals(Constants.xpathProcess) || entityName.Equals(Constants.xpathVariable))
            {
                List<string> list = new List<string>();
                foreach (EcellObject parent in m_env.DataManager.GetData(s_modelID, systemPath))
                {
                    if (parent.Children == null || parent.Children.Count <= 0)
                    {
                        continue;
                    }
                    foreach (EcellObject child in parent.Children)
                    {
                        if (child.Type.Equals(entityName))
                        {
                            list.Add(child.Key.Split(Constants.delimiterColon.ToCharArray())[1]);
                        }
                    }
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the entity property.
        /// </summary>
        /// <param name="fullPN">The full PN</param>
        /// <returns>The entity property of the full PN</returns>
        public EcellValue GetEntityProperty(string fullPN)
        {
            if (this.GetCurrentSimulationTime() <= 0.0)
            {
                return this.GetEntityPropertyFromDic(fullPN);
            }
            else
            {
                EcellValue v = this.GetEntityPropertyFromSimulator(fullPN);
                return v;
            }
        }

        /// <summary>
        /// Returns the entity property of the dic.
        /// </summary>
        /// <param name="fullPN">The full PN</param>
        /// <returns>The entity property of the full PN</returns>
        private EcellValue GetEntityPropertyFromDic(string fullPN)
        {
            if (fullPN.IndexOf(Constants.delimiterColon) < 0)
            {
                return null;
            }
            string[] pathElements = fullPN.Split(Constants.delimiterColon.ToCharArray());
            if (pathElements[0].Equals(Constants.xpathSystem))
            {
                EcellObject system
                    = (m_env.DataManager.GetData(
                        s_modelID, pathElements[1] + Constants.delimiterColon + pathElements[2]))[0];
                foreach (EcellData systemProperty in system.Value)
                {
                    if (systemProperty.Name.Equals(pathElements[3]))
                    {
                        return systemProperty.Value;
                    }
                }
            }
            else
            {
                EcellObject system
                    = (m_env.DataManager.GetData(
                        s_modelID, pathElements[1]))[0];
                foreach (EcellObject entity in system.Children)
                {
                    if (entity.Type.Equals(pathElements[0])
                        && entity.Key.Equals(pathElements[1] + Constants.delimiterColon + pathElements[2]))
                    {
                        foreach (EcellData entityProperty in entity.Value)
                        {
                            if (entityProperty.Name.Equals(pathElements[3]))
                            {
                                return entityProperty.Value;
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
        /// <param name="fullPN">The full PN</param>
        /// <returns>The entity property of the full PN</returns>
        private EcellValue GetEntityPropertyFromSimulator(string fullPN)
        {
            return m_env.DataManager.GetEntityProperty(fullPN);
        }

        /// <summary>
        /// Returns the logger list.
        /// </summary>
        /// <returns>The logger list</returns>
        public List<string> GetLoggerList()
        {
            List<string> list = new List<string>();
            foreach (string systemPath in m_env.DataManager.GetSystemList(s_modelID))
            {
                foreach (EcellObject system in m_env.DataManager.GetData(s_modelID, systemPath))
                {
                    if (system.Value != null && system.Value.Count > 0)
                    {
                        foreach (EcellData data in system.Value)
                        {
                            if (data.Logged)
                            {
                                list.Add(data.EntityPath);
                            }
                        }
                    }
                    if (system.Children != null && system.Children.Count > 0)
                    {
                        foreach (EcellObject entity in system.Children)
                        {
                            if (entity.Value != null && entity.Value.Count > 0)
                            {
                                foreach (EcellData data in entity.Value)
                                {
                                    if (data.Logged)
                                    {
                                        list.Add(data.EntityPath);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return list;
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
        /// <param name="parameterID">The parameter ID</param>
        /// <returns>The stepper list</returns>
        public List<string> GetStepperList(string parameterID)
        {
            List<string> list = new List<string>();
            foreach (EcellObject stepper in m_env.DataManager.GetStepper(parameterID, s_modelID))
            {
                list.Add(stepper.Key);
            }
            return list;
        }

        /// <summary>
        /// Returns the selected log data.
        /// </summary>
        /// <param name="startTime">The start time of the logger</param>
        /// <param name="endTime">The end time of the logger</param>
        /// <param name="fullPN">The logged full PN</param>
        /// <returns></returns>
        public List<LogValue> GetLogData(string fullPN, double startTime, double endTime)
        {
            double interval
                    = m_env.DataManager
                            .GetLoggerPolicy(m_env.DataManager.GetCurrentSimulationParameterID())
                            .ReloadInterval;
            return m_env.DataManager
                    .GetLogData(startTime, endTime, interval, fullPN).logValueList;
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
        public IList GetNextEvent()
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
            return m_env.DataManager.IsActive;
        }

        /// <summary>
        /// Activates the "IronPythonConsole.exe".
        /// </summary>
        public void Interact()
        {
            Process process = new Process();
            process.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\" + s_consoleExe;
            process.Start();
        }

        /// <summary>
        /// Loads the model.
        /// </summary>
        /// <param name="fileName">The "EML" file name</param>
        public void LoadModel(string fileName)
        {
            m_env.DataManager.LoadProject(fileName);
            s_modelID = m_env.DataManager.CurrentProjectID;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            m_env.DataManager.CloseProject();            
        }

        /// <summary>
        /// Runs the simulator.
        /// </summary>
        /// <param name="interval">The time limit of the simulator</param>
        public void Run(double interval)
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Running);
//            m_env.DataManager.SimulationStartKeepSetting(interval);
            m_env.DataManager.StartStepSimulation(interval);
        }

        /// <summary>
        /// Runs the simulator.
        /// </summary>
        /// <param name="interval">The time limit of the simulator</param>
        public void RunNotSuspend(double interval)
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Running);
//            m_env.DataManager.SimulationStart(interval, 0);
            m_env.DataManager.StartSimulation(interval);
        }

        /// <summary>
        /// Saves the selected log data.
        /// </summary>
        /// <param name="savedDirName">The saved directory name</param>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="fullID">The logged full ID</param>
        public void SaveLoggerData(string fullID, string savedDirName, double startTime, double endTime)
        {
            List<string> fullIDList = new List<string>();
            fullIDList.Add(fullID);
            m_env.DataManager
                    .SaveSimulationResult(savedDirName, startTime, endTime, Constants.xpathCsv, fullIDList);
        }

        /// <summary>
        /// Saves the model.
        /// </summary>
        /// <param name="modelID">The saved model ID</param>
        public void SaveModel(string modelID)
        {
            m_env.DataManager.SaveModel(modelID);
        }

        /// <summary>
        /// Sets the entity property.
        /// </summary>
        /// <param name="fullPN">The full PN</param>
        /// <param name="value">The property value</param>
        public void SetEntityProperty(string fullPN, string value)
        {
            m_env.DataManager.SetEntityProperty(fullPN, value);
        }

        /// <summary>
        /// Steps the simulator.
        /// </summary>
        /// <param name="count">The step limit of the simulator</param>
        public void Step(int count)
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Running);           
//            m_env.DataManager.SimulationStartKeepSetting(count);
            m_env.DataManager.StartStepSimulation(count);
        }

        /// <summary>
        /// Steps the simulator.
        /// </summary>
        /// <param name="count">The step limit of the simulator</param>
        public void StepNotSuspend(int count)
        {
            m_env.PluginManager.ChangeStatus(ProjectStatus.Running);
//            m_env.DataManager.SimulationStart(count, 0);
            m_env.DataManager.StartStepSimulation(count);
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

        public void UpdateInitialCondition(Dictionary<string, double> initialDic)
        {
            m_env.DataManager.UpdateInitialCondition(null, s_modelID, initialDic);
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
            /// <param name="fullID">the full ID</param>
            public EntityStub(CommandManager cManager, string fullID)
            {
                this.m_cManager = cManager;
                this.m_fullID = fullID;
            }

            /// <summary>
            /// Creates the entity stub.
            /// </summary>
            /// <param name="className">the class name</param>
            public void Create(string className)
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
                string key = null;
                string type = null;
                string systemKey = null;
                this.RefinedFullID(ref key, ref type, ref systemKey);
                //
                // Searches the loaded "EcellObject".
                //
                foreach (EcellObject system
                        in m_cManager.DataManager.GetData(
                            CommandManager.s_modelID, systemKey))
                {
                    if (type.Equals(Constants.xpathSystem))
                    {
                        this.m_ecellObject = system;
                        return;
                    }
                    else
                    {
                        if (system.Children != null && system.Children.Count > 0)
                        {
                            foreach (EcellObject entity in system.Children)
                            {
                                if (entity.Type.Equals(type) && entity.Key.Equals(key))
                                {
                                    this.m_ecellObject = entity;
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
                this.Create(key, type, className);
                //
                // Adds the "EcellObject" to the "DataManager". 
                //
                m_cManager.DataManager.DataAdd(m_ecellObject);
            }

            /// <summary>
            /// Creates the entity stub with some parameters.
            /// </summary>
            /// <param name="key">the "System" path of the entity.</param>
            /// <param name="type">the type of entity</param>
            /// <param name="className">the class name</param>
            private void Create(string key, string type, string className)
            {
                List<string> entityList = null;
                if (type.Equals(Constants.xpathSystem))
                {
                    entityList = m_cManager.DataManager.CurrentProject.SystemDmList;
                }
                else if (type.Equals(Constants.xpathProcess))
                {
                    entityList = m_cManager.DataManager.CurrentProject.ProcessDmList;
                }
                else if (type.Equals(Constants.xpathVariable))
                {
                    entityList = m_cManager.DataManager.CurrentProject.VariableDmList;
                }

                if (entityList != null && entityList.Count > 0)
                {
                    foreach (string entity in entityList)
                    {
                        if (className.Equals(entity))
                        {
                            List<EcellData> propertyList = new List<EcellData>();
                            if (type.Equals(Constants.xpathSystem))
                            {
                                Dictionary<string, EcellData> propertyDic
                                        = m_cManager.DataManager.GetSystemProperty();
                                foreach (string property in m_cManager.DataManager.GetSystemProperty().Keys)
                                {
                                    EcellData ecellData = propertyDic[property];
                                    ecellData.EntityPath
                                            = type + Constants.delimiterColon
                                            + key + Constants.delimiterColon + property;
                                    propertyList.Add(ecellData);
                                }
                            }
                            else if (type.Equals(Constants.xpathProcess))
                            {
                                Dictionary<string, EcellData> propertyDic
                                        = m_cManager.DataManager.GetProcessProperty(className);
                                foreach (string property in propertyDic.Keys)
                                {
                                    EcellData ecellData = propertyDic[property];
                                    ecellData.EntityPath
                                            = type + Constants.delimiterColon
                                            + key + Constants.delimiterColon + property;
                                    propertyList.Add(ecellData);
                                }
                            }
                            else
                            {
                                Dictionary<string, EcellData> propertyDic
                                        = m_cManager.DataManager.GetVariableProperty();
                                foreach (string property in m_cManager.DataManager.GetVariableProperty().Keys)
                                {
                                    EcellData ecellData = propertyDic[property];
                                    ecellData.EntityPath
                                            = type + Constants.delimiterColon
                                            + key + Constants.delimiterColon + property;
                                    propertyList.Add(ecellData);
                                }
                            }
                            this.m_ecellObject = EcellObject.CreateObject(
                                    CommandManager.s_modelID, key, type, className, propertyList);
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
                string key = null;
                string type = null;
                string systemKey = null;
                this.RefinedFullID(ref key, ref type, ref systemKey);
                m_cManager.DataManager.DataDelete(CommandManager.s_modelID, key, type);
                this.m_ecellObject = null;
                this.m_fullID = null;
            }

            /// <summary>
            /// Tests whether the entity is already loaded.
            /// </summary>
            /// <returns>true if the entity is loaded; false otherwise</returns>
            public bool Exists()
            {
                string key = null;
                string type = null;
                Util.ParseFullID(m_fullID, out type, out key);
                return m_cManager.DataManager.IsDataExists(CommandManager.s_modelID, key, type);
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
            /// <param name="propertyName">the property name</param>
            /// <returns>the value of the property</returns>
            public string GetProperty(string propertyName)
            {
                if (this.m_ecellObject != null)
                {
                    if (this.m_ecellObject.Value != null && this.m_ecellObject.Value.Count > 0)
                    {
                        foreach (EcellData data in this.m_ecellObject.Value)
                        {
                            if (data.Name.Equals(propertyName))
                            {
                                return data.Value.ToString();
                            }
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// Returns the attributes of the property.
            /// </summary>
            /// <param name="propertyName">the property name</param>
            /// <returns>the attributes of the property(Settable, Gettable, Loadable, Savable)</returns>
            public List<bool> GetPropertyAttributes(string propertyName)
            {
                if (this.m_ecellObject != null)
                {
                    if (this.m_ecellObject.Value != null && this.m_ecellObject.Value.Count > 0)
                    {
                        foreach (EcellData data in this.m_ecellObject.Value)
                        {
                            if (data.Name.Equals(propertyName))
                            {
                                List<bool> list = new List<bool>();
                                list.Add(data.Settable);
                                list.Add(data.Gettable);
                                list.Add(data.Loadable);
                                list.Add(data.Saveable);
                                return list;
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
                        List<string> list = new List<string>();
                        foreach (EcellData data in this.m_ecellObject.Value)
                        {
                            list.Add(data.Name);
                        }
                        list.Sort();
                        return list;
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
                string key = null;
                string type = null;
                string systemKey = null;
                this.RefinedFullID(ref key, ref type, ref systemKey);
                //
                // Searches the loaded "EcellObject".
                //
                foreach (EcellObject system
                        in m_cManager.DataManager.GetData(CommandManager.s_modelID, systemKey))
                {
                    if (type.Equals(Constants.xpathSystem))
                    {
                        this.m_ecellObject = system;
                        return;
                    }
                    else
                    {
                        if (system.Children != null && system.Children.Count > 0)
                        {
                            foreach (EcellObject entity in system.Children)
                            {
                                if (entity.Type.Equals(type) && entity.Key.Equals(key))
                                {
                                    this.m_ecellObject = entity;
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
            /// <param name="key">the system path of the entity</param>
            /// <param name="type">the type of the entity</param>
            /// <param name="systemKey">the system path of the entity</param>
            private void RefinedFullID(ref string key, ref string type, ref string systemKey)
            {
                string[] infos = this.m_fullID.Split(Constants.delimiterColon.ToCharArray());

                key = null;
                type = infos[0];
                systemKey = null;
                if (infos[1].Equals("") && infos[2].Equals(Constants.delimiterPath))
                {
                    key = Constants.delimiterPath;
                    systemKey = Constants.delimiterPath;
                }
                else
                {
                    if (infos[0].Equals(Constants.xpathSystem))
                    {
                        key = infos[1] + Constants.delimiterPath + infos[2];
                        systemKey = infos[1] + Constants.delimiterPath + infos[2];
                    }
                    else
                    {
                        key = infos[1] + Constants.delimiterColon + infos[2];
                        systemKey = infos[1];
                    }
                    key = key.Replace(Constants.delimiterPath + Constants.delimiterPath, Constants.delimiterPath);
                    systemKey = systemKey.Replace(Constants.delimiterPath + Constants.delimiterPath, Constants.delimiterPath);
                }
            }

            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="fullPN">the property</param>
            /// <param name="value">the value</param>
            public void SetProperty(string fullPN, string value)
            {
                //
                // Get a current EcellObject.
                //
                this.RefinedEcellObject();
                string[] ele = fullPN.Split(new char[] { ':' });
                string propertyName = ele[ele.Length - 1];
                //
                // Set.
                //
                if (this.m_ecellObject == null)
                    return;
                if (this.m_ecellObject.Value == null || this.m_ecellObject.Value.Count <= 0)
                    return;

                EcellData data = m_ecellObject.GetEcellData(propertyName);
                // Add new parameter.
                if (data == null)
                {
                    EcellData newData
                        = new EcellData(
                            propertyName,
                            new EcellValue(Convert.ToDouble(value)),
                            fullPN);
                    newData.Logable = true;
                    this.m_ecellObject.Value.Add(newData);
                }
                // Update current parameter.
                else if (data.EntityPath.Equals(fullPN))
                {
                    if (!data.Settable)
                    {
                        throw new EcellException(String.Format(MessageResources.ErrSetProp,
                            new object[] { fullPN }));
                    }
                    else if (data.Name.Equals(Constants.xpathVRL))
                    {
                        List<EcellReference> list = EcellReference.ConvertFromString(value);
                        string path = this.m_fullID.Split(Constants.delimiterColon.ToCharArray())[1];
                        // Normalize
                        foreach (EcellReference er in list)
                        {
                            Util.NormalizeVariableReference(er, path);
                        }
                        data.Value = EcellReference.ConvertToEcellValue(list);
                    }
                    else if (data.Value.IsDouble)
                    {
                        data.Value = new EcellValue(XmlConvert.ToDouble(value));
                    }
                    else if (data.Value.IsInt)
                    {
                        data.Value = new EcellValue(Convert.ToInt32(value));
                    }
                    else if (data.Value.IsString)
                    {
                        data.Value = new EcellValue(value);
                    }
                }
                m_cManager.DataManager.DataChanged(
                        this.m_ecellObject.ModelID,
                        this.m_ecellObject.Key,
                        this.m_ecellObject.Type,
                            this.m_ecellObject);
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
            /// <param name="fullPN">the full PN</param>
            public LoggerStub(CommandManager cManager, string fullPN)
            {
                this.m_cManager = cManager;
                this.m_fullPN = fullPN;
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
            public List<LogValue> GetData(double startTime, double endTime)
            {
                return m_cManager.GetLogData(this.m_fullPN, startTime, endTime);
            }

            /// <summary>
            /// Returns the end time of the logger.
            /// </summary>
            /// <returns>the end time of the logger</returns>
            public double GetEndTime()
            {
                List<LogValue> logDataList =
                        m_cManager.GetLogData(
                                this.m_fullPN,
                                0.0,
                                m_cManager.GetCurrentSimulationTime());
                return logDataList[logDataList.Count - 1].time;
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
                List<LogValue> logDataList =
                        m_cManager.GetLogData(
                            this.m_fullPN,
                            0.0,
                            m_cManager.GetCurrentSimulationTime());
                return logDataList.Count;
            }

            /// <summary>
            /// Returns the start time of the logger.
            /// </summary>
            /// <returns>the start time of the logger</returns>
            public double GetStartTime()
            {
                List<LogValue> logDataList =
                    m_cManager.GetLogData(
                        this.m_fullPN, 0.0,
                        m_cManager.GetCurrentSimulationTime());
                return logDataList[0].time;
            }

            /// <summary>
            /// Sets the logger policy of the logger.
            /// </summary>
            /// <param name="savedStepCount">the saved step count</param>
            /// <param name="savedInterval">the saved interval</param>
            /// <param name="diskFullAction">the HDD full action</param>
            /// <param name="maxDiskSpace">the max HDD space</param>
            public void SetLoggerPolicy(
                    int savedStepCount,
                    double savedInterval,
                    int diskFullAction,
                    int maxDiskSpace)
            {
                m_cManager.CreateLoggerPolicy(
                    savedStepCount, savedInterval, (diskFullAction == 0 ? DiskFullAction.Terminate : DiskFullAction.Overwrite), maxDiskSpace);
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
            private Dictionary<string, double> m_initialCondition = null;

            /// <summary>
            /// Creates the simulation parameter stub with the simulation parameter ID.
            /// </summary>
            /// <param name="cManager">the CommandManager associated this object.</param>
            /// <param name="parameterID">the simulation parameter ID</param>
            public SimulationParameterStub(CommandManager cManager, string parameterID)
            {
                this.m_cManager = cManager;
                this.m_parameterID = parameterID;
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
                bool existFlag = false;
                foreach (string parameterID in m_cManager.GetSimulationParameterIDList())
                {
                    if (this.m_parameterID.Equals(parameterID))
                    {
                        existFlag = true;
                        break;
                    }
                }
                if (!existFlag)
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
            /// <param name="stepperID">the stepper ID</param>
            /// <returns>the created stepper stub</returns>
            public StepperStub CreateStepperStub(string stepperID)
            {
                return new StepperStub(this.m_parameterID, stepperID);
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
                foreach (string parameterID in m_cManager.DataManager.GetSimulationParameterIDs())
                {
                    if (parameterID.Equals(this.m_parameterID))
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

            public Dictionary<string, double> GetInitialCondition()
            {
                return this.m_initialCondition;
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
                List<string> stepperIDList = new List<string>();
                foreach (EcellObject ecellObject in this.m_stepperList)
                {
                    stepperIDList.Add(ecellObject.Key);
                }

                return stepperIDList;
            }

            /// <summary>
            /// Sets the logger policy of the logger.
            /// </summary>
            /// <param name="savedStepCount">the saved step count</param>
            /// <param name="savedInterval">the saved interval</param>
            /// <param name="diskFullAction">the HDD full action</param>
            /// <param name="maxDiskSpace">the max HDD space</param>
            public void SetLoggerPolicy(
                    int savedStepCount,
                    double savedInterval,
                    DiskFullAction diskFullAction,
                    int maxDiskSpace)
            {
                LoggerPolicy loggerPolicy
                        = new LoggerPolicy(savedStepCount, savedInterval, diskFullAction, maxDiskSpace);
                m_cManager.DataManager.SetLoggerPolicy(this.m_parameterID, loggerPolicy);
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
            public StepperStub(CommandManager cManager, string ID)
            {
                this.m_cManager = cManager;
                this.m_ID = ID;
            }

            /// <summary>
            /// Creates the stepper stub with the simulation parameter ID and the stepper ID.
            /// </summary>
            /// <param name="parameterID">the simulation parameter</param>
            /// <param name="ID">the stepper ID</param>
            public StepperStub(string parameterID, string ID)
            {
                this.m_ID = ID;
                this.m_parameterID = parameterID;
            }

            /// <summary>
            /// Creates the stepper stub.
            /// </summary>
            /// <param name="className">the class name</param>
            public void Create(string className)
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
                bool existFlag = false;
                foreach (string parameterID in m_cManager.GetSimulationParameterIDList())
                {
                    if (this.m_parameterID.Equals(parameterID))
                    {
                        existFlag = true;
                        break;
                    }
                }
                if (!existFlag)
                {
                    m_cManager.DataManager.CreateSimulationParameter(this.m_parameterID);
                }
                //
                // Searches the loaded "Stepper".
                //
                foreach (EcellObject stepper
                        in m_cManager.DataManager.GetStepper(this.m_parameterID, CommandManager.s_modelID))
                {
                    if (stepper.Key.Equals(this.m_ID) && stepper.Classname.Equals(className))
                    {
                        this.m_stepper = stepper;
                        return;
                    }
                }
                //
                // Creates a new "Stepper".
                //
                //
                // Checks whether the class exists.
                //
                this.Create(this.m_ID, className);
                //
                // Adds the "EcellObject" to the "DataManager". 
                //
                m_cManager.DataManager.AddStepperID(this.m_parameterID, this.m_stepper);
            }

            /// <summary>
            /// Creates the stepper stub with the key and the class name.
            /// </summary>
            /// <param name="key">the key</param>
            /// <param name="className">the class name</param>
            private void Create(string key, string className)
            {
                List<string> entityList = m_cManager.DataManager.CurrentProject.StepperDmList;
                if (entityList != null && entityList.Count > 0)
                {
                    foreach (string entity in entityList)
                    {
                        if (className.Equals(entity))
                        {
                            List<EcellData> propertyList = m_cManager.DataManager.GetStepperProperty(className);
                            this.m_stepper = EcellObject.CreateObject(
                                    CommandManager.s_modelID,
                                    key,
                                    Constants.xpathStepper,
                                    className,
                                    propertyList);
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
                foreach (EcellObject stepper
                        in m_cManager.DataManager.GetStepper(this.m_parameterID, CommandManager.s_modelID))
                {
                    if (stepper.Key.Equals(this.m_ID))
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
            /// <param name="propertyName">the property</param>
            /// <returns>the value of the property</returns>
            public string GetProperty(string propertyName)
            {
                if (this.m_stepper != null)
                {
                    if (this.m_stepper.Value != null && this.m_stepper.Value.Count > 0)
                    {
                        foreach (EcellData data in this.m_stepper.Value)
                        {
                            if (data.Name.Equals(propertyName))
                            {
                                return data.Value.ToString();
                            }
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// Returns the attributes of the property.
            /// </summary>
            /// <param name="propertyName">the property</param>
            /// <returns>the attributes of the property(Settable, Gettable, Loadable, Savable)</returns>
            public List<bool> GetPropertyAttributes(string propertyName)
            {
                if (this.m_stepper != null)
                {
                    if (this.m_stepper.Value != null && this.m_stepper.Value.Count > 0)
                    {
                        foreach (EcellData data in this.m_stepper.Value)
                        {
                            if (data.Name.Equals(propertyName))
                            {
                                List<bool> list = new List<bool>();
                                list.Add(data.Settable);
                                list.Add(data.Gettable);
                                list.Add(data.Loadable);
                                list.Add(data.Saveable);
                                return list;
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
                        List<string> list = new List<string>();
                        foreach (EcellData data in this.m_stepper.Value)
                        {
                            list.Add(data.Name);
                        }
                        list.Sort();
                        return list;
                    }
                }
                return null;
            }

            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="propertyName">the property</param>
            /// <param name="value">the value</param>
            public void SetProperty(string propertyName, string value)
            {
                if (this.m_stepper != null)
                {
                    if (this.m_stepper.Value != null && this.m_stepper.Value.Count > 0)
                    {
                        bool findFlag = false;
                        for (int i = 0; i < this.m_stepper.Value.Count; i++)
                        {
                            EcellData data = this.m_stepper.Value[i];
                            if (data.EntityPath.Equals(propertyName))
                            {
                                if (!data.Settable)
                                {
                                    throw new EcellException(String.Format(MessageResources.ErrSetProp,
                                        new object[] { propertyName }));
                                }
                                else if (data.Value.IsDouble)
                                {
                                    data.Value = new EcellValue(XmlConvert.ToDouble(value));
                                    findFlag = true;
                                }
                                else if (data.Value.IsInt)
                                {
                                    data.Value = new EcellValue(Convert.ToInt32(value));
                                    findFlag = true;
                                }
                                else if (data.Value.IsString)
                                {
                                    data.Value = new EcellValue(value);
                                    findFlag = true;
                                }
                            }
                        }
                        Debug.Assert(findFlag);
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
