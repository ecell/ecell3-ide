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
            this.OpenButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.PrjCommentLabel = new System.Windows.Forms.Label();
            this.PrjDateText = new System.Windows.Forms.TextBox();
            this.PrjCommentText = new System.Windows.Forms.TextBox();
            this.PrjDateLabel = new System.Windows.Forms.Label();
            this.PrjIDText = new System.Windows.Forms.TextBox();
            this.PrjIDLabel = new System.Windows.Forms.Label();
            this.PrjTreeView = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenButton
            // 
            resources.ApplyResources(this.OpenButton, "OpenButton");
            this.OpenButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.UseVisualStyleBackColor = true;
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
            // PrjCommentLabel
            // 
            resources.ApplyResources(this.PrjCommentLabel, "PrjCommentLabel");
            this.PrjCommentLabel.Name = "PrjCommentLabel";
            // 
            // PrjDateText
            // 
            resources.ApplyResources(this.PrjDateText, "PrjDateText");
            this.PrjDateText.Name = "PrjDateText";
            this.PrjDateText.ReadOnly = true;
            this.PrjDateText.TabStop = false;
            // 
            // PrjCommentText
            // 
            resources.ApplyResources(this.PrjCommentText, "PrjCommentText");
            this.PrjCommentText.Name = "PrjCommentText";
            this.PrjCommentText.ReadOnly = true;
            this.PrjCommentText.TabStop = false;
            // 
            // PrjDateLabel
            // 
            resources.ApplyResources(this.PrjDateLabel, "PrjDateLabel");
            this.PrjDateLabel.Name = "PrjDateLabel";
            // 
            // PrjIDText
            // 
            resources.ApplyResources(this.PrjIDText, "PrjIDText");
            this.PrjIDText.Name = "PrjIDText";
            this.PrjIDText.ReadOnly = true;
            this.PrjIDText.TabStop = false;
            // 
            // PrjIDLabel
            // 
            resources.ApplyResources(this.PrjIDLabel, "PrjIDLabel");
            this.PrjIDLabel.Name = "PrjIDLabel";
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
            this.splitContainer1.Panel2.Controls.Add(this.PrjCommentText);
            this.splitContainer1.Panel2.Controls.Add(this.PrjIDLabel);
            this.splitContainer1.Panel2.Controls.Add(this.PrjCommentLabel);
            this.splitContainer1.Panel2.Controls.Add(this.PrjIDText);
            this.splitContainer1.Panel2.Controls.Add(this.PrjDateText);
            this.splitContainer1.Panel2.Controls.Add(this.PrjDateLabel);
            // 
            // ProjectExplorerDialog
            // 
            this.AcceptButton = this.OpenButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OpenButton);
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
        public System.Windows.Forms.Button OpenButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button cancelButton;
        /// <summary>
        /// ImageList of OPPrjTreeView.
        /// </summary>
        public System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.Label PrjCommentLabel;
        public System.Windows.Forms.TextBox PrjDateText;
        public System.Windows.Forms.TextBox PrjCommentText;
        private System.Windows.Forms.Label PrjDateLabel;
        public System.Windows.Forms.TextBox PrjIDText;
        private System.Windows.Forms.Label PrjIDLabel;
        public System.Windows.Forms.TreeView PrjTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}