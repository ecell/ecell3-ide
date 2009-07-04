namespace Ecell.IDE.Plugins.Analysis
{
    partial class SensitivityAnalysisSettingDialog
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensitivityAnalysisSettingDialog));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.executeButton = new System.Windows.Forms.Button();
            this.sensitivityStepTextBox = new System.Windows.Forms.TextBox();
            this.sensitivityAbsolutePerturbationTextBox = new System.Windows.Forms.TextBox();
            this.sensitivityRelativePerturbationTextBox = new System.Windows.Forms.TextBox();
            this.sensitivityToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.abstractTextBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // executeButton
            // 
            resources.ApplyResources(this.executeButton, "executeButton");
            this.executeButton.Name = "executeButton";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.ExecuteButtonClick);
            // 
            // sensitivityStepTextBox
            // 
            resources.ApplyResources(this.sensitivityStepTextBox, "sensitivityStepTextBox");
            this.sensitivityStepTextBox.Name = "sensitivityStepTextBox";
            this.sensitivityStepTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Step_Validating);
            // 
            // sensitivityAbsolutePerturbationTextBox
            // 
            resources.ApplyResources(this.sensitivityAbsolutePerturbationTextBox, "sensitivityAbsolutePerturbationTextBox");
            this.sensitivityAbsolutePerturbationTextBox.Name = "sensitivityAbsolutePerturbationTextBox";
            this.sensitivityAbsolutePerturbationTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.AbsolutePerturbation_Validating);
            // 
            // sensitivityRelativePerturbationTextBox
            // 
            resources.ApplyResources(this.sensitivityRelativePerturbationTextBox, "sensitivityRelativePerturbationTextBox");
            this.sensitivityRelativePerturbationTextBox.Name = "sensitivityRelativePerturbationTextBox";
            this.sensitivityRelativePerturbationTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.RelativePerturbation_Validating);
            // 
            // abstractTextBox
            // 
            resources.ApplyResources(this.abstractTextBox, "abstractTextBox");
            this.abstractTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.abstractTextBox.Name = "abstractTextBox";
            this.abstractTextBox.ReadOnly = true;
            this.abstractTextBox.TabStop = false;
            // 
            // SensitivityAnalysisSettingDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.abstractTextBox);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.sensitivityRelativePerturbationTextBox);
            this.Controls.Add(this.sensitivityStepTextBox);
            this.Controls.Add(this.sensitivityAbsolutePerturbationTextBox);
            this.Controls.Add(label1);
            this.Name = "SensitivityAnalysisSettingDialog";
            this.Load += new System.EventHandler(this.FormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sensitivityStepTextBox;
        private System.Windows.Forms.TextBox sensitivityAbsolutePerturbationTextBox;
        private System.Windows.Forms.TextBox sensitivityRelativePerturbationTextBox;
        private System.Windows.Forms.ToolTip sensitivityToolTip;
        private System.Windows.Forms.TextBox abstractTextBox;
        private System.Windows.Forms.Button executeButton;
    }
}