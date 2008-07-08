namespace Ecell.IDE.Plugins.Analysis
{
    partial class PEAdvancedWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PEAdvancedWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.PEAApplyButton = new System.Windows.Forms.Button();
            this.PEACloseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.PEUpsilonTextBox = new System.Windows.Forms.TextBox();
            this.PEMTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PEM0TextBox = new System.Windows.Forms.TextBox();
            this.PEKTextBox = new System.Windows.Forms.TextBox();
            this.PEMaxRateTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.PEAApplyButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.PEACloseButton, 2, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // PEAApplyButton
            // 
            resources.ApplyResources(this.PEAApplyButton, "PEAApplyButton");
            this.PEAApplyButton.Name = "PEAApplyButton";
            this.PEAApplyButton.UseVisualStyleBackColor = true;
            // 
            // PEACloseButton
            // 
            resources.ApplyResources(this.PEACloseButton, "PEACloseButton");
            this.PEACloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.PEACloseButton.Name = "PEACloseButton";
            this.PEACloseButton.UseVisualStyleBackColor = true;
            this.PEACloseButton.Click += new System.EventHandler(this.PEACloseButtonClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.PEUpsilonTextBox, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.PEMTextBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // PEUpsilonTextBox
            // 
            resources.ApplyResources(this.PEUpsilonTextBox, "PEUpsilonTextBox");
            this.PEUpsilonTextBox.Name = "PEUpsilonTextBox";
            // 
            // PEMTextBox
            // 
            resources.ApplyResources(this.PEMTextBox, "PEMTextBox");
            this.PEMTextBox.Name = "PEMTextBox";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.PEM0TextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.PEKTextBox, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.PEMaxRateTextBox, 1, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // PEM0TextBox
            // 
            resources.ApplyResources(this.PEM0TextBox, "PEM0TextBox");
            this.PEM0TextBox.Name = "PEM0TextBox";
            // 
            // PEKTextBox
            // 
            resources.ApplyResources(this.PEKTextBox, "PEKTextBox");
            this.PEKTextBox.Name = "PEKTextBox";
            // 
            // PEMaxRateTextBox
            // 
            resources.ApplyResources(this.PEMaxRateTextBox, "PEMaxRateTextBox");
            this.PEMaxRateTextBox.Name = "PEMaxRateTextBox";
            // 
            // PEAdvancedWindow
            // 
            this.AcceptButton = this.PEAApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.PEACloseButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PEAdvancedWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button PEACloseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PEM0TextBox;
        private System.Windows.Forms.TextBox PEKTextBox;
        private System.Windows.Forms.TextBox PEMaxRateTextBox;
        private System.Windows.Forms.TextBox PEUpsilonTextBox;
        private System.Windows.Forms.TextBox PEMTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        /// <summary>
        /// Apply button.
        /// </summary>
        public System.Windows.Forms.Button PEAApplyButton;
    }
}