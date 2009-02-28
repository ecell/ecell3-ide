namespace Ecell.IDE
{
    partial class AddPropertyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPropertyDialog));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.propertyNameLabel = new System.Windows.Forms.Label();
            this.propertyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // propertyNameLabel
            // 
            resources.ApplyResources(this.propertyNameLabel, "propertyNameLabel");
            this.propertyNameLabel.Name = "propertyNameLabel";
            // 
            // propertyTextBox
            // 
            resources.ApplyResources(this.propertyTextBox, "propertyTextBox");
            this.propertyTextBox.Name = "propertyTextBox";
            this.propertyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PropertyKeyPress);
            // 
            // AddPropertyDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.propertyNameLabel);
            this.Controls.Add(this.propertyTextBox);
            this.Name = "AddPropertyDialog";
            this.Shown += new System.EventHandler(this.AddPropertyDialogShown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddPropertyDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label propertyNameLabel;
        /// <summary>
        /// Button to add button this window,
        /// </summary>
        public System.Windows.Forms.Button okButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox propertyTextBox;
    }
}