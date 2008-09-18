using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Ecell
{
    public class ProjectInfo
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
        #endregion

        #region Constructor
        /// <summary>
        /// Creates the new "Project" instance with no argument.
        /// </summary>
        public ProjectInfo()
        {
            SetParams(Constants.defaultPrjID, Constants.defaultComment, "", Constants.defaultSimParam);
        }

        /// <summary>
        /// Creates the new "Project" instance with initialized arguments.
        /// </summary>
        /// <param name="prjName">The project name</param>
        /// <param name="comment">The comment</param>
        /// <param name="time">The update time</param>
        public ProjectInfo(string prjName, string comment, string time)
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
        public ProjectInfo(string prjName, string comment, string time, string simParam)
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
        #endregion

        #region Methods
        /// <summary>
        /// Save project.
        /// </summary>
        /// <param name="filePath"></param>
        public void Save()
        {
            ProjectInfoSaver.Save(this, m_filePath);
        }

        /// <summary>
        /// Save project.
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            ProjectInfoSaver.Save(this, filePath);
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
        private static ProjectInfo LoadProjectFromXML(string filepath)
        {
            ProjectInfo project = null;
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
            project = new ProjectInfo(prjName, comment, time, param);
            return project;
        }

        /// <summary>
        /// Get Project from Info file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static ProjectInfo LoadProjectFromInfo(string filepath)
        {
            ProjectInfo project = null;
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
            project = new ProjectInfo(prjName, comment, time, simParam);
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
            project = new ProjectInfo(name, comment, time);
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
        /// <param name="project"></param>
        /// <param name="filepath"></param>
        public static void Save(ProjectInfo project, string filepath)
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
        public static void SaveProjectINFO(ProjectInfo project, string filePath)
        {
            StreamWriter writer = null;
            string sepalator = Constants.delimiterSpace + Constants.delimiterEqual + Constants.delimiterSpace;
            try
            {
                writer = new StreamWriter(filePath, false, Encoding.UTF8);
                writer.WriteLine(Constants.xpathProject + sepalator + project.Name);
                writer.WriteLine(Constants.textComment + sepalator + project.Comment);
                writer.WriteLine(Constants.textParameter + sepalator + project.SimulationParam);
                project.FilePath = filePath;
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
        public static void SaveProjectXML(ProjectInfo project, string filePath)
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
                project.FilePath = filePath;
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
