namespace Ecell.IDE.Plugins.Analysis
{
    partial class GraphResultWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphResultWindow));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RAYComboBox = new System.Windows.Forms.ComboBox();
            this.RAXComboBox = new System.Windows.Forms.ComboBox();
            this.groupNameLabel = new System.Windows.Forms.Label();
            this.m_zCnt = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
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
            // RAYComboBox
            // 
            resources.ApplyResources(this.RAYComboBox, "RAYComboBox");
            this.RAYComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RAYComboBox.FormattingEnabled = true;
            this.RAYComboBox.Name = "RAYComboBox";
            this.RAYComboBox.SelectedIndexChanged += new System.EventHandler(this.YSelectedIndexChanged);
            // 
            // RAXComboBox
            // 
            resources.ApplyResources(this.RAXComboBox, "RAXComboBox");
            this.RAXComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RAXComboBox.FormattingEnabled = true;
            this.RAXComboBox.Name = "RAXComboBox";
            this.RAXComboBox.SelectedIndexChanged += new System.EventHandler(this.XSelectedIndexChanged);
            // 
            // groupNameLabel
            // 
            resources.ApplyResources(this.groupNameLabel, "groupNameLabel");
            this.groupNameLabel.Name = "groupNameLabel";
            // 
            // m_zCnt
            // 
            resources.ApplyResources(this.m_zCnt, "m_zCnt");
            this.m_zCnt.Name = "m_zCnt";
            this.m_zCnt.ScrollGrace = 0;
            this.m_zCnt.ScrollMaxX = 0;
            this.m_zCnt.ScrollMaxY = 0;
            this.m_zCnt.ScrollMaxY2 = 0;
            this.m_zCnt.ScrollMinX = 0;
            this.m_zCnt.ScrollMinY = 0;
            this.m_zCnt.ScrollMinY2 = 0;
            // 
            // GraphResultWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_zCnt);
            this.Controls.Add(this.RAXComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RAYComboBox);
            this.Controls.Add(this.groupNameLabel);
            this.Controls.Add(this.label2);
            this.Name = "GraphResultWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox RAXComboBox;
        private System.Windows.Forms.ComboBox RAYComboBox;
        private System.Windows.Forms.Label groupNameLabel;
        /// <summary>
        /// Graph control object.
        /// </summary>
        public ZedGraph.ZedGraphControl m_zCnt;
    }
}
