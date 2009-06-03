﻿

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

        /// <summary>
        /// 
        /// </summary>
        public List<IAnimationItem> Items
        {
            get
            {
                List<IAnimationItem> list = new List<IAnimationItem>();
                foreach (IAnimationItem item in listBox.Items)
                {
                    list.Add(item);
                }
                return list;
            }
        }

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
            if (listBox.SelectedIndex < 0)
                return;

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Point point = this.Location;
            point.Offset(this.buttonAdd.Location);
            contextMenuAddItem.Show(point);
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
            EdgeAnimatioinItem item = new EdgeAnimatioinItem(_control);
            AddItem(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(IAnimationItem item)
        {
            AnimationItemBase obj = (AnimationItemBase)item;
            obj.SetViewItem();
            _control.Items.Add(obj);
            this.listBox.Items.Add(obj);
            this.panel.Controls.Clear();
            this.panel.Controls.Add(obj);
        }

        private void AnimationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.panel.Controls.Clear();
        }
    }
}
