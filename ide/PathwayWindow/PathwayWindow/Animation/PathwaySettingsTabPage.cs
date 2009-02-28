using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ecell.IDE.Plugins.PathwayWindow.Dialog;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class PathwayTabPage : PropertyDialogTabPage
    {
        private AnimationControl m_con;
        private EditModeItems m_editModeItems;
        private ViewModeItems m_viewModeItems;

        public PathwayTabPage(AnimationControl control)
            : base()
        {
            InitializeComponent();
            m_con = control;
            m_editModeItems = new EditModeItems(control);
            m_viewModeItems = new ViewModeItems(control);

            this.SuspendLayout();
            this.Controls.Add(m_editModeItems);
            this.Controls.Add(m_viewModeItems);

            m_viewModeItems.Top = m_editModeItems.Top + m_editModeItems.Height;
            this.ResumeLayout();
            this.PerformLayout();
        }

        public override void ApplyChange()
        {
            try
            {
                base.ApplyChange();
                m_editModeItems.ApplyChanges();
                m_viewModeItems.ApplyChanges();
                m_con.SaveSettings();
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrUpdateConfig);
            }
        }

        public override void TabPageClosing()
        {
            base.TabPageClosing();
            m_editModeItems.ItemClosing();
            m_viewModeItems.ItemClosing();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathwayTabPage));
            this.SuspendLayout();
            // 
            // PathwayTabPage
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
