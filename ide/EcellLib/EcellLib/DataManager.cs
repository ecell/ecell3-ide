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

namespace EcellLib
{
    /// <summary>
    /// Manages data of projects, models, and so on.
    /// </summary>
    public class DataManager
    {
        #region Fields
        /// <summary>
        /// The current parameter ID
        /// </summary>
        private string m_currentParameterID = null;
        /// <summary>
        /// The current project ID
        /// </summary>
        private string m_currentProjectID = null;
        /// <summary>
        /// The default directory
        /// </summary>
        private string m_defaultDir = null;
        /// <summary>
        /// The list of the DM
        /// </summary>
        private Dictionary<string, List<string>> m_dmDic = null;
        /// <summary>
        /// The dictionary of the "InitialCondition" with
        ///     the project ID, the parameter ID, the model ID, the data type and the full ID
        /// </summary>
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string,
                Dictionary<string, double>>>>> m_initialCondition = null;
        /// <summary>
        /// The dictionary of the logable entity path
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> m_logableEntityPathDic = null;
        /// <summary>
        /// The dictionary of the "LoggerPolicy" with the project ID and the parameter ID
        /// </summary>
        private Dictionary<string, Dictionary<string, LoggerPolicy>> m_loggerPolicyDic = null;
        /// <summary>
        /// The dictionary of the "Model" with the project ID
        /// </summary>
        private Dictionary<string, List<EcellObject>> m_modelDic = null;
        /// <summary>
        /// The "PluginManager"
        /// </summary>
        private PluginManager m_pManager = null;
        /// <summary>
        /// The list of "Project" with the project ID
        /// </summary>
        private List<Project> m_projectList = null;
        private String m_currentProjectPath = null;
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
        /// The dictionary of "Simulator" with the project ID
        /// </summary>
        private Dictionary<string, WrappedSimulator> m_simulatorDic = null;
        /// <summary>
        /// The dictionary of the executed flag of "Simulator" with the project ID
        /// </summary>
        private Dictionary<string, int> m_simulatorExeFlagDic = null;
        /// <summary>
        /// The dictionary of the "Stepper" with the project ID, the parameter ID and the model ID
        /// </summary>
        private Dictionary<string, Dictionary<string, Dictionary<string, List<EcellObject>>>> m_stepperDic = null;
        /// <summary>
        /// The dictionary of the "System" with the project ID and the model ID 
        /// </summary>
        private Dictionary<string, Dictionary<string, List<EcellObject>>> m_systemDic = null;
        /// <summary>
        /// The default process name
        /// </summary>
        private const string s_defaultProcessName = "ConstantFluxProcess"; // "BisectionRapidEquilibriumProcess";
        /// <summary>
        /// The default count of the step 
        /// </summary>
        private int s_defaultStepCount = 10;
        /// <summary>
        /// The default stepper name
        /// </summary>
        private const string s_defaultStepperName = "FixedODE1Stepper";
        /// <summary>
        /// s_instance (singleton instance)
        /// </summary>
        private static DataManager s_instance = null;
        /// <summary>
        /// The running flag of the simulation
        /// </summary>
        private const int s_simulationRun = 1;
        /// <summary>
        /// The suspending flag of the simulation
        /// </summary>
        private const int s_simulationSuspend = 2;
        /// <summary>
        /// The waiting flag of the simulation
        /// </summary>
        private const int s_simulationWait = 0;
        /// <summary>
        /// ActionManager.
        /// </summary>
        private ActionManager m_aManager;
        /// <summary>
        /// The dictionary of loading the file of model.
        /// </summary>
        private Dictionary<string, string> m_loadDirList;

        private string m_loadingProject = null;
        private int m_processNumbering = 0;
        private int m_systemNumbering = 0;
        private int m_variableNumbering = 0;
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
            this.m_initialCondition = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string,
                    Dictionary<string, double>>>>>();
            this.m_loggerPolicyDic = new Dictionary<string, Dictionary<string, LoggerPolicy>>();
            this.m_modelDic = new Dictionary<string, List<EcellObject>>();
            this.m_pManager = PluginManager.GetPluginManager();
            this.m_projectList = new List<Project>();
            this.m_simulatorDic = new Dictionary<string, WrappedSimulator>();
            this.m_simulatorExeFlagDic = new Dictionary<string, int>();
            this.m_stepperDic = new Dictionary<string, Dictionary<string, Dictionary<string, List<EcellObject>>>>();
            this.m_systemDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
            this.m_loadDirList = new Dictionary<string, string>();
            m_aManager = ActionManager.GetActionManager();
        }

        /// <summary>
        /// get / set StepCount
        /// </summary>
        public int StepCount
        {
            get { return this.s_defaultStepCount; }
            set { this.s_defaultStepCount = value; }
        }

        /// <summary>
        /// get / set CurrentProjectID
        /// </summary>
        public string CurrentProjectID
        {
            get { return this.m_currentProjectID; }
        }

