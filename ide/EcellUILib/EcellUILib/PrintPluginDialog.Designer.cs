namespace Ecell.IDE
{
    partial class PrintPluginDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPluginDialog));
            this.printButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.printTargetSelectLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // printButton
            // 
            resources.ApplyResources(this.printButton, "printButton");
            this.printButton.Name = "printButton";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // printTargetSelectLabel
            // 
            resources.ApplyResources(this.printTargetSelectLabel, "printTargetSelectLabel");
            this.printTargetSelectLabel.Name = "printTargetSelectLabel";
            // 
            // PrintPluginDialog
            // 
            this.AcceptButton = this.printButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.printTargetSelectLabel);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.listBox1);
            this.Name = "PrintPluginDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button cancelButton;
        /// <summary>
        /// ListBox to select the plugin to print.
        /// </summary>
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label printTargetSelectLabel;
    }
}