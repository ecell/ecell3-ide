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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageWindowControl));
            this.simText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // simText
            // 
            this.simText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simText.Location = new System.Drawing.Point(0, 0);
            this.simText.Multiline = true;
            this.simText.Name = "simText";
            this.simText.ReadOnly = true;
            this.simText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.simText.Size = new System.Drawing.Size(362, 182);
            this.simText.TabIndex = 0;
            // 
            // MessageWindowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 182);
            this.Controls.Add(this.simText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MessageWindowControl";
            this.TabText = "MessageWindow";
            this.Text = "MessageWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// TextBox for  Simulation Message and Edit Model.
        /// </summary>
        public System.Windows.Forms.TextBox simText;
    }
}
