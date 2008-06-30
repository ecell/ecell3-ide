namespace EcellLib
{
    /// <summary>
    /// Object class to manage the VariableReferenceList of process.
    /// </summary>
    partial class VariableRefWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableRefWindow));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.DeleteVarButton = new System.Windows.Forms.Button();
            this.VRCloseButton = new System.Windows.Forms.Button();
            this.VRApplyButton = new System.Windows.Forms.Button();
            this.AddVarButton = new System.Windows.Forms.Button();
            this.ReferenceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Coefficient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
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
            // 
            // DeleteVarButton
            // 
            resources.ApplyResources(this.DeleteVarButton, "DeleteVarButton");
            this.DeleteVarButton.Name = "DeleteVarButton";
            this.DeleteVarButton.UseVisualStyleBackColor = true;
            // 
            // VRCloseButton
            // 
            resources.ApplyResources(this.VRCloseButton, "VRCloseButton");
            this.VRCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.VRCloseButton.Name = "VRCloseButton";
            this.VRCloseButton.UseVisualStyleBackColor = true;
            // 
            // VRApplyButton
            // 
            resources.ApplyResources(this.VRApplyButton, "VRApplyButton");
            this.VRApplyButton.Name = "VRApplyButton";
            this.VRApplyButton.UseVisualStyleBackColor = true;
            // 
            // AddVarButton
            // 
            resources.ApplyResources(this.AddVarButton, "AddVarButton");
            this.AddVarButton.Name = "AddVarButton";
            this.AddVarButton.UseVisualStyleBackColor = true;
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
            // 
            // Coefficient
            // 
            this.Coefficient.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Coefficient.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Coefficient, "Coefficient");
            this.Coefficient.Name = "Coefficient";
            // 
            // VariableRefWindow
            // 
            this.AcceptButton = this.VRApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.VRCloseButton;
            this.Controls.Add(this.AddVarButton);
            this.Controls.Add(this.DeleteVarButton);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.VRApplyButton);
            this.Controls.Add(this.VRCloseButton);
            this.Name = "VariableRefWindow";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// DataGridView to display the VariableReferenceList.
        /// </summary>
        public System.Windows.Forms.DataGridView dgv;
        /// <summary>
        /// Button to delete the variable from VaribleReferenceList.
        /// </summary>
        public System.Windows.Forms.Button DeleteVarButton;
        /// <summary>
        /// Button to close this window.
        /// </summary>
        public System.Windows.Forms.Button VRCloseButton;
        /// <summary>
        /// Button to update the list of VariableReferenceList.
        /// </summary>
        public System.Windows.Forms.Button VRApplyButton;
        /// <summary>
        /// Button to add the variable to VariableReferenceList.
        /// </summary>
        public System.Windows.Forms.Button AddVarButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReferenceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coefficient;
    }
}