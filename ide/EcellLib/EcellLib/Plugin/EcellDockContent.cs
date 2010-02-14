//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Ecell.Plugin
{
    /// <summary>
    /// Class managed the DockContent
    /// </summary>
    public class EcellDockContent : DockContent
    {
        #region Fields
        /// <summary>
        /// The content type.
        /// </summary>
        protected DockContentType m_type = DockContentType.NONE;

        /// <summary>
        /// get / set the content type.
        /// </summary>
        public DockContentType ContentType
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EcellDockContent()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EcellDockContent));
            this.SuspendLayout();
            // 
            // EcellDockContent
            // 
            this.ClientSize = new System.Drawing.Size(392, 373);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(50, 50);
            this.Name = "EcellDockContent";
            this.ResumeLayout(false);

        }
        #endregion

        #region Methods
        /// <summary>
        /// Get DesktopBounds
        /// </summary>
        /// <returns></returns>
        public Point GetDesktopLocation()
        {
            if (this.Pane.FloatWindow != null)
            {
                return this.Pane.FloatWindow.DesktopLocation; 
            }
            else
            {
                return GetDesktopLocation(this);
            }
        }

        /// <summary>
        /// Get the location on desktop of control.
        /// </summary>
        /// <param name="control">Control object.</param>
        /// <returns>The position of control.</returns>
        public Point GetDesktopLocation(Control control)
        {
            Point pos = control.Location;
            if (control.Parent != null)
            {
                Point temp = GetDesktopLocation(control.Parent);
                pos = new Point(temp.X + pos.X, temp.Y + pos.Y);
            }
            return pos;
        }
        #endregion
    }

    /// <summary>
    /// DockContent Type.
    /// </summary>
    public enum DockContentType
    {
        /// <summary>
        /// no style.
        /// </summary>
        NONE = 0,
        /// <summary>
        /// analysis style.
        /// </summary>
        ANALYSIS = 1,
        /// <summary>
        /// graph style.
        /// </summary>
        GRAPH = 2
    }
}
