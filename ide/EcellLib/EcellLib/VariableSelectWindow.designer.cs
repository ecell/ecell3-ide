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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableSelectWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.selectTree = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.VSProductButton = new System.Windows.Forms.Button();
            this.VSCloseButton = new System.Windows.Forms.Button();
            this.VSSourceButton = new System.Windows.Forms.Button();
            this.VSConstantButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.selectTree, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // selectTree
            // 
            resources.ApplyResources(this.selectTree, "selectTree");
            this.selectTree.ImageList = PluginManager.GetPluginManager().GetImageList();
            this.selectTree.Name = "selectTree";
            this.selectTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SelectTreeDoubleClick);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.VSProductButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.VSCloseButton, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.VSSourceButton, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.VSConstantButton, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
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
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "VariableSelectWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
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