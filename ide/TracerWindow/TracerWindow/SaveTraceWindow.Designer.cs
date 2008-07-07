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

namespace Ecell.IDE.Plugins.TracerWindow
{
    partial class SaveTraceWindow
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveTraceWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.startTextBox = new System.Windows.Forms.TextBox();
            this.endTextBox = new System.Windows.Forms.TextBox();
            this.dirTextBox = new System.Windows.Forms.TextBox();
            this.STSearchDirButton = new System.Windows.Forms.Button();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.SaveEntrySelectView = new System.Windows.Forms.DataGridView();
            this.SaveEntryColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.STSaveButton = new System.Windows.Forms.Button();
            this.STCloseButton = new System.Windows.Forms.Button();
            this.m_folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SaveEntrySelectView)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SaveEntrySelectView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.startTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.endTextBox, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.dirTextBox, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.STSearchDirButton, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.typeComboBox, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // startTextBox
            // 
            resources.ApplyResources(this.startTextBox, "startTextBox");
            this.startTextBox.Name = "startTextBox";
            // 
            // endTextBox
            // 
            resources.ApplyResources(this.endTextBox, "endTextBox");
            this.endTextBox.Name = "endTextBox";
            // 
            // dirTextBox
            // 
            resources.ApplyResources(this.dirTextBox, "dirTextBox");
            this.dirTextBox.Name = "dirTextBox";
            // 
            // STSearchDirButton
            // 
            resources.ApplyResources(this.STSearchDirButton, "STSearchDirButton");
            this.STSearchDirButton.Name = "STSearchDirButton";
            this.STSearchDirButton.UseVisualStyleBackColor = true;
            this.STSearchDirButton.Click += new System.EventHandler(this.STSearchDirButtonClick);
            // 
            // typeComboBox
            // 
            resources.ApplyResources(this.typeComboBox, "typeComboBox");
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            resources.GetString("typeComboBox.Items"),
            resources.GetString("typeComboBox.Items1")});
            this.typeComboBox.Name = "typeComboBox";
            // 
            // SaveEntrySelectView
            // 
            this.SaveEntrySelectView.AllowUserToAddRows = false;
            this.SaveEntrySelectView.AllowUserToDeleteRows = false;
            this.SaveEntrySelectView.AllowUserToResizeRows = false;
            this.SaveEntrySelectView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.SaveEntrySelectView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SaveEntrySelectView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SaveEntryColumn,
            this.IDColumn});
            resources.ApplyResources(this.SaveEntrySelectView, "SaveEntrySelectView");
            this.SaveEntrySelectView.MultiSelect = false;
            this.SaveEntrySelectView.Name = "SaveEntrySelectView";
            this.SaveEntrySelectView.RowHeadersVisible = false;
            this.SaveEntrySelectView.RowTemplate.Height = 21;
            this.SaveEntrySelectView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // SaveEntryColumn
            // 
            this.SaveEntryColumn.FillWeight = 20F;
            resources.ApplyResources(this.SaveEntryColumn, "SaveEntryColumn");
            this.SaveEntryColumn.Name = "SaveEntryColumn";
            // 
            // IDColumn
            // 
            this.IDColumn.FillWeight = 80F;
            resources.ApplyResources(this.IDColumn, "IDColumn");
            this.IDColumn.Name = "IDColumn";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.STSaveButton, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.STCloseButton, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // STSaveButton
            // 
            resources.ApplyResources(this.STSaveButton, "STSaveButton");
            this.STSaveButton.Name = "STSaveButton";
            this.STSaveButton.UseVisualStyleBackColor = true;
            this.STSaveButton.Click += new System.EventHandler(this.STSaveButtonClick);
            // 
            // STCloseButton
            // 
            resources.ApplyResources(this.STCloseButton, "STCloseButton");
            this.STCloseButton.Name = "STCloseButton";
            this.STCloseButton.UseVisualStyleBackColor = true;
            this.STCloseButton.Click += new System.EventHandler(this.STCloseButtonClick);
            // 
            // SaveTraceWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SaveTraceWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SaveEntrySelectView)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        /// <summary>
        /// Button to save the trace.
        /// </summary>
        public System.Windows.Forms.Button STSaveButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button STCloseButton;
        /// <summary>
        /// Button to set the save directory.
        /// </summary>
        public System.Windows.Forms.Button STSearchDirButton;
        /// <summary>
        /// DataGridView to show the save logger entry.
        /// </summary>
        public System.Windows.Forms.DataGridView SaveEntrySelectView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SaveEntryColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDColumn;
        /// <summary>
        /// FolderBrowserDialog to set the save directory.
        /// </summary>
        public System.Windows.Forms.FolderBrowserDialog m_folderDialog;
        /// <summary>
        /// TextBox to set the save directory.
        /// </summary>
        public System.Windows.Forms.TextBox dirTextBox;
        /// <summary>
        /// ComboBox to set the file type of save file.
        /// </summary>
        public System.Windows.Forms.ComboBox typeComboBox;
        /// <summary>
        /// TextBox to set the saving start time.
        /// </summary>
        public System.Windows.Forms.TextBox startTextBox;
        /// <summary>
        /// TextBox to set the saving end time.
        /// </summary>
        public System.Windows.Forms.TextBox endTextBox;
    }
}