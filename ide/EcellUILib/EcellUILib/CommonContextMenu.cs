//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using System.Windows.Forms;

using Ecell.Objects;
using Ecell.Plugin;
using Ecell.IDE;

namespace Ecell.IDE
{
    /// <summary>
    /// Common context menu
    /// </summary>
    public partial class CommonContextMenu : Component
    {
        #region Fields
        /// <summary>
        /// Target object.
        /// </summary>
        protected EcellObject m_object;

        /// <summary>
        /// Environment manager.
        /// </summary>
        protected ApplicationEnvironment m_env;
        
        #endregion

        #region Accessors
        /// <summary>
        /// ContextMenuStrip
        /// </summary>
        public ContextMenuStrip Menu
        {
            get { return commonContextMenuStrip; }
        }

        /// <summary>
        /// Get/Set ApplicationEnvironment
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return this.m_env; }
            set { this.m_env = value; }
        }

        /// <summary>
        /// Get/Set target object
        /// </summary>
        public EcellObject Object
        {
            get { return this.m_object; }
            set
            {
                this.m_object = value;

                // Set Parent System
                string parentSys = "";
                if (value != null && value.ParentSystemID != null)
                    parentSys = value.ParentSystemID;

                // 
                bool isSystem = (value is EcellSystem);
                bool isParentSys = string.IsNullOrEmpty(parentSys);
                bool isLoaded = m_env.PluginManager.Status == ProjectStatus.Loaded;

                // Set Menu Visibility.
                addToolStripMenuItem.Visible = isSystem && isLoaded;
                deleteToolStripMenuItem.Visible = !isParentSys && isLoaded;
                mergeSystemToolStripMenuItem.Visible = isSystem && !isParentSys && isLoaded;
                mergeSystemToolStripMenuItem.Text = MessageResources.MergeSystem + "(" + parentSys + ")";
                toolStripSeparator1.Visible = isLoaded;

                if (m_env == null)
                    return;
//                propertyToolStripMenuItem.Enabled = (m_env.DataManager.CurrentProject.SimulationStatus == SimulationStatus.Wait);
                SetLoggerMenus();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public CommonContextMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with container object.
        /// </summary>
        /// <param name="container">the container object.</param>
        public CommonContextMenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Constructor with parameter, EcellObject and ApplicationEnvironment.
        /// </summary>
        /// <param name="obj">the selected object.</param>
        /// <param name="env">ApplicationEnvironment.</param>
        public CommonContextMenu(EcellObject obj, ApplicationEnvironment env)
        {
            InitializeComponent();
            m_env = env;
            Object = obj;
        }
        #endregion


        /// <summary>
        /// Set Logger Menus;
        /// </summary>
        private void SetLoggerMenus()
        {
            loggingToolStripMenuItem.DropDownItems.Clear();
            observedToolStripMenuItem.DropDownItems.Clear();
            parameterToolStripMenuItem.DropDownItems.Clear();

            if (m_object == null || m_object.Type.Equals(Constants.xpathStepper))
                return;

            loggingToolStripMenuItem.DropDownItems.AddRange(CreateLoggerPopupMenu(m_object));
            observedToolStripMenuItem.DropDownItems.AddRange(CreateObservedPopupMenu(m_object));
            parameterToolStripMenuItem.DropDownItems.AddRange(CreateParameterPopupMenu(m_object));
        }

        /// <summary>
        /// Create popup menu for the logger entry.
        /// </summary>
        /// <param name="obj">object to display the popup menu.</param>
        /// <returns>the list of menu.</returns>
        private ToolStripItem[] CreateLoggerPopupMenu(EcellObject obj)
        {
            List<ToolStripItem> retval = new List<ToolStripItem>();
            foreach (EcellData d in obj.Value)
            {                
                if (d.Logable)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(d.Name);
                    item.Tag = d.Name;
                    item.Click += new EventHandler(ClickCreateLogger);
                    item.Checked = d.Logged;
                    retval.Add(item);
                }
            }
            return retval.ToArray();
        }

        /// <summary>
        /// Create popup menu for the parameter data.
        /// </summary>
        /// <param name="obj">object to display the popup menu.</param>
        /// <returns>the list of menu.</returns>
        private ToolStripItem[] CreateParameterPopupMenu(EcellObject obj)
        {
            List<ToolStripItem> retval = new List<ToolStripItem>();
            foreach (EcellData d in obj.Value)
            {
                if (d.Value == null)
                    continue;
                if (!d.Settable || !d.Value.IsDouble || d.Name.Equals(Constants.xpathSize))
                    continue;
                ToolStripMenuItem item = new ToolStripMenuItem(d.Name);
                item.Tag = d.Name;
                item.Checked = m_env.DataManager.IsContainsParameterData(d.EntityPath);
                item.Click += new EventHandler(ClickCreateParameterData);
                retval.Add(item);
            }
            if (obj.Type.Equals(EcellObject.SYSTEM))
            {
                ToolStripMenuItem item = new ToolStripMenuItem(Constants.xpathSize);
                item.Tag = Constants.xpathSize;
                item.Checked = m_env.DataManager.IsContainsParameterData(Constants.xpathVariable + ":" + obj.Key + ":SIZE:Value");
                item.Click += new EventHandler(ClickCreateParameterData);
                retval.Add(item);
            }
            return retval.ToArray();
        }

        /// <summary>
        /// Create popup menu for the observed data.
        /// </summary>
        /// <param name="obj">object to display the popup menu.</param>
        /// <returns>the list of menu.</returns>
        private ToolStripItem[] CreateObservedPopupMenu(EcellObject obj)
        {
            List<ToolStripItem> retval = new List<ToolStripItem>();

            foreach (EcellData d in obj.Value)
            {
                if (!d.Logable) continue;
                ToolStripMenuItem item = new ToolStripMenuItem(d.Name);
                item.Tag = d.Name;
                item.Checked = m_env.DataManager.IsContainsObservedData(d.EntityPath);
                item.Click += new EventHandler(ClickCreateObservedData);
                retval.Add(item);
            }
            return retval.ToArray();
        }

        #region Eventhandler
        /// <summary>
        /// The action of selecting [Create Logger] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void ClickCreateLogger(object sender, EventArgs e)
        {
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            string prop = m.Tag as string;

            string model = m_object.ModelID;
            string key = m_object.Key;
            string type = m_object.Type;
            EcellObject obj = m_env.DataManager.GetEcellObject(model, key, type);

            if (m.Checked)
            {
                obj.GetEcellData(prop).Logged = false;
            }
            else
            {
                EcellData d = obj.GetEcellData(prop);
                Debug.Assert(d != null);
//                m_env.LoggerManager.AddLoggerEntry(model, key, type, d.EntityPath);
                d.Logged = true;
            }
            // modify changes
            m_env.DataManager.DataChanged(model, key, type, obj);
        }

        /// <summary>
        /// The action of selecting [Create Parameter] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ClickCreateParameterData(object sender, EventArgs e)
        {
            ToolStripMenuItem m = sender as ToolStripMenuItem;
            string key = m.Tag as string;
            EcellData d = null;
            string fullID = null;

            if (key.Equals(Constants.xpathSize))
            {
                fullID = Constants.xpathVariable + ":" + m_object.Key + ":SIZE:Value";
                d = new EcellData(Constants.xpathValue, new EcellValue(((EcellSystem)m_object).SizeInVolume),
                    "");
            }
            else
            {
                d = m_object.GetEcellData(key);
                fullID = d.EntityPath;
            }

            if (m.Checked)
            {
                m_env.DataManager.RemoveParameterData(
                    new EcellParameterData(fullID, 0.0));
            }
            else
            {
                m_env.DataManager.SetParameterData(
                    new EcellParameterData(fullID, Convert.ToDouble(d.Value.ToString())));
            }
        }

        /// <summary>
        /// The action of selecting [Create Observed] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ClickCreateObservedData(object sender, EventArgs e)
        {
            ToolStripMenuItem m = sender as ToolStripMenuItem;
            string key = m.Tag as string;
            EcellData d = null;;
            string fullID = null;

                d = m_object.GetEcellData(key);
                fullID = d.EntityPath;

            if (m.Checked)
            {
                m_env.DataManager.RemoveObservedData(
                    new EcellObservedData(fullID, 0.0));
            }
            else
            {
                m_env.DataManager.SetObservedData(
                    new EcellObservedData(fullID, Convert.ToDouble(d.Value.ToString())));
            }
        }

        /// <summary>
        /// The action of selecting [Add ...] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ClickAddToolStripMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string type = item.Tag as string;
            EcellObject eo = m_env.DataManager.CreateDefaultObject(m_object.ModelID, m_object.Key, type);
            m_env.DataManager.DataAdd(eo);
            m_env.PluginManager.SelectChanged(eo);
        }

        /// <summary>
        /// The action of selecting [Property] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ClickPropertyToolStripMenuItem(object sender, EventArgs e)
        {
            ShowDialogDelegate dlg = m_env.PluginManager.GetDelegate(Constants.delegateShowPropertyWindow) as ShowDialogDelegate;
            if (dlg != null)
                dlg();
        }

        /// <summary>
        /// The action of selecting [Delete] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ClickDeleteToolStripMenuItem(object sender, EventArgs e)
        {
            try
            {
                m_env.DataManager.DataDelete(m_object);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// The action of selecting [Merge to upper system] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ClickMergeSystemToolStripMenuItem(object sender, EventArgs e)
        {
            try
            {
                if (Util.ShowYesNoDialog(MessageResources.ConfirmMerge))
                {
                    m_env.DataManager.DataMerge(m_object.ModelID, m_object.Key);
                }
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }
        #endregion

    }
}
