using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.IDE.Plugins.PathwayWindow.Components
{
    /// <summary>
    /// private class for ComponentSettingDialog
    /// </summary>
    internal class ComponentSettingsPage : PropertyDialogPage
    {
        ComponentManager m_manager = null;

        public ComponentSettingsPage(ComponentManager manager)
            : base()
        {
            InitializeComponent();

            m_manager = manager;

            this.SuspendLayout();
            int top = 0;
            foreach (ComponentSetting cs in m_manager.ComponentSettings)
            {
                ComponentItem item = new ComponentItem(cs);
                item.Top = top;
                item.SuspendLayout();
                this.Controls.Add(item);
                item.ResumeLayout();
                item.PerformLayout();
                top += item.Height;
            }
            this.ResumeLayout();
        }
        public override void ApplyChange()
        {
            base.ApplyChange();
            foreach (ComponentItem item in this.Controls)
            {
                item.ApplyChange();
            }
            m_manager.SaveComponentSettings();
            m_manager.Control.ResetObjectSettings();

        }

        public override void PropertyDialogClosing()
        {
            base.PropertyDialogClosing();
            foreach (ComponentItem item in this.Controls)
            {
                item.ItemClosing();
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentSettingsPage));
            this.SuspendLayout();
            // 
            // ComponentSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "ComponentSettingsPage";
            this.ResumeLayout(false);

        }
    }
}
