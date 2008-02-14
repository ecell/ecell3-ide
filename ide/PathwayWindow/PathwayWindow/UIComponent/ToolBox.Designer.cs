﻿namespace EcellLib.PathwayWindow.UIComponent
{
    partial class ToolBox
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
            this.pCanvas1 = new EcellLib.PathwayWindow.UIComponent.PToolBoxCanvas();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pCanvas2 = new EcellLib.PathwayWindow.UIComponent.PToolBoxCanvas();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pCanvas3 = new EcellLib.PathwayWindow.UIComponent.PToolBoxCanvas();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pCanvas1);
            this.groupBox1.Location = new System.Drawing.Point(5, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(94, 86);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // pCanvas1
            // 
            this.pCanvas1.AllowDrop = true;
            this.pCanvas1.BackColor = System.Drawing.Color.Silver;
            this.pCanvas1.GridFitText = false;
            this.pCanvas1.Location = new System.Drawing.Point(10, 13);
            this.pCanvas1.Name = "pCanvas1";
            this.pCanvas1.PPathwayObject = null;
            this.pCanvas1.RegionManagement = true;
            this.pCanvas1.Size = new System.Drawing.Size(77, 65);
            this.pCanvas1.TabIndex = 0;
            this.pCanvas1.Text = "pCanvas1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pCanvas2);
            this.groupBox2.Location = new System.Drawing.Point(4, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(95, 86);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // pCanvas2
            // 
            this.pCanvas2.AllowDrop = true;
            this.pCanvas2.BackColor = System.Drawing.Color.Silver;
            this.pCanvas2.GridFitText = false;
            this.pCanvas2.Location = new System.Drawing.Point(10, 13);
            this.pCanvas2.Name = "pCanvas2";
            this.pCanvas2.PPathwayObject = null;
            this.pCanvas2.RegionManagement = true;
            this.pCanvas2.Size = new System.Drawing.Size(78, 65);
            this.pCanvas2.TabIndex = 0;
            this.pCanvas2.Text = "pCanvas2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pCanvas3);
            this.groupBox3.Location = new System.Drawing.Point(4, 185);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(95, 86);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // pCanvas3
            // 
            this.pCanvas3.AllowDrop = true;
            this.pCanvas3.BackColor = System.Drawing.Color.Silver;
            this.pCanvas3.GridFitText = false;
            this.pCanvas3.Location = new System.Drawing.Point(10, 13);
            this.pCanvas3.Name = "pCanvas3";
            this.pCanvas3.PPathwayObject = null;
            this.pCanvas3.RegionManagement = true;
            this.pCanvas3.Size = new System.Drawing.Size(78, 65);
            this.pCanvas3.TabIndex = 0;
            this.pCanvas3.Text = "pCanvas3";
            // 
            // ToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(104, 273);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.IsSavable = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolBox";
            this.TabText = "ToolBox";
            this.Text = "ToolBox";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private EcellLib.PathwayWindow.UIComponent.PToolBoxCanvas pCanvas1;
        private System.Windows.Forms.GroupBox groupBox2;
        private EcellLib.PathwayWindow.UIComponent.PToolBoxCanvas pCanvas2;
        private System.Windows.Forms.GroupBox groupBox3;
        private EcellLib.PathwayWindow.UIComponent.PToolBoxCanvas pCanvas3;
    }
}