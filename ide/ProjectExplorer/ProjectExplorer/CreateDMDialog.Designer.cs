namespace Ecell.IDE.Plugins.ProjectExplorer
{
    partial class CreateDMDialog
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
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateDMDialog));
            this.INNewButton = new System.Windows.Forms.Button();
            this.INCancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.INTextBox = new System.Windows.Forms.TextBox();
            this.templateComboBox = new System.Windows.Forms.ComboBox();
            this.templateRichText = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // INNewButton
            // 
            resources.ApplyResources(this.INNewButton, "INNewButton");
            this.INNewButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.INNewButton.Name = "INNewButton";
            this.INNewButton.UseVisualStyleBackColor = true;
            this.INNewButton.Click += new System.EventHandler(this.INNewButton_Click);
            // 
            // INCancelButton
            // 
            resources.ApplyResources(this.INCancelButton, "INCancelButton");
            this.INCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.INCancelButton.Name = "INCancelButton";
            this.INCancelButton.UseVisualStyleBackColor = true;
            this.INCancelButton.Click += new System.EventHandler(this.INCancelButton_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // INTextBox
            // 
            resources.ApplyResources(this.INTextBox, "INTextBox");
            this.INTextBox.Name = "INTextBox";
            // 
            // templateComboBox
            // 
            resources.ApplyResources(this.templateComboBox, "templateComboBox");
            this.templateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateComboBox.FormattingEnabled = true;
            this.templateComboBox.Name = "templateComboBox";
            this.templateComboBox.SelectedIndexChanged += new System.EventHandler(this.TemplateComboBoxSelectedChanged);
            // 
            // templateRichText
            // 
            resources.ApplyResources(this.templateRichText, "templateRichText");
            this.templateRichText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.templateRichText.Name = "templateRichText";
            this.templateRichText.ReadOnly = true;
            this.templateRichText.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // CreateDMDialog
            // 
            this.AcceptButton = this.INNewButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.INCancelButton;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.templateRichText);
            this.Controls.Add(label2);
            this.Controls.Add(this.templateComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.INNewButton);
            this.Controls.Add(this.INTextBox);
            this.Controls.Add(this.INCancelButton);
            this.Name = "CreateDMDialog";
            this.Shown += new System.EventHandler(this.InputNameShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button INNewButton;
        private System.Windows.Forms.Button INCancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox INTextBox;
        private System.Windows.Forms.ComboBox templateComboBox;
        private System.Windows.Forms.RichTextBox templateRichText;
        private System.Windows.Forms.Label label3;
    }
}
