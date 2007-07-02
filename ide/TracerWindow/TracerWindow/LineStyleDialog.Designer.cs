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

namespace EcellLib.TracerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineStyleDialog));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.LSApplyButton = new System.Windows.Forms.Button();
            this.LSCloseButton = new System.Windows.Forms.Button();
            this.dashDotDotRadioButton = new System.Windows.Forms.RadioButton();
            this.dotRadioButton = new System.Windows.Forms.RadioButton();
            this.dashDotRadioButton = new System.Windows.Forms.RadioButton();
            this.dashRadioButton = new System.Windows.Forms.RadioButton();
            this.solidRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dashDotDotRadioButton);
            this.groupBox1.Controls.Add(this.dotRadioButton);
            this.groupBox1.Controls.Add(this.dashDotRadioButton);
            this.groupBox1.Controls.Add(this.dashRadioButton);
            this.groupBox1.Controls.Add(this.solidRadioButton);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.LSApplyButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.LSCloseButton, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // LSApplyButton
            // 
            resources.ApplyResources(this.LSApplyButton, "LSApplyButton");
            this.LSApplyButton.Name = "LSApplyButton";
            this.LSApplyButton.UseVisualStyleBackColor = true;
            // 
            // LSCloseButton
            // 
            resources.ApplyResources(this.LSCloseButton, "LSCloseButton");
            this.LSCloseButton.Name = "LSCloseButton";
            this.LSCloseButton.UseVisualStyleBackColor = true;
            this.LSCloseButton.Click += new System.EventHandler(this.LineCancelButton_Click);
            // 
            // dashDotDotRadioButton
            // 
            resources.ApplyResources(this.dashDotDotRadioButton, "dashDotDotRadioButton");
            this.dashDotDotRadioButton.Image = global::TracerWindow.Properties.Resources.dashdotdot;
            this.dashDotDotRadioButton.Name = "dashDotDotRadioButton";
            this.dashDotDotRadioButton.TabStop = true;
            this.dashDotDotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dotRadioButton
            // 
            resources.ApplyResources(this.dotRadioButton, "dotRadioButton");
            this.dotRadioButton.Image = global::TracerWindow.Properties.Resources.dot;
            this.dotRadioButton.Name = "dotRadioButton";
            this.dotRadioButton.TabStop = true;
            this.dotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dashDotRadioButton
            // 
            resources.ApplyResources(this.dashDotRadioButton, "dashDotRadioButton");
            this.dashDotRadioButton.Image = global::TracerWindow.Properties.Resources.dashdot;
            this.dashDotRadioButton.Name = "dashDotRadioButton";
            this.dashDotRadioButton.TabStop = true;
            this.dashDotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dashRadioButton
            // 
            resources.ApplyResources(this.dashRadioButton, "dashRadioButton");
            this.dashRadioButton.Image = global::TracerWindow.Properties.Resources.dash;
            this.dashRadioButton.Name = "dashRadioButton";
            this.dashRadioButton.TabStop = true;
            this.dashRadioButton.UseVisualStyleBackColor = true;
            // 
            // solidRadioButton
            // 
            resources.ApplyResources(this.solidRadioButton, "solidRadioButton");
            this.solidRadioButton.Image = global::TracerWindow.Properties.Resources.solid;
            this.solidRadioButton.Name = "solidRadioButton";
            this.solidRadioButton.TabStop = true;
            this.solidRadioButton.UseVisualStyleBackColor = true;
            // 
            // LineStyleDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LineStyleDialog";
            this.Shown += new System.EventHandler(this.LineStyleShown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        /// <summary>
        /// Radio button to set line style with Solid.
        /// </summary>
        public System.Windows.Forms.RadioButton solidRadioButton;
        /// <summary>
        /// Radio button to set line style with Dash.
        /// </summary>
        public System.Windows.Forms.RadioButton dashRadioButton;
        /// <summary>
        /// Radio button to set line style with DashDot.
        /// </summary>

        public System.Windows.Forms.RadioButton dashDotRadioButton;
        /// <summary>
        /// Radio button to set line style with Dot.
        /// </summary>
        public System.Windows.Forms.RadioButton dotRadioButton;
        /// <summary>
        /// Radio button to set line style with DashDotDot.
        /// </summary>
        public System.Windows.Forms.RadioButton dashDotDotRadioButton;
        private System.Windows.Forms.Button LSApplyButton;
        private System.Windows.Forms.Button LSCloseButton;


    }
}