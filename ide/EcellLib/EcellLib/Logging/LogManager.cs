using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Ecell.Logging
{
    public class LogEntryEventArgs: EventArgs
    {
        private ILogEntry m_entry;

        public ILogEntry LogEntry
        {
            get { return m_entry; }
        }

        public LogEntryEventArgs(ILogEntry entry)
        {
            m_entry = entry;
        }
    }

    public delegate void LogEntryAppendedEventHandler(object o, LogEntryEventArgs e);

    /// <summary>
    /// Class to manage the error message.
    /// </summary>
    public class LogManager
    {
        private ApplicationEnvironment m_env;

        private List<ILogEntry> m_entries;

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
            LogEntryAppended(this, new LogEntryEventArgs(entry));
        }
        #endregion
    }
}
