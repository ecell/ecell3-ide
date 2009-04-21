using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ecell.IDE;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.TracerWindow
{
    public partial class TracerConfigurationDialog : Form
    {
        private double m_plotNumber;
        private double m_intervalSecond;
        private int m_stepNumber;
        private ValueDataFormat m_dataformat;
        /// <summary>
        /// 
        /// </summary>
        public double PlotNumber
        {
            get { return m_plotNumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double IntervalSecond
        {
            get { return m_intervalSecond; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StepNumber
        {
            get { return m_stepNumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DataFormat
        {
            get
            {
                switch (m_dataformat)
                {
                    case ValueDataFormat.Normal:
                        return "G";
                    case ValueDataFormat.Exponential1:
                        return "e1";
                    case ValueDataFormat.Exponential2:
                        return "e2";
                    case ValueDataFormat.Exponential3:
                        return "e3";
                    case ValueDataFormat.Exponential4:
                        return "e4";
                    case ValueDataFormat.Exponential5:
                        return "e5";
                    default:
                        return "G";
                }
            }
            set
            {
                switch (value)
                {
                    case "G":
                        m_dataformat = ValueDataFormat.Normal;
                        valueFormatComboBox.SelectedIndex = 0;
                        break;
                    case "e1":
                        m_dataformat = ValueDataFormat.Exponential1;
                        valueFormatComboBox.SelectedIndex = 1;
                        break;
                    case "e2":
                        m_dataformat = ValueDataFormat.Exponential2;
                        valueFormatComboBox.SelectedIndex = 2;
                        break;
                    case "e3":
                        m_dataformat = ValueDataFormat.Exponential3;
                        valueFormatComboBox.SelectedIndex = 3;
                        break;
                    case "e4":
                        m_dataformat = ValueDataFormat.Exponential4;
                        valueFormatComboBox.SelectedIndex = 4;
                        break;
                    case "e5":
                        m_dataformat = ValueDataFormat.Exponential5;
                        valueFormatComboBox.SelectedIndex = 5;
                        break;
                    default:
                        m_dataformat = ValueDataFormat.Normal;
                        break;
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TracerConfigurationDialog(double number, double interval, int step, string format)
        {
            InitializeComponent();
            numberTextBox.Text = Convert.ToString(number);
            intervalTextBox.Text = Convert.ToString(interval);
            stepCountTextBox.Text = Convert.ToString(step);
            m_plotNumber = number;
            m_intervalSecond = interval;
            m_stepNumber = step;
            DataFormat = format;
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
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePlotNumber));
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
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRedrawInterval));
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
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameStepInterval));
                stepCountTextBox.Text = Convert.ToString(m_stepNumber);
                e.Cancel = true;
                return;
            }
            m_stepNumber = dummy;
        }

        private void DataFormatChanged(object sender, EventArgs e)
        {
            int ind = valueFormatComboBox.SelectedIndex;
            if (ind == 0)
                m_dataformat = ValueDataFormat.Normal;
            else if (ind == 1)
                m_dataformat = ValueDataFormat.Exponential1;
            else if (ind == 2)
                m_dataformat = ValueDataFormat.Exponential2;
            else if (ind == 3)
                m_dataformat = ValueDataFormat.Exponential3;
            else if (ind == 4)
                m_dataformat = ValueDataFormat.Exponential4;
            else if (ind == 5)
                m_dataformat = ValueDataFormat.Exponential5;
        }

        private void TracerConfigurationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (m_stepNumber < 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameStepInterval));
                e.Cancel = true;
                return;
            }
            if (m_intervalSecond <= 0.0 || m_intervalSecond > 3600.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRedrawInterval));
                e.Cancel = true;
                return;
            }
            if (m_plotNumber < 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePlotNumber));
                e.Cancel = true;
                return;
            }
        }

        private void TracerConfigurationDialog_Load(object sender, EventArgs e)
        {
            graphSettingToolTip.SetToolTip(numberTextBox,
                String.Format(MessageResources.CommonToolTipIntMoreThan, 0));
            graphSettingToolTip.SetToolTip(stepCountTextBox,
                String.Format(MessageResources.CommonToolTipIntMoreThan, 0));
            graphSettingToolTip.SetToolTip(intervalTextBox,
                string.Format(MessageResources.CommonToolTipRange, 0, 3600));
        }
    }
}