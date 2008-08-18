namespace Ecell.IDE.Plugins.EntityList
{
    partial class EntityListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityListControl));
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.objectListDataGrid = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewImageColumn();
            this.ClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ObjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.objectListDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // searchTextBox
            // 
            resources.ApplyResources(this.searchTextBox, "searchTextBox");
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Enter += new System.EventHandler(this.searchTextBox_Enter);
            this.searchTextBox.Leave += new System.EventHandler(this.searchTextBox_Leave);
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // clearButton
            // 
            resources.ApplyResources(this.clearButton, "clearButton");
            this.clearButton.Name = "clearButton";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // objectListDataGrid
            // 
            this.objectListDataGrid.AllowUserToAddRows = false;
            this.objectListDataGrid.AllowUserToDeleteRows = false;
            this.objectListDataGrid.AllowUserToOrderColumns = true;
            this.objectListDataGrid.AllowUserToResizeRows = false;
            resources.ApplyResources(this.objectListDataGrid, "objectListDataGrid");
            this.objectListDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.objectListDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.ClassName,
            this.ID,
            this.ObjectName});
            this.objectListDataGrid.Name = "objectListDataGrid";
            this.objectListDataGrid.ReadOnly = true;
            this.objectListDataGrid.RowHeadersVisible = false;
            this.objectListDataGrid.RowTemplate.Height = 21;
            this.objectListDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.objectListDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ClickObjectCell);
            // 
            // Type
            // 
            this.Type.FillWeight = 10F;
            this.Type.Frozen = true;
            resources.ApplyResources(this.Type, "Type");
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ClassName
            // 
            this.ClassName.FillWeight = 30F;
            resources.ApplyResources(this.ClassName, "ClassName");
            this.ClassName.Name = "ClassName";
            this.ClassName.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.FillWeight = 30F;
            resources.ApplyResources(this.ID, "ID");
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // ObjectName
            // 
            this.ObjectName.FillWeight = 30F;
            resources.ApplyResources(this.ObjectName, "ObjectName");
            this.ObjectName.Name = "ObjectName";
            this.ObjectName.ReadOnly = true;
            // 
            // EntityListControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectListDataGrid);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.searchTextBox);
            this.Name = "EntityListControl";
            ((System.ComponentModel.ISupportInitialize)(this.objectListDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.DataGridView objectListDataGrid;
        private System.Windows.Forms.DataGridViewImageColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObjectName;
    }
}
