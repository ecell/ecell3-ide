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
    partial class SaveTraceDialog
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveTraceDialog));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.STSearchDirButton = new System.Windows.Forms.Button();
            this.dirTextBox = new System.Windows.Forms.TextBox();
            this.startTextBox = new System.Windows.Forms.TextBox();
            this.endTextBox = new System.Windows.Forms.TextBox();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.SaveEntrySelectView = new System.Windows.Forms.DataGridView();
            this.SaveEntryColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STSaveButton = new System.Windows.Forms.Button();
            this.STCloseButton = new System.Windows.Forms.Button();
            this.m_folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SaveEntrySelectView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.STSearchDirButton);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(label4);
            this.groupBox1.Controls.Add(this.dirTextBox);
            this.groupBox1.Controls.Add(this.startTextBox);
            this.groupBox1.Controls.Add(label3);
            this.groupBox1.Controls.Add(label2);
            this.groupBox1.Controls.Add(this.endTextBox);
            this.groupBox1.Controls.Add(this.typeComboBox);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // STSearchDirButton
            // 
            resources.ApplyResources(this.STSearchDirButton, "STSearchDirButton");
            this.STSearchDirButton.Name = "STSearchDirButton";
            this.STSearchDirButton.UseVisualStyleBackColor = true;
            this.STSearchDirButton.Click += new System.EventHandler(this.STSearchDirButtonClick);
            // 
            // dirTextBox
            // 
            resources.ApplyResources(this.dirTextBox, "dirTextBox");
            this.dirTextBox.Name = "dirTextBox";
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
            resources.ApplyResources(this.SaveEntrySelectView, "SaveEntrySelectView");
            this.SaveEntrySelectView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.SaveEntrySelectView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SaveEntrySelectView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SaveEntryColumn,
            this.IDColumn});
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
            // STSaveButton
            // 
            resources.ApplyResources(this.STSaveButton, "STSaveButton");
            this.STSaveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.STSaveButton.Name = "STSaveButton";
            this.STSaveButton.UseVisualStyleBackColor = true;
            // 
            // STCloseButton
            // 
            resources.ApplyResources(this.STCloseButton, "STCloseButton");
            this.STCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.STCloseButton.Name = "STCloseButton";
            this.STCloseButton.UseVisualStyleBackColor = true;
            this.STCloseButton.Click += new System.EventHandler(this.STCloseButtonClick);
            // 
            // SaveTraceDialog
            // 
            this.AcceptButton = this.STSaveButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.STCloseButton;
            this.Controls.Add(this.STSaveButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.STCloseButton);
            this.Controls.Add(this.SaveEntrySelectView);
            this.Name = "SaveTraceDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveTraceDialogClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SaveEntrySelectView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.Button STSaveButton;
        private System.Windows.Forms.Button STCloseButton;
        private System.Windows.Forms.Button STSearchDirButton;
        private System.Windows.Forms.TextBox dirTextBox;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.TextBox startTextBox;
        private System.Windows.Forms.TextBox endTextBox;
    }
}