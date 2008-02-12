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
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using EcellLib.PathwayWindow.Figure;
using EcellLib.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo.Util;
using EcellLib.PathwayWindow.Exceptions;

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
        private string m_name;

        /// <summary>
        /// List of FigureBase, which will be used for getting contact point to edge.
        /// </summary>
        private FigureBase m_figure = null;

        /// <summary>
        /// Whether NormalBrush is set or not.
        /// </summary>
        private bool isNormalBrushSet = false;

        /// <summary>
        /// Brush for drawing this component when normal.
        /// </summary>
        private Brush m_fillBrush = Brushes.White;

        /// <summary>
        /// Brush for drawing this component when normal.
        /// </summary>
        private Brush m_lineBrush = Brushes.White;

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
            set { this.m_componentType = value; }
        }

        /// <summary>
        /// Accessor for ClassName of PPathwayObject.
        /// </summary>
        public string Class
        {
            get
            {
                switch(m_componentType)
                {
                    case ComponentType.System:
                        return ComponentManager.ClassPPathwaySystem;
                    case ComponentType.Process:
                        return ComponentManager.ClassPPathwayProcess;
                    case ComponentType.Variable:
                        return ComponentManager.ClassPPathwayVariable;
                }
                return null;
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
            set { m_isDefault = value; }
        }

        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush FillBrush
        {
            get { return this.m_fillBrush; }
            set
            {
                this.m_fillBrush = value;
                isNormalBrushSet = true;
            }
        }
        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush LineBrush
        {
            get { return this.m_lineBrush; }
            set
            {
                this.m_lineBrush = value;
                isNormalBrushSet = true;
            }
        }
        /// <summary>
        /// Accessor for m_highlightBrush.
        /// </summary>
        public Brush HighlightBrush
        {
            get { return this.m_highlightBrush; }
            set { this.m_highlightBrush = value; }
        }
        /// <summary>
        /// Accessor for m_figureList.
        /// </summary>
        public FigureBase Figure
        {
            get { return this.m_figure; }
            set { this.m_figure = value; }
        }
        /// <summary>
        /// Getter for IconImage.
        /// </summary>
        public Image IconImage
        {
            get { return CreateIconImage(); }
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

            if (!isNormalBrushSet)
                lackInfos.Add("Color");

            if (lackInfos.Count == 0)
                return null;
            else
                return lackInfos;
        }

        /// <summary>
        /// Add a E-cell class of this ComponentSetting.
        /// </summary>
        /// <param name="className">a name of class</param>
        public void AddComponentClass(string className)
        {
            if(className == null || className.Equals(""))
                throw new NoSuchComponentClassException();

            if (className.Equals(ComponentManager.ClassPPathwayVariable))
            {
                PPathwayVariable variable = new PPathwayVariable();
                m_createMethod = variable.CreateNewObject;
            }
            else if (className.Equals(ComponentManager.ClassPPathwayProcess))
            {
                PPathwayProcess process = new PPathwayProcess();
                m_createMethod = process.CreateNewObject;
            }
            else if (className.Equals(ComponentManager.ClassPPathwaySystem))
            {
                PPathwaySystem system = new PPathwaySystem();
                m_createMethod = system.CreateNewObject;
            }
        }

        /// <summary>
        /// This method create a new component with information in this class.
        /// </summary>
        /// <param name="eo">EcellObject</param>
        /// <param name="canvas">CanvasControl instance</param>
        /// <returns>Created component</returns>
        public PPathwayObject CreateNewComponent(EcellObject eo, CanvasControl canvas)
        {
            PPathwayObject obj = CreateTemplate();
            obj.CanvasControl = canvas;
            obj.ShowingID = canvas.ShowingID;
            obj.EcellObject = eo;
            if (obj is PPathwaySystem)
            {
                obj.Width = eo.Width;
                obj.Height = eo.Height;
            }
            obj.IsHighLighted = false;
            return obj;
        }

        /// <summary>
        /// Create object template.
        /// </summary>
        /// <returns></returns>
        public PPathwayObject CreateTemplate()
        {
            PPathwayObject obj = m_createMethod();
            obj.Setting = this;
            obj.FillBrush = m_fillBrush;
            obj.LineBrush = m_lineBrush;
            if (m_componentType == ComponentType.System)
            {
                obj.Width = PPathwaySystem.MIN_X_LENGTH;
                obj.Height = PPathwaySystem.MIN_Y_LENGTH;
            }
            else
            {
                obj.AddPath(m_figure.GraphicsPath, false);
                obj.Width = PPathwayNode.DEFAULT_WIDTH;
                obj.Height = PPathwayNode.DEFAULT_HEIGHT;
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Image CreateIconImage()
        {
            Image icon = new Bitmap(256, 256);
            Graphics gra = Graphics.FromImage(icon);
            if (m_componentType == ComponentType.System)
            {
                Rectangle rect = new Rectangle(3, 3, 240, 240);
                gra.DrawRectangle(new Pen(Brushes.Black, 16), rect);
            }
            else
            {
                GraphicsPath gp = m_figure.TransformedPath;
                gra.FillPath(m_fillBrush, gp);
                gra.DrawPath(new Pen(m_lineBrush, 16), gp);
            }
            return icon;
        }
        #endregion
    }
}