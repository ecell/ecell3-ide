namespace EcellLib.MainWindow
{
    partial class ProjectWizardWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectWizardWindow));
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.SWSPatternListLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ProjectCommentTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ProjectIDTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.SWSPatternListLayoutPanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.CloseButton, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.OKButton, 2, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OKButton
            // 
            resources.ApplyResources(this.OKButton, "OKButton");
            this.OKButton.Name = "OKButton";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.PictureBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.SWSPatternListLayoutPanel, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // PictureBox
            // 
            this.PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.PictureBox, "PictureBox");
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.TabStop = false;
            // 
            // SWSPatternListLayoutPanel
            // 
            resources.ApplyResources(this.SWSPatternListLayoutPanel, "SWSPatternListLayoutPanel");
            this.SWSPatternListLayoutPanel.Controls.Add(this.groupBox3, 0, 6);
            this.SWSPatternListLayoutPanel.Controls.Add(this.groupBox1, 0, 5);
            this.SWSPatternListLayoutPanel.Name = "SWSPatternListLayoutPanel";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ProjectCommentTextBox);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // ProjectCommentTextBox
            // 
            this.ProjectCommentTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.ProjectCommentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.ProjectCommentTextBox, "ProjectCommentTextBox");
            this.ProjectCommentTextBox.Name = "ProjectCommentTextBox";
            this.ProjectCommentTextBox.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ProjectIDTextBox);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // ProjectIDTextBox
            // 
            this.ProjectIDTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.ProjectIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.ProjectIDTextBox, "ProjectIDTextBox");
            this.ProjectIDTextBox.Name = "ProjectIDTextBox";
            this.ProjectIDTextBox.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // ProjectWizardWindow
            // 
            this.AcceptButton = this.OKButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectWizardWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.SWSPatternListLayoutPanel.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TableLayoutPanel SWSPatternListLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox ProjectCommentTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ProjectIDTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;


    }
}