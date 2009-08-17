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
using System.Windows.Forms;

using Ecell;
using Ecell.Reporting;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.MessageListWindow
{
    /// <summary>
    /// User Control to display the list of error message.
    /// </summary>
    public partial class MessageListWindowControl : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// owner object.
        /// </summary>
        private MessageListWindow m_owner;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageListWindowControl(MessageListWindow owner)
        {
            m_owner = owner;
            InitializeComponent();
            this.TabText = this.Text;
        }
        #endregion

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        /// <param name="group">the clear report group name.</param>
        public void Clear(string group)
        {
            if (group == null)
            {
                MLWMessageDridView.Rows.Clear();
                return;
            }
            List<DataGridViewRow> delList = new List<DataGridViewRow>();
            foreach (DataGridViewRow r in MLWMessageDridView.Rows)
            {
                if (r.Tag == null) continue;
                IReport rep = r.Tag as IReport;
                if (rep == null) continue;

                if (rep.Group.Equals(group)) delList.Add(r);
            }

            foreach (DataGridViewRow r in delList)
            {
                MLWMessageDridView.Rows.Remove(r);
            }
        }

        /// <summary>
        /// The event sequence to display the message.
        /// </summary>
        /// <param name="mes">the message entry object.</param>
        public void AddMessageEntry(IReport mes)
        {
            DataGridViewRow r = new DataGridViewRow();

            DataGridViewImageCell c1 = new DataGridViewImageCell(true);
            switch (mes.Type)
            {
                case MessageType.Error:
                    c1.Value = Properties.Resources.ErrorIcon;
                    break;
                case MessageType.Warning:
                    c1.Value = Properties.Resources.WarningIcon;
                    break;
            }
            r.Cells.Add(c1);

            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = mes.Location;
            r.Cells.Add(c2);

            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = mes.Message;            
            r.Cells.Add(c3);            
            r.Tag = mes;
            
            MLWMessageDridView.Rows.Add(r);
        }

        /// <summary>
        /// Double click on DataGridView of message.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void MessageCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (MLWMessageDridView.Rows[e.RowIndex].Tag == null) return;
            ObjectReport mes = MLWMessageDridView.Rows[e.RowIndex].Tag as ObjectReport;
            if (mes == null) return;

            m_owner.PluginManager.SelectChanged(mes.Object.ModelID,
                mes.Object.Key, mes.Object.Type);
        }
    }
}
