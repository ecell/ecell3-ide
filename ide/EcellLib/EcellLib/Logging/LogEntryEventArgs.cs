using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Logging
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void LogEntryAppendedEventHandler(object o, LogEntryEventArgs e);

    /// <summary>
    /// LogEntryEventArgs
    /// </summary>
    public class LogEntryEventArgs : EventArgs
    {
        private ILogEntry m_entry;
        /// <summary>
        /// LogEntry
        /// </summary>
        public ILogEntry LogEntry
        {
            get { return m_entry; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entry"></param>
        public LogEntryEventArgs(ILogEntry entry)
        {
            m_entry = entry;
        }
    }
}
