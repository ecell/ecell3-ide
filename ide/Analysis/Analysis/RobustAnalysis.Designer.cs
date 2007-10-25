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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.RASimLabel = new System.Windows.Forms.Label();
            this.RASimTimeText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.RASampleNumLabel = new System.Windows.Forms.Label();
            this.RASampleNumText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.RAWinSizeLabel = new System.Windows.Forms.Label();
            this.RAWinSizeText = new System.Windows.Forms.TextBox();
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
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.RAMaxFreqText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.RAMinFreqText = new System.Windows.Forms.TextBox();
            this.robustTabControl.SuspendLayout();
            this.AnalysisTab.SuspendLayout();
            this.RAAnalysisTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RAResultGridView)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.settingTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RAParamGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RAObservGridView)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
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
            this.RAXComboBox.SelectedIndexChanged += new System.EventHandler(this.XIndexChanged);
            // 
            // RAYComboBox
            // 
            resources.ApplyResources(this.RAYComboBox, "RAYComboBox");
            this.RAYComboBox.FormattingEnabled = true;
            this.RAYComboBox.Name = "RAYComboBox";
            this.RAYComboBox.SelectedIndexChanged += new System.EventHandler(this.YIndexChanged);
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
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.RAParamGridView, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.RAObservGridView, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.RASimLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.RASimTimeText, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // RASimLabel
            // 
            resources.ApplyResources(this.RASimLabel, "RASimLabel");
            this.RASimLabel.Name = "RASimLabel";
            // 
            // RASimTimeText
            // 
            resources.ApplyResources(this.RASimTimeText, "RASimTimeText");
            this.RASimTimeText.Name = "RASimTimeText";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.RASampleNumLabel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.RASampleNumText, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // RASampleNumLabel
            // 
            resources.ApplyResources(this.RASampleNumLabel, "RASampleNumLabel");
            this.RASampleNumLabel.Name = "RASampleNumLabel";
            // 
            // RASampleNumText
            // 
            resources.ApplyResources(this.RASampleNumText, "RASampleNumText");
            this.RASampleNumText.Name = "RASampleNumText";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.RAWinSizeLabel, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.RAWinSizeText, 1, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // RAWinSizeLabel
            // 
            resources.ApplyResources(this.RAWinSizeLabel, "RAWinSizeLabel");
            this.RAWinSizeLabel.Name = "RAWinSizeLabel";
            // 
            // RAWinSizeText
            // 
            resources.ApplyResources(this.RAWinSizeText, "RAWinSizeText");
            this.RAWinSizeText.Name = "RAWinSizeText";
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
            this.RAParamGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.ParamDragEnter);
            this.RAParamGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.ParamDragDrop);
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
            this.RAObservGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.ObservDragEnter);
            this.RAObservGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.ObservDragDrop);
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
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.RAMaxFreqText, 1, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // RAMaxFreqText
            // 
            resources.ApplyResources(this.RAMaxFreqText, "RAMaxFreqText");
            this.RAMaxFreqText.Name = "RAMaxFreqText";
            // 
            // tableLayoutPanel7
            // 
            resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
            this.tableLayoutPanel7.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.RAMinFreqText, 1, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // RAMinFreqText
            // 
            resources.ApplyResources(this.RAMinFreqText, "RAMinFreqText");
            this.RAMinFreqText.Name = "RAMinFreqText";
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
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RAParamGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RAObservGridView)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// TextBox to set the max frequency for FFT.
        /// </summary>
        public System.Windows.Forms.TextBox RAMaxFreqText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label4;
        /// <summary>
        /// TextBox to set the min frequency for FFT.
        /// </summary>
        public System.Windows.Forms.TextBox RAMinFreqText;
    }
}