using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace EcellLib
{
    /// <summary>
    /// Stores the project information.
    /// </summary>
    public class Project
    {
        #region Field
        /// <summary>
        /// File Path.
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
        public Project()
        {
            SetParams(Constants.defaultPrjID, Constants.defaultComment, "", Constants.defaultSimParam);
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
            set { this.m_filePath = value; }
        }

        #endregion

        #region Loader
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
            return project;
        }

        /// <summary>
        /// Get Project from XML file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static Project LoadProjectFromXML(string filepath)
        {
            if (!File.Exists(filepath))
                return null;

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
            return new Project(prjName, comment, time, param);
        }

        /// <summary>
        /// Get Project from Info file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static Project LoadProjectFromInfo(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return null;
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
            return new Project(prjName, comment, time, simParam);
        }

        /// <summary>
        /// Get Project from Eml file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static Project LoadProjectFromEml(string filepath)
        {
            string name = Path.GetFileNameWithoutExtension(filepath);
            string comment = "";
            string time = File.GetLastWriteTime(filepath).ToString();

            return new Project(name, comment, time);
        }
        #endregion

        #region Saver
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

        #endregion
    }
}
