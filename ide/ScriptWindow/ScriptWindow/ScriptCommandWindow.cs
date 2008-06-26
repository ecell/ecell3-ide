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
using System.Threading;
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
        private class ScriptRunner
        {
            private AutoResetEvent m_event;
            private PythonEngine m_engine;
            private string m_command;

            public event EventHandler ScriptExecutionStarted;
            public event EventHandler ScriptExecutionStopped;

            public ScriptRunner(PythonEngine engine)
            {
                m_event = new AutoResetEvent(false);
                m_engine = engine;
            }

            public void Execute(string cmd)
            {
                Debug.Assert(cmd != null);
                lock (this)
                {
                    m_command = cmd;
                    m_event.Set();
                }
            }

            public void Stop()
            {
                lock (this)
                {
                    m_command = null;
                    m_event.Set();
                }
            }

            public void Run()
            {
                for (;;)
                {
                    m_event.WaitOne();
                    if (m_command == null)
                        break;
                    ScriptExecutionStarted(this, new EventArgs());
                    m_engine.ExecuteToConsole(m_command);
                    ScriptExecutionStopped(this, new EventArgs());
                }
            }
        }

        private class NotifyingMemoryStream: MemoryStream
        {
            public delegate void StreamFlushEventHandler(NotifyingMemoryStream obj, long prevPosisiton);
            public event StreamFlushEventHandler StreamFlushed;
            long m_previousPosition;

            public NotifyingMemoryStream()
            {
                m_previousPosition = 0;
            }

            public override void Flush()
            {
                base.Flush();
                StreamFlushed(this, m_previousPosition);
                m_previousPosition = Position;
            }
        }

        #region Fields
        private PythonEngine m_engine;
        private NotifyingMemoryStream m_consoleOutput;
        private Font m_boldFont;
        private Font m_defaultFont;
        private Color m_defaultTextColor;
        private StringBuilder m_statementBuffer;
        private Color m_promptColor = Color.SkyBlue;
        private int m_currentPromptCharCount;
        private bool m_interactionContinued;
        private ScriptRunner m_scriptRunner;
        private Thread m_scriptRunnerThread;
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

            m_defaultFont = SWMessageText.SelectionFont;
            m_defaultTextColor = SWMessageText.SelectionColor;
            m_boldFont = new Font(m_defaultFont, FontStyle.Bold);
            m_consoleOutput = new NotifyingMemoryStream();
            m_consoleOutput.StreamFlushed +=
                delegate(NotifyingMemoryStream s, long prevPos)
                {
                    SWCommandText.Invoke(new MethodInvoker(Flush));
                };
            m_statementBuffer = new StringBuilder();
            m_interactionContinued = false;
            m_currentPromptCharCount = 0;
            {
                EngineOptions options = new EngineOptions();
                options.ShowClrExceptions = true;
                options.ClrDebuggingEnabled = true;
                options.ExceptionDetail = false;
                m_engine = new PythonEngine(options);
            }
            m_scriptRunner = new ScriptRunner(m_engine);
            m_scriptRunner.ScriptExecutionStarted +=
                delegate(object obj, EventArgs e)
                {
                    SWCommandText.Invoke(new MethodInvoker(
                        delegate()
                        {
                            SWCommandText.Enabled = false;
                        }
                    ));
                };
            m_scriptRunner.ScriptExecutionStopped +=
                delegate(object obj, EventArgs e)
                {
                    SWCommandText.Invoke(new MethodInvoker(
                        delegate()
                        {
                            SWCommandText.Enabled = true;
                            SWCommandText.Focus();
                            Flush();
                        }
                     ));
                };
            m_engine.Sys.DefaultEncoding = Encoding.UTF8;
            m_engine.SetStandardOutput(m_consoleOutput);
            m_engine.SetStandardError(m_consoleOutput);
            m_engine.AddToPath(Util.GetBinDir());
            ResetCommandLineControl();
            m_engine.Execute("from EcellIDE import *;");
            Flush();
            m_scriptRunnerThread = new Thread(new ThreadStart(m_scriptRunner.Run));
            Disposed +=
                delegate(object o, EventArgs e)
                {
                    m_scriptRunner.Stop();
                    m_scriptRunnerThread.Join();
                };
            m_scriptRunnerThread.Start();

        }
        #endregion

        #region Events
        /// <summary>
        /// The event sequence when key is pressed.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e"></param>
        private void CommandTextKeyDown(object _sender, KeyEventArgs e)
        {
            RichTextBox sender = (RichTextBox)_sender;
            if (e.KeyCode == Keys.Enter)
            {
                if (!e.Control && !e.Shift)
                {
                    Interact(SWCommandText.Text.Substring(m_currentPromptCharCount));
                    ResetCommandLineControl();
                    e.Handled = true;
                }
                else if (!e.Shift)
                {
                    e.SuppressKeyPress = true;
                    sender.AppendText("\r\n");
                    sender.SelectionIndent = sender.SelectionHangingIndent;
                }
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (sender.SelectionStart <= m_currentPromptCharCount)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            }
        }
        private void CommandTextSelectionChanged(object _sender, EventArgs e)
        {
            RichTextBox sender = (RichTextBox)_sender;
            if (sender.SelectionStart < m_currentPromptCharCount)
            {
                sender.Select(
                    m_currentPromptCharCount,
                    sender.SelectionLength - (m_currentPromptCharCount - sender.SelectionStart));
                sender.SelectionColor = sender.ForeColor;
            }
        }
        #endregion

        private void ResetCommandLineControl()
        {
            string prompt = GetCurrentPrompt();
            SWCommandText.ResetText();
            SWCommandText.Select(0, 0);
            SWCommandText.SelectionColor = m_promptColor;
            {
                Graphics g = this.SWCommandText.CreateGraphics();
                g.PageUnit = GraphicsUnit.Pixel;
                this.SWCommandText.SelectionHangingIndent = (int)g.MeasureString(prompt, SWCommandText.Font).Width + 2;
                g.Dispose();
            }
            SWCommandText.AppendText(prompt);
            SWCommandText.Select(SWCommandText.TextLength, 0);
            m_currentPromptCharCount = SWCommandText.TextLength;
            SWCommandText.SelectionColor = m_defaultTextColor;
        }

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

        private string GetCurrentPrompt()
        {
            return (string)(m_interactionContinued ?
                    m_engine.Sys.ps2 : m_engine.Sys.ps1);
        }

        /// <summary>
        /// Execute the script by using the command.
        /// </summary>
        /// <param name="cmd">the command string.</param>
        /// <param name="isOut">the flag whether this command is out.</param>
        public void Interact(string cmd)
        {
            SetTextStyle(m_boldFont, m_promptColor);
            WriteToConsole(GetCurrentPrompt());
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
                m_scriptRunner.Execute(m_statementBuffer.ToString());
            }
            catch (Exception e)
            {
                SetTextStyle(null, Color.DarkSalmon);
                WriteToConsole(m_engine.FormatException(e));
                SetTextStyle(null, Color.Empty);
                SWMessageText.ScrollToCaret();
            }
            m_interactionContinued = false;
            m_statementBuffer.Length = 0;
        }
    }
}