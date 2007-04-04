using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GridLayout
{
    public partial class ProgressDialog : Form
    {
        public ProgressBar Bar
        {
            get { return this.progressBar1; }
        }

        public ProgressDialog()
        {
            InitializeComponent();
        }        
    }
}