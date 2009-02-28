﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
// written by by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ecell.Exceptions;

namespace Ecell.IDE
{
    /// <summary>
    /// Tabbed PropertyDialog for Ecell-IDE.
    /// </summary>
    public partial class PropertyDialog : Form
    {
        List<PropertyDialogPage> m_list;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyPages"></param>
        public PropertyDialog(List<PropertyDialogPage> propertyPages)
        {
            InitializeComponent();
            m_list = propertyPages;
            propertyPanel.Controls.Add(propertyPages[0]);
            foreach (PropertyDialogPage page in propertyPages)
            {
                PropertyNode node = new PropertyNode(page);
                propertyTree.Nodes.Add(node);
            }
        }

        /// <summary>
        /// Apply changes for Properties.
        /// </summary>
        public void ApplyChanges()
        {
            foreach (PropertyDialogPage page in m_list)
                page.ApplyChange();
        }

        private void PropertyDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
                return;
            try
            {
                foreach (PropertyDialogPage page in m_list)
                    page.PropertyDialogClosing();
            }
            catch (EcellException ex)
            {
                Util.ShowErrorDialog(ex.Message);
                e.Cancel = true;
                return;
            }
        }

        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode obj = propertyTree.GetNodeAt(e.X, e.Y);
            if (!(obj is PropertyNode))
                return;
            PropertyNode node = (PropertyNode)obj;
            propertyPanel.Controls.Clear();
            propertyPanel.Controls.Add(node.Page);
        }

        /// <summary>
        /// 
        /// </summary>
        internal class PropertyNode : TreeNode
        {
            PropertyDialogPage m_page;
            /// <summary>
            /// 
            /// </summary>
            internal PropertyDialogPage Page
            {
                get { return m_page; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="page"></param>
            public PropertyNode(PropertyDialogPage page)
            {
                m_page = page;
                this.Text = page.Text;
            }
        }
    }
}