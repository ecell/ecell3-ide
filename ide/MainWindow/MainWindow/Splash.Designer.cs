namespace Ecell.IDE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.label1 = new System.Windows.Forms.Label();
            this.CopyrightNotice = new System.Windows.Forms.Label();
            this.VersionNumber = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressInfo = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.MinimumSize = new System.Drawing.Size(80, 14);
            this.label1.Name = "label1";
            this.label1.UseMnemonic = false;
            // 
            // CopyrightNotice
            // 
            resources.ApplyResources(this.CopyrightNotice, "CopyrightNotice");
            this.CopyrightNotice.BackColor = System.Drawing.Color.Transparent;
            this.CopyrightNotice.MinimumSize = new System.Drawing.Size(452, 42);
            this.CopyrightNotice.Name = "CopyrightNotice";
            this.CopyrightNotice.UseMnemonic = false;
            // 
            // VersionNumber
            // 
            resources.ApplyResources(this.VersionNumber, "VersionNumber");
            this.VersionNumber.BackColor = System.Drawing.Color.Transparent;
            this.VersionNumber.MinimumSize = new System.Drawing.Size(100, 0);
            this.VersionNumber.Name = "VersionNumber";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = global::Ecell.IDE.MainWindow.Properties.Resources.splash;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.VersionNumber);
            this.panel1.Controls.Add(this.CopyrightNotice);
            this.panel1.Name = "panel1";
            // 
            // progressInfo
            // 
            this.progressInfo.BackColor = System.Drawing.SystemColors.Window;
            this.progressInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.progressInfo.Cursor = System.Windows.Forms.Cursors.Arrow;
            resources.ApplyResources(this.progressInfo, "progressInfo");
            this.progressInfo.MinimumSize = new System.Drawing.Size(0, 12);
            this.progressInfo.Name = "progressInfo";
            this.progressInfo.ReadOnly = true;
            // 
            // Splash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ControlBox = false;
            this.Controls.Add(this.progressInfo);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Splash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label VersionNumber;
        private System.Windows.Forms.Label CopyrightNotice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox progressInfo;
    }
}