namespace Ecell.IDE.MainWindow
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
            this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.ProjectPanel = new Ecell.IDE.MainWindow.UIComponents.PWProjectPanel();
            this.DMPanel = new Ecell.IDE.MainWindow.UIComponents.PWDMPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MainLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayoutPanel
            // 
            resources.ApplyResources(this.MainLayoutPanel, "MainLayoutPanel");
            this.MainLayoutPanel.Controls.Add(this.textBox1, 0, 0);
            this.MainLayoutPanel.Controls.Add(this.ProjectPanel, 0, 1);
            this.MainLayoutPanel.Controls.Add(this.panel1, 0, 2);
            this.MainLayoutPanel.Name = "MainLayoutPanel";
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
            this.OKButton.Click += new System.EventHandler(this.GoNext_Click);
            // 
            // ProjectPanel
            // 
            resources.ApplyResources(this.ProjectPanel, "ProjectPanel");
            this.ProjectPanel.Name = "ProjectPanel";
            this.ProjectPanel.Project = null;
            this.ProjectPanel.ProjectChange += new System.EventHandler(this.ProjectPanel_ProjectChange);
            // 
            // DMPanel
            // 
            resources.ApplyResources(this.DMPanel, "DMPanel");
            this.DMPanel.Name = "DMPanel";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CloseButton);
            this.panel1.Controls.Add(this.BackButton);
            this.panel1.Controls.Add(this.OKButton);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // ProjectWizardWindow
            // 
            this.AcceptButton = this.OKButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.Controls.Add(this.MainLayoutPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectWizardWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectWizardWindow_FormClosing);
            this.MainLayoutPanel.ResumeLayout(false);
            this.MainLayoutPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayoutPanel;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button OKButton;
        private Ecell.IDE.MainWindow.UIComponents.PWProjectPanel ProjectPanel;
        private Ecell.IDE.MainWindow.UIComponents.PWDMPanel DMPanel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;

    }
}