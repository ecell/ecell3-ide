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
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public class AnimationItemBase : UserControl, IAnimationItem
    {
        #region Fields
        /// <summary>
        /// Canva manager
        /// </summary>
        protected CanvasControl _canvas;
        /// <summary>
        /// Animation manager
        /// </summary>
        protected AnimationControl _control;
        /// <summary>
        /// data manager
        /// </summary>
        protected DataManager _dManager;
        /// <summary>
        /// ToolStripmenuItem to add animation setting.
        /// </summary>
        protected ToolStripMenuItem _menuItem;
        /// <summary>
        /// list of current systems.
        /// </summary>
        protected List<PPathwaySystem> _systems = new List<PPathwaySystem>();
        /// <summary>
        /// list of current steppers.
        /// </summary>
        protected List<PPathwayStepper> _steppers = new List<PPathwayStepper>();
        /// <summary>
        /// list of current processes.
        /// </summary>
        protected List<PPathwayProcess> _processes = new List<PPathwayProcess>();
        /// <summary>
        /// list of current variables.
        /// </summary>
        protected List<PPathwayVariable> _variables = new List<PPathwayVariable>();
        /// <summary>
        /// format of displayed number.
        /// </summary>
        private string _format = "";
        /// <summary>
        /// name of current stepper.
        /// </summary>
        protected string _stepper = "";
        #endregion

        #region Properties
        /// <summary>
        /// ToolStripmenuItem to add animation setting.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ToolStripMenuItem MenuItem
        {
            get { return _menuItem; }
            set { _menuItem = value; }
        }

        /// <summary>
        /// Title of this animation setting.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public AnimationItemBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public AnimationItemBase(AnimationControl control)
            : this()
        {
            _control = control;
            _dManager = control.Control.Window.DataManager;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            this._menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SuspendLayout();
            // 
            // _menuItem
            // 
            this._menuItem.Name = "_menuItem";
            this._menuItem.Size = new System.Drawing.Size(32, 19);
            this._menuItem.Text = "MenuItem";
            // 
            // AnimationItemBase
            // 
            this.AutoScroll = true;
            this.Name = "AnimationItemBase";
            this.Size = new System.Drawing.Size(476, 320);
            this.ResumeLayout(false);

        }
        #endregion

        #region IAnimationItem メンバ
        /// <summary>
        /// Set up animation.
        /// </summary>
        public virtual void SetAnimation()
        {
            if (_control == null)
                return;
            // Set canvas
            _canvas = _control.Canvas;
            // Set Entities
            ResetDic(); 
            _systems.AddRange(_canvas.Systems.Values);
            _steppers.AddRange(_canvas.Steppers.Values);
            _processes.AddRange(_canvas.Processes.Values);
            _variables.AddRange(_canvas.Variables.Values);

            _format = _dManager.DisplayStringFormat;
            _stepper = _steppers[0].EcellObject.Key;
        }

        /// <summary>
        /// Update animation status.
        /// </summary>
        public virtual void UpdateAnimation()
        {
        }
        /// <summary>
        /// Stop animation.
        /// </summary>
        public virtual void StopAnimation()
        {
        }

        /// <summary>
        /// Reset animation status.
        /// </summary>
        public virtual void ResetAnimation()
        {
            ResetDic();
        }

        /// <summary>
        /// reset current objects.
        /// </summary>
        private void ResetDic()
        {
            _systems.Clear();
            _steppers.Clear();
            _processes.Clear();
            _variables.Clear();
        }

        /// <summary>
        /// setup GUI of animation settings.
        /// </summary>
        public virtual void SetViewItem()
        {
        }

        /// <summary>
        /// reflect new parameters.
        /// </summary>
        public virtual void ApplyChange()
        {
        }

        /// <summary>
        /// check parameters.
        /// </summary>
        public virtual void CheckParameters()
        {
        }

        /// <summary>
        /// ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Text;
        }
        #endregion

        #region Getter
        /// <summary>
        /// Get current time on simulation.
        /// </summary>
        /// <returns></returns>
        protected double GetTime()
        {
            double time = 0.0d;
            try
            {
                if (_dManager.CurrentProject.SimulationStatus == SimulationStatus.Run ||
                        _dManager.CurrentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    time = _dManager.GetPropertyValue4Stepper(_stepper, "CurrentTime");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            return time;
        }

        /// <summary>
        /// Get current value of selected parameter.
        /// </summary>
        /// <param name="fullPN"></param>
        /// <returns></returns>
        protected double GetValue(string fullPN)
        {
            double num = 0.0d;
            try
            {
                if (_dManager.CurrentProject.SimulationStatus == SimulationStatus.Run ||
                        _dManager.CurrentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    num = _dManager.GetPropertyValue(fullPN);
                }
                else
                {
                    string key = "";
                    string type = "";
                    string propName = "";
                    Util.ParseFullPN(fullPN, out type, out key, out propName);
                    Ecell.Objects.EcellObject obj = _dManager.GetEcellObject(
                        _dManager.CurrentProject.Model.ModelID, key, type);
                    if (obj != null)
                    {
                        Ecell.Objects.EcellData data = obj.GetEcellData(propName);
                        num = (float)Double.Parse(data.Value.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                num = float.NaN;
            }
            return num;
        }

        /// <summary>
        /// Get current value of selected parameter.
        /// </summary>
        /// <param name="fullPN"></param>
        /// <returns></returns>
        protected string GetTextValue(string fullPN)
        {
            double value = GetValue(fullPN);
            string text = value.ToString(_format);
            return text;
        }

        #endregion

    }
}
