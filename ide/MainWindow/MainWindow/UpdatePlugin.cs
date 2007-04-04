using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    public partial class UpdatePlugin : Form
    {
        public UpdatePlugin()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            SelectUpdatePlugin p = new SelectUpdatePlugin();
            p.ShowDialog();
        }
    }
}