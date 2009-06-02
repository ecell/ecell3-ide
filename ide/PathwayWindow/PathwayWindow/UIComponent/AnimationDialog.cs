

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
            foreach (IAnimationItem item in control.Items)
            {
                listBox.Items.Add(item);
            }
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

        internal void ApplyChange()
        {

        }
    }
}
