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
// written by Motokazu Ishikawa<m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.ComponentModel;

using EcellLib;
using EcellLib.Plugin;
using EcellLib.Objects;

namespace EcellLib.ObjectList2
{
    /// <summary>
    /// Plugin class to display object by list.
    /// </summary>
    public class ObjectList2 : PluginBase
    {
        #region Fields
        /// <summary>
        /// modelID of a model, which is currently displayed on the ObjectList.
        /// </summary>
        private string m_currentModelID;
        /// <summary>
        ///  tab control of ObjectList.
        /// </summary>
        private TabControl m_tabControl;
        /// <summary>
        /// ComponentResourceManager for ObjectList.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResObjList));
        /// <summary>
        /// Dictionary of name and TabPage.
        /// </summary>
        private Dictionary<string, IObjectListTabPage> m_TabDict;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for ObjectList.
        /// </summary>
        public ObjectList2()
        {
            m_dManager = DataManager.GetDataManager();
            m_TabDict = new Dictionary<string, IObjectListTabPage>();
            m_tabControl = new TabControl();
            m_tabControl.Dock = DockStyle.Fill;

            PropertyTabPage tab = new PropertyTabPage();
            m_tabControl.Controls.Add(tab.GetTabPage());
            m_TabDict.Add(tab.GetTabPageName(), tab);
        }

        /// <summary>
        /// Deconstructor for ObjectList.
        /// </summary>
        ~ObjectList2()
        {
            if (m_tabControl != null)
            {
                foreach (TabPage page in m_tabControl.Controls)
                    page.Dispose();
                m_tabControl.Dispose();
            }
        }
        #endregion

        #region Inherited from PuginBase
        /// <summary>
        /// Get the window form for ObjectList.
        /// </summary>
        /// <returns>UserControl</returns>        
        public override List<EcellDockContent> GetWindowsForms()
        {
            EcellDockContent win = new EcellDockContent();
            m_tabControl.Dock = DockStyle.Fill;
            win.Controls.Add(m_tabControl);
            win.Text = "ObjectList2";
            win.IsSavable = true;
            List<EcellDockContent> list = new List<EcellDockContent>();
            list.Add(win);

            return list;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            if (m_currentModelID == null || !m_currentModelID.Equals(modelID))
            {
                this.Clear();
                List<EcellObject> list = m_dManager.GetData(modelID, null);
                foreach (string id in m_TabDict.Keys)
                {
                    m_TabDict[id].DataAdd(list);
                }
                m_currentModelID = modelID;
            }
            foreach (string id in m_TabDict.Keys)
            {
                m_TabDict[id].SelectChanged(modelID, key, type);
            }
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public override void AddSelect(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            foreach (string id in m_TabDict.Keys)
            {
                m_TabDict[id].AddSelection(modelID, key, type);
            }
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public override void RemoveSelect(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            foreach (string id in m_TabDict.Keys)
            {
                m_TabDict[id].AddSelection(modelID, key, type);
            }
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public override void ResetSelect()
        {
            foreach (string key in m_TabDict.Keys)
            {
                m_TabDict[key].ClearSelection();
            }
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            if (data == null) return;
            foreach (string key in m_TabDict.Keys)
            {
                m_TabDict[key].DataAdd(data);
            }
            foreach (EcellObject obj in data)
            {
                if (obj.ModelID.Equals("")) continue;
                m_currentModelID = obj.ModelID;
                break;
            }
            return;
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>        
        public override void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            if (modelID == null || key == null || m_currentModelID == null ||
               !m_currentModelID.Equals(modelID))
                return;

            foreach (string id in m_TabDict.Keys)
            {
                m_TabDict[id].DataChanged(modelID, key, type, data);
            }

            return;
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            if (this.m_currentModelID == null ||
                this.m_currentModelID != modelID) return;
            if (type.Equals(Constants.xpathStepper)) return;

            foreach (string id in m_TabDict.Keys)
            {
                m_TabDict[id].DataDelete(modelID, key, type, true);
            }
            return;
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            if (m_tabControl == null)
                return;


            foreach (string key in m_TabDict.Keys)
            {
                m_TabDict[key].Clear();
            }
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            foreach (string key in m_TabDict.Keys)
            {
                m_TabDict[key].ChangeStatus(type);
            }
        }

        /// <summary>
        /// get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>bitmap data</returns>
        public override Bitmap Print(string names)
        {
            TabPage tab = m_tabControl.SelectedTab;
            if (tab == null) return null;

            Bitmap b = new Bitmap(tab.Width, tab.Height);
            tab.DrawToBitmap(b, tab.ClientRectangle);

            return b;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"ObjectList"</returns>
        public override string GetPluginName()
        {
            return "ObjectList2";
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
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public override List<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            names.Add("List of entity2.");
            return names;
        }
        #endregion
    }
}