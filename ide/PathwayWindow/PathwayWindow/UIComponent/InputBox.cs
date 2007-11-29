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
// written by by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.PathwayWindow.UIComponent
{
    public partial class InputBox : Form
    {
        #region Accessor
        /// <summary>
        ///  get/set message text.
        /// </summary>
        public string Message
        {
            get { return this.message.Text; }
            set { this.message.Text = value; }
        }
        /// <summary>
        ///  get/set message text.
        /// </summary>
        public string Input
        {
            get { return this.inputText.Text; }
            set { this.inputText.Text = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public InputBox()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InputBox(string message)
        {
            InitializeComponent();
            this.Message = message;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public InputBox(string message, string title)
        {
            InitializeComponent();
            this.Message = message;
            this.Text = title;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="defaultAns"></param>
        public InputBox(string message, string title, string defaultAns)
        {
            InitializeComponent();
            this.Message = message;
            this.Text = title;
            this.Input = defaultAns;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Show InputBoxDialog.
        /// </summary>
        /// <returns></returns>
        public new DialogResult Show()
        {
            this.ShowDialog();
            return this.DialogResult;
        }
        #endregion
    }
}