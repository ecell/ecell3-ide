using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EcellLib.EntityListWindow;

namespace EcellLib.EntityListWindow
{
    public partial class EntityList : EcellDockContent
    {
        /// <summary>
        /// DataManager
        /// </summary>
        EntityListWindow m_owner;

        /// <summary>
        /// 
        /// </summary>
        private static ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResEntList));

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityList(EntityListWindow owner)
        {
            m_owner = owner;
            base.m_isSavable = true;
            InitializeComponent();
            this.Text = m_resources.GetString(MessageConstants.EntityList);
            this.TabText = this.Text;
            this.Icon = (Icon)m_resources.GetObject("$this.Icon");
            this.treeView1.ImageList =m_owner.Environment.PluginManager.NodeImageList;
        }
    }
}
