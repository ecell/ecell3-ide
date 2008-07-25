﻿namespace Ecell.IDE
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
            this.VRCloseButton = new System.Windows.Forms.Button();
            this.VRApplyButton = new System.Windows.Forms.Button();
            this.AddVarButton = new System.Windows.Forms.Button();
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
            // DeleteVarButton
            // 
            resources.ApplyResources(this.DeleteVarButton, "DeleteVarButton");
            this.DeleteVarButton.Name = "DeleteVarButton";
            this.DeleteVarButton.UseVisualStyleBackColor = true;
            this.DeleteVarButton.Click += new System.EventHandler(this.DeleteVarReference);
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
            this.VRApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.VRApplyButton.Name = "VRApplyButton";
            this.VRApplyButton.UseVisualStyleBackColor = true;
            this.VRApplyButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // AddVarButton
            // 
            resources.ApplyResources(this.AddVarButton, "AddVarButton");
            this.AddVarButton.Name = "AddVarButton";
            this.AddVarButton.UseVisualStyleBackColor = true;
            this.AddVarButton.Click += new System.EventHandler(this.AddVarReference);
            // 
            // VariableReferenceEditDialog
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
            this.Name = "VariableReferenceEditDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VariableReferenceEditDialogClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn ReferenceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coefficient;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button DeleteVarButton;
        private System.Windows.Forms.Button VRCloseButton;
        private System.Windows.Forms.Button VRApplyButton;
        private System.Windows.Forms.Button AddVarButton;
    }
}