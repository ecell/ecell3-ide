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

namespace Ecell.IDE.Plugins.MessageWindow
{
    /// <summary>
    /// The plugin to show message.
    /// </summary>
    public class MessageWindow : PluginBase
    {
        #region Fields
        /// <summary>
        ///  MessageWindow form.
        /// </summary>
        private MessageWindowControl m_form = null;
        /// <summary>
        /// The delegate function while simulation is running.
        /// </summary>
        /// <param name="t">message type.</param>
        /// <param name="m">message.</param>
        public delegate void SetTextCallback(string t, string m);
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
            m_form = new MessageWindowControl();
            return new EcellDockContent[] { m_form };
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            m_form.simText.Text = "";
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public override void Message(string type, string message)
        {
            Form parentForm = GetParent(m_form);
            if (parentForm == null && m_form.InvokeRequired)
            {
                SetTextCallback f = new SetTextCallback(SetText);
                m_form.Invoke(f, new object[] { type, message });
            }
            else if (parentForm != null && parentForm.InvokeRequired)
            {
                SetTextCallback f = new SetTextCallback(SetText);
                parentForm.Invoke(f, new object[] { type, message });
            }
            else
            {
                TextBox curBox = null;
                if (type == Constants.messageSimulation) curBox = m_form.simText;
                else if (type == Constants.messageAnalysis) curBox = m_form.simText;
                else if (type == Constants.messageDebug) curBox = m_form.simText;
                if (curBox == null) return;

                curBox.Text += message;
                if (curBox.Visible)
                {
                    curBox.SelectionStart = curBox.Text.Length;
                    curBox.ScrollToCaret();
                }
            }
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"MessageWindow"</returns>
        public override string GetPluginName()
        {
            return "MessageWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>true</returns>
        public override bool IsMessageWindow()
        {
            return true;
        }
        #endregion
    }
}
