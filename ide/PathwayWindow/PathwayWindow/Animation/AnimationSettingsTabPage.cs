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
    internal class AnimationTabPage : PropertyDialogPage
    {
        private AnimationControl m_con;
        private AnimationItems m_animationItems;

        public AnimationTabPage(AnimationControl control)
            : base()
        {
            InitializeComponent();
            m_con = control;
            m_animationItems = new AnimationItems(control);

            this.SuspendLayout();
            this.Controls.Add(m_animationItems);

            this.ResumeLayout();
            this.PerformLayout();
        }

        public override void ApplyChange()
        {
            try
            {
                base.ApplyChange();
                m_animationItems.ApplyChanges();
                m_con.SaveSettings();
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrUpdateConfig);
            }
        }

        public override void PropertyDialogClosing()
        {
            base.PropertyDialogClosing();
            m_animationItems.ItemClosing();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationTabPage));
            this.SuspendLayout();
            // 
            // AnimationTabPage
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
