namespace Ecell.IDE.Plugins.Analysis
{
    partial class BifurcationSettingDialog
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BifurcationSettingDialog));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Button cancelButton;
            System.Windows.Forms.Button okButton;
            System.Windows.Forms.Button executeButton;
            this.bifurcationSimulationTimeTextBox = new System.Windows.Forms.TextBox();
            this.bifurcationWindowSizeTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bifurcationMaxInputTextBox = new System.Windows.Forms.TextBox();
            this.bifurcationMaxFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.bifurcationMinFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bifurcationParameterDataGrid = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bifurcationObservedDataGrid = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bifurcationToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.abstractTextBox = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            cancelButton = new System.Windows.Forms.Button();
            okButton = new System.Windows.Forms.Button();
            executeButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bifurcationParameterDataGrid)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bifurcationObservedDataGrid)).BeginInit();
            this.groupBox4.SuspendLayout();
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
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // cancelButton
            // 
            resources.ApplyResources(cancelButton, "cancelButton");
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            resources.ApplyResources(okButton, "okButton");
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.UseVisualStyleBackColor = true;
            // 
            // executeButton
            // 
            resources.ApplyResources(executeButton, "executeButton");
            executeButton.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            executeButton.Name = "executeButton";
            executeButton.UseVisualStyleBackColor = true;
            // 
            // bifurcationSimulationTimeTextBox
            // 
            resources.ApplyResources(this.bifurcationSimulationTimeTextBox, "bifurcationSimulationTimeTextBox");
            this.bifurcationSimulationTimeTextBox.Name = "bifurcationSimulationTimeTextBox";
            // 
            // bifurcationWindowSizeTextBox
            // 
            resources.ApplyResources(this.bifurcationWindowSizeTextBox, "bifurcationWindowSizeTextBox");
            this.bifurcationWindowSizeTextBox.Name = "bifurcationWindowSizeTextBox";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(label2);
            this.groupBox1.Controls.Add(this.bifurcationWindowSizeTextBox);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.bifurcationSimulationTimeTextBox);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // bifurcationMaxInputTextBox
            // 
            resources.ApplyResources(this.bifurcationMaxInputTextBox, "bifurcationMaxInputTextBox");
            this.bifurcationMaxInputTextBox.Name = "bifurcationMaxInputTextBox";
            // 
            // bifurcationMaxFrequencyTextBox
            // 
            resources.ApplyResources(this.bifurcationMaxFrequencyTextBox, "bifurcationMaxFrequencyTextBox");
            this.bifurcationMaxFrequencyTextBox.Name = "bifurcationMaxFrequencyTextBox";
            // 
            // bifurcationMinFrequencyTextBox
            // 
            resources.ApplyResources(this.bifurcationMinFrequencyTextBox, "bifurcationMinFrequencyTextBox");
            this.bifurcationMinFrequencyTextBox.Name = "bifurcationMinFrequencyTextBox";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(label5);
            this.groupBox2.Controls.Add(this.bifurcationMaxInputTextBox);
            this.groupBox2.Controls.Add(label4);
            this.groupBox2.Controls.Add(this.bifurcationMaxFrequencyTextBox);
            this.groupBox2.Controls.Add(this.bifurcationMinFrequencyTextBox);
            this.groupBox2.Controls.Add(label3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // bifurcationParameterDataGrid
            // 
            this.bifurcationParameterDataGrid.AllowUserToAddRows = false;
            this.bifurcationParameterDataGrid.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.bifurcationParameterDataGrid, "bifurcationParameterDataGrid");
            this.bifurcationParameterDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.bifurcationParameterDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bifurcationParameterDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.bifurcationParameterDataGrid.Name = "bifurcationParameterDataGrid";
            this.bifurcationParameterDataGrid.RowHeadersVisible = false;
            this.bifurcationParameterDataGrid.RowTemplate.Height = 21;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.bifurcationParameterDataGrid);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // bifurcationObservedDataGrid
            // 
            this.bifurcationObservedDataGrid.AllowUserToAddRows = false;
            this.bifurcationObservedDataGrid.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.bifurcationObservedDataGrid, "bifurcationObservedDataGrid");
            this.bifurcationObservedDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.bifurcationObservedDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bifurcationObservedDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            this.bifurcationObservedDataGrid.Name = "bifurcationObservedDataGrid";
            this.bifurcationObservedDataGrid.RowHeadersVisible = false;
            this.bifurcationObservedDataGrid.RowTemplate.Height = 21;
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.bifurcationObservedDataGrid);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
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
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // BifurcationSettingDialog
            // 
            this.AcceptButton = okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cancelButton;
            this.Controls.Add(this.abstractTextBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(executeButton);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "BifurcationSettingDialog";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bifurcationParameterDataGrid)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bifurcationObservedDataGrid)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox bifurcationSimulationTimeTextBox;
        private System.Windows.Forms.TextBox bifurcationWindowSizeTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox bifurcationMaxInputTextBox;
        private System.Windows.Forms.TextBox bifurcationMaxFrequencyTextBox;
        private System.Windows.Forms.TextBox bifurcationMinFrequencyTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView bifurcationParameterDataGrid;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView bifurcationObservedDataGrid;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolTip bifurcationToolTip;
        private System.Windows.Forms.TextBox abstractTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
    }
}