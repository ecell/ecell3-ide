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
    internal class CommonComponentItem : UserControl
    {
        #region Fields

        private Label settingLabel;
        private CheckBox gradationCheckBox;
        private Button fileLoadButton;
        private Label label7;
        private TextBox iconFileTtextBox;
        private BrushComboBox textColorBrushComboBox;
        private BrushComboBox lineColorBrushComboBox;
        private BrushComboBox centerColorBrushComboBox;
        private BrushComboBox fillColorBrushComboBox;
        private Button resetSettingButton;
        private FigureComboBox figureComboBox;

        private PToolBoxCanvas pCanvas;
        #endregion

        public ComponentSetting Setting
        {
            set
            {
                SetItem(value);
                settingLabel.Text = value.Type + ":";
                ChangeFillBrush();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommonComponentItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cs"></param>
        public CommonComponentItem(ComponentSetting cs)
            : this()
        {
            
            SetItem(cs);
            settingLabel.Text = cs.Type + ":";
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonComponentItem));
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label1;
            this.settingLabel = new System.Windows.Forms.Label();
            this.gradationCheckBox = new System.Windows.Forms.CheckBox();
            this.fileLoadButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.iconFileTtextBox = new System.Windows.Forms.TextBox();
            this.resetSettingButton = new System.Windows.Forms.Button();
            this.figureComboBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.FigureComboBox();
            this.fillColorBrushComboBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.centerColorBrushComboBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.lineColorBrushComboBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.textColorBrushComboBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.pCanvas = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PToolBoxCanvas();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // settingLabel
            // 
            resources.ApplyResources(this.settingLabel, "settingLabel");
            this.settingLabel.Name = "settingLabel";
            // 
            // gradationCheckBox
            // 
            resources.ApplyResources(this.gradationCheckBox, "gradationCheckBox");
            this.gradationCheckBox.Name = "gradationCheckBox";
            this.gradationCheckBox.UseVisualStyleBackColor = true;
            this.gradationCheckBox.CheckedChanged += new System.EventHandler(this.gradationCheckBox_CheckedChanged);
            // 
            // fileLoadButton
            // 
            resources.ApplyResources(this.fileLoadButton, "fileLoadButton");
            this.fileLoadButton.Name = "fileLoadButton";
            this.fileLoadButton.UseVisualStyleBackColor = true;
            this.fileLoadButton.Click += new System.EventHandler(this.openFileDialogClick);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // iconFileTtextBox
            // 
            resources.ApplyResources(this.iconFileTtextBox, "iconFileTtextBox");
            this.iconFileTtextBox.Name = "iconFileTtextBox";
            // 
            // resetSettingButton
            // 
            resources.ApplyResources(this.resetSettingButton, "resetSettingButton");
            this.resetSettingButton.Name = "resetSettingButton";
            this.resetSettingButton.UseVisualStyleBackColor = true;
            this.resetSettingButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // figureComboBox
            // 
            this.figureComboBox.Figure = "Rectangle";
            resources.ApplyResources(this.figureComboBox, "figureComboBox");
            this.figureComboBox.Name = "figureComboBox";
            this.figureComboBox.ReadOnly = false;
            this.figureComboBox.TextChange += new System.EventHandler(this.figureBox_TextChange);
            // 
            // fillColorBrushComboBox
            // 
            this.fillColorBrushComboBox.Brush = null;
            resources.ApplyResources(this.fillColorBrushComboBox, "fillColorBrushComboBox");
            this.fillColorBrushComboBox.Name = "fillColorBrushComboBox";
            this.fillColorBrushComboBox.BrushChange += new System.EventHandler(this.fillBrush_BrushChange);
            // 
            // centerColorBrushComboBox
            // 
            this.centerColorBrushComboBox.Brush = null;
            resources.ApplyResources(this.centerColorBrushComboBox, "centerColorBrushComboBox");
            this.centerColorBrushComboBox.Name = "centerColorBrushComboBox";
            // 
            // lineColorBrushComboBox
            // 
            this.lineColorBrushComboBox.Brush = null;
            resources.ApplyResources(this.lineColorBrushComboBox, "lineColorBrushComboBox");
            this.lineColorBrushComboBox.Name = "lineColorBrushComboBox";
            this.lineColorBrushComboBox.BrushChange += new System.EventHandler(this.lineBrush_BrushChange);
            // 
            // textColorBrushComboBox
            // 
            this.textColorBrushComboBox.Brush = null;
            resources.ApplyResources(this.textColorBrushComboBox, "textColorBrushComboBox");
            this.textColorBrushComboBox.Name = "textColorBrushComboBox";
            this.textColorBrushComboBox.BrushChange += new System.EventHandler(this.textBrush_BrushChange);
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
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // CommonComponentItem
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(label1);
            this.Controls.Add(this.figureComboBox);
            this.Controls.Add(this.resetSettingButton);
            this.Controls.Add(this.fillColorBrushComboBox);
            this.Controls.Add(this.centerColorBrushComboBox);
            this.Controls.Add(this.lineColorBrushComboBox);
            this.Controls.Add(this.textColorBrushComboBox);
            this.Controls.Add(this.iconFileTtextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.fileLoadButton);
            this.Controls.Add(this.gradationCheckBox);
            this.Controls.Add(label6);
            this.Controls.Add(label5);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.settingLabel);
            this.Controls.Add(this.pCanvas);
            this.Name = "CommonComponentItem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void SetItem(ComponentSetting cs)
        {
            //this.groupBox.Text = cs.Type;
            // Set ToolCanvas.
            this.pCanvas.Setting = cs;
            this.pCanvas.Object.Text = "Sample";
            this.pCanvas.Object.Refresh();
            this.figureComboBox.ComboBox.Items.Clear();
            this.figureComboBox.ComboBox.Items.AddRange(FigureManager.GetFigureList(cs.Type).ToArray());

            this.textColorBrushComboBox.Brush = cs.TextBrush;
            this.lineColorBrushComboBox.Brush = cs.LineBrush;
            this.fillColorBrushComboBox.Brush = cs.FillBrush;
            this.gradationCheckBox.Checked = cs.IsGradation;
            this.centerColorBrushComboBox.Brush = cs.CenterBrush;            
            this.centerColorBrushComboBox.Enabled = cs.IsGradation;
            this.iconFileTtextBox.Text = cs.IconFile;
            this.figureComboBox.Figure = cs.Figure.Type;

            //// Set Figure.
            //if (this.figureBox.ComboBox.Items.Count <= 0)
            //    this.figureBox.ComboBox.Items.AddRange(FigureManager.GetFigureList(cs.Type).ToArray());

            // Set Parameter.
            //this.figureBox.ComboBox.Text = cs.Figure.Type;
            //this.textBrush.Brush = cs.TextBrush;
            //this.lineBrush.Brush = cs.LineBrush;
            //this.fillBrush.Brush = cs.FillBrush;
            //this.centerBrush.Brush = cs.CenterBrush;
            //this.isGradation.Checked = cs.IsGradation;
            //this.iconFile.FileName = cs.ImageStream;
        }

        /// <summary>
        /// Apply changes to ComponentSettings.
        /// </summary>
        public void  ApplyChange()
        {
            ComponentSetting cs = this.pCanvas.Setting;

            cs.Figure.Type = figureComboBox.Figure;
            cs.TextBrush = textColorBrushComboBox.Brush;
            cs.LineBrush = lineColorBrushComboBox.Brush;
            cs.FillBrush = fillColorBrushComboBox.Brush;
            cs.CenterBrush = centerColorBrushComboBox.Brush;
            cs.IsGradation = gradationCheckBox.Checked;
            try
            {
                cs.IconFile = iconFileTtextBox.Text;
                cs.ImageStream = Util.ImgToBase64(iconFileTtextBox.Text);
            }
            catch (Exception)
            {
                cs.IconFile = null;
                cs.ImageStream = null;
            }
            cs.RaisePropertyChange();
        }

        public void ItemClosing()
        {
        }
        #region EventHandlers
        /// <summary>
        /// Event on ChangeTextBrush
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBrush_BrushChange(object sender, EventArgs e)
        {
            this.pCanvas.Object.PText.TextBrush = textColorBrushComboBox.Brush;
        }
        /// <summary>
        /// Event on ChangeLineBrush
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineBrush_BrushChange(object sender, EventArgs e)
        {
            this.pCanvas.Object.LineBrush = lineColorBrushComboBox.Brush;
        }

        /// <summary>
        /// Set Image
        /// </summary>
        void IconFileChanged()
        {
            Image image = null;
            try
            {
                image = Image.FromFile(iconFileTtextBox.Text);
            }
            catch (Exception)
            {
            }
            if (image == null)
            {
                iconFileTtextBox.Text = null;
            }
            this.pCanvas.Object.Image = image;
        }

        /// <summary>
        /// Event on ChangeFillBrush
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fillBrush_BrushChange(object sender, EventArgs e)
        {
            BrushComboBox brushBox = (BrushComboBox)sender;
            if (brushBox.Brush == null)
                return;
            ChangeFillBrush();
        }

        /// <summary>
        /// Event on ChangeIsGradation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gradationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            centerColorBrushComboBox.Enabled = gradationCheckBox.Checked;
            ChangeFillBrush();
        }
        /// <summary>
        /// ChangeFillBrush
        /// </summary>
        private void ChangeFillBrush()
        {
            if (gradationCheckBox.Checked)
            {
                PathGradientBrush pthGrBrush = BrushManager.CreateGradientBrush(
                    pCanvas.Object.Path,
                    centerColorBrushComboBox.Brush,
                    fillColorBrushComboBox.Brush);
                this.pCanvas.Object.Brush = pthGrBrush;
            }
            else
            {
                this.pCanvas.Object.Brush = fillColorBrushComboBox.Brush;
            }
        }
        /// <summary>
        /// Event on ChangeFigure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void figureBox_TextChange(object sender, EventArgs e)
        {
            string type = figureComboBox.Figure;
            string args = this.pCanvas.Setting.Figure.Coordinates;
            IFigure figure = FigureManager.CreateFigure(type, args);
            this.pCanvas.Object.Figure = figure;
            ChangeFillBrush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            
            SetItem(pCanvas.Setting);
        }


        private void openFileDialogClick(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonComponentItem));
            OpenFileDialog dlg = new OpenFileDialog();
            using (dlg)
            {
                dlg.Filter = resources.GetString("iconFile.Filter");
                dlg.FilterIndex = 0;
                dlg.FileName = iconFileTtextBox.Text;
                dlg.CheckFileExists = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    iconFileTtextBox.Text = dlg.FileName;
                    IconFileChanged();
                }
            }
        }

        #endregion

    }
}
