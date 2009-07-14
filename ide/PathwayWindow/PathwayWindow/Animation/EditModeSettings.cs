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
using Ecell.IDE.Plugins.PathwayWindow.Graphic;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class EditModeSettings : UserControl
    {
        private CheckBox unsharpCheckBox;
        private Ecell.IDE.Plugins.PathwayWindow.Graphic.BrushComboBox backgroundImageComboBox;
        private System.ComponentModel.IContainer components;
        private Ecell.IDE.Plugins.PathwayWindow.Graphic.BrushComboBox arrowColorImageComboBox;
        private TextBox widthTextBox;
        private AnimationControl m_control;

        public AnimationControl Control
        {
            set
            {
                m_control = value;
                backgroundImageComboBox.Brush = m_control.EditBGBrush;
                arrowColorImageComboBox.Brush = m_control.EditEdgeBrush;
                widthTextBox.Text = m_control.EdgeWidth.ToString();
                unsharpCheckBox.Checked = m_control.Control.HighQuality;
            }
        }

        public EditModeSettings()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public EditModeSettings(AnimationControl control) :
            this()
        {
            m_control = control;

            backgroundImageComboBox.Brush = control.EditBGBrush;
            arrowColorImageComboBox.Brush = control.EditEdgeBrush;
            widthTextBox.Text = control.EdgeWidth.ToString();
            unsharpCheckBox.Checked = control.Control.HighQuality;

            //MessageResources.DialogTextBackgroundBrush;
            //MessageResources.DialogTextEdgeWidth;
            //MessageResources.DialogTextEdgeBrush;
        }

        void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditModeSettings));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.unsharpCheckBox = new System.Windows.Forms.CheckBox();
            this.widthTextBox = new System.Windows.Forms.TextBox();
            this.arrowColorImageComboBox = new Ecell.IDE.Plugins.PathwayWindow.Graphic.BrushComboBox();
            this.backgroundImageComboBox = new Ecell.IDE.Plugins.PathwayWindow.Graphic.BrushComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // unsharpCheckBox
            // 
            resources.ApplyResources(this.unsharpCheckBox, "unsharpCheckBox");
            this.unsharpCheckBox.Name = "unsharpCheckBox";
            this.unsharpCheckBox.UseVisualStyleBackColor = true;
            // 
            // widthTextBox
            // 
            resources.ApplyResources(this.widthTextBox, "widthTextBox");
            this.widthTextBox.Name = "widthTextBox";
            // 
            // arrowColorImageComboBox
            // 
            this.arrowColorImageComboBox.Brush = null;
            resources.ApplyResources(this.arrowColorImageComboBox, "arrowColorImageComboBox");
            this.arrowColorImageComboBox.Name = "arrowColorImageComboBox";
            // 
            // backgroundImageComboBox
            // 
            this.backgroundImageComboBox.Brush = null;
            resources.ApplyResources(this.backgroundImageComboBox, "backgroundImageComboBox");
            this.backgroundImageComboBox.Name = "backgroundImageComboBox";
            // 
            // EditModeSettings
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.widthTextBox);
            this.Controls.Add(this.arrowColorImageComboBox);
            this.Controls.Add(this.backgroundImageComboBox);
            this.Controls.Add(this.unsharpCheckBox);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Name = "EditModeSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void EdgeWidthValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = widthTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, MessageResources.NameArrowWidth));
                widthTextBox.Text = Convert.ToString(m_control.EdgeWidth);
                e.Cancel = true;
                return;
            }
            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, MessageResources.NameArrowWidth));
                widthTextBox.Text = Convert.ToString(m_control.EdgeWidth);
                e.Cancel = true;
                return;
            }
        }

        public void ApplyChanges()
        {
            m_control.EditBGBrush = BrushManager.ParseStringToBrush(backgroundImageComboBox.Text);
            m_control.EditEdgeBrush = BrushManager.ParseStringToBrush(arrowColorImageComboBox.Text);
            m_control.EdgeWidth = float.Parse(widthTextBox.Text);
            m_control.Control.HighQuality = unsharpCheckBox.Checked;
            //m_control.EditBGBrush = this.bgBrushItem.Brush;
            //m_control.EditEdgeBrush = this.edgeBrushItem.Brush;
            //m_control.EdgeWidth = float.Parse(this.edgeWidth.Text);
            //m_control.Control.HighQuality = this.highQualityCheckBox.Checked;
        }

        public void ItemClosing()
        {
            string text = widthTextBox.Text;

            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy <= 0 || dummy > 100)
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidValue, MessageResources.NameArrowWidth));
            }
        }
    }
}
