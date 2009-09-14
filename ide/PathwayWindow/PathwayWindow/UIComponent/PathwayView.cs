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

using System;
using System.Drawing;
using System.Windows.Forms;
using Ecell.Plugin;
using Ecell.Reporting;
using UMD.HCIL.Piccolo;
using UMD.HCIL.PiccoloX.Components;
using WeifenLuo.WinFormsUI.Docking;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
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
            this.m_con = control;
            this.m_con.CanvasChange += new EventHandler(m_con_CanvasChange);
            this.DockStateChanged += new EventHandler(PathwayView_DockStateChanged);

            InitializeComponent();
        }

        void PathwayView_DockStateChanged(object sender, EventArgs e)
        {
            m_con.Menu.ToolButtons.Visible = !(this.DockState == DockState.Hidden);
        }
        #endregion

        /// <summary>
        /// Visibility of Overview canvas.
        /// </summary>
        public bool OverviewVisibility
        {
            get { return OverviewContainer.Visible; }
            set { OverviewContainer.Visible = value; }
        }

        #region Inner Methods
        /// <summary>
        /// Change canvas.
        /// </summary>
        /// <param name="_sender"></param>
        /// <param name="e"></param>
        private void m_con_CanvasChange(object _sender, EventArgs e)
        {
            PathwayControl sender = (PathwayControl)_sender;
            SuspendLayout();
            {
                Controls.Clear();
                OverviewContainer.Controls.Clear();
                if (sender.Canvas != null)
                {
                    ScrollContainer.Canvas = sender.Canvas.PCanvas;
                    ScrollContainer.Canvas.MouseMove += new MouseEventHandler(canvas_MouseMove);
                    OverviewContainer.Controls.Add(sender.Canvas.OverviewCanvas);
                    Text = sender.Canvas.ModelID;
                    TabText = this.Text;
                }
                else
                {
                    ScrollContainer.Canvas = new PCanvas();
                    Text = MessageResources.WindowPathway;
                    TabText = Text;
                }
                Controls.Add(OverviewContainer);
                Controls.Add(ScrollContainer.Canvas);
                Controls.Add(ScrollContainer);
            }
            ResumeLayout();
            Activate();
        }

        /// <summary>
        /// Event on mouse move.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            ReportManager rman = m_con.Window.Environment.ReportManager;
            Point systemPos = GetDesktopLocation(m_con.Canvas.PCanvas);
            PointF pos = m_con.Canvas.SystemPosToCanvasPos(new Point(e.Location.X + systemPos.X,e.Location.Y + systemPos.Y));
            // Set pos
            m_con.MousePosition = pos;
            // m_con.Canvas.FocusNode = m_con.Canvas.GetPickedNode(pos);

            rman.SetStatus(
                StatusBarMessageKind.QuickInspector,
                string.Format("X:{0:#0000}, Y:{1:#0000}", pos.X, pos.Y)
            );
        }

        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            this.OverviewContainer = new System.Windows.Forms.Panel();
            this.ScrollContainer = new UMD.HCIL.PiccoloX.Components.PScrollableControl();
            this.SuspendLayout();
            // 
            // OverviewContainer
            // 
            this.OverviewContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OverviewContainer.BackColor = System.Drawing.Color.White;
            this.OverviewContainer.Location = new System.Drawing.Point(456, 312);
            this.OverviewContainer.Name = "OverviewContainer";
            this.OverviewContainer.Size = new System.Drawing.Size(140, 140);
            this.OverviewContainer.TabIndex = 0;
            // 
            // ScrollContainer
            // 
            this.ScrollContainer.Dock = DockStyle.Fill;
            this.ScrollContainer.VsbPolicy = ScrollBarPolicy.Always;
            this.ScrollContainer.HsbPolicy = ScrollBarPolicy.Always;
            // 
            // PathwayView
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(622, 491);
            this.Controls.Add(this.OverviewContainer);
            this.Icon = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.Icon_PathwayView;
            this.Name = "PathwayView";
            this.TabText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.WindowPathway;
            this.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.WindowPathway;
            this.ResumeLayout(false);

        }

        /// <summary>
        /// ProcessCmdKey
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((int)this.DockState != 1)
                return base.ProcessCmdKey(ref msg, keyData);

            if ((int)keyData == (int)Keys.Control + (int)Keys.C)
            {
                m_con.CopyNodes();
                return true;
            }
            else if ((int)keyData == (int)Keys.Control + (int)Keys.V)
            {
                m_con.Canvas.PCanvas.Camera.Pickable = false;
                try
                {
                    m_con.PasteNodes();
                }
                catch (Exception e)
                {
                    Util.ShowErrorDialog(e.Message);
                }
                m_con.Canvas.PCanvas.Camera.Pickable = true;
                return true;
            }
            else if ((int)keyData == (int)Keys.Control + (int)Keys.X)
            {
                m_con.CutNodes();
            }
            else if ((int)keyData == (int)Keys.Delete ||
                      (int)keyData == (int)Keys.Control + (int)Keys.D)
            {
                m_con.DeteleNodes();
            }
            else if ((int)keyData == (int)Keys.Control + (int)Keys.Z)
            {
                m_con.Undo();
            }
            else if ((int)keyData == (int)Keys.Control + (int)Keys.Y)
            {
                m_con.Redo();
            }
            return base.ProcessCmdKey(ref msg, keyData);

        }
        #endregion
    }
}
