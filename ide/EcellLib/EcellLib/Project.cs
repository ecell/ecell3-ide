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
        private string m_prjPath;
        /// <summary>
        /// File Path
        /// </summary>
        private string m_filePath;
        /// <summary>
        /// The comment
        /// </summary>
        private string m_comment;
        /// <summary>
        /// The project name
        /// </summary>
        private string m_prjName;
        /// <summary>
        /// The update time
        /// </summary>
        private string m_updateTime;
        /// <summary>
        /// The simulation parameter.
        /// </summary>
        private string m_simParam;

        /// <summary>
        /// The list of the DM
        /// </summary>
        private Dictionary<string, List<string>> m_dmDic = null;
        /// <summary>
        /// The ModelList of this project
        /// </summary>
        private List<EcellObject> m_modelList = null;
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

        #region Constructor
        /// <summary>
        /// Creates the new "Project" instance with no argument.
        /// </summary>
        public Project()
        {
            SetParams(Constants.defaultPrjID, Constants.defaultComment, "", Constants.defaultSimParam);
        }

        /// <summary>
        /// Creates the new "Project" instance with initialized arguments.
        /// </summary>
        /// <param name="prjName">The project name</param>
        /// <param name="comment">The comment</param>
        /// <param name="time">The update time</param>
        public Project(string prjName, string comment, string time)
        {
            SetParams(prjName, comment, time, Constants.defaultSimParam);
        }

        /// <summary>
        /// Creates the new "Project" instance with initialized arguments.
        /// </summary>
        /// <param name="prjName">The project name</param>
        /// <param name="comment">The comment</param>
        /// <param name="time">The update time</param>
        /// <param name="simParam">The name of simulation parameter.</param>
        public Project(string prjName, string comment, string time, string simParam)
        {
            SetParams(prjName, comment, time, simParam);
        }

        /// <summary>
        /// Set Project parameters.
        /// </summary>
        /// <param name="prjName"></param>
        /// <param name="comment"></param>
        /// <param name="time"></param>
        /// <param name="simParam"></param>
        private void SetParams(string prjName, string comment, string time, string simParam)
        {
            this.Name = prjName;
            this.Comment = comment;
            this.UpdateTime = time;
            this.SimulationParam = simParam;
            this.ProjectPath = Path.Combine(Util.GetBaseDir(), this.Name);
        }
        #endregion

        #region Accessor
        /// <summary>
        /// get/set the project name
        /// </summary>
        public string Name
        {
            get { return m_prjName; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    this.m_prjName = Constants.defaultPrjID;
                else
                    this.m_prjName = value;
            }
        }

        /// <summary>
        /// get/set the comment
        /// </summary>
        public string Comment
        {
            get { return m_comment; }
            set { this.m_comment = value; }
        }

        /// <summary>
        /// get/set the update time
        /// </summary>
        public string UpdateTime
        {
            get { return m_updateTime; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.m_updateTime = DateTime.Now.ToString();
                else
                    this.m_updateTime = value;
            }
        }

        /// <summary>
        /// get/set the simulation parameter.
        /// </summary>
        public string SimulationParam
        {
            get { return m_simParam; }
            set { this.m_simParam = value; }
        }

        /// <summary>
        /// get/set the filePath
        /// </summary>
        public string FilePath
        {
            get { return m_filePath; }
            set
            {
                this.m_filePath = value;
            }
        }

        /// <summary>
        /// get/set the ProjectPath
        /// </summary>
        public string ProjectPath
        {
            get { return m_prjPath; }
            set { this.m_prjPath = value; }
        }

        /// <summary>
        /// The list of the DM
        /// </summary>
        public Dictionary<string, List<string>> DmDic
        {
            get { return m_dmDic; }
            set { m_dmDic = value; }
        }

        /// <summary>
        /// The List of the Model
        /// </summary>
        public List<EcellObject> ModelList
        {
            get { return m_modelList; }
            set { m_modelList = value; }
        }

        /// <summary>
        /// The dictionary of the "System" with the model ID 
        /// </summary>
        public Dictionary<string, List<EcellObject>> SystemDic
        {
            get { return m_systemDic; }
            set { m_systemDic = value; }
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
        /// The dictionary of the "Stepper" with the parameter ID and the model ID
        /// </summary>
        public Dictionary<string, Dictionary<string, List<EcellObject>>> StepperDic
        {
            get { return m_stepperDic; }
            set { m_stepperDic = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Save project.
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            ProjectSaver.SaveProject(this, filePath);
        }

        /// <summary>
        /// Initialize objects.
        /// </summary>
        public void Initialize(string modelID)
        {
            // Checks the current parameter ID.
            if (string.IsNullOrEmpty(m_simParam))
                m_simParam = Constants.defaultSimParam;

            m_initialCondition = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            m_initialCondition[m_simParam] = new Dictionary<string, Dictionary<string, double>>();
            m_initialCondition[m_simParam][modelID] = new Dictionary<string, double>();
        }

        /// <summary>
        /// Sets the list of the DM.
        /// </summary>
        public void SetDMList()
        {
            // Initialize
            Dictionary<string, List<string>> dmDic = Util.GetDmDic(m_prjPath);
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
    /// ProjectLoader
    /// </summary>
    public class ProjectLoader
    {
        /// <summary>
        /// Load Project from setting file.
        /// </summary>
        /// <param name="filepath">this function supports project.xml, project.info, and *.eml</param>
        /// <returns></returns>
        public static Project LoadProject(string filepath)
        {
            Project project = null;
            string ext = Path.GetExtension(filepath);
            if (filepath.EndsWith(Constants.fileProjectXML))
                project = LoadProjectFromXML(filepath);
            else if (filepath.EndsWith(Constants.fileProject))
                project = LoadProjectFromInfo(filepath);
            else if (ext.Equals(Constants.FileExtEML))
                project = LoadProjectFromEml(filepath);
            else
                throw new Exception("Unknown file type");
            project.FilePath = filepath;
            project.ProjectPath = Path.GetDirectoryName(filepath);
            return project;
        }

        /// <summary>
        /// Get Project from XML file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static Project LoadProjectFromXML(string filepath)
        {
            Project project = null;
            string dirPathName = Path.GetDirectoryName(filepath);
            string prjName = Path.GetFileName(dirPathName);
            string comment = "";
            string time = "";
            string param = "";

            try
            {
                // Load XML file
                XmlDocument xmlD = new XmlDocument();
                xmlD.Load(filepath);

                XmlNode settings = null;
                foreach (XmlNode node in xmlD.ChildNodes)
                {
                    if (node.Name.Equals(Constants.xPathEcellProject))
                        settings = node;
                }
                if (settings == null)
                    return null;

                // Load settings.
                foreach (XmlNode setting in settings.ChildNodes)
                {
                    switch (setting.Name)
                    {
                        // Project
                        case Constants.xpathProject:
                            prjName = setting.InnerText;
                            break;
                        // Date
                        case Constants.textDate:
                            time = setting.InnerText;
                            break;
                        // Comment
                        case Constants.textComment:
                            comment = setting.InnerText;
                            break;
                        // SimulationParameter
                        case Constants.textParameter:
                            param = setting.InnerText;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MessageResources.ErrLoadPrj, ex);
            }
            project = new Project(prjName, comment, time, param);
            return project;
        }

        /// <summary>
        /// Get Project from Info file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static Project LoadProjectFromInfo(string filepath)
        {
            Project project = null;
            string line = "";
            string comment = "";
            string simParam = "";
            string time = File.GetLastWriteTime(filepath).ToString();

            string dirPathName = Path.GetDirectoryName(filepath);
            string prjName = Path.GetFileName(dirPathName);
            TextReader reader = new StreamReader(filepath);
            while ((line = reader.ReadLine()) != null)
            {
                if (line.IndexOf(Constants.textComment) == 0)
                {
                    if (line.IndexOf(Constants.delimiterEqual) != -1)
                    {
                        comment = line.Split(Constants.delimiterEqual.ToCharArray())[1].Trim();
                    }
                    else
                    {
                        comment = line.Substring(line.IndexOf(Constants.textComment));
                    }
                }
                else if (line.IndexOf(Constants.textParameter) == 0)
                {
                    simParam = line;
                }
                else if (!comment.Equals(""))
                {
                    comment = comment + "\n" + line;
                }
                else if (line.IndexOf(Constants.xpathProject) == 0)
                {
                    if (line.IndexOf(Constants.delimiterEqual) != -1)
                    {
                        prjName = line.Split(Constants.delimiterEqual.ToCharArray())[1].Trim();
                    }
                    else
                    {
                        prjName = line.Substring(line.IndexOf(Constants.textComment));
                    }
                }
            }
            reader.Close();
            project = new Project(prjName, comment, time, simParam);
            return project;
        }

        /// <summary>
        /// Get Project from Eml file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static Project LoadProjectFromEml(string filepath)
        {
            Project project = null;
            string name = Path.GetFileNameWithoutExtension(filepath);
            string comment = "";
            string time = File.GetLastWriteTime(filepath).ToString();
            project = new Project(name, comment, time);
            return project;
        }
    }

    /// <summary>
    /// ProjectSaver
    /// </summary>
    public class ProjectSaver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="filepath"></param>
        public static void SaveProject(Project project, string filepath)
        {
            string message = "[" + project.Name + "]";
            string saveDir = GetSaveDir(filepath);
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            string projectInfo = Path.Combine(saveDir, Constants.fileProject);
            string projectXML = Path.Combine(saveDir, Constants.fileProjectXML);
            try
            {
                SaveProjectINFO(project, projectInfo);
                SaveProjectXML(project, projectXML);

            }
            catch (Exception ex)
            {
                throw new Exception(message + " {" + ex.ToString() + "}");
            }
        }

        /// <summary>
        /// GetSaveDir
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static string GetSaveDir(string filepath)
        {
            if (Directory.Exists(filepath))
                return filepath;

            string ext = Path.GetExtension(filepath);
            if (ext.Equals(Constants.FileExtINFO) || ext.Equals(Constants.FileExtXML))
                return Path.GetDirectoryName(filepath);
            else if (ext.Equals(Constants.FileExtEML))
                return Path.Combine(Util.GetBaseDir(), Path.GetFileNameWithoutExtension(filepath));
            else
                return filepath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="filePath"></param>
        public static void SaveProjectINFO(Project project, string filePath)
        {
            StreamWriter writer = null;
            string sepalator = Constants.delimiterSpace + Constants.delimiterEqual + Constants.delimiterSpace;
            try
            {
                writer = new StreamWriter(filePath, false, Encoding.UTF8);
                writer.WriteLine(Constants.xpathProject + sepalator + project.Name);
                writer.WriteLine(Constants.textComment + sepalator + project.Comment);
                writer.WriteLine(Constants.textParameter + sepalator + project.SimulationParam);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// SaveProjectXML
        /// </summary>
        /// <param name="project"></param>
        /// <param name="filePath"></param>
        public static void SaveProjectXML(Project project, string filePath)
        {
            XmlTextWriter xmlOut = null;
            try
            {
                // Create xml file
                xmlOut = new XmlTextWriter(filePath, Encoding.UTF8);

                // Use indenting for readability
                xmlOut.Formatting = Formatting.Indented;
                xmlOut.WriteStartDocument();

                // Always begin file with identification and warning
                xmlOut.WriteComment(Constants.xPathFileHeader1);
                xmlOut.WriteComment(Constants.xPathFileHeader2);

                // Save settings.
                xmlOut.WriteStartElement(Constants.xPathEcellProject);
                xmlOut.WriteElementString(Constants.xpathProject, project.Name);
                xmlOut.WriteElementString(Constants.textDate, DateTime.Now.ToString());
                xmlOut.WriteElementString(Constants.textComment, project.Comment);
                xmlOut.WriteElementString(Constants.textParameter, project.SimulationParam);
                xmlOut.WriteEndElement();
                xmlOut.WriteEndDocument();
            }
            finally
            {
                if (xmlOut != null)
                {
                    xmlOut.Close();
                }
            }
        }
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
            bool isCreated = true;
            string key = Constants.xpathProcess + Constants.delimiterColon + ecellObject.Key;
            WrappedPolymorph wrappedPolymorph = null;
            try
            {
                wrappedPolymorph = simulator.GetEntityPropertyList(key);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                isCreated = false;
            }
            if (isCreated != false && !wrappedPolymorph.IsList())
            {
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
                    try
                    {
                        if (storedEcellData.Settable)
                            initialCondition[storedEcellData.EntityPath] = (double)storedEcellData.Value;
                    }
                    catch (InvalidCastException)
                    {
                        // non-numeric properties
                    }
                }
            }
            //
            // Stores the "EcellData"
            //
            if (isCreated)
            {
                List<WrappedPolymorph> processAllPropertyList = wrappedPolymorph.CastToList();
                for (int i = 0; i < processAllPropertyList.Count; i++)
                {
                    Debug.Assert(processAllPropertyList[i].IsString());
                    string name = processAllPropertyList[i].CastToString();
                    List<bool> flag = simulator.GetEntityPropertyAttributes(
                        Util.BuildFullPN(key, name));
                    if (!flag[WrappedSimulator.s_flagGettable])
                        continue;

                    EcellValue value = null;
                    if (name == Constants.xpathActivity || name == Constants.xpathMolarActivity)
                    {
                        value = new EcellValue(0.0);
                    }
                    else if (name == Constants.xpathVRL)
                    {
                        value = new EcellValue(new List<EcellValue>());
                    }
                    else
                    {
                        try
                        {
                            value = new EcellValue(simulator.GetEntityProperty(key + Constants.delimiterColon + name));
                        }
                        catch (WrappedException ex)
                        {
                            Trace.WriteLine(ex);
                            // failed to fetch the default value of a property for unknown reasons
                            value = new EcellValue("");
                        }
                    }

                    EcellData ecellData = new EcellData(
                            name, value, key + Constants.delimiterColon + name);
                    ecellData.Settable = flag[WrappedSimulator.s_flagSettable];
                    ecellData.Gettable = flag[WrappedSimulator.s_flagGettable];
                    ecellData.Loadable = flag[WrappedSimulator.s_flagLoadable];
                    ecellData.Saveable = flag[WrappedSimulator.s_flagSavable];
                    ecellData.Logable = ecellData.Value.IsDouble;
                    try
                    {
                        if (ecellData.Settable)
                            initialCondition[ecellData.EntityPath] = (double)ecellData.Value;
                    }
                    catch (InvalidCastException)
                    {
                        // non-numeric properties
                    }

                    if (storedEcellDataDic.ContainsKey(name))
                    {
                        ecellData.Logged = storedEcellDataDic[name].Logged;
                        processEcellDataList.Remove(storedEcellDataDic[name]);
                    }
                    processEcellDataList.Add(ecellData);
                }
            }
            else
            {
                foreach (EcellData d in processEcellDataList)
                {
                    if (d.Name == Constants.xpathVRL)
                    {
                        string systemPath = "";
                        string name = Util.GetNameFromPath(ecellObject.Key, ref systemPath);
                        EcellValue v = EcellReference.ConvertReferenceInEml(systemPath, d.Value);
                        d.Value = v;
                        v.ToString();
                        break;
                    }
                }
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
            WrappedPolymorph wrappedPolymorph = null;
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
            if (!wrappedPolymorph.IsList())
            {
                return;
            }
            //
            // Sets the class name.
            //
            if (ecellObject.Classname == null || ecellObject.Classname.Length <= 0)
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
            else if (ecellObject.Value != null && ecellObject.Value.Count <= 0)
            {
                //
                // Sets the class name.
                //
                /* 20060315
                EcellData classNameData = new EcellData(
                    Constants.xpathClassName,
                    new EcellValue(simulator.GetStepperClassName(ecellObject.key)), Constants.xpathClassName
                    );
                classNameData.Settable = false;
                classNameData.Saveable = false;
                stepperEcellDataList.Add(classNameData);
                 */
            }
            //
            // Stores the "EcellData"
            //
            List<WrappedPolymorph> stepperAllPropertyList = wrappedPolymorph.CastToList();
            for (int i = 0; i < stepperAllPropertyList.Count; i++)
            {
                if (!(stepperAllPropertyList[i]).IsString())
                {
                    continue;
                }
                string name = (stepperAllPropertyList[i]).CastToString();
                List<bool> flag = simulator.GetStepperPropertyAttributes(ecellObject.Key, name);
                if (!flag[WrappedSimulator.s_flagGettable])
                {
                    continue;
                }
                EcellValue value = null;
                try
                {
                    WrappedPolymorph property = simulator.GetStepperProperty(ecellObject.Key, name);
                    value = new EcellValue(property);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    value = new EcellValue("");
                }
                EcellData ecellData = new EcellData(name, value, name);
                ecellData.Settable = flag[WrappedSimulator.s_flagSettable];
                ecellData.Gettable = flag[WrappedSimulator.s_flagGettable];
                ecellData.Loadable = flag[WrappedSimulator.s_flagLoadable];
                ecellData.Saveable = flag[WrappedSimulator.s_flagSavable];
                if (storedEcellDataDic.ContainsKey(name))
                {
                    if (value.IsString && ((string)value).Length == 0)
                        continue;
                    stepperEcellDataList.Remove(storedEcellDataDic[name]);
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
            string parentPath = ecellObject.Key.Substring(0, ecellObject.Key.LastIndexOf(
                    Constants.delimiterPath));
            string childPath = ecellObject.Key.Substring(ecellObject.Key.LastIndexOf(
                    Constants.delimiterPath) + 1);
            string key = Constants.xpathSystem + Constants.delimiterColon + ecellObject.Key;
            if (parentPath.Length == 0)
            {
                if (childPath.Length == 0)
                {
                    key = Constants.xpathSystem + Constants.delimiterColon +
                        parentPath + Constants.delimiterColon +
                        Constants.delimiterPath;
                }
                else
                {
                    key = Constants.xpathSystem + Constants.delimiterColon +
                        Constants.delimiterPath + Constants.delimiterColon +
                        childPath;
                }
            }
            else
            {
                key = Constants.xpathSystem + Constants.delimiterColon +
                    parentPath + Constants.delimiterColon +
                    childPath;
            }
            // Property List
            WrappedPolymorph wrappedPolymorph = simulator.GetEntityPropertyList(key);
            if (!wrappedPolymorph.IsList())
            {
                return;
            }
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
                    if (initialCondition != null && storedEcellData.Settable)
                    {
                        try
                        {
                            storedEcellData.Logable = storedEcellData.Value.IsDouble;
                            initialCondition[storedEcellData.EntityPath] = (double)storedEcellData.Value;
                        }
                        catch (InvalidCastException) 
                        {
                            // non-numeric property
                        }
                    }
                }
            }
            List<WrappedPolymorph> systemAllPropertyList = wrappedPolymorph.CastToList();
            for (int i = 0; i < systemAllPropertyList.Count; i++)
            {
                if (!(systemAllPropertyList[i]).IsString())
                {
                    continue;
                }
                string name = (systemAllPropertyList[i]).CastToString();
                List<bool> flag = simulator.GetEntityPropertyAttributes(
                        key + Constants.delimiterColon + name);
                if (!flag[WrappedSimulator.s_flagGettable])
                {
                    continue;
                }
                EcellValue value = null;
                WrappedPolymorph property = simulator.GetEntityProperty(key + Constants.delimiterColon + name);
                value = new EcellValue(property);
                EcellData ecellData
                        = new EcellData(name, value, key + Constants.delimiterColon + name);
                ecellData.Settable = flag[WrappedSimulator.s_flagSettable];
                ecellData.Gettable = flag[WrappedSimulator.s_flagGettable];
                ecellData.Loadable = flag[WrappedSimulator.s_flagLoadable];
                ecellData.Saveable = flag[WrappedSimulator.s_flagSavable];
                try
                {
                    ecellData.Logable = ecellData.Value.IsDouble;
                    if (ecellData.Settable)
                        initialCondition[ecellData.EntityPath] = (double)ecellData.Value;
                }
                catch (InvalidCastException)
                {
                    // non-numeric property
                }
                if (storedEcellDataDic.ContainsKey(name))
                {
                    ecellData.Logged = storedEcellDataDic[name].Logged;
                    systemEcellDataList.Remove(storedEcellDataDic[name]);
                }
                systemEcellDataList.Add(ecellData);
            }
            /*
            foreach (EcellData ecellData in notNullPropertyList)
            {
                if (ecellData.Value != null)
                {
                    EcellData newEcelldata = new EcellData(
                        ecellData.Name, ecellData.Value, key + Constants.delimiterColon + ecellData.Name);
                    newEcelldata.Settable = ecellData.Settable;
                    newEcelldata.Gettable = ecellData.Gettable;
                    newEcelldata.Loadable = ecellData.Loadable;
                    newEcelldata.Saveable = ecellData.Saveable;
                    systemEcellDataList.Add(ecellData);
                }
            }
             */
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
            WrappedPolymorph wrappedPolymorph = simulator.GetEntityPropertyList(key);
            if (!wrappedPolymorph.IsList())
            {
                return;
            }
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
                    if (initialCondition != null && storedEcellData.Settable)
                    {
                        try
                        {
                            storedEcellData.Logable = storedEcellData.Value.IsDouble;
                            initialCondition[storedEcellData.EntityPath] = (double)storedEcellData.Value;
                        }
                        catch (InvalidCastException)
                        {
                            // non-numeric property
                        }
                    }
                }
            }
            List<WrappedPolymorph> variableAllPropertyList = wrappedPolymorph.CastToList();
            for (int i = 0; i < variableAllPropertyList.Count; i++)
            {
                if (!(variableAllPropertyList[i]).IsString())
                {
                    continue;
                }
                string name = (variableAllPropertyList[i]).CastToString();
                List<bool> flag = simulator.GetEntityPropertyAttributes(
                        key + Constants.delimiterColon + name);
                if (!flag[WrappedSimulator.s_flagGettable])
                {
                    continue;
                }
                EcellValue value = null;
                WrappedPolymorph property = simulator.GetEntityProperty(
                        key + Constants.delimiterColon + name);
                value = new EcellValue(property);
                EcellData ecellData = new EcellData(
                        name, value, key + Constants.delimiterColon + name);
                ecellData.Settable = flag[WrappedSimulator.s_flagSettable];
                ecellData.Gettable = flag[WrappedSimulator.s_flagGettable];
                ecellData.Loadable = flag[WrappedSimulator.s_flagLoadable];
                ecellData.Saveable = flag[WrappedSimulator.s_flagSavable];
                try
                {
                    ecellData.Logable = ecellData.Value.IsDouble;
                    if (ecellData.Settable)
                        initialCondition[ecellData.EntityPath] = (double)ecellData.Value;
                }
                catch (InvalidCastException)
                {
                    // non-numeric property
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
        #endregion

    }
}
