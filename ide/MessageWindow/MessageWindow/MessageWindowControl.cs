using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.MessageWindow
{
    /// <summary>
    /// User Control for MessageWindow.
    /// </summary>
    public partial class MessageWindowControl : EcellDockContent
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageWindowControl()
        {

            base.m_isSavable = true;
            InitializeComponent();
            this.Name = "MessageWindow";
            this.Text = MessageResMessage.MessageWindow;
            this.TabText = this.Text;
        }
    }
}
