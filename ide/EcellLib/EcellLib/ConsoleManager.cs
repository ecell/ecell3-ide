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
        #region Fields
        /// <summary>
        /// string to output to console
        /// </summary>
        private string m_str;
        #endregion

        #region Accessors
        /// <summary>
        /// get string to output to console.
        /// </summary>
        public string Data
        {
            get { return m_str; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="str">string to output to console.</param>
        public ConsoleDataAvailableEventArgs(string str)
        {
            m_str = str;
        }
        #endregion
    }

    /// <summary>
    /// Delegate to output the console.
    /// </summary>
    /// <param name="o">object.</param>
    /// <param name="e">ConsoleDataAvailableEventArgs</param>
    public delegate void ConsoleDataAvailableEventHandler(object o, ConsoleDataAvailableEventArgs e);

    /// <summary>
    /// Console manager class.
    /// </summary>
    public class ConsoleManager: TextWriter
    {
        #region Fields
        /// <summary>
        /// Application environment class.
        /// </summary>
        private ApplicationEnvironment m_env;
        /// <summary>
        /// String buffer.
        /// </summary>
        private StringBuilder m_buf;
        /// <summary>
        /// status of console
        /// </summary>
        private int m_state;
        /// <summary>
        /// EventHandler for ConsoleDataAvailableEventHandler
        /// </summary>
        public event ConsoleDataAvailableEventHandler ConsoleDataAvailable;
        #endregion

        #region Accessors
        /// <summary>
        /// get Encoding.
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
        /// <summary>
        /// get Application environment.
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="env">application environment.</param>
        public ConsoleManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_buf = new StringBuilder();
            m_state = 0;
        }
        #endregion

        /// <summary>
        /// Flush the buffer string to the console.
        /// </summary>
        public override void Flush()
        {
            lock (this) { _Flush(); }
        }

        /// <summary>
        /// Flush the buffer string to the console.
        /// </summary>
        private void _Flush()
        {
            if (ConsoleDataAvailable != null)
                ConsoleDataAvailable(this, new ConsoleDataAvailableEventArgs(m_buf.ToString()));
            m_buf.Length = 0;
        }

        /// <summary>
        /// Write a character to the console.
        /// </summary>
        /// <param name="c">the output character.</param>
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
        /// Write the characters to the console
        /// </summary>
        /// <param name="buf">the characters.</param>
        /// <param name="index">the start index.</param>
        /// <param name="count">the character count.</param>
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
