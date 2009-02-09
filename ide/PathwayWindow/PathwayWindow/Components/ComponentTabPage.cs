using System;
using System.Collections.Generic;
using System.Text;
using Ecell.IDE.Plugins.PathwayWindow.Dialog;

namespace Ecell.IDE.Plugins.PathwayWindow.Components
{
    /// <summary>
    /// private class for ComponentSettingDialog
    /// </summary>
    internal class ComponentTabPage : PropertyDialogTabPage
    {
        ComponentManager m_manager = null;

        public ComponentTabPage(ComponentManager manager)
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
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentTabPage));
            this.SuspendLayout();
            // 
            // ComponentTabPage
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Font = null;
            this.ResumeLayout(false);

        }
    }
}
