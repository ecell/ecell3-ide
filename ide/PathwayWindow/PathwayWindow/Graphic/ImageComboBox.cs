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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EcellLib.PathwayWindow.Graphic
{
    class ImageComboBox : ComboBox
    {
        /// <summary>
        /// ImageList
        /// </summary>
        private ImageList m_imageList = new ImageList();

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// ImageList
        /// </summary>
        public ImageList ImageList
        {
            get { return m_imageList; }
            set { m_imageList = value; }
        }

        /// <summary>
        /// OnDrawItem
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            Rectangle bounds = e.Bounds;
            Brush brush = new SolidBrush(e.ForeColor);
            string s = this.Items[e.Index].ToString();
            try
            {
                if (m_imageList.Images.Count >= 0)
                {
                    m_imageList.Draw(e.Graphics, bounds.Left, bounds.Top, e.Index);
                    e.Graphics.DrawString(s, e.Font, brush, bounds.Left + m_imageList.Images[e.Index].Width, bounds.Top);
                }
                else
                {
                    e.Graphics.DrawString(s, e.Font, brush, bounds.Left, bounds.Top);
                }
            }
            catch (Exception)
            {
                e.Graphics.DrawString(s, e.Font, brush, bounds.Left, bounds.Top);
            }
            base.OnDrawItem(e);
        }
    }
}
