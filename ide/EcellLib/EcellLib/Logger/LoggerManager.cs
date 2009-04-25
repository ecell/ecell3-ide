//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void LoggerAddEventHandler(object o, LoggerEventArgs e);
    /// <summary>
    /// EventHandler when object is deleted.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void LoggerDeleteEventHandler(object o, LoggerEventArgs e);
    /// <summary>
    /// EventHandler when object is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void LoggerChangedEventHandler(object o, LoggerEventArgs e);

    /// <summary>
    /// LoggerManager
    /// </summary>
    public class LoggerManager
    {
        public event LoggerAddEventHandler LoggerAddEvent;
        public event LoggerDeleteEventHandler LoggerDeleteEvent;
        public event LoggerChangedEventHandler LoggerChangedEvent;
        private int m_count = 0;
        private ApplicationEnvironment m_env;
        private List<LoggerEntry> m_loggerList = new List<LoggerEntry>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public LoggerManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_count = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="Key"></param>
        /// <param name="Type"></param>
        /// <param name="fullPN"></param>
        public void AddLoggerEntry(string modelID, string Key, string Type, string fullPN)
        {
            LoggerEntry entry = new LoggerEntry(modelID, Key, Type, fullPN);
            entry.Color = Util.GetColor(m_count);
            entry.LineStyle = Util.GetLine(m_count);
            m_count++;
            AddLoggerEntry(entry);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        public void Clear()
        {
            m_loggerList.Clear();
            m_count = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgFullPN"></param>
        /// <param name="entry"></param>
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

        public void NodeRemoved(EcellObject obj)
        {
            List<LoggerEntry> delList = GetLoggerEntryForObject(obj.Key, obj.Type);
            foreach (LoggerEntry m in delList)
            {
                LoggerRemoved(m);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sys"></param>
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
        /// 
        /// </summary>
        /// <param name="entry"></param>
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
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="fullPN"></param>
        /// <returns></returns>
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

