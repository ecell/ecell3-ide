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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.Plugin;

namespace Ecell.IDE
{
    public partial class VariableSelectDialog : Form
    {
        #region Fields
        /// <summary>
        /// DataManager
        /// </summary>
        private DataManager m_dManager;
        /// <summary>
        /// PluginManager
        /// </summary>
        private PluginManager m_pManager;
        /// <summary>
        /// the parent windows form.
        /// </summary>
        private IVariableDialog m_win;
        #endregion

        /// <summary>
        /// Constructor for VariableSelectWindow.
        /// </summary>
        public VariableSelectDialog(DataManager dManager, PluginManager pManager)
        {
            InitializeComponent();
            this.m_dManager = dManager;
            this.m_pManager = pManager;
            this.selectTree.ImageList = m_pManager.NodeImageList;
        }

        /// <summary>
        /// Set the windows form to the parent window.
        /// </summary>
        /// <param name="w">windows form</param>
        public void SetParentWindow(IVariableDialog w)
        {
            m_win = w;
        }

        #region Event
        /// <summary>
        /// The action of clicking cancel button in VariableSelectWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void ProductButtonClick(object sender, EventArgs e)
        {
            TreeNode t = this.selectTree.SelectedNode;
            if (t == null)
            {
                Util.ShowWarningDialog(MessageResources.ErrNoSelectVar);
                return;
            }
            string tag = (string)t.Tag;
            if (tag == null || tag.Equals(""))
            {
                Util.ShowWarningDialog(MessageResources.ErrNoSelectVar);

                return;
            }

            string key = tag;
            m_win.AddReference(key, "P");
        }

        /// <summary>
        /// The action of clicking cancel button in VariableSelectWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void SourceButtonClick(object sender, EventArgs e)
        {
            TreeNode t = this.selectTree.SelectedNode;
            if (t == null)
            {
                Util.ShowWarningDialog(MessageResources.ErrNoSelectVar);
                return;
            }
            string tag = (string)t.Tag;
            if (tag == null || tag.Equals(""))
            {
                Util.ShowWarningDialog(MessageResources.ErrNoSelectVar);
                return;
            }

            string key = tag;
            m_win.AddReference(key, "S");
        }

        /// <summary>
        /// The action of clicking cancel button in VariableSelectWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void ConstantButtonClick(object sender, EventArgs e)
        {
            TreeNode t = this.selectTree.SelectedNode;
            if (t == null)
            {
                Util.ShowWarningDialog(MessageResources.ErrNoSelectVar);
                return;
            }
            string tag = (string)t.Tag;
            if (tag == null || tag.Equals(""))
            {
                Util.ShowWarningDialog(MessageResources.ErrNoSelectVar);

                return;
            }

            string key = tag;
            m_win.AddReference(key, "C");
        }

        /// <summary>
        /// Event when tree view is double clicked.
        /// </summary>
        /// <param name="sender">TreeView</param>
        /// <param name="e">TreeNodeMouseClickEventArgs</param>
        private void SelectTreeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode t = this.selectTree.SelectedNode;
            if (t == null)
            {
                Util.ShowWarningDialog(MessageResources.ErrNoSelectVar);
                return;
            }
            string tag = (string)t.Tag;
            if (tag == null || tag.Equals(""))
            {
                Util.ShowWarningDialog(MessageResources.ErrNoSelectVar);
                return;
            }

            string key = tag;
            m_win.AddReference(key, "P");
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IVariableDialog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        void AddReference(string key, string prefix);
    }
}