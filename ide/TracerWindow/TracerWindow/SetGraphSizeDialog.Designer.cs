namespace Ecell.IDE.Plugins.TracerWindow
{
    partial class SetGraphSizeDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button cancelButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetGraphSizeDialog));
            this.okButton = new System.Windows.Forms.Button();
            this.yMaxTextBox = new System.Windows.Forms.TextBox();
            this.yMinTextBox = new System.Windows.Forms.TextBox();
            this.yMaxAutoCheckBox = new System.Windows.Forms.CheckBox();
            this.yMinAutoCheckBox = new System.Windows.Forms.CheckBox();
            this.defaultCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.y2MinTextBox = new System.Windows.Forms.TextBox();
            this.y2MaxTextBox = new System.Windows.Forms.TextBox();
            this.y2MinAutoCheckBox = new System.Windows.Forms.CheckBox();
            this.y2MaxAutoCheckBox = new System.Windows.Forms.CheckBox();
            cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(cancelButton, "cancelButton");
            cancelButton.Name = "cancelButton";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // yMaxTextBox
            // 
            resources.ApplyResources(this.yMaxTextBox, "yMaxTextBox");
            this.yMaxTextBox.Name = "yMaxTextBox";
            this.yMaxTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.YMaxValidating);
            // 
            // yMinTextBox
            // 
            resources.ApplyResources(this.yMinTextBox, "yMinTextBox");
            this.yMinTextBox.Name = "yMinTextBox";
            this.yMinTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.YMinValidating);
            // 
            // yMaxAutoCheckBox
            // 
            resources.ApplyResources(this.yMaxAutoCheckBox, "yMaxAutoCheckBox");
            this.yMaxAutoCheckBox.Name = "yMaxAutoCheckBox";
            this.yMaxAutoCheckBox.UseVisualStyleBackColor = true;
            this.yMaxAutoCheckBox.CheckedChanged += new System.EventHandler(this.MaxAutoCheckChanged);
            // 
            // yMinAutoCheckBox
            // 
            resources.ApplyResources(this.yMinAutoCheckBox, "yMinAutoCheckBox");
            this.yMinAutoCheckBox.Name = "yMinAutoCheckBox";
            this.yMinAutoCheckBox.UseVisualStyleBackColor = true;
            this.yMinAutoCheckBox.CheckedChanged += new System.EventHandler(this.MinAutoCheckChanged);
            // 
            // defaultCheckBox
            // 
            resources.ApplyResources(this.defaultCheckBox, "defaultCheckBox");
            this.defaultCheckBox.Name = "defaultCheckBox";
            this.defaultCheckBox.UseVisualStyleBackColor = true;
            this.defaultCheckBox.CheckedChanged += new System.EventHandler(this.DefaultCheckChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.yMaxAutoCheckBox);
            this.groupBox1.Controls.Add(this.yMinAutoCheckBox);
            this.groupBox1.Controls.Add(this.yMinTextBox);
            this.groupBox1.Controls.Add(this.yMaxTextBox);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.y2MinTextBox);
            this.groupBox2.Controls.Add(this.y2MaxTextBox);
            this.groupBox2.Controls.Add(this.y2MinAutoCheckBox);
            this.groupBox2.Controls.Add(this.y2MaxAutoCheckBox);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // y2MinTextBox
            // 
            resources.ApplyResources(this.y2MinTextBox, "y2MinTextBox");
            this.y2MinTextBox.Name = "y2MinTextBox";
            // 
            // y2MaxTextBox
            // 
            resources.ApplyResources(this.y2MaxTextBox, "y2MaxTextBox");
            this.y2MaxTextBox.Name = "y2MaxTextBox";
            this.y2MaxTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Y2MaxValidating);
            // 
            // y2MinAutoCheckBox
            // 
            resources.ApplyResources(this.y2MinAutoCheckBox, "y2MinAutoCheckBox");
            this.y2MinAutoCheckBox.Name = "y2MinAutoCheckBox";
            this.y2MinAutoCheckBox.UseVisualStyleBackColor = true;
            this.y2MinAutoCheckBox.CheckedChanged += new System.EventHandler(this.MinAuto2CheckChanged);
            // 
            // y2MaxAutoCheckBox
            // 
            resources.ApplyResources(this.y2MaxAutoCheckBox, "y2MaxAutoCheckBox");
            this.y2MaxAutoCheckBox.Name = "y2MaxAutoCheckBox";
            this.y2MaxAutoCheckBox.UseVisualStyleBackColor = true;
            this.y2MaxAutoCheckBox.CheckedChanged += new System.EventHandler(this.MaxAuto2CheckChanged);
            // 
            // SetGraphSizeDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cancelButton;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.defaultCheckBox);
            this.Controls.Add(cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "SetGraphSizeDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphSizeDialogClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox yMaxTextBox;
        private System.Windows.Forms.TextBox yMinTextBox;
        private System.Windows.Forms.CheckBox yMaxAutoCheckBox;
        private System.Windows.Forms.CheckBox yMinAutoCheckBox;
        private System.Windows.Forms.CheckBox defaultCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox y2MinTextBox;
        private System.Windows.Forms.TextBox y2MaxTextBox;
        private System.Windows.Forms.CheckBox y2MinAutoCheckBox;
        private System.Windows.Forms.CheckBox y2MaxAutoCheckBox;
    }
}