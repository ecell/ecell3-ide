//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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

namespace Ecell.IDE.Plugins.TracerWindow
{
    partial class LineStyleDialog
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
            System.Windows.Forms.GroupBox groupBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineStyleDialog));
            System.Windows.Forms.GroupBox groupBox2;
            this.dashDotDotRadioButton = new System.Windows.Forms.RadioButton();
            this.dotRadioButton = new System.Windows.Forms.RadioButton();
            this.dashDotRadioButton = new System.Windows.Forms.RadioButton();
            this.dashRadioButton = new System.Windows.Forms.RadioButton();
            this.solidRadioButton = new System.Windows.Forms.RadioButton();
            this.lineTextBox = new System.Windows.Forms.TextBox();
            this.LSApplyButton = new System.Windows.Forms.Button();
            this.LSCloseButton = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Controls.Add(this.dashDotDotRadioButton);
            groupBox1.Controls.Add(this.dotRadioButton);
            groupBox1.Controls.Add(this.dashDotRadioButton);
            groupBox1.Controls.Add(this.dashRadioButton);
            groupBox1.Controls.Add(this.solidRadioButton);
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // dashDotDotRadioButton
            // 
            resources.ApplyResources(this.dashDotDotRadioButton, "dashDotDotRadioButton");
            this.dashDotDotRadioButton.Image = global::Ecell.IDE.Plugins.TracerWindow.Properties.Resources.dashdotdot;
            this.dashDotDotRadioButton.Name = "dashDotDotRadioButton";
            this.dashDotDotRadioButton.TabStop = true;
            this.dashDotDotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dotRadioButton
            // 
            resources.ApplyResources(this.dotRadioButton, "dotRadioButton");
            this.dotRadioButton.Image = global::Ecell.IDE.Plugins.TracerWindow.Properties.Resources.dot;
            this.dotRadioButton.Name = "dotRadioButton";
            this.dotRadioButton.TabStop = true;
            this.dotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dashDotRadioButton
            // 
            resources.ApplyResources(this.dashDotRadioButton, "dashDotRadioButton");
            this.dashDotRadioButton.Image = global::Ecell.IDE.Plugins.TracerWindow.Properties.Resources.dashdot;
            this.dashDotRadioButton.Name = "dashDotRadioButton";
            this.dashDotRadioButton.TabStop = true;
            this.dashDotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dashRadioButton
            // 
            resources.ApplyResources(this.dashRadioButton, "dashRadioButton");
            this.dashRadioButton.Image = global::Ecell.IDE.Plugins.TracerWindow.Properties.Resources.dash;
            this.dashRadioButton.Name = "dashRadioButton";
            this.dashRadioButton.TabStop = true;
            this.dashRadioButton.UseVisualStyleBackColor = true;
            // 
            // solidRadioButton
            // 
            resources.ApplyResources(this.solidRadioButton, "solidRadioButton");
            this.solidRadioButton.Image = global::Ecell.IDE.Plugins.TracerWindow.Properties.Resources.solid;
            this.solidRadioButton.Name = "solidRadioButton";
            this.solidRadioButton.TabStop = true;
            this.solidRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.lineTextBox);
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // lineTextBox
            // 
            resources.ApplyResources(this.lineTextBox, "lineTextBox");
            this.lineTextBox.Name = "lineTextBox";
            this.lineTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.lineTextBox_Validating);
            // 
            // LSApplyButton
            // 
            resources.ApplyResources(this.LSApplyButton, "LSApplyButton");
            this.LSApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.LSApplyButton.Name = "LSApplyButton";
            this.LSApplyButton.UseVisualStyleBackColor = true;
            // 
            // LSCloseButton
            // 
            resources.ApplyResources(this.LSCloseButton, "LSCloseButton");
            this.LSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LSCloseButton.Name = "LSCloseButton";
            this.LSCloseButton.UseVisualStyleBackColor = true;
            this.LSCloseButton.Click += new System.EventHandler(this.LineCancelButton_Click);
            // 
            // LineStyleDialog
            // 
            this.AcceptButton = this.LSApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.LSCloseButton;
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.LSApplyButton);
            this.Controls.Add(this.LSCloseButton);
            this.Name = "LineStyleDialog";
            this.Shown += new System.EventHandler(this.LineStyleShown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LineStyleDialogClosing);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LSApplyButton;
        private System.Windows.Forms.Button LSCloseButton;
        private System.Windows.Forms.RadioButton solidRadioButton;
        private System.Windows.Forms.RadioButton dashRadioButton;
        private System.Windows.Forms.RadioButton dashDotRadioButton;
        private System.Windows.Forms.RadioButton dotRadioButton;
        private System.Windows.Forms.RadioButton dashDotDotRadioButton;
        private System.Windows.Forms.TextBox lineTextBox;


    }
}