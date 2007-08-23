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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Reflection;



namespace EcellLib
{
    /// <summary>
    /// Availability of Redo/Undo
    /// </summary>
    public enum UndoStatus
    {
        /// <summary>
        /// Both undo and redo are available.
        /// </summary>
        UNDO_REDO,
        /// <summary>
        /// Only undo is available.
        /// </summary>
        UNDO_ONLY,
        /// <summary>
        /// Only redo is available.
        /// </summary>
        REDO_ONLY,
        /// <summary>
        /// Both undo and redo are NOT available.
        /// </summary>
        NOTHING
    }

    /// <summary>
    /// Manage class for the loaded plugin.
    /// </summary>
    public class PluginManager
    {
        #region Fields
        /// <summary>
        /// m_printBase (set plugin for print)
        /// </summary>
        private string m_printBase;
        /// <summary>
        /// m_pluginList (loaded plugin list)
        /// </summary>
        private Dictionary<string, PluginBase> m_pluginList;
        /// <summary>
        /// m_pluginDic (map between plugin and data)
        /// </summary>
        private Dictionary<PluginData, List<PluginBase>> m_pluginDic;
        /// <summary>
        /// m_printDoc (Print Document with .NET framework)
        /// </summary>
        private PrintDocument m_printDoc;
        /// <summary>
        /// m_dialog (Printer Dialog with .NET framerowk)
        /// </summary>
        private System.Windows.Forms.PrintDialog m_dialog;
        /// <summary>
        /// m_imageDict (image list against data type)
        /// </summary>
        private Dictionary<string, int> m_imageDict;
        /// <summary>
        /// s_instance (singleton instance)
        /// </summary>
        private static PluginManager s_instance = null;
        /// <summary>
        /// m_version (Application Version Information)
        /// </summary>
        private Version m_version;
        /// <summary>
        /// CopyRights String.
        /// </summary>
        private String m_copyright;
        #endregion

