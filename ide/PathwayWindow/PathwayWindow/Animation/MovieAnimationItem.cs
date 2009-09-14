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
        private bool m_isNoLimit = true;
        private System.Windows.Forms.TextBox maxSizeTextBox;
        private System.Windows.Forms.RadioButton maxSizeRadio;
        private System.Windows.Forms.RadioButton noLimitRadio;
        private double m_MaxSize = 300000;
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
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MovieAnimationItem));
            this.outputBox = new System.Windows.Forms.GroupBox();
            this.maxSizeTextBox = new System.Windows.Forms.TextBox();
            this.maxSizeRadio = new System.Windows.Forms.RadioButton();
            this.noLimitRadio = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.aviFileName = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertySaveFileItem();
            label2 = new System.Windows.Forms.Label();
            this.outputBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // outputBox
            // 
            resources.ApplyResources(this.outputBox, "outputBox");
            this.outputBox.Controls.Add(label2);
            this.outputBox.Controls.Add(this.maxSizeTextBox);
            this.outputBox.Controls.Add(this.maxSizeRadio);
            this.outputBox.Controls.Add(this.noLimitRadio);
            this.outputBox.Controls.Add(this.label1);
            this.outputBox.Controls.Add(this.aviFileName);
            this.outputBox.Name = "outputBox";
            this.outputBox.TabStop = false;
            // 
            // maxSizeTextBox
            // 
            resources.ApplyResources(this.maxSizeTextBox, "maxSizeTextBox");
            this.maxSizeTextBox.Name = "maxSizeTextBox";
            this.maxSizeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.maxSizeTextBox_Validating);
            // 
            // maxSizeRadio
            // 
            resources.ApplyResources(this.maxSizeRadio, "maxSizeRadio");
            this.maxSizeRadio.Name = "maxSizeRadio";
            this.maxSizeRadio.TabStop = true;
            this.maxSizeRadio.UseVisualStyleBackColor = true;
            // 
            // noLimitRadio
            // 
            resources.ApplyResources(this.noLimitRadio, "noLimitRadio");
            this.noLimitRadio.Checked = true;
            this.noLimitRadio.Name = "noLimitRadio";
            this.noLimitRadio.TabStop = true;
            this.noLimitRadio.UseVisualStyleBackColor = true;
            this.noLimitRadio.CheckedChanged += new System.EventHandler(this.noLimitRadio_CheckedChanged);
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
                if (!m_isNoLimit)
                {
                    System.IO.FileInfo f = new FileInfo(this.aviFileName.FileName);
                    if (f.Length > m_MaxSize * 1000)
                    {
                        Util.ShowErrorDialog(MessageResources.ErrMaxSize);
                        CloseMovie();
                    }
                }
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

        private void noLimitRadio_CheckedChanged(object sender, EventArgs e)
        {
            m_isNoLimit = noLimitRadio.Checked;
            if (m_isNoLimit)
            {
                maxSizeTextBox.Enabled = false;
            }
            else
            {
                maxSizeTextBox.Enabled = true;
            }
        }

        private void maxSizeTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = maxSizeTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, maxSizeRadio.Text));
                maxSizeTextBox.Text = Convert.ToString(m_MaxSize);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!double.TryParse(text, out dummy) || dummy <= 0)
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, maxSizeRadio.Text));
                maxSizeTextBox.Text = Convert.ToString(m_MaxSize);
                e.Cancel = true;
                return;
            }
        }

    }
}
