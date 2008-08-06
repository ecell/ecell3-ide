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

namespace Ecell
{
    /// <summary>
    /// Manages data of projects, models, and so on.
    /// </summary>
    public class DataManager
    {
        #region Fields
        #region Managers
        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        private ApplicationEnvironment m_env;
        #endregion

        #region Project
        /// <summary>
        /// The list of "Project" with the project ID
        /// </summary>
        private List<Project> m_projectList = null;
        /// <summary>
        /// The current project
        /// </summary>
        private Project m_currentProject = null;
        #endregion

        /// <summary>
        /// The default directory
        /// </summary>
        private string m_defaultDir = null;
        /// <summary>
        /// The default count of the step 
        /// </summary>
        private int m_defaultStepCount = 10;
        /// <summary>
        /// The step limit of the simulation
        /// </summary>
        private int m_simulationStepLimit = -1;
        /// <summary>
        /// The time limit of the simulation
        /// </summary>
        private double m_simulationTimeLimit = -1.0;
        /// <summary>
        /// The start time of the simulation
        /// </summary>
        private double m_simulationStartTime = 0.0;
        /// <summary>
        /// add action flag
        /// </summary>
        private bool m_isAdded = false;

        private Dictionary<string, EcellObservedData> m_observedList;
        private Dictionary<string, EcellParameterData> m_parameterList;
        /// <summary>
        /// ResourceManager for StaticDebugSetupWindow.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResources));
        #endregion

        /// <summary>
        /// Creates the new "DataManager" instance with no argument.
        /// </summary>
        public DataManager(ApplicationEnvironment env)
        {
            this.m_env = env;
            this.m_defaultDir = Util.GetBaseDir();
            this.m_projectList = new List<Project>();
            this.m_observedList = new Dictionary<string, EcellObservedData>();
            this.m_parameterList = new Dictionary<string, EcellParameterData>();
        }

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
                return m_currentProject.Name;
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
                throw new Exception(String.Format(MessageResources.ErrSaveAct), ex);
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
                CloseProject(null);
                m_env.ActionManager.LoadActionFile(filenName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new Exception(String.Format(MessageResources.ErrLoadFile,
                    new object[] { filenName }), ex);
            }
        }


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
                    throw new Exception();
                if (!m_currentProject.StepperDic.ContainsKey(parameterID))
                    m_currentProject.StepperDic[parameterID] = new Dictionary<string, List<EcellObject>>();
                stepperDic = m_currentProject.StepperDic[parameterID];
                if (!stepperDic.ContainsKey(stepper.ModelID))
                    throw new Exception();

