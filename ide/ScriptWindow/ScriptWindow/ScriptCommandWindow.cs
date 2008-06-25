//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using EcellLib;
using EcellLib.Message;

using IronPython.Hosting;

namespace EcellLib.ScriptWindow
{
    /// <summary>
    /// Form to input the command and display the script result.
    /// </summary>
    public partial class ScriptCommandWindow : EcellDockContent
    {
        #region Fields
        private PythonEngine m_engine;
        private MemoryStream m_consoleOutput;
        private Font m_boldFont;
        private Font m_defaultFont;
        private Color m_defaultTextColor;
        private StringBuilder m_statementBuffer;
        private bool m_interactionContinued;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public ScriptCommandWindow()
        {
            base.m_isSavable = true;
            InitializeComponent();
            this.SWMessageText.Font = this.SWCommandText.Font = new Font(
                FontFamily.GenericMonospace, 9.0f
            );
            this.Name = "ScriptWindow";
            this.Text = MessageResScript.ScriptWindow;
            this.TabText = this.Text;

            SWCommandText.KeyPress += delegate(object o, KeyPressEventArgs args) {
                if (args.KeyChar == '\r')
                {
                    args.Handled = true;
                }
            };

            m_defaultFont = SWMessageText.SelectionFont;
            m_defaultTextColor = SWMessageText.SelectionColor;
            m_boldFont = new Font(m_defaultFont, FontStyle.Bold);
            m_consoleOutput = new MemoryStream();
            m_interactionContinued = false;
            m_statementBuffer = new StringBuilder();

            {
                EngineOptions options = new EngineOptions();
                options.ShowClrExceptions = true;
                options.ClrDebuggingEnabled = true;
                options.ExceptionDetail = false;
                m_engine = new PythonEngine(options);
            }
            m_engine.Sys.DefaultEncoding = Encoding.UTF8;
            m_engine.SetStandardOutput(m_consoleOutput);
            m_engine.SetStandardError(m_consoleOutput);
            m_engine.AddToPath(Util.GetBinDir());
            m_engine.Execute("from EcellIDE import *;");
            Flush();
        }
        #endregion

        #region Events
        /// <summary>
        /// The event sequence when key is pressed.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e"></param>
        private void CommandTextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!e.Control)
                {
                    Interact(SWCommandText.Text);
                    SWCommandText.Select(0, 0);
                    SWCommandText.ResetText();
                    e.Handled = true;
                }
                else
                {
                    SWCommandText.AppendText("\r\n");
                }
            }
        }
        #endregion

        /// <summary>
        /// Execute the script by using the file.
        /// </summary>
        /// <param name="file">the loaded file.</param>
        public void ExecuteFile(string file)
        {
            m_engine.ExecuteFile(file);
            Flush();
        }

        public void WriteToConsole(string text)
        {
            SWMessageText.Select(SWMessageText.TextLength, 0);
            SWMessageText.AppendText(text);
        }

        public void SetTextStyle(Font f, Color c)
        {
            SWMessageText.Select(SWMessageText.TextLength, 0);
            SWMessageText.SelectionFont = f == null ? m_defaultFont : f;
            SWMessageText.SelectionColor = c == Color.Empty ? m_defaultTextColor : c;
        }

        /// <summary>
        /// Flush the memory stream into MessageText
        /// </summary>
        public void Flush()
        {
            WriteToConsole(
                UTF8Encoding.UTF8.GetString(
                    m_consoleOutput.GetBuffer(),
                    0, (int)m_consoleOutput.Position)
            );
            m_consoleOutput.Seek(0, SeekOrigin.Begin);
            SWMessageText.ScrollToCaret();
        }


        /// <summary>
        /// Execute the script by using the command.
        /// </summary>
        /// <param name="cmd">the command string.</param>
        /// <param name="isOut">the flag whether this command is out.</param>
        public void Interact(string cmd)
        {
            SetTextStyle(m_boldFont, Color.SkyBlue);
            WriteToConsole(
                (string)(m_interactionContinued ?
                    m_engine.Sys.ps2 : m_engine.Sys.ps1) + " ");
            SetTextStyle(m_boldFont, m_defaultTextColor);
            WriteToConsole(cmd + "\r\n");
            SetTextStyle(null, Color.Empty);
            m_statementBuffer.Append(cmd);

            try
            {
                if (!m_engine.ParseInteractiveInput(cmd, false))
                {
                    m_interactionContinued = true;
                    return;
                }
                m_engine.ExecuteToConsole(m_statementBuffer.ToString());
                Flush();
            }
            catch (Exception e)
            {
                SetTextStyle(null, Color.DarkSalmon);
                WriteToConsole(m_engine.FormatException(e));
                SetTextStyle(null, Color.Empty);
            }
            m_interactionContinued = false;
            m_statementBuffer.Length = 0;
        }
    }
}