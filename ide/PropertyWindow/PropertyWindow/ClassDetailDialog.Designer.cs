namespace Ecell.IDE.Plugins.PropertyWindow
{
    partial class ClassDetailDialog
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
            System.Windows.Forms.Button ClassOKButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassDetailDialog));
            this.classRichTextBox = new System.Windows.Forms.RichTextBox();
            ClassOKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ClassOKButton
            // 
            resources.ApplyResources(ClassOKButton, "ClassOKButton");
            ClassOKButton.Name = "ClassOKButton";
            ClassOKButton.UseVisualStyleBackColor = true;
            // 
            // classRichTextBox
            // 
            resources.ApplyResources(this.classRichTextBox, "classRichTextBox");
            this.classRichTextBox.Name = "classRichTextBox";
            // 
            // ClassDetailDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.classRichTextBox);
            this.Controls.Add(ClassOKButton);
            this.Name = "ClassDetailDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox classRichTextBox;
    }
}