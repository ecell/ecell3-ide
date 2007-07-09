using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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

namespace EcellLib
{
    public partial class VariableSelectWindow : Form
    {
        #region Fields
        /// <summary>
        /// the parent windows form.
        /// </summary>
        private VariableRefWindow m_win;
        /// <summary>
        /// ResourceManager for VariableSelectWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResLib));
        #endregion

        /// <summary>
        /// Constructor for VariableSelectWindow.
        /// </summary>
        public VariableSelectWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the windows form to the parent window.
        /// </summary>
        /// <param name="w">windows form</param>
        public void SetParentWindow(VariableRefWindow w)
        {
            m_win = w;
        }

        #region Event
        /// <summary>
        /// The action of clicking the select button in VariableSelectWindow.
        /// </summary>
        /// <param name="sender">Object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void SelectCancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// The action of clicking cancel button in VariableSelectWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void SelectButtonClick(object sender, EventArgs e)
        {
            TreeNode t = this.selectTree.SelectedNode;
            if (t == null)
            {
                String errmes = m_resources.GetString("ErrNoSelect");
                MessageBox.Show(errmes, 
                    "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tag = (string)t.Tag;
            if (tag == null || tag.Equals(""))
            {
                String errmes = m_resources.GetString("ErrNotVar");
                MessageBox.Show(errmes, "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int j = 0;
            string id;
            string key = ":" + tag;
            while (true)
            {
                id = "P" + j;
                bool isHit = false;
                for (int i = 0; i < m_win.dgv.RowCount; i++)
                {
                    if (id == (string)m_win.dgv[0, i].Value)
                    {
                        isHit = true;
                        break;
                    }
                }
                if (isHit == false)
                {
                    break;
                }
                j++;
            }

            m_win.dgv.Rows.Add(new object[] { id, key, 1, true });
        }
        #endregion

        private void SelectTreeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode t = this.selectTree.SelectedNode;
            if (t == null)
            {
                String errmes = m_resources.GetString("ErrNoSelect");
                MessageBox.Show(errmes,
                    "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tag = (string)t.Tag;
            if (tag == null || tag.Equals(""))
            {
                String errmes = m_resources.GetString("ErrNotVar");
                MessageBox.Show(errmes, "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int j = 0;
            string id;
            string key = ":" + tag;
            while (true)
            {
                id = "P" + j;
                bool isHit = false;
                for (int i = 0; i < m_win.dgv.RowCount; i++)
                {
                    if (id == (string)m_win.dgv[0, i].Value)
                    {
                        isHit = true;
                        break;
                    }
                }
                if (isHit == false)
                {
                    break;
                }
                j++;
            }

            m_win.dgv.Rows.Add(new object[] { id, key, 1, true });

        }
    }
}