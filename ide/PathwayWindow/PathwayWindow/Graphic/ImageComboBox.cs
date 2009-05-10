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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Ecell.IDE.Plugins.PathwayWindow.Graphic
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageComboBox : ComboBox
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private EditBox editBox;
        /// <summary>
        /// ImageList
        /// </summary>
        private ImageList imageList;
        /// <summary>
        /// 
        /// </summary>
        private string text;
        /// <summary>
        /// 
        /// </summary>
        private Image icon = null;
        /// <summary>
        /// 
        /// </summary>
        private Graphics gfx;

        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ImageList ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }

        #endregion

        #region external method from User32.dll
        /// <summary>
        /// 
        /// </summary>
        private const int EM_SETMARGINS = 0xD3;
        private const int EC_LEFTMARGIN = 0x1;
        private const int EC_RIGHTMARGIN = 0x2;

        private const int WM_GETTEXT = 0xd;
        private const int WM_GETTEXTLENGTH = 0xe;
        private const int WM_PAINT = 0xf;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_CHAR = 0x102;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x215;
        /// <summary>
        /// 
        /// </summary>
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// 
        /// </summary>
        private struct ComboBoxInfo
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public IntPtr stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndCombo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);


        /// <summary>
        /// 
        /// </summary>
        internal class EditBox : NativeWindow
        {
            /// <summary>
            /// 
            /// </summary>
            private ImageComboBox Owner;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="owner"></param>
            internal EditBox(ImageComboBox owner)
            {
                this.Owner = owner;
                ComboBoxInfo info = owner.GetComboBoxInfo();
                this.AssignHandle(info.hwndEdit);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                switch (m.Msg)
                {
                    case WM_PAINT:
                    case WM_KEYUP:
                    case WM_KEYDOWN:
                    case WM_CHAR:
                    case WM_LBUTTONDOWN:
                    case WM_LBUTTONUP:
                    case WM_GETTEXT:
                    case WM_GETTEXTLENGTH:
                        Owner.SetText();
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            imageList = new ImageList();

            editBox = new EditBox(this);
            SendMessage(editBox.Handle, EM_SETMARGINS, EC_LEFTMARGIN, 20);
            SendMessage(editBox.Handle, EM_SETMARGINS, EC_RIGHTMARGIN, 1000);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public ImageComboBox(ImageList list)
            : this()
        {
            this.imageList = list;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BrushComboBox
            // 
            this.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.MaxDropDownItems = 10;
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void SetText()
        {
            if (!imageList.Images.ContainsKey(text))
                return;
            icon = imageList.Images[text];

            // Set Text.
            gfx = Graphics.FromHwnd(editBox.Handle);
            gfx.Clear(this.BackColor);
            gfx.DrawImage(icon, 0, 0);
            Brush brush = new SolidBrush(this.ForeColor);
            gfx.DrawString(text, this.Font, brush, icon.Width + 2, 0);
            gfx.Flush();
        }

        private ComboBoxInfo GetComboBoxInfo()
        {
            ComboBoxInfo info = new ComboBoxInfo();
            info.cbSize = Marshal.SizeOf(info);
            GetComboBoxInfo(this.Handle, ref info);
            return info;
        }

        #region Inherited EventHandlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            SetText();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            SetText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            int index = this.SelectedIndex;
            text = this.Text;
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
            Brush textBrush = new SolidBrush(e.ForeColor);
            string imageName = this.Items[e.Index].ToString();
            try
            {
                if (imageList.Images.Count >= 0 && imageList.Images.ContainsKey(imageName))
                {
                    Image image = imageList.Images[imageName];
                    e.Graphics.DrawImage(image, bounds.Left, bounds.Top);
                    e.Graphics.DrawString(imageName, e.Font, textBrush, bounds.Left + image.Width, bounds.Top);
                }
                else
                {
                    e.Graphics.DrawString(imageName, e.Font, textBrush, bounds.Left, bounds.Top);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                e.Graphics.DrawString(imageName, e.Font, textBrush, bounds.Left, bounds.Top);
            }
            base.OnDrawItem(e);
        }

        #endregion
    }
}
