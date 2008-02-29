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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace EcellLib
{
    /// <summary>
    /// Interface of plugins on ECell IDE.
    /// </summary>
    public abstract class PluginBase : IEcellPlugin
    {
        #region Fields
        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        protected DataManager m_dManager;

        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        protected PluginManager m_pManager;
        #endregion

        #region Accessors
        /// <summary>
        /// Returns the DataManager instance associated to this plugin.
        /// </summary>
        public DataManager DataManager
        {
            get { return m_dManager; }
        }

        /// <summary>
        /// Returns the PluginManager instance associated to this plugin.
        /// </summary>
        public PluginManager PluginManager
        {
            get { return m_pManager; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// The constructor
        /// </summary>
        public PluginBase()
        {
            m_dManager = DataManager.GetDataManager();
            m_pManager = PluginManager.GetPluginManager();
        }
        #endregion

        #region Inherited methods from IEcellPlugin.
        #region Methods to return objects or answer.
        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>""</returns>
        public virtual string GetPluginName()
        {
            return "PluginBase";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public virtual string GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Get menustrips for each plugin.
        /// </summary>
        /// <returns>null.</returns>
        public virtual List<ToolStripMenuItem> GetMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get toolbar buttons for each plugin.
        /// </summary>
        /// <returns>null</returns>
        public virtual List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the window forms of each plugin.
        /// DockContent is a docking window class of WeifenLuo.WinFormsUI plugin.
        /// </summary>
        /// <returns>UserControl.</returns>
        //List<UserControl> GetWindowsForms();
        public virtual List<EcellDockContent> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>false.</returns>
        public virtual List<String> GetEnablePrintNames()
        {
            return null;
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>   
        public virtual Bitmap Print(string name)
        {
            return null;
        }

        /// <summary>
        /// cCeck whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false</returns>
        public virtual bool IsMessageWindow()
        {
            return false; 
        }
        #endregion

        #region Methods to receive events.
        #region Methods to handle EcellObject.
        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public virtual void DataAdd(List<EcellObject> data)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public virtual void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public virtual void DataDelete(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public virtual void SetPosition(EcellObject data)
        {
            // not implement
        }
        #endregion

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public virtual void SelectChanged(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public virtual void AddSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public virtual void RemoveSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public virtual void ResetSelect()
        {
            // not implement
        }

        /// <summary>
        /// The event sequence when the user add the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID/</param>
        public virtual void ParameterAdd(string projectID, string parameterID)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence when the user delete the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public virtual void ParameterDelete(string projectID, string parameterID)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence when the user set the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The set parameter ID.</param>
        public virtual void ParameterSet(string projectID, string parameterID)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="data">Changed value of object.</param>
        public virtual void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        public virtual void WarnData(string modelID, string key, string type, string warntype)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public virtual void LoggerAdd(string modelID, string type, string key, string path)
        {
            // not implement
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public virtual void Message(string type, string message)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public virtual void AdvancedTime(double time)
        {
            // not implement
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        ///  0:initial 1:load 2:run 3:suspend
        /// </summary>
        /// <param name="type">System status.</param>
        public virtual void ChangeStatus(ProjectStatus type)
        {
            // not implement
        }

        /// <summary>
        /// Change availability of undo/redo function.
        /// </summary>
        /// <param name="status"></param>
        public virtual void ChangeUndoStatus(UndoStatus status)
        {
            // not implement
        }

        /// <summary>
        /// Notify a plugin that it should save model-related information if necessary.
        /// </summary>
        /// <param name="modelID">ModelID of a model which is going to be saved</param>
        /// <param name="directory">A saved file must be under this directory </param>
        public virtual void SaveModel(string modelID, string directory)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>        
        public virtual void Clear()
        {
            // not implement
        }
        #endregion
        #endregion
    }
}
