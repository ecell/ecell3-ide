﻿namespace Ecell.IDE.Plugins.ProjectExplorer
{
    /// <summary>
    /// ProjectExplorerControl UserControl.
    /// </summary>
    partial class ProjectExplorerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorerControl));
            this.treeView1 = new Ecell.IDE.Plugins.ProjectExplorer.MultiSelectTreeView();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(203, 184);
            this.treeView1.TabIndex = 5;
            // 
            // ProjectExplorerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 184);
            this.Controls.Add(this.treeView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectExplorerControl";
            this.TabText = "ProjectExplorerControl";
            this.Text = "ProjectExplorerControl";
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// treeView1
        /// </summary>
        public Ecell.IDE.Plugins.ProjectExplorer.MultiSelectTreeView treeView1;

    }
}