namespace Ecell.IDE.Plugins.AboutWindow
{
    /// <summary>
    /// About Form
    /// </summary>
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.PictureBox pictureBox1;
            this.copyLabel = new System.Windows.Forms.Label();
            this.ecellLink = new System.Windows.Forms.LinkLabel();
            this.manualLink = new System.Windows.Forms.LinkLabel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            this.gnuLink = new System.Windows.Forms.LinkLabel();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
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
            // manualLink
            // 
            resources.ApplyResources(this.manualLink, "manualLink");
            this.manualLink.Name = "manualLink";
            this.manualLink.TabStop = true;
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            // 
            // gnuLink
            // 
            resources.ApplyResources(this.gnuLink, "gnuLink");
            this.gnuLink.Name = "gnuLink";
            this.gnuLink.TabStop = true;
            // 
            // AboutForm
            // 
            this.AcceptButton = this.CloseButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.copyLabel);
            this.Controls.Add(label3);
            this.Controls.Add(this.gnuLink);
            this.Controls.Add(this.manualLink);
            this.Controls.Add(this.ecellLink);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(pictureBox1);
            this.Controls.Add(label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel ecellLink;
        private System.Windows.Forms.LinkLabel manualLink;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.LinkLabel gnuLink;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label copyLabel;
    }
}