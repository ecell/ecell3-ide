namespace EcellLib.TracerWindow
{
    /// <summary>
    /// Form Class to setup the condition of tracer.
    /// </summary>
    partial class TracerWindowSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TracerWindowSetup));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numberTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.TSApplyButton = new System.Windows.Forms.Button();
            this.TSCloseButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.stepCountTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.numberTextBox, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // numberTextBox
            // 
            resources.ApplyResources(this.numberTextBox, "numberTextBox");
            this.numberTextBox.Name = "numberTextBox";
            this.numberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterKeyPress);
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.intervalTextBox, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // intervalTextBox
            // 
            resources.ApplyResources(this.intervalTextBox, "intervalTextBox");
            this.intervalTextBox.Name = "intervalTextBox";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.TSApplyButton, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.TSCloseButton, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // TSApplyButton
            // 
            resources.ApplyResources(this.TSApplyButton, "TSApplyButton");
            this.TSApplyButton.Name = "TSApplyButton";
            this.TSApplyButton.UseVisualStyleBackColor = true;
            // 
            // TSCloseButton
            // 
            resources.ApplyResources(this.TSCloseButton, "TSCloseButton");
            this.TSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.TSCloseButton.Name = "TSCloseButton";
            this.TSCloseButton.UseVisualStyleBackColor = true;
            this.TSCloseButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.stepCountTextBox, 1, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // stepCountTextBox
            // 
            resources.ApplyResources(this.stepCountTextBox, "stepCountTextBox");
            this.stepCountTextBox.Name = "stepCountTextBox";
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // TracerWindowSetup
            // 
            this.AcceptButton = this.TSApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.TSCloseButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TracerWindowSetup";
            this.Shown += new System.EventHandler(this.TracerWinSetupShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        /// <summary>
        /// TextBox to set the number of plots.
        /// </summary>
        public System.Windows.Forms.TextBox numberTextBox;
        /// <summary>
        /// TextBox to set the interval time between plot.
        /// </summary>
        public System.Windows.Forms.TextBox intervalTextBox;
        /// <summary>
        /// TextBoxt to set the step count.
        /// </summary>
        public System.Windows.Forms.TextBox stepCountTextBox;
        /// <summary>
        /// Button to apply this condition.
        /// </summary>
        public System.Windows.Forms.Button TSApplyButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button TSCloseButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}