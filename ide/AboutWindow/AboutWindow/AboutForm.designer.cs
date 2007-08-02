namespace EcellLib.AboutWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.copyLabel = new System.Windows.Forms.Label();
            this.ecellLink = new System.Windows.Forms.LinkLabel();
            this.manualLink = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gnuLink = new System.Windows.Forms.LinkLabel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // copyLabel
            // 
            resources.ApplyResources(this.copyLabel, "copyLabel");
            this.tableLayoutPanel2.SetColumnSpan(this.copyLabel, 2);
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
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.ecellLink, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.gnuLink, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.copyLabel, 2, 7);
            this.tableLayoutPanel2.Controls.Add(this.CloseButton, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.manualLink, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 2, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.tableLayoutPanel2.SetRowSpan(this.pictureBox1, 6);
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // gnuLink
            // 
            resources.ApplyResources(this.gnuLink, "gnuLink");
            this.gnuLink.Name = "gnuLink";
            this.gnuLink.TabStop = true;
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.versionLabel, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            // 
            // AboutForm
            // 
            this.AcceptButton = this.CloseButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.About_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel ecellLink;
        private System.Windows.Forms.LinkLabel manualLink;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel gnuLink;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        /// <summary>
        /// Label shown this program version.
        /// </summary>
        public System.Windows.Forms.Label versionLabel;
        /// <summary>
        /// Label shown this copyrights.
        /// </summary>
        public System.Windows.Forms.Label copyLabel;
    }
}