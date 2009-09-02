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
using Ecell.Objects;
using Ecell.Logger;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphAnimationItem : AnimationItemBase
    {
        #region Fields
        private System.Windows.Forms.GroupBox variableBox;
        private System.Windows.Forms.GroupBox processBox;
        private System.Windows.Forms.CheckBox checkBoxNumberConc;
        private System.Windows.Forms.CheckBox checkBoxMolarConc;
        private System.Windows.Forms.CheckBox checkBoxValue;
        private System.Windows.Forms.CheckBox checkBoxMolarActivity;
        private System.Windows.Forms.CheckBox checkBoxActivity;

        private List<PPathwayGraph> _graphs = new List<PPathwayGraph>();
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public GraphAnimationItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public GraphAnimationItem(AnimationControl control)
            : base(control)
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphAnimationItem));
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
            resources.ApplyResources(this.variableBox, "variableBox");
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
            // GraphAnimationItem
            // 
            this.Controls.Add(this.processBox);
            this.Controls.Add(this.variableBox);
            this.Name = "GraphAnimationItem";
            resources.ApplyResources(this, "$this");
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
        public override void SetAnimation()
        {
            LoggerManager manager = _control.Control.Window.Environment.LoggerManager;
            _graphs.Clear();
            base.SetAnimation();
            // Variable
            foreach (PPathwayVariable variable in _variables)
            {

                if (variable.Graph != null)
                {
                    _graphs.Add(variable.Graph);
                    _canvas.ControlLayer.AddChild(variable.Graph);
                    continue;
                }

                // Create Graph
                PPathwayGraph graph = new PPathwayGraph(variable);
                graph.Title = variable.EcellObject.LocalID;
                graph.EntityPath = variable.EcellObject.FullID + ":MolarConc";
                _canvas.ControlLayer.AddChild(graph);
                graph.PointF = variable.PointF;
                graph.Refresh();

                _graphs.Add(graph);
                variable.Graph = graph;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAnimation()
        {
            base.UpdateAnimation();
            // Graph
            foreach (PPathwayGraph graph in _graphs)
            {
                double value = GetValue(graph.EntityPath);
                graph.SetValue(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StopAnimation()
        {
            // Graph
            foreach (PPathwayGraph graph in _graphs)
            {
                graph.Plots.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetAnimation()
        {
            // Graph
            base.ResetAnimation();
            ResetGraphs();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetGraphs()
        {
            foreach (PPathwayGraph graph in _graphs)
            {
                graph.RemoveFromParent();
            }
            _graphs.Clear();
        }
        #endregion

    }
}