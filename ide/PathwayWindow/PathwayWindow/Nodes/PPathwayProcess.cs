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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayObject for E-Cell variable.
    /// </summary>
    public class PPathwayProcess : PPathwayEntity
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private PPathwayObject _stepper = null;
        #endregion

        #region Accessors
        /// <summary>
        /// get/set the related element.
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return base.m_ecellObj; }
            set
            {
                base.EcellObject = value;
                RefreshStepperIcon();
                ResetEdges();
            }
        }

        /// <summary>
        /// get/set m_isViewMode.
        /// </summary>
        public override bool ViewMode
        {
            get
            {
                return base.ViewMode;
            }
            set
            {
                ChangePath(value);
                base.ViewMode = value;
            }
        }

        /// <summary>
        /// Stepper Icon.
        /// </summary>
        public PPathwayObject Stepper
        {
            get { return _stepper; }
            set { _stepper = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayProcess()
            : base()
        {
        }
        /// <summary>
        /// create new PPathwayProcess.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayProcess();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        protected override void RefreshSettings()
        {
            base.RefreshSettings();
            ChangePath(m_isViewMode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void ChangePath(bool value)
        {
            PointF centerPos = this.CenterPointF;
            if (value)
            {
                base.AddPath(m_tempFigure.GraphicsPath, false);
            }
            else
            {
                base.AddPath(m_figure.GraphicsPath, false);
            }
            base.CenterPointF = centerPos;
            RefreshStepperIcon();
        }

        /// <summary>
        /// check whethet exist invalid process in list.
        /// </summary>
        public void ResetEdges()
        {
            if (base.m_canvas == null || m_ecellObj == null || m_layer == null)
                return;
            DeleteEdges();
            CreateEdges();
        }

        /// <summary>
        /// create edge by using the information of element.
        /// </summary>
        private void CreateEdges()
        {
            // Error Check
            EcellProcess process = (EcellProcess)m_ecellObj;
            if (process == null || process.ReferenceList == null)
                return;
            List<EcellReference> list = process.ReferenceList;
            // Check if this node is tarminal node or not.

            try
            {
                Brush brush = m_canvas.Control.Animation.EdgeBrush;
                float width = m_canvas.Control.Animation.EdgeWidth;
                foreach (EcellReference er in list)
                {
                    if (!base.m_canvas.Variables.ContainsKey(er.Key))
                        continue;

                    PPathwayVariable var = base.m_canvas.Variables[er.Key];
                    EdgeInfo edge = new EdgeInfo(process.Key, list, er);
                    PPathwayLine line = new PPathwayLine(m_canvas, edge, this, var);
                    m_layer.AddChild(line);
                    line.EdgeBrush = brush;
                    line.EdgeWidth = width;
                    line.Selected = this.Selected || var.Selected;
                    line.Visible = this.Visible && var.Visible;
                    line.Pickable = line.Visible;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(" target is " + e.TargetSite);
            }
        }

        /// <summary>
        /// delete all related process from list.
        /// </summary>
        private void DeleteEdges()
        {
            foreach (PPathwayLine line in m_relations)
            {
                line.RemoveFromParent();
                line.Dispose();
            }
            m_relations.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void RefreshStepperIcon()
        {
            if (m_canvas == null)
                return;

            // Create Stepper Icon
            if (_stepper == null)
            {
                _stepper = m_canvas.Control.ComponentManager.StepperSetting.CreateTemplate();
                _stepper.Pickable = false;
            }
            EcellValue value = m_ecellObj.GetEcellValue(EcellProcess.STEPPERID);
            if(value == null)
                return;
            PPathwayStepper stepper = null;
            if(!m_canvas.Steppers.TryGetValue((string)value, out stepper))
                return;

            this.AddChild(_stepper);
            _stepper.AddPath(stepper.Figure.CreatePath(_stepper.Rect), false);
            _stepper.Width = 10;
            _stepper.Height = 10;
            _stepper.X = this.Right - 10;
            _stepper.Y = this.Bottom - 10;

            _stepper.LineBrush = stepper.Setting.LineBrush;
            _stepper.Brush = stepper.Setting.CreateBrush(_stepper.Path);
            _stepper.MoveToFront();
        }

        /// <summary>
        /// Event on Dispose
        /// </summary>
        public override void Dispose()
        {
            DeleteEdges();
            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF refPoint)
        {
            if (m_isViewMode && this is PPathwayProcess)
                return m_tempFigure.GetContactPoint(refPoint, CenterPointF);
            else
                return base.GetContactPoint(refPoint);
        }

        /// <summary>
        /// SetTextVisiblity
        /// </summary>
        protected override void SetTextVisiblity()
        {
            if (m_showingId && !m_isViewMode)
                m_pText.Visible = true;
            else
                m_pText.Visible = false;
        }

        private static bool IsEndNode(List<EcellReference> list)
        {
            bool isEndNode = true;
            foreach (EcellReference er in list)
            {
                if (er.Coefficient != 1)
                    continue;
                isEndNode = false;
                break;
            }
            return isEndNode;
        }
        #endregion
    }
}