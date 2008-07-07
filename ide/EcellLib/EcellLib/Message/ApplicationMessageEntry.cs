using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Message
{
    /// <summary>
    /// Application message.
    /// </summary>
    public class ApplicationMessageEntry: MessageEntry
    {

        private string m_loc;

        /// <summary>
        /// get the location of message.
        /// </summary>
        public override string Location
        {
            get { return m_loc; }
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="type">the type of message.</param>
        /// <param name="msg">the message string.</param>
        /// <param name="loc">the location of message.</param>
        public ApplicationMessageEntry(MessageType type, string msg, object loc)
            : base(type, msg)
        {
            m_loc = loc.GetType().Name; 
        }
    }
}
