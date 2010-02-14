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
//

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.Drawing;
using AviFile;
using System.IO;
using System.Security.AccessControl;
using System.Xml;

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
        /// <summary>
        /// default filename.
        /// </summary>
        private const string MovieFile = "ecell-ide.avi";
        /// <summary>
        /// AVI output manager.
        /// </summary>
        private AviManager _aviManager = null;
        /// <summary>
        /// movie stream
        /// </summary>
        private VideoStream _stream = null;
        /// <summary>
        /// 
        /// </summary>
        private bool _isNoLimit = true;
        /// <summary>
        /// is limit.
        /// </summary>
        bool _isLimit = false;
        /// <summary>
        /// Max size of output movie file.
        /// </summary>
        private double _maxSize = 300000;
        private System.Windows.Forms.TextBox maxSizeTextBox;
        private System.Windows.Forms.RadioButton maxSizeRadio;
        private System.Windows.Forms.Label sizeDetailLabel;
        private System.Windows.Forms.RadioButton noLimitRadio;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public MovieAnimationItem()
        {
            InitializeComponent();
            this.aviFileName.Filter = Constants.FilterAviFile;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public MovieAnimationItem(AnimationControl control)
            : base(control)
        {
            InitializeComponent();
            string movieFile = MovieFile;
            if (control.Canvas != null)
                movieFile = control.Canvas.ModelID + ".avi";
            this.aviFileName.FileName = Path.Combine(Util.GetBaseDir(), MovieFile);
            this.aviFileName.Filter = Constants.FilterAviFile;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MovieAnimationItem));
            this.outputBox = new System.Windows.Forms.GroupBox();
            this.sizeDetailLabel = new System.Windows.Forms.Label();
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
            this.outputBox.Controls.Add(this.sizeDetailLabel);
            this.outputBox.Controls.Add(label2);
            this.outputBox.Controls.Add(this.maxSizeTextBox);
            this.outputBox.Controls.Add(this.maxSizeRadio);
            this.outputBox.Controls.Add(this.noLimitRadio);
            this.outputBox.Controls.Add(this.label1);
            this.outputBox.Controls.Add(this.aviFileName);
            this.outputBox.Name = "outputBox";
            this.outputBox.TabStop = false;
            // 
            // sizeDetailLabel
            // 
            resources.ApplyResources(this.sizeDetailLabel, "sizeDetailLabel");
            this.sizeDetailLabel.Name = "sizeDetailLabel";
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
            this.aviFileName.FileName = "ecell-ide.avi";
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
        /// <param name="doc"></param>
        /// <returns></returns>
        public override System.Xml.XmlElement GetAnimationStatus(System.Xml.XmlDocument doc)
        {
            XmlElement status = doc.CreateElement("MovieAnimationItem");
            status.SetAttribute("Filename", aviFileName.FileName);
            status.SetAttribute("NoLimitCheck", noLimitRadio.Checked.ToString());
            status.SetAttribute("MaxSizeCheck", maxSizeRadio.Checked.ToString());
            status.SetAttribute("MaxSize", maxSizeTextBox.Text);
            return status;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public override void SetAnimationStatus(System.Xml.XmlElement status)
        {
            aviFileName.FileName = status.GetAttribute("Filename");
            noLimitRadio.Checked = bool.Parse(status.GetAttribute("NoLimitCheck"));
            maxSizeRadio.Checked = bool.Parse(status.GetAttribute("MaxSizeCheck"));
            maxSizeTextBox.Text = status.GetAttribute("MaxSize");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ApplyChange()
        {
            base.ApplyChange();
            // SizeLimit
            _isNoLimit = noLimitRadio.Checked;
            // Size
            int maxSize;
            if(int.TryParse(maxSizeTextBox.Text, out maxSize))
                _maxSize = maxSize;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CheckParameters()
        {
            base.CheckParameters();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetAnimation()
        {
            // Set canvas
            _canvas = _control.Canvas;
            // Avi
            if (_aviManager != null || _isLimit || string.IsNullOrEmpty(this.aviFileName.FileName))
                return;

            // Set AviManager.
            string filename = this.aviFileName.FileName;
            try
            {
                // Delete File.
                if (File.Exists(filename))
                    File.Delete(filename);
                // Create dir.
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                // Create Movie.
                _aviManager = new AviManager(filename, false);
                Bitmap bmp = new Bitmap(_canvas.PCanvas.Camera.ToImage(640, 480, _canvas.BackGroundBrush));
                _stream = _aviManager.AddVideoStream(false, 10, bmp);
            }
            catch (Exception e)
            {
                Util.ShowErrorDialog(MessageResources.ErrCreateAvi + "\n" + e.Message);
                this.aviFileName.FileName = "";
                _stream = null;
                _aviManager = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAnimation()
        {
            // Check status.
            if (_stream == null || _isLimit)
                return;
            if (_control.Control.ProjectStatus == ProjectStatus.Suspended)
                return;

            // write video stream.
            Bitmap bmp = new Bitmap(
                _canvas.PCanvas.Camera.ToImage(640, 480, _canvas.BackGroundBrush),
                _stream.Width,
                _stream.Height);
            _stream.AddFrame(bmp);
            if (!_isNoLimit)
            {
                System.IO.FileInfo f = new FileInfo(this.aviFileName.FileName);
                if (f.Length > _maxSize * 1000)
                {
                    _isLimit = true;
                    Util.ShowErrorDialog(MessageResources.ErrMaxSize);
                    CloseMovie();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StopAnimation()
        {
            CloseMovie();
            _isLimit = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetAnimation()
        {
            CloseMovie();
            _isLimit = false;
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
            _isNoLimit = noLimitRadio.Checked;
            if (_isNoLimit)
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
                maxSizeTextBox.Text = Convert.ToString(_maxSize);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy) || dummy <= 0)
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, maxSizeRadio.Text));
                maxSizeTextBox.Text = Convert.ToString(_maxSize);
                e.Cancel = true;
                return;
            }
        }

    }
}
