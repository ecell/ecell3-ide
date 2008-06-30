namespace EcellLib
{
    /// <summary>
    /// Window to select the variable to add to VariableReferenceList.
    /// </summary>
    partial class VariableSelectWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableSelectWindow));
            this.selectTree = new System.Windows.Forms.TreeView();
            this.VSProductButton = new System.Windows.Forms.Button();
            this.VSCloseButton = new System.Windows.Forms.Button();
            this.VSSourceButton = new System.Windows.Forms.Button();
            this.VSConstantButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectTree
            // 
            resources.ApplyResources(this.selectTree, "selectTree");
            this.selectTree.Name = "selectTree";
            this.selectTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SelectTreeDoubleClick);
            // 
            // VSProductButton
            // 
            resources.ApplyResources(this.VSProductButton, "VSProductButton");
            this.VSProductButton.Name = "VSProductButton";
            this.VSProductButton.UseVisualStyleBackColor = true;
            // 
            // VSCloseButton
            // 
            resources.ApplyResources(this.VSCloseButton, "VSCloseButton");
            this.VSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.VSCloseButton.Name = "VSCloseButton";
            this.VSCloseButton.UseVisualStyleBackColor = true;
            // 
            // VSSourceButton
            // 
            resources.ApplyResources(this.VSSourceButton, "VSSourceButton");
            this.VSSourceButton.Name = "VSSourceButton";
            this.VSSourceButton.UseVisualStyleBackColor = true;
            // 
            // VSConstantButton
            // 
            resources.ApplyResources(this.VSConstantButton, "VSConstantButton");
            this.VSConstantButton.Name = "VSConstantButton";
            this.VSConstantButton.UseVisualStyleBackColor = true;
            // 
            // VariableSelectWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VSProductButton);
            this.Controls.Add(this.selectTree);
            this.Controls.Add(this.VSSourceButton);
            this.Controls.Add(this.VSCloseButton);
            this.Controls.Add(this.VSConstantButton);
            this.Name = "VariableSelectWindow";
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
        /// Button to close this dialog.
        /// </summary>
        public System.Windows.Forms.Button VSCloseButton;
        /// <summary>
        /// Button to add the source variable to VariableReferenceList.
        /// </summary>
        public System.Windows.Forms.Button VSSourceButton;
        /// <summary>
        /// Button to add the constant variable to VariableReferenceList.
        /// </summary>
        public System.Windows.Forms.Button VSConstantButton;
    }
}