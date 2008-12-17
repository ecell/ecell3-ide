namespace Ecell.IDE.Plugins.Analysis
{
    partial class ParameterEstimationSettingDialog
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
            System.Windows.Forms.GroupBox groupBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterEstimationSettingDialog));
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.GroupBox groupBox2;
            this.formulatorButton = new System.Windows.Forms.Button();
            this.estimationFormulatorTextBox = new System.Windows.Forms.TextBox();
            this.estimationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.advancedButton = new System.Windows.Forms.Button();
            this.parameterEstimationPopulationTextBox = new System.Windows.Forms.TextBox();
            this.parameterEstimationSimulationTimeTextBox = new System.Windows.Forms.TextBox();
            this.parameterEstimationGenerationTextBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.parameterEstimationParameterDataGrid = new System.Windows.Forms.DataGridView();
            this.closeButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.executeButton = new System.Windows.Forms.Button();
            this.parameterEstimationToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.abstractTextBox = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parameterEstimationParameterDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Controls.Add(this.formulatorButton);
            groupBox1.Controls.Add(this.estimationFormulatorTextBox);
            groupBox1.Controls.Add(this.estimationTypeComboBox);
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // formulatorButton
            // 
            resources.ApplyResources(this.formulatorButton, "formulatorButton");
            this.formulatorButton.Name = "formulatorButton";
            this.formulatorButton.UseVisualStyleBackColor = true;
            this.formulatorButton.Click += new System.EventHandler(this.formulatorButtonClicked);
            // 
            // estimationFormulatorTextBox
            // 
            resources.ApplyResources(this.estimationFormulatorTextBox, "estimationFormulatorTextBox");
            this.estimationFormulatorTextBox.Name = "estimationFormulatorTextBox";
            this.estimationFormulatorTextBox.ReadOnly = true;
            // 
            // estimationTypeComboBox
            // 
            resources.ApplyResources(this.estimationTypeComboBox, "estimationTypeComboBox");
            this.estimationTypeComboBox.FormattingEnabled = true;
            this.estimationTypeComboBox.Items.AddRange(new object[] {
            resources.GetString("estimationTypeComboBox.Items"),
            resources.GetString("estimationTypeComboBox.Items1"),
            resources.GetString("estimationTypeComboBox.Items2"),
            resources.GetString("estimationTypeComboBox.Items3"),
            resources.GetString("estimationTypeComboBox.Items4"),
            resources.GetString("estimationTypeComboBox.Items5")});
            this.estimationTypeComboBox.Name = "estimationTypeComboBox";
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
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // groupBox2
            // 
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(this.advancedButton);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(this.parameterEstimationPopulationTextBox);
            groupBox2.Controls.Add(this.parameterEstimationSimulationTimeTextBox);
            groupBox2.Controls.Add(this.parameterEstimationGenerationTextBox);
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // advancedButton
            // 
            resources.ApplyResources(this.advancedButton, "advancedButton");
            this.advancedButton.Name = "advancedButton";
            this.advancedButton.UseVisualStyleBackColor = true;
            this.advancedButton.Click += new System.EventHandler(this.AdvancedButtonClicked);
            // 
            // parameterEstimationPopulationTextBox
            // 
            resources.ApplyResources(this.parameterEstimationPopulationTextBox, "parameterEstimationPopulationTextBox");
            this.parameterEstimationPopulationTextBox.Name = "parameterEstimationPopulationTextBox";
            this.parameterEstimationPopulationTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Population_Validating);
            // 
            // parameterEstimationSimulationTimeTextBox
            // 
            resources.ApplyResources(this.parameterEstimationSimulationTimeTextBox, "parameterEstimationSimulationTimeTextBox");
            this.parameterEstimationSimulationTimeTextBox.Name = "parameterEstimationSimulationTimeTextBox";
            this.parameterEstimationSimulationTimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.SimulationTime_Validating);
            // 
            // parameterEstimationGenerationTextBox
            // 
            resources.ApplyResources(this.parameterEstimationGenerationTextBox, "parameterEstimationGenerationTextBox");
            this.parameterEstimationGenerationTextBox.Name = "parameterEstimationGenerationTextBox";
            this.parameterEstimationGenerationTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Generation_Validating);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.parameterEstimationParameterDataGrid);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // parameterEstimationParameterDataGrid
            // 
            this.parameterEstimationParameterDataGrid.AllowUserToAddRows = false;
            this.parameterEstimationParameterDataGrid.AllowUserToDeleteRows = false;
            this.parameterEstimationParameterDataGrid.AllowUserToResizeColumns = false;
            this.parameterEstimationParameterDataGrid.AllowUserToResizeRows = false;
            resources.ApplyResources(this.parameterEstimationParameterDataGrid, "parameterEstimationParameterDataGrid");
            this.parameterEstimationParameterDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.parameterEstimationParameterDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parameterEstimationParameterDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.parameterEstimationParameterDataGrid.Name = "parameterEstimationParameterDataGrid";
            this.parameterEstimationParameterDataGrid.RowHeadersVisible = false;
            this.parameterEstimationParameterDataGrid.RowTemplate.Height = 21;
            this.parameterEstimationParameterDataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ParameterDataChanged);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // executeButton
            // 
            resources.ApplyResources(this.executeButton, "executeButton");
            this.executeButton.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.executeButton.Name = "executeButton";
            this.executeButton.UseVisualStyleBackColor = true;
            // 
            // abstractTextBox
            // 
            this.abstractTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.abstractTextBox, "abstractTextBox");
            this.abstractTextBox.Name = "abstractTextBox";
            this.abstractTextBox.ReadOnly = true;
            this.abstractTextBox.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 40F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 40F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // ParameterEstimationSettingDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.Controls.Add(this.abstractTextBox);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Name = "ParameterEstimationSettingDialog";
            this.Load += new System.EventHandler(this.FormLoad);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.parameterEstimationParameterDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox estimationFormulatorTextBox;
        private System.Windows.Forms.ComboBox estimationTypeComboBox;
        private System.Windows.Forms.Button formulatorButton;
        private System.Windows.Forms.TextBox parameterEstimationPopulationTextBox;
        private System.Windows.Forms.TextBox parameterEstimationGenerationTextBox;
        private System.Windows.Forms.Button advancedButton;
        private System.Windows.Forms.TextBox parameterEstimationSimulationTimeTextBox;
        private System.Windows.Forms.DataGridView parameterEstimationParameterDataGrid;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.ToolTip parameterEstimationToolTip;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox abstractTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}