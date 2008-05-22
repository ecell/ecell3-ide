//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
namespace EcellLib.EntityListWindow
{
    partial class DMEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DMEditor));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.DMECloseButton = new System.Windows.Forms.Button();
            this.DMEComileButton = new System.Windows.Forms.Button();
            this.DMESaveButton = new System.Windows.Forms.Button();
            this.DMETextBox = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.DMETextBox, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.DMECloseButton, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.DMEComileButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.DMESaveButton, 2, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // DMECloseButton
            // 
            resources.ApplyResources(this.DMECloseButton, "DMECloseButton");
            this.DMECloseButton.Name = "DMECloseButton";
            this.DMECloseButton.UseVisualStyleBackColor = true;
            this.DMECloseButton.Click += new System.EventHandler(this.DMECloseButtonClick);
            // 
            // DMEComileButton
            // 
            resources.ApplyResources(this.DMEComileButton, "DMEComileButton");
            this.DMEComileButton.Name = "DMEComileButton";
            this.DMEComileButton.UseVisualStyleBackColor = true;
            // 
            // DMESaveButton
            // 
            resources.ApplyResources(this.DMESaveButton, "DMESaveButton");
            this.DMESaveButton.Name = "DMESaveButton";
            this.DMESaveButton.UseVisualStyleBackColor = true;
            this.DMESaveButton.Click += new System.EventHandler(this.DMESaveButtonClick);
            // 
            // DMETextBox
            // 
            resources.ApplyResources(this.DMETextBox, "DMETextBox");
            this.DMETextBox.Name = "DMETextBox";
            // 
            // DMEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DMEditor";
            this.Shown += new System.EventHandler(this.DMEditorShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button DMECloseButton;
        private System.Windows.Forms.Button DMEComileButton;
        private System.Windows.Forms.Button DMESaveButton;
        private System.Windows.Forms.RichTextBox DMETextBox;
    }
}