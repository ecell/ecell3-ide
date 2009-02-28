using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using System.Windows.Forms;

using Ecell.Objects;
using Ecell.IDE;

namespace Ecell.IDE
{
    public partial class CommonContextMenu : Component
    {
        #region MyRegion
        
        #endregion
        /// <summary>
        /// Target object.
        /// </summary>
        protected EcellObject m_object;
        /// <summary>
        /// Environment manager.
        /// </summary>
        protected ApplicationEnvironment m_env;

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

                string parentSys = "";
                if (value != null && value.ParentSystemID != null)
                    parentSys = value.ParentSystemID;
                bool isSystem = (value is EcellSystem);
                bool isParentSys = string.IsNullOrEmpty(parentSys);

                addToolStripMenuItem.Visible = isSystem;
                deleteToolStripMenuItem.Visible = !isParentSys;
                mergeSystemToolStripMenuItem.Visible = isSystem && !isParentSys;
                mergeSystemToolStripMenuItem.Text = MessageResources.MergeSystem + "(" + parentSys + ")";

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
        /// Constructor with parameter, EcellObject and ApplicationEnvironment.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="env"></param>
        public CommonContextMenu(EcellObject obj, ApplicationEnvironment env)
        {
            InitializeComponent();
            m_env = env;
            Object = obj;
        }

        /// <summary>
        /// Set Logger Menus;
        /// </summary>
        private void SetLoggerMenus()
        {
            loggingToolStripMenuItem.DropDownItems.Clear();
            observedToolStripMenuItem.DropDownItems.Clear();
            parameterToolStripMenuItem.DropDownItems.Clear();

            if (m_object == null)
                return;

            loggingToolStripMenuItem.DropDownItems.AddRange(CreateLoggerPopupMenu(m_object));
            observedToolStripMenuItem.DropDownItems.AddRange(CreateObservedPopupMenu(m_object));
            parameterToolStripMenuItem.DropDownItems.AddRange(CreateParameterPopupMenu(m_object));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public CommonContextMenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Create the menu item of popup menu to set and reset the logger.
        /// </summary>
        /// <param name="obj">object to display the popup menu.</param>
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
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private ToolStripItem[] CreateParameterPopupMenu(EcellObject obj)
        {
            List<ToolStripItem> retval = new List<ToolStripItem>();
            foreach (EcellData d in obj.Value)
            {
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
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        #endregion

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


            if (m.Checked)
            {
                m_object.GetEcellData(prop).Logged = false;
            }
            else
            {
                EcellData d = m_object.GetEcellData(prop);
                Debug.Assert(d != null);
                m_env.LoggerManager.AddLoggerEntry(
                    m_object.ModelID, m_object.Key, m_object.Type, d.EntityPath);
                d.Logged = true;
            }
            // modify changes
            m_env.DataManager.DataChanged(m_object.ModelID,
                m_object.Key, m_object.Type, m_object);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickAddToolStripMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string type = item.Tag as string;
            EcellObject eo = m_env.DataManager.CreateDefaultObject(m_object.ModelID, m_object.Key, type);
            m_env.DataManager.DataAdd(eo);
            m_env.PluginManager.SelectChanged(eo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickPropertyToolStripMenuItem(object sender, EventArgs e)
        {
            PropertyEditor.Show(m_env, m_object);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickDeleteToolStripMenuItem(object sender, EventArgs e)
        {
            m_env.DataManager.DataDelete(m_object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickMergeSystemToolStripMenuItem(object sender, EventArgs e)
        {
            m_env.DataManager.DataMerge(m_object.ModelID, m_object.Key);
        }
        #endregion

    }
}
