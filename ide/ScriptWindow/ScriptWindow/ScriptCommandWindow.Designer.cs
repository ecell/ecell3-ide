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
            this.SWMessageText = new System.Windows.Forms.RichTextBox();
            this.SWCommandText = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SWMessageText
            // 
            resources.ApplyResources(this.SWMessageText, "SWMessageText");
            this.SWMessageText.BackColor = System.Drawing.SystemColors.Window;
            this.SWMessageText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SWMessageText.Name = "SWMessageText";
            this.SWMessageText.ReadOnly = true;
            // 
            // SWCommandText
            // 
            resources.ApplyResources(this.SWCommandText, "SWCommandText");
            this.SWCommandText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.SWCommandText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.SWCommandText.Name = "SWCommandText";
            this.SWCommandText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandTextKeyDown);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SWMessageText);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.SWCommandText);
            // 
            // ScriptCommandWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScriptCommandWindow";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox SWMessageText;
        private System.Windows.Forms.TextBox SWCommandText;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}