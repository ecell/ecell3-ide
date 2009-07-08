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
using Ecell.IDE.Plugins.PathwayWindow.Exceptions;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow.Components
{
    /// <summary>
    /// ComponentSetting contains all information for creating one kind of a component of pathway.
    /// ex) Shape, color, etc.
    /// </summary>
    public class ComponentSetting : ICloneable
    {
        #region Fields
        /// <summary>
        /// The name of this component.
        /// </summary>
        private string m_name = null;

        /// <summary>
        /// Type of component which this instance offers.
        /// </summary>
        private string m_type = null;

        /// <summary>
        /// If this Setting create Stencil or not.
        /// </summary>
        private bool m_IsStencil = false;

        /// <summary>
        /// The ImageStream of icon image.
        /// </summary>
        private string m_imageStream = null;
        
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
        public string Type
        {
            get { return this.m_type; }
            set 
            { 
                this.m_type = value;
                AddCreateMethod(m_type);
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
        /// If this Setting create Stencil or not.
        /// </summary>
        public bool IsStencil
        {
            get { return m_IsStencil; }
            set { m_IsStencil = value; }
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
            }
        }
        /// <summary>
        /// Base64 ImageStream.
        /// </summary>
        public string ImageStream
        {
            get { return m_imageStream; }
            set 
            {
                m_imageStream =  value;
            }
        }
        /// <summary>
        /// Return true if icon exists.
        /// </summary>
        public Image Image
        {
            get 
            {
                Image image = null;
                try
                {
                    if (!string.IsNullOrEmpty(m_imageStream))
                        image = Util.Base64ToImage(m_imageStream);
                }
                catch (Exception)
                {
                    m_imageStream = null;
                }
                return image;
            }
        }

        /// <summary>
        /// Getter for IconImage.
        /// </summary>
        public Image Icon
        {
            get { return CreateIconImage(); }
        }
        #endregion

        #region Constructors
        
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
        internal void RaisePropertyChange()
        {
            EventArgs e = new EventArgs();
            OnPropertyChange(e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "(" + m_type + "," + m_name + "," + this.GetHashCode().ToString() + ")";
            return base.ToString() + str;
        }

        /// <summary>
        /// Add a E-cell class of this ComponentSetting.
        /// </summary>
        /// <param name="type">a type of class</param>
        private void AddCreateMethod(string type)
        {
            PPathwayObject obj = null;
            switch (type)
            {
                case EcellObject.SYSTEM:
                    obj = new PPathwaySystem();
                    break;
                case EcellObject.PROCESS:
                    obj = new PPathwayProcess();
                    break;
                case EcellObject.VARIABLE:
                    obj = new PPathwayVariable();
                    break;
                case EcellObject.TEXT:
                    obj = new PPathwayText();
                    break;
                case EcellObject.STEPPER:
                    obj = new PPathwayStepper();
                    break;
                default:
                    throw new PathwayException(MessageResources.ErrUnknowType);
            }

            m_createMethod = obj.CreateNewObject;
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
        /// CreateBrush
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Brush CreateBrush(GraphicsPath path)
        {
            // Create and set FillBrush.
            Brush brush;
            if (m_isGradation)
            {
                brush = BrushManager.CreateGradientBrush(path, m_centerBrush, m_fillBrush);
            }
            else
            {
                brush = m_fillBrush;
            }
            return brush;
        }

        /// <summary>
        /// Create IconImage from file / FigureBase.
        /// </summary>
        /// <returns></returns>
        private Image CreateIconImage()
        {
            // Create Icon from file.
            Image icon = null;
            if (m_imageStream != null)
            {
                try
                {
                    icon = new Bitmap(Image, 16, 16);
                }
                catch (Exception e)
                {
                    m_imageStream = null;
                }
            }
            if (icon != null)
                return icon;

            // Create Icon from FigureBase.
            GraphicsPath gp;
            icon = new Bitmap(16, 16);
            Graphics gra = Graphics.FromImage(icon);
            if (m_type == EcellObject.TEXT)
            {
                gp = new GraphicsPath();
                gp.AddRectangle(new RectangleF(0,0,14,14));
            }
            else
            {
                gp = m_figure.IconPath;
            }
            Brush brush = GetFillBrush(gp);
            gra.FillPath(brush, gp);
            gra.DrawPath(new Pen(m_lineBrush, 0), gp);
            if (m_type == EcellObject.TEXT)
            {
                Font font = new Font("Arial", 10);
                gra.DrawString("T", font, m_textBrush, new PointF(2, 0));
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

        #region ICloneable ÉÅÉìÉo
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ComponentSetting Clone()
        {
            // Set hard coded default system ComponentSettings
            ComponentSetting cs = new ComponentSetting();
            cs.Type = this.Type;
            cs.Name = this.Name;
            cs.IsDefault = this.IsDefault;
            cs.IsStencil = this.IsStencil;
            cs.Figure = (IFigure)this.Figure.Clone();
            cs.CenterBrush = this.CenterBrush;
            cs.FillBrush = this.FillBrush;
            cs.IsGradation = this.IsGradation;
            cs.LineBrush = this.LineBrush;
            cs.ImageStream = this.ImageStream;

            return cs;
        }

        #endregion
    }
}
