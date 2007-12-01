using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Form for progress bar
    /// </summary>
    public partial class ProgressDialog : Form
    {
        /// <summary>
        /// Accessor for ProgressBar.
        /// </summary>
        public ProgressBar Bar
        {
            get { return this.progressBar; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressDialog(int min, int max)
        {
            InitializeComponent();
            Bar.Minimum = min;
            Bar.Maximum = max;
            Bar.Value = min;
            Bar.Step = 1;
        }
    }
}