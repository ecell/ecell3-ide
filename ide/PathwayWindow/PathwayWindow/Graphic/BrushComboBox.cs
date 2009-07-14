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
using System.Drawing;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.PathwayWindow.Graphic
{
    /// <summary>
    /// 
    /// </summary>
    public class BrushComboBox : UserControl
    {
                private ImageComboBox comboBoxBrush;
        private Brush brush;
        private System.ComponentModel.IContainer components;

        #region EventHandler for BrushChange
        private EventHandler m_onBrushChange;
        /// <summary>
        /// Event on brush change.
        /// </summary>
        public event EventHandler BrushChange
        {
            add { m_onBrushChange += value; }
            remove { m_onBrushChange -= value; }
        }
        /// <summary>
        /// Event on brush change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBrushChange(EventArgs e)
        {
            if (m_onBrushChange != null)
                m_onBrushChange(this, e);
        }
        private void RaiseBrushChange()
        {
            EventArgs e = new EventArgs();
            OnBrushChange(e);
        }
        #endregion

        #region Accesor
        /// <summary>
        /// Get/Set m_brush.
        /// </summary>
        public Brush Brush
        {
            get { return brush; }
            set
            { 
                brush = value;
                comboBoxBrush.Text = BrushManager.ParseBrushToString(brush);
                RaiseBrushChange();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new bool Enabled
        {
            get { return comboBoxBrush.Enabled; }
            set 
            {
                comboBoxBrush.Enabled = value;
                base.Enabled = value;
            }
        }
        
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public BrushComboBox()
        {
            InitializeComponent();
            this.comboBoxBrush.ImageList = BrushManager.GetBrushImageList();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="brush"></param>
        public BrushComboBox(Brush brush)
            :this()
        {
            this.Brush = brush;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBoxBrush = new Ecell.IDE.Plugins.PathwayWindow.Graphic.ImageComboBox();
            this.SuspendLayout();

            // 
            // m_comboBoxBrush
            // 
            this.comboBoxBrush.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxBrush.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxBrush.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxBrush.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxBrush.FormattingEnabled = true;
            this.comboBoxBrush.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
            this.comboBoxBrush.Location = new System.Drawing.Point(0, 0);
            this.comboBoxBrush.MaxDropDownItems = 10;
            this.comboBoxBrush.Name = "comboBoxBrush";
            this.comboBoxBrush.Size = new System.Drawing.Size(158, 20);
            this.comboBoxBrush.TabIndex = 0;
            this.comboBoxBrush.Text = "Black";
            this.comboBoxBrush.SelectedIndexChanged += new System.EventHandler(this.cBoxBrush_SelectedIndexChanged);
            this.comboBoxBrush.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cBoxNomalBrush_KeyDown);
            // 
            // BrushComboBox
            // 
            this.Controls.Add(this.comboBoxBrush);
            this.Name = "BrushComboBox";
            this.Size = new System.Drawing.Size(158, 21);
            this.ResumeLayout(false);

        }

        void cBoxBrush_SelectedIndexChanged(object sender, EventArgs e)
        {
            string brushName = ((ComboBox)sender).Text;
            SetBrush(brushName);
        }

        void cBoxNomalBrush_KeyDown(object sender, KeyEventArgs e)
        {
            if(!(e.KeyCode == Keys.Enter))
                return;
            string brushName = ((ComboBox)sender).Text;
            SetBrush(brushName);
        }

        private void SetBrush(string brushName)
        {
            Brush brush = BrushManager.ParseStringToBrush(brushName);
            if (brush == null)
                brush = Brushes.Transparent;
            this.Brush = brush;
        }
    }
}
