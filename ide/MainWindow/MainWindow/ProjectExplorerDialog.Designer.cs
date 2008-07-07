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
namespace EcellLib.MainWindow
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
            this.openButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.commentLabel = new System.Windows.Forms.Label();
            this.dateText = new System.Windows.Forms.TextBox();
            this.commentText = new System.Windows.Forms.TextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.idText = new System.Windows.Forms.TextBox();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.PrjTreeView = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "folder.png");
            this.ImageList.Images.SetKeyName(1, "project.png");
            this.ImageList.Images.SetKeyName(2, "model.png");
            // 
            // commentLabel
            // 
            resources.ApplyResources(this.commentLabel, "commentLabel");
            this.commentLabel.Name = "commentLabel";
            // 
            // dateText
            // 
            resources.ApplyResources(this.dateText, "dateText");
            this.dateText.Name = "dateText";
            this.dateText.ReadOnly = true;
            this.dateText.TabStop = false;
            // 
            // commentText
            // 
            resources.ApplyResources(this.commentText, "commentText");
            this.commentText.Name = "commentText";
            this.commentText.ReadOnly = true;
            this.commentText.TabStop = false;
            // 
            // dateLabel
            // 
            resources.ApplyResources(this.dateLabel, "dateLabel");
            this.dateLabel.Name = "dateLabel";
            // 
            // idText
            // 
            resources.ApplyResources(this.idText, "idText");
            this.idText.Name = "idText";
            this.idText.ReadOnly = true;
            this.idText.TabStop = false;
            // 
            // projectNameLabel
            // 
            resources.ApplyResources(this.projectNameLabel, "projectNameLabel");
            this.projectNameLabel.Name = "projectNameLabel";
            // 
            // PrjTreeView
            // 
            resources.ApplyResources(this.PrjTreeView, "PrjTreeView");
            this.PrjTreeView.ImageList = this.ImageList;
            this.PrjTreeView.Name = "PrjTreeView";
            this.PrjTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MPPrjTreeView_MouseDown);
            this.PrjTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeMouseClick);
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
            this.splitContainer1.Panel2.Controls.Add(this.commentText);
            this.splitContainer1.Panel2.Controls.Add(this.projectNameLabel);
            this.splitContainer1.Panel2.Controls.Add(this.commentLabel);
            this.splitContainer1.Panel2.Controls.Add(this.idText);
            this.splitContainer1.Panel2.Controls.Add(this.dateText);
            this.splitContainer1.Panel2.Controls.Add(this.dateLabel);
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
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Button to open the selected project.
        /// </summary>
        public System.Windows.Forms.Button openButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button cancelButton;
        /// <summary>
        /// ImageList of OPPrjTreeView.
        /// </summary>
        public System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.Label commentLabel;
        public System.Windows.Forms.TextBox dateText;
        public System.Windows.Forms.TextBox commentText;
        private System.Windows.Forms.Label dateLabel;
        public System.Windows.Forms.TextBox idText;
        private System.Windows.Forms.Label projectNameLabel;
        public System.Windows.Forms.TreeView PrjTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}