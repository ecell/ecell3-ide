//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// Form class to select the initial window setting.
    /// </summary>
    public partial class ProjectWizardWindow : Form
    {
        #region Fields
        /// <summary>
        /// The path of the initial window setting file.
        /// </summary>
        private ProjectInfo m_project = null;
        
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public ProjectInfo SelectedProject
        {
            get { return m_project; }
            set { m_project = value; }
        }

        /// <summary>
        /// Get the list of dm directory.
        /// </summary>
        /// <returns>the list of dm directory.</returns>
        public List<string> DMList
        {
            get
            {
                List<string> list = new List<string>();
                int len = DMListBox.Items.Count;
                for (int i = 0; i < len; i++)
                {
                    string dir = DMListBox.Items[i] as string;
                    if (dir == null) 
                        continue;
                    list.Add(dir);
                }
                return list;
            }
        }

        #endregion

        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectWizardWindow()
        {
            InitializeComponent();
            textBox1.Text = MessageResources.ProjectWizardSelectTemplete;
            LoadProjectTemplete();

            MainLayoutPanel.Controls.Remove(DMLayoutPanel);
            MainLayoutPanel.Controls.Add(ProjectLayoutPanel, 0, 1);

            Dictionary<string,List<string>> dmDic = Util.GetDmDic(null);

        }
        #endregion

        #region private Method
        /// <summary>
        /// Load the list of window setting.
        /// </summary>
        private void LoadProjectTemplete()
        {
            // Load Projects
            string path = Path.Combine(Util.GetWindowSettingDir(), "Templates");
            if (path == null || !Directory.Exists(path))
                return;
            string[] dirs = Directory.GetDirectories(path);
            int i = 0;
            foreach (string dir in dirs)
            {
                if (i >= 5)
                    break;
                string prjXMLFileName = Path.Combine(dir, Constants.fileProjectXML);
                // Check project.xml and load.
                if (!File.Exists(prjXMLFileName))
                    continue;

                ProjectInfo project = ProjectInfoLoader.Load(prjXMLFileName);
                ProjectLabel label = new ProjectLabel(project);
                label.Click += new EventHandler(label_Click);
                ProjectPatternList.Controls.Add(label, 0, i);
                i++;
            }
        }
        /// <summary>
        /// Event on click ProjectLabel.
        /// This event set the selected project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_Click(object sender, EventArgs e)
        {
            ProjectLabel label = (ProjectLabel)sender;
            m_project = label.Project;
            string filepath = Path.Combine(m_project.ProjectPath, "model.png");
            PictureBox.Image = Image.FromFile(filepath);
            ProjectIDTextBox.Text = m_project.Name;
            ProjectCommentTextBox.Text = m_project.Comment;
            OKButton.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            if (m_project == null)
                return;
            m_project.Name = ProjectIDTextBox.Text;
            m_project.Comment = ProjectCommentTextBox.Text;
            // Set Page
            SetNextPage();
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetNextPage()
        {
            textBox1.Text = MessageResources.ProjectWizardSelectDM;
            OKButton.Text = MessageResources.ProjectWizardCreate;
            OKButton.DialogResult = DialogResult.OK;
            DMRemoveButton.Visible = true;
            DMAddButon.Visible = true;
            MainLayoutPanel.Controls.Remove(ProjectLayoutPanel);
            ProjectLayoutPanel.Dock = DockStyle.Fill;
            MainLayoutPanel.Controls.Add(DMLayoutPanel, 0, 1);
            BackButton.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = MessageResources.ProjectWizardSelectTemplete;
            OKButton.Text = MessageResources.ProjectWizardGoForward;
            OKButton.DialogResult = DialogResult.None;
            DMAddButon.Visible = false;
            DMRemoveButton.Visible = false;
            MainLayoutPanel.Controls.Remove(DMLayoutPanel);
            MainLayoutPanel.Controls.Add(ProjectLayoutPanel, 0, 1);
            BackButton.Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DMAdd_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog win = new FolderBrowserDialog();
            win.Description = MessageResources.ExpModelMes;
            using (win)
            {
                if (win.ShowDialog() != DialogResult.OK)
                    return;

                if (DMListBox.Items.Contains(win.SelectedPath))
                    return;

                DMListBox.Items.Add(win.SelectedPath);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DMRemove_Click(object sender, EventArgs e)
        {
            while (DMListBox.SelectedIndex > -1)
            {
                DMListBox.Items.RemoveAt(DMListBox.SelectedIndex);
            }
        }

        #endregion


        #region Internal class
        /// <summary>
        /// Custom Label.
        /// </summary>
        private class ProjectLabel : Label
        {
            private ProjectInfo m_project;
            /// <summary>
            /// 
            /// </summary>
            public ProjectInfo Project
            {
                get { return m_project; }
                set { m_project = value; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="project"></param>
            public ProjectLabel(ProjectInfo project)
            {
                m_project = project;

                this.Text = project.Name;
                this.Font = new Font(Font.SystemFontName, 10);
                this.MouseHover += new EventHandler(label_MouseHover);
                this.MouseLeave += new EventHandler(label_MouseLeave);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void label_MouseLeave(object sender, EventArgs e)
            {
                Label label = (Label)sender;
                label.Font = new Font(label.Font, FontStyle.Regular);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void label_MouseHover(object sender, EventArgs e)
            {
                Label label = (Label)sender;
                label.Font = new Font(label.Font, FontStyle.Underline);
            }

        }
        #endregion

    }
}
