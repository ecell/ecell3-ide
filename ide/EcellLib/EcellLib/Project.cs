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
using System.Collections;
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
                WrappedSimulator sim =  new WrappedSimulator(Util.GetDMDirs(m_info.ProjectPath));
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
            get { return m_systemDic[""];}
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
            m_loggerPolicyDic = new Dictionary<string, LoggerPolicy>();
            m_stepperDic = new Dictionary<string, Dictionary<string, List<EcellObject>>>();
            m_modelList = new List<EcellModel>();
            m_systemDic = new Dictionary<string, List<EcellObject>>();
            ResetSimulator();
        }
        #endregion

        #region EventHandler
        private void SimulationEventHandler(object sender, EventArgs e)
        {
            if (m_simulationStatus == SimulationStatus.Suspended
                || m_simulationStatus == SimulationStatus.Wait)
            {
                ((WrappedSimulator)sender).Stop();
            }
        }
        #endregion

        #region Methods
        public void ResetSimulator()
        {
            SetDMList();
            m_simulator = new WrappedSimulator(Util.GetDMDirs(m_info.ProjectPath));
            m_simulator.EventHandler = new EventHandler(SimulationEventHandler);
        }

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

        #endregion

    }

    /// <summary>
    /// DataStorer
    /// </summary>
    internal class DataStorer
    {
        #region DataStored
        /// <summary>
        /// Stores the "EcellObject"
        /// </summary>
        /// <param name="simulator">The "simulator"</param>
        /// <param name="ecellObject">The stored "EcellObject"</param>
        /// <param name="initialCondition">The initial condition.</param>
        internal static void DataStored(
                WrappedSimulator simulator,
                EcellObject ecellObject,
                Dictionary<string, double> initialCondition)
        {
            if (ecellObject.Type.Equals(Constants.xpathStepper))
            {
                DataStored4Stepper(simulator, ecellObject);
            }
            else if (ecellObject.Type.Equals(Constants.xpathSystem))
            {
                DataStored4System(
                        simulator,
                        ecellObject,
                        initialCondition);
            }
            else if (ecellObject.Type.Equals(Constants.xpathProcess))
            {
                DataStored4Process(
                        simulator,
                        ecellObject,
                        initialCondition);
            }
            else if (ecellObject.Type.Equals(Constants.xpathVariable))
            {
                DataStored4Variable(
                        simulator,
                        ecellObject,
                        initialCondition);
            }
            //
            // 4 children
            //
            if (ecellObject.Children != null)
            {
                foreach (EcellObject childEcellObject in ecellObject.Children)
                    DataStored(simulator, childEcellObject, initialCondition);
            }
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Process".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="ecellObject">The stored "Process"</param>
        /// <param name="initialCondition">The initial condition.</param>
        internal static void DataStored4Process(
                WrappedSimulator simulator,
                EcellObject ecellObject,
                Dictionary<string, double> initialCondition)
        {
            string key = Constants.xpathProcess + Constants.delimiterColon + ecellObject.Key;
            IList<string> wrappedPolymorph = null;
            try
            {
                wrappedPolymorph = simulator.GetEntityPropertyList(key);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return;
            }
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> processEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> storedEcellDataDic
                    = new Dictionary<string, EcellData>();
            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData storedEcellData in ecellObject.Value)
                {
                    storedEcellDataDic[storedEcellData.Name] = storedEcellData;
                    processEcellDataList.Add(storedEcellData);
                    if (!storedEcellData.Settable)
                        continue;
                    if (!storedEcellData.Value.IsDouble)
                        continue;
                    try
                    {
                        initialCondition[storedEcellData.EntityPath] = (double)storedEcellData.Value;
                    }
                    catch (InvalidCastException)
                    {
                        // non-numeric value
                    }
                }
            }
            //
            // Stores the "EcellData"
            //
            foreach (string name in wrappedPolymorph)
            {
                string entityPath = Util.BuildFullPN(key, name);

                PropertyAttributes flag = simulator.GetEntityPropertyAttributes(entityPath);
                if (!flag.Gettable)
                {
                    continue;
                }
                EcellValue value = null;

                if (name == Constants.xpathVRL)
                {
                    // Won't restore the variable reference list from the simulator's corresponding
                    // object.
                    if (storedEcellDataDic.ContainsKey(name))
                        value = storedEcellDataDic[name].Value;
                    else
                        value = new EcellValue(new List<object>());
                }
                else if (name == Constants.xpathActivity && name == Constants.xpathMolarActivity)
                {
                    value = new EcellValue(0.0);
                }
                else
                {
                    try
                    {
                        value = new EcellValue(simulator.GetEntityProperty(entityPath));
                    }
                    catch (WrappedException ex)
                    {
                        Trace.WriteLine(ex);
                        value = new EcellValue("");
                    }
                }

                EcellData ecellData = CreateEcellData(name, value, entityPath, flag.Settable, flag.Gettable, flag.Loadable, flag.Savable);
                if (ecellData.Value != null)
                {
                    if (!ecellData.Value.IsDouble)
                        continue;
                    ecellData.Logable = ecellData.Settable == false || ecellData.Saveable == false;
                    if (ecellData.Settable)
                        continue;
                    try
                    {
                        initialCondition[ecellData.EntityPath] = (double)ecellData.Value;
                    }
                    catch
                    {
                        // non-numeric value
                    }
                }
                if (storedEcellDataDic.ContainsKey(name))
                {
                    ecellData.Logged = storedEcellDataDic[name].Logged;
                    processEcellDataList.Remove(storedEcellDataDic[name]);
                }
                processEcellDataList.Add(ecellData);
            }
            ecellObject.SetEcellDatas(processEcellDataList);
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Stepper".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="ecellObject">The stored "Stepper"</param>
        internal static void DataStored4Stepper(
                WrappedSimulator simulator, EcellObject ecellObject)
        {
            List<EcellData> stepperEcellDataList = new List<EcellData>();
            IList<string> wrappedPolymorph = null;
            //
            // Property List
            //
            try
            {
                wrappedPolymorph = simulator.GetStepperPropertyList(ecellObject.Key);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return;
            }
            //
            // Sets the class name.
            //
            if (string.IsNullOrEmpty(ecellObject.Classname))
            {
                ecellObject.Classname = simulator.GetStepperClassName(ecellObject.Key);
            }
            //
            // Checks the stored "EcellData"
            //
            Dictionary<string, EcellData> storedEcellDataDic = new Dictionary<string, EcellData>();
            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData storedEcellData in ecellObject.Value)
                {
                    storedEcellDataDic[storedEcellData.Name] = storedEcellData;
                    stepperEcellDataList.Add(storedEcellData);
                }
            }
            //
            // Stores the "EcellData"
            //
            foreach (string name in wrappedPolymorph)
            {
                PropertyAttributes flags = simulator.GetStepperPropertyAttributes(ecellObject.Key, name);
                if (!flags.Gettable)
                {
                    continue;
                }
                EcellValue value = null;
                try
                {
                    value = new EcellValue(simulator.GetStepperProperty(ecellObject.Key, name));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    value = new EcellValue("");
                }
                EcellData ecellData = CreateEcellData(name, value, name, flags.Settable, flags.Gettable, flags.Loadable, flags.Savable);
                if (storedEcellDataDic.ContainsKey(name))
                {
                    if (value.IsString && ((string)value).Equals(""))
                    {
                        continue;
                    }
                    else
                    {
                        stepperEcellDataList.Remove(storedEcellDataDic[name]);
                    }
                }
                stepperEcellDataList.Add(ecellData);
            }
            ecellObject.SetEcellDatas(stepperEcellDataList);
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "System".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="ecellObject">The stored "System"</param>
        /// <param name="initialCondition">The initial condition.</param>
        internal static void DataStored4System(
                WrappedSimulator simulator,
                EcellObject ecellObject,
                Dictionary<string, double> initialCondition)
        {
            // Creates an entityPath.
            string parentPath = ecellObject.ParentSystemID;
            string childPath = ecellObject.LocalID;
            string key = Constants.xpathSystem + Constants.delimiterColon +
                parentPath + Constants.delimiterColon +
                childPath;
            // Property List
            IList<string> wrappedPolymorph = simulator.GetEntityPropertyList(key);
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> systemEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> storedEcellDataDic
                    = new Dictionary<string, EcellData>();
            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData storedEcellData in ecellObject.Value)
                {
                    storedEcellDataDic[storedEcellData.Name] = storedEcellData;
                    systemEcellDataList.Add(storedEcellData);
                    if (!storedEcellData.Settable)
                        continue;
                    storedEcellData.Logable = storedEcellData.Value.IsDouble;
                    if (!storedEcellData.Logable)
                        continue;

                    try
                    {
                        initialCondition[storedEcellData.EntityPath] = (double)storedEcellData.Value;
                    }
                    catch (InvalidCastException)
                    {
                        // non-numeric value
                    }
                }
            }
            foreach (string name in wrappedPolymorph)
            {
                string entityPath = key + Constants.delimiterColon + name;
                PropertyAttributes flags = simulator.GetEntityPropertyAttributes(entityPath);

                if (!flags.Gettable)
                {
                    continue;
                }

                object value = null;
                if (name.Equals(Constants.xpathSize))
                {
                    value = 0.0;
                }
                else
                {
                    try
                    {
                        value = simulator.GetEntityProperty(entityPath);
                    }
                    catch (WrappedException ex)
                    {
                        Trace.WriteLine(ex);
                        if (storedEcellDataDic.ContainsKey(name))
                        {
                            IEnumerable val = storedEcellDataDic[name].Value as IEnumerable;
                            object firstItem = null;
                            {
                                IEnumerator i = val.GetEnumerator();
                                if (i.MoveNext())
                                    firstItem = i.Current;
                            }
                            if (firstItem is IEnumerable)
                            {
                                value = val;
                            }
                            else
                            {
                                value = firstItem;
                            }
                        }
                        else
                        {
                            value = "";
                        }
                    }
                }

                EcellData ecellData = CreateEcellData(name, new EcellValue(value), entityPath, flags.Settable, flags.Gettable, flags.Loadable, flags.Savable);
                if (ecellData.Value != null)
                {
                    if (!ecellData.Settable)
                        continue;
                    ecellData.Logable = ecellData.Value.IsDouble;
                    if (!ecellData.Logable)
                        continue;
                    try
                    {
                        initialCondition[ecellData.EntityPath] = (double)ecellData.Value;
                    }
                    catch (InvalidCastException)
                    {
                        // non-numeric value
                    }
                }
                if (storedEcellDataDic.ContainsKey(name))
                {
                    ecellData.Logged = storedEcellDataDic[name].Logged;
                    systemEcellDataList.Remove(storedEcellDataDic[name]);
                }
                systemEcellDataList.Add(ecellData);
            }

            ecellObject.SetEcellDatas(systemEcellDataList);
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Variable".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="ecellObject">The stored "Variable"</param>
        /// <param name="initialCondition">The initial condition.</param>
        internal static void DataStored4Variable(
                WrappedSimulator simulator,
                EcellObject ecellObject,
                Dictionary<string, double> initialCondition)
        {
            string key = Constants.xpathVariable + Constants.delimiterColon + ecellObject.Key;
            IList<string> wrappedPolymorph = simulator.GetEntityPropertyList(key);
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> variableEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> storedEcellDataDic
                    = new Dictionary<string, EcellData>();
            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData storedEcellData in ecellObject.Value)
                {
                    storedEcellDataDic[storedEcellData.Name] = storedEcellData;
                    variableEcellDataList.Add(storedEcellData);

                    if (!storedEcellData.Settable)
                        continue;
                    storedEcellData.Logable = storedEcellData.Value.IsDouble;
                    if (!storedEcellData.Logable)
                        continue;

                    try
                    {
                        initialCondition[storedEcellData.EntityPath] = (double)storedEcellData.Value;
                    }
                    catch (InvalidCastException)
                    {
                        // non-numeric value
                    }
                }
            }
            foreach (string name in wrappedPolymorph)
            {
                string entityPath = key + Constants.delimiterColon + name;
                PropertyAttributes flags = simulator.GetEntityPropertyAttributes(entityPath);
                if (!flags.Gettable)
                {
                    continue;
                }
                object value = null;
                try
                {
                    value = simulator.GetEntityProperty(entityPath);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    IEnumerable val = (IEnumerable)storedEcellDataDic[name].Value;
                    object firstItem = null;
                    {
                        IEnumerator i = val.GetEnumerator();
                        if (i.MoveNext())
                            firstItem = i.Current;
                    }
                    if (storedEcellDataDic.ContainsKey(name))
                    {
                        if (val is IEnumerable)
                        {
                            value = val;
                        }
                        else
                        {
                            value = firstItem; 
                        }
                    }
                    else if (name.Equals(Constants.xpathMolarConc) || name.Equals(Constants.xpathNumberConc))
                    {
                        value = 0.0;
                    }
                    else
                    {
                        value = "";
                    }
                }
                EcellData ecellData = CreateEcellData(name, new EcellValue(value), entityPath, flags.Settable, flags.Gettable, flags.Loadable, flags.Savable);
                if (ecellData.Value != null)
                {
                    if (!ecellData.Settable)
                        continue;
                    ecellData.Logable = ecellData.Value.IsDouble;
                    if (!ecellData.Logable)
                        continue;

                    try
                    {
                        initialCondition[ecellData.EntityPath] = (double)ecellData.Value;
                    }
                    catch (InvalidCastException)
                    {
                        // non-numeric value
                    }
                }
                if (storedEcellDataDic.ContainsKey(name))
                {
                    ecellData.Logged = storedEcellDataDic[name].Logged;
                    variableEcellDataList.Remove(storedEcellDataDic[name]);
                }
                variableEcellDataList.Add(ecellData);
            }
            ecellObject.SetEcellDatas(variableEcellDataList);
        }

        /// <summary>
        /// Create new EcellData.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="entityPath"></param>
        /// <returns></returns>
        private static EcellData CreateEcellData(string name, EcellValue value, string entityPath, bool settable, bool gettable, bool loadable, bool savable)
        {
            EcellData data = new EcellData(name, value, entityPath);
            data.Settable = settable;
            data.Gettable = gettable;
            data.Loadable = loadable;
            data.Saveable = savable;
            return data;
        }

        #endregion

    }
}
