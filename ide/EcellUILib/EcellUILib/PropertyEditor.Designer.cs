namespace Ecell.IDE
{
    /// <summary>
    /// The dialog class to edit and update the property of object.
    /// </summary>
    partial class PropertyEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyEditor));
            this.PEApplyButton = new System.Windows.Forms.Button();
            this.PECloseButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.PEPropTabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PECommitTabPage = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.commitLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1.SuspendLayout();
            this.PEPropTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PECommitTabPage.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PEApplyButton
            // 
            resources.ApplyResources(this.PEApplyButton, "PEApplyButton");
            this.PEApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.PEApplyButton.Name = "PEApplyButton";
            this.PEApplyButton.UseVisualStyleBackColor = true;
            // 
            // PECloseButton
            // 
            resources.ApplyResources(this.PECloseButton, "PECloseButton");
            this.PECloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.PECloseButton.Name = "PECloseButton";
            this.PECloseButton.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.PEPropTabPage);
            this.tabControl1.Controls.Add(this.PECommitTabPage);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // PEPropTabPage
            // 
            this.PEPropTabPage.Controls.Add(this.panel1);
            resources.ApplyResources(this.PEPropTabPage, "PEPropTabPage");
            this.PEPropTabPage.Name = "PEPropTabPage";
            this.PEPropTabPage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.layoutPanel);
            this.panel1.Name = "panel1";
            // 
            // layoutPanel
            // 
            resources.ApplyResources(this.layoutPanel, "layoutPanel");
            this.layoutPanel.Name = "layoutPanel";
            // 
            // PECommitTabPage
            // 
            this.PECommitTabPage.Controls.Add(this.panel2);
            resources.ApplyResources(this.PECommitTabPage, "PECommitTabPage");
            this.PECommitTabPage.Name = "PECommitTabPage";
            this.PECommitTabPage.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.commitLayoutPanel);
            this.panel2.Name = "panel2";
            // 
            // commitLayoutPanel
            // 
            resources.ApplyResources(this.commitLayoutPanel, "commitLayoutPanel");
            this.commitLayoutPanel.Name = "commitLayoutPanel";
            // 
            // PropertyEditor
            // 
            this.AcceptButton = this.PEApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.PECloseButton;
            this.Controls.Add(this.PEApplyButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.PECloseButton);
            this.Name = "PropertyEditor";
            this.Shown += new System.EventHandler(this.PropertyEditorShown);
            this.tabControl1.ResumeLayout(false);
            this.PEPropTabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.PECommitTabPage.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Button to update the property of object.
        /// </summary>
        public System.Windows.Forms.Button PEApplyButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button PECloseButton;
        /// <summary>
        /// Panel of PropertyEditor.
        /// </summary>
        public System.Windows.Forms.Panel panel1;
        /// <summary>
        /// Layouter of property in PropertyEditor.
        /// </summary>
        public System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.TabControl tabControl1;
        /// <summary>
        /// Tab page of property in PropertyEditor.
        /// </summary>
        public System.Windows.Forms.TabPage PEPropTabPage;
        /// <summary>
        /// Tab page of commitment for property in PropertyEditor.
        /// </summary>
        public System.Windows.Forms.TabPage PECommitTabPage;
        private System.Windows.Forms.Panel panel2;
        /// <summary>
        /// Layouter of commitment for property in PropertyEditor.
        /// </summary>
        public System.Windows.Forms.TableLayoutPanel commitLayoutPanel;
    }
}