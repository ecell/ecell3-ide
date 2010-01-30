//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Objects;

namespace Ecell.Logger
{
    /// <summary>
    /// EventHandler when object is added.
    /// </summary>
    /// <param name="o">LoggerManager</param>
    /// <param name="e">LoggerEventArgs</param>
    public delegate void LoggerAddEventHandler(object o, LoggerEventArgs e);
    /// <summary>
    /// EventHandler when object is deleted.
    /// </summary>
    /// <param name="o">LoggerManager</param>
    /// <param name="e">LoggerEventArgs</param>
    public delegate void LoggerDeleteEventHandler(object o, LoggerEventArgs e);
    /// <summary>
    /// EventHandler when object is changed.
    /// </summary>
    /// <param name="o">LoggerManager</param>
    /// <param name="e">LoggerEventArgs</param>
    public delegate void LoggerChangedEventHandler(object o, LoggerEventArgs e);

    /// <summary>
    /// LoggerManager
    /// </summary>
    public class LoggerManager
    {
        #region Fields
        /// <summary>
        /// EventHandler to add the logger.
        /// </summary>
        public event LoggerAddEventHandler LoggerAddEvent;
        /// <summary>
        /// EventHandler to delete the logger.
        /// </summary>
        public event LoggerDeleteEventHandler LoggerDeleteEvent;
        /// <summary>
        /// EventHandler to changet the logger.
        /// </summary>
        public event LoggerChangedEventHandler LoggerChangedEvent;
        /// <summary>
        /// the logger count.
        /// </summary>
        private int m_count = 0;
        /// <summary>
        /// The Application environment.
        /// </summary>
        private ApplicationEnvironment m_env;
        /// <summary>
        /// The list of logger entry.
        /// </summary>
        private List<LoggerEntry> m_loggerList = new List<LoggerEntry>();
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env">the application environment.</param>
        public LoggerManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_count = 0;
        }

        /// <summary>
        /// Add the logger entry.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        /// <param name="Key">the object key.</param>
        /// <param name="Type">the object type.</param>
        /// <param name="fullPN">the FullPN.</param>
        public void AddLoggerEntry(string modelID, string Key, string Type, string fullPN)
        {
            LoggerEntry entry = new LoggerEntry(modelID, Key, Type, fullPN);
            entry.Color = Util.GetColor(m_count);
            entry.LineStyle = Util.GetLine(m_count);
            m_count++;
            AddLoggerEntry(entry);
        }

        /// <summary>
        /// Add the logger entry.
        /// </summary>
        /// <param name="entry">the logger entry.</param>
        public void AddLoggerEntry(LoggerEntry entry)
        {
            if (entry == null) return;
            if (m_loggerList.Contains(entry))
                return;

            m_loggerList.Add(entry);

            if (LoggerAddEvent != null)
            {
                LoggerAddEvent(this, new LoggerEventArgs(entry.FullPN, entry));
            }
        }

        /// <summary>
        /// Get the list of logger entry name.
        /// </summary>
        /// <returns>the list of logger entry.</returns>
        public List<string> GetLoggerList()
        {
            List<string> list = new List<string>();
            foreach (LoggerEntry m in m_loggerList)
            {
                if (m.IsLoaded) continue;
                if (list.Contains(m.FullPN))
                    continue;
                list.Add(m.FullPN);
            }
            return list;
        }

        /// <summary>
        /// Clear the list of logger entry.
        /// </summary>
        public void Clear()
        {
            m_loggerList.Clear();
            m_count = 0;
        }

        /// <summary>
        /// Chage the logger entry.
        /// </summary>
        /// <param name="orgFullPN">the original FullPN.</param>
        /// <param name="entry">the logger entry.</param>
        public void LoggerChanged(string orgFullPN, LoggerEntry entry)
        {
            LoggerEntry m = GetLoggerEntryForFullPN(orgFullPN);
            if (m != null)
            {
                m_loggerList.Remove(m);
            }             
            m_loggerList.Add(entry);

            if (LoggerChangedEvent != null)
            {               
                LoggerChangedEvent(this, new LoggerEventArgs(orgFullPN, entry));
            }
        }

        /// <summary>
        /// Remove the EcellObject.
        /// </summary>
        /// <param name="obj">the removed object.</param>
        public void NodeRemoved(EcellObject obj)
        {
            List<LoggerEntry> delList = GetLoggerEntryForObject(obj.Key, obj.Type);
            foreach (LoggerEntry m in delList)
            {
                LoggerRemoved(m);
            }
        }

        /// <summary>
        /// Remove the System object.
        /// </summary>
        /// <param name="sys">the removed system object.</param>
        public void SystemRemoved(EcellObject sys)
        {
            List<LoggerEntry> delList = new List<LoggerEntry>();
            foreach (LoggerEntry m in m_loggerList)
            {
                if (m.ModelID == sys.ModelID &&
                    m.ID.StartsWith(sys.Key))
                {
                    delList.Add(m);
                }
            }
            foreach (LoggerEntry m in delList)
            {
                LoggerRemoved(m);
            }
        }

        /// <summary>
        /// Remove the logger entry.
        /// </summary>
        /// <param name="entry">the removed logger entry.</param>
        public void LoggerRemoved(LoggerEntry entry)
        {
            if (entry == null) return;

            if (!m_loggerList.Contains(entry)) 
                return;

            m_loggerList.Remove(entry);

            if (LoggerDeleteEvent != null)
            {
                LoggerDeleteEvent(this, new LoggerEventArgs(entry.FullPN, entry));
            }
        }
        /// <summary>
        /// Get the logger entry from Ecellobject.
        /// </summary>
        /// <param name="ID">the object key.</param>
        /// <param name="type">the object type.</param>
        /// <returns>the list of logger entry.</returns>
        public List<LoggerEntry> GetLoggerEntryForObject(string ID, string type)
        {
            List<LoggerEntry> result = new List<LoggerEntry>();

            foreach (LoggerEntry m in m_loggerList)
            {
                if (m.ID == ID && m.Type == type)
                    result.Add(m);
            }

            return result;
        }
        /// <summary>
        /// Get the logger entry from FullPN.
        /// </summary>
        /// <param name="fullPN">the FullPN.</param>
        /// <returns>the logger entry.</returns>
        public LoggerEntry GetLoggerEntryForFullPN(string fullPN)
        {
            foreach (LoggerEntry m in m_loggerList)
            {
                if (m.FullPN == fullPN)
                    return m;
            }
            return null;
        }
    }
}

