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
            m_plotNumber = number;
            m_intervalSecond = interval;
            m_stepNumber = step;
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

        private void PlotNumber_Validating(object sender, CancelEventArgs e)
        {
            string text = numberTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NamePlotNumber));
                numberTextBox.Text = Convert.ToString(m_plotNumber);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy) || dummy <= 0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                numberTextBox.Text = Convert.ToString(m_plotNumber);
                e.Cancel = true;
                return;
            }
            m_plotNumber = dummy;
        }

        private void RedrawInterval_Validating(object sender, CancelEventArgs e)
        {
            string text = intervalTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameRedrawInterval));
                intervalTextBox.Text = Convert.ToString(m_intervalSecond);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy) || dummy <= 0.0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                intervalTextBox.Text = Convert.ToString(m_intervalSecond);
                e.Cancel = true;
                return;
            }
            m_intervalSecond = dummy;
        }

        private void StepCount_Validating(object sender, CancelEventArgs e)
        {
            string text = stepCountTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameStepInterval));
                stepCountTextBox.Text = Convert.ToString(m_stepNumber);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy) || dummy <= 0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                stepCountTextBox.Text = Convert.ToString(m_stepNumber);
                e.Cancel = true;
                return;
            }
            m_stepNumber = dummy;
        }

    }
}