        /// <summary>
        /// get CurrentProjectPath
        /// </summary>
        public string CurrentProjectPath
        {
            get { return this.m_currentProjectPath; }
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
            try
            {
                l_message = "[" + l_parameterID + "][" + l_stepper.modelID + "][" + l_stepper.key + "]";
                if (l_stepper != null && l_stepper.modelID != null && l_stepper.modelID.Length > 0)
                {
                    if (!this.m_stepperDic[this.m_currentProjectID].ContainsKey(l_parameterID))
                    {
                        this.m_stepperDic[this.m_currentProjectID][l_parameterID]
                                = new Dictionary<string, List<EcellObject>>();
                    }
                    if (this.m_stepperDic[this.m_currentProjectID][l_parameterID]
                            .ContainsKey(l_stepper.modelID))
                    {
                        foreach (EcellObject l_storedStepper in
                            this.m_stepperDic[this.m_currentProjectID][l_parameterID][l_stepper.modelID])
                        {
                            if (l_stepper.key.Equals(l_storedStepper.key))
                            {
                                throw new Exception(
                                    String.Format(
                                        m_resources.GetString("ErrExistStepper"),
                                        new object[] { l_message }
                                    )
                                );
                            }
                        }
                        this.m_stepperDic[this.m_currentProjectID][l_parameterID][l_stepper.modelID]
                                .Add(l_stepper);
                        if (m_currentParameterID.Equals(l_parameterID))
                        {
                            List<EcellObject> stepperList = new List<EcellObject>();
                            stepperList.Add(l_stepper);
                            m_pManager.DataAdd(stepperList);
                        }
                        this.m_pManager.Message(
                            Constants.messageSimulation,
                            "Create Stepper: " + l_message + System.Environment.NewLine
                            );
                    }
                }
                if (l_isRecorded)
                    m_aManager.AddAction(new AddStepperAction(l_parameterID, l_stepper));
            }
            catch (Exception l_ex)
            {
                l_message = l_message + m_resources.GetString("ErrNotCreStepper");
                this.m_pManager.Message(
                        Constants.messageSimulation, l_message + System.Environment.NewLine);
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
            string l_message = null;
            if (l_parameterID != null && l_parameterID.Length > 0)
            {
                l_message = "[" + l_parameterID + "][" + l_src.modelID + "][" + l_src.key + "]";
            }
            else
            {
                l_message = "[" + l_src.modelID + "][" + l_src.key + "]";
            }
            if (!l_src.classname.Equals(l_dest.classname))
            {
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Update Data: " + l_message + "[" + Constants.xpathClassName + "]"
                        + System.Environment.NewLine
                        + "\t[" + l_src.classname + "]->[" + l_dest.classname + "]"
                        + System.Environment.NewLine);
            }
            if (!l_src.key.Equals(l_dest.key))
            {
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Update Data: " + l_message + "[" + Constants.xpathKey + "]"
                        + System.Environment.NewLine
                        + "\t[" + l_src.key + "]->[" + l_dest.key + "]"
                        + System.Environment.NewLine);
            }
            //
            // Changes a className and not change a key.
            //
            if (!l_src.classname.Equals(l_dest.classname) && l_src.key.Equals(l_dest.key))
            {
                foreach (EcellData l_srcEcellData in l_src.Value)
                {
                    //
                    // Changes the logger.
                    //
                    if (l_srcEcellData.Logged)
                    {
                        this.m_pManager.Message(
                            Constants.messageSimulation,
                            "Delete Logger: " + l_message + "[" + l_srcEcellData.Name + "]"
                                + System.Environment.NewLine);
                    }
                    //
                    // Changes the initial parameter.
                    //
                    if (l_src.type.Equals(Constants.xpathSystem)
                        || l_src.type.Equals(Constants.xpathProcess)
                        || l_src.type.Equals(Constants.xpathVariable))
                    {
                        if (l_srcEcellData.IsInitialized())
                        {
                            if (l_parameterID != null && l_parameterID.Length > 0)
                            {
                                if (this.m_initialCondition[this.m_currentProjectID][l_parameterID]
                                    [l_src.modelID][l_src.type].ContainsKey(l_srcEcellData.EntityPath))
                                {
                                    this.m_initialCondition[this.m_currentProjectID][l_parameterID]
                                    [l_src.modelID][l_src.type].Remove(l_srcEcellData.EntityPath);
                                }
                            }
                            else
                            {
                                foreach (string l_keyParameterID
                                    in this.m_initialCondition[this.m_currentProjectID].Keys)
                                {
                                    if (this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                        [l_src.modelID][l_src.type].ContainsKey(l_srcEcellData.EntityPath))
                                    {
                                        this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                        [l_src.modelID][l_src.type].Remove(l_srcEcellData.EntityPath);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (EcellData l_destEcellData in l_dest.Value)
                {
                    //
                    // Changes the initial parameter.
                    //
                    if (l_dest.type.Equals(Constants.xpathSystem)
                        || l_dest.type.Equals(Constants.xpathProcess)
                        || l_dest.type.Equals(Constants.xpathVariable))
                    {
                        if (l_destEcellData.IsInitialized())
                        {
                            if (l_parameterID != null && l_parameterID.Length > 0)
                            {
                                if (l_destEcellData.Value.IsDouble())
                                {
                                    this.m_initialCondition[this.m_currentProjectID][l_parameterID]
                                        [l_dest.modelID][l_dest.type][l_destEcellData.EntityPath]
                                        = l_destEcellData.Value.CastToDouble();
                                }
                                else if (l_destEcellData.Value.IsInt())
                                {
                                    this.m_initialCondition[this.m_currentProjectID][l_parameterID]
                                        [l_dest.modelID][l_dest.type][l_destEcellData.EntityPath]
                                        = l_destEcellData.Value.CastToInt();
                                }
                            }
                            else
                            {
                                if (l_destEcellData.Value.IsDouble())
                                {
                                    foreach (string l_keyParameterID
                                        in this.m_initialCondition[this.m_currentProjectID].Keys)
                                    {
                                        this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                            [l_dest.modelID][l_dest.type][l_destEcellData.EntityPath]
                                            = l_destEcellData.Value.CastToDouble();
                                    }
                                }
                                else if (l_destEcellData.Value.IsInt())
                                {
                                    foreach (string l_keyParameterID
                                        in this.m_initialCondition[this.m_currentProjectID].Keys)
                                    {
                                        this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                            [l_dest.modelID][l_dest.type][l_destEcellData.EntityPath]
                                            = l_destEcellData.Value.CastToInt();
                                    }
                                }
                            }
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
                        if (l_srcEcellData.Name.Equals(l_destEcellData.Name) &&
                            l_srcEcellData.EntityPath.Equals(l_destEcellData.EntityPath))
                        {
                            if (!l_srcEcellData.Logged && l_destEcellData.Logged)
                            {
                                this.m_pManager.Message(
                                    Constants.messageSimulation,
                                    "Create Logger: " + l_message + "[" + l_srcEcellData.Name + "]"
                                        + System.Environment.NewLine);
                            }
                            else if (l_srcEcellData.Logged && !l_destEcellData.Logged)
                            {
                                this.m_pManager.Message(
                                    Constants.messageSimulation,
                                    "Delete Logger: " + l_message + "[" + l_srcEcellData.Name + "]"
                                        + System.Environment.NewLine);
                            }
                            if (!l_srcEcellData.Value.ToString()
                                    .Equals(l_destEcellData.Value.ToString()))
                            {
                                this.m_pManager.Message(
                                    Constants.messageSimulation,
                                    "Update Data: " + l_message
                                        + "[" + l_srcEcellData.Name + "]"
                                        + System.Environment.NewLine
                                        + "\t[" + l_srcEcellData.Value.ToString()
                                        + "]->[" + l_destEcellData.Value.ToString() + "]"
                                        + System.Environment.NewLine);
                            }
                            //
                            // Changes the initial parameter.
                            //
                            if (l_src.type.Equals(Constants.xpathSystem)
                                || l_src.type.Equals(Constants.xpathProcess)
                                || l_src.type.Equals(Constants.xpathVariable))
                            {
                                if (!l_srcEcellData.Value.Equals(l_destEcellData.Value)
                                    && l_srcEcellData.IsInitialized())
                                {
                                    if (l_parameterID != null && l_parameterID.Length > 0)
                                    {
                                        if (this.m_initialCondition[this.m_currentProjectID][l_parameterID]
                                            [l_src.modelID][l_src.type].ContainsKey(l_srcEcellData.EntityPath))
                                        {
                                            if (l_destEcellData.Value.IsDouble())
                                            {
                                                this.m_initialCondition[this.m_currentProjectID][l_parameterID]
                                                        [l_src.modelID][l_src.type][l_srcEcellData.EntityPath]
                                                    = l_destEcellData.Value.CastToDouble();
                                            }
                                            else if (l_destEcellData.Value.IsInt())
                                            {
                                                this.m_initialCondition[this.m_currentProjectID][l_parameterID]
                                                        [l_src.modelID][l_src.type][l_srcEcellData.EntityPath]
                                                    = l_destEcellData.Value.CastToInt();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (string l_keyParameterID
                                                in this.m_initialCondition[this.m_currentProjectID].Keys)
                                        {
                                            if (this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                                    [l_src.modelID][l_src.type].ContainsKey(
                                                    l_srcEcellData.EntityPath))
                                            {
                                                if (l_destEcellData.Value.IsDouble())
                                                {
                                                    this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                                            [l_src.modelID][l_src.type][l_srcEcellData.EntityPath]
                                                        = l_destEcellData.Value.CastToDouble();
                                                }
                                                else if (l_destEcellData.Value.IsInt())
                                                {
                                                    this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                                            [l_src.modelID][l_src.type][l_srcEcellData.EntityPath]
                                                        = l_destEcellData.Value.CastToInt();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks differences between the key and the entity path.
        /// </summary>
        /// <param name="l_ecellObject">The checked "EcellObject"</param>
        private void CheckEntityPath(EcellObject l_ecellObject)
        {
            if (l_ecellObject.key == null)
            {
                return;
            }
            if (l_ecellObject.type.Equals(Constants.xpathSystem))
            {
                string l_entityPath = null;
                string l_parentPath
                    = l_ecellObject.key.Substring(0, l_ecellObject.key.LastIndexOf(Constants.delimiterPath));
                string l_childPath
                    = l_ecellObject.key.Substring(l_ecellObject.key.LastIndexOf(Constants.delimiterPath) + 1);
                if (l_ecellObject.key.Equals(Constants.delimiterPath))
                {
                    if (l_childPath.Length == 0)
                    {
                        l_childPath = Constants.delimiterPath;
                    }
                }
                else
                {
                    if (l_parentPath.Length == 0)
                    {
                        l_parentPath = Constants.delimiterPath;
                    }
                }
                l_entityPath = l_ecellObject.type + Constants.delimiterColon
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
                        this.CheckEntityPath(l_ecellObject.Children[i]);
                    }
                }
            }
            else if (l_ecellObject.type.Equals(Constants.xpathProcess) || l_ecellObject.type.Equals(Constants.xpathVariable))
            {
                string l_entityPath
                    = l_ecellObject.type + Constants.delimiterColon
                    + l_ecellObject.key + Constants.delimiterColon;
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
        private bool CheckVariableReferenceList(
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
            foreach (EcellObject l_system in this.GetData(l_modelID, null))
            {
                if (l_system.Children == null || l_system.Children.Count <= 0)
                {
                    continue;
                }
                foreach (EcellObject l_instance in l_system.Children)
                {
                    EcellObject l_entity = l_instance.Copy();
                    if (!l_entity.type.Equals(Constants.xpathProcess))
                    {
                        continue;
                    }
                    else if (l_entity.Value == null || l_entity.Value.Count <= 0)
                    {
                        continue;
                    }
                    bool l_changedFlag = false;
                    foreach (EcellData l_ecellData in l_entity.Value)
                    {
                        if (l_ecellData.Name.Equals(Constants.xpathVRL))
                        {
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
                        l_tmpList.Add(prj.M_prjName);
                    }
                }
                else
                {
                    l_tmpList.Add(l_prj);
                }

                foreach (string l_str in l_tmpList)
                {
                    if (this.m_stepperDic.ContainsKey(l_str))
                    {
                        this.m_stepperDic.Remove(l_str);
                    }
                    if (this.m_loggerPolicyDic.ContainsKey(l_str))
                    {
                        this.m_loggerPolicyDic.Remove(l_str);
                    }
                    if (this.m_systemDic.ContainsKey(l_str))
                    {
                        this.m_systemDic.Remove(l_str);
                    }
                    if (this.m_modelDic.ContainsKey(l_str))
                    {
                        foreach (EcellObject l_model in m_modelDic[l_str])
                        {
                            if (this.m_loadDirList.ContainsKey(l_model.modelID))
                            {
                                this.m_loadDirList.Remove(l_model.modelID);
                            }
                        }
                        this.m_modelDic.Remove(l_str);
                    }
                    if (this.m_simulatorDic.ContainsKey(l_str))
                    {
                        this.m_simulatorDic[l_str].Dispose();
                        this.m_simulatorDic.Remove(l_str);
                    }
                    if (this.m_initialCondition.ContainsKey(l_str))
                    {
                        this.m_initialCondition.Remove(l_str);
                    }

                    foreach (Project prj in m_projectList)
                    {
                        if (prj.M_prjName == l_str)
                        {
                            m_projectList.Remove(prj);
                            break;
                        }
                    }
                }

                this.m_currentProjectID = null;
                this.m_currentParameterID = null;
                this.m_pManager.AdvancedTime(0);
                this.m_pManager.Clear();
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Close Project: " + l_message + System.Environment.NewLine + System.Environment.NewLine
                    );
                m_aManager.Clear();
                ResetTemporaryID();
            }
            catch (Exception l_ex)
            {
                l_message = l_message + m_resources.GetString("ErrClosePrj");
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
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
                foreach (EcellObject l_ecellObject in this.m_modelDic[this.m_currentProjectID])
                {
                    if (l_ecellObject.modelID.Equals(l_modelID))
                    {
                        l_foundFlag = true;
                        break;
                    }
                }
                return l_foundFlag;
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrFindModel") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Copys the initial condition.
        /// </summary>
        /// <param name="l_srcDic">The source</param>
        /// <param name="l_destDic">The destination</param>
        private void Copy4InitialCondition(
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
        private void BuildDefaultSimulator(
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
                        l_defaultProcess = s_defaultProcessName;
                        l_processFlag = true;
                    }
                    //
                    // 4 Stepper
                    //
                    if (!l_stepperFlag)
                    {
                        l_defaultStepper = s_defaultStepperName;
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
                    s_resources.GetString("ErrCombiStepProc") +
                    "[" + l_defaultStepper + ", " + l_defaultProcess + "]", l_ex);
            }
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
            if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun ||
                this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationSuspend)
            {
                String mes = m_resources.GetString("ConfirmReset");
                DialogResult r = MessageBox.Show(mes,
                    "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (r != DialogResult.OK)
                {
                    throw new IgnoreException("Can't delete the object.");
                }
                SimulationStop();
                m_pManager.ChangeStatus(ProjectStatus.Loaded);
            }

            List<EcellObject> l_usableList = new List<EcellObject>();
            string l_message = null;
            bool l_isUndoable = true; // Whether DataAdd action is undoable or not
            try
            {
                foreach (EcellObject l_ecellObject in l_ecellObjectList)
                {
                    l_message = "[" + l_ecellObject.modelID + "][" + l_ecellObject.key + "]";
                    if (!this.IsUsable(l_ecellObject))
                    {
                        continue;
                    }
                    if (l_ecellObject.type.Equals(Constants.xpathProcess))
                    {
                        this.DataAdd4Entity(l_ecellObject, true);
                        l_usableList.Add(l_ecellObject);
                    }
                    else if (l_ecellObject.type.Equals(Constants.xpathStepper))
                    {
                        // this.DataAdd4Stepper(l_ecellObject);
                        // l_usableList.Add(l_ecellObject);
                    }
                    else if (l_ecellObject.type.Equals(Constants.xpathSystem))
                    {
                        this.DataAdd4System(l_ecellObject, true);
                        if ("/".Equals(l_ecellObject.key))
                            l_isUndoable = false;
                        l_usableList.Add(l_ecellObject);
                    }
                    else if (l_ecellObject.type.Equals(Constants.xpathVariable))
                    {
                        this.DataAdd4Entity(l_ecellObject, true);
                        l_usableList.Add(l_ecellObject);
                    }
                    else if (l_ecellObject.type.Equals(Constants.xpathModel))
                    {
                        l_isUndoable = false;
                        this.DataAdd4Model(l_ecellObject, l_usableList);
                    }
                }
            }
            catch (Exception l_ex)
            {
                l_usableList = null;
                l_message = l_message + m_resources.GetString("ErrAddObj");
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
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
//                        m_pManager.SetPosition(obj);
                        if (l_isRecorded)
                            m_aManager.AddAction(new DataAddAction(obj, l_isUndoable, l_isAnchor));
                    }
                }
            }
        }

        bool m_isAdded = false;

        /// <summary>
        /// Adds the "Process" or the "Variable".
        /// </summary>
        /// <param name="l_ecellObject">The "Variable"</param>
        /// <param name="l_messageFlag">The flag of the messages</param>
        private void DataAdd4Entity(EcellObject l_ecellObject, bool l_messageFlag)
        {
            string l_message = "[" + l_ecellObject.modelID + "][" + l_ecellObject.key + "]";
            if (this.m_systemDic[this.m_currentProjectID] == null ||
                this.m_systemDic[this.m_currentProjectID].Count <= 0 ||
                !this.m_systemDic[this.m_currentProjectID].ContainsKey(l_ecellObject.modelID)
                )
            {
                throw new Exception(m_resources.GetString("ErrFindSuper"));
            }

            bool l_findFlag = false;
            string l_systemKey = Constants.delimiterPath;
            if (l_ecellObject.key.IndexOf(Constants.delimiterColon) > 0)
            {
                l_systemKey = l_ecellObject.key.Substring(0, l_ecellObject.key.IndexOf(Constants.delimiterColon));
                if (!l_systemKey.Equals("/"))
                {
                    l_systemKey.Substring(0, l_systemKey.Length - 1);
                }
            }
            foreach (EcellObject l_parentSystem in this.m_systemDic[this.m_currentProjectID][l_ecellObject.modelID])
            {
                if (l_parentSystem.modelID.Equals(l_ecellObject.modelID) && l_parentSystem.key.Equals(l_systemKey))
                {
                    if (l_parentSystem.Children == null)
                    {
                        l_parentSystem.Children = new List<EcellObject>();
                        this.CheckEntityPath(l_ecellObject);
                        l_parentSystem.Children.Add(l_ecellObject.Copy());
                        l_findFlag = true;
                        if (l_messageFlag)
                        {
                            this.m_pManager.Message(
                                Constants.messageSimulation,
                                "Create " + l_ecellObject.type + ": " + l_message + System.Environment.NewLine);
                        }
                        break;
                    }
                    else
                    {
                        foreach (EcellObject l_child in l_parentSystem.Children)
                        {
                            if (l_child.key.Equals(l_ecellObject.key) && l_child.type.Equals(l_ecellObject.type))
                            {
                                throw new Exception(
                                    String.Format(
                                        m_resources.GetString("ErrExistObj"),
                                        new object[] {
                                            l_message + " " + l_ecellObject.type.ToLower()
                                        }
                                    )
                                );
                                ;
                            }
                        }
                        this.CheckEntityPath(l_ecellObject);
                        l_parentSystem.Children.Add(l_ecellObject.Copy());
                        l_findFlag = true;
                        if (l_messageFlag)
                        {
                            this.m_pManager.Message(
                            Constants.messageSimulation,
                            "Create " + l_ecellObject.type + ": " + l_message + System.Environment.NewLine);
                        }
                    }
                }
            }
            if (!l_findFlag)
            {
                l_message = l_message + " " + l_ecellObject.type.ToLower() + m_resources.GetString("ErrAddObj");
                if (l_messageFlag)
                {
                    this.m_pManager.Message(
                        Constants.messageSimulation, l_message + System.Environment.NewLine);
                }
                throw new Exception(l_message);
            }
            else
            {
                if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
                {
                    foreach (EcellData l_data in l_ecellObject.Value)
                    {
                        if (l_data.IsInitialized())
                        {
                            if (l_data.Value.IsDouble())
                            {
                                foreach (string l_keyParameterID
                                        in this.m_initialCondition[this.m_currentProjectID].Keys)
                                {
                                    this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                            [l_ecellObject.modelID][l_ecellObject.type][l_data.EntityPath]
                                        = l_data.Value.CastToDouble();
                                }
                            }
                            else if (l_data.Value.IsInt())
                            {
                                foreach (string l_keyParameterID
                                        in this.m_initialCondition[this.m_currentProjectID].Keys)
                                {
                                    this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                            [l_ecellObject.modelID][l_ecellObject.type][l_data.EntityPath]
                                        = l_data.Value.CastToInt();
                                }
                            }
                        }
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
            string l_message = "[" + l_ecellObject.modelID + "]";
            foreach (EcellObject l_model in this.m_modelDic[this.m_currentProjectID])
            {
                if (l_model.modelID.Equals(l_ecellObject.modelID))
                {
                    throw new Exception(
                        String.Format(
                            m_resources.GetString("ErrExistObj"),
                            new object[] { l_message }
                        )
                    );
                }
            }
            //
            // Sets the "Model".
            //
            if (this.m_modelDic[this.m_currentProjectID] == null)
            {
                this.m_modelDic[this.m_currentProjectID] = new List<EcellObject>();
            }
            this.m_modelDic[this.m_currentProjectID].Add(l_ecellObject);
            l_usableList.Add(l_ecellObject);
            //
            // Sets the root "System".
            //
            if (this.m_systemDic[this.m_currentProjectID] == null)
            {
                this.m_systemDic[this.m_currentProjectID] = new Dictionary<string, List<EcellObject>>();
            }
            if (!this.m_systemDic[this.m_currentProjectID].ContainsKey(l_ecellObject.modelID))
            {
                this.m_systemDic[this.m_currentProjectID][l_ecellObject.modelID] = new List<EcellObject>();
            }
            Dictionary<string, EcellObject> l_dic = GetDefaultSystem(l_ecellObject.modelID);
            Debug.Assert(l_dic != null);
            this.m_systemDic[this.m_currentProjectID][l_ecellObject.modelID].Add(l_dic[Constants.xpathSystem]);
            l_usableList.Add(l_dic[Constants.xpathSystem]);
            //
            // Sets the default parameter.
            //
            if (this.m_currentParameterID == null || this.m_currentParameterID.Length <= 0)
            {
                this.m_currentParameterID = Constants.parameterKey;
                this.m_stepperDic[this.m_currentProjectID]
                    = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID]
                    = new Dictionary<string, List<EcellObject>>();
                this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID][l_ecellObject.modelID]
                    = new List<EcellObject>();
                this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID][l_ecellObject.modelID]
                    .Add(l_dic[Constants.xpathStepper]);
                this.m_loggerPolicyDic[this.m_currentProjectID] = new Dictionary<string, LoggerPolicy>();
                this.m_loggerPolicyDic[this.m_currentProjectID][this.m_currentParameterID]
                    = new LoggerPolicy(
                        LoggerPolicy.s_reloadStepCount,
                        LoggerPolicy.s_reloadInterval,
                        LoggerPolicy.s_diskFullAction,
                        LoggerPolicy.s_maxDiskSpace
                        );
                //
                // Sets initial conditions.
                //
                this.m_initialCondition[this.m_currentProjectID]
                        = new Dictionary<string, Dictionary<string, Dictionary<string,
                                Dictionary<string, double>>>>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        [l_ecellObject.modelID] = new Dictionary<string, Dictionary<string, double>>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        [l_ecellObject.modelID][Constants.xpathSystem] = new Dictionary<string, double>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        [l_ecellObject.modelID][Constants.xpathProcess] = new Dictionary<string, double>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        [l_ecellObject.modelID][Constants.xpathVariable] = new Dictionary<string, double>();
            }
            //
            // Messages
            //
            this.m_pManager.Message(
                Constants.messageSimulation,
                "Create Model: " + l_message + System.Environment.NewLine
                );
            this.m_pManager.Message(
                Constants.messageSimulation,
                "Create System: " + l_message + "[/]" + System.Environment.NewLine
                );
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
            string l_message = "[" + l_ecellObject.modelID + "][" + l_ecellObject.key + "]";
            if (this.m_systemDic[this.m_currentProjectID].ContainsKey(l_ecellObject.modelID))
            {
                foreach (EcellObject l_system in this.m_systemDic[this.m_currentProjectID][l_ecellObject.modelID])
                {
                    if (l_system.key.Equals(l_ecellObject.key))
                    {
                        throw new Exception(l_message + m_resources.GetString("ErrAddObj"));
                    }
                }
            }
            else
            {
                this.m_systemDic[this.m_currentProjectID][l_ecellObject.modelID] = new List<EcellObject>();
            }
            this.CheckEntityPath(l_ecellObject);
            this.m_systemDic[this.m_currentProjectID][l_ecellObject.modelID].Add(l_ecellObject.Copy());
            if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
            {
                foreach (EcellData l_data in l_ecellObject.Value)
                {
                    if (l_data.IsInitialized())
                    {
                        if (l_data.Value.IsDouble())
                        {
                            foreach (string l_keyParameterID
                                    in this.m_initialCondition[this.m_currentProjectID].Keys)
                            {
                                this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                        [l_ecellObject.modelID][l_ecellObject.type][l_data.EntityPath]
                                    = l_data.Value.CastToDouble();
                            }
                        }
                        else if (l_data.Value.IsInt())
                        {
                            foreach (string l_keyParameterID
                                    in this.m_initialCondition[this.m_currentProjectID].Keys)
                            {
                                this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                        [l_ecellObject.modelID][l_ecellObject.type][l_data.EntityPath]
                                    = l_data.Value.CastToInt();
                            }
                        }
                    }
                }
            }
            if (l_messageFlag)
            {
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Create System: " + l_message + System.Environment.NewLine);
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
                DataChanged(obj.modelID, obj.key, obj.type, obj, l_isRecorded, l_isAnchor);
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
            if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun ||
                this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationSuspend)
            {
                String mes = m_resources.GetString("ConfirmReset");
                DialogResult r = MessageBox.Show(mes,
                    "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (r != DialogResult.OK)
                {
                    throw new IgnoreException("Can't change the object.");
                }
                SimulationStop();
                m_pManager.ChangeStatus(ProjectStatus.Loaded);
            }
            string l_message = null;
            try
            {
                l_message = "[" + l_ecellObject.modelID + "][" + l_ecellObject.key + "]";
                if (l_modelID == null || l_modelID.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrNullData") + l_message);
                }
                else if (l_key == null || l_key.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrNullData") + l_message);
                }
                else if (l_type == null || l_type.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrNullData") + l_message);
                }

                // Record action
                EcellObject l_oldObj = GetEcellObject(l_modelID, l_key, l_type);
                //if (!l_oldObj.IsPosSet)
                //    m_pManager.SetPosition(l_oldObj);
                if (l_isRecorded && !m_isAdded)
                    this.m_aManager.AddAction(new DataChangeAction(l_modelID, l_type, l_oldObj, l_ecellObject.Copy(), l_isAnchor));

                //
                // Searches the "System".
                //
                List<EcellObject> l_systemList = this.m_systemDic[this.m_currentProjectID][l_modelID];
                if (l_systemList == null || l_systemList.Count <= 0)
                    throw new Exception(m_resources.GetString("ErrFindSystem") + l_message);
                //
                // Checks the EcellObject
                //
                this.CheckEntityPath(l_ecellObject);
                //
                // 4 System & Entity
                //
                if (l_ecellObject.type.Equals(Constants.xpathSystem))
                {
                    this.DataChanged4System(l_modelID, l_key, l_type, l_ecellObject, l_isRecorded, l_isAnchor);
                }
                else if (l_ecellObject.type.Equals(Constants.xpathProcess))
                {
                    this.DataChanged4Entity(l_modelID, l_key, l_type, l_ecellObject, l_isRecorded, l_isAnchor);
                }
                else if (l_ecellObject.type.Equals(Constants.xpathVariable))
                {
                    this.DataChanged4Entity(l_modelID, l_key, l_type, l_ecellObject, l_isRecorded, l_isAnchor);
                    if (!l_modelID.Equals(l_ecellObject.modelID) || !l_key.Equals(l_ecellObject.key))
                    {
                        List<EcellObject> l_changedProcessList = new List<EcellObject>();
                        this.CheckVariableReferenceList(l_modelID, l_ecellObject.key, l_key, l_changedProcessList);
                        foreach (EcellObject l_changedProcess in l_changedProcessList)
                        {
                            this.DataChanged4Entity(
                                l_changedProcess.modelID, l_changedProcess.key,
                                l_changedProcess.type, l_changedProcess, l_isRecorded, l_isAnchor);
                        }
                    }
                }
            }
            catch (Exception l_ex)
            {
                l_message = String.Format(
                    m_resources.GetString("ErrUpdate"),
                    new object[] { l_ecellObject.type }
                ) + l_message + " " + l_ecellObject.type;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
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
            string l_message = "[" + l_ecellObject.modelID + "][" + l_ecellObject.key + "]";
            List<EcellObject> l_changedProcessList = new List<EcellObject>();
            List<EcellObject> l_systemList = this.m_systemDic[this.m_currentProjectID][l_modelID];
            for (int i = 0; i < l_systemList.Count; i++)
            {
                if (l_systemList[i].Children == null || l_systemList[i].Children.Count <= 0)
                    continue;

                for (int j = 0; j < l_systemList[i].Children.Count; j++)
                {
                    if (l_systemList[i].Children[j].modelID.Equals(l_modelID)
                        && l_systemList[i].Children[j].key.Equals(l_key)
                        && l_systemList[i].Children[j].type.Equals(l_type))
                    {
                        this.CheckDifferences(l_systemList[i].Children[j], l_ecellObject, null);
                        if (l_modelID.Equals(l_ecellObject.modelID)
                            && l_key.Equals(l_ecellObject.key)
                            && l_type.Equals(l_ecellObject.type))
                        {
                            l_systemList[i].Children[j] = l_ecellObject.Copy();
                            this.m_pManager.DataChanged(l_modelID, l_key, l_type, l_ecellObject);
                        }
                        else
                        {
                            //
                            // Adds the new object.
                            //
                            this.DataAdd4Entity(l_ecellObject.Copy(), false);
                            this.m_pManager.DataChanged(l_modelID, l_key, l_type, l_ecellObject);
                            //
                            // Deletes the old object.
                            //
                            this.DataDelete4Node(l_modelID, l_key, l_type, false, l_isRecorded, l_isAnchor);
                        }
                        goto LOOP;
                    }
                }
            }
        LOOP:
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
            string l_message = "[" + l_ecellObject.modelID + "][" + l_ecellObject.key + "]";
            List<EcellObject> l_systemList = this.m_systemDic[this.m_currentProjectID][l_modelID];
            if (l_modelID.Equals(l_ecellObject.modelID)
                && l_key.Equals(l_ecellObject.key)
                && l_type.Equals(l_ecellObject.type))
            {
                //
                // Changes some properties.
                //
                for (int i = 0; i < l_systemList.Count; i++)
                {
                    if (l_systemList[i].modelID.Equals(l_modelID) && l_systemList[i].key.Equals(l_key))
                    {
                        this.CheckDifferences(l_systemList[i], l_ecellObject, null);
                        l_systemList[i] = l_ecellObject.Copy();
                        this.m_pManager.DataChanged(l_modelID, l_key, l_type, l_ecellObject);
                        /* Deleted by m.ishikawa
                        this.m_aManager.AddAction(new DataChangeAction(l_modelID, l_key, l_type, l_ecellObject));*/
                        return;
                    }
                }
            }
            else
            {
                //
                // Changes the key.
                //
                Dictionary<string, string> l_variableKeyDic = new Dictionary<string, string>();
                Dictionary<string, string> l_processKeyDic = new Dictionary<string, string>();
                List<string> l_createdSystemKeyList = new List<string>();
                List<string> l_deletedSystemKeyList = new List<string>();
                for (int i = 0; i < l_systemList.Count; i++)
                {
                    bool l_deletedFlag = false;
                    if (l_systemList[i].modelID.Equals(l_modelID)
                        && (l_systemList[i].key.Equals(l_key)
                            || l_systemList[i].key.StartsWith(l_key + Constants.delimiterPath)))
                    {
                        //
                        // Adds the new "System" object.
                        //
                        string l_newKey = l_ecellObject.key + l_systemList[i].key.Substring(l_key.Length);
                        EcellObject l_createdSystem
                            = EcellObject.CreateObject(
                                l_systemList[i].modelID, l_newKey, l_systemList[i].type, l_systemList[i].classname,
                                l_systemList[i].Value);
                        l_createdSystem.X = l_systemList[i].X;
                        l_createdSystem.Y = l_systemList[i].Y;
                        l_createdSystem.OffsetX = l_systemList[i].OffsetX;
                        l_createdSystem.OffsetY = l_systemList[i].OffsetY;
                        l_createdSystem.Width = l_systemList[i].Width;
                        l_createdSystem.Height = l_systemList[i].Height;
                        this.CheckEntityPath(l_createdSystem);
                        this.DataAdd4System(l_createdSystem, false);
                        this.CheckDifferences(l_systemList[i], l_createdSystem, null);
                        this.m_pManager.DataChanged(l_modelID, l_systemList[i].key, l_type, l_createdSystem);
                        /* deleted by m.ishikawa
                        this.m_aManager.AddAction(new DataChangeAction(
                            l_modelID, l_systemList[i].key, l_type, l_createdSystem)); */
                        l_createdSystemKeyList.Add(l_newKey);
                        //
                        // Deletes the old "System" object.
                        //
                        l_deletedFlag = true;
                        //
                        // 4 Children
                        //
                        if (l_systemList[i].Children != null && l_systemList[i].Children.Count > 0)
                        {
                            List<EcellObject> l_instanceList = new List<EcellObject>();
                            l_instanceList.AddRange(l_systemList[i].Children);
                            foreach (EcellObject l_childObject in l_instanceList)
                            {
                                EcellObject l_copy = l_childObject.Copy();
                                string l_childKey = l_copy.key;
                                string l_keyName = l_childKey.Split(Constants.delimiterColon.ToCharArray())[1];
                                if ((l_systemList[i].key.Equals(l_key)))
                                {
                                    l_copy.key = l_ecellObject.key + Constants.delimiterColon + l_keyName;
                                }
                                else
                                {
                                    l_copy.key = l_ecellObject.key + l_copy.key.Substring(l_key.Length);
                                }
                                this.CheckEntityPath(l_copy);
                                if (l_copy.type.Equals(Constants.xpathVariable))
                                {
                                    l_variableKeyDic[l_childKey] = l_copy.key;
                                    this.DataChanged4Entity(l_copy.modelID, l_childKey, l_copy.type, l_copy, l_isRecorded, l_isAnchor);
                                }
                                else
                                {
                                    l_processKeyDic[l_childKey] = l_copy.key;
                                }
                            }
                        }
                    }
                    //
                    // Deletes the old object.
                    //
                    if (l_deletedFlag)
                    {
                        l_deletedSystemKeyList.Add(l_systemList[i].key);
                    }
                }
                //
                // Checks all processes.
                //
                l_systemList = this.m_systemDic[this.m_currentProjectID][l_modelID];
                for (int i = 0; i < l_systemList.Count; i++)
                {
                    if (l_createdSystemKeyList.Contains(l_systemList[i].key))
                    {
                        continue;
                    }
                    if (l_systemList[i].Children == null || l_systemList[i].Children.Count <= 0)
                    {
                        continue;
                    }
                    List<EcellObject> l_instanceList = new List<EcellObject>();
                    l_instanceList.AddRange(l_systemList[i].Children);
                    foreach (EcellObject l_childObject in l_instanceList)
                    {
                        if (!l_childObject.type.Equals(Constants.xpathProcess))
                        {
                            continue;
                        }
                        bool l_changedFlag = false;
                        //
                        // 4 VariableReferenceList
                        //
                        EcellObject l_dest = null;
                        if (this.CheckVariableReferenceList(l_childObject, ref l_dest, l_variableKeyDic))
                        {
                            l_changedFlag = true;
                        }
                        //
                        // 4 key
                        //
                        string l_oldKey = l_dest.key;
                        string l_keyName = l_oldKey.Split(Constants.delimiterColon.ToCharArray())[1];
                        if (l_processKeyDic.ContainsKey(l_oldKey))
                        {
                            l_dest.key = l_processKeyDic[l_oldKey];
                            this.CheckEntityPath(l_dest);
                            l_changedFlag = true;
                        }
                        if (l_changedFlag)
                        {
                            this.DataChanged4Entity(l_dest.modelID, l_oldKey, l_dest.type, l_dest, l_isRecorded, l_isAnchor);
                        }
                    }
                }
                //
                // Deletes old "System"s.
                //
                foreach (string l_deletedKey in l_deletedSystemKeyList)
                {
                    this.DataDelete4System(l_modelID, l_deletedKey, false);
                }
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
            if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun ||
                this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationSuspend)
            {
                String mes = m_resources.GetString("ConfirmReset");
                DialogResult r = MessageBox.Show(mes,
                    "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (r != DialogResult.OK)
                {
                    throw new IgnoreException("Can't delete the object.");
                }
                SimulationStop();
                m_pManager.ChangeStatus(ProjectStatus.Loaded);

            }

            string l_message = null;
            EcellObject deleteObj = null;
            try
            {
                l_message = "[" + l_modelID + "][" + l_key + "]";
                if (l_modelID == null || l_modelID.Length <= 0) return;

                deleteObj = GetEcellObject(l_modelID, l_key, l_type);

                if (l_key == null || l_key.Length <= 0)
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
                l_message = m_resources.GetString("ErrDelete") + l_message + " " + l_type;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
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
            foreach (EcellObject obj in m_modelDic[m_currentProjectID])
            {
                if (obj.modelID == l_modelID)
                {
                    m_modelDic[m_currentProjectID].Remove(obj);
                    l_isDelete = true;
                    break;
                }
            }
            if (!l_isDelete)
            {
                throw new Exception(m_resources.GetString("ErrFindModel") + l_message);
            }
            //
            // Deletes "System"s.
            //
            if (m_systemDic[m_currentProjectID].ContainsKey(l_modelID))
            {
                m_systemDic[m_currentProjectID].Remove(l_modelID);
            }
            //
            // Deletes "Stepper"s.
            //
            foreach (string l_param in m_stepperDic[m_currentProjectID].Keys)
            {
                if (m_stepperDic[m_currentProjectID][l_param].ContainsKey(l_modelID))
                {
                    m_stepperDic[m_currentProjectID][l_param].Remove(l_modelID);
                }
            }
            this.m_pManager.Message(
                Constants.messageSimulation,
                "Delete Model: " + l_message + System.Environment.NewLine
                );
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
            string l_message = "[" + l_model + "][" + l_key + "]";
            int i = -1;
            string[] keys = l_key.Split(Constants.delimiterColon.ToCharArray());
            List<EcellObject> l_delList = new List<EcellObject>();
            if (m_systemDic[m_currentProjectID].ContainsKey(l_model))
            {
                foreach (EcellObject l_obj in m_systemDic[m_currentProjectID][l_model])
                {
                    i++;
                    if (l_obj.modelID != l_model || l_obj.key != keys[0]) continue;
                    if (l_obj.Children == null) continue;
                    foreach (EcellObject l_v in l_obj.Children)
                    {
                        if (l_v.key == l_key && l_v.type == l_type) l_delList.Add(l_v);
                    }
                    foreach (EcellObject l_v in l_delList)
                    {
                        m_systemDic[m_currentProjectID][l_model][i].Children.Remove(l_v);
                        if (l_v.Value != null && l_v.Value.Count > 0)
                        {
                            foreach (EcellData l_data in l_v.Value)
                            {
                                if (l_data.Settable)
                                {
                                    foreach (string l_keyParameterID
                                            in this.m_initialCondition[this.m_currentProjectID].Keys)
                                    {
                                        if (this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                                [l_v.modelID][l_v.type].ContainsKey(l_data.EntityPath))
                                        {
                                            this.m_initialCondition[this.m_currentProjectID][l_keyParameterID]
                                                    [l_v.modelID][l_v.type].Remove(l_data.EntityPath);
                                        }
                                    }
                                }
                            }
                        }
                        if (l_messageFlag)
                        {
                            this.m_pManager.Message(
                                Constants.messageSimulation,
                                "Delete " + l_type + ": " + l_message + System.Environment.NewLine);
                        }
                    }
                    if (l_messageFlag)
                    {
                        this.DataDelete4VariableReferenceList(l_delList, l_isRecorded, l_isAnchor);
                    }
                    l_delList.Clear();
                }
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
                if (!l_del.type.Equals(EcellObject.VARIABLE))
                    continue;

                string l_variableKey = l_del.key;
                foreach (EcellObject l_system in this.m_systemDic[this.m_currentProjectID][l_del.modelID])
                {
                    List<EcellObject> changeList = new List<EcellObject>();
                    foreach (EcellObject l_child in l_system.Children)
                    {
                        bool l_changedFlag = false;
                        if (!l_child.type.Equals(EcellObject.PROCESS))
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
                        this.DataChanged(l_change.modelID, l_change.key, l_change.type, l_change, l_isRecorded, l_isAnchor);
                    }
                }
            }
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
            List<EcellObject> l_list = this.m_systemDic[this.m_currentProjectID][modelID];
            foreach (EcellObject l_sys in l_list)
            {
                // Check systems.
                if (key.Equals(l_sys.key) && type.Equals(l_sys.type))
                    return true;
                // Continue if system has no node.
                if (l_sys.Children == null)
                    continue;
                // Check processes and variables
                foreach (EcellObject subEo in l_sys.Children)
                {
                    if (key.Equals(subEo.key) && type.Equals(subEo.type))
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
            string sizeKey = key + ":SIZE";
            Dictionary<String, String> variableList = new Dictionary<String, String>();
            List<EcellObject> targetSysList = new List<EcellObject>();
            List<EcellObject> targetObjList = new List<EcellObject>();

            List<EcellObject> saveSysList = new List<EcellObject>();
            List<EcellObject> saveObjList = new List<EcellObject>();

            EcellObject toBeDeleted = GetEcellObject(modelID, key, "System");
            toBeDeleted.Children = new List<EcellObject>();

            if (m_systemDic[m_currentProjectID].ContainsKey(modelID))
            {
                foreach (EcellObject obj in m_systemDic[m_currentProjectID][modelID])
                {
                    if (obj.modelID == modelID && obj.key.StartsWith(key) &&
                        (obj.key.Length == key.Length || obj.key[key.Length] == '/'))
                    {
                        if (obj.key.Length == key.Length)
                        {
                            if (obj.Children == null) continue;
                            foreach (EcellObject ins in obj.Children)
                            {
                                if (sizeKey.Equals(ins.key)) continue;
                                targetObjList.Add(ins.Copy());
                                saveObjList.Add(ins.Copy());
                            }
                        }
                        else if (obj.type == "System")
                        {
                            targetSysList.Add(obj.Copy());
                            saveSysList.Add(obj.Copy());
                        }
                    }
                }
            }

            DataDelete(modelID, key, "System", false, false);
            string[] el = key.Split(new char[] { '/' });
            int delPoint = el.Length - 1;
            List<EcellObject> list = new List<EcellObject>();
            foreach (EcellObject obj in targetSysList)
            {
                String orgKey = obj.key;
                String newKey = "";
                string[] nel = orgKey.Split(new char[] { '/' });
                for (int i = 0; i < nel.Length; i++)
                {
                    if (i == delPoint) continue;
                    if (nel[i] == "") newKey = "";
                    else newKey = newKey + "/" + nel[i];
                }
                List<EcellObject> tmpList = new List<EcellObject>();
                if (obj.Children != null)
                {
                    foreach (EcellObject ins in obj.Children)
                    {
                        saveObjList.Add(ins);
                        String iNewKey = "";
                        if (sizeKey.Equals(ins.key)) continue;
                        string[] iel = ins.key.Split(new char[] { '/' });
                        for (int j = 0; j < iel.Length; j++)
                        {
                            if (j == delPoint) continue;
                            if (iel[j] == "") iNewKey = "";
                            else iNewKey = iNewKey + "/" + iel[j];
                        }
                        if (ins.type == "Variable")
                            variableList.Add(ins.key, iNewKey);
                        ins.key = iNewKey;
                        tmpList.Add(ins);
                    }
                }
                obj.key = newKey;
                obj.Children = new List<EcellObject>();
                //                obj.Children = new List<EcellObject>();
                //                DataChanged(modelID, orgKey, obj.type, obj);
                CheckEntityPath(obj);
                list.Add(obj);
                foreach (EcellObject p in tmpList)
                {
                    list.Add(p);
                }
            }
            foreach (EcellObject obj in targetObjList)
            {
                String iNewKey = "";
                string[] iel = obj.key.Split(new char[] { '/' });
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
                if (obj.type == "Variable")
                    variableList.Add(obj.key, iNewKey);
                obj.key = iNewKey;
                list.Add(obj);
            }
            DataAdd(list, false, isAnchor);

            foreach (String oldKey in variableList.Keys)
            {
                List<EcellObject> changeProcList = new List<EcellObject>();
                String newKey = variableList[oldKey];
                CheckVariableReferenceList(modelID, newKey, oldKey, changeProcList);
                foreach (EcellObject cProcess in changeProcList)
                {
                    DataChanged4Entity(modelID, cProcess.key, cProcess.type, cProcess, isRecorded, isAnchor);
                }
            }

            // Add action
            if (isRecorded)
                m_aManager.AddAction(new SystemMergeAction(modelID, toBeDeleted, saveSysList, saveObjList, isAnchor));


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
                DataDelete(l_modelID, sys.key, "System", false, false);

            string[] el = l_obj.key.Split(new char[] { '/' });
            int delPoint = el.Length - 1;
            List<EcellObject> list = new List<EcellObject>();
            foreach (EcellObject obj in l_sysList)
            {
                String orgKey = obj.key;
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
                string[] iel = obj.key.Split(new char[] { '/' });
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
                DataDelete(l_modelID, iNewKey, obj.type, false, false);
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
            string l_message = "[" + l_model + "][" + l_key + "]";
            List<EcellObject> l_delList = new List<EcellObject>();
            if (m_systemDic[m_currentProjectID].ContainsKey(l_model))
            {
                foreach (EcellObject obj in m_systemDic[m_currentProjectID][l_model])
                {
                    if (obj.modelID == l_model && obj.key.StartsWith(l_key) &&
                        (obj.key.Length == l_key.Length || obj.key[l_key.Length] == '/')) l_delList.Add(obj);
                }
            }
            foreach (EcellObject l_obj in l_delList)
            {
                m_systemDic[m_currentProjectID][l_model].Remove(l_obj);
                if (l_obj.type == "System")
                {
                    foreach (string l_keyParamID in m_initialCondition[CurrentProjectID].Keys)
                    {
                        foreach (string l_delModel in m_initialCondition[CurrentProjectID][l_keyParamID].Keys)
                        {
                            foreach (string l_cType in m_initialCondition[CurrentProjectID][l_keyParamID][l_delModel].Keys)
                            {
                                String delKey = l_cType + ":" + l_key;
                                List<String> delKeyList = new List<string>();
                                foreach (String entKey in m_initialCondition[CurrentProjectID][l_keyParamID][l_delModel][l_cType].Keys)
                                {
                                    if (entKey.StartsWith(delKey))
                                        delKeyList.Add(entKey);
                                }
                                foreach (String entKey in delKeyList)
                                {
                                    m_initialCondition[CurrentProjectID][l_keyParamID][l_delModel][l_cType].Remove(entKey);
                                }
                            }
                        }
                    }
                }
                // Record deletion of child system. 
                if (!l_obj.key.Equals(l_key))
                    m_aManager.AddAction(new DataDeleteAction(l_obj.modelID, l_obj.key, l_obj.type, l_obj, false));
                if (l_messageFlag)
                {
                    this.m_pManager.Message(
                        Constants.messageSimulation,
                        "Delete System: " + l_message + System.Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Stores the "EcellObject"
        /// </summary>
        /// <param name="l_simulator">The "simulator"</param>
        /// <param name="l_ecellObject">The stored "EcellObject"</param>
        /// <param name="l_initialCondition">The initial condition.</param>
        private void DataStored(
                WrappedSimulator l_simulator,
                EcellObject l_ecellObject,
                Dictionary<string, Dictionary<string, double>> l_initialCondition)
        {
            if (l_ecellObject.type.Equals(Constants.xpathStepper))
            {
                DataStored4Stepper(l_simulator, l_ecellObject);
            }
            else if (l_ecellObject.type.Equals(Constants.xpathSystem))
            {
                Dictionary<string, double> l_childInitialCondition = null;
                if (l_initialCondition != null)
                {
                    l_childInitialCondition = l_initialCondition[Constants.xpathSystem];
                }
                DataStored4System(
                        l_simulator,
                        l_ecellObject,
                        l_childInitialCondition);
            }
            else if (l_ecellObject.type.Equals(Constants.xpathProcess))
            {
                Dictionary<string, double> l_childInitialCondition = null;
                if (l_initialCondition != null)
                {
                    l_childInitialCondition = l_initialCondition[Constants.xpathProcess];
                }
                DataStored4Process(
                        l_simulator,
                        l_ecellObject,
                        l_childInitialCondition);
            }
            else if (l_ecellObject.type.Equals(Constants.xpathVariable))
            {
                Dictionary<string, double> l_childInitialCondition = null;
                if (l_initialCondition != null)
                {
                    l_childInitialCondition = l_initialCondition[Constants.xpathVariable];
                }
                DataStored4Variable(
                        l_simulator,
                        l_ecellObject,
                        l_childInitialCondition);
            }
            //
            // 4 children
            //
            if (l_ecellObject.Children != null && l_ecellObject.Children.Count > 0)
            {
                for (int i = 0; i < l_ecellObject.Children.Count; i++)
                {
                    EcellObject l_childEcellObject = l_ecellObject.Children[i];
                    this.DataStored(l_simulator, l_childEcellObject, l_initialCondition);
                }
            }
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Process".
        /// </summary>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_ecellObject">The stored "Process"</param>
        /// <param name="l_initialCondition">The initial condition.</param>
        private static void DataStored4Process(
                WrappedSimulator l_simulator,
                EcellObject l_ecellObject,
                Dictionary<string, double> l_initialCondition)
        {
            bool isCreated = true;
            string l_key = Constants.xpathProcess + Constants.delimiterColon + l_ecellObject.key;
            WrappedPolymorph l_wrappedPolymorph = null;
            try
            {
                l_wrappedPolymorph = l_simulator.GetEntityPropertyList(l_key);
            }
            catch (Exception l_ex)
            {
                l_ex.ToString();
                isCreated = false;
            }
            if (isCreated != false && !l_wrappedPolymorph.IsList())
            {
                return;
            }
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> l_processEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> l_storedEcellDataDic
                    = new Dictionary<string, EcellData>();
            if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
            {
                foreach (EcellData l_storedEcellData in l_ecellObject.Value)
                {
                    l_storedEcellDataDic[l_storedEcellData.Name] = l_storedEcellData;
                    l_processEcellDataList.Add(l_storedEcellData);
                    if (l_initialCondition != null && l_storedEcellData.Settable)
                    {
                        if (l_storedEcellData.Value.IsDouble())
                        {
                            l_storedEcellData.Logable = true;
                            l_initialCondition[l_storedEcellData.EntityPath]
                                    = l_storedEcellData.Value.CastToDouble();
                        }
                        // else if (l_storedEcellData.Value.IsInt() && !l_storedEcellData.Name.StartsWith("Is"))
                        else if (l_storedEcellData.Value.IsInt())
                        {
                            l_initialCondition[l_storedEcellData.EntityPath]
                                = l_storedEcellData.Value.CastToInt();
                        }
                    }
                }
            }
            //
            // Stores the "EcellData"
            //
            if (isCreated)
            {
                List<WrappedPolymorph> l_processAllPropertyList = l_wrappedPolymorph.CastToList();
                for (int i = 0; i < l_processAllPropertyList.Count; i++)
                {
                    if (!(l_processAllPropertyList[i]).IsString())
                    {
                        continue;
                    }
                    string l_name = (l_processAllPropertyList[i]).CastToString();
                    List<bool> l_flag = l_simulator.GetEntityPropertyAttributes(
                            l_key + Constants.delimiterColon + l_name);
                    if (!l_flag[WrappedSimulator.s_flagGettable])
                    {
                        continue;
                    }
                    EcellValue l_value = null;
                    try
                    {
                        WrappedPolymorph l_property = l_simulator.GetEntityProperty(
                                l_key + Constants.delimiterColon + l_name);
                        l_value = new EcellValue(l_property);
                    }
                    catch (Exception)
                    {
                        if (l_storedEcellDataDic.ContainsKey(l_name))
                        {
                            if (l_storedEcellDataDic[l_name].Value.CastToList()[0].IsList())
                            {
                                l_value = l_storedEcellDataDic[l_name].Value;
                                if (l_name.Equals(Constants.xpathVRL))
                                {
                                    foreach (EcellValue l_vr in l_value.CastToList())
                                    {
                                        l_vr.CastToList()[2]
                                            = new EcellValue(Convert.ToInt32(l_vr.CastToList()[2].ToString()));
                                        l_vr.CastToList()[3]
                                            = new EcellValue(Convert.ToInt32(l_vr.CastToList()[3].ToString()));
                                    }
                                }
                            }
                            else
                            {
                                l_value = l_storedEcellDataDic[l_name].Value.CastToList()[0];
                            }
                        }
                        else if (l_name.Equals(Constants.xpathActivity))
                        {
                            l_value = new EcellValue(0.0);
                        }
                        else
                        {
                            l_value = new EcellValue("");
                        }
                    }
                    EcellData l_ecellData = new EcellData(
                            l_name, l_value, l_key + Constants.delimiterColon + l_name);
                    l_ecellData.Settable = l_flag[WrappedSimulator.s_flagSettable];
                    l_ecellData.Gettable = l_flag[WrappedSimulator.s_flagGettable];
                    l_ecellData.Loadable = l_flag[WrappedSimulator.s_flagLoadable];
                    l_ecellData.Saveable = l_flag[WrappedSimulator.s_flagSavable];
                    if (l_ecellData.Value != null)
                    {
                        if (l_ecellData.Value.IsDouble())
                        {
                            l_ecellData.Logable = true;
                            if (l_initialCondition != null && l_ecellData.Settable)
                            {
                                l_initialCondition[l_ecellData.EntityPath] = l_ecellData.Value.CastToDouble();
                            }
                        }
                        else if (l_ecellData.Value.IsInt())
                        {
                            if (l_initialCondition != null && l_ecellData.Settable)
                            {
                                l_initialCondition[l_ecellData.EntityPath] = l_ecellData.Value.CastToInt();
                            }
                        }
                    }
                    if (l_storedEcellDataDic.ContainsKey(l_name))
                    {
                        l_ecellData.Logged = l_storedEcellDataDic[l_name].Logged;
                        l_processEcellDataList.Remove(l_storedEcellDataDic[l_name]);
                    }
                    l_processEcellDataList.Add(l_ecellData);
                }
            }
            else
            {
                foreach (EcellData d in l_processEcellDataList)
                {
                    if (d.Name == Constants.xpathVRL)
                    {
                        string systemPath = "";
                        string name = Util.GetNameFromPath(l_ecellObject.key, ref systemPath);
                        EcellValue v = EcellReference.ConvertReferenceInEml(systemPath, d.Value);
                        d.Value = v;
                        v.ToString();
                        break;
                    }
                }
            }
            l_ecellObject.SetEcellDatas(l_processEcellDataList);
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Stepper".
        /// </summary>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_ecellObject">The stored "Stepper"</param>
        private static void DataStored4Stepper(
                WrappedSimulator l_simulator, EcellObject l_ecellObject)
        {
            List<EcellData> l_stepperEcellDataList = new List<EcellData>();
            WrappedPolymorph l_wrappedPolymorph = null;
            //
            // Property List
            //
            try
            {
                l_wrappedPolymorph = l_simulator.GetStepperPropertyList(l_ecellObject.key);
            }
            catch (Exception l_ex)
            {
                l_ex.ToString();
                return;
            }
            if (!l_wrappedPolymorph.IsList())
            {
                return;
            }
            //
            // Sets the class name.
            //
            if (l_ecellObject.classname == null || l_ecellObject.classname.Length <= 0)
            {
                l_ecellObject.classname = l_simulator.GetStepperClassName(l_ecellObject.key);
            }
            //
            // Checks the stored "EcellData"
            //
            Dictionary<string, EcellData> l_storedEcellDataDic = new Dictionary<string, EcellData>();
            if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
            {
                foreach (EcellData l_storedEcellData in l_ecellObject.Value)
                {
                    l_storedEcellDataDic[l_storedEcellData.Name] = l_storedEcellData;
                    l_stepperEcellDataList.Add(l_storedEcellData);
                }
            }
            else if (l_ecellObject.Value != null && l_ecellObject.Value.Count <= 0)
            {
                //
                // Sets the class name.
                //
                /* 20060315
                EcellData l_classNameData = new EcellData(
                    Constants.xpathClassName,
                    new EcellValue(l_simulator.GetStepperClassName(l_ecellObject.key)), Constants.xpathClassName
                    );
                l_classNameData.Settable = false;
                l_classNameData.Saveable = false;
                l_stepperEcellDataList.Add(l_classNameData);
                 */
            }
            //
            // Stores the "EcellData"
            //
            List<WrappedPolymorph> l_stepperAllPropertyList = l_wrappedPolymorph.CastToList();
            for (int i = 0; i < l_stepperAllPropertyList.Count; i++)
            {
                if (!(l_stepperAllPropertyList[i]).IsString())
                {
                    continue;
                }
                string l_name = (l_stepperAllPropertyList[i]).CastToString();
                List<bool> l_flag = l_simulator.GetStepperPropertyAttributes(l_ecellObject.key, l_name);
                if (!l_flag[WrappedSimulator.s_flagGettable])
                {
                    continue;
                }
                EcellValue l_value = null;
                try
                {
                    WrappedPolymorph l_property = l_simulator.GetStepperProperty(l_ecellObject.key, l_name);
                    l_value = new EcellValue(l_property);
                }
                catch (Exception)
                {
                    l_value = new EcellValue("");
                }
                EcellData l_ecellData = new EcellData(l_name, l_value, l_name);
                l_ecellData.Settable = l_flag[WrappedSimulator.s_flagSettable];
                l_ecellData.Gettable = l_flag[WrappedSimulator.s_flagGettable];
                l_ecellData.Loadable = l_flag[WrappedSimulator.s_flagLoadable];
                l_ecellData.Saveable = l_flag[WrappedSimulator.s_flagSavable];
                if (l_storedEcellDataDic.ContainsKey(l_name))
                {
                    if (l_value.IsString() && l_value.CastToString().Equals(""))
                    {
                        continue;
                    }
                    else
                    {
                        l_stepperEcellDataList.Remove(l_storedEcellDataDic[l_name]);
                    }
                }
                l_stepperEcellDataList.Add(l_ecellData);
            }
            l_ecellObject.SetEcellDatas(l_stepperEcellDataList);
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "System".
        /// </summary>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_ecellObject">The stored "System"</param>
        /// <param name="l_initialCondition">The initial condition.</param>
        private static void DataStored4System(
                WrappedSimulator l_simulator,
                EcellObject l_ecellObject,
                Dictionary<string, double> l_initialCondition)
        {
            //
            // Creates a not null properties.
            //
            /*
            List<EcellData> l_notNullPropertyList = new List<EcellData>();
            EcellData l_ecellDataSize = new EcellData(Constants.xpathSize, new EcellValue(0.0), "");
            l_ecellDataSize.Settable = false;
            l_ecellDataSize.Gettable = true;
            l_ecellDataSize.Loadable = false;
            l_ecellDataSize.Saveable = false;
            l_notNullPropertyList.Add(l_ecellDataSize);
            EcellData l_ecellDataSID = new EcellData(Constants.xpathStepper + Constants.xpathID, new EcellValue(""), "");
            l_ecellDataSID.Settable = true;
            l_ecellDataSID.Gettable = true;
            l_ecellDataSID.Loadable = true;
            l_ecellDataSID.Saveable = true;
            l_notNullPropertyList.Add(l_ecellDataSID);
             */
            //
            // Creates an entityPath.
            //
            string l_parentPath = l_ecellObject.key.Substring(0, l_ecellObject.key.LastIndexOf(
                    Constants.delimiterPath));
            string l_childPath = l_ecellObject.key.Substring(l_ecellObject.key.LastIndexOf(
                    Constants.delimiterPath) + 1);
            string l_key = null;
            if (l_parentPath.Length == 0)
            {
                if (l_childPath.Length == 0)
                {
                    l_key = Constants.xpathSystem + Constants.delimiterColon +
                        l_parentPath + Constants.delimiterColon +
                        Constants.delimiterPath;
                }
                else
                {
                    l_key = Constants.xpathSystem + Constants.delimiterColon +
                        Constants.delimiterPath + Constants.delimiterColon +
                        l_childPath;
                }
            }
            else
            {
                l_key = Constants.xpathSystem + Constants.delimiterColon +
                    l_parentPath + Constants.delimiterColon +
                    l_childPath;
            }
            //
            // Property List
            //
            WrappedPolymorph l_wrappedPolymorph = l_simulator.GetEntityPropertyList(l_key);
            if (!l_wrappedPolymorph.IsList())
            {
                return;
            }
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> l_systemEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> l_storedEcellDataDic
                    = new Dictionary<string, EcellData>();
            if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
            {
                foreach (EcellData l_storedEcellData in l_ecellObject.Value)
                {
                    l_storedEcellDataDic[l_storedEcellData.Name] = l_storedEcellData;
                    l_systemEcellDataList.Add(l_storedEcellData);
                    if (l_initialCondition != null && l_storedEcellData.Settable)
                    {
                        if (l_storedEcellData.Value.IsDouble())
                        {
                            l_storedEcellData.Logable = true;
                            l_initialCondition[l_storedEcellData.EntityPath]
                                = l_storedEcellData.Value.CastToDouble();
                        }
                        // else if (l_storedEcellData.Value.IsInt() && !l_storedEcellData.Name.StartsWith("Is"))
                        else if (l_storedEcellData.Value.IsInt())
                        {
                            l_initialCondition[l_storedEcellData.EntityPath]
                                = l_storedEcellData.Value.CastToInt();
                        }
                    }
                }
            }
            List<WrappedPolymorph> l_systemAllPropertyList = l_wrappedPolymorph.CastToList();
            for (int i = 0; i < l_systemAllPropertyList.Count; i++)
            {
                if (!(l_systemAllPropertyList[i]).IsString())
                {
                    continue;
                }
                string l_name = (l_systemAllPropertyList[i]).CastToString();
                List<bool> l_flag = l_simulator.GetEntityPropertyAttributes(
                        l_key + Constants.delimiterColon + l_name);
                if (!l_flag[WrappedSimulator.s_flagGettable])
                {
                    continue;
                }
                /*
                foreach (EcellData l_notNullEcellData in l_notNullPropertyList)
                {
                    if (l_notNullEcellData.Name.Equals(l_name))
                    {
                        l_notNullEcellData.Value = null;
                        break;
                    }
                }
                 */
                EcellValue l_value = null;
                try
                {
                    WrappedPolymorph l_property = l_simulator.GetEntityProperty(l_key + Constants.delimiterColon + l_name);
                    l_value = new EcellValue(l_property);
                }
                catch (Exception)
                {
                    if (l_storedEcellDataDic.ContainsKey(l_name))
                    {
                        if (l_storedEcellDataDic[l_name].Value.CastToList()[0].IsList())
                        {
                            l_value = l_storedEcellDataDic[l_name].Value;
                        }
                        else
                        {
                            l_value = l_storedEcellDataDic[l_name].Value.CastToList()[0];
                        }
                    }
                    else if (l_name.Equals(Constants.xpathSize))
                    {
                        l_value = new EcellValue(0.0);
                    }
                    else
                    {
                        l_value = new EcellValue("");
                    }
                }
                EcellData l_ecellData
                        = new EcellData(l_name, l_value, l_key + Constants.delimiterColon + l_name);
                l_ecellData.Settable = l_flag[WrappedSimulator.s_flagSettable];
                l_ecellData.Gettable = l_flag[WrappedSimulator.s_flagGettable];
                l_ecellData.Loadable = l_flag[WrappedSimulator.s_flagLoadable];
                l_ecellData.Saveable = l_flag[WrappedSimulator.s_flagSavable];
                if (l_ecellData.Value != null)
                {
                    if (l_ecellData.Value.IsDouble())
                    {
                        l_ecellData.Logable = true;
                        if (l_initialCondition != null && l_ecellData.Settable)
                        {
                            l_initialCondition[l_ecellData.EntityPath] = l_ecellData.Value.CastToDouble();
                        }
                    }
                    else if (l_ecellData.Value.IsInt())
                    {
                        if (l_initialCondition != null && l_ecellData.Settable)
                        {
                            l_initialCondition[l_ecellData.EntityPath] = l_ecellData.Value.CastToInt();
                        }
                    }
                }
                if (l_storedEcellDataDic.ContainsKey(l_name))
                {
                    l_ecellData.Logged = l_storedEcellDataDic[l_name].Logged;
                    l_systemEcellDataList.Remove(l_storedEcellDataDic[l_name]);
                }
                l_systemEcellDataList.Add(l_ecellData);
            }
            /*
            foreach (EcellData l_ecellData in l_notNullPropertyList)
            {
                if (l_ecellData.Value != null)
                {
                    EcellData l_newEcelldata = new EcellData(
                        l_ecellData.Name, l_ecellData.Value, l_key + Constants.delimiterColon + l_ecellData.Name);
                    l_newEcelldata.Settable = l_ecellData.Settable;
                    l_newEcelldata.Gettable = l_ecellData.Gettable;
                    l_newEcelldata.Loadable = l_ecellData.Loadable;
                    l_newEcelldata.Saveable = l_ecellData.Saveable;
                    l_systemEcellDataList.Add(l_ecellData);
                }
            }
             */
            l_ecellObject.SetEcellDatas(l_systemEcellDataList);
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Variable".
        /// </summary>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_ecellObject">The stored "Variable"</param>
        /// <param name="l_initialCondition">The initial condition.</param>
        private static void DataStored4Variable(
                WrappedSimulator l_simulator,
                EcellObject l_ecellObject,
                Dictionary<string, double> l_initialCondition)
        {
            string l_key = Constants.xpathVariable + Constants.delimiterColon + l_ecellObject.key;
            WrappedPolymorph l_wrappedPolymorph = l_simulator.GetEntityPropertyList(l_key);
            if (!l_wrappedPolymorph.IsList())
            {
                return;
            }
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> l_variableEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> l_storedEcellDataDic
                    = new Dictionary<string, EcellData>();
            if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
            {
                foreach (EcellData l_storedEcellData in l_ecellObject.Value)
                {
                    l_storedEcellDataDic[l_storedEcellData.Name] = l_storedEcellData;
                    l_variableEcellDataList.Add(l_storedEcellData);
                    if (l_initialCondition != null && l_storedEcellData.Settable)
                    {
                        if (l_storedEcellData.Value.IsDouble())
                        {
                            l_storedEcellData.Logable = true;
                            l_initialCondition[l_storedEcellData.EntityPath]
                                = l_storedEcellData.Value.CastToDouble();
                        }
                        // else if (l_storedEcellData.Value.IsInt() && !l_storedEcellData.Name.StartsWith("Is"))
                        else if (l_storedEcellData.Value.IsInt())
                        {
                            l_initialCondition[l_storedEcellData.EntityPath]
                                = l_storedEcellData.Value.CastToInt();
                        }
                    }
                }
            }
            List<WrappedPolymorph> l_variableAllPropertyList = l_wrappedPolymorph.CastToList();
            for (int i = 0; i < l_variableAllPropertyList.Count; i++)
            {
                if (!(l_variableAllPropertyList[i]).IsString())
                {
                    continue;
                }
                string l_name = (l_variableAllPropertyList[i]).CastToString();
                List<bool> l_flag = l_simulator.GetEntityPropertyAttributes(
                        l_key + Constants.delimiterColon + l_name);
                if (!l_flag[WrappedSimulator.s_flagGettable])
                {
                    continue;
                }
                EcellValue l_value = null;
                try
                {
                    WrappedPolymorph l_property = l_simulator.GetEntityProperty(
                            l_key + Constants.delimiterColon + l_name);
                    l_value = new EcellValue(l_property);
                }
                catch (Exception)
                {
                    if (l_storedEcellDataDic.ContainsKey(l_name))
                    {
                        if (l_storedEcellDataDic[l_name].Value.CastToList()[0].IsList())
                        {
                            l_value = l_storedEcellDataDic[l_name].Value;
                        }
                        else
                        {
                            l_value = l_storedEcellDataDic[l_name].Value.CastToList()[0];
                        }
                    }
                    else if (l_name.Equals(Constants.xpathMolarConc) || l_name.Equals(Constants.xpathNumberConc))
                    {
                        l_value = new EcellValue(0.0);
                    }
                    else
                    {
                        l_value = new EcellValue("");
                    }
                }
                EcellData l_ecellData = new EcellData(
                        l_name, l_value, l_key + Constants.delimiterColon + l_name);
                l_ecellData.Settable = l_flag[WrappedSimulator.s_flagSettable];
                l_ecellData.Gettable = l_flag[WrappedSimulator.s_flagGettable];
                l_ecellData.Loadable = l_flag[WrappedSimulator.s_flagLoadable];
                l_ecellData.Saveable = l_flag[WrappedSimulator.s_flagSavable];
                if (l_ecellData.Value != null)
                {
                    if (l_ecellData.Value.IsDouble())
                    {
                        l_ecellData.Logable = true;
                        if (l_initialCondition != null && l_ecellData.Settable)
                        {
                            l_initialCondition[l_ecellData.EntityPath] = l_ecellData.Value.CastToDouble();
                        }
                    }
                    else if (l_ecellData.Value.IsInt())
                    {
                        if (l_initialCondition != null && l_ecellData.Settable)
                        {
                            l_initialCondition[l_ecellData.EntityPath] = l_ecellData.Value.CastToInt();
                        }
                    }
                }
                if (l_storedEcellDataDic.ContainsKey(l_name))
                {
                    l_ecellData.Logged = l_storedEcellDataDic[l_name].Logged;
                    l_variableEcellDataList.Remove(l_storedEcellDataDic[l_name]);
                }
                l_variableEcellDataList.Add(l_ecellData);
            }
            l_ecellObject.SetEcellDatas(l_variableEcellDataList);
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
                    throw new Exception(m_resources.GetString("ErrNullData") + "[ParameterID]");
                }
                this.SetDefaultDir();
                if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrBaseDir"));
                }
                if (this.m_stepperDic[this.m_currentProjectID].ContainsKey(l_parameterID))
                {
                    this.m_stepperDic[this.m_currentProjectID].Remove(l_parameterID);
                    string l_simulationDirName
                            = this.m_defaultDir + Constants.delimiterPath
                            + this.m_currentProjectID + Constants.delimiterPath + Constants.xpathParameters;
                    string l_pattern
                            = "_????_??_??_??_??_??_" + l_parameterID + Constants.delimiterPeriod + Constants.xpathXml;
                    if (Directory.Exists(l_simulationDirName))
                    {
                        foreach (string l_fileName in Directory.GetFiles(l_simulationDirName, l_pattern))
                        {
                            File.Delete(l_fileName);
                        }
                        string l_simulationFileName
                                = l_simulationDirName + Constants.delimiterPath + l_parameterID + Constants.delimiterPeriod
                                + Constants.xpathXml;
                        File.Delete(l_simulationFileName);
                    }
                    this.m_loggerPolicyDic[m_currentProjectID].Remove(l_parameterID);
                    m_pManager.ParameterDelete(m_currentProjectID, l_parameterID);
                    this.m_pManager.Message(
                        Constants.messageSimulation,
                        "Delete Simulation Parameter: " + l_message + System.Environment.NewLine
                        );
                }
                else
                {
                    throw new Exception(m_resources.GetString("ErrFindSimParam") + l_message);
                }

                if (l_isRecorded)
                    m_aManager.AddAction(new DeleteSimParamAction(l_parameterID, l_isAnchor));
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrDeleteSimParam") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
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
                l_message = "[" + l_parameterID + "][" + l_stepper.modelID + "][" + l_stepper.key + "]";
                int l_point = -1;
                List<EcellObject> l_storedStepperList
                    = this.m_stepperDic[this.m_currentProjectID][l_parameterID][l_stepper.modelID];
                for (int i = 0; i < l_storedStepperList.Count; i++)
                {
                    if (l_storedStepperList[i].key.Equals(l_stepper.key))
                    {
                        l_point = i;
                        break;
                    }
                }
                if (l_point != -1)
                {
                    l_storedStepperList.RemoveAt(l_point);
                    this.m_pManager.Message(
                        Constants.messageSimulation,
                        "Delete Stepper: " + l_message + System.Environment.NewLine
                        );
                }
                if (l_isRecorded)
                    m_aManager.AddAction(new DeleteStepperAction(l_parameterID, l_stepper));
                if (m_currentParameterID.Equals(l_parameterID))
                {
                    m_pManager.DataDelete(l_stepper.modelID, l_stepper.key, l_stepper.type);
                }
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrDelete") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
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
                    throw new Exception(m_resources.GetString("ErrIDUnform") + l_message);
                }
                else if (!l_infos[0].Equals(Constants.xpathSystem)
                        && !l_infos[0].Equals(Constants.xpathProcess)
                        && !l_infos[0].Equals(Constants.xpathVariable))
                {
                    throw new Exception(m_resources.GetString("ErrIDUnform") + l_message);
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
                if (this.m_systemDic[this.m_currentProjectID][l_modelID] == null
                        || this.m_systemDic[this.m_currentProjectID][l_modelID].Count <= 0)
                {
                    return false;
                }
                foreach (EcellObject l_system in this.m_systemDic[this.m_currentProjectID][l_modelID])
                {
                    if (l_infos[0].Equals(Constants.xpathSystem))
                    {
                        if (l_system.type.Equals(l_infos[0]) && l_system.key.Equals(l_key))
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
                            if (l_entity.type.Equals(l_infos[0]) && l_entity.key.Equals(l_key))
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
                throw new Exception(m_resources.GetString("ErrCheckID") + l_message + " {" + l_ex.ToString() + "}");
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
                foreach (string l_modelID in l_modelIDList)
                {
                    l_storedStepperList.AddRange(
                        this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID][l_modelID]
                        );
                    l_storedSystemList.AddRange(
                        this.m_systemDic[this.m_currentProjectID][l_modelID]
                        );
                }
                if (l_storedStepperList == null || l_storedStepperList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrFindStepper"));
                }
                else if (l_storedSystemList == null || l_storedSystemList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrFindSystem"));
                }
                //
                // Exports.
                //
                l_storedStepperList.AddRange(l_storedSystemList);
                EmlWriter.Create(l_fileName, l_storedStepperList);
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Export Model: " + l_message + System.Environment.NewLine
                    );
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrExportFile") + l_fileName + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the current logger policy.
        /// </summary>
        /// <returns>The current logger policy</returns>
        private WrappedPolymorph GetCurrentLoggerPolicy()
        {
            List<WrappedPolymorph> l_policyList = new List<WrappedPolymorph>();
            l_policyList.Add(new WrappedPolymorph(
                this.m_loggerPolicyDic[this.m_currentProjectID][this.m_currentParameterID].m_reloadStepCount)
                );
            l_policyList.Add(new WrappedPolymorph(
                this.m_loggerPolicyDic[this.m_currentProjectID][this.m_currentParameterID].m_reloadInterval)
                );
            l_policyList.Add(new WrappedPolymorph(
                this.m_loggerPolicyDic[this.m_currentProjectID][this.m_currentParameterID].m_diskFullAction)
                );
            l_policyList.Add(new WrappedPolymorph(
                this.m_loggerPolicyDic[this.m_currentProjectID][this.m_currentParameterID].m_maxDiskSpace)
                );
            return new WrappedPolymorph(l_policyList);
        }

        /// <summary>
        /// Returns the current simulation parameter ID.
        /// </summary>
        /// <returns>The current simulation parameter ID</returns>
        public string GetCurrentSimulationParameterID()
        {
            return this.m_currentParameterID;
        }

        /// <summary>
        /// Returns the current simulation time.
        /// </summary>
        /// <returns>The current simulation time</returns>
        public double GetCurrentSimulationTime()
        {
            if (this.m_simulatorDic[this.m_currentProjectID] != null)
            {
                return this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
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
            List<EcellObject> l_ecellObjectList = new List<EcellObject>();
            try
            {
                //
                // Returns all stored "EcellObject".
                //
                if (l_modelID == null || l_modelID.Length <= 0)
                {
                    //
                    // Searches the model.
                    //
                    foreach (EcellObject l_model in this.m_modelDic[this.m_currentProjectID])
                    {
                        l_ecellObjectList.Add(l_model.Copy());
                    }
                    //
                    // Searches the "System".
                    //
                    foreach (string l_storedModelID in this.m_systemDic[this.m_currentProjectID].Keys)
                    {
                        foreach (EcellObject l_system in this.m_systemDic[this.m_currentProjectID][l_storedModelID])
                        {
                            l_ecellObjectList.Add(l_system.Copy());
                        }
                    }
                }
                else
                {
                    //
                    // Searches the model.
                    //
                    if (l_key == null || l_key.Length <= 0)
                    {
                        foreach (EcellObject l_model in this.m_modelDic[this.m_currentProjectID])
                        {
                            if (l_model.modelID.Equals(l_modelID))
                            {
                                l_ecellObjectList.Add(l_model.Copy());
                                break;
                            }
                        }
                    }
                    //
                    // Searches the "System".
                    //
                    foreach (EcellObject l_system in this.m_systemDic[this.m_currentProjectID][l_modelID])
                    {
                        if (l_key == null
                            || l_key.Length <= 0
                            || (l_system.key != null && l_system.key.Equals(l_key)))
                        {
                            l_ecellObjectList.Add(l_system.Copy());
                        }
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
            if (string.IsNullOrEmpty(key))
                return null;
            List<EcellObject> list = null;
            if (key.Contains(":"))
            {
                String[] data = key.Split(new char[] { ':' });
                list = GetData(modelId, data[0]);
            }
            else
                list = GetData(modelId, key);

            if (list == null)
            {
                return null;
            }
            foreach (EcellObject eo in list)
            {
                if (key.Equals(eo.key) && type.Equals(eo.type))
                {
                    return eo;
                }
                if (eo.Children == null) continue;
                foreach (EcellObject subEo in eo.Children)
                {
                    if (key.Equals(subEo.key) && type.Equals(subEo.type))
                    {
                        return subEo;
                    }
                }
            }
            return null;
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
            WrappedSimulator l_simulator = m_simulatorDic[m_currentProjectID];
            BuildDefaultSimulator(l_simulator, null, null);
            l_systemEcellObject
                    = EcellObject.CreateObject(
                        l_modelID,
                        Constants.delimiterPath,
                        Constants.xpathSystem,
                        Constants.xpathSystem,
                        null);
            DataStored4System(
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
            DataStored4Stepper(l_simulator, l_stepperEcellObject);
            l_dic[Constants.xpathSystem] = l_systemEcellObject;
            l_dic[Constants.xpathStepper] = l_stepperEcellObject;
            return l_dic;
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
                //
                // Initialize
                //
                if (this.m_simulatorDic[this.m_currentProjectID] == null
                    || this.m_logableEntityPathDic == null)
                {
                    //
                    // still in the preparatory stage ...
                    //
                    return null;
                }
                if (this.m_logableEntityPathDic[this.m_currentProjectID] == null ||
                    this.m_logableEntityPathDic[this.m_currentProjectID].Count == 0)
                {
                    return null;
                }
                //
                // GetLogData
                //
                return this.GetUniqueLogData(l_startTime, l_endTime, l_interval, l_fullID);
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrGetData") + " {" + l_ex.ToString() + "}");
            }
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
                return this.m_initialCondition[this.m_currentProjectID][l_paremterID][l_modelID];
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                        m_resources.GetString("ErrInitParam") + l_message + " {"
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
                return this.m_initialCondition[this.m_currentProjectID][l_paremterID][l_modelID]
                        [l_type];
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                        m_resources.GetString("ErrInitParam") + l_message + " {"
                        + l_ex.ToString() + "}");
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
                //
                // Initialize
                //
                if (this.m_simulatorDic[this.m_currentProjectID] == null
                    || this.m_logableEntityPathDic == null)
                {
                    //
                    // still in the preparatory stage ...
                    //
                    return null;
                }
                if (this.m_logableEntityPathDic[this.m_currentProjectID] == null ||
                    this.m_logableEntityPathDic[this.m_currentProjectID].Count == 0)
                {
                    return null;
                }
                WrappedPolymorph l_loggerList = this.m_simulatorDic[this.m_currentProjectID].GetLoggerList();
                if (l_loggerList.IsList())
                {
                    foreach (WrappedPolymorph l_logger in l_loggerList.CastToList())
                    {
                        l_logDataList.Add(
                                this.GetUniqueLogData(l_startTime, l_endTime, l_interval, l_logger.CastToString()));
                    }
                }
                else
                {
                    ; // do nothing
                }
                return l_logDataList;
            }
            catch (Exception l_ex)
            {
                l_logDataList = null;
                throw new Exception(m_resources.GetString("ErrGetData") + " {" + l_ex.ToString() + "}");
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
                l_endTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
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
                lock (this.m_simulatorDic[this.m_currentProjectID])
                {
                    l_dataPointVector
                        = this.m_simulatorDic[this.m_currentProjectID].GetLoggerData(
                            l_fullID,
                            l_startTime,
                            l_endTime
                            );
                }
            }
            else
            {
                lock (this.m_simulatorDic[this.m_currentProjectID])
                {
                    l_dataPointVector
                        = this.m_simulatorDic[this.m_currentProjectID].GetLoggerData(
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
            if (this.m_logableEntityPathDic[this.m_currentProjectID].ContainsKey(l_fullID))
            {
                l_modelID = this.m_logableEntityPathDic[this.m_currentProjectID][l_fullID];
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
                WrappedPolymorph l_polymorphList = this.m_simulatorDic[this.m_currentProjectID].GetLoggerList();
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
            return this.m_loggerPolicyDic[this.m_currentProjectID][l_parameterID];
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
                        = this.m_simulatorDic[this.m_currentProjectID].GetNextEvent().CastToList();
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
        public string GetDirPath(string modelName)
        {
            if (m_loadDirList.ContainsKey(modelName))
            {
                return m_loadDirList[modelName];
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
                foreach (EcellObject l_system in this.m_systemDic[this.m_currentProjectID][l_modelID])
                {
                    if (l_entityName.Equals(Constants.xpathSystem))
                    {
                        string l_parentPath = l_system.key.Substring(0, l_system.key.LastIndexOf(Constants.delimiterPath));
                        string l_childPath = l_system.key.Substring(l_system.key.LastIndexOf(Constants.delimiterPath) + 1);
                        if (l_system.key.Equals(Constants.delimiterPath))
                        {
                            if (l_childPath.Length == 0)
                            {
                                l_childPath = Constants.delimiterPath;
                            }
                        }
                        else
                        {
                            if (l_parentPath.Length == 0)
                            {
                                l_parentPath = Constants.delimiterPath;
                            }
                        }
                        l_entityList.Add(
                            Constants.xpathSystem + Constants.delimiterColon
                            + l_parentPath + Constants.delimiterColon + l_childPath);
                    }
                    else
                    {
                        if (l_system.Children == null || l_system.Children.Count <= 0)
                        {
                            continue;
                        }
                        foreach (EcellObject l_entity in l_system.Children)
                        {
                            if (l_entity.type.Equals(l_entityName))
                            {
                                l_entityList.Add(l_entity.type + Constants.delimiterColon + l_entity.key);
                            }
                        }
                    }
                }
                return l_entityList;
            }
            catch (Exception l_ex)
            {
                l_entityList.Clear();
                l_entityList = null;
                throw new Exception(m_resources.GetString("ErrFindEnt") +
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
                if (this.m_simulatorDic[this.m_currentProjectID] == null)
                {
                    return null;
                }
                EcellValue l_value
                    = new EcellValue(this.m_simulatorDic[this.m_currentProjectID].GetEntityProperty(l_fullPN));
                return l_value;
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrGetProp") +
                    l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// get model list loaded by this system now.
        /// </summary>
        /// <returns></returns>
        public List<string> GetModelList()
        {
            List<EcellObject> objList = m_modelDic[m_currentProjectID];
            List<string> list = new List<string>();

            if (objList != null)
            {
                foreach (EcellObject obj in objList)
                {
                    list.Add(obj.modelID);
                }
            }

            return list;
        }

        /// <summary>
        /// Returns the list of the "Process" DM.
        /// </summary>
        public List<string> GetProcessList()
        {
            SetDMList();
            Util.GetDMDirs(null);
            return this.m_dmDic[Constants.xpathProcess];
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
                //    s_resources.GetString("ErrGetProp") +
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
                DataStored4Process(
                        sim,
                        dummyEcellObject,
                        new Dictionary<string, double>());
                SetPropertyList(dummyEcellObject, l_dic);
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                    s_resources.GetString("ErrGetProp") +
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
                        l_reader = new StreamReader(l_prjFile);
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
                if (this.m_modelDic[this.m_currentProjectID] != null
                    && this.m_modelDic[this.m_currentProjectID].Count > 0)
                {
                    List<string> l_modelIDList = new List<string>();
                    foreach (EcellObject l_model in this.m_modelDic[this.m_currentProjectID])
                    {
                        l_modelIDList.Add(l_model.modelID);
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
                throw new Exception(m_resources.GetString("ErrFindModel") +
                    " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Returns the savable project ID.
        /// </summary>
        /// <returns>The savable project ID</returns>
        public string GetSavableProject()
        {
            return this.m_currentProjectID;
        }

        /// <summary>
        /// Returns the savable simulation parameter ID.
        /// </summary>
        /// <returns>The savable simulation parameter ID</returns>
        public List<string> GetSavableSimulationParameter()
        {
            try
            {
                if (this.m_loggerPolicyDic[this.m_currentProjectID] != null
                    && this.m_loggerPolicyDic[this.m_currentProjectID].Count > 0)
                {
                    List<string> l_prmIDList = new List<string>();
                    foreach (string l_prmID in this.m_loggerPolicyDic[this.m_currentProjectID].Keys)
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
                throw new Exception(m_resources.GetString("ErrSimParam") +
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
                if (l_modelID == null || l_modelID.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrNullData"));
                }
                List<string> l_parameterList = new List<string>();
                if (l_parameterID == null || l_parameterID.Length <= 0)
                {
                    if (this.m_currentParameterID == null || this.m_currentParameterID.Length <= 0)
                    {
                        throw new Exception(m_resources.GetString("ErrCurParamID"));
                    }
                    l_parameterList.Add(this.m_currentParameterID);
                }
                else
                {
                    l_parameterList.Add(l_parameterID);
                }
                foreach (string l_intendedParameterID in l_parameterList)
                {
                    for (int i = 0;
                        i < this.m_stepperDic[this.m_currentProjectID][l_intendedParameterID][l_modelID].Count; i++)
                    {
                        EcellObject l_stepper =
                            this.m_stepperDic[this.m_currentProjectID][l_intendedParameterID][l_modelID][i];
                        // DataStored4Stepper(l_simulator, l_stepper);
                        l_returnedStepper.Add(l_stepper.Copy());
                    }
                }
                return l_returnedStepper;
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                    m_resources.GetString("ErrGetStep") +
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
                return this.m_stepperDic.ContainsKey(this.m_currentProjectID) ?
                    new List<string>(this.m_stepperDic[this.m_currentProjectID].Keys) :
                    new List<string>();
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrGetSimParams") + " {" + l_ex.ToString() + "}");
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
            l_stepperList.AddRange(this.m_dmDic[Constants.xpathStepper]);
            l_stepperList.Sort();
            return l_stepperList;
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
                DataStored4Stepper(sim, dummyEcellObject);
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
            return this.m_dmDic[Constants.xpathSystem];
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
                foreach (EcellObject l_system in this.m_systemDic[this.m_currentProjectID][l_modelID])
                {
                    l_systemList.Add(l_system.key);
                }
                return l_systemList;
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrGetSysList") + " [" + l_modelID + "] {"
                        + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Get the list of system under the input key on model.
        /// </summary>
        /// <param name="modelID">modelID.</param>
        /// <param name="key">system key.</param>
        /// <returns>the list of system.</returns>
        public List<EcellObject> GetSystemList(string modelID, string key)
        {
            List<EcellObject> list = new List<EcellObject>();

            foreach (EcellObject l_system in this.m_systemDic[this.m_currentProjectID][modelID])
            {
                if (l_system.key.StartsWith(key))
                {
                    list.Add(l_system);
                }
            }

            return list;
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
            DataStored4System(
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
            return this.m_dmDic[Constants.xpathVariable];
        }

        /// <summary>
        /// Get the temporary id in projects.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="type">object type.</param>
        /// <param name="systemID">ID of parent system.</param>
        /// <returns>the temporary id.</returns>
        public String GetTemporaryID(string modelID, string type, string systemID)
        {
            // Set Preface
            String pref = "";
            int i = 0;
            if (type.Equals(EcellObject.PROCESS))
            {
                pref = systemID + ":P";
                i = m_processNumbering;
            }
            else if (type.Equals(EcellObject.VARIABLE))
            {
                pref = systemID + ":V";
                i = m_variableNumbering;
            }
            else
            {
                if (systemID == null || systemID == "/")
                    systemID = "";
                pref = systemID + "/S";
                i = m_systemNumbering;
            }

            // Set tmpID
            string tmpID = pref + i;
            List<EcellObject> list = GetData(modelID, null);
            while (IsDataExists(modelID, tmpID, type))
            {
                i++;
                tmpID = pref + i;
            }

            // Set TmpNumber
            if (type.Equals(EcellObject.PROCESS))
            {
                m_processNumbering = i + 1;
            }
            else if (type.Equals(EcellObject.VARIABLE))
            {
                m_variableNumbering = i + 1;
            }
            else
            {
                m_systemNumbering = i + 1;
            }

            return tmpID;
        }

        /// <summary>
        /// Reset the temporary id in projects.
        /// </summary>
        public void ResetTemporaryID()
        {
            m_processNumbering = 0;
            m_variableNumbering = 0;
            m_systemNumbering = 0;
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
                DataStored4Variable(
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
            else if (l_ecellObject.modelID == null || l_ecellObject.modelID.Length < 0)
            {
                return l_flag;
            }
            else if (l_ecellObject.type == null || l_ecellObject.type.Length < 0)
            {
                return l_flag;
            }
            //
            // 4 "Process", "Stepper", "System" and "Variable"
            //
            if (!l_ecellObject.type.Equals(Constants.xpathProject) && !l_ecellObject.type.Equals(Constants.xpathModel))
            {
                if (l_ecellObject.key == null || l_ecellObject.key.Length < 0)
                {
                    return l_flag;
                }
                else if (l_ecellObject.classname == null || l_ecellObject.classname.Length < 0)
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
            try
            {
                this.m_simulatorDic[this.m_currentProjectID] = CreateSimulatorInstance();
                //
                // Loads steppers on the simulator.
                //
                List<EcellObject> l_stepperList = new List<EcellObject>();
                List<string> l_modelIDList = new List<string>();
                Dictionary<string, Dictionary<string, WrappedPolymorph>> l_setStepperPropertyDic
                    = new Dictionary<string, Dictionary<string, WrappedPolymorph>>();
                foreach (string l_modelID in this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID].Keys)
                {
                    l_stepperList.AddRange(
                        this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID][l_modelID]);
                    l_modelIDList.Add(l_modelID);
                }
                LoadStepper(
                    this.m_simulatorDic[this.m_currentProjectID],
                    l_stepperList,
                    l_setStepperPropertyDic);

                //
                // Loads systems on the simulator.
                //
                List<string> l_allLoggerList = new List<string>();
                List<EcellObject> l_systemList = new List<EcellObject>();
                this.m_logableEntityPathDic = new Dictionary<string, Dictionary<string, string>>();
                this.m_logableEntityPathDic[this.m_currentProjectID] = new Dictionary<string, string>();
                Dictionary<string, WrappedPolymorph> l_setSystemPropertyDic
                    = new Dictionary<string, WrappedPolymorph>();
                foreach (string l_modelID in l_modelIDList)
                {
                    List<string> l_loggerList = new List<string>();
                    if (l_flag)
                    {
                        LoadSystem(
                            this.m_simulatorDic[this.m_currentProjectID],
                            this.m_systemDic[this.m_currentProjectID][l_modelID],
                            l_loggerList,
                            this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID][l_modelID],
                            l_setSystemPropertyDic);
                    }
                    else
                    {
                        LoadSystem(
                            this.m_simulatorDic[this.m_currentProjectID],
                            this.m_systemDic[this.m_currentProjectID][l_modelID],
                            l_loggerList,
                            null,
                            l_setSystemPropertyDic);
                    }
                    foreach (string l_logger in l_loggerList)
                    {
                        this.m_logableEntityPathDic[this.m_currentProjectID][l_logger] = l_modelID;
                    }
                    l_allLoggerList.AddRange(l_loggerList);
                }

                //
                // Initializes
                //
                this.m_simulatorDic[this.m_currentProjectID].Initialize();
                //
                // Sets the "Settable" and "Not Savable" properties
                //
                foreach (string l_key in l_setStepperPropertyDic.Keys)
                {
                    foreach (string l_path in l_setStepperPropertyDic[l_key].Keys)
                    {
                        this.m_simulatorDic[this.m_currentProjectID].SetStepperProperty(
                            l_key, l_path, l_setStepperPropertyDic[l_key][l_path]);
                    }
                }
                foreach (string l_path in l_setSystemPropertyDic.Keys)
                {
                    try
                    {
                        EcellValue l_storedEcellValue = new EcellValue(
                            this.m_simulatorDic[this.m_currentProjectID].GetEntityProperty(l_path));
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
                    this.m_simulatorDic[this.m_currentProjectID].SetEntityProperty(
                        l_path, l_setSystemPropertyDic[l_path]);
                }
                //
                // Set the initial condition property.
                //
                foreach (string l_modelID in l_modelIDList)
                {
                    foreach (string l_type
                        in this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID][l_modelID].Keys)
                    {
                        foreach (string l_fullPN
                            in this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID][l_modelID]
                                [l_type].Keys)
                        {
                            EcellValue l_storedValue
                                = new EcellValue(
                                    this.m_simulatorDic[this.m_currentProjectID].GetEntityProperty(l_fullPN));
                            double l_initialValue
                                = this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                                        [l_modelID][l_type][l_fullPN];
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
                            this.m_simulatorDic[this.m_currentProjectID].SetEntityProperty(
                                l_fullPN, l_newValue);
                        }
                    }
                }
                //
                // Reinitializes
                //
                // this.m_simulatorDic[this.m_currentProjectID].Initialize();
                //
                // Creates the "Logger" only after the initialization.
                //
                if (l_allLoggerList != null && l_allLoggerList.Count > 0)
                {
                    WrappedPolymorph l_loggerPolicy = this.GetCurrentLoggerPolicy();
                    foreach (string l_logger in l_allLoggerList)
                    {
                        this.m_simulatorDic[this.m_currentProjectID].CreateLogger(l_logger, l_loggerPolicy);
                    }
                }
                //
                // Messages
                //
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Initialize the simulator:" + System.Environment.NewLine);
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrInitSim"), l_ex);
            }
        }

        /// <summary>
        /// Checks whether the simulator is running.
        /// </summary>
        /// <returns>true if the simulator is running; false otherwise</returns>
        public bool IsActive()
        {
            bool l_runningFlag = false;
            if (this.m_simulatorExeFlagDic != null
                && this.m_currentProjectID != null && this.m_simulatorExeFlagDic.ContainsKey(this.m_currentProjectID))
            {
                if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun)
                {
                    l_runningFlag = true;
                }
            }
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

        /// <summary>
        /// Loads the "Process" and the "Variable" to the "EcellCoreLib".
        /// </summary>
        /// <param name="l_entityList">The list of the "Process" and the "Variable"</param>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_loggerList">The list of the "Logger"</param>
        /// <param name="l_processPropertyDic">The dictionary of the process property</param>
        /// <param name="l_initialCondition">The dictionary of the initial condition</param>
        /// <param name="l_setPropertyDic"></param>
        private void LoadEntity(
            WrappedSimulator l_simulator,
            List<EcellObject> l_entityList,
            List<string> l_loggerList,
            Dictionary<string, WrappedPolymorph> l_processPropertyDic,
            Dictionary<string, Dictionary<string, double>> l_initialCondition,
            Dictionary<string, WrappedPolymorph> l_setPropertyDic)
        {
            if (l_entityList == null || l_entityList.Count <= 0)
            {
                return;
            }
            try
            {
                foreach (EcellObject l_entity in l_entityList)
                {
                    l_simulator.CreateEntity(
                        l_entity.classname,
                        l_entity.type + Constants.delimiterColon + l_entity.key);
                    if (l_entity.Value == null || l_entity.Value.Count <= 0)
                    {
                        continue;
                    }
                    foreach (EcellData l_ecellData in l_entity.Value)
                    {
                        if (l_ecellData.Name == null
                                || l_ecellData.Name.Length <= 0
                                || l_ecellData.Value == null
                                || (l_ecellData.Value.IsString() &&
                                    l_ecellData.Value.CastToString().Length == 0))
                        {
                            continue;
                        }

                        if (l_ecellData.Logged)
                        {
                            l_loggerList.Add(l_ecellData.EntityPath);
                        }

                        if (l_ecellData.Saveable)
                        {
                            if (l_ecellData.EntityPath.EndsWith(Constants.xpathVRL))
                            {
                                l_processPropertyDic[l_ecellData.EntityPath]
                                    = EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellData.Value);
                            }
                            else
                            {
                                if (l_ecellData.EntityPath.EndsWith("FluxDistributionList")) continue;
                                if (l_ecellData.Value.IsDouble()
                                    &&
                                    (Double.IsInfinity(l_ecellData.Value.CastToDouble())
                                        || Double.IsNaN(l_ecellData.Value.CastToDouble())))
                                {
                                    continue;
                                }
                                l_simulator.LoadEntityProperty(
                                    l_ecellData.EntityPath,
                                    EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellData.Value));
                            }
                        }
                        else if (l_ecellData.Settable)
                        {
                            if (l_ecellData.Value.IsDouble() &&
                                    (Double.IsInfinity(l_ecellData.Value.CastToDouble())
                                    || Double.IsNaN(l_ecellData.Value.CastToDouble())))
                            {
                                continue;
                            }
                            l_setPropertyDic[l_ecellData.EntityPath]
                                = EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellData.Value);
                        }
                    }
                }
            }
            catch (WrappedException e)
            {
                throw new Exception("Failed to create entity", e);
            }
        }

        private void InitializeModel(EcellObject l_ecellObject)
        {
            DataStored(
                this.m_simulatorDic[this.m_currentProjectID],
                l_ecellObject,
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID][l_ecellObject.modelID]);
            //
            // Sets the "EcellObject".
            //
            if (l_ecellObject.type.Equals(Constants.xpathModel))
            {
                this.m_modelDic[this.m_currentProjectID].Add(l_ecellObject);
            }
            else if (l_ecellObject.type.Equals(Constants.xpathSystem))
            {
                if (!this.m_systemDic[this.m_currentProjectID].ContainsKey(l_ecellObject.modelID))
                {
                    this.m_systemDic[this.m_currentProjectID][l_ecellObject.modelID]
                            = new List<EcellObject>();
                }
                this.m_systemDic[this.m_currentProjectID][l_ecellObject.modelID].Add(l_ecellObject);
            }
            else if (l_ecellObject.type.Equals(Constants.xpathStepper))
            {
                if (!this.m_stepperDic[this.m_currentProjectID]
                        .ContainsKey(this.m_currentParameterID))
                {
                    this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID]
                        = new Dictionary<string, List<EcellObject>>();
                }
                if (!this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID]
                        .ContainsKey(l_ecellObject.modelID))
                {
                    this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID]
                        [l_ecellObject.modelID] = new List<EcellObject>();
                }
                this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID]
                        [l_ecellObject.modelID].Add(l_ecellObject);
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
                if (m_currentProjectPath == null)
                {
                    m_currentProjectPath = Path.GetDirectoryName(l_filename);
                }
                if (!m_simulatorDic.ContainsKey(m_currentProjectID))
                {
                    this.m_simulatorDic[this.m_currentProjectID] = CreateSimulatorInstance();
                    this.SetDMList();
                }
                EcellObject l_modelObj = EmlReader.Parse(l_filename,
                        m_simulatorDic[m_currentProjectID]);
                l_modelID = l_modelObj.modelID;

                //
                // Checks the old model ID
                //
                foreach (EcellObject l_model in this.m_modelDic[this.m_currentProjectID])
                {
                    if (l_model.modelID.Equals(l_modelID))
                    {
                        throw new Exception(
                            String.Format(
                                m_resources.GetString("ErrExistObj"),
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
                    this.m_simulatorDic[this.m_currentProjectID].Initialize();
                }
                catch (Exception)
                {
                    l_message = m_resources.GetString("ErrInitSim") + "[" + m_currentProjectID + "]";
                }
                //
                // Checks the current parameter ID.
                //
                if (this.m_currentParameterID == null || this.m_currentParameterID.Length <= 0)
                {
                    this.m_currentParameterID = Constants.parameterKey;
                }
                //
                // Sets initial conditions.
                //
                this.m_initialCondition[this.m_currentProjectID]
                        = new Dictionary<string, Dictionary<string, Dictionary<string,
                                Dictionary<string, double>>>>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        [l_modelID] = new Dictionary<string, Dictionary<string, double>>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        [l_modelID][Constants.xpathSystem] = new Dictionary<string, double>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        [l_modelID][Constants.xpathProcess] = new Dictionary<string, double>();
                this.m_initialCondition[this.m_currentProjectID][this.m_currentParameterID]
                        [l_modelID][Constants.xpathVariable] = new Dictionary<string, double>();
                InitializeModel(l_modelObj);
                //
                // Stores the "LoggerPolicy"
                //
                if (!this.m_loggerPolicyDic[this.m_currentProjectID]
                        .ContainsKey(this.m_currentParameterID))
                {
                    this.m_loggerPolicyDic[this.m_currentProjectID][this.m_currentParameterID]
                        = new LoggerPolicy(
                            LoggerPolicy.s_reloadStepCount,
                            LoggerPolicy.s_reloadInterval,
                            LoggerPolicy.s_diskFullAction,
                            LoggerPolicy.s_maxDiskSpace
                            );
                }
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Load Model: " + l_message + System.Environment.NewLine
                    );
                if (isLogging)
                    m_aManager.AddAction(new ImportModelAction(l_filename));
                string l_dirName = Path.GetDirectoryName(l_filename);
                m_loadDirList.Add(l_modelID, l_dirName);

                return l_modelID;
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrLoadModel") + "[" + l_message + "]";
                this.m_pManager.Message(
                        Constants.messageSimulation, l_message + System.Environment.NewLine);
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
                m_currentProjectID = l_prjID;
                m_loadingProject = l_prjID;
                l_message = "[" + l_prjID + "]";
                //
                // Initializes
                //
                if (l_prjID == null || l_prjID.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrNullData"));
                }

                List<EcellData> l_ecellDataList = new List<EcellData>();
                StreamReader l_reader = null;
                try
                {
                    l_reader = new StreamReader(l_prjFile);
                    string l_line = "";
                    string l_comment = "";
                    string l_parameter = null;
                    while ((l_line = l_reader.ReadLine()) != null)
                    {
                        if (l_line.IndexOf(Constants.textComment) == 0)
                        {
                            if (l_line.IndexOf(Constants.delimiterEqual) != -1)
                            {
                                l_comment = l_line.Split(Constants.delimiterEqual.ToCharArray())[1].Trim();
                            }
                            else
                            {
                                l_comment = l_line.Substring(l_line.IndexOf(Constants.textComment));
                            }
                        }
                        else if (l_line.IndexOf(Constants.textParameter) == 0)
                        {
                            if (l_line.IndexOf(Constants.delimiterEqual) != -1)
                            {
                                l_parameter = l_line.Split(Constants.delimiterEqual.ToCharArray())[1].Trim();
                            }
                            else
                            {
                                l_parameter = l_line.Substring(l_line.IndexOf(Constants.textParameter));
                            }
                        }
                        else if (!l_comment.Equals(""))
                        {
                            l_comment = l_comment + "\n" + l_line;
                        }
                        else if (l_line.IndexOf(Constants.xpathProject) == 0)
                        {
                            if (l_line.IndexOf(Constants.delimiterEqual) != -1)
                            {
                                l_prjID = l_line.Split(Constants.delimiterEqual.ToCharArray())[1].Trim();
                            }
                            else
                            {
                                l_prjID = l_line.Substring(l_line.IndexOf(Constants.textComment));
                            }
                        }
                    }
                    l_prj = new Project(l_prjID, l_comment, File.GetLastWriteTime(l_prjFile).ToString());
                    this.m_projectList.Add(l_prj);
                    l_ecellDataList.Add(new EcellData(Constants.textComment, new EcellValue(l_comment), null));
                    l_passList.Add(EcellObject.CreateObject(l_prjID, "", Constants.xpathProject, "", l_ecellDataList));
                    //
                    // Initializes.
                    //
                    this.m_currentProjectID = l_prjID;
                    this.m_currentProjectPath = Path.GetDirectoryName(l_prjFile);
                    this.m_currentParameterID = l_parameter;
                    this.m_simulatorDic[l_prjID] = CreateSimulatorInstance();
                    this.m_simulatorExeFlagDic[l_prjID] = s_simulationWait;
                    this.m_projectList.Add(new Project(l_prjID, l_comment, DateTime.Now.ToString()));
                    this.m_loggerPolicyDic[l_prjID] = new Dictionary<string, LoggerPolicy>();
                    //this.m_stepperDic[l_prjID] = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                    m_stepperDic.Add(l_prjID, new Dictionary<string, Dictionary<string, List<EcellObject>>>());
                    this.m_systemDic[l_prjID] = new Dictionary<string, List<EcellObject>>();
                    this.m_modelDic[l_prjID] = new List<EcellObject>();
                    this.m_initialCondition[l_prjID]
                        = new Dictionary<string,
                            Dictionary<string, Dictionary<string, Dictionary<string, double>>>>();
                }
                finally
                {
                    if (l_reader != null)
                    {
                        l_reader.Close();
                    }
                }

                if (l_prj == null)
                {
                    throw new Exception(m_resources.GetString("ErrFindPrjFile") + " [" + Constants.fileProject + "]");
                }
                //
                // Loads the model.
                //
                string l_modelDirName =
                    Path.GetDirectoryName(l_prjFile) + Constants.delimiterPath + Constants.xpathModel;
                if (Directory.Exists(l_modelDirName))
                {
                    string[] l_models = Directory.GetFileSystemEntries(
                        l_modelDirName,
                        Constants.delimiterWildcard + Constants.delimiterPeriod + Constants.xpathEml
                        );
                    if (l_models != null && l_models.Length > 0)
                    {
                        foreach (string l_model in l_models)
                        {
                            string l_fileName = Path.GetFileName(l_model);
                            if (l_fileName.IndexOf(Constants.delimiterUnderbar) != 0)
                            {
                                this.LoadModel(l_model, false);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(m_resources.GetString("ErrFindModel"));
                    }
                    l_passList.AddRange(this.m_modelDic[this.m_currentProjectID]);
                    foreach (string l_storedModelID in this.m_systemDic[this.m_currentProjectID].Keys)
                    {
                        l_passList.AddRange(this.m_systemDic[this.m_currentProjectID][l_storedModelID]);
                    }
                }
                else
                {
                    throw new Exception(m_resources.GetString("ErrFindModel"));
                }
                //
                // Loads the simulation parameter.
                //
                string l_simulationDirName =
                    Path.GetDirectoryName(l_prjFile) + Constants.delimiterPath + Constants.xpathParameters;

                if (Directory.Exists(l_simulationDirName))
                {
                    l_parameters = Directory.GetFileSystemEntries(
                        l_simulationDirName,
                        Constants.delimiterWildcard + Constants.delimiterPeriod + Constants.xpathXml
                        );
                    if (l_parameters != null && l_parameters.Length > 0)
                    {
                        foreach (string l_parameter in l_parameters)
                        {
                            string l_fileName = Path.GetFileName(l_parameter);
                            if (l_fileName.IndexOf(Constants.delimiterUnderbar) != 0)
                            {
                                this.LoadSimulationParameter(l_parameter);
                            }
                        }
                    }
                }
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Load Project: " + l_message + System.Environment.NewLine);
            }
            catch (Exception l_ex)
            {
                m_loadingProject = null;
                l_passList = null;
                if (this.m_simulatorDic.ContainsKey(l_prjID))
                {
                    this.m_simulatorDic.Remove(l_prjID);
                }
                if (this.m_simulatorExeFlagDic.ContainsKey(l_prjID))
                {
                    this.m_simulatorExeFlagDic.Remove(l_prjID);
                }
                if (l_prj != null)
                {
                    if (this.m_projectList.Contains(l_prj))
                    {
                        this.m_projectList.Remove(l_prj);
                        l_prj = null;
                    }
                }
                if (this.m_loggerPolicyDic.ContainsKey(l_prjID))
                {
                    this.m_loggerPolicyDic.Remove(l_prjID);
                }
                if (this.m_stepperDic.ContainsKey(l_prjID))
                {
                    this.m_stepperDic.Remove(l_prjID);
                }
                if (this.m_systemDic.ContainsKey(l_prjID))
                {
                    this.m_systemDic.Remove(l_prjID);
                }
                if (this.m_modelDic.ContainsKey(l_prjID))
                {
                    this.m_modelDic.Remove(l_prjID);
                }
                if (this.m_initialCondition.ContainsKey(l_prjID))
                {
                    this.m_initialCondition.Remove(l_prjID);
                }
                l_message = m_resources.GetString("ErrLoadPrj") + "[" + l_message + "]";
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine);
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
                m_loadingProject = null;
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
                //
                // Initializes
                //
                if (l_fileName == null || l_fileName.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrNullData"));
                }
                //
                // Parses the simulation parameter.
                //
                SimulationParameter simParam = SimulationParameterReader.Parse(
                        l_fileName, m_simulatorDic[m_currentProjectID]);

                //
                // Stores the simulation parameter.
                //
                if (!this.m_currentParameterID.Equals(simParam.ID))
                {
                    if (!this.m_stepperDic[this.m_currentProjectID].ContainsKey(simParam.ID))
                    {
                        this.m_stepperDic[this.m_currentProjectID][simParam.ID]
                            = new Dictionary<string, List<EcellObject>>();
                    }
                    foreach (EcellObject l_stepper in simParam.Steppers)
                    {
                        if (!this.m_stepperDic[this.m_currentProjectID][simParam.ID]
                            .ContainsKey(l_stepper.modelID))
                        {
                            this.m_stepperDic[this.m_currentProjectID][simParam.ID][l_stepper.modelID]
                                = new List<EcellObject>();
                        }
                        foreach (EcellData l_data in l_stepper.Value)
                        {
                            double l_value = 0.0;
                            try
                            {
                                if (l_data.Value.CastToList()[0].ToString().Equals(
                                    Double.PositiveInfinity.ToString()))
                                {
                                    l_value = Double.PositiveInfinity;
                                }
                                else if (l_data.Value.CastToList()[0].ToString().Equals(
                                    Double.MaxValue.ToString()))
                                {
                                    l_value = Double.MaxValue;
                                }
                                else
                                {
                                    l_value = XmlConvert.ToDouble(l_data.Value.CastToList()[0].ToString());
                                }
                            }
                            catch (Exception)
                            {
                                l_value = Double.PositiveInfinity;
                            }
                            l_data.Value = new EcellValue(l_value);
                        }
                        this.m_stepperDic[this.m_currentProjectID][simParam.ID][l_stepper.modelID].Add(l_stepper);
                    }
                }
                else
                {
                    foreach (EcellObject l_stepper in simParam.Steppers)
                    {
                        bool l_matchFlag = false;
                        if (!this.m_stepperDic[this.m_currentProjectID][simParam.ID].ContainsKey(l_stepper.modelID))
                        {
                            this.m_stepperDic[this.m_currentProjectID][simParam.ID][l_stepper.modelID]
                                = new List<EcellObject>();
                        }
                        for (int j = 0;
                            j < this.m_stepperDic[this.m_currentProjectID][simParam.ID][l_stepper.modelID].Count;
                            j++)
                        {
                            EcellObject l_storedStepper
                                = this.m_stepperDic[this.m_currentProjectID][simParam.ID][l_stepper.modelID][j];
                            if (l_storedStepper.classname.Equals(l_stepper.classname)
                                && l_storedStepper.key.Equals(l_stepper.key)
                                && l_storedStepper.modelID.Equals(l_stepper.modelID)
                                && l_storedStepper.type.Equals(l_stepper.type))
                            {
                                List<EcellData> l_newDataList = new List<EcellData>();
                                foreach (EcellData l_storedData in l_storedStepper.Value)
                                {
                                    bool l_existFlag = false;
                                    foreach (EcellData l_newData in l_stepper.Value)
                                    {
                                        if (l_storedData.Name.Equals(l_newData.Name)
                                            && l_storedData.EntityPath.Equals(l_newData.EntityPath))
                                        {
                                            if (l_storedData.Value.IsDouble())
                                            {
                                                try
                                                {
                                                    string l_newValue = l_newData.Value.CastToList()[0].ToString();
                                                    if (l_newValue.Equals(Double.PositiveInfinity.ToString()))
                                                    {
                                                        l_newData.Value = new EcellValue(Double.PositiveInfinity);
                                                    }
                                                    else
                                                    {
                                                        l_newData.Value
                                                            = new EcellValue(XmlConvert.ToDouble(l_newValue));
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    l_newData.Value = new EcellValue(Double.PositiveInfinity);
                                                }
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
                                    }
                                    if (!l_existFlag)
                                    {
                                        l_newDataList.Add(l_storedData);
                                    }
                                }
                                this.m_stepperDic[this.m_currentProjectID][simParam.ID][l_stepper.modelID][j]
                                    = EcellObject.CreateObject(
                                        l_stepper.modelID,
                                        l_stepper.key,
                                        l_stepper.type,
                                        l_stepper.classname,
                                        l_newDataList);
                                l_matchFlag = true;
                                break;
                            }
                        }
                        if (!l_matchFlag)
                        {
                            this.m_stepperDic[this.m_currentProjectID][simParam.ID][l_stepper.modelID]
                                .Add(l_stepper);
                        }
                    }
                }
                this.m_loggerPolicyDic[this.m_currentProjectID][simParam.ID] = simParam.LoggerPolicy;
                this.m_initialCondition[this.m_currentProjectID][simParam.ID] = simParam.InitialConditions;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Load Simulation Parameter: " + l_message + System.Environment.NewLine);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrLoadSimParam") + "[" + l_message + "]";
                this.m_pManager.Message(Constants.messageSimulation, l_message + System.Environment.NewLine);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Loads the "Stepper" 2 the "EcellCoreLib".
        /// </summary>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_stepperList">The list of the "Stepper"</param>
        /// <param name="l_setStepperDic"></param>
        private void LoadStepper(
            WrappedSimulator l_simulator,
            List<EcellObject> l_stepperList,
            Dictionary<string, Dictionary<string, WrappedPolymorph>> l_setStepperDic)
        {
            if (l_stepperList == null || l_stepperList.Count <= 0)
            {
                throw new Exception(m_resources.GetString("ErrFindSimParam"));
            }
            bool l_existStepper = false;
            foreach (EcellObject l_stepper in l_stepperList)
            {
                if (l_stepper == null)
                {
                    continue;
                }
                l_existStepper = true;
                l_simulator.CreateStepper(l_stepper.classname, l_stepper.key);
                //
                // 4 property
                //
                if (l_stepper.Value == null || l_stepper.Value.Count <= 0)
                {
                    continue;
                }
                foreach (EcellData l_ecellData in l_stepper.Value)
                {
                    if (l_ecellData.Name == null || l_ecellData.Name.Length <= 0 || l_ecellData.Value == null)
                    {
                        continue;
                    }
                    else if (!l_ecellData.Value.IsDouble() && !l_ecellData.Value.IsInt())
                    {
                        continue;
                    }
                    //
                    // 4 MaxStepInterval == Double.MaxValue
                    //
                    try
                    {
                        string l_value
                            = l_ecellData.Value.ToString().Replace("(", "").Replace(")", "").Replace("\"", "");
                        if (l_value.Equals(Double.PositiveInfinity.ToString()))
                        {
                            continue;
                        }
                        else if (l_value.Equals(Double.MaxValue.ToString()))
                        {
                            continue;
                        }
                        XmlConvert.ToDouble(l_value);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    if (l_ecellData.Saveable)
                    {
                        if (l_ecellData.Value.IsDouble()
                            &&
                            (Double.IsInfinity(l_ecellData.Value.CastToDouble())
                                || Double.IsNaN(l_ecellData.Value.CastToDouble())))
                        {
                            continue;
                        }
                        l_simulator.LoadStepperProperty(
                            l_stepper.key,
                            l_ecellData.Name,
                            EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellData.Value));
                    }
                    else if (l_ecellData.Settable)
                    {
                        if (l_ecellData.Value.IsDouble()
                            &&
                            (Double.IsInfinity(l_ecellData.Value.CastToDouble())
                                || Double.IsNaN(l_ecellData.Value.CastToDouble())))
                        {
                            continue;
                        }
                        if (!l_setStepperDic.ContainsKey(l_stepper.key))
                        {
                            l_setStepperDic[l_stepper.key] = new Dictionary<string, WrappedPolymorph>();
                        }
                        l_setStepperDic[l_stepper.key][l_ecellData.Name]
                            = EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellData.Value);
                    }
                }
            }
            if (!l_existStepper)
            {
                throw new Exception(m_resources.GetString("ErrFindSimParam"));
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
        private void LoadSystem(
            WrappedSimulator l_simulator,
            List<EcellObject> l_systemList,
            List<string> l_loggerList,
            Dictionary<string, Dictionary<string, double>> l_initialCondition,
            Dictionary<string, WrappedPolymorph> l_setPropertyDic)
        {
            if (l_systemList == null || l_systemList.Count <= 0)
            {
                throw new Exception(m_resources.GetString("ErrFindSystem"));
            }
            bool l_existSystem = false;
            Dictionary<string, WrappedPolymorph> l_processPropertyDic = new Dictionary<string, WrappedPolymorph>();
            try
            {
                foreach (EcellObject l_system in l_systemList)
                {
                    if (l_system == null)
                    {
                        continue;
                    }
                    l_existSystem = true;
                    string l_parentPath = l_system.key.Substring(0, l_system.key.LastIndexOf(Constants.delimiterPath));
                    string l_childPath = l_system.key.Substring(l_system.key.LastIndexOf(Constants.delimiterPath) + 1);
                    if (l_system.key.Equals(Constants.delimiterPath))
                    {
                        if (l_childPath.Length == 0)
                        {
                            l_childPath = Constants.delimiterPath;
                        }
                    }
                    else
                    {
                        if (l_parentPath.Length == 0)
                        {
                            l_parentPath = Constants.delimiterPath;
                        }
                        l_simulator.CreateEntity(
                            l_system.classname,
                            l_system.classname + Constants.delimiterColon
                                + l_parentPath + Constants.delimiterColon + l_childPath);
                    }
                    //
                    // 4 property
                    //
                    if (l_system.Value == null || l_system.Value.Count <= 0)
                    {
                        continue;
                    }
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
                    //
                    // 4 children
                    //
                    if (l_system.Children != null && l_system.Children.Count > 0)
                    {
                        this.LoadEntity(
                            l_simulator,
                            l_system.Children,
                            l_loggerList,
                            l_processPropertyDic,
                            l_initialCondition,
                            l_setPropertyDic);
                    }
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
                    throw new Exception(m_resources.GetString("ErrFindSystem"));
                }
            }
            catch (WrappedException e)
            {
                throw new Exception("Failed to create System", e);
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

                Dictionary<string, EcellData> list = GetProcessProperty(DataManager.s_defaultProcessName);
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
                    Constants.xpathProcess, DataManager.s_defaultProcessName, data);

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
                String errmes = m_resources.GetString("ErrAddObj");
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
                String errmes = m_resources.GetString("ErrAddObj");
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
                String errmes = m_resources.GetString("ErrAddObj");
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
                if (this.m_currentProjectID != null)
                {
                    this.CloseProject(this.m_currentProjectID);
                }
                //
                // Initialize
                //
                m_currentProjectID = l_prjID;
                m_currentProjectPath = l_projectPath;
                m_simulatorDic[l_prjID] = CreateSimulatorInstance();
                CreateProjectDir(l_prjID);
                SetDMList();
                m_simulatorExeFlagDic[l_prjID] = s_simulationWait;
                l_prj = new Project(l_prjID, l_comment, DateTime.Now.ToString());
                m_projectList.Add(l_prj);
                m_loggerPolicyDic[l_prjID] = new Dictionary<string, LoggerPolicy>();
                // this.m_stepperDic[l_prjID] = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
                m_stepperDic.Add(l_prjID, new Dictionary<string, Dictionary<string, List<EcellObject>>>());
                m_systemDic[l_prjID] = new Dictionary<string, List<EcellObject>>();
                m_modelDic[l_prjID] = new List<EcellObject>();
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
                m_pManager.Message(
                    Constants.messageSimulation,
                    "Create Project: [" + l_prjID + "]" + System.Environment.NewLine);
                m_aManager.AddAction(new NewProjectAction(l_prjID, l_comment, l_projectPath));
            }
            catch (Exception l_ex)
            {
                if (this.m_simulatorDic.ContainsKey(l_prjID))
                {
                    this.m_simulatorDic.Remove(l_prjID);
                }
                if (this.m_simulatorExeFlagDic.ContainsKey(l_prjID))
                {
                    this.m_simulatorExeFlagDic.Remove(l_prjID);
                }
                if (l_prj != null)
                {
                    if (this.m_projectList.Contains(l_prj))
                    {
                        this.m_projectList.Remove(l_prj);
                        l_prj = null;
                    }
                }
                if (this.m_loggerPolicyDic.ContainsKey(l_prjID))
                {
                    this.m_loggerPolicyDic.Remove(l_prjID);
                }
                if (this.m_stepperDic.ContainsKey(l_prjID))
                {
                    this.m_stepperDic.Remove(l_prjID);
                }
                if (this.m_systemDic.ContainsKey(l_prjID))
                {
                    this.m_systemDic.Remove(l_prjID);
                }
                if (this.m_modelDic.ContainsKey(l_prjID))
                {
                    this.m_modelDic.Remove(l_prjID);
                }
                string l_message = String.Format(
                        m_resources.GetString("ErrCrePrj"),
                        new object[] { l_prjID });
                m_pManager.Message(Constants.messageSimulation, l_message + System.Environment.NewLine);
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
                if (!this.m_stepperDic[this.m_currentProjectID].ContainsKey(l_parameterID))
                {
                    //
                    // Searches the source stepper.
                    //
                    if (this.m_currentParameterID != null && this.m_currentParameterID.Length > 0)
                    {
                        l_storedParameterID = this.m_currentParameterID;
                    }
                    else
                    {
                        if (this.m_stepperDic[this.m_currentProjectID] != null
                            && this.m_stepperDic[this.m_currentProjectID].Count > 0)
                        {
                            foreach (string l_key
                                    in this.m_stepperDic[this.m_currentProjectID].Keys)
                            {
                                l_storedParameterID = l_key;
                                break;
                            }
                        }
                        else
                        {
                            throw new Exception(m_resources.GetString("ErrFindSimParam"));
                        }
                    }
                    //
                    // Sets the destination stepper.
                    //
                    this.m_stepperDic[this.m_currentProjectID][l_parameterID]
                            = new Dictionary<string, List<EcellObject>>();
                    foreach (string l_key
                            in this.m_stepperDic[this.m_currentProjectID][l_storedParameterID].Keys)
                    {
                        this.m_stepperDic[this.m_currentProjectID][l_parameterID][l_key]
                                = new List<EcellObject>();
                        foreach (EcellObject l_stepper
                                in this.m_stepperDic[this.m_currentProjectID][l_storedParameterID]
                                [l_key])
                        {
                            this.m_stepperDic[this.m_currentProjectID][l_parameterID][l_key]
                                    .Add(l_stepper.Copy());
                        }
                    }
                }
                else
                {
                    throw new Exception(
                        String.Format(
                            m_resources.GetString("ErrExistSimParam"),
                            new object[] { l_message }
                        )
                    );
                }
                //
                // 4 LoggerPolicy
                //
                LoggerPolicy l_loggerPolicy
                    = this.m_loggerPolicyDic[this.m_currentProjectID][l_storedParameterID];
                this.m_loggerPolicyDic[this.m_currentProjectID][l_parameterID]
                    = new LoggerPolicy(
                        l_loggerPolicy.m_reloadStepCount,
                        l_loggerPolicy.m_reloadInterval,
                        l_loggerPolicy.m_diskFullAction,
                        l_loggerPolicy.m_maxDiskSpace);
                //
                // 4 Initial Condition
                //
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_srcInitialCondition
                    = this.m_initialCondition[this.m_currentProjectID][l_storedParameterID];
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_dstInitialCondition
                    = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
                this.Copy4InitialCondition(l_srcInitialCondition, l_dstInitialCondition);
                this.m_initialCondition[this.m_currentProjectID][l_parameterID] = l_dstInitialCondition;
                m_pManager.ParameterAdd(m_currentProjectID, l_parameterID);
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Create Simulation Parameter: " + l_message + System.Environment.NewLine);
                if (l_isRecorded)
                    m_aManager.AddAction(new NewSimParamAction(l_parameterID, l_isAnchor));
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrCreSimParam") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine);
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
        private void CreateProjectDir(string prjID)
        {
            SetDefaultDir();
            string baseDir = this.m_defaultDir + Constants.delimiterPath + prjID;
            string modelDir = baseDir + Constants.delimiterPath + Constants.xpathModel;
            string dmDir = baseDir + Constants.delimiterPath + Constants.DMDirName;
            string paramDir = baseDir +Constants.delimiterPath+ Constants.xpathParameters; 

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
                    throw new Exception(m_resources.GetString("ErrNullData"));
                }
                this.SetDefaultDir();
                if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrBaseDir"));
                }
                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + this.m_currentProjectID))
                {
                    this.SaveProject(this.m_currentProjectID);
                }
                string l_modelDirName
                    = this.m_defaultDir + Constants.delimiterPath +
                    this.m_currentProjectID + Constants.delimiterPath + Constants.xpathModel;
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
                    = this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID][l_modelID];
                if (l_stepperList == null || l_stepperList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrFindStepper"));
                }
                l_storedList.AddRange(l_stepperList);
                //
                // Picks the "System" up.
                //
                List<EcellObject> l_systemList = this.m_systemDic[this.m_currentProjectID][l_modelID];
                if (l_systemList == null || l_systemList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrFindSystem"));
                }
                l_storedList.AddRange(l_systemList);
                //
                // Creates.
                //
                EmlWriter.Create(l_modelFileName, l_storedList);
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Save Model: " + l_message + System.Environment.NewLine);
                //
                // 4 Project
                //
                this.SaveProject(this.m_currentProjectID);
                m_pManager.SaveModel(l_modelID, l_modelDirName);
            }
            catch (Exception l_ex)
            {
                l_storedList = null;
                l_message = m_resources.GetString("ErrSaveModel") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Saves only the project using the project ID.
        /// </summary>
        /// <param name="l_prjID">The saved project ID</param>
        public void SaveProject(string l_prjID)
        {
            Project l_thisPrj = null;
            string l_message = null;
            try
            {
                l_message = "[" + l_prjID + "]";
                //
                // Initializes
                //
                if (l_prjID == null || l_prjID.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrNullData"));
                }
                if (this.m_projectList == null || this.m_projectList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrFindPrj"));
                }
                else
                {
                    foreach (Project l_prj in this.m_projectList)
                    {
                        if (l_prj.M_prjName.Equals(l_prjID))
                        {
                            l_thisPrj = l_prj;
                            break;
                        }
                    }
                    if (l_thisPrj == null)
                    {
                        throw new Exception(m_resources.GetString("ErrFindPrj"));
                    }
                }
                this.SetDefaultDir();
                if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrBaseDir"));
                }
                //
                // Saves the project.
                //
                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + l_prjID))
                {
                    Directory.CreateDirectory(this.m_defaultDir + Constants.delimiterPath + l_prjID);
                }
                string l_prjFile = this.m_defaultDir + Constants.delimiterPath +
                    l_prjID + Constants.delimiterPath + Constants.fileProject;
                StreamWriter l_writer = null;
                try
                {
                    l_writer = new StreamWriter(l_prjFile);
                    l_writer.WriteLine(
                        Constants.xpathProject + Constants.delimiterSpace + Constants.delimiterEqual + Constants.delimiterSpace
                        + l_thisPrj.M_prjName);
                    l_writer.WriteLine(
                        Constants.textComment + Constants.delimiterSpace + Constants.delimiterEqual + Constants.delimiterSpace
                            + l_thisPrj.M_comment);
                    l_writer.WriteLine(
                        Constants.textParameter + Constants.delimiterSpace + Constants.delimiterEqual + Constants.delimiterSpace
                            + this.m_currentParameterID);
                }
                finally
                {
                    if (l_writer != null)
                    {
                        l_writer.Close();
                    }
                }
                l_thisPrj.M_updateTime = File.GetLastAccessTime(l_prjFile).ToString();
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Save Project: " + l_message + System.Environment.NewLine);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrSavePrj") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine);
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Clear the counter to count object in model.
        /// </summary>
        public void ClearScriptInfo()
        {
            s_stepperCount = 0;
            s_sysCount = 0;
            s_varCount = 0;
            s_proCount = 0;
            s_logCount = 0;
            s_exportSystem.Clear();
            s_exportProcess.Clear();
            s_exportVariable.Clear();
            s_exportStepper.Clear();
        }

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="l_fileName"></param>
        public void SaveScript(string l_fileName)
        {
            try
            {
                ClearScriptInfo();
                Encoding enc = Encoding.GetEncoding(932);
                File.WriteAllText(l_fileName, "", enc);
                WritePrefix(l_fileName, enc);
                foreach (EcellObject modelObj in m_modelDic[m_currentProjectID])
                {
                    String modelName = modelObj.modelID;
                    WriteModelEntry(l_fileName, enc, modelName);
                    WriteModelProperty(l_fileName, enc, modelName);
                    File.AppendAllText(l_fileName, "\n# System\n", enc);
                    foreach (EcellObject sysObj in
                        m_systemDic[m_currentProjectID][modelName])
                    {
                        WriteSystemEntry(l_fileName, enc, modelName, sysObj);
                        WriteSystemProperty(l_fileName, enc, modelName, sysObj);
                    }
                    foreach (EcellObject sysObj in
                        m_systemDic[m_currentProjectID][modelName])
                    {
                        WriteComponentEntry(l_fileName, enc, sysObj);
                        WriteComponentProperty(l_fileName, enc, sysObj);
                    }
                }
                WriteSimulationForStep(l_fileName, 100, enc);

            }
            catch (Exception l_ex)
            {
                l_ex.ToString();
            }
        }
        static private int s_stepperCount = 0;
        static private int s_sysCount = 0;
        static private int s_proCount = 0;
        static private int s_varCount = 0;
        static private int s_logCount = 0;
        static private Dictionary<string, int> s_exportStepper = new Dictionary<string, int>();
        static private Dictionary<string, int> s_exportSystem = new Dictionary<string, int>();
        static private Dictionary<string, int> s_exportProcess = new Dictionary<string, int>();
        static private Dictionary<string, int> s_exportVariable = new Dictionary<string, int>();

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
                    throw new Exception(m_resources.GetString("ErrNullData"));
                }
                this.SetDefaultDir();
                if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrBaseDir"));
                }
                if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + this.m_currentProjectID))
                {
                    this.SaveProject(this.m_currentProjectID);
                }
                string l_simulationDirName =
                    this.m_defaultDir + Constants.delimiterPath +
                    this.m_currentProjectID + Constants.delimiterPath + Constants.xpathParameters;
                if (!Directory.Exists(l_simulationDirName))
                {
                    Directory.CreateDirectory(l_simulationDirName);
                }
                string l_simulationFileName
                    = l_simulationDirName + Constants.delimiterPath + l_paramID + Constants.delimiterPeriod + Constants.xpathXml;
                //
                // Picks the "Stepper" up.
                //
                List<EcellObject> l_stepperList = new List<EcellObject>();
                foreach (string l_modelID in this.m_stepperDic[this.m_currentProjectID][l_paramID].Keys)
                {
                    l_stepperList.AddRange(this.m_stepperDic[this.m_currentProjectID][l_paramID][l_modelID]);
                }
                if (l_stepperList == null || l_stepperList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrFindStepper"));
                }
                //
                // Picks the "LoggerPolicy" up.
                //
                LoggerPolicy l_loggerPolicy = this.m_loggerPolicyDic[this.m_currentProjectID][l_paramID];
                //
                // Picks the "InitialCondition" up.
                //
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_initialCondition
                        = this.m_initialCondition[this.m_currentProjectID][l_paramID];
                //
                // Creates.
                //
                SimulationParameterWriter.Create(l_simulationFileName,
                    new SimulationParameter(
                        l_stepperList,
                        l_initialCondition,
                        l_loggerPolicy,
                        l_paramID));
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Save Simulation Parameter: " + l_message + System.Environment.NewLine);
                //
                // 4 Project
                //
                this.SaveProject(this.m_currentProjectID);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrSaveSim") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
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
                    "[" + this.m_currentProjectID + "][" + this.m_currentParameterID + "]";
                //
                // Initializes.
                //
                if (l_fullIDList == null || l_fullIDList.Count <= 0)
                {
                    return;
                }
                string l_simulationDirName = null;
                if (l_savedDirName != null && l_savedDirName.Length > 0)
                {
                    l_simulationDirName = l_savedDirName;
                }
                else
                {
                    this.SetDefaultDir();
                    if (this.m_defaultDir == null || this.m_defaultDir.Length <= 0)
                    {
                        throw new Exception(m_resources.GetString("ErrBaseDir"));
                    }
                    if (!Directory.Exists(this.m_defaultDir + Constants.delimiterPath + this.m_currentProjectID))
                    {
                        this.SaveProject(this.m_currentProjectID);
                    }
                    l_simulationDirName =
                        this.m_defaultDir + Constants.delimiterPath +
                        this.m_currentProjectID + Constants.delimiterPath + Constants.xpathParameters;
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
                    this.m_loggerPolicyDic[this.m_currentProjectID][this.m_currentParameterID].m_reloadInterval
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
                            this.m_pManager.Message(
                                Constants.messageSimulation,
                                "Save Simulation Result: " + l_message + System.Environment.NewLine
                                );
                        }
                    }
                }
                //
                // 4 Project
                //
                //this.SaveProject(this.m_currentProjectID);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrSaveSim") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
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
        /// Sets the list of the DM.
        /// </summary>
        private void SetDMList()
        {
            //
            // Initialize
            //
            this.m_dmDic = new Dictionary<string, List<string>>();
            // 4 Process
            this.m_dmDic.Add(Constants.xpathProcess, new List<string>());
            // 4 Stepper
            this.m_dmDic.Add(Constants.xpathStepper, new List<string>());
            // 4 System
            List<string> l_systemList = new List<string>();
            l_systemList.Add(Constants.xpathSystem);
            this.m_dmDic.Add(Constants.xpathSystem, l_systemList);
            // 4 Variable
            List<string> l_variableList = new List<string>();
            l_variableList.Add(Constants.xpathVariable);
            this.m_dmDic.Add(Constants.xpathVariable, l_variableList);
            //
            // Searches the DM paths
            //
            string[] l_dmPathArray = Util.GetDMDirs(m_currentProjectPath);
            if (l_dmPathArray == null)
            {
                throw new Exception(m_resources.GetString("ErrFindDmDir"));
            }
            foreach (string dmPath in l_dmPathArray)
            {
                if (!Directory.Exists(dmPath))
                {
                    continue;
                }
                // 4 Process
                string[] l_processDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathProcess + Constants.dmFileExtension
                    );
                foreach (string l_processDM in l_processDMArray)
                {
                    this.m_dmDic[Constants.xpathProcess].Add(Path.GetFileNameWithoutExtension(l_processDM));
                }
                // 4 Stepper
                string[] l_stepperDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathStepper + Constants.dmFileExtension
                    );
                foreach (string l_stepperDM in l_stepperDMArray)
                {
                    this.m_dmDic[Constants.xpathStepper].Add(Path.GetFileNameWithoutExtension(l_stepperDM));
                }
                // 4 System
                string[] l_systemDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathSystem + Constants.dmFileExtension
                    );
                foreach (string l_systemDM in l_systemDMArray)
                {
                    this.m_dmDic[Constants.xpathSystem].Add(Path.GetFileNameWithoutExtension(l_systemDM));
                }
                // 4 Variable
                string[] l_variableDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathVariable + Constants.dmFileExtension
                    );
                foreach (string l_variableDM in l_variableDMArray)
                {
                    this.m_dmDic[Constants.xpathVariable].Add(Path.GetFileNameWithoutExtension(l_variableDM));
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
                if (this.m_simulatorDic[this.m_currentProjectID] == null
                    || this.GetCurrentSimulationTime() <= 0.0)
                {
                    return;
                }
                EcellValue l_storedValue
                    = new EcellValue(this.m_simulatorDic[this.m_currentProjectID].GetEntityProperty(l_fullPN));
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
                this.m_simulatorDic[this.m_currentProjectID].LoadEntityProperty(
                    l_fullPN,
                    EcellValue.CastToWrappedPolymorph4EcellValue(l_newValue));
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrSetProp") + l_message + " {" + l_ex.ToString() + "}");
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
                this.m_loggerPolicyDic[this.m_currentProjectID][l_parameterID] = l_loggerPolicy;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Update Logger Policy: " + l_message + System.Environment.NewLine);
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrUpdateLogPol") + l_message;
                this.m_pManager.Message(Constants.messageSimulation, l_message + System.Environment.NewLine);
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
            if (null != l_modelID && m_systemDic[this.m_currentProjectID].ContainsKey(l_modelID))
            {
                foreach (EcellObject eo in m_systemDic[m_currentProjectID][l_modelID])
                    m_pManager.SetPosition(eo);
            }
        }

        /// <summary>
        /// Sets the parameter of the simulator.
        /// </summary>
        public void SetSimulationParameter(string l_parameterID)
        {
            SetSimulationParameter(l_parameterID, true, true);
        }

        /// <summary>
        /// Sets the parameter of the simulator.
        /// </summary>
        /// <param name="l_parameterID"></param>
        /// <param name="l_isRecorded">Whether this action is recorded or not</param>
        /// <param name="l_isAnchor">Whether this action is an anchor or not</param>
        public void SetSimulationParameter(string l_parameterID, bool l_isRecorded, bool l_isAnchor)
        {
            string l_message = null;
            try
            {
                l_message = "[" + l_parameterID + "]";
                string l_oldParameterID = this.m_currentParameterID;
                if (this.m_currentParameterID != l_parameterID)
                {
                    foreach (string l_modelID
                        in this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID].Keys)
                    {
                        if (!this.m_stepperDic[this.m_currentProjectID][l_parameterID].ContainsKey(l_modelID))
                        {
                            continue;
                        }
                        List<EcellObject> l_currentList
                            = this.m_stepperDic[this.m_currentProjectID][this.m_currentParameterID][l_modelID];
                        List<EcellObject> l_newList
                            = this.m_stepperDic[this.m_currentProjectID][l_parameterID][l_modelID];
                        foreach (EcellObject l_current in l_currentList)
                        {
                            foreach (EcellObject l_new in l_newList)
                            {
                                if (!l_current.classname.Equals(l_new.classname))
                                {
                                    continue;
                                }
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
                    this.m_currentParameterID = l_parameterID;
                    this.Initialize(true);
                    foreach (string l_modelID
                        in this.m_stepperDic[this.m_currentProjectID][l_oldParameterID].Keys)
                    {
                        foreach (EcellObject l_old
                            in this.m_stepperDic[this.m_currentProjectID][l_oldParameterID][l_modelID])
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
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Set Simulation Parameter: " + l_message + System.Environment.NewLine);
                if (l_isRecorded)
                    m_aManager.AddAction(new SetSimParamAction(l_parameterID, l_oldParameterID, l_isAnchor));
            }
            catch (Exception l_ex)
            {
                l_message = m_resources.GetString("ErrSetSimParam") + l_message;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
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
                //
                // Checks the simulator's status.
                //
                if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationWait)
                {
                    this.Initialize(true);
                    this.m_simulationStepLimit = l_stepLimit;
                    this.m_pManager.Message(
                        Constants.messageSimulation,
                        "Start Simulator: [" + this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime() + "]"
                            + System.Environment.NewLine
                        );
                }
                else if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationSuspend)
                {
                    if (this.m_simulationStepLimit == -1)
                    {
                        this.m_simulationStepLimit = l_stepLimit;
                    }
                    this.m_pManager.Message(
                        Constants.messageSimulation,
                        "Restart Simulator: [" + this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime() + "]"
                            + System.Environment.NewLine
                        );
                }
                else
                {
                    throw new Exception(m_resources.GetString("ErrRunning"));
                }
                //
                // Debug
                //
                // this.LookIntoSimulator(this.m_simulatorDic[this.m_currentProjectID]);
                //
                // Selects the type of the simulation and Starts.
                //
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationRun;
                if (this.m_simulationStepLimit > 0)
                {
                    while (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun)
                    {
                        if (this.m_simulationStepLimit <= s_defaultStepCount)
                        {
                            this.m_simulatorDic[this.m_currentProjectID].Step(this.m_simulationStepLimit);
                            Application.DoEvents();
                            double l_currentTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
                            if (l_statusNum == s_simulationSuspend)
                            {
                                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationSuspend;
                            }
                            else
                            {
                                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationWait;
                            }
                            this.m_pManager.AdvancedTime(l_currentTime);
                            this.m_simulationStepLimit = -1;
                            break;
                        }
                        else
                        {
                            this.m_simulatorDic[this.m_currentProjectID].Step(s_defaultStepCount);
                            Application.DoEvents();
                            double l_currentTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
                            this.m_pManager.AdvancedTime(l_currentTime);
                            this.m_simulationStepLimit = this.m_simulationStepLimit - s_defaultStepCount;
                        }
                    }
                }
                else
                {
                    while (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun)
                    {
                        this.m_simulatorDic[this.m_currentProjectID].Step(s_defaultStepCount);
                        Application.DoEvents();
                        double l_currentTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
                        this.m_pManager.AdvancedTime(l_currentTime);
                    }
                }
            }
            catch (Exception l_ex)
            {
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationWait;
                throw new Exception(m_resources.GetString("ErrRunSim") + " {" + l_ex.ToString() + "}");
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
                if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationWait)
                {
                    this.Initialize(true);
                    this.m_simulationTimeLimit = l_timeLimit;
                    this.m_simulationStartTime = 0.0;
                    this.m_pManager.Message(
                        Constants.messageSimulation,
                        "Start Simulator: [" + this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime() + "]"
                            + System.Environment.NewLine
                        );
                }
                else if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationSuspend)
                {
                    if (this.m_simulationTimeLimit == -1.0 || this.m_simulationTimeLimit == 0.0)
                    {
                        this.m_simulationTimeLimit = l_timeLimit;
                        this.m_simulationStartTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
                    }
                    this.m_pManager.Message(
                        Constants.messageSimulation,
                        "Restart Simulator: [" + this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime() + "]"
                            + System.Environment.NewLine
                        );
                }
                else
                {
                    throw new Exception(m_resources.GetString("ErrRunning"));
                }
                //
                // Selects the type of the simulation and Starts.
                //
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationRun;
                if (this.m_simulationTimeLimit > 0.0)
                {
                    l_startTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
                    Thread l_thread = new Thread(new ThreadStart(SimulationStartByThreadWithLimit));
                    l_thread.Start();
                    int i = 0;
                    while (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun)
                    {
                        i++;
                        if (i == 1000)
                        {
                            Thread.Sleep(1);
                            i = 0;
                        }
                        Application.DoEvents();
                        double l_currentTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
                        this.m_pManager.AdvancedTime(l_currentTime);
                        if (l_currentTime >= (this.m_simulationStartTime + this.m_simulationTimeLimit))
                        {
                            if (l_statusNum == s_simulationSuspend)
                            {
                                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationSuspend;
                            }
                            else
                            {
                                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationWait;
                            }
                            l_currentTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
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
                    while (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun)
                    {
                        if (i == 1000)
                        {
                            Thread.Sleep(1);
                            i = 0;
                        }
                        this.m_simulatorDic[this.m_currentProjectID].Step(s_defaultStepCount);
                        Application.DoEvents();
                        double l_currentTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
                        this.m_pManager.AdvancedTime(l_currentTime);
                    }
                }
            }
            catch (Exception l_ex)
            {
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationWait;
                string l_message = m_resources.GetString("ErrRunSim");
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Starts this simulation without the time limit by the thread.
        /// </summary>
        void SimulationStartByThread()
        {
            Util.InitialLanguage();
            this.m_simulatorDic[this.m_currentProjectID].Run();
        }

        /// <summary>
        /// Starts this simulation with the time limit by the thread.
        /// </summary>
        void SimulationStartByThreadWithLimit()
        {
            Util.InitialLanguage();
            this.m_simulatorDic[this.m_currentProjectID].Run(this.m_simulationTimeLimit);
        }

        /// <summary>
        /// Starts this simulation with the step limit.
        /// </summary>
        /// <param name="l_stepLimit">The step limit</param>
        public void SimulationStartKeepSetting(int l_stepLimit)
        {
            try
            {
                if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun)
                {
                    throw new Exception(m_resources.GetString("ErrRunnning"));
                }
                this.SimulationStart(l_stepLimit, s_simulationSuspend);
            }
            catch (Exception l_ex)
            {
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationWait;
                throw new Exception(m_resources.GetString("ErrRunSim") + " {" + l_ex.ToString() + "}");
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
                if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun)
                {
                    throw new Exception(m_resources.GetString("ErrRunning"));
                }
                // double l_currentSimulationTime = this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime();
                // this.SimulationStart(l_currentSimulationTime + l_timeLimit);
                /*
                this.SimulationStart(l_timeLimit);
                if (this.m_simulatorExeFlagDic[this.m_currentProjectID] == s_simulationRun)
                {
                    this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationSuspend;
                }
                 */
                this.SimulationStart(l_timeLimit, s_simulationSuspend);
            }
            catch (Exception l_ex)
            {
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationWait;
                throw new Exception(m_resources.GetString("ErrRunSim") + " {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Stops this simulation.
        /// </summary>
        public void SimulationStop()
        {
            try
            {
                if (this.m_simulatorDic[this.m_currentProjectID] == null)
                {
                    throw new Exception(m_resources.GetString("ErrFindRunSim"));
                }
                lock (this.m_simulatorDic[this.m_currentProjectID])
                {
                    this.m_simulatorDic[this.m_currentProjectID].Stop();
                }
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Reset Simulator: [" + this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime() + "]"
                        + System.Environment.NewLine
                    );
            }
            catch (Exception l_ex)
            {
                string l_message = m_resources.GetString("ErrResetSim");
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
                throw new Exception(l_message + "{" + l_ex.ToString() + "}");
            }
            finally
            {
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationWait;
            }
        }

        /// <summary>
        /// Suspends the simulation.
        /// </summary>
        public void SimulationSuspend()
        {
            try
            {
                this.m_simulatorDic[this.m_currentProjectID].Suspend();
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationSuspend;
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    "Suspend Simulator: [" + this.m_simulatorDic[this.m_currentProjectID].GetCurrentTime() + "]"
                        + System.Environment.NewLine
                    );
            }
            catch (Exception l_ex)
            {
                string l_message = m_resources.GetString("ErrSuspendSim");
                this.m_pManager.Message(
                    Constants.messageSimulation,
                    l_message + System.Environment.NewLine
                    );
                throw new Exception(l_message + " {" + l_ex.ToString() + "}");
            }
            /*
            try
            {
                if (this.m_simulatorDic[this.m_currentProjectID] == null)
                {
                    throw new Exception("Can't find the suspentable simulator.");
                }
                lock (this.m_simulatorDic[this.m_currentProjectID])
                {
                    this.m_simulatorDic[this.m_currentProjectID].Stop();
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't suspend the simulation. {" + l_ex.ToString() + "}");
            }
            finally
            {
                this.m_simulatorExeFlagDic[this.m_currentProjectID] = s_simulationSuspend;
            }
             */
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
            string l_message = null;
            try
            {
                if (l_parameterID == null || l_parameterID.Length <= 0)
                {
                    l_parameterID = this.m_currentParameterID;
                }
                l_message = "[" + l_parameterID + "][" + l_modelID + "][" + l_type + "]";
                if (l_initialList != null && l_initialList.Count > 0)
                {
                    foreach (string l_key in l_initialList.Keys)
                    {
                        if (this.m_initialCondition[this.m_currentProjectID][l_parameterID][l_modelID][l_type]
                                .ContainsKey(l_key))
                        {
                            this.m_initialCondition[this.m_currentProjectID][l_parameterID][l_modelID][l_type]
                                .Remove(l_key);
                        }
                        this.m_initialCondition[this.m_currentProjectID][l_parameterID][l_modelID][l_type][l_key]
                                = l_initialList[l_key];
                    }
                    this.m_pManager.Message(
                        Constants.messageSimulation,
                        "Update Initial Condition: " + l_message + System.Environment.NewLine
                        );
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrSetInitParam") + l_message
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
                    l_message = "[" + l_parameterID + "][" + l_stepper.modelID + "][" + l_stepper.key + "]";
                    if (!this.m_stepperDic[this.m_currentProjectID][l_parameterID].ContainsKey(l_stepper.modelID))
                    {
                        throw new Exception(m_resources.GetString("ErrFindStepper") + l_message);
                    }
                    bool l_updateFlag = false;
                    List<EcellObject> l_storedStepperList
                        = this.m_stepperDic[this.m_currentProjectID][l_parameterID][l_stepper.modelID];
                    for (int i = 0; i < l_storedStepperList.Count; i++)
                    {
                        if (l_storedStepperList[i].key.Equals(l_stepper.key))
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
                        throw new Exception(m_resources.GetString("ErrFindStepper") + l_message);
                    }
                }
                if (l_isRecorded)
                    m_aManager.AddAction(new UpdateStepperAction(l_parameterID, l_stepperList, l_oldStepperList));
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrSetStepper") + l_message + " {" + l_ex.ToString() + "}");
            }
        }

        void LookIntoSimulator(WrappedSimulator l_simulator)
        {
            List<string> entityList = new List<string>();
            entityList.Add(Constants.xpathProcess);
            entityList.Add(Constants.xpathVariable);
            foreach (string entityName in entityList)
            {
                foreach (WrappedPolymorph wpFullID in
                    this.m_simulatorDic[this.m_currentProjectID].GetEntityList(
                        entityName, Constants.delimiterPath).CastToList())
                {
                    EcellValue fullID = new EcellValue(wpFullID);
                    Console.WriteLine(entityName + ": " + fullID);
                    foreach (WrappedPolymorph wpEntityProperty in
                        this.m_simulatorDic[this.m_currentProjectID].GetEntityPropertyList(
                            entityName + Constants.delimiterColon + Constants.delimiterPath + Constants.delimiterColon +
                            fullID).CastToList())
                    {
                        string entityProperty = (new EcellValue(wpEntityProperty)).CastToString();
                        List<bool> wpAttrList
                            = this.m_simulatorDic[this.m_currentProjectID].GetEntityPropertyAttributes(
                                entityName + Constants.delimiterColon + Constants.delimiterPath + Constants.delimiterColon +
                                fullID + Constants.delimiterColon + entityProperty);
                        if (wpAttrList[1])
                        {
                            WrappedPolymorph wp
                                = this.m_simulatorDic[this.m_currentProjectID].GetEntityProperty(
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
                this.m_simulatorDic[this.m_currentProjectID].GetStepperList().CastToList())
            {
                string stepperID = (new EcellValue(wpStepperID)).CastToString();
                Console.WriteLine(Constants.xpathStepper + " " + stepperID);
                foreach (WrappedPolymorph wpStepperProperty in
                    this.m_simulatorDic[this.m_currentProjectID].GetStepperPropertyList(stepperID).CastToList())
                {
                    string stepperProperty = (new EcellValue(wpStepperProperty)).CastToString();
                    List<bool> wpAttrList
                        = this.m_simulatorDic[this.m_currentProjectID].GetStepperPropertyAttributes(
                        stepperID, stepperProperty);
                    if (wpAttrList[1])
                    {
                        WrappedPolymorph wp
                            = this.m_simulatorDic[this.m_currentProjectID].GetStepperProperty(
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
        /// Write the prefix information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        public void WritePrefix(string fileName, Encoding enc)
        {
            DateTime n = DateTime.Now;
            String timeStr = n.ToString("d");
            File.AppendAllText(fileName, "from EcellIDE import *\n", enc);
            File.AppendAllText(fileName, "import time\n", enc);
            File.AppendAllText(fileName, "import System.Threading\n\n", enc);

            File.AppendAllText(fileName, "count = 1000\n", enc);
            File.AppendAllText(fileName, "session=Session()\n", enc);
            File.AppendAllText(fileName, "session.createProject(\"" + m_currentProjectID + "\",\"" + timeStr + "\")\n\n", enc);
        }

        /// <summary>
        /// Write the postfix information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="count">step count.</param>
        /// <param name="enc">encoding(SJIS)</param>
        public void WriteSimulationForStep(string fileName, int count, Encoding enc)
        {
            File.AppendAllText(fileName, "session.step(" + count + ")\n", enc);
            File.AppendAllText(fileName, "while session.isActive():\n", enc);
            File.AppendAllText(fileName, "    System.Threading.Thread.Sleep(1000)\n", enc);
        }

        /// <summary>
        /// Write the postfix information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="time">simulation time.</param>
        /// <param name="enc">encoding(SJIS)</param>
        public void WriteSimulationForTime(string fileName, double time, Encoding enc)
        {
            File.AppendAllText(fileName, "session.run(" + time + ")\n", enc);
            File.AppendAllText(fileName, "while session.isActive():\n", enc);
            File.AppendAllText(fileName, "    System.Threading.Thread.Sleep(1000)\n", enc);
        }

        /// <summary>
        /// Write the model information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        public void WriteModelEntry(string fileName, Encoding enc, string modelName)
        {
            File.AppendAllText(fileName, "session.createModel(\"" + modelName + "\")\n\n", enc);
            File.AppendAllText(fileName, "# Stepper\n", enc);
            foreach (EcellObject stepObj in
                m_stepperDic[m_currentProjectID][m_currentParameterID][modelName])
            {
                File.AppendAllText(fileName, "stepperStub" + s_stepperCount + "=session.createStepperStub(\"" + stepObj.key + "\")\n", enc);
                File.AppendAllText(fileName, "stepperStub" + s_stepperCount + ".create(\"" + stepObj.classname + "\")\n", enc);
                s_exportStepper.Add(stepObj.key, s_stepperCount);
                s_stepperCount++;
            }
        }

        /// <summary>
        /// Write the model property in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        public void WriteModelProperty(string fileName, Encoding enc, string modelName)
        {
            File.AppendAllText(fileName, "\n# Stepper\n", enc);
            foreach (EcellObject stepObj in
                m_stepperDic[m_currentProjectID][m_currentParameterID][modelName])
            {
                int count = s_exportStepper[stepObj.key];
                foreach (EcellData d in stepObj.Value)
                {
                    if (!d.Settable) continue;
                    if (d.Value.ToString().Equals("+Åá"))
                        File.AppendAllText(fileName,
                            "stepperStub" + count + ".setProperty(\"" + d.Name + "\",\"" + "1.79769313486231E+308" + "\")\n", enc);
                    else
                        File.AppendAllText(fileName,
                            "stepperStub" + count + ".setProperty(\"" + d.Name + "\",\"" + Convert.ToDouble(d.Value.ToString()) + "\")\n", enc);
                }
            }
        }

        /// <summary>
        /// Write the system entry in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        /// <param name="sysObj">written system object.</param>
        public void WriteSystemEntry(string fileName, Encoding enc, string modelName, EcellObject sysObj)
        {
            if (sysObj == null) return;
            s_exportSystem.Add(sysObj.key, s_sysCount);
            string prefix = "";
            string data = "";
            if (sysObj.key.Equals("/"))
            {
                prefix = "";
                data = "/";
            }
            else
            {
                string[] ele = sysObj.key.Split(new char[] { '/' });
                if (ele.Length == 2)
                {
                    prefix = "/";
                    data = ele[ele.Length - 1];
                }
                else
                {
                    for (int i = 1; i < ele.Length - 1; i++)
                    {
                        prefix = prefix + "/" + ele[i];
                    }
                    data = ele[ele.Length - 1];
                }
            }
            File.AppendAllText(fileName, "systemStub" + s_sysCount + "= session.createEntityStub(\"System:" + prefix + ":" + data + "\")\n", enc);
            File.AppendAllText(fileName, "systemStub" + s_sysCount + ".create(\"System\")\n", enc);
            s_sysCount++;
        }

        /// <summary>
        /// Write the system property in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        /// <param name="sysObj">written system object.</param>
        public void WriteSystemProperty(string fileName, Encoding enc, string modelName, EcellObject sysObj)
        {
            if (sysObj == null) return;
            int count = s_exportSystem[sysObj.key];
            if (sysObj.Value == null) return;
            foreach (EcellData d in sysObj.Value)
            {
                if (!d.Settable) continue;
                File.AppendAllText(fileName,
                    "systemStub" + count + ".setProperty(\"" + d.Name + "\",\"" + d.Value.ToString() + "\")\n", enc);
            }
        }

        /// <summary>
        /// Write the logger property in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="logList">Logger list.</param>
        public void WriteLoggerProperty(string fileName, Encoding enc, List<string> logList)
        {
            string curParam = GetCurrentSimulationParameterID();
            if (curParam == null) return;
            LoggerPolicy l = GetLoggerPolicy(curParam);
            File.AppendAllText(fileName, "\n# Logger Policy\n");
            if (logList == null) return;
            foreach (string path in logList)
            {
                File.AppendAllText(fileName,
                    "logger" + s_logCount + "=session.createLoggerStub(\"" + path + "\")\n",
                    enc);
                File.AppendAllText(fileName,
                    "logger" + s_logCount + ".create()\n",
                    enc);
                File.AppendAllText(fileName,
                    "logger" + s_logCount + ".setLoggerPolicy(" +
                    l.m_reloadStepCount + "," +
                    l.m_reloadInterval + "," +
                    l.m_diskFullAction + "," +
                    l.m_maxDiskSpace + ")\n", enc);
                s_logCount++;
            }

        }

        /// <summary>
        /// Write the logger property to save the log in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="saveList">save property list.</param>
        public void WriteLoggerSaveEntry(string fileName, Encoding enc, List<SaveLoggerProperty> saveList)
        {
            File.AppendAllText(fileName, "\n# Save logging\n", enc);
            if (saveList == null) return;
            foreach (SaveLoggerProperty s in saveList)
            {
                File.AppendAllText(
                    fileName,
                    "session.saveLoggerData(\"" + s.FullPath + "\",\"" + s.DirName + "\"," +
                    s.Start + "," + s.End + ")",
                    enc);
            }
        }


        /// <summary>
        /// Write the component entry of model in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="sysObj">system object.</param>
        public void WriteComponentEntry(string fileName, Encoding enc, EcellObject sysObj)
        {
            File.AppendAllText(fileName, "\n# Variable\n", enc);
            if (sysObj.Children == null) return;
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.type.Equals("Variable")) continue;
                string[] names = obj.key.Split(new char[] { ':' });
                string name = names[names.Length - 1];

                File.AppendAllText(fileName,
                    "variableStub" + s_varCount + name + "= session.createEntityStub(\"Variable:" + obj.key + "\")\n",
                    enc);
                File.AppendAllText(fileName,
                    "variableStub" + s_varCount + name + ".create(\"Variable\")\n", enc);
                s_exportVariable.Add(obj.key, s_varCount);
                s_varCount++;
            }

            File.AppendAllText(fileName, "\n# Process\n", enc);
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.type.Equals("Process")) continue;
                string[] names = obj.key.Split(new char[] { ':' });
                string name = names[names.Length - 1];

                File.AppendAllText(fileName,
                    "processStub" + s_proCount + name + "= session.createEntityStub(\"Process:" + obj.key + "\")\n",
                    enc);
                File.AppendAllText(fileName,
                    "processStub" + s_proCount + name + ".create(\"" + obj.classname + "\")\n", enc);
                s_exportProcess.Add(obj.key, s_proCount);
                s_proCount++;
            }
        }

        /// <summary>
        /// Write the component property of model in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="sysObj">system object.</param>
        public void WriteComponentProperty(string fileName, Encoding enc, EcellObject sysObj)
        {
            File.AppendAllText(fileName, "\n# Variable\n", enc);
            if (sysObj.Children == null) return;
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.type.Equals("Variable")) continue;
                string[] names = obj.key.Split(new char[] { ':' });
                string name = names[names.Length - 1];
                int count = s_exportVariable[obj.key];

                foreach (EcellData d in obj.Value)
                {
                    if (!d.Settable) continue;
                    File.AppendAllText(fileName,
                        "variableStub" + count + name + ".setProperty(\"" + d.Name + "\",\"" + d.Value.ToString() + "\")\n", enc);
                }
            }

            File.AppendAllText(fileName, "\n# Process\n", enc);
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.type.Equals("Process")) continue;
                string[] names = obj.key.Split(new char[] { ':' });
                string name = names[names.Length - 1];
                int count = s_exportProcess[obj.key];

                foreach (EcellData d in obj.Value)
                {
                    if (!d.Settable) continue;
                    File.AppendAllText(fileName,
                        "processStub" + count + name + ".setProperty(\"" + d.Name + "\",\"" + d.Value.ToString().Replace("\"", "\\\"") + "\")\n", enc);
                }
            }
        }

        /// <summary>
        /// Create a new WrappedSimulator instance.
        /// </summary>
        protected WrappedSimulator CreateSimulatorInstance()
        {
            return new WrappedSimulator(Util.GetDMDirs(m_currentProjectPath));
        }

    }
}