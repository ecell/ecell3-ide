namespace EcellLib
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
            this.APAddButton = new System.Windows.Forms.Button();
            this.APCancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PropertyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // APAddButton
            // 
            resources.ApplyResources(this.APAddButton, "APAddButton");
            this.APAddButton.Name = "APAddButton";
            this.APAddButton.UseVisualStyleBackColor = true;
            this.APAddButton.Click += new System.EventHandler(this.AddPropertyApplyButton_Click);
            // 
            // APCancelButton
            // 
            resources.ApplyResources(this.APCancelButton, "APCancelButton");
            this.APCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.APCancelButton.Name = "APCancelButton";
            this.APCancelButton.UseVisualStyleBackColor = true;
            this.APCancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // PropertyTextBox
            // 
            resources.ApplyResources(this.PropertyTextBox, "PropertyTextBox");
            this.PropertyTextBox.Name = "PropertyTextBox";
            this.PropertyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PropertyKeyPress);
            // 
            // AddPropertyDialog
            // 
            this.AcceptButton = this.APAddButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.APCancelButton;
            this.Controls.Add(this.APCancelButton);
            this.Controls.Add(this.APAddButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PropertyTextBox);
            this.Name = "AddPropertyDialog";
            this.Shown += new System.EventHandler(this.AddPropertyDialogShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        /// <summary>
        /// Button to add button this window,
        /// </summary>
        public System.Windows.Forms.Button APAddButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button APCancelButton;
        /// <summary>
        /// TextBox to input the name of property.
        /// </summary>
        public System.Windows.Forms.TextBox PropertyTextBox;
    }
}