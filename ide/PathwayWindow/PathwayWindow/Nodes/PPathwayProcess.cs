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
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.Objects;
using System.Diagnostics;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayObject for E-Cell variable.
    /// </summary>
    public class PPathwayProcess : PPathwayNode
    {
        #region Static readonly fields
        #endregion

        #region Fields
        /// <summary>
        /// edge brush.
        /// </summary>
        private Brush m_edgeBrush = Brushes.Black;
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
                ResetEdges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush EdgeBrush
        {
            get { return m_edgeBrush; }
            set 
            {
                m_edgeBrush = value;
                foreach (PPathwayLine line in m_relations)
                {
                    line.Pen.Brush = m_edgeBrush;
                    line.Brush = m_edgeBrush;
                }
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
                if (m_isViewMode != value)
                {
                    ChangePath(value);
                }
                base.ViewMode = value;
            }
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
        /// Set Line Width.
        /// </summary>
        /// <param name="width"></param>
        public void SetLineWidth(float width)
        {
            foreach (PPathwayLine line in m_relations)
                line.Pen.Width = width;
        }
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
            m_path.Reset();
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
        public void CreateEdges()
        {
            // Error Check
            EcellProcess process = (EcellProcess)m_ecellObj;
            if (process == null || process.ReferenceList == null)
                return;
            List<EcellReference> list = process.ReferenceList;
            // Check if this node is tarminal node or not.

            try
            {
                foreach (EcellReference er in list)
                {
                    if (!base.m_canvas.Variables.ContainsKey(er.Key))
                        continue;

                    PPathwayVariable var = base.m_canvas.Variables[er.Key];
                    EdgeInfo edge = new EdgeInfo(process.Key, list, er);
                    PPathwayLine line = new PPathwayLine(m_canvas, edge, this, var);
                    m_layer.AddChild(line);
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
        public void DeleteEdges()
        {
            foreach (PPathwayLine line in m_relations)
            {
                if (line.Parent != null)
                    line.Parent.RemoveChild(line);
                else
                    line.CloseAllFigures();
                line.Dispose();
            }
            m_relations.Clear();
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