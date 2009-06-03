namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    partial class AnimationDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationDialog));
            this.listBox = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelItems = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.contextMenuAddItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEdgeAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPropertyViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuAddItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox
            // 
            resources.ApplyResources(this.listBox, "listBox");
            this.listBox.FormattingEnabled = true;
            this.listBox.Name = "listBox";
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelItems
            // 
            resources.ApplyResources(this.labelItems, "labelItems");
            this.labelItems.BackColor = System.Drawing.Color.Transparent;
            this.labelItems.Name = "labelItems";
            // 
            // panel
            // 
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // contextMenuAddItem
            // 
            this.contextMenuAddItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEdgeAnimationToolStripMenuItem,
            this.addNodeAnimationToolStripMenuItem,
            this.addPropertyViewToolStripMenuItem});
            this.contextMenuAddItem.Name = "contextMenuAddItem";
            resources.ApplyResources(this.contextMenuAddItem, "contextMenuAddItem");
            // 
            // addEdgeAnimationToolStripMenuItem
            // 
            this.addEdgeAnimationToolStripMenuItem.Name = "addEdgeAnimationToolStripMenuItem";
            resources.ApplyResources(this.addEdgeAnimationToolStripMenuItem, "addEdgeAnimationToolStripMenuItem");
            this.addEdgeAnimationToolStripMenuItem.Click += new System.EventHandler(this.addEdgeAnimationToolStripMenuItem_Click);
            // 
            // addNodeAnimationToolStripMenuItem
            // 
            this.addNodeAnimationToolStripMenuItem.Name = "addNodeAnimationToolStripMenuItem";
            resources.ApplyResources(this.addNodeAnimationToolStripMenuItem, "addNodeAnimationToolStripMenuItem");
            this.addNodeAnimationToolStripMenuItem.Click += new System.EventHandler(this.addNodeAnimationToolStripMenuItem_Click);
            // 
            // addPropertyViewToolStripMenuItem
            // 
            this.addPropertyViewToolStripMenuItem.Name = "addPropertyViewToolStripMenuItem";
            resources.ApplyResources(this.addPropertyViewToolStripMenuItem, "addPropertyViewToolStripMenuItem");
            this.addPropertyViewToolStripMenuItem.Click += new System.EventHandler(this.addPropertyViewToolStripMenuItem_Click);
            // 
            // AnimationDialog
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.labelItems);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnimationDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnimationDialog_FormClosing);
            this.contextMenuAddItem.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelItems;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ContextMenuStrip contextMenuAddItem;
        private System.Windows.Forms.ToolStripMenuItem addEdgeAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNodeAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPropertyViewToolStripMenuItem;
    }
}