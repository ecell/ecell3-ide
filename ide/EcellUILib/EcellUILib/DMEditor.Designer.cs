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
namespace Ecell.IDE
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
            this.DMEComileButton = new System.Windows.Forms.Button();
            this.DMESaveButton = new System.Windows.Forms.Button();
            this.DMELoadButton = new System.Windows.Forms.Button();
            this.DMETextBox = new System.Windows.Forms.RichTextBox();
            this.DMEOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.DMESaveAsButton = new System.Windows.Forms.Button();
            this.DMESaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // DMEComileButton
            // 
            resources.ApplyResources(this.DMEComileButton, "DMEComileButton");
            this.DMEComileButton.Name = "DMEComileButton";
            this.DMEComileButton.UseVisualStyleBackColor = true;
            this.DMEComileButton.Click += new System.EventHandler(this.DMECompileButtonClick);
            // 
            // DMESaveButton
            // 
            resources.ApplyResources(this.DMESaveButton, "DMESaveButton");
            this.DMESaveButton.Name = "DMESaveButton";
            this.DMESaveButton.UseVisualStyleBackColor = true;
            this.DMESaveButton.Click += new System.EventHandler(this.DMESaveButtonClick);
            // 
            // DMELoadButton
            // 
            resources.ApplyResources(this.DMELoadButton, "DMELoadButton");
            this.DMELoadButton.Name = "DMELoadButton";
            this.DMELoadButton.UseVisualStyleBackColor = true;
            this.DMELoadButton.Click += new System.EventHandler(this.DMELoadButtonClick);
            // 
            // DMETextBox
            // 
            resources.ApplyResources(this.DMETextBox, "DMETextBox");
            this.DMETextBox.Name = "DMETextBox";
            // 
            // DMEOpenFileDialog
            // 
            this.DMEOpenFileDialog.FileName = "DME";
            // 
            // DMESaveAsButton
            // 
            resources.ApplyResources(this.DMESaveAsButton, "DMESaveAsButton");
            this.DMESaveAsButton.Name = "DMESaveAsButton";
            this.DMESaveAsButton.UseVisualStyleBackColor = true;
            this.DMESaveAsButton.Click += new System.EventHandler(this.DMESaveAsButton_Click);
            // 
            // DMEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DMESaveAsButton);
            this.Controls.Add(this.DMEComileButton);
            this.Controls.Add(this.DMETextBox);
            this.Controls.Add(this.DMELoadButton);
            this.Controls.Add(this.DMESaveButton);
            this.Name = "DMEditor";
            this.Shown += new System.EventHandler(this.DMEditorShown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DMEditor_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Button to compile the source file.
        /// </summary>
        protected System.Windows.Forms.Button DMEComileButton;
        /// <summary>
        /// Button to save the source file.
        /// </summary>
        protected System.Windows.Forms.Button DMESaveButton;
        /// <summary>
        /// Button to load the source file.
        /// </summary>
        protected System.Windows.Forms.Button DMELoadButton;
        /// <summary>
        /// File selection dialog.
        /// </summary>
        protected System.Windows.Forms.OpenFileDialog DMEOpenFileDialog;
        /// <summary>
        /// RichTextBox to display the source file.
        /// </summary>
        protected System.Windows.Forms.RichTextBox DMETextBox;
        private System.Windows.Forms.Button DMESaveAsButton;
        protected System.Windows.Forms.SaveFileDialog DMESaveFileDialog;
    }
}