namespace Ecell.IDE.Plugins.Simulation
{
    partial class SimulationConfigurationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationConfigurationDialog));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.stepperPage = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.SSDeleteStepperButton = new System.Windows.Forms.Button();
            this.SSAddStepperButton = new System.Windows.Forms.Button();
            this.stepperListBox = new System.Windows.Forms.ListBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.Property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Get = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Set = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stepCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.loggingPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.freqByStepRadio = new System.Windows.Forms.RadioButton();
            this.freqBySecRadio = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.freqByStepTextBox = new System.Windows.Forms.TextBox();
            this.freqBySecTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.exceptionRadio = new System.Windows.Forms.RadioButton();
            this.overrideRadio = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.noLimitRadio = new System.Windows.Forms.RadioButton();
            this.maxSizeRadio = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.maxKbTextBox = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.InitVarDGV = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.InitProDGV = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.initialvalue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.modelCombo = new System.Windows.Forms.ComboBox();
            this.SSCreateButton = new System.Windows.Forms.Button();
            this.paramCombo = new System.Windows.Forms.ComboBox();
            this.SSDeleteButton = new System.Windows.Forms.Button();
            this.SSSetButton = new System.Windows.Forms.Button();
            this.SSApplyButton = new System.Windows.Forms.Button();
            this.SSCloseButton = new System.Windows.Forms.Button();
            this.configurationLabel = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.stepperPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.loggingPage.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InitVarDGV)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InitProDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.stepperPage);
            this.tabControl1.Controls.Add(this.loggingPage);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // stepperPage
            // 
            this.stepperPage.Controls.Add(this.label7);
            this.stepperPage.Controls.Add(this.SSDeleteStepperButton);
            this.stepperPage.Controls.Add(this.SSAddStepperButton);
            this.stepperPage.Controls.Add(this.stepperListBox);
            this.stepperPage.Controls.Add(this.dgv);
            this.stepperPage.Controls.Add(this.stepCombo);
            this.stepperPage.Controls.Add(this.label1);
            this.stepperPage.Controls.Add(this.label2);
            resources.ApplyResources(this.stepperPage, "stepperPage");
            this.stepperPage.Name = "stepperPage";
            this.stepperPage.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // SSDeleteStepperButton
            // 
            resources.ApplyResources(this.SSDeleteStepperButton, "SSDeleteStepperButton");
            this.SSDeleteStepperButton.Name = "SSDeleteStepperButton";
            this.SSDeleteStepperButton.UseVisualStyleBackColor = true;
            // 
            // SSAddStepperButton
            // 
            resources.ApplyResources(this.SSAddStepperButton, "SSAddStepperButton");
            this.SSAddStepperButton.Name = "SSAddStepperButton";
            this.SSAddStepperButton.UseVisualStyleBackColor = true;
            // 
            // stepperListBox
            // 
            this.stepperListBox.FormattingEnabled = true;
            resources.ApplyResources(this.stepperListBox, "stepperListBox");
            this.stepperListBox.Name = "stepperListBox";
            this.stepperListBox.Sorted = true;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Property,
            this.Value,
            this.Get,
            this.Set});
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 21;
            // 
            // Property
            // 
            this.Property.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.Property, "Property");
            this.Property.Name = "Property";
            this.Property.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.FillWeight = 120F;
            resources.ApplyResources(this.Value, "Value");
            this.Value.Name = "Value";
            // 
            // Get
            // 
            this.Get.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Get.FillWeight = 10F;
            resources.ApplyResources(this.Get, "Get");
            this.Get.Name = "Get";
            this.Get.ReadOnly = true;
            // 
            // Set
            // 
            this.Set.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Set.FillWeight = 10F;
            resources.ApplyResources(this.Set, "Set");
            this.Set.Name = "Set";
            this.Set.ReadOnly = true;
            // 
            // stepCombo
            // 
            this.stepCombo.FormattingEnabled = true;
            resources.ApplyResources(this.stepCombo, "stepCombo");
            this.stepCombo.Name = "stepCombo";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // loggingPage
            // 
            this.loggingPage.Controls.Add(this.tableLayoutPanel7);
            resources.ApplyResources(this.loggingPage, "loggingPage");
            this.loggingPage.Name = "loggingPage";
            this.loggingPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
            this.tableLayoutPanel7.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel8);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel8
            // 
            resources.ApplyResources(this.tableLayoutPanel8, "tableLayoutPanel8");
            this.tableLayoutPanel8.Controls.Add(this.freqByStepRadio, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.freqBySecRadio, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.label4, 2, 1);
            this.tableLayoutPanel8.Controls.Add(this.freqByStepTextBox, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.freqBySecTextBox, 1, 1);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            // 
            // freqByStepRadio
            // 
            resources.ApplyResources(this.freqByStepRadio, "freqByStepRadio");
            this.freqByStepRadio.Checked = true;
            this.freqByStepRadio.Name = "freqByStepRadio";
            this.freqByStepRadio.TabStop = true;
            this.freqByStepRadio.UseVisualStyleBackColor = true;
            this.freqByStepRadio.CheckedChanged += new System.EventHandler(this.FreqCheckChanged);
            // 
            // freqBySecRadio
            // 
            resources.ApplyResources(this.freqBySecRadio, "freqBySecRadio");
            this.freqBySecRadio.Name = "freqBySecRadio";
            this.freqBySecRadio.UseVisualStyleBackColor = true;
            this.freqBySecRadio.CheckedChanged += new System.EventHandler(this.FreqCheckChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // freqByStepTextBox
            // 
            resources.ApplyResources(this.freqByStepTextBox, "freqByStepTextBox");
            this.freqByStepTextBox.Name = "freqByStepTextBox";
            // 
            // freqBySecTextBox
            // 
            resources.ApplyResources(this.freqBySecTextBox, "freqBySecTextBox");
            this.freqBySecTextBox.Name = "freqBySecTextBox";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel9);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel9
            // 
            resources.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
            this.tableLayoutPanel9.Controls.Add(this.exceptionRadio, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.overrideRadio, 0, 1);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            // 
            // exceptionRadio
            // 
            resources.ApplyResources(this.exceptionRadio, "exceptionRadio");
            this.exceptionRadio.Checked = true;
            this.exceptionRadio.Name = "exceptionRadio";
            this.exceptionRadio.TabStop = true;
            this.exceptionRadio.UseVisualStyleBackColor = true;
            this.exceptionRadio.CheckedChanged += new System.EventHandler(this.ActionCheckChanged);
            // 
            // overrideRadio
            // 
            resources.ApplyResources(this.overrideRadio, "overrideRadio");
            this.overrideRadio.Name = "overrideRadio";
            this.overrideRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel10);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // tableLayoutPanel10
            // 
            resources.ApplyResources(this.tableLayoutPanel10, "tableLayoutPanel10");
            this.tableLayoutPanel10.Controls.Add(this.noLimitRadio, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.maxSizeRadio, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel10.Controls.Add(this.maxKbTextBox, 1, 1);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            // 
            // noLimitRadio
            // 
            resources.ApplyResources(this.noLimitRadio, "noLimitRadio");
            this.noLimitRadio.Checked = true;
            this.noLimitRadio.Name = "noLimitRadio";
            this.noLimitRadio.TabStop = true;
            this.noLimitRadio.UseVisualStyleBackColor = true;
            this.noLimitRadio.CheckedChanged += new System.EventHandler(this.SpaceCheckChanged);
            // 
            // maxSizeRadio
            // 
            resources.ApplyResources(this.maxSizeRadio, "maxSizeRadio");
            this.maxSizeRadio.Name = "maxSizeRadio";
            this.maxSizeRadio.UseVisualStyleBackColor = true;
            this.maxSizeRadio.CheckedChanged += new System.EventHandler(this.SpaceCheckChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // maxKbTextBox
            // 
            resources.ApplyResources(this.maxKbTextBox, "maxKbTextBox");
            this.maxKbTextBox.Name = "maxKbTextBox";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl2);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            resources.ApplyResources(this.tabControl2, "tabControl2");
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.InitVarDGV);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // InitVarDGV
            // 
            this.InitVarDGV.AllowUserToAddRows = false;
            resources.ApplyResources(this.InitVarDGV, "InitVarDGV");
            this.InitVarDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.InitVarDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InitVarDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.InitVarDGV.Name = "InitVarDGV";
            this.InitVarDGV.RowHeadersVisible = false;
            this.InitVarDGV.RowTemplate.Height = 21;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.InitProDGV);
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // InitProDGV
            // 
            this.InitProDGV.AllowUserToAddRows = false;
            resources.ApplyResources(this.InitProDGV, "InitProDGV");
            this.InitProDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.InitProDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InitProDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.initialvalue});
            this.InitProDGV.Name = "InitProDGV";
            this.InitProDGV.RowHeadersVisible = false;
            this.InitProDGV.RowTemplate.Height = 21;
            // 
            // id
            // 
            this.id.FillWeight = 200F;
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            // 
            // initialvalue
            // 
            this.initialvalue.FillWeight = 101.5228F;
            resources.ApplyResources(this.initialvalue, "initialvalue");
            this.initialvalue.Name = "initialvalue";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // modelCombo
            // 
            this.modelCombo.FormattingEnabled = true;
            resources.ApplyResources(this.modelCombo, "modelCombo");
            this.modelCombo.Name = "modelCombo";
            // 
            // SSCreateButton
            // 
            resources.ApplyResources(this.SSCreateButton, "SSCreateButton");
            this.SSCreateButton.Name = "SSCreateButton";
            this.SSCreateButton.UseVisualStyleBackColor = true;
            // 
            // paramCombo
            // 
            this.paramCombo.FormattingEnabled = true;
            resources.ApplyResources(this.paramCombo, "paramCombo");
            this.paramCombo.Name = "paramCombo";
            // 
            // SSDeleteButton
            // 
            resources.ApplyResources(this.SSDeleteButton, "SSDeleteButton");
            this.SSDeleteButton.Name = "SSDeleteButton";
            this.SSDeleteButton.UseVisualStyleBackColor = true;
            // 
            // SSSetButton
            // 
            resources.ApplyResources(this.SSSetButton, "SSSetButton");
            this.SSSetButton.Name = "SSSetButton";
            this.SSSetButton.UseVisualStyleBackColor = true;
            // 
            // SSApplyButton
            // 
            resources.ApplyResources(this.SSApplyButton, "SSApplyButton");
            this.SSApplyButton.Name = "SSApplyButton";
            this.SSApplyButton.UseVisualStyleBackColor = true;
            // 
            // SSCloseButton
            // 
            resources.ApplyResources(this.SSCloseButton, "SSCloseButton");
            this.SSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SSCloseButton.Name = "SSCloseButton";
            this.SSCloseButton.UseVisualStyleBackColor = true;
            // 
            // configurationLabel
            // 
            resources.ApplyResources(this.configurationLabel, "configurationLabel");
            this.configurationLabel.Name = "configurationLabel";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 200F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 101.5228F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // SimulationConfigurationDialog
            // 
            this.AcceptButton = this.SSApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.SSCloseButton;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.SSApplyButton);
            this.Controls.Add(this.SSSetButton);
            this.Controls.Add(this.SSCloseButton);
            this.Controls.Add(this.SSDeleteButton);
            this.Controls.Add(this.SSCreateButton);
            this.Controls.Add(this.configurationLabel);
            this.Controls.Add(this.paramCombo);
            this.Controls.Add(this.modelCombo);
            this.Controls.Add(this.label6);
            this.Name = "SimulationConfigurationDialog";
            this.Shown += new System.EventHandler(this.ShowSimulationSetupWin);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SetupKeyPress);
            this.tabControl1.ResumeLayout(false);
            this.stepperPage.ResumeLayout(false);
            this.stepperPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.loggingPage.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InitVarDGV)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InitProDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// TabPage to set the properties of stepper.
        /// </summary>
        private System.Windows.Forms.TabPage stepperPage;
        /// <summary>
        /// TabControl of simulation properties.
        /// </summary>
        public System.Windows.Forms.TabControl tabControl1;
        /// <summary>
        /// TabPage to set logging properties.
        /// </summary>
        public System.Windows.Forms.TabPage loggingPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn initialvalue;
        private System.Windows.Forms.Button SSCreateButton;
        private System.Windows.Forms.Button SSDeleteButton;
        private System.Windows.Forms.Button SSSetButton;
        private System.Windows.Forms.ComboBox stepCombo;
        private System.Windows.Forms.ComboBox modelCombo;
        private System.Windows.Forms.ListBox stepperListBox;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button SSApplyButton;
        private System.Windows.Forms.Button SSCloseButton;
        private System.Windows.Forms.Button SSAddStepperButton;
        private System.Windows.Forms.Button SSDeleteStepperButton;
        private System.Windows.Forms.RadioButton freqByStepRadio;
        private System.Windows.Forms.RadioButton freqBySecRadio;
        private System.Windows.Forms.RadioButton exceptionRadio;
        private System.Windows.Forms.RadioButton overrideRadio;
        private System.Windows.Forms.RadioButton noLimitRadio;
        private System.Windows.Forms.RadioButton maxSizeRadio;
        private System.Windows.Forms.TextBox freqByStepTextBox;
        private System.Windows.Forms.TextBox freqBySecTextBox;
        private System.Windows.Forms.TextBox maxKbTextBox;
        private System.Windows.Forms.DataGridView InitProDGV;
        private System.Windows.Forms.DataGridView InitVarDGV;
        private System.Windows.Forms.ComboBox paramCombo;
        private System.Windows.Forms.Label configurationLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Property;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Get;
        private System.Windows.Forms.DataGridViewTextBoxColumn Set;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}