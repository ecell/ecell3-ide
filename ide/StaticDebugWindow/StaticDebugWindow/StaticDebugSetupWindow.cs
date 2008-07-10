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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.StaticDebugWindow
{
    /// <summary>
    /// The form class to setup static debug.
    /// </summary>
    public partial class StaticDebugSetupWindow : Form
    {
        #region Fields
        /// <summary>
        /// The plugin(StaticDebugWindow) diplayed this window.
        /// </summary>
        private StaticDebugWindow m_staticDebug;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public StaticDebugSetupWindow()
        {
            InitializeComponent();

            debugResultView.CellDoubleClick += new DataGridViewCellEventHandler(CellDoubleClick);
        }

        /// <summary>
        /// Set the plugin displayed this window.
        /// </summary>
        /// <param name="p"></param>
        public void SetPlugin(StaticDebugWindow p)
        {
            m_staticDebug = p;
        }

        /// <summary>
        /// Change the layout of check box in this window.
        /// </summary>
        /// <param name="checkList">the list of displayed check box.</param>
        public void LayoutCheckList(List<string> checkList)
        {
            layoutPanel.SuspendLayout();

            int width = layoutPanel.Width;

            layoutPanel.Controls.Clear();
            layoutPanel.RowStyles.Clear();
            layoutPanel.Size = new Size(width, 25 * (checkList.Count + 1) / 2);
            layoutPanel.RowCount = checkList.Count / 2 + 1;

            int j = 0, k = 0;
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            for (int i = 0; i < checkList.Count; i++)
            {
                CheckBox c1 = new CheckBox();
                c1.Checked = true;
                c1.Width = 150;
                c1.Text = checkList[i];
                layoutPanel.Controls.Add(c1, j, k);
                if (j == 0) j++;
                else
                {
                    layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                    j = 0;
                    k++;
                }
            }
            panel1.ClientSize = panel1.Size;
            layoutPanel.ResumeLayout(false);
        }

        #region Common
        /// <summary>
        /// Get path from entity path.
        /// </summary>
        /// <param name="entityPath">input entity path.</param>
        /// <returns>path.</returns>
        public String GetSystemFromPath(String entityPath)
        {
            String result = "";
            String[] list = entityPath.Split(new char[] { ':' });
            if (list.Length == 2) return list[0];
            else if (list.Length > 1) return list[1];
            return result;
        }

        /// <summary>
        /// Get key from entity path.
        /// </summary>
        /// <param name="entityPath">input entity path.</param>
        /// <returns>key.</returns>
        public String GetKeyFromPath(String entityPath)
        {
            String[] list = entityPath.Split(new char[] { ':' });
            if (list.Length == 2) return entityPath;
            bool isSystem = false;
            if (list[0] == Constants.xpathSystem) isSystem = true;
            String result = list[1];
            for (int i = 2; i < list.Length - 1; i++)
            {
                if (isSystem)
                {
                    if (result == "") result = list[i];
                    else result = result + "/" + list[i];
                }
                else
                    result = result + ":" + list[i];
            }
            return result;
        }
        #endregion

        #region Events
        /// <summary>
        /// The action of clicking the debug button.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void debugButton_Click(object sender, EventArgs e)
        {
            debugResultView.Rows.Clear();
            IEnumerator iter = layoutPanel.Controls.GetEnumerator();
            List<string> list = new List<string>();

            while (iter.MoveNext())
            {
                CheckBox c = (CheckBox)iter.Current;
                if (c.Checked) list.Add(c.Text);
            }
            m_staticDebug.Debug(list);

            if (m_staticDebug.ErrorMessageList.Count <= 0)
            {
                Util.ShowNoticeDialog(MessageResources.NoError);
                return;
            }

            iter = m_staticDebug.ErrorMessageList.GetEnumerator();
            while (iter.MoveNext())
            {
                ErrorMessage em = (ErrorMessage)iter.Current;
                debugResultView.Rows.Add(new object[] { 
                    em.Message, em.EntityPath, em.ModelID, em.Type });
            }
        }

        /// <summary>
        /// The action of double clicking the cell in this DataGridView.
        /// </summary>
        /// <param name="sender">DataGirdView.</param>
        /// <param name="e">DataGridViewCellEventArgs.</param>
        void CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = debugResultView.Rows[e.RowIndex];
            String modelID = (String)row.Cells[2].Value;
            String path = (String)row.Cells[1].Value;
            String type = (String)row.Cells[3].Value;

            String key = GetKeyFromPath(path);

            EcellObject obj = m_staticDebug.DataManager.GetEcellObject(modelID, key, type);
            if (obj != null)
            {
                PropertyEditor.Show(m_staticDebug.DataManager, m_staticDebug.PluginManager, obj);
            }
        }

        /// <summary>
        /// The action of clicking the close button.
        /// Close this window.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// Event when this form is shown.
        /// </summary>
        /// <param name="sender">StaticDebugSetupWindow.</param>
        /// <param name="e">EventArgs.</param>
        private void StaticDebugWinShown(object sender, EventArgs e)
        {
            this.SSDebugButton.Focus();
        }
        #endregion
    }
}
