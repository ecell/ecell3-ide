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
            propertyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // propertyTextBox
            // 
            propertyTextBox.AccessibleDescription = null;
            propertyTextBox.AccessibleName = null;
            resources.ApplyResources(propertyTextBox, "propertyTextBox");
            propertyTextBox.BackgroundImage = null;
            propertyTextBox.Font = null;
            propertyTextBox.Name = "propertyTextBox";
            propertyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PropertyKeyPress);
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = null;
            this.okButton.AccessibleName = null;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackgroundImage = null;
            this.okButton.Font = null;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.AddPropertyApplyButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = null;
            this.cancelButton.AccessibleName = null;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackgroundImage = null;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = null;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // propertyNameLabel
            // 
            this.propertyNameLabel.AccessibleDescription = null;
            this.propertyNameLabel.AccessibleName = null;
            resources.ApplyResources(this.propertyNameLabel, "propertyNameLabel");
            this.propertyNameLabel.Font = null;
            this.propertyNameLabel.Name = "propertyNameLabel";
            // 
            // AddPropertyDialog
            // 
            this.AcceptButton = this.okButton;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.propertyNameLabel);
            this.Controls.Add(propertyTextBox);
            this.Font = null;
            this.Name = "AddPropertyDialog";
            this.Shown += new System.EventHandler(this.AddPropertyDialogShown);
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

        public System.Windows.Forms.TextBox propertyTextBox;
    }
}