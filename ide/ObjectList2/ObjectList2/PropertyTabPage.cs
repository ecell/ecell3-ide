//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using EcellLib.Objects;

namespace EcellLib.ObjectList2
{
    /// <summary>
    /// TabPage to display the property of model.
    /// </summary>
    class PropertyTabPage : IObjectListTabPage
    {
        private Dictionary<Type, VPropertyTabPage> m_tabList = 
            new Dictionary<Type,VPropertyTabPage>();

        /// <summary>
        /// TabPage to dock DataGridView.
        /// </summary>
        private TabPage m_tabPage;
        private TabControl m_tabControl = null;
        /// <summary>
        /// Popup menu.
        /// </summary>
        private ContextMenuStrip m_contextMenu;

        /// <summary>
        /// Timer for executing redraw event at each 0.5 minutes.
        /// </summary>
        private System.Windows.Forms.Timer m_time;

        /// <summary>
        /// The status of selected Model now.
        /// </summary>
        private ProjectStatus m_type = ProjectStatus.Uninitialized;

        /// <summary>
        /// The ID of the selected Model now.
        /// </summary>
        private string m_currentModelID = null;




        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyTabPage()
        {
            m_tabPage = new TabPage();
            m_tabPage.Text = GetTabPageName();

            m_tabControl = new TabControl();
            m_tabControl.Dock = DockStyle.Fill;
            m_tabPage.Controls.Add(m_tabControl);

            VPropertyTabPage sysPage = new SystemPropertyTabPage();
            sysPage.Type = typeof(EcellSystem);
            m_tabList.Add(typeof(EcellSystem), sysPage);
            m_tabControl.Controls.Add(sysPage.GetTabPage());

            VPropertyTabPage varPage = new VariablePropertyTabPage();
            varPage.Type = typeof(EcellVariable);
            m_tabList.Add(typeof(EcellVariable), varPage);
            m_tabControl.Controls.Add(varPage.GetTabPage());

            VPropertyTabPage proPage = new ProcessPropertyTabPage();
            proPage.Type = typeof(EcellProcess);
            m_tabList.Add(typeof(EcellProcess), proPage);
            m_tabControl.Controls.Add(proPage.GetTabPage());


            m_contextMenu = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = ObjectList2.s_resources.GetString("SearchMenuText");
            it.ShortcutKeys = Keys.Control | Keys.F;
            it.Click += new EventHandler(ClickSearchMenu);

            m_contextMenu.Items.AddRange(new ToolStripItem[] { it });

            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(FireTimer);
        }





        /// <summary>
        /// The property of object is reset when simulation is stopped.
        /// </summary>
        private void ResetPropForSimulation()
        {
            DataManager manager = DataManager.GetDataManager();
            List<EcellObject> list = manager.GetData(m_currentModelID, null);
            Clear();
            DataAdd(list);
        }

        /// <summary>
        /// Add the object to DataGridView.
        /// </summary>
        /// <param name="obj">The added object.</param>
        private void DataAdd(EcellObject obj)
        {
            if (obj is EcellSystem)
            {
                m_tabList[typeof(EcellSystem)].DataAdd(obj);
            }
            else if (obj is EcellVariable)
            {
                m_tabList[typeof(EcellVariable)].DataAdd(obj);
            }
            else if (obj is EcellProcess)
            {
                m_tabList[typeof(EcellProcess)].DataAdd(obj);
            }
            m_currentModelID = obj.ModelID;
        }


        #region IObjectListTabPage
        /// <summary>
        /// Event when system search the object by text.
        /// </summary>
        /// <param name="text">search condition.</param>
        public void SearchInstance(string text)
        {
            // not implement.
        }

        /// <summary>
        /// Event when the project is closed.
        /// </summary>
        public void Clear()
        {
            foreach (Type t in m_tabList.Keys)
            {
                m_tabList[t].Clear();
            }
            m_currentModelID = null;
        }

        /// <summary>
        /// Event when the selected object is added.
        /// </summary>
        /// <param name="modelID">ModelID of the selected object.</param>
        /// <param name="key">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        public void AddSelection(string modelID, string key, string type)
        {
            foreach (Type t in m_tabList.Keys)
            {
                m_tabList[t].AddSelection(modelID, key, type);
            }
        }

        /// <summary>
        /// Event when the selected object is removed.
        /// </summary>
        /// <param name="modelID">ModelID of the removed object.</param>
        /// <param name="key">ID of the removed object.</param>
        /// <param name="type">Type of the removed object.</param>
        public void RemoveSelection(string modelID, string key, string type)
        {
            foreach (Type t in m_tabList.Keys)
            {
                m_tabList[t].RemoveSelection(modelID, key, type);
            }
        }

        /// <summary>
        /// Event when the selected object is changed to no select.
        /// </summary>
        public void ClearSelection()
        {
            foreach (Type t in m_tabList.Keys)
            {
                m_tabList[t].ClearSelection();
            }
        }

