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
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// Dialog to create the project.
    /// </summary>
    public partial class NewProjectDialog : Form
    {
        public string ProjectName
        {
            get { return textName.Text; }
            set { textName.Text = value; }
        }

        public string Comment
        {
            get { return textComment.Text; }
            set { textComment.Text = value; }
        }

        /// <summary>
        /// Get the list of dm directory.
        /// </summary>
        /// <returns>the list of dm directory.</returns>
        public List<string> DMList
        {
            get
            {
                List<string> result = new List<string>();
                int len = CPListBox.Items.Count;
                for (int i = 0; i < len; i++)
                {
                    string dir = CPListBox.Items[i] as string;
                    if (dir == null)
                        continue;
                    result.Add(dir);
                }
                return result;
            }
            set
            {
                foreach (string dm in value)
                {
                    CPListBox.Items.Add(dm);
                }
            }

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NewProjectDialog()
        {
            InitializeComponent();
            string prjName = Util.GetNewProjectName();
            textName.Text = prjName;
            FormClosing += new FormClosingEventHandler(NewProjectDialog_FormClosing);
        }

        /// <summary>
        /// Event process when user enter return on TextBox.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">KeyPressEventArgs.</param>
        private void EnterKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPCreateButton.PerformClick();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                CPCancelButton.PerformClick();
            }
        }

        /// <summary>
        /// Event process when this dialog is shown.
        /// </summary>
        /// <param name="sender">this dialog.</param>
        /// <param name="e">EventArgs.</param>
        private void NewProjectDialogShown(object sender, EventArgs e)
        {
            this.textName.Focus();
        }

        /// <summary>
        /// Event when Remove Button is clicked to remove the selected dm directory.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void ClickRemoveButton(object sender, EventArgs e)
        {
            int ind = CPListBox.SelectedIndex;
            if (ind < 0) return;
            CPListBox.Items.RemoveAt(ind);
        }

        /// <summary>
        /// Event when Add Button is cliecked to add the dm directory.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void ClickAddButton(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = MessageResources.SelectDMDir;
            using (fbd)
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    CPListBox.Items.Add(fbd.SelectedPath);
                }
            }
        }

        private void NewProjectDialog_FormClosing(object obj, FormClosingEventArgs args)
        {
            if (DialogResult == DialogResult.OK && !ValidateForm())
            {
                args.Cancel = true;
                return;
            }
        }

        private bool ValidateForm()
        {
            return ValidateProjectName() && ValidateModelName();
        }

        private bool ValidateProjectName()
        {
            string projectName = textName.Text;
            if (string.IsNullOrEmpty(projectName))
            {
                Util.ShowWarningDialog(string.Format(MessageResources.ErrNoSet, "Project ID"));
                return false;
            }
            if (Util.IsExistProject(projectName)
                && !Util.ShowOKCancelDialog(
                string.Format(MessageResources.ErrExistProject, projectName)
                + "\n" + MessageResources.ConfirmOverwrite)
                )
            {
                return false;
            }
            if (Util.IsNGforIDonWindows(projectName) || projectName.Length > 64)
            {
                Util.ShowWarningDialog(string.Format(MessageResources.ErrIDNG, "Project ID"));
                return false;
            }
            return true;
        }

        private bool ValidateModelName()
        {
            string modelName = textName.Text;
            if (String.IsNullOrEmpty(modelName))
            {
                Util.ShowWarningDialog(String.Format(MessageResources.ErrNoSet,
                    new object[] { "Model ID" }));
                return false;
            }
            if (modelName.Length > 64 || Util.IsNGforIDonWindows(modelName))
            {
                Util.ShowWarningDialog(String.Format(MessageResources.ErrIDNG,
                    new object[] { "Model ID" }));
                return false;
            }
            return true;
        }
    }
}
