namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Form Class to setup the condition of tracer.
    /// </summary>
    partial class TracerConfigurationDialog
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TracerConfigurationDialog));
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label5;
            this.numberTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.TSApplyButton = new System.Windows.Forms.Button();
            this.TSCloseButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.stepCountTextBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // numberTextBox
            // 
            resources.ApplyResources(this.numberTextBox, "numberTextBox");
            this.numberTextBox.Name = "numberTextBox";
            this.numberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterKeyPress);
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
            // TSApplyButton
            // 
            resources.ApplyResources(this.TSApplyButton, "TSApplyButton");
            this.TSApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // stepCountTextBox
            // 
            resources.ApplyResources(this.stepCountTextBox, "stepCountTextBox");
            this.stepCountTextBox.Name = "stepCountTextBox";
            // 
            // TracerWindowSetup
            // 
            this.AcceptButton = this.TSApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.TSCloseButton;
            this.Controls.Add(this.TSApplyButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TSCloseButton);
            this.Controls.Add(label5);
            this.Controls.Add(this.stepCountTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(label3);
            this.Controls.Add(this.intervalTextBox);
            this.Controls.Add(label1);
            this.Controls.Add(this.numberTextBox);
            this.Name = "TracerWindowSetup";
            this.Shown += new System.EventHandler(this.TracerWinSetupShown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TracerWindowSetup_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        /// <summary>
        /// Button to apply this condition.
        /// </summary>
        public System.Windows.Forms.Button TSApplyButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button TSCloseButton;
        private System.Windows.Forms.TextBox numberTextBox;
        private System.Windows.Forms.TextBox intervalTextBox;
        private System.Windows.Forms.TextBox stepCountTextBox;
    }
}