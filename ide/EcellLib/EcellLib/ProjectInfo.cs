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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Ecell.Exceptions;
using System.Diagnostics;

namespace Ecell
{
    public class ProjectInfo
    {
        #region Field
        /// <summary>
        /// Project Folder Path.
        /// This field contains noll when This Project is created from EML or ProjectTemplate
        /// </summary>
        private string m_prjPath;
        /// <summary>
        /// Loaded project file.
        /// </summary>
        private string m_prjFile;
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
        /// The model file of this project.
        /// </summary>
        private List<string> m_modelList;
        /// <summary>
        /// The list of additional DMs.
        /// </summary>
        private List<string> m_dmList;
        #endregion

        #region Constructor
        /// <summary>
        /// Create the new "Project" instance with no argument.
        /// </summary>
        public ProjectInfo()
        {
            SetParams(Constants.defaultPrjID, Constants.defaultComment, "", Constants.defaultSimParam);
        }

        /// <summary>
        /// Create the new "Project" instance with initialized arguments.
        /// </summary>
        /// <param name="prjName">TThe project name. Not null.</param>
        /// <param name="comment">The comment. Arrows null.</param>
        /// <param name="time">The update time.</param>
        public ProjectInfo(string prjName, string comment, string time)
        {
            SetParams(prjName, comment, time, Constants.defaultSimParam);
        }

        /// <summary>
        /// Creates the new "Project" instance with initialized arguments.
        /// </summary>
        /// <param name="prjName">The project name. Not null.</param>
        /// <param name="comment">The comment</param>
        /// <param name="time">The update time</param>
        /// <param name="simParam">The name of simulation parameter. Not null.</param>
        public ProjectInfo(string prjName, string comment, string time, string simParam)
        {
            SetParams(prjName, comment, time, simParam);
        }

        /// <summary>
        /// Set Project parameters.
        /// </summary>
        /// <param name="prjName">The project name. Not null.</param>
        /// <param name="comment"></param>
        /// <param name="time"></param>
        /// <param name="simParam">The name of Simulation parameter. Not null.</param>
        private void SetParams(string prjName, string comment, string time, string simParam)
        {
            if (string.IsNullOrEmpty(prjName))
                throw new EcellException("Project name cannot be null.");
            if (string.IsNullOrEmpty(simParam))
                throw new EcellException("Simulation parameter cannot be null.");
            if (string.IsNullOrEmpty(time))
                time = DateTime.Now.ToString();
            this.Name = prjName;
            this.Comment = comment;
            this.UpdateTime = time;
            this.SimulationParam = simParam;
            this.m_prjPath = null;
            this.m_dmList = new List<string>();
            this.m_modelList = new List<string>();
        }
        #endregion

        #region Accessor
        /// <summary>
        /// The name of this project.
        /// </summary>
        public string Name
        {
            get { return m_prjName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new EcellException("Project name cannot be null.");
                else
                    this.m_prjName = value;
            }
        }

        /// <summary>
        /// Comment of this project.
        /// </summary>
        public string Comment
        {
            get { return m_comment; }
            set { this.m_comment = value; }
        }

