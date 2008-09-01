using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.TracerWindow
{
    public partial class TracerConfigurationDialog : Form
    {
        private double m_plotNumber;
        private double m_intervalSecond;
        private int m_stepNumber;

        public double PlotNumber
        {
            get { return m_plotNumber; }
        }

        public double IntervalSecond
        {
            get { return m_intervalSecond; }
        }

        public int StepNumber
        {
            get { return m_stepNumber; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TracerConfigurationDialog(double number, double interval, int step)
        {
            InitializeComponent();
            numberTextBox.Text = Convert.ToString(number);
            intervalTextBox.Text = Convert.ToString(interval);
            stepCountTextBox.Text = Convert.ToString(step);
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

        private void TracerWindowSetup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK) return;
            try
            {
                m_plotNumber = Convert.ToDouble(numberTextBox.Text);
                m_intervalSecond = Convert.ToDouble(intervalTextBox.Text);
                m_stepNumber = Convert.ToInt32(stepCountTextBox.Text);

            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrInputData);
                e.Cancel = true;
            }
        }
    }
}