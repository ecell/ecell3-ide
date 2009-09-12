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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Ecell.Exceptions;
using Ecell.Logger;
using Ecell.Logging;
using Ecell.Objects;
using WeifenLuo.WinFormsUI.Docking;

namespace Ecell.Plugin
{
    /// <summary>
    /// Manage class for the loaded plugin.
    /// </summary>
    public class PluginManager
    {
        #region Fields
        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        private ApplicationEnvironment m_env;
        /// <summary>
        /// m_pluginList (loaded plugin list)
        /// </summary>
        private Dictionary<string, IEcellPlugin> m_pluginList;
        /// <summary>
        /// </summary>
        private List<IDataHandler> m_dataHandlerList;
        /// <summary>
        /// </summary>
        private List<IAnalysis> m_analysisList;
        /// <summary>
        /// </summary>
        private List<IRasterizable> m_rasterizableList;
        /// <summary>
        /// </summary>
        private List<ILayoutAlgorithm> m_layoutAlgorithmList;
        /// <summary>
        /// m_dialog (Printer Dialog with .NET framerowk)
        /// </summary>
        private PrintDialog m_dialog;
        /// <summary>
        /// m_imageList
        /// </summary>
        private ImageList m_imageList;
        /// <summary>
        /// m_version (Application Version Information)
        /// </summary>
        private Version m_version;
        /// <summary>
        /// The owner of the DockPanel (MainWindow)
        /// </summary>
        private IDockOwner m_dockOwner;
        /// <summary>
        /// </summary>
        private IRootMenuProvider m_rootMenuProvider;
        /// <summary>
        /// </summary>
        private IDiagramEditor m_diagramEditor;
        /// <summary>
        /// Status of the current project.
        /// </summary>
        private ProjectStatus m_status;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Delegate> m_delegateDic = new Dictionary<string, Delegate>();

        private Stopwatch stopWatch = new Stopwatch();
        #endregion

        /// <summary>
        /// constructer for PluginManager.
        /// </summary>
        public PluginManager(ApplicationEnvironment env)
        {
            this.m_env = env;
            this.m_pluginList = new Dictionary<string, IEcellPlugin>();
            this.m_rasterizableList = new List<IRasterizable>();
            this.m_dataHandlerList = new List<IDataHandler>();
            this.m_analysisList = new List<IAnalysis>();
            this.m_layoutAlgorithmList = new List<ILayoutAlgorithm>();
            this.m_dialog = new System.Windows.Forms.PrintDialog();
            this.m_status = ProjectStatus.Uninitialized;

            // default image type
            m_imageList = new NodeImageComponent().ImageList;
        }

        /// <summary>
        /// get/set main form of application.
        /// </summary>
        public DockPanel DockPanel
        {
            get
            {
                DockPanel panel = null;
                if(this.m_dockOwner != null)
                    panel = this.m_dockOwner.DockPanel;
                return panel;
            }
        }

        public IRootMenuProvider RootMenuProvider
        {
            get { return m_rootMenuProvider; }
        }

        public IDiagramEditor DiagramEditor
        {
            get { return m_diagramEditor; }
        }

