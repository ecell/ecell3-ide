namespace Ecell.IDE.Plugins.Plotter
{
    partial class SelectPlotDataDialog
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectPlotDataDialog));
            System.Windows.Forms.Label label2;
            this.SPDCreateButton = new System.Windows.Forms.Button();
            this.SPDCancelButton = new System.Windows.Forms.Button();
            this.XplotComboBox = new System.Windows.Forms.ComboBox();
            this.YPlotComboBox = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // SPDCreateButton
            // 
            resources.ApplyResources(this.SPDCreateButton, "SPDCreateButton");
            this.SPDCreateButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SPDCreateButton.Name = "SPDCreateButton";
            this.SPDCreateButton.UseVisualStyleBackColor = true;
            // 
            // SPDCancelButton
            // 
            resources.ApplyResources(this.SPDCancelButton, "SPDCancelButton");
            this.SPDCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SPDCancelButton.Name = "SPDCancelButton";
            this.SPDCancelButton.UseVisualStyleBackColor = true;
            // 
            // XplotComboBox
            // 
            resources.ApplyResources(this.XplotComboBox, "XplotComboBox");
            this.XplotComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XplotComboBox.FormattingEnabled = true;
            this.XplotComboBox.Name = "XplotComboBox";
            // 
            // YPlotComboBox
            // 
            resources.ApplyResources(this.YPlotComboBox, "YPlotComboBox");
            this.YPlotComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.YPlotComboBox.FormattingEnabled = true;
            this.YPlotComboBox.Name = "YPlotComboBox";
            // 
            // SelectPlotDataDialog
            // 
            this.AcceptButton = this.SPDCreateButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.SPDCancelButton;
            this.Controls.Add(this.YPlotComboBox);
            this.Controls.Add(label2);
            this.Controls.Add(this.XplotComboBox);
            this.Controls.Add(label1);
            this.Controls.Add(this.SPDCancelButton);
            this.Controls.Add(this.SPDCreateButton);
            this.Name = "SelectPlotDataDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingSelectPlotDataDialog);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SPDCreateButton;
        private System.Windows.Forms.Button SPDCancelButton;
        private System.Windows.Forms.ComboBox XplotComboBox;
        private System.Windows.Forms.ComboBox YPlotComboBox;
    }
}