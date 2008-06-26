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
namespace EcellLib.MainWindow
{
    partial class NewProjectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.textComment = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textModelName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CPAddButton = new System.Windows.Forms.Button();
            this.CPRemoveButton = new System.Windows.Forms.Button();
            this.CPCreateButton = new System.Windows.Forms.Button();
            this.CPCancelButton = new System.Windows.Forms.Button();
            this.CPListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
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
            // textName
            // 
            resources.ApplyResources(this.textName, "textName");
            this.textName.Name = "textName";
            this.textName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterKeyPress);
            // 
            // textComment
            // 
            resources.ApplyResources(this.textComment, "textComment");
            this.textComment.Name = "textComment";
            this.textComment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterKeyPress);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textModelName
            // 
            resources.ApplyResources(this.textModelName, "textModelName");
            this.textModelName.Name = "textModelName";
            this.textModelName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterKeyPress);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // CPAddButton
            // 
            resources.ApplyResources(this.CPAddButton, "CPAddButton");
            this.CPAddButton.Name = "CPAddButton";
            this.CPAddButton.UseVisualStyleBackColor = true;
            this.CPAddButton.Click += new System.EventHandler(this.ClickAddButton);
            // 
            // CPRemoveButton
            // 
            resources.ApplyResources(this.CPRemoveButton, "CPRemoveButton");
            this.CPRemoveButton.Name = "CPRemoveButton";
            this.CPRemoveButton.UseVisualStyleBackColor = true;
            this.CPRemoveButton.Click += new System.EventHandler(this.ClickRemoveButton);
            // 
            // CPCreateButton
            // 
            resources.ApplyResources(this.CPCreateButton, "CPCreateButton");
            this.CPCreateButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CPCreateButton.Name = "CPCreateButton";
            this.CPCreateButton.UseVisualStyleBackColor = true;
            // 
            // CPCancelButton
            // 
            resources.ApplyResources(this.CPCancelButton, "CPCancelButton");
            this.CPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CPCancelButton.Name = "CPCancelButton";
            this.CPCancelButton.UseVisualStyleBackColor = true;
            // 
            // CPListBox
            // 
            resources.ApplyResources(this.CPListBox, "CPListBox");
            this.CPListBox.FormattingEnabled = true;
            this.CPListBox.Name = "CPListBox";
            // 
            // NewProjectDialog
            // 
            this.AcceptButton = this.CPCreateButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CPCancelButton;
            this.Controls.Add(this.CPCancelButton);
            this.Controls.Add(this.CPRemoveButton);
            this.Controls.Add(this.CPCreateButton);
            this.Controls.Add(this.CPListBox);
            this.Controls.Add(this.CPAddButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textModelName);
            this.Controls.Add(this.textComment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textName);
            this.Name = "NewProjectDialog";
            this.Shown += new System.EventHandler(this.NewProjectDialogShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        /// <summary>
        /// TextBox to set project id.
        /// </summary>
        public System.Windows.Forms.TextBox textName;
        /// <summary>
        /// TextBox to set default model of project.
        /// </summary>
        public System.Windows.Forms.TextBox textModelName;
        /// <summary>
        /// TextBox to set the comment of project.
        /// </summary>
        public System.Windows.Forms.TextBox textComment;
        /// <summary>
        /// Button to create the project by using input properties.
        /// </summary>
        public System.Windows.Forms.Button CPCreateButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button CPCancelButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CPAddButton;
        private System.Windows.Forms.Button CPRemoveButton;
        private System.Windows.Forms.ListBox CPListBox;
    }
}