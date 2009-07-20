namespace Ecell.IDE.MainWindow.UIComponents
{
    partial class PWDMPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PWDMPanel));
            this.DMListLabel = new System.Windows.Forms.Label();
            this.DMListBox = new System.Windows.Forms.ListBox();
            this.DMRemoveButton = new System.Windows.Forms.Button();
            this.DMAddButon = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ProjectName = new System.Windows.Forms.TextBox();
            this.Comment = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DMListLabel
            // 
            resources.ApplyResources(this.DMListLabel, "DMListLabel");
            this.DMListLabel.Name = "DMListLabel";
            // 
            // DMListBox
            // 
            resources.ApplyResources(this.DMListBox, "DMListBox");
            this.DMListBox.FormattingEnabled = true;
            this.DMListBox.Name = "DMListBox";
            this.DMListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // DMRemoveButton
            // 
            resources.ApplyResources(this.DMRemoveButton, "DMRemoveButton");
            this.DMRemoveButton.Name = "DMRemoveButton";
            this.DMRemoveButton.UseVisualStyleBackColor = true;
            this.DMRemoveButton.Click += new System.EventHandler(this.DMRemoveButton_Click);
            // 
            // DMAddButon
            // 
            resources.ApplyResources(this.DMAddButon, "DMAddButon");
            this.DMAddButon.Name = "DMAddButon";
            this.DMAddButon.UseVisualStyleBackColor = true;
            this.DMAddButon.Click += new System.EventHandler(this.DMAddButon_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ProjectName
            // 
            resources.ApplyResources(this.ProjectName, "ProjectName");
            this.ProjectName.Name = "ProjectName";
            // 
            // Comment
            // 
            this.Comment.AcceptsReturn = true;
            this.Comment.AcceptsTab = true;
            resources.ApplyResources(this.Comment, "Comment");
            this.Comment.Name = "Comment";
            // 
            // PWDMPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Comment);
            this.Controls.Add(this.ProjectName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DMRemoveButton);
            this.Controls.Add(this.DMAddButon);
            this.Controls.Add(this.DMListBox);
            this.Controls.Add(this.DMListLabel);
            this.Name = "PWDMPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DMListLabel;
        public System.Windows.Forms.ListBox DMListBox;
        private System.Windows.Forms.Button DMRemoveButton;
        private System.Windows.Forms.Button DMAddButon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox ProjectName;
        public System.Windows.Forms.TextBox Comment;
    }
}
