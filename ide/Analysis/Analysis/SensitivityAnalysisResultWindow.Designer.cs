namespace Ecell.IDE.Plugins.Analysis
{
    partial class SensitivityAnalysisResultWindow
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensitivityAnalysisResultWindow));
            System.Windows.Forms.SplitContainer splitContainer1;
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SAFCCGridView = new System.Windows.Forms.DataGridView();
            this.SACCCGridView = new System.Windows.Forms.DataGridView();
            this.RATrackLabel = new System.Windows.Forms.Label();
            this.ARTrackBar = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.SAFCCGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SACCCGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARTrackBar)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.ARTrackBar.ValueChanged += new System.EventHandler(this.ARTrackBarChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.label6);
            splitContainer1.Panel1.Controls.Add(this.SACCCGridView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.label7);
            splitContainer1.Panel2.Controls.Add(this.SAFCCGridView);
            // 
            // SensitivityAnalysisResultWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer1);
            this.Controls.Add(this.RATrackLabel);
            this.Controls.Add(this.ARTrackBar);
            this.Controls.Add(this.label5);
            this.Name = "SensitivityAnalysisResultWindow";
            ((System.ComponentModel.ISupportInitialize)(this.SAFCCGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SACCCGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARTrackBar)).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView SAFCCGridView;
        private System.Windows.Forms.DataGridView SACCCGridView;
        private System.Windows.Forms.Label RATrackLabel;
        private System.Windows.Forms.TrackBar ARTrackBar;
        private System.Windows.Forms.Label label5;
    }
}
