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

namespace Ecell.IDE.MainWindow.UIComponents
{
    /// <summary>
    /// Job manage page in the common setting dialog.
    /// </summary>
    public class JobManagerSettingPage : PropertyDialogPage
    {
        #region Fields
        private IJobManager m_manager;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox envComboBox;
        private System.Windows.Forms.DataGridView envDataGridView;
        private System.Windows.Forms.TextBox concTextBox;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn ValueColumn;
        private System.Windows.Forms.Label label2;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manager"></param>
        public JobManagerSettingPage(IJobManager manager)
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

        private void InitializeComponent()
        {
            System.Windows.Forms.Label label3;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobManagerSettingPage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.envComboBox = new System.Windows.Forms.ComboBox();
            this.envDataGridView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.concTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.envDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // envComboBox
            // 
            resources.ApplyResources(this.envComboBox, "envComboBox");
            this.envComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.envComboBox.FormattingEnabled = true;
            this.envComboBox.Name = "envComboBox";
            this.envComboBox.SelectedIndexChanged += new System.EventHandler(this.envComboBox_SelectedIndexChanged);
            // 
            // envDataGridView
            // 
            this.envDataGridView.AllowUserToAddRows = false;
            this.envDataGridView.AllowUserToDeleteRows = false;
            this.envDataGridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.envDataGridView, "envDataGridView");
            this.envDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.envDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.envDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.envDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.ValueColumn});
            this.envDataGridView.Name = "envDataGridView";
            this.envDataGridView.RowHeadersVisible = false;
            this.envDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.envDataGridView.RowTemplate.Height = 21;
            // 
            // NameColumn
            // 
            resources.ApplyResources(this.NameColumn, "NameColumn");
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // ValueColumn
            // 
            resources.ApplyResources(this.ValueColumn, "ValueColumn");
            this.ValueColumn.Name = "ValueColumn";
            // 
            // concTextBox
            // 
            resources.ApplyResources(this.concTextBox, "concTextBox");
            this.concTextBox.Name = "concTextBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // JobManagerSettingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.concTextBox);
            this.Controls.Add(this.envDataGridView);
            this.Controls.Add(this.envComboBox);
            this.Controls.Add(this.label1);
            this.Name = "JobManagerSettingPage";
            ((System.ComponentModel.ISupportInitialize)(this.envDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
            concTextBox.Text = m_manager.GetDefaultConcurrency(name).ToString();
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
