//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
    public class LayoutPane : EcellDockContent
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
        public ILayoutAlgorithm CurrentAlgorithm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Location = new System.Drawing.Point(0, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(345, 238);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabIndexChanged += new System.EventHandler(this.tabControl_TabIndexChanged);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyButton.Location = new System.Drawing.Point(258, 247);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 1;
            this.ApplyButton.Text = "Execute";
            this.ApplyButton.UseVisualStyleBackColor = true;
            // 
            // LayoutPane
            // 
            this.ClientSize = new System.Drawing.Size(345, 282);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "LayoutPane";
            this.Text = "Layout Settings.";
            this.ResumeLayout(false);

        }

        #endregion

        private void tabControl_TabIndexChanged(object sender, EventArgs e)
        {
            TabPage page = tabControl.SelectedTab;
            SetAlgorithm(page);
        }

        private void SetAlgorithm(TabPage page)
        {
            ILayoutPanel panel = (ILayoutPanel)page.Controls[0];
            m_algorithm = panel.Algorithm;
        }

    }
}
