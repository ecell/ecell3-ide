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
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.EntityListWindow
{
    /// <summary>
    /// Form to input the new DM name.
    /// </summary>
    public partial class InputName : Form
    {
        #region Fields
        /// <summary>
        /// The path of dm directory for the current project.
        /// </summary>
        private string m_dir;
        /// <summary>
        /// The current selected node.
        /// </summary>
        private TreeNode m_node;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="dmDir">The path of dm directory.</param>
        /// <param name="node">The current selected node.</param>
        public InputName(string dmDir, TreeNode node)
        {
            InitializeComponent();
            m_dir = dmDir;
            m_node = node;
        }
        #endregion

        #region Events
        /// <summary>
        /// The event sequence when the cancel button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void INCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The event sequence when the create button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void INNewButton_Click(object sender, EventArgs e)
        {
            String name = INTextBox.Text;
            if (String.IsNullOrEmpty(name)) return;
            if (!name.EndsWith(Constants.xpathProcess) && !name.EndsWith(Constants.xpathStepper))
            {
                Util.ShowWarningDialog(MessageResEntList.WarnDMName);
                return;
            }

            try
            {
                string filename = Path.Combine(m_dir, name);
                filename = filename + Constants.FileExtSource;
                File.Create(filename);

                TreeNode dNode = new TreeNode(name);
                ApplicationEnvironment env = ApplicationEnvironment.GetInstance();
                dNode.ImageIndex = env.PluginManager.GetImageIndex(Constants.xpathDM);
                dNode.SelectedImageIndex = dNode.ImageIndex;
                dNode.Tag = new TagData("", "", Constants.xpathDM);
                m_node.Nodes.Add(dNode);

                this.Close();
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(string.Format(MessageResEntList.ErrCreateFile,
                    new object[] { name }));                
            }
        }

        /// <summary>
        /// The event sequence to show this form.
        /// </summary>
        /// <param name="sender">This form.</param>
        /// <param name="e">EventArgs.</param>
        private void InputNameShown(object sender, EventArgs e)
        {
            INTextBox.Focus();
        }
        #endregion
    }
}