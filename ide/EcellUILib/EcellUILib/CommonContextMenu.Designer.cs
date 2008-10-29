namespace Ecell.IDE
{
    partial class CommonContextMenu
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonContextMenu));
            this.commonContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mergeSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.observedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parameterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addVariableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commonContextMenuStrip.SuspendLayout();
            // 
            // commonContextMenuStrip
            // 
            this.commonContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.mergeSystemToolStripMenuItem,
            this.toolStripSeparator1,
            this.loggingToolStripMenuItem,
            this.observedToolStripMenuItem,
            this.parameterToolStripMenuItem,
            this.toolStripSeparator2,
            this.propertyToolStripMenuItem});
            this.commonContextMenuStrip.Name = "commonContextMenuStrip";
            resources.ApplyResources(this.commonContextMenuStrip, "commonContextMenuStrip");
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSystemToolStripMenuItem,
            this.addVariableToolStripMenuItem,
            this.addProcessToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            resources.ApplyResources(this.addToolStripMenuItem, "addToolStripMenuItem");
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // mergeSystemToolStripMenuItem
            // 
            this.mergeSystemToolStripMenuItem.Name = "mergeSystemToolStripMenuItem";
            resources.ApplyResources(this.mergeSystemToolStripMenuItem, "mergeSystemToolStripMenuItem");
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            resources.ApplyResources(this.loggingToolStripMenuItem, "loggingToolStripMenuItem");
            // 
            // observedToolStripMenuItem
            // 
            this.observedToolStripMenuItem.Name = "observedToolStripMenuItem";
            resources.ApplyResources(this.observedToolStripMenuItem, "observedToolStripMenuItem");
            // 
            // parameterToolStripMenuItem
            // 
            this.parameterToolStripMenuItem.Name = "parameterToolStripMenuItem";
            resources.ApplyResources(this.parameterToolStripMenuItem, "parameterToolStripMenuItem");
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            resources.ApplyResources(this.propertyToolStripMenuItem, "propertyToolStripMenuItem");
            // 
            // addSystemToolStripMenuItem
            // 
            this.addSystemToolStripMenuItem.Name = "addSystemToolStripMenuItem";
            resources.ApplyResources(this.addSystemToolStripMenuItem, "addSystemToolStripMenuItem");
            this.addSystemToolStripMenuItem.Tag = "System";
            // 
            // addVariableToolStripMenuItem
            // 
            this.addVariableToolStripMenuItem.Name = "addVariableToolStripMenuItem";
            resources.ApplyResources(this.addVariableToolStripMenuItem, "addVariableToolStripMenuItem");
            this.addVariableToolStripMenuItem.Tag = "Variable";
            // 
            // addProcessToolStripMenuItem
            // 
            this.addProcessToolStripMenuItem.Name = "addProcessToolStripMenuItem";
            resources.ApplyResources(this.addProcessToolStripMenuItem, "addProcessToolStripMenuItem");
            this.addProcessToolStripMenuItem.Tag = "Process";
            this.commonContextMenuStrip.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip commonContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addVariableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergeSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem observedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parameterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;
    }
}
