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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using EcellLib.PathwayWindow.Figure;
using EcellLib.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo.Util;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// ComponentSetting contains all information for creating one kind of a component of pathway.
    /// ex) Shape, color, etc.
    /// </summary>
    public class ComponentSetting
    {
        #region Enums
        /// <summary>
        /// Type of errors which are returned by methods of this class.
        /// </summary>
        public enum ErrorType
        {
            /// <summary>
            /// not an error.
            /// </summary>
            No_Error,
            /// <summary>
            /// figure is null
            /// </summary>
            Error_FigureNull,
            /// <summary>
            /// argument is null
            /// </summary>
            Error_ArgsNull,
            /// <summary>
            /// a figure doesn't exist
            /// </summary>
            Error_NoSuchFigure,
            /// <summary>
            /// some argument is lost
            /// </summary>
            Error_LessArgs,
            /// <summary>
            /// format is illegal
            /// </summary>
            Error_IllegalFormat
        };
        #endregion

        #region Fields
        /// <summary>
        /// Type of component which this instance offers.
        /// </summary>
        private ComponentType m_componentKind;

        /// <summary>
        /// The name of this component.
        /// </summary>
        private string m_name;

        /// <summary>
        /// GraphicsPath of this component.
        /// </summary>
        private GraphicsPath m_gp = new GraphicsPath();

        /// <summary>
        /// List of FigureBase, which will be used for getting contact point to edge.
        /// </summary>
        private List<FigureBase> m_figureList = new List<FigureBase>();

        /// <summary>
        /// Whether NormalBrush is set or not.
        /// </summary>
        private bool isNormalBrushSet = false;

        /// <summary>
        /// Brush for drawing this component when normal.
        /// </summary>
        private Brush m_normalBrush = Brushes.White;

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
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_componentKind.
        /// </summary>
        public ComponentType ComponentType
        {
            get { return this.m_componentKind; }
            set { this.m_componentKind = value; }
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
        /// Accessor for m_gp.
        /// </summary>
        public GraphicsPath GraphicsPath
        {
            get { return m_gp; }
        }
        /// <summary>
        /// Accessor for resized m_gp for being used as ToolBox item.
        /// </summary>
        public GraphicsPath TransformedPath
        {
            get
            {
                GraphicsPath transPath = (GraphicsPath)m_gp.Clone();
                RectangleF rect = m_gp.GetBounds();
                Matrix matrix = new Matrix();
                
                matrix.Translate(-1f * (rect.X + rect.Width / 2f),
                                 -1f * (rect.Y + rect.Height / 2f));

                transPath.Transform(matrix);

                matrix = new Matrix();
                if(rect.Width > rect.Height)
                {
                    matrix.Scale(240f / rect.Width, 240f / rect.Width);
                    matrix.Translate(128f * rect.Width / 256f, 128f * rect.Width / 256f);
                }
                else
                {
                    matrix.Scale(240f / rect.Height, 240f / rect.Height);
                    matrix.Translate(128f * rect.Height / 256f, 128f * rect.Height / 256f);
                }
                
                transPath.Transform(matrix);
                return transPath;
            }
        }
        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush NormalBrush
        {
            get { return this.m_normalBrush; }
            set
            {
                this.m_normalBrush = value;
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
        public List<FigureBase> FigureList
        {
            get { return this.m_figureList; }
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
            if (m_gp.PathData == null || m_gp.PointCount == 0)
                if(m_componentKind != ComponentType.System)
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
            {
                throw new NoSuchComponentClassException();
            }
            if(className.Equals("PEcellVariable"))
            {
                PPathwayVariable variable = new PPathwayVariable();
                m_createMethod = variable.CreateNewObject;
            }
            else if(className.Equals("PEcellProcess"))
            {
                PPathwayProcess process = new PPathwayProcess();
                m_createMethod = process.CreateNewObject;
            }
            else if(className.Equals("PEcellSystem"))
            {
                PPathwaySystem system = new PPathwaySystem();
                m_createMethod = system.CreateNewObject;
            }
        }

        /// <summary>
        /// This method create a new component with information in this class.
        /// </summary>
        /// <param name="eo">EcellObject</param>
        /// <param name="control">PathwayView instance</param>
        /// <returns>Created component</returns>
        public PPathwayObject CreateNewComponent(EcellObject eo, CanvasControl canvas)
        {
            PPathwayObject obj = m_createMethod();
            obj.CanvasControl = canvas;
            obj.ShowingID = canvas.ShowingID;
            obj.CsID = m_name;
            obj.Setting = this;
            obj.EcellObject = eo;
            if (m_componentKind == ComponentType.System)
            {
                obj.NormalBrush = Brushes.LightBlue;
                obj.Pen = null;
            }
            else
            {
                obj.AddPath(m_gp,false);
                obj.NormalBrush = m_normalBrush;
                obj.Width = PPathwayNode.DEFAULT_WIDTH;
                obj.Height = PPathwayNode.DEFAULT_HEIGHT;
            }
            obj.IsHighLighted = false;
            return obj;
        }

        /// <summary>
        /// Set figure for this ComponentSetting.
        /// </summary>
        /// <param name="type">type of figure</param>
        /// <param name="argString">arguments to create a figure</param>
        /// <returns></returns>
        public ErrorType AddFigure(string type, string argString)
        {
            if (type == null || type.Equals(""))
            {
                return ErrorType.Error_FigureNull;
            }
            if (argString == null || argString.Equals(""))
            {
                return ErrorType.Error_ArgsNull;
            }
            type = type.ToLower();
            string[] args = argString.Split(new Char[] { ',', ' ' });
            ErrorType returnCode = ComponentSetting.ErrorType.No_Error;
            if(type.Equals("arc"))
            {
                returnCode = AddArc(args);
                return ErrorType.No_Error;
            }
            else if(type.Equals("bezier"))
            {
                returnCode = AddBezier(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("beziers"))
            {
                returnCode = AddBeziers(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("closedcurve"))
            {
                returnCode = AddClosedCurve(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("curve"))
            {
                returnCode = AddCurve(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("ellipse"))
            {
                returnCode = AddEllipse(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("line"))
            {
                returnCode = AddLine(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("lines"))
            {
                returnCode = AddLines(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("pie"))
            {
                returnCode = AddPie(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("polygon"))
            {
                returnCode = AddPolygon(args);
                return ErrorType.No_Error;
            }
            else if (type.Equals("rectangle"))
            {
                returnCode = AddRectangle(args);
                return ErrorType.No_Error;
            }
            else
            {
                return ErrorType.Error_NoSuchFigure;
            }
        }

        private ErrorType AddArc(String[] args)
        {
            if (args.Length < 6)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddArc(int.Parse(args[0]),
                                int.Parse(args[1]),
                                int.Parse(args[2]),
                                int.Parse(args[3]),
                                float.Parse(args[4]),
                                float.Parse(args[5]));
                return ComponentSetting.ErrorType.No_Error;
            }
            catch(FormatException)
            {
                return ComponentSetting.ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddBezier(String[] args)
        {
            if (args.Length < 8)
                return ErrorType.Error_LessArgs;

            try
            {
                Point p1 = new Point(int.Parse(args[0]), int.Parse(args[1]));
                Point p2 = new Point(int.Parse(args[2]), int.Parse(args[3]));
                Point p3 = new Point(int.Parse(args[4]), int.Parse(args[5]));
                Point p4 = new Point(int.Parse(args[6]), int.Parse(args[7]));
                m_gp.AddBezier(p1, p2, p3, p4);
                return ErrorType.No_Error;
            }
            catch(FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddBeziers(String[] args)
        {
            if(args.Length < 2)
                return ComponentSetting.ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                Point[] pArray = new Point[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new Point(int.Parse(args[m]), int.Parse(args[m + 1]));
                m_gp.AddBeziers(pArray);
                return ComponentSetting.ErrorType.No_Error;
            }
            catch(FormatException)
            {
                return ComponentSetting.ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddClosedCurve(String[] args)
        {
            if (args.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                Point[] pArray = new Point[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new Point(int.Parse(args[m]), int.Parse(args[m + 1]));
                m_gp.AddClosedCurve(pArray);
                return ErrorType.No_Error;
            }
            catch(FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddCurve(String[] args)
        {
            if (args.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                Point[] pArray = new Point[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new Point(int.Parse(args[m]), int.Parse(args[m + 1]));
                m_gp.AddCurve(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddEllipse(String[] args)
        {
            if (args.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                RectangleF rect = new RectangleF( float.Parse(args[0]),
                                                  float.Parse(args[1]),
                                                  float.Parse(args[2]),
                                                  float.Parse(args[3]));
                m_gp.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
                m_figureList.Add(new EllipseFigure(rect.X, rect.Y, rect.Width, rect.Height));
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddLine(String[] args)
        {
            if (args.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddLine(float.Parse(args[0]),
                             float.Parse(args[1]),
                             float.Parse(args[2]),
                             float.Parse(args[3]));
                m_gp.CloseFigure();
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddLines(String[] args)
        {
            if (args.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                Point[] pArray = new Point[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new Point(int.Parse(args[m]), int.Parse(args[m + 1]));
                m_gp.AddLines(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddPie(String[] args)
        {
            if (args.Length < 6)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddPie(float.Parse(args[0]),
                            float.Parse(args[1]),
                            float.Parse(args[2]),
                            float.Parse(args[3]),
                            float.Parse(args[4]),
                            float.Parse(args[5]));
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddPolygon(String[] args)
        {
            if (args.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                Point[] pArray = new Point[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new Point(int.Parse(args[m]), int.Parse(args[m + 1]));
                m_gp.AddPolygon(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        private ErrorType AddRectangle(String[] args)
        {
            if (args.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                RectangleF rect = new RectangleF(float.Parse(args[0]),
                                float.Parse(args[1]),
                                float.Parse(args[2]),
                                float.Parse(args[3]));
                m_figureList.Add(new RectangleFigure(rect.X, rect.Y, rect.Width, rect.Height));
                m_gp.AddRectangle(rect);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }      
        #endregion
    }
}