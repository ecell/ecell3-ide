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
namespace Ecell.IDE.MainWindow
{
    partial class SaveProjectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveProjectDialog));
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.savedItemListBox = new System.Windows.Forms.CheckedListBox();
            this.savedItemsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.AccessibleDescription = null;
            this.saveButton.AccessibleName = null;
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.BackgroundImage = null;
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Font = null;
            this.saveButton.Name = "saveButton";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = null;
            this.cancelButton.AccessibleName = null;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackgroundImage = null;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = null;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // savedItemListBox
            // 
            this.savedItemListBox.AccessibleDescription = null;
            this.savedItemListBox.AccessibleName = null;
            resources.ApplyResources(this.savedItemListBox, "savedItemListBox");
            this.savedItemListBox.BackgroundImage = null;
            this.savedItemListBox.Font = null;
            this.savedItemListBox.FormattingEnabled = true;
            this.savedItemListBox.Name = "savedItemListBox";
            // 
            // savedItemsLabel
            // 
            this.savedItemsLabel.AccessibleDescription = null;
            this.savedItemsLabel.AccessibleName = null;
            resources.ApplyResources(this.savedItemsLabel, "savedItemsLabel");
            this.savedItemsLabel.Font = null;
            this.savedItemsLabel.Name = "savedItemsLabel";
            // 
            // SaveProjectDialog
            // 
            this.AcceptButton = this.saveButton;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.savedItemsLabel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.savedItemListBox);
            this.Font = null;
            this.Name = "SaveProjectDialog";
            this.Shown += new System.EventHandler(this.SaveProjectDialogShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Button to save the selected data.
        /// </summary>
        public System.Windows.Forms.Button saveButton;
        /// <summary>
        /// Button to close this dialog.
        /// </summary>
        public System.Windows.Forms.Button cancelButton;
        /// <summary>
        /// CheckBox to select the saved data.
        /// </summary>
        public System.Windows.Forms.CheckedListBox savedItemListBox;
        private System.Windows.Forms.Label savedItemsLabel;
    }
}