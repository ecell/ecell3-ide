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
            this.DMLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DMListBox = new System.Windows.Forms.ListBox();
            this.ProjectDMListBox = new System.Windows.Forms.ListBox();
            this.DMSelectLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DMAddAllButton = new System.Windows.Forms.Button();
            this.DMAddButon = new System.Windows.Forms.Button();
            this.DMRemoveButton = new System.Windows.Forms.Button();
            this.DMListLabel = new System.Windows.Forms.Label();
            this.ProjectDMLabel = new System.Windows.Forms.Label();
            this.ProjectLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.ProjectPatternList = new System.Windows.Forms.TableLayoutPanel();
            this.CommentBox = new System.Windows.Forms.GroupBox();
            this.ProjectCommentTextBox = new System.Windows.Forms.TextBox();
            this.ProjectBox = new System.Windows.Forms.GroupBox();
            this.ProjectIDTextBox = new System.Windows.Forms.TextBox();
            this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.DMLayoutPanel.SuspendLayout();
            this.DMSelectLayoutPanel.SuspendLayout();
            this.ProjectLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.ProjectPatternList.SuspendLayout();
            this.CommentBox.SuspendLayout();
            this.ProjectBox.SuspendLayout();
            this.MainLayoutPanel.SuspendLayout();
            this.ButtonLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DMLayoutPanel
            // 
            resources.ApplyResources(this.DMLayoutPanel, "DMLayoutPanel");
            this.DMLayoutPanel.Controls.Add(this.DMListBox, 1, 1);
            this.DMLayoutPanel.Controls.Add(this.ProjectDMListBox, 3, 1);
            this.DMLayoutPanel.Controls.Add(this.DMSelectLayoutPanel, 2, 1);
            this.DMLayoutPanel.Controls.Add(this.DMListLabel, 1, 0);
            this.DMLayoutPanel.Controls.Add(this.ProjectDMLabel, 3, 0);
            this.DMLayoutPanel.Name = "DMLayoutPanel";
            // 
            // DMListBox
            // 
            resources.ApplyResources(this.DMListBox, "DMListBox");
            this.DMListBox.FormattingEnabled = true;
            this.DMListBox.Name = "DMListBox";
            this.DMListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            // 
            // ProjectDMListBox
            // 
            resources.ApplyResources(this.ProjectDMListBox, "ProjectDMListBox");
            this.ProjectDMListBox.FormattingEnabled = true;
            this.ProjectDMListBox.Name = "ProjectDMListBox";
            this.ProjectDMListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            // 
            // DMSelectLayoutPanel
            // 
            resources.ApplyResources(this.DMSelectLayoutPanel, "DMSelectLayoutPanel");
            this.DMSelectLayoutPanel.Controls.Add(this.DMAddAllButton, 0, 1);
            this.DMSelectLayoutPanel.Controls.Add(this.DMAddButon, 0, 2);
            this.DMSelectLayoutPanel.Controls.Add(this.DMRemoveButton, 0, 3);
            this.DMSelectLayoutPanel.Name = "DMSelectLayoutPanel";
            // 
            // DMAddAllButton
            // 
            resources.ApplyResources(this.DMAddAllButton, "DMAddAllButton");
            this.DMAddAllButton.Name = "DMAddAllButton";
            this.DMAddAllButton.UseVisualStyleBackColor = true;
            this.DMAddAllButton.Click += new System.EventHandler(this.DMAddAll_Click);
            // 
            // DMAddButon
            // 
            resources.ApplyResources(this.DMAddButon, "DMAddButon");
            this.DMAddButon.Name = "DMAddButon";
            this.DMAddButon.UseVisualStyleBackColor = true;
            this.DMAddButon.Click += new System.EventHandler(this.DMAdd_Click);
            // 
            // DMRemoveButton
            // 
            resources.ApplyResources(this.DMRemoveButton, "DMRemoveButton");
            this.DMRemoveButton.Name = "DMRemoveButton";
            this.DMRemoveButton.UseVisualStyleBackColor = true;
            this.DMRemoveButton.Click += new System.EventHandler(this.DMRemove_Click);
            // 
            // DMListLabel
            // 
            resources.ApplyResources(this.DMListLabel, "DMListLabel");
            this.DMListLabel.Name = "DMListLabel";
            // 
            // ProjectDMLabel
            // 
            resources.ApplyResources(this.ProjectDMLabel, "ProjectDMLabel");
            this.ProjectDMLabel.Name = "ProjectDMLabel";
            // 
            // ProjectLayoutPanel
            // 
            resources.ApplyResources(this.ProjectLayoutPanel, "ProjectLayoutPanel");
            this.ProjectLayoutPanel.Controls.Add(this.PictureBox, 0, 0);
            this.ProjectLayoutPanel.Controls.Add(this.ProjectPatternList, 1, 0);
            this.ProjectLayoutPanel.Name = "ProjectLayoutPanel";
            // 
            // PictureBox
            // 
            this.PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.PictureBox, "PictureBox");
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.TabStop = false;
            // 
            // ProjectPatternList
            // 
            resources.ApplyResources(this.ProjectPatternList, "ProjectPatternList");
            this.ProjectPatternList.Controls.Add(this.CommentBox, 0, 6);
            this.ProjectPatternList.Controls.Add(this.ProjectBox, 0, 5);
            this.ProjectPatternList.Name = "ProjectPatternList";
            // 
            // CommentBox
            // 
            this.CommentBox.Controls.Add(this.ProjectCommentTextBox);
            resources.ApplyResources(this.CommentBox, "CommentBox");
            this.CommentBox.Name = "CommentBox";
            this.CommentBox.TabStop = false;
            // 
            // ProjectCommentTextBox
            // 
            this.ProjectCommentTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.ProjectCommentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.ProjectCommentTextBox, "ProjectCommentTextBox");
            this.ProjectCommentTextBox.Name = "ProjectCommentTextBox";
            this.ProjectCommentTextBox.ReadOnly = true;
            // 
            // ProjectBox
            // 
            this.ProjectBox.Controls.Add(this.ProjectIDTextBox);
            resources.ApplyResources(this.ProjectBox, "ProjectBox");
            this.ProjectBox.Name = "ProjectBox";
            this.ProjectBox.TabStop = false;
            // 
            // ProjectIDTextBox
            // 
            this.ProjectIDTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.ProjectIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.ProjectIDTextBox, "ProjectIDTextBox");
            this.ProjectIDTextBox.Name = "ProjectIDTextBox";
            this.ProjectIDTextBox.ReadOnly = true;
            // 
            // MainLayoutPanel
            // 
            resources.ApplyResources(this.MainLayoutPanel, "MainLayoutPanel");
            this.MainLayoutPanel.Controls.Add(this.DMLayoutPanel, 0, 1);
            this.MainLayoutPanel.Controls.Add(this.ProjectLayoutPanel, 0, 2);
            this.MainLayoutPanel.Controls.Add(this.ButtonLayoutPanel, 0, 3);
            this.MainLayoutPanel.Controls.Add(this.textBox1, 0, 0);
            this.MainLayoutPanel.Name = "MainLayoutPanel";
            // 
            // ButtonLayoutPanel
            // 
            resources.ApplyResources(this.ButtonLayoutPanel, "ButtonLayoutPanel");
            this.ButtonLayoutPanel.Controls.Add(this.CloseButton, 3, 0);
            this.ButtonLayoutPanel.Controls.Add(this.BackButton, 1, 0);
            this.ButtonLayoutPanel.Controls.Add(this.OKButton, 2, 0);
            this.ButtonLayoutPanel.Name = "ButtonLayoutPanel";
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // BackButton
            // 
            resources.ApplyResources(this.BackButton, "BackButton");
            this.BackButton.Name = "BackButton";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // OKButton
            // 
            resources.ApplyResources(this.OKButton, "OKButton");
            this.OKButton.Name = "OKButton";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);

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
            this.Controls.Add(this.MainLayoutPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectWizardWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.DMLayoutPanel.ResumeLayout(false);
            this.DMLayoutPanel.PerformLayout();
            this.DMSelectLayoutPanel.ResumeLayout(false);
            this.ProjectLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ProjectPatternList.ResumeLayout(false);
            this.CommentBox.ResumeLayout(false);
            this.CommentBox.PerformLayout();
            this.ProjectBox.ResumeLayout(false);
            this.ProjectBox.PerformLayout();
            this.MainLayoutPanel.ResumeLayout(false);
            this.MainLayoutPanel.PerformLayout();
            this.ButtonLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel DMLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ProjectLayoutPanel;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.TableLayoutPanel MainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ProjectPatternList;
        private System.Windows.Forms.GroupBox CommentBox;
        private System.Windows.Forms.TextBox ProjectCommentTextBox;
        private System.Windows.Forms.GroupBox ProjectBox;
        private System.Windows.Forms.TextBox ProjectIDTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox DMListBox;
        public System.Windows.Forms.ListBox ProjectDMListBox;
        private System.Windows.Forms.TableLayoutPanel DMSelectLayoutPanel;
        private System.Windows.Forms.Button DMAddAllButton;
        private System.Windows.Forms.Button DMAddButon;
        private System.Windows.Forms.Button DMRemoveButton;
        private System.Windows.Forms.Label DMListLabel;
        private System.Windows.Forms.Label ProjectDMLabel;
        private System.Windows.Forms.TableLayoutPanel ButtonLayoutPanel;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button OKButton;


    }
}