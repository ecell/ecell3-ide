using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ToolLauncher
{
    public partial class PropertyForm : Form
    {
        private Util util = null;

        public PropertyForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonEcell_Click(object sender, EventArgs e)
        {
            if (this.fbdEcell.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxEcell.Text = this.fbdEcell.SelectedPath;
            }
        }

        private void buttonBoost_Click(object sender, EventArgs e)
        {
            if (this.fbdBoost.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxBoost.Text = this.fbdBoost.SelectedPath;
            }
        }

        private void buttonGSL_Click(object sender, EventArgs e)
        {
            if (this.fbdGSL.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxGSL.Text = this.fbdGSL.SelectedPath;
            }
        }

        private void buttonVC_Click(object sender, EventArgs e)
        {
            if (this.fbdVC.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxVC.Text = this.fbdVC.SelectedPath;
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            this.textBoxBoost.Text = this.util.RootPathBoost;
            this.textBoxEcell.Text = this.util.RootPathEcell;
            this.textBoxGSL.Text = this.util.RootPathGSL;
            this.textBoxVC.Text = this.util.RootPathVC;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.util.OverwriteInitFile(
                    this.textBoxBoost.Text, this.textBoxEcell.Text, this.textBoxGSL.Text, this.textBoxVC.Text);
            this.Close();
        }

        private void Initialize()
        {
            this.util = Util.GetInstance();
            this.textBoxBoost.Text = this.util.RootPathBoost;
            this.textBoxEcell.Text = this.util.RootPathEcell;
            this.textBoxGSL.Text = this.util.RootPathGSL;
            this.textBoxVC.Text = this.util.RootPathVC;
        }
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
