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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using EcellCoreLib;
using EcellLib.Objects;

namespace EcellLib
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
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>> m_initialCondition = null;
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
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public Project(string filePath)
        {
            Project prj = ProjectLoader.LoadProject(filePath);
            this.m_prjName = prj.m_prjName;
            this.m_comment = prj.m_comment;
            this.m_simParam = prj.m_simParam;
            this.m_updateTime = prj.m_updateTime;
            this.m_filePath = prj.m_filePath;
            this.m_prjPath = prj.m_prjPath;
        }

        /// <summary>
        /// Creates the new "Project" instance with initialized arguments.
        /// </summary>
        /// <param name="l_prjName">The project name</param>
        /// <param name="l_comment">The comment</param>
        /// <param name="l_time">The update time</param>
        public Project(string l_prjName, string l_comment, string l_time)
        {
            SetParams(l_prjName, l_comment, l_time, Constants.defaultSimParam);
        }

        /// <summary>
        /// Creates the new "Project" instance with initialized arguments.
        /// </summary>
        /// <param name="l_prjName">The project name</param>
        /// <param name="l_comment">The comment</param>
        /// <param name="l_time">The update time</param>
        /// <param name="l_simParam">The name of simulation parameter.</param>
        public Project(string l_prjName, string l_comment, string l_time, string l_simParam)
        {
            SetParams(l_prjName, l_comment, l_time, l_simParam);
        }

        /// <summary>
        /// Set Project parameters.
        /// </summary>
        /// <param name="l_prjName"></param>
        /// <param name="l_comment"></param>
        /// <param name="l_time"></param>
        /// <param name="l_simParam"></param>
        private void SetParams(string l_prjName, string l_comment, string l_time, string l_simParam)
        {
            this.Name = l_prjName;
            this.Comment = l_comment;
            this.UpdateTime = l_time;
            this.SimulationParam = l_simParam;
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
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.m_simParam = Constants.defaultSimParam;
                else
                    this.m_simParam = value;
            }
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
        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>> InitialCondition
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

            m_initialCondition = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, double>>>>();
            m_initialCondition[m_simParam] = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            m_initialCondition[m_simParam][modelID] = new Dictionary<string, Dictionary<string, double>>();
            m_initialCondition[m_simParam][modelID][Constants.xpathSystem] = new Dictionary<string, double>();
            m_initialCondition[m_simParam][modelID][Constants.xpathProcess] = new Dictionary<string, double>();
            m_initialCondition[m_simParam][modelID][Constants.xpathVariable] = new Dictionary<string, double>();
            m_initialCondition[m_simParam][modelID][Constants.xpathText] = new Dictionary<string, double>();

        }

        /// <summary>
        /// Sets the list of the DM.
        /// </summary>
        internal void SetDMList()
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
            string[] l_dmPathArray = Util.GetDMDirs(m_prjPath);
            if (l_dmPathArray == null)
            {
                throw new Exception("ErrFindDmDir");
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
                    Constants.delimiterWildcard + Constants.xpathProcess + Constants.FileExtDM
                    );
                foreach (string l_processDM in l_processDMArray)
                {
                    this.m_dmDic[Constants.xpathProcess].Add(Path.GetFileNameWithoutExtension(l_processDM));
                }
                // 4 Stepper
                string[] l_stepperDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathStepper + Constants.FileExtDM
                    );
                foreach (string l_stepperDM in l_stepperDMArray)
                {
                    this.m_dmDic[Constants.xpathStepper].Add(Path.GetFileNameWithoutExtension(l_stepperDM));
                }
                // 4 System
                string[] l_systemDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathSystem + Constants.FileExtDM
                    );
                foreach (string l_systemDM in l_systemDMArray)
                {
                    this.m_dmDic[Constants.xpathSystem].Add(Path.GetFileNameWithoutExtension(l_systemDM));
                }
                // 4 Variable
                string[] l_variableDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathVariable + Constants.FileExtDM
                    );
                foreach (string l_variableDM in l_variableDMArray)
                {
                    this.m_dmDic[Constants.xpathVariable].Add(Path.GetFileNameWithoutExtension(l_variableDM));
                }
            }
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
            EcellObject system = GetSystem(model, EcellObject.GetParentSystemId(key));
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

            if (ext.Equals(Constants.FileExtXML))
                project = LoadProjectFromXML(filepath);
            else if (ext.Equals(Constants.FileExtINFO))
                project = LoadProjectFromInfo(filepath);
            else if (ext.Equals(Constants.FileExtEML))
                project = LoadProjectFromEml(filepath);
            else
                return project;
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
            if (!File.Exists(filepath))
                return project;
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
                string errmsg = "ErrLoadProjectSettings" + Environment.NewLine + filepath + Environment.NewLine + ex.Message;
                MessageBox.Show(errmsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (!File.Exists(filepath))
            {
                return project;
            }
            string line = "";
            string comment = "";
            string simParam = "";
            string time = File.GetLastWriteTime(filepath).ToString();

            string dirPathName = Path.GetDirectoryName(filepath);
            string prjName = Path.GetFileName(dirPathName);
            TextReader l_reader = new StreamReader(filepath);
            while ((line = l_reader.ReadLine()) != null)
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
            l_reader.Close();
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
            if (!File.Exists(filepath))
            {
                return project;
            }

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
            catch (Exception l_ex)
            {
                throw new Exception(message + " {" + l_ex.ToString() + "}");
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
        /// <param name="l_simulator">The "simulator"</param>
        /// <param name="l_ecellObject">The stored "EcellObject"</param>
        /// <param name="l_initialCondition">The initial condition.</param>
        internal static void DataStored(
                WrappedSimulator l_simulator,
                EcellObject l_ecellObject,
                Dictionary<string, Dictionary<string, double>> l_initialCondition)
        {
            Dictionary<string, double> l_childInitialCondition = null;
            if (l_ecellObject.Type.Equals(Constants.xpathStepper))
            {
                DataStored4Stepper(l_simulator, l_ecellObject);
            }
            else if (l_ecellObject.Type.Equals(Constants.xpathSystem))
            {
                if (l_initialCondition != null)
                {
                    l_childInitialCondition = l_initialCondition[Constants.xpathSystem];
                }
                DataStored4System(
                        l_simulator,
                        l_ecellObject,
                        l_childInitialCondition);
            }
            else if (l_ecellObject.Type.Equals(Constants.xpathProcess))
            {
                if (l_initialCondition != null)
                {
                    l_childInitialCondition = l_initialCondition[Constants.xpathProcess];
                }
                DataStored4Process(
                        l_simulator,
                        l_ecellObject,
                        l_childInitialCondition);
            }
            else if (l_ecellObject.Type.Equals(Constants.xpathVariable))
            {
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
                    DataStored(l_simulator, l_childEcellObject, l_initialCondition);
                }
            }
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Process".
        /// </summary>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_ecellObject">The stored "Process"</param>
        /// <param name="l_initialCondition">The initial condition.</param>
        internal static void DataStored4Process(
                WrappedSimulator l_simulator,
                EcellObject l_ecellObject,
                Dictionary<string, double> l_initialCondition)
        {
            bool isCreated = true;
            string l_key = Constants.xpathProcess + Constants.delimiterColon + l_ecellObject.Key;
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
                        if (l_storedEcellData.Value.IsDouble() &&
                            (l_storedEcellData.Settable == false ||
                            l_storedEcellData.Saveable == false))
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
                    catch (Exception ex)
                    {
                        ex.ToString();
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
                        if (l_ecellData.Value.IsDouble() &&
                            (l_ecellData.Settable == false ||
                            l_ecellData.Saveable == false))
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
                        string name = Util.GetNameFromPath(l_ecellObject.Key, ref systemPath);
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
        internal static void DataStored4Stepper(
                WrappedSimulator l_simulator, EcellObject l_ecellObject)
        {
            List<EcellData> l_stepperEcellDataList = new List<EcellData>();
            WrappedPolymorph l_wrappedPolymorph = null;
            //
            // Property List
            //
            try
            {
                l_wrappedPolymorph = l_simulator.GetStepperPropertyList(l_ecellObject.Key);
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
            if (l_ecellObject.Classname == null || l_ecellObject.Classname.Length <= 0)
            {
                l_ecellObject.Classname = l_simulator.GetStepperClassName(l_ecellObject.Key);
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
                List<bool> l_flag = l_simulator.GetStepperPropertyAttributes(l_ecellObject.Key, l_name);
                if (!l_flag[WrappedSimulator.s_flagGettable])
                {
                    continue;
                }
                EcellValue l_value = null;
                try
                {
                    WrappedPolymorph l_property = l_simulator.GetStepperProperty(l_ecellObject.Key, l_name);
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
        internal static void DataStored4System(
                WrappedSimulator l_simulator,
                EcellObject l_ecellObject,
                Dictionary<string, double> l_initialCondition)
        {
            // Creates an entityPath.
            string l_parentPath = l_ecellObject.Key.Substring(0, l_ecellObject.Key.LastIndexOf(
                    Constants.delimiterPath));
            string l_childPath = l_ecellObject.Key.Substring(l_ecellObject.Key.LastIndexOf(
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
            // Property List
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
        internal static void DataStored4Variable(
                WrappedSimulator l_simulator,
                EcellObject l_ecellObject,
                Dictionary<string, double> l_initialCondition)
        {
            string l_key = Constants.xpathVariable + Constants.delimiterColon + l_ecellObject.Key;
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
        #endregion

    }
}
