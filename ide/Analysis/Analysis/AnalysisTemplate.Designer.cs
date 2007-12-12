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

namespace EcellLib.Analysis
{
    partial class AnalysisTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalysisTemplate));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.isFixCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.targetCheckBox = new System.Windows.Forms.CheckBox();
            this.maxText = new System.Windows.Forms.TextBox();
            this.minText = new System.Windows.Forms.TextBox();
            this.maxValueText = new System.Windows.Forms.TextBox();
            this.minValueText = new System.Windows.Forms.TextBox();
            this.diffText = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.rateText = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.maxFreqText = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.minFreqText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.winSizeText = new System.Windows.Forms.TextBox();
            this.simTimeText = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.sampleNumText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.ATExecuteButton = new System.Windows.Forms.Button();
            this.ATStopButton = new System.Windows.Forms.Button();
            this.ATSaveButton = new System.Windows.Forms.Button();
            this.ATLoadButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.xComboBox = new System.Windows.Forms.ComboBox();
            this.ATViewButton = new System.Windows.Forms.Button();
            this.yComboBox = new System.Windows.Forms.ComboBox();
            this.SaveRobustFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenRobustFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel8, 0, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.treeView1, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // treeView1
            // 
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.Name = "treeView1";
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewNodeMouseClick);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterExpand);
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.isFixCheckBox, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel5.Controls.Add(this.label6, 0, 7);
            this.tableLayoutPanel5.Controls.Add(this.label7, 0, 8);
            this.tableLayoutPanel5.Controls.Add(this.label8, 0, 9);
            this.tableLayoutPanel5.Controls.Add(this.targetCheckBox, 1, 6);
            this.tableLayoutPanel5.Controls.Add(this.maxText, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.minText, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.maxValueText, 1, 7);
            this.tableLayoutPanel5.Controls.Add(this.minValueText, 1, 8);
            this.tableLayoutPanel5.Controls.Add(this.diffText, 1, 9);
            this.tableLayoutPanel5.Controls.Add(this.label11, 0, 10);
            this.tableLayoutPanel5.Controls.Add(this.rateText, 1, 10);
            this.tableLayoutPanel5.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.maxFreqText, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.minFreqText, 1, 4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // isFixCheckBox
            // 
            resources.ApplyResources(this.isFixCheckBox, "isFixCheckBox");
            this.isFixCheckBox.Name = "isFixCheckBox";
            this.isFixCheckBox.UseVisualStyleBackColor = true;
            this.isFixCheckBox.CheckedChanged += new System.EventHandler(this.IsFixCheckBoxCheckedChanged);
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // targetCheckBox
            // 
            resources.ApplyResources(this.targetCheckBox, "targetCheckBox");
            this.targetCheckBox.Name = "targetCheckBox";
            this.targetCheckBox.UseVisualStyleBackColor = true;
            this.targetCheckBox.CheckedChanged += new System.EventHandler(this.TargetCheckBoxCheckedChanged);
            // 
            // maxText
            // 
            resources.ApplyResources(this.maxText, "maxText");
            this.maxText.Name = "maxText";
            // 
            // minText
            // 
            resources.ApplyResources(this.minText, "minText");
            this.minText.Name = "minText";
            // 
            // maxValueText
            // 
            resources.ApplyResources(this.maxValueText, "maxValueText");
            this.maxValueText.Name = "maxValueText";
            // 
            // minValueText
            // 
            resources.ApplyResources(this.minValueText, "minValueText");
            this.minValueText.Name = "minValueText";
            // 
            // diffText
            // 
            resources.ApplyResources(this.diffText, "diffText");
            this.diffText.Name = "diffText";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // rateText
            // 
            resources.ApplyResources(this.rateText, "rateText");
            this.rateText.Name = "rateText";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // maxFreqText
            // 
            resources.ApplyResources(this.maxFreqText, "maxFreqText");
            this.maxFreqText.Name = "maxFreqText";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // minFreqText
            // 
            resources.ApplyResources(this.minFreqText, "minFreqText");
            this.minFreqText.Name = "minFreqText";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.winSizeText, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.simTimeText, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.sampleNumText, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // winSizeText
            // 
            resources.ApplyResources(this.winSizeText, "winSizeText");
            this.winSizeText.Name = "winSizeText";
            // 
            // simTimeText
            // 
            resources.ApplyResources(this.simTimeText, "simTimeText");
            this.simTimeText.Name = "simTimeText";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // sampleNumText
            // 
            resources.ApplyResources(this.sampleNumText, "sampleNumText");
            this.sampleNumText.Name = "sampleNumText";
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.ATExecuteButton, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.ATStopButton, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.ATSaveButton, 4, 0);
            this.tableLayoutPanel6.Controls.Add(this.ATLoadButton, 6, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // ATExecuteButton
            // 
            resources.ApplyResources(this.ATExecuteButton, "ATExecuteButton");
            this.ATExecuteButton.Name = "ATExecuteButton";
            this.ATExecuteButton.UseVisualStyleBackColor = true;
            this.ATExecuteButton.Click += new System.EventHandler(this.AnalysisButtonClicked);
            // 
            // ATStopButton
            // 
            resources.ApplyResources(this.ATStopButton, "ATStopButton");
            this.ATStopButton.Name = "ATStopButton";
            this.ATStopButton.UseVisualStyleBackColor = true;
            this.ATStopButton.Click += new System.EventHandler(this.StopButtonClicked);
            // 
            // ATSaveButton
            // 
            resources.ApplyResources(this.ATSaveButton, "ATSaveButton");
            this.ATSaveButton.Name = "ATSaveButton";
            this.ATSaveButton.UseVisualStyleBackColor = true;
            this.ATSaveButton.Click += new System.EventHandler(this.SaveButtonClicked);
            // 
            // ATLoadButton
            // 
            resources.ApplyResources(this.ATLoadButton, "ATLoadButton");
            this.ATLoadButton.Name = "ATLoadButton";
            this.ATLoadButton.UseVisualStyleBackColor = true;
            this.ATLoadButton.Click += new System.EventHandler(this.LoadButtonClicked);
            // 
            // tableLayoutPanel8
            // 
            resources.ApplyResources(this.tableLayoutPanel8, "tableLayoutPanel8");
            this.tableLayoutPanel8.Controls.Add(this.xComboBox, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.ATViewButton, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.yComboBox, 1, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            // 
            // xComboBox
            // 
            resources.ApplyResources(this.xComboBox, "xComboBox");
            this.xComboBox.FormattingEnabled = true;
            this.xComboBox.Name = "xComboBox";
            // 
            // ATViewButton
            // 
            resources.ApplyResources(this.ATViewButton, "ATViewButton");
            this.ATViewButton.Name = "ATViewButton";
            this.ATViewButton.UseVisualStyleBackColor = true;
            this.ATViewButton.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // yComboBox
            // 
            resources.ApplyResources(this.yComboBox, "yComboBox");
            this.yComboBox.FormattingEnabled = true;
            this.yComboBox.Name = "yComboBox";
            // 
            // OpenRobustFileDialog
            // 
            this.OpenRobustFileDialog.FileName = "openFileDialog1";
            // 
            // AnalysisTemplate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AnalysisTemplate";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ATExecuteButton;
        private System.Windows.Forms.Button ATSaveButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button ATViewButton;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox winSizeText;
        private System.Windows.Forms.ComboBox xComboBox;
        private System.Windows.Forms.ComboBox yComboBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox sampleNumText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox simTimeText;
        /// <summary>
        /// CheckBox to select the fixed parameter.
        /// </summary>
        public System.Windows.Forms.CheckBox isFixCheckBox;
        /// <summary>
        /// CheckBox to select the parameter by using judge of the convergence.
        /// </summary>
        public System.Windows.Forms.CheckBox targetCheckBox;
        /// <summary>
        /// TextBox to set the parameter of robust analysis[Max].
        /// </summary>
        public System.Windows.Forms.TextBox maxText;
        /// <summary>
        /// TextBox to set the parameter of robust analysis[Min].
        /// </summary>
        public System.Windows.Forms.TextBox minText;
        /// <summary>
        /// TextBox to set the parameter of robust analysis[Max Value].
        /// </summary>
        public System.Windows.Forms.TextBox maxValueText;
        /// <summary>
        /// TextBox to set the parameter of robust analysis[Min Value].
        /// </summary>
        public System.Windows.Forms.TextBox minValueText;
        /// <summary>
        /// TextBox to set the parameter of robust analysis[Difference].
        /// </summary>
        public System.Windows.Forms.TextBox diffText;
        /// <summary>
        /// TextBox to set the parameter of robust analysis[Max Frequency].
        /// </summary>
        public System.Windows.Forms.TextBox maxFreqText;
        /// <summary>
        /// TextBox to set the parameter of robust analysis[Min Frequency].
        /// </summary>
        public System.Windows.Forms.TextBox minFreqText;
        /// <summary>
        /// TextBox to set the parameter of robust analysis[rate].
        /// </summary>
        public System.Windows.Forms.TextBox rateText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        /// <summary>
        /// Button to stop the analysis.
        /// </summary>
        public System.Windows.Forms.Button ATStopButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        /// <summary>
        /// SaveFileDialog to save the parameter file of analysis.
        /// </summary>
        public System.Windows.Forms.SaveFileDialog SaveRobustFileDialog;
        /// <summary>
        /// OpenFileDialog to open the parameter file of analysis.
        /// </summary>
        public System.Windows.Forms.OpenFileDialog OpenRobustFileDialog;
        /// <summary>
        /// Button to load the parameter of analysis.
        /// </summary>
        public System.Windows.Forms.Button ATLoadButton;
    }
}
