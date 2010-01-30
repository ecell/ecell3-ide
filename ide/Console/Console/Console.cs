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
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

using Ecell;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.Console
{
    /// <summary>
    /// The plugin to show message.
    /// </summary>
    public class Console : PluginBase
    {
        #region Fields
        /// <summary>
        ///  MessageWindow form.
        /// </summary>
        private ConsoleControl m_form = null;
        #endregion

        #region Internal Methods
        /// <summary>
        /// The delegate function of setting massage log to window.
        /// </summary>
        /// <param name="type">log type.</param>
        /// <param name="message">message.</param>
        public void SetText(string type, string message)
        {
            TextBox curBox = null;
            if (type == Constants.messageSimulation) curBox = m_form.simText;
            else if (type == Constants.messageAnalysis) curBox = m_form.simText;
            else if (type == Constants.messageDebug) curBox = m_form.simText;
            if (curBox == null) return;

            curBox.Text += System.Environment.NewLine + message;
            if (curBox.Visible)
            {
                curBox.SelectionStart = curBox.Text.Length;
                curBox.ScrollToCaret();
            }
        }

        /// <summary>
        /// Get parent form from user control.
        /// </summary>
        /// <param name="f">user control.</param>
        /// <returns>form includes this parent control.</returns>
        public Form GetParent(UserControl f)
        {
            if (f.ParentForm != null)
            {
                return GetParent(f.ParentForm);
            }
            return null;
        }


        /// <summary>
        /// Get parent form from child form.
        /// </summary>
        /// <param name="f">child form.</param>
        /// <returns>form includes this child form.</returns>
        public Form GetParent(Form f)
        {
            if (f.ParentForm != null)
            {
                return GetParent(f.ParentForm);
            }
            return f;
        }        
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get the window form for MessageWindow plugin.
        /// </summary>
        /// <returns>Windows form</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return new EcellDockContent[] { m_form };
        }

        /// <summary>
        /// Initialize plugin.
        /// </summary>
        public override void Initialize()
        {
            m_form = new ConsoleControl();
            Environment.Console.ConsoleDataAvailable +=
                new ConsoleDataAvailableEventHandler(Console_ConsoleDataAvailable);

        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"MessageWindow"</returns>
        public override string GetPluginName()
        {
            return "Console";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        /// <summary>
        /// Event when console is available to write the data.
        /// </summary>
        /// <param name="o">Console</param>
        /// <param name="args">ConsoleDataAvailableEventArgs</param>
        private void Console_ConsoleDataAvailable(object o, ConsoleDataAvailableEventArgs args)
        {
            if (m_form.InvokeRequired)
            {
                m_form.Invoke(new MethodInvoker(delegate() { AppendText(args.Data); } ));
            }
            else
            {
                AppendText(args.Data);
            }
        }

        /// <summary>
        /// Append the text data.
        /// </summary>
        /// <param name="data">the text data.</param>
        private void AppendText(string data)
        {
            if (m_form.InvokeRequired)
                m_form.Invoke(new MethodInvoker(delegate() { m_form.AppendText(data); }));
            else
                m_form.AppendText(data);
        }

    }
}
