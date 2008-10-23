namespace Ecell.IDE.Plugins.ProjectExplorer
{
    partial class InputNameDialog
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
            System.Windows.Forms.Button CancelButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputNameDialog));
            System.Windows.Forms.Button CreateButton;
            this.NameTextBox = new System.Windows.Forms.TextBox();
            CancelButton = new System.Windows.Forms.Button();
            CreateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            resources.ApplyResources(CancelButton, "CancelButton");
            CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelButton.Name = "CancelButton";
            CancelButton.UseVisualStyleBackColor = true;
            // 
            // CreateButton
            // 
            resources.ApplyResources(CreateButton, "CreateButton");
            CreateButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            CreateButton.Name = "CreateButton";
            CreateButton.UseVisualStyleBackColor = true;
            // 
            // NameTextBox
            // 
            resources.ApplyResources(this.NameTextBox, "NameTextBox");
            this.NameTextBox.Name = "NameTextBox";
            // 
            // InputNameDialog
            // 
            this.AcceptButton = CreateButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = CancelButton;
            this.Controls.Add(CreateButton);
            this.Controls.Add(CancelButton);
            this.Controls.Add(this.NameTextBox);
            this.Name = "InputNameDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameTextBox;
    }
}