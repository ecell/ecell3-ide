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
using EcellLib.Objects;

namespace EcellLib.SearchWindow
{
    /// <summary>
    /// Form to input the condition to search the object.
    /// </summary>
    internal partial class SearchCnd : Form
    {
        #region Fields
        /// <summary>
        /// The owner of this object.
        /// </summary>
        SearchWindow m_owner;
        /// <summary>
        /// ResourceManager for SearchCnd.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResSearch));
        #endregion

        /// <summary>
        /// the constructor for SearchCnd.
        /// </summary>
        public SearchCnd(SearchWindow owner)
        {
            m_owner = owner;
            InitializeComponent();
        }

        /// <summary>
        /// Get Bitmap to print this window.
        /// </summary>
        /// <returns>bitmap of this window.</returns>
        public Bitmap Print()
        {
            Bitmap bitmap = new Bitmap(dgv.Width, dgv.Height);
            dgv.DrawToBitmap(bitmap, dgv.ClientRectangle);

            return bitmap;
        }

        /// <summary>
        /// Search the object by using the search conditions.
        /// </summary>
        /// <param name="str"></param>
        public void Search(string str)
        {
            idText.Text = str;
            SearchButtonClick(SCSearchButton, new EventArgs());
        }


        #region Events
        /// <summary>
        /// the action of clicking the search button.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void SearchButtonClick(object sender, EventArgs e)
        {
            dgv.Rows.Clear();

            string searchId = idText.Text;
            if (searchId.Equals("") || searchId == null) return;
            List<String> modelList = m_owner.DataManager.GetModelList();
            if (modelList == null) return;

            foreach (string model in modelList)
            {
                List<EcellObject> list = m_owner.DataManager.GetData(model, null);
                if (list == null) continue;
                foreach (EcellObject obj in list)
                {
                    if (obj.Key == null) continue;
                    if (obj.Key.Contains(searchId))
                    {
                        String name = "";
                        if (obj.IsEcellValueExists("Name"))
                            name = obj.GetEcellValue("Name").ToString();
                        dgv.Rows.Add(new Object[] { obj.Key, name, model, obj.Type });
                    }
                    else
                    {
                        EcellValue value = obj.GetEcellValue("Name");
                        if (value != null && value.ToString().Contains(searchId))
                            dgv.Rows.Add(new Object[] { obj.Key, value.ToString(), model, obj.Type });
                    }

                    if (obj.Children == null) continue;
                    foreach (EcellObject ins in obj.Children)
                    {
                        if (ins.Key.Contains(searchId)) {
                            String name = "";
                            if (ins.IsEcellValueExists("Name"))
                                name = ins.GetEcellValue("Name").ToString();
                            dgv.Rows.Add(new Object[] { ins.Key, name, model, ins.Type });
                        }
                        else
                        {
                            EcellValue value = ins.GetEcellValue("Name");
                            if ( value != null && value.ToString().Contains(searchId))
                                dgv.Rows.Add(new Object[] { ins.Key, value.ToString(), model, ins.Type });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// the action of clicking the close button.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void CloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// the action of double clicking in DataGridView.
        /// </summary>
        /// <param name="sender">object(DataGridView)</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void DgvCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;
            string model = (string)dgv.Rows[index].Cells[2].Value;
            string id = (string)dgv.Rows[index].Cells[0].Value;
            string type = (string)dgv.Rows[index].Cells[3].Value;

            EcellObject obj = m_owner.DataManager.GetEcellObject(model, id, type);
            if (obj == null)
            {
                Util.ShowWarningDialog(m_resources.GetString(MessageConstants.ErrNotFind) + "(" + model + "," + id + ")");
                return;
            }
            ShowPropEditWindow(obj);
        }

        /// <summary>
        /// Show property window displayed the selected object.
        /// </summary>
        /// <param name="obj">the selected object</param>
        public void ShowPropEditWindow(EcellObject obj)
        {
            PropertyEditor.Show(m_owner.DataManager, m_owner.PluginManager, obj);
        }

        /// <summary>
        /// the action of pressing key in idText.
        /// </summary>
        /// <param name="sender">object(TextBox)</param>
        /// <param name="e">KeyPressEventArgs</param>
        private void idTextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SCSearchButton.PerformClick();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                SCCloseButton.PerformClick();
            }
        }

        /// <summary>
        /// The event to show this window.
        /// </summary>
        /// <param name="sender">This window.</param>
        /// <param name="e">EventArgs.</param>
        private void SearchCndShown(object sender, EventArgs e)
        {
            this.idText.Focus();
        }

        /// <summary>
        /// Event when the search result item is clicked.
        /// Select the object corresponding with the search result item.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellMouseEventArgs.</param>
        private void DgvCellClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;

            string model = (string)dgv.Rows[index].Cells[2].Value;
            string id = (string)dgv.Rows[index].Cells[0].Value;
            string type = (string)dgv.Rows[index].Cells[3].Value;

            m_owner.PluginManager.SelectChanged(model, id, type);
            this.Select();
        }
        #endregion

    }
}