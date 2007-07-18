using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.TracerWindow
{
    public partial class TracerWindowSetup : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TracerWindowSetup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The event when user click close button.
        /// </summary>
        /// <param name="sender">close button.</param>
        /// <param name="e">EventArgs.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// The event when user return enter key in TextBox.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">KeyPressEventArgs.</param>
        private void EnterKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                TSApplyButton.PerformClick();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                TSCloseButton.PerformClick();
            }
        }

        /// <summary>
        /// The event to show this window.
        /// </summary>
        /// <param name="sender">this window.</param>
        /// <param name="e">EventArgs.</param>
        private void TracerWinSetupShown(object sender, EventArgs e)
        {
            this.numberTextBox.Focus();
        }
    }
}