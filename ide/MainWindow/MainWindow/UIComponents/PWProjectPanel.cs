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
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Ecell.IDE.MainWindow.UIComponents
{
    /// <summary>
    /// Project wizard panel class.
    /// </summary>
    public class PWProjectPanel : UserControl
    {
        #region Fields
        public ListBox ProjectListBox;
        private PictureBox PictureBox;
        private GroupBox ProjectBox;
        public TextBox IDTextBox;
        private GroupBox CommentBox;
        public TextBox CommentTextBox;
        private Label ProjectListLabel;
        private ProjectInfo m_project;
        #endregion

        /// <summary>
        /// get / set the project wizard.
        /// </summary>
        public ProjectInfo Project
        {
            get { return m_project; }
            set { m_project = value; }
        }
    
        /// <summary>
        /// InitializeComponent
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PWProjectPanel));
            this.ProjectListBox = new System.Windows.Forms.ListBox();
            this.ProjectListLabel = new System.Windows.Forms.Label();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.ProjectBox = new System.Windows.Forms.GroupBox();
            this.IDTextBox = new System.Windows.Forms.TextBox();
            this.CommentBox = new System.Windows.Forms.GroupBox();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.ProjectBox.SuspendLayout();
            this.CommentBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProjectListBox
            // 
            resources.ApplyResources(this.ProjectListBox, "ProjectListBox");
            this.ProjectListBox.FormattingEnabled = true;
            this.ProjectListBox.Name = "ProjectListBox";
            this.ProjectListBox.SelectedIndexChanged += new System.EventHandler(this.ProjectListBox_SelectedIndexChanged);
            // 
            // ProjectListLabel
            // 
            resources.ApplyResources(this.ProjectListLabel, "ProjectListLabel");
            this.ProjectListLabel.Name = "ProjectListLabel";
            // 
            // PictureBox
            // 
            resources.ApplyResources(this.PictureBox, "PictureBox");
            this.PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.TabStop = false;
            // 
            // ProjectBox
            // 
            resources.ApplyResources(this.ProjectBox, "ProjectBox");
            this.ProjectBox.Controls.Add(this.IDTextBox);
            this.ProjectBox.Name = "ProjectBox";
            this.ProjectBox.TabStop = false;
            // 
            // IDTextBox
            // 
            this.IDTextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.IDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.IDTextBox, "IDTextBox");
            this.IDTextBox.Name = "IDTextBox";
            this.IDTextBox.ReadOnly = true;
            this.IDTextBox.TabStop = false;
            // 
            // CommentBox
            // 
            resources.ApplyResources(this.CommentBox, "CommentBox");
            this.CommentBox.Controls.Add(this.CommentTextBox);
            this.CommentBox.Name = "CommentBox";
            this.CommentBox.TabStop = false;
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.CommentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.CommentTextBox, "CommentTextBox");
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.ReadOnly = true;
            this.CommentTextBox.TabStop = false;
            // 
            // PWProjectPanel
            // 
            this.Controls.Add(this.CommentBox);
            this.Controls.Add(this.ProjectBox);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.ProjectListBox);
            this.Controls.Add(this.ProjectListLabel);
            this.Name = "PWProjectPanel";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ProjectBox.ResumeLayout(false);
            this.ProjectBox.PerformLayout();
            this.CommentBox.ResumeLayout(false);
            this.CommentBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PWProjectPanel()
        {
            InitializeComponent();
            LoadProjectTemplates();
        }

        /// <summary>
        /// Load the list of window setting.
        /// </summary>
        private void LoadProjectTemplates()
        {
            // Load Projects
            string path = Path.Combine(Util.GetWindowSettingDir(), "Templates");
            if (path == null || !Directory.Exists(path))
                return;
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                string prjXMLFileName = Path.Combine(dir, Constants.fileProjectXML);
                // Check project.xml and load.
                if (!File.Exists(prjXMLFileName))
                    continue;

                ProjectInfo project = ProjectInfoLoader.Load(prjXMLFileName);
                ProjectListBox.Items.Add(project);
            }
        }

        /// <summary>
        /// The selection changed of project list.
        /// </summary>
        /// <param name="sender">ListBox</param>
        /// <param name="e">EventArgs</param>
        private void ProjectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ProjectListBox.SelectedIndex;
            ProjectInfo project = null;
            if (index >= 0)
                project = (ProjectInfo)ProjectListBox.SelectedItem;

            SetProject(project); 

        }

        /// <summary>
        /// Set the project.
        /// </summary>
        /// <param name="project">the project information object.</param>
        private void SetProject(ProjectInfo project)
        {
            m_project = project;
            // If null
            if (project == null)
            {
                PictureBox.Image = null;
                IDTextBox.Text = null;
                CommentTextBox.Text = null;
            }
            // else
            else
            {
                string filepath = Path.Combine(project.ProjectPath, "model.png");
                PictureBox.Image = Image.FromFile(filepath);
                IDTextBox.Text = project.Name;
                CommentTextBox.Text = project.Comment;
            }
            RaiseProjectChange();
        }

        #region EventHandler for ProjectChange
        private EventHandler m_onProjectChange;
        /// <summary>
        /// Event on canvas change.
        /// </summary>
        public event EventHandler ProjectChange
        {
            add { m_onProjectChange += value; }
            remove { m_onProjectChange -= value; }
        }
        /// <summary>
        /// Event on canvas change.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected virtual void OnProjectChange(EventArgs e)
        {
            if (m_onProjectChange != null)
                m_onProjectChange(this, e);
        }
        /// <summary>
        /// Raise the event on canvas change.
        /// </summary>
        private void RaiseProjectChange()
        {
            EventArgs e = new EventArgs();
            OnProjectChange(e);
        }
        #endregion

    }
}
