using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EcellLib.PathwayWindow.UIComponent
{

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public partial class PropertyDialogItem : Panel
    {
        protected Label m_label;
        protected Point m_position = new Point(150, 5);
        protected Size m_size = new Size(128, 20);

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyDialogItem()
        {
            Initialize("");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        public PropertyDialogItem(string label)
        {
            Initialize(label);
        }

        private void Initialize(string label)
        {

            this.m_label = new Label();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left);
            this.AutoSize = true;
            this.Controls.Add(this.m_label);
            this.TabStop = false;
            this.Height = 25;
            // 
            // m_label
            // 
            this.m_label.AutoSize = true;
            this.m_label.Location = new System.Drawing.Point(10, 9);
            this.m_label.Size = new System.Drawing.Size(35, 12);
            this.m_label.Text = label;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyBrushItem : PropertyDialogItem
    {
        private ComboBox m_comboBoxBrush;
        private Brush m_brush;

        public Brush Brush
        {
            get { return m_brush; }
            set { m_brush = value; }
        }

        public PropertyBrushItem(string label, Brush brush, List<string> brushList)
        {
            // set Brushes
            m_brush = brush;
            this.m_label.Text = label;

            this.m_comboBoxBrush = new ComboBox();
            this.SuspendLayout();
            this.Controls.Add(this.m_comboBoxBrush);

            // 
            // m_comboBoxBrush
            // 
            this.m_comboBoxBrush.FormattingEnabled = true;
            this.m_comboBoxBrush.Location = m_position;
            this.m_comboBoxBrush.Size = m_size;
            this.m_comboBoxBrush.TabIndex = 0;
            this.m_comboBoxBrush.Text = BrushManager.ParseBrushToString(m_brush);
            this.m_comboBoxBrush.Items.AddRange(brushList.ToArray());
            this.m_comboBoxBrush.TextChanged += new EventHandler(cBoxNomalBrush_TextChanged);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        void cBoxNomalBrush_TextChanged(object sender, EventArgs e)
        {
            ComboBox cBox = (ComboBox)sender;
            Brush brush = BrushManager.ParseStringToBrush(cBox.Text);
            m_brush = brush;
        }
    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyLabelItem : PropertyDialogItem
    {
        private Label m_text;

        public override string Text
        {
            get { return m_text.Text; }
            set { m_text.Text = value; }
        }

        public PropertyLabelItem(string label, string text)
        {
            this.m_label.Text = label;

            this.m_text = new Label();
            this.SuspendLayout();
            this.Controls.Add(this.m_text);
            // 
            // m_textBox
            // 
            this.m_text.Location = m_position;
            this.m_text.Size = m_size;
            this.m_text.TabIndex = 0;
            this.m_text.Text = text;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyTextItem : PropertyDialogItem
    {
        private TextBox m_textBox;

        public override string Text
        {
            get { return m_textBox.Text; }
            set { m_textBox.Text = value; }
        }

        public PropertyTextItem(string label, string text)
        {
            this.m_label.Text = label;

            this.m_textBox = new TextBox();
            this.SuspendLayout();
            this.Controls.Add(this.m_textBox);
            // 
            // m_textBox
            // 
            this.m_textBox.Location = m_position;
            this.m_textBox.Size = m_size;
            this.m_textBox.TabIndex = 0;
            this.m_textBox.Text = text;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyCheckBoxItem : PropertyDialogItem
    {
        private CheckBox m_checkBox;

        public bool Checked
        {
            get { return m_checkBox.Checked; }
            set { m_checkBox.Checked = value; }
        }

        public PropertyCheckBoxItem(string label, bool isChecked)
        {
            this.m_label.Text = label;

            this.m_checkBox = new CheckBox();
            this.SuspendLayout();
            this.Controls.Add(this.m_checkBox);
            // 
            // m_checkBox
            // 
            this.m_checkBox.Location = m_position;
            this.m_checkBox.Size = m_size;
            this.m_checkBox.TabIndex = 0;
            this.m_checkBox.Checked = isChecked;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
