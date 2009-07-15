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
        private ImageComboBox figureComboBox;

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
            get { return figureComboBox.Text; }
            set
            {
                figureComboBox.Text = value;
                RaiseFigureChange();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new bool Enabled
        {
            get { return figureComboBox.Enabled; }
            set
            {
                figureComboBox.Enabled = value;
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
            this.figureComboBox.ImageList = FigureManager.FigureIcons;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="figure"></param>
        public FigureComboBox(string figure)
            :this()
        {
            this.figureComboBox.Text = figure;
        }

        private void InitializeComponent()
        {
            // set Brushes
            this.figureComboBox = new ImageComboBox();
            this.SuspendLayout();
            this.Controls.Add(this.figureComboBox);
            // 
            // comboBoxBrush
            // 
            this.figureComboBox.Dock = DockStyle.Fill;
            this.figureComboBox.FormattingEnabled = true;
            this.figureComboBox.Location = new Point(1, 1);
            this.figureComboBox.Size = new Size(100, 20);
            this.figureComboBox.TabIndex = 0;
            this.figureComboBox.Text = "Rectangle";
            this.figureComboBox.Items.AddRange(FigureManager.GetFigureList().ToArray());
            this.figureComboBox.SelectedIndexChanged += new EventHandler(figureComboBox_SelectedIndexChanged);
            //
            //
            //
            this.Size = new Size(105, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Get ComboBox.
        /// </summary>
        /// 
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ImageComboBox ComboBox
        {
            get { return figureComboBox; }
        }

        /// <summary>
        /// Get ComboBox.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ReadOnly
        {
            get { return (figureComboBox.DropDownStyle == ComboBoxStyle.DropDownList); }
            set
            {
                if (value)
                    this.figureComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                else
                    this.figureComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        /// <summary>
        /// Event on text changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void figureComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RaiseTextChange();
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
