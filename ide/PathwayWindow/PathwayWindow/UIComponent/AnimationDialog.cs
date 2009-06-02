

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Animation;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AnimationDialog : Form
    {
        private AnimationControl _control;

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public AnimationDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public AnimationDialog(AnimationControl control)
            : this()
        {
            this._control = control;
            List<IAnimationItem> items =  control.Items;
            foreach (IAnimationItem item in items)
            {
                item.SetViewItem();
                listBox.Items.Add(item);
            }
            if (items.Count > 0)
                this.panel.Controls.Add((AnimationItemBase)items[0]);
        }
        #endregion

        #region Events
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Controls.Add((AnimationItemBase)listBox.SelectedItem);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            contextMenuAddItem.Show(this.buttonAdd.Location);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        internal void ApplyChange()
        {
            foreach (IAnimationItem item in _control.Items)
            {
                item.ApplyChange();
            }
        }

        private void addPropertyViewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addNodeAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addEdgeAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItem(new EdgeAnimatioinItem(_control));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(IAnimationItem item)
        {
            AnimationItemBase obj = (AnimationItemBase)item;
            _control.Items.Add(obj);
            this.listBox.Items.Add(obj);
            this.panel.Controls.Clear();
            this.panel.Controls.Add(obj);
        }

        private void AnimationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.listBox.Items.Clear();
            this.panel.Controls.Clear();
        }
    }
}
