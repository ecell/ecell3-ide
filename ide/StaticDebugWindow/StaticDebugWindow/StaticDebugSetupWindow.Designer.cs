//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

namespace EcellLib.StaticDebugWindow
{
    partial class StaticDebugSetupWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StaticDebugSetupWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.debugResultView = new System.Windows.Forms.DataGridView();
            this.MessageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PathColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SSDebugButton = new System.Windows.Forms.Button();
            this.SSCloseButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.debugResultView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.debugResultView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // debugResultView
            // 
            this.debugResultView.AllowUserToAddRows = false;
            this.debugResultView.AllowUserToDeleteRows = false;
            this.debugResultView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.debugResultView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.debugResultView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MessageColumn,
            this.PathColumn,
            this.ModelColumn,
            this.TypeColumn});
            resources.ApplyResources(this.debugResultView, "debugResultView");
            this.debugResultView.Name = "debugResultView";
            this.debugResultView.RowHeadersVisible = false;
            this.debugResultView.RowTemplate.Height = 21;
            // 
            // MessageColumn
            // 
            resources.ApplyResources(this.MessageColumn, "MessageColumn");
            this.MessageColumn.Name = "MessageColumn";
            this.MessageColumn.ReadOnly = true;
            // 
            // PathColumn
            // 
            this.PathColumn.FillWeight = 40F;
            resources.ApplyResources(this.PathColumn, "PathColumn");
            this.PathColumn.Name = "PathColumn";
            this.PathColumn.ReadOnly = true;
            // 
            // ModelColumn
            // 
            this.ModelColumn.FillWeight = 15F;
            resources.ApplyResources(this.ModelColumn, "ModelColumn");
            this.ModelColumn.Name = "ModelColumn";
            this.ModelColumn.ReadOnly = true;
            // 
            // TypeColumn
            // 
            this.TypeColumn.FillWeight = 15F;
            resources.ApplyResources(this.TypeColumn, "TypeColumn");
            this.TypeColumn.Name = "TypeColumn";
            this.TypeColumn.ReadOnly = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.SSDebugButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.SSCloseButton, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // SSDebugButton
            // 
            resources.ApplyResources(this.SSDebugButton, "SSDebugButton");
            this.SSDebugButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SSDebugButton.Name = "SSDebugButton";
            this.SSDebugButton.UseVisualStyleBackColor = true;
            this.SSDebugButton.Click += new System.EventHandler(this.debugButton_Click);
            // 
            // SSCloseButton
            // 
            resources.ApplyResources(this.SSCloseButton, "SSCloseButton");
            this.SSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SSCloseButton.Name = "SSCloseButton";
            this.SSCloseButton.UseVisualStyleBackColor = true;
            this.SSCloseButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.layoutPanel);
            this.panel1.Name = "panel1";
            // 
            // layoutPanel
            // 
            resources.ApplyResources(this.layoutPanel, "layoutPanel");
            this.layoutPanel.Name = "layoutPanel";
            // 
            // StaticDebugSetupWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "StaticDebugSetupWindow";
            this.Shown += new System.EventHandler(this.StaticDebugWinShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.debugResultView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.DataGridView debugResultView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button SSDebugButton;
        public System.Windows.Forms.Button SSCloseButton;
        public System.Windows.Forms.TableLayoutPanel layoutPanel;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn MessageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PathColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeColumn;
    }
}