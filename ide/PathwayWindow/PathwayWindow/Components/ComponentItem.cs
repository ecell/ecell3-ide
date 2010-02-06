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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using Ecell.IDE.Plugins.PathwayWindow.Graphics;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using System.Drawing;

namespace Ecell.IDE.Plugins.PathwayWindow.Components
{

    /// <summary>
    /// private class for ComponentSettingDialog
    /// </summary>
    internal class ComponentItem : UserControl
    {
        #region Fields
        private GroupBox groupBox;
        private FigureComboBox figureBox;
        private PropertyBrushItem textBrush;
        private PropertyBrushItem lineBrush;
        private PropertyBrushItem fillBrush;
        private PropertyBrushItem centerBrush;
        private PropertyCheckBoxItem isGradation;
        private PropertyOpenFileItem iconFile;
        private Button resetButton;
        private Label label1;

        private PToolBoxCanvas pCanvas;
        /// <summary>
        /// 
        /// </summary>
        public PToolBoxCanvas PCanvas
        {
            get { return pCanvas; }
            set { pCanvas = value; }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public bool Changed
        {
            get 
            {
                if (pCanvas.Setting.Figure.Type != figureBox.Figure)
                    return true;
                if (pCanvas.Setting.TextBrush != textBrush.Brush)
                    return true;
                if (pCanvas.Setting.LineBrush != lineBrush.Brush)
                    return true;
                if (pCanvas.Setting.FillBrush != fillBrush.Brush)
                    return true;
                if (pCanvas.Setting.IsGradation != isGradation.Checked)
                    return true;
                if (pCanvas.Setting.FillBrush != fillBrush.Brush)
                    return true;
                if (pCanvas.Setting.CenterBrush != centerBrush.Brush)
                    return true;
                if (pCanvas.Setting.ImageStream != Util.ImgToBase64(iconFile.FileName))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ComponentItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cs"></param>
        public ComponentItem(ComponentSetting cs)
            : this()
        {
            SetItem(cs);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentItem));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.isGradation = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyCheckBoxItem();
            this.figureBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.FigureComboBox();
            this.textBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.lineBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.fillBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.centerBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.iconFile = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyOpenFileItem();
            this.pCanvas = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PToolBoxCanvas();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.resetButton);
            this.groupBox.Controls.Add(this.isGradation);
            this.groupBox.Controls.Add(this.figureBox);
            this.groupBox.Controls.Add(this.textBrush);
            this.groupBox.Controls.Add(this.lineBrush);
            this.groupBox.Controls.Add(this.fillBrush);
            this.groupBox.Controls.Add(this.centerBrush);
            this.groupBox.Controls.Add(this.iconFile);
            this.groupBox.Controls.Add(this.pCanvas);
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // resetButton
            // 
            resources.ApplyResources(this.resetButton, "resetButton");
            this.resetButton.Name = "resetButton";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // isGradation
            // 
            resources.ApplyResources(this.isGradation, "isGradation");
            this.isGradation.Checked = false;
            this.isGradation.Name = "isGradation";
            this.isGradation.CheckedChanged += new System.EventHandler(this.isGradation_CheckedChanged);
            // 
            // figureBox
            // 
            resources.ApplyResources(this.figureBox, "figureBox");
            this.figureBox.Figure = "Rectangle";
            this.figureBox.Name = "figureBox";
            this.figureBox.TextChange += new System.EventHandler(this.figureBox_TextChange);
            // 
            // textBrush
            // 
            resources.ApplyResources(this.textBrush, "textBrush");
            this.textBrush.Name = "textBrush";
            this.textBrush.BrushChange += new System.EventHandler(this.textBrush_BrushChange);
            // 
            // lineBrush
            // 
            resources.ApplyResources(this.lineBrush, "lineBrush");
            this.lineBrush.Name = "lineBrush";
            this.lineBrush.BrushChange += new System.EventHandler(this.lineBrush_BrushChange);
            // 
            // fillBrush
            // 
            resources.ApplyResources(this.fillBrush, "fillBrush");
            this.fillBrush.Name = "fillBrush";
            this.fillBrush.BrushChange += new System.EventHandler(this.fillBrush_BrushChange);
            // 
            // centerBrush
            // 
            resources.ApplyResources(this.centerBrush, "centerBrush");
            this.centerBrush.Name = "centerBrush";
            this.centerBrush.BrushChange += new System.EventHandler(this.fillBrush_BrushChange);
            // 
            // iconFile
            // 
            resources.ApplyResources(this.iconFile, "iconFile");
            this.iconFile.FileName = "";
            this.iconFile.Filter = resources.GetString("iconFile.Filter");
            this.iconFile.FilterIndex = 0;
            this.iconFile.Name = "iconFile";
            this.iconFile.FileChange += new System.EventHandler(this.iconFile_FileChange);
            // 
            // pCanvas
            // 
            this.pCanvas.AllowDrop = true;
            this.pCanvas.BackColor = System.Drawing.Color.Silver;
            this.pCanvas.GridFitText = false;
            resources.ApplyResources(this.pCanvas, "pCanvas");
            this.pCanvas.Name = "pCanvas";
            this.pCanvas.Object = null;
            this.pCanvas.RegionManagement = true;
            this.pCanvas.Setting = null;
            // 
            // ComponentItem
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox);
            this.Name = "ComponentItem";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        public void SetItem(ComponentSetting cs)
        {
            this.groupBox.Text = cs.Type;
            // Set ToolCanvas.
            this.pCanvas.Setting = cs;
            this.pCanvas.Object.Text = "Sample";
            this.pCanvas.Object.Refresh();
            // Set Figure.
            //if (this.figureBox.ComboBox.Items.Count <= 0)
            this.figureBox.SetItemType(cs.Type);

            // Set Parameter.
            this.figureBox.Figure = cs.Figure.Type;
            this.textBrush.Brush = cs.TextBrush;
            this.lineBrush.Brush = cs.LineBrush;
            this.fillBrush.Brush = cs.FillBrush;
            this.centerBrush.Brush = cs.CenterBrush;
            this.isGradation.Checked = cs.IsGradation;
            this.iconFile.FileName = cs.IconFile;
            RaiseItemChange();
        }

