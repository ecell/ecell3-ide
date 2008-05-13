using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EcellLib.EntityListWindow;

namespace EcellLib
{
    public partial class EntityList : EcellDockContent
    {
        /// <summary>
        /// 
        /// </summary>
        private static ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResEntList));

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityList()
        {
            base.m_isSavable = true;
            InitializeComponent();
            this.Text = m_resources.GetString(EntityListWindow.MessageConstants.EntityList);
            this.TabText = this.Text;
            this.treeView1.ImageList = PluginManager.GetPluginManager().NodeImageList;
        }
    }
}
