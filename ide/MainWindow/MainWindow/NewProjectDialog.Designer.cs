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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.textComment = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textModelName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.CPAddButton = new System.Windows.Forms.Button();
            this.CPRemoveButton = new System.Windows.Forms.Button();
            this.CPListBox = new System.Windows.Forms.ListBox();
            this.CPCreateButton = new System.Windows.Forms.Button();
            this.CPCancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textComment, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textModelName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
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
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.CPListBox, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.CPAddButton, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.CPRemoveButton, 0, 1);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
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
            // CPListBox
            // 
            resources.ApplyResources(this.CPListBox, "CPListBox");
            this.CPListBox.FormattingEnabled = true;
            this.CPListBox.Name = "CPListBox";
            // 
            // CPCreateButton
            // 
            this.CPCreateButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.CPCreateButton, "CPCreateButton");
            this.CPCreateButton.Name = "CPCreateButton";
            this.CPCreateButton.UseVisualStyleBackColor = true;
            // 
            // CPCancelButton
            // 
            this.CPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.CPCancelButton, "CPCancelButton");
            this.CPCancelButton.Name = "CPCancelButton";
            this.CPCancelButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.CPCancelButton, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.CPCreateButton, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // NewProjectDialog
            // 
            this.AcceptButton = this.CPCreateButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CPCancelButton;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "NewProjectDialog";
            this.Shown += new System.EventHandler(this.NewProjectDialogShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button CPAddButton;
        private System.Windows.Forms.Button CPRemoveButton;
        private System.Windows.Forms.ListBox CPListBox;
    }
}