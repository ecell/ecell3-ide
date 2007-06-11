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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dashDotDotRadioButton = new System.Windows.Forms.RadioButton();
            this.dotRadioButton = new System.Windows.Forms.RadioButton();
            this.dashDotRadioButton = new System.Windows.Forms.RadioButton();
            this.dashRadioButton = new System.Windows.Forms.RadioButton();
            this.solidRadioButton = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.LineCancelButton = new System.Windows.Forms.Button();
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
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 231);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LineStyle";
            // 
            // dashDotDotRadioButton
            // 
            this.dashDotDotRadioButton.AutoSize = true;
            this.dashDotDotRadioButton.Image = global::TracerWindow.Properties.Resources.dashdotdot;
            this.dashDotDotRadioButton.Location = new System.Drawing.Point(15, 155);
            this.dashDotDotRadioButton.Name = "dashDotDotRadioButton";
            this.dashDotDotRadioButton.Size = new System.Drawing.Size(123, 22);
            this.dashDotDotRadioButton.TabIndex = 4;
            this.dashDotDotRadioButton.TabStop = true;
            this.dashDotDotRadioButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.dashDotDotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dotRadioButton
            // 
            this.dotRadioButton.AutoSize = true;
            this.dotRadioButton.Image = global::TracerWindow.Properties.Resources.dot;
            this.dotRadioButton.Location = new System.Drawing.Point(15, 122);
            this.dotRadioButton.Name = "dotRadioButton";
            this.dotRadioButton.Size = new System.Drawing.Size(123, 22);
            this.dotRadioButton.TabIndex = 3;
            this.dotRadioButton.TabStop = true;
            this.dotRadioButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.dotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dashDotRadioButton
            // 
            this.dashDotRadioButton.AutoSize = true;
            this.dashDotRadioButton.Image = global::TracerWindow.Properties.Resources.dashdot;
            this.dashDotRadioButton.Location = new System.Drawing.Point(15, 92);
            this.dashDotRadioButton.Name = "dashDotRadioButton";
            this.dashDotRadioButton.Size = new System.Drawing.Size(123, 22);
            this.dashDotRadioButton.TabIndex = 2;
            this.dashDotRadioButton.TabStop = true;
            this.dashDotRadioButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.dashDotRadioButton.UseVisualStyleBackColor = true;
            // 
            // dashRadioButton
            // 
            this.dashRadioButton.AutoSize = true;
            this.dashRadioButton.Image = global::TracerWindow.Properties.Resources.dash;
            this.dashRadioButton.Location = new System.Drawing.Point(15, 62);
            this.dashRadioButton.Name = "dashRadioButton";
            this.dashRadioButton.Size = new System.Drawing.Size(123, 22);
            this.dashRadioButton.TabIndex = 1;
            this.dashRadioButton.TabStop = true;
            this.dashRadioButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.dashRadioButton.UseVisualStyleBackColor = true;
            // 
            // solidRadioButton
            // 
            this.solidRadioButton.AutoSize = true;
            this.solidRadioButton.Image = global::TracerWindow.Properties.Resources.solid;
            this.solidRadioButton.Location = new System.Drawing.Point(15, 29);
            this.solidRadioButton.Name = "solidRadioButton";
            this.solidRadioButton.Size = new System.Drawing.Size(123, 22);
            this.solidRadioButton.TabIndex = 0;
            this.solidRadioButton.TabStop = true;
            this.solidRadioButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.solidRadioButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(182, 272);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ApplyButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.LineCancelButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 240);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(176, 29);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ApplyButton.Location = new System.Drawing.Point(6, 3);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 0;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            // 
            // LineCancelButton
            // 
            this.LineCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.LineCancelButton.Location = new System.Drawing.Point(94, 3);
            this.LineCancelButton.Name = "LineCancelButton";
            this.LineCancelButton.Size = new System.Drawing.Size(75, 23);
            this.LineCancelButton.TabIndex = 1;
            this.LineCancelButton.Text = "Cancel";
            this.LineCancelButton.UseVisualStyleBackColor = true;
            this.LineCancelButton.Click += new System.EventHandler(this.LineCancelButton_Click);
            // 
            // LineStyleDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 272);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LineStyleDialog";
            this.Text = "Line Style";
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
        public System.Windows.Forms.RadioButton solidRadioButton;
        public System.Windows.Forms.RadioButton dashRadioButton;
        public System.Windows.Forms.RadioButton dashDotRadioButton;
        public System.Windows.Forms.RadioButton dotRadioButton;
        public System.Windows.Forms.RadioButton dashDotDotRadioButton;
        public System.Windows.Forms.Button ApplyButton;
        public System.Windows.Forms.Button LineCancelButton;


    }
}