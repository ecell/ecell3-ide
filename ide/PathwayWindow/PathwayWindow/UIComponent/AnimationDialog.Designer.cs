﻿namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
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
            this.listBox.AccessibleDescription = null;
            this.listBox.AccessibleName = null;
            resources.ApplyResources(this.listBox, "listBox");
            this.listBox.BackgroundImage = null;
            this.listBox.Font = null;
            this.listBox.FormattingEnabled = true;
            this.listBox.Name = "listBox";
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.AccessibleDescription = null;
            this.buttonAdd.AccessibleName = null;
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.BackgroundImage = null;
            this.buttonAdd.Font = null;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.AccessibleDescription = null;
            this.buttonDelete.AccessibleName = null;
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.BackgroundImage = null;
            this.buttonDelete.Font = null;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelItems
            // 
            this.labelItems.AccessibleDescription = null;
            this.labelItems.AccessibleName = null;
            resources.ApplyResources(this.labelItems, "labelItems");
            this.labelItems.BackColor = System.Drawing.Color.Transparent;
            this.labelItems.Font = null;
            this.labelItems.Name = "labelItems";
            // 
            // panel
            // 
            this.panel.AccessibleDescription = null;
            this.panel.AccessibleName = null;
            resources.ApplyResources(this.panel, "panel");
            this.panel.BackgroundImage = null;
            this.panel.Font = null;
            this.panel.Name = "panel";
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleDescription = null;
            this.buttonOK.AccessibleName = null;
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.BackgroundImage = null;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Font = null;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleDescription = null;
            this.buttonCancel.AccessibleName = null;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.BackgroundImage = null;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = null;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // contextMenuAddItem
            // 
            this.contextMenuAddItem.AccessibleDescription = null;
            this.contextMenuAddItem.AccessibleName = null;
            resources.ApplyResources(this.contextMenuAddItem, "contextMenuAddItem");
            this.contextMenuAddItem.BackgroundImage = null;
            this.contextMenuAddItem.Font = null;
            this.contextMenuAddItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEdgeAnimationToolStripMenuItem,
            this.addNodeAnimationToolStripMenuItem,
            this.addPropertyViewToolStripMenuItem});
            this.contextMenuAddItem.Name = "contextMenuAddItem";
            // 
            // addEdgeAnimationToolStripMenuItem
            // 
            this.addEdgeAnimationToolStripMenuItem.AccessibleDescription = null;
            this.addEdgeAnimationToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.addEdgeAnimationToolStripMenuItem, "addEdgeAnimationToolStripMenuItem");
            this.addEdgeAnimationToolStripMenuItem.BackgroundImage = null;
            this.addEdgeAnimationToolStripMenuItem.Name = "addEdgeAnimationToolStripMenuItem";
            this.addEdgeAnimationToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // addNodeAnimationToolStripMenuItem
            // 
            this.addNodeAnimationToolStripMenuItem.AccessibleDescription = null;
            this.addNodeAnimationToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.addNodeAnimationToolStripMenuItem, "addNodeAnimationToolStripMenuItem");
            this.addNodeAnimationToolStripMenuItem.BackgroundImage = null;
            this.addNodeAnimationToolStripMenuItem.Name = "addNodeAnimationToolStripMenuItem";
            this.addNodeAnimationToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // addPropertyViewToolStripMenuItem
            // 
            this.addPropertyViewToolStripMenuItem.AccessibleDescription = null;
            this.addPropertyViewToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.addPropertyViewToolStripMenuItem, "addPropertyViewToolStripMenuItem");
            this.addPropertyViewToolStripMenuItem.BackgroundImage = null;
            this.addPropertyViewToolStripMenuItem.Name = "addPropertyViewToolStripMenuItem";
            this.addPropertyViewToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // AnimationDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.labelItems);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listBox);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnimationDialog";
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