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
            this.flowLayoutPanel.AccessibleDescription = null;
            this.flowLayoutPanel.AccessibleName = null;
            resources.ApplyResources(this.flowLayoutPanel, "flowLayoutPanel");
            this.flowLayoutPanel.BackColor = System.Drawing.SystemColors.Window;
            this.flowLayoutPanel.BackgroundImage = null;
            this.flowLayoutPanel.ContextMenuStrip = this.PanelMenuStrip;
            this.flowLayoutPanel.Font = null;
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Stencil_MouseDown);
            // 
            // PanelMenuStrip
            // 
            this.PanelMenuStrip.AccessibleDescription = null;
            this.PanelMenuStrip.AccessibleName = null;
            resources.ApplyResources(this.PanelMenuStrip, "PanelMenuStrip");
            this.PanelMenuStrip.BackgroundImage = null;
            this.PanelMenuStrip.Font = null;
            this.PanelMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddStencilMenuItem});
            this.PanelMenuStrip.Name = "contextMenuStrip";
            // 
            // AddStencilMenuItem
            // 
            this.AddStencilMenuItem.AccessibleDescription = null;
            this.AddStencilMenuItem.AccessibleName = null;
            resources.ApplyResources(this.AddStencilMenuItem, "AddStencilMenuItem");
            this.AddStencilMenuItem.BackgroundImage = null;
            this.AddStencilMenuItem.Name = "AddStencilMenuItem";
            this.AddStencilMenuItem.ShortcutKeyDisplayString = null;
            this.AddStencilMenuItem.Click += new System.EventHandler(this.AddStencilMenuItem_Click);
            // 
            // StencilMenuStrip
            // 
            this.StencilMenuStrip.AccessibleDescription = null;
            this.StencilMenuStrip.AccessibleName = null;
            resources.ApplyResources(this.StencilMenuStrip, "StencilMenuStrip");
            this.StencilMenuStrip.BackgroundImage = null;
            this.StencilMenuStrip.Font = null;
            this.StencilMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteStencilMenuItem,
            this.propertyToolStripMenuItem});
            this.StencilMenuStrip.Name = "contextMenuStrip";
            // 
            // DeleteStencilMenuItem
            // 
            this.DeleteStencilMenuItem.AccessibleDescription = null;
            this.DeleteStencilMenuItem.AccessibleName = null;
            resources.ApplyResources(this.DeleteStencilMenuItem, "DeleteStencilMenuItem");
            this.DeleteStencilMenuItem.BackgroundImage = null;
            this.DeleteStencilMenuItem.Name = "DeleteStencilMenuItem";
            this.DeleteStencilMenuItem.ShortcutKeyDisplayString = null;
            this.DeleteStencilMenuItem.Click += new System.EventHandler(this.DeleteStencilMenuItem_Click);
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.AccessibleDescription = null;
            this.propertyToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.propertyToolStripMenuItem, "propertyToolStripMenuItem");
            this.propertyToolStripMenuItem.BackgroundImage = null;
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            this.propertyToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.propertyToolStripMenuItem_Click);
            // 
            // Stencils
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = null;
            this.Controls.Add(this.flowLayoutPanel);
            this.Icon = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.Icon_Stencil;
            this.IsSavable = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Stencils";
            this.ToolTipText = null;
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
            foreach (ComponentSetting cs in m_con.ComponentManager.GetAllSettings())
            {
                if(cs.IsStencil)
                    SetNewItem(cs);
            }
        }

        private void SetNewItem(ComponentSetting cs)
        {
            PToolBoxCanvas pCanvas = new PToolBoxCanvas(cs);
            cs.IsStencil = true;
            ToolBoxDragHandler eventHandler = new ToolBoxDragHandler(this);
            pCanvas.AddInputEventListener(eventHandler);
            pCanvas.ContextMenuStrip = this.StencilMenuStrip;
            //pCanvas.MouseDown += new MouseEventHandler(Stencil_MouseDown);
            flowLayoutPanel.Controls.Add(pCanvas);
        }

        private void AddStencilMenuItem_Click(object sender, System.EventArgs e)
        {
            List<PPathwayObject> objects = m_con.Canvas.SelectedNodes;
            if (objects.Count != 1)
            {
                Util.ShowErrorDialog(MessageResources.ErrNoStencil);
                return;
            }
            PPathwayObject obj = objects[0];
            ComponentSetting cs = obj.Setting.Clone();
            if (cs.IsDefault || cs.IsStencil)
            {
                Util.ShowErrorDialog(MessageResources.ErrAddStencil);
                return;
            }

            SetNewItem(cs);
            m_con.ComponentManager.SaveSettings();
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
            this.flowLayoutPanel.Controls.Remove(m_stencil);
            m_stencil = null;
        }

        private void propertyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (m_stencil == null)
                return;
            // Show Setting Dialog.
            ComponentDialog dlg = new ComponentDialog(m_stencil.Setting);
            using (dlg)
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                dlg.ApplyChange();
                m_stencil.Setting.RaisePropertyChange();
            }
        }
    }
}