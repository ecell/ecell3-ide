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
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using System.Drawing;
using System.ComponentModel;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// Combobox to select figures.
    /// </summary>
    public class FigureComboBox : UserControl
    {
        private ImageComboBox imageComboBox;
        private IContainer components;

        #region EventHandler for FigureChange
        private EventHandler m_onFigureChange;
        /// <summary>
        /// Event on brush change.
        /// </summary>
        public event EventHandler FigureChange
        {
            add { m_onFigureChange += value; }
            remove { m_onFigureChange -= value; }
        }
        /// <summary>
        /// Event on brush change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFigureChange(EventArgs e)
        {
            if (m_onFigureChange != null)
                m_onFigureChange(this, e);
        }
        private void RaiseFigureChange()
        {
            EventArgs e = new EventArgs();
            OnFigureChange(e);
        }
        #endregion

        #region Accesor
        /// <summary>
        /// Get/Set m_brush.
        /// </summary>
        public string Figure
        {
            get { return imageComboBox.Text; }
            set
            {
                imageComboBox.Text = value;
                RaiseFigureChange();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new bool Enabled
        {
            get { return imageComboBox.Enabled; }
            set
            {
                imageComboBox.Enabled = value;
                base.Enabled = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public FigureComboBox()
        {
            InitializeComponent();
            this.imageComboBox.ImageList = FigureManager.FigureIcons;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="figure"></param>
        public FigureComboBox(string figure)
            :this()
        {
            this.Figure = figure;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageComboBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.ImageComboBox();
            this.SuspendLayout();
            // 
            // imageComboBox
            // 
            this.imageComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.imageComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.imageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.imageComboBox.FormattingEnabled = true;
            this.imageComboBox.Items.AddRange(new object[] {
            "Ellipse",
            "Rectangle",
            "Diamond",
            "Triangle",
            "RoundedRectangle",
            "SystemRectangle",
            "SystemEllipse"});
            this.imageComboBox.Location = new System.Drawing.Point(0, 0);
            this.imageComboBox.MaxDropDownItems = 10;
            this.imageComboBox.Name = "imageComboBox";
            this.imageComboBox.Size = new System.Drawing.Size(146, 20);
            this.imageComboBox.TabIndex = 0;
            this.imageComboBox.Text = "Rectangle";
            this.imageComboBox.SelectedIndexChanged += new System.EventHandler(this.figureComboBox_SelectedIndexChanged);
            this.imageComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.figureComboBox_KeyDown);
            // 
            // FigureComboBox
            // 
            this.Controls.Add(this.imageComboBox);
            this.Name = "FigureComboBox";
            this.Size = new System.Drawing.Size(146, 20);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Get ComboBox.
        /// </summary>
        /// 
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ImageComboBox ComboBox
        {
            get { return imageComboBox; }
        }

        /// <summary>
        /// Event on text changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void figureComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RaiseTextChange();
            string figure = ((ComboBox)sender).Text;
            this.Figure = figure;
        }

        void figureComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.KeyCode == Keys.Enter))
                return;
            string figure = ((ComboBox)sender).Text;
            this.Figure = figure;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void SetItemType(string type)
        {
            imageComboBox.Items.Clear();
            imageComboBox.Items.AddRange(FigureManager.GetFigureList(type).ToArray());

        }

        #region EventHandler for ComboBoxChange
        private EventHandler m_onTextChange;
        /// <summary>
        /// Event on text change.
        /// </summary>
        public event EventHandler TextChange
        {
            add { m_onTextChange += value; }
            remove { m_onTextChange -= value; }
        }
        /// <summary>
        /// Event on text change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChange(EventArgs e)
        {
            if (m_onTextChange != null)
                m_onTextChange(this, e);
        }
        /// <summary>
        /// Event on text change.
        /// </summary>
        private void RaiseTextChange()
        {
            EventArgs e = new EventArgs();
            OnTextChange(e);
        }
        #endregion
    }
}
