namespace Ecell.IDE.Plugins.Analysis
{
    partial class ParameterEstimationAdvancedSettingDialog
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
            System.Windows.Forms.Label label4;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterEstimationAdvancedSettingDialog));
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            this.PEAApplyButton = new System.Windows.Forms.Button();
            this.PEACloseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PEUpsilonTextBox = new System.Windows.Forms.TextBox();
            this.PEMTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PEMaxRateTextBox = new System.Windows.Forms.TextBox();
            this.PEKTextBox = new System.Windows.Forms.TextBox();
            this.PEM0TextBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // PEAApplyButton
            // 
            resources.ApplyResources(this.PEAApplyButton, "PEAApplyButton");
            this.PEAApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.PEAApplyButton.Name = "PEAApplyButton";
            this.PEAApplyButton.UseVisualStyleBackColor = true;
            // 
            // PEACloseButton
            // 
            resources.ApplyResources(this.PEACloseButton, "PEACloseButton");
            this.PEACloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.PEACloseButton.Name = "PEACloseButton";
            this.PEACloseButton.UseVisualStyleBackColor = true;
            this.PEACloseButton.Click += new System.EventHandler(this.PEACloseButtonClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PEUpsilonTextBox);
            this.groupBox1.Controls.Add(this.PEMTextBox);
            this.groupBox1.Controls.Add(label4);
            this.groupBox1.Controls.Add(label5);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // PEUpsilonTextBox
            // 
            resources.ApplyResources(this.PEUpsilonTextBox, "PEUpsilonTextBox");
            this.PEUpsilonTextBox.Name = "PEUpsilonTextBox";
            // 
            // PEMTextBox
            // 
            resources.ApplyResources(this.PEMTextBox, "PEMTextBox");
            this.PEMTextBox.Name = "PEMTextBox";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PEMaxRateTextBox);
            this.groupBox2.Controls.Add(label1);
            this.groupBox2.Controls.Add(label3);
            this.groupBox2.Controls.Add(this.PEKTextBox);
            this.groupBox2.Controls.Add(label2);
            this.groupBox2.Controls.Add(this.PEM0TextBox);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // PEMaxRateTextBox
            // 
            resources.ApplyResources(this.PEMaxRateTextBox, "PEMaxRateTextBox");
            this.PEMaxRateTextBox.Name = "PEMaxRateTextBox";
            // 
            // PEKTextBox
            // 
            resources.ApplyResources(this.PEKTextBox, "PEKTextBox");
            this.PEKTextBox.Name = "PEKTextBox";
            // 
            // PEM0TextBox
            // 
            resources.ApplyResources(this.PEM0TextBox, "PEM0TextBox");
            this.PEM0TextBox.Name = "PEM0TextBox";
            // 
            // ParameterEstimationAdvancedSettingDialog
            // 
            this.AcceptButton = this.PEAApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.PEACloseButton;
            this.Controls.Add(this.PEACloseButton);
            this.Controls.Add(this.PEAApplyButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ParameterEstimationAdvancedSettingDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdvancedFormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button PEACloseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox PEM0TextBox;
        private System.Windows.Forms.TextBox PEKTextBox;
        private System.Windows.Forms.TextBox PEMaxRateTextBox;
        private System.Windows.Forms.TextBox PEUpsilonTextBox;
        private System.Windows.Forms.TextBox PEMTextBox;
        /// <summary>
        /// Apply button.
        /// </summary>
        public System.Windows.Forms.Button PEAApplyButton;
    }
}