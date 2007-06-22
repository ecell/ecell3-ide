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


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using EcellLib;
using System.Drawing.Drawing2D;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using PathwayWindow;
using UMD.HCIL.PiccoloX.Components;
using System.Xml;
using System.IO;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX.Nodes;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using EcellLib.PathwayWindow.Element;
using EcellLib.PathwayWindow.Node;
using System.Collections;
using System.Reflection;
using System.Collections.Specialized;

namespace EcellLib.PathwayWindow
{
    #region enum
    /// <summary>
    /// Type of change to one reference of VariableReference
    /// </summary>
    public enum RefChangeType { SingleDir, BiDir, Delete };
    #endregion

    public class PathwayWindow : PluginBase
    {
        #region Static fields
        /// <summary>
        /// The name of default layout algorithm
        /// </summary>
        static string m_defLayout = "GridLayout";
        #endregion

        #region Fields
        /// <summary>
        /// PathwayView, which contains and controls all GUI-related objects.
        /// </summary>
        PathwayView m_view;

        /// <summary>
        /// ModelID of Ecell "Model" which is currently focused on.
        /// </summary>
        string m_modelId = "";

        /// <summary>
        /// Default CanvasID
        /// </summary>
        string m_defCanvasId = "Metabolic";

        /// <summary>
        /// Default LayerID
        /// </summary>
        string m_defLayerId = "first";

        /// <summary>
        /// A list for layout algorithms, which implement ILayoutAlgorithm.
        /// </summary>
        private List<ILayoutAlgorithm> m_layoutList = new List<ILayoutAlgorithm>();

        /// <summary>
        /// Index for a default layout algorithm in m_layoutList
        /// </summary>
        private int m_defLayoutIdx = -1;

