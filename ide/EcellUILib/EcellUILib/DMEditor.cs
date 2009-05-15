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

namespace Ecell.IDE
{
    /// <summary>
    /// Form class to display the source of DM.
    /// </summary>
    public partial class DMEditor : Form
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
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="env"></param>
        public DMEditor(string path, ApplicationEnvironment env)
        {
            m_path = path;
            InitializeComponent();
            m_env = env;
        }
        #endregion

        #region Events
        /// <summary>
        /// The event sequence when the close button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DMECloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The event sequence when this form is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DMEditorShown(object sender, EventArgs e)
        {
            if (!Util.IsInstalledSDK())
                DMEComileButton.Enabled = false;
            else
                DMEComileButton.Enabled = true;

            if (m_path == null) return;
            LoadFile();
        }

        /// <summary>
        /// Load the selected file.
        /// </summary>
        protected void LoadFile()
        {
            string line = "";
            DMETextBox.Text = "";
            TextReader l_reader = new StreamReader(m_path);
            while ((line = l_reader.ReadLine()) != null)
            {
                DMETextBox.Text += line + "\n";
            }
            l_reader.Close();
        }

        /// <summary>
        /// The event sequence when the save button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DMESaveButtonClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(m_path)) return;
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(m_path, false, Encoding.UTF8);
                writer.Write(DMETextBox.Text);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// The event sequence when the load button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DMELoadButtonClick(object sender, EventArgs e)
        {
            DMEOpenFileDialog.Filter = Constants.FilterDMFile;
            if (DMEOpenFileDialog.ShowDialog() != DialogResult.OK)
                return;

            DMETextBox.Text = "";
            m_path = DMEOpenFileDialog.FileName;
            LoadFile();
        }

        /// <summary>
        /// The event sequence when the comile button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DMECompileButtonClick(object sender, EventArgs e)
        {
            DMESaveButtonClick(DMESaveButton, e);
            DMCompiler.Compile(m_path, m_env);
        }

        private void DMEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;

        }

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
            
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(path, false, Encoding.UTF8);
                writer.Write(DMETextBox.Text);
                AddDMDelegate dlg = m_env.PluginManager.GetDelegate(Constants.delegateAddDM) as AddDMDelegate;
                if (dlg != null)
                    dlg(dmName, path);
                m_path = path;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
        #endregion
    }
}