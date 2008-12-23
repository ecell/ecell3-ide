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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using Ecell.Logging;
using Ecell.Objects;

namespace Ecell.Plugin
{
    /// <summary>
    /// Interface of plugins on ECell IDE.
    /// </summary>
    public abstract class PluginBase : IEcellPlugin, IToolStripProvider, IMenuStripProvider, IDockContentProvider, IDataHandler
    {
        #region Fields
        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        protected ApplicationEnvironment m_env;

        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        protected DataManager m_dManager;

        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        protected PluginManager m_pManager;

        /// <summary>
        /// MessageManager instance
        /// </summary>
        protected LogManager m_mManager;
        #endregion

        #region Accessors
        /// <summary>
        /// The DataManager instance associated to this plugin.
        /// </summary>
        public DataManager DataManager
        {
            get { return m_env.DataManager; }
        }

        /// <summary>
        /// The PluginManager instance associated to this plugin.
        /// </summary>
        public PluginManager PluginManager
        {
            get { return m_env.PluginManager; }
        }

        /// <summary>
        /// The MessageManager instance associated to this plugin.
        /// </summary>
        public LogManager MessageManager
        {
            get { return m_env.LogManager; }
        }

        /// <summary>
        /// The application environment associated to this plugin
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            set
            {
                m_env = value;
                m_dManager = value.DataManager;
                m_pManager = value.PluginManager;
                m_mManager = value.LogManager;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public PluginBase()
        {
        }
        #endregion

        #region Inherited methods from IEcellPlugin.
        #region Methods involved in the object lifecycle
        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
        }
        #endregion

        #region Methods to return objects or answer.
        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>""</returns>
        public virtual string GetPluginName()
        {
            return GetType().Name;
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
        public virtual IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get toolbar buttons of each plugin.
        /// </summary>
        /// <returns>null</returns>
        public virtual ToolStrip GetToolBarMenuStrip()
        {
            return null;
        }

        /// <summary>
        /// Get the window forms of each plugin.
        /// DockContent is a docking window class of WeifenLuo.WinFormsUI plugin.
        /// </summary>
        /// <returns>UserControl.</returns>
        //List<UserControl> GetWindowsForms();
        public virtual IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return null;
        }
        #endregion

