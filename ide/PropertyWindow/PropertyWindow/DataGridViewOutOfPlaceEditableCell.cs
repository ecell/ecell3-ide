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
//

using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Ecell.IDE.Plugins.PropertyWindow
{
    public delegate bool DataGridViewOutOfPlaceEditRequestedHandler(DataGridViewOutOfPlaceEditableCell c);

    public class DataGridViewOutOfPlaceEditableCell : DataGridViewTextBoxCell
    {
        const string s_threeDots = "...";
        Size m_buttonSize;
        bool m_pressed;
        bool m_mouseOverButton;
        bool m_isInOutOfPlaceEditMode;

        public DataGridViewOutOfPlaceEditRequestedHandler OnOutOfPlaceEditRequested;

        public DataGridViewOutOfPlaceEditableCell()
        {
            m_buttonSize = new Size(-1, -1);
            m_pressed = false;
            m_mouseOverButton = false;
            m_isInOutOfPlaceEditMode = false;
        }

        public override object Clone()
        {
            object o = base.Clone();
            ((DataGridViewOutOfPlaceEditableCell)o).m_buttonSize = m_buttonSize;
            ((DataGridViewOutOfPlaceEditableCell)o).m_pressed = m_pressed;
            return o;
        }

        public bool IsInOutOfPlaceEditMode
        {
            get { return m_isInOutOfPlaceEditMode; }
            protected set { m_isInOutOfPlaceEditMode = value; }
        }

        private bool IsPointInButtonArea(int x, int y)
        {
            Rectangle bounds = DataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);
            return x >= bounds.Width - m_buttonSize.Width;
        }

        protected override void OnContentDoubleClick(DataGridViewCellEventArgs e)
        {
            DataGridView.BeginEdit(true);
        }

        private void RenderButtonUnpressed()
        {
            m_pressed = false;
            DataGridView.InvalidateCell(this);
        }

        private void RenderButtonPressed()
        {
            m_pressed = true;
            DataGridView.InvalidateCell(this);
        }

        protected override bool ClickUnsharesRow(DataGridViewCellEventArgs e)
        {
            return true;
        }

        protected void OnButtonPressed()
        {
            RenderButtonPressed();
            RaiseCellClick(new DataGridViewCellEventArgs(ColumnIndex, RowIndex));

            if (OnOutOfPlaceEditRequested != null)
            {
                IsInOutOfPlaceEditMode = true;
                bool retval = OnOutOfPlaceEditRequested(this);
                IsInOutOfPlaceEditMode = false;
                RenderButtonUnpressed();
                if (retval)
                    DataGridView.UpdateCellValue(ColumnIndex, RowIndex);
            }
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                if (m_pressed && !IsInOutOfPlaceEditMode)
                    RenderButtonUnpressed();
            }
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                if (IsPointInButtonArea(e.X, e.Y))
                {
                    OnButtonPressed();
                }
                else
                {
                    RaiseCellContentClick(new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
                }
            }
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            if (IsPointInButtonArea(e.X, e.Y))
            {
                if (!m_mouseOverButton)
                {
                    m_mouseOverButton = true;
                    DataGridView.InvalidateCell(this);
                }
            }
            else
            {
                if (m_mouseOverButton)
                {
                    m_mouseOverButton = false;
                    DataGridView.InvalidateCell(this);
                }
            }
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            if (m_mouseOverButton)
            {
                m_mouseOverButton = false;
                DataGridView.InvalidateCell(this);
            }
        }

        protected override void OnMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                if (!IsPointInButtonArea(e.X, e.Y))
                {
                    RaiseCellContentDoubleClick(new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
                }
            }
        }

        public override object DefaultNewRowValue
        {
            get { return ""; }
        }

        public override Rectangle PositionEditingPanel(Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
        {
            cellBounds.Width -= m_buttonSize.Width;
            return base.PositionEditingPanel(
                cellBounds, cellClip, cellStyle, singleVerticalBorderAdded,
                singleHorizontalBorderAdded, isFirstDisplayedColumn,
                isFirstDisplayedColumn);
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Size buttonSize = graphics.MeasureString(s_threeDots, cellStyle.Font).ToSize();
            buttonSize.Width += 8; // margin
            m_buttonSize = buttonSize;

            if ((paintParts & DataGridViewPaintParts.Background) != 0)
            {
                Brush b = new SolidBrush(Selected ? cellStyle.SelectionBackColor: cellStyle.BackColor);
                using (b) graphics.FillRectangle(b, cellBounds);
            }

            Rectangle textBounds = new Rectangle(
                cellBounds.X, cellBounds.Y, cellBounds.Width - m_buttonSize.Width, cellBounds.Height);
            Rectangle buttonBounds = new Rectangle(
                cellBounds.Right - m_buttonSize.Width, cellBounds.Top, m_buttonSize.Width, cellBounds.Height);
                        
            if ((paintParts & DataGridViewPaintParts.ContentForeground) != 0)
            {
                Brush b = new SolidBrush(Selected ? cellStyle.SelectionForeColor: cellStyle.ForeColor);
                using (b)
                {
                    StringFormat sf = new StringFormat();
                    switch (cellStyle.Alignment)
                    {
                        case DataGridViewContentAlignment.BottomCenter:
                            sf.LineAlignment = StringAlignment.Far;
                            sf.Alignment = StringAlignment.Center;
                            break;
                        case DataGridViewContentAlignment.BottomLeft:
                            sf.LineAlignment = StringAlignment.Far;
                            sf.Alignment = StringAlignment.Near;
                            break;
                        case DataGridViewContentAlignment.BottomRight:
                            sf.LineAlignment = StringAlignment.Far;
                            sf.Alignment = StringAlignment.Far;
                            break;
                        case DataGridViewContentAlignment.MiddleCenter:
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;
                            break;
                        case DataGridViewContentAlignment.MiddleLeft:
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Near;
                            break;
                        case DataGridViewContentAlignment.MiddleRight:
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Far;
                            break;
                        case DataGridViewContentAlignment.TopCenter:
                            sf.LineAlignment = StringAlignment.Near;
                            sf.Alignment = StringAlignment.Center;
                            break;
                        case DataGridViewContentAlignment.TopLeft:
                            sf.LineAlignment = StringAlignment.Near;
                            sf.Alignment = StringAlignment.Near;
                            break;
                        case DataGridViewContentAlignment.TopRight:
                            sf.LineAlignment = StringAlignment.Near;
                            sf.Alignment = StringAlignment.Far;
                            break;
                        case DataGridViewContentAlignment.NotSet:
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Near;
                            break;
                    }
                    sf.Trimming = StringTrimming.None;
                    graphics.DrawString((string)Value, cellStyle.Font, b, textBounds, sf);
                }
                ButtonRenderer.DrawButton(graphics,
                    buttonBounds,
                    s_threeDots, cellStyle.Font, false,
                    this.ReadOnly ? PushButtonState.Disabled :
                    (m_pressed ? PushButtonState.Pressed:
                        (m_mouseOverButton ? PushButtonState.Hot: PushButtonState.Normal)));
            }
            if ((paintParts & DataGridViewPaintParts.Focus) != 0)
            {
                if (Selected)
                    ControlPaint.DrawFocusRectangle(graphics, textBounds);
            }
            if ((paintParts & DataGridViewPaintParts.Border) != 0)
            {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            }
        }
    }
}