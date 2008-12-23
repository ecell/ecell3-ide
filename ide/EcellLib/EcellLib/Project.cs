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
        /// The list of the DM with Object type (stepper, system, process, variable).
        /// </summary>
        private Dictionary<string, List<string>> m_dmDic = null;
        /// <summary>
        /// The ModelList of this project
        /// </summary>
        private List<EcellModel> m_modelList = null;
        /// <summary>
        /// The ModelFileList of this project
        /// </summary>
        private Dictionary<string, string> m_modelFileList = new Dictionary<string,string>();
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
        private Dictionary<string, Dictionary<string, List<EcellObject>>> m_stepperDic = null;

        /// <summary>
        /// The executed flag of Simulator.
        /// </summary>
        private SimulationStatus m_simulationStatus = 0;
        /// <summary>
        /// 
        /// </summary>
        private int m_processNumbering = 0;
        /// <summary>
        /// 
        /// </summary>
        private int m_systemNumbering = 0;
        /// <summary>
        /// 
        /// </summary>
        private int m_variableNumbering = 0;
        /// <summary>
        /// 
        /// </summary>
        private int m_textNumbering = 0;

        #endregion

        #region Accessor
        /// <summary>
        /// The ProjectInfo
        /// </summary>
        public ProjectInfo Info
        {
            get { return m_info; }
            set { m_info = value; }
        }
        /// <summary>
        /// The list of the DM filepath.
        /// </summary>
        public Dictionary<string, List<string>> DmDic
        {
            get { return m_dmDic; }
            set { m_dmDic = value; }
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
                    stepperList.AddRange(m_dmDic[Constants.xpathStepper]);
                stepperList.Sort();
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
            set { m_modelList = value; }
        }

        /// <summary>
        /// Current Model
        /// </summary>
        public EcellObject Model
        {
            get { return m_modelList[0]; }
        }

        /// <summary>
        /// The dictionary of the "Systems" with the model ID 
        /// </summary>
        public Dictionary<string, List<EcellObject>> SystemDic
        {
            get { return m_systemDic; }
            set { m_systemDic = value; }
        }

        public List<EcellObject> SystemList
        {
            get { return m_systemDic[m_modelList[0].ModelID]; }
        }

        /// <summary>
        /// The List of the Model
        /// </summary>
        public Dictionary<string, string> ModelFileDic
        {
            get { return m_modelFileList; }
            set { m_modelFileList = value; }
        }

        /// <summary>
        /// The Simulator of this project.
        /// </summary>
        public WrappedSimulator Simulator
        {
            get { return m_simulator; }
            set { m_simulator = value; }
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
            set { m_initialCondition = value; }
        }

        /// <summary>
        /// The dictionary of the "LoggerPolicy" with the parameter ID
        /// </summary>
        public Dictionary<string, LoggerPolicy> LoggerPolicyDic
        {
            get { return m_loggerPolicyDic; }
            set { m_loggerPolicyDic = value; }
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
        public Dictionary<string, Dictionary<string, List<EcellObject>>> StepperDic
        {
            get { return m_stepperDic; }
            set { m_stepperDic = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Creates the new "Project" instance with ProjectInfo.
        /// </summary>
        public Project(ProjectInfo info)
        {
            m_info = info;
            SetDMList();
            m_loggerPolicyDic = new Dictionary<string, LoggerPolicy>();
            m_stepperDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
            m_modelList = new List<EcellModel>();
            m_systemDic = new Dictionary<string, List<EcellObject>>();
            m_simulator = CreateSimulatorInstance();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initialize objects.
        /// </summary>
        public void Initialize(string modelID)
        {
            // Checks the current parameter ID.
            if (string.IsNullOrEmpty(m_info.SimulationParam))
                m_info.SimulationParam = Constants.defaultSimParam;

            m_initialCondition = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            m_initialCondition[m_info.SimulationParam] = new Dictionary<string, Dictionary<string, double>>();
            m_initialCondition[m_info.SimulationParam][modelID] = new Dictionary<string, double>();
        }

        /// <summary>
        /// Sets the list of the DM.
        /// </summary>
        public void SetDMList()
        {
            // Initialize
            Dictionary<string, List<string>> dmDic = Util.GetDmDic(m_info.ProjectPath);
            this.m_dmDic = dmDic;
        }

        /// <summary>
        /// Create a new WrappedSimulator instance.
        /// </summary>
        internal WrappedSimulator CreateSimulatorInstance()
        {
            string[] dmpath = Util.GetDMDirs(m_info.ProjectPath);
            Trace.WriteLine("Creating simulator (dmpath=" + string.Join(";", dmpath) + ")");
            return new WrappedSimulator(dmpath);
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

        #region Saver
        public void Save()
        {
            m_info.ProjectPath = Path.Combine(Util.GetBaseDir(), m_info.Name);
            m_info.Save();

            //List<string> modelList = GetSavableModel();
            //List<string> paramList = GetSavableSimulationParameter();
            //List<string> logList = GetSavableSimulationResult();

            //foreach (string name in modelList)
            //{
            //    SaveModel(name);
            //}
            //foreach (string name in paramList)
            //{
            //    SaveSimulationParameter(name);
            //}
            //SaveSimulationResult();
        }

        /// <summary>
        /// Returns the savable model ID.
        /// </summary>
        /// <returns>The savable model ID</returns>
        internal List<string> GetSavableModel()
        {
            if (m_modelList.Count <= 0)
                return null;

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
        /// Get the temporary id in projects.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="type">object type.</param>
        /// <param name="systemID">ID of parent system.</param>
        /// <returns>the temporary id.</returns>
        public string GetTemporaryID(string modelID, string type, string systemID)
        {
            // Set Preface
            string pref = "";
            int i = 0;
            if (type.Equals(EcellObject.PROCESS))
            {
                pref = systemID + ":P";
                i = m_processNumbering;
                m_processNumbering++;
            }
            else if (type.Equals(EcellObject.VARIABLE))
            {
                pref = systemID + ":V";
                i = m_variableNumbering;
                m_variableNumbering++;
            }
            else if (type.Equals(EcellObject.TEXT))
            {
                pref = systemID + ":Text";
                i = m_textNumbering;
                m_textNumbering++;
            }
            else
            {
                if (systemID == null || systemID == "/")
                    systemID = "";
                pref = systemID + "/S";
                i = m_systemNumbering;
                m_systemNumbering++;
            }
            while (GetEcellObject(modelID, type, pref + i.ToString()) != null)
            {
                i++;
            }
            return pref + i.ToString();
        }

        /// <summary>
        /// Get EcellObject of this project.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public EcellObject GetEcellObject(string model, string type, string key)
        {
            if (type.Equals(EcellObject.MODEL))
                return m_modelList[0];
            if (type.Equals(EcellObject.SYSTEM))
                return GetSystem(model, key);
            else 
                return GetEntity(model, key, type);
        }

        /// <summary>
        /// Get System.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public EcellObject GetSystem(string model, string key)
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
            return system;
        }

        /// <summary>
        /// Get Entity.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public EcellObject GetEntity(string model, string key, string type)
        {
            EcellObject system = GetSystem(model, Util.GetSuperSystemPath(key));
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
            return entity;
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
            AddSimulationParameter(system);
            foreach (EcellObject child in system.Children)
            {
                AddSimulationParameter(child);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(EcellObject entity)
        {
            EcellObject system = GetSystem(entity.ModelID, entity.ParentSystemID);
            system.Children.Add(entity);
            AddSimulationParameter(entity);
        }
        #endregion

        #region Delete Object
        /// <summary>
        /// Delete System.
        /// </summary>
        /// <param name="system"></param>
        public void DeleteSystem(EcellObject system)
        {
            m_systemDic[system.ModelID].Remove(system);
            // Delete Simulation parameter.
            foreach (string keyParamID in m_initialCondition.Keys)
            {
                foreach (string delModel in m_initialCondition[keyParamID].Keys)
                {
                    Debug.Assert(system.Type == Constants.xpathSystem);
                    String delKey = system.Key;
                    List<String> delKeyList = new List<string>();
                    foreach (String entKey in m_initialCondition[keyParamID][delModel].Keys)
                    {
                        if (entKey.Contains(delKey))
                            delKeyList.Add(entKey);
                    }
                    foreach (String entKey in delKeyList)
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

        #region SimulationParameter
        /// <summary>
        /// Add SimulationParameter
        /// </summary>
        /// <param name="eo"></param>
        public void AddSimulationParameter(EcellObject eo)
        {
            foreach (string keyParameterID in m_initialCondition.Keys)
            {
                Dictionary<string, double> initialCondition = m_initialCondition[keyParameterID][eo.ModelID];
                foreach (EcellData data in eo.Value)
                {
                    if (!data.IsInitialized())
                        continue;

                    double value = 0;
                    if (data.Value.IsDouble)
                        value = (double)data.Value;
                    else if (data.Value.IsInt)
                        value = (double)data.Value;

                    initialCondition[data.EntityPath] = value;
                }
            }
        }

        /// <summary>
        /// Delete SimulationParameter
        /// </summary>
        /// <param name="eo"></param>
        public void DeleteSimulationParameter(EcellObject eo)
        {

        }
        #endregion

        #endregion

    }
}
