namespace EcellLib.MessageWindow
{
    partial class MessageWindowControl
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabContorl = new System.Windows.Forms.TabControl();
            this.simTab = new System.Windows.Forms.TabPage();
            this.simText = new System.Windows.Forms.TextBox();
            this.debTab = new System.Windows.Forms.TabPage();
            this.debText = new System.Windows.Forms.TextBox();
            this.anaTab = new System.Windows.Forms.TabPage();
            this.anaText = new System.Windows.Forms.TextBox();
            this.tabContorl.SuspendLayout();
            this.simTab.SuspendLayout();
            this.debTab.SuspendLayout();
            this.anaTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabContorl
            // 
            this.tabContorl.Controls.Add(this.simTab);
            this.tabContorl.Controls.Add(this.debTab);
            this.tabContorl.Controls.Add(this.anaTab);
            this.tabContorl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContorl.Location = new System.Drawing.Point(0, 0);
            this.tabContorl.Name = "tabContorl";
            this.tabContorl.SelectedIndex = 0;
            this.tabContorl.Size = new System.Drawing.Size(370, 209);
            this.tabContorl.TabIndex = 0;
            // 
            // simTab
            // 
            this.simTab.Controls.Add(this.simText);
            this.simTab.Location = new System.Drawing.Point(4, 21);
            this.simTab.Name = "simTab";
            this.simTab.Padding = new System.Windows.Forms.Padding(3);
            this.simTab.Size = new System.Drawing.Size(362, 184);
            this.simTab.TabIndex = 0;
            this.simTab.Text = "Simulation";
            this.simTab.UseVisualStyleBackColor = true;
            // 
            // simText
            // 
            this.simText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simText.Location = new System.Drawing.Point(3, 3);
            this.simText.Multiline = true;
            this.simText.Name = "simText";
            this.simText.ReadOnly = true;
            this.simText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.simText.Size = new System.Drawing.Size(356, 178);
            this.simText.TabIndex = 0;
            // 
            // debTab
            // 
            this.debTab.Controls.Add(this.debText);
            this.debTab.Location = new System.Drawing.Point(4, 21);
            this.debTab.Name = "debTab";
            this.debTab.Padding = new System.Windows.Forms.Padding(3);
            this.debTab.Size = new System.Drawing.Size(362, 184);
            this.debTab.TabIndex = 1;
            this.debTab.Text = "Debug";
            this.debTab.UseVisualStyleBackColor = true;
            // 
            // debText
            // 
            this.debText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debText.Location = new System.Drawing.Point(3, 3);
            this.debText.Multiline = true;
            this.debText.Name = "debText";
            this.debText.ReadOnly = true;
            this.debText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.debText.Size = new System.Drawing.Size(356, 178);
            this.debText.TabIndex = 0;
            // 
            // anaTab
            // 
            this.anaTab.Controls.Add(this.anaText);
            this.anaTab.Location = new System.Drawing.Point(4, 21);
            this.anaTab.Name = "anaTab";
            this.anaTab.Padding = new System.Windows.Forms.Padding(3);
            this.anaTab.Size = new System.Drawing.Size(362, 184);
            this.anaTab.TabIndex = 2;
            this.anaTab.Text = "Analysis";
            this.anaTab.UseVisualStyleBackColor = true;
            // 
            // anaText
            // 
            this.anaText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.anaText.Location = new System.Drawing.Point(3, 3);
            this.anaText.Multiline = true;
            this.anaText.Name = "anaText";
            this.anaText.ReadOnly = true;
            this.anaText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.anaText.Size = new System.Drawing.Size(356, 178);
            this.anaText.TabIndex = 0;
            // 
            // MessageWindowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabContorl);
            this.Name = "MessageWindowControl";
            this.Size = new System.Drawing.Size(370, 209);
            this.tabContorl.ResumeLayout(false);
            this.simTab.ResumeLayout(false);
            this.simTab.PerformLayout();
            this.debTab.ResumeLayout(false);
            this.debTab.PerformLayout();
            this.anaTab.ResumeLayout(false);
            this.anaTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabContorl;
        public System.Windows.Forms.TabPage simTab;
        public System.Windows.Forms.TabPage debTab;
        public System.Windows.Forms.TabPage anaTab;
        public System.Windows.Forms.TextBox simText;
        public System.Windows.Forms.TextBox debText;
        public System.Windows.Forms.TextBox anaText;
    }
}