        /// <summary>
        /// Event when object is selected.
        /// </summary>
        /// <param name="modelID">ModelID of the selected object.</param>
        /// <param name="id">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        public void SelectChanged(string modelID, string id, string type)
        {
            ClearSelection();
            if (type == EcellObject.SYSTEM)
            {
                m_tabControl.SelectedTab = m_tabList[typeof(EcellSystem)].GetTabPage();
                m_tabList[typeof(EcellSystem)].SelectChanged(modelID, id, type);
            }
            else if (type == EcellObject.VARIABLE)
            {
                m_tabControl.SelectedTab = m_tabList[typeof(EcellVariable)].GetTabPage();
                m_tabList[typeof(EcellVariable)].SelectChanged(modelID, id, type);
            }
            else if (type == EcellObject.PROCESS)
            {
                m_tabControl.SelectedTab = m_tabList[typeof(EcellProcess)].GetTabPage();
                m_tabList[typeof(EcellProcess)].SelectChanged(modelID, id, type);
            }
            else
            {
                return;
            }
            m_currentModelID = modelID;


        }

        /// <summary>
        /// Event when object is added.
        /// </summary>
        /// <param name="objList">the list of added object.</param>
        public void DataAdd(List<EcellObject> objList)
        {
            foreach (EcellObject obj in objList)
            {
                if (obj.Type.Equals(Constants.xpathModel)) continue;
                if (obj.Type.Equals(Constants.xpathProject)) continue;
                if (m_currentModelID != null && !m_currentModelID.Equals(obj.ModelID)) continue;
                DataAdd(obj);
                if (obj.Children == null) continue;
                foreach (EcellObject cobj in obj.Children)
                {
                    DataAdd(cobj);
                }
            }
        }

        /// <summary>
        /// Event when the property of object is changed.
        /// </summary>
        /// <param name="modelID">ModelID of the changed object.</param>
        /// <param name="id">ID of the changed object.</param>
        /// <param name="type">Type of the changed object.</param>
        /// <param name="obj">The changed object.</param>
        public void DataChanged(string modelID, string id, string type, EcellObject obj)
        {
            if (obj is EcellSystem)
            {
                m_tabList[typeof(EcellSystem)].DataChanged(modelID, id, obj);
            }
            else if (obj is EcellVariable)
            {
                m_tabList[typeof(EcellVariable)].DataChanged(modelID, id, obj);
            }
            else if (obj is EcellProcess)
            {
                m_tabList[typeof(EcellProcess)].DataChanged(modelID, id, obj);
            }
        }

        /// <summary>
        /// Event when object is deleted.
        /// </summary>
        /// <param name="modelID">ModelID of the deleted object.</param>
        /// <param name="id">ID of the deleted object.</param>
        /// <param name="type">Type of the deleted object.</param>
        /// <param name="isChanged"></param>
        public void DataDelete(string modelID, string id, string type, bool isChanged)
        {
            foreach (Type t in m_tabList.Keys)
            {
                if (type == EcellObject.SYSTEM)
                    m_tabList[t].DataDelete(modelID, id, isChanged, typeof(EcellSystem));
                else if (type == EcellObject.PROCESS)
                    m_tabList[t].DataDelete(modelID, id, isChanged, typeof(EcellProcess));
                else if (type == EcellObject.VARIABLE)
                    m_tabList[t].DataDelete(modelID, id, isChanged, typeof(EcellVariable));
            }
        }

        /// <summary>
        /// Event when the status of system is changed.
        /// </summary>
        /// <param name="status">the changed status.</param>
        public void ChangeStatus(ProjectStatus status)
        {
            //if (status == ProjectStatus.Running ||
            //    status == ProjectStatus.Suspended ||
            //    status == ProjectStatus.Uninitialized)
            //{
            //    m_gridView.ContextMenu = null;
            //}
            //else
            //{
            //    m_gridView.ContextMenuStrip = m_contextMenu;
            //}

            if (status == ProjectStatus.Running)
            {
                m_time.Enabled = true;
                m_time.Start();
            }
            else if (status == ProjectStatus.Suspended)
            {
                m_time.Enabled = false;
                m_time.Stop();
                UpdatePropForSimulation();
            }
            else if ((m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping) &&
                    status == ProjectStatus.Loaded)
            {
                m_time.Enabled = false;
                m_time.Stop();
                ResetPropForSimulation();
            }
            else if (status == ProjectStatus.Stepping)
            {
                UpdatePropForSimulation();
            }
            m_type = status;
        }

        private void UpdatePropForSimulation()
        {
            foreach (Type t in m_tabList.Keys)
            {
                m_tabList[t].UpdatePropForSimulation();
            }
        }

        /// <summary>
        /// Get tab name.
        /// </summary>
        /// <returns>"Model"</returns>
        public string GetTabPageName()
        {
            return "Property";
        }

        /// <summary>
        /// Get TabPage.
        /// </summary>
        /// <returns>TabPage.</returns>
        public TabPage GetTabPage()
        {
            return m_tabPage;
        }
        #endregion

        #region Events
        /// <summary>
        /// Event when search button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void ClickSearchMenu(object sender, EventArgs e)
        {
            SearchInstance win = new SearchInstance();
            win.SetPlugin(this);
            win.ShowDialog();
        }



        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void FireTimer(object sender, EventArgs e)
        {
            m_time.Enabled = false;
            UpdatePropForSimulation();
            m_time.Enabled = true;
        }
        #endregion
    }
}