        #region Methods to receive events.
        #region Methods to handle EcellObject.
        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public virtual void DataAdd(EcellObject data)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public virtual void DataAdd(List<EcellObject> data)
        {
            // do nothing
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
            // do nothing
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public virtual void DataDelete(string modelID, string key, string type)
        {
            // do nothing
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public virtual void SetPosition(EcellObject data)
        {
            // do nothing
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
            // do nothing
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public virtual void AddSelect(string modelID, string key, string type)
        {
            // do nothing
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public virtual void RemoveSelect(string modelID, string key, string type)
        {
            // do nothing
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public virtual void ResetSelect()
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence when the user add the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID/</param>
        public virtual void ParameterAdd(string projectID, string parameterID)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence when the user delete the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public virtual void ParameterDelete(string projectID, string parameterID)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence when the user sets the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The set parameter ID.</param>
        public virtual void ParameterSet(string projectID, string parameterID)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence when the user updates the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The set parameter ID.</param>
        public virtual void ParameterUpdate(string projectID, string parameterID)
        {
            // do nothing
        }


        /// <summary>
        /// The event sequence when the user set and change the observed data.
        /// </summary>
        /// <param name="data">The observed data.</param>
        public virtual void SetObservedData(EcellObservedData data)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public virtual void RemoveObservedData(EcellObservedData data)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence when the user add and change the parameter data.
        /// </summary>
        /// <param name="data">The parameter data.</param>
        public virtual void SetParameterData(EcellParameterData data)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed parameter data.</param>
        public virtual void RemoveParameterData(EcellParameterData data)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public virtual void LoggerAdd(string modelID, string key, string type, string path)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence to remove the message.
        /// </summary>
        /// <param name="message">the message entry object.</param>
        public virtual void RemoveMessage(ILogEntry message)
        {
            // do nothing.
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public virtual void AdvancedTime(double time)
        {
            // do nothing
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        ///  0:initial 1:load 2:run 3:suspend
        /// </summary>
        /// <param name="type">System status.</param>
        public virtual void ChangeStatus(ProjectStatus type)
        {
            // do nothing
        }

        /// <summary>
        /// Change availability of undo/redo function.
        /// </summary>
        /// <param name="status"></param>
        public virtual void ChangeUndoStatus(UndoStatus status)
        {
            // do nothing
        }

        /// <summary>
        /// Notify a plugin that it should save model-related information if necessary.
        /// </summary>
        /// <param name="modelID">ModelID of a model which is going to be saved</param>
        /// <param name="directory">A saved file must be under this directory </param>
        public virtual void SaveModel(string modelID, string directory)
        {
            // do nothing
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>        
        public virtual void Clear()
        {
            // do nothing
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="msg"></param>
        public virtual void SetStatusBarMessage(StatusBarMessageKind kind, string msg)
        {
            // do nothing
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="percent"></param>
        public virtual void SetProgressBarValue(int percent)
        {
            // do nothing
        }

        public virtual Dictionary<string, Delegate> GetPublicDelegate()
        {
            return null;
        }
        #endregion
        #endregion

        #region Methods to get EcellObject data.
                /// <summary>
        /// Method to get current EcellObject Datas.
        /// </summary>
        public List<EcellObject> GetData(string modelID)
        {
            return m_dManager.GetData(modelID, "/");
        }
        /// <summary>
        /// Method to get a EcellObject.
        /// </summary>
        /// <param name="modelID">the model of object.</param>
        /// <param name="key">the key of object.</param>
        /// <param name="type">the type of object.</param>
        /// <returns>the list of EcellObject.</returns>
        public EcellObject GetEcellObject(string modelID, string key, string type)
        {
            return m_dManager.GetEcellObject(modelID, key, type);
        }
        /// <summary>
        /// Method to get a EcellObject.
        /// </summary>
        /// <param name="eo">the EcellObject.</param>
        public EcellObject GetEcellObject(EcellObject eo)
        {
            return m_dManager.GetEcellObject(eo.ModelID, eo.Key, eo.Type);
        }
        /// <summary>
        /// Get a temporary key of EcellObject.
        /// </summary>
        /// <param name="modelID">The model ID of EcellObject.</param>
        /// <param name="type">The ID of parent system.</param>
        /// <param name="systemID">The system ID include this object.</param>
        /// <returns>"TemporaryID"</returns> 
        public string GetTemporaryID(string modelID, string type, string systemID)
        {
            return m_dManager.GetTemporaryID(modelID, type, systemID);
        }
        #endregion

        #region Methods to notify changes from plugin to ECellLib
        /// <summary>
        /// Inform the adding of EcellOBject in plugin to DataManager.
        /// </summary>
        /// <param name="data">the list of added object.</param>
        /// <param name="isAnchor">Whether this action is anchor or not.</param>
        public void NotifyDataAdd(EcellObject data, bool isAnchor)
        {
            m_dManager.DataAdd(data, true, isAnchor);
        }
        /// <summary>
        /// Inform the adding of EcellOBject in plugin to DataManager.
        /// </summary>
        /// <param name="list">the list of added object.</param>
        /// <param name="isAnchor">Whether this action is anchor or not.</param>
        public void NotifyDataAdd(List<EcellObject> list, bool isAnchor)
        {
            m_dManager.DataAdd(list, true, isAnchor);
        }

        /// <summary>
        /// Inform the adding of logger in plugin to PluginManager.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="entityPath"></param>
        public void NotifyLoggerAdd(string modelID, string key, string type, string entityPath)
        {
            m_pManager.LoggerAdd(
                modelID,
                key,
                type,
                entityPath);
        }

        /// <summary>
        /// Inform the changing of EcellObject in plugin to DataManager.
        /// </summary>
        /// <param name="oldKey">the key of object before edit.</param>
        /// <param name="eo">The EcellObject changed the property.</param>
        /// <param name="isRecorded">Whether to record this change.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            EcellObject eo,
            bool isRecorded,
            bool isAnchor)
        {
            m_dManager.DataChanged(eo.ModelID, oldKey, eo.Type, eo, isRecorded, isAnchor);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eo"></param>
        public void NotifySetPosition(EcellObject eo)
        {
            m_dManager.SetPosition(eo);
        }

        /// <summary>
        /// Inform the deleting of EcellObject in plugin to DataManager.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="isAnchor"></param>
        public void NotifyDataDelete(string modelID, string key, string type, bool isAnchor)
        {
            m_dManager.DataDelete(modelID, key, type, true, isAnchor);
        }

        /// <summary>
        /// Inform the deleting of EcellObject in plugin to DataManager.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        public void NotifyDataMerge(string modelID, string key)
        {
            try
            {
                m_dManager.SystemDeleteAndMove(modelID, key);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// Inform the selected EcellObject in plugin to PluginManager.
        /// </summary>
        /// <param name="modelID">the modelID of selected object.</param>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void NotifySelectChanged(string modelID, string key, string type)
        {
            m_pManager.SelectChanged(modelID, key, type);

        }
        /// <summary>
        /// Inform the unselected EcellObject in plugin to PluginManager.
        /// </summary>
        /// <param name="modelID">the modelID of selected object.</param>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void NotifyAddSelect(string modelID, string key, string type)
        {
            m_pManager.AddSelect(modelID, key, type);
        }
        /// <summary>
        /// Inform the selected EcellObject in plugin to PluginManager.
        /// </summary>
        /// <param name="modelID">the modelID of selected object.</param>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void NotifyRemoveSelect(string modelID, string key, string type)
        {
            m_pManager.RemoveSelect(modelID, key, type);
        }

        /// <summary>
        /// Inform the ResetSelect() to PluginManager
        /// </summary>
        public void NotifyResetSelect()
        {
            m_pManager.ResetSelect();
        }
        #endregion
    }
}
