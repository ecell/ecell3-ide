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
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace EcellLib.MainWindow
{

    /// <summary>
    /// Dialog to select the opened project.
    /// </summary>
    public partial class ManageProjectDialog : Form
    {
        /// <summary>
        /// ResourceManager for MainWindow.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResMain));

        private string m_prjID = "";
        private string m_fileName = "";
        private string m_simName = "";
        private string m_comment = "";
        private static string[] ignoredDirList = {
            "Model",
            "Simulation",
            "Parameters",
            Constants.DMDirName
        };

        /// <summary>
        /// Constructor.
        /// </summary>
        public ManageProjectDialog()
        {
            InitializeComponent();
            MPPrjTreeView.ContextMenuStrip = CreatePopupMenus();
            MPOpenButton.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ContextMenuStrip CreatePopupMenus()
        {
            // Preparing a context menu.
            ContextMenuStrip menus = new ContextMenuStrip();

            // SaveZip
            ToolStripItem savezip = new ToolStripMenuItem(MngPrjDlgConstants.SaveZip);
            savezip.Name = MngPrjDlgConstants.SaveZip;
            savezip.Text = s_resources.GetString(MngPrjDlgConstants.SaveZip);
            savezip.Click += new EventHandler(SaveZipClick);
            menus.Items.Add(savezip);

            return menus;
        }

        /// <summary>
        /// get the project ID.
        /// </summary>
        public string PrjID
        {
            get { return this.m_prjID; }
        }

        /// <summary>
        /// get the project file name.
        /// </summary>
        public string FileName
        {
            get { return this.m_fileName; }
        }

        /// <summary>
        /// get the simulation parameter for this project.
        /// </summary>
        public string SimulationParam
        {
            get { return this.m_simName; }
        }

        /// <summary>
        /// get the comment of this project.
        /// </summary>
        public string Comment
        {
            get { return this.m_comment; }
        }

        /// <summary>
        /// Create the project tree.
        /// Add the project file and directory on path to node TreeNode.
        /// </summary>
        /// <param name="node">The current TreeNode.</param>
        /// <param name="path">The current path.</param>
        /// <param name="isProject">The flag whether the current path is in the project directory.</param>
        public void CreateProjectTreeView(TreeNode node, string path, bool isProject)
        {
            if (node == null)
            {
                node = new ProjectTreeNode(path);
                MPPrjTreeView.Nodes.Add(node);
            }

            string prjFileName = Path.Combine(path, Constants.fileProject);
            string prjXMLFileName = Path.Combine(path, Constants.fileProjectXML);

            // Check project.xml and load.
            if (File.Exists(prjXMLFileName))
            {
                TreeNode childNode = new ProjectTreeNode(prjXMLFileName);
                node.Nodes.Add(childNode);
                isProject = true;
            }
            // Check project.info and load.
            else if (File.Exists(prjFileName))
            {
                TreeNode childNode = new ProjectTreeNode(prjFileName);
                node.Nodes.Add(childNode);
                isProject = true;
            }
            else if (isProject == false)
            {
                string[] files = Directory.GetFiles(path, "*.eml");
                foreach (string file in files)
                {
                    TreeNode childNode = new ProjectTreeNode(file);
                    node.Nodes.Add(childNode);
                }
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                string name = Path.GetFileNameWithoutExtension(dir);
                bool ignored = false;
                foreach (string ignoredDir in ignoredDirList)
                {
                    if (ignoredDir.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        ignored = true;
                        break;
                    }
                }
                if (ignored)
                    continue;

                ProjectTreeNode childNode = new ProjectTreeNode(dir);
                node.Nodes.Add(childNode);

                CreateProjectTreeView(childNode, dir, isProject);
            }
        }

        /// <summary>
        /// Event to click the node by mouse.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs.</param>
        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView tView = (TreeView)sender;
            ProjectTreeNode node = (ProjectTreeNode)tView.GetNodeAt(e.X, e.Y);

            // Reset selected project if project is null.
            if (node == null || node.Project == null)
            {
                ResetSelectedProject();
                return;
            }

            // Reflect Project parameters.
            Project prj = node.Project;
            m_fileName = node.FileName;
            MPPrjIDText.Text = prj.Name;
            MPPrjDateText.Text = prj.UpdateTime;
            MPPrjCommentText.Text = prj.Comment;

            MPPrjIDText.BackColor = Color.White;
            MPPrjDateText.BackColor = Color.White;
            MPPrjCommentText.BackColor = Color.White;

            MPOpenButton.Enabled = true;
            MPPrjCommentText.ReadOnly = false;
            MPPrjIDText.ReadOnly = false;
        }

        /// <summary>
        /// ResetSelectedProject
        /// </summary>
        private void ResetSelectedProject()
        {
            MPPrjIDText.Text = "";
            MPPrjDateText.Text = "";
            MPPrjCommentText.Text = "";

            MPPrjIDText.BackColor = Color.Silver;
            MPPrjDateText.BackColor = Color.Silver;
            MPPrjCommentText.BackColor = Color.Silver;

            MPOpenButton.Enabled = false;
            MPPrjCommentText.ReadOnly = true;
            MPPrjIDText.ReadOnly = true;
            m_fileName = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveZipClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_prjID))
                return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = Constants.FilterZipFile;
            if(dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            if (string.IsNullOrEmpty(filename))
                return;
            string foldername = Path.Combine(Util.GetBaseDir(), m_prjID);
            ZipUtil.ZipFolder(filename, foldername);
            dialog.Dispose();
        }

        /// <summary>
        /// ProjectTreeNode
        /// </summary>
        internal class ProjectTreeNode : TreeNode
        {
            private string m_fileName = null;
            private int m_nodeType = 0;
            private Project m_project = null;

            /// <summary>
            /// filename
            /// </summary>
            public string FileName
            {
                get { return m_fileName; }
                set { m_fileName = value; }
            }

            public Project Project
            {
                get { return m_project; }
                set { m_project = value; }
            }
            /// <summary>
            /// Type of node.
            /// </summary>
            public int NodeType
            {
                get { return m_nodeType; }
                set { m_nodeType = value; }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public ProjectTreeNode()
            {
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="filepath"></param>
            public ProjectTreeNode(string filepath)
            {
                this.m_fileName = filepath;
                this.m_nodeType = GetNodeType(filepath);
                this.m_project = GetProject(m_nodeType, filepath);
                if (m_project == null)
                    this.Text = Path.GetFileNameWithoutExtension(filepath);
                else
                    this.Text = m_project.Name;
                this.ImageIndex = m_nodeType;
                this.SelectedImageIndex = m_nodeType;
                this.Tag = filepath;
            }

            /// <summary>
            /// Get NodeType
            /// </summary>
            /// <param name="filepath"></param>
            /// <returns></returns>
            private int GetNodeType(string filepath)
            {
                string ext = Path.GetExtension(filepath);
                if (ext.Equals(Constants.FileExtXML))
                    return MngPrjDlgConstants.NodeGraphicsProject;
                else if (ext.Equals(Constants.FileExtINFO))
                    return MngPrjDlgConstants.NodeGraphicsProject;
                else if (ext.Equals(Constants.FileExtEML))
                    return MngPrjDlgConstants.NodeGraphicsModel;
                else
                    return MngPrjDlgConstants.NodeGraphicsFolder;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="nodeType"></param>
            /// <param name="filepath"></param>
            private Project GetProject(int nodeType, string filepath)
            {
                Project project = null;
                switch (nodeType)
                {
                    case MngPrjDlgConstants.NodeGraphicsFolder:
                        break;
                    case MngPrjDlgConstants.NodeGraphicsModel:
                        project = GetProjectFromEml(filepath);
                        break;
                    case MngPrjDlgConstants.NodeGraphicsProject:
                        if (Path.GetExtension(filepath).Equals(Constants.FileExtXML))
                            project = GetProjectFromXML(filepath);
                        else
                            project = GetProjectFromInfo(filepath);
                        break;
                }

                return project;
            }

            /// <summary>
            /// Get Project from XML file.
            /// </summary>
            /// <param name="filepath"></param>
            /// <returns></returns>
            private static Project GetProjectFromXML(string filepath)
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
                            case "Project":
                                prjName = setting.InnerText;
                                break;
                            // Date
                            case "Date":
                                time = setting.InnerText;
                                break;
                            // Comment
                            case "Comment":
                                comment = setting.InnerText;
                                break;
                            // SimulationParameter
                            case "SimulationParameter":
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
                return new Project(prjName, comment, time);
            }

            /// <summary>
            /// Get Project from Info file.
            /// </summary>
            /// <param name="filepath"></param>
            /// <returns></returns>
            private static Project GetProjectFromInfo(string filepath)
            {
                if (!File.Exists(filepath))
                {
                    return null;
                }
                string line = "";
                string comment = "";
                string simname = "";
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
                        simname = line;
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
                return new Project(prjName, comment, time);
            }

            /// <summary>
            /// Get Project from Eml file.
            /// </summary>
            /// <param name="filepath"></param>
            /// <returns></returns>
            private static Project GetProjectFromEml(string filepath)
            {
                string name = Path.GetFileNameWithoutExtension(filepath);
                string comment = "";
                string time = File.GetLastWriteTime(filepath).ToString();

                return new Project(name, comment, time);
            }

        }

        /// <summary>
        /// Constants
        /// </summary>
        internal class MngPrjDlgConstants
        {
            /// <summary>
            /// Index of FolderIcon on TreeNode.
            /// </summary>
            public const int NodeGraphicsFolder = 0;
            /// <summary>
            /// Index of ProjectIcon on TreeNode.
            /// </summary>
            public const int NodeGraphicsProject = 1;
            /// <summary>
            /// Index of ModelIcon on TreeNode.
            /// </summary>
            public const int NodeGraphicsModel = 2;
            /// <summary>
            /// 
            /// </summary>
            public const string SaveZip = "SaveZip";
        }
    }
}