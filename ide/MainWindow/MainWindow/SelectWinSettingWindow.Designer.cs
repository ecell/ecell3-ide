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
            this.SWSPatternListLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SWSNoteTextBox = new System.Windows.Forms.TextBox();
            this.SWSSelectButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SWSPictureBox)).BeginInit();
            this.SWSPatternListLayoutPanel.SuspendLayout();
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
            this.tableLayoutPanel2.Controls.Add(this.SWSPatternListLayoutPanel, 2, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // SWSPictureBox
            // 
            resources.ApplyResources(this.SWSPictureBox, "SWSPictureBox");
            this.SWSPictureBox.Name = "SWSPictureBox";
            this.SWSPictureBox.TabStop = false;
            // 
            // SWSPatternListLayoutPanel
            // 
            resources.ApplyResources(this.SWSPatternListLayoutPanel, "SWSPatternListLayoutPanel");
            this.SWSPatternListLayoutPanel.Controls.Add(this.groupBox1, 0, 6);
            this.SWSPatternListLayoutPanel.Name = "SWSPatternListLayoutPanel";
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
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectWinSettingWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SWSPictureBox)).EndInit();
            this.SWSPatternListLayoutPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button SWSSelectButton;
        private System.Windows.Forms.PictureBox SWSPictureBox;
        private System.Windows.Forms.TableLayoutPanel SWSPatternListLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox SWSNoteTextBox;
    }
}