namespace EcellLib.Analysis
{
    partial class RobustAnalysis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobustAnalysis));
            this.robustTabControl = new System.Windows.Forms.TabControl();
            this.AnalysisTab = new System.Windows.Forms.TabPage();
            this.RAAnalysisTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.RAResultGridView = new System.Windows.Forms.DataGridView();
            this.JudgeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.XColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RAXComboBox = new System.Windows.Forms.ComboBox();
            this.RAYComboBox = new System.Windows.Forms.ComboBox();
            this.settingTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RAParamGridView = new System.Windows.Forms.DataGridView();
            this.IDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MinColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StepColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RAObservGridView = new System.Windows.Forms.DataGridView();
            this.OIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OMaxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OMinColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ODiffColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ORateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.RAWinSizeText = new System.Windows.Forms.TextBox();
            this.RAWinSizeLabel = new System.Windows.Forms.Label();
            this.RASimTimeText = new System.Windows.Forms.TextBox();
            this.RASimLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.RASampleNumText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.RARandomCheck = new System.Windows.Forms.CheckBox();
            this.RAMatrixCheck = new System.Windows.Forms.CheckBox();
            this.RASampleNumLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.RAMinFreqText = new System.Windows.Forms.TextBox();
            this.RAMaxFreqText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RMAMaxData = new System.Windows.Forms.TextBox();
            this.robustTabControl.SuspendLayout();
            this.AnalysisTab.SuspendLayout();
            this.RAAnalysisTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RAResultGridView)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.settingTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RAParamGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RAObservGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // robustTabControl
            // 
            this.robustTabControl.Controls.Add(this.AnalysisTab);
            this.robustTabControl.Controls.Add(this.settingTab);
            resources.ApplyResources(this.robustTabControl, "robustTabControl");
            this.robustTabControl.Name = "robustTabControl";
            this.robustTabControl.SelectedIndex = 0;
            // 
            // AnalysisTab
            // 
            this.AnalysisTab.Controls.Add(this.RAAnalysisTableLayout);
            resources.ApplyResources(this.AnalysisTab, "AnalysisTab");
            this.AnalysisTab.Name = "AnalysisTab";
            this.AnalysisTab.UseVisualStyleBackColor = true;
            // 
            // RAAnalysisTableLayout
            // 
            resources.ApplyResources(this.RAAnalysisTableLayout, "RAAnalysisTableLayout");
            this.RAAnalysisTableLayout.Controls.Add(this.RAResultGridView, 0, 2);
            this.RAAnalysisTableLayout.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.RAAnalysisTableLayout.Name = "RAAnalysisTableLayout";
            // 
            // RAResultGridView
            // 
            this.RAResultGridView.AllowDrop = true;
            this.RAResultGridView.AllowUserToAddRows = false;
            this.RAResultGridView.AllowUserToDeleteRows = false;
            this.RAResultGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.RAResultGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RAResultGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JudgeColumn,
            this.XColumn,
            this.YColumn});
            resources.ApplyResources(this.RAResultGridView, "RAResultGridView");
            this.RAResultGridView.MultiSelect = false;
            this.RAResultGridView.Name = "RAResultGridView";
            this.RAResultGridView.RowHeadersVisible = false;
            this.RAResultGridView.RowTemplate.Height = 21;
            this.RAResultGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // JudgeColumn
            // 
            this.JudgeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.JudgeColumn.Frozen = true;
            resources.ApplyResources(this.JudgeColumn, "JudgeColumn");
            this.JudgeColumn.Name = "JudgeColumn";
            this.JudgeColumn.ReadOnly = true;
            // 
            // XColumn
            // 
            resources.ApplyResources(this.XColumn, "XColumn");
            this.XColumn.Name = "XColumn";
            this.XColumn.ReadOnly = true;
            // 
            // YColumn
            // 
            resources.ApplyResources(this.YColumn, "YColumn");
            this.YColumn.Name = "YColumn";
            this.YColumn.ReadOnly = true;
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.RAXComboBox, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.RAYComboBox, 3, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // RAXComboBox
            // 
            resources.ApplyResources(this.RAXComboBox, "RAXComboBox");
            this.RAXComboBox.FormattingEnabled = true;
            this.RAXComboBox.Name = "RAXComboBox";
            this.RAXComboBox.SelectedIndexChanged += new System.EventHandler(this.ChangeXIndex);
            // 
            // RAYComboBox
            // 
            resources.ApplyResources(this.RAYComboBox, "RAYComboBox");
            this.RAYComboBox.FormattingEnabled = true;
            this.RAYComboBox.Name = "RAYComboBox";
            this.RAYComboBox.SelectedIndexChanged += new System.EventHandler(this.ChangeYIndex);
            // 
            // settingTab
            // 
            this.settingTab.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.settingTab, "settingTab");
            this.settingTab.Name = "settingTab";
            this.settingTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.RAParamGridView, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.RAObservGridView, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // RAParamGridView
            // 
            this.RAParamGridView.AllowDrop = true;
            this.RAParamGridView.AllowUserToAddRows = false;
            this.RAParamGridView.AllowUserToDeleteRows = false;
            this.RAParamGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.RAParamGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RAParamGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDColumn,
            this.MaxColumn,
            this.MinColumn,
            this.StepColumn});
            resources.ApplyResources(this.RAParamGridView, "RAParamGridView");
            this.RAParamGridView.Name = "RAParamGridView";
            this.RAParamGridView.RowHeadersVisible = false;
            this.RAParamGridView.RowTemplate.Height = 21;
            this.RAParamGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterParam);
            this.RAParamGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropParam);
            // 
            // IDColumn
            // 
            resources.ApplyResources(this.IDColumn, "IDColumn");
            this.IDColumn.Name = "IDColumn";
            // 
            // MaxColumn
            // 
            this.MaxColumn.FillWeight = 30F;
            resources.ApplyResources(this.MaxColumn, "MaxColumn");
            this.MaxColumn.Name = "MaxColumn";
            // 
            // MinColumn
            // 
            this.MinColumn.FillWeight = 30F;
            resources.ApplyResources(this.MinColumn, "MinColumn");
            this.MinColumn.Name = "MinColumn";
            // 
            // StepColumn
            // 
            this.StepColumn.FillWeight = 30F;
            resources.ApplyResources(this.StepColumn, "StepColumn");
            this.StepColumn.Name = "StepColumn";
            // 
            // RAObservGridView
            // 
            this.RAObservGridView.AllowDrop = true;
            this.RAObservGridView.AllowUserToAddRows = false;
            this.RAObservGridView.AllowUserToDeleteRows = false;
            this.RAObservGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.RAObservGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RAObservGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OIDColumn,
            this.OMaxColumn,
            this.OMinColumn,
            this.ODiffColumn,
            this.ORateColumn});
            resources.ApplyResources(this.RAObservGridView, "RAObservGridView");
            this.RAObservGridView.Name = "RAObservGridView";
            this.RAObservGridView.RowHeadersVisible = false;
            this.RAObservGridView.RowTemplate.Height = 21;
            this.RAObservGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterObserv);
            this.RAObservGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropObserv);
            // 
            // OIDColumn
            // 
            resources.ApplyResources(this.OIDColumn, "OIDColumn");
            this.OIDColumn.Name = "OIDColumn";
            // 
            // OMaxColumn
            // 
            this.OMaxColumn.FillWeight = 30F;
            resources.ApplyResources(this.OMaxColumn, "OMaxColumn");
            this.OMaxColumn.Name = "OMaxColumn";
            // 
            // OMinColumn
            // 
            this.OMinColumn.FillWeight = 30F;
            resources.ApplyResources(this.OMinColumn, "OMinColumn");
            this.OMinColumn.Name = "OMinColumn";
            // 
            // ODiffColumn
            // 
            this.ODiffColumn.FillWeight = 30F;
            resources.ApplyResources(this.ODiffColumn, "ODiffColumn");
            this.ODiffColumn.Name = "ODiffColumn";
            // 
            // ORateColumn
            // 
            this.ORateColumn.FillWeight = 30F;
            resources.ApplyResources(this.ORateColumn, "ORateColumn");
            this.ORateColumn.Name = "ORateColumn";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel9);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel9
            // 
            resources.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
            this.tableLayoutPanel9.Controls.Add(this.RAWinSizeText, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.RAWinSizeLabel, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.RASimTimeText, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.RASimLabel, 0, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            // 
            // RAWinSizeText
            // 
            resources.ApplyResources(this.RAWinSizeText, "RAWinSizeText");
            this.RAWinSizeText.Name = "RAWinSizeText";
            // 
            // RAWinSizeLabel
            // 
            resources.ApplyResources(this.RAWinSizeLabel, "RAWinSizeLabel");
            this.RAWinSizeLabel.Name = "RAWinSizeLabel";
            // 
            // RASimTimeText
            // 
            resources.ApplyResources(this.RASimTimeText, "RASimTimeText");
            this.RASimTimeText.Name = "RASimTimeText";
            // 
            // RASimLabel
            // 
            resources.ApplyResources(this.RASimLabel, "RASimLabel");
            this.RASimLabel.Name = "RASimLabel";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel10);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel10
            // 
            resources.ApplyResources(this.tableLayoutPanel10, "tableLayoutPanel10");
            this.tableLayoutPanel10.Controls.Add(this.RASampleNumText, 1, 1);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.RASampleNumLabel, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            // 
            // RASampleNumText
            // 
            resources.ApplyResources(this.RASampleNumText, "RASampleNumText");
            this.RASampleNumText.Name = "RASampleNumText";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.RARandomCheck, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.RAMatrixCheck, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // RARandomCheck
            // 
            resources.ApplyResources(this.RARandomCheck, "RARandomCheck");
            this.RARandomCheck.Name = "RARandomCheck";
            this.RARandomCheck.UseVisualStyleBackColor = true;
            this.RARandomCheck.CheckedChanged += new System.EventHandler(this.ChangeRARandomCheck);
            // 
            // RAMatrixCheck
            // 
            resources.ApplyResources(this.RAMatrixCheck, "RAMatrixCheck");
            this.RAMatrixCheck.Name = "RAMatrixCheck";
            this.RAMatrixCheck.UseVisualStyleBackColor = true;
            this.RAMatrixCheck.CheckedChanged += new System.EventHandler(this.ChangeRAMatrixCheck);
            // 
            // RASampleNumLabel
            // 
            resources.ApplyResources(this.RASampleNumLabel, "RASampleNumLabel");
            this.RASampleNumLabel.Name = "RASampleNumLabel";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel11);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // tableLayoutPanel11
            // 
            resources.ApplyResources(this.tableLayoutPanel11, "tableLayoutPanel11");
            this.tableLayoutPanel11.Controls.Add(this.RAMinFreqText, 1, 2);
            this.tableLayoutPanel11.Controls.Add(this.RAMaxFreqText, 1, 1);
            this.tableLayoutPanel11.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel11.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.RMAMaxData, 1, 0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            // 
            // RAMinFreqText
            // 
            resources.ApplyResources(this.RAMinFreqText, "RAMinFreqText");
            this.RAMinFreqText.Name = "RAMinFreqText";
            // 
            // RAMaxFreqText
            // 
            resources.ApplyResources(this.RAMaxFreqText, "RAMaxFreqText");
            this.RAMaxFreqText.Name = "RAMaxFreqText";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // RMAMaxData
            // 
            resources.ApplyResources(this.RMAMaxData, "RMAMaxData");
            this.RMAMaxData.Name = "RMAMaxData";
            // 
            // RobustAnalysis
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.robustTabControl);
            this.Name = "RobustAnalysis";
            this.robustTabControl.ResumeLayout(false);
            this.AnalysisTab.ResumeLayout(false);
            this.RAAnalysisTableLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RAResultGridView)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.settingTab.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RAParamGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RAObservGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// TabControl on RobustAnalysis Window.
        /// </summary>
        public System.Windows.Forms.TabControl robustTabControl;
        /// <summary>
        /// TabPage to display the result of robust analysis.
        /// </summary>
        public System.Windows.Forms.TabPage AnalysisTab;
        /// <summary>
        /// TabPage to set the condition of robust analysis.
        /// </summary>
        public System.Windows.Forms.TabPage settingTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label RASimLabel;
        /// <summary>
        /// TextBox to set the simulation time.
        /// </summary>
        public System.Windows.Forms.TextBox RASimTimeText;
        /// <summary>
        /// DataGridView to set the parameter property.
        /// </summary>
        public System.Windows.Forms.DataGridView RAParamGridView;
        /// <summary>
        /// DataGridView to set the observed property.
        /// </summary>
        public System.Windows.Forms.DataGridView RAObservGridView;
        /// <summary>
        /// DataGridView to display the property that become OK because of the judgement.
        /// </summary>
        public System.Windows.Forms.DataGridView RAResultGridView;
        private System.Windows.Forms.TableLayoutPanel RAAnalysisTableLayout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        /// <summary>
        /// ComboBox to set the property to X axis.
        /// </summary>
        public System.Windows.Forms.ComboBox RAXComboBox;
        /// <summary>
        /// ComboBox to set the property to Y axis.
        /// </summary>
        public System.Windows.Forms.ComboBox RAYComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OMaxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OMinColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ODiffColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORateColumn;
        private System.Windows.Forms.Label RASampleNumLabel;
        /// <summary>
        /// TextBoxt to set the number of samples.
        /// </summary>
        public System.Windows.Forms.TextBox RASampleNumText;
        private System.Windows.Forms.Label RAWinSizeLabel;
        /// <summary>
        /// TextBox to set the window size.
        /// </summary>
        public System.Windows.Forms.TextBox RAWinSizeText;
        private System.Windows.Forms.DataGridViewCheckBoxColumn JudgeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn XColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn YColumn;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// TextBox to set the max frequency for FFT.
        /// </summary>
        public System.Windows.Forms.TextBox RAMaxFreqText;
        private System.Windows.Forms.Label label4;
        /// <summary>
        /// TextBox to set the min frequency for FFT.
        /// </summary>
        public System.Windows.Forms.TextBox RAMinFreqText;
        private System.Windows.Forms.Label label5;
        /// <summary>
        /// CheckBox to set whether RobustAnalysis use the Matrix parameters or
        /// the random parameters.
        /// </summary>
        public System.Windows.Forms.CheckBox RAMatrixCheck;
        /// <summary>
        /// CheckBox to set whether RobustAnalysis use the Matrix parameters or
        /// the random parameters.
        /// </summary>
        public System.Windows.Forms.CheckBox RARandomCheck;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label6;
        /// <summary>
        /// TextBox to set the number of input data to calculate FFT.
        /// </summary>
        public System.Windows.Forms.TextBox RMAMaxData;
    }
}