        /// <summary>
        /// constructer for PluginManager.
        /// </summary>
        public PluginManager()
        {
            this.m_printBase = null;
            this.m_printDoc = new PrintDocument();
            this.m_printDoc.PrintPage += 
                    new PrintPageEventHandler(this.printDoc_PrintPage);
            this.m_pluginList = new Dictionary<string, PluginBase>();
            this.m_pluginDic = new Dictionary<PluginData,List<PluginBase>>();
            this.m_dialog = new System.Windows.Forms.PrintDialog();
            this.m_imageDict = new Dictionary<string, int>();

            // default image type
            m_imageDict.Add("Project", 0);
            m_imageDict.Add("System", 0);
            m_imageDict.Add("Variable", 2);
            m_imageDict.Add("Process", 1);
            m_imageDict.Add("Model", 3);
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
        /// get/set CopyRights.
        /// </summary>
        public String CopyRight
        {
            get { return this.m_copyright; }
            set { this.m_copyright = value; }
        }

        /// <summary>
        /// set data that the plugin focus on.
        /// </summary>
        /// <param name="modelID">the model ID of data that plugin focus on.</param>
        /// <param name="key">the key ID of data that plugin focus on.</param>
        /// <param name="pbase">the plugin that focus on data.</param>
        public void FocusDataChanged(string modelID, string key, PluginBase pbase)
        {
            PluginData ent = new PluginData(modelID, key);
            if (m_pluginDic.ContainsKey(ent))
            {
                List<PluginBase> pList = m_pluginDic[ent];
                if (!pList.Contains(pbase))
                {
                    pList.Add(pbase);
                    m_pluginDic[ent] = pList;
                }
            }
        }

        /// <summary>
        ///  clear focus list that the plugin focus on.
        /// </summary>
        public void FocusClear()
        {
            this.m_pluginDic.Clear();
        }

        /// <summary>
        /// event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">selected the model ID</param>
        /// <param name="key">selected the key ID</param>
        /// <param name="type">selected the data type</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.SelectChanged(modelID, key, type);
            }
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            foreach (PluginBase p in m_pluginList.Values)
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
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.RemoveSelect(modelID, key, type);
            }
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.ResetSelect();
            }
        }

        /// <summary>
        /// event sequence on changing value on executing the simulation.
        /// </summary>
        /// <param name="modelID">the model ID of changing value.</param>
        /// <param name="key">the key of changing value.</param>
        /// <param name="type">the data type of changing value.</param>
        /// <param name="propName">the property name of changin value.</param>
        /// <param name="data">the simulation result.</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.LogData(modelID, key, type, propName, data);
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
            foreach (PluginBase p in m_pluginList.Values)
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
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.DataAdd(data);
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
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.DataDelete(modelID, key, type);
            }            
        }

        /// <summary>
        /// event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID generating warning data</param>
        /// <param name="key">the key ID generating warning data</param>
        /// <param name="type">the data type generating warning data</param>
        /// <param name="warntype">the type of warning data</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.WarnData(modelID, key, type, warntype);
            }
        }

        /// <summary>
        /// trans the load data to loading all plugin.
        /// </summary>
        /// <param name="modelID"></param>
        public void LoadData(string modelID)
        {
            DataAdd(DataManager.GetDataManager().GetData(modelID, null));
        }

        /// <summary>
        /// add the plugin to plugin list.
        /// </summary>
        /// <param name="p">plugin</param>
        public void AddPlugin(PluginBase p)
        {
            m_pluginList.Add(p.GetPluginName(), p);
        }

        /// <summary>
        /// display plugin list dialog to print plugin image.
        /// </summary>
        public void ShowSelectPlugin()
        {
            if (m_pluginList == null) return;

            // plugin base list show
            PrintPluginDialog d = new PrintPluginDialog();

            foreach (KeyValuePair<string, PluginBase> kvp in m_pluginList)
            {
                if (kvp.Value.IsEnablePrint())
                    d.listBox1.Items.Add(kvp.Key);
            }
            d.Show();
        }

        /// <summary>
        /// print the display image of plugin using PrintDoc.
        /// </summary>
        /// <param name="pluginName">the plugin to print</param>
        public void Print(string pluginName)
        {
            this.m_printBase = pluginName;

            m_dialog.Document = m_printDoc;
            System.Windows.Forms.DialogResult result = m_dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_printDoc.Print();
            }
        }

        /// <summary>
        /// event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.Clear();
            }
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="path">The property name of object changed value.</param>
        public void LoggerAdd(string modelID, string type, string key, string path)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.LoggerAdd(modelID, type, key, path);
            }
        }

        /// <summary>
        /// event sequence on advancing time.
        /// </summary>
        /// <param name="time">current simulation time</param>
        public void AdvancedTime(double time)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.AdvancedTime(time);
            }
        }

        /// <summary>
        /// display the message of executing simulation, debug, analysis and so on.
        /// </summary>
        /// <param name="type">display tab of this message</param>
        /// <param name="message">message of executing simulation, debug, analysis and so on</param>
        public void Message(string type, string message)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                if (p.IsMessageWindow())
                {
                    p.Message(type, message);
                }
            }

        }

        /// <summary>
        /// Save the selected model to directory.
        /// </summary>
        /// <param name="modelID">selected model.</param>
        /// <param name="path">output directory.</param>
        public void SaveModel(string modelID, string path)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.SaveModel(modelID, path);
            }
        }

        /// <summary>
        /// add new data type to image index.
        /// </summary>
        /// <param name="type">data type</param>
        /// <param name="imageIndex">image type</param>
        public void ImageAdd(string type, int imageIndex)
        {
            if (!m_imageDict.ContainsKey(type))
                m_imageDict.Add(type, imageIndex);
        }

        /// <summary>
        /// get image index from data type.
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns>image index</returns>
        public int GetImageIndex(string type)
        {
            if (m_imageDict.ContainsKey(type))
            {
                return m_imageDict[type];
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// load the plugin and control the plugin.
        /// </summary>
        /// <param name="path">path of plugin dll.</param>
        /// <param name="className">class name.</param>
        public PluginBase LoadPlugin(string path, string className)
        {
            Assembly m_theHandle = Assembly.LoadFile(path);
            Type aType = m_theHandle.GetType(className);
            Object anAllocator = aType.InvokeMember(
                null,
                BindingFlags.CreateInstance,
                null,
                null,
                null
            );

            PluginBase p = (PluginBase)anAllocator;
            if (!m_pluginList.ContainsKey(p.GetPluginName()))
            {
                m_pluginList.Add(p.GetPluginName(), p);
            }

            return p;
        }

        /// <summary>
        /// Unload the plugin and release the plugin.
        /// </summary>
        /// <param name="p">the unloading plugin data</param>
        public void UnloadPlugin(PluginBase p)
        {
            if (m_pluginList.ContainsKey(p.GetPluginName()))
            {
                m_pluginList.Remove(p.GetPluginName());
            }
        }

        /// <summary>
        /// ????
        /// </summary>
        /// <returns>????</returns>
        public string CurrentToolBarMenu()
        {
            // not implement
            return null;
        }

        /// <summary>
        /// when change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">system type</param>
        public void ChangeStatus(int type)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.ChangeStatus(type);
            }
        }

        /// <summary>
        /// Change availability of undo/redo function.
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                p.ChangeUndoStatus(status);
            }
        }

        /// <summary>
        /// Get plugin by using name of plugin.
        /// </summary>
        /// <param name="name">name of plugin</param>
        /// <returns>the plugin. if not find the plugin, return null.</returns>
        public PluginBase GetPlugin(string name)
        {
            foreach (PluginBase p in m_pluginList.Values)
            {
                if (p.GetPluginName().Equals(name))
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
            foreach (PluginBase p in m_pluginList.Values)
            {
                result.Add(p.GetPluginName(), p.GetVersionString());
            }
            return result;
        }

        /// <summary>
        /// Get PluginManager with using singleton pattern.
        /// </summary>
        /// <returns>The PluginManager in application</returns>
        public static PluginManager GetPluginManager()
        {
            if (s_instance == null)
            {
                s_instance = new PluginManager();
            }
            return s_instance;
        }

        /// <summary>
        /// Request event of printing plugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            PluginBase p = m_pluginList[m_printBase];
            if (p != null)
            {
                Bitmap bitmap = p.Print();
                if (bitmap == null)
                    return;
                e.Graphics.DrawImage(bitmap, new Point(0, 0));
                bitmap.Dispose();
            }
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
            foreach (PluginBase p in m_pluginList.Values)
                p.SetPosition(data);
        }
    }

    /// <summary>
    /// Data element displayed on plugins.
    /// </summary>
    public class PluginData
    {
        /// <summary>
        /// m_modelID (the model ID)
        /// </summary>
        private string m_modelID;
        /// <summary>
        /// m_key (the key ID)
        /// </summary>
        private string m_key;

        /// <summary>
        /// constructor of PluginData.
        /// </summary>
        public PluginData()
        {
            this.m_modelID = "model1";
            this.m_key = "key1";
        }

        /// <summary>
        /// constructir of PluginData with initial data.
        /// </summary>
        /// <param name="id">initial model ID</param>
        /// <param name="key">initial key ID</param>
        public PluginData(string id, string key)
        {
            this.m_modelID = id;
            this.m_key = key;
        }

        /// <summary>
        /// get/set m_modelID.
        /// </summary>
        public string M_modelID
        {
            get { return this.m_modelID; }
            set { this.m_modelID = value; }
        }

        /// <summary>
        /// get/set m_key.
        /// </summary>
        public string M_key
        {
            get { return this.m_key; }
            set { this.m_key = value; }
        }

        /// <summary>
        /// override equals method for PluginData.
        /// </summary>
        /// <param name="obj">the comparing object</param>
        /// <returns>if equal, return true</returns>
        public bool Equals(PluginData obj)
        {
            if (this.m_modelID == obj.m_modelID && this.m_key == obj.m_key)
            {
                return true;
            }
            return false;
        }
    }
}
