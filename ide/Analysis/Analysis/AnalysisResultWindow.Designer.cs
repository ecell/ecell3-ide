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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.RAAnalysisTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RAXComboBox = new System.Windows.Forms.ComboBox();
            this.RAYComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.PEEstimateView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PEGenerateValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PEEstimationValue = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SAFCCGridView = new System.Windows.Forms.DataGridView();
            this.SACCCGridView = new System.Windows.Forms.DataGridView();
            this.RATrackLabel = new System.Windows.Forms.Label();
            this.ARTrackBar = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.RAAnalysisTableLayout.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PEEstimateView)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SAFCCGridView)).BeginInit();
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
            this.RAXComboBox.SelectedIndexChanged += new System.EventHandler(this.XSelectedIndexChanged);
            // 
            // RAYComboBox
            // 
            resources.ApplyResources(this.RAYComboBox, "RAYComboBox");
            this.RAYComboBox.FormattingEnabled = true;
            this.RAYComboBox.Name = "RAYComboBox";
            this.RAYComboBox.SelectedIndexChanged += new System.EventHandler(this.YSelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
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
            this.tabPage2.Controls.Add(this.PEEstimateView);
            this.tabPage2.Controls.Add(this.PEGenerateValue);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.PEEstimationValue);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // PEEstimateView
            // 
            this.PEEstimateView.AllowUserToAddRows = false;
            this.PEEstimateView.AllowUserToDeleteRows = false;
            this.PEEstimateView.AllowUserToResizeColumns = false;
            this.PEEstimateView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.PEEstimateView, "PEEstimateView");
            this.PEEstimateView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.PEEstimateView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.PEEstimateView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.PEEstimateView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.PEEstimateView.Name = "PEEstimateView";
            this.PEEstimateView.RowHeadersVisible = false;
            this.PEEstimateView.RowTemplate.Height = 21;
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
            // PEGenerateValue
            // 
            resources.ApplyResources(this.PEGenerateValue, "PEGenerateValue");
            this.PEGenerateValue.Name = "PEGenerateValue";
            this.PEGenerateValue.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.SAFCCGridView);
            this.tabPage3.Controls.Add(this.SACCCGridView);
            this.tabPage3.Controls.Add(this.RATrackLabel);
            this.tabPage3.Controls.Add(this.ARTrackBar);
            this.tabPage3.Controls.Add(this.label5);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // SAFCCGridView
            // 
            this.SAFCCGridView.AllowUserToAddRows = false;
            this.SAFCCGridView.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.SAFCCGridView, "SAFCCGridView");
            this.SAFCCGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SAFCCGridView.ColumnHeadersVisible = false;
            this.SAFCCGridView.Name = "SAFCCGridView";
            this.SAFCCGridView.RowHeadersVisible = false;
            this.SAFCCGridView.RowTemplate.Height = 21;
            // 
            // SACCCGridView
            // 
            this.SACCCGridView.AllowUserToAddRows = false;
            this.SACCCGridView.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.SACCCGridView, "SACCCGridView");
            this.SACCCGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SACCCGridView.ColumnHeadersVisible = false;
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
            // AnalysisResultWindow
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.IsSavable = true;
            this.Name = "AnalysisResultWindow";
            this.TabText = "AnalysisResultWindow";
            this.RAAnalysisTableLayout.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PEEstimateView)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SAFCCGridView)).EndInit();
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
        private System.Windows.Forms.DataGridView PEEstimateView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PEEstimationValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PEGenerateValue;
        private System.Windows.Forms.DataGridView SACCCGridView;
        private System.Windows.Forms.DataGridView SAFCCGridView;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar ARTrackBar;
        private System.Windows.Forms.Label RATrackLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}