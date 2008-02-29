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

namespace EcellLib.StaticDebugWindow
{
    /// <summary>
    /// Controls the static debug.
    /// </summary>
    public class StaticDebugWindow : IEcellPlugin
    {
        #region Fields
        /// <summary>
        /// The "DataMenager"
        /// </summary>
        DataManager m_dManager;
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
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">the current simulation time</param>
        public void AdvancedTime(double time)
        {
            // nothing
        }

        /// <summary>
        ///  When the system status is changed, the menu is changed to enable/disable.
        /// </summary>
        /// <param name="type">the status type</param>
        public void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Loaded) m_staticDebug.Enabled = true;
            else m_staticDebug.Enabled = false;
        }

        /// <summary>
        /// Change availability of undo/redo function.
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The list of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID before values are changed</param>
        /// <param name="key">the ID before values are changed</param>
        /// <param name="type">the data type before values are changed</param>
        /// <param name="data">the data after values are changed</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID of the deleted object</param>
        /// <param name="key">the ID of the deleted object</param>
        /// <param name="type">the data type of the deleted object</param>
        public void DataDelete(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID.</param>
        public void ParameterAdd(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is deleted.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterDelete(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is set.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterSet(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// Returns items of the menu strip used on the main menu.
        /// </summary>
        /// <returns>items of the menu strip</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StaticDebugWindow));
            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_staticDebug = new ToolStripMenuItem();
            m_staticDebug.Name = "MenuItemStaticDebug";
            m_staticDebug.Size = new Size(96, 22);
            m_staticDebug.Text = StaticDebugWindow.s_resources.GetString("MenuItemStaticDebugText");
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
        public string GetPluginName()
        {
            return "StaticDebugWindow";
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
        /// Returns items of the menu strip used on the toolbar.
        /// </summary>
        /// <returns>items of the menu strip</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Returns window forms used on the main window.
        /// </summary>
        /// <returns>window forms</returns>
        public List<EcellDockContent> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// Checks whether this plugin can print the display image.
        /// </summary>
        /// <returns>false(Fixed)</returns>
        public List<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            return names;
        }

        /// <summary>
        /// Checks whether this plugin is the "MessageWindow".
        /// </summary>
        /// <returns>false(Fixed)</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">the model ID of the object to which values are changed</param>
        /// <param name="key">the ID of the object to which values are changed</param>
        /// <param name="type">the data type of the object to which values are changed</param>
        /// <param name="propName">the property name of the object to which values are changed</param>
        /// <param name="data">changed values of the object</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="key">the IDt</param>
        /// <param name="type">the data type</param>
        /// <param name="path">the path of the entity</param>
        public void LoggerAdd(string modelID, string key, string type, string path)
        {
            // nothing
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">the log type</param>
        /// <param name="message">the message</param>
        public void Message(string type, string message)
        {
            // nothing
        }

        /// <summary>
        /// Returns the bitmap image that converts the display image on this plugin.
        /// </summary>
        /// <returns>the bitmap image</returns>
        public Bitmap Print(string name)
        {
            return null;
        }

        /// <summary>
        /// Saves the model to the selected directory.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="directory">the selected directory</param>
        public void SaveModel(string modelID, string directory)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">the selected model ID</param>
        /// <param name="key">the selected ID</param>
        /// <param name="type">the selected data type</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID generating warning data</param>
        /// <param name="key">the ID generating warning data</param>
        /// <param name="type">the data type generating warning data</param>
        /// <param name="warntype">the type of warning data</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            // nothing
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
        }

        /// <summary>
        /// Close the current project..
        /// </summary>
        public void Clear()
        {
            this.m_errorMessageList.Clear();
        }
        #endregion

        /// <summary>
        /// Initializes validated patterns.
        /// </summary>
        void Initialize()
        {
            StaticDebugPlugin p1 = new StaticDebugForModel();
            StaticDebugPlugin p2 = new StaticDebugForNetwork();

            m_pluginDict.Add(p1.GetDebugName(), p1);
            m_pluginDict.Add(p2.GetDebugName(), p2);
        }

        /// <summary>
        /// Creates the new "StaticDebugWindow".
        /// </summary>
        public StaticDebugWindow()
        {
            this.m_dManager = DataManager.GetDataManager();
            this.m_errorMessageList = new List<ErrorMessage>();
            this.Initialize();
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

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
