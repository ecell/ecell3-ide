namespace Ecell.IDE
{
    /// <summary>
    /// Object class to manage the VariableReferenceList of process.
    /// </summary>
    partial class VariableReferenceEditDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableReferenceEditDialog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.ReferenceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Coefficient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeleteVarButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.AddVarButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReferenceName,
            this.FullID,
            this.Coefficient});
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 21;
            this.dgv.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataCellValidated);
            this.dgv.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DataCellValidating);
            // 
            // ReferenceName
            // 
            this.ReferenceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.ReferenceName, "ReferenceName");
            this.ReferenceName.Name = "ReferenceName";
            // 
            // FullID
            // 
            resources.ApplyResources(this.FullID, "FullID");
            this.FullID.Name = "FullID";
            this.FullID.ReadOnly = true;
            // 
            // Coefficient
            // 
            this.Coefficient.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Coefficient.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Coefficient, "Coefficient");
            this.Coefficient.Name = "Coefficient";
            // 
            // DeleteVarButton
            // 
            resources.ApplyResources(this.DeleteVarButton, "DeleteVarButton");
            this.DeleteVarButton.Name = "DeleteVarButton";
            this.DeleteVarButton.UseVisualStyleBackColor = true;
            this.DeleteVarButton.Click += new System.EventHandler(this.DeleteVarReference);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // AddVarButton
            // 
            resources.ApplyResources(this.AddVarButton, "AddVarButton");
            this.AddVarButton.Name = "AddVarButton";
            this.AddVarButton.UseVisualStyleBackColor = true;
            this.AddVarButton.Click += new System.EventHandler(this.AddVarButtonClick);
            // 
            // VariableReferenceEditDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.AddVarButton);
            this.Controls.Add(this.DeleteVarButton);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "VariableReferenceEditDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VariableReferenceEditDialogClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button DeleteVarButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button AddVarButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReferenceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coefficient;
    }
}