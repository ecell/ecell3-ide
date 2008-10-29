using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using System.Windows.Forms;

using Ecell.Objects;

namespace Ecell.IDE
{
    public partial class CommonContextMenu : Component
    {
        private EcellObject m_object;
        private ApplicationEnvironment m_env;

        public CommonContextMenu()
        {
            InitializeComponent();
        }

        public CommonContextMenu(EcellObject obj, ApplicationEnvironment env)
        {
            InitializeComponent();
            m_object = obj;
            m_env = env;
            if (obj.Type == Constants.xpathSystem)
            {
                addToolStripMenuItem.Enabled = true;
                addSystemToolStripMenuItem.Click += new EventHandler(ClickAddToolStripMenuItem);
                addVariableToolStripMenuItem.Click += new EventHandler(ClickAddToolStripMenuItem);
                addProcessToolStripMenuItem.Click += new EventHandler(ClickAddToolStripMenuItem);

                string superSys = Util.GetSuperSystemPath(m_object.Key);
                if (string.IsNullOrEmpty(superSys))
                {
                    mergeSystemToolStripMenuItem.Visible = false;
                }
                else
                {
                    mergeSystemToolStripMenuItem.Visible = true;
                    mergeSystemToolStripMenuItem.Text = mergeSystemToolStripMenuItem.Text + "(" + superSys + ")";
                }
            }
            else
            {
                addToolStripMenuItem.Visible = false;
                mergeSystemToolStripMenuItem.Visible = false;
            }
            loggingToolStripMenuItem.DropDownItems.Clear();
            loggingToolStripMenuItem.DropDownItems.AddRange(CreateLoggerPopupMenu(m_object));
            observedToolStripMenuItem.DropDownItems.Clear();
            observedToolStripMenuItem.DropDownItems.AddRange(CreateObservedPopupMenu(m_object));
            parameterToolStripMenuItem.DropDownItems.Clear();
            parameterToolStripMenuItem.DropDownItems.AddRange(CreateParameterPopupMenu(m_object));
            deleteToolStripMenuItem.Click += new EventHandler(ClickDeleteToolStripMenuItem);
            mergeSystemToolStripMenuItem.Click += new EventHandler(ClickMergeSystemToolStripMenuItem);
            propertyToolStripMenuItem.Click += new EventHandler(ClickPropertyToolStripMenuItem);
        }

        public CommonContextMenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public ContextMenuStrip Menu
        {
            get { return commonContextMenuStrip; }
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

        private ToolStripItem[] CreateParameterPopupMenu(EcellObject obj)
        {
            List<ToolStripItem> retval = new List<ToolStripItem>();
            foreach (EcellData d in obj.Value)
            {
                if (!d.Settable || !d.Value.IsDouble)
                    continue;
                ToolStripMenuItem item = new ToolStripMenuItem(d.Name);
                item.Tag = d.Name;
                item.Checked = m_env.DataManager.IsContainsParameterData(d.EntityPath);
                item.Click += new EventHandler(ClickCreateParameterData);
                retval.Add(item);
            }
            return retval.ToArray();
        }

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
                m_env.PluginManager.LoggerAdd(
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

            if (m.Checked)
            {
                m_env.DataManager.RemoveParameterData(
                    new EcellParameterData(m_object.GetEcellData(key).EntityPath, 0.0));
            }
            else
            {
                EcellData d = m_object.GetEcellData(key);
                m_env.DataManager.SetParameterData(
                    new EcellParameterData(
                        d.EntityPath,
                        Convert.ToDouble(d.Value.ToString())));
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

            if (m.Checked)
            {
                m_env.DataManager.RemoveObservedData(
                    new EcellObservedData(m_object.GetEcellData(key).EntityPath, 0.0));
            }
            else
            {
                EcellData d = m_object.GetEcellData(key);
                Debug.Assert(d != null);
                m_env.DataManager.SetObservedData(
                    new EcellObservedData(d.EntityPath, Convert.ToDouble(d.Value.ToString())));
            }
        }

        void ClickAddToolStripMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string type = item.Tag as string;

            m_env.DataManager.CreateDefaultObject(m_object.ModelID, m_object.Key, type, true);
        }

        void ClickPropertyToolStripMenuItem(object sender, EventArgs e)
        {
            PropertyEditor.Show(m_env.DataManager, m_env.PluginManager, m_object);
        }

        void ClickDeleteToolStripMenuItem(object sender, EventArgs e)
        {
            m_env.DataManager.DataDelete(m_object.ModelID, m_object.Key, m_object.Type);
        }

        void ClickMergeSystemToolStripMenuItem(object sender, EventArgs e)
        {
            m_env.DataManager.SystemDeleteAndMove(m_object.ModelID, m_object.Key);
        }
    }
}
