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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.DeleteVarButton = new System.Windows.Forms.Button();
            this.VRCloseButton = new System.Windows.Forms.Button();
            this.VRApplyButton = new System.Windows.Forms.Button();
            this.AddVarButton = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Coeff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isAccessor = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.dgv, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.FullID,
            this.Coeff,
            this.isAccessor});
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 21;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.DeleteVarButton, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.VRCloseButton, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.VRApplyButton, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.AddVarButton, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // DeleteVarButton
            // 
            resources.ApplyResources(this.DeleteVarButton, "DeleteVarButton");
            this.DeleteVarButton.Name = "DeleteVarButton";
            this.DeleteVarButton.UseVisualStyleBackColor = true;
            // 
            // VRCloseButton
            // 
            this.VRCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.VRCloseButton, "VRCloseButton");
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
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            // 
            // FullID
            // 
            resources.ApplyResources(this.FullID, "FullID");
            this.FullID.Name = "FullID";
            // 
            // Coeff
            // 
            this.Coeff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Coeff.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Coeff, "Coeff");
            this.Coeff.Name = "Coeff";
            // 
            // isAccessor
            // 
            this.isAccessor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.isAccessor, "isAccessor");
            this.isAccessor.Name = "isAccessor";
            this.isAccessor.ReadOnly = true;
            this.isAccessor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isAccessor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // VariableRefWindow
            // 
            this.AcceptButton = this.VRApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.VRCloseButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "VariableRefWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        /// <summary>
        /// DataGridView to display the VariableReferenceList.
        /// </summary>
        public System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coeff;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAccessor;
    }
}