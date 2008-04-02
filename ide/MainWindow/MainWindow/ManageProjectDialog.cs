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
            ToolStripItem savezip = new ToolStripMenuItem(MenuConstants.SaveZip);
            savezip.Name = MenuConstants.SaveZip;
            savezip.Text = s_resources.GetString(MenuConstants.SaveZip);
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
                node = new TreeNode(path);
                node.Tag = null;
                node.ImageIndex = 0;
                node.SelectedImageIndex = node.ImageIndex;
                MPPrjTreeView.Nodes.Add(node);
            }

            string prjFileName = Path.Combine(path, Constants.fileProject);
            string prjXMLFileName = Path.Combine(path, Constants.fileProjectXML);

            // Check project.xml and load.
            if (File.Exists(prjXMLFileName))
            {
                Project prj = GetProjectXML(prjXMLFileName);
                TreeNode p = new TreeNode(prj.Name);
                p.Tag = prjFileName;
                p.ImageIndex = 1;
                p.SelectedImageIndex = p.ImageIndex;
                node.Nodes.Add(p);
                isProject = true;
            }
            // Check project.info and load.
            else if (File.Exists(prjFileName))
            {
                Project prj = GetProject(prjFileName);
                TreeNode p = new TreeNode(prj.Name);
                p.Tag = prjFileName;
                p.ImageIndex = 1;
                p.SelectedImageIndex = p.ImageIndex;
                node.Nodes.Add(p);
                isProject = true;
            }
            else if (isProject == false)
            {
                string[] files = Directory.GetFiles(path, "*.eml");
                foreach (string file in files)
                {
                    string modelName = Path.GetFileNameWithoutExtension(file);
                    TreeNode p = new TreeNode(modelName);
                    p.Tag = file;
                    p.ImageIndex = 2;
                    p.SelectedImageIndex = p.ImageIndex;
                    node.Nodes.Add(p);
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

                TreeNode p = new TreeNode(name);
                p.Tag = null;
                p.ImageIndex = 0;
                p.SelectedImageIndex = p.ImageIndex;
                node.Nodes.Add(p);

                CreateProjectTreeView(p, dir, isProject);
            }
        }

        /// <summary>
        /// Event to click the node by mouse.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs.</param>
        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DataManager manager = DataManager.GetDataManager();
            TreeView t = (TreeView)sender;
            if (t == null) return;
            TreeNode node = t.GetNodeAt(e.X, e.Y);
            if (node == null) return;
            if (node.Tag == null)
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
                return;
            }

            String filename = node.Tag as String;
            if (filename == null) return;

            if (filename.EndsWith("eml"))
            {
                m_fileName = filename;
                MPPrjIDText.Text = Constants.defaultPrjID;
                MPPrjDateText.Text = "";
                MPPrjCommentText.Text = Constants.defaultComment;

                MPPrjIDText.BackColor = Color.White;
                MPPrjDateText.BackColor = Color.Silver;
                MPPrjCommentText.BackColor = Color.White;

                MPOpenButton.Enabled = true;
                MPPrjCommentText.ReadOnly = false;
                MPPrjIDText.ReadOnly = false;
            }
            else
            {
                Project prj = GetProject(filename);
                m_fileName = filename;
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
        }

        /// <summary>
        /// Get the project information from the project file.
        /// </summary>
        /// <param name="fileName">the project file name.</param>
        /// <returns>project information.</returns>
        private Project GetProject(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            string line = "";
            string comment = "";

            string dirPathName = Path.GetDirectoryName(fileName);
            string prjName = Path.GetFileName(dirPathName);
            TextReader l_reader = new StreamReader(fileName);
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
                    m_simName = line;
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
            m_comment = comment;
            m_prjID = prjName;
            return new Project(prjName, comment, File.GetLastWriteTime(fileName).ToString());
        }

        /// <summary>
        /// Get the project information from the project file.
        /// </summary>
        /// <param name="fileName">the project file name.</param>
        /// <returns>project information.</returns>
        private Project GetProjectXML(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            string dirPathName = Path.GetDirectoryName(fileName);
            string prjName = Path.GetFileName(dirPathName);
            string comment = "";
            string time = "";
            string param = "";

            try
            {
                // Load XML file
                XmlDocument xmlD = new XmlDocument();
                xmlD.Load(fileName);

                XmlNode settings = GetProjectSetting(xmlD);
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
                string errmsg = "ErrLoadProjectSettings" + Environment.NewLine + fileName + Environment.NewLine + ex.Message;
                MessageBox.Show(errmsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Project(prjName, comment, time);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlD"></param>
        /// <returns></returns>
        private static XmlNode GetProjectSetting(XmlDocument xmlD)
        {
            XmlNode settings = null;
            foreach (XmlNode node in xmlD.ChildNodes)
            {
                if (node.Name.Equals(Constants.xPathEcellProject))
                    settings = node;
            }
            return settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveZipClick(object sender, EventArgs e)
        {

        }

    }

    internal class MenuConstants
    {
        public const string SaveZip = "SaveZip";
    }
}