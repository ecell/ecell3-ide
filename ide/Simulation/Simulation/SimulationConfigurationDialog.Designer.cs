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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TabPage variableInitialConditionsPage;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationConfigurationDialog));
            System.Windows.Forms.BindingSource variableInitialConditionsBindingSource;
            System.Windows.Forms.TabPage processInitialConditionsPage;
            System.Windows.Forms.TabPage initialConditionPage;
            System.Windows.Forms.TabPage stepperDefinitionsPage;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Button button2;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            this.InitVarDGV = new System.Windows.Forms.DataGridView();
            this.keyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perTypeInitialConditionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.InitProDGV = new System.Windows.Forms.DataGridView();
            this.keyDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processInitialConditionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.keyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertiesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.propertiesBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.steppersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.stepCombo = new System.Windows.Forms.ComboBox();
            this.stepperListBox = new System.Windows.Forms.ListBox();
            this.modelCombo = new System.Windows.Forms.ComboBox();
            this.perModelSimulationParameterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.m_simParamSets = new System.Windows.Forms.BindingSource(this.components);
            this.SSCreateButton = new System.Windows.Forms.Button();
            this.paramCombo = new System.Windows.Forms.ComboBox();
            this.SSDeleteButton = new System.Windows.Forms.Button();
            this.SSApplyButton = new System.Windows.Forms.Button();
            this.SSCloseButton = new System.Windows.Forms.Button();
            this.configurationLabel = new System.Windows.Forms.Label();
            this.loggingPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.overrideRadio = new System.Windows.Forms.RadioButton();
            this.exceptionRadio = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.noLimitRadio = new System.Windows.Forms.RadioButton();
            this.maxKbTextBox = new System.Windows.Forms.TextBox();
            this.maxSizeRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.freqBySecRadio = new System.Windows.Forms.RadioButton();
            this.freqBySecTextBox = new System.Windows.Forms.TextBox();
            this.freqByStepRadio = new System.Windows.Forms.RadioButton();
            this.freqByStepTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            variableInitialConditionsPage = new System.Windows.Forms.TabPage();
            variableInitialConditionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            processInitialConditionsPage = new System.Windows.Forms.TabPage();
            initialConditionPage = new System.Windows.Forms.TabPage();
            stepperDefinitionsPage = new System.Windows.Forms.TabPage();
            label2 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            variableInitialConditionsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InitVarDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(variableInitialConditionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perTypeInitialConditionsBindingSource)).BeginInit();
            processInitialConditionsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InitProDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processInitialConditionsBindingSource)).BeginInit();
            initialConditionPage.SuspendLayout();
            this.tabControl2.SuspendLayout();
            stepperDefinitionsPage.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.steppersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perModelSimulationParameterBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_simParamSets)).BeginInit();
            this.loggingPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // variableInitialConditionsPage
            // 
            variableInitialConditionsPage.AccessibleDescription = null;
            variableInitialConditionsPage.AccessibleName = null;
            resources.ApplyResources(variableInitialConditionsPage, "variableInitialConditionsPage");
            variableInitialConditionsPage.BackgroundImage = null;
            variableInitialConditionsPage.Controls.Add(this.InitVarDGV);
            variableInitialConditionsPage.Font = null;
            variableInitialConditionsPage.Name = "variableInitialConditionsPage";
            variableInitialConditionsPage.UseVisualStyleBackColor = true;
            // 
            // InitVarDGV
            // 
            this.InitVarDGV.AccessibleDescription = null;
            this.InitVarDGV.AccessibleName = null;
            this.InitVarDGV.AllowUserToAddRows = false;
            resources.ApplyResources(this.InitVarDGV, "InitVarDGV");
            this.InitVarDGV.AutoGenerateColumns = false;
            this.InitVarDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.InitVarDGV.BackgroundImage = null;
            this.InitVarDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InitVarDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn1,
            this.valueDataGridViewTextBoxColumn1});
            this.InitVarDGV.DataSource = variableInitialConditionsBindingSource;
            this.InitVarDGV.Font = null;
            this.InitVarDGV.Name = "InitVarDGV";
            this.InitVarDGV.RowHeadersVisible = false;
            this.InitVarDGV.RowTemplate.Height = 21;
            // 
            // keyDataGridViewTextBoxColumn1
            // 
            this.keyDataGridViewTextBoxColumn1.DataPropertyName = "Key";
            resources.ApplyResources(this.keyDataGridViewTextBoxColumn1, "keyDataGridViewTextBoxColumn1");
            this.keyDataGridViewTextBoxColumn1.Name = "keyDataGridViewTextBoxColumn1";
            this.keyDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn1
            // 
            this.valueDataGridViewTextBoxColumn1.DataPropertyName = "Value";
            resources.ApplyResources(this.valueDataGridViewTextBoxColumn1, "valueDataGridViewTextBoxColumn1");
            this.valueDataGridViewTextBoxColumn1.Name = "valueDataGridViewTextBoxColumn1";
            // 
            // variableInitialConditionsBindingSource
            // 
            variableInitialConditionsBindingSource.AllowNew = true;
            variableInitialConditionsBindingSource.DataMember = "VariableInitialConditions";
            variableInitialConditionsBindingSource.DataSource = this.perTypeInitialConditionsBindingSource;
            // 
            // perTypeInitialConditionsBindingSource
            // 
            this.perTypeInitialConditionsBindingSource.DataSource = typeof(Ecell.IDE.Plugins.Simulation.PerTypeInitialConditions);
            // 
            // processInitialConditionsPage
            // 
            processInitialConditionsPage.AccessibleDescription = null;
            processInitialConditionsPage.AccessibleName = null;
            resources.ApplyResources(processInitialConditionsPage, "processInitialConditionsPage");
            processInitialConditionsPage.BackgroundImage = null;
            processInitialConditionsPage.Controls.Add(this.InitProDGV);
            processInitialConditionsPage.Font = null;
            processInitialConditionsPage.Name = "processInitialConditionsPage";
            processInitialConditionsPage.UseVisualStyleBackColor = true;
            // 
            // InitProDGV
            // 
            this.InitProDGV.AccessibleDescription = null;
            this.InitProDGV.AccessibleName = null;
            this.InitProDGV.AllowUserToAddRows = false;
            resources.ApplyResources(this.InitProDGV, "InitProDGV");
            this.InitProDGV.AutoGenerateColumns = false;
            this.InitProDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.InitProDGV.BackgroundImage = null;
            this.InitProDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InitProDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn2,
            this.valueDataGridViewTextBoxColumn2});
            this.InitProDGV.DataSource = this.processInitialConditionsBindingSource;
            this.InitProDGV.Font = null;
            this.InitProDGV.Name = "InitProDGV";
            this.InitProDGV.RowHeadersVisible = false;
            this.InitProDGV.RowTemplate.Height = 21;
            // 
            // keyDataGridViewTextBoxColumn2
            // 
            this.keyDataGridViewTextBoxColumn2.DataPropertyName = "Key";
            resources.ApplyResources(this.keyDataGridViewTextBoxColumn2, "keyDataGridViewTextBoxColumn2");
            this.keyDataGridViewTextBoxColumn2.Name = "keyDataGridViewTextBoxColumn2";
            this.keyDataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn2
            // 
            this.valueDataGridViewTextBoxColumn2.DataPropertyName = "Value";
            resources.ApplyResources(this.valueDataGridViewTextBoxColumn2, "valueDataGridViewTextBoxColumn2");
            this.valueDataGridViewTextBoxColumn2.Name = "valueDataGridViewTextBoxColumn2";
            // 
            // processInitialConditionsBindingSource
            // 
            this.processInitialConditionsBindingSource.AllowNew = true;
            this.processInitialConditionsBindingSource.DataMember = "ProcessInitialConditions";
            this.processInitialConditionsBindingSource.DataSource = this.perTypeInitialConditionsBindingSource;
            // 
            // initialConditionPage
            // 
            initialConditionPage.AccessibleDescription = null;
            initialConditionPage.AccessibleName = null;
            resources.ApplyResources(initialConditionPage, "initialConditionPage");
            initialConditionPage.BackgroundImage = null;
            initialConditionPage.Controls.Add(this.tabControl2);
            initialConditionPage.Controls.Add(label6);
            initialConditionPage.Controls.Add(this.modelCombo);
            initialConditionPage.Font = null;
            initialConditionPage.Name = "initialConditionPage";
            initialConditionPage.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.AccessibleDescription = null;
            this.tabControl2.AccessibleName = null;
            resources.ApplyResources(this.tabControl2, "tabControl2");
            this.tabControl2.BackgroundImage = null;
            this.tabControl2.Controls.Add(variableInitialConditionsPage);
            this.tabControl2.Controls.Add(processInitialConditionsPage);
            this.tabControl2.Controls.Add(stepperDefinitionsPage);
            this.tabControl2.Font = null;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            // 
            // stepperDefinitionsPage
            // 
            stepperDefinitionsPage.AccessibleDescription = null;
            stepperDefinitionsPage.AccessibleName = null;
            resources.ApplyResources(stepperDefinitionsPage, "stepperDefinitionsPage");
            stepperDefinitionsPage.BackgroundImage = null;
            stepperDefinitionsPage.Controls.Add(label2);
            stepperDefinitionsPage.Controls.Add(button1);
            stepperDefinitionsPage.Controls.Add(this.panel1);
            stepperDefinitionsPage.Controls.Add(button2);
            stepperDefinitionsPage.Controls.Add(this.stepperListBox);
            stepperDefinitionsPage.Font = null;
            stepperDefinitionsPage.Name = "stepperDefinitionsPage";
            stepperDefinitionsPage.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AccessibleDescription = null;
            label2.AccessibleName = null;
            resources.ApplyResources(label2, "label2");
            label2.Font = null;
            label2.Name = "label2";
            // 
            // button1
            // 
            button1.AccessibleDescription = null;
            button1.AccessibleName = null;
            resources.ApplyResources(button1, "button1");
            button1.BackgroundImage = null;
            button1.Font = null;
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.AddStepperClick);
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(label1);
            this.panel1.Controls.Add(this.dgv);
            this.panel1.Controls.Add(label7);
            this.panel1.Controls.Add(this.stepCombo);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            label1.AccessibleDescription = null;
            label1.AccessibleName = null;
            resources.ApplyResources(label1, "label1");
            label1.Font = null;
            label1.Name = "label1";
            // 
            // dgv
            // 
            this.dgv.AccessibleDescription = null;
            this.dgv.AccessibleName = null;
            this.dgv.AllowUserToAddRows = false;
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.AutoGenerateColumns = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundImage = null;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn});
            this.dgv.DataSource = this.propertiesBindingSource;
            this.dgv.Font = null;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 21;
            // 
            // keyDataGridViewTextBoxColumn
            // 
            this.keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
            resources.ApplyResources(this.keyDataGridViewTextBoxColumn, "keyDataGridViewTextBoxColumn");
            this.keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
            this.keyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            resources.ApplyResources(this.valueDataGridViewTextBoxColumn, "valueDataGridViewTextBoxColumn");
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            // 
            // propertiesBindingSource
            // 
            this.propertiesBindingSource.DataSource = this.propertiesBindingSource1;
            // 
            // propertiesBindingSource1
            // 
            this.propertiesBindingSource1.DataMember = "Properties";
            this.propertiesBindingSource1.DataSource = this.steppersBindingSource;
            // 
            // steppersBindingSource
            // 
            this.steppersBindingSource.DataSource = typeof(Ecell.IDE.Plugins.Simulation.StepperConfiguration);
            // 
            // label7
            // 
            label7.AccessibleDescription = null;
            label7.AccessibleName = null;
            resources.ApplyResources(label7, "label7");
            label7.Font = null;
            label7.Name = "label7";
            // 
            // stepCombo
            // 
            this.stepCombo.AccessibleDescription = null;
            this.stepCombo.AccessibleName = null;
            resources.ApplyResources(this.stepCombo, "stepCombo");
            this.stepCombo.BackgroundImage = null;
            this.stepCombo.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.steppersBindingSource, "ClassName", true));
            this.stepCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stepCombo.Font = null;
            this.stepCombo.FormattingEnabled = true;
            this.stepCombo.Name = "stepCombo";
            this.stepCombo.SelectedValueChanged += new System.EventHandler(this.stepCombo_SelectedValueChanged);
            // 
            // button2
            // 
            button2.AccessibleDescription = null;
            button2.AccessibleName = null;
            resources.ApplyResources(button2, "button2");
            button2.BackgroundImage = null;
            button2.Font = null;
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(this.DeleteStepperClick);
            // 
            // stepperListBox
            // 
            this.stepperListBox.AccessibleDescription = null;
            this.stepperListBox.AccessibleName = null;
            resources.ApplyResources(this.stepperListBox, "stepperListBox");
            this.stepperListBox.BackgroundImage = null;
            this.stepperListBox.DataSource = this.steppersBindingSource;
            this.stepperListBox.DisplayMember = "Name";
            this.stepperListBox.Font = null;
            this.stepperListBox.FormattingEnabled = true;
            this.stepperListBox.Name = "stepperListBox";
            this.stepperListBox.Sorted = true;
            // 
            // label6
            // 
            label6.AccessibleDescription = null;
            label6.AccessibleName = null;
            resources.ApplyResources(label6, "label6");
            label6.Font = null;
            label6.Name = "label6";
            // 
            // modelCombo
            // 
            this.modelCombo.AccessibleDescription = null;
            this.modelCombo.AccessibleName = null;
            resources.ApplyResources(this.modelCombo, "modelCombo");
            this.modelCombo.BackgroundImage = null;
            this.modelCombo.DataSource = this.perModelSimulationParameterBindingSource;
            this.modelCombo.DisplayMember = "ModelID";
            this.modelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modelCombo.Font = null;
            this.modelCombo.FormattingEnabled = true;
            this.modelCombo.Name = "modelCombo";
            // 
            // perModelSimulationParameterBindingSource
            // 
            this.perModelSimulationParameterBindingSource.DataMember = "PerModelSimulationParameters";
            this.perModelSimulationParameterBindingSource.DataSource = this.m_simParamSets;
            // 
            // m_simParamSets
            // 
            this.m_simParamSets.AllowNew = true;
            this.m_simParamSets.DataSource = typeof(Ecell.IDE.Plugins.Simulation.SimulationParameterSet);
            this.m_simParamSets.CurrentChanged += new System.EventHandler(this.m_simParamSets_CurrentChanged);
            // 
            // label5
            // 
            label5.AccessibleDescription = null;
            label5.AccessibleName = null;
            resources.ApplyResources(label5, "label5");
            label5.Font = null;
            label5.Name = "label5";
            // 
            // SSCreateButton
            // 
            this.SSCreateButton.AccessibleDescription = null;
            this.SSCreateButton.AccessibleName = null;
            resources.ApplyResources(this.SSCreateButton, "SSCreateButton");
            this.SSCreateButton.BackgroundImage = null;
            this.SSCreateButton.Font = null;
            this.SSCreateButton.Name = "SSCreateButton";
            this.SSCreateButton.UseVisualStyleBackColor = true;
            this.SSCreateButton.Click += new System.EventHandler(this.NewButtonClick);
            // 
            // paramCombo
            // 
            this.paramCombo.AccessibleDescription = null;
            this.paramCombo.AccessibleName = null;
            resources.ApplyResources(this.paramCombo, "paramCombo");
            this.paramCombo.BackgroundImage = null;
            this.paramCombo.DataSource = this.m_simParamSets;
            this.paramCombo.DisplayMember = "Name";
            this.paramCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paramCombo.Font = null;
            this.paramCombo.FormattingEnabled = true;
            this.paramCombo.Name = "paramCombo";
            // 
            // SSDeleteButton
            // 
            this.SSDeleteButton.AccessibleDescription = null;
            this.SSDeleteButton.AccessibleName = null;
            resources.ApplyResources(this.SSDeleteButton, "SSDeleteButton");
            this.SSDeleteButton.BackgroundImage = null;
            this.SSDeleteButton.Font = null;
            this.SSDeleteButton.Name = "SSDeleteButton";
            this.SSDeleteButton.UseVisualStyleBackColor = true;
            this.SSDeleteButton.Click += new System.EventHandler(this.DeleteButtonClick);
            // 
            // SSApplyButton
            // 
            this.SSApplyButton.AccessibleDescription = null;
            this.SSApplyButton.AccessibleName = null;
            resources.ApplyResources(this.SSApplyButton, "SSApplyButton");
            this.SSApplyButton.BackgroundImage = null;
            this.SSApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SSApplyButton.Font = null;
            this.SSApplyButton.Name = "SSApplyButton";
            this.SSApplyButton.UseVisualStyleBackColor = true;
            // 
            // SSCloseButton
            // 
            this.SSCloseButton.AccessibleDescription = null;
            this.SSCloseButton.AccessibleName = null;
            resources.ApplyResources(this.SSCloseButton, "SSCloseButton");
            this.SSCloseButton.BackgroundImage = null;
            this.SSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SSCloseButton.Font = null;
            this.SSCloseButton.Name = "SSCloseButton";
            this.SSCloseButton.UseVisualStyleBackColor = true;
            // 
            // configurationLabel
            // 
            this.configurationLabel.AccessibleDescription = null;
            this.configurationLabel.AccessibleName = null;
            resources.ApplyResources(this.configurationLabel, "configurationLabel");
            this.configurationLabel.Font = null;
            this.configurationLabel.Name = "configurationLabel";
            // 
            // loggingPage
            // 
            this.loggingPage.AccessibleDescription = null;
            this.loggingPage.AccessibleName = null;
            resources.ApplyResources(this.loggingPage, "loggingPage");
            this.loggingPage.BackgroundImage = null;
            this.loggingPage.Controls.Add(this.groupBox2);
            this.loggingPage.Controls.Add(this.groupBox3);
            this.loggingPage.Controls.Add(this.groupBox1);
            this.loggingPage.Font = null;
            this.loggingPage.Name = "loggingPage";
            this.loggingPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.overrideRadio);
            this.groupBox2.Controls.Add(this.exceptionRadio);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // overrideRadio
            // 
            this.overrideRadio.AccessibleDescription = null;
            this.overrideRadio.AccessibleName = null;
            resources.ApplyResources(this.overrideRadio, "overrideRadio");
            this.overrideRadio.BackgroundImage = null;
            this.overrideRadio.Font = null;
            this.overrideRadio.Name = "overrideRadio";
            this.overrideRadio.UseVisualStyleBackColor = true;
            this.overrideRadio.CheckedChanged += new System.EventHandler(this.overrideRadio_CheckedChanged);
            // 
            // exceptionRadio
            // 
            this.exceptionRadio.AccessibleDescription = null;
            this.exceptionRadio.AccessibleName = null;
            resources.ApplyResources(this.exceptionRadio, "exceptionRadio");
            this.exceptionRadio.BackgroundImage = null;
            this.exceptionRadio.Checked = true;
            this.exceptionRadio.Font = null;
            this.exceptionRadio.Name = "exceptionRadio";
            this.exceptionRadio.TabStop = true;
            this.exceptionRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleDescription = null;
            this.groupBox3.AccessibleName = null;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackgroundImage = null;
            this.groupBox3.Controls.Add(label5);
            this.groupBox3.Controls.Add(this.noLimitRadio);
            this.groupBox3.Controls.Add(this.maxKbTextBox);
            this.groupBox3.Controls.Add(this.maxSizeRadio);
            this.groupBox3.Font = null;
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // noLimitRadio
            // 
            this.noLimitRadio.AccessibleDescription = null;
            this.noLimitRadio.AccessibleName = null;
            resources.ApplyResources(this.noLimitRadio, "noLimitRadio");
            this.noLimitRadio.BackgroundImage = null;
            this.noLimitRadio.Checked = true;
            this.noLimitRadio.Font = null;
            this.noLimitRadio.Name = "noLimitRadio";
            this.noLimitRadio.TabStop = true;
            this.noLimitRadio.UseVisualStyleBackColor = true;
            this.noLimitRadio.CheckedChanged += new System.EventHandler(this.noLimitRadio_CheckedChanged);
            // 
            // maxKbTextBox
            // 
            this.maxKbTextBox.AccessibleDescription = null;
            this.maxKbTextBox.AccessibleName = null;
            resources.ApplyResources(this.maxKbTextBox, "maxKbTextBox");
            this.maxKbTextBox.BackgroundImage = null;
            this.maxKbTextBox.Font = null;
            this.maxKbTextBox.Name = "maxKbTextBox";
            // 
            // maxSizeRadio
            // 
            this.maxSizeRadio.AccessibleDescription = null;
            this.maxSizeRadio.AccessibleName = null;
            resources.ApplyResources(this.maxSizeRadio, "maxSizeRadio");
            this.maxSizeRadio.BackgroundImage = null;
            this.maxSizeRadio.Font = null;
            this.maxSizeRadio.Name = "maxSizeRadio";
            this.maxSizeRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.freqBySecRadio);
            this.groupBox1.Controls.Add(this.freqBySecTextBox);
            this.groupBox1.Controls.Add(this.freqByStepRadio);
            this.groupBox1.Controls.Add(this.freqByStepTextBox);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // freqBySecRadio
            // 
            this.freqBySecRadio.AccessibleDescription = null;
            this.freqBySecRadio.AccessibleName = null;
            resources.ApplyResources(this.freqBySecRadio, "freqBySecRadio");
            this.freqBySecRadio.BackgroundImage = null;
            this.freqBySecRadio.Font = null;
            this.freqBySecRadio.Name = "freqBySecRadio";
            this.freqBySecRadio.UseVisualStyleBackColor = true;
            // 
            // freqBySecTextBox
            // 
            this.freqBySecTextBox.AccessibleDescription = null;
            this.freqBySecTextBox.AccessibleName = null;
            resources.ApplyResources(this.freqBySecTextBox, "freqBySecTextBox");
            this.freqBySecTextBox.BackgroundImage = null;
            this.freqBySecTextBox.Font = null;
            this.freqBySecTextBox.Name = "freqBySecTextBox";
            this.freqBySecTextBox.Validated += new System.EventHandler(this.freqBySecTextBox_Validated);
            this.freqBySecTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.freqBySecTextBox_Validating);
            this.freqBySecTextBox.TextChanged += new System.EventHandler(this.freqBySecTextBox_TextChanged);
            // 
            // freqByStepRadio
            // 
            this.freqByStepRadio.AccessibleDescription = null;
            this.freqByStepRadio.AccessibleName = null;
            resources.ApplyResources(this.freqByStepRadio, "freqByStepRadio");
            this.freqByStepRadio.BackgroundImage = null;
            this.freqByStepRadio.Checked = true;
            this.freqByStepRadio.Font = null;
            this.freqByStepRadio.Name = "freqByStepRadio";
            this.freqByStepRadio.TabStop = true;
            this.freqByStepRadio.UseVisualStyleBackColor = true;
            this.freqByStepRadio.CheckedChanged += new System.EventHandler(this.freqByStepRadio_CheckedChanged);
            // 
            // freqByStepTextBox
            // 
            this.freqByStepTextBox.AccessibleDescription = null;
            this.freqByStepTextBox.AccessibleName = null;
            resources.ApplyResources(this.freqByStepTextBox, "freqByStepTextBox");
            this.freqByStepTextBox.BackgroundImage = null;
            this.freqByStepTextBox.Font = null;
            this.freqByStepTextBox.Name = "freqByStepTextBox";
            this.freqByStepTextBox.Validated += new System.EventHandler(this.freqByStepTextBox_Validated);
            this.freqByStepTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.freqByStepTextBox_Validating);
            this.freqByStepTextBox.TextChanged += new System.EventHandler(this.freqByStepTextBox_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleDescription = null;
            this.tabControl1.AccessibleName = null;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.BackgroundImage = null;
            this.tabControl1.Controls.Add(initialConditionPage);
            this.tabControl1.Controls.Add(this.loggingPage);
            this.tabControl1.Font = null;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // SimulationConfigurationDialog
            // 
            this.AcceptButton = this.SSApplyButton;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.SSCloseButton;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.SSApplyButton);
            this.Controls.Add(this.SSCloseButton);
            this.Controls.Add(this.SSDeleteButton);
            this.Controls.Add(this.SSCreateButton);
            this.Controls.Add(this.configurationLabel);
            this.Controls.Add(this.paramCombo);
            this.Font = null;
            this.Name = "SimulationConfigurationDialog";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SetupKeyPress);
            variableInitialConditionsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InitVarDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(variableInitialConditionsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perTypeInitialConditionsBindingSource)).EndInit();
            processInitialConditionsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InitProDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processInitialConditionsBindingSource)).EndInit();
            initialConditionPage.ResumeLayout(false);
            initialConditionPage.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            stepperDefinitionsPage.ResumeLayout(false);
            stepperDefinitionsPage.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.steppersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perModelSimulationParameterBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_simParamSets)).EndInit();
            this.loggingPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.Button SSCreateButton;
        private System.Windows.Forms.Button SSDeleteButton;
        private System.Windows.Forms.ComboBox modelCombo;
        private System.Windows.Forms.Button SSApplyButton;
        private System.Windows.Forms.Button SSCloseButton;
        private System.Windows.Forms.DataGridView InitProDGV;
        private System.Windows.Forms.DataGridView InitVarDGV;
        private System.Windows.Forms.ComboBox paramCombo;
        private System.Windows.Forms.Label configurationLabel;
        private System.Windows.Forms.BindingSource m_simParamSets;
        private System.Windows.Forms.ListBox stepperListBox;
        public System.Windows.Forms.TabPage loggingPage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton overrideRadio;
        private System.Windows.Forms.RadioButton exceptionRadio;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton noLimitRadio;
        private System.Windows.Forms.TextBox maxKbTextBox;
        private System.Windows.Forms.RadioButton maxSizeRadio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton freqBySecRadio;
        private System.Windows.Forms.TextBox freqBySecTextBox;
        private System.Windows.Forms.RadioButton freqByStepRadio;
        private System.Windows.Forms.TextBox freqByStepTextBox;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ComboBox stepCombo;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource perModelSimulationParameterBindingSource;
        private System.Windows.Forms.BindingSource propertiesBindingSource;
        private System.Windows.Forms.BindingSource steppersBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn2;
        private System.Windows.Forms.BindingSource perTypeInitialConditionsBindingSource;
        private System.Windows.Forms.BindingSource processInitialConditionsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn1;
        private System.Windows.Forms.BindingSource propertiesBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
    }
}
