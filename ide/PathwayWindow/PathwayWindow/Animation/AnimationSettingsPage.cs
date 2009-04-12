﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class AnimationSettingsPage : PropertyDialogPage
    {
        private AnimationControl m_con;
        private AnimationItems m_animationItems;

        public AnimationSettingsPage(AnimationControl control)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationSettingsPage));
            this.SuspendLayout();
            // 
            // AnimationSettingsPage
            // 
            this.Name = "AnimationSettingsPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }
    }
}