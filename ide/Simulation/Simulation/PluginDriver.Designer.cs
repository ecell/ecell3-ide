namespace Ecell.IDE.Plugins.Simulation
{
    /// <summary>
    /// Class of test driver.
    /// </summary>
    partial class PluginDriver 
    {
        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.mainstrip = new System.Windows.Forms.MenuStrip();
            this.toolstrip = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // mainstrip
            // 
            this.mainstrip.Location = new System.Drawing.Point(0, 0);
            this.mainstrip.Name = "mainstrip";
            this.mainstrip.Size = new System.Drawing.Size(292, 24);
            this.mainstrip.TabIndex = 1;
            this.mainstrip.Text = "mainstrip";
            // 
            // toolstrip
            // 
            this.toolstrip.Location = new System.Drawing.Point(0, 24);
            this.toolstrip.Name = "toolstrip";
            this.toolstrip.Size = new System.Drawing.Size(292, 25);
            this.toolstrip.TabIndex = 2;
            this.toolstrip.Text = "toolStrip1";
            // 
            // PluginDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.toolstrip);
            this.Controls.Add(this.mainstrip);
            this.MainMenuStrip = this.mainstrip;
            this.Name = "PluginDriver";
            this.Text = "PluginDriver";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainstrip;
        private System.Windows.Forms.ToolStrip toolstrip;
    }
}