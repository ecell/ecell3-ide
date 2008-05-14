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
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EcellLib.SessionManager;

namespace EcellLib.MainWindow
{
    /// <summary>
    /// Form to setup the distributed environment.
    /// </summary>
    public partial class DistributedEnvSetupWindow : Form
    {
        #region Fields
        /// <summary>
        /// Dictionary of jobID and the job object.
        /// </summary>
        private Dictionary<string, object> m_propDict = new Dictionary<string, object>();
        /// <summary>
        /// SessionManager object.
        /// </summary>
        private ISessionManager m_manager = null;
        #endregion

        #region Events
        /// <summary>
        /// Constructor.
        /// </summary>
        public DistributedEnvSetupWindow(ISessionManager manager)
        {
            m_manager = manager;
            InitializeComponent();
        }

        /// <summary>
        /// The event when this form is shown.
        /// Display the property of default distributed environment.
        /// </summary>
        /// <param name="sender">this form.</param>
        /// <param name="e">EventArgs.</param>
        public void WindowShown(object sender, EventArgs e)
        {
            List<string> list = m_manager.GetEnvironmentList();
            string envName = m_manager.GetCurrentEnvironment();
            foreach (string env in list)
            {
                DEEnvComboBox.Items.Add(env);
            }
            DEEnvComboBox.SelectedText = envName;
            DEConcTextBox.Text = Convert.ToString(m_manager.GetDefaultConcurrency());

            m_propDict = m_manager.GetDefaultEnvironmentProperty(envName);
            if (m_propDict != null)
            {
                foreach (string propName in m_propDict.Keys)
                {
                    DEOptionGridView.Rows.Add(new object[] { propName, m_propDict[propName].ToString() });
                }
            }
            DEEnvComboBox.SelectedIndexChanged += new EventHandler(DEEnvComboBox_SelectedIndexChanged);
        }

        /// <summary>
        /// Event when the distributed environment is changed.
        /// Get the property of selected distributed environment from SessionManager.
        /// </summary>
        /// <param name="sender">ComboBox.</param>
        /// <param name="e">EventArgs.</param>
        private void DEEnvComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string envName = DEEnvComboBox.Text;

            int dConv = m_manager.GetDefaultConcurrency(envName);
            DEConcTextBox.Text = Convert.ToString(dConv);
            DEOptionGridView.Rows.Clear();
            m_propDict = m_manager.GetDefaultEnvironmentProperty(envName);
            if (m_propDict != null)
            {
                foreach (string propName in m_propDict.Keys)
                {
                    DEOptionGridView.Rows.Add(new object[] { propName, m_propDict[propName].ToString() });
                }
            }
        }

        /// <summary>
        /// Event when the close button is clicked.
        /// Close this form.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void DECloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event when the apply button is clicked.
        /// Set the property of distributed environment to SessionManager.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void DEApplyButton_Click(object sender, EventArgs e)
        {
            try
            {
                int conc = Convert.ToInt32(DEConcTextBox.Text);
                if (conc <= 0)
                {
                    string errmes = MainWindow.s_resources.GetString(MessageConstants.ErrConcInvalid);
                    Util.ShowErrorDialog(errmes);
                    return;
                }
                if (DEWorkDirTextBox.Text == null ||
                    DEWorkDirTextBox.Text == "")
                {
                    string errmes = MainWindow.s_resources.GetString(MessageConstants.ErrNoWorkDir);
                    Util.ShowErrorDialog(errmes);
                    return;
                }
                string envName = DEEnvComboBox.Text;
                m_manager.SetCurrentEnvironment(envName);
                m_manager.TmpRootDir = DEWorkDirTextBox.Text;
                m_manager.Concurrency = conc;
                for (int i = 1; i < DEOptionGridView.Rows.Count; i++)
                {
                    string propName = DEOptionGridView[0, i].Value.ToString();
                    if (m_propDict.ContainsKey(propName))
                    {
                        m_propDict[propName] = DEOptionGridView[1, i].Value.ToString();
                    }
                }
                m_manager.SetEnvironmentProperty(m_propDict);
            }
            catch (Exception ex)
            {
                ex.ToString();
                string errmes = MainWindow.s_resources.GetString(MessageConstants.ErrUpdateDistEnv);
                Util.ShowErrorDialog(errmes);
                return;
            }
            this.Close();
        }

        /// <summary>
        /// Event when the search directory button is clicked.
        /// Set the working directory with using FolderSelectDialog.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void DESearchDir_Click(object sender, EventArgs e)
        {
            if (m_folderSelectDialog.ShowDialog() == DialogResult.OK)
            {
                DEWorkDirTextBox.Text = m_folderSelectDialog.SelectedPath;
            }
            else
            {
                // nothing.
            }
        }
        #endregion
    }
}