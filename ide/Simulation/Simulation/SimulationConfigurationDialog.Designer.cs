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
            System.Windows.Forms.TabPage initialParametersPage;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationConfigurationDialog));
            System.Windows.Forms.TabPage perModelSimulationParametersPage;
            System.Windows.Forms.TabPage stepperDefinitionsPage;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Button button2;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            this.initialParameters = new System.Windows.Forms.DataGridView();
            this.initialConditionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.keyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            initialParametersPage = new System.Windows.Forms.TabPage();
            perModelSimulationParametersPage = new System.Windows.Forms.TabPage();
            stepperDefinitionsPage = new System.Windows.Forms.TabPage();
            label2 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            initialParametersPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.initialParameters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialConditionsBindingSource)).BeginInit();
            perModelSimulationParametersPage.SuspendLayout();
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
            // initialParametersPage
            // 
            initialParametersPage.Controls.Add(this.initialParameters);
            resources.ApplyResources(initialParametersPage, "initialParametersPage");
            initialParametersPage.Name = "initialParametersPage";
            initialParametersPage.UseVisualStyleBackColor = true;
            // 
            // initialParameters
            // 
            this.initialParameters.AllowUserToAddRows = false;
            resources.ApplyResources(this.initialParameters, "initialParameters");
            this.initialParameters.AutoGenerateColumns = false;
            this.initialParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.initialParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.initialParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn1,
            this.valueDataGridViewTextBoxColumn1});
            this.initialParameters.DataSource = this.initialConditionsBindingSource;
            this.initialParameters.Name = "initialParameters";
            this.initialParameters.RowHeadersVisible = false;
            this.initialParameters.RowTemplate.Height = 21;
            // 
            // initialConditionsBindingSource
            // 
            this.initialConditionsBindingSource.DataSource = typeof(Ecell.IDE.Plugins.Simulation.MutableKeyValuePair<string, double>);
            // 
            // perModelSimulationParametersPage
            // 
            perModelSimulationParametersPage.Controls.Add(this.tabControl2);
            perModelSimulationParametersPage.Controls.Add(label6);
            perModelSimulationParametersPage.Controls.Add(this.modelCombo);
            resources.ApplyResources(perModelSimulationParametersPage, "perModelSimulationParametersPage");
            perModelSimulationParametersPage.Name = "perModelSimulationParametersPage";
            perModelSimulationParametersPage.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            resources.ApplyResources(this.tabControl2, "tabControl2");
            this.tabControl2.Controls.Add(initialParametersPage);
            this.tabControl2.Controls.Add(stepperDefinitionsPage);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            // 
            // stepperDefinitionsPage
            // 
            stepperDefinitionsPage.Controls.Add(label2);
            stepperDefinitionsPage.Controls.Add(button1);
            stepperDefinitionsPage.Controls.Add(this.panel1);
            stepperDefinitionsPage.Controls.Add(button2);
            stepperDefinitionsPage.Controls.Add(this.stepperListBox);
            resources.ApplyResources(stepperDefinitionsPage, "stepperDefinitionsPage");
            stepperDefinitionsPage.Name = "stepperDefinitionsPage";
            stepperDefinitionsPage.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.AddStepperClick);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(label1);
            this.panel1.Controls.Add(this.dgv);
            this.panel1.Controls.Add(label7);
            this.panel1.Controls.Add(this.stepCombo);
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.AutoGenerateColumns = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn});
            this.dgv.DataSource = this.propertiesBindingSource;
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
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // stepCombo
            // 
            resources.ApplyResources(this.stepCombo, "stepCombo");
            this.stepCombo.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.steppersBindingSource, "ClassName", true));
            this.stepCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stepCombo.FormattingEnabled = true;
            this.stepCombo.Name = "stepCombo";
            this.stepCombo.SelectedValueChanged += new System.EventHandler(this.stepCombo_SelectedValueChanged);
            // 
            // button2
            // 
            resources.ApplyResources(button2, "button2");
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(this.DeleteStepperClick);
            // 
            // stepperListBox
            // 
            resources.ApplyResources(this.stepperListBox, "stepperListBox");
            this.stepperListBox.DataSource = this.steppersBindingSource;
            this.stepperListBox.DisplayMember = "Name";
            this.stepperListBox.FormattingEnabled = true;
            this.stepperListBox.Name = "stepperListBox";
            this.stepperListBox.Sorted = true;
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // modelCombo
            // 
            resources.ApplyResources(this.modelCombo, "modelCombo");
            this.modelCombo.DataSource = this.perModelSimulationParameterBindingSource;
            this.modelCombo.DisplayMember = "ModelID";
            this.modelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // SSCreateButton
            // 
            resources.ApplyResources(this.SSCreateButton, "SSCreateButton");
            this.SSCreateButton.Name = "SSCreateButton";
            this.SSCreateButton.UseVisualStyleBackColor = true;
            this.SSCreateButton.Click += new System.EventHandler(this.NewButtonClick);
            // 
            // paramCombo
            // 
            resources.ApplyResources(this.paramCombo, "paramCombo");
            this.paramCombo.DataSource = this.m_simParamSets;
            this.paramCombo.DisplayMember = "Name";
            this.paramCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paramCombo.FormattingEnabled = true;
            this.paramCombo.Name = "paramCombo";
            // 
            // SSDeleteButton
            // 
            resources.ApplyResources(this.SSDeleteButton, "SSDeleteButton");
            this.SSDeleteButton.Name = "SSDeleteButton";
            this.SSDeleteButton.UseVisualStyleBackColor = true;
            this.SSDeleteButton.Click += new System.EventHandler(this.DeleteButtonClick);
            // 
            // SSApplyButton
            // 
            resources.ApplyResources(this.SSApplyButton, "SSApplyButton");
            this.SSApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            // loggingPage
            // 
            this.loggingPage.Controls.Add(this.groupBox2);
            this.loggingPage.Controls.Add(this.groupBox3);
            this.loggingPage.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.loggingPage, "loggingPage");
            this.loggingPage.Name = "loggingPage";
            this.loggingPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.overrideRadio);
            this.groupBox2.Controls.Add(this.exceptionRadio);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // overrideRadio
            // 
            resources.ApplyResources(this.overrideRadio, "overrideRadio");
            this.overrideRadio.Name = "overrideRadio";
            this.overrideRadio.UseVisualStyleBackColor = true;
            this.overrideRadio.CheckedChanged += new System.EventHandler(this.overrideRadio_CheckedChanged);
            // 
            // exceptionRadio
            // 
            resources.ApplyResources(this.exceptionRadio, "exceptionRadio");
            this.exceptionRadio.Checked = true;
            this.exceptionRadio.Name = "exceptionRadio";
            this.exceptionRadio.TabStop = true;
            this.exceptionRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(label5);
            this.groupBox3.Controls.Add(this.noLimitRadio);
            this.groupBox3.Controls.Add(this.maxKbTextBox);
            this.groupBox3.Controls.Add(this.maxSizeRadio);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // noLimitRadio
            // 
            resources.ApplyResources(this.noLimitRadio, "noLimitRadio");
            this.noLimitRadio.Checked = true;
            this.noLimitRadio.Name = "noLimitRadio";
            this.noLimitRadio.TabStop = true;
            this.noLimitRadio.UseVisualStyleBackColor = true;
            this.noLimitRadio.CheckedChanged += new System.EventHandler(this.noLimitRadio_CheckedChanged);
            // 
            // maxKbTextBox
            // 
            resources.ApplyResources(this.maxKbTextBox, "maxKbTextBox");
            this.maxKbTextBox.Name = "maxKbTextBox";
            // 
            // maxSizeRadio
            // 
            resources.ApplyResources(this.maxSizeRadio, "maxSizeRadio");
            this.maxSizeRadio.Name = "maxSizeRadio";
            this.maxSizeRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.freqBySecRadio);
            this.groupBox1.Controls.Add(this.freqBySecTextBox);
            this.groupBox1.Controls.Add(this.freqByStepRadio);
            this.groupBox1.Controls.Add(this.freqByStepTextBox);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // freqBySecRadio
            // 
            resources.ApplyResources(this.freqBySecRadio, "freqBySecRadio");
            this.freqBySecRadio.Name = "freqBySecRadio";
            this.freqBySecRadio.UseVisualStyleBackColor = true;
            // 
            // freqBySecTextBox
            // 
            resources.ApplyResources(this.freqBySecTextBox, "freqBySecTextBox");
            this.freqBySecTextBox.Name = "freqBySecTextBox";
            this.freqBySecTextBox.Validated += new System.EventHandler(this.freqBySecTextBox_Validated);
            this.freqBySecTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.freqBySecTextBox_Validating);
            this.freqBySecTextBox.TextChanged += new System.EventHandler(this.freqBySecTextBox_TextChanged);
            // 
            // freqByStepRadio
            // 
            resources.ApplyResources(this.freqByStepRadio, "freqByStepRadio");
            this.freqByStepRadio.Checked = true;
            this.freqByStepRadio.Name = "freqByStepRadio";
            this.freqByStepRadio.TabStop = true;
            this.freqByStepRadio.UseVisualStyleBackColor = true;
            this.freqByStepRadio.CheckedChanged += new System.EventHandler(this.freqByStepRadio_CheckedChanged);
            // 
            // freqByStepTextBox
            // 
            resources.ApplyResources(this.freqByStepTextBox, "freqByStepTextBox");
            this.freqByStepTextBox.Name = "freqByStepTextBox";
            this.freqByStepTextBox.Validated += new System.EventHandler(this.freqByStepTextBox_Validated);
            this.freqByStepTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.freqByStepTextBox_Validating);
            this.freqByStepTextBox.TextChanged += new System.EventHandler(this.freqByStepTextBox_TextChanged);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(perModelSimulationParametersPage);
            this.tabControl1.Controls.Add(this.loggingPage);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // keyDataGridViewTextBoxColumn1
            // 
            this.keyDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.keyDataGridViewTextBoxColumn1.DataPropertyName = "Key";
            this.keyDataGridViewTextBoxColumn1.FillWeight = 80F;
            resources.ApplyResources(this.keyDataGridViewTextBoxColumn1, "keyDataGridViewTextBoxColumn1");
            this.keyDataGridViewTextBoxColumn1.Name = "keyDataGridViewTextBoxColumn1";
            // 
            // valueDataGridViewTextBoxColumn1
            // 
            this.valueDataGridViewTextBoxColumn1.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn1.FillWeight = 20F;
            resources.ApplyResources(this.valueDataGridViewTextBoxColumn1, "valueDataGridViewTextBoxColumn1");
            this.valueDataGridViewTextBoxColumn1.Name = "valueDataGridViewTextBoxColumn1";
            // 
            // SimulationConfigurationDialog
            // 
            this.AcceptButton = this.SSApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.SSCloseButton;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.SSApplyButton);
            this.Controls.Add(this.SSCloseButton);
            this.Controls.Add(this.SSDeleteButton);
            this.Controls.Add(this.SSCreateButton);
            this.Controls.Add(this.configurationLabel);
            this.Controls.Add(this.paramCombo);
            this.Name = "SimulationConfigurationDialog";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SetupKeyPress);
            initialParametersPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.initialParameters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialConditionsBindingSource)).EndInit();
            perModelSimulationParametersPage.ResumeLayout(false);
            perModelSimulationParametersPage.PerformLayout();
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
        private System.Windows.Forms.DataGridView initialParameters;
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
        private System.Windows.Forms.BindingSource propertiesBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource initialConditionsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn1;
    }
}
