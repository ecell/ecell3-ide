using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ecell.Plugin;

namespace Ecell.IDE
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyNode : TreeNode, IPropertyItem
    {
        PropertyDialogPage m_page;
        /// <summary>
        /// get the PAge.
        /// </summary>
        internal PropertyDialogPage Page
        {
            get
            {
                if (m_page == null && Nodes.Count > 0)
                    return ((PropertyNode)Nodes[0]).Page;
                return m_page;
            }
        }

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">the node string.</param>
        public PropertyNode(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">the property page.</param>
        public PropertyNode(PropertyDialogPage page)
        {
            m_page = page;
            this.Text = page.Text;
        }
        #endregion

        #region IPropertyItem メンバ
        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            if (m_page != null)
                m_page.Initialize();
            foreach (TreeNode node in this.Nodes)
            {
                if (!(node is PropertyNode))
                    continue;
                ((PropertyNode)node).Initialize();
            }
        }

        /// <summary>
        /// Apply the property.
        /// </summary>
        public void ApplyChange()
        {
            if (m_page != null)
                m_page.ApplyChange();
            foreach (TreeNode node in this.Nodes)
            {
                if (!(node is PropertyNode))
                    continue;
                ((PropertyNode)node).ApplyChange();
            }

        }

        /// <summary>
        /// Closing the this page.
        /// </summary>
        public void PropertyDialogClosing()
        {
            if (m_page != null)
                m_page.PropertyDialogClosing();
            foreach (TreeNode node in this.Nodes)
            {
                if (!(node is PropertyNode))
                    continue;
                ((PropertyNode)node).PropertyDialogClosing();
            }

        }
        #endregion
    }
}
