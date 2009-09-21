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
using System.Collections.Generic;
using System.Text;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.Drawing;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyViewAnimationItem : AnimationItemBase
    {
        #region Fields
        private System.Windows.Forms.GroupBox variableBox;
        private System.Windows.Forms.GroupBox processBox;
        private System.Windows.Forms.CheckBox checkBoxNumberConc;
        private System.Windows.Forms.CheckBox checkBoxMolarConc;
        private System.Windows.Forms.CheckBox checkBoxValue;
        private System.Windows.Forms.CheckBox checkBoxMolarActivity;
        private System.Windows.Forms.CheckBox checkBoxActivity;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public PropertyViewAnimationItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public PropertyViewAnimationItem(AnimationControl control)
            : base(control)
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetPropertyItems()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyViewAnimationItem));
            System.Windows.Forms.Label label2;
            this.variableBox = new System.Windows.Forms.GroupBox();
            this.checkBoxNumberConc = new System.Windows.Forms.CheckBox();
            this.checkBoxMolarConc = new System.Windows.Forms.CheckBox();
            this.checkBoxValue = new System.Windows.Forms.CheckBox();
            this.processBox = new System.Windows.Forms.GroupBox();
            this.checkBoxMolarActivity = new System.Windows.Forms.CheckBox();
            this.checkBoxActivity = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.variableBox.SuspendLayout();
            this.processBox.SuspendLayout();
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
            // variableBox
            // 
            resources.ApplyResources(this.variableBox, "variableBox");
            this.variableBox.Controls.Add(label1);
            this.variableBox.Controls.Add(this.checkBoxNumberConc);
            this.variableBox.Controls.Add(this.checkBoxMolarConc);
            this.variableBox.Controls.Add(this.checkBoxValue);
            this.variableBox.Name = "variableBox";
            this.variableBox.TabStop = false;
            // 
            // checkBoxNumberConc
            // 
            resources.ApplyResources(this.checkBoxNumberConc, "checkBoxNumberConc");
            this.checkBoxNumberConc.Name = "checkBoxNumberConc";
            this.checkBoxNumberConc.UseVisualStyleBackColor = true;
            // 
            // checkBoxMolarConc
            // 
            resources.ApplyResources(this.checkBoxMolarConc, "checkBoxMolarConc");
            this.checkBoxMolarConc.Name = "checkBoxMolarConc";
            this.checkBoxMolarConc.UseVisualStyleBackColor = true;
            // 
            // checkBoxValue
            // 
            resources.ApplyResources(this.checkBoxValue, "checkBoxValue");
            this.checkBoxValue.Name = "checkBoxValue";
            this.checkBoxValue.UseVisualStyleBackColor = true;
            // 
            // processBox
            // 
            resources.ApplyResources(this.processBox, "processBox");
            this.processBox.Controls.Add(label2);
            this.processBox.Controls.Add(this.checkBoxMolarActivity);
            this.processBox.Controls.Add(this.checkBoxActivity);
            this.processBox.Name = "processBox";
            this.processBox.TabStop = false;
            // 
            // checkBoxMolarActivity
            // 
            resources.ApplyResources(this.checkBoxMolarActivity, "checkBoxMolarActivity");
            this.checkBoxMolarActivity.Name = "checkBoxMolarActivity";
            this.checkBoxMolarActivity.UseVisualStyleBackColor = true;
            // 
            // checkBoxActivity
            // 
            resources.ApplyResources(this.checkBoxActivity, "checkBoxActivity");
            this.checkBoxActivity.Name = "checkBoxActivity";
            this.checkBoxActivity.UseVisualStyleBackColor = true;
            // 
            // PropertyViewAnimationItem
            // 
            this.Controls.Add(this.processBox);
            this.Controls.Add(this.variableBox);
            this.Name = "PropertyViewAnimationItem";
            resources.ApplyResources(this, "$this");
            this.variableBox.ResumeLayout(false);
            this.variableBox.PerformLayout();
            this.processBox.ResumeLayout(false);
            this.processBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        #region Inherited methods
        /// <summary>
        /// 
        /// </summary>
        public override void ApplyChange()
        {
            base.ApplyChange();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetAnimation()
        {
            base.SetAnimation();
            // Variable
            foreach (PPathwayVariable variable in _variables)
            {
                variable.Property.Visible = true;
                // Visibility
                variable.Property.Properties[0].Visible = checkBoxValue.Checked;
                variable.Property.Properties[1].Visible = checkBoxMolarConc.Checked;
                variable.Property.Properties[2].Visible = checkBoxNumberConc.Checked;
                // SetParam
                foreach (PPathwayProperty property in variable.Property.Properties)
                {
                    string fullPN = variable.EcellObject.FullID + ":" + property.Label;
                    property.Value = this.GetTextValue(fullPN);
                }
                variable.Refresh();
            }

            // Process
            foreach (PPathwayProcess process in _processes)
            {
                process.Property.Visible = true;
                // Visibility
                process.Property.Properties[0].Visible = checkBoxActivity.Checked;
                process.Property.Properties[1].Visible = checkBoxMolarActivity.Checked;
                // SetParam
                foreach (PPathwayProperty property in process.Property.Properties)
                {
                    string fullPN = process.EcellObject.FullID + ":" + property.Label;
                    property.Value = this.GetTextValue(fullPN);
                }
                process.Refresh();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAnimation()
        {
            base.UpdateAnimation();
            // Variable
            foreach (PPathwayVariable variable in _variables)
            {
                if (!variable.Visible)
                    continue;
                foreach (PPathwayProperty property in variable.Property.Properties)
                {
                    if (!property.Visible)
                        continue;
                    string fullPN = variable.EcellObject.FullID + ":" + property.Label;
                    property.Value = this.GetTextValue(fullPN);
                }
            }
            // Process
            foreach (PPathwayProcess process in _processes)
            {
                if (!process.Visible)
                    continue;
                foreach (PPathwayProperty property in process.Property.Properties)
                {
                    if (!property.Visible)
                        continue;
                    string fullPN = process.EcellObject.FullID + ":" + property.Label;
                    property.Value = this.GetTextValue(fullPN);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StopAnimation()
        {
            UpdateAnimation();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetAnimation()
        {
            // Variable
            foreach (PPathwayVariable variable in _variables)
            {
                variable.Property.Visible = false;
            }
            // Process
            foreach (PPathwayProcess process in _processes)
            {
                process.Property.Visible = false;
            }
            base.ResetAnimation();

        }
        #endregion

    }
}
