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

        private DataManager m_dManager;

        private PluginManager m_pManager;

        private DataManager DataManager
        {
            get { return m_dManager; }
        }

        /// <summary>
        /// Creates the new "CommandManager" instance with no argument.
        /// </summary>
        private CommandManager(DataManager dManager, PluginManager pManager)
        {
            m_dManager = dManager;
            m_pManager = pManager;
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
            try
            {
                string[] l_fullIDs = l_fullPN.Split(Constants.delimiterColon.ToCharArray());
                if (l_fullIDs.Length != 4)
                {
                    throw new Exception("The format of this [" + l_fullPN + "] is wrong.");
                }
                List<EcellObject> l_systemObjectList
                        = m_dManager.GetData(s_modelID, l_fullIDs[1]);
                if (l_systemObjectList == null || l_systemObjectList.Count <= 0)
                {
                    throw new Exception("The entity of this ["
                            + l_fullIDs[0] + Constants.delimiterColon
                            + l_fullIDs[1] + Constants.delimiterColon
                            + l_fullIDs[2] + "] is nothing.");
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
                    m_dManager.DataChanged(
                            s_modelID, l_changedKey, l_changedType, l_changedObject);
                    m_pManager.LoggerAdd(
                            s_modelID, l_changedType, l_changedKey, l_fullPN);
                }
                else
                {
                    throw new Exception("The property of this [" + l_fullIDs[3] + "] is nothing.");
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't create the logger of this [" + l_fullPN + "]. {" + l_ex.ToString() + "}");
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
            try
            {
                LoggerPolicy l_loggerPolicy
                        = new LoggerPolicy(l_savedStepCount, l_savedInterval, l_diskFullAction, l_maxDiskSpace);
                m_dManager.SetLoggerPolicy(m_dManager.GetCurrentSimulationParameterID(),
                        ref l_loggerPolicy);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't create the logger policy. {" + l_ex.ToString() + "}");
            }
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
            try
            {
                List<EcellObject> l_list = new List<EcellObject>();
                l_list.Add(EcellObject.CreateObject(l_modelID, null, Constants.xpathModel, null, null));
                m_dManager.DataAdd(l_list);
                m_pManager.ChangeStatus(ProjectStatus.Loaded);
                s_modelID = l_modelID;
//                m_cManager.DataManager.CurrentProject.Initialize(l_modelID);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't create the model of this [" + l_modelID + "]. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Creates the project.
        /// </summary>
        /// <param name="l_projectID">The project name</param>
        /// <param name="l_comment">The comment of the project</param>
        public void CreateProject(string l_projectID, string l_comment)
        {
            try
            {
                m_dManager.CreateProject(l_projectID, l_comment, null, new List<string>());
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                        "Can't create the project of this [" + l_projectID + "]. {" + l_ex.ToString() + "}");
            }
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
            try
            {
                CommandManager.StepperStub l_defaultStepper
                    = this.CreateStepperStub("DefaultParameter", "DefaultStepper");
                l_defaultStepper.Create("FixedODE1Stepper");
                l_defaultStepper.Delete();
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't delete the stepper named [DefaultStepper]. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Deletes the logger of the full PN.
        /// </summary>
        /// <param name="l_fullPN">the full PN</param>
        public void DeleteLogger(string l_fullPN)
        {
            try
            {
                string[] l_fullPNDivs = l_fullPN.Split(Constants.delimiterColon.ToCharArray());
                if (l_fullPNDivs.Length != 4)
                {
                    throw new Exception("The format of this [" + l_fullPN + "] is wrong.");
                }
                List<EcellObject> l_systemObjectList
                        = m_dManager.GetData(s_modelID, l_fullPNDivs[1]);
                if (l_systemObjectList == null || l_systemObjectList.Count <= 0)
                {
                    throw new Exception("The entity of this ["
                            + l_fullPNDivs[0] + Constants.delimiterColon
                            + l_fullPNDivs[1] + Constants.delimiterColon
                            + l_fullPNDivs[2] + "] is nothing.");
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
                    m_dManager.DataChanged(s_modelID, l_changedKey, l_changedType, l_changedObject);
                }
                else
                {
                    throw new Exception("The property of this [" + l_fullPNDivs[3] + "] is nothing.");
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't create the logger of this [" + l_fullPN + "]. {" + l_ex.ToString() + "}");
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
                throw new Exception("Can't execute the [" + l_execFileName + "] file. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the current project ID.
        /// </summary>
        /// <returns>the current project ID</returns>
        public string GetCurrentProjectID()
        {
            return m_dManager.CurrentProjectID;
        }

        /// <summary>
        /// Returns the current simulation parameter.
        /// </summary>
        /// <returns>the current simulaton parameter</returns>
        public string GetCurrentSimulationParameterID()
        {
            try
            {
                return m_dManager.GetCurrentSimulationParameterID();
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the current simulation parameter. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the current time of the simulator.
        /// </summary>
        /// <returns>The current time of the simulator</returns>
        public double GetCurrentSimulationTime()
        {
            try
            {
                return m_dManager.GetCurrentSimulationTime();
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the current simulation time. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the entity list.
        /// </summary>
        /// <param name="l_entityName">The entity name</param>
        /// <param name="l_systemPath">The system path</param>
        /// <returns>The entity list</returns>
        public List<string> GetEntityList(string l_entityName, string l_systemPath)
        {
            try
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
                        foreach (string l_system in m_dManager.GetSystemList(s_modelID))
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
                        foreach (string l_system in m_dManager.GetSystemList(s_modelID))
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
                else if(l_entityName.Equals(Constants.xpathProcess) || l_entityName.Equals(Constants.xpathVariable))
                {
                    List<string> l_list = new List<string>();
                    foreach (EcellObject l_parent in m_dManager.GetData(s_modelID, l_systemPath))
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
            catch (Exception l_ex)
            {
                throw new Exception(
                        "Can't obtain the entity list of this [" + l_entityName + "][" + l_systemPath+ "] . {" 
                        + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the entity property.
        /// </summary>
        /// <param name="l_fullPN">The full PN</param>
        /// <returns>The entity property of the full PN</returns>
        public EcellValue GetEntityProperty(string l_fullPN)
        {
            try
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
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the [" + l_fullPN + "] property. {" + l_ex.ToString() + "}");
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
                    = (m_dManager.GetData(
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
                    = (m_dManager.GetData(
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
            return m_dManager.GetEntityProperty(l_fullPN);
        }

        /// <summary>
        /// Returns the logger list.
        /// </summary>
        /// <returns>The logger list</returns>
        public List<string> GetLoggerList()
        {
            try
            {
                List<string> l_list = new List<string>();
                foreach (string l_systemPath in m_dManager.GetSystemList(s_modelID))
                {
                    foreach (EcellObject l_system in m_dManager.GetData(s_modelID, l_systemPath))
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
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the logger list. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the process list.
        /// </summary>
        /// <returns>The process list</returns>
        public List<string> GetProcessList()
        {
            return m_dManager.GetEntityList(s_modelID, Constants.xpathProcess);
        }

        /// <summary>
        /// Returns the list of the simulation parameter ID.
        /// </summary>
        /// <returns>the list of the simulation parameter ID</returns>
        public List<string> GetSimulationParameterIDList()
        {
            try
            {
                return m_dManager.GetSimulationParameterIDs();
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the simulation parameter. {" + l_ex.ToString() + "}");
            }
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
            try
            {
                List<string> l_list = new List<string>();
                foreach (EcellObject l_stepper in m_dManager.GetStepper(l_parameterID, s_modelID))
                {
                    l_list.Add(l_stepper.Key);
                }
                return l_list;
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the stepper list. {" + l_ex.ToString() + "}");
            }
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
            try
            {
                double l_interval
                        = m_dManager
                                .GetLoggerPolicy(m_dManager.GetCurrentSimulationParameterID())
                                .m_reloadInterval;
                return m_dManager
                        .GetLogData(l_startTime, l_endTime, l_interval, l_fullPN).logValueList;
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the log of this [" + l_fullPN + "]. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the logger policy.
        /// </summary>
        /// <returns>the logger policy</returns>
        public LoggerPolicy GetLoggerPolicy()
        {
            try
            {
                return m_dManager.GetLoggerPolicy(
                        m_dManager.GetCurrentSimulationParameterID());
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the logger policy. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the next event.
        /// </summary>
        /// <returns>The next event</returns>
        public ArrayList GetNextEvent()
        {
            try
            {
                return m_dManager.GetNextEvent();
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the next event. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the variable list.
        /// </summary>
        /// <returns>The variable list</returns>
        public List<string> GetVariableList()
        {
            return m_dManager.GetEntityList(s_modelID, Constants.xpathVariable);
        }

        /// <summary>
        /// Initializes the simulation.
        /// </summary>
        public void Initialize()
        {
            try
            {
                m_dManager.Initialize(true);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't initialize the simulator. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Checks whether the simulator is running.
        /// </summary>
        /// <returns>true if the simulator is running; false otherwise</returns>
        public bool IsActive()
        {
            return m_dManager.IsActive();
        }

        /// <summary>
        /// Activates the "IronPythonConsole.exe".
        /// </summary>
        public void Interact()
        {
            try
            {
                Process l_process = new Process();
                l_process.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\" + s_consoleExe;
                l_process.Start();
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't open the \"IronPython\" console. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Loads the model.
        /// </summary>
        /// <param name="l_fileName">The "EML" file name</param>
        public void LoadModel(string l_fileName)
        {
            try
            {
                if (m_dManager.CurrentProjectID == null)
                {
                    String modelDir = Path.GetDirectoryName(l_fileName);
                    if (modelDir.EndsWith(Constants.xpathModel))
                    {
                        modelDir = modelDir.Substring(0, modelDir.Length - 5);
                    }
                    m_dManager.CreateProject(Constants.defaultPrjID, DateTime.Now.ToString(), modelDir, new List<string>());
                }
                s_modelID = m_dManager.LoadModel(l_fileName, false);
                m_pManager.LoadData(s_modelID);                
                m_pManager.ChangeStatus(ProjectStatus.Loaded);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't load the model of this [" + l_fileName + "]. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Sends to the "Message" window the message.
        /// </summary>
        /// <param name="l_message">The message</param>
        public void Message(string l_message)
        {
            try
            {
                m_pManager.Message(Constants.messageSimulation, l_message);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't create the message of this [" + l_message + "]. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            m_dManager.CloseProject(null);
        }
        
        /// <summary>
        /// Runs the simulator.
        /// </summary>
        /// <param name="l_interval">The time limit of the simulator</param>
        public void Run(double l_interval)
        {
            try
            {
                m_pManager.ChangeStatus(ProjectStatus.Running);
                m_dManager.SimulationStartKeepSetting(l_interval);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't run the simulator. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Runs the simulator.
        /// </summary>
        /// <param name="l_interval">The time limit of the simulator</param>
        public void RunNotSuspend(double l_interval)
        {
            try
            {
                m_pManager.ChangeStatus(ProjectStatus.Running);
                m_dManager.SimulationStart(l_interval, 0);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't run the simulator. {" + l_ex.ToString() + "}");
            }
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
            try
            {
                List<string> l_fullIDList = new List<string>();
                l_fullIDList.Add(l_fullID);
                m_dManager
                        .SaveSimulationResult(l_savedDirName, l_startTime, l_endTime, Constants.xpathCsv, l_fullIDList);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't run the simulator. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Saves the model.
        /// </summary>
        /// <param name="l_modelID">The saved model ID</param>
        public void SaveModel(string l_modelID)
        {
            try
            {
                m_dManager.SaveModel(l_modelID);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't save the model of this [" + l_modelID + "]. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Sets the entity property.
        /// </summary>
        /// <param name="l_fullPN">The full PN</param>
        /// <param name="l_value">The property value</param>
        public void SetEntityProperty(string l_fullPN, string l_value)
        {
            try
            {
                m_dManager.SetEntityProperty(l_fullPN, l_value);
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                    "Can't set the [" + l_value + "] value of the [" + l_fullPN + "] property. {"
                    + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Steps the simulator.
        /// </summary>
        /// <param name="l_count">The step limit of the simulator</param>
        public void Step(int l_count)
        {
            try
            {
                m_pManager.ChangeStatus(ProjectStatus.Running);
                m_dManager.SimulationStartKeepSetting(l_count);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't run the simulator. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Steps the simulator.
        /// </summary>
        /// <param name="l_count">The step limit of the simulator</param>
        public void StepNotSuspend(int l_count)
        {
            try
            {
                m_pManager.ChangeStatus(ProjectStatus.Running);
                m_dManager.SimulationStart(l_count, 0);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't run the simulator. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Stops the simulator.
        /// </summary>
        public void Stop()
        {
            try
            {
                m_pManager.ChangeStatus(ProjectStatus.Loaded);
                m_dManager.SimulationStop();
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't stop the simulator. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Suspends the simulator.
        /// </summary>
        public void Suspend()
        {
            try
            {
                m_pManager.ChangeStatus(ProjectStatus.Suspended);
                m_dManager.SimulationSuspend();
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't suspend the simulator. {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_type">data type to set initial parameter.</param>
        /// <param name="l_initialDic">the dictionary of initial parameter.</param>
        public void UpdateInitialCondition(string l_type, Dictionary<string, double> l_initialDic)
        {
            try
            {
                if (l_type == null || l_type.Length <= 0
                    || l_initialDic == null || l_initialDic.Count <= 0)
                {
                    return;
                }
                m_dManager.UpdateInitialCondition(null, s_modelID, l_type, l_initialDic);
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't update the initial condition. {" + l_ex.ToString() + "}");
            }
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
            /// <param name="dManager">DataManager instance to associate</param>
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
                try
                {
                    if (CommandManager.s_modelID == null || CommandManager.s_modelID.Length <= 0)
                    {
                        throw new Exception("The model ID is \"null\".");
                    }
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
                    foreach(EcellObject l_system
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
                    if (this.m_ecellObject == null)
                    {
                        throw new Exception("The class named [" + l_className + "] isn't found.");
                    }
                    //
                    // Adds the "EcellObject" to the "DataManager". 
                    //
                    List<EcellObject> l_list = new List<EcellObject>();
                    l_list.Add(this.m_ecellObject);
                    m_cManager.DataManager.DataAdd(l_list);
                }
                catch(Exception l_ex)
                {
                    throw new Exception("Can't create the entity stub named [" + l_className + "]. {"
                            + l_ex.ToString() + "}");
                }
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
                else
                {
                    throw new Exception("The [" + this.m_fullID + "] isn't up to standard.");
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
                try
                {
                    string l_key = null;
                    string l_type = null;
                    string l_systemKey = null;
                    this.RefinedFullID(ref l_key, ref l_type, ref l_systemKey);
                    m_cManager.DataManager.DataDelete(CommandManager.s_modelID, l_key, l_type);
                    this.m_ecellObject = null;
                    this.m_fullID = null;
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't delete the entity stub named [" + this.m_fullID + "]. {"
                            + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Tests whether the entity is already loaded.
            /// </summary>
            /// <returns>true if the entity is loaded; false otherwise</returns>
            public bool Exists()
            {
                try
                {
                    return m_cManager.DataManager.Exists(CommandManager.s_modelID, this.m_fullID);
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't confirm the existence of the entity stub named ["
                            + this.m_fullID + "]. {" + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Returns the class name.
            /// </summary>
            /// <returns>the class name</returns>
            public string GetClassName()
            {
                try
                {
                    if (this.m_ecellObject != null)
                    {
                        return this.m_ecellObject.Classname;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the class name of this entity stub named ["
                            + this.m_fullID + "]. {" + l_ex.ToString() + "}");
                }
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
                try
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the value of this [" + l_propertyName + "]. {"
                            + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Returns the attributes of the property.
            /// </summary>
            /// <param name="l_propertyName">the property name</param>
            /// <returns>the attributes of the property(Settable, Gettable, Loadable, Savable)</returns>
            public List<bool> GetPropertyAttributes(string l_propertyName)
            {
                try
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the attributes of this [" + l_propertyName + "]. {"
                            + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Returns the list of the property.
            /// </summary>
            /// <returns>the list of the property</returns>
            public List<string> GetPropertyList()
            {
                try
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the property list of this entity stub named ["
                            + this.m_fullID + "]. {" + l_ex.ToString() + "}");
                }
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
                if (l_infos.Length != 3)
                {
                    throw new Exception("The [" + this.m_fullID + "] isn't up to standard.");
                }
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
                try
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
                                        throw new Exception("The property named [" + l_propertyName + "]"
                                                + "isn't settable.");
                                    }
                                    else if (l_propertyName.Equals(Constants.xpathVRL))
                                    {
                                        try
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
                                        catch (Exception l_ex)
                                        {
                                            throw new Exception("The [" + l_value + "]"
                                                    + "isn't cast to \"VariableReferenceList\". {"
                                                    + l_ex.ToString() + "}");
                                        }
                                    }
                                    else if (l_data.Value.IsDouble())
                                    {
                                        try
                                        {
                                            l_data.Value = new EcellValue(XmlConvert.ToDouble(l_value));
                                            l_findFlag = true;
                                        }
                                        catch (Exception l_ex)
                                        {
                                            throw new Exception("The [" + l_value + "]" + "isn't cast to \"double\". {"
                                                    + l_ex.ToString() + "}");
                                        }
                                    }
                                    else if (l_data.Value.IsInt())
                                    {
                                        try
                                        {
                                            l_data.Value = new EcellValue(Convert.ToInt32(l_value));
                                            l_findFlag = true;
                                        }
                                        catch (Exception l_ex)
                                        {
                                            throw new Exception("The [" + l_value + "]" + "isn't cast to \"int\". {"
                                                    + l_ex.ToString() + "}");
                                        }
                                    }
                                    else if (l_data.Value.IsString())
                                    {
                                        try
                                        {
                                            l_data.Value = new EcellValue(l_value);
                                            l_findFlag = true;
                                        }
                                        catch (Exception l_ex)
                                        {
                                            throw new Exception("The [" + l_value + "]" + "isn't cast to \"string\". {"
                                                    + l_ex.ToString() + "}");
                                        }
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't set the property named [" + l_propertyName + "]. {"
                            + l_ex.ToString() + "}");
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
                try
                {
                    if (!this.m_isExist)
                    {
                        m_cManager.CreateLogger(this.m_fullPN);
                        this.m_isExist = true;
                    }
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't create the logger stub of [" + this.m_fullPN + "]. {"
                            + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Deletes the logger.
            /// </summary>
            public void Delete()
            {
                try
                {
                    m_cManager.DeleteLogger(this.m_fullPN);
                    this.m_isExist = false;
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't delete the logger stub of [" + this.m_fullPN + "]. {"
                            + l_ex.ToString() + "}");
                }
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
                try
                {
                    return m_cManager.GetLogData(this.m_fullPN, l_startTime, l_endTime);
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the logger data of [" + this.m_fullPN + "]. {"
                            + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Returns the end time of the logger.
            /// </summary>
            /// <returns>the end time of the logger</returns>
            public double GetEndTime()
            {
                try
                {
                    List<LogValue> l_logDataList =
                            m_cManager.GetLogData(
                                    this.m_fullPN,
                                    0.0,
                                    m_cManager.GetCurrentSimulationTime());
                    return l_logDataList[l_logDataList.Count - 1].time;
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the logger end time of [" + this.m_fullPN + "]. {"
                            + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Returns the logger policy of the logger.
            /// </summary>
            /// <returns>the logger policy</returns>
            public LoggerPolicy GetLoggerPolicy()
            {
                try
                {
                    return m_cManager.GetLoggerPolicy();
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the logger policy of [" + this.m_fullPN + "]. {"
                            + l_ex.ToString() + "}");
                }
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
                try
                {
                    List<LogValue> l_logDataList =
                            m_cManager.GetLogData(
                                this.m_fullPN,
                                0.0,
                                m_cManager.GetCurrentSimulationTime());
                    return l_logDataList.Count;
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the logger size of [" + this.m_fullPN + "]. {"
                            + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Returns the start time of the logger.
            /// </summary>
            /// <returns>the start time of the logger</returns>
            public double GetStartTime()
            {
                try
                {
                    List<LogValue> l_logDataList =
                        m_cManager.GetLogData(
                            this.m_fullPN, 0.0,
                            m_cManager.GetCurrentSimulationTime());
                    return l_logDataList[0].time;
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the logger start time of [" + this.m_fullPN + "]. {"
                            + l_ex.ToString() + "}");
                }
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
                try
                {
                    m_cManager.CreateLoggerPolicy(
                            l_savedStepCount, l_savedInterval, l_diskFullAction, l_maxDiskSpace);
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't create the logger policy of [" + this.m_fullPN + "]. {"
                            + l_ex.ToString() + "}");
                }
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
                try
                {
                    if (CommandManager.s_modelID == null || CommandManager.s_modelID.Length <= 0)
                    {
                        throw new Exception("The model ID is \"null\".");
                    }
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't create the ID named [" + this.m_parameterID
                            + "] of this simulation parameter stub. {" + l_ex.ToString() + "}");
                }
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
                try
                {
                    m_cManager.DataManager.DeleteSimulationParameter(this.m_parameterID);
                    this.m_parameterID = null;
                    this.m_loggerPolicy = new LoggerPolicy();
                    this.m_stepperList = null;
                    this.m_initialCondition = null;
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't delete this simulation parameter stub. {" + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Tests whether the simulation parameter is already loaded.
            /// </summary>
            /// <returns>true if the simulation parameter is loaded; false otherwise</returns>
            public bool Exists()
            {
                try
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't confirm the existence of the simulation parameter stub named ["
                            + this.m_parameterID + "]. {" + l_ex.ToString() + "}");
                }
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
                try
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
                    else
                    {
                        throw new Exception("Not found the type of [" + l_type + "].");
                    }
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the initial condition of [" + l_type + "]. {"
                            + l_ex.ToString() + "}");
                }
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
                try
                {
                    List<string> l_stepperIDList = new List<string>();
                    foreach (EcellObject l_ecellObject in this.m_stepperList)
                    {
                        l_stepperIDList.Add(l_ecellObject.Key);
                    }
                    return l_stepperIDList;
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the stepper ID list. {" + l_ex.ToString() + "}");
                }
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
                try
                {
                    LoggerPolicy l_loggerPolicy
                            = new LoggerPolicy(l_savedStepCount, l_savedInterval, l_diskFullAction, l_maxDiskSpace);
                    m_cManager.DataManager.SetLoggerPolicy(this.m_parameterID, ref l_loggerPolicy);
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't create the logger policy. {" + l_ex.ToString() + "}");
                }
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
                try
                {
                    if (CommandManager.s_modelID == null || CommandManager.s_modelID.Length <= 0)
                    {
                        throw new Exception("The model ID is \"null\".");
                    }
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
                    if (this.m_stepper == null)
                    {
                        throw new Exception("The class named [" + l_className + "] isn't found.");
                    }
                    //
                    // Adds the "EcellObject" to the "DataManager". 
                    //
                    m_cManager.DataManager.AddStepperID(this.m_parameterID, this.m_stepper);
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't create the class named [" + l_className + "] of this stepper stub. {"
                            + l_ex.ToString() + "}");
                }
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
                try
                {
                    m_cManager.DataManager.DeleteStepperID(this.m_parameterID, this.m_stepper);
                    this.m_ID = null;
                    this.m_parameterID = null;
                    this.m_stepper = null;
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't delete this stepper stub. {" + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Tests whether the stepper is already loaded.
            /// </summary>
            /// <returns>true if the stepper is loaded; false otherwise</returns>
            public bool Exists()
            {
                try
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't confirm the existence of the stepper stub named ["
                            + this.m_parameterID + "][" + this.m_ID + "]. {" + l_ex.ToString() + "}");
                }
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
                try
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the property named [" + l_propertyName + "]. {"
                            + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Returns the attributes of the property.
            /// </summary>
            /// <param name="l_propertyName">the property</param>
            /// <returns>the attributes of the property(Settable, Gettable, Loadable, Savable)</returns>
            public List<bool> GetPropertyAttributes(string l_propertyName)
            {
                try
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the attributes of this property named [" + l_propertyName
                            + "]. {" + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Returns the list of the property.
            /// </summary>
            /// <returns>the list of the property</returns>
            public List<string> GetPropertyList()
            {
                try
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
                catch (Exception l_ex)
                {
                    throw new Exception("Can't obtain the property list. {" + l_ex.ToString() + "}");
                }
            }

            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="l_propertyName">the property</param>
            /// <param name="l_value">the value</param>
            public void SetProperty(string l_propertyName, string l_value)
            {
                try
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
                                        throw new Exception("The [" + l_propertyName + "]" + "isn't settable.");
                                    }
                                    else if (l_data.Value.IsDouble())
                                    {
                                        try
                                        {
                                            l_data.Value = new EcellValue(XmlConvert.ToDouble(l_value));
                                            l_findFlag = true;
                                        }
                                        catch (Exception l_ex)
                                        {
                                            throw new Exception("The [" + l_value + "]" + "isn't cast to \"double\". {"
                                                    + l_ex.ToString() + "}");
                                        }
                                    }
                                    else if (l_data.Value.IsInt())
                                    {
                                        try
                                        {
                                            l_data.Value = new EcellValue(Convert.ToInt32(l_value));
                                            l_findFlag = true;
                                        }
                                        catch (Exception l_ex)
                                        {
                                            throw new Exception("The [" + l_value + "]" + "isn't cast to \"int\". {"
                                                    + l_ex.ToString() + "}");
                                        }
                                    }
                                    else if (l_data.Value.IsString())
                                    {
                                        try
                                        {
                                            l_data.Value = new EcellValue(l_value);
                                            l_findFlag = true;
                                        }
                                        catch (Exception l_ex)
                                        {
                                            throw new Exception("The [" + l_value + "]" + "isn't cast to \"string\". {"
                                                    + l_ex.ToString() + "}");
                                        }
                                    }
                                }
                            }
                            if (l_findFlag)
                            {
                                m_cManager.DataManager.DataChanged(
                                        this.m_stepper.ModelID,
                                        this.m_stepper.Key,
                                        this.m_stepper.Type,
                                        this.m_stepper,
                                        false,
                                        false);
                            }
                            else
                            {
                                throw new Exception("The property named [" + l_propertyName + "]" + "isn't found.");
                            }
                        }
                    }
                }
                catch (Exception l_ex)
                {
                    throw new Exception("Can't set the property named [" + l_propertyName + "]. {"
                            + l_ex.ToString() + "}");
                }
            }
        }
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
