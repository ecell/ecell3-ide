//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
// written by Moriyoshi Koizumi <mozo@sfc.keio.ac.jp>
//

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Ecell
{
    /// <summary>
    /// ConsoleDataAvailableEventArgs
    /// </summary>
    public class ConsoleDataAvailableEventArgs: EventArgs
    {
        private string m_str;
        /// <summary>
        /// 
        /// </summary>
        public string Data
        {
            get { return m_str; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public ConsoleDataAvailableEventArgs(string str)
        {
            m_str = str;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void ConsoleDataAvailableEventHandler(object o, ConsoleDataAvailableEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleManager: TextWriter
    {
        private ApplicationEnvironment m_env;
        private StringBuilder m_buf;
        private int m_state;
        /// <summary>
        /// 
        /// </summary>
        public event ConsoleDataAvailableEventHandler ConsoleDataAvailable; 
        /// <summary>
        /// 
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public ConsoleManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_buf = new StringBuilder();
            m_state = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
            lock (this) { _Flush(); }
        }

        private void _Flush()
        {
            if (ConsoleDataAvailable != null)
                ConsoleDataAvailable(this, new ConsoleDataAvailableEventArgs(m_buf.ToString()));
            m_buf.Length = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public override void Write(char c)
        {
            lock (this)
            {
                switch (c)
                {
                    case '\r':
                        m_state = 1;
                        break;
                    case '\n':
                        if (m_state == 0)
                            m_buf.Append('\r');
                        m_buf.Append('\n');
                        _Flush();
                        return;
                }
                m_buf.Append(c);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public override void Write(char[] buf, int index, int count)
        {
            int e = index + count;
            int i = index, ni;

            for (;;)
            {
                ni = Array.IndexOf(buf, '\n', i);
                if (ni < 0 || ni >= e)
                    break;
                m_buf.Append(buf, i, ni - i);
                if (ni == 0 || buf[ni - 1] != '\r')
                    m_buf.Append('\r');
                m_buf.Append('\n');
                _Flush();
                i = ni + 1;
            }
            m_buf.Append(buf, i, e - i);
        }
    }
}
