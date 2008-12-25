namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Window to display the result of analysis.
    /// </summary>
    partial class AnalysisResultWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalysisResultWindow));
            this.RAAnalysisTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RAXComboBox = new System.Windows.Forms.ComboBox();
            this.RAYComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.PEEstimateView = new System.Windows.Forms.DataGridView();
            this.PEGenerateValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PEEstimationValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SAFCCGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SACCCGridView = new System.Windows.Forms.DataGridView();
            this.RATrackLabel = new System.Windows.Forms.Label();
            this.ARTrackBar = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RAAnalysisTableLayout.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PEEstimateView)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SAFCCGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SACCCGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // RAAnalysisTableLayout
            // 
            resources.ApplyResources(this.RAAnalysisTableLayout, "RAAnalysisTableLayout");
            this.RAAnalysisTableLayout.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.RAAnalysisTableLayout.Name = "RAAnalysisTableLayout";
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
            // 
            // RAYComboBox
            // 
            resources.ApplyResources(this.RAYComboBox, "RAYComboBox");
            this.RAYComboBox.FormattingEnabled = true;
            this.RAYComboBox.Name = "RAYComboBox";
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.RAAnalysisTableLayout);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.PEEstimateView);
            this.groupBox3.Controls.Add(this.PEGenerateValue);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.PEEstimationValue);
            this.groupBox3.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // PEEstimateView
            // 
            this.PEEstimateView.AllowUserToAddRows = false;
            this.PEEstimateView.AllowUserToDeleteRows = false;
            this.PEEstimateView.AllowUserToResizeColumns = false;
            this.PEEstimateView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.PEEstimateView, "PEEstimateView");
            this.PEEstimateView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PEEstimateView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PEEstimateView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.PEEstimateView.Name = "PEEstimateView";
            this.PEEstimateView.RowHeadersVisible = false;
            this.PEEstimateView.RowTemplate.Height = 21;
            // 
            // PEGenerateValue
            // 
            resources.ApplyResources(this.PEGenerateValue, "PEGenerateValue");
            this.PEGenerateValue.Name = "PEGenerateValue";
            this.PEGenerateValue.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // PEEstimationValue
            // 
            resources.ApplyResources(this.PEEstimationValue, "PEEstimationValue");
            this.PEEstimationValue.Name = "PEEstimationValue";
            this.PEEstimationValue.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.RATrackLabel);
            this.tabPage3.Controls.Add(this.ARTrackBar);
            this.tabPage3.Controls.Add(this.label5);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.SAFCCGridView);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // SAFCCGridView
            // 
            this.SAFCCGridView.AllowUserToAddRows = false;
            this.SAFCCGridView.AllowUserToDeleteRows = false;
            this.SAFCCGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SAFCCGridView.ColumnHeadersVisible = false;
            resources.ApplyResources(this.SAFCCGridView, "SAFCCGridView");
            this.SAFCCGridView.Name = "SAFCCGridView";
            this.SAFCCGridView.RowHeadersVisible = false;
            this.SAFCCGridView.RowTemplate.Height = 21;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.SACCCGridView);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // SACCCGridView
            // 
            this.SACCCGridView.AllowUserToAddRows = false;
            this.SACCCGridView.AllowUserToDeleteRows = false;
            this.SACCCGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SACCCGridView.ColumnHeadersVisible = false;
            resources.ApplyResources(this.SACCCGridView, "SACCCGridView");
            this.SACCCGridView.Name = "SACCCGridView";
            this.SACCCGridView.RowHeadersVisible = false;
            this.SACCCGridView.RowTemplate.Height = 21;
            // 
            // RATrackLabel
            // 
            resources.ApplyResources(this.RATrackLabel, "RATrackLabel");
            this.RATrackLabel.Name = "RATrackLabel";
            // 
            // ARTrackBar
            // 
            resources.ApplyResources(this.ARTrackBar, "ARTrackBar");
            this.ARTrackBar.Maximum = 500;
            this.ARTrackBar.Name = "ARTrackBar";
            this.ARTrackBar.Value = 500;
            this.ARTrackBar.Scroll += new System.EventHandler(this.ARTrackBarChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 40F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 60F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // AnalysisResultWindow
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.IsSavable = true;
            this.Name = "AnalysisResultWindow";
            this.RAAnalysisTableLayout.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PEEstimateView)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SAFCCGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SACCCGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RAAnalysisTableLayout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox RAXComboBox;
        private System.Windows.Forms.ComboBox RAYComboBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView PEEstimateView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PEEstimationValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PEGenerateValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView SACCCGridView;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView SAFCCGridView;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar ARTrackBar;
        private System.Windows.Forms.Label RATrackLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}