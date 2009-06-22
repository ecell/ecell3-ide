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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.executeButton = new System.Windows.Forms.Button();
            this.bifurcationSimulationTimeTextBox = new System.Windows.Forms.TextBox();
            this.bifurcationWindowSizeTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.bifurcationMaxInputTextBox = new System.Windows.Forms.TextBox();
            this.bifurcationMaxFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.bifurcationMinFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bifurcationParameterDataGrid = new System.Windows.Forms.DataGridView();
            this.paramFullPNColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paramBContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteBParamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bifurcationObservedDataGrid = new System.Windows.Forms.DataGridView();
            this.observedFullPNColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.observedBContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteBObservedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bifurcationToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.abstractTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bifurcationParameterDataGrid)).BeginInit();
            this.paramBContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bifurcationObservedDataGrid)).BeginInit();
            this.observedBContextMenuStrip.SuspendLayout();
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
            // executeButton
            // 
            resources.ApplyResources(this.executeButton, "executeButton");
            this.executeButton.Name = "executeButton";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.ExecuteButtonClick);
            // 
            // bifurcationSimulationTimeTextBox
            // 
            resources.ApplyResources(this.bifurcationSimulationTimeTextBox, "bifurcationSimulationTimeTextBox");
            this.bifurcationSimulationTimeTextBox.Name = "bifurcationSimulationTimeTextBox";
            this.bifurcationSimulationTimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.SimulationTime_Validating);
            // 
            // bifurcationWindowSizeTextBox
            // 
            resources.ApplyResources(this.bifurcationWindowSizeTextBox, "bifurcationWindowSizeTextBox");
            this.bifurcationWindowSizeTextBox.Name = "bifurcationWindowSizeTextBox";
            this.bifurcationWindowSizeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.WindowSize_Validating);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(label2);
            this.groupBox1.Controls.Add(this.bifurcationWindowSizeTextBox);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.bifurcationSimulationTimeTextBox);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // bifurcationMaxInputTextBox
            // 
            resources.ApplyResources(this.bifurcationMaxInputTextBox, "bifurcationMaxInputTextBox");
            this.bifurcationMaxInputTextBox.Name = "bifurcationMaxInputTextBox";
            this.bifurcationMaxInputTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.MaxInput_Validating);
            // 
            // bifurcationMaxFrequencyTextBox
            // 
            resources.ApplyResources(this.bifurcationMaxFrequencyTextBox, "bifurcationMaxFrequencyTextBox");
            this.bifurcationMaxFrequencyTextBox.Name = "bifurcationMaxFrequencyTextBox";
            this.bifurcationMaxFrequencyTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.MaxFrequency_Validating);
            // 
            // bifurcationMinFrequencyTextBox
            // 
            resources.ApplyResources(this.bifurcationMinFrequencyTextBox, "bifurcationMinFrequencyTextBox");
            this.bifurcationMinFrequencyTextBox.Name = "bifurcationMinFrequencyTextBox";
            this.bifurcationMinFrequencyTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.MinFrequency_Validating);
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
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.bifurcationParameterDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.bifurcationParameterDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.paramFullPNColumn,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.bifurcationParameterDataGrid.ContextMenuStrip = this.paramBContextMenuStrip;
            this.bifurcationParameterDataGrid.MultiSelect = false;
            this.bifurcationParameterDataGrid.Name = "bifurcationParameterDataGrid";
            this.bifurcationParameterDataGrid.RowHeadersVisible = false;
            this.bifurcationParameterDataGrid.RowTemplate.Height = 21;
            this.bifurcationParameterDataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ParameterDataChanged);
            // 
            // paramFullPNColumn
            // 
            this.paramFullPNColumn.FillWeight = 80F;
            resources.ApplyResources(this.paramFullPNColumn, "paramFullPNColumn");
            this.paramFullPNColumn.Name = "paramFullPNColumn";
            this.paramFullPNColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn2.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn3.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewTextBoxColumn4.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // paramBContextMenuStrip
            // 
            this.paramBContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteBParamToolStripMenuItem});
            this.paramBContextMenuStrip.Name = "paramBContextMenuStrip";
            resources.ApplyResources(this.paramBContextMenuStrip, "paramBContextMenuStrip");
            this.paramBContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.paramContextMenuStripOpening);
            // 
            // deleteBParamToolStripMenuItem
            // 
            this.deleteBParamToolStripMenuItem.Name = "deleteBParamToolStripMenuItem";
            resources.ApplyResources(this.deleteBParamToolStripMenuItem, "deleteBParamToolStripMenuItem");
            this.deleteBParamToolStripMenuItem.Click += new System.EventHandler(this.DeleteParamClick);
            // 
            // bifurcationObservedDataGrid
            // 
            this.bifurcationObservedDataGrid.AllowUserToAddRows = false;
            this.bifurcationObservedDataGrid.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.bifurcationObservedDataGrid, "bifurcationObservedDataGrid");
            this.bifurcationObservedDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.bifurcationObservedDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.bifurcationObservedDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.observedFullPNColumn,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            this.bifurcationObservedDataGrid.ContextMenuStrip = this.observedBContextMenuStrip;
            this.bifurcationObservedDataGrid.Name = "bifurcationObservedDataGrid";
            this.bifurcationObservedDataGrid.RowHeadersVisible = false;
            this.bifurcationObservedDataGrid.RowTemplate.Height = 21;
            this.bifurcationObservedDataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ObservedDataChanged);
            // 
            // observedFullPNColumn
            // 
            this.observedFullPNColumn.FillWeight = 90F;
            resources.ApplyResources(this.observedFullPNColumn, "observedFullPNColumn");
            this.observedFullPNColumn.Name = "observedFullPNColumn";
            this.observedFullPNColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewTextBoxColumn6.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewTextBoxColumn7.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridViewTextBoxColumn8.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle17;
            this.dataGridViewTextBoxColumn9.FillWeight = 30F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // observedBContextMenuStrip
            // 
            this.observedBContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteBObservedToolStripMenuItem});
            this.observedBContextMenuStrip.Name = "observedBContextMenuStrip";
            resources.ApplyResources(this.observedBContextMenuStrip, "observedBContextMenuStrip");
            this.observedBContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.observedConextMenuOpening);
            // 
            // deleteBObservedToolStripMenuItem
            // 
            this.deleteBObservedToolStripMenuItem.Name = "deleteBObservedToolStripMenuItem";
            resources.ApplyResources(this.deleteBObservedToolStripMenuItem, "deleteBObservedToolStripMenuItem");
            this.deleteBObservedToolStripMenuItem.Click += new System.EventHandler(this.ObservedDeleteClick);
            // 
            // abstractTextBox
            // 
            resources.ApplyResources(this.abstractTextBox, "abstractTextBox");
            this.abstractTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.abstractTextBox.Name = "abstractTextBox";
            this.abstractTextBox.ReadOnly = true;
            this.abstractTextBox.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // BifurcationSettingDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bifurcationObservedDataGrid);
            this.Controls.Add(this.bifurcationParameterDataGrid);
            this.Controls.Add(this.abstractTextBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.executeButton);
            this.Name = "BifurcationSettingDialog";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bifurcationParameterDataGrid)).EndInit();
            this.paramBContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bifurcationObservedDataGrid)).EndInit();
            this.observedBContextMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridView bifurcationObservedDataGrid;
        private System.Windows.Forms.ToolTip bifurcationToolTip;
        private System.Windows.Forms.TextBox abstractTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ContextMenuStrip paramBContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteBParamToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip observedBContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteBObservedToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn observedFullPNColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn paramFullPNColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Button executeButton;
    }
}
