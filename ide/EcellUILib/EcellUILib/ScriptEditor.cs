//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Fireball.Syntax;
using Fireball.CodeEditor.SyntaxFiles;

namespace Ecell.IDE
{
    /// <summary>
    /// Form to edit the script.
    /// </summary>
    public class ScriptEditor : DMEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env">ApplicationEnvironment</param>
        public ScriptEditor(ApplicationEnvironment env)
        {
            m_env = env;
            CodeEditorSyntaxLoader.SetSyntax(codeEditorControl, SyntaxLanguage.Python);
            DMESaveButton.Enabled = true;
            DMESaveAsButton.Enabled = true;
        }

        /// <summary>
        /// set dm file path.
        /// </summary>
        public override string path
        {
            set
            {
                m_path = value;
                if (m_path != null)
                {
                    LoadFile();
                }
            }
            get
            {
                return m_path;
            }
        }

        /// <summary>
        /// Change the status of project.
        /// </summary>
        /// <param name="status">the project status.</param>
        public override void ChangeStatus(ProjectStatus status)
        {
        }

        /// <summary>
        /// The event sequence when this form is shown.
        /// </summary>
        /// <param name="sender">ScriptEditor</param>
        /// <param name="e">EventArgs</param>
        protected override void DMEditorShown(object sender, EventArgs e)
        {
            DMEComileButton.Text = MessageResources.NameExecute;

            if (m_path == null) return;
            LoadFile();
            DMEComileButton.Enabled = true;
        }

        /// <summary>
        /// The event sequence when the load button is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        protected override void DMELoadButtonClick(object sender, EventArgs e)
        {
            DMEOpenFileDialog.Filter = Constants.FilterEssFile;

            if (DMEOpenFileDialog.ShowDialog() != DialogResult.OK)
                return;

            codeEditorControl.Text = "";
            m_path = DMEOpenFileDialog.FileName;
            LoadFile();
            DMEComileButton.Enabled = true;
        }

        /// <summary>
        /// The event sequence when the comile button is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        protected override void DMECompileButtonClick(object sender, EventArgs e)
        {
            DMESaveButtonClick(DMESaveButton, e);
            DMEComileButton.Enabled = false;
            try
            {
                m_env.DataManager.ExecuteScript(m_path);
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrLoadFile,
                        new object[] { m_path }));
            }
            DMEComileButton.Enabled = true;
        }

        /// <summary>
        /// The event sequence when the save as button is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        protected override void DMESaveAsButton_Click(object sender, EventArgs e)
        {
            DMESaveFileDialog.Filter = Constants.FilterEssFile;
            if (DMESaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string path = DMESaveFileDialog.FileName;
            //StreamWriter writer = null;
            try
            {
                codeEditorControl.Save(path);
                //writer = new StreamWriter(path, false, Encoding.UTF8);
                //writer.Write(codeEditorControl.Text);
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
    }
}
