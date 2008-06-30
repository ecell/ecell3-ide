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
            this.MPOpenButton = new System.Windows.Forms.Button();
            this.MPCancelButton = new System.Windows.Forms.Button();
            this.MPPrjTreeView = new System.Windows.Forms.TreeView();
            this.MPImageList = new System.Windows.Forms.ImageList(this.components);
            this.MPPrjDateLabel = new System.Windows.Forms.Label();
            this.MPPrjCommentLabel = new System.Windows.Forms.Label();
            this.MPPrjIDText = new System.Windows.Forms.TextBox();
            this.MPPrjDateText = new System.Windows.Forms.TextBox();
            this.MPPrjCommentText = new System.Windows.Forms.TextBox();
            this.MPPrjIDLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MPOpenButton
            // 
            resources.ApplyResources(this.MPOpenButton, "MPOpenButton");
            this.MPOpenButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.MPOpenButton.Name = "MPOpenButton";
            this.MPOpenButton.UseVisualStyleBackColor = true;
            // 
            // MPCancelButton
            // 
            resources.ApplyResources(this.MPCancelButton, "MPCancelButton");
            this.MPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MPCancelButton.Name = "MPCancelButton";
            this.MPCancelButton.UseVisualStyleBackColor = true;
            // 
            // MPPrjTreeView
            // 
            resources.ApplyResources(this.MPPrjTreeView, "MPPrjTreeView");
            this.MPPrjTreeView.ImageList = this.MPImageList;
            this.MPPrjTreeView.Name = "MPPrjTreeView";
            this.MPPrjTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MPPrjTreeView_MouseDown);
            this.MPPrjTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeMouseClick);
            // 
            // MPImageList
            // 
            this.MPImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MPImageList.ImageStream")));
            this.MPImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.MPImageList.Images.SetKeyName(0, "folder.png");
            this.MPImageList.Images.SetKeyName(1, "project.png");
            this.MPImageList.Images.SetKeyName(2, "model.png");
            // 
            // MPPrjDateLabel
            // 
            resources.ApplyResources(this.MPPrjDateLabel, "MPPrjDateLabel");
            this.MPPrjDateLabel.Name = "MPPrjDateLabel";
            // 
            // MPPrjCommentLabel
            // 
            resources.ApplyResources(this.MPPrjCommentLabel, "MPPrjCommentLabel");
            this.MPPrjCommentLabel.Name = "MPPrjCommentLabel";
            // 
            // MPPrjIDText
            // 
            resources.ApplyResources(this.MPPrjIDText, "MPPrjIDText");
            this.MPPrjIDText.Name = "MPPrjIDText";
            this.MPPrjIDText.ReadOnly = true;
            this.MPPrjIDText.TabStop = false;
            // 
            // MPPrjDateText
            // 
            resources.ApplyResources(this.MPPrjDateText, "MPPrjDateText");
            this.MPPrjDateText.Name = "MPPrjDateText";
            this.MPPrjDateText.ReadOnly = true;
            this.MPPrjDateText.TabStop = false;
            // 
            // MPPrjCommentText
            // 
            resources.ApplyResources(this.MPPrjCommentText, "MPPrjCommentText");
            this.MPPrjCommentText.Name = "MPPrjCommentText";
            this.MPPrjCommentText.ReadOnly = true;
            this.MPPrjCommentText.TabStop = false;
            // 
            // MPPrjIDLabel
            // 
            resources.ApplyResources(this.MPPrjIDLabel, "MPPrjIDLabel");
            this.MPPrjIDLabel.Name = "MPPrjIDLabel";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MPPrjTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.MPPrjIDLabel);
            this.splitContainer1.Panel2.Controls.Add(this.MPPrjIDText);
            this.splitContainer1.Panel2.Controls.Add(this.MPPrjDateLabel);
            this.splitContainer1.Panel2.Controls.Add(this.MPPrjCommentText);
            this.splitContainer1.Panel2.Controls.Add(this.MPPrjDateText);
            this.splitContainer1.Panel2.Controls.Add(this.MPPrjCommentLabel);
            // 
            // ProjectExplorerDialog
            // 
            this.AcceptButton = this.MPOpenButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.MPCancelButton;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.MPCancelButton);
            this.Controls.Add(this.MPOpenButton);
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
        public System.Windows.Forms.Button MPOpenButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button MPCancelButton;
        private System.Windows.Forms.Label MPPrjIDLabel;
        private System.Windows.Forms.Label MPPrjDateLabel;
        private System.Windows.Forms.Label MPPrjCommentLabel;
        /// <summary>
        /// TreeView of project.
        /// </summary>
        public System.Windows.Forms.TreeView MPPrjTreeView;
        /// <summary>
        /// ImageList of OPPrjTreeView.
        /// </summary>
        public System.Windows.Forms.ImageList MPImageList;
        /// <summary>
        /// TextBox to input the comment of project.
        /// </summary>
        public System.Windows.Forms.TextBox MPPrjCommentText;
        /// <summary>
        /// TextBox to input the update date.
        /// </summary>
        public System.Windows.Forms.TextBox MPPrjDateText;
        /// <summary>
        /// TextBox to input the project id.
        /// </summary>
        public System.Windows.Forms.TextBox MPPrjIDText;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}