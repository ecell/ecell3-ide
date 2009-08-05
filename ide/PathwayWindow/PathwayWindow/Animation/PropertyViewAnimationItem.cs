﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyComboboxItem propertyCombobox;
        private System.Windows.Forms.Label edgeLabel;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem propertyBrush;
        private string _property;
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
            SetPropertyItems();
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetPropertyItems()
        {
            this.propertyCombobox.ComboBox.Text = "MolarConc";
            this.propertyCombobox.ComboBox.Items.Add("MolarConc");
            this.propertyCombobox.ComboBox.Items.Add("NumberConc");
            this.propertyCombobox.ComboBox.Items.Add("Value");
            this.propertyBrush.Brush = _control.PropertyBrush;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyViewAnimationItem));
            this.variableBox = new System.Windows.Forms.GroupBox();
            this.edgeLabel = new System.Windows.Forms.Label();
            this.propertyCombobox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyComboboxItem();
            this.propertyBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.variableBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // variableBox
            // 
            resources.ApplyResources(this.variableBox, "variableBox");
            this.variableBox.Controls.Add(this.edgeLabel);
            this.variableBox.Controls.Add(this.propertyCombobox);
            this.variableBox.Controls.Add(this.propertyBrush);
            this.variableBox.Name = "variableBox";
            this.variableBox.TabStop = false;
            // 
            // edgeLabel
            // 
            this.edgeLabel.AutoEllipsis = true;
            this.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.edgeLabel, "edgeLabel");
            this.edgeLabel.Name = "edgeLabel";
            // 
            // propertyCombobox
            // 
            resources.ApplyResources(this.propertyCombobox, "propertyCombobox");
            this.propertyCombobox.Name = "propertyCombobox";
            // 
            // propertyBrush
            // 
            resources.ApplyResources(this.propertyBrush, "propertyBrush");
            this.propertyBrush.Name = "propertyBrush";
            // 
            // PropertyViewAnimationItem
            // 
            this.Controls.Add(this.variableBox);
            this.Name = "PropertyViewAnimationItem";
            resources.ApplyResources(this, "$this");
            this.variableBox.ResumeLayout(false);
            this.variableBox.PerformLayout();
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
            _property = this.propertyCombobox.ComboBox.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetProperty()
        {
            base.SetProperty();
            _property = this.propertyCombobox.ComboBox.Text;
            foreach (PPathwayVariable variable in _variables)
            {
                variable.Property.Visible = true;
                variable.Property.TextBrush = propertyBrush.Brush;
                string fullPN = variable.EcellObject.FullID + ":" + _property;
                variable.Property.Label = _property;
                variable.Property.Value = this.GetTextValue(fullPN);
                variable.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateProperty()
        {
            base.UpdateProperty();
            foreach (PPathwayVariable variable in _variables)
            {
                string fullPN = variable.EcellObject.FullID + ":" + _property;
                variable.Property.Value = this.GetTextValue(fullPN);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetProperty()
        {
            foreach (PPathwayVariable variable in _variables)
            {
                variable.Property.Visible = false;
                variable.Property.Label = "";
                variable.Property.Value = "";
            }
            base.ResetProperty();
        }
        #endregion

    }
}
