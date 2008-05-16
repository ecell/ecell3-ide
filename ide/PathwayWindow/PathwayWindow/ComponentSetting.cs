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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
// 
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using EcellLib.PathwayWindow.Exceptions;
using EcellLib.PathwayWindow.Figure;
using EcellLib.PathwayWindow.Graphic;
using EcellLib.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo.Util;
using EcellLib.Objects;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// ComponentSetting contains all information for creating one kind of a component of pathway.
    /// ex) Shape, color, etc.
    /// </summary>
    public class ComponentSetting
    {
        #region Fields
        /// <summary>
        /// Type of component which this instance offers.
        /// </summary>
        private ComponentType m_componentType;

        /// <summary>
        /// The name of this component.
        /// </summary>
        private string m_name = null;

        /// <summary>
        /// The class name.
        /// </summary>
        private string m_class = null;

        /// <summary>
        /// The icon file name.
        /// </summary>
        private string m_iconFileName = null;

        /// <summary>
        /// A FigureBase for Edit Mode.
        /// </summary>
        private IFigure m_figure = null;

        /// <summary>
        /// True if gradation Brush is available.
        /// </summary>
        private bool m_isGradation = false;

        /// <summary>
        /// TextBrush for drawing this component.
        /// </summary>
        private Brush m_textBrush = Brushes.Black;

        /// <summary>
        /// Brush for drawing this component when normal.
        /// </summary>
        private Brush m_lineBrush = Brushes.White;

        /// <summary>
        /// Brush for drawing this component when normal.
        /// </summary>
        private Brush m_centerBrush = Brushes.White;

        /// <summary>
        /// Brush for drawing this component when normal.
        /// </summary>
        private Brush m_fillBrush = Brushes.White;

        /// <summary>
        /// Brush for drawing this component when highlighted.
        /// </summary>
        private Brush m_highlightBrush = Brushes.Gold;

        /// <summary>
        /// Definition of delegate for creating each component.
        /// </summary>
        /// <returns></returns>
        private delegate PPathwayObject CreateComponent();

        /// <summary>
        /// delegate of CreateComponent.
        /// </summary>
        private CreateComponent m_createMethod;

        /// <summary>
        /// Default flag.
        /// </summary>
        private bool m_isDefault = false;

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_componentKind.
        /// </summary>
        public ComponentType ComponentType
        {
            get { return this.m_componentType; }
            set 
            { 
                this.m_componentType = value;
                RaisePropertyChange();
            }
        }

        /// <summary>
        /// Accessor for ClassName of PPathwayObject.
        /// </summary>
        public string Class
        {
            get { return m_class; }
            set 
            {
                this.m_class = value;
                AddClassCreateMethod(m_class);
            }
        }

        /// <summary>
        /// Accessor for m_name.
        /// </summary>
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// Accessor for m_isDefault.
        /// </summary>
        public bool IsDefault
        {
            get { return m_isDefault; }
            set 
            { 
                m_isDefault = value;
                RaisePropertyChange();
            }
        }

        /// <summary>
        /// Accessor for m_isGradation.
        /// </summary>
        public bool IsGradation
        {
            get { return m_isGradation; }
            set 
            { 
                m_isGradation = value;
                RaisePropertyChange();
            }
        }

        /// <summary>
        /// Accessor for m_centerBrush.
        /// </summary>
        public Brush CenterBrush
        {
            get { return this.m_centerBrush; }
            set { 
                this.m_centerBrush = value;
                RaisePropertyChange();
            }
        }

        /// <summary>
        /// Accessor for m_roundBrush.
        /// </summary>
        public Brush FillBrush
        {
            get { return this.m_fillBrush; }
            set 
            { 
                this.m_fillBrush = value;
                RaisePropertyChange();
            }
        }

        /// <summary>
        /// Accessor for m_textBrush.
        /// </summary>
        public Brush TextBrush
        {
            get { return this.m_textBrush; }
            set 
            { 
                this.m_textBrush = value;
                RaisePropertyChange();
            }
        }
        /// <summary>
        /// Accessor for m_lineBrush.
        /// </summary>
        public Brush LineBrush
        {
            get { return this.m_lineBrush; }
            set
            {
                this.m_lineBrush = value;
                RaisePropertyChange();
            }
        }
        /// <summary>
        /// Accessor for m_highlightBrush.
        /// </summary>
        public Brush HighlightBrush
        {
            get { return this.m_highlightBrush; }
            set 
            { 
                this.m_highlightBrush = value;
                RaisePropertyChange();
            }
        }
        /// <summary>
        /// Accessor for m_figure.
        /// </summary>
        public IFigure Figure
        {
            get { return this.m_figure; }
            set
            { 
                this.m_figure = value;
                RaisePropertyChange();
            }
        }
        /// <summary>
        /// Getter for IconImage.
        /// </summary>
        public string IconFileName
        {
            get { return m_iconFileName; }
            set 
            {
                m_iconFileName =  value;
                RaisePropertyChange();
            }
        }
        /// <summary>
        /// Getter for IconImage.
        /// </summary>
        public Image IconImage
        {
            get { return CreateIconImage(); }
        }
        #endregion

        #region EventHandler for PropertyChange
        private EventHandler m_onPropertyChange;
        /// <summary>
        /// Event on property change.
        /// </summary>
        public event EventHandler PropertyChange
        {
            add { m_onPropertyChange += value; }
            remove { m_onPropertyChange -= value; }
        }
        /// <summary>
        /// Event on property change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPropertyChange(EventArgs e)
        {
            if (m_onPropertyChange != null)
                m_onPropertyChange(this, e);
        }
        /// <summary>
        /// Event on property change.
        /// </summary>
        private void RaisePropertyChange()
        {
            EventArgs e = new EventArgs();
            OnPropertyChange(e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validate this ComponentSetting
        /// </summary>
        /// <returns>List of information this instance lacks. If this contains all information wanted, return null.</returns>
        public List<string> Validate()
        {
            List<string> lackInfos = new List<string>();
            if (m_figure.GraphicsPath.PathData == null || m_figure.GraphicsPath.PointCount == 0)
                if(m_componentType != ComponentType.System)
                    lackInfos.Add("Drawing");

            if (m_createMethod == null)
                lackInfos.Add("Class");

            if (m_textBrush == null)
                lackInfos.Add("TextBrush");

            if (m_centerBrush == null)
                lackInfos.Add("FillBrush");

            if (m_lineBrush == null)
                lackInfos.Add("LineBrush");

            if (m_fillBrush == null)
                lackInfos.Add("RoundBrush");

            if (lackInfos.Count == 0)
                return null;
            else
                return lackInfos;
        }

        /// <summary>
        /// Add a E-cell class of this ComponentSetting.
        /// </summary>
        /// <param name="className">a name of class</param>
        private void AddClassCreateMethod(string className)
        {
            PPathwayObject obj = null;
            if (className.Equals(PathwayConstants.ClassPPathwayVariable))
            {
                obj = new PPathwayVariable();
            }
            else if (className.Equals(PathwayConstants.ClassPPathwayProcess))
            {
                obj = new PPathwayProcess();
            }
            else if (className.Equals(PathwayConstants.ClassPPathwaySystem))
            {
                obj = new PPathwaySystem();
            }
            else if (className.Equals(PathwayConstants.ClassPPathwayText))
            {
                obj = new PPathwayText();
            }
            else
            {
                throw new NoSuchComponentClassException();
            }
            m_createMethod = obj.CreateNewObject;
        }

        /// <summary>
        /// This method create a new component with information in this class.
        /// </summary>
        /// <param name="eo">EcellObject</param>
        /// <returns>Created component object</returns>
        public PPathwayObject CreateNewComponent(EcellObject eo)
        {
            PPathwayObject obj = CreateTemplate();
            obj.EcellObject = eo;
            return obj;
        }

        /// <summary>
        /// Create object template.
        /// </summary>
        /// <returns></returns>
        public PPathwayObject CreateTemplate()
        {
            PPathwayObject obj = m_createMethod();
            obj.AddPath(m_figure.GraphicsPath, false);
            obj.Setting = this;
            return obj;
        }

        /// <summary>
        /// Create IconImage from file / FigureBase.
        /// </summary>
        /// <returns></returns>
        private Image CreateIconImage()
        {
            // Create Icon from file.
            Image icon = null;
            if (!string.IsNullOrEmpty(m_iconFileName) && File.Exists(m_iconFileName))
            {
                icon = CreateIconImageFromFile(m_iconFileName);
            }
            if (icon != null)
                return icon;

            // Create Icon from FigureBase.
            icon = new Bitmap(16, 16);
            Graphics gra = Graphics.FromImage(icon);
            GraphicsPath gp = m_figure.IconPath;
            Brush brush = GetFillBrush(gp);
            gra.FillPath(brush, gp);
            gra.DrawPath(new Pen(m_lineBrush, 0), gp);
            return icon;
        }

        /// <summary>
        /// Create icon image from file
        /// </summary>
        /// <returns></returns>
        private Image CreateIconImageFromFile(string fileName)
        {
            Image icon = null;
            try
            {
                Bitmap bitmap = new Bitmap(fileName);
                icon = new Bitmap(bitmap, 16, 16);
            }
            catch (Exception e)
            {
                m_iconFileName = null;
                Debug.WriteLine(e);
            }
            return icon;
        }

        /// <summary>
        /// Get FillBrush
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Brush GetFillBrush(GraphicsPath path)
        {
            if (m_isGradation)
            {
                PathGradientBrush pthGrBrush = new PathGradientBrush(path);
                pthGrBrush.CenterColor = BrushManager.ParseBrushToColor(m_centerBrush);
                pthGrBrush.SurroundColors = new Color[] { BrushManager.ParseBrushToColor(m_fillBrush) };
                return pthGrBrush;
            }
            else
            {
                return this.m_fillBrush;
            }
        }
        #endregion
    }
}