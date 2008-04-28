using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MessageWindow
{
    /// <summary>
    /// User Control for MessageWindow.
    /// </summary>
    public partial class MessageWindowControl : EcellDockContent
    {
        /// <summary>
        /// 
        /// </summary>
        private static ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResMessage));

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageWindowControl()
        {

            base.m_isSavable = true;
            InitializeComponent();
            this.Name = "MessageWindow";
            this.Text = m_resources.GetString("MessageWindow");
            this.TabText = this.Text;
        }
    }
}
