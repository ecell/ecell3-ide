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
            get
            {
                m_project.Name = DMPanel.ProjectName.Text;
                m_project.Comment = DMPanel.Comment.Text;
                return m_project;
            }
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
                int len = DMPanel.DMListBox.Items.Count;
                for (int i = 0; i < len; i++)
                {
                    string dir = DMPanel.DMListBox.Items[i] as string;
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

            MainLayoutPanel.Controls.Remove(DMPanel);
            MainLayoutPanel.Controls.Add(ProjectPanel, 0, 1);
        }
        #endregion

        #region private Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProjectPanel_ProjectChange(object sender, System.EventArgs e)
        {
            m_project = ProjectPanel.Project;
            this.OKButton.Enabled = (ProjectPanel.Project != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoNext_Click(object sender, EventArgs e)
        {
            if (m_project == null)
                return;
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
            OKButton.Click -= GoNext_Click;
            MainLayoutPanel.Controls.Remove(ProjectPanel);
            DMPanel.Dock = DockStyle.Fill;
            MainLayoutPanel.Controls.Add(DMPanel, 0, 1);
            BackButton.Enabled = true;

            DMPanel.ProjectName.Text = Util.GetNewProjectName();
            DMPanel.Comment.Text = "Template:" + m_project.Name + "\r\nComment:" + m_project.Comment;
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
            OKButton.Click += GoNext_Click;
            MainLayoutPanel.Controls.Remove(DMPanel);
            MainLayoutPanel.Controls.Add(ProjectPanel, 0, 1);
            BackButton.Enabled = false;
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

        private void ProjectWizardWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
                return;
            string projectName = m_project.Name;
            string msg = string.Format(MessageResources.ErrExistProject, projectName)
                        + "\n" + MessageResources.ConfirmOverwrite;

            if (Util.IsNGforIDonWindows(projectName) || projectName.Length > 64)
            {
                Util.ShowWarningDialog(string.Format(MessageResources.ErrIDNG, "Project ID"));
                e.Cancel = true;
            }
            else if (Util.IsExistProject(projectName) && !Util.ShowOKCancelDialog(msg))
            {
                e.Cancel = true;
            }
            return;
        }
        #endregion


    }
}