        /// <summary>
        /// ToolStripMenuItem for switching visibility of IDs on each PPathwayNode
        /// </summary>
        private ToolStripMenuItem m_showIdItem;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_modelId.
        /// </summary>
        public string ModelID
        {
            get { return m_modelId; }
        }
        public List<ILayoutAlgorithm> LayoutAlgorithm
        {
            get { return m_layoutList; }
        }
        public ILayoutAlgorithm DefaultLayoutAlgorithm
        {
            get
            {
                if (m_defLayoutIdx == -1)
                    return new DefaultLayoutAlgorithm();
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
            // Read component settings from ComopnentSettings.xml
            // string settingFile = PathUtil.GetEnvironmentVariable4DirPath("ecellide_plugin");
            string settingFile = EcellLib.Util.GetPluginDir();
            settingFile += "\\pathway\\ComponentSettings.xml";

            List<ComponentSetting> componentSettings = new List<ComponentSetting>();

            XmlDocument xmlD = new XmlDocument();
            try
            {
                xmlD.Load(settingFile);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Can't find ComponentSettings.xml file in the <ECell-HOME>" +
                "\\plugins\\pathway directory", "WARNING", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                xmlD = null;
            }

            ComponentSettingsManager manager = new ComponentSettingsManager();
            
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
                        cs.ComponentKind = ComponentSetting.ParseComponentKind(componentKind);
                    }
                    catch (NoSuchComponentKindException e)
                    {
                        MessageBox.Show(e.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        componentSettings.Add(cs);
                        switch (cs.ComponentKind)
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
                    warnMessage += "Default system was not found in the ComponentSettings.xml file, so hard coded one will be used instead.\n\n";                    
                if (manager.DefaultVariableSetting == null)
                    warnMessage += "Default variable was not found in the ComponentSettings.xml file, so hard coded one will be used instead.\n\n";
                if (manager.DefaultProcessSetting == null)
                    warnMessage += "Default process was not found in the ComponentSettings.xml file, so hard coded one will be used instead.\n\n";
                
                if (!isAllValid)
                {
                    warnMessage += "ComponentSettings.xml is imcomplete for the following reason";
                    if (lackMessages.Count != 1)
                        warnMessage += "s";
                    warnMessage += "\n";

                    foreach (string msg in lackMessages)
                    {
                        warnMessage += "    " + msg;
                    }

                }

                if(warnMessage != null && warnMessage.Length != 0)
                {
                    MessageBox.Show(warnMessage, "WARNING by PathwayWindow", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                }
            }

            if(manager.DefaultSystemSetting == null)
            {
                // Set hard coded default system ComponentSettings
                ComponentSetting defSysCs = new ComponentSetting();
                defSysCs.ComponentKind = ComponentType.System;
                defSysCs.Name = "DefaultSystem";
                defSysCs.NormalBrush = Brushes.Black;
                defSysCs.AddComponentClass("PEcellSystem");
                componentSettings.Add(defSysCs);
                manager.RegisterSystemSetting(defSysCs.Name, defSysCs, true);
            }

            if(manager.DefaultVariableSetting == null)
            {
                // Set hard coded default variable ComponentSettings
                ComponentSetting defVarCs = new ComponentSetting();
                defVarCs.ComponentKind = ComponentType.Variable;
                defVarCs.Name = "DefaultVariable";
                defVarCs.NormalBrush = Brushes.LightBlue;
                defVarCs.AddFigure("Ellipse", "-30,-20,60,40");
                defVarCs.AddComponentClass("PEcellVariable");
                componentSettings.Add(defVarCs);
                manager.RegisterVariableSetting(defVarCs.Name, defVarCs, true);
            }

            if (manager.DefaultProcessSetting == null)
            {
                // Set hard coded default process ComponentSettings
                ComponentSetting defProCs = new ComponentSetting();
                defProCs.ComponentKind = ComponentType.Process;
                defProCs.Name = "DefaultProcess";
                defProCs.NormalBrush = Brushes.LightGreen;
                defProCs.AddFigure("Rectangle","-30,-20,60,40");
                defProCs.AddComponentClass("PEcellProcess");
                componentSettings.Add(defProCs);
                manager.RegisterProcessSetting(defProCs.Name, defProCs, true);
            }

            m_view = new PathwayView();
            m_view.Window = this;
            m_view.SetSettings(componentSettings);
            m_view.ComponentSettingsManager = manager;

            CheckLayoutAlgorithmDlls();
        }
        #endregion

        /// <summary>
        /// copy selected object(EcellObject) to new object.
        /// </summary>
        /// <param name="originalKey">the key of selected object.</param>
        /// <param name="newKey">the key of new object.</param>
        public void CopyEcellObject(string originalKey, string newKey)
        {
            if (originalKey == null || newKey == null)
                return;
            DataManager dm = DataManager.GetDataManager();
            List<EcellObject> list = dm.GetData(m_modelId,originalKey);
            EcellObject eo = null;
            foreach(EcellObject eachObj in list)
            {
                if(eachObj.key.Equals(originalKey))
                {
                    eo = eachObj;
                    break;
                }
            }
            if (eo == null)
                return;
            eo.key = newKey;
            List<EcellObject> newList = new List<EcellObject>();
            newList.Add(eo);
            dm.DataAdd(newList);
        }

        /// <summary>
        /// Get EcellObject from DataManager.
        /// </summary>
        /// <param name="key">the key of EcellObject.</param>
        /// <returns>EcellObject.</returns>
        public EcellObject GetData(string key, string type)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            DataManager dm = DataManager.GetDataManager();
            List<EcellObject> list = null;
            if (key.Contains(":"))
            {
                String[] data = key.Split(new char[] { ':' });
                list = dm.GetData(m_modelId, data[0]);
            }
            else
                list = dm.GetData(m_modelId, key);

            if (list == null)
            {
                return null;
            }
            foreach (EcellObject eo in list)
            {
                if (key.Equals(eo.key) && type.Equals(eo.type))
                {
                    return eo;
                }
                foreach (EcellObject subEo in eo.M_instances)
                {
                    if (key.Equals(subEo.key) && type.Equals(subEo.type))
                    {
                        return subEo;
                    }
                }
            }
            return null;
        }

        public string GetEcellData(string key, string type, string name)
        {
            if (name == null)
                return null;

            EcellObject obj = GetData(key, type);

            if (obj == null)
                return null;

            foreach (EcellData ed in obj.M_value)
            {
                if (ed.M_name.Equals(name))
                {
                    return ed.M_value.ToString();                    
                }
            }
            return null;
        }

        #region Methods to notify from inside (pathway) to outside(ECell Core)

        /// <summary>
        /// Inform the adding of EcellOBject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="list">the list of added object.</param>
        public void NotifyDataAdd(List<EcellObject> list)
        {
            DataManager dm = DataManager.GetDataManager();
            dm.DataAdd(list);
        }

        /// <summary>
        /// Inform the changing of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="oldKey">the key of object before edit.</param>
        /// <param name="newKey">the key of object after edit.</param>
        /// <param name="type">the type of object.</param>
        public void NotifyDataChanged(string oldKey, string newKey, string type)
        {
            if(oldKey == null || newKey == null || type == null)
                return;

            // Get EcellObject which has oldKey
            DataManager dm = DataManager.GetDataManager();
            if(type.Equals(PathwayView.SYSTEM_STRING))
            {
                List<EcellObject> list = dm.GetData(m_modelId, PathUtil.GetParentSystemId(oldKey));
                
                if (list == null || list.Count == 0)
                    return;

                list[0].key = newKey;
                list[0].M_instances = null;
                dm.DataChanged(m_modelId, oldKey, type, list[0]);
            }
            else
            {
                List<EcellObject> list = dm.GetData(m_modelId, PathUtil.GetParentSystemId(oldKey));
                EcellObject toBeChanged = null;
                foreach(EcellObject system in list)
                {
                    if (system.M_instances == null)
                        continue;

                    foreach(EcellObject obj in system.M_instances)
                    {
                        if (obj.key.Equals(oldKey) && obj.type.Equals(type))
                        {
                            toBeChanged = obj;
                            break;
                        }
                    }
                }
                if (toBeChanged == null)
                    return;
                
                // Change key of EcellObject to new one
                toBeChanged.key = newKey;

                // Register change to DataManager
                dm.DataChanged(m_modelId, oldKey, type, toBeChanged);
            }
        }

        /// <summary>
        /// Inform the changing of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="proKey"></param>
        /// <param name="varKey"></param>
        /// <param name="coefficient"></param>
        public void NotifyVariableReferenceChanged(string proKey, string varKey, RefChangeType changeType, int coefficient)
        {
            DataManager dm = DataManager.GetDataManager();

            List<EcellObject> list = dm.GetData(m_modelId, PathUtil.GetParentSystemId(proKey));

            EcellObject toBeChanged = null;
            foreach(EcellObject system in list)
            {
                if (system.M_instances == null)
                    continue;

                foreach(EcellObject obj in system.M_instances)
                {
                    if (obj.key.Equals(proKey) && obj.type.Equals(PathwayView.PROCESS_STRING))
                    {
                        toBeChanged = obj;
                        break;
                    }
                }
            }

            if (null == toBeChanged)
                return;
            
            foreach(EcellData data in toBeChanged.M_value)
            {
                if(data.M_name == "VariableReferenceList")
                {
                    List<EcellReference> refList = EcellReference.ConvertString(data.M_value.ToString());
                    List<EcellReference> newList = new List<EcellReference>();
                    EcellReference changedRef = null;
                    int i = 0;
                    foreach (EcellReference v in refList)
                    {
                        if (v.fullID.EndsWith(varKey))
                            changedRef = v;
                        else
                            newList.Add(v);
                    }

                    if (null != changedRef && changeType != RefChangeType.Delete)
                    {
                        switch(changeType)
                        {
                            case RefChangeType.SingleDir:
                                changedRef.coefficient = coefficient;
                                newList.Add(changedRef);
                                break;
                            case RefChangeType.BiDir:
                                EcellReference copyRef = PathUtil.CopyEcellReference(changedRef);
                                changedRef.coefficient = -1;
                                changedRef.name = PathUtil.GetNewReferenceName(newList, -1);
                                copyRef.coefficient = 1;
                                copyRef.name = PathUtil.GetNewReferenceName(newList, 1);
                                newList.Add(changedRef);
                                newList.Add(copyRef);
                                break;
                        }
                    }
                    else if(null == changedRef)
                    {
                        switch(changeType)
                        {
                            case RefChangeType.SingleDir:
                                EcellReference addRef = new EcellReference();
                                addRef.coefficient = coefficient;
                                addRef.fullID = varKey;
                                addRef.name = PathUtil.GetNewReferenceName(newList, coefficient);
                                addRef.isAccessor = 1;
                                newList.Add(addRef);
                                break;
                            case RefChangeType.BiDir:
                                EcellReference addSRef = new EcellReference();
                                addSRef.coefficient = -1;
                                addSRef.fullID = varKey;
                                addSRef.name = PathUtil.GetNewReferenceName(newList, -1);
                                addSRef.isAccessor = 1;
                                newList.Add(addSRef);

                                EcellReference addPRef = new EcellReference();
                                addPRef.coefficient = 1;
                                addPRef.fullID = varKey;
                                addPRef.name = PathUtil.GetNewReferenceName(newList, 1);
                                addPRef.isAccessor = 1;
                                newList.Add(addPRef);
                                break;
                        }
                    }

                    string refStr = "(";
                    foreach (EcellReference v in newList)
                    {
                        if (i == 0) refStr = refStr + v.ToString();
                        else refStr = refStr + ", " + v.ToString();
                    }
                    refStr = refStr + ")";
                    data.M_value = EcellValue.ToVariableReferenceList( refStr );
                }
            }
            
            dm.DataChanged(m_modelId, proKey, PathwayView.PROCESS_STRING, toBeChanged);
        }

        /// <summary>
        /// Inform the deleting of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public void NotifyDataDelete(string key, string type)
        {
            DataManager dm = DataManager.GetDataManager();
            dm.DataDelete(m_modelId, key, type);
        }

        /// <summary>
        /// Inform the selected EcellObject in PathwayEditor to PluginManager.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void NotifySelectChanged(string key, string type)
        {
            PluginManager pm = PluginManager.GetPluginManager();
            pm.SelectChanged(m_modelId, key, type);
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for PathwayWindow plugin.
        /// </summary>
        /// <returns>the list of menu.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();

            // Setup menu
            m_showIdItem = new ToolStripMenuItem();
            m_showIdItem.CheckOnClick = true;
            m_showIdItem.CheckState = CheckState.Checked;
            m_showIdItem.Text = "Show IDs(Pathway)";
            m_showIdItem.ToolTipText = "Visibility of Node's name of each pathway object";
            m_showIdItem.Click += new EventHandler(ShowIdClick);
            
            ToolStripMenuItem viewMenu = new ToolStripMenuItem();
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { m_showIdItem });
            viewMenu.Text = "Setup";
            viewMenu.Name = "MenuItemView";
            
            list.Add(viewMenu);

            // Layout menu
            ToolStripMenuItem layoutMenu = new ToolStripMenuItem();
            layoutMenu.Text = "Layout";
            layoutMenu.Name = "MenuItemLayout";

            int count = 0; // index for position in m_layoutList
            foreach (ILayoutAlgorithm algo in m_layoutList)
            {
                ToolStripMenuItem eachLayoutItem = new ToolStripMenuItem();
                eachLayoutItem.Text = algo.GetName();
                eachLayoutItem.Tag = count;
                eachLayoutItem.ToolTipText = algo.GetToolTipText();
                eachLayoutItem.Click += new EventHandler(eachLayoutItem_Click);

                List<string> subCommands = algo.GetSubCommands();
                if (subCommands != null && subCommands.Count != 0)
                {
                    int subCount = 0;
                    foreach(string subCommandName in subCommands)
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
                count += 1;
            }

            list.Add(layoutMenu);

            return list;
        }

        public void eachLayoutItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                int layoutIdx = 0;
                int subIdx = 0;
                try
                {
                    if(((ToolStripMenuItem)sender).Tag is int)
                    {
                        layoutIdx = (int)((ToolStripMenuItem)sender).Tag;
                        subIdx = -1;
                    }
                    else
                    {
                        string[] tags = ((string)((ToolStripMenuItem)sender).Tag).Split(',');
                        layoutIdx = int.Parse(tags[0]);
                        subIdx = int.Parse(tags[1]);
                    }
                }
                catch(Exception)
                {
                    MessageBox.Show("Can't execute this layout due to an unexpected error, sorry.");
                }
                ILayoutAlgorithm algorithm = m_layoutList[layoutIdx];

                if (algorithm.GetLayoutType() == LayoutType.Selected)
                {
                    m_view.LayoutSelected(algorithm,subIdx);
                }
                else
                {
                    PathwayElements elements = new PathwayElements();
                    elements.Elements = m_view.GetElements().ToArray();

                    List<SystemElement> systemList = new List<SystemElement>();
                    List<NodeElement> nodeList = new List<NodeElement>();

                    foreach (PathwayElement ele in elements.Elements)
                    {
                        if (ele is SystemElement)
                            systemList.Add((SystemElement)ele);
                        else if (ele is NodeElement)
                            nodeList.Add((NodeElement)ele);
                    }

                    algorithm.DoLayout(subIdx, false, systemList, nodeList);
                    m_view.Clear();
                    m_view.Replace(new List<PathwayElement>(elements.Elements), false);
                }
            }            
        }

