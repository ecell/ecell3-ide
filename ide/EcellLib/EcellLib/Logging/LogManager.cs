using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Ecell.Logging
{
    /// <summary>
    /// Class to manage the error message.
    /// </summary>
    public class LogManager
    {
        private ApplicationEnvironment m_env;

        private List<ILogEntry> m_entries;
        /// <summary>
        /// 
        /// </summary>
        public event LogEntryAppendedEventHandler LogEntryAppended;

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
        public ILogEntry this[int idx]
        {
            get { return m_entries[idx]; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="env">the application environment.</param>
        public LogManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_entries = new List<ILogEntry>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add the error message.
        /// </summary>
        /// <param name="entry">the error message entry.</param>
        public void Append(ILogEntry entry)
        {
            Trace.WriteLine(entry);
            m_entries.Add(entry);
            if (LogEntryAppended != null)
                LogEntryAppended(this, new LogEntryEventArgs(entry));
        }
        #endregion
    }
}
