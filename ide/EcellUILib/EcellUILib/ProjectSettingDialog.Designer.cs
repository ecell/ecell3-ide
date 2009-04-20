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
namespace Ecell.IDE
{
    partial class ProjectSettingDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectSettingDialog));
            this.labelName = new System.Windows.Forms.Label();
            this.labelComment = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.textComment = new System.Windows.Forms.TextBox();
            this.CPCreateButton = new System.Windows.Forms.Button();
            this.CPCancelButton = new System.Windows.Forms.Button();
            this.labelCreator = new System.Windows.Forms.Label();
            this.textCreator = new System.Windows.Forms.TextBox();
            this.labelCreated = new System.Windows.Forms.Label();
            this.textCreated = new System.Windows.Forms.TextBox();
            this.labelUpdate = new System.Windows.Forms.Label();
            this.textLastUpdate = new System.Windows.Forms.TextBox();
            this.labelEditCount = new System.Windows.Forms.Label();
            this.textEditCount = new System.Windows.Forms.TextBox();
            this.labelDefaultParameter = new System.Windows.Forms.Label();
            this.textDefaultParameter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AccessibleDescription = null;
            this.labelName.AccessibleName = null;
            resources.ApplyResources(this.labelName, "labelName");
            this.labelName.Font = null;
            this.labelName.Name = "labelName";
            // 
            // labelComment
            // 
            this.labelComment.AccessibleDescription = null;
            this.labelComment.AccessibleName = null;
            resources.ApplyResources(this.labelComment, "labelComment");
            this.labelComment.Font = null;
            this.labelComment.Name = "labelComment";
            // 
            // textName
            // 
            this.textName.AccessibleDescription = null;
            this.textName.AccessibleName = null;
            resources.ApplyResources(this.textName, "textName");
            this.textName.BackgroundImage = null;
            this.textName.Font = null;
            this.textName.Name = "textName";
            // 
            // textComment
            // 
            this.textComment.AcceptsReturn = true;
            this.textComment.AccessibleDescription = null;
            this.textComment.AccessibleName = null;
            resources.ApplyResources(this.textComment, "textComment");
            this.textComment.BackgroundImage = null;
            this.textComment.Font = null;
            this.textComment.Name = "textComment";
            // 
            // CPCreateButton
            // 
            this.CPCreateButton.AccessibleDescription = null;
            this.CPCreateButton.AccessibleName = null;
            resources.ApplyResources(this.CPCreateButton, "CPCreateButton");
            this.CPCreateButton.BackgroundImage = null;
            this.CPCreateButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CPCreateButton.Font = null;
            this.CPCreateButton.Name = "CPCreateButton";
            this.CPCreateButton.UseVisualStyleBackColor = true;
            // 
            // CPCancelButton
            // 
            this.CPCancelButton.AccessibleDescription = null;
            this.CPCancelButton.AccessibleName = null;
            resources.ApplyResources(this.CPCancelButton, "CPCancelButton");
            this.CPCancelButton.BackgroundImage = null;
            this.CPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CPCancelButton.Font = null;
            this.CPCancelButton.Name = "CPCancelButton";
            this.CPCancelButton.UseVisualStyleBackColor = true;
            // 
            // labelCreator
            // 
            this.labelCreator.AccessibleDescription = null;
            this.labelCreator.AccessibleName = null;
            resources.ApplyResources(this.labelCreator, "labelCreator");
            this.labelCreator.Font = null;
            this.labelCreator.Name = "labelCreator";
            // 
            // textCreator
            // 
            this.textCreator.AccessibleDescription = null;
            this.textCreator.AccessibleName = null;
            resources.ApplyResources(this.textCreator, "textCreator");
            this.textCreator.BackgroundImage = null;
            this.textCreator.Font = null;
            this.textCreator.Name = "textCreator";
            // 
            // labelCreated
            // 
            this.labelCreated.AccessibleDescription = null;
            this.labelCreated.AccessibleName = null;
            resources.ApplyResources(this.labelCreated, "labelCreated");
            this.labelCreated.Font = null;
            this.labelCreated.Name = "labelCreated";
            // 
            // textCreated
            // 
            this.textCreated.AccessibleDescription = null;
            this.textCreated.AccessibleName = null;
            resources.ApplyResources(this.textCreated, "textCreated");
            this.textCreated.BackgroundImage = null;
            this.textCreated.Font = null;
            this.textCreated.Name = "textCreated";
            this.textCreated.ReadOnly = true;
            // 
            // labelUpdate
            // 
            this.labelUpdate.AccessibleDescription = null;
            this.labelUpdate.AccessibleName = null;
            resources.ApplyResources(this.labelUpdate, "labelUpdate");
            this.labelUpdate.Font = null;
            this.labelUpdate.Name = "labelUpdate";
            // 
            // textLastUpdate
            // 
            this.textLastUpdate.AccessibleDescription = null;
            this.textLastUpdate.AccessibleName = null;
            resources.ApplyResources(this.textLastUpdate, "textLastUpdate");
            this.textLastUpdate.BackgroundImage = null;
            this.textLastUpdate.Font = null;
            this.textLastUpdate.Name = "textLastUpdate";
            this.textLastUpdate.ReadOnly = true;
            // 
            // labelEditCount
            // 
            this.labelEditCount.AccessibleDescription = null;
            this.labelEditCount.AccessibleName = null;
            resources.ApplyResources(this.labelEditCount, "labelEditCount");
            this.labelEditCount.Font = null;
            this.labelEditCount.Name = "labelEditCount";
            // 
            // textEditCount
            // 
            this.textEditCount.AccessibleDescription = null;
            this.textEditCount.AccessibleName = null;
            resources.ApplyResources(this.textEditCount, "textEditCount");
            this.textEditCount.BackgroundImage = null;
            this.textEditCount.Font = null;
            this.textEditCount.Name = "textEditCount";
            this.textEditCount.ReadOnly = true;
            // 
            // labelDefaultParameter
            // 
            this.labelDefaultParameter.AccessibleDescription = null;
            this.labelDefaultParameter.AccessibleName = null;
            resources.ApplyResources(this.labelDefaultParameter, "labelDefaultParameter");
            this.labelDefaultParameter.Font = null;
            this.labelDefaultParameter.Name = "labelDefaultParameter";
            // 
            // textDefaultParameter
            // 
            this.textDefaultParameter.AccessibleDescription = null;
            this.textDefaultParameter.AccessibleName = null;
            resources.ApplyResources(this.textDefaultParameter, "textDefaultParameter");
            this.textDefaultParameter.BackgroundImage = null;
            this.textDefaultParameter.Font = null;
            this.textDefaultParameter.Name = "textDefaultParameter";
            this.textDefaultParameter.ReadOnly = true;
            // 
            // ProjectSettingDialog
            // 
            this.AcceptButton = this.CPCreateButton;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.CPCancelButton;
            this.Controls.Add(this.labelDefaultParameter);
            this.Controls.Add(this.textDefaultParameter);
            this.Controls.Add(this.labelEditCount);
            this.Controls.Add(this.textEditCount);
            this.Controls.Add(this.labelUpdate);
            this.Controls.Add(this.textLastUpdate);
            this.Controls.Add(this.labelCreated);
            this.Controls.Add(this.textCreated);
            this.Controls.Add(this.labelCreator);
            this.Controls.Add(this.textCreator);
            this.Controls.Add(this.CPCreateButton);
            this.Controls.Add(this.CPCancelButton);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textComment);
            this.Controls.Add(this.labelComment);
            this.Controls.Add(this.textName);
            this.Font = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectSettingDialog";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelComment;
        /// <summary>
        /// Button to create the project by using input properties.
        /// </summary>
        public System.Windows.Forms.Button CPCreateButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button CPCancelButton;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.TextBox textComment;
        private System.Windows.Forms.Label labelCreator;
        private System.Windows.Forms.TextBox textCreator;
        private System.Windows.Forms.Label labelCreated;
        private System.Windows.Forms.TextBox textCreated;
        private System.Windows.Forms.Label labelUpdate;
        private System.Windows.Forms.TextBox textLastUpdate;
        private System.Windows.Forms.Label labelEditCount;
        private System.Windows.Forms.TextBox textEditCount;
        private System.Windows.Forms.Label labelDefaultParameter;
        private System.Windows.Forms.TextBox textDefaultParameter;
    }
}