namespace Ecell.IDE.Plugins.SearchWindow
{
    partial class SearchCriteriaBuilderDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchCriteriaBuilderDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.idText = new System.Windows.Forms.TextBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.SCSearchButton = new System.Windows.Forms.Button();
            this.SCCloseButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dgv, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.idText, 1, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // idText
            // 
            resources.ApplyResources(this.idText, "idText");
            this.idText.Name = "idText";
            this.idText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.idTextKeyPress);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnId,
            this.ColumnName,
            this.ColumnModel,
            this.ColumnType});
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 21;
            this.dgv.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvCellClick);
            this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCellDoubleClick);
            // 
            // ColumnId
            // 
            this.ColumnId.FillWeight = 122.7241F;
            resources.ApplyResources(this.ColumnId, "ColumnId");
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            // 
            // ColumnName
            // 
            this.ColumnName.FillWeight = 70F;
            resources.ApplyResources(this.ColumnName, "ColumnName");
            this.ColumnName.Name = "ColumnName";
            // 
            // ColumnModel
            // 
            this.ColumnModel.FillWeight = 50F;
            resources.ApplyResources(this.ColumnModel, "ColumnModel");
            this.ColumnModel.Name = "ColumnModel";
            // 
            // ColumnType
            // 
            this.ColumnType.FillWeight = 41.74289F;
            resources.ApplyResources(this.ColumnType, "ColumnType");
            this.ColumnType.Name = "ColumnType";
            this.ColumnType.ReadOnly = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.SCSearchButton, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.SCCloseButton, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // SCSearchButton
            // 
            resources.ApplyResources(this.SCSearchButton, "SCSearchButton");
            this.SCSearchButton.Name = "SCSearchButton";
            this.SCSearchButton.UseVisualStyleBackColor = true;
            this.SCSearchButton.Click += new System.EventHandler(this.SearchButtonClick);
            // 
            // SCCloseButton
            // 
            resources.ApplyResources(this.SCCloseButton, "SCCloseButton");
            this.SCCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SCCloseButton.Name = "SCCloseButton";
            this.SCCloseButton.UseVisualStyleBackColor = true;
            // 
            // SearchCriteriaBuilderDialog
            // 
            this.AcceptButton = this.SCSearchButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.SCCloseButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SearchCriteriaBuilderDialog";
            this.Shown += new System.EventHandler(this.SearchCriteriaBuilderDialogShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// The condition to search the object.
        /// </summary>
        public System.Windows.Forms.TextBox idText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button SCSearchButton;
        private System.Windows.Forms.Button SCCloseButton;
        /// <summary>
        /// The list to show the search results.
        /// </summary>
        public System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
    }
}