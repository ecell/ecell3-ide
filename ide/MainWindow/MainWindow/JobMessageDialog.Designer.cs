namespace Ecell.IDE.MainWindow
{
    partial class JobMessageDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobMessageDialog));
            this.MOKButton = new System.Windows.Forms.Button();
            this.MRichTextBox = new System.Windows.Forms.RichTextBox();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // MOKButton
            // 
            resources.ApplyResources(this.MOKButton, "MOKButton");
            this.MOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.MOKButton.Name = "MOKButton";
            this.MOKButton.UseVisualStyleBackColor = true;
            // 
            // MRichTextBox
            // 
            resources.ApplyResources(this.MRichTextBox, "MRichTextBox");
            this.MRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MRichTextBox.Name = "MRichTextBox";
            this.MRichTextBox.ReadOnly = true;
            // 
            // JobMessageDialog
            // 
            this.AcceptButton = this.MOKButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(label1);
            this.Controls.Add(this.MRichTextBox);
            this.Controls.Add(this.MOKButton);
            this.Name = "JobMessageDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MOKButton;
        private System.Windows.Forms.RichTextBox MRichTextBox;
    }
}