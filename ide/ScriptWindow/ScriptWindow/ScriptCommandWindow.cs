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
using System.Reflection;
using System.Windows.Forms;

using Ecell;
using Ecell.Logging;

using IronPython.Hosting;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.ScriptWindow
{
    /// <summary>
    /// Form to input the command and display the script result.
    /// </summary>
    public partial class ScriptCommandWindow : EcellDockContent
    {
        /// <summary>
        /// Script executing object.
        /// </summary>
        private class ScriptRunner
        {
            #region Fields
            /// <summary>
            /// Event when auto command is reset.
            /// </summary>
            private AutoResetEvent m_event;
            /// <summary>
            /// Python engine.
            /// </summary>
            private PythonEngine m_engine;
            /// <summary>
            /// command string.
            /// </summary>
            private string m_command;
            /// <summary>
            /// Event hanadler to start the script execution.
            /// </summary>
            public event EventHandler ScriptExecutionStarted;
            /// <summary>
            /// Event handller to stop the script execution.
            /// </summary>
            public event EventHandler<StopEventArgs> ScriptExecutionStopped;
            #endregion

            /// <summary>
            /// StopEventArgs
            /// </summary>
            public class StopEventArgs: EventArgs
            {
                #region Fields
                /// <summary>
                /// The reason to stop.
                /// </summary>
                private Exception m_reason;
                #endregion

                /// <summary>
                /// get the reason to stop.
                /// </summary>
                public Exception Reason
                {
                    get { return m_reason; }
                }

                #region Constructors
                /// <summary>
                /// Constructors.
                /// </summary>
                public StopEventArgs()
                {
                    m_reason = null;
                }

                /// <summary>
                /// Constructors with the initial conditions.
                /// </summary>
                /// <param name="reason">the reason to stop.</param>
                public StopEventArgs(Exception reason)
                {
                    m_reason = reason;
                }
                #endregion
            }

            /// <summary>
            /// Constructors.
            /// </summary>
            /// <param name="engine">Python engine.</param>
            public ScriptRunner(PythonEngine engine)
            {
                m_event = new AutoResetEvent(false);
                m_engine = engine;
            }

            /// <summary>
            /// Execute the script command.
            /// </summary>
            /// <param name="cmd">the script command.</param>
            public void Execute(string cmd)
            {
                Debug.Assert(cmd != null);
                lock (this)
                {
                    m_command = cmd;
                    m_event.Set();
                }
            }

            /// <summary>
            /// Stop the script.
            /// </summary>
            public void Stop()
            {
                lock (this)
                {
                    m_command = null;
                    m_event.Set();
                }
            }

            /// <summary>
            /// Run the script commnad.
            /// </summary>
            public void Run()
            {
                for (;;)
                {
                    Exception stopReason = null;
                    m_event.WaitOne();
                    if (m_command == null)
                        break;
                    ScriptExecutionStarted(this, new EventArgs());
                    try
                    {
                        m_engine.ExecuteToConsole(m_command);
                    }
                    catch (Exception e)
                    {
                        stopReason = e;
                    }
                    ScriptExecutionStopped(this, new StopEventArgs(stopReason));
                }
            }
        }

        /// <summary>
        /// NotifyingMemoryStream
        /// </summary>
        private class NotifyingMemoryStream: MemoryStream
        {
            #region Fields
            /// <summary>
            /// EventHandler object for Flush stream.
            /// </summary>
            /// <param name="obj">NotifyingMemoryStream</param>
            /// <param name="prevPosisiton">Previous cursor position</param>
            public delegate void StreamFlushEventHandler(NotifyingMemoryStream obj, long prevPosisiton);
            /// <summary>
            /// Event handler when stream is flushed.
            /// </summary>
            public event StreamFlushEventHandler StreamFlushed;
            /// <summary>
            /// Previous cursor position.
            /// </summary>
            long m_previousPosition;
            #endregion

            #region Constructors
            /// <summary>
            /// Constructor
            /// </summary>
            public NotifyingMemoryStream()
            {
                m_previousPosition = 0;
            }
            #endregion

            /// <summary>
            /// Flush the stream.
            /// </summary>
            public override void Flush()
            {
                base.Flush();
                StreamFlushed(this, m_previousPosition);
                m_previousPosition = Position;
            }
        }

        #region Fields
        /// <summary>
        /// Python egine.
        /// </summary>
        private PythonEngine m_engine;
        /// <summary>
        /// Stream the console output.
        /// </summary>
        private NotifyingMemoryStream m_consoleOutput;
        /// <summary>
        /// Bold font style.
        /// </summary>
        private Font m_boldFont;
        /// <summary>
        /// Default font style.
        /// </summary>
        private Font m_defaultFont;
        /// <summary>
        /// Default text color.
        /// </summary>
        private Color m_defaultTextColor;
        /// <summary>
        /// String build object.
        /// </summary>
        private StringBuilder m_statementBuffer;
        /// <summary>
        /// Prompt color.
        /// </summary>
        private Color m_promptColor = Color.RoyalBlue;
        /// <summary>
        /// The current prompt position.
        /// </summary>
        private int m_currentPromptCharCount;
        /// <summary>
        ///  The flag whether interaction is continued.
        /// </summary>
        private bool m_interactionContinued;
        /// <summary>
        /// Script execute object.
        /// </summary>
        private ScriptRunner m_scriptRunner;
        /// <summary>
        /// Thread object.
        /// </summary>
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
            this.Text = MessageResources.ScriptWindow;
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
                options.ShowClrExceptions = false;
                options.ClrDebuggingEnabled = false;
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
                delegate(object obj, ScriptRunner.StopEventArgs e)
                {
                    SWCommandText.Invoke(new MethodInvoker(
                        delegate()
                        {
                            if (e.Reason != null)
                            {
                                ReportException(e.Reason);
                            }
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
            //m_engine.Execute("from EcellIDE import *;");
            Flush();
            m_scriptRunnerThread = new Thread(new ThreadStart(m_scriptRunner.Run));
            Disposed +=
                delegate(object o, EventArgs e)
                {
                    m_scriptRunner.Stop();
                    m_scriptRunnerThread.Join();
                };
            m_scriptRunnerThread.Start();
            WriteHeader();
        }
        #endregion

        #region Events
        /// <summary>
        /// The event sequence when key is pressed.
        /// </summary>
        /// <param name="_sender">RichTextBox</param>
        /// <param name="e">KeyEventArgs</param>
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
            else if (e.KeyCode == Keys.Z && e.Control == true)
            {
                sender.Text = "";
                ResetCommandLineControl();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event when the selection text is changed.
        /// </summary>
        /// <param name="_sender">RichTextBox.</param>
        /// <param name="e">EventArgs</param>
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

        /// <summary>
        /// Event when Scirpt command window is shown.
        /// </summary>
        /// <param name="sender">ScriptCommandWindow</param>
        /// <param name="e">EventArgs</param>
        private void ShownScriptCommandWindow(object sender, EventArgs e)
        {
            SWCommandText.Focus();
        }
        #endregion

        /// <summary>
        /// Press key on DataGridView.
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="keyData">Key data.</param>
        /// <returns>whether this event is handled.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((int)keyData == (int)Keys.Control + (int)Keys.C)
            {
                string copytext;
                if (!string.IsNullOrEmpty(SWCommandText.SelectedText))
                {
                    copytext = SWCommandText.SelectedText;
                }
                else
                {
                    copytext = SWCommandText.Text;
                }
                if (!string.IsNullOrEmpty(copytext))
                    Clipboard.SetText(copytext);
                return true;
            }
            if ((int)keyData == (int)Keys.Control + (int)Keys.V)
            {
                string pastetext = Clipboard.GetText();
                if (!String.IsNullOrEmpty(pastetext))
                {
                    SWCommandText.Text =
                        SWCommandText.Text.Insert(SWCommandText.SelectionStart + SWCommandText.SelectionLength,
                        pastetext);
                }
                return true;
            }
            if ((int)keyData == (int)Keys.Control + (int)Keys.X)
            {
                string copytext;
                string data;
                if (!string.IsNullOrEmpty(SWCommandText.SelectedText))
                {
                    data = SWCommandText.Text;
                    data = data.Substring(0, SWCommandText.SelectionStart)
                    + data.Substring(SWCommandText.SelectionStart + SWCommandText.SelectionLength);
                    copytext = SWCommandText.SelectedText;
                }
                else
                {
                    data = "";
                    copytext = SWCommandText.Text;
                }
                if (!string.IsNullOrEmpty(copytext))
                {
                    Clipboard.SetText(copytext);
                    ResetCommandLineControl();                   
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Reset command line text box.
        /// </summary>
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

        /// <summary>
        /// Write header.
        /// </summary>
        private void WriteHeader()
        {
            WriteToConsole("<E-Cell IDE>\n");
            WriteToConsole("Script window (Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")\n\n");

        }

        /// <summary>
        /// Report the return string from the script.
        /// </summary>
        /// <param name="e">Exception</param>
        private void ReportException(Exception e)
        {
            SetTextStyle(null, Color.DarkSalmon);
            WriteToConsole(m_engine.FormatException(e));
            SetTextStyle(null, Color.Empty);
            SWMessageText.ScrollToCaret();
        }

        /// <summary>
        /// Write the return string to the console.
        /// </summary>
        /// <param name="text">the return string.</param>
        public void WriteToConsole(string text)
        {
            SWMessageText.Select(SWMessageText.TextLength, 0);
            SWMessageText.AppendText(text);
        }
        /// <summary>
        /// Set the text style of console.
        /// </summary>
        /// <param name="f">Font style</param>
        /// <param name="c">Colort</param>
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
        /// Get current prompt string.
        /// </summary>
        /// <returns></returns>
        private string GetCurrentPrompt()
        {
            return (string)(m_interactionContinued ?
                    m_engine.Sys.ps2 : m_engine.Sys.ps1);
        }

        /// <summary>
        /// Execute the script by using the command.
        /// </summary>
        /// <param name="cmd">the command string.</param>
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
                ReportException(e);
            }
            m_interactionContinued = false;
            m_statementBuffer.Length = 0;
        }
    }
}
