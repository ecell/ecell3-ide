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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Components;
using EcellLib.PathwayWindow.Nodes;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// Control class to display pathway.
    /// </summary>
    public class PathwayView : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// The PathwayControl, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        protected PathwayControl m_con;
        private StatusStrip StatusStrip;
        private ToolStripStatusLabel ObjectIDLabel;
        private ToolStripStatusLabel LocationLabel;
        private Panel OverviewContainer;
        private PScrollableControl ScrollContainer;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control">PathwayControl</param>
        public PathwayView(PathwayControl control)
        {
            base.m_isSavable = true;
            this.m_con = control;
            this.m_con.CanvasChange += new EventHandler(m_con_CanvasChange);

            InitializeComponent();
        }
        #endregion

        #region Inner Methods
        /// <summary>
        /// Change canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_con_CanvasChange(object _sender, EventArgs e)
        {
            PathwayControl sender = (PathwayControl)_sender;
            if (sender.Canvas == null)
                return;
            SuspendLayout();
            {
                Controls.Clear();
                OverviewContainer.Controls.Clear();
                ScrollContainer.Canvas = sender.Canvas.PCanvas;
                ScrollContainer.Canvas.MouseMove += new MouseEventHandler(canvas_MouseMove);
                OverviewContainer.Controls.Add(sender.Canvas.OverviewCanvas);
                Controls.Add(OverviewContainer);
                Controls.Add(ScrollContainer);
                Controls.Add(StatusStrip);
                Text = sender.Canvas.ModelID;
                TabText = this.Text;
            }
            ResumeLayout();
            Activate();
            Parent.Refresh();
        }

        /// <summary>
        /// Event on mouse move.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            PNode node = m_con.Canvas.FocusNode;
            if (node is PPathwayObject)
                this.ObjectIDLabel.Text = ((PPathwayObject)node).EcellObject.Key;
            else
                this.ObjectIDLabel.Text = null;
            Point systemPos = GetDesktopLocation(m_con.Canvas.PCanvas);
            PointF pos = m_con.Canvas.SystemPosToCanvasPos(new Point(e.Location.X + systemPos.X,e.Location.Y + systemPos.Y));
            this.LocationLabel.Text = "X:" + pos.X.ToString("###.##") + ", Y:" + pos.Y.ToString("###.##");
        }

        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ObjectIDLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.LocationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.OverviewContainer = new System.Windows.Forms.Panel();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ObjectIDLabel,
            this.LocationLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 469);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(622, 22);
            this.StatusStrip.TabIndex = 0;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // ObjectIDLabel
            // 
            this.ObjectIDLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ObjectIDLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.ObjectIDLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ObjectIDLabel.Name = "ObjectIDLabel";
            this.ObjectIDLabel.Size = new System.Drawing.Size(507, 17);
            this.ObjectIDLabel.Spring = true;
            this.ObjectIDLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LocationLabel
            // 
            this.LocationLabel.AutoSize = false;
            this.LocationLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.LocationLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new System.Drawing.Size(100, 17);
            this.LocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.OverviewContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OverviewContainer.Location = new System.Drawing.Point(476, 332);
            this.OverviewContainer.Name = "panel1";
            this.OverviewContainer.Size = new System.Drawing.Size(121, 112);
            this.OverviewContainer.BackColor = Color.Transparent;
            // 
            // PathwayView
            // 
            this.ClientSize = new System.Drawing.Size(622, 491);
            this.Controls.Add(this.OverviewContainer);
            this.Controls.Add(this.StatusStrip);
            this.Icon = global::EcellLib.PathwayWindow.PathwayResource.Icon_PathwayView;
            this.Name = "PathwayView";
            this.Text = MessageResPathway.WindowPathway;
            this.TabText = this.Text;

            this.ScrollContainer = new PScrollableControl();
            this.ScrollContainer.Dock = DockStyle.Fill;
           
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Refresh the canvas on size changed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (m_con.Canvas == null)
                return;
            this.PerformLayout();
        }
        #endregion
    }
}
