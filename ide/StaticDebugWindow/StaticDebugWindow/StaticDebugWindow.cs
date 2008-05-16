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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;	// ñºëOãÛä‘ÇÃêÈåæ
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.ComponentModel;

using EcellLib;
using EcellLib.Plugin;
using EcellLib.Objects;

namespace EcellLib.StaticDebugWindow
{
    /// <summary>
    /// Controls the static debug.
    /// </summary>
    public class StaticDebugWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// The list of the error message
        /// </summary>
        List<ErrorMessage> m_errorMessageList;
        /// <summary>
        /// MenuItem of [Debug]->[Static Debug].
        /// </summary>
        ToolStripMenuItem m_staticDebug;
        /// <summary>
        /// The dictionary of StaticDebugPlugin.
        /// Word is the name of static debug. Data is the plugin of static debug.
        /// </summary>
        Dictionary<string, StaticDebugPlugin> m_pluginDict = new Dictionary<string,StaticDebugPlugin>();
        /// <summary>
        /// ResourceManager for StaticDebugWindow.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResStDebug));
        #endregion

        #region Property
        /// <summary>
        /// get/set the list of the "ErrorMessage"
        /// </summary>
        public List<ErrorMessage> ErrorMessageList
        {
            get { return this.m_errorMessageList; }
        }
        #endregion

        #region PluginBase
        /// <summary>
        ///  When the system status is changed, the menu is changed to enable/disable.
        /// </summary>
        /// <param name="type">the status type</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Loaded) m_staticDebug.Enabled = true;
            else m_staticDebug.Enabled = false;
        }

        /// <summary>
        /// Returns items of the menu strip used on the main menu.
        /// </summary>
        /// <returns>items of the menu strip</returns>
        public override List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StaticDebugWindow));
            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_staticDebug = new ToolStripMenuItem();
            m_staticDebug.Name = "MenuItemStaticDebug";
            m_staticDebug.Size = new Size(96, 22);
            m_staticDebug.Text = MessageResStDebug.MenuItemStaticDebugText;
            m_staticDebug.Tag = 10;
            m_staticDebug.Enabled = false;
            m_staticDebug.Click += new EventHandler(this.ShowStaticDebugSetupWindow);

            ToolStripMenuItem debug = new ToolStripMenuItem();
            debug.DropDownItems.AddRange(new ToolStripItem[] {
                m_staticDebug
            });
            debug.Name = "MenuItemDebug";
            debug.Size = new Size(36, 20);
            debug.Text = "Debug";
            tmp.Add(debug);

            return tmp;
        }

        /// <summary>
        /// Returns the name of this plugin.
        /// </summary>
        /// <returns>"StaticDebugWindow"(Fixed)</returns>        
        public override string GetPluginName()
        {
            return "StaticDebugWindow";
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
        /// Checks whether this plugin can print the display image.
        /// </summary>
        /// <returns>false(Fixed)</returns>
        public override List<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            return names;
        }

        /// <summary>
        /// Close the current project..
        /// </summary>
        public override void Clear()
        {
            this.m_errorMessageList.Clear();
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Initializes validated patterns.
        /// </summary>
        public override void Initialize()
        {
            m_errorMessageList = new List<ErrorMessage>();

            StaticDebugPlugin p1 = new StaticDebugForModel(this);
            StaticDebugPlugin p2 = new StaticDebugForNetwork(this);

            m_pluginDict.Add(p1.GetDebugName(), p1);
            m_pluginDict.Add(p2.GetDebugName(), p2);
        }
        
        /// <summary>
        /// Validates the list of the "EcellObject" 4 the mass conservation.
        /// </summary>
        /// <param name="ecellObjectList"></param>
        private void ValidateMassConservation(List<EcellObject> ecellObjectList)
        {
            // MEN WORKING
        }

        /// <summary>
        /// Validates the mass conservation.
        /// </summary>
        /// <param name="modelID"></param>
        public void ValidateMassConservation(string modelID)
        {
            try
            {
                this.ValidateMassConservation(this.m_dManager.GetData(modelID, null));
            }
            catch (Exception ex)
            {
                throw new Exception("The static debug of the mass conservation failed. [" + ex.ToString() + "]");
            }
        }

        /// <summary>
        /// The action of selecting the menu [Debug]->[Static Debug].
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        public void ShowStaticDebugSetupWindow(object sender, EventArgs e)
        {
            StaticDebugSetupWindow win = new StaticDebugSetupWindow();
            win.SetPlugin(this);
            List<String> list = new List<string>();

            foreach (string key in m_pluginDict.Keys)
            {
                list.Add(key);
            }

            win.LayoutCheckList(list);
            win.SSDebugButton.Select();

            win.ShowDialog();
        }

        /// <summary>
        /// execute the static debug in existing the list.
        /// </summary>
        /// <param name="list">the list of static debug.</param>
        public void Debug(List<string> list)
        {
            m_errorMessageList.Clear();
            List<string> mList = m_dManager.GetModelList();
            foreach (string modelID in mList)
            {
                List<EcellObject> olist = m_dManager.GetData(modelID, null);
                foreach (string key in m_pluginDict.Keys)
                {
                    if (!list.Contains(key)) continue;
                    List<ErrorMessage> tmp = m_pluginDict[key].Debug(olist);
                    foreach (ErrorMessage mes in tmp)
                    {
                        m_errorMessageList.Add(mes);
                    }
                }
            }

        }
        #endregion
    }
}

/// <summary>
/// Controls the error message.
/// </summary>
public class ErrorMessage
{
    #region Fields
    /// <summary>
    /// The model ID
    /// </summary>
    string m_modelID = null;
    /// <summary>
    /// The type
    /// </summary>
    string m_type = null;
    /// <summary>
    /// The entity path
    /// </summary>
    string m_entityPath = null;
    /// <summary>
    /// The message
    /// </summary>
    string m_message = null;
    #endregion

    #region Property
    /// <summary>
    /// get/set the model ID 
    /// </summary>
    public string ModelID
    {
        get { return this.m_modelID; }
    }
    /// <summary>
    /// get/set the type
    /// </summary>
    public string Type
    {
        get { return this.m_type; }
    }
    /// <summary>
    /// get/set the entity path
    /// </summary>
    public string EntityPath
    {
        get { return this.m_entityPath; }
    }
    /// <summary>
    /// get/set the message
    /// </summary>
    public string Message
    {
        get { return this.m_message; }
    }
    #endregion

    /// <summary>
    /// Creates the new "ErrorMessage".
    /// </summary>
    private ErrorMessage()
    {
    }

    /// <summary>
    /// Creates the new "ErrorMessage" with some parameters.
    /// </summary>
    /// <param name="modelID">the model ID</param>
    /// <param name="type">data type.</param>
    /// <param name="entityPath">the entity path</param>
    /// <param name="message">the error message</param>
    public ErrorMessage(string modelID, string type, string entityPath, string message)
    {
        this.m_modelID = modelID;
        this.m_type = type;
        this.m_entityPath = entityPath;
        this.m_message = message;
    }
}
