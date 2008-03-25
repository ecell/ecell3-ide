using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Event;
using System.Drawing;

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// 
    /// </summary>
    public class PPathwayText : PText
    {
        private CanvasControl m_canvas;
        private TextBox m_tbox = new TextBox();

        /// <summary>
        /// PointF
        /// </summary>
        public PointF PointF
        {
            get { return new PointF(this.X,this.Y);}
            set
            { 
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PPathwayText(CanvasControl canvas)
        {
            this.m_canvas = canvas;
            base.Brush = Brushes.Beige;
            base.Text = "Text";
            base.AddInputEventListener(new ObjectDragHandler());
            this.m_tbox.LostFocus += new EventHandler(m_tbox_LostFocus);
            this.m_tbox.KeyPress += new KeyPressEventHandler(m_tbox_KeyPress);
            this.m_tbox.Multiline = true;
        }

        /// <summary>
        /// Called when the mouse leaves this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_canvas == null)
                return;
            m_canvas.FocusNode = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnDoubleClick(PInputEventArgs e)
        {
            base.OnDoubleClick(e);
            PointF pos = new PointF(base.X + base.OffsetX, base.Y + base.OffsetY);
            m_canvas.PathwayCanvas.Controls.Add(m_tbox);
            m_tbox.Text = base.Text;
            m_tbox.Location = m_canvas.CanvasPosToSystemPos(pos);
            m_tbox.Width = (int)base.Width;
            m_tbox.Height = (int)base.Height;
            m_tbox.Focus();
        }

        void m_tbox_LostFocus(object sender, EventArgs e)
        {
            SetText();
        }

        void m_tbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
                SetText();
        }

        private void SetText()
        {
            m_canvas.PathwayCanvas.Controls.Remove(m_tbox);
            base.Text = m_tbox.Text;
            if (string.IsNullOrEmpty(base.Text))
                m_canvas.RemoveText(this);
        }

    }
}
