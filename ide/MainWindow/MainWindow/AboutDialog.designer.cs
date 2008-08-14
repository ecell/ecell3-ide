namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// About Form
    /// </summary>
    partial class AboutDialog
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
            System.Windows.Forms.Label label3;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            System.Windows.Forms.Label assemblyLabel;
            this.productNameLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.copyLabel = new System.Windows.Forms.Label();
            this.ecellLink = new System.Windows.Forms.LinkLabel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.assemblyVersionLabel = new System.Windows.Forms.Label();
            this.creditButton = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            assemblyLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // productNameLabel
            // 
            resources.ApplyResources(this.productNameLabel, "productNameLabel");
            this.productNameLabel.Name = "productNameLabel";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // assemblyLabel
            // 
            resources.ApplyResources(assemblyLabel, "assemblyLabel");
            assemblyLabel.Name = "assemblyLabel";
            // 
            // copyLabel
            // 
            resources.ApplyResources(this.copyLabel, "copyLabel");
            this.copyLabel.Name = "copyLabel";
            // 
            // ecellLink
            // 
            resources.ApplyResources(this.ecellLink, "ecellLink");
            this.ecellLink.Name = "ecellLink";
            this.ecellLink.TabStop = true;
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // assemblyVersionLabel
            // 
            resources.ApplyResources(this.assemblyVersionLabel, "assemblyVersionLabel");
            this.assemblyVersionLabel.Name = "assemblyVersionLabel";
            // 
            // creditButton
            // 
            resources.ApplyResources(this.creditButton, "creditButton");
            this.creditButton.Name = "creditButton";
            this.creditButton.UseVisualStyleBackColor = true;
            this.creditButton.Click += new System.EventHandler(this.creditButton_Click);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.CloseButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.Controls.Add(this.creditButton);
            this.Controls.Add(assemblyLabel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.copyLabel);
            this.Controls.Add(label3);
            this.Controls.Add(this.ecellLink);
            this.Controls.Add(this.assemblyVersionLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.productNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel ecellLink;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label assemblyVersionLabel;
        private System.Windows.Forms.Label copyLabel;
        private System.Windows.Forms.Button creditButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label productNameLabel;
    }
}