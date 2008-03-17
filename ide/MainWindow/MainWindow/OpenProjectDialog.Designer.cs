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
    partial class OpenProjectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenProjectDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.OPOpenButton = new System.Windows.Forms.Button();
            this.OPCancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.OPPrjTreeView = new System.Windows.Forms.TreeView();
            this.OPImageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OPPrjIDText = new System.Windows.Forms.TextBox();
            this.OPDateText = new System.Windows.Forms.TextBox();
            this.OPCommentText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.OPOpenButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.OPCancelButton, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // OPOpenButton
            // 
            this.OPOpenButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.OPOpenButton, "OPOpenButton");
            this.OPOpenButton.Name = "OPOpenButton";
            this.OPOpenButton.UseVisualStyleBackColor = true;
            // 
            // OPCancelButton
            // 
            this.OPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.OPCancelButton, "OPCancelButton");
            this.OPCancelButton.Name = "OPCancelButton";
            this.OPCancelButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.OPPrjTreeView, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // OPPrjTreeView
            // 
            resources.ApplyResources(this.OPPrjTreeView, "OPPrjTreeView");
            this.OPPrjTreeView.ImageList = this.OPImageList;
            this.OPPrjTreeView.Name = "OPPrjTreeView";
            this.OPPrjTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeMouseClick);
            // 
            // OPImageList
            // 
            this.OPImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("OPImageList.ImageStream")));
            this.OPImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.OPImageList.Images.SetKeyName(0, "folder.png");
            this.OPImageList.Images.SetKeyName(1, "nav_up_right_blue.png");
            this.OPImageList.Images.SetKeyName(2, "nav_up_right_green.png");
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.OPPrjIDText, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.OPDateText, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.OPCommentText, 0, 7);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // OPPrjIDText
            // 
            resources.ApplyResources(this.OPPrjIDText, "OPPrjIDText");
            this.OPPrjIDText.Name = "OPPrjIDText";
            this.OPPrjIDText.ReadOnly = true;
            // 
            // OPDateText
            // 
            resources.ApplyResources(this.OPDateText, "OPDateText");
            this.OPDateText.Name = "OPDateText";
            this.OPDateText.ReadOnly = true;
            // 
            // OPCommentText
            // 
            resources.ApplyResources(this.OPCommentText, "OPCommentText");
            this.OPCommentText.Name = "OPCommentText";
            this.OPCommentText.ReadOnly = true;
            // 
            // OpenProjectDialog
            // 
            this.AcceptButton = this.OPOpenButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OPCancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OpenProjectDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        /// <summary>
        /// Button to open the selected project.
        /// </summary>
        public System.Windows.Forms.Button OPOpenButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button OPCancelButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        /// <summary>
        /// TreeView of project.
        /// </summary>
        public System.Windows.Forms.TreeView OPPrjTreeView;
        /// <summary>
        /// ImageList of OPPrjTreeView.
        /// </summary>
        public System.Windows.Forms.ImageList OPImageList;
        /// <summary>
        /// TextBox to input the comment of project.
        /// </summary>
        public System.Windows.Forms.TextBox OPCommentText;
        /// <summary>
        /// TextBox to input the update date.
        /// </summary>
        public System.Windows.Forms.TextBox OPDateText;
        /// <summary>
        /// TextBox to input the project id.
        /// </summary>
        public System.Windows.Forms.TextBox OPPrjIDText;
    }
}