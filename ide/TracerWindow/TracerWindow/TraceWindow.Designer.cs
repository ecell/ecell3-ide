namespace Ecell.IDE.Plugins.TracerWindow
{
    partial class TraceWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TraceWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.m_colorDialog = new System.Windows.Forms.ColorDialog();
            this.m_openDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // TraceWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TraceWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        /// <summary>
        /// FolderBrowseDialog to set the output directory.
        /// </summary>
        public System.Windows.Forms.FolderBrowserDialog m_folderDialog;
        /// <summary>
        /// ColorDialog to set line color.
        /// </summary>
        public System.Windows.Forms.ColorDialog m_colorDialog;
        private System.Windows.Forms.OpenFileDialog m_openDialog;
    }
}