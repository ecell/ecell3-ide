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
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.Drawing;
using AviFile;
using System.IO;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public class MovieAnimationItem : AnimationItemBase
    {
        private System.Windows.Forms.GroupBox outputBox;
        private System.Windows.Forms.Label label1;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertySaveFileItem aviFileName;

        #region Fields
        private const string MovieFile = "ecell.avi";
        /// <summary>
        /// 
        /// </summary>
        private AviManager _aviManager = null;
        /// <summary>
        /// 
        /// </summary>
        private VideoStream _stream = null;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public MovieAnimationItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public MovieAnimationItem(AnimationControl control)
            : base(control)
        {
            InitializeComponent();
            this.aviFileName.FileName = Path.Combine(Util.GetBaseDir(), MovieFile);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MovieAnimationItem));
            this.outputBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.aviFileName = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertySaveFileItem();
            this.outputBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // outputBox
            // 
            resources.ApplyResources(this.outputBox, "outputBox");
            this.outputBox.Controls.Add(this.label1);
            this.outputBox.Controls.Add(this.aviFileName);
            this.outputBox.Name = "outputBox";
            this.outputBox.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // aviFileName
            // 
            resources.ApplyResources(this.aviFileName, "aviFileName");
            this.aviFileName.FileName = "ecell.avi";
            this.aviFileName.Filter = null;
            this.aviFileName.FilterIndex = 0;
            this.aviFileName.Name = "aviFileName";
            // 
            // MovieAnimationItem
            // 
            this.Controls.Add(this.outputBox);
            this.Name = "MovieAnimationItem";
            resources.ApplyResources(this, "$this");
            this.outputBox.ResumeLayout(false);
            this.outputBox.PerformLayout();
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
            // Set canvas
            _canvas = _control.Canvas;
            // Avi
            if (_aviManager != null)
                return;

            // Set AviManager.
            string filename = this.aviFileName.FileName;
            try
            {
                // Delete File.
                if (File.Exists(filename))
                    File.Delete(filename);
                // Create Movie.
                _aviManager = new AviManager(filename, false);
                Bitmap bmp = new Bitmap(_canvas.PCanvas.Camera.ToImage(640, 480, _canvas.BackGroundBrush));
                _stream = _aviManager.AddVideoStream(false, 10, bmp);
            }
            catch (Exception e)
            {
                Util.ShowErrorDialog(MessageResources.ErrCreateAvi + "\n" + e.Message);
                _stream = null;
                _aviManager = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAnimation()
        {
            // write video stream.
            if (_stream != null)
            {
                Bitmap bmp = new Bitmap(
                    _canvas.PCanvas.Camera.ToImage(640, 480, _canvas.BackGroundBrush),
                    _stream.Width,
                    _stream.Height);
                _stream.AddFrame(bmp);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StopAnimation()
        {
            CloseMovie();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetAnimation()
        {
            CloseMovie();
        }

        private void CloseMovie()
        {
            if (_aviManager != null)
            {
                _stream = null;
                _aviManager.Close();
                _aviManager = null;
            }
        }
        #endregion

    }
}
