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
            this.SPSaveButton = new System.Windows.Forms.Button();
            this.SPCancelButton = new System.Windows.Forms.Button();
            this.CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SPSaveButton
            // 
            resources.ApplyResources(this.SPSaveButton, "SPSaveButton");
            this.SPSaveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SPSaveButton.Name = "SPSaveButton";
            this.SPSaveButton.UseVisualStyleBackColor = true;
            // 
            // SPCancelButton
            // 
            resources.ApplyResources(this.SPCancelButton, "SPCancelButton");
            this.SPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SPCancelButton.Name = "SPCancelButton";
            this.SPCancelButton.UseVisualStyleBackColor = true;
            // 
            // CheckedListBox
            // 
            this.CheckedListBox.FormattingEnabled = true;
            resources.ApplyResources(this.CheckedListBox, "CheckedListBox");
            this.CheckedListBox.Name = "CheckedListBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // SaveProjectDialog
            // 
            this.AcceptButton = this.SPSaveButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.SPCancelButton;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SPSaveButton);
            this.Controls.Add(this.SPCancelButton);
            this.Controls.Add(this.CheckedListBox);
            this.Name = "SaveProjectDialog";
            this.Shown += new System.EventHandler(this.SaveProjectDialogShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Button to save the selected data.
        /// </summary>
        public System.Windows.Forms.Button SPSaveButton;
        /// <summary>
        /// Button to close this dialog.
        /// </summary>
        public System.Windows.Forms.Button SPCancelButton;
        /// <summary>
        /// CheckBox to select the saved data.
        /// </summary>
        public System.Windows.Forms.CheckedListBox CheckedListBox;
        private System.Windows.Forms.Label label1;
    }
}