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

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public class AnimationItemBase : UserControl, IAnimationItem
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        protected CanvasControl _canvas;
        
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
        /// <param name="canvas"></param>
        public AnimationItemBase(CanvasControl canvas)
            : this()
        {
            _canvas = canvas;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AnimationItem
            // 
            this.Name = "AnimationItem";
            this.Size = new System.Drawing.Size(369, 320);
            this.ResumeLayout(false);
        }
        #endregion


        #region IAnimationItem メンバ
        /// <summary>
        /// 
        /// </summary>
        public virtual void SetPropForSimulation()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdatePropForSimulation()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ResetPropForSimulation()
        {
        }

        #endregion
    }
}
