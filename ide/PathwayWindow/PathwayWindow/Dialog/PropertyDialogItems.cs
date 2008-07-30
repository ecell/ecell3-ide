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
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;

namespace Ecell.IDE.Plugins.PathwayWindow.Dialog
{
    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public partial class PropertyDialogItem : Panel
    {
        /// <summary>
        /// label to explain the DialogItem.
        /// </summary>
        protected Label m_label;
        
        /// <summary>
        /// position of itembox.
        /// </summary>
        protected Point m_position = new Point(120, 5);
        
        /// <summary>
        /// size of the itembox.
        /// </summary>
        protected Size m_size = new Size(100, 20);

        /// <summary>
        /// Accessor for m_label.
        /// </summary>
        public Label Label
        {
            get { return m_label; }
            set { m_label = value; }
        }

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
    public class PropertyComboboxItem : PropertyDialogItem
    {
        private ComboBox m_comboBox;

        /// <summary>
        /// Get ComboBox.
        /// </summary>
        public ComboBox ComboBox
        {
            get {return m_comboBox;}
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
        /// <param name="itemList"></param>
        public PropertyComboboxItem(string label, string text, List<string> itemList)
        {
            // set Brushes
            this.m_label.Text = label;

            this.m_comboBox = new ComboBox();
            this.SuspendLayout();
            this.Controls.Add(this.m_comboBox);

            // 
            // m_comboBoxBrush
            // 
            this.m_comboBox.FormattingEnabled = true;
            this.m_comboBox.Location = m_position;
            this.m_comboBox.Size = m_size;
            this.m_comboBox.TabIndex = 0;
            this.m_comboBox.Text = text;
            this.m_comboBox.Items.AddRange(itemList.ToArray());
            this.m_comboBox.TextChanged += new EventHandler(ComboBox_TextChanged);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Event on text changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ComboBox_TextChanged(object sender, EventArgs e)
        {
            RaiseTextChange();
        }

    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyBrushItem : PropertyDialogItem
    {
        private ImageComboBox m_comboBoxBrush;
        private Brush m_brush;

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
            get { return m_brush; }
            set
            { 
                m_brush = value;
                m_comboBoxBrush.Text = BrushManager.ParseBrushToString(m_brush);
                RaiseBrushChange();
            }
        }

        /// <summary>
        /// Get ComboBox.
        /// </summary>
        public ComboBox ComboBox
        {
            get { return m_comboBoxBrush; }
        }
        
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="brush"></param>
        public PropertyBrushItem(string label, Brush brush)
        {
            // set Brushes
            this.m_brush = brush;
            this.m_label.Text = label;

            this.m_comboBoxBrush = new ImageComboBox();
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
            this.m_comboBoxBrush.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
            this.m_comboBoxBrush.ImageList = BrushManager.GetBrushImageList();
            this.m_comboBoxBrush.KeyDown += new KeyEventHandler(cBoxNomalBrush_KeyDown);
            this.m_comboBoxBrush.SelectedIndexChanged += new EventHandler(cBoxBrush_SelectedIndexChanged);

            this.ResumeLayout(false);
            this.PerformLayout();
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

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyLabelItem : PropertyDialogItem
    {
        private Label m_text;

        /// <summary>
        /// Get/Set m_text.Text
        /// </summary>
        public override string Text
        {
            get { return m_text.Text; }
            set { m_text.Text = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
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
        /// <summary>
        /// Get/Set m_textBox.Text
        /// </summary>
        public override string Text
        {
            get { return m_textBox.Text; }
            set { m_textBox.Text = value; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
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
    public class PropertyFileItem : PropertyDialogItem
    {
        private TextBox m_textBox;
        private Button m_button;
        private string m_filter;
        private int m_filterIndex;


        /// <summary>
        /// Get/Set m_textBox.Text
        /// </summary>
        public string FileName
        {
            get { return m_textBox.Text; }
            set { m_textBox.Text = value; }
        }
        /// <summary>
        /// Filter
        /// </summary>
        public string Filter
        {
            get { return m_filter; }
            set { m_filter = value; }
        }
        /// <summary>
        /// FilterIndex
        /// </summary>
        public int FilterIndex
        {
            get { return m_filterIndex; }
            set { m_filterIndex = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="filename"></param>
        public PropertyFileItem(string label, string filename)
        {
            this.m_label.Text = label;

            // Create New Object.
            this.m_textBox = new TextBox();
            this.m_button = new Button();

            this.SuspendLayout();
            this.Controls.Add(this.m_button);
            this.Controls.Add(this.m_textBox);
            // 
            this.m_textBox.Location = m_position;
            this.m_textBox.Size = m_size;
            this.m_textBox.TabIndex = 0;
            this.m_textBox.Text = filename;

            this.m_button.Text = "...";
            this.m_button.Top = m_textBox.Top;
            this.m_button.Left = m_textBox.Left + m_textBox.Width;
            this.m_button.Height = m_size.Height;
            this.m_button.Width = m_size.Height;
            this.m_button.Click += new EventHandler(m_button_Click);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        void m_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            using (fileDialog)
            {
                fileDialog.Filter = m_filter;
                fileDialog.FilterIndex = m_filterIndex;
                fileDialog.FileName = m_textBox.Text;
                DialogResult result = fileDialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                m_textBox.Text = fileDialog.FileName;
            }
        }
    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyCheckBoxItem : PropertyDialogItem
    {
        private CheckBox m_checkBox;

        /// <summary>
        /// Get/Set m_checkBox.
        /// </summary>
        public CheckBox CheckBox
        {
            get { return m_checkBox; }
            set { m_checkBox = value; }
        }

        /// <summary>
        /// Get/Set m_checkBox.Checked
        /// </summary>
        public bool Checked
        {
            get { return m_checkBox.Checked; }
            set 
            { 
                m_checkBox.Checked = value;
                RaiseCheckedChanged();
            }
        }

        #region EventHandler for CheckedChanged
        private EventHandler m_onCheckedChanged;
        /// <summary>
        /// Event on checked change.
        /// </summary>
        public event EventHandler CheckedChanged
        {
            add { m_onCheckedChanged += value; }
            remove { m_onCheckedChanged -= value; }
        }
        /// <summary>
        /// Event on brush change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (m_onCheckedChanged != null)
                m_onCheckedChanged(this, e);
        }
        private void RaiseCheckedChanged()
        {
            EventArgs e = new EventArgs();
            OnCheckedChanged(e);
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="isChecked"></param>
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
            this.m_checkBox.Size = new Size(20,20);
            this.m_checkBox.TabIndex = 0;
            this.m_checkBox.Checked = isChecked;
            this.m_checkBox.CheckedChanged += new EventHandler(m_checkBox_CheckedChanged);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void m_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            this.Checked = checkBox.Checked;
        }
    }
}
