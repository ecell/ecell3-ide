namespace Ecell.IDE
{
    /// <summary>
    /// Dialog to edit the formulator.
    /// </summary>
    partial class FormulatorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormulatorDialog));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FApplyButton = new System.Windows.Forms.Button();
            this.FCloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // FApplyButton
            // 
            resources.ApplyResources(this.FApplyButton, "FApplyButton");
            this.FApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            // FormulatorDialog
            // 
            this.AcceptButton = this.FApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.FCloseButton;
            this.Controls.Add(this.FApplyButton);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.FCloseButton);
            this.Name = "FormulatorDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormulatorDialog_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FApplyButton;
        private System.Windows.Forms.Button FCloseButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}