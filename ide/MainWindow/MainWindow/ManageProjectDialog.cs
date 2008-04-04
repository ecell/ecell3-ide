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
        #region Fields
        /// <summary>
        /// ResourceManager for MainWindow.
        /// </summary>
        private static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResMain));

        /// <summary>
        /// Selected Node.
        /// </summary>
        private ProjectTreeNode m_node = null;

        /// <summary>
        /// List of ToolStripMenuItems for ContextMenu
        /// </summary>
        private Dictionary<string, ToolStripItem> m_popMenuDict = new Dictionary<string, ToolStripItem>();

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
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public ManageProjectDialog()
        {
            InitializeComponent();
            MPPrjTreeView.ContextMenuStrip = CreatePopupMenus();
            MPOpenButton.Enabled = false;
        }
        
        #endregion

        #region Accessors
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
        
        #endregion

        #region Methods
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
        /// Create the project tree.
        /// Add the project file and directory on path to node TreeNode.
        /// </summary>
        /// <param name="node">The current TreeNode.</param>
        /// <param name="path">The current path.</param>
        public void CreateProjectTreeView(TreeNode node, string path)
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
            }
            // Check project.info and load.
            else if (File.Exists(prjFileName))
            {
                TreeNode childNode = new ProjectTreeNode(prjFileName);
                node.Nodes.Add(childNode);
            }
            else
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

                CreateProjectTreeView(childNode, dir);
            }
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
        /// <param name="m_node"></param>
        private void ResetPopupMenus(ProjectTreeNode m_node)
        {

        }

        #endregion

        #region Event
        /// <summary>
        /// Event to click the node by mouse.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs.</param>
        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView tView = (TreeView)sender;
            m_node = (ProjectTreeNode)tView.GetNodeAt(e.X, e.Y);

            // Reset selected project if project is null.
            if (m_node == null || m_node.Project == null)
            {
                ResetSelectedProject();
                return;
            }
            ResetPopupMenus(m_node);

            // Reflect Project parameters.
            Project prj = m_node.Project;
            m_fileName = m_node.FileName;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveZipClick(object sender, EventArgs e)
        {
            if (m_node == null)
                return;

            // Show SaveFileDialog and get saving filename.
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = Constants.FilterZipFile;
            if(dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            if (string.IsNullOrEmpty(filename))
                return;

            switch (m_node.NodeType)
            {
                case MngPrjDlgConstants.NodeTypeFolder:
                    ZipUtil.ZipFolder(filename, m_node.FileName);
                    break;
                case MngPrjDlgConstants.NodeTypeProject:
                    ZipUtil.ZipFolder(filename, Path.GetDirectoryName(m_node.FileName));
                    break;
                case MngPrjDlgConstants.NodeTypeModel:
                    ZipUtil.ZipFile(filename, m_node.FileName);
                    break;
            }
            
            dialog.Dispose();
        }

        #endregion

        /// <summary>
        /// ProjectTreeNode
        /// </summary>
        internal class ProjectTreeNode : TreeNode
        {
            #region Fields
            private string m_fileName = null;
            private int m_nodeType = 0;
            private Project m_project = null;
            #endregion

            #region Accessors
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
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="filepath"></param>
            public ProjectTreeNode(string filepath)
            {
                this.m_fileName = filepath;
                this.m_nodeType = GetNodeType(filepath);
                this.m_project = Project.LoadProject(filepath);
                if (m_project == null)
                    this.Text = Path.GetFileNameWithoutExtension(filepath);
                else
                    this.Text = m_project.Name;
                this.ImageIndex = m_nodeType;
                this.SelectedImageIndex = m_nodeType;
                this.Tag = filepath;
            }
            
            #endregion

            #region Methods
            /// <summary>
            /// Get NodeType
            /// </summary>
            /// <param name="filepath"></param>
            /// <returns></returns>
            private int GetNodeType(string filepath)
            {
                string ext = Path.GetExtension(filepath);
                if (ext.Equals(Constants.FileExtXML))
                    return MngPrjDlgConstants.NodeTypeProject;
                else if (ext.Equals(Constants.FileExtINFO))
                    return MngPrjDlgConstants.NodeTypeProject;
                else if (ext.Equals(Constants.FileExtEML))
                    return MngPrjDlgConstants.NodeTypeModel;
                else
                    return MngPrjDlgConstants.NodeTypeFolder;
            }
            #endregion
        }

        /// <summary>
        /// Constants
        /// </summary>
        internal class MngPrjDlgConstants
        {
            /// <summary>
            /// Index of FolderIcon on TreeNode.
            /// </summary>
            public const int NodeTypeFolder = 0;
            /// <summary>
            /// Index of ProjectIcon on TreeNode.
            /// </summary>
            public const int NodeTypeProject = 1;
            /// <summary>
            /// Index of ModelIcon on TreeNode.
            /// </summary>
            public const int NodeTypeModel = 2;
            /// <summary>
            /// 
            /// </summary>
            public const string SaveZip = "SaveZip";
        }
    }
}