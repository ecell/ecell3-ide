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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyViewAnimationItem));
            this.variableBox = new System.Windows.Forms.GroupBox();
            this.checkBoxNumberConc = new System.Windows.Forms.CheckBox();
            this.checkBoxMolarConc = new System.Windows.Forms.CheckBox();
            this.checkBoxValue = new System.Windows.Forms.CheckBox();
            this.processBox = new System.Windows.Forms.GroupBox();
            this.checkBoxMolarActivity = new System.Windows.Forms.CheckBox();
            this.checkBoxActivity = new System.Windows.Forms.CheckBox();
            this.variableBox.SuspendLayout();
            this.processBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // variableBox
            // 
            this.variableBox.AccessibleDescription = null;
            this.variableBox.AccessibleName = null;
            resources.ApplyResources(this.variableBox, "variableBox");
            this.variableBox.BackgroundImage = null;
            this.variableBox.Controls.Add(this.checkBoxNumberConc);
            this.variableBox.Controls.Add(this.checkBoxMolarConc);
            this.variableBox.Controls.Add(this.checkBoxValue);
            this.variableBox.Font = null;
            this.variableBox.Name = "variableBox";
            this.variableBox.TabStop = false;
            // 
            // checkBoxNumberConc
            // 
            this.checkBoxNumberConc.AccessibleDescription = null;
            this.checkBoxNumberConc.AccessibleName = null;
            resources.ApplyResources(this.checkBoxNumberConc, "checkBoxNumberConc");
            this.checkBoxNumberConc.BackgroundImage = null;
            this.checkBoxNumberConc.Font = null;
            this.checkBoxNumberConc.Name = "checkBoxNumberConc";
            this.checkBoxNumberConc.UseVisualStyleBackColor = true;
            // 
            // checkBoxMolarConc
            // 
            this.checkBoxMolarConc.AccessibleDescription = null;
            this.checkBoxMolarConc.AccessibleName = null;
            resources.ApplyResources(this.checkBoxMolarConc, "checkBoxMolarConc");
            this.checkBoxMolarConc.BackgroundImage = null;
            this.checkBoxMolarConc.Font = null;
            this.checkBoxMolarConc.Name = "checkBoxMolarConc";
            this.checkBoxMolarConc.UseVisualStyleBackColor = true;
            // 
            // checkBoxValue
            // 
            this.checkBoxValue.AccessibleDescription = null;
            this.checkBoxValue.AccessibleName = null;
            resources.ApplyResources(this.checkBoxValue, "checkBoxValue");
            this.checkBoxValue.BackgroundImage = null;
            this.checkBoxValue.Font = null;
            this.checkBoxValue.Name = "checkBoxValue";
            this.checkBoxValue.UseVisualStyleBackColor = true;
            // 
            // processBox
            // 
            this.processBox.AccessibleDescription = null;
            this.processBox.AccessibleName = null;
            resources.ApplyResources(this.processBox, "processBox");
            this.processBox.BackgroundImage = null;
            this.processBox.Controls.Add(this.checkBoxMolarActivity);
            this.processBox.Controls.Add(this.checkBoxActivity);
            this.processBox.Font = null;
            this.processBox.Name = "processBox";
            this.processBox.TabStop = false;
            // 
            // checkBoxMolarActivity
            // 
            this.checkBoxMolarActivity.AccessibleDescription = null;
            this.checkBoxMolarActivity.AccessibleName = null;
            resources.ApplyResources(this.checkBoxMolarActivity, "checkBoxMolarActivity");
            this.checkBoxMolarActivity.BackgroundImage = null;
            this.checkBoxMolarActivity.Font = null;
            this.checkBoxMolarActivity.Name = "checkBoxMolarActivity";
            this.checkBoxMolarActivity.UseVisualStyleBackColor = true;
            // 
            // checkBoxActivity
            // 
            this.checkBoxActivity.AccessibleDescription = null;
            this.checkBoxActivity.AccessibleName = null;
            resources.ApplyResources(this.checkBoxActivity, "checkBoxActivity");
            this.checkBoxActivity.BackgroundImage = null;
            this.checkBoxActivity.Font = null;
            this.checkBoxActivity.Name = "checkBoxActivity";
            this.checkBoxActivity.UseVisualStyleBackColor = true;
            // 
            // PropertyViewAnimationItem
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.processBox);
            this.Controls.Add(this.variableBox);
            this.Font = null;
            this.Name = "PropertyViewAnimationItem";
            this.variableBox.ResumeLayout(false);
            this.variableBox.PerformLayout();
            this.processBox.ResumeLayout(false);
            this.processBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        public override void SetProperty()
        {
            base.SetProperty();
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
        public override void UpdateProperty()
        {
            base.UpdateProperty();
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
        public override void ResetProperty()
        {
            if (_control.Control.IsAnimation)
            {
                SetProperty();
                return;
            }
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
            base.ResetProperty();

        }
        #endregion

    }
}
