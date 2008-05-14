using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace EcellLib.Message
{
    public class MessageManager
    {
        private ApplicationEnvironment m_env;

        private List<IMessageEntry> m_entries;

        #region Accessors
        public int Count
        {
            get { return m_entries.Count; }
        }

        public IMessageEntry this[int idx]
        {
            get { return m_entries[idx]; }
        }
        #endregion

        #region Constructor
        public MessageManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_entries = new List<IMessageEntry>();
        }
        #endregion

        #region Methods
        public void Append(IMessageEntry entry)
        {
            Trace.WriteLine(entry);
            m_entries.Add(entry);
        }
        #endregion
    }
}
