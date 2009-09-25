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
// written by by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ecell.Exceptions;
using Ecell.Plugin;
using Ecell.IDE.Plugins.PathwayWindow.Components;

namespace Ecell.IDE
{
    /// <summary>
    /// Tabbed PropertyDialog for Ecell-IDE.
    /// </summary>
    public partial class ComponentDialog : Form
    {
        #region Fields
        /// <summary>
        /// ComponentManager
        /// </summary>
        private ComponentManager m_csManager = null;
        /// <summary>
        /// Is pathway or not.
        /// </summary>
        private bool m_isPathway = false;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isExistSetting = false;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ComponentSetting Setting
        {
            get { return componentItem.PCanvas.Setting; }
            set
            {
                componentItem.SetItem(value);
                SetContextMenu();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPathway
        {
            get { return m_isPathway; }
            set
            {
                m_isPathway = value;
                registerCheckBox.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DoesRegister
        {
            get { return registerCheckBox.Checked; }
        }
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ComponentDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="csManager"></param>
        public ComponentDialog(ComponentManager csManager)
            :this()
        {
            m_csManager = csManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void componentItem_ItemChange(object sender, System.EventArgs e)
        {
            this.buttonOK.Enabled = componentItem.Changed || m_isExistSetting;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void SetContextMenu()
        {
            if (!m_isPathway)
                return;
            setExistingStencilToolStripMenuItem.Visible = true;
            setExistingStencilToolStripMenuItem.DropDownItems.Clear();
            // Set Stencil template
            string type = componentItem.PCanvas.Setting.Type;
            List<ComponentSetting> list = m_csManager.GetSettings(type);
            foreach (ComponentSetting cs in list)
            {
                if (!cs.IsStencil)
                    continue;
                StencilMenuItem item = new StencilMenuItem(cs.Icon);
                item.Setting = cs;
                item.Click += new EventHandler(item_Click);
                setExistingStencilToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            StencilMenuItem item = (StencilMenuItem)sender;
            this.Setting = item.Setting;
            m_isExistSetting = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ApplyChange()
        {
            componentItem.ApplyChange();
        }

        /// <summary>
        /// 
        /// </summary>
        public class StencilMenuItem : ToolStripMenuItem
        {
            private ComponentSetting m_setting = null;
            /// <summary>
            /// 
            /// </summary>
            public ComponentSetting Setting
            {
                get { return m_setting; }
                set { m_setting = value; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="image"></param>
            public StencilMenuItem(Image image)
                : base(image)
            {
            }
        }

        private void ComponentDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!componentItem.Changed && !m_isExistSetting)
                this.DialogResult = DialogResult.Cancel;
            else if(componentItem.Changed && m_isExistSetting)
            {
                ComponentSetting oldCs = componentItem.PCanvas.Setting;
                ComponentSetting newCs = oldCs.Clone();
                // Set Setting
                newCs.Name = m_csManager.GetRandomKey();
                newCs.IsDefault = false;
                newCs.IsStencil = false;
                componentItem.UpdateSetting(newCs);
                componentItem.SetItem(newCs);
            }
        }
    }
}