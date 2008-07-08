namespace Ecell.IDE.MainWindow
{
    partial class InitialPreferencesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialPreferencesDialog));
            this.SWSPictureBox = new System.Windows.Forms.PictureBox();
            this.SWSPatternListLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SWSNoteTextBox = new System.Windows.Forms.TextBox();
            this.SILangGroupBox = new System.Windows.Forms.GroupBox();
            this.SIEnglishRadioButton = new System.Windows.Forms.RadioButton();
            this.SIJapaneseRadioButton = new System.Windows.Forms.RadioButton();
            this.SIAutoRadioButton = new System.Windows.Forms.RadioButton();
            this.SWSSelectButton = new System.Windows.Forms.Button();
            this.SWSCloseButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SWSPictureBox)).BeginInit();
            this.SILangGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SWSPictureBox
            // 
            resources.ApplyResources(this.SWSPictureBox, "SWSPictureBox");
            this.SWSPictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SWSPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SWSPictureBox.Name = "SWSPictureBox";
            this.SWSPictureBox.TabStop = false;
            // 
            // SWSPatternListLayoutPanel
            // 
            resources.ApplyResources(this.SWSPatternListLayoutPanel, "SWSPatternListLayoutPanel");
            this.SWSPatternListLayoutPanel.Name = "SWSPatternListLayoutPanel";
            // 
            // SWSNoteTextBox
            // 
            resources.ApplyResources(this.SWSNoteTextBox, "SWSNoteTextBox");
            this.SWSNoteTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.SWSNoteTextBox.Name = "SWSNoteTextBox";
            this.SWSNoteTextBox.ReadOnly = true;
            // 
            // SILangGroupBox
            // 
            resources.ApplyResources(this.SILangGroupBox, "SILangGroupBox");
            this.SILangGroupBox.Controls.Add(this.SIEnglishRadioButton);
            this.SILangGroupBox.Controls.Add(this.SIJapaneseRadioButton);
            this.SILangGroupBox.Controls.Add(this.SIAutoRadioButton);
            this.SILangGroupBox.Name = "SILangGroupBox";
            this.SILangGroupBox.TabStop = false;
            // 
            // SIEnglishRadioButton
            // 
            resources.ApplyResources(this.SIEnglishRadioButton, "SIEnglishRadioButton");
            this.SIEnglishRadioButton.Name = "SIEnglishRadioButton";
            this.SIEnglishRadioButton.TabStop = true;
            this.SIEnglishRadioButton.UseVisualStyleBackColor = true;
            // 
            // SIJapaneseRadioButton
            // 
            resources.ApplyResources(this.SIJapaneseRadioButton, "SIJapaneseRadioButton");
            this.SIJapaneseRadioButton.Name = "SIJapaneseRadioButton";
            this.SIJapaneseRadioButton.TabStop = true;
            this.SIJapaneseRadioButton.UseVisualStyleBackColor = true;
            // 
            // SIAutoRadioButton
            // 
            resources.ApplyResources(this.SIAutoRadioButton, "SIAutoRadioButton");
            this.SIAutoRadioButton.Name = "SIAutoRadioButton";
            this.SIAutoRadioButton.TabStop = true;
            this.SIAutoRadioButton.UseVisualStyleBackColor = true;
            // 
            // SWSSelectButton
            // 
            resources.ApplyResources(this.SWSSelectButton, "SWSSelectButton");
            this.SWSSelectButton.Name = "SWSSelectButton";
            this.SWSSelectButton.UseVisualStyleBackColor = true;
            this.SWSSelectButton.Click += new System.EventHandler(this.ClickSWSSelectButton);
            // 
            // SWSCloseButton
            // 
            resources.ApplyResources(this.SWSCloseButton, "SWSCloseButton");
            this.SWSCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SWSCloseButton.Name = "SWSCloseButton";
            this.SWSCloseButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.SWSNoteTextBox);
            this.groupBox2.Controls.Add(this.SWSPictureBox);
            this.groupBox2.Controls.Add(this.SWSPatternListLayoutPanel);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // InitialPreferencesDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.SWSSelectButton);
            this.Controls.Add(this.SWSCloseButton);
            this.Controls.Add(this.SILangGroupBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitialPreferencesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.SWSPictureBox)).EndInit();
            this.SILangGroupBox.ResumeLayout(false);
            this.SILangGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SWSSelectButton;
        private System.Windows.Forms.PictureBox SWSPictureBox;
        private System.Windows.Forms.TableLayoutPanel SWSPatternListLayoutPanel;
        private System.Windows.Forms.TextBox SWSNoteTextBox;
        private System.Windows.Forms.GroupBox SILangGroupBox;
        private System.Windows.Forms.RadioButton SIAutoRadioButton;
        private System.Windows.Forms.RadioButton SIEnglishRadioButton;
        private System.Windows.Forms.RadioButton SIJapaneseRadioButton;
        private System.Windows.Forms.Button SWSCloseButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}