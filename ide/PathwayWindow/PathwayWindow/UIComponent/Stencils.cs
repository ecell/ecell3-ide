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
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Components;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.Plugin;
using System.Collections.Generic;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.Diagnostics;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// UI class for PathwayWindow
    /// </summary>
    public partial class Stencils : EcellDockContent
    {
        private PathwayControl m_con;

        private ContextMenuStrip PanelMenuStrip;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem AddStencilMenuItem;
        private ContextMenuStrip StencilMenuStrip;
        private ToolStripMenuItem DeleteStencilMenuItem;
        private FlowLayoutPanel flowLayoutPanel;
        private ToolStripMenuItem propertyToolStripMenuItem;
        private PToolBoxCanvas m_stencil = null;

        /// <summary>
        /// 
        /// </summary>
        public PToolBoxCanvas Stencil
        {
            get { return m_stencil; }
            set { m_stencil = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public PathwayControl PathwayControl
        {
            get { return m_con; }
            set { m_con = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public Stencils(PathwayControl control)
        {
            m_con = control;
            InitializeComponent();
            base.Text = MessageResources.WindowStencil;
            base.TabText = base.Text;

            SetDefaultItems();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stencils));
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.PanelMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddStencilMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StencilMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteStencilMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelMenuStrip.SuspendLayout();
            this.StencilMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            resources.ApplyResources(this.flowLayoutPanel, "flowLayoutPanel");
            this.flowLayoutPanel.BackColor = System.Drawing.SystemColors.Window;
            this.flowLayoutPanel.ContextMenuStrip = this.PanelMenuStrip;
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Stencil_MouseDown);
            // 
            // PanelMenuStrip
            // 
            this.PanelMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddStencilMenuItem});
            this.PanelMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.PanelMenuStrip, "PanelMenuStrip");
            this.PanelMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.PanelMenuStrip_Opening);
            // 
            // AddStencilMenuItem
            // 
            this.AddStencilMenuItem.Name = "AddStencilMenuItem";
            resources.ApplyResources(this.AddStencilMenuItem, "AddStencilMenuItem");
            this.AddStencilMenuItem.Click += new System.EventHandler(this.AddStencilMenuItem_Click);
            // 
            // StencilMenuStrip
            // 
            this.StencilMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteStencilMenuItem,
            this.propertyToolStripMenuItem});
            this.StencilMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.StencilMenuStrip, "StencilMenuStrip");
            this.StencilMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.StencilMenuStrip_Opening);
            // 
            // DeleteStencilMenuItem
            // 
            this.DeleteStencilMenuItem.Name = "DeleteStencilMenuItem";
            resources.ApplyResources(this.DeleteStencilMenuItem, "DeleteStencilMenuItem");
            this.DeleteStencilMenuItem.Click += new System.EventHandler(this.DeleteStencilMenuItem_Click);
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            resources.ApplyResources(this.propertyToolStripMenuItem, "propertyToolStripMenuItem");
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.propertyToolStripMenuItem_Click);
            // 
            // Stencils
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.flowLayoutPanel);
            this.Icon = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.Icon_Stencil;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Stencils";
            this.PanelMenuStrip.ResumeLayout(false);
            this.StencilMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDefaultItems()
        {
            List<ComponentSetting> list = m_con.ComponentManager.GetAllSettings();
            foreach (ComponentSetting cs in list)
            {
                if(cs.IsStencil)
                    SetNewItem(cs);
            }
        }

        private void SetNewItem(ComponentSetting cs)
        {
            PToolBoxCanvas pCanvas = new PToolBoxCanvas(cs);
            ToolBoxDragHandler eventHandler = new ToolBoxDragHandler(this);
            pCanvas.AddInputEventListener(eventHandler);
            pCanvas.ContextMenuStrip = this.StencilMenuStrip;
            //pCanvas.MouseDown += new MouseEventHandler(Stencil_MouseDown);
            flowLayoutPanel.Controls.Add(pCanvas);
        }

        private void AddStencilMenuItem_Click(object sender, System.EventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            List<EcellObject> list = m_con.CopiedNodes;
            if (list.Count != 1)
            {
                Util.ShowErrorDialog(MessageResources.ErrNoStencil);
                return;
            }

            EcellObject obj = list[0];
            ComponentSetting cs = m_con.ComponentManager.GetSetting(obj.Type, obj.Layout.Figure);
            if (cs.IsStencil)
            {
                Util.ShowErrorDialog(MessageResources.ErrAddStencil);
                return;
            }
            AddStencil(cs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cs"></param>
        public void AddStencil(ComponentSetting cs)
        {
            if (cs.IsDefault || cs.IsStencil)
            {
                Util.ShowErrorDialog(MessageResources.ErrAddStencil);
                return;
            }
            cs.IsStencil = true;
            m_con.ComponentManager.RegisterSetting(cs);
            m_con.ComponentManager.SaveSettings();
            SetNewItem(cs);
            m_con.SetNodeIcons();
        }

        private void Stencil_MouseDown(object sender, MouseEventArgs e)
        {
            m_stencil = null;

        }

        private void DeleteStencilMenuItem_Click(object sender, System.EventArgs e)
        {
            if (m_stencil == null)
                return;
            if (m_stencil.Setting.IsDefault)
            {
                Util.ShowErrorDialog(MessageResources.ErrDeleteDefaultStencil);
                return;
            }

            // Remove Stencil
            m_stencil.Setting.IsStencil = false;
            this.flowLayoutPanel.Controls.Remove(m_stencil);
            m_stencil = null;
            m_con.ComponentManager.SaveSettings();
        }

        private void propertyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (m_stencil == null)
                return;
            // Show Setting Dialog.
            ComponentDialog dlg = new ComponentDialog(m_con.ComponentManager);
            dlg.Setting = m_stencil.Setting;
            using (dlg)
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                dlg.ApplyChange();
                m_con.SetNodeIcons();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StencilMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DeleteStencilMenuItem.Enabled = !m_stencil.Setting.IsDefault;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = m_con.CopiedNodes.Count == 0;
        }
    }
}