namespace Ecell.UI.Components
{
    /// <summary>
    /// UserConstrol to manager the formula.
    /// </summary>
    partial class FormulatorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormulatorControl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.reserveBox = new System.Windows.Forms.ComboBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.PlusButton = new System.Windows.Forms.Button();
            this.MinusButton = new System.Windows.Forms.Button();
            this.MultiplyButton = new System.Windows.Forms.Button();
            this.SplitButton = new System.Windows.Forms.Button();
            this.ParentButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.FunctionBox = new System.Windows.Forms.ComboBox();
            this.AddFunctionButton = new System.Windows.Forms.Button();
            this.StringButton = new System.Windows.Forms.Button();
            this.stringBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TemplateApplyButton = new System.Windows.Forms.Button();
            this.templateComboBox = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // reserveBox
            // 
            this.reserveBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reserveBox.FormattingEnabled = true;
            resources.ApplyResources(this.reserveBox, "reserveBox");
            this.reserveBox.Name = "reserveBox";
            // 
            // AddButton
            // 
            resources.ApplyResources(this.AddButton, "AddButton");
            this.AddButton.Name = "AddButton";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // PlusButton
            // 
            resources.ApplyResources(this.PlusButton, "PlusButton");
            this.PlusButton.Name = "PlusButton";
            this.PlusButton.UseVisualStyleBackColor = true;
            this.PlusButton.Click += new System.EventHandler(this.PlusButton_Click);
            // 
            // MinusButton
            // 
            resources.ApplyResources(this.MinusButton, "MinusButton");
            this.MinusButton.Name = "MinusButton";
            this.MinusButton.UseVisualStyleBackColor = true;
            this.MinusButton.Click += new System.EventHandler(this.MinusButton_Click);
            // 
            // MultiplyButton
            // 
            resources.ApplyResources(this.MultiplyButton, "MultiplyButton");
            this.MultiplyButton.Name = "MultiplyButton";
            this.MultiplyButton.UseVisualStyleBackColor = true;
            this.MultiplyButton.Click += new System.EventHandler(this.MultiplyButton_Click);
            // 
            // SplitButton
            // 
            resources.ApplyResources(this.SplitButton, "SplitButton");
            this.SplitButton.Name = "SplitButton";
            this.SplitButton.UseVisualStyleBackColor = true;
            this.SplitButton.Click += new System.EventHandler(this.SplitButton_Click);
            // 
            // ParentButton
            // 
            resources.ApplyResources(this.ParentButton, "ParentButton");
            this.ParentButton.Name = "ParentButton";
            this.ParentButton.UseVisualStyleBackColor = true;
            this.ParentButton.Click += new System.EventHandler(this.ParentButton_Click);
            // 
            // DeleteButton
            // 
            resources.ApplyResources(this.DeleteButton, "DeleteButton");
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FunctionBox
            // 
            this.FunctionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FunctionBox.FormattingEnabled = true;
            resources.ApplyResources(this.FunctionBox, "FunctionBox");
            this.FunctionBox.Name = "FunctionBox";
            // 
            // AddFunctionButton
            // 
            resources.ApplyResources(this.AddFunctionButton, "AddFunctionButton");
            this.AddFunctionButton.Name = "AddFunctionButton";
            this.AddFunctionButton.UseVisualStyleBackColor = true;
            this.AddFunctionButton.Click += new System.EventHandler(this.AddFunctionButton_Click);
            // 
            // StringButton
            // 
            resources.ApplyResources(this.StringButton, "StringButton");
            this.StringButton.Name = "StringButton";
            this.StringButton.UseVisualStyleBackColor = true;
            this.StringButton.Click += new System.EventHandler(this.StringButton_Click);
            // 
            // stringBox
            // 
            resources.ApplyResources(this.stringBox, "stringBox");
            this.stringBox.Name = "stringBox";
            this.stringBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.stringBox_KeyDown);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Name = "panel1";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // TemplateApplyButton
            // 
            resources.ApplyResources(this.TemplateApplyButton, "TemplateApplyButton");
            this.TemplateApplyButton.Name = "TemplateApplyButton";
            this.TemplateApplyButton.UseVisualStyleBackColor = true;
            this.TemplateApplyButton.Click += new System.EventHandler(this.TemplateApplyButton_Click);
            // 
            // templateComboBox
            // 
            resources.ApplyResources(this.templateComboBox, "templateComboBox");
            this.templateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateComboBox.FormattingEnabled = true;
            this.templateComboBox.Name = "templateComboBox";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.templateComboBox);
            this.panel2.Controls.Add(this.TemplateApplyButton);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.AddButton);
            this.panel3.Controls.Add(this.reserveBox);
            this.panel3.Controls.Add(this.PlusButton);
            this.panel3.Controls.Add(this.StringButton);
            this.panel3.Controls.Add(this.AddFunctionButton);
            this.panel3.Controls.Add(this.MinusButton);
            this.panel3.Controls.Add(this.FunctionBox);
            this.panel3.Controls.Add(this.MultiplyButton);
            this.panel3.Controls.Add(this.ParentButton);
            this.panel3.Controls.Add(this.stringBox);
            this.panel3.Controls.Add(this.SplitButton);
            this.panel3.Controls.Add(this.DeleteButton);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // FormulatorControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormulatorControl";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        /// <summary>
        /// Button to add the "+" to selected position.
        /// </summary>
        public System.Windows.Forms.Button PlusButton;
        /// <summary>
        /// Button to add the "-" to selected position.
        /// </summary>
        public System.Windows.Forms.Button MinusButton;
        /// <summary>
        /// Button to add the "*" to selected position.
        /// </summary>
        public System.Windows.Forms.Button MultiplyButton;
        /// <summary>
        /// Button to add the "/" to selected position.
        /// </summary>
        public System.Windows.Forms.Button SplitButton;
        /// <summary>
        /// Button to add the input string to selected position.
        /// </summary>
        public System.Windows.Forms.Button StringButton;
        /// <summary>
        /// TextBox to input the string to add the formula.
        /// </summary>
        public System.Windows.Forms.TextBox stringBox;
        /// <summary>
        /// Button to add the "( )" to selected position.
        /// </summary>
        public System.Windows.Forms.Button ParentButton;
        private System.Windows.Forms.Panel panel1;
        /// <summary>
        /// PictureBox to display the formula.
        /// </summary>
        public System.Windows.Forms.PictureBox pictureBox1;
        /// <summary>
        /// ComboBox to display the reserved strings.
        /// </summary>
        public System.Windows.Forms.ComboBox reserveBox;
        /// <summary>
        /// Button to add the selected data.
        /// </summary>
        public System.Windows.Forms.Button AddButton;
        /// <summary>
        /// Button to delete the selected data.
        /// </summary>
        public System.Windows.Forms.Button DeleteButton;
        /// <summary>
        /// ComboBox to display the reserved functions.
        /// </summary>
        public System.Windows.Forms.ComboBox FunctionBox;
        /// <summary>
        /// Button to add the function to selected position.
        /// </summary>
        public System.Windows.Forms.Button AddFunctionButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button TemplateApplyButton;
        private System.Windows.Forms.ComboBox templateComboBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}
