namespace Ecell.IDE.Plugins.Analysis
{
    partial class RobustAnalysisSettingDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobustAnalysisSettingDialog));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.GroupBox groupBox5;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Button button2;
            System.Windows.Forms.Button button3;
            this.robustAnalysisWindowSizeTextBox = new System.Windows.Forms.TextBox();
            this.robustAnalysisSimulationTimeTextBox = new System.Windows.Forms.TextBox();
            this.robustAnalysisSampleNumberTextBox = new System.Windows.Forms.TextBox();
            this.robustAnalysisRandomCheckBox = new System.Windows.Forms.CheckBox();
            this.robustAnalysisMatrixCheckBox = new System.Windows.Forms.CheckBox();
            this.robustAnalysisMinFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.robustAnalysisMaxSampleTextBox = new System.Windows.Forms.TextBox();
            this.robustAnalysisMaxFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.robustAnalysisObservedDataGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.robustAnalysisParameterDataGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.abstractTextBox = new System.Windows.Forms.TextBox();
            this.robustToolTip = new System.Windows.Forms.ToolTip(this.components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.robustAnalysisObservedDataGrid)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.robustAnalysisParameterDataGrid)).BeginInit();
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
            // groupBox1
            // 
            groupBox1.Controls.Add(this.robustAnalysisWindowSizeTextBox);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(this.robustAnalysisSimulationTimeTextBox);
            groupBox1.Controls.Add(label2);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // robustAnalysisWindowSizeTextBox
            // 
            resources.ApplyResources(this.robustAnalysisWindowSizeTextBox, "robustAnalysisWindowSizeTextBox");
            this.robustAnalysisWindowSizeTextBox.Name = "robustAnalysisWindowSizeTextBox";
            // 
            // robustAnalysisSimulationTimeTextBox
            // 
            resources.ApplyResources(this.robustAnalysisSimulationTimeTextBox, "robustAnalysisSimulationTimeTextBox");
            this.robustAnalysisSimulationTimeTextBox.Name = "robustAnalysisSimulationTimeTextBox";
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
            // groupBox2
            // 
            groupBox2.Controls.Add(this.robustAnalysisSampleNumberTextBox);
            groupBox2.Controls.Add(this.robustAnalysisRandomCheckBox);
            groupBox2.Controls.Add(this.robustAnalysisMatrixCheckBox);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label3);
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // robustAnalysisSampleNumberTextBox
            // 
            resources.ApplyResources(this.robustAnalysisSampleNumberTextBox, "robustAnalysisSampleNumberTextBox");
            this.robustAnalysisSampleNumberTextBox.Name = "robustAnalysisSampleNumberTextBox";
            // 
            // robustAnalysisRandomCheckBox
            // 
            resources.ApplyResources(this.robustAnalysisRandomCheckBox, "robustAnalysisRandomCheckBox");
            this.robustAnalysisRandomCheckBox.Name = "robustAnalysisRandomCheckBox";
            this.robustAnalysisRandomCheckBox.UseVisualStyleBackColor = true;
            this.robustAnalysisRandomCheckBox.CheckedChanged += new System.EventHandler(this.ChangeRARandomCheck);
            // 
            // robustAnalysisMatrixCheckBox
            // 
            resources.ApplyResources(this.robustAnalysisMatrixCheckBox, "robustAnalysisMatrixCheckBox");
            this.robustAnalysisMatrixCheckBox.Name = "robustAnalysisMatrixCheckBox";
            this.robustAnalysisMatrixCheckBox.UseVisualStyleBackColor = true;
            this.robustAnalysisMatrixCheckBox.CheckedChanged += new System.EventHandler(this.ChangeRAMatrixCheck);
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.robustAnalysisMinFrequencyTextBox);
            groupBox3.Controls.Add(this.robustAnalysisMaxSampleTextBox);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(this.robustAnalysisMaxFrequencyTextBox);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(label6);
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // robustAnalysisMinFrequencyTextBox
            // 
            resources.ApplyResources(this.robustAnalysisMinFrequencyTextBox, "robustAnalysisMinFrequencyTextBox");
            this.robustAnalysisMinFrequencyTextBox.Name = "robustAnalysisMinFrequencyTextBox";
            // 
            // robustAnalysisMaxSampleTextBox
            // 
            resources.ApplyResources(this.robustAnalysisMaxSampleTextBox, "robustAnalysisMaxSampleTextBox");
            this.robustAnalysisMaxSampleTextBox.Name = "robustAnalysisMaxSampleTextBox";
            // 
            // robustAnalysisMaxFrequencyTextBox
            // 
            resources.ApplyResources(this.robustAnalysisMaxFrequencyTextBox, "robustAnalysisMaxFrequencyTextBox");
            this.robustAnalysisMaxFrequencyTextBox.Name = "robustAnalysisMaxFrequencyTextBox";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(this.robustAnalysisObservedDataGrid);
            resources.ApplyResources(groupBox5, "groupBox5");
            groupBox5.Name = "groupBox5";
            groupBox5.TabStop = false;
            // 
            // robustAnalysisObservedDataGrid
            // 
            this.robustAnalysisObservedDataGrid.AllowUserToAddRows = false;
            this.robustAnalysisObservedDataGrid.AllowUserToDeleteRows = false;
            this.robustAnalysisObservedDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.robustAnalysisObservedDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.robustAnalysisObservedDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            resources.ApplyResources(this.robustAnalysisObservedDataGrid, "robustAnalysisObservedDataGrid");
            this.robustAnalysisObservedDataGrid.Name = "robustAnalysisObservedDataGrid";
            this.robustAnalysisObservedDataGrid.RowHeadersVisible = false;
            this.robustAnalysisObservedDataGrid.RowTemplate.Height = 21;
            // 
            // dataGridViewTextBoxColumn5
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
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
            // button1
            // 
            button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(button2, "button2");
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            resources.ApplyResources(button3, "button3");
            button3.Name = "button3";
            button3.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.robustAnalysisParameterDataGrid);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // robustAnalysisParameterDataGrid
            // 
            this.robustAnalysisParameterDataGrid.AllowUserToAddRows = false;
            this.robustAnalysisParameterDataGrid.AllowUserToDeleteRows = false;
            this.robustAnalysisParameterDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.robustAnalysisParameterDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.robustAnalysisParameterDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            resources.ApplyResources(this.robustAnalysisParameterDataGrid, "robustAnalysisParameterDataGrid");
            this.robustAnalysisParameterDataGrid.Name = "robustAnalysisParameterDataGrid";
            this.robustAnalysisParameterDataGrid.RowHeadersVisible = false;
            this.robustAnalysisParameterDataGrid.RowTemplate.Height = 21;
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
            // abstractTextBox
            // 
            this.abstractTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.abstractTextBox, "abstractTextBox");
            this.abstractTextBox.Name = "abstractTextBox";
            this.abstractTextBox.ReadOnly = true;
            this.abstractTextBox.TabStop = false;
            // 
            // RobustAnalysisSettingDialog
            // 
            this.AcceptButton = button2;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = button1;
            this.Controls.Add(this.abstractTextBox);
            this.Controls.Add(button3);
            this.Controls.Add(button2);
            this.Controls.Add(button1);
            this.Controls.Add(groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(groupBox1);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox3);
            this.Name = "RobustAnalysisSettingDialog";
            this.Load += new System.EventHandler(this.FormLoad);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.robustAnalysisObservedDataGrid)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.robustAnalysisParameterDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox robustAnalysisWindowSizeTextBox;
        private System.Windows.Forms.TextBox robustAnalysisSimulationTimeTextBox;
        private System.Windows.Forms.TextBox robustAnalysisSampleNumberTextBox;
        private System.Windows.Forms.CheckBox robustAnalysisRandomCheckBox;
        private System.Windows.Forms.CheckBox robustAnalysisMatrixCheckBox;
        private System.Windows.Forms.TextBox robustAnalysisMinFrequencyTextBox;
        private System.Windows.Forms.TextBox robustAnalysisMaxFrequencyTextBox;
        private System.Windows.Forms.TextBox robustAnalysisMaxSampleTextBox;
        private System.Windows.Forms.DataGridView robustAnalysisParameterDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridView robustAnalysisObservedDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.TextBox abstractTextBox;
        private System.Windows.Forms.ToolTip robustToolTip;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}