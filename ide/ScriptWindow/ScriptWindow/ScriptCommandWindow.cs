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
        #endregion

        #region Construcotor
        /// <summary>
        /// Constructor.
        /// </summary>
        public ScriptCommandWindow()
        {
            base.m_isSavable = true;
            InitializeComponent();
            this.Name = "ScriptWindow";
            this.Text = MessageResScript.ScriptWindow;
            this.TabText = this.Text;

            EngineOptions options = new EngineOptions();
            options.ShowClrExceptions = true;
            options.ClrDebuggingEnabled = true;
            options.ExceptionDetail = false;            
            m_engine = new PythonEngine(options);
            m_engine.AddToPath(Directory.GetCurrentDirectory());

            ExecuteToConsole("from EcellIDE import *", false);
            ExecuteToConsole("import time", false);
            ExecuteToConsole("import System.Threading", false);
        }
        #endregion

        #region Events
        /// <summary>
        /// The event sequence when key is pressed.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e"></param>
        private void CommandTextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ExecuteToConsole(SWCommandText.Text, true);
                SWCommandText.Text = "";
            }
            base.OnKeyPress(e);
        }
        #endregion

        /// <summary>
        /// Execute the script by using the file.
        /// </summary>
        /// <param name="file">the loaded file.</param>
        public void ExecuteFile(string file)
        {
            if (String.IsNullOrEmpty(file)) return;

            string stdOut;
            MemoryStream standardOutput = new MemoryStream();
            try
            {
                m_engine.SetStandardOutput(standardOutput);
                m_engine.ExecuteFile(file);
                stdOut = ASCIIEncoding.ASCII.GetString(standardOutput.ToArray());
                SWMessageText.Text += stdOut;
            }
            catch (Exception ex)
            {
                SWMessageText.Text += ex.Message + "\r\n";
            }
            finally
            {
                standardOutput.Dispose();
            }
        }

        /// <summary>
        /// Execute the script by using the command.
        /// </summary>
        /// <param name="cmd">the command string.</param>
        /// <param name="isOut">the flag whether this command is out.</param>
        public void ExecuteToConsole(string cmd, bool isOut)
        {
            if (String.IsNullOrEmpty(cmd)) return;

            string stdOut;
            MemoryStream standardOutput = new MemoryStream();
            try
            {
                m_engine.SetStandardOutput(standardOutput);
                m_engine.ExecuteToConsole(cmd);
                stdOut = ASCIIEncoding.ASCII.GetString(standardOutput.ToArray());
                if (isOut)
                    SWMessageText.Text += ">>> " + cmd + "\r\n";
                SWMessageText.Text += stdOut;
            }
            catch (Exception ex)
            {
                SWMessageText.Text += ex.Message + "\r\n";
            }
            finally
            {
                standardOutput.Dispose();
            }
        }
    }
}