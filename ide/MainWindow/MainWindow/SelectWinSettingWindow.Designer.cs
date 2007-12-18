namespace EcellLib.MainWindow
{
    partial class SelectWinSettingWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectWinSettingWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SWSPictureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.SWSSetting1RadioButton = new System.Windows.Forms.RadioButton();
            this.SWSSetting4RadioButton = new System.Windows.Forms.RadioButton();
            this.SWSSetting3RadioButton = new System.Windows.Forms.RadioButton();
            this.SWSSetting2RadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SWSNoteTextBox = new System.Windows.Forms.TextBox();
            this.SWSSelectButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SWSPictureBox)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SWSSelectButton, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.SWSPictureBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 2, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // SWSSetting1PictureBox
            // 
            resources.ApplyResources(this.SWSPictureBox, "SWSSetting1PictureBox");
            this.SWSPictureBox.Name = "SWSSetting1PictureBox";
            this.SWSPictureBox.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.SWSSetting1RadioButton, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.SWSSetting4RadioButton, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.SWSSetting3RadioButton, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.SWSSetting2RadioButton, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 5);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // SWSSetting1RadioButton
            // 
            resources.ApplyResources(this.SWSSetting1RadioButton, "SWSSetting1RadioButton");
            this.SWSSetting1RadioButton.Checked = true;
            this.SWSSetting1RadioButton.Name = "SWSSetting1RadioButton";
            this.SWSSetting1RadioButton.TabStop = true;
            this.SWSSetting1RadioButton.UseVisualStyleBackColor = true;
            this.SWSSetting1RadioButton.CheckedChanged += new System.EventHandler(this.ChangePatternRadioBox);
            // 
            // SWSSetting4RadioButton
            // 
            resources.ApplyResources(this.SWSSetting4RadioButton, "SWSSetting4RadioButton");
            this.SWSSetting4RadioButton.Name = "SWSSetting4RadioButton";
            this.SWSSetting4RadioButton.UseVisualStyleBackColor = true;
            this.SWSSetting4RadioButton.CheckedChanged += new System.EventHandler(this.ChangePatternRadioBox);
            // 
            // SWSSetting3RadioButton
            // 
            resources.ApplyResources(this.SWSSetting3RadioButton, "SWSSetting3RadioButton");
            this.SWSSetting3RadioButton.Name = "SWSSetting3RadioButton";
            this.SWSSetting3RadioButton.UseVisualStyleBackColor = true;
            this.SWSSetting3RadioButton.CheckedChanged += new System.EventHandler(this.ChangePatternRadioBox);
            // 
            // SWSSetting2RadioButton
            // 
            resources.ApplyResources(this.SWSSetting2RadioButton, "SWSSetting2RadioButton");
            this.SWSSetting2RadioButton.Name = "SWSSetting2RadioButton";
            this.SWSSetting2RadioButton.UseVisualStyleBackColor = true;
            this.SWSSetting2RadioButton.CheckedChanged += new System.EventHandler(this.ChangePatternRadioBox);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SWSNoteTextBox);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // SWSNoteTextBox
            // 
            this.SWSNoteTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.SWSNoteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.SWSNoteTextBox, "SWSNoteTextBox");
            this.SWSNoteTextBox.Name = "SWSNoteTextBox";
            this.SWSNoteTextBox.ReadOnly = true;
            // 
            // SWSSelectButton
            // 
            resources.ApplyResources(this.SWSSelectButton, "SWSSelectButton");
            this.SWSSelectButton.Name = "SWSSelectButton";
            this.SWSSelectButton.UseVisualStyleBackColor = true;
            this.SWSSelectButton.Click += new System.EventHandler(this.ClickSWSSelectButton);
            // 
            // SelectWinSettingWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SelectWinSettingWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SWSPictureBox)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button SWSSelectButton;
        private System.Windows.Forms.RadioButton SWSSetting4RadioButton;
        private System.Windows.Forms.RadioButton SWSSetting3RadioButton;
        private System.Windows.Forms.RadioButton SWSSetting2RadioButton;
        private System.Windows.Forms.RadioButton SWSSetting1RadioButton;
        private System.Windows.Forms.PictureBox SWSPictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox SWSNoteTextBox;
    }
}