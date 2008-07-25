using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ecell.IDE.Plugins.ProjectExplorer;

namespace Ecell.IDE.Plugins.ProjectExplorer
{
    public partial class ProjectExplorerControl : EcellDockContent
    {
        /// <summary>
        /// DataManager
        /// </summary>
        ProjectExplorer m_owner;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProjectExplorerControl(ProjectExplorer owner)
        {
            m_owner = owner;
            base.m_isSavable = true;
            InitializeComponent();
            this.Text = MessageResources.ProjectExplorer;
            this.TabText = this.Text;
            this.treeView1.ImageList = m_owner.Environment.PluginManager.NodeImageList;
        }
    }
}
