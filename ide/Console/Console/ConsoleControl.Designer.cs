namespace Ecell.IDE.Plugins.Console
{
    partial class ConsoleControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleControl));
            this.simText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // simText
            // 
            this.simText.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.simText, "simText");
            this.simText.Name = "simText";
            this.simText.ReadOnly = true;
            // 
            // ConsoleControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simText);
            this.Name = "ConsoleControl";
            this.TabText = "MessageWindow";
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
