namespace Ecell.IDE.Plugins.Spreadsheet
{
    /// <summary>
    /// Dialog to search object in Spreadsheet.
    /// </summary>
    partial class SearchInstanceDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchInstanceDialog));
            System.Windows.Forms.Label label1;
            this.searchButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.searchText = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            resources.ApplyResources(this.searchButton, "searchButton");
            this.searchButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.searchButton.Name = "searchButton";
            this.searchButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // searchText
            // 
            resources.ApplyResources(this.searchText, "searchText");
            this.searchText.Name = "searchText";
            this.searchText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextKeyPress);
            // 
            // SearchInstanceDialog
            // 
            this.AcceptButton = this.searchButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.Controls.Add(label1);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.closeButton);
            this.Name = "SearchInstanceDialog";
            this.Shown += new System.EventHandler(this.SearchInstanceShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox searchText;
        private System.Windows.Forms.Button closeButton;

    }
}
