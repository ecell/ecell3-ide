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
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label2;
            this.numberTextBox = new System.Windows.Forms.TextBox();
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.TSApplyButton = new System.Windows.Forms.Button();
            this.TSCloseButton = new System.Windows.Forms.Button();
            this.stepCountTextBox = new System.Windows.Forms.TextBox();
            this.valueFormatComboBox = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
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
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // numberTextBox
            // 
            resources.ApplyResources(this.numberTextBox, "numberTextBox");
            this.numberTextBox.Name = "numberTextBox";
            this.numberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterKeyPress);
            this.numberTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.PlotNumber_Validating);
            // 
            // intervalTextBox
            // 
            resources.ApplyResources(this.intervalTextBox, "intervalTextBox");
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.RedrawInterval_Validating);
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
            // 
            // stepCountTextBox
            // 
            resources.ApplyResources(this.stepCountTextBox, "stepCountTextBox");
            this.stepCountTextBox.Name = "stepCountTextBox";
            this.stepCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.StepCount_Validating);
            // 
            // valueFormatComboBox
            // 
            resources.ApplyResources(this.valueFormatComboBox, "valueFormatComboBox");
            this.valueFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueFormatComboBox.FormattingEnabled = true;
            this.valueFormatComboBox.Items.AddRange(new object[] {
            resources.GetString("valueFormatComboBox.Items"),
            resources.GetString("valueFormatComboBox.Items1"),
            resources.GetString("valueFormatComboBox.Items2"),
            resources.GetString("valueFormatComboBox.Items3"),
            resources.GetString("valueFormatComboBox.Items4"),
            resources.GetString("valueFormatComboBox.Items5")});
            this.valueFormatComboBox.Name = "valueFormatComboBox";
            this.valueFormatComboBox.SelectedIndexChanged += new System.EventHandler(this.DataFormatChanged);
            // 
            // TracerConfigurationDialog
            // 
            this.AcceptButton = this.TSApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.TSCloseButton;
            this.Controls.Add(this.valueFormatComboBox);
            this.Controls.Add(label2);
            this.Controls.Add(this.TSApplyButton);
            this.Controls.Add(label6);
            this.Controls.Add(this.TSCloseButton);
            this.Controls.Add(label5);
            this.Controls.Add(this.stepCountTextBox);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(this.intervalTextBox);
            this.Controls.Add(label1);
            this.Controls.Add(this.numberTextBox);
            this.Name = "TracerConfigurationDialog";
            this.Shown += new System.EventHandler(this.TracerWinSetupShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.ComboBox valueFormatComboBox;
    }
}