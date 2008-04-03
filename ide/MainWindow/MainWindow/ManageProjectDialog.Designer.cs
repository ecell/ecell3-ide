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
    partial class ManageProjectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageProjectDialog));
            this.MPLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MPButtonPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MPOpenButton = new System.Windows.Forms.Button();
            this.MPCancelButton = new System.Windows.Forms.Button();
            this.MPGUIPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MPPrjTreeView = new System.Windows.Forms.TreeView();
            this.MPImageList = new System.Windows.Forms.ImageList(this.components);
            this.MPMessagePanel = new System.Windows.Forms.TableLayoutPanel();
            this.MPPrjIDLabel = new System.Windows.Forms.Label();
            this.MPPrjDateLabel = new System.Windows.Forms.Label();
            this.MPPrjCommentLabel = new System.Windows.Forms.Label();
            this.MPPrjIDText = new System.Windows.Forms.TextBox();
            this.MPPrjDateText = new System.Windows.Forms.TextBox();
            this.MPPrjCommentText = new System.Windows.Forms.TextBox();
            this.MPLayoutPanel.SuspendLayout();
            this.MPButtonPanel.SuspendLayout();
            this.MPGUIPanel.SuspendLayout();
            this.MPMessagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MPLayoutPanel
            // 
            resources.ApplyResources(this.MPLayoutPanel, "MPLayoutPanel");
            this.MPLayoutPanel.Controls.Add(this.MPButtonPanel, 0, 1);
            this.MPLayoutPanel.Controls.Add(this.MPGUIPanel, 0, 0);
            this.MPLayoutPanel.Name = "MPLayoutPanel";
            // 
            // MPButtonPanel
            // 
            resources.ApplyResources(this.MPButtonPanel, "MPButtonPanel");
            this.MPButtonPanel.Controls.Add(this.MPOpenButton, 1, 0);
            this.MPButtonPanel.Controls.Add(this.MPCancelButton, 3, 0);
            this.MPButtonPanel.Name = "MPButtonPanel";
            // 
            // MPOpenButton
            // 
            this.MPOpenButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.MPOpenButton, "MPOpenButton");
            this.MPOpenButton.Name = "MPOpenButton";
            this.MPOpenButton.UseVisualStyleBackColor = true;
            // 
            // MPCancelButton
            // 
            this.MPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.MPCancelButton, "MPCancelButton");
            this.MPCancelButton.Name = "MPCancelButton";
            this.MPCancelButton.UseVisualStyleBackColor = true;
            // 
            // MPGUIPanel
            // 
            resources.ApplyResources(this.MPGUIPanel, "MPGUIPanel");
            this.MPGUIPanel.Controls.Add(this.MPPrjTreeView, 0, 0);
            this.MPGUIPanel.Controls.Add(this.MPMessagePanel, 1, 0);
            this.MPGUIPanel.Name = "MPGUIPanel";
            // 
            // MPPrjTreeView
            // 
            resources.ApplyResources(this.MPPrjTreeView, "MPPrjTreeView");
            this.MPPrjTreeView.ImageList = this.MPImageList;
            this.MPPrjTreeView.Name = "MPPrjTreeView";
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
            // MPMessagePanel
            // 
            resources.ApplyResources(this.MPMessagePanel, "MPMessagePanel");
            this.MPMessagePanel.Controls.Add(this.MPPrjIDLabel, 0, 0);
            this.MPMessagePanel.Controls.Add(this.MPPrjDateLabel, 0, 3);
            this.MPMessagePanel.Controls.Add(this.MPPrjCommentLabel, 0, 6);
            this.MPMessagePanel.Controls.Add(this.MPPrjIDText, 0, 1);
            this.MPMessagePanel.Controls.Add(this.MPPrjDateText, 0, 4);
            this.MPMessagePanel.Controls.Add(this.MPPrjCommentText, 0, 7);
            this.MPMessagePanel.Name = "MPMessagePanel";
            // 
            // MPPrjIDLabel
            // 
            resources.ApplyResources(this.MPPrjIDLabel, "MPPrjIDLabel");
            this.MPPrjIDLabel.Name = "MPPrjIDLabel";
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
            // 
            // MPPrjDateText
            // 
            resources.ApplyResources(this.MPPrjDateText, "MPPrjDateText");
            this.MPPrjDateText.Name = "MPPrjDateText";
            this.MPPrjDateText.ReadOnly = true;
            // 
            // MPPrjCommentText
            // 
            resources.ApplyResources(this.MPPrjCommentText, "MPPrjCommentText");
            this.MPPrjCommentText.Name = "MPPrjCommentText";
            this.MPPrjCommentText.ReadOnly = true;
            // 
            // ManageProjectDialog
            // 
            this.AcceptButton = this.MPOpenButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.MPCancelButton;
            this.Controls.Add(this.MPLayoutPanel);
            this.Name = "ManageProjectDialog";
            this.MPLayoutPanel.ResumeLayout(false);
            this.MPButtonPanel.ResumeLayout(false);
            this.MPGUIPanel.ResumeLayout(false);
            this.MPMessagePanel.ResumeLayout(false);
            this.MPMessagePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MPLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel MPButtonPanel;
        /// <summary>
        /// Button to open the selected project.
        /// </summary>
        public System.Windows.Forms.Button MPOpenButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button MPCancelButton;
        private System.Windows.Forms.TableLayoutPanel MPGUIPanel;
        private System.Windows.Forms.TableLayoutPanel MPMessagePanel;
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
    }
}