        /// <summary>
        /// Apply changes to ComponentSettings.
        /// </summary>
        public void ApplyChange()
        {
            ComponentSetting cs = this.pCanvas.Setting;
            UpdateSetting(cs);
            cs.RaisePropertyChange();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cs"></param>
        internal void UpdateSetting(ComponentSetting cs)
        {
            cs.TextBrush = textBrush.Brush;
            cs.LineBrush = lineBrush.Brush;
            cs.FillBrush = fillBrush.Brush;
            cs.CenterBrush = centerBrush.Brush;
            cs.IsGradation = isGradation.Checked;
            try
            {
                cs.IconFile = iconFile.FileName;
                cs.ImageStream = Util.ImgToBase64(iconFile.FileName);
            }
            catch (Exception)
            {
                cs.IconFile = null;
                cs.ImageStream = null;
            }
            string type = figureBox.Figure;
            string args = cs.Figure.Coordinates;
            cs.Figure = FigureManager.CreateFigure(type, args);
        }

        #region EventHandlers
        /// <summary>
        /// Event on ChangeTextBrush
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBrush_BrushChange(object sender, EventArgs e)
        {
            this.pCanvas.Object.PText.TextBrush = textBrush.Brush;
            RaiseItemChange();
        }
        /// <summary>
        /// Event on ChangeLineBrush
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineBrush_BrushChange(object sender, EventArgs e)
        {
            this.pCanvas.Object.LineBrush = lineBrush.Brush;
            RaiseItemChange();
        }

        /// <summary>
        /// Set Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void iconFile_FileChange(object sender, EventArgs e)
        {
            Image image = null;
            try
            {
                image = Image.FromFile(iconFile.FileName);
            }
            catch (Exception)
            {
            }
            if (image == null)
            {
                iconFile.FileName = null;
            }
            this.pCanvas.Object.Image = image;
            RaiseItemChange();
        }

        /// <summary>
        /// Event on ChangeFillBrush
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fillBrush_BrushChange(object sender, EventArgs e)
        {
            PropertyBrushItem brushBox = (PropertyBrushItem)sender;
            if (brushBox.Brush == null)
                return;
            ChangeFillBrush();
            RaiseItemChange();
        }
        /// <summary>
        /// Event on ChangeIsGradation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isGradation_CheckedChanged(object sender, EventArgs e)
        {
            centerBrush.Enabled = isGradation.Checked;
            ChangeFillBrush();
            RaiseItemChange();
        }
        /// <summary>
        /// ChangeFillBrush
        /// </summary>
        private void ChangeFillBrush()
        {
            if (isGradation.Checked)
            {
                PathGradientBrush pthGrBrush = BrushManager.CreateGradientBrush(
                    pCanvas.Object.Path,
                    centerBrush.Brush,
                    fillBrush.Brush);
                this.pCanvas.Object.Brush = pthGrBrush;
            }
            else
            {
                this.pCanvas.Object.Brush = fillBrush.Brush;
            }
        }
        /// <summary>
        /// Event on ChangeFigure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void figureBox_TextChange(object sender, EventArgs e)
        {
            string type = figureBox.Figure;
            if (string.IsNullOrEmpty(type))
                return;
            string args = this.pCanvas.Setting.Figure.Coordinates;
            IFigure figure = FigureManager.CreateFigure(type, args);
            this.pCanvas.Object.Figure = figure;
            ChangeFillBrush();
            RaiseItemChange();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            SetItem(pCanvas.Setting);
            RaiseItemChange();
        }

        #endregion


        #region EventHandler for ItemChange
        private EventHandler m_onItemChange;
        /// <summary>
        /// Event on item change.
        /// </summary>
        public event EventHandler ItemChange
        {
            add { m_onItemChange += value; }
            remove { m_onItemChange -= value; }
        }
        /// <summary>
        /// Event on item change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnItemChange(EventArgs e)
        {
            if (m_onItemChange != null)
                m_onItemChange(this, e);
        }
        /// <summary>
        /// Raise ItemChange event.
        /// </summary>
        private void RaiseItemChange()
        {
            EventArgs e = new EventArgs();
            OnItemChange(e);
        }
        #endregion

    }
}
