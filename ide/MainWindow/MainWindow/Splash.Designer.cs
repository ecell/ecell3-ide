namespace EcellLib.MainWindow
{
    partial class Splash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.CopyrightNotice = new System.Windows.Forms.Label();
            this.VersionNumber = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.CausesValidation = false;
            label1.Name = "label1";
            label1.UseMnemonic = false;
            // 
            // CopyrightNotice
            // 
            resources.ApplyResources(this.CopyrightNotice, "CopyrightNotice");
            this.CopyrightNotice.BackColor = System.Drawing.Color.Transparent;
            this.CopyrightNotice.CausesValidation = false;
            this.CopyrightNotice.Name = "CopyrightNotice";
            this.CopyrightNotice.UseMnemonic = false;
            // 
            // VersionNumber
            // 
            resources.ApplyResources(this.VersionNumber, "VersionNumber");
            this.VersionNumber.BackColor = System.Drawing.Color.Transparent;
            this.VersionNumber.Name = "VersionNumber";
            // 
            // Splash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::EcellLib.MainWindow.Properties.Resources.splash;
            this.ControlBox = false;
            this.Controls.Add(this.VersionNumber);
            this.Controls.Add(this.CopyrightNotice);
            this.Controls.Add(label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Splash";
            this.Size = this.BackgroundImage.Size;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label VersionNumber;
        private System.Windows.Forms.Label CopyrightNotice;
    }
}