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
    /// <summary>
    /// ProjectInfo
    /// </summary>
    public class ProjectInfo
    {
        #region Field
        /// <summary>
        /// 
        /// </summary>
        private ProjectType m_type;
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
        /// The project name
        /// </summary>
        private string m_prjName;
        /// <summary>
        /// 
        /// </summary>
        private string m_createTime;
        /// <summary>
        /// The update time
        /// </summary>
        private string m_updateTime;
        /// <summary>
        /// Edit Count.
        /// </summary>
        private int m_editCount;
        /// <summary>
        /// The creator
        /// </summary>
        private string m_creator;
        /// <summary>
        /// The comment
        /// </summary>
        private string m_comment;
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
            this.m_creator = "";
            this.m_createTime = time;
            this.m_updateTime = time;
            this.SimulationParam = simParam;
            this.m_prjPath = null;
            this.m_dmList = new List<string>();
            this.m_modelList = new List<string>();
            this.m_type = ProjectType.NewProject;
            this.m_editCount = 0;
        }
        #endregion

        #region Accessor
        /// <summary>
        /// The ProjectType of this project.
        /// </summary>
        public ProjectType ProjectType
        {
            get { return m_type; }
            set { this.m_type = value; }
        }
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
        /// The Creator.
        /// </summary>
        public string Creator
        {
            get { return m_creator; }
            set { this.m_creator = value; }
        }

        /// <summary>
        /// The created time.
        /// </summary>
        public string CreationTime
        {
            get { return m_createTime; }
            set { this.m_createTime = value; }
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
        /// get / set EditCount.
        /// </summary>
        public int EditCount
        {
            get { return m_editCount; }
            set { m_editCount = value; }
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
        /// The list of Additional DMDirs.
        /// </summary>
        public List<string> DMDirList
        {
            get { return m_dmList; }
            set { m_dmList = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Save project.
        /// </summary>
        public void Save()
        {
            Save(m_prjPath);
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
                else if (ext.Equals(Constants.FileExtEML))
                    project = LoadProjectFromEml(filepath);
                //else if (ext.Equals(Constants.FileExtSBML))
                //    project = LoadProjectFromSbml(filepath);
                else
                    throw new EcellException("Unknown file type :" + filepath);

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
            string createTime = "";
            string param = Constants.defaultSimParam;
            string creator = "";
            int editCount = 0;
            ProjectType type = ProjectType.Project;
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
                            createTime = setting.InnerText;
                            break;
                        // Comment
                        case Constants.textComment:
                            comment = setting.InnerText;
                            break;
                        // Creator
                        case Constants.textCreator:
                            creator = setting.InnerText;
                            break;
                        // EditCount
                        case Constants.textEditCount:
                            int.TryParse(setting.InnerText, out editCount);
                            break;
                        // SimulationParameter
                        //case Constants.textParameter:
                        //    param = setting.InnerText;
                        //    break;
                        case Constants.xpathType:
                            type = (ProjectType)Convert.ToInt32(setting.InnerText);
                            break;
                    }
                }
                project = new ProjectInfo(prjName, comment, createTime, param);
                project.ProjectType = type;
                project.ProjectPath = dirPath;
                project.CreationTime = createTime;
                project.Creator = creator;
                project.EditCount = editCount;
                project.UpdateTime = File.GetLastWriteTime(filepath).ToString();
                // Set DM Path.
                string dmPath = Path.Combine(dirPath, Constants.DMDirName);
                if(Directory.Exists(dmPath))
                    project.DMDirList.Add(dmPath);
            }
            catch (Exception ex)
            {
                throw new EcellException("Failed to load Project XML", ex);
            }
            return project;
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
            project.ProjectType = ProjectType.Model;
            return project;
        }

        ///// <summary>
        ///// Get Project from SBML file.
        ///// </summary>
        ///// <param name="filepath"></param>
        ///// <returns></returns>
        //private static ProjectInfo LoadProjectFromSbml(string filepath)
        //{
        //    ProjectInfo project = null;
        //    string name = Path.GetFileNameWithoutExtension(filepath);
        //    string comment = "";
        //    string time = File.GetLastWriteTime(filepath).ToString();
        //    project = new ProjectInfo(name, comment, time, Constants.defaultSimParam);
        //    project.Models.Add(filepath);
        //    project.ProjectType = ProjectType.SBML;
        //    return project;
        //}

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
            // Get Saving Directory.
            string saveDir = GetSaveDir(project, filepath);
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);
            // Save both InfoText and XML setting file.
            try
            {
                SaveProjectXML(project, saveDir);
            }
            catch (Exception ex)
            {
                throw new EcellException(ex.Message);
            }
        }

        /// <summary>
        /// GetSaveDir
        /// </summary>
        /// <param name="info"></param>
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

                project.EditCount++;
                // Save settings.
                xmlOut.WriteStartElement(Constants.xPathEcellProject);
                xmlOut.WriteElementString(Constants.xpathProject, project.Name);
                xmlOut.WriteElementString(Constants.textDate, project.CreationTime);
                xmlOut.WriteElementString(Constants.textCreator, project.Creator);
                xmlOut.WriteElementString(Constants.textEditCount, project.EditCount.ToString());
                xmlOut.WriteElementString(Constants.textComment, project.Comment);
                xmlOut.WriteElementString(Constants.textParameter, project.SimulationParam);
                xmlOut.WriteEndElement();
                xmlOut.WriteEndDocument();

                project.ProjectPath = saveDir;
                project.UpdateTime = DateTime.Now.ToString();
                project.ProjectFile = projectXML;
                project.ProjectType = ProjectType.Project;
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
    /// 
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// 
        /// </summary>
        Project = 0,
        /// <summary>
        /// 
        /// </summary>
        Model = 1,
        /// <summary>
        /// 
        /// </summary>
        Template = 2,
        /// <summary>
        /// 
        /// </summary>
        NewProject = 3,
        /// <summary>
        /// 
        /// </summary>
        SBML = 4,
        /// <summary>
        /// 
        /// </summary>
        Revision = 5
    }

}
