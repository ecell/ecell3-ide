//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Plugin;
using System.Windows.Forms;

namespace Ecell.IDE
{
    /// <summary>
    /// 
    /// </summary>
    public class LayoutPane : EcellDockContent, ILayoutPanel
    {
        #region Fields
        public System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.TabControl tabControl;
        /// <summary>
        /// 
        /// </summary>
        private ApplicationEnvironment _env = null;
        /// <summary>
        /// 
        /// </summary>
        private ILayoutAlgorithm m_algorithm = null; 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ILayoutAlgorithm Algorithm
        {
            get { return m_algorithm; }
        }

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public LayoutPane()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public LayoutPane(ApplicationEnvironment env)
        {
            InitializeComponent();
            _env = env;

            foreach (ILayoutPanel panel in _env.PluginManager.GetLayoutPanels())
            {
                SetPanel((LayoutPanel)panel);
            }
            if(tabControl.TabPages.Count > 0)
                SetAlgorithm(tabControl.TabPages[0]);
        }

        private void SetPanel(LayoutPanel panel)
        {
            TabPage page = new TabPage(panel.Text);
            page.Controls.Add(panel);
            tabControl.TabPages.Add(page);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutPane));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // ApplyButton
            // 
            resources.ApplyResources(this.ApplyButton, "ApplyButton");
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.UseVisualStyleBackColor = true;
            // 
            // LayoutPane
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.tabControl);
            this.Name = "LayoutPane";
            this.ResumeLayout(false);

        }

        #endregion

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            TabPage page = e.TabPage;
            SetAlgorithm(page);
        }

        private void SetAlgorithm(TabPage page)
        {
            ILayoutPanel panel = (ILayoutPanel)page.Controls[0];
            m_algorithm = panel.Algorithm;
        }


        #region ILayoutPanel メンバ

        /// <summary>
        /// 
        /// </summary>
        public void ApplyChange()
        {
            if(m_algorithm == null)
                return;
            m_algorithm.Panel.ApplyChange();
        }

        #endregion
    }
}
