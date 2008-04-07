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
        private ProjectTreeNode m_selectedNode = null;

        /// <summary>
        /// Copied Node.
        /// </summary>
        private ProjectTreeNode m_copiedNode = null;

        /// <summary>
        /// List of ToolStripMenuItems for ContextMenu
        /// </summary>
        private Dictionary<string, ToolStripItem> m_popMenuDict = new Dictionary<string, ToolStripItem>();

        private Project m_selectedProject = null;

        private string m_fileName = "";
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
        /// get the project file name.
        /// </summary>
        public string FileName
        {
            get { return this.m_fileName; }
        }

        /// <summary>
        /// get the project.
        /// </summary>
        public Project Project
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
        /// Set selected Project
        /// </summary>
        private void SetSelectedProject()
        {
            // Reflect Project parameters.
            Project prj = m_selectedNode.Project;
            m_selectedProject = prj;
            m_fileName = m_selectedNode.FilePath;
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
        /// Reset selected Project
        /// </summary>
        private void ResetSelectedProject()
        {
            m_selectedProject = null;

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
            savezip.Text = s_resources.GetString(PrjDlgConstants.MenuSaveZip);
            savezip.Click += new EventHandler(SaveZipClick);
            menus.Items.Add(savezip);
            m_popMenuDict.Add(PrjDlgConstants.MenuSaveZip, savezip);

            // Delete
            ToolStripItem delete = new ToolStripMenuItem(PrjDlgConstants.MenuDelete);
            delete.Name = PrjDlgConstants.MenuDelete;
            delete.Text = s_resources.GetString(PrjDlgConstants.MenuDelete);
            delete.Click += new EventHandler(DeleteClick);
            menus.Items.Add(delete);
            m_popMenuDict.Add(PrjDlgConstants.MenuDelete, delete);

            // CreateNewProject
            ToolStripItem createProject = new ToolStripMenuItem(PrjDlgConstants.MenuCreateNewProject);
            createProject.Name = PrjDlgConstants.MenuCreateNewProject;
            createProject.Text = s_resources.GetString(PrjDlgConstants.MenuCreateNewProject);
            createProject.Click += new EventHandler(CreateNewProjectClick);
            menus.Items.Add(createProject);
            m_popMenuDict.Add(PrjDlgConstants.MenuCreateNewProject, createProject);

            // CreateNewRevision
            ToolStripItem createRevision = new ToolStripMenuItem(PrjDlgConstants.MenuCreateNewRevision);
            createRevision.Name = PrjDlgConstants.MenuCreateNewRevision;
            createRevision.Text = s_resources.GetString(PrjDlgConstants.MenuCreateNewRevision);
            createRevision.Click += new EventHandler(CreateNewRevisionClick);
            menus.Items.Add(createRevision);
            m_popMenuDict.Add(PrjDlgConstants.MenuCreateNewRevision, createRevision);

            // Copy
            ToolStripItem copy = new ToolStripMenuItem(PrjDlgConstants.MenuCopy);
            copy.Name = PrjDlgConstants.MenuCopy;
            copy.Text = s_resources.GetString(PrjDlgConstants.MenuCopy);
            copy.Click += new EventHandler(CopyClick);
            menus.Items.Add(copy);
            m_popMenuDict.Add(PrjDlgConstants.MenuCopy, copy);

            // Delete
            ToolStripItem paste = new ToolStripMenuItem(PrjDlgConstants.MenuPaste);
            paste.Name = PrjDlgConstants.MenuPaste;
            paste.Text = s_resources.GetString(PrjDlgConstants.MenuPaste);
            paste.Click += new EventHandler(PasteClick);
            menus.Items.Add(paste);
            m_popMenuDict.Add(PrjDlgConstants.MenuPaste, paste);

            return menus;
        }

        /// <summary>
        /// Set Popup menu visibility.
        /// </summary>
        /// <param name="m_node"></param>
        private void ResetPopupMenus(ProjectTreeNode m_node)
        {
            // Set Visibility flags.
            bool isVisible = (m_node != null);
            bool isProject = false;
            bool isModel = false;
            bool isFolder = false;
            bool isCopied = (m_copiedNode != null);
            bool unfinished = false;
            if(isVisible)
            {
                isProject = (m_node.Type == FileType.Project);
                isModel = (m_node.Type == FileType.Model);
                isFolder = (m_node.Type == FileType.Folder);
            }

            // Set visibility.
            m_popMenuDict[PrjDlgConstants.MenuSaveZip].Visible = isVisible;
            m_popMenuDict[PrjDlgConstants.MenuCreateNewProject].Visible = isVisible && isFolder && unfinished;
            m_popMenuDict[PrjDlgConstants.MenuCreateNewRevision].Visible = isVisible && isProject;
            m_popMenuDict[PrjDlgConstants.MenuDelete].Visible = isVisible && (isFolder || isModel);
            m_popMenuDict[PrjDlgConstants.MenuCopy].Visible = isVisible;
            m_popMenuDict[PrjDlgConstants.MenuPaste].Visible = isVisible && isFolder && isCopied;
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

            string name = newPrjDialog.textName.Text;
            string model = newPrjDialog.textModelName.Text;
            string comment = newPrjDialog.textComment.Text;
            List<string> dmList = newPrjDialog.GetDmList();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(model))
                return;

            Project project = new Project(name, comment, DateTime.Now.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="path"></param>
        /// <param name="modelName"></param>
        /// <param name="dmList"></param>
        private void CreateNewProject(Project project, string path, string modelName, List<string> dmList)
        {
            Project.SaveProject(project, path);
            // DataManagerに新規プロジェクトを作成する関数を追加する。
            // 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewRevisionClick(object sender, EventArgs e)
        {
            string sourceDir = Path.GetDirectoryName(m_selectedNode.FilePath);
            string targetDir = Path.Combine(sourceDir, GetRevNo(sourceDir));
            foreach (string dir in ignoredDirList)
            {
                string tempdir = Path.Combine(sourceDir, dir);
                if (Directory.Exists(tempdir))
                    CopyDirectory(tempdir, Path.Combine(targetDir, dir));
            }
            string[] files = Directory.GetFiles(sourceDir, "project.*");
            foreach (string file in files)
                CopyFile(file, targetDir);

            TreeNode childNode = new ProjectTreeNode(targetDir);
            m_selectedNode.Nodes.Add(childNode);
            CreateProjectTreeView(childNode, targetDir);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <returns></returns>
        private static string GetRevNo(string sourceDir)
        {
            int revNo = 0;
            string revision = "";
            do
            {
                revNo++;
                revision = "Revision" + revNo.ToString();
            } while (Directory.Exists(Path.Combine(sourceDir, revision)));
            return revision;
        }

        /// <summary>
        /// Copy node.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyClick(object sender, EventArgs e)
        {
            m_copiedNode = m_selectedNode;
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
            if (path.Equals(targetPath))
                targetPath = GetNewDir(targetPath);

            // Copy Directory / File.
            switch (type)
            {
                case FileType.Project:
                case FileType.Folder:
                    CopyDirectory(path, targetPath);
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

        /// <summary>
        /// Copy Directory
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        public static void CopyDirectory(string sourceDir, string targetDir)
        {
            if (sourceDir.Equals(targetDir))
                targetDir = GetNewDir(targetDir);

            // List up directories and files.
            string[] dirs = System.IO.Directory.GetDirectories(sourceDir, "*.*", SearchOption.AllDirectories);
            string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

            // Create directory if necessary.
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
                File.SetAttributes(targetDir, File.GetAttributes(sourceDir));
            }
            // Copy directories.
            foreach (string dir in dirs)
                Directory.CreateDirectory(dir.Replace(sourceDir, targetDir));
            // Copy Files.
            foreach (string file in files)
                File.Copy(file, file.Replace(sourceDir, targetDir));
        }

        /// <summary>
        /// Get New Directory name.
        /// </summary>
        /// <param name="targetDir"></param>
        /// <returns></returns>
        private static string GetNewDir(string targetDir)
        {
            int revNo = 0;
            string newDir = "";
            do
            {
                revNo++;
                newDir = targetDir + revNo.ToString();
            } while (Directory.Exists(newDir));
            return newDir;
        }

        /// <summary>
        /// Copy File
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="targetDir"></param>
        public static void CopyFile(string filename, string targetDir)
        {
            File.Copy(filename, Path.Combine(targetDir, Path.GetFileName(filename)), true);
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
            private Project m_project = null;
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

            public Project Project
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
                this.m_project = Project.LoadProject(filepath);
                if (m_project == null)
                    this.Text = Path.GetFileNameWithoutExtension(filepath);
                else
                    this.Text = m_project.Name;
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
                if (ext.Equals(Constants.FileExtXML))
                    return FileType.Project;
                else if (ext.Equals(Constants.FileExtINFO))
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