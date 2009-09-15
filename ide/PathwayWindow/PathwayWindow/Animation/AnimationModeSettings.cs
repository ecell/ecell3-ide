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
using System.Windows.Forms;

using Ecell.Exceptions;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.IDE.Plugins.PathwayWindow.Graphics;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class AnimationModeSettings : UserControl
    {
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox backgroundImageComboBox;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox arrowImageComboBox;
        private TextBox maxWidthTextBox;

        private AnimationControl m_control;

        public AnimationControl Control
        {
            set
            {
                this.m_control = value;
                backgroundImageComboBox.Brush = m_control.ViewBGBrush;
                arrowImageComboBox.Brush = m_control.ViewEdgeBrush;
                maxWidthTextBox.Text = m_control.MaxEdgeWidth.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AnimationModeSettings()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public AnimationModeSettings(AnimationControl control) :
            this()
        {
            m_control = control;

            backgroundImageComboBox.Brush = control.ViewBGBrush;
            arrowImageComboBox.Brush = control.ViewEdgeBrush;
            maxWidthTextBox.Text = control.MaxEdgeWidth.ToString();
        }

        void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationModeSettings));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.maxWidthTextBox = new System.Windows.Forms.TextBox();
            this.arrowImageComboBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.backgroundImageComboBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AccessibleDescription = null;
            label1.AccessibleName = null;
            resources.ApplyResources(label1, "label1");
            label1.Font = null;
            label1.Name = "label1";
            // 
            // label2
            // 
            label2.AccessibleDescription = null;
            label2.AccessibleName = null;
            resources.ApplyResources(label2, "label2");
            label2.Font = null;
            label2.Name = "label2";
            // 
            // label3
            // 
            label3.AccessibleDescription = null;
            label3.AccessibleName = null;
            resources.ApplyResources(label3, "label3");
            label3.Font = null;
            label3.Name = "label3";
            // 
            // label4
            // 
            label4.AccessibleDescription = null;
            label4.AccessibleName = null;
            resources.ApplyResources(label4, "label4");
            label4.Font = null;
            label4.Name = "label4";
            // 
            // maxWidthTextBox
            // 
            this.maxWidthTextBox.AccessibleDescription = null;
            this.maxWidthTextBox.AccessibleName = null;
            resources.ApplyResources(this.maxWidthTextBox, "maxWidthTextBox");
            this.maxWidthTextBox.BackgroundImage = null;
            this.maxWidthTextBox.Font = null;
            this.maxWidthTextBox.Name = "maxWidthTextBox";
            // 
            // arrowImageComboBox
            // 
            this.arrowImageComboBox.AccessibleDescription = null;
            this.arrowImageComboBox.AccessibleName = null;
            resources.ApplyResources(this.arrowImageComboBox, "arrowImageComboBox");
            this.arrowImageComboBox.BackgroundImage = null;
            this.arrowImageComboBox.Font = null;
            this.arrowImageComboBox.Name = "arrowImageComboBox";
            // 
            // backgroundImageComboBox
            // 
            this.backgroundImageComboBox.AccessibleDescription = null;
            this.backgroundImageComboBox.AccessibleName = null;
            resources.ApplyResources(this.backgroundImageComboBox, "backgroundImageComboBox");
            this.backgroundImageComboBox.BackgroundImage = null;
            this.backgroundImageComboBox.Font = null;
            this.backgroundImageComboBox.Name = "backgroundImageComboBox";
            // 
            // AnimationModeSettings
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.maxWidthTextBox);
            this.Controls.Add(label4);
            this.Controls.Add(this.arrowImageComboBox);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.backgroundImageComboBox);
            this.Controls.Add(label1);
            this.Font = null;
            this.Name = "AnimationModeSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        void MaxEdgeWidthValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = maxWidthTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, MessageResources.NameArrowMaxWidth));
                maxWidthTextBox.Text = Convert.ToString(m_control.MaxEdgeWidth);
                e.Cancel = true;
                return;
            }
            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, MessageResources.NameArrowMaxWidth));
                maxWidthTextBox.Text = Convert.ToString(m_control.MaxEdgeWidth);
                e.Cancel = true;
                return;
            }
        }

        public void ApplyChanges()
        {
            m_control.ViewBGBrush = backgroundImageComboBox.Brush;
            m_control.ViewEdgeBrush = arrowImageComboBox.Brush;
            m_control.MaxEdgeWidth = float.Parse(maxWidthTextBox.Text);
            //m_control.ViewBGBrush = bgBrush.Brush;
            //m_control.MaxEdgeWidth = float.Parse(edgeWidth.Text);
            //m_control.ViewEdgeBrush = edgeBrush.Brush;
        }

        public void ItemClosing()
        {
            string text = maxWidthTextBox.Text;

            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy <= 0 || dummy > 100)
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidValue, MessageResources.NameArrowMaxWidth));
            }
        }
    }
}
