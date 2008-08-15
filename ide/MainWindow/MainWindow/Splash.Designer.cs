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
            this.CopyrightNotice = new System.Windows.Forms.Label();
            this.VersionNumber = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressInfo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CopyrightNotice
            // 
            this.CopyrightNotice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyrightNotice.BackColor = System.Drawing.Color.Transparent;
            this.CopyrightNotice.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold);
            this.CopyrightNotice.Location = new System.Drawing.Point(6, 240);
            this.CopyrightNotice.Margin = new System.Windows.Forms.Padding(0);
            this.CopyrightNotice.Name = "CopyrightNotice";
            this.CopyrightNotice.Size = new System.Drawing.Size(440, 58);
            this.CopyrightNotice.TabIndex = 2;
            this.CopyrightNotice.Text = "bbb";
            this.CopyrightNotice.UseMnemonic = false;
            // 
            // VersionNumber
            // 
            this.VersionNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VersionNumber.BackColor = System.Drawing.Color.Transparent;
            this.VersionNumber.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.VersionNumber.Location = new System.Drawing.Point(175, 9);
            this.VersionNumber.Margin = new System.Windows.Forms.Padding(9);
            this.VersionNumber.Name = "VersionNumber";
            this.VersionNumber.Size = new System.Drawing.Size(256, 14);
            this.VersionNumber.TabIndex = 3;
            this.VersionNumber.Text = "0.0.0.0";
            this.VersionNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BackgroundImage = global::Ecell.IDE.MainWindow.Properties.Resources.splash;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.VersionNumber);
            this.panel1.Controls.Add(this.CopyrightNotice);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(446, 300);
            this.panel1.TabIndex = 5;
            // 
            // progressInfo
            // 
            this.progressInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.progressInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressInfo.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.progressInfo.Location = new System.Drawing.Point(0, 298);
            this.progressInfo.Name = "progressInfo";
            this.progressInfo.Size = new System.Drawing.Size(446, 20);
            this.progressInfo.TabIndex = 6;
            this.progressInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Splash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(446, 318);
            this.ControlBox = false;
            this.Controls.Add(this.progressInfo);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(0, 324);
            this.Name = "Splash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Splash_FormClosing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label VersionNumber;
        private System.Windows.Forms.Label CopyrightNotice;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label progressInfo;
    }
}