namespace Ecell.IDE
{
    /// <summary>
    /// Window to select the variable to add to VariableReferenceList.
    /// </summary>
    partial class VariableSelectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableSelectDialog));
            this.selectTree = new System.Windows.Forms.TreeView();
            this.VSProductButton = new System.Windows.Forms.Button();
            this.VSCloseButton = new System.Windows.Forms.Button();
            this.VSSourceButton = new System.Windows.Forms.Button();
            this.VSConstantButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectTree
            // 
            this.selectTree.AccessibleDescription = null;
            this.selectTree.AccessibleName = null;
            resources.ApplyResources(this.selectTree, "selectTree");
            this.selectTree.BackgroundImage = null;
            this.selectTree.Font = null;
            this.selectTree.Name = "selectTree";
            this.selectTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SelectTreeDoubleClick);
            // 
            // VSProductButton
            // 
            this.VSProductButton.AccessibleDescription = null;
            this.VSProductButton.AccessibleName = null;
            resources.ApplyResources(this.VSProductButton, "VSProductButton");
            this.VSProductButton.BackgroundImage = null;
            this.VSProductButton.Font = null;
            this.VSProductButton.Name = "VSProductButton";
            this.VSProductButton.UseVisualStyleBackColor = true;
            this.VSProductButton.Click += new System.EventHandler(this.ProductButtonClick);
            // 
            // VSCloseButton
            // 
            this.VSCloseButton.AccessibleDescription = null;
            this.VSCloseButton.AccessibleName = null;
            resources.ApplyResources(this.VSCloseButton, "VSCloseButton");
            this.VSCloseButton.BackgroundImage = null;
            this.VSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.VSCloseButton.Font = null;
            this.VSCloseButton.Name = "VSCloseButton";
            this.VSCloseButton.UseVisualStyleBackColor = true;
            // 
            // VSSourceButton
            // 
            this.VSSourceButton.AccessibleDescription = null;
            this.VSSourceButton.AccessibleName = null;
            resources.ApplyResources(this.VSSourceButton, "VSSourceButton");
            this.VSSourceButton.BackgroundImage = null;
            this.VSSourceButton.Font = null;
            this.VSSourceButton.Name = "VSSourceButton";
            this.VSSourceButton.UseVisualStyleBackColor = true;
            this.VSSourceButton.Click += new System.EventHandler(this.SourceButtonClick);
            // 
            // VSConstantButton
            // 
            this.VSConstantButton.AccessibleDescription = null;
            this.VSConstantButton.AccessibleName = null;
            resources.ApplyResources(this.VSConstantButton, "VSConstantButton");
            this.VSConstantButton.BackgroundImage = null;
            this.VSConstantButton.Font = null;
            this.VSConstantButton.Name = "VSConstantButton";
            this.VSConstantButton.UseVisualStyleBackColor = true;
            this.VSConstantButton.Click += new System.EventHandler(this.ConstantButtonClick);
            // 
            // VariableSelectDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.VSProductButton);
            this.Controls.Add(this.selectTree);
            this.Controls.Add(this.VSSourceButton);
            this.Controls.Add(this.VSCloseButton);
            this.Controls.Add(this.VSConstantButton);
            this.Font = null;
            this.Name = "VariableSelectDialog";
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Display the object list by TreeView.
        /// </summary>
        public System.Windows.Forms.TreeView selectTree;
        /// <summary>
        /// Button to select the variable to add to VaribleReferenceList.
        /// </summary>
        public System.Windows.Forms.Button VSProductButton;
        /// <summary>
        /// Button to add the source variable to VariableReferenceList.
        /// </summary>
        public System.Windows.Forms.Button VSSourceButton;
        /// <summary>
        /// Button to add the constant variable to VariableReferenceList.
        /// </summary>
        public System.Windows.Forms.Button VSConstantButton;
        private System.Windows.Forms.Button VSCloseButton;
    }
}