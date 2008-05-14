using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.Message
{
    public class ApplicationMessageEntry: MessageEntry
    {
        private string m_loc;

        public override string Location
        {
            get { return m_loc; }
        }

        public ApplicationMessageEntry(MessageType type, string msg, object loc)
            : base(type, msg)
        {
            m_loc = loc.GetType().Name; 
        }
    }
}
