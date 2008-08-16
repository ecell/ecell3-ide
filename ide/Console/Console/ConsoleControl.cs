using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.Console
{
    /// <summary>
    /// User Control for MessageWindow.
    /// </summary>
    public partial class ConsoleControl : EcellDockContent
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleControl()
        {
            base.m_isSavable = true;
            InitializeComponent();
            this.TabText = this.Text;
        }

        public void AppendText(string text)
        {
            simText.AppendText(text);
        }
    }
}
