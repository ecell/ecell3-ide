namespace EcellLib
{
    /// <summary>
    /// Dialog to edit the formulator.
    /// </summary>
    partial class FormulatorWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormulatorWindow));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.FApplyButton = new System.Windows.Forms.Button();
            this.FCloseButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.FApplyButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.FCloseButton, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // FApplyButton
            // 
            resources.ApplyResources(this.FApplyButton, "FApplyButton");
            this.FApplyButton.Name = "FApplyButton";
            this.FApplyButton.UseVisualStyleBackColor = true;
            // 
            // FCloseButton
            // 
            resources.ApplyResources(this.FCloseButton, "FCloseButton");
            this.FCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.FCloseButton.Name = "FCloseButton";
            this.FCloseButton.UseVisualStyleBackColor = true;
            // 
            // FormulatorWindow
            // 
            this.AcceptButton = this.FApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.FCloseButton;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "FormulatorWindow";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        /// <summary>
        /// OK button to apply the edht formulator.
        /// </summary>
        public System.Windows.Forms.Button FApplyButton;
        /// <summary>
        /// Close this window.
        /// </summary>
        public System.Windows.Forms.Button FCloseButton;
        /// <summary>
        /// Layouter for FormulatorWindow.
        /// </summary>
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}