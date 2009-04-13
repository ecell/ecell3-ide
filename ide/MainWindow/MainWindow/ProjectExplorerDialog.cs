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
        /// Copied Node.
        /// </summary>
        private ProjectTreeNode m_copiedNode = null;

        /// <summary>
        /// List of ToolStripMenuItems for ContextMenu
        /// </summary>
        private Dictionary<string, ToolStripItem> m_popMenuDict = new Dictionary<string, ToolStripItem>();

        /// <summary>
        /// Selected Project
        /// </summary>
        private ProjectInfo m_selectedProject = null;

        /// <summary>
        /// FileName
        /// </summary>
        private string m_fileName = "";
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public ProjectExplorerDialog(string dir)
        {
            InitializeComponent();
            PrjTreeView.ContextMenuStrip = CreatePopupMenus();
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

            string prjFileName = Path.Combine(path, Constants.fileProject);
            string prjXMLFileName = Path.Combine(path, Constants.fileProjectXML);

            // Check project.xml and load.
            if (File.Exists(prjXMLFileName))
            {
                if (!IsExistModelFile(path)) 
                    return;
                TreeNode childNode = new ProjectTreeNode(prjXMLFileName);
                node.Nodes.Add(childNode);
            }
            // Check project.info and load.
            else if (File.Exists(prjFileName))
            {
                if (!IsExistModelFile(path)) 
                    return;
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
            m_fileName = m_selectedNode.FilePath;
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

            projectNameText.BackColor = Color.White;
            dateText.BackColor = Color.White;
            commentText.BackColor = Color.White;

            openButton.Enabled = true;
            commentText.ReadOnly = false;
            projectNameText.ReadOnly = false;
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

            projectNameText.BackColor = Color.Silver;
            dateText.BackColor = Color.Silver;
            commentText.BackColor = Color.Silver;

            openButton.Enabled = false;
            commentText.ReadOnly = true;
            projectNameText.ReadOnly = true;
            m_fileName = "";

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
        }

        #endregion

        #region Menu Event
        /// <summary>
        /// Reset popup menus on MouseDown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MPPrjTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            ResetPopupMenus(null);
        }

        /// <summary>
        /// Event to click the node by mouse.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs.</param>
        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView tView = (TreeView)sender;
            m_selectedNode = (ProjectTreeNode)tView.GetNodeAt(e.X, e.Y);

            // Set menus.
            ResetPopupMenus(m_selectedNode);
            tView.SelectedNode = m_selectedNode;

            // Reset selected project if project is null.
            if (m_selectedNode.Project == null)
                ResetSelectedProject();
            else
                SetSelectedProject();
        }


        /// <summary>
        /// Event to click the node by mouse.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs.</param>
        private void NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView tView = (TreeView)sender;
            m_selectedNode = (ProjectTreeNode)tView.GetNodeAt(e.X, e.Y);
            if (m_selectedNode.Parent == null || string.IsNullOrEmpty(m_selectedNode.Text))
                return;
            m_selectedNode.BeginEdit();
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

        /// <summary>
        /// CreatePopupMenus
        /// </summary>
        /// <returns></returns>
        private ContextMenuStrip CreatePopupMenus()
        {
            // Preparing a context menu.
            ContextMenuStrip menus = new ContextMenuStrip();

            // SaveZip
            ToolStripItem savezip = new ToolStripMenuItem(PrjDlgConstants.MenuSaveZip);
            savezip.Name = PrjDlgConstants.MenuSaveZip;
            savezip.Text = MessageResources.MenuSaveZip;
            savezip.Click += new EventHandler(SaveZipClick);
            menus.Items.Add(savezip);
            m_popMenuDict.Add(PrjDlgConstants.MenuSaveZip, savezip);

            // Delete
            ToolStripItem delete = new ToolStripMenuItem(PrjDlgConstants.MenuDelete);
            delete.Name = PrjDlgConstants.MenuDelete;
            delete.Text = MessageResources.MenuDelete;
            delete.Click += new EventHandler(DeleteClick);
            menus.Items.Add(delete);
            m_popMenuDict.Add(PrjDlgConstants.MenuDelete, delete);

            // CreateNewProject
            ToolStripItem createProject = new ToolStripMenuItem(PrjDlgConstants.MenuCreateNewProject);
            createProject.Name = PrjDlgConstants.MenuCreateNewProject;
            createProject.Text = MessageResources.MenuCreateNewProject;
            createProject.Click += new EventHandler(CreateNewProjectClick);
            menus.Items.Add(createProject);
            m_popMenuDict.Add(PrjDlgConstants.MenuCreateNewProject, createProject);

            // CreateNewRevision
            ToolStripItem createRevision = new ToolStripMenuItem(PrjDlgConstants.MenuCreateNewRevision);
            createRevision.Name = PrjDlgConstants.MenuCreateNewRevision;
            createRevision.Text = MessageResources.MenuCreateNewRevision;
            createRevision.Click += new EventHandler(CreateNewRevisionClick);
            menus.Items.Add(createRevision);
            m_popMenuDict.Add(PrjDlgConstants.MenuCreateNewRevision, createRevision);

            // Copy
            ToolStripItem copy = new ToolStripMenuItem(PrjDlgConstants.MenuCopy);
            copy.Name = PrjDlgConstants.MenuCopy;
            copy.Text = MessageResources.MenuCopy;
            copy.Click += new EventHandler(CopyClick);
            menus.Items.Add(copy);
            m_popMenuDict.Add(PrjDlgConstants.MenuCopy, copy);

            // Delete
            ToolStripItem paste = new ToolStripMenuItem(PrjDlgConstants.MenuPaste);
            paste.Name = PrjDlgConstants.MenuPaste;
            paste.Text = MessageResources.MenuPaste;
            paste.Click += new EventHandler(PasteClick);
            menus.Items.Add(paste);
            m_popMenuDict.Add(PrjDlgConstants.MenuPaste, paste);

            return menus;
        }

        /// <summary>
        /// Set Popup menu visibility.
        /// </summary>
        /// <param name="node">The tree node of project.</param>
        private void ResetPopupMenus(ProjectTreeNode node)
        {
            // Set Visibility flags.
            if (node == PrjTreeView.Nodes[0])
                node = null;

            bool isVisible = (node != null);
            bool isProject = false;
            bool isModel = false;
            bool isFolder = false;
            bool isCopied = (m_copiedNode != null);
            bool unfinished = false;
            if(isVisible)
            {
                isProject = (node.Type == FileType.Project);
                isModel = (node.Type == FileType.Model);
                isFolder = (node.Type == FileType.Folder);
            }

            // Set visibility.
            m_popMenuDict[PrjDlgConstants.MenuSaveZip].Visible = isVisible;
            m_popMenuDict[PrjDlgConstants.MenuCreateNewProject].Visible = isVisible && isFolder && unfinished;
            m_popMenuDict[PrjDlgConstants.MenuCreateNewRevision].Visible = isVisible && isProject;
            m_popMenuDict[PrjDlgConstants.MenuDelete].Visible = isVisible && (isFolder || isModel);
            m_popMenuDict[PrjDlgConstants.MenuCopy].Visible = isVisible && isProject;
            m_popMenuDict[PrjDlgConstants.MenuPaste].Visible = isVisible && isFolder && isCopied && unfinished;
        }

        /// <summary>
        /// SaveZip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveZipClick(object sender, EventArgs e)
        {
            if (m_selectedNode == null)
                return;

            // Show SaveFileDialog and get saving filename.
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = Constants.FilterZipFile;
            if(dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            if (string.IsNullOrEmpty(filename))
                return;

            switch (m_selectedNode.Type)
            {
                case FileType.Folder:
                    ZipUtil.ZipFolder(filename, m_selectedNode.FilePath);
                    break;
                case FileType.Project:
                    ZipUtil.ZipFolder(filename, Path.GetDirectoryName(m_selectedNode.FilePath));
                    break;
                case FileType.Model:
                    ZipUtil.ZipFile(filename, m_selectedNode.FilePath);
                    break;
            }
            
            dialog.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewProjectClick(object sender, EventArgs e)
        {
            string path = m_selectedNode.FilePath;
            NewProjectDialog newPrjDialog = new NewProjectDialog();
            if (newPrjDialog.ShowDialog() != DialogResult.OK)
                return;

            string name = newPrjDialog.ProjectName;
            string model = newPrjDialog.Comment;
            string comment = newPrjDialog.ProjectName;

            ProjectInfo project = new ProjectInfo(name, comment, DateTime.Now.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewRevisionClick(object sender, EventArgs e)
        {
            string sourceDir = Path.GetDirectoryName(m_selectedNode.FilePath);
            string targetDir = Path.Combine(sourceDir, Util.GetRevNo(sourceDir));
            foreach (string dir in Util.IgnoredDirList)
            {
                string tempdir = Path.Combine(sourceDir, dir);
                if (Directory.Exists(tempdir))
                    Util.CopyDirectory(tempdir, Path.Combine(targetDir, dir));
            }
            string[] files = Directory.GetFiles(sourceDir, "project.*");
            foreach (string file in files)
                Util.CopyFile(file, targetDir);

            TreeNode childNode = new ProjectTreeNode(targetDir);
            m_selectedNode.Parent.Nodes.Add(childNode);
            CreateProjectTreeView(childNode, targetDir);
        }

        /// <summary>
        /// Copy node.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyClick(object sender, EventArgs e)
        {
            m_copiedNode = m_selectedNode;

            // RootNode.
            m_selectedNode = (ProjectTreeNode)PrjTreeView.Nodes[0];

            // Set NodeType. 
            FileType type = m_copiedNode.Type;
            // Set sourcePath.
            string path = m_copiedNode.FilePath;
            if (type == FileType.Project)
                path = Path.GetDirectoryName(path);
            // Set targetPath.
            string targetPath = Path.Combine(m_selectedNode.FilePath, Path.GetFileName(path));
            if (path.Equals(targetPath) || Directory.Exists(targetPath))
                targetPath = Util.GetNewDir(targetPath);

            // Copy Directory / File.
            switch (type)
            {
                case FileType.Project:
                case FileType.Folder:
                    Util.CopyDirectory(path, targetPath);
                    break;
                case FileType.Model:
                    File.Copy(path, targetPath, true);
                    break;
            }

            // Create new node
            TreeNode childNode = new ProjectTreeNode(targetPath);
            m_selectedNode.Nodes.Add(childNode);
            if (m_copiedNode.Type != FileType.Model)
            {
                CreateProjectTreeView(childNode, targetPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteClick(object sender, EventArgs e)
        {
            // Set NodeType. 
            FileType type = m_copiedNode.Type;
            // Set sourcePath.
            string path = m_copiedNode.FilePath;
            if(type == FileType.Project)
                path = Path.GetDirectoryName(path);
            // Set targetPath.
            string targetPath = Path.Combine(m_selectedNode.FilePath, Path.GetFileName(path));
            if (path.Equals(targetPath) || Directory.Exists(targetPath))
                targetPath = Util.GetNewDir(targetPath);

            // Copy Directory / File.
            switch (type)
            {
                case FileType.Project:
                case FileType.Folder:
                    Util.CopyDirectory(path, targetPath);
                    break;
                case FileType.Model:
                    File.Copy(path, targetPath, true);
                    break;
            }

            // Create new node
            TreeNode childNode = new ProjectTreeNode(targetPath);
            m_selectedNode.Nodes.Add(childNode);
            if (m_copiedNode.Type != FileType.Model)
            {
                CreateProjectTreeView(childNode, targetPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClick(object sender, EventArgs e)
        {
            if (m_selectedNode.Type == FileType.Folder)
                Directory.Delete(m_selectedNode.FilePath, true);
            else if (m_selectedNode.Type == FileType.Project)
                Directory.Delete(Path.GetDirectoryName(m_selectedNode.FilePath), true);
            else if (m_selectedNode.Type == FileType.Model)
                File.Delete(m_selectedNode.FilePath);
            m_selectedNode.Remove();
            m_selectedNode = null;
        }

        private void ProjectExplorerDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
        }

        private void PrjTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            m_selectedNode.Text = e.Label;
            RefreshNode(m_selectedNode);
        }

        internal void RefreshNode(ProjectTreeNode node)
        {
            // if null or not changed, reset Label.
            if (string.IsNullOrEmpty(node.Text))
            {
                if (node.Project != null)
                {
                    node.Text = node.Project.Name;
                }
                else
                {
                    node.Text = Path.GetFileNameWithoutExtension(node.FilePath);
                }
                return;
            }

            string name = node.Text;
            string path = node.FilePath;
            string dir = Path.GetDirectoryName(path);
            switch (node.Type)
            {
                case FileType.Folder:
                    string oldDir = path;
                    string newDir = Path.Combine(dir, name);
                    Directory.Move(oldDir, newDir);
                    node.FilePath = newDir;
                    node.Nodes.Clear();
                    CreateProjectTreeView(node, newDir);
                    break;

                case FileType.Model:
                    // rename eml.
                    string oldEml = path;
                    // get directory name.
                    string newEml = Path.Combine(dir, name + Constants.FileExtEML);
                    if (File.Exists(oldEml))
                    {
                        File.Move(oldEml, newEml);
                    }
                    // rename leml.
                    string oldLeml = oldEml.Replace(Constants.FileExtEML, Constants.FileExtLEML);
                    string newLeml = newEml.Replace(Constants.FileExtEML, Constants.FileExtLEML);
                    if (File.Exists(oldLeml))
                    {
                        File.Move(oldLeml, newLeml);
                    }
                    node.FilePath = newEml;
                    node.Project.Name = name;
                    SetSelectedProject();
                    break;

                case FileType.Project:
                    node.Project.Name = name;
                    node.Project.Save();
                    SetSelectedProject();
                    break;
            }
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
                else if (filepath.EndsWith(Constants.fileProject))
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
