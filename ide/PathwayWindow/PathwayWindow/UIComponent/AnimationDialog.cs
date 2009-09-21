﻿

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Animation;
using Ecell.Exceptions;

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
            AnimationItemBase item = (AnimationItemBase)listBox.SelectedItem;
            listBox.Items.Remove(item);

            panel.Controls.Clear();
            if(listBox.Items.Count > 0)
                panel.Controls.Add((AnimationItemBase)listBox.Items[0]);
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
            foreach (IAnimationItem item in this.Items)
            {
                item.ApplyChange();
            }
        }

        private void addPropertyViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IAnimationItem item in this.listBox.Items)
            {
                if (!(item is PropertyViewAnimationItem))
                    continue;
                this.panel.Controls.Clear();
                this.panel.Controls.Add((AnimationItemBase)item);
                return;
            }

            PropertyViewAnimationItem newItem = new PropertyViewAnimationItem(_control);
            AddItem(newItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsExistItem(Type type)
        {
            return false;
        }

        private void addMassCalculationAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IAnimationItem item in this.listBox.Items)
            {
                if (!(item is MassCalculationAnimationItem))
                    continue;
                this.panel.Controls.Clear();
                this.panel.Controls.Add((AnimationItemBase)item);
                return;
            }

            MassCalculationAnimationItem newItem = new MassCalculationAnimationItem(_control);
            AddItem(newItem);

        }

        private void addNodeAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void outputMovieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IAnimationItem item in this.listBox.Items)
            {
                if (!(item is MovieAnimationItem))
                    continue;
                this.panel.Controls.Clear();
                this.panel.Controls.Add((AnimationItemBase)item);
                return;
            }

            MovieAnimationItem newItem = new MovieAnimationItem(_control);
            AddItem(newItem);

        }
        private void addEdgeAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IAnimationItem item in this.listBox.Items)
            {
                if (!(item is EdgeAnimationItem))
                    continue;
                this.panel.Controls.Clear();
                this.panel.Controls.Add((AnimationItemBase)item);
                return;
            }

            EdgeAnimationItem newItem = new EdgeAnimationItem(_control);
            AddItem(newItem);
        }

        private void addVariableAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IAnimationItem item in this.listBox.Items)
            {
                if (!(item is EntityAnimationItem))
                    continue;
                this.panel.Controls.Clear();
                this.panel.Controls.Add((AnimationItemBase)item);
                return;
            }

            EntityAnimationItem newItem = new EntityAnimationItem(_control);
            AddItem(newItem);
        }

        private void addEntityGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IAnimationItem item in this.listBox.Items)
            {
                if (!(item is GraphAnimationItem))
                    continue;
                this.panel.Controls.Clear();
                this.panel.Controls.Add((AnimationItemBase)item);
                return;
            }

            GraphAnimationItem newItem = new GraphAnimationItem(_control);
            AddItem(newItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(IAnimationItem item)
        {
            AnimationItemBase obj = (AnimationItemBase)item;
            obj.SetViewItem();
            obj.Dock = DockStyle.Fill;
            this.listBox.Items.Add(obj);
            this.panel.Controls.Clear();
            this.panel.Controls.Add(obj);
        }

        private void AnimationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            // On Cancel
            if (this.DialogResult == DialogResult.Cancel)
            {
                this.panel.Controls.Clear();
                return;
            }

            try
            {
                foreach (IAnimationItem page in Items)
                    page.CheckParameters();
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
                e.Cancel = true;
                return;
            }
            this.panel.Controls.Clear();

        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            // Set BackGround
            e.DrawBackground();
            // Set Text Brush
            Brush brush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                brush = Brushes.White;
            }
            // Draw Text
            object item = ((ListBox)sender).Items[e.Index];
            string text = item.ToString();
            e.Graphics.DrawString(text, e.Font, brush, e.Bounds);

            e.DrawFocusRectangle();

        }

    }
}
