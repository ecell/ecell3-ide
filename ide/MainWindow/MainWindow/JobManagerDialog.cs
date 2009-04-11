//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ecell.IDE;
using Ecell.Job;
using Ecell.Exceptions;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// Job manage page in the common setting dialog.
    /// </summary>
    partial class JobManagerDialog : PropertyDialogPage
    {
        #region Fields
        private IJobManager m_manager;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manager"></param>
        public JobManagerDialog(IJobManager manager)
        {
            InitializeComponent();
            m_manager = manager;

            string name = m_manager.GetCurrentEnvironment();
            List<string> envList = m_manager.GetEnvironmentList();
            foreach (string envname in envList)
            {
                int index = envComboBox.Items.Add(envname);
                if (name.Equals(envname))
                {
                    envComboBox.SelectedIndex = index;
                    Dictionary<string, object> propDic =
                        m_manager.GetEnvironmentProperty();
                    int conc = m_manager.GetDefaultConcurrency();
                    concTextBox.Text = conc.ToString();
                    envDataGridView.Rows.Clear();
                    foreach (string propName in propDic.Keys)
                    {
                        int i = envDataGridView.Rows.Add(
                            new object[] { propName, propDic[propName].ToString() });
                        envDataGridView.Rows[i].Tag = propDic[propName].GetType();
                    }
                }
            }
            this.Name = MessageResources.NameJobManage;
        }
        #endregion

        #region Override
        /// <summary>
        /// Apply this property.
        /// </summary>
        public override void ApplyChange()
        {
            string ename = envComboBox.Text;
            int dummy;
            if (String.IsNullOrEmpty(concTextBox.Text) || !Int32.TryParse(concTextBox.Text, out dummy))
            {
                throw new EcellException(String.Format(MessageResources.ErrInvalidValue, label2.Text));
            }

            m_manager.SetCurrentEnvironment(ename);
            m_manager.Proxy.Concurrency = dummy;

            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (DataGridViewRow r in envDataGridView.Rows)
            {
                string name = r.Cells[0].Value.ToString();
                string data = r.Cells[1].Value.ToString();

                if (string.IsNullOrEmpty(data))
                {
                    throw new EcellException(String.Format(MessageResources.ErrInvalidValue, name));
                }
                result.Add(name, data);
            }
            m_manager.SetEnvironmentProperty(result);
        }

        /// <summary>
        /// Check the input property data.
        /// </summary>
        public override void PropertyDialogClosing()
        {
            int dummy;
            if (String.IsNullOrEmpty(concTextBox.Text) || !Int32.TryParse(concTextBox.Text,out dummy))
            {
                throw new EcellException(String.Format(MessageResources.ErrInvalidValue, label2.Text));
            }

            foreach (DataGridViewRow r in envDataGridView.Rows)
            {
                string name = r.Cells[0].Value.ToString();
                string data = r.Cells[1].Value.ToString();

                if (string.IsNullOrEmpty(data))
                {
                    throw new EcellException(String.Format(MessageResources.ErrInvalidValue, name));
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void envComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = envComboBox.Text;
            Dictionary<string, object> propDic = m_manager.GetDefaultEnvironmentProperty(name);
            envDataGridView.Rows.Clear();
            foreach (string propName in propDic.Keys)
            {
                int i = envDataGridView.Rows.Add(
                    new object[] { propName, propDic[propName].ToString() });
                envDataGridView.Rows[i].Tag = propDic[propName].GetType();
            }
        }
        #endregion
    }
}
