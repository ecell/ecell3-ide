namespace Ecell.IDE.Plugins.Simulation
{
    /// <summary>
    /// Dialog class to create parameter set.
    /// </summary>
    partial class InputParameterNameDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputParameterNameDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.paramTextBox = new System.Windows.Forms.TextBox();
            this.CPCreateButton = new System.Windows.Forms.Button();
            this.CPCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // paramTextBox
            // 
            resources.ApplyResources(this.paramTextBox, "paramTextBox");
            this.paramTextBox.Name = "paramTextBox";
            // 
            // CPCreateButton
            // 
            resources.ApplyResources(this.CPCreateButton, "CPCreateButton");
            this.CPCreateButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CPCreateButton.Name = "CPCreateButton";
            this.CPCreateButton.UseVisualStyleBackColor = true;
            // 
            // CPCancelButton
            // 
            resources.ApplyResources(this.CPCancelButton, "CPCancelButton");
            this.CPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CPCancelButton.Name = "CPCancelButton";
            this.CPCancelButton.UseVisualStyleBackColor = true;
            // 
            // InputParameterNameDialog
            // 
            this.AcceptButton = this.CPCreateButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CPCancelButton;
            this.Controls.Add(this.CPCreateButton);
            this.Controls.Add(this.paramTextBox);
            this.Controls.Add(this.CPCancelButton);
            this.Controls.Add(this.label1);
            this.Name = "InputParameterNameDialog";
            this.Shown += new System.EventHandler(this.ShowCreateParameterWin);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        /// <summary>
        /// TextBox of parameter name on dialog to create parameter set.
        /// </summary>
        public System.Windows.Forms.TextBox paramTextBox;
        /// <summary>
        /// Create button on dialog to create parameter set.
        /// </summary>
        public System.Windows.Forms.Button CPCreateButton;
        /// <summary>
        /// Cancel button on dialog to create prameter set.
        /// </summary>
        public System.Windows.Forms.Button CPCancelButton;
    }
}