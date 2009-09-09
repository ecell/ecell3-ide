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
        private System.Windows.Forms.RadioButton radioButtonNumberConc;
        private System.Windows.Forms.RadioButton radioButtonMolarConc;
        private System.Windows.Forms.RadioButton radioButtonValue;
        private System.Windows.Forms.RadioButton radioButtonMolarActivity;
        private System.Windows.Forms.RadioButton radioButtonActivity;
        private System.Windows.Forms.CheckBox checkBoxVariable;
        private System.Windows.Forms.CheckBox checkBoxProcess;

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
            this.checkBoxVariable = new System.Windows.Forms.CheckBox();
            this.radioButtonNumberConc = new System.Windows.Forms.RadioButton();
            this.radioButtonMolarConc = new System.Windows.Forms.RadioButton();
            this.radioButtonValue = new System.Windows.Forms.RadioButton();
            this.processBox = new System.Windows.Forms.GroupBox();
            this.checkBoxProcess = new System.Windows.Forms.CheckBox();
            this.radioButtonMolarActivity = new System.Windows.Forms.RadioButton();
            this.radioButtonActivity = new System.Windows.Forms.RadioButton();
            this.variableBox.SuspendLayout();
            this.processBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // variableBox
            // 
            resources.ApplyResources(this.variableBox, "variableBox");
            this.variableBox.Controls.Add(this.checkBoxVariable);
            this.variableBox.Controls.Add(this.radioButtonNumberConc);
            this.variableBox.Controls.Add(this.radioButtonMolarConc);
            this.variableBox.Controls.Add(this.radioButtonValue);
            this.variableBox.Name = "variableBox";
            this.variableBox.TabStop = false;
            // 
            // checkBoxVariable
            // 
            resources.ApplyResources(this.checkBoxVariable, "checkBoxVariable");
            this.checkBoxVariable.Name = "checkBoxVariable";
            this.checkBoxVariable.UseVisualStyleBackColor = true;
            // 
            // radioButtonNumberConc
            // 
            resources.ApplyResources(this.radioButtonNumberConc, "radioButtonNumberConc");
            this.radioButtonNumberConc.Name = "radioButtonNumberConc";
            this.radioButtonNumberConc.UseVisualStyleBackColor = true;
            // 
            // radioButtonMolarConc
            // 
            resources.ApplyResources(this.radioButtonMolarConc, "radioButtonMolarConc");
            this.radioButtonMolarConc.Name = "radioButtonMolarConc";
            this.radioButtonMolarConc.UseVisualStyleBackColor = true;
            // 
            // radioButtonValue
            // 
            resources.ApplyResources(this.radioButtonValue, "radioButtonValue");
            this.radioButtonValue.Checked = true;
            this.radioButtonValue.Name = "radioButtonValue";
            this.radioButtonValue.TabStop = true;
            this.radioButtonValue.UseVisualStyleBackColor = true;
            // 
            // processBox
            // 
            resources.ApplyResources(this.processBox, "processBox");
            this.processBox.Controls.Add(this.checkBoxProcess);
            this.processBox.Controls.Add(this.radioButtonMolarActivity);
            this.processBox.Controls.Add(this.radioButtonActivity);
            this.processBox.Name = "processBox";
            this.processBox.TabStop = false;
            // 
            // checkBoxProcess
            // 
            resources.ApplyResources(this.checkBoxProcess, "checkBoxProcess");
            this.checkBoxProcess.Name = "checkBoxProcess";
            this.checkBoxProcess.UseVisualStyleBackColor = true;
            // 
            // radioButtonMolarActivity
            // 
            resources.ApplyResources(this.radioButtonMolarActivity, "radioButtonMolarActivity");
            this.radioButtonMolarActivity.Name = "radioButtonMolarActivity";
            this.radioButtonMolarActivity.UseVisualStyleBackColor = true;
            // 
            // radioButtonActivity
            // 
            resources.ApplyResources(this.radioButtonActivity, "radioButtonActivity");
            this.radioButtonActivity.Checked = true;
            this.radioButtonActivity.Name = "radioButtonActivity";
            this.radioButtonActivity.TabStop = true;
            this.radioButtonActivity.UseVisualStyleBackColor = true;
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
                if (!checkBoxVariable.Checked)
                    continue;
                if (variable.Graph != null)
                {
                    _graphs.Add(variable.Graph);
                    _canvas.ControlLayer.AddChild(variable.Graph);
                    continue;
                }

                // Create Graph
                PPathwayGraph graph = new PPathwayGraph();
                graph.Title = variable.EcellObject.LocalID;
                graph.EntityPath = variable.EcellObject.FullID + ":" + GetVariableParam();
                _canvas.ControlLayer.AddChild(graph);
                graph.PointF = variable.PointF;
                graph.Refresh();

                _graphs.Add(graph);
                variable.Graph = graph;
            }
            // Process
            foreach (PPathwayProcess process in _processes)
            {
                if (!checkBoxProcess.Checked)
                    continue;
                if (process.Graph != null)
                {
                    _graphs.Add(process.Graph);
                    _canvas.ControlLayer.AddChild(process.Graph);
                    continue;
                }

                // Create Graph
                PPathwayGraph graph = new PPathwayGraph();
                graph.Title = process.EcellObject.LocalID;
                graph.EntityPath = process.EcellObject.FullID + ":" + GetProcessParam();
                _canvas.ControlLayer.AddChild(graph);
                graph.PointF = process.PointF;
                graph.Refresh();

                _graphs.Add(graph);
                process.Graph = graph;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetVariableParam()
        {
            string param = null;
            if (radioButtonValue.Checked)
                param = Constants.xpathValue;
            else if (radioButtonMolarConc.Checked)
                param = Constants.xpathMolarConc;
            else if (radioButtonNumberConc.Checked)
                param = Constants.xpathNumberConc;
            return param;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetProcessParam()
        {
            string param = null;
            if (radioButtonActivity.Checked)
                param = Constants.xpathActivity;
            else if (radioButtonMolarActivity.Checked)
                param = Constants.xpathMolarActivity;
            return param;
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
