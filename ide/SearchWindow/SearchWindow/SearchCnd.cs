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

namespace EcellLib.SearchWindow
{
    public partial class SearchCnd : Form
    {
        #region Fields
        /// <summary>
        /// DataManager.
        /// </summary>
        DataManager m_dManager;
        /// <summary>
        /// PluginManager.
        /// </summary>
        PluginManager m_pManager;
        #endregion


        /// <summary>
        /// the constructor for SearchCnd.
        /// </summary>
        public SearchCnd()
        {
            InitializeComponent();
            m_dManager = DataManager.GetDataManager();
            m_pManager = PluginManager.GetPluginManager();
        }

        public Bitmap Print()
        {
            Bitmap bitmap = new Bitmap(dgv.Width, dgv.Height);
            dgv.DrawToBitmap(bitmap, dgv.ClientRectangle);

            return bitmap;
        }

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
            List<String> modelList = m_dManager.GetModelList();
            if (modelList == null) return;

            foreach (string model in modelList)
            {
                List<EcellObject> list = m_dManager.GetData(model, null);
                if (list == null) continue;
                foreach (EcellObject obj in list)
                {
                    if (obj.key == null) continue;
                    if (obj.key.Contains(searchId))
                    {
                        dgv.Rows.Add(new Object[] { obj.key, model, obj.type });
                    }

                    if (obj.M_instances == null) continue;
                    foreach (EcellObject ins in obj.M_instances)
                    {
                        if (ins.key.Contains(searchId))
                        {
                            dgv.Rows.Add(new Object[] { ins.key, model, ins.type });
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

            string model = (string)dgv.Rows[index].Cells[1].Value;
            string id = (string)dgv.Rows[index].Cells[0].Value;
            string type = (string)dgv.Rows[index].Cells[2].Value;

            m_pManager.SelectChanged(model, id, type);
            this.Select();
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
                SearchButtonClick(sender, new EventArgs());
            }
        }
        #endregion

        private void SearchCndShown(object sender, EventArgs e)
        {
            this.idText.Focus();
        }
    }
}