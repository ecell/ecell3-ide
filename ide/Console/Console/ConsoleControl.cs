using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ecell.Plugin;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void AppendText(string text)
        {
            simText.Select(simText.TextLength, 0);
            simText.AppendText(text);
        }
    }
}
