using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace EcellLib.Message
{
    /// <summary>
    /// Class to manage the error message.
    /// </summary>
    public class MessageManager
    {
        private ApplicationEnvironment m_env;

        private List<IMessageEntry> m_entries;

        #region Accessors
        /// <summary>
        /// get the number of messages.
        /// </summary>
        public int Count
        {
            get { return m_entries.Count; }
        }

        /// <summary>
        /// get the error message by using the index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public IMessageEntry this[int idx]
        {
            get { return m_entries[idx]; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="env">the application environment.</param>
        public MessageManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_entries = new List<IMessageEntry>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add the error message.
        /// </summary>
        /// <param name="entry">the error message entry.</param>
        public void Append(IMessageEntry entry)
        {
            Trace.WriteLine(entry);
            m_entries.Add(entry);
            m_env.PluginManager.Message2(entry);
        }

        /// <summary>
        /// Clear the error message.
        /// </summary>
        public void Clear()
        {
            m_entries.Clear();
        }
        #endregion
    }
}
