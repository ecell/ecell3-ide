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

using EcellLib;

namespace EcellLib.MessageWindow
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
        /// The panel that show this plugin in MainWindow.
        /// </summary>
        private Panel m_panel = null;
        /// <summary>
        /// The delegate function while simulation is running.
        /// </summary>
        /// <param name="t">message type.</param>
        /// <param name="m">message.</param>
        public delegate void SetTextCallback(string t, string m);


        #endregion

        /// <summary>
        /// The delegate function of setting massage log to window.
        /// </summary>
        /// <param name="type">log type.</param>
        /// <param name="message">message.</param>
        public void SetText(string type, string message)
        {
            if (type == "simulation")
            {
                m_form.simText.Text += System.Environment.NewLine + message;
                m_form.simText.SelectionStart = m_form.simText.Text.Length;
                m_form.simText.ScrollToCaret();
            }
            else if (type == "analysis")
            {
                m_form.anaText.Text += System.Environment.NewLine + message;
                m_form.anaText.SelectionStart = m_form.anaText.Text.Length;
                m_form.anaText.ScrollToCaret();
            }
            else if (type == "debug")
            {
                m_form.debText.Text += System.Environment.NewLine + message;
                m_form.debText.SelectionStart = m_form.debText.Text.Length;
                m_form.debText.ScrollToCaret();
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

        #region PluginBase
        /// <summary>
        /// Get menustrips for MessageWindow.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get toolbar buttons for TracerWindow plugin.
        /// </summary>
        /// <returns>List of ToolStripItem</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the window form for TracerWindow plugin.
        /// </summary>
        /// <returns>Windows form</returns>
        public List<UserControl> GetWindowsForms()
        {
            List<UserControl> array = new List<UserControl>();
            m_form = new MessageWindowControl();
            array.Add(m_form);

            return array;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public void LoggerAdd(string modelID, string key, string type, string path)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="data">Changed value of object.</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            m_form.simText.Text = "";
            m_form.debText.Text = "";
            m_form.anaText.Text = "";
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            // nothing
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
            Form parentForm = GetParent(m_form);
            if (parentForm != null && parentForm.InvokeRequired)
            {
                SetTextCallback f = new SetTextCallback(SetText);
                parentForm.Invoke(f, new object[] { type, message });
            }
            else
            {
                if (type == "simulation")
                {
                    m_form.simText.Text += message;
                    m_form.simText.SelectionStart = m_form.simText.Text.Length;
                    m_form.simText.ScrollToCaret();
                }
                else if (type == "analysis")
                {
                    m_form.anaText.Text += message;
                    m_form.anaText.SelectionStart = m_form.anaText.Text.Length;
                    m_form.anaText.ScrollToCaret();
                }
                else if (type == "debug")
                {
                    m_form.debText.Text += message;
                    m_form.debText.SelectionStart = m_form.debText.Text.Length;
                    m_form.debText.ScrollToCaret();
                }
            }
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
            // nothing
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(int type)
        {
        }

        /// <summary>
        /// Save the selected model to directory.
        /// </summary>
        /// <param name="modelID">selected model.</param>
        /// <param name="directory">output directory.</param>
        public void SaveModel(string modelID, string directory)
        {
        }

        /// <summary>
        /// Set the panel that show this plugin in MainWindow.
        /// </summary>
        /// <param name="panel">The set panel.</param>
        public void SetPanel(Panel panel)
        {
            this.m_panel = panel;
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print()
        {
            return null;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"MessageWindow"</returns>
        public string GetPluginName()
        {
            return "MessageWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>true</returns>
        public bool IsMessageWindow()
        {
            return true;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>false</returns>
        public bool IsEnablePrint()
        {
            return false;
        }
        #endregion

        #region Event
        /// <summary>
        /// The action when select [Show MessageWindow] menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void ShowPlugin(object sender, EventArgs e)
        {
            if (this.m_panel != null)
                this.m_panel.Show();
        }

        /// <summary>
        /// The action when select [Hide MessageWindow] menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void HidePlugin(object sender, EventArgs e)
        {
            if (this.m_panel != null)
                this.m_panel.Hide();
        }
        #endregion
    }
}
