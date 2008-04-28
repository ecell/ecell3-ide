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
using EcellCoreLib;
using EcellLib.Objects;

namespace EcellLib
{
    /// <summary>
    /// Manages data of projects, models, and so on.
    /// </summary>
    public class DataManager
    {
        #region Fields
        #region Managers
        /// <summary>
        /// s_instance (singleton instance)
        /// </summary>
        private static DataManager s_instance = null;
        /// <summary>
        /// The "PluginManager"
        /// </summary>
        private PluginManager m_pManager = null;
        /// <summary>
        /// ActionManager.
        /// </summary>
        private ActionManager m_aManager;
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
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResLib));
        /// <summary>
        /// ResourceManager for StaticDebugSetupWindow.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResLib));
        #endregion

        /// <summary>
        /// Creates the new "DataManager" instance with no argument.
        /// </summary>
        private DataManager()
        {
            this.m_pManager = PluginManager.GetPluginManager();
            this.m_projectList = new List<Project>();
            this.m_observedList = new Dictionary<string, EcellObservedData>();
            this.m_parameterList = new Dictionary<string, EcellParameterData>();
            m_aManager = ActionManager.GetActionManager();
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
        /// Save the user action to the set file.
        /// </summary>
        /// <param name="fileName">saved file name.</param>
        public void SaveUserAction(string fileName)
        {
            m_aManager.SaveActionFile(fileName);
        }

        /// <summary>
        /// Load the user action from the set file.
        /// </summary>
        /// <param name="filenName">saved file name.</param>
        public void LoadUserActionFile(string filenName)
        {
            m_aManager.LoadActionFile(filenName);
        }


        /// <summary>
        /// Adds the new "Stepper"
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <param name="l_stepper">The "Stepper"</param>
        public void AddStepperID(string l_parameterID, EcellObject l_stepper)
        {
            AddStepperID(l_parameterID, l_stepper, true);
        }

        /// <summary>
        /// Adds the new "Stepper"
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <param name="l_stepper">The "Stepper"</param>
        /// <param name="l_isRecorded">Whether this action is recorded</param>
        public void AddStepperID(string l_parameterID, EcellObject l_stepper, bool l_isRecorded)
        {
            string l_message = null;
            Dictionary<string, List<EcellObject>> stepperDic = null;
            try
            {
                // Get stepperDic
                l_message = "[" + l_parameterID + "][" + l_stepper.ModelID + "][" + l_stepper.Key + "]";
                if (l_stepper == null || string.IsNullOrEmpty(l_parameterID) ||string.IsNullOrEmpty(l_stepper.ModelID))
                    throw new Exception();
                if (!m_currentProject.StepperDic.ContainsKey(l_parameterID))
                    m_currentProject.StepperDic[l_parameterID] = new Dictionary<string, List<EcellObject>>();
                stepperDic = m_currentProject.StepperDic[l_parameterID];
                if (!stepperDic.ContainsKey(l_stepper.ModelID))
                    throw new Exception();

                // Check duplication.
                foreach (EcellObject l_storedStepper in stepperDic[l_stepper.ModelID])
                {
                    if (!l_stepper.Key.Equals(l_storedStepper.Key))
                        continue;
                    throw new Exception(
                        string.Format(
                            m_resources.GetString(ErrorConstants.ErrExistStepper),
                            new object[] { l_message }
                        )
                    );
                }
                // Set Stteper.
                stepperDic[l_stepper.ModelID].Add(l_stepper);
                if (m_currentProject.SimulationParam.Equals(l_parameterID))
                {
                    List<EcellObject> stepperList = new List<EcellObject>();
                    stepperList.Add(l_stepper);
                    m_pManager.DataAdd(stepperList);
                }
                MessageCreateEntity("Stepper", l_message);
                if (l_isRecorded)
                    m_aManager.AddAction(new AddStepperAction(l_parameterID, l_stepper));
            }
            catch (Exception l_ex)
            {
                l_message = l_message + m_resources.GetString(ErrorConstants.ErrNotCreStepper);
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Checks differences between the source "EcellObject" and the destination.
        /// </summary>
        /// <param name="l_src">The source "EcellObject"</param>
        /// <param name="l_dest">The  destination "EcellObject"</param>
        /// <param name="l_parameterID">The simulation parameter ID</param>
        private void CheckDifferences(EcellObject l_src, EcellObject l_dest, string l_parameterID)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>> initialCondition = this.m_currentProject.InitialCondition;

            // Set Message
            string l_message = null;
            if (string.IsNullOrEmpty(l_parameterID))
                l_message = "[" + l_src.ModelID + "][" + l_src.Key + "]";
            else
                l_message = "[" + l_parameterID + "][" + l_src.ModelID + "][" + l_src.Key + "]";

            // Check Class change.
            if (!l_src.Classname.Equals(l_dest.Classname))
                MessageUpdateData(Constants.xpathClassName, l_message, l_src.Classname, l_dest.Classname);
            // Check Key change.
            if (!l_src.Key.Equals(l_dest.Key))
                MessageUpdateData(Constants.xpathKey, l_message, l_src.Key, l_dest.Key);

            // Changes a className and not change a key.
            if (!l_src.Classname.Equals(l_dest.Classname) && l_src.Key.Equals(l_dest.Key))
            {
                foreach (EcellData l_srcEcellData in l_src.Value)
                {
                    // Changes the logger.
                    if (l_srcEcellData.Logged)
                    {
                        MessageDeleteEntity("Logger", l_message + "[" + l_srcEcellData.Name + "]");
                    }
                    // Changes the initial parameter.
                    if (!l_src.Type.Equals(Constants.xpathSystem)
                        && !l_src.Type.Equals(Constants.xpathProcess)
                        && !l_src.Type.Equals(Constants.xpathVariable))
                        continue;
                    if (!l_srcEcellData.IsInitialized())
                        continue;

                    if (string.IsNullOrEmpty(l_parameterID))
                    {
                        foreach (string l_keyParameterID in initialCondition.Keys)
                        {
                            Dictionary<string, double> condition = initialCondition[l_keyParameterID][l_src.ModelID][l_src.Type];
                            if (condition.ContainsKey(l_srcEcellData.EntityPath))
                            {
                                condition.Remove(l_srcEcellData.EntityPath);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, double> condition = initialCondition[l_parameterID][l_src.ModelID][l_src.Type];
                        if (condition.ContainsKey(l_srcEcellData.EntityPath))
                        {
                            condition.Remove(l_srcEcellData.EntityPath);
                        }
                    }
                }
                foreach (EcellData l_destEcellData in l_dest.Value)
                {
                    // Changes the initial parameter.
                    if (!l_dest.Type.Equals(Constants.xpathSystem)
                        && !l_dest.Type.Equals(Constants.xpathProcess)
                        && !l_dest.Type.Equals(Constants.xpathVariable))
                        continue;
                    if (!l_destEcellData.IsInitialized())
                        continue;

                    // GetValue
                    EcellValue l_value = l_destEcellData.Value;
                    double temp = 0;
                    if (l_value.IsDouble())
                        temp = l_value.CastToDouble();
                    else if (l_value.IsInt())
                        temp = l_value.CastToInt();
                    else
                        continue;

                    if (!string.IsNullOrEmpty(l_parameterID))
                    {
                        initialCondition[l_parameterID][l_dest.ModelID][l_dest.Type][l_destEcellData.EntityPath] = temp;
                    }
                    else
                    {
                        foreach (string l_keyParameterID in initialCondition.Keys)
                        {
                            initialCondition[l_keyParameterID][l_dest.ModelID][l_dest.Type][l_destEcellData.EntityPath] = temp;
                        }
                    }
                }
            }
            else
            {
                foreach (EcellData l_srcEcellData in l_src.Value)
                {
                    foreach (EcellData l_destEcellData in l_dest.Value)
                    {
                        if (!l_srcEcellData.Name.Equals(l_destEcellData.Name) ||
                            !l_srcEcellData.EntityPath.Equals(l_destEcellData.EntityPath))
                            continue;

                        if (!l_srcEcellData.Logged && l_destEcellData.Logged)
                        {
                            MessageCreateEntity("Logger", l_message + "[" + l_srcEcellData.Name + "]");
                        }
                        else if (l_srcEcellData.Logged && !l_destEcellData.Logged)
                        {
                            MessageDeleteEntity("Logger", l_message + "[" + l_srcEcellData.Name + "]");
                        }
                        if (!l_srcEcellData.Value.ToString()
                                .Equals(l_destEcellData.Value.ToString()))
                        {
                            Message(
                                "Update Data: " + l_message
                                    + "[" + l_srcEcellData.Name + "]"
                                    + System.Environment.NewLine
                                    + "\t[" + l_srcEcellData.Value.ToString()
                                    + "]->[" + l_destEcellData.Value.ToString() + "]");
                        }
                        //
                        // Changes the initial parameter.
                        //
                        if (!l_src.Type.Equals(Constants.xpathSystem)
                            && !l_src.Type.Equals(Constants.xpathProcess)
                            && !l_src.Type.Equals(Constants.xpathVariable))
                            continue;

                        EcellValue l_value = l_destEcellData.Value;
                        if (!l_srcEcellData.IsInitialized()
                            || l_srcEcellData.Value.Equals(l_value))
                            continue;

                        // GetValue
                        double temp = 0;
                        if (l_value.IsDouble())
                            temp = l_value.CastToDouble();
                        else if (l_value.IsInt())
                            temp = l_value.CastToInt();
                        else
                            continue;

                        if (!string.IsNullOrEmpty(l_parameterID))
                        {
                            Dictionary<string, double> condition = initialCondition[l_parameterID][l_src.ModelID][l_src.Type];
                            if (!condition.ContainsKey(l_srcEcellData.EntityPath))
                                continue;
                            condition[l_srcEcellData.EntityPath] = temp;
                        }
                        else
                        {
                            foreach (string l_keyParameterID in initialCondition.Keys)
                            {
                                Dictionary<string, double> condition = initialCondition[l_keyParameterID][l_src.ModelID][l_src.Type];

                                if (!condition.ContainsKey(l_srcEcellData.EntityPath))
                                    continue;
                                condition[l_srcEcellData.EntityPath] = temp;
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
        /// <param name="l_ecellObject">The checked "EcellObject"</param>
        private static void CheckEntityPath(EcellObject l_ecellObject)
        {
            if (l_ecellObject.Key == null)
            {
                return;
            }
            if (l_ecellObject.Type.Equals(Constants.xpathSystem))
            {
                string l_entityPath = null;
                string l_parentPath = l_ecellObject.ParentSystemID;
                string l_childPath = l_ecellObject.Name;
                l_entityPath = l_ecellObject.Type + Constants.delimiterColon
                    + l_parentPath + Constants.delimiterColon
                    + l_childPath + Constants.delimiterColon;
                if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
                {
                    for (int i = 0; i < l_ecellObject.Value.Count; i++)
                    {
                        if (!l_ecellObject.Value[i].EntityPath.Equals(
                            l_entityPath + l_ecellObject.Value[i].Name))
                        {
                            l_ecellObject.Value[i].EntityPath
                                = l_entityPath + l_ecellObject.Value[i].Name;
                        }
                    }
                }
                if (l_ecellObject.Children != null && l_ecellObject.Children.Count > 0)
                {
                    for (int i = 0; i < l_ecellObject.Children.Count; i++)
                    {
                        CheckEntityPath(l_ecellObject.Children[i]);
                    }
                }
            }
            else if (l_ecellObject.Type.Equals(Constants.xpathProcess) || l_ecellObject.Type.Equals(Constants.xpathVariable))
            {
                string l_entityPath
                    = l_ecellObject.Type + Constants.delimiterColon
                    + l_ecellObject.Key + Constants.delimiterColon;
                if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
                {
                    for (int i = 0; i < l_ecellObject.Value.Count; i++)
                    {
                        if (!l_ecellObject.Value[i].EntityPath.Equals(
                            l_entityPath + l_ecellObject.Value[i].Name))
                        {
                            l_ecellObject.Value[i].EntityPath
                                = l_entityPath + l_ecellObject.Value[i].Name;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the "VariableReferenceList" has available "Variable"s.
        /// </summary>
        /// <param name="l_src">The checked "EcellObject"</param>
        /// <param name="l_dest">The available "EcellObject"</param>
        /// <param name="l_variableDic">The dictionary of the name of available "Variable"s</param>
        /// <returns>true if it modified; false otherwise</returns>
        private static bool CheckVariableReferenceList(
            EcellObject l_src, ref EcellObject l_dest, Dictionary<string, string> l_variableDic)
        {
            bool l_changedFlag = false;
            l_dest = l_src.Copy();
            EcellValue l_varList = l_dest.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST);
            if (l_varList == null || l_varList.ToString().Length <= 0)
                return l_changedFlag;

            List<EcellValue> l_changedValue = new List<EcellValue>();
            foreach (EcellValue l_ecellValue in l_varList.CastToList())
            {
                List<EcellValue> l_changedElements = new List<EcellValue>();
                foreach (EcellValue l_element in l_ecellValue.CastToList())
                {
                    if (l_element.IsString()
                        && l_element.CastToString().StartsWith(Constants.delimiterColon))
                    {
                        string l_oldKey = l_element.CastToString().Substring(1);
                        if (l_variableDic.ContainsKey(l_oldKey))
                        {
                            l_changedElements.Add(
                                new EcellValue(Constants.delimiterColon + l_variableDic[l_oldKey]));
                            l_changedFlag = true;
                        }
                        else
                        {
                            l_changedElements.Add(l_element);
                        }
                    }
                    else
                    {
                        l_changedElements.Add(l_element);
                    }
                }
                l_changedValue.Add(new EcellValue(l_changedElements));
            }
            l_dest.GetEcellData(EcellProcess.VARIABLEREFERENCELIST).Value = new EcellValue(l_changedValue);

            return l_changedFlag;
        }

        /// <summary>
        /// Checks whether the "VariableReferenceList" has available "Variable"s.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_newKey">The new key</param>
        /// <param name="l_oldKey">The old key</param>
        /// <param name="l_changedList">The list of the modified "ECekllObject"</param>
        private void CheckVariableReferenceList(
            string l_modelID, string l_newKey, string l_oldKey, List<EcellObject> l_changedList)
        {
            foreach (EcellObject l_system in GetData(l_modelID, null))
            {
                if (l_system.Children == null || l_system.Children.Count <= 0)
                    continue;

                foreach (EcellObject l_instance in l_system.Children)
                {
                    EcellObject l_entity = l_instance.Copy();
                    if (!l_entity.Type.Equals(Constants.xpathProcess))
                        continue;
                    else if (l_entity.Value == null || l_entity.Value.Count <= 0)
                        continue;

                    bool l_changedFlag = false;
                    foreach (EcellData l_ecellData in l_entity.Value)
                    {
                        if (!l_ecellData.Name.Equals(Constants.xpathVRL))
                            continue;

                        List<EcellValue> l_changedValue = new List<EcellValue>();
                        if (l_ecellData.Value == null) continue;
                        if (l_ecellData.Value.ToString() == "") continue;
                        foreach (EcellValue l_ecellValue in l_ecellData.Value.CastToList())
                        {
                            List<EcellValue> l_changedElements = new List<EcellValue>();
                            foreach (EcellValue l_element in l_ecellValue.CastToList())
                            {
                                if (l_element.IsString()
                                    && l_element.CastToString().Equals(Constants.delimiterColon + l_oldKey))
                                {
                                    l_changedElements.Add(
                                        new EcellValue(Constants.delimiterColon + l_newKey));
                                    l_changedFlag = true;
                                }
                                else
                                {
                                    l_changedElements.Add(l_element);
                                }
                            }
                            l_changedValue.Add(new EcellValue(l_changedElements));
                        }
                        l_ecellData.Value = new EcellValue(l_changedValue);
                    }
                    if (l_changedFlag)
                    {
                        l_changedList.Add(l_entity);
                    }
                }
            }
        }

        /// <summary>
        /// Closes project without confirming save or no save.
        /// </summary>
        /// <param name="l_prj">The project ID</param>
        public void CloseProject(string l_prj)
        {
            string l_message = null;
            try
            {
                List<string> l_tmpList = new List<string>();
                l_message = "[" + l_prj + "]";
                if (l_prj == null)
                {
                    foreach (Project prj in m_projectList)
                    {
                        l_tmpList.Add(prj.Name);
                    }
                }
                else
                {
                    l_tmpList.Add(l_prj);
                }

                foreach (string l_str in l_tmpList)
                {
                    foreach (Project prj in m_projectList)
                    {
                        if (prj.Name == l_str)
                        {
                            m_projectList.Remove(prj);
                            break;
                        }
                    }
                }
                this.m_currentProject = null;
                this.m_pManager.AdvancedTime(0);
                this.m_pManager.Clear();
                Message("Close Project: " + l_message + System.Environment.NewLine);
                m_aManager.Clear();
            }
            catch (Exception l_ex)
            {
                l_message = l_message + m_resources.GetString(ErrorConstants.ErrClosePrj);
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Checks whether the model ID exists in this project.
        /// </summary>
        /// <param name="l_modelID">The checked model ID</param>
        /// <returns>true if the model ID exists; false otherwise</returns>
        public bool ContainsModel(string l_modelID)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_modelID + "]";
                bool l_foundFlag = false;
                foreach (EcellObject l_ecellObject in m_currentProject.ModelList)
                {
                    if (l_ecellObject.ModelID.Equals(l_modelID))
                    {
                        l_foundFlag = true;
                        break;
                    }
                }
                return l_foundFlag;
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrFindModel) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Copys the initial condition.
        /// </summary>
        /// <param name="l_srcDic">The source</param>
        /// <param name="l_destDic">The destination</param>
        private static void Copy4InitialCondition(
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_srcDic,
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_destDic)
        {
            foreach (string l_parentKey in l_srcDic.Keys)
            {
                l_destDic[l_parentKey] = new Dictionary<string, Dictionary<string, double>>();
                foreach (string l_childKey in l_srcDic[l_parentKey].Keys)
                {
                    l_destDic[l_parentKey][l_childKey] = new Dictionary<string, double>();
                    foreach (string l_grandChildKey in l_srcDic[l_parentKey][l_childKey].Keys)
                    {
                        l_destDic[l_parentKey][l_childKey][l_grandChildKey]
                                = l_srcDic[l_parentKey][l_childKey][l_grandChildKey];
                    }
                }
            }
        }

        /// <summary>
        /// Creates the dummy simulator 4 property lists.
        /// </summary>
        /// <param name="l_simulator">The dummy simulator</param>
        /// <param name="l_defaultProcess">The dm name of "Process"</param>
        /// <param name="l_defaultStepper">The dm name of "Stepper"</param>
        private static void BuildDefaultSimulator(
                WrappedSimulator l_simulator, string l_defaultProcess, string l_defaultStepper)
        {
            try
            {
                bool l_processFlag = false;
                bool l_stepperFlag = false;
                if (l_defaultProcess != null)
                {
                    l_processFlag = true;
                }
                if (l_defaultStepper != null)
                {
                    l_stepperFlag = true;
                }
                if (!l_processFlag || !l_stepperFlag)
                {
                    // 4 Process
                    //
                    if (!l_processFlag)
                    {
                        l_defaultProcess = Constants.DefaultProcessName;
                        l_processFlag = true;
                    }
                    //
                    // 4 Stepper
                    //
                    if (!l_stepperFlag)
                    {
                        l_defaultStepper = Constants.DefaultStepperName;
                        l_stepperFlag = true;
                    }
                }
                l_simulator.CreateStepper(l_defaultStepper, Constants.textKey);
                l_simulator.CreateEntity(
                    Constants.xpathVariable,
                    Constants.xpathVariable + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper()
                    );
                l_simulator.CreateEntity(
                    l_defaultProcess,
                    Constants.xpathProcess + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper()
                );
                l_simulator.LoadEntityProperty(
                    Util.BuildFullPN(
                        Constants.xpathSystem,
                        "",
                        Constants.delimiterPath,
                        Constants.xpathStepperID
                    ),
                    new string[] { Constants.textKey }
                );
                l_simulator.LoadEntityProperty(
                    Util.BuildFullPN(
                        Constants.xpathVariable,
                        Constants.delimiterPath,
                        Constants.xpathSize.ToUpper(),
                        Constants.xpathValue
                    ),
                    new string[] { "0.1" }
                );
                l_simulator.Initialize();
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                    s_resources.GetString(ErrorConstants.ErrCombiStepProc) +
                    "[" + l_defaultStepper + ", " + l_defaultProcess + "]", l_ex);
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

            String mes = m_resources.GetString("ConfirmReset");
            DialogResult r = MessageBox.Show(mes,
                "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (r != DialogResult.OK)
            {
                throw new IgnoreException("Can't " + action + " the object.");
            }
            SimulationStop();
            m_pManager.ChangeStatus(ProjectStatus.Loaded);
        }
        /// <summary>
        /// Adds the list of "EcellObject".
        /// </summary>
        /// <param name="l_ecellObjectList">The list of "EcellObject"</param>
        public void DataAdd(List<EcellObject> l_ecellObjectList)
        {
            DataAdd(l_ecellObjectList, true, true);
        }

        /// <summary>
        /// Adds the list of "EcellObject".
        /// </summary>
        /// <param name="l_ecellObjectList">The list of "EcellObject"</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        public void DataAdd(List<EcellObject> l_ecellObjectList, bool l_isRecorded, bool l_isAnchor)
        {
            foreach (EcellObject l_ecellObject in l_ecellObjectList)
            {
                DataAdd(l_ecellObject, l_isRecorded, l_isAnchor);
            }
        }

        /// <summary>
        /// Adds the "EcellObject".
        /// </summary>
        /// <param name="l_ecellObject">The "EcellObject"</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        public void DataAdd(EcellObject l_ecellObject, bool l_isRecorded, bool l_isAnchor)
        {
            List<EcellObject> l_usableList = new List<EcellObject>();
            string l_message = null;
            string l_type = null;
            bool l_isUndoable = true; // Whether DataAdd action is undoable or not
            try
            {
                if (!this.IsUsable(l_ecellObject))
                    return;
                l_message = "[" + l_ecellObject.ModelID + "][" + l_ecellObject.Key + "]";
                l_type = l_ecellObject.Type;
                
                ConfirmReset("add", l_type);

                if (l_type.Equals(Constants.xpathProcess) || l_type.Equals(Constants.xpathVariable) || l_type.Equals(Constants.xpathText))
                {
                    this.DataAdd4Entity(l_ecellObject, true);
                    l_usableList.Add(l_ecellObject);
                }
                else if (l_type.Equals(Constants.xpathSystem))
                {
                    this.DataAdd4System(l_ecellObject, true);
                    if ("/".Equals(l_ecellObject.Key))
                        l_isUndoable = false;
                    l_usableList.Add(l_ecellObject);
                }
                else if (l_type.Equals(Constants.xpathStepper))
                {
                    // this.DataAdd4Stepper(l_ecellObject);
                    // l_usableList.Add(l_ecellObject);
                }
                else if (l_type.Equals(Constants.xpathModel))
                {
                    l_isUndoable = false;
                    DataAdd4Model(l_ecellObject, l_usableList);
                }
            }
            catch (Exception l_ex)
            {
                l_usableList = null;
                l_message = l_message + m_resources.GetString(ErrorConstants.ErrAddObj);
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
            finally
            {
                if (l_usableList != null && l_usableList.Count > 0)
                {
                    m_isAdded = true;
                    m_pManager.DataAdd(l_usableList);
                    m_isAdded = false;
                    foreach (EcellObject obj in l_usableList)
                    {
                        if (l_isRecorded)
                            m_aManager.AddAction(new DataAddAction(obj, l_isUndoable, l_isAnchor));
                    }
                }
            }
        }

        /// <summary>
        /// Adds the "Model"
        /// </summary>
        /// <param name="l_ecellObject">The "Model"</param>
        /// <param name="l_usableList">The list of the added "EcellObject"</param>
        private void DataAdd4Model(EcellObject l_ecellObject, List<EcellObject> l_usableList)
        {
            List<EcellObject> modelList = m_currentProject.ModelList;

            string l_modelID = l_ecellObject.ModelID;

            string l_message = "[" + l_modelID + "]";
            foreach (EcellObject l_model in modelList)
            {
                if (l_model.ModelID.Equals(l_modelID))
                {
                    throw new Exception(
                        String.Format(
                            m_resources.GetString(ErrorConstants.ErrExistObj),
                            new object[] { l_message }
                        )
                    );
                }
            }
            //
            // Sets the "Model".
            //
            if (modelList == null)
            {
                modelList = new List<EcellObject>();
            }
            modelList.Add(l_ecellObject);
            l_usableList.Add(l_ecellObject);
            //
            // Sets the root "System".
            //
            if (m_currentProject.SystemDic == null)
                m_currentProject.SystemDic = new Dictionary<string, List<EcellObject>>();
            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
            if (!sysDic.ContainsKey(l_modelID))
                sysDic[l_modelID] = new List<EcellObject>();

            Dictionary<string, EcellObject> l_dic = GetDefaultSystem(l_modelID);
            Debug.Assert(l_dic != null);
            sysDic[l_modelID].Add(l_dic[Constants.xpathSystem]);
            l_usableList.Add(l_dic[Constants.xpathSystem]);
            //
            // Sets the default parameter.
            //
            m_currentProject.Initialize(l_modelID);
            foreach (string l_simParam in m_currentProject.InitialCondition.Keys)
            {
                // Sets initial conditions.
                m_currentProject.StepperDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                m_currentProject.StepperDic[l_simParam] = new Dictionary<string, List<EcellObject>>();
                m_currentProject.StepperDic[l_simParam][l_modelID] = new List<EcellObject>();
                m_currentProject.StepperDic[l_simParam][l_modelID].Add(l_dic[Constants.xpathStepper]);
                m_currentProject.LoggerPolicyDic = new Dictionary<string, LoggerPolicy>();
                m_currentProject.LoggerPolicyDic[l_simParam]
                    = new LoggerPolicy(
                        LoggerPolicy.s_reloadStepCount,
                        LoggerPolicy.s_reloadInterval,
                        LoggerPolicy.s_diskFullAction,
                        LoggerPolicy.s_maxDiskSpace
                        );
            }
            //
            // Messages
            //
            MessageCreateEntity(EcellObject.MODEL, l_message);
            MessageCreateEntity(EcellObject.SYSTEM, l_message);
        }

        /// <summary>
        /// Adds the "Stepper".
        /// </summary>
        /// <param name="l_ecellObject">The "Stepper"</param>
        private void DataAdd4Stepper(EcellObject l_ecellObject)
        {
            // Bypasses now.
        }

        /// <summary>
        /// Adds the "System".
        /// </summary>
        /// <param name="l_ecellObject">The System</param>
        /// <param name="l_messageFlag">The flag of the messages</param>
        private void DataAdd4System(EcellObject l_ecellObject, bool l_messageFlag)
        {
            string l_modelID = l_ecellObject.ModelID;
            string l_type = l_ecellObject.Type;
            string l_message = "[" + l_modelID + "][" + l_ecellObject.Key + "]";

            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;

            if (!sysDic.ContainsKey(l_modelID))
                sysDic[l_modelID] = new List<EcellObject>();
            // Check duplicated system.
            foreach (EcellObject l_system in sysDic[l_modelID])
            {
                if (!l_system.Key.Equals(l_ecellObject.Key))
                    continue;
                throw new Exception(l_message + m_resources.GetString(ErrorConstants.ErrAddObj));
            }
            CheckEntityPath(l_ecellObject);
            sysDic[l_modelID].Add(l_ecellObject.Copy());

            if (l_messageFlag)
            {
                MessageCreateEntity(EcellObject.SYSTEM, l_message);
            }

            if (l_ecellObject.Value == null || l_ecellObject.Value.Count <= 0)
                return;
            // Set simulation parameter
            SetSimulationParameter(l_ecellObject, l_modelID, l_type);
        }

        /// <summary>
        /// Adds the "Process" or the "Variable".
        /// </summary>
        /// <param name="l_ecellObject">The "Variable"</param>
        /// <param name="l_messageFlag">The flag of the messages</param>
        private void DataAdd4Entity(EcellObject l_ecellObject, bool l_messageFlag)
        {
            string l_modelID = l_ecellObject.ModelID;
            string l_key = l_ecellObject.Key;
            string l_type = l_ecellObject.Type;
            string l_systemKey = l_ecellObject.ParentSystemID;
            string l_message = "[" + l_modelID + "][" + l_key + "]";

            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
            if (sysDic == null || sysDic.Count <= 0 || !sysDic.ContainsKey(l_modelID))
                throw new Exception(m_resources.GetString(ErrorConstants.ErrFindSuper));

            // Add object.
            bool l_findFlag = false;
            foreach (EcellObject l_system in sysDic[l_modelID])
            {
                if (!l_system.ModelID.Equals(l_modelID) || !l_system.Key.Equals(l_systemKey))
                    continue;
                if (l_system.Children == null)
                    l_system.Children = new List<EcellObject>();
                // Check duplicated object.
                foreach (EcellObject l_child in l_system.Children)
                {
                    if (!l_child.Key.Equals(l_key) || !l_child.Type.Equals(l_type))
                        continue;
                    throw new Exception(
                        string.Format(
                            m_resources.GetString(ErrorConstants.ErrExistObj),
                            new object[] { l_message + " " + l_type.ToLower() }
                        )
                    );
                }
                // Set object.
                CheckEntityPath(l_ecellObject);
                l_system.Children.Add(l_ecellObject.Copy());
                l_findFlag = true;
                if (l_messageFlag)
                {
                    MessageCreateEntity(l_type, l_message);
                }
                break;
            }
            // Throw Error if object is not found.
            if (!l_findFlag)
            {
                l_message = l_message + " " + l_type.ToLower() + m_resources.GetString(ErrorConstants.ErrAddObj);
                if (l_messageFlag)
                {
                    Message(l_message);
                }
                throw new Exception(l_message);
            }

            if (l_ecellObject.Value == null || l_ecellObject.Value.Count <= 0)
                return;
            if (l_ecellObject is EcellText)
                return;

            // Set Simulation param
            SetSimulationParameter(l_ecellObject, l_modelID, l_type);
        }

        /// <summary>
        /// Set simulation parameter
        /// </summary>
        /// <param name="l_ecellObject"></param>
        /// <param name="l_modelID"></param>
        /// <param name="l_type"></param>
        private void SetSimulationParameter(EcellObject l_ecellObject, string l_modelID, string l_type)
        {
            foreach (string l_keyParameterID in m_currentProject.InitialCondition.Keys)
            {
                Dictionary<string, double> initialCondition = m_currentProject.InitialCondition[l_keyParameterID][l_modelID][l_type];
                foreach (EcellData l_data in l_ecellObject.Value)
                {
                    if (!l_data.IsInitialized())
                        continue;

                    double value = 0;
                    if (l_data.Value.IsDouble())
                        value = l_data.Value.CastToDouble();
                    else if (l_data.Value.IsInt())
                        value = l_data.Value.CastToInt();

                    initialCondition[l_data.EntityPath] = value;
                }
            }
        }

        /// <summary>
        /// Changes the "EcellObject".
        /// </summary>
        /// <param name="l_ecellObjectList">The changed "EcellObject"</param>
        public void DataChanged(List<EcellObject> l_ecellObjectList)
        {
            DataChanged(l_ecellObjectList, true, true);
        }
        /// <summary>
        /// Changes the "EcellObject".
        /// </summary>
        /// <param name="l_ecellObjectList">The changed "EcellObject"</param>
        /// <param name="l_isRecorded">The flag whether this action is recorded.</param>
        /// <param name="l_isAnchor">The flag whether this action is anchor.</param>
        public void DataChanged(List<EcellObject> l_ecellObjectList, bool l_isRecorded, bool l_isAnchor)
        {
            foreach (EcellObject obj in l_ecellObjectList)
                DataChanged(obj.ModelID, obj.Key, obj.Type, obj, l_isRecorded, l_isAnchor);
        }

        /// <summary>
        /// Changes the "EcellObject".
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key</param>
        /// <param name="l_type">The type of the "EcellObject"</param>
        /// <param name="l_ecellObject">The changed "EcellObject"</param>
        public void DataChanged(string l_modelID, string l_key, string l_type, EcellObject l_ecellObject)
        {
            DataChanged(l_modelID, l_key, l_type, l_ecellObject, true, true);
        }

        /// <summary>
        /// Changes the "EcellObject".
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key</param>
        /// <param name="l_type">The type of the "EcellObject"</param>
        /// <param name="l_ecellObject">The changed "EcellObject"</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        public void DataChanged(
            string l_modelID,
            string l_key,
            string l_type,
            EcellObject l_ecellObject,
            bool l_isRecorded,
            bool l_isAnchor)
        {
            // StatusCheck
            if (m_currentProject.SimulationStatus == SimulationStatus.Run ||
                m_currentProject.SimulationStatus == SimulationStatus.Suspended)
            {
                EcellObject obj = GetEcellObject(l_modelID, l_key, l_type);
                if (!l_key.Equals(l_ecellObject.Key) ||
                    obj.Value.Count != l_ecellObject.Value.Count)
                    ConfirmReset("change", l_type);

                foreach (EcellData d in obj.Value)
                {
                    foreach (EcellData d1 in l_ecellObject.Value)
                    {
                        if (!d.Name.Equals(d1.Name)) continue;
                        if (!d.Value.ToString().Equals(d1.Value.ToString()))
                        {
                            WrappedPolymorph l_newValue = EcellValue.CastToWrappedPolymorph4EcellValue(d1.Value);
                            m_currentProject.Simulator.SetEntityProperty(d1.EntityPath, l_newValue);
                        }
                        break;
                    }
                }
            }
            string l_message = null;

            try
            {
                // Check null.

                l_message = "[" + l_ecellObject.ModelID + "][" + l_ecellObject.Key + "]";
                if (string.IsNullOrEmpty(l_modelID) || string.IsNullOrEmpty(l_key) || string.IsNullOrEmpty(l_type))
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrNullData) + l_message);

                // Record action
                EcellObject l_oldObj = GetEcellObject(l_modelID, l_key, l_type);

                //if (!l_oldObj.IsPosSet)
                //    m_pManager.SetPosition(l_oldObj);                
                if (l_isRecorded && !m_isAdded)
                    this.m_aManager.AddAction(new DataChangeAction(l_modelID, l_type, l_oldObj.Copy(), l_ecellObject.Copy(), l_isAnchor));

                // Searches the "System".
                List<EcellObject> l_systemList = m_currentProject.SystemDic[l_modelID];
                if (l_systemList == null || l_systemList.Count <= 0)
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindSystem) + l_message);

                // Checks the EcellObject
                CheckEntityPath(l_ecellObject);

                // 4 System & Entity
                if (l_ecellObject.Type.Equals(Constants.xpathSystem))
                {
                    DataChanged4System(l_modelID, l_key, l_type, l_ecellObject, l_isRecorded, l_isAnchor);
                }
                else if (l_ecellObject.Type.Equals(Constants.xpathProcess))
                {
                    DataChanged4Entity(l_modelID, l_key, l_type, l_ecellObject, l_isRecorded, l_isAnchor);
                }
                else if (l_ecellObject.Type.Equals(Constants.xpathText))
                {
                    DataChanged4Entity(l_modelID, l_key, l_type, l_ecellObject, l_isRecorded, l_isAnchor);
                }
                else if (l_ecellObject.Type.Equals(Constants.xpathVariable))
                {
                    DataChanged4Entity(l_modelID, l_key, l_type, l_ecellObject, l_isRecorded, l_isAnchor);
                    if (!l_modelID.Equals(l_ecellObject.ModelID) || !l_key.Equals(l_ecellObject.Key))
                    {
                        List<EcellObject> l_changedProcessList = new List<EcellObject>();
                        CheckVariableReferenceList(l_modelID, l_ecellObject.Key, l_key, l_changedProcessList);
                        foreach (EcellObject l_changedProcess in l_changedProcessList)
                        {
                            this.DataChanged4Entity(
                                l_changedProcess.ModelID, l_changedProcess.Key,
                                l_changedProcess.Type, l_changedProcess, l_isRecorded, l_isAnchor);
                        }
                    }
                }
            }
            catch (Exception l_ex)
            {
                l_message = String.Format(
                    m_resources.GetString(ErrorConstants.ErrUpdate),
                    new object[] { l_ecellObject.Type }
                ) + l_message + " " + l_ecellObject.Type;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + l_ex.StackTrace + "}");
            }
        }

        /// <summary>
        /// Changes the "Variable" or the "Process".
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key</param>
        /// <param name="l_type">The type</param>
        /// <param name="l_ecellObject">The changed "Variable" or the "Process"</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        private void DataChanged4Entity(
            string l_modelID, string l_key, string l_type, EcellObject l_ecellObject, bool l_isRecorded, bool l_isAnchor)
        {
            string l_message = "[" + l_modelID + "][" + l_ecellObject.Key + "]";
            List<EcellObject> l_changedProcessList = new List<EcellObject>();

            // Get parent system.
            EcellObject oldSystem = m_currentProject.GetSystem(l_modelID, EcellObject.GetParentSystemId(l_key));
            EcellObject newSystem = m_currentProject.GetSystem(l_ecellObject.ModelID, l_ecellObject.ParentSystemID);
            if (oldSystem == null || newSystem == null)
                throw new Exception(m_resources.GetString(ErrorConstants.ErrFindSystem) + l_message);

            // Get changed node.
            EcellObject oldNode = m_currentProject.GetEcellObject(l_modelID, l_type, l_key);
            if (oldNode == null)
                throw new Exception(m_resources.GetString(ErrorConstants.ErrExistObj) + l_message);

            this.CheckDifferences(oldNode, l_ecellObject, null);
            if (l_modelID.Equals(l_ecellObject.ModelID)
                && l_key.Equals(l_ecellObject.Key)
                && l_type.Equals(l_ecellObject.Type))
            {
                newSystem.Children.Remove(oldNode);
                newSystem.Children.Add(l_ecellObject);
                this.m_pManager.DataChanged(l_modelID, l_key, l_type, l_ecellObject);
            }
            else
            {
                // Add new object.
                this.DataAdd4Entity(l_ecellObject.Copy(), false);
                this.m_pManager.DataChanged(l_modelID, l_key, l_type, l_ecellObject);
                // Deletes the old object.
                this.DataDelete4Node(l_modelID, l_key, l_type, false, l_isRecorded, l_isAnchor);
            }

            return;
        }

        /// <summary>
        /// Changes the "System".
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key</param>
        /// <param name="l_type">The type</param>
        /// <param name="l_ecellObject">The changed "System"</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        private void DataChanged4System(string l_modelID, string l_key, string l_type, EcellObject l_ecellObject, bool l_isRecorded, bool l_isAnchor)
        {
            string l_message = "[" + l_ecellObject.ModelID + "][" + l_ecellObject.Key + "]";
            m_currentProject.SortSystems();
            List<EcellObject> l_systemList = m_currentProject.SystemDic[l_modelID];

            if (l_modelID.Equals(l_ecellObject.ModelID)
                && l_key.Equals(l_ecellObject.Key)
                && l_type.Equals(l_ecellObject.Type))
            {
                // Changes some properties.
                for (int i = 0; i < l_systemList.Count; i++)
                {
                    if (!l_systemList[i].Key.Equals(l_key))
                        continue;

                    this.CheckDifferences(l_systemList[i], l_ecellObject, null);
                    l_systemList[i] = l_ecellObject.Copy();
                    this.m_pManager.DataChanged(l_modelID, l_key, l_type, l_ecellObject);
                    break;
                }
                return;
            }

            // Changes the key.
            Dictionary<string, string> l_variableKeyDic = new Dictionary<string, string>();
            Dictionary<string, string> l_processKeyDic = new Dictionary<string, string>();
            List<string> l_createdSystemKeyList = new List<string>();
            List<string> l_deletedSystemKeyList = new List<string>();
            List<EcellObject> tempList = new List<EcellObject>();
            tempList.AddRange(l_systemList);

            foreach (EcellObject l_system in tempList)
            {
                if (!l_system.Key.Equals(l_key) && !l_system.Key.StartsWith(l_key + Constants.delimiterPath))
                    continue;

                // Adds the new "System" object.
                string l_newKey = l_ecellObject.Key + l_system.Key.Substring(l_key.Length);
                EcellObject l_newSystem
                    = EcellObject.CreateObject(l_modelID, l_newKey, l_system.Type, l_system.Classname, l_system.Value);
                l_newSystem.SetPosition(l_system);
                CheckEntityPath(l_newSystem);
                DataAdd4System(l_newSystem, false);
                CheckDifferences(l_system, l_newSystem, null);
                m_pManager.DataChanged(l_modelID, l_system.Key, l_type, l_newSystem);
                l_createdSystemKeyList.Add(l_newKey);

                // Deletes the old "System" object.
                l_deletedSystemKeyList.Add(l_system.Key);
                // 4 Children
                if (l_system.Children == null || l_system.Children.Count <= 0)
                    continue;

                List<EcellObject> l_instanceList = new List<EcellObject>();
                l_instanceList.AddRange(l_system.Children);
                foreach (EcellObject l_childObject in l_instanceList)
                {
                    EcellObject l_copy = l_childObject.Copy();
                    string l_childKey = l_childObject.Key;
                    string l_keyName = l_childObject.Name;
                    if ((l_system.Key.Equals(l_key)))
                    {
                        l_copy.Key = l_ecellObject.Key + Constants.delimiterColon + l_keyName;
                    }
                    else
                    {
                        l_copy.Key = l_ecellObject.Key + l_copy.Key.Substring(l_key.Length);
                    }
                    CheckEntityPath(l_copy);
                    if (l_copy.Type.Equals(Constants.xpathVariable))
                    {
                        l_variableKeyDic[l_childKey] = l_copy.Key;
                        this.DataChanged4Entity(l_copy.ModelID, l_childKey, l_copy.Type, l_copy, l_isRecorded, l_isAnchor);
                    }
                    else
                    {
                        l_processKeyDic[l_childKey] = l_copy.Key;
                    }
                }
            }
            // Checks all processes.
            m_currentProject.SortSystems();
            l_systemList = m_currentProject.SystemDic[l_modelID];
            foreach (EcellObject l_system in l_systemList)
            {
                if (l_createdSystemKeyList.Contains(l_system.Key))
                    continue;
                if (l_system.Children == null || l_system.Children.Count <= 0)
                    continue;

                List<EcellObject> l_instanceList = new List<EcellObject>();
                l_instanceList.AddRange(l_system.Children);
                foreach (EcellObject l_childObject in l_instanceList)
                {
                    if (!l_childObject.Type.Equals(Constants.xpathProcess))
                        continue;

                    bool l_changedFlag = false;
                    // 4 VariableReferenceList
                    EcellObject l_dest = null;
                    if (CheckVariableReferenceList(l_childObject, ref l_dest, l_variableKeyDic))
                    {
                        l_changedFlag = true;
                    }
                    // 4 key
                    string l_oldKey = l_dest.Key;
                    string l_keyName = l_oldKey.Split(Constants.delimiterColon.ToCharArray())[1];
                    if (l_processKeyDic.ContainsKey(l_oldKey))
                    {
                        l_dest.Key = l_processKeyDic[l_oldKey];
                        CheckEntityPath(l_dest);
                        l_changedFlag = true;
                    }
                    if (l_changedFlag)
                    {
                        this.DataChanged4Entity(l_dest.ModelID, l_oldKey, l_dest.Type, l_dest, l_isRecorded, l_isAnchor);
                    }
                }
            }
            // Deletes old "System"s.
            for (int i = l_deletedSystemKeyList.Count -1; i >= 0; i--)
            {
                this.DataDelete4System(l_modelID, l_deletedSystemKeyList[i], false);
            }
    }

        /// <summary>
        /// Deletes the "EcellObject" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key of the "EcellObject"</param>
        /// <param name="l_type">The type of the "EcellObject"</param>
        public void DataDelete(string l_modelID, string l_key, string l_type)
        {
            DataDelete(l_modelID, l_key, l_type, true, true);
        }

        /// <summary>
        /// Deletes the "EcellObject" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key of the "EcellObject"</param>
        /// <param name="l_type">The type of the "EcellObject"</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        public void DataDelete(string l_modelID, string l_key, string l_type, bool l_isRecorded, bool l_isAnchor)
        {
            ConfirmReset("delete", l_type);

            string l_message = null;
            EcellObject deleteObj = null;
            try
            {
                if (string.IsNullOrEmpty(l_modelID))
                    return;
                l_message = "[" + l_modelID + "][" + l_key + "]";

                deleteObj = GetEcellObject(l_modelID, l_key, l_type);

                if (string.IsNullOrEmpty(l_key))
                {
                    DataDelete4Model(l_modelID);
                }
                else if (l_key.Contains(":"))
                { // not system
                    DataDelete4Node(l_modelID, l_key, l_type, true, l_isRecorded, false);
                }
                else
                { // system
                    DataDelete4System(l_modelID, l_key, true);
                }
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrDelete) + l_message + " " + l_type;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
            finally
            {
                m_pManager.DataDelete(l_modelID, l_key, l_type);
                if (l_isRecorded)
                    m_aManager.AddAction(new DataDeleteAction(l_modelID, l_key, l_type, deleteObj, l_isAnchor));
            }
        }

        /// <summary>
        /// Deletes the "Model" using the model ID.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        private void DataDelete4Model(string l_modelID)
        {
            string l_message = "[" + l_modelID + "]";
            //
            // Delete the "Model".
            //
            bool l_isDelete = false;
            foreach (EcellObject obj in m_currentProject.ModelList)
            {
                if (obj.ModelID == l_modelID)
                {
                    m_currentProject.ModelList.Remove(obj);
                    l_isDelete = true;
                    break;
                }
            }
            if (!l_isDelete)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrFindModel) + l_message);
            }
            //
            // Deletes "System"s.
            //
            if (m_currentProject.SystemDic.ContainsKey(l_modelID))
            {
                m_currentProject.SystemDic.Remove(l_modelID);
            }
            //
            // Deletes "Stepper"s.
            //
            foreach (string l_param in m_currentProject.StepperDic.Keys)
            {
                if (m_currentProject.StepperDic[l_param].ContainsKey(l_modelID))
                {
                    m_currentProject.StepperDic[l_param].Remove(l_modelID);
                }
            }
            MessageDeleteEntity(EcellObject.MODEL, l_message);
        }

        /// <summary>
        /// Deletes the "Process" or the "Variable" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="l_model">The model ID</param>
        /// <param name="l_key">The key of the "EcellObject"</param>
        /// <param name="l_type">The type of the "EcellObject"</param>
        /// <param name="l_messageFlag">The flag of the message</param>
        /// <param name="l_isRecorded">The flag whether this action is recorded.</param>
        /// <param name="l_isAnchor">The flag whether this action is anchor.</param>
        private void DataDelete4Node(
            string l_model,
            string l_key,
            string l_type,
            bool l_messageFlag,
            bool l_isRecorded,
            bool l_isAnchor)
        {
            if (!m_currentProject.SystemDic.ContainsKey(l_model))
                return;

            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>> initialCondition = this.m_currentProject.InitialCondition;

            string l_message = "[" + l_model + "][" + l_key + "]";
            List<EcellObject> l_delList = new List<EcellObject>();

            List<EcellObject> sysList = m_currentProject.SystemDic[l_model];
            foreach (EcellObject system in sysList)
            {
                if (system.ModelID != l_model || system.Key != EcellObject.GetParentSystemId(l_key))
                    continue;
                if (system.Children == null)
                    continue;

                foreach (EcellObject l_child in system.Children)
                {
                    if (l_child.Key == l_key && l_child.Type == l_type)
                        l_delList.Add(l_child);
                }
                foreach (EcellObject l_child in l_delList)
                {
                    system.Children.Remove(l_child);
                    if (l_messageFlag)
                    {
                        MessageDeleteEntity(l_type, l_message);
                    }

                    if (l_child.Value == null || l_child.Value.Count <= 0)
                        continue;
                    if (l_child is EcellText)
                        continue;

                    foreach (string l_keyParameterID in initialCondition.Keys)
                    {
                        Dictionary<string, double> condition = initialCondition[l_keyParameterID][l_child.ModelID][l_child.Type];
                        foreach (EcellData l_data in l_child.Value)
                        {
                            if (!l_data.Settable)
                                continue;
                            if (!condition.ContainsKey(l_data.EntityPath))
                                continue;
                            condition.Remove(l_data.EntityPath);
                        }
                    }
                }
                if (l_messageFlag)
                {
                    this.DataDelete4VariableReferenceList(l_delList, l_isRecorded, l_isAnchor);
                }
                l_delList.Clear();
            }
        }

        /// <summary>
        /// Deletes entries of the "VariableRefereceList".
        /// </summary>
        /// <param name="l_delList">The list of the deleted "Variable"</param>
        /// <param name="l_isRecorded">The flag whether this action is recorded.</param>
        /// <param name="l_isAnchor">The flag whether this action is anchor.</param>
        private void DataDelete4VariableReferenceList(List<EcellObject> l_delList, bool l_isRecorded, bool l_isAnchor)
        {
            if (l_delList == null || l_delList.Count <= 0)
                return;

            foreach (EcellObject l_del in l_delList)
            {
                if (!l_del.Type.Equals(EcellObject.VARIABLE))
                    continue;

                string l_variableKey = l_del.Key;
                foreach (EcellObject l_system in m_currentProject.SystemDic[l_del.ModelID])
                {
                    List<EcellObject> changeList = new List<EcellObject>();
                    foreach (EcellObject l_child in l_system.Children)
                    {
                        bool l_changedFlag = false;
                        if (!l_child.Type.Equals(EcellObject.PROCESS))
                            continue;

                        EcellProcess l_process = (EcellProcess)l_child.Copy();
                        List<EcellReference> l_er = new List<EcellReference>();
                        foreach (EcellReference er in l_process.ReferenceList)
                        {
                            if (er.Key.Equals(l_variableKey))
                                l_changedFlag = true;
                            else
                                l_er.Add(er);
                        }
                        l_process.ReferenceList = l_er;

                        if (l_changedFlag)
                            changeList.Add(l_process);
                    }
                    foreach (EcellObject l_change in changeList)
                    {
                        this.DataChanged(l_change.ModelID, l_change.Key, l_change.Type, l_change, l_isRecorded, l_isAnchor);
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
            m_pManager.SetObservedData(data);
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public void RemoveObservedData(EcellObservedData data)
        {
            if (m_observedList.ContainsKey(data.Key))
                m_observedList.Remove(data.Key);
            m_pManager.RemoveObservedData(data);
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
            m_pManager.SetParameterData(data);
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
            m_pManager.RemoveParameterData(data);
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
            List<EcellObject> l_list = m_currentProject.SystemDic[modelID];
            foreach (EcellObject l_sys in l_list)
            {
                // Check systems.
                if (key.Equals(l_sys.Key) && type.Equals(l_sys.Type))
                    return true;
                // Continue if system has no node.
                if (l_sys.Children == null)
                    continue;
                // Check processes and variables
                foreach (EcellObject subEo in l_sys.Children)
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
                MessageBox.Show(m_resources.GetString(ErrorConstants.ErrDelRoot),
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
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
                    MessageBox.Show(newKey + m_resources.GetString(ErrorConstants.ErrExistObj),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
            }
            // Confirm system merge.
            DialogResult result = MessageBox.Show(m_resources.GetString("ConfirmMerge"),
                "Merge",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Cancel)
                return;

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
        /// <param name="l_modelID">Model id of an added system</param>
        /// <param name="l_obj">An added system</param>
        /// <param name="l_sysList">Child systems of added system</param>
        /// <param name="l_objList">Child objects of added system</param>
        public void SystemAddAndMove(string l_modelID,
            EcellObject l_obj,
            List<EcellObject> l_sysList,
            List<EcellObject> l_objList)
        {
            // Temporary delete objects to be moved into a new system.
            foreach (EcellObject sys in l_sysList)
                DataDelete(l_modelID, sys.Key, "System", false, false);

            string[] el = l_obj.Key.Split(new char[] { '/' });
            int delPoint = el.Length - 1;
            List<EcellObject> list = new List<EcellObject>();
            foreach (EcellObject obj in l_sysList)
            {
                String orgKey = obj.Key;
                String newKey = "";
                string[] nel = orgKey.Split(new char[] { '/' });
                for (int i = 0; i < nel.Length; i++)
                {
                    if (i == delPoint) continue;
                    if (nel[i] == "") newKey = "";
                    else newKey = newKey + "/" + nel[i];
                }
                DataDelete(l_modelID, newKey, "System", false, false);
            }
            foreach (EcellObject obj in l_objList)
            {
                String iNewKey = "";
                string[] iel = obj.Key.Split(new char[] { '/' });
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
                DataDelete(l_modelID, iNewKey, obj.Type, false, false);
            }

            // Add a system.
            List<EcellObject> l_list = new List<EcellObject>();
            l_list.Add(l_obj);

            foreach (EcellObject l_sys in l_sysList)
                l_list.Add(l_sys);

            foreach (EcellObject l_object in l_objList)
                l_list.Add(l_object);

            DataAdd(l_list);

        }

        /// <summary>
        /// Deletes the "System" using the model ID and the key of the "EcellObject".
        /// </summary>
        /// <param name="l_model">The model ID</param>
        /// <param name="l_key">The key of the "EcellObject"</param>
        /// <param name="l_messageFlag">The flag of the messages</param>
        private void DataDelete4System(string l_model, string l_key, bool l_messageFlag)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>> initialCondition = this.m_currentProject.InitialCondition;
            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;

            string l_message = "[" + l_model + "][" + l_key + "]";
            List<EcellObject> l_delList = new List<EcellObject>();
            if (!sysDic.ContainsKey(l_model))
                return;
            foreach (EcellObject obj in sysDic[l_model])
            {
                if (obj.ModelID != l_model || !obj.Key.StartsWith(l_key))
                    continue;
                if (obj.Key.Length == l_key.Length || obj.Key[l_key.Length] == '/')
                    l_delList.Add(obj);
            }
            foreach (EcellObject l_obj in l_delList)
            {
                sysDic[l_model].Remove(l_obj);
                if (l_obj.Type == "System")
                {
                    foreach (string l_keyParamID in initialCondition.Keys)
                    {
                        foreach (string l_delModel in initialCondition[l_keyParamID].Keys)
                        {
                            foreach (string l_cType in initialCondition[l_keyParamID][l_delModel].Keys)
                            {
                                String delKey = l_cType + ":" + l_key;
                                List<String> delKeyList = new List<string>();
                                foreach (String entKey in initialCondition[l_keyParamID][l_delModel][l_cType].Keys)
                                {
                                    if (entKey.StartsWith(delKey))
                                        delKeyList.Add(entKey);
                                }
                                foreach (String entKey in delKeyList)
                                {
                                    initialCondition[l_keyParamID][l_delModel][l_cType].Remove(entKey);
                                }
                            }
                        }
                    }
                }
                // Record deletion of child system. 
                if (!l_obj.Key.Equals(l_key))
                    m_aManager.AddAction(new DataDeleteAction(l_obj.ModelID, l_obj.Key, l_obj.Type, l_obj, false));
                if (l_messageFlag)
                {
                    MessageDeleteEntity(EcellObject.SYSTEM, l_message);
                }
            }
        }

        /// <summary>
        /// Deletes the parameter.
        /// </summary>
        /// <param name="l_parameterID"></param>
        public void DeleteSimulationParameter(string l_parameterID)
        {
            DeleteSimulationParameter(l_parameterID, true, true);
        }

        /// <summary>
        /// Deletes the parameter.
        /// </summary>
        /// <param name="l_parameterID"></param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        public void DeleteSimulationParameter(string l_parameterID, bool l_isRecorded, bool l_isAnchor)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_parameterID + "]";
                //
                // Initializes.
                //
                if (l_parameterID == null || l_parameterID.Length <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrNullData) + "[ParameterID]");
                }
                this.SetDefaultDir();
                if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrBaseDir));
                }
                if (m_currentProject.StepperDic.ContainsKey(l_parameterID))
                {
                    m_currentProject.StepperDic.Remove(l_parameterID);
                    string l_simulationDirName
                            = this.m_defaultDir + Constants.delimiterPath
                            + m_currentProject.Name + Constants.delimiterPath + Constants.xpathParameters;
                    string l_pattern
                            = "_????_??_??_??_??_??_" + l_parameterID + Constants.FileExtXML;
                    if (Directory.Exists(l_simulationDirName))
                    {
                        foreach (string l_fileName in Directory.GetFiles(l_simulationDirName, l_pattern))
                        {
                            File.Delete(l_fileName);
                        }
                        string l_simulationFileName
                                = l_simulationDirName + Constants.delimiterPath + l_parameterID + Constants.FileExtXML;
                        File.Delete(l_simulationFileName);
                    }
                    m_currentProject.LoggerPolicyDic.Remove(l_parameterID);
                    m_pManager.ParameterDelete(m_currentProject.Name, l_parameterID);
                    MessageDeleteEntity("Simulation Parameter", l_message);
                }
                else
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindSimParam) + l_message);
                }

                if (l_isRecorded)
                    m_aManager.AddAction(new DeleteSimParamAction(l_parameterID, l_isAnchor));
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrDeleteSimParam) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Deletes the "Stepper".
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <param name="l_stepper">The "Stepper"</param>
        public void DeleteStepperID(string l_parameterID, EcellObject l_stepper)
        {
            DeleteStepperID(l_parameterID, l_stepper, true);
        }

        /// <summary>
        /// Deletes the "Stepper".
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <param name="l_stepper">The "Stepper"</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        public void DeleteStepperID(string l_parameterID, EcellObject l_stepper, bool l_isRecorded)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_parameterID + "][" + l_stepper.ModelID + "][" + l_stepper.Key + "]";
                int l_point = -1;
                List<EcellObject> l_storedStepperList
                    = m_currentProject.StepperDic[l_parameterID][l_stepper.ModelID];
                for (int i = 0; i < l_storedStepperList.Count; i++)
                {
                    if (l_storedStepperList[i].Key.Equals(l_stepper.Key))
                    {
                        l_point = i;
                        break;
                    }
                }
                if (l_point != -1)
                {
                    l_storedStepperList.RemoveAt(l_point);
                    Message("Delete Stepper: " + l_message);
                }
                if (l_isRecorded)
                    m_aManager.AddAction(new DeleteStepperAction(l_parameterID, l_stepper));
                if (m_currentProject.SimulationParam.Equals(l_parameterID))
                {
                    m_pManager.DataDelete(l_stepper.ModelID, l_stepper.Key, l_stepper.Type);
                }
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrDelete) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Tests whether the full ID exists.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_fullID">The full ID</param>
        /// <returns>true if the full ID exists; false otherwise</returns>
        public bool Exists(string l_modelID, string l_fullID)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_modelID + "][" + l_fullID + "]";
                string[] l_infos = l_fullID.Split(Constants.delimiterColon.ToCharArray());
                if (l_infos.Length != 3)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrIDUnform) + l_message);
                }
                else if (!l_infos[0].Equals(Constants.xpathSystem)
                        && !l_infos[0].Equals(Constants.xpathProcess)
                        && !l_infos[0].Equals(Constants.xpathVariable))
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrIDUnform) + l_message);
                }
                string l_key = null;
                if (l_infos[1].Equals("") && l_infos[2].Equals(Constants.delimiterPath))
                {
                    l_key = l_infos[2];
                }
                else
                {
                    l_key = l_infos[1] + Constants.delimiterColon + l_infos[2];
                }
                //
                // Checks the full ID.
                //
                List<EcellObject> systemList = m_currentProject.SystemDic[l_modelID];
                if (systemList == null || systemList.Count <= 0)
                {
                    return false;
                }
                foreach (EcellObject l_system in systemList)
                {
                    if (l_infos[0].Equals(Constants.xpathSystem))
                    {
                        if (l_system.Type.Equals(l_infos[0]) && l_system.Key.Equals(l_key))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (l_system.Children == null
                                || l_system.Children.Count <= 0)
                        {
                            continue;
                        }
                        foreach (EcellObject l_entity in l_system.Children)
                        {
                            if (l_entity.Type.Equals(l_infos[0]) && l_entity.Key.Equals(l_key))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrCheckID) + l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Exports the models to ths designated file.
        /// </summary>
        /// <param name="l_modelIDList">The list of the model ID</param>
        /// <param name="l_fileName">The designated file</param>
        public void ExportModel(List<string> l_modelIDList, string l_fileName)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_fileName + "]";
                //
                // Initializes.
                //
                if (l_modelIDList == null || l_modelIDList.Count <= 0)
                {
                    return;
                }
                else if (l_fileName == null || l_fileName.Length <= 0)
                {
                    return;
                }
                //
                // Checks the parent directory.
                //
                string l_parentPath = Path.GetDirectoryName(l_fileName);
                if (l_parentPath != null && l_parentPath.Length > 0 && !Directory.Exists(l_parentPath))
                {
                    Directory.CreateDirectory(l_parentPath);
                }
                //
                // Searchs the "Stepper" & the "System".
                //
                List<EcellObject> l_storedStepperList = new List<EcellObject>();
                List<EcellObject> l_storedSystemList = new List<EcellObject>();

                Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
                Dictionary<string, List<EcellObject>> stepperDic = m_currentProject.StepperDic[m_currentProject.SimulationParam];

                foreach (string l_modelID in l_modelIDList)
                {
                    l_storedStepperList.AddRange(stepperDic[l_modelID]);
                    l_storedSystemList.AddRange(sysDic[l_modelID]);
                }
                if (l_storedStepperList == null || l_storedStepperList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindStepper));
                }
                else if (l_storedSystemList == null || l_storedSystemList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindSystem));
                }
                //
                // Exports.
                //
                l_storedStepperList.AddRange(l_storedSystemList);
                EmlWriter.Create(l_fileName, l_storedStepperList, false);
                Message("Export Model: " + l_message);
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrExportFile) + l_fileName + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the current logger policy.
        /// </summary>
        /// <returns>The current logger policy</returns>
        private WrappedPolymorph GetCurrentLoggerPolicy()
        {
            List<WrappedPolymorph> l_policyList = new List<WrappedPolymorph>();
            string simParam = m_currentProject.SimulationParam;
            l_policyList.Add(new WrappedPolymorph(m_currentProject.LoggerPolicyDic[simParam].m_reloadStepCount));
            l_policyList.Add(new WrappedPolymorph(m_currentProject.LoggerPolicyDic[simParam].m_reloadInterval));
            l_policyList.Add(new WrappedPolymorph(m_currentProject.LoggerPolicyDic[simParam].m_diskFullAction));
            l_policyList.Add(new WrappedPolymorph(m_currentProject.LoggerPolicyDic[simParam].m_maxDiskSpace));
            return new WrappedPolymorph(l_policyList);
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
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key</param>
        /// <returns>The list of a "EcellObject"</returns>
        public List<EcellObject> GetData(string l_modelID, string l_key)
        {
            Dictionary<string, List<EcellObject>> sysDic = m_currentProject.SystemDic;
            List<EcellObject> l_ecellObjectList = new List<EcellObject>();
            try
            {
                // Returns all stored "EcellObject".
                if (string.IsNullOrEmpty(l_modelID))
                {
                    // Searches the model.
                    l_ecellObjectList.AddRange(m_currentProject.ModelList);
                    // Searches the "System".
                    m_currentProject.SortSystems();
                    foreach (List<EcellObject> systemList in sysDic.Values)
                    {
                        l_ecellObjectList.AddRange(systemList);
                    }
                }
                // Searches the model.
                else if (string.IsNullOrEmpty(l_key))
                {
                    foreach (EcellObject l_model in m_currentProject.ModelList)
                    {
                        if (!l_model.ModelID.Equals(l_modelID))
                            continue;
                        l_ecellObjectList.Add(l_model.Copy());
                        break;
                    }
                    l_ecellObjectList.AddRange(sysDic[l_modelID]);
                }
                // Searches the "System".
                else
                {
                    foreach (EcellObject l_system in sysDic[l_modelID])
                    {
                        if (!l_key.Equals(l_system.Key))
                            continue;
                        l_ecellObjectList.Add(l_system.Copy());
                        break;
                    }
                }
                return l_ecellObjectList;
            }
            catch (Exception l_ex)
            {
                l_ex.ToString();
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
        /// Returns the singleton of this.
        /// </summary>
        /// <returns>The singleton</returns>
        public static DataManager GetDataManager()
        {
            if (s_instance == null)
            {
                s_instance = new DataManager();
            }
            return s_instance;
        }

        /// <summary>
        /// Returns the dictionary of the default "System" and the "Stepper".
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <returns>The dictionary of the default "System" and the "Stepper"</returns>
        private Dictionary<string, EcellObject> GetDefaultSystem(string l_modelID)
        {
            Dictionary<string, EcellObject> l_dic = new Dictionary<string, EcellObject>();
            EcellObject l_systemEcellObject = null;
            EcellObject l_stepperEcellObject = null;
            WrappedSimulator l_simulator = m_currentProject.Simulator;
            BuildDefaultSimulator(l_simulator, null, null);
            l_systemEcellObject
                    = EcellObject.CreateObject(
                        l_modelID,
                        Constants.delimiterPath,
                        Constants.xpathSystem,
                        Constants.xpathSystem,
                        null);
            DataStorer.DataStored4System(
                    l_simulator,
                    l_systemEcellObject,
                    new Dictionary<string, double>());
            l_stepperEcellObject
                    = EcellObject.CreateObject(
                        l_modelID,
                        Constants.textKey,
                        Constants.xpathStepper,
                        "",
                        null);
            DataStorer.DataStored4Stepper(l_simulator, l_stepperEcellObject);
            l_dic[Constants.xpathSystem] = l_systemEcellObject;
            l_dic[Constants.xpathStepper] = l_stepperEcellObject;
            return l_dic;
        }

        /// <summary>
        /// Returns the initial condition.
        /// </summary>
        /// <param name="l_paremterID">The parameter ID</param>
        /// <param name="l_modelID">The model ID</param>
        /// <returns>The initial condition</returns>
        public Dictionary<string, Dictionary<string, double>>
                GetInitialCondition(string l_paremterID, string l_modelID)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_paremterID + "][" + l_modelID + "]";
                return this.m_currentProject.InitialCondition[l_paremterID][l_modelID];
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                        m_resources.GetString(ErrorConstants.ErrInitParam) + l_message + " {"
                        + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the initial condition.
        /// </summary>
        /// <param name="l_paremterID">The parameter ID</param>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_type">The data type</param>
        /// <returns>The initial condition</returns>
        public Dictionary<string, double>
                GetInitialCondition(string l_paremterID, string l_modelID, string l_type)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_paremterID + "][" + l_modelID + "][" + l_type + "]";
                return this.m_currentProject.InitialCondition[l_paremterID][l_modelID]
                        [l_type];
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                        m_resources.GetString(ErrorConstants.ErrInitParam) + l_message + " {"
                        + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_startTime"></param>
        /// <param name="l_endTime"></param>
        /// <param name="l_interval"></param>
        /// <param name="l_fullID"></param>
        /// <returns></returns>
        public LogData GetLogData(double l_startTime, double l_endTime, double l_interval, string l_fullID)
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
                return this.GetUniqueLogData(l_startTime, l_endTime, l_interval, l_fullID);
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrGetData) + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the list of the "LogData".
        /// </summary>
        /// <param name="l_startTime">The start time</param>
        /// <param name="l_endTime">The end time</param>
        /// <param name="l_interval">The interval</param>
        /// <returns>The list of the "LogData"</returns>
        public List<LogData> GetLogData(double l_startTime, double l_endTime, double l_interval)
        {
            List<LogData> l_logDataList = new List<LogData>();
            try
            {
                // Initialize
                if (m_currentProject.Simulator == null)
                    return null;
                if (m_currentProject.LogableEntityPathDic == null ||
                    m_currentProject.LogableEntityPathDic.Count == 0)
                    return null;

                WrappedPolymorph l_loggerList = m_currentProject.Simulator.GetLoggerList();
                if (!l_loggerList.IsList())
                    return l_logDataList;

                foreach (WrappedPolymorph l_logger in l_loggerList.CastToList())
                {
                    l_logDataList.Add(
                            this.GetUniqueLogData(l_startTime, l_endTime, l_interval, l_logger.CastToString()));
                }
                return l_logDataList;
            }
            catch (Exception l_ex)
            {
                l_logDataList = null;
                throw new Exception(m_resources.GetString(ErrorConstants.ErrGetData) + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_startTime"></param>
        /// <param name="l_endTime"></param>
        /// <param name="l_interval"></param>
        /// <param name="l_fullID"></param>
        /// <returns></returns>
        private LogData GetUniqueLogData(
                double l_startTime,
                double l_endTime,
                double l_interval,
                string l_fullID)
        {
            if (l_startTime < 0.0)
            {
                l_startTime = 0.0;
            }
            if (l_endTime <= 0.0)
            {
                l_endTime = m_currentProject.Simulator.GetCurrentTime();
            }
            if (this.m_simulationTimeLimit > 0.0 && l_endTime > this.m_simulationTimeLimit)
            {
                l_endTime = this.m_simulationTimeLimit;
            }
            if (l_startTime > l_endTime)
            {
                double l_tmpTime = l_startTime;
                l_startTime = l_endTime;
                l_endTime = l_tmpTime;
            }
            WrappedDataPointVector l_dataPointVector = null;
            if (l_interval <= 0.0)
            {
                lock (m_currentProject.Simulator)
                {
                    l_dataPointVector
                        = m_currentProject.Simulator.GetLoggerData(
                            l_fullID,
                            l_startTime,
                            l_endTime
                            );
                }
            }
            else
            {
                lock (m_currentProject.Simulator)
                {
                    l_dataPointVector
                        = m_currentProject.Simulator.GetLoggerData(
                            l_fullID,
                            l_startTime,
                            l_endTime,
                            l_interval
                            );
                }
            }
            List<LogValue> l_logValueList = new List<LogValue>();
            double l_lastTime = -1.0;
            for (int i = 0; i < l_dataPointVector.GetArraySize(); i++)
            {
                if (l_lastTime == l_dataPointVector.GetTime(i))
                {
                    continue;
                }
                LogValue l_logValue = new LogValue(
                    l_dataPointVector.GetTime(i),
                    l_dataPointVector.GetValue(i),
                    l_dataPointVector.GetAvg(i),
                    l_dataPointVector.GetMin(i),
                    l_dataPointVector.GetMax(i)
                    );
                l_logValueList.Add(l_logValue);
                l_lastTime = l_dataPointVector.GetTime(i);
            }
            string l_modelID = null;
            if (m_currentProject.LogableEntityPathDic.ContainsKey(l_fullID))
            {
                l_modelID = m_currentProject.LogableEntityPathDic[l_fullID];
            }
            string l_key = null;
            string l_type = null;
            string l_propName = null;
            this.Split4EntityPath(ref l_key, ref l_type, ref l_propName, l_fullID);
            if (l_logValueList.Count == 1 && l_logValueList[0].time == 0.0)
            {
                LogValue l_logValue =
                    new LogValue(
                       l_endTime,
                       l_logValueList[0].value,
                       l_logValueList[0].avg,
                       l_logValueList[0].min,
                       l_logValueList[0].max
                       );
                l_logValueList.Add(l_logValue);
            }
            LogData l_logData = new LogData(
                l_modelID,
                l_key,
                l_type,
                l_propName,
                l_logValueList
                );
            return l_logData;
        }

        /// <summary>
        /// Returns the list of the registred logger.
        /// </summary>
        /// <returns></returns>
        public List<string> GetLoggerList()
        {
            List<string> l_loggerList = new List<string>();
            try
            {
                WrappedPolymorph l_polymorphList = m_currentProject.Simulator.GetLoggerList();
                if (l_polymorphList.IsList())
                {
                    foreach (WrappedPolymorph l_polymorph in l_polymorphList.CastToList())
                    {
                        l_loggerList.Add(l_polymorph.CastToString());
                    }
                }
                return l_loggerList;
            }
            catch (Exception l_ex)
            {
                l_ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// Returns the "LoggerPolicy".
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <returns>The "LoggerPolicy"</returns>
        public LoggerPolicy GetLoggerPolicy(string l_parameterID)
        {
            return m_currentProject.LoggerPolicyDic[l_parameterID];
        }

        /// <summary>
        /// Returns the next event.
        /// </summary>
        /// <returns>The current simulation time, The stepper</returns>
        public ArrayList GetNextEvent()
        {
            try
            {
                ArrayList l_list = new ArrayList();
                List<WrappedPolymorph> l_polymorphList
                        = m_currentProject.Simulator.GetNextEvent().CastToList();
                l_list.Add(l_polymorphList[0].CastToDouble());
                l_list.Add(l_polymorphList[1].CastToString());
                return l_list;
            }
            catch (Exception l_ex)
            {
                l_ex.ToString();
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
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_entityName">The entity name</param>
        /// <returns></returns>
        public List<string> GetEntityList(string l_modelID, string l_entityName)
        {
            List<string> l_entityList = new List<string>();
            try
            {
                foreach (EcellObject l_system in m_currentProject.SystemDic[l_modelID])
                {
                    if (l_entityName.Equals(Constants.xpathSystem))
                    {
                        string l_parentPath = l_system.ParentSystemID;
                        string l_childPath = l_system.Name;
                        l_entityList.Add(
                            Constants.xpathSystem + Constants.delimiterColon
                            + l_parentPath + Constants.delimiterColon + l_childPath);
                    }
                    else
                    {
                        if (l_system.Children == null || l_system.Children.Count <= 0)
                            continue;
                        foreach (EcellObject l_entity in l_system.Children)
                        {
                            if (!l_entity.Type.Equals(l_entityName))
                                continue;
                            l_entityList.Add(l_entity.Type + Constants.delimiterColon + l_entity.Key);
                        }
                    }
                }
                return l_entityList;
            }
            catch (Exception l_ex)
            {
                l_entityList.Clear();
                l_entityList = null;
                throw new Exception(m_resources.GetString(ErrorConstants.ErrFindEnt) +
                    " [" + l_entityName + "][" + l_modelID + "]. {"
                    + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Get the EcellValue from fullPath.
        /// </summary>
        /// <param name="l_fullPN"></param>
        /// <returns></returns>
        public EcellValue GetEntityProperty(string l_fullPN)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_fullPN + "]";
                if (m_currentProject.Simulator == null)
                {
                    return null;
                }
                EcellValue l_value
                    = new EcellValue(m_currentProject.Simulator.GetEntityProperty(l_fullPN));
                return l_value;
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrGetProp) +
                    l_message + " {" + l_ex.ToString() + "}");
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
            Util.GetDMDirs(null);
            return m_currentProject.DmDic[Constants.xpathProcess];
        }

        /// <summary>
        /// Check whether this dm is able to add the property.
        /// </summary>
        /// <param name="l_dmName">dm Name.</param>
        /// <returns>if this dm is enable to add property, return true.</returns>
        public bool IsEnableAddProperty(string l_dmName)
        {
            bool isEnable = true;
            try
            {
                WrappedSimulator sim = CreateSimulatorInstance();
                sim.CreateEntity(
                    l_dmName,
                    Constants.xpathProcess + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper());
                try
                {
                    string fullPath = Constants.xpathProcess + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper() + Constants.delimiterColon + "CheckProperty";
                    WrappedPolymorph l_newValue = EcellValue.CastToWrappedPolymorph4EcellValue(new EcellValue(0.01));
                    sim.SetEntityProperty(fullPath, l_newValue);
                }
                catch (Exception)
                {
                    isEnable = false;
                }
            }
            catch (Exception l_ex)
            {
                l_ex.ToString();
                return false;
                //throw new Exception(
                //    s_resources.GetString(ErrorConstants.ErrGetProp) +
                //    "[" + l_dmName + "] {" + l_ex.ToString() + "}", l_ex);
            }
            return isEnable;
        }

        /// <summary>
        /// Returns the list of the "Process" property. 
        /// </summary>
        /// <param name="l_dmName">The DM name</param>
        /// <returns>The dictionary of the "Process" property</returns>
        public Dictionary<string, EcellData> GetProcessProperty(string l_dmName)
        {
            Dictionary<string, EcellData> l_dic = new Dictionary<string, EcellData>();
            try
            {
                WrappedSimulator sim = CreateSimulatorInstance();
                sim.CreateEntity(
                    l_dmName,
                    Constants.xpathProcess + Constants.delimiterColon +
                    Constants.delimiterPath + Constants.delimiterColon +
                    Constants.xpathSize.ToUpper());
                string l_key = Constants.delimiterPath + Constants.delimiterColon + Constants.xpathSize.ToUpper();
                EcellObject dummyEcellObject = EcellObject.CreateObject("", l_key, "", "", null);
                DataStorer.DataStored4Process(
                        sim,
                        dummyEcellObject,
                        new Dictionary<string, double>());
                SetPropertyList(dummyEcellObject, l_dic);
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                    m_resources.GetString(ErrorConstants.ErrGetProp) +
                    "[" + l_dmName + "] {" + l_ex.ToString() + "}", l_ex);
            }
            return l_dic;
        }

        /// <summary>
        /// Returns the list of the "Project" with the directory name.
        /// </summary>
        /// <param name="l_dir">The directory name</param>
        /// <returns>The list of the "Project"</returns>
        public List<Project> GetProjects(string l_dir)
        {
            if (!Directory.Exists(l_dir))
            {
                return null;
            }
            List<Project> l_list = new List<Project>();
            string[] l_dirList = Directory.GetDirectories(l_dir);
            if (l_dirList == null || l_dirList.Length <= 0)
            {
                return null;
            }
            for (int i = 0; i < l_dirList.Length; i++)
            {
                string l_prjFile = l_dirList[i] + Constants.delimiterPath + Constants.fileProject;
                if (File.Exists(l_prjFile))
                {
                    StreamReader l_reader = null;
                    try
                    {
                        l_reader = new StreamReader(l_prjFile, Encoding.UTF8);
                        l_list.Add(
                            new Project(
                                Path.GetFileName(l_dirList[i]),
                                l_reader.ReadLine(),
                                File.GetLastWriteTime(l_prjFile).ToString()
                                )
                            );
                    }
                    catch (Exception l_ex)
                    {
                        l_ex.ToString();
                        continue;
                    }
                    finally
                    {
                        if (l_reader != null)
                        {
                            l_reader.Close();
                        }
                    }
                }
            }
            if (l_list.Count > 0)
            {
                this.m_defaultDir = l_dir;
            }
            return l_list;
        }

        /// <summary>
        /// Returns the savable model ID.
        /// </summary>
        /// <returns>The savable model ID</returns>
        public List<string> GetSavableModel()
        {
            try
            {
                if (m_currentProject.ModelList != null
                    && m_currentProject.ModelList.Count > 0)
                {
                    List<string> l_modelIDList = new List<string>();
                    foreach (EcellObject l_model in m_currentProject.ModelList)
                    {
                        l_modelIDList.Add(l_model.ModelID);
                    }
                    return l_modelIDList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrFindModel) +
                    " {" + l_ex.ToString() + "}");
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
            try
            {
                if (m_currentProject.LoggerPolicyDic != null
                    && m_currentProject.LoggerPolicyDic.Count > 0)
                {
                    List<string> l_prmIDList = new List<string>();
                    foreach (string l_prmID in m_currentProject.LoggerPolicyDic.Keys)
                    {
                        l_prmIDList.Add(l_prmID);
                    }
                    return l_prmIDList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrSimParam) +
                    " {" + l_ex.ToString() + "}");
            }
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
        /// <param name="l_parameterID">The parameter ID</param>
        /// <param name="l_modelID"> model ID</param>
        /// <returns>The list of the "Stepper"</returns>
        public List<EcellObject> GetStepper(string l_parameterID, string l_modelID)
        {
            List<EcellObject> l_returnedStepper = new List<EcellObject>();
            try
            {
                if (string.IsNullOrEmpty(l_modelID))
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrNullData));
                if (string.IsNullOrEmpty(l_parameterID))
                    l_parameterID = m_currentProject.SimulationParam;
                if (string.IsNullOrEmpty(l_parameterID))
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrCurParamID));

                List<EcellObject> tempList = m_currentProject.StepperDic[l_parameterID][l_modelID];
                foreach (EcellObject l_stepper in tempList)
                {
                    // DataStored4Stepper(l_simulator, l_stepper);
                    l_returnedStepper.Add(l_stepper.Copy());
                }
                return l_returnedStepper;
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                    m_resources.GetString(ErrorConstants.ErrGetStep) +
                    " [" + l_parameterID + ", " + l_modelID + "] {" +
                    l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the list of the parameter ID with the model ID.
        /// </summary>
        /// <returns>The list of parameter ID</returns>
        public List<string> GetSimulationParameterIDs()
        {
            try
            {
                if (m_currentProject.StepperDic == null)
                    return new List<string>();

                return new List<string>(m_currentProject.StepperDic.Keys);
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrGetSimParams) + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the list of the "Stepper" DM.
        /// </summary>
        /// <returns>The list of the "Stepper" DM</returns>
        public List<string> GetStepperList()
        {
            List<string> l_stepperList = new List<string>();
            WrappedSimulator sim = CreateSimulatorInstance();
            foreach (WrappedPolymorph l_polymorph in sim.GetDMInfo().CastToList())
            {
                List<WrappedPolymorph> l_dmInfoList = l_polymorph.CastToList();
                if (l_dmInfoList[0].CastToString().Equals(Constants.xpathStepper))
                {
                    l_stepperList.Add(l_dmInfoList[1].CastToString());
                }
            }
            if (m_currentProject.DmDic != null)
                l_stepperList.AddRange(m_currentProject.DmDic[Constants.xpathStepper]);
            l_stepperList.Sort();
            return l_stepperList;
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
        /// <param name="l_dmName">The DM name</param>
        /// <returns>The dictionary of the "Stepper" property</returns>
        public Dictionary<string, EcellData> GetStepperProperty(string l_dmName)
        {
            Dictionary<string, EcellData> l_dic = new Dictionary<string, EcellData>();
            EcellObject dummyEcellObject = null;
            try
            {
                WrappedSimulator sim = CreateSimulatorInstance();
                sim.CreateStepper(l_dmName, Constants.textKey);
                dummyEcellObject = EcellObject.CreateObject("", Constants.textKey, "", "", null);
                DataStorer.DataStored4Stepper(sim, dummyEcellObject);
                SetPropertyList(dummyEcellObject, l_dic);
            }
            finally
            {
                dummyEcellObject = null;
            }
            return l_dic;
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
        /// <param name="l_modelID">model ID.</param>
        /// <returns>the list of system.</returns>
        public List<string> GetSystemList(string l_modelID)
        {
            try
            {
                List<string> l_systemList = new List<string>();
                foreach (EcellObject l_system in m_currentProject.SystemDic[l_modelID])
                {
                    l_systemList.Add(l_system.Key);
                }
                return l_systemList;
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrGetSysList) + " [" + l_modelID + "] {"
                        + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the list of the "System" property. 
        /// </summary>
        /// <returns>The dictionary of the "System" property</returns>
        public Dictionary<string, EcellData> GetSystemProperty()
        {
            Dictionary<string, EcellData> l_dic = new Dictionary<string, EcellData>();
            WrappedSimulator sim = CreateSimulatorInstance();
            BuildDefaultSimulator(sim, null, null);
            ArrayList l_list = new ArrayList();
            l_list.Clear();
            l_list.Add("");
            sim.LoadEntityProperty(
                Constants.xpathSystem + Constants.delimiterColon +
                Constants.delimiterColon +
                Constants.delimiterPath + Constants.delimiterColon +
                Constants.xpathName,
                l_list
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
            SetPropertyList(dummyEcellObject, l_dic);
            return l_dic;
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
            Dictionary<string, EcellData> l_dic = new Dictionary<string, EcellData>();
            WrappedSimulator l_simulator = null;
            EcellObject dummyEcellObject = null;
            try
            {
                l_simulator = CreateSimulatorInstance();
                BuildDefaultSimulator(l_simulator, null, null);
                dummyEcellObject = EcellObject.CreateObject(
                    "",
                    Constants.delimiterPath + Constants.delimiterColon + Constants.xpathSize.ToUpper(),
                    "",
                    "",
                    null
                    );
                DataStorer.DataStored4Variable(
                        l_simulator,
                        dummyEcellObject,
                        new Dictionary<string, double>());
                SetPropertyList(dummyEcellObject, l_dic);
            }
            /*
            catch (Exception l_ex)
            {
                throw new Exception("Can't obtain the property of the \"Variable\". {"
                        + l_ex.ToString() + "}");
            }
             */
            finally
            {
                l_simulator = null;
                dummyEcellObject = null;
            }
            return l_dic;
        }

        /// <summary>
        /// Tests whether the "EcellObject" is usable.
        /// </summary>
        /// <param name="l_ecellObject">The tested "EcellObject"</param>
        /// <returns>true if the "EcellObject" is usable; false otherwise</returns>
        private bool IsUsable(EcellObject l_ecellObject)
        {
            bool l_flag = false;
            if (l_ecellObject == null)
            {
                return l_flag;
            }
            else if (l_ecellObject.ModelID == null || l_ecellObject.ModelID.Length < 0)
            {
                return l_flag;
            }
            else if (l_ecellObject.Type == null || l_ecellObject.Type.Length < 0)
            {
                return l_flag;
            }
            //
            // 4 "Process", "Stepper", "System" and "Variable"
            //
            if (!l_ecellObject.Type.Equals(Constants.xpathProject) && !l_ecellObject.Type.Equals(Constants.xpathModel))
            {
                if (l_ecellObject.Key == null || l_ecellObject.Key.Length < 0)
                {
                    return l_flag;
                }
                else if (l_ecellObject.Classname == null || l_ecellObject.Classname.Length < 0)
                {
                    return l_flag;
                }
            }
            return true;
        }

        /// <summary>
        /// Initialize the simulator before it starts.
        /// </summary>
        public void Initialize(bool l_flag)
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
                List<EcellObject> l_newStepperList = new List<EcellObject>();
                List<string> l_modelIDList = new List<string>();
                Dictionary<string, Dictionary<string, WrappedPolymorph>> l_setStepperPropertyDic
                    = new Dictionary<string, Dictionary<string, WrappedPolymorph>>();
                foreach (string l_modelID in stepperList.Keys)
                {
                    l_newStepperList.AddRange(stepperList[l_modelID]);
                    l_modelIDList.Add(l_modelID);
                }
                LoadStepper(
                    simulator,
                    l_newStepperList,
                    l_setStepperPropertyDic);

                //
                // Loads systems on the simulator.
                //
                List<string> l_allLoggerList = new List<string>();
                List<EcellObject> l_systemList = new List<EcellObject>();
                m_currentProject.LogableEntityPathDic = new Dictionary<string, string>();
                Dictionary<string, WrappedPolymorph> l_setSystemPropertyDic = new Dictionary<string, WrappedPolymorph>();
                foreach (string l_modelID in l_modelIDList)
                {
                    List<string> l_loggerList = new List<string>();
                    if (l_flag)
                    {
                        LoadSystem(
                            simulator,
                            m_currentProject.SystemDic[l_modelID],
                            l_loggerList,
                            initialCondition[l_modelID],
                            l_setSystemPropertyDic);
                    }
                    else
                    {
                        LoadSystem(
                            simulator,
                            m_currentProject.SystemDic[l_modelID],
                            l_loggerList,
                            null,
                            l_setSystemPropertyDic);
                    }
                    foreach (string l_logger in l_loggerList)
                    {
                        m_currentProject.LogableEntityPathDic[l_logger] = l_modelID;
                    }
                    l_allLoggerList.AddRange(l_loggerList);
                }

                //
                // Initializes
                //
                simulator.Initialize();
                //
                // Sets the "Settable" and "Not Savable" properties
                //
                foreach (string l_key in l_setStepperPropertyDic.Keys)
                {
                    foreach (string l_path in l_setStepperPropertyDic[l_key].Keys)
                    {
                        simulator.SetStepperProperty(l_key, l_path, l_setStepperPropertyDic[l_key][l_path]);
                    }
                }
                foreach (string l_path in l_setSystemPropertyDic.Keys)
                {
                    try
                    {
                        EcellValue l_storedEcellValue = new EcellValue(simulator.GetEntityProperty(l_path));
                        EcellValue l_newEcellValue = new EcellValue(l_setSystemPropertyDic[l_path]);
                        if (l_storedEcellValue.Type.Equals(l_newEcellValue.Type)
                            && l_storedEcellValue.Value.Equals(l_newEcellValue.Value))
                        {
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                    simulator.SetEntityProperty(l_path, l_setSystemPropertyDic[l_path]);
                }
                //
                // Set the initial condition property.
                //
                foreach (string l_modelID in l_modelIDList)
                {
                    foreach (string l_type
                        in initialCondition[l_modelID].Keys)
                    {
                        foreach (string l_fullPN
                            in initialCondition[l_modelID]
                                [l_type].Keys)
                        {
                            EcellValue l_storedValue = new EcellValue(simulator.GetEntityProperty(l_fullPN));
                            double l_initialValue = initialCondition[l_modelID][l_type][l_fullPN];
                            WrappedPolymorph l_newValue = null;
                            if (l_storedValue.IsInt())
                            {
                                int l_initialValueInt = Convert.ToInt32(l_initialValue);
                                if (l_storedValue.CastToInt().Equals(l_initialValueInt))
                                {
                                    continue;
                                }
                                l_newValue
                                    = EcellValue.CastToWrappedPolymorph4EcellValue(new EcellValue(l_initialValueInt));
                            }
                            else
                            {
                                if (l_storedValue.CastToDouble().Equals(l_initialValue))
                                {
                                    continue;
                                }
                                l_newValue
                                    = EcellValue.CastToWrappedPolymorph4EcellValue(new EcellValue(l_initialValue));
                            }
                            simulator.SetEntityProperty(l_fullPN, l_newValue);
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
                if (l_allLoggerList != null && l_allLoggerList.Count > 0)
                {
                    WrappedPolymorph l_loggerPolicy = this.GetCurrentLoggerPolicy();
                    foreach (string l_logger in l_allLoggerList)
                    {
                        simulator.CreateLogger(l_logger, l_loggerPolicy);
                    }
                }
                //
                // Messages
                //
                Message("Initialize the simulator:");
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrInitSim), l_ex);
            }
        }

        /// <summary>
        /// Checks whether the simulator is running.
        /// </summary>
        /// <returns>true if the simulator is running; false otherwise</returns>
        public bool IsActive()
        {
            bool l_runningFlag = false;
            if (m_currentProject != null && m_currentProject.SimulationStatus == SimulationStatus.Run)
                l_runningFlag = true;

            return l_runningFlag;
        }

        /// <summary>
        /// Tests whether the "EcellData" is usable.
        /// </summary>
        /// <param name="l_ecellData">The tested "EcellData"</param>
        /// <returns>true if the "EcellData" is usable; false otherwise</returns>
        private bool IsUsable(EcellData l_ecellData)
        {
            bool l_flag = false;
            if (l_ecellData == null)
            {
                return l_flag;
            }
            else if (l_ecellData.Name == null || l_ecellData.Name.Length <= 0)
            {
                return l_flag;
            }
            else if (l_ecellData.Value == null)
            {
                return l_flag;
            }
            else if (l_ecellData.EntityPath == null || l_ecellData.EntityPath.Length <= 0)
            {
                return l_flag;
            }
            return true;
        }

        private void InitializeModel(EcellObject l_ecellObject)
        {
            DataStorer.DataStored(
                m_currentProject.Simulator,
                l_ecellObject,
                m_currentProject.InitialCondition[m_currentProject.SimulationParam][l_ecellObject.ModelID]);
            // Sets the "EcellObject".
            string modelID = l_ecellObject.ModelID;
            string simParam = m_currentProject.SimulationParam;
            if (l_ecellObject.Type.Equals(Constants.xpathModel))
            {
                m_currentProject.ModelList.Add(l_ecellObject);
            }
            else if (l_ecellObject.Type.Equals(Constants.xpathSystem))
            {
                if (!m_currentProject.SystemDic.ContainsKey(modelID))
                {
                    m_currentProject.SystemDic[modelID]
                            = new List<EcellObject>();
                }
                m_currentProject.SystemDic[modelID].Add(l_ecellObject);
            }
            else if (l_ecellObject.Type.Equals(Constants.xpathStepper))
            {
                if (!m_currentProject.StepperDic.ContainsKey(simParam))
                {
                    m_currentProject.StepperDic[simParam] = new Dictionary<string, List<EcellObject>>();
                }
                if (!m_currentProject.StepperDic[simParam].ContainsKey(modelID))
                {
                    m_currentProject.StepperDic[simParam][modelID] = new List<EcellObject>();
                }
                m_currentProject.StepperDic[simParam][modelID].Add(l_ecellObject);
            }
            foreach (EcellObject l_childEcellObject in l_ecellObject.Children)
            {
                InitializeModel(l_childEcellObject);
            }
        }

        /// <summary>
        /// Loads the eml formatted file and returns the model ID.
        /// </summary>
        /// <param name="l_filename">The eml formatted file name</param>
        /// <param name="isLogging">The flag whether this function is in logging.</param>
        /// <returns>The model ID</returns>
        public string LoadModel(string l_filename, bool isLogging)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_filename + "]";
                //
                // To load
                //
                string l_modelID = null;
                if (m_currentProject.FilePath == null)
                {
                    m_currentProject.FilePath = l_filename;
                }
                if (m_currentProject.Simulator == null)
                {
                    m_currentProject.Simulator = CreateSimulatorInstance();
                    m_currentProject.SetDMList();
                }
                EcellObject l_modelObj = EmlReader.Parse(l_filename,
                        m_currentProject.Simulator);
                l_modelID = l_modelObj.ModelID;

                //
                // Checks the old model ID
                //
                foreach (EcellObject l_model in m_currentProject.ModelList)
                {
                    if (l_model.ModelID.Equals(l_modelID))
                    {
                        throw new Exception(
                            String.Format(
                                m_resources.GetString(ErrorConstants.ErrExistObj),
                                new object[] { l_message + "[Model]" }
                            )
                        );
                    }
                }
                //
                // Initialize
                //
                try
                {
                    m_currentProject.Simulator.Initialize();
                }
                catch (Exception)
                {
                    l_message = m_resources.GetString(ErrorConstants.ErrInitSim) + "[" + m_currentProject.Name + "]";
                }
                // Sets initial conditions.
                m_currentProject.Initialize(l_modelID);
                string l_simParam = m_currentProject.SimulationParam;
                InitializeModel(l_modelObj);
                // Stores the "LoggerPolicy"
                if (!m_currentProject.LoggerPolicyDic.ContainsKey(l_simParam))
                {
                    m_currentProject.LoggerPolicyDic[l_simParam]
                        = new LoggerPolicy(
                            LoggerPolicy.s_reloadStepCount,
                            LoggerPolicy.s_reloadInterval,
                            LoggerPolicy.s_diskFullAction,
                            LoggerPolicy.s_maxDiskSpace
                            );
                }
                Message("Load Model: " + l_message);
                if (isLogging)
                    m_aManager.AddAction(new ImportModelAction(l_filename));
                if (m_currentProject.ModelFileDic.ContainsKey(l_modelID))
                    m_currentProject.ModelFileDic.Remove(l_modelID);
                m_currentProject.ModelFileDic.Add(l_modelID, l_filename);

                return l_modelID;
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrLoadModel) + "[" + l_message + "]";
                Message(l_message);
                throw new Exception(l_message, l_ex);
            }
        }

        /// <summary>
        /// Loads the project.
        /// </summary>
        /// <param name="l_prjID">The load project ID</param>
        /// <param name="l_prjFile">The load project file.</param>
        public void LoadProject(string l_prjID, string l_prjFile)
        {
            List<EcellObject> l_passList = new List<EcellObject>();
            string[] l_parameters = new string[0];
            Project l_prj = null;
            string l_message = null;
            try
            {
                // Check ProjectID Error.
                if (string.IsNullOrEmpty(l_prjID))
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrNullData));
                l_message = "[" + l_prjID + "]";

                // Initializes.
                l_prj = new Project(l_prjFile);
                if (l_prj == null)
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindPrjFile) + " [" + Constants.fileProject + "]");

                l_prj.Name = l_prjID;
                m_currentProject = l_prj;
                m_currentProject.SetDMList();
                m_pManager.ParameterSet(l_prjID, m_currentProject.SimulationParam);
                m_currentProject.Simulator = CreateSimulatorInstance();
                m_currentProject.LoggerPolicyDic = new Dictionary<string, LoggerPolicy>();
                m_currentProject.StepperDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                m_currentProject.ModelList = new List<EcellObject>();
                m_currentProject.SystemDic = new Dictionary<string, List<EcellObject>>();
                m_projectList.Add(l_prj);

                List<EcellData> l_ecellDataList = new List<EcellData>();
                l_ecellDataList.Add(new EcellData(Constants.textComment, new EcellValue(l_prj.Comment), null));
                l_passList.Add(EcellObject.CreateObject(l_prjID, "", Constants.xpathProject, "", l_ecellDataList));

                // Loads the model.
                string l_modelDirName = Path.Combine(l_prj.ProjectPath, Constants.xpathModel);
                if (!Directory.Exists(l_modelDirName))
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindModel));

                string[] l_models = Directory.GetFileSystemEntries(
                    l_modelDirName,
                    Constants.delimiterWildcard + Constants.FileExtEML
                    );
                if (l_models == null || l_models.Length <= 0)
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindModel));

                foreach (string l_model in l_models)
                {
                    string l_fileName = Path.GetFileName(l_model);
                    if (l_fileName.IndexOf(Constants.delimiterUnderbar) > 0 ||
                        l_fileName.EndsWith(Constants.FileExtBackUp))
                        continue;
                    this.LoadModel(l_model, false);
                }
                l_passList.AddRange(m_currentProject.ModelList);
                foreach (string l_storedModelID in m_currentProject.SystemDic.Keys)
                {
                    l_passList.AddRange(m_currentProject.SystemDic[l_storedModelID]);
                }
                //
                // Loads the simulation parameter.
                //
                string l_simulationDirName = Path.Combine(l_prj.ProjectPath, Constants.xpathParameters);

                if (Directory.Exists(l_simulationDirName))
                {
                    l_parameters = Directory.GetFileSystemEntries(
                        l_simulationDirName,
                        Constants.delimiterWildcard + Constants.FileExtXML);
                    if (l_parameters != null && l_parameters.Length > 0)
                    {
                        foreach (string l_parameter in l_parameters)
                        {
                            string l_fileName = Path.GetFileName(l_parameter);
                            if (l_fileName.IndexOf(Constants.delimiterUnderbar) != 0)
                            {
                                LoadSimulationParameter(l_parameter);
                            }
                        }
                    }
                }
                Message("Load Project: " + l_message);
            }
            catch (Exception l_ex)
            {
                l_passList = null;
                if (l_prj != null)
                {
                    if (this.m_projectList.Contains(l_prj))
                    {
                        this.m_projectList.Remove(l_prj);
                        l_prj = null;
                    }
                }
                Trace.WriteLine(l_ex.ToString());
                l_message = m_resources.GetString(ErrorConstants.ErrLoadPrj) + "[" + l_message + "]";
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
            finally
            {
                if (l_passList != null && l_passList.Count > 0)
                {
                    this.m_pManager.DataAdd(l_passList);
                }
                foreach (string paramID in this.GetSimulationParameterIDs())
                {
                    this.m_pManager.ParameterAdd(l_prjID, paramID);
                }
                m_aManager.AddAction(new LoadProjectAction(l_prjID, l_prjFile));
            }
        }



        /// <summary>
        /// Loads the simulation parameter.
        /// </summary>
        /// <param name="l_fileName">The simulation parameter file name</param>
        public void LoadSimulationParameter(string l_fileName)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_fileName + "]";
                // Initializes
                if (string.IsNullOrEmpty(l_fileName))
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrNullData));
                }
                // Parses the simulation parameter.
                SimulationParameter simParam = SimulationParameterReader.Parse(
                        l_fileName, m_currentProject.Simulator);
                string simParamID = simParam.ID;
                // Stores the simulation parameter.
                if (!m_currentProject.SimulationParam.Equals(simParamID))
                {
                    if (!m_currentProject.StepperDic.ContainsKey(simParamID))
                    {
                        m_currentProject.StepperDic[simParamID]
                            = new Dictionary<string, List<EcellObject>>();
                    }
                    foreach (EcellObject l_stepper in simParam.Steppers)
                    {
                        if (!m_currentProject.StepperDic[simParamID]
                            .ContainsKey(l_stepper.ModelID))
                        {
                            m_currentProject.StepperDic[simParamID][l_stepper.ModelID]
                                = new List<EcellObject>();
                        }
                        foreach (EcellData l_data in l_stepper.Value)
                        {
                            l_data.Value = GetEcellValue(l_data);
                        }
                        m_currentProject.StepperDic[simParamID][l_stepper.ModelID].Add(l_stepper);
                    }
                }
                else
                {
                    foreach (EcellObject l_stepper in simParam.Steppers)
                    {
                        bool l_matchFlag = false;
                        if (!m_currentProject.StepperDic[simParamID].ContainsKey(l_stepper.ModelID))
                        {
                            m_currentProject.StepperDic[simParamID][l_stepper.ModelID]
                                = new List<EcellObject>();
                        }
                        for (int j = 0;
                            j < m_currentProject.StepperDic[simParamID][l_stepper.ModelID].Count;
                            j++)
                        {
                            EcellObject l_storedStepper
                                = m_currentProject.StepperDic[simParamID][l_stepper.ModelID][j];
                            if (!l_storedStepper.Classname.Equals(l_stepper.Classname)
                                || !l_storedStepper.Key.Equals(l_stepper.Key)
                                || !l_storedStepper.ModelID.Equals(l_stepper.ModelID)
                                || !l_storedStepper.Type.Equals(l_stepper.Type))
                                continue;

                            List<EcellData> l_newDataList = new List<EcellData>();
                            foreach (EcellData l_storedData in l_storedStepper.Value)
                            {
                                bool l_existFlag = false;
                                foreach (EcellData l_newData in l_stepper.Value)
                                {
                                    if (!l_storedData.Name.Equals(l_newData.Name)
                                        || !l_storedData.EntityPath.Equals(l_newData.EntityPath))
                                        continue;

                                    if (l_storedData.Value.IsDouble())
                                    {
                                        l_newData.Value = GetEcellValue(l_newData);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            l_newData.Value
                                                = new EcellValue(
                                                    Convert.ToInt32(
                                                        l_newData.Value.CastToList()[0].ToString()));
                                        }
                                        catch (Exception)
                                        {
                                            // do nothing
                                        }
                                    }
                                    l_newData.Gettable = l_storedData.Gettable;
                                    l_newData.Loadable = l_storedData.Loadable;
                                    l_newData.Saveable = l_storedData.Saveable;
                                    l_newData.Settable = l_storedData.Settable;
                                    l_newDataList.Add(l_newData);
                                    l_existFlag = true;
                                    break;
                                }
                                if (!l_existFlag)
                                {
                                    l_newDataList.Add(l_storedData);
                                }
                            }
                            m_currentProject.StepperDic[simParamID][l_stepper.ModelID][j]
                                = EcellObject.CreateObject(
                                    l_stepper.ModelID,
                                    l_stepper.Key,
                                    l_stepper.Type,
                                    l_stepper.Classname,
                                    l_newDataList);
                            l_matchFlag = true;
                            break;
                        }
                        if (!l_matchFlag)
                        {
                            m_currentProject.StepperDic[simParamID][l_stepper.ModelID]
                                .Add(l_stepper);
                        }
                    }
                }
                m_currentProject.LoggerPolicyDic[simParamID] = simParam.LoggerPolicy;
                m_currentProject.InitialCondition[simParamID] = simParam.InitialConditions;
                Message("Load Simulation Parameter: " + l_message);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrLoadSimParam) + "[" + l_message + "]";
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }
        /// <summary>
        /// GetEcellValue
        /// </summary>
        /// <param name="l_data"></param>
        /// <returns></returns>
        private static EcellValue GetEcellValue(EcellData l_data)
        {
            double l_value = 0.0;
            try
            {
                // Get new value.
                string l_newValue = l_data.Value.CastToList()[0].ToString();
                if (l_newValue.Equals(Double.PositiveInfinity.ToString()))
                    l_value = Double.PositiveInfinity;
                else if (l_newValue.Equals(Double.MaxValue.ToString()))
                    l_value = Double.MaxValue;
                else
                    l_value = XmlConvert.ToDouble(l_newValue);
            }
            catch (Exception)
            {
                l_value = Double.PositiveInfinity;
            }
            return new EcellValue(l_value);
        }

        /// <summary>
        /// Loads the "Stepper" 2 the "EcellCoreLib".
        /// </summary>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_stepperList">The list of the "Stepper"</param>
        /// <param name="l_setStepperDic"></param>
        private static void LoadStepper(
            WrappedSimulator l_simulator,
            List<EcellObject> l_stepperList,
            Dictionary<string, Dictionary<string, WrappedPolymorph>> l_setStepperDic)
        {
            if (l_stepperList == null || l_stepperList.Count <= 0)
            {
                throw new Exception(s_resources.GetString(ErrorConstants.ErrFindSimParam));
            }
            bool l_existStepper = false;
            foreach (EcellObject l_stepper in l_stepperList)
            {
                if (l_stepper == null)
                    continue;

                l_existStepper = true;
                l_simulator.CreateStepper(l_stepper.Classname, l_stepper.Key);

                // 4 property
                if (l_stepper.Value == null || l_stepper.Value.Count <= 0)
                    continue;

                foreach (EcellData l_ecellData in l_stepper.Value)
                {
                    if (l_ecellData.Name == null || l_ecellData.Name.Length <= 0 || l_ecellData.Value == null)
                        continue;
                    else if (!l_ecellData.Value.IsDouble() && !l_ecellData.Value.IsInt())
                        continue;

                    // 4 MaxStepInterval == Double.MaxValue
                    EcellValue velue = l_ecellData.Value;
                    try
                    {
                        string l_value
                            = velue.ToString().Replace("(", "").Replace(")", "").Replace("\"", "");
                        if (l_value.Equals(Double.PositiveInfinity.ToString()))
                            continue;
                        else if (l_value.Equals(Double.MaxValue.ToString()))
                            continue;

                        XmlConvert.ToDouble(l_value);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (velue.IsDouble()
                        && (Double.IsInfinity(velue.CastToDouble()) || Double.IsNaN(velue.CastToDouble())))
                        continue;

                    if (l_ecellData.Saveable)
                    {
                        l_simulator.LoadStepperProperty(
                            l_stepper.Key,
                            l_ecellData.Name,
                            EcellValue.CastToWrappedPolymorph4EcellValue(velue));
                    }
                    else if (l_ecellData.Settable)
                    {
                        if (!l_setStepperDic.ContainsKey(l_stepper.Key))
                        {
                            l_setStepperDic[l_stepper.Key] = new Dictionary<string, WrappedPolymorph>();
                        }
                        l_setStepperDic[l_stepper.Key][l_ecellData.Name]
                            = EcellValue.CastToWrappedPolymorph4EcellValue(velue);
                    }
                }
            }
            if (!l_existStepper)
            {
                throw new Exception(s_resources.GetString(ErrorConstants.ErrFindSimParam));
            }
        }

        /// <summary>
        /// Loads the "System" 2 the "ECellCoreLib".
        /// </summary>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_systemList">The list of "System"</param>
        /// <param name="l_loggerList">The list of the "Logger"</param>
        /// <param name="l_initialCondition">The dictionary of initial condition.</param>
        /// <param name="l_setPropertyDic">The dictionary of simulation library.</param>
        private static void LoadSystem(
            WrappedSimulator l_simulator,
            List<EcellObject> l_systemList,
            List<string> l_loggerList,
            Dictionary<string, Dictionary<string, double>> l_initialCondition,
            Dictionary<string, WrappedPolymorph> l_setPropertyDic)
        {
            if (l_systemList == null || l_systemList.Count <= 0)
                throw new Exception(s_resources.GetString(ErrorConstants.ErrFindSystem));

            bool l_existSystem = false;
            Dictionary<string, WrappedPolymorph> l_processPropertyDic = new Dictionary<string, WrappedPolymorph>();
            try
            {
                foreach (EcellObject l_system in l_systemList)
                {
                    if (l_system == null)
                        continue;

                    l_existSystem = true;
                    string l_parentPath = l_system.ParentSystemID;
                    string l_childPath = l_system.Name;
                    if (!l_system.Key.Equals(Constants.delimiterPath))
                    {
                        l_simulator.CreateEntity(
                            l_system.Classname,
                            l_system.Classname + Constants.delimiterColon
                                + l_parentPath + Constants.delimiterColon + l_childPath);
                    }
                    // 4 property
                    if (l_system.Value == null || l_system.Value.Count <= 0)
                        continue;

                    foreach (EcellData l_ecellData in l_system.Value)
                    {
                        if (l_ecellData.Name == null || l_ecellData.Name.Length <= 0
                            || l_ecellData.Value == null)
                        {
                            continue;
                        }
                        EcellValue l_value = l_ecellData.Value;
                        if (l_ecellData.Saveable)
                        {
                            l_simulator.LoadEntityProperty(
                                l_ecellData.EntityPath,
                                EcellValue.CastToWrappedPolymorph4EcellValue(l_value));
                        }
                        else if (l_ecellData.Settable)
                        {
                            l_setPropertyDic[l_ecellData.EntityPath]
                                = EcellValue.CastToWrappedPolymorph4EcellValue(l_value);
                        }
                        if (l_ecellData.Logged)
                        {
                            l_loggerList.Add(l_ecellData.EntityPath);
                        }
                    }
                    // 4 children
                    if (l_system.Children == null || l_system.Children.Count <= 0)
                        continue;
                    LoadEntity(
                        l_simulator,
                        l_system.Children,
                        l_loggerList,
                        l_processPropertyDic,
                        l_initialCondition,
                        l_setPropertyDic);
                }
                if (l_processPropertyDic.Count > 0)
                {
                    // The "VariableReferenceList" is previously loaded. 
                    string[] l_keys = null;
                    l_processPropertyDic.Keys.CopyTo(l_keys = new string[l_processPropertyDic.Keys.Count], 0);
                    foreach (string l_entityPath in l_keys)
                    {
                        if (l_entityPath.EndsWith(Constants.xpathVRL))
                        {
                            l_simulator.LoadEntityProperty(l_entityPath, l_processPropertyDic[l_entityPath]);
                            l_processPropertyDic.Remove(l_entityPath);
                        }
                    }
                    foreach (string l_entityPath in l_processPropertyDic.Keys)
                    {
                        if (!l_entityPath.EndsWith("Fixed"))
                        {
                            l_simulator.LoadEntityProperty(l_entityPath, l_processPropertyDic[l_entityPath]);
                        }
                    }
                }
                if (!l_existSystem)
                {
                    throw new Exception(s_resources.GetString(ErrorConstants.ErrFindSystem));
                }
            }
            catch (WrappedException e)
            {
                throw new Exception("Failed to create System", e);
            }
        }

        /// <summary>
        /// Loads the "Process" and the "Variable" to the "EcellCoreLib".
        /// </summary>
        /// <param name="l_entityList">The list of the "Process" and the "Variable"</param>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_loggerList">The list of the "Logger"</param>
        /// <param name="l_processPropertyDic">The dictionary of the process property</param>
        /// <param name="l_initialCondition">The dictionary of the initial condition</param>
        /// <param name="l_setPropertyDic"></param>
        private static void LoadEntity(
            WrappedSimulator l_simulator,
            List<EcellObject> l_entityList,
            List<string> l_loggerList,
            Dictionary<string, WrappedPolymorph> l_processPropertyDic,
            Dictionary<string, Dictionary<string, double>> l_initialCondition,
            Dictionary<string, WrappedPolymorph> l_setPropertyDic)
        {
            if (l_entityList == null || l_entityList.Count <= 0)
                return;

            try
            {
                foreach (EcellObject l_entity in l_entityList)
                {
                    if (l_entity is EcellText)
                        continue;
                    l_simulator.CreateEntity(
                        l_entity.Classname,
                        l_entity.Type + Constants.delimiterColon + l_entity.Key);
                    if (l_entity.Value == null || l_entity.Value.Count <= 0)
                        continue;

                    foreach (EcellData l_ecellData in l_entity.Value)
                    {
                        EcellValue value = l_ecellData.Value;
                        if (string.IsNullOrEmpty(l_ecellData.Name)
                                || value == null
                                || (value.IsString() && value.CastToString().Length == 0))
                        {
                            continue;
                        }

                        if (l_ecellData.Logged)
                        {
                            l_loggerList.Add(l_ecellData.EntityPath);
                        }

                        if (value.IsDouble()
                            && (Double.IsInfinity(value.CastToDouble()) || Double.IsNaN(value.CastToDouble())))
                        {
                            continue;
                        }
                        if (l_ecellData.Saveable)
                        {
                            if (l_ecellData.EntityPath.EndsWith(Constants.xpathVRL))
                            {
                                l_processPropertyDic[l_ecellData.EntityPath]
                                    = EcellValue.CastToWrappedPolymorph4EcellValue(value);
                            }
                            else
                            {
                                if (l_ecellData.EntityPath.EndsWith("FluxDistributionList"))
                                    continue;
                                l_simulator.LoadEntityProperty(
                                    l_ecellData.EntityPath,
                                    EcellValue.CastToWrappedPolymorph4EcellValue(value));
                            }
                        }
                        else if (l_ecellData.Settable)
                        {
                            l_setPropertyDic[l_ecellData.EntityPath]
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
            return null;
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
            try
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
                    data.Add(d);
                }
                EcellObject obj = EcellObject.CreateObject(modelID, tmpID,
                    Constants.xpathProcess, Constants.DefaultProcessName, data);

                if (isProper)
                {
                    List<EcellObject> rList = new List<EcellObject>();
                    rList.Add(obj);
                    DataAdd(rList);
                    m_pManager.SelectChanged(modelID, tmpID, Constants.xpathProcess);
                }
                return obj;
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString(ErrorConstants.ErrAddObj);
                MessageBox.Show(errmes + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

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
            try
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
                    m_pManager.SelectChanged(modelID, tmpID, Constants.xpathVariable);
                }
                return obj;
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString(ErrorConstants.ErrAddObj);
                MessageBox.Show(errmes + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

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
            try
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
                    m_pManager.SelectChanged(modelID, tmpID, Constants.xpathSystem);
                }
                return obj;
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString(ErrorConstants.ErrAddObj);
                MessageBox.Show(errmes + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        /// <summary>
        /// Creates the new "Project" object.
        /// </summary>
        /// <param name="l_prjID">The "Project" ID</param>
        /// <param name="l_comment">The comment</param>
        /// <param name="l_projectPath">The project directory path to load the dm of this project.</param>
        /// <param name="l_setDirList">The list of dm directory.</param>
        public void CreateProject(string l_prjID, string l_comment, string l_projectPath,
            List<String> l_setDirList)
        {
            Project l_prj = null;
            try
            {
                //
                // Closes the current project.
                //
                if (m_currentProject != null)
                {
                    this.CloseProject(m_currentProject.Name);
                }
                //
                // Initialize
                //
                l_prj = new Project(l_prjID, l_comment, DateTime.Now.ToString());
                m_projectList.Add(l_prj);
                m_currentProject = l_prj;
                if (l_projectPath != null)
                    m_currentProject.ProjectPath = l_projectPath;

                CreateProjectDir(l_prjID, l_setDirList);
                m_currentProject.SetDMList();
                m_currentProject.Simulator = CreateSimulatorInstance();
                m_currentProject.LoggerPolicyDic = new Dictionary<string, LoggerPolicy>();
                m_currentProject.StepperDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                m_currentProject.ModelList = new List<EcellObject>();
                m_currentProject.SystemDic = new Dictionary<string, List<EcellObject>>();
                //
                // 4 PluginManager
                //
                List<EcellData> l_ecellDataList = new List<EcellData>();
                l_ecellDataList.Add(new EcellData(Constants.textComment, new EcellValue(l_comment), null));
                EcellObject l_ecellObject
                        = EcellObject.CreateObject(l_prjID, "", Constants.xpathProject, "", l_ecellDataList);
                List<EcellObject> l_ecellObjectList = new List<EcellObject>();
                l_ecellObjectList.Add(l_ecellObject);
                m_pManager.DataAdd(l_ecellObjectList);
                m_aManager.AddAction(new NewProjectAction(l_prjID, l_comment, l_projectPath));
                Message("Create Project: [" + l_prjID + "]");
            }
            catch (Exception l_ex)
            {
                if (l_prj != null)
                {
                    if (this.m_projectList.Contains(l_prj))
                    {
                        this.m_projectList.Remove(l_prj);
                        l_prj = null;
                    }
                }
                string l_message = String.Format(
                        m_resources.GetString(ErrorConstants.ErrCrePrj),
                        new object[] { l_prjID });
                Message(l_message);
                throw new Exception(l_message, l_ex);
            }
        }

        /// <summary>
        /// Creates the new simulation parameter.
        /// </summary>
        /// <param name="l_parameterID">The new parameter ID</param>
        /// <returns>The new parameter</returns>
        public void CreateSimulationParameter(string l_parameterID)
        {
            CreateSimulationParameter(l_parameterID, true, true);
        }

        /// <summary>
        /// Creates the new simulation parameter.
        /// </summary>
        /// <param name="l_parameterID">The new parameter ID</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>        
        public void CreateSimulationParameter(string l_parameterID, bool l_isRecorded, bool l_isAnchor)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_parameterID + "]";
                //
                // 4 Stepper
                //
                string l_storedParameterID = null;
                if (!m_currentProject.StepperDic.ContainsKey(l_parameterID))
                {
                    //
                    // Searches the source stepper.
                    //
                    if (m_currentProject.SimulationParam != null && m_currentProject.SimulationParam.Length > 0)
                    {
                        l_storedParameterID = m_currentProject.SimulationParam;
                    }
                    else
                    {
                        if (m_currentProject.StepperDic != null
                            && m_currentProject.StepperDic.Count > 0)
                        {
                            foreach (string l_key in m_currentProject.StepperDic.Keys)
                            {
                                l_storedParameterID = l_key;
                                break;
                            }
                        }
                        else
                        {
                            throw new Exception(m_resources.GetString(ErrorConstants.ErrFindSimParam));
                        }
                    }
                    //
                    // Sets the destination stepper.
                    //
                    m_currentProject.StepperDic[l_parameterID]
                            = new Dictionary<string, List<EcellObject>>();

                    foreach (string l_key in m_currentProject.StepperDic[l_storedParameterID].Keys)
                    {
                        m_currentProject.StepperDic[l_parameterID][l_key] = new List<EcellObject>();
                        foreach (EcellObject l_stepper in m_currentProject.StepperDic[l_storedParameterID][l_key])
                        {
                            m_currentProject.StepperDic[l_parameterID][l_key]
                                    .Add(l_stepper.Copy());
                        }
                    }
                }
                else
                {
                    throw new Exception(
                        String.Format(
                            m_resources.GetString(ErrorConstants.ErrExistSimParam),
                            new object[] { l_message }
                        )
                    );
                }
                //
                // 4 LoggerPolicy
                //

                LoggerPolicy l_loggerPolicy = m_currentProject.LoggerPolicyDic[l_storedParameterID];

                m_currentProject.LoggerPolicyDic[l_parameterID]
                    = new LoggerPolicy(
                        l_loggerPolicy.m_reloadStepCount,
                        l_loggerPolicy.m_reloadInterval,
                        l_loggerPolicy.m_diskFullAction,
                        l_loggerPolicy.m_maxDiskSpace);
                //
                // 4 Initial Condition
                //

                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_srcInitialCondition
                    = m_currentProject.InitialCondition[l_storedParameterID];
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_dstInitialCondition
                    = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();

                Copy4InitialCondition(l_srcInitialCondition, l_dstInitialCondition);

                m_currentProject.InitialCondition[l_parameterID] = l_dstInitialCondition;

                m_pManager.ParameterAdd(m_currentProject.Name, l_parameterID);

                Message("Create Simulation Parameter: " + l_message);
                if (l_isRecorded)
                    m_aManager.AddAction(new NewSimParamAction(l_parameterID, l_isAnchor));
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrCreSimParam) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Copy the dm data from the set directory to the project directory.
        /// </summary>
        /// <param name="prjID">Project ID.</param>
        /// <param name="dmList">The list of set dm directory.</param>
        private void CopyDmData(string prjID, List<string> dmList)
        {
            string baseDir = this.m_defaultDir + Constants.delimiterPath + prjID;
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
        /// <param name="prjID">Project ID.</param>
        /// <param name="dmList">A list of DM.</param>
        private void CreateProjectDir(string prjID, List<string> dmList)
        {
            SetDefaultDir();
            string baseDir = this.m_defaultDir + Constants.delimiterPath + prjID;
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
        /// <param name="l_modelID">The saved model ID</param>
        public void SaveModel(string l_modelID)
        {
            List<EcellObject> l_storedList = new List<EcellObject>();
            string l_message = null;
            try
            {
                l_message = "[" + l_modelID + "]";
                //
                // Initializes
                //
                if (l_modelID == null || l_modelID.Length <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrNullData));
                }
                this.SetDefaultDir();
                if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrBaseDir));
                }
                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + m_currentProject.Name))
                {
                    this.SaveProject(m_currentProject.Name);
                }
                string l_modelDirName
                    = this.m_defaultDir + Constants.delimiterPath +
                    m_currentProject.Name + Constants.delimiterPath + Constants.xpathModel;
                if (!Directory.Exists(l_modelDirName))
                {
                    Directory.CreateDirectory(l_modelDirName);
                }
                string l_modelFileName
                    = l_modelDirName + Constants.delimiterPath + l_modelID + Constants.delimiterPeriod + Constants.xpathEml;
                //
                // Picks the "Stepper" up.
                //
                List<EcellObject> l_stepperList
                    = m_currentProject.StepperDic[m_currentProject.SimulationParam][l_modelID];
                if (l_stepperList == null || l_stepperList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindStepper));
                }
                l_storedList.AddRange(l_stepperList);
                //
                // Picks the "System" up.
                //
                List<EcellObject> l_systemList = m_currentProject.SystemDic[l_modelID];
                if (l_systemList == null || l_systemList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindSystem));
                }
                l_storedList.AddRange(l_systemList);
                //
                // Creates.
                //
                EmlWriter.Create(l_modelFileName, l_storedList, true);
                Message("Save Model: " + l_message);
                //
                // 4 Project
                //
                this.SaveProject(m_currentProject.Name);
                m_pManager.SaveModel(l_modelID, l_modelDirName);
            }
            catch (Exception l_ex)
            {
                l_storedList = null;
                l_message = m_resources.GetString(ErrorConstants.ErrSaveModel) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Get Project.
        /// </summary>
        /// <param name="l_prjID"></param>
        /// <returns></returns>
        private Project GetProject(string l_prjID)
        {
            if (string.IsNullOrEmpty(l_prjID))
                throw new Exception(m_resources.GetString(ErrorConstants.ErrNullData));
            if (this.m_projectList == null || this.m_projectList.Count <= 0)
                throw new Exception(m_resources.GetString(ErrorConstants.ErrFindPrj));

            foreach (Project l_prj in this.m_projectList)
                if (l_prj.Name.Equals(l_prjID))
                    return l_prj;

            throw new Exception(m_resources.GetString(ErrorConstants.ErrFindPrj));
        }
        /// <summary>
        /// Saves only the project using the project ID.
        /// </summary>
        /// <param name="l_prjID">The saved project ID</param>
        public void SaveProject(string l_prjID)
        {
            string l_message = null;
            Project l_thisPrj = null;
            try
            {
                // Initializes
                l_message = "[" + l_prjID + "]";
                l_thisPrj = GetProject(l_prjID);
                this.SetDefaultDir();
                if (string.IsNullOrEmpty(m_defaultDir))
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrBaseDir));

                // Saves the project.
                string l_prjFile = Path.Combine(this.m_defaultDir, l_prjID);
                l_thisPrj.Save(l_prjFile);

                Message("Save Project: " + l_message);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrSavePrj) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="l_fileName"></param>
        public void SaveScript(string l_fileName)
        {
            ScriptWriter writer = new ScriptWriter(m_currentProject);
            writer.SaveScript(l_fileName);
        }

        /// <summary>
        /// Saves the simulation parameter.
        /// </summary>
        /// <param name="l_paramID">The simulation parameter ID</param>
        public void SaveSimulationParameter(string l_paramID)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_paramID + "]";
                //
                // Initializes.
                //
                if (l_paramID == null || l_paramID.Length <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrNullData));
                }
                this.SetDefaultDir();
                if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrBaseDir));
                }
                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + m_currentProject.Name))
                {
                    this.SaveProject(m_currentProject.Name);
                }
                string l_simulationDirName =
                    this.m_defaultDir + Constants.delimiterPath +
                    m_currentProject.Name + Constants.delimiterPath + Constants.xpathParameters;
                if (!Directory.Exists(l_simulationDirName))
                {
                    Directory.CreateDirectory(l_simulationDirName);
                }
                string l_simulationFileName
                    = l_simulationDirName + Constants.delimiterPath + l_paramID + Constants.FileExtXML;
                //
                // Picks the "Stepper" up.
                //
                List<EcellObject> l_stepperList = new List<EcellObject>();
                foreach (string l_modelID in m_currentProject.StepperDic[l_paramID].Keys)
                {
                    l_stepperList.AddRange(m_currentProject.StepperDic[l_paramID][l_modelID]);
                }
                if (l_stepperList == null || l_stepperList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindStepper));
                }
                //
                // Picks the "LoggerPolicy" up.
                //
                LoggerPolicy l_loggerPolicy = m_currentProject.LoggerPolicyDic[l_paramID];
                //
                // Picks the "InitialCondition" up.
                //
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_initialCondition
                        = this.m_currentProject.InitialCondition[l_paramID];
                //
                // Creates.
                //
                SimulationParameterWriter.Create(l_simulationFileName,
                    new SimulationParameter(
                        l_stepperList,
                        l_initialCondition,
                        l_loggerPolicy,
                        l_paramID));
                Message("Save Simulation Parameter: " + l_message);
                //
                // 4 Project
                //
                this.SaveProject(m_currentProject.Name);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrSaveSim) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Saves the simulation result.
        /// </summary>
        public void SaveSimulationResult()
        {
            SaveSimulationResult(null, 0.0, GetCurrentSimulationTime(), null, GetLoggerList());
        }

        /// <summary>
        /// Saves the simulation result.
        /// </summary>
        /// <param name="l_savedDirName">The saved directory name</param>
        /// <param name="l_startTime">The start time</param>
        /// <param name="l_endTime">The end time</param>
        /// <param name="l_savedType">The saved type (ECD or Binary)</param>
        /// <param name="l_fullIDList">The list of the saved fullID</param>
        public void SaveSimulationResult(
            string l_savedDirName,
            double l_startTime,
            double l_endTime,
            string l_savedType,
            List<string> l_fullIDList
            )
        {
            string l_message = null;
            try
            {
                l_message =
                    "[" + m_currentProject.Name + "][" + m_currentProject.SimulationParam + "]";
                //
                // Initializes.
                //
                if (l_fullIDList == null || l_fullIDList.Count <= 0)
                {
                    return;
                }
                string l_simulationDirName = null;
                if (!string.IsNullOrEmpty(l_savedDirName))
                {
                    l_simulationDirName = l_savedDirName;
                }
                else
                {
                    this.SetDefaultDir();
                    if (string.IsNullOrEmpty(m_defaultDir))
                    {
                        throw new Exception(m_resources.GetString(ErrorConstants.ErrBaseDir));
                    }
                    if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + m_currentProject.Name))
                    {
                        this.SaveProject(m_currentProject.Name);
                    }
                    l_simulationDirName =
                        this.m_defaultDir + Constants.delimiterPath +
                        m_currentProject.Name + Constants.delimiterPath + Constants.xpathParameters;
                }
                if (!Directory.Exists(l_simulationDirName))
                {
                    Directory.CreateDirectory(l_simulationDirName);
                }
                //
                // Saves the "LogData".
                //
                List<LogData> l_logDataList = this.GetLogData(
                    l_startTime,
                    l_endTime,
                    m_currentProject.LoggerPolicyDic[m_currentProject.SimulationParam].m_reloadInterval
                    );
                if (l_logDataList == null || l_logDataList.Count <= 0)
                {
                    return;
                }
                foreach (LogData l_logData in l_logDataList)
                {
                    string l_fullID =
                        l_logData.type + Constants.delimiterColon +
                        l_logData.key + Constants.delimiterColon +
                        l_logData.propName;
                    if (l_fullIDList.Contains(l_fullID))
                    {
                        if (l_savedType == null || l_savedType.Equals(Constants.xpathCsv) ||
                            l_savedType.Equals(Constants.xpathEcd))
                        {
                            Ecd l_ecd = new Ecd();
                            l_ecd.Create(l_simulationDirName, l_logData, l_savedType);
                            l_message = "[" + l_fullID + "]";
                            Message("Save Simulation Result: " + l_message);
                        }
                    }
                }
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrSaveSim) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Sets the directory name to "m_defaultDir"
        /// </summary>
        private void SetDefaultDir()
        {
            if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
            {
                string l_baseDirs = Util.GetBaseDir();
                if (l_baseDirs == null || l_baseDirs.Length <= 0)
                {
                    return;
                }
                foreach (string l_baseDir in l_baseDirs.Split(Path.PathSeparator))
                {
                    if (Directory.Exists(l_baseDir))
                    {
                        this.m_defaultDir = l_baseDir;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Set the value to the full Path.
        /// </summary>
        /// <param name="l_fullPN">set full path.</param>
        /// <param name="l_value">set value.</param>
        public void SetEntityProperty(string l_fullPN, string l_value)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_fullPN + "]";
                if (m_currentProject.Simulator == null
                    || this.GetCurrentSimulationTime() <= 0.0)
                {
                    return;
                }
                EcellValue l_storedValue
                    = new EcellValue(m_currentProject.Simulator.GetEntityProperty(l_fullPN));
                EcellValue l_newValue = null;
                if (l_storedValue.IsDouble())
                {
                    l_newValue = new EcellValue(XmlConvert.ToDouble(l_value));
                }
                else if (l_storedValue.IsInt())
                {
                    l_newValue = new EcellValue(XmlConvert.ToInt32(l_value));
                }
                else if (l_storedValue.IsList())
                {
                    // l_newValue = new EcellValue(l_value);
                    return;
                }
                else
                {
                    l_newValue = new EcellValue(l_value);
                }
                m_currentProject.Simulator.LoadEntityProperty(
                    l_fullPN,
                    EcellValue.CastToWrappedPolymorph4EcellValue(l_newValue));
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrSetProp) + l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Sets the "LoggerPolicy".
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <param name="l_loggerPolicy">The "LoggerPolicy"</param>
        public void SetLoggerPolicy(string l_parameterID, ref LoggerPolicy l_loggerPolicy)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_parameterID + "]";
                m_currentProject.LoggerPolicyDic[l_parameterID] = l_loggerPolicy;
                Message("Update Logger Policy: " + l_message);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrUpdateLogPol) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Sets the property list.
        /// </summary>
        /// <param name="l_ecellObject">The "EcellObject"</param>
        /// <param name="l_dic">The dictionary of "EcellData"</param>
        private static void SetPropertyList(EcellObject l_ecellObject, Dictionary<string, EcellData> l_dic)
        {
            if (l_ecellObject.Value == null || l_ecellObject.Value.Count <= 0)
            {
                return;
            }
            foreach (EcellData l_ecellData in l_ecellObject.Value)
            {
                if (l_ecellData.Name.Equals(EcellProcess.VARIABLEREFERENCELIST))
                {
                    l_ecellData.Value = new EcellValue(new List<EcellValue>());
                }
                l_dic[l_ecellData.Name] = l_ecellData;
            }
        }

        /// <summary>
        /// Set positions of all EcellObjects.
        /// </summary>
        /// <param name="l_modelID">Model ID</param>
        public void SetPositions(string l_modelID)
        {
            if (null != l_modelID && m_currentProject.SystemDic.ContainsKey(l_modelID))
            {
                foreach (EcellObject eo in m_currentProject.SystemDic[l_modelID])
                    m_pManager.SetPosition(eo);
            }
        }

        /// <summary>
        /// Sets the parameter of the simulator.
        /// </summary>
        /// <param name="l_parameterID">the set parameter ID</param>
        public void SetSimulationParameter(string l_parameterID)
        {
            SetSimulationParameter(l_parameterID, true, true);
        }

        /// <summary>
        /// Sets the parameter of the simulator.
        /// </summary>
        /// <param name="l_parameterID">the set parameter ID</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        public void SetSimulationParameter(string l_parameterID, bool l_isRecorded, bool l_isAnchor)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_parameterID + "]";
                string l_oldParameterID = m_currentProject.SimulationParam;
                if (m_currentProject.SimulationParam != l_parameterID)
                {
                    foreach (string l_modelID in m_currentProject.StepperDic[m_currentProject.SimulationParam].Keys)
                    {
                        if (!m_currentProject.StepperDic[l_parameterID].ContainsKey(l_modelID))
                            continue;

                        List<EcellObject> l_currentList
                            = m_currentProject.StepperDic[m_currentProject.SimulationParam][l_modelID];
                        List<EcellObject> l_newList
                            = m_currentProject.StepperDic[l_parameterID][l_modelID];
                        foreach (EcellObject l_current in l_currentList)
                        {
                            foreach (EcellObject l_new in l_newList)
                            {
                                if (!l_current.Classname.Equals(l_new.Classname))
                                    continue;

                                foreach (EcellData l_currentData in l_current.Value)
                                {
                                    foreach (EcellData l_newData in l_new.Value)
                                    {
                                        if (l_currentData.Name.Equals(l_newData.Name)
                                            && l_currentData.EntityPath.Equals(l_newData.EntityPath))
                                        {
                                            l_newData.Gettable = l_currentData.Gettable;
                                            l_newData.Loadable = l_currentData.Loadable;
                                            l_newData.Saveable = l_currentData.Saveable;
                                            l_newData.Settable = l_currentData.Settable;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    m_currentProject.SimulationParam = l_parameterID;
                    this.Initialize(true);
                    foreach (string l_modelID
                        in m_currentProject.StepperDic[l_oldParameterID].Keys)
                    {
                        foreach (EcellObject l_old
                            in m_currentProject.StepperDic[l_oldParameterID][l_modelID])
                        {
                            List<EcellData> l_delList = new List<EcellData>();
                            foreach (EcellData l_oldData in l_old.Value)
                            {
                                if (l_oldData.Gettable
                                    && !l_oldData.Loadable
                                    && !l_oldData.Saveable
                                    && !l_oldData.Settable)
                                {
                                    l_delList.Add(l_oldData);
                                }
                            }
                            foreach (EcellData l_del in l_delList)
                            {
                                l_old.Value.Remove(l_del);
                            }
                        }
                    }
                }
                m_pManager.ParameterSet(m_currentProject.Name, l_parameterID);
                Message("Set Simulation Parameter: " + l_message);
                if (l_isRecorded)
                    m_aManager.AddAction(new SetSimParamAction(l_parameterID, l_oldParameterID, l_isAnchor));
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString(ErrorConstants.ErrSetSimParam) + l_message;
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Starts this simulation with the step limit.
        /// </summary>
        /// <param name="l_stepLimit">The step limit</param>
        /// <param name="l_statusNum">simulation status.</param>
        public void SimulationStart(int l_stepLimit, int l_statusNum)
        {
            try
            {
                // Checks the simulator's status.
                if (m_currentProject.SimulationStatus == SimulationStatus.Wait)
                {
                    this.Initialize(true);
                    this.m_simulationStepLimit = l_stepLimit;
                    Message("Start Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
                }
                else if (m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    if (this.m_simulationStepLimit == -1)
                    {
                        this.m_simulationStepLimit = l_stepLimit;
                    }
                    Message("Restart Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
                }
                else
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrRunning));
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
                            double l_currentTime = m_currentProject.Simulator.GetCurrentTime();
                            if (l_statusNum == (int)SimulationStatus.Suspended)
                            {
                                m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                            }
                            else
                            {
                                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                            }
                            this.m_pManager.AdvancedTime(l_currentTime);
                            this.m_simulationStepLimit = -1;
                            break;
                        }
                        else
                        {
                            m_currentProject.Simulator.Step(m_defaultStepCount);
                            Application.DoEvents();
                            double l_currentTime = m_currentProject.Simulator.GetCurrentTime();
                            this.m_pManager.AdvancedTime(l_currentTime);
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
                        double l_currentTime = m_currentProject.Simulator.GetCurrentTime();
                        this.m_pManager.AdvancedTime(l_currentTime);
                    }
                }
            }
            catch (Exception l_ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new Exception(m_resources.GetString(ErrorConstants.ErrRunSim) + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Starts this simulation with the time limit.
        /// </summary>
        /// <param name="l_timeLimit">The time limit</param>
        /// <param name="l_statusNum">Simulation status./</param>
        public void SimulationStart(double l_timeLimit, int l_statusNum)
        {
            try
            {
                //
                // Checks the simulator's status.
                //
                double l_startTime = 0.0;
                if (m_currentProject.SimulationStatus == SimulationStatus.Wait)
                {
                    this.Initialize(true);
                    this.m_simulationTimeLimit = l_timeLimit;
                    this.m_simulationStartTime = 0.0;
                    Message("Start Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
                }
                else if (m_currentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    if (this.m_simulationTimeLimit == -1.0 || this.m_simulationTimeLimit == 0.0)
                    {
                        this.m_simulationTimeLimit = l_timeLimit;
                        this.m_simulationStartTime = m_currentProject.Simulator.GetCurrentTime();
                    }
                    Message("Restart Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
                }
                else
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrRunning));
                }
                //
                // Selects the type of the simulation and Starts.
                //
                m_currentProject.SimulationStatus = SimulationStatus.Run;
                if (this.m_simulationTimeLimit > 0.0)
                {
                    l_startTime = m_currentProject.Simulator.GetCurrentTime();
                    Thread l_thread = new Thread(new ThreadStart(SimulationStartByThreadWithLimit));
                    l_thread.Start();
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
                        double l_currentTime = m_currentProject.Simulator.GetCurrentTime();
                        this.m_pManager.AdvancedTime(l_currentTime);
                        if (l_currentTime >= (this.m_simulationStartTime + this.m_simulationTimeLimit))
                        {
                            if (l_statusNum == (int)SimulationStatus.Suspended)
                            {
                                m_currentProject.SimulationStatus = SimulationStatus.Suspended;
                            }
                            else
                            {
                                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                            }
                            l_currentTime = m_currentProject.Simulator.GetCurrentTime();
                            this.m_pManager.AdvancedTime(l_currentTime);
                            this.m_simulationTimeLimit = -1.0;
                            break;
                        }
                    }
                }
                else
                {
                    // Thread l_thread = new Thread(new ThreadStart(SimulationStartByThreadWithLimit));
                    // l_thread.Start();
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
                        double l_currentTime = m_currentProject.Simulator.GetCurrentTime();
                        this.m_pManager.AdvancedTime(l_currentTime);
                    }
                }
            }
            catch (Exception l_ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                string l_message = m_resources.GetString(ErrorConstants.ErrRunSim);
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
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
        /// <param name="l_stepLimit">The step limit</param>
        public void SimulationStartKeepSetting(int l_stepLimit)
        {
            try
            {
                if (m_currentProject.SimulationStatus == SimulationStatus.Run)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrRunning));
                }
                this.SimulationStart(l_stepLimit, (int)SimulationStatus.Suspended);
            }
            catch (Exception l_ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new Exception(m_resources.GetString(ErrorConstants.ErrRunSim) + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Starts this simulation with the time limit.
        /// </summary>
        /// <param name="l_timeLimit">The time limit</param>
        public void SimulationStartKeepSetting(double l_timeLimit)
        {
            try
            {
                if (m_currentProject.SimulationStatus == SimulationStatus.Run)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrRunning));
                }
                this.SimulationStart(l_timeLimit, (int)SimulationStatus.Suspended);
            }
            catch (Exception l_ex)
            {
                m_currentProject.SimulationStatus = SimulationStatus.Wait;
                throw new Exception(m_resources.GetString(ErrorConstants.ErrRunSim) + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Stops this simulation.
        /// </summary>
        public void SimulationStop()
        {
            try
            {
                if (m_currentProject.Simulator == null)
                {
                    throw new Exception(m_resources.GetString(ErrorConstants.ErrFindRunSim));
                }
                lock (m_currentProject.Simulator)
                {
                    m_currentProject.Simulator.Stop();
                }
                Message("Reset Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
            }
            catch (Exception l_ex)
            {
                string l_message = m_resources.GetString(ErrorConstants.ErrResetSim);
                Message(l_message);
                throw new Exception(l_message + "{" + l_ex.ToString() + "}");
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
                Message("Suspend Simulator: [" + m_currentProject.Simulator.GetCurrentTime() + "]");
            }
            catch (Exception l_ex)
            {
                string l_message = m_resources.GetString(ErrorConstants.ErrSuspendSim);
                Message(l_message);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Splits the entity path.
        /// </summary>
        /// <param name="l_key">The key of the "EcellObject"</param>
        /// <param name="l_type">The type of the "EcellObject"</param>
        /// <param name="l_propName">The property name of the "ECellObject"</param>
        /// <param name="l_entityPath"></param>
        private void Split4EntityPath(ref string l_key, ref string l_type, ref string l_propName, string l_entityPath)
        {
            string[] l_data = l_entityPath.Split(Constants.delimiterColon.ToCharArray());
            if (l_data.Length < 4)
            {
                return;
            }
            l_key = l_data[1] + Constants.delimiterColon + l_data[2];
            l_type = l_data[0];
            l_propName = l_data[3];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_parameterID"></param>
        /// <param name="l_modelID"></param>
        /// <param name="l_type"></param>
        /// <param name="l_initialList"></param>
        public void UpdateInitialCondition(
                string l_parameterID, string l_modelID, string l_type, Dictionary<string, double> l_initialList)
        {
            // Check null.
            if (l_initialList == null || l_initialList.Count <= 0)
                return;

            string l_message = null;
            try
            {
                if (string.IsNullOrEmpty(l_parameterID))
                    l_parameterID = m_currentProject.SimulationParam;

                l_message = "[" + l_parameterID + "][" + l_modelID + "][" + l_type + "]";
                Dictionary<string, double> parameters = this.m_currentProject.InitialCondition[l_parameterID][l_modelID][l_type];
                foreach (string l_key in l_initialList.Keys)
                {
                    if (parameters.ContainsKey(l_key))
                        parameters.Remove(l_key);

                    parameters[l_key] = l_initialList[l_key];
                }
                Message("Update Initial Condition: " + l_message);
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrSetInitParam) + l_message
                    + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Updates the "Stepper".
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <param name="l_stepperList">The list of the "Stepper"</param>
        public void UpdateStepperID(string l_parameterID, List<EcellObject> l_stepperList)
        {
            UpdateStepperID(l_parameterID, l_stepperList, true);
        }

        /// <summary>
        /// Updates the "Stepper".
        /// </summary>
        /// <param name="l_parameterID">The parameter ID</param>
        /// <param name="l_stepperList">The list of the "Stepper"</param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        public void UpdateStepperID(string l_parameterID, List<EcellObject> l_stepperList, bool l_isRecorded)
        {
            string l_message = null;
            try
            {
                List<EcellObject> l_oldStepperList = new List<EcellObject>();
                foreach (EcellObject l_stepper in l_stepperList)
                {
                    l_message = "[" + l_parameterID + "][" + l_stepper.ModelID + "][" + l_stepper.Key + "]";
                    if (!m_currentProject.StepperDic[l_parameterID].ContainsKey(l_stepper.ModelID))
                    {
                        throw new Exception(m_resources.GetString(ErrorConstants.ErrFindStepper) + l_message);
                    }
                    bool l_updateFlag = false;
                    List<EcellObject> l_storedStepperList
                        = m_currentProject.StepperDic[l_parameterID][l_stepper.ModelID];
                    for (int i = 0; i < l_storedStepperList.Count; i++)
                    {
                        if (l_storedStepperList[i].Key.Equals(l_stepper.Key))
                        {
                            l_oldStepperList.Add(l_storedStepperList[i]);
                            this.CheckDifferences(l_storedStepperList[i], l_stepper, l_parameterID);
                            l_storedStepperList[i] = l_stepper.Copy();
                            l_updateFlag = true;
                            break;
                        }
                    }
                    if (!l_updateFlag)
                    {
                        throw new Exception(m_resources.GetString(ErrorConstants.ErrFindStepper) + l_message);
                    }
                }
                if (l_isRecorded)
                    m_aManager.AddAction(new UpdateStepperAction(l_parameterID, l_stepperList, l_oldStepperList));
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString(ErrorConstants.ErrSetStepper) + l_message + " {" + l_ex.ToString() + "}");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_simulator"></param>
        private void LookIntoSimulator(WrappedSimulator l_simulator)
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
            m_aManager.UndoAction();
        }

        /// <summary>
        /// Redo action.
        /// </summary>
        public void RedoAction()
        {
            m_aManager.RedoAction();
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
            Message("Create " + type + ": " + message);
        }
        /// <summary>
        /// Message on DeleteEntity
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void MessageDeleteEntity(string type, string message)
        {
            Message("Delete " + type + ": " + message);
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
            Message(
                "Update Data: " + message + "[" + type + "]" + System.Environment.NewLine
                    + "\t[" + src + "]->[" + dest + "]");
        }
        /// <summary>
        /// Message
        /// </summary>
        /// <param name="message"></param>
        public void Message(string message)
        {
            this.m_pManager.Message(Constants.messageSimulation, message + System.Environment.NewLine);
        }
        #endregion
    }
}