namespace EcellLib.TracerWindow
{
    partial class TraceWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TraceWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.TWSaveButton = new System.Windows.Forms.Button();
            this.TWCloseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.startText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.endText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.saveTypeCombo = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.searchDirButton = new System.Windows.Forms.Button();
            this.dirTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.view = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.color = new System.Windows.Forms.DataGridViewImageColumn();
            this.LineStyle = new System.Windows.Forms.DataGridViewImageColumn();
            this.full = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.m_colorDialog = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgv, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.TWSaveButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.TWCloseButton, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // TWSaveButton
            // 
            resources.ApplyResources(this.TWSaveButton, "TWSaveButton");
            this.TWSaveButton.Name = "TWSaveButton";
            this.TWSaveButton.UseVisualStyleBackColor = true;
            this.TWSaveButton.Click += new System.EventHandler(this.UpdateButtonClick);
            // 
            // TWCloseButton
            // 
            resources.ApplyResources(this.TWCloseButton, "TWCloseButton");
            this.TWCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.TWCloseButton.Name = "TWCloseButton";
            this.TWCloseButton.UseVisualStyleBackColor = true;
            this.TWCloseButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel7, 1, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.startText, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // startText
            // 
            resources.ApplyResources(this.startText, "startText");
            this.startText.Name = "startText";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.endText, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // endText
            // 
            resources.ApplyResources(this.endText, "endText");
            this.endText.Name = "endText";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.saveTypeCombo, 1, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // saveTypeCombo
            // 
            resources.ApplyResources(this.saveTypeCombo, "saveTypeCombo");
            this.saveTypeCombo.FormattingEnabled = true;
            this.saveTypeCombo.Items.AddRange(new object[] {
            resources.GetString("saveTypeCombo.Items"),
            resources.GetString("saveTypeCombo.Items1")});
            this.saveTypeCombo.Name = "saveTypeCombo";
            // 
            // tableLayoutPanel7
            // 
            resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
            this.tableLayoutPanel7.Controls.Add(this.searchDirButton, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.dirTextBox, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            // 
            // searchDirButton
            // 
            resources.ApplyResources(this.searchDirButton, "searchDirButton");
            this.searchDirButton.Name = "searchDirButton";
            this.searchDirButton.UseVisualStyleBackColor = true;
            this.searchDirButton.Click += new System.EventHandler(this.SearchDirButtonClick);
            // 
            // dirTextBox
            // 
            resources.ApplyResources(this.dirTextBox, "dirTextBox");
            this.dirTextBox.Name = "dirTextBox";
            this.dirTextBox.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // dgv
            // 
            this.dgv.AllowDrop = true;
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.view,
            this.color,
            this.LineStyle,
            this.full});
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 21;
            // 
            // view
            // 
            this.view.FillWeight = 41.50502F;
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            // 
            // color
            // 
            this.color.FillWeight = 40.81528F;
            resources.ApplyResources(this.color, "color");
            this.color.Name = "color";
            // 
            // LineStyle
            // 
            this.LineStyle.FillWeight = 40F;
            resources.ApplyResources(this.LineStyle, "LineStyle");
            this.LineStyle.Name = "LineStyle";
            // 
            // full
            // 
            this.full.FillWeight = 185.7F;
            resources.ApplyResources(this.full, "full");
            this.full.Name = "full";
            // 
            // TraceWindow
            // 
            this.AcceptButton = this.TWSaveButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.TWCloseButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TraceWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        /// <summary>
        /// DataGrid to show the list of trace.
        /// </summary>
        public System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        /// <summary>
        /// Button to set save directory.
        /// </summary>
        public System.Windows.Forms.Button searchDirButton;
        private System.Windows.Forms.TextBox dirTextBox;
        /// <summary>
        /// FolderBrowseDialog to set the output directory.
        /// </summary>
        public System.Windows.Forms.FolderBrowserDialog m_folderDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        /// <summary>
        /// ComboBox to set the file type of logger.
        /// </summary>
        public System.Windows.Forms.ComboBox saveTypeCombo;
        private System.Windows.Forms.Label label4;
        /// <summary>
        /// TextBox to set start time to save the logger.
        /// </summary>
        public System.Windows.Forms.TextBox startText;
        /// <summary>
        /// TextBox to set end time to save the logger.
        /// </summary>
        public System.Windows.Forms.TextBox endText;
        /// <summary>
        /// Button to save the logger.
        /// </summary>
        public System.Windows.Forms.Button TWSaveButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button TWCloseButton;
        /// <summary>
        /// ColorDialog to set line color.
        /// </summary>
        public System.Windows.Forms.ColorDialog m_colorDialog;
        private System.Windows.Forms.DataGridViewCheckBoxColumn view;
        private System.Windows.Forms.DataGridViewImageColumn color;
        private System.Windows.Forms.DataGridViewImageColumn LineStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn full;
    }
}