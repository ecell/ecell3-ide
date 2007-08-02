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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.VSSelectButton = new System.Windows.Forms.Button();
            this.VSCloseButton = new System.Windows.Forms.Button();
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
            this.selectTree.ImageList = this.imageList1;
            this.selectTree.Name = "selectTree";
            this.selectTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SelectTreeDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "subsystem.png");
            this.imageList1.Images.SetKeyName(1, "process.png");
            this.imageList1.Images.SetKeyName(2, "variable.png");
            this.imageList1.Images.SetKeyName(3, "system.png");
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.VSSelectButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.VSCloseButton, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // VSSelectButton
            // 
            resources.ApplyResources(this.VSSelectButton, "VSSelectButton");
            this.VSSelectButton.Name = "VSSelectButton";
            this.VSSelectButton.UseVisualStyleBackColor = true;
            // 
            // VSCloseButton
            // 
            resources.ApplyResources(this.VSCloseButton, "VSCloseButton");
            this.VSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.VSCloseButton.Name = "VSCloseButton";
            this.VSCloseButton.UseVisualStyleBackColor = true;
            // 
            // VariableSelectWindow
            // 
            this.AcceptButton = this.VSSelectButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.VSCloseButton;
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
        public System.Windows.Forms.Button VSSelectButton;
        /// <summary>
        /// Button to close this dialog.
        /// </summary>
        public System.Windows.Forms.Button VSCloseButton;
        private System.Windows.Forms.ImageList imageList1;
    }
}