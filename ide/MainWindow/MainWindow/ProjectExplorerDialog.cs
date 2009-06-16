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

namespace Ecell.IDE.MainWindow
{

    /// <summary>
    /// Dialog to select the opened project.
    /// </summary>
    public partial class ProjectExplorerDialog : Form
    {
        #region Fields
        /// <summary>
        /// Selected Node.
        /// </summary>
        private ProjectTreeNode m_selectedNode = null;

        /// <summary>
        /// Selected Project
        /// </summary>
        private ProjectInfo m_selectedProject = null;
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public ProjectExplorerDialog(string dir)
        {
            InitializeComponent();
            openButton.Enabled = false;
            CreateProjectTreeView(null, dir);
        }
        
        #endregion

        #region Accessors
        /// <summary>
        /// get the project.
        /// </summary>
        public ProjectInfo Project
        {
            get { return this.m_selectedProject; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Create the project tree.
        /// Add the project file and directory on path to node TreeNode.
        /// </summary>
        /// <param name="node">The current TreeNode.</param>
        /// <param name="path">The current path.</param>
        public void CreateProjectTreeView(TreeNode node, string path)
        {
            if (!Directory.Exists(path))
                return;

            if (node == null)
            {
                node = new ProjectTreeNode(path);
                PrjTreeView.Nodes.Add(node);
                node.Expand();
            }

            // Check project.xml and load.
            string prjXMLFileName = Path.Combine(path, Constants.fileProjectXML);
            if (File.Exists(prjXMLFileName))
            {
                if (!IsExistModelFile(path)) 
                    return;
                TreeNode childNode = new ProjectTreeNode(prjXMLFileName);
                node.Nodes.Add(childNode);
            }
            // Create eml node.
            else if (path.Length < 248)
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
                if (Util.IsIgnoredDir(dir) || Util.IsHidden(dir))
                    continue;

                ProjectTreeNode childNode = new ProjectTreeNode(dir);
                node.Nodes.Add(childNode);

                CreateProjectTreeView(childNode, dir);
            }
        }

        private static bool IsExistModelFile(string dir)
        {
            string path = Path.Combine(dir, "Model");
            if (!Directory.Exists(path)) return false;

            string[] files = Directory.GetFiles(path, "*.eml");
            if (files.Length > 0) return true;
            return false;
        }

        /// <summary>
        /// Set selected Project
        /// </summary>
        private void SetSelectedProject()
        {
            // Reflect Project parameters.
            ProjectInfo prj = m_selectedNode.Project;
            m_selectedProject = prj;

            projectNameText.Text = prj.Name;
            dateText.Text = prj.UpdateTime;
            commentText.Text = prj.Comment;

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
            if (prj.ProjectPath != null)
            {
                string filepath = Path.Combine(prj.ProjectPath, "model.png");
                if (File.Exists(filepath))
                    pictureBox1.Image = Image.FromFile(filepath);
            }
            openButton.Enabled = true;
        }

        /// <summary>
        /// Reset selected Project
        /// </summary>
        private void ResetSelectedProject()
        {
            m_selectedProject = null;

            projectNameText.Text = "";
            dateText.Text = "";
            commentText.Text = "";

            openButton.Enabled = false;

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
        }

        #endregion

        #region Menu Event


        /// <summary>
        /// Event to click the node by mouse.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs.</param>
        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView tView = (TreeView)sender;
            m_selectedNode = (ProjectTreeNode)tView.GetNodeAt(e.X, e.Y);
            tView.SelectedNode = m_selectedNode;

            // Reset selected project if project is null.
            if (m_selectedNode.Project == null)
                ResetSelectedProject();
            else
                SetSelectedProject();
        }

        /// <summary>
        /// Constants
        /// </summary>
        internal class PrjDlgConstants
        {
            /// <summary>
            /// Save zip
            /// </summary>
            public const string MenuSaveZip = "MenuSaveZip";
            /// <summary>
            /// Delete
            /// </summary>
            public const string MenuDelete = "MenuDelete";
            /// <summary>
            /// CreateNewProject
            /// </summary>
            public const string MenuCreateNewProject = "MenuCreateNewProject";
            /// <summary>
            /// CreateNewRevision
            /// </summary>
            public const string MenuCreateNewRevision = "MenuCreateNewRevision";
            /// <summary>
            /// CopyProject
            /// </summary>
            public const string MenuCopy = "MenuCopy";
            /// <summary>
            /// PasteProject
            /// </summary>
            public const string MenuPaste = "MenuPaste";
        }

        private void ProjectExplorerDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
        }
        #endregion

        /// <summary>
        /// ProjectTreeNode
        /// </summary>
        internal class ProjectTreeNode : TreeNode
        {
            #region Fields
            private string m_filePath = null;
            private FileType m_nodeType = FileType.Folder;
            private ProjectInfo m_project = null;
            #endregion

            #region Accessors
            /// <summary>
            /// filename
            /// </summary>
            public string FilePath
            {
                get { return m_filePath; }
                set { m_filePath = value; }
            }

            public ProjectInfo Project
            {
                get { return m_project; }
                set { m_project = value; }
            }
            /// <summary>
            /// Type of node.
            /// </summary>
            public FileType Type
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
                this.m_filePath = filepath;
                this.m_nodeType = GetNodeType(filepath);
                if (this.m_nodeType == FileType.Project ||
                    this.m_nodeType == FileType.Model)
                {
                    this.m_project = ProjectInfoLoader.Load(filepath);
                    this.Text = m_project.Name;
                }
                else 
                    this.Text = Path.GetFileName(filepath);
                
                this.ImageIndex = (int)m_nodeType;
                this.SelectedImageIndex = ImageIndex;
                this.Tag = filepath;
            }
            
            #endregion

            #region Methods
            /// <summary>
            /// Get NodeType
            /// </summary>
            /// <param name="filepath"></param>
            /// <returns></returns>
            private FileType GetNodeType(string filepath)
            {
                string ext = Path.GetExtension(filepath);
                if (filepath.EndsWith(Constants.fileProjectXML))
                    return FileType.Project;
                else if (ext.Equals(Constants.FileExtEML))
                    return FileType.Model;
                else
                    return FileType.Folder;
            }
            #endregion
        }
    }
}
