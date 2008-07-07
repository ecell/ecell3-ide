namespace Ecell.ScriptWindow
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
            this.SWCommandText = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SWMessageText
            // 
            this.SWMessageText.BackColor = System.Drawing.SystemColors.Window;
            this.SWMessageText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SWMessageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SWMessageText.Font = new System.Drawing.Font("Courier New", 9F);
            this.SWMessageText.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.SWMessageText.Location = new System.Drawing.Point(0, 0);
            this.SWMessageText.Margin = new System.Windows.Forms.Padding(0);
            this.SWMessageText.Name = "SWMessageText";
            this.SWMessageText.ReadOnly = true;
            this.SWMessageText.ShortcutsEnabled = false;
            this.SWMessageText.Size = new System.Drawing.Size(513, 415);
            this.SWMessageText.TabIndex = 0;
            this.SWMessageText.Text = "";
            this.SWMessageText.WordWrap = false;
            // 
            // SWCommandText
            // 
            this.SWCommandText.AcceptsTab = true;
            this.SWCommandText.BackColor = System.Drawing.SystemColors.Window;
            this.SWCommandText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SWCommandText.DetectUrls = false;
            this.SWCommandText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SWCommandText.Font = new System.Drawing.Font("Courier New", 9F);
            this.SWCommandText.Location = new System.Drawing.Point(0, 0);
            this.SWCommandText.Margin = new System.Windows.Forms.Padding(0);
            this.SWCommandText.Name = "SWCommandText";
            this.SWCommandText.Size = new System.Drawing.Size(513, 48);
            this.SWCommandText.TabIndex = 1;
            this.SWCommandText.Text = "";
            this.SWCommandText.WordWrap = false;
            this.SWCommandText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandTextKeyDown);
            this.SWCommandText.SelectionChanged += new System.EventHandler(this.CommandTextSelectionChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SWMessageText);
            this.splitContainer1.Panel1MinSize = 24;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.SWCommandText);
            this.splitContainer1.Panel2MinSize = 12;
            this.splitContainer1.Size = new System.Drawing.Size(513, 465);
            this.splitContainer1.SplitterDistance = 415;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 1;
            // 
            // ScriptCommandWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 465);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScriptCommandWindow";
            this.TabText = "Script";
            this.Text = "Script";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox SWMessageText;
        private System.Windows.Forms.RichTextBox SWCommandText;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}