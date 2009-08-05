//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
using System.IO;
using System.Text;
using System.Windows.Forms;

using Ecell.Plugin;

using Fireball.Syntax;
using Fireball.CodeEditor.SyntaxFiles;

namespace Ecell.IDE
{
    /// <summary>
    /// Form class to display the source of DM.
    /// </summary>
    public partial class DMEditor : EcellDockContent
    {
        #region Fileds
        /// <summary>
        /// The path of loaded file.
        /// </summary>
        protected string m_path;
        /// <summary>
        /// Application Environment object.
        /// </summary>
        protected ApplicationEnvironment m_env;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public DMEditor()
        {
            InitializeComponent();
            m_path = null;
            CodeEditorSyntaxLoader.SetSyntax(codeEditorControl, SyntaxLanguage.CPP);
        }
        #endregion

        /// <summary>
        /// set Application Environment.
        /// </summary>
        public ApplicationEnvironment Environment
        {
            set { this.m_env = value; }
        }

        /// <summary>
        /// set dm file path.
        /// </summary>
        public string path
        {
            set
            {                
                m_path = value;
                if (m_path != null)
                {
                    LoadFile();
                    DMESaveButton.Enabled = true;
                    DMESaveAsButton.Enabled = true;
                    if (!Util.IsInstalledSDK())
                        DMEComileButton.Enabled = false;
                    else
                        DMEComileButton.Enabled = true;
                }
                else
                {
                    DMESaveButton.Enabled = false;
                    DMESaveAsButton.Enabled = false;
                    DMEComileButton.Enabled = false;
                    codeEditorControl.Text = "";
                }
            }
        }

        /// <summary>
        /// Change the status of project.
        /// </summary>
        /// <param name="status">the status of project.</param>
        public void ChangeStatus(ProjectStatus status)
        {
            if (status == ProjectStatus.Uninitialized)
                DMEComileButton.Enabled = false;
            else
                DMEComileButton.Enabled = true;
        }

        #region Events
        /// <summary>
        /// The event sequence when the close button is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void DMECloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The event sequence when this form is shown.
        /// </summary>
        /// <param name="sender">DMEditor</param>
        /// <param name="e">EventArgs</param>
        protected virtual void DMEditorShown(object sender, EventArgs e)
        {
            if (m_path == null) return;
        }

        /// <summary>
        /// Load the selected file.
        /// </summary>
        protected void LoadFile()
        {
            codeEditorControl.Open(m_path);
            //string line = "";
            //codeEditorControl.Text = "";
            //TextReader l_reader = new StreamReader(m_path);
            //while ((line = l_reader.ReadLine()) != null)
            //{
            //    codeEditorControl.Text += line + "\n";
            //}
            //l_reader.Close();
        }

        /// <summary>
        /// The event sequence when the save button is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        protected void DMESaveButtonClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(m_path))
            {
                DMESaveAsButton_Click(sender, e);
                return;
            }
            //StreamWriter writer = null;
            try
            {
                codeEditorControl.Save();
                //writer = new StreamWriter(m_path, false, Encoding.UTF8);
                //writer.Write(codeEditorControl.Text);
            }
            finally
            {
                //if (writer != null)
                //{
                //    writer.Close();
                //}
            }
        }

        /// <summary>
        /// The event sequence when the load button is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        protected virtual void DMELoadButtonClick(object sender, EventArgs e)
        {
            DMEOpenFileDialog.Filter = Constants.FilterDMFile;
            if (DMEOpenFileDialog.ShowDialog() != DialogResult.OK)
                return;

            codeEditorControl.Text = "";
            m_path = DMEOpenFileDialog.FileName;
            fileNameLabel.Text = m_path;
            LoadFile();
        }

        /// <summary>
        /// The event sequence when the comile button is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        protected virtual void DMECompileButtonClick(object sender, EventArgs e)
        {
            DMESaveButtonClick(DMESaveButton, e);
            DMCompiler.Compile(m_path, m_env);
        }

        /// <summary>
        /// The event sequence when the save as button is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        protected virtual void DMESaveAsButton_Click(object sender, EventArgs e)
        {
            DMESaveFileDialog.Filter = Constants.FilterDMFile;           
            if (DMESaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string path = DMESaveFileDialog.FileName;
            string dmName = Path.GetFileNameWithoutExtension(path);

            if (!dmName.EndsWith(Constants.xpathProcess) &&
                !dmName.EndsWith(Constants.xpathStepper))
            {
                Util.ShowWarningDialog(MessageResources.WarnDMName);
                return;
            }
            
            //StreamWriter writer = null;
            try
            {
                codeEditorControl.Save(path);
                //writer = new StreamWriter(path, false, Encoding.UTF8);
                //writer.Write(codeEditorControl.Text);
                AddDMDelegate dlg = m_env.PluginManager.GetDelegate(Constants.delegateAddDM) as AddDMDelegate;
                if (dlg != null)
                    dlg(dmName, path);
                m_path = path;
                fileNameLabel.Text = m_path;
            }
            finally
            {
                //if (writer != null)
                //{
                //    writer.Close();
                //}
            }
        }
        #endregion
    }
}