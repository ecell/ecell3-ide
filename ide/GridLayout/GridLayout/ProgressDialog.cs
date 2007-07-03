using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.GridLayout
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
            get { return this.progressBar1; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressDialog()
        {
            InitializeComponent();
        }        
    }
}