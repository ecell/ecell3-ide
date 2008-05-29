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

namespace EcellLib.MainWindow
{
    /// <summary>
    /// Form class to select the initial window setting.
    /// </summary>
    public partial class ProjectWizardWindow : Form
    {
        /// <summary>
        /// The path of the initial window setting file.
        /// </summary>
        private Project m_selectedProject = null;
        /// <summary>
        /// 
        /// </summary>
        public Project SelectedProject
        {
            get { return m_selectedProject; }
            set { m_selectedProject = value; }
        }
        /// <summary>
        /// Constructor/
        /// </summary>
        public ProjectWizardWindow()
        {
            InitializeComponent();
            LoadProjectTemplete();
        }

        /// <summary>
        /// Load the list of window setting.
        /// </summary>
        private void LoadProjectTemplete()
        {
            // Load Projects
            string path = Util.GetWindowSettingDir();
            if (path == null)
                return;
            string[] dirs = Directory.GetDirectories(path);
            int i = 0;
            foreach (string dir in dirs)
            {
                string prjXMLFileName = Path.Combine(dir, Constants.fileProjectXML);
                // Check project.xml and load.
                if (!File.Exists(prjXMLFileName))
                    continue;

                Project project = new Project(prjXMLFileName);
                ProjectLabel label = new ProjectLabel(project);
                label.Click += new EventHandler(label_Click);
                SWSPatternListLayoutPanel.Controls.Add(label, 0, i);
            }
        }
        /// <summary>
        /// Event on click ProjectLabel.
        /// This event set the selected project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void label_Click(object sender, EventArgs e)
        {
            ProjectLabel label = (ProjectLabel)sender;
            m_selectedProject = label.Project;
            string filepath = Path.Combine(m_selectedProject.ProjectPath, m_selectedProject.Name + Constants.FileExtPNG);
            SWSPictureBox.Image = Image.FromFile(filepath); 
        }

        #region Internal class
        /// <summary>
        /// Custom Label.
        /// </summary>
        private class ProjectLabel : Label
        {
            private Project m_project;
            /// <summary>
            /// 
            /// </summary>
            public Project Project
            {
                get { return m_project; }
                set { m_project = value; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="project"></param>
            public ProjectLabel(Project project)
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
