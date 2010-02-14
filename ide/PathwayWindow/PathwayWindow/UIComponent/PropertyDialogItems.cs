//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Graphics;
using System.IO;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public partial class PropertyDialogItem : UserControl
    {
        /// <summary>
        /// label to explain the DialogItem.
        /// </summary>
        protected Label label;
        
        /// <summary>
        /// position of itembox.
        /// </summary>
        protected static Point POSITION = new Point(120, 5);
        
        /// <summary>
        /// size of the itembox.
        /// </summary>
        protected static Size SIZE = new Size(100, 20);

        /// <summary>
        /// Accessor for m_label.
        /// </summary>
        public Label Label
        {
            get { return label; }
            set { label = value; }
        }

        /// <summary>
        /// Accessor for m_label.Text.
        /// </summary>
        [Localizable(true)]
        public string LabelText
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyDialogItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        public PropertyDialogItem(string label)
        {
            InitializeComponent();
            this.LabelText = label;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyDialogItem));
            this.label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label
            // 
            resources.ApplyResources(this.label, "label");
            this.label.Name = "label";
            // 
            // PropertyDialogItem
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label);
            this.Name = "PropertyDialogItem";
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
        /// 
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ComboBox ComboBox
        {
            get {return m_comboBox;}
        }

        /// <summary>
        /// Get ComboBox.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ReadOnly
        {
            get { return (m_comboBox.DropDownStyle == ComboBoxStyle.DropDownList); }
            set
            {
                if(value)
                    this.m_comboBox.DropDownStyle = ComboBoxStyle.DropDownList; 
                else
                    this.m_comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            }
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
        public PropertyComboboxItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
        /// <param name="itemList"></param>
        public PropertyComboboxItem(string label, string text, List<string> itemList)
        {
            InitializeComponent();
            // set Brushes
            this.LabelText = label;
            this.m_comboBox.Text = text;
            this.m_comboBox.Items.AddRange(itemList.ToArray());
        }

        private void InitializeComponent()
        {
            this.m_comboBox = new ComboBox();
            this.SuspendLayout();
            this.Controls.Add(this.m_comboBox);

            // 
            // m_comboBoxBrush
            // 
            this.m_comboBox.FormattingEnabled = true;
            this.m_comboBox.Location = new Point(120, 5);
            this.m_comboBox.Size = SIZE;
            this.m_comboBox.TabIndex = 0;
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
        private ImageComboBox comboBoxBrush;
        private Brush brush;

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
        public PropertyBrushItem()
        {
            InitializeComponent();
            this.comboBoxBrush.ImageList = BrushManager.BrushImageList;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="brush"></param>
        public PropertyBrushItem(string label, Brush brush)
            :this()
        {
            this.LabelText = label;
            this.Brush = brush;
        }

        private void InitializeComponent()
        {
            // set Brushes
            this.brush = Brushes.Black;
            this.comboBoxBrush = new ImageComboBox();
            this.SuspendLayout();
            this.Controls.Add(this.comboBoxBrush);

            // 
            // m_comboBoxBrush
            // 
            this.comboBoxBrush.FormattingEnabled = true;
            this.comboBoxBrush.Location = POSITION;
            this.comboBoxBrush.Size = SIZE;
            this.comboBoxBrush.TabIndex = 0;
            this.comboBoxBrush.Text = BrushManager.ParseBrushToString(brush);
            this.comboBoxBrush.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
            this.comboBoxBrush.KeyDown += new KeyEventHandler(cBoxNomalBrush_KeyDown);
            this.comboBoxBrush.SelectedIndexChanged += new EventHandler(cBoxBrush_SelectedIndexChanged);

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
        public PropertyLabelItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
        public PropertyLabelItem(string label, string text)
        {
            InitializeComponent();
            this.LabelText = label;
            this.m_text.Text = text;


        }

        private void InitializeComponent()
        {
            this.m_text = new Label();
            this.SuspendLayout();
            this.Controls.Add(this.m_text);
            // 
            // m_textBox
            // 
            this.m_text.Location = POSITION;
            this.m_text.Size = SIZE;
            this.m_text.TabIndex = 0;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyTextItem : PropertyDialogItem
    {
        private TextBox textBox;
        /// <summary>
        /// Get/Set textBox.Text
        /// </summary>
        public override string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        /// <summary>
        /// Get textBox
        /// </summary>
        public TextBox TextBox
        {
            get { return textBox; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyTextItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
        public PropertyTextItem(string label, string text)
        {
            InitializeComponent();
            this.LabelText = label;
            this.textBox.Text = text;
        }

        private void InitializeComponent()
        {
            this.textBox = new TextBox();
            this.SuspendLayout();
            this.Controls.Add(this.textBox);
            // 
            // m_textBox
            // 
            this.textBox.Location = POSITION;
            this.textBox.Size = SIZE;
            this.textBox.TabIndex = 0;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertySaveFileItem : PropertyDialogItem
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
        public PropertySaveFileItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="filename"></param>
        public PropertySaveFileItem(string label, string filename)
        {
            InitializeComponent();
            this.LabelText = label;
            this.m_textBox.Text = filename;
        }

        private void InitializeComponent()
        {
            // Create New Object.
            this.m_textBox = new TextBox();
            this.m_button = new Button();

            this.SuspendLayout();
            this.Controls.Add(this.m_button);
            this.Controls.Add(this.m_textBox);
            // 
            this.m_textBox.Location = POSITION;
            this.m_textBox.Size = SIZE;
            this.m_textBox.TabIndex = 0;

            this.m_button.Text = "...";
            this.m_button.Top = m_textBox.Top;
            this.m_button.Left = m_textBox.Left + m_textBox.Width;
            this.m_button.Height = SIZE.Height;
            this.m_button.Width = SIZE.Height;
            this.m_button.Click += new EventHandler(m_button_Click);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        void m_button_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            using (fileDialog)
            {
                fileDialog.Filter = m_filter;
                fileDialog.FileName = FileName;
//                fileDialog.FilterIndex = m_filterIndex;
                fileDialog.InitialDirectory = Path.GetDirectoryName(m_textBox.Text);
                fileDialog.OverwritePrompt = true;
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
    public class PropertyOpenFileItem : PropertyDialogItem
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
        public PropertyOpenFileItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="filename"></param>
        public PropertyOpenFileItem(string label, string filename)
        {
            InitializeComponent();
            this.LabelText = label;
            this.m_textBox.Text = filename;
        }

        private void InitializeComponent()
        {
            // Create New Object.
            this.m_textBox = new TextBox();
            this.m_button = new Button();

            this.SuspendLayout();
            this.Controls.Add(this.m_button);
            this.Controls.Add(this.m_textBox);
            // 
            this.m_textBox.Location = POSITION;
            this.m_textBox.Size = SIZE;
            this.m_textBox.TabIndex = 0;

            this.m_button.Text = "...";
            this.m_button.Top = m_textBox.Top;
            this.m_button.Left = m_textBox.Left + m_textBox.Width;
            this.m_button.Height = SIZE.Height;
            this.m_button.Width = SIZE.Height;
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
                fileDialog.CheckFileExists = true;
                DialogResult result = fileDialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                m_textBox.Text = fileDialog.FileName;
                RaiseFileChange();
            }
        }

        #region EventHandler for FileChange
        private EventHandler m_onFileChange;
        /// <summary>
        /// Event on brush change.
        /// </summary>
        public event EventHandler FileChange
        {
            add { m_onFileChange += value; }
            remove { m_onFileChange -= value; }
        }
        /// <summary>
        /// Event on brush change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFileChange(EventArgs e)
        {
            if (m_onFileChange != null)
                m_onFileChange(this, e);
        }
        private void RaiseFileChange()
        {
            EventArgs e = new EventArgs();
            OnFileChange(e);
        }
        #endregion

    }

    /// <summary>
    /// UI class for PropertyDialog
    /// </summary>
    public class PropertyCheckBoxItem : PropertyDialogItem
    {
        private CheckBox checkBox;

        /// <summary>
        /// Get/Set m_checkBox.
        /// </summary>
        public CheckBox CheckBox
        {
            get { return checkBox; }
            set { checkBox = value; }
        }

        /// <summary>
        /// Get/Set m_checkBox.Checked
        /// </summary>
        public bool Checked
        {
            get { return checkBox.Checked; }
            set 
            { 
                checkBox.Checked = value;
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
        public PropertyCheckBoxItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="isChecked"></param>
        public PropertyCheckBoxItem(string label, bool isChecked)
        {
            InitializeComponent();
            this.LabelText = label;
            this.Checked = isChecked;
         }

        private void InitializeComponent()
        {
            this.checkBox = new CheckBox();
            this.SuspendLayout();
            this.Controls.Add(this.checkBox);
            // 
            // m_checkBox
            // 
            this.checkBox.Location = POSITION;
            this.checkBox.Size = new Size(20,20);
            this.checkBox.TabIndex = 0;
            this.checkBox.Checked = false;
            this.checkBox.CheckedChanged += new EventHandler(m_checkBox_CheckedChanged);
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
