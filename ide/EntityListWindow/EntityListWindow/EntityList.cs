using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ecell.IDE.Plugins.EntityListWindow;

namespace Ecell.IDE.Plugins.EntityListWindow
{
    public partial class EntityList : EcellDockContent
    {
        /// <summary>
        /// DataManager
        /// </summary>
        EntityListWindow m_owner;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityList(EntityListWindow owner)
        {
            m_owner = owner;
            base.m_isSavable = true;
            InitializeComponent();
            this.Text = MessageResEntList.EntityList;
            this.TabText = this.Text;
            this.treeView1.ImageList = m_owner.Environment.PluginManager.NodeImageList;
        }
    }
}
