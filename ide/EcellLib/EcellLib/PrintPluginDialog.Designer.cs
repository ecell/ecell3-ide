namespace EcellLib
{
    partial class PrintPluginDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPluginDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.PPSelectButton = new System.Windows.Forms.Button();
            this.PPCancelButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.PPSelectButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.PPCancelButton, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // PPSelectButton
            // 
            resources.ApplyResources(this.PPSelectButton, "PPSelectButton");
            this.PPSelectButton.Name = "PPSelectButton";
            this.PPSelectButton.UseVisualStyleBackColor = true;
            this.PPSelectButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // PPCancelButton
            // 
            resources.ApplyResources(this.PPCancelButton, "PPCancelButton");
            this.PPCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.PPCancelButton.Name = "PPCancelButton";
            this.PPCancelButton.UseVisualStyleBackColor = true;
            this.PPCancelButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            // 
            // PrintPluginDialog
            // 
            this.AcceptButton = this.PPSelectButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.PPCancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PrintPluginDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button PPSelectButton;
        private System.Windows.Forms.Button PPCancelButton;
        /// <summary>
        /// ListBox to select the plugin to print.
        /// </summary>
        public System.Windows.Forms.ListBox listBox1;
    }
}