                // Check duplication.
                foreach (EcellObject storedStepper in stepperDic[stepper.ModelID])
                {
                    if (!stepper.Key.Equals(storedStepper.Key))
                        continue;
                    throw new Exception(
                        string.Format(MessageResources.ErrExistStepper,
                            new object[] { stepper.Key }
                        )
                    );
                }
                // Set Stteper.
                stepperDic[stepper.ModelID].Add(stepper);
                if (m_currentProject.SimulationParam.Equals(parameterID))
                {
                    List<EcellObject> stepperList = new List<EcellObject>();
                    stepperList.Add(stepper);
                    m_env.PluginManager.DataAdd(stepperList);
                }
                MessageCreateEntity("Stepper", message);
                if (isRecorded)
                    m_env.ActionManager.AddAction(new AddStepperAction(parameterID, stepper));
            }
            catch (Exception ex)
            {
                message = String.Format(MessageResources.ErrNotCreStepper,
                    new object[] { stepper.Key });
                Trace.WriteLine(message);
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Checks differences between the source "EcellObject" and the destination.
        /// </summary>
        /// <param name="src">The source "EcellObject"</param>
        /// <param name="dest">The  destination "EcellObject"</param>
        /// <param name="parameterID">The simulation parameter ID</param>
        private void CheckDifferences(EcellObject src, EcellObject dest, string parameterID)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>> initialCondition = this.m_currentProject.InitialCondition;

            // Set Message
            string message = null;
            if (string.IsNullOrEmpty(parameterID))
                message = "[" + src.ModelID + "][" + src.Key + "]";
            else
                message = "[" + parameterID + "][" + src.ModelID + "][" + src.Key + "]";

            // Check Class change.
            if (!src.Classname.Equals(dest.Classname))
                MessageUpdateData(Constants.xpathClassName, message, src.Classname, dest.Classname);
            // Check Key change.
            if (!src.Key.Equals(dest.Key))
                MessageUpdateData(Constants.xpathKey, message, src.Key, dest.Key);

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
                            Dictionary<string, double> condition = initialCondition[keyParameterID][src.ModelID][src.Type];
                            if (condition.ContainsKey(srcEcellData.EntityPath))
                            {
                                condition.Remove(srcEcellData.EntityPath);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, double> condition = initialCondition[parameterID][src.ModelID][src.Type];
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
                        temp = value.CastToDouble();
                    else if (value.IsInt)
                        temp = value.CastToInt();
                    else
                        continue;

                    if (!string.IsNullOrEmpty(parameterID))
                    {
                        initialCondition[parameterID][dest.ModelID][dest.Type][destEcellData.EntityPath] = temp;
                    }
                    else
                    {
                        foreach (string keyParameterID in initialCondition.Keys)
                        {
                            initialCondition[keyParameterID][dest.ModelID][dest.Type][destEcellData.EntityPath] = temp;
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
                        }
                        else if (srcEcellData.Logged && !destEcellData.Logged)
                        {
                            MessageDeleteEntity("Logger", message + "[" + srcEcellData.Name + "]");
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
                            temp = value.CastToDouble();
                        else if (value.IsInt)
                            temp = value.CastToInt();
                        else
                            continue;

                        if (!string.IsNullOrEmpty(parameterID))
                        {
                            Dictionary<string, double> condition = initialCondition[parameterID][src.ModelID][src.Type];
                            if (!condition.ContainsKey(srcEcellData.EntityPath))
                                continue;
                            condition[srcEcellData.EntityPath] = temp;
                        }
                        else
                        {
                            foreach (string keyParameterID in initialCondition.Keys)
                            {
                                Dictionary<string, double> condition = initialCondition[keyParameterID][src.ModelID][src.Type];

                                if (!condition.ContainsKey(srcEcellData.EntityPath))
                                    continue;
                                condition[srcEcellData.EntityPath] = temp;
                            }
                        }
                        break;
                    }
                }
            }
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
                string childPath = ecellObject.Name;
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
        /// Checks whether the "VariableReferenceList" has available "Variable"s.
        /// </summary>
        /// <param name="src">The checked "EcellObject"</param>
        /// <param name="dest">The available "EcellObject"</param>
        /// <param name="variableDic">The dictionary of the name of available "Variable"s</param>
        /// <returns>true if it modified; false otherwise</returns>
        private static bool CheckVariableReferenceList(
            EcellObject src, ref EcellObject dest, Dictionary<string, string> variableDic)
        {
            bool changedFlag = false;
            dest = src.Copy();
            EcellValue varList = dest.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST);
            if (varList == null || varList.ToString().Length <= 0)
                return changedFlag;

            List<EcellValue> changedValue = new List<EcellValue>();
            foreach (EcellValue ecellValue in varList.CastToList())
            {
                List<EcellValue> changedElements = new List<EcellValue>();
                foreach (EcellValue element in ecellValue.CastToList())
                {
                    if (element.IsString
                        && element.CastToString().StartsWith(Constants.delimiterColon))
                    {
                        string oldKey = element.CastToString().Substring(1);
                        if (variableDic.ContainsKey(oldKey))
                        {
                            changedElements.Add(
                                new EcellValue(Constants.delimiterColon + variableDic[oldKey]));
                            changedFlag = true;
                        }
                        else
                        {
                            changedElements.Add(element);
                        }
                    }
                    else
                    {
                        changedElements.Add(element);
                    }
                }
                changedValue.Add(new EcellValue(changedElements));
            }
            dest.GetEcellData(EcellProcess.VARIABLEREFERENCELIST).Value = new EcellValue(changedValue);

            return changedFlag;
        }

        /// <summary>
        /// Checks whether the "VariableReferenceList" has available "Variable"s.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="newKey">The new key</param>
        /// <param name="oldKey">The old key</param>
        /// <param name="changedList">The list of the modified "ECekllObject"</param>
        private void CheckVariableReferenceList(
            string modelID, string newKey, string oldKey, List<EcellObject> changedList)
        {
            foreach (EcellObject system in GetData(modelID, null))
            {
                if (system.Children == null || system.Children.Count <= 0)
                    continue;

                foreach (EcellObject instance in system.Children)
                {
                    EcellObject entity = instance.Copy();
                    if (!entity.Type.Equals(Constants.xpathProcess))
                        continue;
                    else if (entity.Value == null || entity.Value.Count <= 0)
                        continue;

                    bool changedFlag = false;
                    foreach (EcellData ecellData in entity.Value)
                    {
                        if (!ecellData.Name.Equals(Constants.xpathVRL))
                            continue;

                        List<EcellValue> changedValue = new List<EcellValue>();
                        if (ecellData.Value == null) continue;
                        if (ecellData.Value.ToString() == "") continue;
                        foreach (EcellValue ecellValue in ecellData.Value.CastToList())
                        {
                            List<EcellValue> changedElements = new List<EcellValue>();
                            foreach (EcellValue element in ecellValue.CastToList())
                            {
                                if (element.IsString
                                    && element.CastToString().Equals(Constants.delimiterColon + oldKey))
                                {
                                    changedElements.Add(
                                        new EcellValue(Constants.delimiterColon + newKey));
                                    changedFlag = true;
                                }
                                else
                                {
                                    changedElements.Add(element);
                                }
                            }
                            changedValue.Add(new EcellValue(changedElements));
                        }
                        ecellData.Value = new EcellValue(changedValue);
                    }
                    if (changedFlag)
                    {
                        changedList.Add(entity);
                    }
                }
            }
        }

        /// <summary>
        /// Closes project without confirming save or no save.
        /// </summary>
        /// <param name="projectID">The project ID</param>
        public void CloseProject(string projectID)
        {
            try
            {
                List<string> tmpList = new List<string>();
                if (projectID == null)
                {
                    foreach (Project prj in m_projectList)
                    {
                        tmpList.Add(prj.Name);
                    }
                }
                else
                {
                    tmpList.Add(projectID);
                }

                foreach (string str in tmpList)
                {
                    foreach (Project prj in m_projectList)
                    {
                        if (prj.Name == str)
                        {
                            m_projectList.Remove(prj);
                            break;
                        }
                    }
                }
                if (this.m_currentProject != null)
                {
                    this.m_currentProject.Simulator.Dispose();
                    this.m_currentProject = null;
                }
                this.m_env.PluginManager.AdvancedTime(0);
                this.m_env.PluginManager.Clear();
                Trace.WriteLine(String.Format(MessageResources.InfoClose,
                    new object[] { projectID }));
                m_env.ActionManager.Clear();
                m_env.MessageManager.Clear();
                m_env.PluginManager.ChangeStatus(ProjectStatus.Uninitialized);
            }
            catch (Exception ex)
            {
                String errmes = string.Format(MessageResources.ErrClosePrj,
                    new object[] { projectID });

                Trace.WriteLine(errmes);
                throw new Exception(errmes, ex);
            }
        }

        /// <summary>
        /// Checks whether the model ID exists in this project.
        /// </summary>
        /// <param name="modelID">The checked model ID</param>
        /// <returns>true if the model ID exists; false otherwise</returns>
        public bool ContainsModel(string modelID)
        {
            bool foundFlag = false;
            foreach (EcellObject ecellObject in m_currentProject.ModelList)
            {
                if (ecellObject.ModelID.Equals(modelID))
                {
                    foundFlag = true;
                    break;
                }
            }
            return foundFlag;
        }

        /// <summary>
        /// Copys the initial condition.
        /// </summary>
        /// <param name="srcDic">The source</param>
        /// <param name="destDic">The destination</param>
        private static void Copy4InitialCondition(
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> srcDic,
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> destDic)
        {
            foreach (string parentKey in srcDic.Keys)
            {
                destDic[parentKey] = new Dictionary<string, Dictionary<string, double>>();
                foreach (string childKey in srcDic[parentKey].Keys)
                {
                    destDic[parentKey][childKey] = new Dictionary<string, double>();
                    foreach (string grandChildKey in srcDic[parentKey][childKey].Keys)
                    {
                        destDic[parentKey][childKey][grandChildKey]
                                = srcDic[parentKey][childKey][grandChildKey];
                    }
                }
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
                bool processFlag = false;
                bool stepperFlag = false;
                if (defaultProcess != null)
                {
                    processFlag = true;
                }
                if (defaultStepper != null)
                {
                    stepperFlag = true;
                }
                if (!processFlag || !stepperFlag)
                {
                    // 4 Process
                    //
                    if (!processFlag)
                    {
                        defaultProcess = Constants.DefaultProcessName;
                        processFlag = true;
                    }
                    //
                    // 4 Stepper
                    //
                    if (!stepperFlag)
                    {
                        defaultStepper = Constants.DefaultStepperName;
                        stepperFlag = true;
                    }
                }
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
                    Util.BuildFullPN(
                        Constants.xpathSystem,
                        "",
                        Constants.delimiterPath,
                        Constants.xpathStepperID
                    ),
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
                throw new Exception(
                    MessageResources.ErrCombiStepProc, ex);
            }
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
        /// Adds the list of "EcellObject".
        /// </summary>
        /// <param name="ecellObjectList">The list of "EcellObject"</param>
        public void DataAdd(List<EcellObject> ecellObjectList)
        {
            DataAdd(ecellObjectList, true, true);
        }

        /// <summary>
        /// Adds the list of "EcellObject".
        /// </summary>
        /// <param name="ecellObjectList">The list of "EcellObject"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DataAdd(List<EcellObject> ecellObjectList, bool isRecorded, bool isAnchor)
        {
            foreach (EcellObject ecellObject in ecellObjectList)
            {
                DataAdd(ecellObject, isRecorded, isAnchor);
            }
        }

        /// <summary>
        /// Adds the "EcellObject".
        /// </summary>
        /// <param name="ecellObject">The "EcellObject"</param>
        /// <param name="isRecorded">Whether this action is recorded or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void DataAdd(EcellObject ecellObject, bool isRecorded, bool isAnchor)
        {
            List<EcellObject> usableList = new List<EcellObject>();
            string type = null;

            bool isUndoable = true; // Whether DataAdd action is undoable or not
            try
            {
                if (!this.IsUsable(ecellObject))
                    return;
                type = ecellObject.Type;

                ConfirmReset("add", type);

                if (type.Equals(Constants.xpathProcess) || type.Equals(Constants.xpathVariable) || type.Equals(Constants.xpathText))
                {
                    this.DataAdd4Entity(ecellObject, true);
                    usableList.Add(ecellObject);
                }
                else if (type.Equals(Constants.xpathSystem))
                {
                    this.DataAdd4System(ecellObject, true);
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
            catch (Exception ex)
            {
                string message = String.Format(MessageResources.ErrAdd,
                    new object[] { type, ecellObject.Key });

                usableList = null;
                Trace.WriteLine(message);
                throw new Exception(message, ex);
            }
            finally
            {
                if (usableList != null && usableList.Count > 0)
                {
                    m_isAdded = true;
                    m_env.PluginManager.DataAdd(usableList);
                    m_isAdded = false;
                    foreach (EcellObject obj in usableList)
                    {
                        if (isRecorded)
                            m_env.ActionManager.AddAction(new DataAddAction(obj, isUndoable, isAnchor));
                    }
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
            List<EcellObject> modelList = m_currentProject.ModelList;

            string modelID = ecellObject.ModelID;

            string message = String.Format(MessageResources.InfoAdd,
                new object[] { ecellObject.Type, modelID });
            foreach (EcellObject model in modelList)
            {
                Debug.Assert(!model.ModelID.Equals(modelID));
            }
            //
            // Sets the "Model".
            //
            if (modelList == null)
            {
                modelList = new List<EcellObject>();
            }
            modelList.Add(ecellObject);
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
                m_currentProject.StepperDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                m_currentProject.StepperDic[simParam] = new Dictionary<string, List<EcellObject>>();
                m_currentProject.StepperDic[simParam][modelID] = new List<EcellObject>();
                m_currentProject.StepperDic[simParam][modelID].Add(dic[Constants.xpathStepper]);
                m_currentProject.LoggerPolicyDic = new Dictionary<string, LoggerPolicy>();
                m_currentProject.LoggerPolicyDic[simParam] = new LoggerPolicy();
            }
            //
            // Messages
            //
            MessageCreateEntity(EcellObject.MODEL, message);
            MessageCreateEntity(EcellObject.SYSTEM, message);
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
        /// <param name="ecellObject">The System</param>
        /// <param name="messageFlag">The flag of the messages</param>
        private void DataAdd4System(EcellObject ecellObject, bool messageFlag)
        {
            string modelID = ecellObject.ModelID;
            string type = ecellObject.Type;
            string message = String.Format(MessageResources.InfoAdd,
                new object[] { type, ecellObject.Key });

            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;

            if (!sysDic.ContainsKey(modelID))
                sysDic[modelID] = new List<EcellObject>();
            // Check duplicated system.
            foreach (EcellObject system in sysDic[modelID])
            {
                if (!system.Key.Equals(ecellObject.Key))
                    continue;
                String errmessage = String.Format(MessageResources.ErrAdd,
                    new object[] { type, ecellObject.Key });

                throw new Exception(errmessage);
            }
            CheckEntityPath(ecellObject);
            sysDic[modelID].Add(ecellObject.Copy());

            if (messageFlag)
            {
                MessageCreateEntity(EcellObject.SYSTEM, message);
            }

            if (ecellObject.Value == null || ecellObject.Value.Count <= 0)
                return;
            // Set simulation parameter
            SetSimulationParameter(ecellObject, modelID, type);
        }

        /// <summary>
        /// Adds the "Process" or the "Variable".
        /// </summary>
        /// <param name="ecellObject">The "Variable"</param>
        /// <param name="messageFlag">The flag of the messages</param>
        private void DataAdd4Entity(EcellObject ecellObject, bool messageFlag)
        {
            string modelID = ecellObject.ModelID;
            string key = ecellObject.Key;
            string type = ecellObject.Type;
            string systemKey = ecellObject.ParentSystemID;
            string message = String.Format(MessageResources.InfoAdd,
                new object[] { type, ecellObject.Key });

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
                    throw new Exception(
                        string.Format(
                            MessageResources.ErrExistObj,
                            new object[] { key }
                        )
                    );
                }
                // Set object.
                CheckEntityPath(ecellObject);
                system.Children.Add(ecellObject.Copy());
                findFlag = true;
                if (messageFlag)
                {
                    MessageCreateEntity(type, message);
                }
                break;
            }

            Debug.Assert(findFlag);

            if (ecellObject.Value == null || ecellObject.Value.Count <= 0)
                return;
            if (ecellObject is EcellText)
                return;

            // Set Simulation param
            SetSimulationParameter(ecellObject, modelID, type);
        }

        /// <summary>
        /// Set simulation parameter
        /// </summary>
        /// <param name="ecellObject"></param>
        /// <param name="modelID"></param>
        /// <param name="type"></param>
        private void SetSimulationParameter(EcellObject ecellObject, string modelID, string type)
        {
            foreach (string keyParameterID in m_currentProject.InitialCondition.Keys)
            {
                Dictionary<string, double> initialCondition = m_currentProject.InitialCondition[keyParameterID][modelID][type];
                foreach (EcellData data in ecellObject.Value)
                {
                    if (!data.IsInitialized())
                        continue;

                    double value = 0;
                    if (data.Value.IsDouble)
                        value = data.Value.CastToDouble();
                    else if (data.Value.IsInt)
                        value = data.Value.CastToInt();

                    initialCondition[data.EntityPath] = value;
                }
            }
        }

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
            foreach (EcellObject obj in ecellObjectList)
                DataChanged(obj.ModelID, obj.Key, obj.Type, obj, isRecorded, isAnchor);
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
            // StatusCheck
            if (m_currentProject.SimulationStatus == SimulationStatus.Run ||
                m_currentProject.SimulationStatus == SimulationStatus.Suspended)
            {
                EcellObject obj = GetEcellObject(modelID, key, type);
                if (!key.Equals(ecellObject.Key) ||
                    obj.Value.Count != ecellObject.Value.Count)
                    ConfirmReset("change", type);

                foreach (EcellData d in obj.Value)
                {
                    foreach (EcellData d1 in ecellObject.Value)
                    {
                        if (!d.Name.Equals(d1.Name)) continue;
                        if (!d.Value.ToString().Equals(d1.Value.ToString()))
                        {
                            WrappedPolymorph newValue = EcellValue.CastToWrappedPolymorph4EcellValue(d1.Value);
                            m_currentProject.Simulator.SetEntityProperty(d1.EntityPath, newValue);
                        }
                        break;
                    }
                }
            }
            string message = null;

            try
            {
                // Check null.

                message = "[" + ecellObject.ModelID + "][" + ecellObject.Key + "]";
                Debug.Assert(!String.IsNullOrEmpty(modelID));
                Debug.Assert(!String.IsNullOrEmpty(key));
                Debug.Assert(!String.IsNullOrEmpty(type));

                // Record action
                EcellObject oldObj = GetEcellObject(modelID, key, type);

                //if (!oldObj.IsPosSet)
                //    m_env.PluginManager.SetPosition(oldObj);                
                if (isRecorded && !m_isAdded)
                    this.m_env.ActionManager.AddAction(new DataChangeAction(modelID, type, oldObj.Copy(), ecellObject.Copy(), isAnchor));

                // Searches the "System".
                List<EcellObject> systemList = m_currentProject.SystemDic[modelID];
                Debug.Assert(systemList != null && systemList.Count > 0);

                // Checks the EcellObject
                CheckEntityPath(ecellObject);

                // 4 System & Entity
                if (ecellObject.Type.Equals(Constants.xpathSystem))
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
                    if (!modelID.Equals(ecellObject.ModelID) || !key.Equals(ecellObject.Key))
                    {
                        List<EcellObject> changedProcessList = new List<EcellObject>();
                        CheckVariableReferenceList(modelID, ecellObject.Key, key, changedProcessList);
                        foreach (EcellObject changedProcess in changedProcessList)
                        {
                            this.DataChanged4Entity(
                                changedProcess.ModelID, changedProcess.Key,
                                changedProcess.Type, changedProcess, isRecorded, isAnchor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                    MessageResources.ErrSetProp,
                    new object[] { key }),
                    ex);
            }
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
            string message = "[" + modelID + "][" + ecellObject.Key + "]";
            List<EcellObject> changedProcessList = new List<EcellObject>();

            // Get parent system.
            EcellObject oldSystem = m_currentProject.GetSystem(modelID, Util.GetSuperSystemPath(key));
            EcellObject newSystem = m_currentProject.GetSystem(ecellObject.ModelID, ecellObject.ParentSystemID);
            Debug.Assert(oldSystem != null && newSystem != null);

            // Get changed node.
            EcellObject oldNode = m_currentProject.GetEcellObject(modelID, type, key);
            Debug.Assert(oldNode != null);

            this.CheckDifferences(oldNode, ecellObject, null);
            if (modelID.Equals(ecellObject.ModelID)
                && key.Equals(ecellObject.Key)
                && type.Equals(ecellObject.Type))
            {
                newSystem.Children.Remove(oldNode);
                newSystem.Children.Add(ecellObject);
                this.m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
            }
            else
            {
                // Add new object.
                this.DataAdd4Entity(ecellObject.Copy(), false);
                this.m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
                // Deletes the old object.
                this.DataDelete4Node(modelID, key, type, false, isRecorded, isAnchor);
            }

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
            string message = "[" + ecellObject.ModelID + "][" + ecellObject.Key + "]";
            m_currentProject.SortSystems();
            List<EcellObject> systemList = m_currentProject.SystemDic[modelID];

            if (modelID.Equals(ecellObject.ModelID)
                && key.Equals(ecellObject.Key)
                && type.Equals(ecellObject.Type))
            {
                // Changes some properties.
                for (int i = 0; i < systemList.Count; i++)
                {
                    if (!systemList[i].Key.Equals(key))
                        continue;

                    this.CheckDifferences(systemList[i], ecellObject, null);
                    systemList[i] = ecellObject.Copy();
                    this.m_env.PluginManager.DataChanged(modelID, key, type, ecellObject);
                    break;
                }
                return;
            }

            // Changes the key.
            Dictionary<string, string> variableKeyDic = new Dictionary<string, string>();
            Dictionary<string, string> processKeyDic = new Dictionary<string, string>();
            List<string> createdSystemKeyList = new List<string>();
            List<string> deletedSystemKeyList = new List<string>();
            List<EcellObject> tempList = new List<EcellObject>();
            tempList.AddRange(systemList);

            foreach (EcellObject system in tempList)
            {
                if (!system.Key.Equals(key) && !system.Key.StartsWith(key + Constants.delimiterPath))
                    continue;

                // Adds the new "System" object.
                string newKey = ecellObject.Key + system.Key.Substring(key.Length);
                EcellObject newSystem
                    = EcellObject.CreateObject(modelID, newKey, system.Type, system.Classname, system.Value);
                newSystem.SetPosition(system);
                CheckEntityPath(newSystem);
                DataAdd4System(newSystem, false);
                CheckDifferences(system, newSystem, null);
                m_env.PluginManager.DataChanged(modelID, system.Key, type, newSystem);
                createdSystemKeyList.Add(newKey);

                // Deletes the old "System" object.
                deletedSystemKeyList.Add(system.Key);
                // 4 Children
                if (system.Children == null || system.Children.Count <= 0)
                    continue;

                List<EcellObject> instanceList = new List<EcellObject>();
                instanceList.AddRange(system.Children);
                foreach (EcellObject childObject in instanceList)
                {
                    EcellObject copy = childObject.Copy();
                    string childKey = childObject.Key;
                    string keyName = childObject.Name;
                    if ((system.Key.Equals(key)))
                    {
                        copy.Key = ecellObject.Key + Constants.delimiterColon + keyName;
                    }
                    else
                    {
                        copy.Key = ecellObject.Key + copy.Key.Substring(key.Length);
                    }
                    CheckEntityPath(copy);
                    if (copy.Type.Equals(Constants.xpathVariable))
                    {
                        variableKeyDic[childKey] = copy.Key;
                        this.DataChanged4Entity(copy.ModelID, childKey, copy.Type, copy, isRecorded, isAnchor);
                    }
                    else
                    {
                        processKeyDic[childKey] = copy.Key;
                    }
                }
            }
            // Checks all processes.
            m_currentProject.SortSystems();
            systemList = m_currentProject.SystemDic[modelID];
            foreach (EcellObject system in systemList)
            {
                if (createdSystemKeyList.Contains(system.Key))
                    continue;
                if (system.Children == null || system.Children.Count <= 0)
                    continue;

                List<EcellObject> instanceList = new List<EcellObject>();
                instanceList.AddRange(system.Children);
                foreach (EcellObject childObject in instanceList)
                {
                    if (!childObject.Type.Equals(Constants.xpathProcess))
                        continue;

                    bool changedFlag = false;
                    // 4 VariableReferenceList
                    EcellObject dest = null;
                    if (CheckVariableReferenceList(childObject, ref dest, variableKeyDic))
                    {
                        changedFlag = true;
                    }
                    // 4 key
                    string oldKey = dest.Key;
                    string keyName = oldKey.Split(Constants.delimiterColon.ToCharArray())[1];
                    if (processKeyDic.ContainsKey(oldKey))
                    {
                        dest.Key = processKeyDic[oldKey];
                        CheckEntityPath(dest);
                        changedFlag = true;
                    }
                    if (changedFlag)
                    {
                        this.DataChanged4Entity(dest.ModelID, oldKey, dest.Type, dest, isRecorded, isAnchor);
                    }
                }
            }
            // Deletes old "System"s.
            for (int i = deletedSystemKeyList.Count - 1; i >= 0; i--)
            {
                this.DataDelete4System(modelID, deletedSystemKeyList[i], false);
            }
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
            ConfirmReset("delete", type);

            string message = null;
            EcellObject deleteObj = null;
            try
            {
                if (string.IsNullOrEmpty(modelID))
                    return;
                message = "[" + modelID + "][" + key + "]";

                deleteObj = GetEcellObject(modelID, key, type);

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
                    DataDelete4System(modelID, key, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(MessageResources.ErrDelete,
                    new object[] { key }), ex);
            }
            finally
            {
                m_env.PluginManager.DataDelete(modelID, key, type);
                if (isRecorded)
                    m_env.ActionManager.AddAction(new DataDeleteAction(modelID, key, type, deleteObj, isAnchor));
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
                    m_currentProject.ModelList.Remove(obj);
                    isDelete = true;
                    break;
                }
            }
            Debug.Assert(isDelete);

            //
            // Deletes "System"s.
            //
            if (m_currentProject.SystemDic.ContainsKey(modelID))
            {
                m_currentProject.SystemDic.Remove(modelID);
            }
            //
            // Deletes "Stepper"s.
            //
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

            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>> initialCondition = this.m_currentProject.InitialCondition;

            string message = "[" + model + "][" + key + "]";
            List<EcellObject> delList = new List<EcellObject>();

            List<EcellObject> sysList = m_currentProject.SystemDic[model];
            foreach (EcellObject system in sysList)
            {
                if (system.ModelID != model || system.Key != Util.GetSuperSystemPath(key))
                    continue;
                if (system.Children == null)
                    continue;

                foreach (EcellObject child in system.Children)
                {
                    if (child.Key == key && child.Type == type)
                        delList.Add(child);
                }
                foreach (EcellObject child in delList)
                {
                    system.Children.Remove(child);
                    if (messageFlag)
                    {
                        MessageDeleteEntity(type, message);
                    }

                    if (child.Value == null || child.Value.Count <= 0)
                        continue;
                    if (child is EcellText)
                        continue;

                    foreach (string keyParameterID in initialCondition.Keys)
                    {
                        Dictionary<string, double> condition = initialCondition[keyParameterID][child.ModelID][child.Type];
                        foreach (EcellData data in child.Value)
                        {
                            if (!data.Settable)
                                continue;
                            if (!condition.ContainsKey(data.EntityPath))
                                continue;
                            condition.Remove(data.EntityPath);
                        }
                    }
                }
                if (messageFlag)
                {
                    this.DataDelete4VariableReferenceList(delList, isRecorded, isAnchor);
                }
                delList.Clear();
            }
        }

        /// <summary>
        /// Deletes entries of the "VariableRefereceList".
        /// </summary>
        /// <param name="delList">The list of the deleted "Variable"</param>
        /// <param name="isRecorded">The flag whether this action is recorded.</param>
        /// <param name="isAnchor">The flag whether this action is anchor.</param>
        private void DataDelete4VariableReferenceList(List<EcellObject> delList, bool isRecorded, bool isAnchor)
        {
            if (delList == null || delList.Count <= 0)
                return;

            foreach (EcellObject del in delList)
            {
                if (!del.Type.Equals(EcellObject.VARIABLE))
                    continue;

                string variableKey = del.Key;
                foreach (EcellObject system in m_currentProject.SystemDic[del.ModelID])
                {
                    List<EcellObject> changeList = new List<EcellObject>();
                    foreach (EcellObject child in system.Children)
                    {
                        bool changedFlag = false;
                        if (!child.Type.Equals(EcellObject.PROCESS))
                            continue;

                        EcellProcess process = (EcellProcess)child.Copy();
                        List<EcellReference> erList = new List<EcellReference>();
                        foreach (EcellReference er in process.ReferenceList)
                        {
                            if (er.Key.Equals(variableKey))
                                changedFlag = true;
                            else
                                erList.Add(er);
                        }
                        process.ReferenceList = erList;

                        if (changedFlag)
                            changeList.Add(process);
                    }
                    foreach (EcellObject change in changeList)
                    {
                        this.DataChanged(change.ModelID, change.Key, change.Type, change, isRecorded, isAnchor);
                    }
                }
            }
        }

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
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public void RemoveObservedData(EcellObservedData data)
        {
            if (m_observedList.ContainsKey(data.Key))
                m_observedList.Remove(data.Key);
            m_env.PluginManager.RemoveObservedData(data);
        }

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
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            if (m_parameterList.ContainsKey(data.Key))
                m_parameterList.Remove(data.Key);
            m_env.PluginManager.RemoveParameterData(data);
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
            List<EcellObject> list = m_currentProject.SystemDic[modelID];
            foreach (EcellObject sys in list)
            {
                // Check systems.
                if (key.Equals(sys.Key) && type.Equals(sys.Type))
                    return true;
                // Continue if system has no node.
                if (sys.Children == null)
                    continue;
                // Check processes and variables
                foreach (EcellObject subEo in sys.Children)
                {
                    if (key.Equals(subEo.Key) && type.Equals(subEo.Type))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Move the component to the upper system, when system is deleted.
        /// </summary>
        /// <param name="modelID">modelID of deleted system.</param>
        /// <param name="key">key of deleted system.</param>
        public void SystemDeleteAndMove(string modelID, string key)
        {
            SystemDeleteAndMove(modelID, key, true, true);
        }

        /// <summary>
        /// Move the component to the upper system, when system is deleted.
        /// </summary>
        /// <param name="modelID">modelID of deleted system.</param>
        /// <param name="key">key of deleted system.</param>
        /// <param name="isRecorded">whether this action will be recorded or not</param>
        /// <param name="isAnchor">whether this action is an anchor or not</param>
        public void SystemDeleteAndMove(string modelID, string key, bool isRecorded, bool isAnchor)
        {
            EcellObject system = GetEcellObject(modelID, key, EcellObject.SYSTEM);
            if (system.Key.Equals("/"))
            {
                throw new Exception(MessageResources.ErrDelRoot);
            }
            // Get objects under this system.
            List<EcellObject> eoList = GetObjectUnder(modelID, key);

            // Check Object duplication.
            string sysKey = system.Key;
            string parentSysKey = system.ParentSystemID;
            foreach (EcellObject eo in eoList)
            {
                string newKey = Util.GetMovedKey(eo.Key, sysKey, parentSysKey);
                if (GetEcellObject(modelID, newKey, eo.Type) != null)
                {
                    throw new Exception(String.Format(MessageResources.ErrExistObj,
                        new object[] { newKey }));
                }
            }
            // Confirm system merge.
            if (!Util.ShowYesNoDialog(MessageResources.ConfirmMerge))
            {
                return;
            }

            // Move systems and nodes under merged system.
            foreach (EcellObject eo in eoList)
            {
                string oldKey = eo.Key;
                EcellObject obj = GetEcellObject(modelID, oldKey, eo.Type);
                if (obj == null)
                    continue;
                obj.Key = Util.GetMovedKey(oldKey, sysKey, parentSysKey);
                DataChanged(modelID, oldKey, eo.Type, obj, true, false);
            }
            DataDelete(modelID, sysKey, system.Type, true, true);
        }
        /// <summary>
        /// Get object list under the system.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private List<EcellObject> GetObjectUnder(string modelID, string key)
        {
            List<EcellObject> eoList = new List<EcellObject>();

            foreach (EcellObject obj in m_currentProject.SystemDic[modelID])
            {
                if (obj.ModelID != modelID || !obj.Key.StartsWith(key))
                    continue;
                if (!obj.Key.Equals(key))
                    eoList.Add(obj.Copy());

                foreach (EcellObject node in obj.Children)
                {
                    if (node.Key.EndsWith(":SIZE"))
                        continue;
                    eoList.Add(node.Copy());
                }
            }
            return eoList;
        }

        /// <summary>
        /// Used only to undo SystemDeleteAndMove. So, this method should be used only by ActionManager
        /// </summary>
        /// <param name="modelID">Model id of an added system</param>
        /// <param name="obj">An added system</param>
        /// <param name="sysList">Child systems of added system</param>
        /// <param name="nodeList">Child objects of added system</param>
        public void SystemAddAndMove(string modelID,
            EcellObject obj,
            List<EcellObject> sysList,
            List<EcellObject> nodeList)
        {
            // Temporary delete objects to be moved into a new system.
            foreach (EcellObject sys in sysList)
                DataDelete(modelID, sys.Key, "System", false, false);

            string[] el = obj.Key.Split(new char[] { '/' });
            int delPoint = el.Length - 1;
            List<EcellObject> list = new List<EcellObject>();
            foreach (EcellObject sys in sysList)
            {
                String orgKey = sys.Key;
                String newKey = "";
                string[] nel = orgKey.Split(new char[] { '/' });
                for (int i = 0; i < nel.Length; i++)
                {
                    if (i == delPoint) continue;
                    if (nel[i] == "") newKey = "";
                    else newKey = newKey + "/" + nel[i];
                }
                DataDelete(modelID, newKey, "System", false, false);
            }
            foreach (EcellObject node in nodeList)
            {
                String iNewKey = "";
                string[] iel = node.Key.Split(new char[] { '/' });
                for (int j = 0; j < iel.Length; j++)
                {
                    if (j == delPoint)
                    {
                        if (j == 1) iNewKey = "/";
                        iNewKey = iNewKey + iel[j].Substring(iel[j].LastIndexOf(":"));
                    }
                    else if (iel[j] == "") iNewKey = "";
                    else iNewKey = iNewKey + "/" + iel[j];
                }
                DataDelete(modelID, iNewKey, node.Type, false, false);
            }

            // Add a system.
            List<EcellObject> newlist = new List<EcellObject>();
            newlist.Add(obj);

            foreach (EcellObject sys in sysList)
                newlist.Add(sys);

            foreach (EcellObject node in nodeList)
                newlist.Add(node);

            DataAdd(newlist);

        }

        /// <summary>
        /// Deletes the "System" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="model">The model ID</param>
        /// <param name="key">The key of the "EcellObject"</param>
        /// <param name="messageFlag">The flag of the messages</param>
        private void DataDelete4System(string model, string key, bool messageFlag)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>> initialCondition = this.m_currentProject.InitialCondition;
            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;

            string message = "[" + model + "][" + key + "]";
            // Select systems for delete.
            List<EcellObject> delList = new List<EcellObject>();
            if (!sysDic.ContainsKey(model))
                return;
            foreach (EcellObject obj in sysDic[model])
            {
                if (obj.Key.Equals(key) || obj.Key.StartsWith(key + "/"))
                    delList.Add(obj);
            }

            //
            foreach (EcellObject obj in delList)
            {
                sysDic[model].Remove(obj);
                foreach (string keyParamID in initialCondition.Keys)
                {
                    foreach (string delModel in initialCondition[keyParamID].Keys)
                    {
                        foreach (string cType in initialCondition[keyParamID][delModel].Keys)
                        {
                            String delKey = cType + ":" + key;
                            List<String> delKeyList = new List<string>();
                            foreach (String entKey in initialCondition[keyParamID][delModel][cType].Keys)
                            {
                                if (entKey.StartsWith(delKey))
                                    delKeyList.Add(entKey);
                            }
                            foreach (String entKey in delKeyList)
                            {
                                initialCondition[keyParamID][delModel][cType].Remove(entKey);
                            }
                        }
                    }
                }
                // Record deletion of child system. 
                if (!obj.Key.Equals(key))
                    m_env.ActionManager.AddAction(new DataDeleteAction(obj.ModelID, obj.Key, obj.Type, obj, false));
                if (messageFlag)
                {
                    MessageDeleteEntity(EcellObject.SYSTEM, message);
                }
            }
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
            try
            {
                Debug.Assert(!String.IsNullOrEmpty(parameterID));
                message = "[" + parameterID + "]";
                //
                // Initializes.
                //

                this.SetDefaultDir();
                if (string.IsNullOrEmpty(m_defaultDir))
                {
                    throw new Exception(String.Format(MessageResources.ErrNoSet,
                        new object[] { MessageResources.NameWorkDir }));
                }

                Debug.Assert(m_currentProject.StepperDic.ContainsKey(parameterID));
                m_currentProject.StepperDic.Remove(parameterID);
                string simulationDirName
                        = this.m_defaultDir + Constants.delimiterPath
                        + m_currentProject.Name + Constants.delimiterPath + Constants.xpathParameters;
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
                m_env.PluginManager.ParameterDelete(m_currentProject.Name, parameterID);
                MessageDeleteEntity("Simulation Parameter", message);

                if (isRecorded)
                    m_env.ActionManager.AddAction(new DeleteSimParamAction(parameterID, isAnchor));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(MessageResources.ErrDelete,
                    new object[] { parameterID }), ex);
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
                if (m_currentProject.SimulationParam.Equals(parameterID))
                {
                    m_env.PluginManager.DataDelete(stepper.ModelID, stepper.Key, stepper.Type);
                }
            }
            catch (Exception ex)
            {
                String errmes = String.Format(MessageResources.ErrDelete,
                    new object[] { stepper.Key });
                Trace.WriteLine(errmes);
                throw new Exception(errmes, ex);
            }
        }

        /// <summary>
        /// Tests whether the full ID exists.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="fullID">The full ID</param>
        /// <returns>true if the full ID exists; false otherwise</returns>
        public bool Exists(string modelID, string fullID)
        {
            string[] infos = fullID.Split(Constants.delimiterColon.ToCharArray());
            Debug.Assert(infos.Length == 3, MessageResources.ErrInvalidID);
            Debug.Assert(infos[0].Equals(Constants.xpathSystem)
                    || infos[0].Equals(Constants.xpathProcess)
                    || infos[0].Equals(Constants.xpathVariable), MessageResources.ErrInvalidID);

            string key = null;
            if (infos[1].Equals("") && infos[2].Equals(Constants.delimiterPath))
            {
                key = infos[2];
            }
            else
            {
                key = infos[1] + Constants.delimiterColon + infos[2];
            }
            //
            // Checks the full ID.
            //
            List<EcellObject> systemList = m_currentProject.SystemDic[modelID];
            if (systemList == null || systemList.Count <= 0)
            {
                return false;
            }
            foreach (EcellObject system in systemList)
            {
                if (infos[0].Equals(Constants.xpathSystem))
                {
                    if (system.Type.Equals(infos[0]) && system.Key.Equals(key))
                    {
                        return true;
                    }
                }
                else
                {
                    if (system.Children == null
                            || system.Children.Count <= 0)
                    {
                        continue;
                    }
                    foreach (EcellObject entity in system.Children)
                    {
                        if (entity.Type.Equals(infos[0]) && entity.Key.Equals(key))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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
                Dictionary<string, List<EcellObject>> stepperDic = m_currentProject.StepperDic[m_currentProject.SimulationParam];

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
                throw new Exception(String.Format(MessageResources.ErrCreFile,
                    new object[] { fileName }), ex);
            }
        }

        /// <summary>
        /// Returns the current logger policy.
        /// </summary>
        /// <returns>The current logger policy</returns>
        private WrappedPolymorph GetCurrentLoggerPolicy()
        {
            List<WrappedPolymorph> policyList = new List<WrappedPolymorph>();
            string simParam = m_currentProject.SimulationParam;
            policyList.Add(new WrappedPolymorph(m_currentProject.LoggerPolicyDic[simParam].ReloadStepCount));
            policyList.Add(new WrappedPolymorph(m_currentProject.LoggerPolicyDic[simParam].ReloadInterval));
            policyList.Add(new WrappedPolymorph((int)m_currentProject.LoggerPolicyDic[simParam].DiskFullAction));
            policyList.Add(new WrappedPolymorph(m_currentProject.LoggerPolicyDic[simParam].MaxDiskSpace));
            return new WrappedPolymorph(policyList);
        }

        /// <summary>
        /// Returns the current simulation parameter ID.
        /// </summary>
        /// <returns>The current simulation parameter ID</returns>
        public string GetCurrentSimulationParameterID()
        {
            return m_currentProject.SimulationParam;
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
                    ecellObjectList.AddRange(m_currentProject.ModelList);
                    // Searches the "System".
                    m_currentProject.SortSystems();
                    foreach (List<EcellObject> systemList in sysDic.Values)
                    {
                        ecellObjectList.AddRange(systemList);
                    }
                }
                // Searches the model.
                else if (string.IsNullOrEmpty(key))
                {
                    foreach (EcellObject model in m_currentProject.ModelList)
                    {
                        if (!model.ModelID.Equals(modelID))
                            continue;
                        ecellObjectList.Add(model.Copy());
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
                        ecellObjectList.Add(system.Copy());
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
                return obj.Copy();
        }

        /// <summary>
        /// Call value of E-cell object.
        /// </summary>
        /// <param name="modelId">the modelId of EcellObject.</param>
        /// <param name="key">key of EcellObject</param>
        /// <param name="type">type of EcellObject</param>
        /// <param name="name">name of EcellData</param>
        /// <returns></returns>
        public string GetEcellData(string modelId, string key, string type, string name)
        {
            if (name == null)
                return null;

            EcellObject obj = GetEcellObject(modelId, key, type);

            if (obj == null)
                return null;

            foreach (EcellData ed in obj.Value)
            {
                if (ed.Name.Equals(name))
                {
                    return ed.Value.ToString();
                }
            }
            return null;
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
            DataStorer.DataStored4Stepper(simulator, stepperEcellObject);
            dic[Constants.xpathSystem] = systemEcellObject;
            dic[Constants.xpathStepper] = stepperEcellObject;
            return dic;
        }

        /// <summary>
        /// Returns the initial condition.
        /// </summary>
        /// <param name="paremterID">The parameter ID</param>
        /// <param name="modelID">The model ID</param>
        /// <returns>The initial condition</returns>
        public Dictionary<string, Dictionary<string, double>>
                GetInitialCondition(string paremterID, string modelID)
        {
            return this.m_currentProject.InitialCondition[paremterID][modelID];
        }

        /// <summary>
        /// Returns the initial condition.
        /// </summary>
        /// <param name="paremterID">The parameter ID</param>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The data type</param>
        /// <returns>The initial condition</returns>
        public Dictionary<string, double>
                GetInitialCondition(string paremterID, string modelID, string type)
        {
            return this.m_currentProject.InitialCondition[paremterID][modelID][type];
        }

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
                throw new Exception(MessageResources.ErrGetLogData, ex);
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

                WrappedPolymorph loggerList = m_currentProject.Simulator.GetLoggerList();
                if (!loggerList.IsList())
                    return logDataList;

                foreach (WrappedPolymorph logger in loggerList.CastToList())
                {
                    logDataList.Add(
                            this.GetUniqueLogData(startTime, endTime, interval, logger.CastToString()));
                }
                return logDataList;
            }
            catch (Exception ex)
            {
                logDataList = null;
                throw new Exception(MessageResources.ErrGetLogData, ex);
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
            this.Split4EntityPath(ref key, ref type, ref propName, fullID);
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
                WrappedPolymorph polymorphList = m_currentProject.Simulator.GetLoggerList();
                if (polymorphList.IsList())
                {
                    foreach (WrappedPolymorph polymorph in polymorphList.CastToList())
                    {
                        loggerList.Add(polymorph.CastToString());
                    }
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
        /// Returns the "LoggerPolicy".
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <returns>The "LoggerPolicy"</returns>
        public LoggerPolicy GetLoggerPolicy(string parameterID)
        {
            return m_currentProject.LoggerPolicyDic[parameterID];
        }

        /// <summary>
        /// Returns the next event.
        /// </summary>
        /// <returns>The current simulation time, The stepper</returns>
        public ArrayList GetNextEvent()
        {
            try
            {
                ArrayList list = new ArrayList();
                List<WrappedPolymorph> polymorphList
                        = m_currentProject.Simulator.GetNextEvent().CastToList();
                list.Add(polymorphList[0].CastToDouble());
                list.Add(polymorphList[1].CastToString());
                return list;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// Get the path of a directory which contains model-related files (ex: *.eml, *.leml)
        /// </summary>
        /// <param name="modelName">a model name</param>
        /// <returns>a directory path</returns>
        public string GetEMLPath(string modelName)
        {
            if (m_currentProject == null || m_currentProject.ModelFileDic == null)
                return null;

            if (m_currentProject.ModelFileDic.ContainsKey(modelName))
            {
                return m_currentProject.ModelFileDic[modelName];
            }
            return null;
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
                        string childPath = system.Name;
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
                throw new Exception(String.Format(MessageResources.ErrFindEnt,
                    new object[] { entityName }), ex);
            }
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
                throw new Exception(String.Format(MessageResources.ErrSimPropData,
                    new object[] { fullPN }), ex);
            }
        }

        /// <summary>
        /// get model list loaded by this system now.
        /// </summary>
        /// <returns></returns>
        public List<string> GetModelList()
        {
            List<EcellObject> objList = m_currentProject.ModelList;
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
        /// Returns the list of the "Process" DM.
        /// </summary>
        public List<string> GetProcessList()
        {
            m_currentProject.SetDMList();
            return m_currentProject.DmDic[Constants.xpathProcess];
        }

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
                WrappedSimulator sim = CreateSimulatorInstance();
                sim.CreateEntity(
                    dmName,
                    Constants.xpathProcess + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper());

                string fullPath = Constants.xpathProcess + Constants.delimiterColon +
                Constants.delimiterPath + Constants.delimiterColon +
                Constants.xpathSize.ToUpper() + Constants.delimiterColon + "CheckProperty";
                WrappedPolymorph newValue = EcellValue.CastToWrappedPolymorph4EcellValue(new EcellValue(0.01));
                sim.SetEntityProperty(fullPath, newValue);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return false;
            }
            return isEnable;
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
                WrappedSimulator sim = CreateSimulatorInstance();
                sim.CreateEntity(
                    dmName,
                    Constants.xpathProcess + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper());
                string key = Constants.delimiterPath + Constants.delimiterColon + Constants.xpathSize.ToUpper();
                EcellObject dummyEcellObject = EcellObject.CreateObject("", key, "", "", null);
                DataStorer.DataStored4Process(
                        sim,
                        dummyEcellObject,
                        new Dictionary<string, double>());
                SetPropertyList(dummyEcellObject, dic);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    String.Format(MessageResources.ErrGetProp,
                    new object[] { dmName }), ex);
            }
            return dic;
        }

        /// <summary>
        /// Returns the list of the "Project" with the directory name.
        /// </summary>
        /// <param name="dir">The directory name</param>
        /// <returns>The list of the "Project"</returns>
        public List<Project> GetProjects(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return null;
            }
            List<Project> list = new List<Project>();
            string[] dirList = Directory.GetDirectories(dir);
            if (dirList == null || dirList.Length <= 0)
            {
                return null;
            }
            for (int i = 0; i < dirList.Length; i++)
            {
                string prjFile = dirList[i] + Constants.delimiterPath + Constants.fileProject;
                if (File.Exists(prjFile))
                {
                    StreamReader reader = null;
                    try
                    {
                        reader = new StreamReader(prjFile, Encoding.UTF8);
                        list.Add(
                            new Project(
                                Path.GetFileName(dirList[i]),
                                reader.ReadLine(),
                                File.GetLastWriteTime(prjFile).ToString()
                            )
                        );
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        continue;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                }
            }
            if (list.Count > 0)
            {
                this.m_defaultDir = dir;
            }
            return list;
        }

        /// <summary>
        /// Returns the savable model ID.
        /// </summary>
        /// <returns>The savable model ID</returns>
        public List<string> GetSavableModel()
        {
            if (m_currentProject.ModelList != null
                && m_currentProject.ModelList.Count > 0)
            {
                List<string> modelIDList = new List<string>();
                foreach (EcellObject model in m_currentProject.ModelList)
                {
                    modelIDList.Add(model.ModelID);
                }
                return modelIDList;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the savable project ID.
        /// </summary>
        /// <returns>The savable project ID</returns>
        public string GetSavableProject()
        {
            return m_currentProject.Name;
        }

        /// <summary>
        /// Returns the savable simulation parameter ID.
        /// </summary>
        /// <returns>The savable simulation parameter ID</returns>
        public List<string> GetSavableSimulationParameter()
        {
            Debug.Assert(m_currentProject.LoggerPolicyDic != null);
            List<string> prmIDList = new List<string>();
            foreach (string prmID in m_currentProject.LoggerPolicyDic.Keys)
            {
                prmIDList.Add(prmID);
            }
            return prmIDList;
        }

        /// <summary>
        /// Returns the savable simulation result.
        /// </summary>
        /// <returns>The savable simulation result</returns>
        public string GetSavableSimulationResult()
        {
            return Constants.xpathParameters + Constants.xpathResult;
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
                parameterID = m_currentProject.SimulationParam;
            if (string.IsNullOrEmpty(parameterID))
                throw new Exception(String.Format(MessageResources.ErrNoSet,
                    new object[] { MessageResources.NameSimParam }));

            List<EcellObject> tempList = m_currentProject.StepperDic[parameterID][modelID];
            foreach (EcellObject stepper in tempList)
            {
                // DataStored4Stepper(simulator, stepper);
                returnedStepper.Add(stepper.Copy());
            }
            return returnedStepper;
        }

        /// <summary>
        /// Returns the list of the parameter ID with the model ID.
        /// </summary>
        /// <returns>The list of parameter ID</returns>
        public List<string> GetSimulationParameterIDs()
        {
            if (m_currentProject.StepperDic == null)
                return new List<string>();

            return new List<string>(m_currentProject.StepperDic.Keys);
        }

        /// <summary>
        /// Returns the list of the "Stepper" DM.
        /// </summary>
        /// <returns>The list of the "Stepper" DM</returns>
        public List<string> GetStepperList()
        {
            List<string> stepperList = new List<string>();
            WrappedSimulator sim = CreateSimulatorInstance();
            foreach (WrappedPolymorph polymorph in sim.GetDMInfo().CastToList())
            {
                List<WrappedPolymorph> dmInfoList = polymorph.CastToList();
                if (dmInfoList[0].CastToString().Equals(Constants.xpathStepper))
                {
                    stepperList.Add(dmInfoList[1].CastToString());
                }
            }
            if (m_currentProject.DmDic != null)
                stepperList.AddRange(m_currentProject.DmDic[Constants.xpathStepper]);
            stepperList.Sort();
            return stepperList;
        }

        /// <summary>
        /// Get the dm file and the source file for dm in the directory of current project.
        /// </summary>
        /// <returns>The list of dm in the directory of current project.</returns>
        public List<string> GetDMDirData()
        {
            List<string> resultList = new List<string>();
            string path = Path.Combine(m_currentProject.ProjectPath, Constants.DMDirName);
            if (!Directory.Exists(path))
                return resultList;

            string[] files = Directory.GetFiles(path, "*" + Constants.FileExtDM);
            for (int i = 0; i < files.Length; i++)
            {
                string name = Path.GetFileNameWithoutExtension(files[i]);
                if (!resultList.Contains(name))
                    resultList.Add(name);
            }
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
            return Path.Combine(m_currentProject.ProjectPath, Constants.DMDirName);
        }

        private string GetParameterDir()
        {
            return Path.Combine(m_currentProject.ProjectPath, Constants.ParameterDirName);
        }

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
                    string logdata = paramName + Path.PathSeparator + Path.GetFileName(pfiles[j]);
                    result.Add(logdata);
                }

                for (int j = 0; j < vfiles.Length; j++)
                {
                    string logdata = paramName + Path.PathSeparator + Path.GetFileName(vfiles[j]);
                    result.Add(logdata);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public string GetDMFileName(string indexName)
        {
            string path = Path.Combine(m_currentProject.ProjectPath, Constants.DMDirName);
            path = Path.Combine(path, indexName + Constants.FileExtSource);
            if (!File.Exists(path))
                return null;
            return path;
        }

        /// <summary>
        /// Returns the list of the "Stepper" property. 
        /// </summary>
        /// <param name="dmName">The DM name</param>
        /// <returns>The dictionary of the "Stepper" property</returns>
        public Dictionary<string, EcellData> GetStepperProperty(string dmName)
        {
            Dictionary<string, EcellData> dic = new Dictionary<string, EcellData>();
            EcellObject dummyEcellObject = null;
            try
            {
                WrappedSimulator sim = CreateSimulatorInstance();
                sim.CreateStepper(dmName, Constants.textKey);
                dummyEcellObject = EcellObject.CreateObject("", Constants.textKey, "", "", null);
                DataStorer.DataStored4Stepper(sim, dummyEcellObject);
                SetPropertyList(dummyEcellObject, dic);
            }
            finally
            {
                dummyEcellObject = null;
            }
            return dic;
        }

        /// <summary>
        /// Returns the list of the "System" DM.
        /// </summary>
        /// <returns>The list of the "System" DM</returns>
        public List<string> GetSystemList()
        {
            return m_currentProject.DmDic[Constants.xpathSystem];
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
        /// Returns the list of the "System" property. 
        /// </summary>
        /// <returns>The dictionary of the "System" property</returns>
        public Dictionary<string, EcellData> GetSystemProperty()
        {
            Dictionary<string, EcellData> dic = new Dictionary<string, EcellData>();
            WrappedSimulator sim = CreateSimulatorInstance();
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
                "",
                "",
                null);
            DataStorer.DataStored4System(
                sim,
                dummyEcellObject,
                new Dictionary<string, double>());
            SetPropertyList(dummyEcellObject, dic);
            return dic;
        }

        /// <summary>
        /// Returns the list of the "Variable" DM.
        /// </summary>
        /// <returns>The list of the "Variable" DM</returns>
        public List<string> GetVariableList()
        {
            return m_currentProject.DmDic[Constants.xpathVariable];
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
                simulator = CreateSimulatorInstance();
                BuildDefaultSimulator(simulator, null, null);
                dummyEcellObject = EcellObject.CreateObject(
                    "",
                    Constants.delimiterPath + Constants.delimiterColon + Constants.xpathSize.ToUpper(),
                    "",
                    "",
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
        /// Tests whether the "EcellObject" is usable.
        /// </summary>
        /// <param name="ecellObject">The tested "EcellObject"</param>
        /// <returns>true if the "EcellObject" is usable; false otherwise</returns>
        private bool IsUsable(EcellObject ecellObject)
        {
            bool flag = false;
            if (ecellObject == null)
            {
                return flag;
            }
            else if (ecellObject.ModelID == null || ecellObject.ModelID.Length < 0)
            {
                return flag;
            }
            else if (ecellObject.Type == null || ecellObject.Type.Length < 0)
            {
                return flag;
            }
            //
            // 4 "Process", "Stepper", "System" and "Variable"
            //
            if (!ecellObject.Type.Equals(Constants.xpathProject) && !ecellObject.Type.Equals(Constants.xpathModel))
            {
                if (ecellObject.Key == null || ecellObject.Key.Length < 0)
                {
                    return flag;
                }
                else if (ecellObject.Classname == null || ecellObject.Classname.Length < 0)
                {
                    return flag;
                }
            }
            return true;
        }

        /// <summary>
        /// Initialize the simulator before it starts.
        /// </summary>
        public void Initialize(bool flag)
        {
            string simParam = m_currentProject.SimulationParam;
            Dictionary<string, List<EcellObject>> stepperList = m_currentProject.StepperDic[simParam];
            WrappedSimulator simulator = null;
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> initialCondition = m_currentProject.InitialCondition[simParam];

            try
            {
                m_currentProject.Simulator = CreateSimulatorInstance();
                simulator = m_currentProject.Simulator;
                //
                // Loads steppers on the simulator.
                //
                List<EcellObject> newStepperList = new List<EcellObject>();
                List<string> modelIDList = new List<string>();
                Dictionary<string, Dictionary<string, WrappedPolymorph>> setStepperPropertyDic
                    = new Dictionary<string, Dictionary<string, WrappedPolymorph>>();
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
                Dictionary<string, WrappedPolymorph> setSystemPropertyDic = new Dictionary<string, WrappedPolymorph>();
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
                    foreach (string type
                        in initialCondition[modelID].Keys)
                    {
                        foreach (string fullPN
                            in initialCondition[modelID]
                                [type].Keys)
                        {
                            EcellValue storedValue = new EcellValue(simulator.GetEntityProperty(fullPN));
                            double initialValue = initialCondition[modelID][type][fullPN];
                            WrappedPolymorph newValue = null;
                            if (storedValue.IsInt)
                            {
                                int initialValueInt = Convert.ToInt32(initialValue);
                                if (storedValue.CastToInt().Equals(initialValueInt))
                                {
                                    continue;
                                }
                                newValue
                                    = EcellValue.CastToWrappedPolymorph4EcellValue(new EcellValue(initialValueInt));
                            }
                            else
                            {
                                if (storedValue.CastToDouble().Equals(initialValue))
                                {
                                    continue;
                                }
                                newValue
                                    = EcellValue.CastToWrappedPolymorph4EcellValue(new EcellValue(initialValue));
                            }
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
                if (allLoggerList != null && allLoggerList.Count > 0)
                {
                    WrappedPolymorph loggerPolicy = this.GetCurrentLoggerPolicy();
                    foreach (string logger in allLoggerList)
                    {
                        simulator.CreateLogger(logger, loggerPolicy);
                    }
                }
                //
                // Messages
                //
                Trace.WriteLine("Initialize the simulator:");
            }
            catch (Exception ex)
            {
                throw new Exception(MessageResources.ErrInitSim, ex);
            }
        }

        /// <summary>
        /// Checks whether the simulator is running.
        /// </summary>
        /// <returns>true if the simulator is running; false otherwise</returns>
        public bool IsActive()
        {
            bool runningFlag = false;
            if (m_currentProject != null && m_currentProject.SimulationStatus == SimulationStatus.Run)
                runningFlag = true;

            return runningFlag;
        }

        /// <summary>
        /// Tests whether the "EcellData" is usable.
        /// </summary>
        /// <param name="ecellData">The tested "EcellData"</param>
        /// <returns>true if the "EcellData" is usable; false otherwise</returns>
        private bool IsUsable(EcellData ecellData)
        {
            bool flag = false;
            if (ecellData == null)
            {
                return flag;
            }
            else if (ecellData.Name == null || ecellData.Name.Length <= 0)
            {
                return flag;
            }
            else if (ecellData.Value == null)
            {
                return flag;
            }
            else if (ecellData.EntityPath == null || ecellData.EntityPath.Length <= 0)
            {
                return flag;
            }
            return true;
        }

        /// <summary>
        /// Loads the project.
        /// </summary>
        /// <param name="filename">Project file or EML file.</param>
        public void LoadProject(string filename)
        {
            try
            {

                Project project = new Project(filename);
                LoadProject(project);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(MessageResources.ErrLoadPrj,
                    new object[] { filename }), ex);
            }

        }
        /// <summary>
        /// Loads the project.
        /// </summary>
        /// <param name="projectID">The load project ID</param>
        /// <param name="prjFile">The load project file.</param>
        public void LoadProject(string projectID, string prjFile)
        {
            Project project = null;
            try
            {
                // Initializes.
                project = new Project(prjFile);
                if (project == null)
                    throw new Exception(MessageResources.ErrFindFile + " [" + Constants.fileProject + "]");

                project.Name = projectID;
                LoadProject(project);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(MessageResources.ErrLoadPrj,
                    new object[] { projectID }), ex);
            }
        }

        /// <summary>
        /// LoadProject
        /// </summary>
        /// <param name="project"></param>
        public void LoadProject(Project project)
        {
            List<EcellObject> passList = new List<EcellObject>();
            string[] parameters = new string[0];
            string message = null;
            string projectID = null;
            try
            {
                if (m_currentProject != null)
                    CloseProject(null);
                if (project == null)
                    throw new Exception(MessageResources.ErrLoadPrj);

                // Initializes.
                message = "[" + project.Name + "]";
                m_currentProject = project;
                projectID = project.Name;

                project.SetDMList();
                project.Simulator = CreateSimulatorInstance();
                project.LoggerPolicyDic = new Dictionary<string, LoggerPolicy>();
                project.StepperDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                project.ModelList = new List<EcellObject>();
                project.SystemDic = new Dictionary<string, List<EcellObject>>();

                m_projectList.Add(project);

                m_env.PluginManager.ParameterSet(projectID, project.SimulationParam);

                List<EcellData> ecellDataList = new List<EcellData>();
                ecellDataList.Add(new EcellData(Constants.textComment, new EcellValue(project.Comment), null));
                passList.Add(EcellObject.CreateObject(projectID, "", Constants.xpathProject, "", ecellDataList));

                // Loads the model.
                string[] models;
                if (project.FilePath.EndsWith(Constants.FileExtEML))
                {
                    models = new string[] { project.FilePath };
                }
                else
                {
                    string modelDirName = Path.Combine(project.ProjectPath, Constants.xpathModel);
                    Debug.Assert(Directory.Exists(modelDirName));

                    models = Directory.GetFileSystemEntries(
                        modelDirName,
                        Constants.delimiterWildcard + Constants.FileExtEML
                        );
                    Debug.Assert(models != null && models.Length > 0);
                }
                foreach (string model in models)
                {
                    string fileName = Path.GetFileName(model);
                    if (fileName.IndexOf(Constants.delimiterUnderbar) > 0 ||
                        fileName.EndsWith(Constants.FileExtBackUp))
                        continue;
                    this.LoadModel(model, false);
                }
                passList.AddRange(m_currentProject.ModelList);
                foreach (string storedModelID in m_currentProject.SystemDic.Keys)
                {
                    passList.AddRange(m_currentProject.SystemDic[storedModelID]);
                }

                // Loads the simulation parameter.
                string simulationDirName = Path.Combine(project.ProjectPath, Constants.xpathParameters);

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
                                LoadSimulationParameter(parameter);
                            }
                        }
                    }
                }
                Trace.WriteLine("Load project: " + message);
            }
            catch (Exception ex)
            {
                passList = null;
                CloseProject(null);
                throw new Exception(String.Format(MessageResources.ErrLoadPrj,
                    new object[] { projectID }), ex);
            }
            finally
            {
                if (passList != null && passList.Count > 0)
                {
                    this.m_env.PluginManager.DataAdd(passList);
                }
                foreach (string paramID in this.GetSimulationParameterIDs())
                {
                    this.m_env.PluginManager.ParameterAdd(projectID, paramID);
                }
                m_env.ActionManager.AddAction(new LoadProjectAction(projectID, project.FilePath));
                m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
            }
        }

        /// <summary>
        /// Loads the eml formatted file and returns the model ID.
        /// </summary>
        /// <param name="filename">The eml formatted file name</param>
        /// <param name="isLogging">The flag whether this function is in logging.</param>
        /// <returns>The model ID</returns>
        public string LoadModel(string filename, bool isLogging)
        {
            string message = null;
            try
            {
                message = "[" + filename + "]";
                //
                // To load
                //
                string modelID = null;
                if (m_currentProject.FilePath == null)
                {
                    m_currentProject.FilePath = filename;
                }
                if (m_currentProject.Simulator == null)
                {
                    m_currentProject.SetDMList();
                    m_currentProject.Simulator = CreateSimulatorInstance();
                }
                EcellObject modelObj = EmlReader.Parse(filename,
                        m_currentProject.Simulator);
                modelID = modelObj.ModelID;

                //
                // Checks the old model ID
                //
                foreach (EcellObject model in m_currentProject.ModelList)
                {
                    Debug.Assert(!model.ModelID.Equals(modelID));
                }
                //
                // Initialize
                //
                m_currentProject.Simulator.Initialize();
                // Sets initial conditions.
                m_currentProject.Initialize(modelID);
                string simParam = m_currentProject.SimulationParam;
                InitializeModel(modelObj);
                // Stores the "LoggerPolicy"
                if (!m_currentProject.LoggerPolicyDic.ContainsKey(simParam))
                {
                    m_currentProject.LoggerPolicyDic[simParam] = new LoggerPolicy();
                }
                Trace.WriteLine("Load Model: " + message);
                if (isLogging)
                    m_env.ActionManager.AddAction(new ImportModelAction(filename));
                if (m_currentProject.ModelFileDic.ContainsKey(modelID))
                    m_currentProject.ModelFileDic.Remove(modelID);
                m_currentProject.ModelFileDic.Add(modelID, filename);

                return modelID;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(MessageResources.ErrLoadModel,
                    new object[] { filename }), ex);
            }
        }

        private void InitializeModel(EcellObject ecellObject)
        {
            DataStorer.DataStored(
                m_currentProject.Simulator,
                ecellObject,
                m_currentProject.InitialCondition[m_currentProject.SimulationParam][ecellObject.ModelID]);
            // Sets the "EcellObject".
            string modelID = ecellObject.ModelID;
            string simParam = m_currentProject.SimulationParam;
            if (ecellObject.Type.Equals(Constants.xpathModel))
            {
                m_currentProject.ModelList.Add(ecellObject);
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

        /// <summary>
        /// Loads the simulation parameter.
        /// </summary>
        /// <param name="fileName">The simulation parameter file name</param>
        public void LoadSimulationParameter(string fileName)
        {
            string message = null;
            string projectID = m_currentProject.Name;
            try
            {
                message = "[" + fileName + "]";
                // Initializes
                Debug.Assert(!string.IsNullOrEmpty(fileName));
                // Parses the simulation parameter.
                SimulationParameter simParam = SimulationParameterReader.Parse(
                        fileName, m_currentProject.Simulator);
                string simParamID = simParam.ID;
                // Stores the simulation parameter.
                if (!m_currentProject.SimulationParam.Equals(simParamID))
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
                                        newData.Value = GetEcellValue(newData);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            newData.Value = new EcellValue(
                                                Convert.ToInt32(
                                                    newData.Value.CastToList()[0].ToString()
                                                )
                                            );
                                        }
                                        catch (Exception ex)
                                        {
                                            Trace.WriteLine(ex);
                                            // do nothing
                                        }
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
                Trace.WriteLine("Load Simulation Parameter: " + message);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(MessageResources.ErrLoadPrj,
                    new object[] { projectID }), ex);
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
                string newValue = data.Value.CastToList()[0].ToString();
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
        /// Loads the "Stepper" 2 the "EcellCoreLib".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="stepperList">The list of the "Stepper"</param>
        /// <param name="setStepperDic"></param>
        private static void LoadStepper(
            WrappedSimulator simulator,
            List<EcellObject> stepperList,
            Dictionary<string, Dictionary<string, WrappedPolymorph>> setStepperDic)
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
                    EcellValue velue = ecellData.Value;
                    try
                    {
                        string value = velue.ToString().Replace("(", "").Replace(")", "").Replace("\"", "");
                        if (value.Equals(Double.PositiveInfinity.ToString()))
                            continue;
                        else if (value.Equals(Double.MaxValue.ToString()))
                            continue;

                        XmlConvert.ToDouble(value);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                        continue;
                    }

                    if (velue.IsDouble
                        && (Double.IsInfinity(velue.CastToDouble()) || Double.IsNaN(velue.CastToDouble())))
                        continue;

                    if (ecellData.Saveable)
                    {
                        simulator.LoadStepperProperty(
                            stepper.Key,
                            ecellData.Name,
                            EcellValue.CastToWrappedPolymorph4EcellValue(velue));
                    }
                    else if (ecellData.Settable)
                    {
                        if (!setStepperDic.ContainsKey(stepper.Key))
                        {
                            setStepperDic[stepper.Key] = new Dictionary<string, WrappedPolymorph>();
                        }
                        setStepperDic[stepper.Key][ecellData.Name]
                            = EcellValue.CastToWrappedPolymorph4EcellValue(velue);
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
            Dictionary<string, Dictionary<string, double>> initialCondition,
            Dictionary<string, WrappedPolymorph> setPropertyDic)
        {
            Debug.Assert(systemList != null && systemList.Count > 0);

            bool existSystem = false;
            Dictionary<string, WrappedPolymorph> processPropertyDic = new Dictionary<string, WrappedPolymorph>();

            foreach (EcellObject system in systemList)
            {
                if (system == null)
                    continue;

                existSystem = true;
                string parentPath = system.ParentSystemID;
                string childPath = system.Name;
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
                            EcellValue.CastToWrappedPolymorph4EcellValue(value));
                    }
                    else if (ecellData.Settable)
                    {
                        setPropertyDic[ecellData.EntityPath]
                            = EcellValue.CastToWrappedPolymorph4EcellValue(value);
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
            Dictionary<string, WrappedPolymorph> processPropertyDic,
            Dictionary<string, Dictionary<string, double>> initialCondition,
            Dictionary<string, WrappedPolymorph> setPropertyDic)
        {
            if (entityList == null || entityList.Count <= 0)
                return;

            try
            {
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
                                || (value.IsString && value.CastToString().Length == 0))
                        {
                            continue;
                        }

                        if (ecellData.Logged)
                        {
                            loggerList.Add(ecellData.EntityPath);
                        }

                        if (value.IsDouble
                            && (Double.IsInfinity(value.CastToDouble()) || Double.IsNaN(value.CastToDouble())))
                        {
                            continue;
                        }
                        if (ecellData.Saveable)
                        {
                            if (ecellData.EntityPath.EndsWith(Constants.xpathVRL))
                            {
                                processPropertyDic[ecellData.EntityPath]
                                    = EcellValue.CastToWrappedPolymorph4EcellValue(value);
                            }
                            else
                            {
                                if (ecellData.EntityPath.EndsWith("FluxDistributionList"))
                                    continue;
                                simulator.LoadEntityProperty(
                                    ecellData.EntityPath,
                                    EcellValue.CastToWrappedPolymorph4EcellValue(value));
                            }
                        }
                        else if (ecellData.Settable)
                        {
                            setPropertyDic[ecellData.EntityPath]
                                = EcellValue.CastToWrappedPolymorph4EcellValue(value);
                        }
                    }
                }
            }
            catch (WrappedException e)
            {
                throw new Exception("Failed to create entity", e);
            }
        }

        /// <summary>
        /// Create the default object(Process, Variable and System).
        /// </summary>
        /// <param name="modelID">the model ID of created object.</param>
        /// <param name="key">the system path of parent object.</param>
        /// <param name="type">the type of created object.</param>
        /// <param name="isProper">the flag whether the create object is propergated.</param>
        /// <returns>the create object.</returns>
        public EcellObject CreateDefaultObject(string modelID, string key, string type, bool isProper)
        {
            try
            {
                if (type.Equals(Constants.xpathSystem))
                {
                    return CreateDefaultSystem(modelID, key, isProper);
                }
                else if (type.Equals(Constants.xpathProcess))
                {
                    return CreateDefaultProcess(modelID, key, isProper);
                }
                else if (type.Equals(Constants.xpathVariable))
                {
                    return CreateDefaultVariable(modelID, key, isProper);
                }
                else if (type.Equals(Constants.xpathText))
                {
                    string nodeKey = GetTemporaryID(modelID, EcellObject.TEXT, "/");
                    EcellText text = new EcellText(modelID, nodeKey, EcellObject.TEXT, EcellObject.TEXT, new List<EcellData>());
                    text.Comment = text.Name;
                    return text;
                }
                return null;
            }
            catch (Exception ex)
            {
                String message = String.Format(MessageResources.ErrAdd,
                    new object[] { type, key });
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Create the process with temporary ID.
        /// </summary>
        /// <param name="modelID">model ID of created object.</param>
        /// <param name="key">the key of parent system object.</param>
        /// <param name="isProper">the flag whether the create object is propergated.</param>
        /// <returns>the create object.</returns>
        private EcellObject CreateDefaultProcess(string modelID, string key, bool isProper)
        {
            String tmpID = GetTemporaryID(modelID,
                Constants.xpathProcess, key);
            EcellObject sysobj = GetEcellObject(modelID, key, Constants.xpathSystem);
            if (sysobj == null) return null;
            String stepperID = "";
            foreach (EcellData d in sysobj.Value)
            {
                if (!d.Name.Equals(Constants.xpathStepperID)) continue;
                stepperID = d.Value.ToString();
            }

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
                    d.Value = new EcellValue(1);
                }
                data.Add(d);
            }
            EcellObject obj = EcellObject.CreateObject(modelID, tmpID,
                Constants.xpathProcess, Constants.DefaultProcessName, data);

            if (isProper)
            {
                List<EcellObject> rList = new List<EcellObject>();
                rList.Add(obj);
                DataAdd(rList);
                m_env.PluginManager.SelectChanged(modelID, tmpID, Constants.xpathProcess);
            }
            return obj;
        }

        /// <summary>
        /// Create the variable with temporary ID.
        /// </summary>
        /// <param name="modelID">model ID of created object.</param>
        /// <param name="key">the key of parent system object.</param>
        /// <param name="isProper">the flag whether the create object is propergated.</param>
        /// <returns>the create object.</returns>
        private EcellObject CreateDefaultVariable(string modelID, string key, bool isProper)
        {
            String tmpID =
                GetTemporaryID(modelID, Constants.xpathVariable, key);

            Dictionary<string, EcellData> list = GetVariableProperty();
            List<EcellData> data = new List<EcellData>();
            foreach (EcellData d in list.Values)
            {
                data.Add(d);
            }
            EcellObject obj = EcellObject.CreateObject(modelID, tmpID,
                Constants.xpathVariable, Constants.xpathVariable, data);

            if (isProper)
            {
                List<EcellObject> rList = new List<EcellObject>();
                rList.Add(obj);
                DataAdd(rList);
                m_env.PluginManager.SelectChanged(modelID, tmpID, Constants.xpathVariable);
            }
            return obj;
        }

        /// <summary>
        /// Create the system with temporary ID.
        /// </summary>
        /// <param name="modelID">model ID of created object.</param>
        /// <param name="key">the key of parent system object.</param>
        /// <param name="isProper">the flag whether the create object is propergated.</param>
        /// <returns>the create object.</returns>
        private EcellObject CreateDefaultSystem(string modelID, string key, bool isProper)
        {
            String tmpID =
                GetTemporaryID(modelID, Constants.xpathSystem, key);

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

            if (isProper)
            {
                List<EcellObject> rList = new List<EcellObject>();
                rList.Add(obj);
                DataAdd(rList);
                m_env.PluginManager.SelectChanged(modelID, tmpID, Constants.xpathSystem);
            }
            return obj;
        }

        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="modelID"></param>
        /// <param name="comment"></param>
        /// <param name="setDirList"></param>
        public void CreateNewProject(string projectID, string modelID, string comment, IEnumerable<String> setDirList)
        {
            try
            {
                CreateProject(projectID, modelID, comment, setDirList);
                List<EcellObject> list = new List<EcellObject>();
                list.Add(EcellObject.CreateObject(modelID, null, Constants.xpathModel, null, null));
                DataAdd(list);
                foreach (string paramID in GetSimulationParameterIDs())
                {
                    m_env.PluginManager.ParameterAdd(projectID, paramID);
                }
                m_env.PluginManager.ParameterSet(CurrentProjectID, GetCurrentSimulationParameterID());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
                CloseProject(projectID);
            }

        }
        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="projectID">The "Project" ID</param>
        /// <param name="comment">The comment</param>
        /// <param name="projectPath">The project directory path to load the dm of this project.</param>
        /// <param name="setDirList">The list of dm directory.</param>
        public void CreateProject(string projectID, string comment, string projectPath,
            IEnumerable<String> setDirList)
        {
            Project prj = null;
            try
            {
                //
                // Closes the current project.
                //
                if (m_currentProject != null)
                {
                    this.CloseProject(null);
                }
                //
                // Initialize
                //
                prj = new Project(projectID, comment, DateTime.Now.ToString());
                m_projectList.Add(prj);
                m_currentProject = prj;
                if (projectPath != null)
                    m_currentProject.ProjectPath = projectPath;

                CreateProjectDir(projectID, setDirList);
                m_currentProject.SetDMList();
                m_currentProject.Simulator = CreateSimulatorInstance();
                m_currentProject.LoggerPolicyDic = new Dictionary<string, LoggerPolicy>();
                m_currentProject.StepperDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                m_currentProject.ModelList = new List<EcellObject>();
                m_currentProject.SystemDic = new Dictionary<string, List<EcellObject>>();
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

                Trace.WriteLine("Create Project: [" + projectID + "]");
            }
            catch (Exception ex)
            {
                if (prj != null)
                {
                    if (this.m_projectList.Contains(prj))
                    {
                        this.m_projectList.Remove(prj);
                        prj = null;
                    }
                }
                string message = String.Format(
                        MessageResources.ErrCrePrj,
                        new object[] { projectID });
                Trace.WriteLine(message);
                throw new Exception(message, ex);
            }
        }

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
                string storedParameterID = null;
                if (!m_currentProject.StepperDic.ContainsKey(parameterID))
                {
                    //
                    // Searches the source stepper.
                    //
                    if (m_currentProject.SimulationParam != null && m_currentProject.SimulationParam.Length > 0)
                    {
                        storedParameterID = m_currentProject.SimulationParam;
                    }
                    else
                    {
                        Debug.Assert(m_currentProject.StepperDic != null &&
                            m_currentProject.StepperDic.Count > 0);

                        foreach (string key in m_currentProject.StepperDic.Keys)
                        {
                            storedParameterID = key;
                            break;
                        }
                    }
                    //
                    // Sets the destination stepper.
                    //
                    m_currentProject.StepperDic[parameterID]
                            = new Dictionary<string, List<EcellObject>>();

                    foreach (string key in m_currentProject.StepperDic[storedParameterID].Keys)
                    {
                        m_currentProject.StepperDic[parameterID][key] = new List<EcellObject>();
                        foreach (EcellObject stepper in m_currentProject.StepperDic[storedParameterID][key])
                        {
                            m_currentProject.StepperDic[parameterID][key]
                                    .Add(stepper.Copy());
                        }
                    }
                }
                else
                {
                    throw new Exception(
                        String.Format(MessageResources.ErrExistObj,
                        new object[] { parameterID }));
                }
                //
                // 4 LoggerPolicy
                //

                LoggerPolicy loggerPolicy = m_currentProject.LoggerPolicyDic[storedParameterID];

                m_currentProject.LoggerPolicyDic[parameterID]
                    = new LoggerPolicy(
                        loggerPolicy.ReloadStepCount,
                        loggerPolicy.ReloadInterval,
                        loggerPolicy.DiskFullAction,
                        loggerPolicy.MaxDiskSpace);
                //
                // 4 Initial Condition
                //

                Dictionary<string, Dictionary<string, Dictionary<string, double>>> srcInitialCondition
                    = m_currentProject.InitialCondition[storedParameterID];
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> dstInitialCondition
                    = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();

                Copy4InitialCondition(srcInitialCondition, dstInitialCondition);

                m_currentProject.InitialCondition[parameterID] = dstInitialCondition;

                m_env.PluginManager.ParameterAdd(m_currentProject.Name, parameterID);


                Trace.WriteLine(String.Format(MessageResources.InfoCreSim,
                    new object[] { parameterID }));
                if (isRecorded)
                    m_env.ActionManager.AddAction(new NewSimParamAction(parameterID, isAnchor));
            }
            catch (Exception ex)
            {
                string message = String.Format(MessageResources.ErrCreSimParam,
                    new object[] { parameterID });
                Trace.WriteLine(message);
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Copy the dm data from the set directory to the project directory.
        /// </summary>
        /// <param name="projectID">Project ID.</param>
        /// <param name="dmList">The list of set dm directory.</param>
        private void CopyDmData(string projectID, List<string> dmList)
        {
            string baseDir = this.m_defaultDir + Constants.delimiterPath + projectID;
            string dmDir = baseDir + Constants.delimiterPath + Constants.DMDirName;

            foreach (string dmPath in dmList)
            {
                string[] dmData = Directory.GetFiles(dmPath, "*.dll");
                if (dmData == null) continue;
                for (int i = 0; i < dmData.Length; i++)
                {
                    dmData[i].ToString();
                }
            }
        }

        /// <summary>
        /// Create the project directory.
        /// </summary>
        /// <param name="projectID">Project ID.</param>
        /// <param name="dmList">A list of DM.</param>
        public void CreateProjectDir(string projectID, IEnumerable<string> dmList)
        {
            SetDefaultDir();
            string baseDir = this.m_defaultDir + Constants.delimiterPath + projectID;
            string modelDir = baseDir + Constants.delimiterPath + Constants.xpathModel;
            string dmDir = baseDir + Constants.delimiterPath + Constants.DMDirName;
            string paramDir = baseDir + Constants.delimiterPath + Constants.xpathParameters;

            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }
            if (!Directory.Exists(modelDir))
            {
                Directory.CreateDirectory(modelDir);
            }
            if (!Directory.Exists(dmDir))
            {
                Directory.CreateDirectory(dmDir);
            }
            foreach (string sourceDirName in dmList)
            {
                Util.CopyDirectory(sourceDirName, dmDir);
            }
        }


        /// <summary>
        /// Saves the model using the model ID.
        /// </summary>
        /// <param name="modelID">The saved model ID</param>
        public void SaveModel(string modelID)
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
                this.SetDefaultDir();
                if (string.IsNullOrEmpty(m_defaultDir))
                {
                    throw new Exception(String.Format(MessageResources.ErrNoSet,
                        new object[] { MessageResources.NameWorkDir }));
                }
                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + m_currentProject.Name))
                {
                    this.SaveProject(m_currentProject.Name);
                }
                string modelDirName
                    = this.m_defaultDir + Constants.delimiterPath +
                    m_currentProject.Name + Constants.delimiterPath + Constants.xpathModel;
                if (!Directory.Exists(modelDirName))
                {
                    Directory.CreateDirectory(modelDirName);
                }
                string modelFileName
                    = modelDirName + Constants.delimiterPath + modelID + Constants.delimiterPeriod + Constants.xpathEml;
                //
                // Picks the "Stepper" up.
                //
                List<EcellObject> stepperList
                    = m_currentProject.StepperDic[m_currentProject.SimulationParam][modelID];
                Debug.Assert(stepperList != null && stepperList.Count > 0);
                storedList.AddRange(stepperList);
                //
                // Picks the "System" up.
                //
                List<EcellObject> systemList = m_currentProject.SystemDic[modelID];
                Debug.Assert(systemList != null && systemList.Count > 0);
                storedList.AddRange(systemList);
                //
                // Creates.
                //
                EmlWriter.Create(modelFileName, storedList, true);
                Trace.WriteLine("Save Model: " + message);
                //
                // 4 Project
                //
                this.SaveProject(m_currentProject.Name);
                m_env.PluginManager.SaveModel(modelID, modelDirName);
            }
            catch (Exception ex)
            {
                storedList = null;
                message = String.Format(MessageResources.ErrSaveModel,
                    new object[] { modelID });
                Trace.WriteLine(message);
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Get Project.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private Project GetProject(string projectID)
        {
            Debug.Assert(!string.IsNullOrEmpty(projectID));
            Debug.Assert(this.m_projectList != null && this.m_projectList.Count > 0);

            foreach (Project prj in this.m_projectList)
                if (prj.Name.Equals(projectID))
                    return prj;

            return null;
        }
        /// <summary>
        /// Saves only the project using the project ID.
        /// </summary>
        /// <param name="projectID">The saved project ID</param>
        private void SaveProject(string projectID)
        {
            string message = null;
            Project thisPrj = null;
            try
            {
                // Initializes
                message = String.Format(MessageResources.InfoSavePrj,
                    new object[] { projectID }); ;
                thisPrj = GetProject(projectID);
                this.SetDefaultDir();
                if (string.IsNullOrEmpty(m_defaultDir))
                    throw new Exception(String.Format(MessageResources.ErrNoSet,
                        new object[] { MessageResources.NameWorkDir }));


                // Saves the project.
                string prjFile = Path.Combine(this.m_defaultDir, projectID);
                thisPrj.Save(prjFile);

                Trace.WriteLine(message);
            }
            catch (Exception ex)
            {
                message = String.Format(MessageResources.ErrSavePrj,
                    new object[] { projectID });
                Trace.WriteLine(message);
                throw new Exception(message, ex);
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
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new Exception(MessageResources.ErrSaveScript, ex);
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
                engine.ExecuteFile(scriptFile);
                string stdOut = ASCIIEncoding.ASCII.GetString(standardOutput.ToArray());
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLoadFile,
                    new object[] { scriptFile }));
            }
        }

        /// <summary>
        /// Saves the simulation parameter.
        /// </summary>
        /// <param name="paramID">The simulation parameter ID</param>
        public void SaveSimulationParameter(string paramID)
        {
            string message = null;
            try
            {
                message = "[" + paramID + "]";
                //
                // Initializes.
                //
                Debug.Assert(!String.IsNullOrEmpty(paramID));
                this.SetDefaultDir();
                if (String.IsNullOrEmpty(m_defaultDir))
                {
                    throw new Exception(String.Format(MessageResources.ErrNoSet,
                        new object[] { MessageResources.NameWorkDir }));
                }
                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + m_currentProject.Name))
                {
                    this.SaveProject(m_currentProject.Name);
                }
                string simulationDirName =
                    this.m_defaultDir + Constants.delimiterPath +
                    m_currentProject.Name + Constants.delimiterPath + Constants.xpathParameters;
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
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> initialCondition
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
                //
                // 4 Project
                //
                this.SaveProject(m_currentProject.Name);
            }
            catch (Exception ex)
            {
                message = String.Format(MessageResources.ErrSavePrj,
                    new object[] { m_currentProject.Name });
                Trace.WriteLine(message);
                throw new Exception(message, ex);
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
                throw new Exception(String.Format(MessageResources.ErrSavePrj,
                    new object[] { m_currentProject.Name }), ex);
            }
        }

        public LogData LoadSimulationResult(string fileName)
        {
            return Ecd.LoadSavedLogData(fileName);
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
            string message = null;
            try
            {
                message =
                    "[" + m_currentProject.Name + "][" + m_currentProject.SimulationParam + "]";
                //
                // Initializes.
                //
                if (fullIDList == null || fullIDList.Count <= 0)
                {
                    return;
                }
                string simulationDirName = null;
                if (!string.IsNullOrEmpty(savedDirName))
                {
                    simulationDirName = savedDirName;
                }
                else
                {
                    this.SetDefaultDir();
                    if (string.IsNullOrEmpty(m_defaultDir))
                    {
                        throw new Exception(String.Format(MessageResources.ErrNoSet,
                            new object[] { MessageResources.NameWorkDir }));
                    }
                    if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + m_currentProject.Name))
                    {
                        this.SaveProject(m_currentProject.Name);
                    }
                    simulationDirName = GetSimulationResultSaveDirectory();
                }
                if (!Directory.Exists(simulationDirName))
                {
                    Directory.CreateDirectory(simulationDirName);
                }
                //
                // Saves the "LogData".
                //
                List<LogData> logDataList = this.GetLogData(
                    startTime,
                    endTime,
                    m_currentProject.LoggerPolicyDic[m_currentProject.SimulationParam].ReloadInterval
                    );
                if (logDataList == null || logDataList.Count <= 0)
                {
                    return;
                }
                foreach (LogData logData in logDataList)
                {
                    string fullID =
                        logData.type + Constants.delimiterColon +
                        logData.key + Constants.delimiterColon +
                        logData.propName;
                    if (fullIDList.Contains(fullID))
                    {
                        if (savedType == null || savedType.Equals(Constants.xpathCsv) ||
                            savedType.Equals(Constants.xpathEcd))
                        {
                            Ecd ecd = new Ecd();
                            ecd.Create(simulationDirName, logData, savedType);
                            message = "[" + fullID + "]";
                            Trace.WriteLine("Save Simulation Result: " + message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = MessageResources.ErrSaveLog;
                Trace.WriteLine(message);
                throw new Exception(message, ex);
            }
        }

        public string GetSimulationResultSaveDirectory()
        {
            return this.m_defaultDir + "\\" +
                        m_currentProject.Name + "\\" + Constants.xpathParameters +
                        "\\" + m_currentProject.SimulationParam;
        }

        /// <summary>
        /// Sets the directory name to "m_defaultDir"
        /// </summary>
        private void SetDefaultDir()
        {
            if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
            {
                string baseDirs = Util.GetBaseDir();
                if (baseDirs == null || baseDirs.Length <= 0)
                {
                    return;
                }
                foreach (string baseDir in baseDirs.Split(Path.PathSeparator))
                {
                    if (Directory.Exists(baseDir))
                    {
                        this.m_defaultDir = baseDir;
                        break;
                    }
                }
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
                EcellValue.CastToWrappedPolymorph4EcellValue(newValue));
        }

        /// <summary>
        /// Sets the "LoggerPolicy".
        /// </summary>
        /// <param name="parameterID">The parameter ID</param>
        /// <param name="loggerPolicy">The "LoggerPolicy"</param>
        public void SetLoggerPolicy(string parameterID, LoggerPolicy loggerPolicy)
        {
            m_currentProject.LoggerPolicyDic[parameterID] = loggerPolicy;
            Trace.WriteLine(String.Format(MessageResources.InfoUpdateLogPol,
                new object[] { parameterID }));
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
                    ecellData.Value = new EcellValue(new List<EcellValue>());
                }
                dic[ecellData.Name] = ecellData;
            }
        }

        /// <summary>
        /// Set positions of all EcellObjects.
        /// </summary>
        /// <param name="modelID">Model ID</param>
        public void SetPositions(string modelID)
        {
            if (null != modelID && m_currentProject.SystemDic.ContainsKey(modelID))
            {
                foreach (EcellObject eo in m_currentProject.SystemDic[modelID])
                    m_env.PluginManager.SetPosition(eo);
            }
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
            m_env.PluginManager.SetPosition(oldNode.Copy());
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
                string oldParameterID = m_currentProject.SimulationParam;
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
                    m_currentProject.SimulationParam = parameterID;
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
                m_env.PluginManager.ParameterSet(m_currentProject.Name, parameterID);
                Trace.WriteLine("Set Simulation Parameter: " + message);
                if (isRecorded)
                    m_env.ActionManager.AddAction(new SetSimParamAction(parameterID, oldParameterID, isAnchor));
            }
            catch (Exception ex)
            {
                message = MessageResources.ErrSetSimParam;
                Trace.WriteLine(message);
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Starts this simulation with the step limit.
        /// </summary>
        /// <param name="stepLimit">The step limit</param>
        /// <param name="statusNum">simulation status.</param>
        public void SimulationStart(int stepLimit, int statusNum)
        {
            try
            {
                // Checks the simulator's status.
                if (m_currentProject.SimulationStatus == SimulationStatus.Wait)
                {
                    this.Initialize(true);
                    this.m_simulationStepLimit = stepLimit;
                    Trace.WriteLine("Start Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
                }
                else if (m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    if (this.m_simulationStepLimit == -1)
                    {
                        this.m_simulationStepLimit = stepLimit;
                    }
                    Trace.WriteLine("Restart Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
                }

                // Debug
                // this.LookIntoSimulator(m_currentProject.Simulator);
                // Selects the type of the simulation and Starts.
                m_currentProject.SimulationStatus = SimulationStatus.Run;
                if (this.m_simulationStepLimit > 0)
                {
                    while (m_currentProject.SimulationStatus == SimulationStatus.Run)
                    {
                        if (this.m_simulationStepLimit <= m_defaultStepCount)
                        {
                            m_currentProject.Simulator.Step(this.m_simulationStepLimit);
                            Application.DoEvents();
                            double currentTime = m_currentProject.Simulator.GetCurrentTime();
                            if (statusNum == (int)SimulationStatus.Suspended)
                            {
                                m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                            }
                            else
                            {
                                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                            }
                            this.m_env.PluginManager.AdvancedTime(currentTime);
                            this.m_simulationStepLimit = -1;
                            break;
                        }
                        else
                        {
                            m_currentProject.Simulator.Step(m_defaultStepCount);
                            Application.DoEvents();
                            double currentTime = m_currentProject.Simulator.GetCurrentTime();
                            this.m_env.PluginManager.AdvancedTime(currentTime);
                            this.m_simulationStepLimit = this.m_simulationStepLimit - m_defaultStepCount;
                        }
                    }
                }
                else
                {
                    while (m_currentProject.SimulationStatus == SimulationStatus.Run)
                    {
                        m_currentProject.Simulator.Step(m_defaultStepCount);
                        Application.DoEvents();
                        double currentTime = m_currentProject.Simulator.GetCurrentTime();
                        this.m_env.PluginManager.AdvancedTime(currentTime);
                    }
                }
            }
            catch (Exception ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                string message = MessageResources.ErrRunSim;
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Starts this simulation with the time limit.
        /// </summary>
        /// <param name="timeLimit">The time limit</param>
        /// <param name="statusNum">Simulation status./</param>
        public void SimulationStart(double timeLimit, int statusNum)
        {
            try
            {
                //
                // Checks the simulator's status.
                //
                double startTime = 0.0;
                if (m_currentProject.SimulationStatus == SimulationStatus.Wait)
                {
                    this.Initialize(true);
                    this.m_simulationTimeLimit = timeLimit;
                    this.m_simulationStartTime = 0.0;
                    Trace.WriteLine("Start Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
                }
                else if (m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    if (this.m_simulationTimeLimit == -1.0 || this.m_simulationTimeLimit == 0.0)
                    {
                        this.m_simulationTimeLimit = timeLimit;
                        this.m_simulationStartTime = m_currentProject.Simulator.GetCurrentTime();
                    }
                    Trace.WriteLine("Restart Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
                }

                //
                // Selects the type of the simulation and Starts.
                //
                m_currentProject.SimulationStatus = SimulationStatus.Run;
                if (this.m_simulationTimeLimit > 0.0)
                {
                    startTime = m_currentProject.Simulator.GetCurrentTime();
                    Thread thread = new Thread(new ThreadStart(SimulationStartByThreadWithLimit));
                    thread.Start();
                    int i = 0;
                    while (m_currentProject.SimulationStatus == SimulationStatus.Run)
                    {
                        i++;
                        if (i == 1000)
                        {
                            Thread.Sleep(1);
                            i = 0;
                        }
                        Application.DoEvents();
                        double currentTime = m_currentProject.Simulator.GetCurrentTime();
                        this.m_env.PluginManager.AdvancedTime(currentTime);
                        if (currentTime >= (this.m_simulationStartTime + this.m_simulationTimeLimit))
                        {
                            if (statusNum == (int)SimulationStatus.Suspended)
                            {
                                m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                            }
                            else
                            {
                                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                            }
                            currentTime = m_currentProject.Simulator.GetCurrentTime();
                            this.m_env.PluginManager.AdvancedTime(currentTime);
                            this.m_simulationTimeLimit = -1.0;
                            break;
                        }
                    }
                }
                else
                {
                    // Thread thread = new Thread(new ThreadStart(SimulationStartByThreadWithLimit));
                    // thread.Start();
                    int i = 0;
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
            }
            catch (Exception ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                string message = MessageResources.ErrRunSim;
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Starts this simulation without the time limit by the thread.
        /// </summary>
        void SimulationStartByThread()
        {
            Util.InitialLanguage();
            m_currentProject.Simulator.Run();
        }

        /// <summary>
        /// Starts this simulation with the time limit by the thread.
        /// </summary>
        void SimulationStartByThreadWithLimit()
        {
            Util.InitialLanguage();
            m_currentProject.Simulator.Run(this.m_simulationTimeLimit);
        }

        /// <summary>
        /// Starts this simulation with the step limit.
        /// </summary>
        /// <param name="stepLimit">The step limit</param>
        public void SimulationStartKeepSetting(int stepLimit)
        {
            if (m_currentProject.SimulationStatus == SimulationStatus.Run)
            {
                throw new Exception(MessageResources.ErrRunning);
            }

            this.SimulationStart(stepLimit, (int)SimulationStatus.Suspended);
        }

        /// <summary>
        /// Starts this simulation with the time limit.
        /// </summary>
        /// <param name="timeLimit">The time limit</param>
        public void SimulationStartKeepSetting(double timeLimit)
        {
            if (m_currentProject.SimulationStatus == SimulationStatus.Run)
            {
                throw new Exception(MessageResources.ErrRunning);
            }

            this.SimulationStart(timeLimit, (int)SimulationStatus.Suspended);
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
                Trace.WriteLine(String.Format(MessageResources.InfoResetSim,
                    new object[] { m_currentProject.Simulator.GetCurrentTime().ToString() }));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new Exception(MessageResources.ErrResetSim, ex);
            }
            finally
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
            }
        }

        /// <summary>
        /// Suspends the simulation.
        /// </summary>
        public void SimulationSuspend()
        {
            try
            {
                m_currentProject.Simulator.Suspend();
                m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                Trace.WriteLine(String.Format(MessageResources.InfoSuspend,
                    new object[] { m_currentProject.Simulator.GetCurrentTime() }));
                m_env.PluginManager.ChangeStatus(ProjectStatus.Suspended);
            }
            catch (Exception ex)
            {
                string message = MessageResources.ErrSuspendSim;
                Trace.WriteLine(message);
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Splits the entity path.
        /// </summary>
        /// <param name="key">The key of the "EcellObject"</param>
        /// <param name="type">The type of the "EcellObject"</param>
        /// <param name="propName">The property name of the "ECellObject"</param>
        /// <param name="entityPath"></param>
        private void Split4EntityPath(ref string key, ref string type, ref string propName, string entityPath)
        {
            string[] data = entityPath.Split(Constants.delimiterColon.ToCharArray());
            if (data.Length < 4)
            {
                return;
            }
            key = data[1] + Constants.delimiterColon + data[2];
            type = data[0];
            propName = data[3];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterID"></param>
        /// <param name="modelID"></param>
        /// <param name="type"></param>
        /// <param name="initialList"></param>
        public void UpdateInitialCondition(
                string parameterID, string modelID, string type, Dictionary<string, double> initialList)
        {
            // Check null.
            if (initialList == null || initialList.Count <= 0)
                return;

            string message = null;
            if (string.IsNullOrEmpty(parameterID))
                parameterID = m_currentProject.SimulationParam;

            message = "[" + parameterID + "][" + modelID + "][" + type + "]";
            Dictionary<string, double> parameters = this.m_currentProject.InitialCondition[parameterID][modelID][type];
            foreach (string key in initialList.Keys)
            {
                if (parameters.ContainsKey(key))
                    parameters.Remove(key);

                parameters[key] = initialList[key];
            }
            Trace.WriteLine("Update Initial Condition: " + message);
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
            bool updateFlag = false;

            try
            {
                List<EcellObject> oldStepperList = new List<EcellObject>();
                Dictionary<string, List<EcellObject>> perParameterStepperListDic = m_currentProject.StepperDic[parameterID];
                foreach (EcellObject model in m_currentProject.ModelList)
                {
                    List<EcellObject> remainingStepperList = new List<EcellObject>();
                    foreach (EcellObject stepper in perParameterStepperListDic[model.ModelID])
                    {
                        oldStepperList.Add(stepper.Copy());
                        bool deleted = true;
                        foreach (EcellObject newStepper in stepperList)
                        {
                            if (stepper.Name == newStepper.Name)
                            {
                                remainingStepperList.Add(stepper);
                                break;
                            }
                        }
                    }
                    perParameterStepperListDic[model.ModelID] = remainingStepperList;
                }
                foreach (EcellObject stepper in stepperList)
                {
                    message = stepper.Key;

                    List<EcellObject> perModelStepperList = perParameterStepperListDic[stepper.ModelID];
                    bool added = true;
                    foreach (EcellObject oldStepper in perModelStepperList)
                    {
                        if (oldStepper.Key.Equals(stepper.Key))
                        {
                            this.CheckDifferences(oldStepper, stepper, parameterID);
                            updateFlag = true;
                            added = false;
                            break;
                        }
                    }
                    if (added)
                    {
                        perModelStepperList.Add(stepper);
                    }
                }
                if (updateFlag && isRecorded)
                    m_env.ActionManager.AddAction(new UpdateStepperAction(parameterID, stepperList, oldStepperList));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageResources.ErrSetSimParam, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulator"></param>
        private void LookIntoSimulator(WrappedSimulator simulator)
        {
            List<string> entityList = new List<string>();
            entityList.Add(Constants.xpathProcess);
            entityList.Add(Constants.xpathVariable);
            foreach (string entityName in entityList)
            {
                foreach (WrappedPolymorph wpFullID in
                    m_currentProject.Simulator.GetEntityList(
                        entityName, Constants.delimiterPath).CastToList())
                {
                    EcellValue fullID = new EcellValue(wpFullID);
                    Console.WriteLine(entityName + ": " + fullID);
                    foreach (WrappedPolymorph wpEntityProperty in
                        m_currentProject.Simulator.GetEntityPropertyList(
                            entityName + Constants.delimiterColon + Constants.delimiterPath + Constants.delimiterColon +
                            fullID).CastToList())
                    {
                        string entityProperty = (new EcellValue(wpEntityProperty)).CastToString();
                        List<bool> wpAttrList
                            = m_currentProject.Simulator.GetEntityPropertyAttributes(
                                entityName + Constants.delimiterColon + Constants.delimiterPath + Constants.delimiterColon +
                                fullID + Constants.delimiterColon + entityProperty);
                        if (wpAttrList[1])
                        {
                            WrappedPolymorph wp
                                = m_currentProject.Simulator.GetEntityProperty(
                                    entityName + Constants.delimiterColon + Constants.delimiterPath + Constants.delimiterColon +
                                    fullID + Constants.delimiterColon + entityProperty);
                            EcellValue wpv = new EcellValue(wp);
                            Console.WriteLine(
                                entityName + Constants.delimiterColon + Constants.delimiterPath + Constants.delimiterColon +
                                fullID + Constants.delimiterColon + entityProperty + ", " + wpv);
                        }
                        else
                        {
                            Console.WriteLine(
                                entityName + Constants.delimiterColon + Constants.delimiterPath + Constants.delimiterColon +
                                fullID + Constants.delimiterColon + entityProperty + ", " + "Not Get");
                        }
                    }
                }
            }
            foreach (WrappedPolymorph wpStepperID in
                m_currentProject.Simulator.GetStepperList().CastToList())
            {
                string stepperID = (new EcellValue(wpStepperID)).CastToString();
                Console.WriteLine(Constants.xpathStepper + " " + stepperID);
                foreach (WrappedPolymorph wpStepperProperty in
                    m_currentProject.Simulator.GetStepperPropertyList(stepperID).CastToList())
                {
                    string stepperProperty = (new EcellValue(wpStepperProperty)).CastToString();
                    List<bool> wpAttrList
                        = m_currentProject.Simulator.GetStepperPropertyAttributes(
                        stepperID, stepperProperty);
                    if (wpAttrList[1])
                    {
                        WrappedPolymorph wp
                            = m_currentProject.Simulator.GetStepperProperty(
                            stepperID, stepperProperty);
                        EcellValue wpv = new EcellValue(wp);
                        Console.WriteLine(stepperProperty + ", " + wpv);
                    }
                    else
                    {
                        Console.WriteLine(stepperProperty + ", " + "Not Get");
                    }
                }
            }
        }
        /// <summary>
        /// Undo action.
        /// </summary>
        public void UndoAction()
        {
            m_env.ActionManager.UndoAction();
        }

        /// <summary>
        /// Redo action.
        /// </summary>
        public void RedoAction()
        {
            m_env.ActionManager.RedoAction();
        }

        /// <summary>
        /// Create a new WrappedSimulator instance.
        /// </summary>
        protected WrappedSimulator CreateSimulatorInstance()
        {
            string[] dmpath = Util.GetDMDirs(m_currentProject.ProjectPath);
            Trace.WriteLine("Creating simulator (dmpath=" + string.Join(";", dmpath) + ")");
            return new WrappedSimulator(dmpath);
        }

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