        /// <summary>
        /// Get toolbar buttons for PathwayWindow plugin.
        /// </summary>
        /// <returns>the list of ToolBarMenu.</returns>
        public List<System.Windows.Forms.ToolStripItem> GetToolBarMenuStripItems()
        {
            return m_view.GetToolBarMenuStripItems();
        }

        /// <summary>
        /// Called by PluginManager for getting UseControl.
        /// UseControl for pathway is created and configurated in the PathwayView instance actually.
        /// PathwayWindow get it and attach some delegates to them and pass it to PluginManager.
        /// </summary>
        /// <returns>UserControl with pathway canvases, etc.</returns>
        public List<UserControl> GetWindowsForms()
        {
            List<UserControl> array = new List<UserControl>();
            UserControl uc = new UserControl();

            uc.Controls.Add(m_view.Control);
            uc.Dock = DockStyle.Fill;
            uc.Load += new EventHandler(m_view.SizeChanged);
            uc.Resize += new EventHandler(m_view.SizeChanged);
            uc.MouseWheel += new MouseEventHandler(uc_MouseWheel);            
            
            array.Add(uc);

            return array;
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
                m_view.Clear();
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
            bool toBeLoaded = false;// Whether there is any EcellObject should be loaded onto PathwayView.
            bool isOtherModel = false;// Whether this model is the same as currently displayed model or not.
            string modelId = null;
            foreach(EcellObject obj in data)
            {
                modelId = obj.modelID;
                if(obj.type.Equals("System"))
                {
                    toBeLoaded = true;
                    if (obj.modelID != null && !m_modelId.Equals(obj.modelID))
                    {
                        isOtherModel = true;
                        m_modelId = modelId;
                        break;
                    }
                }
                else if(obj.type.Equals("Variable") || obj.type.Equals("Process"))
                {
                    toBeLoaded = true;
                }
            }

            if (!toBeLoaded)
                return;

            try
            {
                if (isOtherModel)
                {
                    // New model will be added.
                    DataManager dm = DataManager.GetDataManager();
                    string fileName = dm.GetDirPath(modelId) + "\\" + modelId + ".leml";
                    
                    if (File.Exists(fileName))
                    {
                        this.DataAddWithLeml(fileName, data);
                    }
                    else
                    {
                        this.DataAddWithoutLeml(data);
                    }
                }
                else
                {
                    // New EcellObjects will be added to the currently selected model.
                    this.NewDataAddToModel(data);
                }
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
            if(String.IsNullOrEmpty(ModelID) || !m_modelId.Equals(modelID))
                return;

            if (type == null || String.IsNullOrEmpty(key) || data == null)
                return;
            try
            {
                if (type.Equals(PathwayView.SYSTEM_STRING))
                    m_view.DataChanged(key, data, ComponentType.System);
                else if (type.Equals(PathwayView.VARIABLE_STRING))
                    m_view.DataChanged(key, data, ComponentType.Variable);
                else if (type.Equals(PathwayView.PROCESS_STRING))
                    m_view.DataChanged(key, data, ComponentType.Process);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            if (m_modelId.Equals(modelID))
            {
                if (type.Equals(PathwayView.MODEL_STRING))
                {
                    m_modelId = "";
                    m_view.Clear();
                }
                if(type.Equals(PathwayView.SYSTEM_STRING))
                    m_view.DataDelete(key, ComponentType.System);
                else if(type.Equals(PathwayView.VARIABLE_STRING))
                    m_view.DataDelete(key, ComponentType.Variable);
                else if(type.Equals(PathwayView.PROCESS_STRING))
                    m_view.DataDelete(key, ComponentType.Process);
            }
            else
            {
                // Change Model & delete
            }
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
        /// <param name="data">Changed value of object.</param>
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
            if (m_view != null)
                return m_view.Print();
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
            {
                return;
            }
            
            if(ModelID.Equals(m_modelId))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PathwayElements));
                PathwayElements elements = new PathwayElements();
                elements.Elements = m_view.GetElements().ToArray();
                string fileName = directory + "\\" + modelID + ".leml";
                using (Stream writer = new FileStream(fileName, FileMode.Create))
                {
                    serializer.Serialize(writer, elements);
                }
            }
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="key">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            if (type == null)
                return;
            if(m_modelId.Equals(modelID))
            {
                if(type.Equals(PathwayView.SYSTEM_STRING))
                    m_view.SelectChanged(key,ComponentType.System);
                else if(type.Equals(PathwayView.VARIABLE_STRING))
                    m_view.SelectChanged(key,ComponentType.Variable);
                else if(type.Equals(PathwayView.PROCESS_STRING))
                    m_view.SelectChanged(key, ComponentType.Process);
            }
            else
            {
                // Change Model
            }
        }

        /// <summary>
        /// Set the panel that show this plugin in MainWindow.
        /// </summary>
        /// <param name="panel">The set panel.</param>
        public void SetPanel(System.Windows.Forms.Panel panel)
        {
            //this.m_panel = panel;
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="key">The data type generating warning data.</param>
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

        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        #region Internal use

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
                if (!string.IsNullOrEmpty(pluginName) && pluginName.EndsWith(EcellLib.Util.s_dmFileExtension))
                {
                    try
                    {
                        Assembly handle = Assembly.LoadFile(pluginName);

                        string className = Path.GetFileName(pluginName).Replace(EcellLib.Util.s_dmFileExtension, "");
                        
                        foreach(Type type in handle.GetTypes())
                        {
                            foreach (Type intType in type.GetInterfaces())
                            {
                                if(intType.Name.Equals("ILayoutAlgorithm"))
                                {
                                    Object anAllocator = type.InvokeMember(
                                        null,
                                        BindingFlags.CreateInstance,
                                        null,
                                        null,
                                        null
                                    );
                                    m_layoutList.Add((ILayoutAlgorithm)anAllocator);
                                    
                                    string name = ((ILayoutAlgorithm)anAllocator).GetName();
                                    if (!string.IsNullOrEmpty(name) && m_defLayout.Equals(name))
                                        m_defLayoutIdx = m_layoutList.Count - 1;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
        /*
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            throw new Exception("The method or operation is not implemented.");
        }*/

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
                string systemName = PathUtil.GetParentSystemId(obj.key);
                if (obj.type.Equals(PathwayView.SYSTEM_STRING))
                {
                    if (systemName.Equals("/"))
                    {
                        m_view.AddNewObj(m_defCanvasId, systemName, ComponentType.System, null, obj.key, false, 0, 0, 1000, 1000, false, null);
                    }
                    else
                    {
                        m_view.AddNewObj(m_defCanvasId, systemName, ComponentType.System, null, obj.key, false, 0, 0, 0, 0, false, null);
                    }
                }
                else if (obj.type.Equals(PathwayView.VARIABLE_STRING))
                    m_view.AddNewObj(m_defCanvasId, systemName, ComponentType.Variable, null, obj.key, false, 0, 0, 0, 0, false, null);                    
                else if (obj.type.Equals(PathwayView.PROCESS_STRING))
                    m_view.AddNewObj(m_defCanvasId, systemName, ComponentType.Process, null, obj.key, false, 0, 0, 0, 0, false, null);                
                else
                {
                    throw new PathwayException("Unknown EcellObject is add");
                }
            }
        }

        /// <summary>
        /// This method was made for dividing long and redundant DataAdd method.
        /// So, used by DataAdd only.
        /// </summary>
        /// <param name="fileName">Leml file path</param>
        /// <param name="data">The same argument for DataAdd</param>
        private void DataAddWithLeml(string fileName, List<EcellObject> data)
        {
            // Deserialize objects from a file
            XmlSerializer serializer = new XmlSerializer(typeof(PathwayElements));
            List<PathwayElement> pathList = new List<PathwayElement>();
            PathwayElements pathEles;
            using (Stream reader = new FileStream(fileName, FileMode.Open))
            {
                pathEles = (PathwayElements)serializer.Deserialize(reader);
            }

            Dictionary<string, ComponentElement> sysEleDict = new Dictionary<string, ComponentElement>();
            Dictionary<string, ComponentElement> varEleDict = new Dictionary<string, ComponentElement>();
            Dictionary<string, ComponentElement> proEleDict = new Dictionary<string, ComponentElement>();

            foreach (PathwayElement pathEle in pathEles.Elements)
            {
                if (pathEle is ComponentElement)
                {
                    ComponentElement comEle = (ComponentElement)pathEle;
                    if (comEle is SystemElement)
                        sysEleDict.Add(comEle.Key, comEle);
                    else if (comEle is VariableElement)
                        varEleDict.Add(comEle.Key, comEle);
                    else if (comEle is ProcessElement)
                        proEleDict.Add(comEle.Key, comEle);
                }
                else
                {
                    pathList.Add(pathEle);
                }
            }

            foreach (EcellObject obj in data)
            {
                if (obj.type.Equals(PathwayView.SYSTEM_STRING))
                {
                    if (!sysEleDict.ContainsKey(obj.key))
                    {
                        SystemElement systemElement = new SystemElement();
                        systemElement.CanvasID = m_defCanvasId;
                        systemElement.LayerID = m_defLayerId;
                        systemElement.ModelID = obj.modelID;
                        systemElement.Type = obj.type;
                        systemElement.Key = obj.key;
                        pathList.Add(systemElement);
                    }
                    else
                    {
                        sysEleDict[obj.key].Type = PathwayView.SYSTEM_STRING;
                        pathList.Add(sysEleDict[obj.key]);
                    }

                    if (obj.M_instances == null)
                        continue;
                    foreach (EcellObject node in obj.M_instances)
                    {
                        if (node.type == null)
                            return;
                        if (PathwayView.VARIABLE_STRING.Equals(node.type))
                        {
                            if (varEleDict.ContainsKey(node.key))
                            {
                                varEleDict[node.key].Type = PathwayView.VARIABLE_STRING;
                                pathList.Add(varEleDict[node.key]);
                            }
                            else if (node.key.EndsWith("SIZE"))
                            {
                                AttributeElement attrElement = new AttributeElement();
                                attrElement.Attribute = AttributeElement.AttributeType.Size;
                                attrElement.CanvasID = m_defLayerId;
                                attrElement.LayerID = m_defLayerId;
                                attrElement.ModelID = node.modelID;
                                attrElement.Key = node.key;
                                attrElement.Type = node.type;
                                attrElement.TargetModelID = node.modelID;
                                foreach (EcellData ed in node.M_value)
                                {
                                    if (ed.M_name.Equals("Value"))
                                    {
                                        attrElement.Value = ed.M_value.ToString();
                                        break;
                                    }
                                }
                                pathList.Add(attrElement);
                            }
                            else if (node.type.Equals(PathwayView.VARIABLE_STRING))
                            {
                                VariableElement varElement = new VariableElement();
                                varElement.CanvasID = m_defCanvasId;
                                varElement.LayerID = m_defLayerId;
                                varElement.ModelID = node.modelID;
                                varElement.Key = node.key;
                                varElement.Type = node.type;
                                varElement.CsId = m_view.ComponentSettingsManager.DefaultVariableSetting.Name;

                                pathList.Add(varElement);
                            }
                        }
                        else if (PathwayView.PROCESS_STRING.Equals(node.type))
                        {
                            if (proEleDict.ContainsKey(node.key))
                            {
                                proEleDict[node.key].Type = PathwayView.PROCESS_STRING;                                
                                if (proEleDict[node.key] is ProcessElement)
                                {
                                    foreach (EcellData ed in node.M_value)
                                    {
                                        if (ed.M_name.Equals("VariableReferenceList"))
                                        {
                                            ((ProcessElement)proEleDict[node.key]).SetEdgesByStr(ed.M_value.ToString());
                                        }
                                    }
                                }
                                pathList.Add(proEleDict[node.key]);
                            }
                            else
                            {
                                ProcessElement proElement = new ProcessElement();
                                foreach (EcellData ed in node.M_value)
                                {
                                    if (ed.M_name.Equals("VariableReferenceList"))
                                    {
                                        proElement.SetEdgesByStr(ed.M_value.ToString());
                                    }
                                }

                                proElement.CanvasID = m_defCanvasId;
                                proElement.LayerID = m_defLayerId;
                                proElement.ModelID = node.modelID;
                                proElement.Key = node.key;
                                proElement.Type = node.type;
                                proElement.CsId = m_view.ComponentSettingsManager.DefaultProcessSetting.Name;

                                pathList.Add(proElement);
                            }
                        }
                    }
                }
            }
            try
            {
                m_view.Replace(pathList, false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// This method was made for dividing long and redundant DataAdd method.
        /// So, used by DataAdd only.
        /// </summary>
        /// <param name="fileName">Leml file path</param>
        private void DataAddWithoutLeml(List<EcellObject> data)
        {
            string previousModelId = m_modelId;

            List<PathwayElement> pathElements = new List<PathwayElement>();

            CanvasElement metaCanvas = new CanvasElement();
            metaCanvas.CanvasID = m_defCanvasId;
            metaCanvas.HasGrid = false;
            metaCanvas.OffsetX = 0f;
            metaCanvas.OffsetY = 0f;
            pathElements.Add(metaCanvas);

            LayerElement firstLayer = new LayerElement();
            firstLayer.CanvasID = m_defCanvasId;
            firstLayer.LayerID = m_defLayerId;
            firstLayer.ZOrder = 0;
            pathElements.Add(firstLayer);

            // Currently displayed model must be saved
            foreach (EcellObject obj in data)
            {
                if (obj.type.Equals(PathwayView.SYSTEM_STRING))
                {
                    //m_modelId = obj.modelID;
                    SystemElement systemElement = new SystemElement();
                    systemElement.CanvasID = m_defCanvasId;
                    systemElement.LayerID = m_defLayerId;
                    systemElement.ModelID = obj.modelID;
                    systemElement.Type = obj.type;
                    systemElement.Key = obj.key;

                    if (obj.key.Equals("/"))
                    {
                        systemElement.Width = 300;
                        systemElement.Height = 300;
                    }

                    pathElements.Add(systemElement);

                    if (obj.M_instances == null)
                        continue;
                    foreach (EcellObject node in obj.M_instances)
                    {
                        if (node.key.EndsWith("SIZE"))
                        {
                            AttributeElement attrElement = new AttributeElement();
                            attrElement.Attribute = AttributeElement.AttributeType.Size;
                            attrElement.CanvasID = m_defLayerId;
                            attrElement.LayerID = m_defLayerId;
                            attrElement.ModelID = node.modelID;
                            attrElement.Key = node.key;
                            attrElement.Type = node.type;
                            attrElement.TargetModelID = node.modelID;

                            foreach (EcellData ed in node.M_value)
                            {
                                if (ed.M_name.Equals("Value"))
                                {
                                    attrElement.Value = ed.M_value.ToString();
                                    break;
                                }
                            }

                            pathElements.Add(attrElement);
                        }
                        else if (node.type.Equals(PathwayView.VARIABLE_STRING))
                        {
                            VariableElement varElement = new VariableElement();
                            varElement.CanvasID = m_defCanvasId;
                            varElement.LayerID = m_defLayerId;
                            varElement.ModelID = node.modelID;
                            varElement.Key = node.key;
                            varElement.Type = node.type;
                            varElement.CsId = m_view.ComponentSettingsManager.DefaultVariableSetting.Name;

                            pathElements.Add(varElement);
                        }
                        else if (node.type.Equals(PathwayView.PROCESS_STRING))
                        {
                            ProcessElement proElement = new ProcessElement();
                            
                            proElement.CanvasID = m_defCanvasId;
                            proElement.LayerID = m_defLayerId;
                            proElement.ModelID = node.modelID;
                            proElement.Key = node.key;
                            proElement.Type = node.type;
                            proElement.CsId = m_view.ComponentSettingsManager.DefaultProcessSetting.Name;

                            foreach (EcellData ed in node.M_value)
                            {
                                if (ed.M_name.Equals("VariableReferenceList"))
                                {
                                    proElement.SetEdgesByStr(ed.M_value.ToString());
                                }
                            }

                            pathElements.Add(proElement);
                        }
                    }
                }
            }

            try
            {
                List<PathwayElement> saveElements = m_view.Replace(pathElements, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                m_view.ShowingID = true;
            else
                m_view.ShowingID = false;
        }

        /// <summary>
        /// the event sequence of wheeling the mouse.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">MouseEventArgs.</param>
        void uc_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
                m_view.PanCanvas(PathwayView.Direction.Horizontal, e.Delta);
            else
                m_view.PanCanvas(PathwayView.Direction.Vertical, e.Delta);
        }
        #endregion
    }
}