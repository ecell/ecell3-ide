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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using EcellLib;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.PiccoloX.Components;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX.Nodes;
using WeifenLuo.WinFormsUI.Docking;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// PathwayWindow plugin
    /// </summary>
    public class PathwayWindow : PluginBase
    {
        #region Static fields
        /// <summary>
        /// The name of default layout algorithm
        /// </summary>
        static string m_defLayout = "Grid";
        #endregion

        #region Fields
        /// <summary>
        /// PathwayView, which contains and controls all GUI-related objects.
        /// </summary>
        PathwayControl m_con;

        /// <summary>
        /// ModelID of Ecell "Model" which is currently focused on.
        /// </summary>
        string m_modelId = "";

        /// <summary>
        /// A list for layout algorithms, which implement ILayoutAlgorithm.
        /// </summary>
        private List<ILayoutAlgorithm> m_layoutList = new List<ILayoutAlgorithm>();

        /// <summary>
        /// A list for menu of layout algorithm, which implement ILayoutAlgorithm.
        /// </summary>
        private List<ToolStripMenuItem> m_menuList;

        /// <summary>
        /// A list for menu of layout algorithm, which implement ILayoutAlgorithm.
        /// </summary>
        private List<ToolStripMenuItem> m_menuLayoutList;

        /// <summary>
        /// Index for a default layout algorithm in m_layoutList
        /// </summary>
        private int m_defLayoutIdx = -1;

        /// <summary>
        /// ToolStripMenuItem for switching visibility of IDs on each PPathwayNode
        /// </summary>
        private ToolStripMenuItem m_showIdItem;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));
        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        private DataManager m_dManager;

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for layout algorithm.
        /// </summary>
        public List<ILayoutAlgorithm> LayoutAlgorithm
        {
            get { return m_layoutList; }
        }
        /// <summary>
        /// Accessor for default layout algorithm.
        /// </summary>
        public ILayoutAlgorithm DefaultLayoutAlgorithm
        {
            get
            {
                if (m_defLayoutIdx == -1)
                    return new GridLayout();
                else if (m_defLayoutIdx < m_layoutList.Count)
                    return m_layoutList[m_defLayoutIdx];
                else
                    return null;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// The constructor
        /// </summary>
        public PathwayWindow()
        {
            m_dManager = DataManager.GetDataManager();

            m_con = new PathwayControl(this);
            m_con.ComponentManager = LoadComponentSettings();

            CheckLayoutAlgorithmDlls();
            CreateMenu();
        }
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
        /// <param name="modelID">the key of object.</param>
        /// <param name="modelID">the type of object.</param>
        /// <returns>the list of EcellObject.</returns>
        public EcellObject GetEcellObject(string modelID, string key, string type)
        {
            return m_dManager.GetEcellObject(modelID, key ,type);
        }
        #endregion

        #region Methods to notify from inside (pathway) to outside(ECell Core)

        /// <summary>
        /// Inform the adding of EcellOBject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="list">the list of added object.</param>
        /// <param name="isAnchor">Whether this action is anchor or not.</param>
        public void NotifyDataAdd(List<EcellObject> list, bool isAnchor)
        {
            m_dManager.DataAdd(list, true, isAnchor);
        }

        /// <summary>
        /// Inform the changing of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="oldKey">the key of object before edit.</param>
        /// <param name="newKey">the key of object after edit.</param>
        /// <param name="type">the type of object.</param>
        /// <param name="x">x coordinate of object.</param>
        /// <param name="y">y coordinate of object.</param>
        /// <param name="offsetx">x offset of object.</param>
        /// <param name="offsety">y offset of object.</param>
        /// <param name="width">width of object.</param>
        /// <param name="height">height of object.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            string newKey,
            EcellObject eo,
            bool isAnchor)
        {
            if(oldKey == null || newKey == null || eo.type == null)
                return;
            try
            {
                eo.key = newKey;
                m_dManager.DataChanged(eo.modelID, oldKey, eo.type, eo, true, isAnchor);
            }
            catch (IgnoreException)
            {
                this.DataChanged(eo.modelID, newKey, eo.type, eo);
            }
        }

        /// <summary>
        /// Inform the deleting of EcellObject in PathwayEditor to DataManager.
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
        /// Inform the deleting of EcellObject in PathwayEditor to DataManager.
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
                ex.ToString();
                String errmes = m_resources.GetString("ErrMerge");
                MessageBox.Show(errmes + "\n" + ex.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        /// <summary>
        /// Inform the selected EcellObject in PathwayEditor to PluginManager.
        /// </summary>
        /// <param name="modelID">the modelID of selected object.</param>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        /// <param name="isSelected">Is object is selected or not</param>
        public void NotifySelectChanged(string modelID, string key, string type, bool isSelected)
        {
            PluginManager pm = PluginManager.GetPluginManager();
            if(isSelected)
                pm.AddSelect(modelID, key, type);
            else
                pm.RemoveSelect(modelID, key, type);
            pm.SelectChanged(modelID, key, type);

        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for PathwayWindow plugin.
        /// </summary>
        /// <returns>the list of menu.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            return m_menuList;
        }

        /// <summary>
        /// Called when one of layout menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void eachLayoutItem_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                return;
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            int layoutIdx = 0;
            int subIdx = 0;
            try
            {
                if( ((ToolStripMenuItem)sender).Tag is int)
                {
                    layoutIdx = (int)item.Tag;
                    subIdx = -1;
                }
                else
                {
                    string[] tags = ((string)item.Tag).Split(',');
                    layoutIdx = int.Parse(tags[0]);
                    subIdx = int.Parse(tags[1]);
                }
            }
            catch(Exception)
            {
                MessageBox.Show(m_resources.GetString("ErrLayout"));
            }
            ILayoutAlgorithm algorithm = m_layoutList[layoutIdx];

            m_con.DoLayout(algorithm, subIdx, false);
        }

        /// <summary>
        /// Get toolbar buttons for PathwayWindow plugin.
        /// </summary>
        /// <returns>the list of ToolBarMenu.</returns>
        public List<System.Windows.Forms.ToolStripItem> GetToolBarMenuStripItems()
        {
            return m_con.GetToolBarMenuStripItems();
        }

        /// <summary>
        /// Called by PluginManager for getting UseControl.
        /// UseControl for pathway is created and configurated in the PathwayView instance actually.
        /// PathwayWindow get it and attach some delegates to them and pass it to PluginManager.
        /// </summary>
        /// <returns>UserControl with pathway canvases, etc.</returns>
        public List<DockContent> GetWindowsForms()
        {
            List<DockContent> list = new List<DockContent>();
            list.Add(m_con.PathwayView);
            list.Add(m_con.OverView);
            list.Add(m_con.LayerView);
            return list;
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(int type)
        {
            bool isShow = false;
            if (type == Util.LOADED)
            {
                isShow = true;
            }

            foreach (ToolStripMenuItem t in m_menuLayoutList)
            {
                t.Enabled = isShow;
            }
        }

        /// <summary>
        /// Change availability of undo/redo status
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            //if (!m_hasComponentSetting)
            //    return;

            try
            {
                m_modelId = "";
                m_con.Clear();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Called by PluginManager for newly added EcellObjects on the core.
        /// </summary>
        /// <param name="data">List of EcellObjects to be added</param>
        public void DataAdd(List<EcellObject> data)
        {
            if (data == null || data.Count == 0)
                return;

            bool isOtherModel = false;  // Whether this model is the same as currently displayed model or not.
            string modelId = null;
            foreach(EcellObject obj in data)
            {
                modelId = obj.modelID;
                if (!(obj is EcellSystem))
                    continue;
                if (obj.modelID == null || m_modelId.Equals(obj.modelID))
                    continue;

                isOtherModel = true;
                m_modelId = obj.modelID;
                break;
            }

            bool layoutFlag = false;
            try
            {
                if (isOtherModel)
                {
                    // Create new canvas
                    this.m_con.CreateCanvas(modelId);
                    // New model will be added.
                    string fileName = m_dManager.GetDirPath(modelId) + "\\" + modelId + ".leml";
                    if (File.Exists(fileName))
                        this.SetPositionFromLeml(fileName, data);
                    else
                        layoutFlag = true;
                }
                this.NewDataAddToModel(data);

                if (layoutFlag)
                    m_con.DoLayout(DefaultLayoutAlgorithm, 0, true);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
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
            // Null Check.
            if (String.IsNullOrEmpty(modelID) || String.IsNullOrEmpty(key) || String.IsNullOrEmpty(type))
                return;
            if (data == null)
                return;
            try
            {
                m_con.DataChanged(modelID, key, type, data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            if (type == null)
                return;
            if (type.Equals(PathwayControl.MODEL_STRING))
                this.Clear();
            CanvasView canvas = this.m_con.CanvasDictionary[modelID];
            if (canvas != null)
                canvas.DataDelete(key, ComponentManager.ParseComponentKind(type));
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true.</returns>
        public bool IsEnablePrint()
        {
            return true;
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false.</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="log">a list of LogData</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> log)
        {
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public void LoggerAdd(string modelID, string type, string key, string path)
        {
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print()
        {
            if (m_con != null)
                return m_con.Print();
            else
                return new Bitmap(1,1);
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public void SaveModel(string modelID, string directory)
        {
            if(String.IsNullOrEmpty(modelID) || String.IsNullOrEmpty(directory))
                return;
            if (!this.m_con.CanvasDictionary.ContainsKey(modelID))
                return;

            List<EcellObject> list = new List<EcellObject>();
            list.AddRange(m_con.GetSystemList(modelID));
            list.AddRange(m_con.GetNodeList(modelID));
            string fileName = directory + "\\" + modelID + ".leml";
            EcellSerializer.SaveAsXML(list, fileName);
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            if (type == null || type == "Model" || type == "Project")
                return;
            CanvasView canvas = this.m_con.CanvasDictionary[modelID];
            ComponentType cType = ComponentManager.ParseComponentKind(type);
            if (canvas != null)
                canvas.SelectChanged(key, cType);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // not implement
            //PPathwayObject obj = m_view.ActiveCanvas.
            this.m_con.ActiveCanvas.SelectChanged(key, ComponentManager.ParseComponentKind(type));
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
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
        public void WarnData(string modelID, string key, string type, string warntype)
        {
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"PathwayWindow"</returns> 
        public string GetPluginName()
        {
            return "PathwayWindow";
        }
        /// <summary>
        /// Get a temporary key of EcellObject.
        /// </summary>
        /// <param name="type">The data type of EcellObject.</param>
        /// <param name="key">The ID of parent system.</param>
        /// <returns>"TemporaryID"</returns> 
        public string GetTemporaryID(string modelID, string type, string systemID)
        {
            return m_dManager.GetTemporaryID(modelID, type, systemID);
        }

        /// <summary>
        /// Get version of this plugin.
        /// </summary>
        /// <returns>version</returns>
        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
            if (data == null)
                return;

            m_con.SetPosition(data.modelID, data);
        }
        #endregion

        #region Internal use
        /// <summary>
        /// Load ComponentSettings from default setting file.
        /// </summary>
        private ComponentManager LoadComponentSettings()
        {
            // Read component settings from ComopnentSettings.xml
            // string settingFile = PathUtil.GetEnvironmentVariable4DirPath("ecellide_plugin");
            string settingFile = EcellLib.Util.GetPluginDir();
            settingFile += "\\pathway\\ComponentSettings.xml";

            ComponentManager manager = new ComponentManager();

            XmlDocument xmlD = new XmlDocument();
            try
            {
                xmlD.Load(settingFile);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(m_resources.GetString("ErrNotComXml"), "WARNING", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                xmlD = null;
            }

            if (xmlD != null)
            {
                // All ComponentSettings in the xml file are in valid format or not.
                bool isAllValid = true;
                // If any ComponentSettings in the xml file is invalid, these messages are shown.
                List<string> lackMessages = new List<string>();
                int csCount = 1;

                // Read ComponentSettings information from xml file.
                foreach (XmlNode componentNode in xmlD.ChildNodes[0].ChildNodes)
                {
                    ComponentSetting cs = new ComponentSetting();

                    String componentKind = componentNode.Attributes["kind"].Value;

                    bool isDefault = false;
                    if (componentNode.Attributes["isDefault"] != null
                     && componentNode.Attributes["isDefault"].ToString().ToLower().Equals("true"))
                        isDefault = true;

                    try
                    {
                        cs.ComponentType = ComponentManager.ParseComponentKind(componentKind);
                    }
                    catch (NoSuchComponentKindException e)
                    {
                        MessageBox.Show(m_resources.GetString("ErrCreateKind") + "\n\n" + e.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    foreach (XmlNode parameterNode in componentNode.ChildNodes)
                    {
                        if ("Name".Equals(parameterNode.Name))
                        {
                            cs.Name = parameterNode.InnerText;
                        }
                        else if ("Color".Equals(parameterNode.Name))
                        {
                            Brush brush = PathUtil.GetBrushFromString(parameterNode.InnerText);
                            if (brush != null)
                            {
                                cs.NormalBrush = brush;
                            }
                        }
                        else if ("Drawings".Equals(parameterNode.Name))
                        {
                            foreach (XmlNode drawNode in parameterNode.ChildNodes)
                            {
                                if (drawNode.Attributes["type"] != null)
                                    cs.AddFigure(drawNode.Attributes["type"].Value, drawNode.InnerText);
                            }
                        }
                        else if ("Class".Equals(parameterNode.Name))
                        {
                            cs.AddComponentClass(parameterNode.InnerText);
                        }
                    }

                    List<string> lackInfos = cs.Validate();

                    if (lackInfos == null)
                    {
                        switch (cs.ComponentType)
                        {
                            case ComponentType.System:
                                manager.RegisterSystemSetting(cs.Name, cs, isDefault);
                                break;
                            case ComponentType.Process:
                                manager.RegisterProcessSetting(cs.Name, cs, isDefault);
                                break;
                            case ComponentType.Variable:
                                manager.RegisterVariableSetting(cs.Name, cs, isDefault);
                                break;
                        }
                    }
                    else
                    {
                        isAllValid = false;
                        string nameForCs = null;
                        if (cs.Name == null)
                            nameForCs = "ComponentSetting No." + csCount;
                        else
                            nameForCs = cs.Name;
                        foreach (string lackInfo in lackInfos)
                            lackMessages.Add(nameForCs + " lacks " + lackInfo + "\n");
                    }
                    csCount++;
                }

                string warnMessage = "";

                if (manager.DefaultSystemSetting == null)
                    warnMessage += m_resources.GetString("ErrCompSystem") + "\n\n";
                if (manager.DefaultVariableSetting == null)
                    warnMessage += m_resources.GetString("ErrCompVariable") + "\n\n";
                if (manager.DefaultProcessSetting == null)
                    warnMessage += m_resources.GetString("ErrCompProcess") + "\n\n";

                if (!isAllValid)
                {
                    warnMessage += m_resources.GetString("ErrCompInvalid");
                    if (lackMessages.Count != 1)
                        warnMessage += "s";
                    warnMessage += "\n";

                    foreach (string msg in lackMessages)
                    {
                        warnMessage += "    " + msg;
                    }

                }

                if (warnMessage != null && warnMessage.Length != 0)
                {
                    MessageBox.Show(warnMessage, "WARNING by PathwayWindow", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                }
            }
            return manager;
        }

        /// <summary>
        /// Check layout algorithm's dlls in a plugin\pathway directory and register them
        /// to m_layoutList
        /// </summary>
        private void CheckLayoutAlgorithmDlls()
        {
            // Read component settings from ComopnentSettings.xml
            // string pathwayDir = PathUtil.GetEnvironmentVariable4DirPath("ecellide_plugin");
            string pathwayDir = EcellLib.Util.GetPluginDir();

            pathwayDir += "\\pathway";
            
            foreach(string pluginName in Directory.GetFiles(pathwayDir))
            {
                // Only dlls will be loaded (NOT xml)!
                if (string.IsNullOrEmpty(pluginName) || !pluginName.EndsWith(EcellLib.Util.s_dmFileExtension))
                    continue;
                try
                {
                    Assembly handle = Assembly.LoadFile(pluginName);
                    string className = Path.GetFileName(pluginName).Replace(EcellLib.Util.s_dmFileExtension, "");
                    
                    foreach(Type type in handle.GetTypes())
                    {
                        foreach (Type intType in type.GetInterfaces())
                        {
                            if (!intType.Name.Equals("ILayoutAlgorithm"))
                                continue;

                            Object anAllocator = type.InvokeMember(
                                null,
                                BindingFlags.CreateInstance,
                                null,
                                null,
                                null);
                            m_layoutList.Add((ILayoutAlgorithm)anAllocator);
                            
                            string name = ((ILayoutAlgorithm)anAllocator).GetName();
                            if (!string.IsNullOrEmpty(name) && m_defLayout.Equals(name))
                                m_defLayoutIdx = m_layoutList.Count - 1;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void CreateMenu()
        {
            this.m_menuList = new List<ToolStripMenuItem>();
            this.m_menuLayoutList = new List<ToolStripMenuItem>();

            // Setup menu
            m_showIdItem = new ToolStripMenuItem();
            m_showIdItem.CheckOnClick = true;
            m_showIdItem.CheckState = CheckState.Checked;
            //            m_showIdItem.Text = "Show IDs(Pathway)";
            m_showIdItem.ToolTipText = "Visibility of Node's name of each pathway object";
            m_showIdItem.Text = m_resources.GetString("MenuItemShowIDText");
            m_showIdItem.Click += new EventHandler(ShowIdClick);

            ToolStripMenuItem viewMenu = new ToolStripMenuItem();
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { m_showIdItem });
            viewMenu.Text = "Setup";
            viewMenu.Name = "MenuItemView";

            m_menuList.Add(viewMenu);

            // Edit menu
            ToolStripMenuItem editMenu = new ToolStripMenuItem();
            editMenu.Text = "Edit";
            editMenu.Name = "MenuItemEdit";

            ToolStripSeparator separator = new ToolStripSeparator();

            ToolStripMenuItem deleteMenu = new ToolStripMenuItem();
            deleteMenu.Text = m_resources.GetString("DeleteMenuText");
            deleteMenu.Name = "MenuItemPaste";
            deleteMenu.Click += new EventHandler(m_con.DeleteClick);
            deleteMenu.ShortcutKeys = Keys.Delete;
            deleteMenu.ShowShortcutKeys = true;
            ToolStripMenuItem cutMenu = new ToolStripMenuItem();
            cutMenu.Text = m_resources.GetString("CutMenuText");
            cutMenu.Name = "MenuItemCut";
            cutMenu.Click += new EventHandler(m_con.CutClick);
            cutMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            cutMenu.ShowShortcutKeys = true;
            ToolStripMenuItem copyMenu = new ToolStripMenuItem();
            copyMenu.Text = m_resources.GetString("CopyMenuText");
            copyMenu.Name = "MenuItemCopy";
            copyMenu.Click += new EventHandler(m_con.CopyClick);
            copyMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            copyMenu.ShowShortcutKeys = true;
            ToolStripMenuItem pasteMenu = new ToolStripMenuItem();
            pasteMenu.Text = m_resources.GetString("PasteMenuText");
            pasteMenu.Name = "MenuItemPaste";
            pasteMenu.Click += new EventHandler(m_con.PasteClick);
            pasteMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            pasteMenu.ShowShortcutKeys = true;

            editMenu.DropDownItems.AddRange(new ToolStripItem[] { cutMenu, copyMenu, pasteMenu, deleteMenu });
            m_menuList.Add(editMenu);

            // Layout menu
            ToolStripMenuItem layoutMenu = new ToolStripMenuItem();
            layoutMenu.Text = "Layout";
            layoutMenu.Name = "MenuItemLayout";

            int count = 0; // index for position in m_layoutList
            foreach (ILayoutAlgorithm algo in m_layoutList)
            {
                ToolStripMenuItem eachLayoutItem = new ToolStripMenuItem();
                eachLayoutItem.Text = algo.GetMenuText();
                eachLayoutItem.Tag = count;
                eachLayoutItem.ToolTipText = algo.GetToolTipText();
                eachLayoutItem.Click += new EventHandler(eachLayoutItem_Click);

                List<string> subCommands = algo.GetSubCommands();
                if (subCommands != null && subCommands.Count != 0)
                {
                    int subCount = 0;
                    foreach (string subCommandName in subCommands)
                    {
                        ToolStripMenuItem layoutSubItem = new ToolStripMenuItem();
                        layoutSubItem.Text = subCommandName;
                        layoutSubItem.Tag = count + "," + subCount;
                        layoutSubItem.Click += new EventHandler(eachLayoutItem_Click);
                        eachLayoutItem.DropDownItems.Add(layoutSubItem);
                        subCount++;
                    }
                }

                layoutMenu.DropDownItems.Add(eachLayoutItem);
                m_menuLayoutList.Add(eachLayoutItem);
                count += 1;
            }

            m_menuList.Add(layoutMenu);
        }

        /// <summary>
        /// This method was made for dividing long and redundant DataAdd method.
        /// So, used by DataAdd only.
        /// </summary>
        /// <param name="data">The same argument for DataAdd</param>
        private void NewDataAddToModel(List<EcellObject> data)
        {
            // These new EcellObjects will be loaded onto the Model currently displayed
            foreach (EcellObject obj in data)
            {
                if (!(obj is EcellSystem || obj is EcellProcess || obj is EcellVariable))
                    continue;
                try
                {
                    m_con.AddNewObj(obj.modelID,
                                    obj.parentSystemID,
                                    obj,
                                    true);
                    if (obj is EcellSystem)
                        foreach (EcellObject node in obj.M_instances)
                            m_con.AddNewObj(node.modelID,
                                            node.parentSystemID,
                                            node.Copy(),
                                            true);

                } catch (Exception ex)
                {
                    throw new PathwayException(m_resources.GetString("ErrUnknowType") + "\n" + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// This method was made for dividing long and redundant DataAdd method.
        /// So, used by DataAdd only.
        /// </summary>
        /// <param name="fileName">Leml file path</param>
        /// <param name="data">The same argument for DataAdd</param>
        private void SetPositionFromLeml(string fileName, List<EcellObject> data)
        {
            // Deserialize objects from a file
            List<EcellObject> objList = EcellSerializer.LoadFromXML(fileName);

            // Create Object dictionary.
            Dictionary<string, EcellObject> objDict = new Dictionary<string, EcellObject>();
            foreach (EcellObject eo in objList)
                objDict.Add(eo.type + ":" + eo.key, eo);
            // Set position.
            string dictKey;
            foreach (EcellObject eo in data)
            {
                dictKey = eo.type + ":" + eo.key;
                if (objDict.ContainsKey(dictKey))
                    eo.SetPosition(objDict[dictKey]);
                if (eo.M_instances == null)
                    continue;
                foreach(EcellObject child in eo.M_instances)
                {
                    dictKey = child.type + ":" + eo.key;
                    if (objDict.ContainsKey(dictKey))
                        child.SetPosition(objDict[dictKey]);
                }
            }
        }
        #endregion

        #region Event delegate
        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        void ShowIdClick(object sender, EventArgs e)
        {
            if (m_showIdItem.CheckState == CheckState.Checked)
                m_con.ShowingID = true;
            else
                m_con.ShowingID = false;
        }
        #endregion
    }
}
