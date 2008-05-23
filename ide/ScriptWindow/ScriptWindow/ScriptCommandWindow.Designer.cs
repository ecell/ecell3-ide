namespace EcellLib.ScriptWindow
{
    partial class ScriptCommandWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptCommandWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SWMessageText = new System.Windows.Forms.TextBox();
            this.SWCommandText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.SWMessageText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SWCommandText, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // SWMessageText
            // 
            resources.ApplyResources(this.SWMessageText, "SWMessageText");
            this.SWMessageText.Name = "SWMessageText";
            this.SWMessageText.ReadOnly = true;
            // 
            // SWCommandText
            // 
            resources.ApplyResources(this.SWCommandText, "SWCommandText");
            this.SWCommandText.Name = "SWCommandText";
            this.SWCommandText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CommandTextKeyPress);
            // 
            // ScriptCommandWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScriptCommandWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox SWMessageText;
        private System.Windows.Forms.TextBox SWCommandText;
    }
}