        /// <summary>
        /// get/set version of application.
        /// </summary>
        public Version AppVersion
        {
            get { return this.m_version; }
            set { this.m_version = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IEcellPlugin> Plugins
        {
            get
            {
                List<IEcellPlugin> list = new List<IEcellPlugin>();
                foreach (IEcellPlugin plugin in m_pluginList.Values)
                    list.Add(plugin);
                return list;
            }
        }

        public IEnumerable<IRasterizable> Rasterizables
        {
            get { return m_rasterizableList; }
        }

        public IEnumerable<IDataHandler> DataHandlers
        {
            get { return m_dataHandlerList; }
        }

        public IEnumerable<ILayoutAlgorithm> LayoutAlgorithms
        {
            get { return m_layoutAlgorithmList; }
        }
    
        /// <summary>
        /// get /set ImageList
        /// </summary>
        /// <returns></returns>
        public ImageList NodeImageList
        {
            get { return m_imageList; }
        }

        /// <summary>
        /// get status of the current project.
        /// </summary>
        public ProjectStatus Status
        {
            get { return this.m_status; }
        }

        /// <summary>
        /// event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="obj">selected object</param>
        public void SelectChanged(EcellObject obj)
        {
            SelectChanged(obj.ModelID, obj.Key, obj.Type);
        }

        /// <summary>
        /// event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">selected the model ID</param>
        /// <param name="key">selected the key ID</param>
        /// <param name="type">selected the data type</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                //stopWatch.Start();
                p.SelectChanged(modelID, key, type);
                //stopWatch.Stop();
                //Trace.WriteLine(((IEcellPlugin)p).GetPluginName() + stopWatch.ElapsedMilliseconds);
                //stopWatch.Reset();
            }
            m_env.ReportManager.SetStatus(
                StatusBarMessageKind.Generic,
                key);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.AddSelect(modelID, key, type);
            }
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.RemoveSelect(modelID, key, type);
            }
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                //stopWatch.Start();
                p.ResetSelect();
                //stopWatch.Stop();
                //Trace.WriteLine(((IEcellPlugin)p).GetPluginName() + stopWatch.ElapsedMilliseconds);
                //stopWatch.Reset();
            }
        }

        /// <summary>
        /// event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID before value change</param>
        /// <param name="key">the key ID before value change</param>
        /// <param name="type">the data type before value change</param>
        /// <param name="data">changed value of data</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.DataChanged(modelID, key, type, data);
            }
        }

        /// <summary>
        /// event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">value of the adding object</param>
        public void DataAdd(List<EcellObject> data)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                // Show statusbar message.
                if (m_status == ProjectStatus.Loading)
                {
                    m_env.ReportManager.SetStatus(
                        StatusBarMessageKind.Generic,
                        string.Format(MessageResources.InfoDataAdd,((IEcellPlugin)p).GetPluginName()) );

                }
                p.DataAdd(data);
            }
            // Reset 
            if (m_status == ProjectStatus.Loading)
            {
                m_env.ReportManager.SetStatus(
                    StatusBarMessageKind.Generic,
                    "");

            }

        }

        /// <summary>
        /// event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">the deleting model ID</param>
        /// <param name="key">the deleting key ID</param>
        /// <param name="type">the deleting data type</param>
        public void DataDelete(string modelID, string key, string type)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.DataDelete(modelID, key, type);
            }
        }

        /// <summary>
        /// The event sequence when the parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="paramID">The added model ID.</param>
        public void ParameterAdd(string projectID, string paramID)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.ParameterAdd(projectID, paramID);
            }
        }

        /// <summary>
        /// The event sequence when the parameter is deleted.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="paramID">The deleted model ID.</param>
        public void ParameterDelete(string projectID, string paramID)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.ParameterDelete(projectID, paramID);
            }
        }

        /// <summary>
        /// The event sequence when the parameter is set.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="paramID">The set model ID.</param>
        public void ParameterSet(string projectID, string paramID)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.ParameterSet(projectID, paramID);
            }
        }

        /// <summary>
        /// The event sequence when the parameter is updated.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="paramID">The set model ID.</param>
        public void ParameterUpdate(string projectID, string paramID)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.ParameterUpdate(projectID, paramID);
            }
        }


        /// <summary>
        /// The event sequence when the user set and change the observed data.
        /// </summary>
        /// <param name="data">The observed data.</param>
        public void SetObservedData(EcellObservedData data)
        {
            foreach (IAnalysis p in m_analysisList)
            {
                p.SetObservedData(data);
            }
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        public void RemoveObservedData(EcellObservedData data)
        {
            foreach (IAnalysis p in m_analysisList)
            {
                p.RemoveObservedData(data);
            }
        }

        /// <summary>
        /// The event sequence when the user add and change the parameter data.
        /// </summary>
        /// <param name="data">The parameter data.</param>
        public void SetParameterData(EcellParameterData data)
        {
            foreach (IAnalysis p in m_analysisList)
            {
                p.SetParameterData(data);
            }
        }

        /// <summary>
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed parameter data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            foreach (IAnalysis p in m_analysisList)
            {
                p.RemoveParameterData(data);
            }
        }

        /// <summary>
        /// add the plugin to plugin list.
        /// </summary>
        /// <param name="p">plugin</param>
        public void AddPlugin(IEcellPlugin p)
        {
            EcellException ex = new EcellException(string.Format(MessageResources.ErrAdd, p.GetPluginName(), "Plugin"));
            if (p is IDockOwner)
            {
                if (m_dockOwner != null)
                    throw ex;
                m_dockOwner = (IDockOwner)p;
            }

            if (p is IDiagramEditor)
            {
                if (m_diagramEditor != null)
                    throw ex;
                m_diagramEditor = (IDiagramEditor)p;
            }

            if (p is IRootMenuProvider)
            {
                if (m_rootMenuProvider != null)
                    throw ex;
                m_rootMenuProvider = (IRootMenuProvider)p;
            }

            if (p is IDataHandler)
            {
                m_dataHandlerList.Add((IDataHandler)p);
            }

            if (p is IAnalysis)
            {
                m_analysisList.Add((IAnalysis)p);
            }

            if (p is IRasterizable)
            {
                m_rasterizableList.Add((IRasterizable)p);
            }

            if (p is ILayoutAlgorithm)
            {
                m_layoutAlgorithmList.Add((ILayoutAlgorithm)p);
            }

            if (!m_pluginList.ContainsKey(p.GetPluginName()))
            {
                p.Environment = m_env;
                p.Initialize();
                m_pluginList.Add(p.GetPluginName(), p);
                Dictionary<string, Delegate> plist = p.GetPublicDelegate();
                if (plist == null) return;

                foreach (string key in plist.Keys)
                {
                    m_delegateDic.Add(key, plist[key]);
                }
            }
        }

        public Delegate GetDelegate(string name)
        {
            if (m_delegateDic.ContainsKey(name))
                return m_delegateDic[name];
            return null;
        }

        /// <summary>
        /// event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.Clear();
            }
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="entry">Logger entry data.</param>
        public void LoggerAdd(LoggerEntry entry)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.LoggerAdd(entry);
            }
        }

        /// <summary>
        /// event sequence on advancing time.
        /// </summary>
        /// <param name="time">current simulation time</param>
        public void AdvancedTime(double time)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.AdvancedTime(time);
            }
        }

        /// <summary>
        /// Save the selected model to directory.
        /// </summary>
        /// <param name="modelID">selected model.</param>
        /// <param name="path">output directory.</param>
        public void SaveModel(string modelID, string path)
        {
            foreach (IDataHandler p in m_dataHandlerList)
            {
                p.SaveModel(modelID, path);
            }
        }

        /// <summary>
        /// add new data type to image index.
        /// </summary>
        /// <param name="key">data type</param>
        /// <param name="image">image type</param>
        /// <param name="eventFrag">image type</param>
        public void SetIconImage(string key, Image image, bool eventFrag)
        {
            if (m_imageList.Images.ContainsKey(key))
            {
                int i = GetImageIndex(key);
                m_imageList.Images[i] = image;
            }
            else
            {
                m_imageList.Images.Add(key, image);
            }
            if (eventFrag)
                RaiseNodeImageListChange();
        }

        /// <summary>
        /// get image index from data type.
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns>image index</returns>
        public int GetImageIndex(string type)
        {
            int i = -1;
            if (m_imageList.Images.ContainsKey(type))
                i = m_imageList.Images.IndexOfKey(type);
            return i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetImageIndex(EcellObject obj)
        {
            int i = -1;
            if (obj == null)
                i = -1;
            else if (m_imageList.Images.ContainsKey(obj.Layout.Figure))
                i = m_imageList.Images.IndexOfKey(obj.Layout.Figure);
            else if (m_imageList.Images.ContainsKey(obj.Type))
                i = m_imageList.Images.IndexOfKey(obj.Type);
            return i;
        }

        /// <summary>
        /// load the plugin and control the plugin.
        /// </summary>
        /// <param name="path">path of plugin dll.</param>
        public IEcellPlugin LoadPlugin(string path)
        {
            IEcellPlugin pb = null;
            string pName = Path.GetFileNameWithoutExtension(path);
            string className = "Ecell.IDE.Plugins." + pName + "." + pName;
            
            m_env.LogManager.Append(new ApplicationLogEntry(
                MessageType.Information,
                string.Format(MessageResources.InfoLoadPlugin, pName),
                this
            ));

            try
            {
                Assembly handle = path != null ?
                    Assembly.LoadFile(path) :
                    Assembly.GetCallingAssembly();
                Type aType = handle.GetType(className);
                if (aType == null)
                {
                    throw new EcellException(String.Format(MessageResources.ErrLoadFile,
                        new object[] { path }));
                }                
                pb = RegisterPlugin(aType);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                m_env.LogManager.Append(
                    new ApplicationLogEntry(
                        MessageType.Error,
                        String.Format(MessageResources.ErrLoadPlugin, className, path), this));
                m_env.Console.WriteLine(e);
                return null;
            }

            return pb;
        }

        /// <summary>
        /// Add a plugin to the registry
        /// </summary>
        /// <param name="pluginType">the plugin</param>
        public IEcellPlugin RegisterPlugin(Type pluginType)
        {
            IEcellPlugin p = null;
            try
            {
                p = (IEcellPlugin)pluginType.InvokeMember(
                    null,
                    BindingFlags.CreateInstance,
                    null,
                    null,
                    null
                );
            }
            catch (Exception e)
            {
                throw new EcellException(string.Format(MessageResources.ErrLoadPlugin, pluginType.ToString(), "RegisterPlugin"), e);
            }

            AddPlugin(p);

            return p;
        }

        /// <summary>
        /// Unload the plugin and release the plugin.
        /// </summary>
        /// <param name="p">the unloading plugin data</param>
        public void UnloadPlugin(IEcellPlugin p)
        {
            if (m_pluginList.ContainsKey(p.GetPluginName()))
            {
                m_pluginList.Remove(p.GetPluginName());
            }
        }

        /// <summary>
        /// when change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">system type</param>
        public void ChangeStatus(ProjectStatus type)
        {
            foreach (IEcellPlugin p in m_pluginList.Values)
            {
                p.ChangeStatus(type);
            }
            m_status = type;
        }

        /// <summary>
        /// Get plugin by using name of plugin.
        /// </summary>
        /// <param name="name">name of plugin</param>
        /// <returns>the plugin. if not find the plugin, return null.</returns>
        public IEcellPlugin GetPlugin(string name)
        {
            foreach (IEcellPlugin p in m_pluginList.Values)
            {
                string pname = p.GetPluginName();
                if (pname.Equals(name))
                    return p;
            }
            return null;
        }

        /// <summary>
        /// Get the name and version of plugin.
        /// </summary>
        /// <returns>dictionary of name and version. name is key. version is data.</returns>
        public Dictionary<String, String> GetPluginVersionList()
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            foreach (IEcellPlugin p in m_pluginList.Values)
            {
                result.Add(p.GetPluginName(), p.GetVersionString());
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<XmlNode> GetPluginStatus()
        {
            List<XmlNode> nodeList = new List<XmlNode>();
            foreach (IEcellPlugin p in m_pluginList.Values)
            {
                XmlNode node = p.GetPluginStatus();
                if (node != null)
                    nodeList.Add(node);
            }
            return nodeList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="status"></param>
        public void SetPluginStatus(string pluginName, XmlNode status)
        {
            IEcellPlugin plugin = GetPlugin(pluginName);
            plugin.SetPluginStatus(status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IPropertyItem> GetPropertySettings()
        {
            List<IPropertyItem> propertyNodes = new List<IPropertyItem>();
            foreach (IEcellPlugin plugin in m_pluginList.Values)
            {
                List<IPropertyItem> pluginProperties = plugin.GetPropertySettings();
                if (pluginProperties == null)
                    continue;
                propertyNodes.AddRange(pluginProperties);
            }
            return propertyNodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ILayoutPanel> GetLayoutPanels()
        {
            List<ILayoutPanel> panels = new List<ILayoutPanel>();
            foreach (ILayoutAlgorithm layoutAlgorithm in m_layoutAlgorithmList)
            {
                ILayoutPanel panel = layoutAlgorithm.Panel;
                if (panel == null)
                    continue;
                panels.Add(panel);
            }
            return panels;
        }
        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
            if (m_diagramEditor == null)
                return;
            m_diagramEditor.SetPosition(data);
        }

        #region EventHandler for NodeImageListChange
        private EventHandler m_onNodeImageListChange;
        /// <summary>
        /// Event on NodeImageList change.
        /// </summary>
        public event EventHandler NodeImageListChange
        {
            add { m_onNodeImageListChange += value; }
            remove { m_onNodeImageListChange -= value; }
        }
        /// <summary>
        /// Event on NodeImageList change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNodeImageListChange(EventArgs e)
        {
            if (m_onNodeImageListChange != null)
                m_onNodeImageListChange(this, e);
        }
        /// <summary>
        /// Raise NodeImageListChange event.
        /// </summary>
        protected void RaiseNodeImageListChange()
        {
            EventArgs e = new EventArgs();
            OnNodeImageListChange(e);
        }
        #endregion

        #region EventHandler for Refresh
        private EventHandler m_onRefresh;
        /// <summary>
        /// Event on NodeImageList change.
        /// </summary>
        public event EventHandler Refresh
        {
            add { m_onRefresh += value; }
            remove { m_onRefresh -= value; }
        }
        /// <summary>
        /// Event on NodeImageList change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRefresh(EventArgs e)
        {
            if (m_onRefresh != null)
                m_onRefresh(this, e);
        }
        /// <summary>
        /// Raise NodeImageListChange event.
        /// </summary>
        internal void RaiseRefreshEvent()
        {
            EventArgs e = new EventArgs();
            OnRefresh(e);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    public delegate void SaveSimulationResultDelegate(List<string> list);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="isNewWin"></param>
    public delegate void ShowGraphDelegate(string file, bool isNewWin);
    /// <summary>
    /// 
    /// </summary>
    public delegate void ShowDialogDelegate();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    public delegate void SetDockContentDelegate(EcellDockContent s);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dm"></param>
    /// <param name="path"></param>
    public delegate void AddDMDelegate(string dm, string path);
}
