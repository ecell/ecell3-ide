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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
namespace Ecell.IDE.MainWindow
{
    partial class ProjectExplorerDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorerDialog));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.PrjTreeView = new System.Windows.Forms.TreeView();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.commentText = new System.Windows.Forms.TextBox();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.commentLabel = new System.Windows.Forms.Label();
            this.projectNameText = new System.Windows.Forms.TextBox();
            this.dateText = new System.Windows.Forms.TextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.openButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.PrjTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.commentText);
            this.splitContainer1.Panel2.Controls.Add(this.projectNameLabel);
            this.splitContainer1.Panel2.Controls.Add(this.commentLabel);
            this.splitContainer1.Panel2.Controls.Add(this.projectNameText);
            this.splitContainer1.Panel2.Controls.Add(this.dateText);
            this.splitContainer1.Panel2.Controls.Add(this.dateLabel);
            // 
            // PrjTreeView
            // 
            resources.ApplyResources(this.PrjTreeView, "PrjTreeView");
            this.PrjTreeView.ImageList = this.ImageList;
            this.PrjTreeView.Name = "PrjTreeView";
            this.PrjTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeMouseDoubleClick);

            this.PrjTreeView.LabelEdit = true;
            this.PrjTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.PrjTreeView_AfterLabelEdit);
            this.PrjTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MPPrjTreeView_MouseDown);
            this.PrjTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeMouseClick);
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "folder.png");
            this.ImageList.Images.SetKeyName(1, "project.png");
            this.ImageList.Images.SetKeyName(2, "model.png");
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // commentText
            // 
            resources.ApplyResources(this.commentText, "commentText");
            this.commentText.Name = "commentText";
            this.commentText.ReadOnly = true;
            this.commentText.TabStop = false;
            // 
            // projectNameLabel
            // 
            resources.ApplyResources(this.projectNameLabel, "projectNameLabel");
            this.projectNameLabel.Name = "projectNameLabel";
            // 
            // commentLabel
            // 
            resources.ApplyResources(this.commentLabel, "commentLabel");
            this.commentLabel.Name = "commentLabel";
            // 
            // projectNameText
            // 
            resources.ApplyResources(this.projectNameText, "projectNameText");
            this.projectNameText.Name = "projectNameText";
            this.projectNameText.ReadOnly = true;
            this.projectNameText.TabStop = false;
            // 
            // dateText
            // 
            resources.ApplyResources(this.dateText, "dateText");
            this.dateText.Name = "dateText";
            this.dateText.ReadOnly = true;
            this.dateText.TabStop = false;
            // 
            // dateLabel
            // 
            resources.ApplyResources(this.dateLabel, "dateLabel");
            this.dateLabel.Name = "dateLabel";
            // 
            // openButton
            // 
            resources.ApplyResources(this.openButton, "openButton");
            this.openButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.openButton.Name = "openButton";
            this.openButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // ProjectExplorerDialog
            // 
            this.AcceptButton = this.openButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.openButton);
            this.Name = "ProjectExplorerDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectExplorerDialog_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Button to open the selected project.
        /// </summary>
        private System.Windows.Forms.Button openButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        private System.Windows.Forms.Button cancelButton;
        /// <summary>
        /// ImageList of OPPrjTreeView.
        /// </summary>
        private System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.TextBox dateText;
        private System.Windows.Forms.TextBox commentText;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.TextBox projectNameText;
        private System.Windows.Forms.Label projectNameLabel;
        private System.Windows.Forms.TreeView PrjTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