        /// <summary>
        /// The last update time.
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
                    throw new EcellException("Project name cannot be null.");
                else
                    this.m_simParam = value;
            }
        }

        /// <summary>
        /// The ProjectPath of this project.
        /// This field contains null when the project is created from eml or Project Template.
        /// </summary>
        public string ProjectPath
        {
            get { return m_prjPath; }
            set { this.m_prjPath = value; }
        }

        /// <summary>
        /// The Loaded ProjectFile of this project.
        /// </summary>
        public string ProjectFile
        {
            get { return m_prjFile; }
            set { this.m_prjFile = value; }
        }

        /// <summary>
        /// The list of ModelFiles.
        /// </summary>
        public List<string> Models
        {
            get { return m_modelList; }
        }

        /// <summary>
        /// The list of Additional DMs.
        /// </summary>
        public List<string> DMList
        {
            get { return m_dmList; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Save project.
        /// </summary>
        public void Save()
        {
            ProjectInfoSaver.Save(this, m_prjPath);
        }

        /// <summary>
        /// Save project.
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            ProjectInfoSaver.Save(this, filePath);
        }

        /// <summary>
        /// Find Models on this project.
        /// </summary>
        public void FindModels()
        {
            if (m_prjPath == null)
                return;

            // Get Dir
            string modelDirName = Path.Combine(m_prjPath, Constants.xpathModel);

            string[] models = Directory.GetFileSystemEntries(
                modelDirName,
                Constants.delimiterWildcard + Constants.FileExtEML
                );
            Debug.Assert(models != null && models.Length > 0);
            foreach (string model in models)
            {
                string fileName = Path.GetFileName(model);
                if (fileName.EndsWith(Constants.FileExtBackUp))
                    continue;
                m_modelList.Add(model);
            }
        }

        /// <summary>
        /// Find Additional DMs on this project.
        /// </summary>
        public void FindDMs()
        {
            if (m_prjPath == null)
                return;

            string dmDirName = Path.Combine(m_prjPath, Constants.DMDirName);
            if (!Directory.Exists(dmDirName))
                return;
            string[] dms = Directory.GetFileSystemEntries(
                dmDirName,
                Constants.delimiterWildcard + Constants.FileExtDM
                );
            foreach (string dm in dms)
            {
                string fileName = Path.GetFileName(dm);
                if (fileName.EndsWith(Constants.FileExtBackUp))
                    continue;
                m_dmList.Add(dm);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, List<string>> GetDMDic()
        {
            Dictionary<string, List<string>> dmDic = new Dictionary<string, List<string>>();
            // 4 System
            List<string> systemList = new List<string>();
            systemList.Add(Constants.xpathSystem);
            dmDic.Add(Constants.xpathSystem, systemList);

            // 4 Variable
            List<string> variableList = new List<string>();
            variableList.Add(Constants.xpathVariable);
            dmDic.Add(Constants.xpathVariable, variableList);

            // 4 Process
            List<string> processList = new List<string>();
            dmDic.Add(Constants.xpathProcess, processList);

            // 4 Stepper
            List<string> stepperList = new List<string>();
            dmDic.Add(Constants.xpathStepper, stepperList);

            foreach (string dmFile in m_dmList)
            {
                if (!File.Exists(dmFile))
                    continue;
                if (dmFile.EndsWith(Constants.xpathProcess + Constants.FileExtDM))
                    processList.Add(dmFile);
                if (dmFile.EndsWith(Constants.xpathStepper + Constants.FileExtDM))
                    stepperList.Add(dmFile);
                if (dmFile.EndsWith(Constants.xpathSystem + Constants.FileExtDM))
                    systemList.Add(dmFile);
                if (dmFile.EndsWith(Constants.xpathVariable + Constants.FileExtDM))
                    variableList.Add(dmFile);
            }
            return dmDic;
        }
        #endregion

    }


    /// <summary>
    /// ProjectLoader
    /// </summary>
    public class ProjectInfoLoader
    {
        /// <summary>
        /// Load Project from setting file.
        /// </summary>
        /// <param name="filepath">this function supports project.xml, project.info, and *.eml</param>
        /// <returns></returns>
        public static ProjectInfo Load(string filepath)
        {
            ProjectInfo project = null;
            try
            {
                string ext = Path.GetExtension(filepath);
                if (ext.Equals(Constants.FileExtXML))
                    project = LoadProjectFromXML(filepath);
                else if (ext.Equals(Constants.FileExtINFO))
                    project = LoadProjectFromInfoText(filepath);
                else if (ext.Equals(Constants.FileExtEML))
                    project = LoadProjectFromEml(filepath);
                else
                    throw new EcellException("Unknown file type");
                project.ProjectFile = filepath;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                throw new EcellException(MessageResources.ErrLoadProjectInfo, e);
            }

            return project;
        }

        /// <summary>
        /// Get Project from XML file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static ProjectInfo LoadProjectFromXML(string filepath)
        {
            ProjectInfo project = null;
            string dirPath = Path.GetDirectoryName(filepath);
            string prjName = Path.GetFileName(dirPath);
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
                    throw new EcellException("No Project Settings.");

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
                project = new ProjectInfo(prjName, comment, time, param);
                project.ProjectPath = dirPath;
            }
            catch (Exception ex)
            {
                throw new EcellException("Failed to load Project XML", ex);
            }
            return project;
        }

        /// <summary>
        /// Get Project from Info file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static ProjectInfo LoadProjectFromInfoText(string filepath)
        {
            ProjectInfo project = null;
            string comment = "";
            string simParam = "";
            string time = File.GetLastWriteTime(filepath).ToString();

            string dirPath = Path.GetDirectoryName(filepath);
            string prjName = Path.GetFileName(dirPath);

            TextReader reader = new StreamReader(filepath);
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                if (line.IndexOf(Constants.textComment) == 0)
                {
                    if (string.IsNullOrEmpty(comment))
                        comment = ParseProjectInfo(line, Constants.textComment);
                    else
                        comment = comment + "\n" + ParseProjectInfo(line, Constants.textComment);
                }
                else if (line.IndexOf(Constants.textParameter) == 0)
                {
                    simParam = ParseProjectInfo(line, Constants.textParameter);
                }
                else if (line.IndexOf(Constants.xpathProject) == 0)
                {
                    prjName = ParseProjectInfo(line, Constants.xpathProject);
                }
            }
            reader.Close();
            project = new ProjectInfo(prjName, comment, time, simParam);
            project.ProjectPath = dirPath;
            return project;
        }

        /// <summary>
        /// Get Parameter text from ProjectInfo.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string ParseProjectInfo(string line, string param)
        {
            string comment;
            if (line.IndexOf(Constants.delimiterEqual) != -1)
            {
                comment = line.Split(Constants.delimiterEqual.ToCharArray())[1].Trim();
            }
            else
            {
                comment = line.Substring(param.Length).Trim();
            }
            return comment;
        }

        /// <summary>
        /// Get Project from Eml file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static ProjectInfo LoadProjectFromEml(string filepath)
        {
            ProjectInfo project = null;
            string name = Path.GetFileNameWithoutExtension(filepath);
            string comment = "";
            string time = File.GetLastWriteTime(filepath).ToString();
            project = new ProjectInfo(name, comment, time, Constants.defaultSimParam);
            project.Models.Add(filepath);
            return project;
        }
    }

    /// <summary>
    /// ProjectSaver
    /// </summary>
    public class ProjectInfoSaver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project">target project</param>
        /// <param name="filepath"></param>
        public static void Save(ProjectInfo project, string filepath)
        {
            string message = "[" + project.Name + "]";
            // Get Saving Directory.
            string saveDir = GetSaveDir(project, filepath);
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);
            // Save both InfoText and XML setting file.
            try
            {
                SaveProjectINFO(project, saveDir);
                SaveProjectXML(project, saveDir);
            }
            catch (Exception ex)
            {
                throw new EcellException(message + " {" + ex.ToString() + "}");
            }
        }

        /// <summary>
        /// GetSaveDir
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static string GetSaveDir( ProjectInfo info, string filepath)
        {
            // If filepath is empty, create new path.
            if (string.IsNullOrEmpty(filepath))
                return Path.Combine(Util.GetBaseDir(), info.Name);
            // if filepath is directory. return directory path.
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
        /// <param name="saveDir"></param>
        public static void SaveProjectINFO(ProjectInfo project, string saveDir)
        {
            StreamWriter writer = null;
            string projectInfo = Path.Combine(saveDir, Constants.fileProject);
            string sepalator = Constants.delimiterSpace + Constants.delimiterEqual + Constants.delimiterSpace;
            try
            {
                writer = new StreamWriter(projectInfo, false, Encoding.UTF8);
                writer.WriteLine(Constants.xpathProject + sepalator + project.Name);
                writer.WriteLine(Constants.textComment + sepalator + project.Comment);
                writer.WriteLine(Constants.textParameter + sepalator + project.SimulationParam);
                project.ProjectPath = saveDir;
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
        /// <param name="saveDir"></param>
        public static void SaveProjectXML(ProjectInfo project, string saveDir)
        {
            XmlTextWriter xmlOut = null;
            string projectXML = Path.Combine(saveDir, Constants.fileProjectXML);
            try
            {
                // Create xml file
                xmlOut = new XmlTextWriter(projectXML, Encoding.UTF8);

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
                project.ProjectPath = saveDir;
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

}
