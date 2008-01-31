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
            get { return this.m_componentKind; }
            set { this.m_componentKind = value; }
        }

        /// <summary>
        /// Accessor for ClassName of PPathwayObject.
        /// </summary>
        public string Class
        {
            get
            {
                switch(m_componentKind)
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
            obj.CsID = m_name;
            obj.Setting = this;
            obj.FillBrush = m_fillBrush;
            obj.LineBrush = m_lineBrush;
            if (m_componentKind == ComponentType.System)
            {
                obj.Width = PPathwaySystem.MIN_X_LENGTH;
                obj.Height = PPathwaySystem.MIN_Y_LENGTH;
            }
            else
            {
                obj.AddPath(m_gp, false);
                obj.Width = PPathwayNode.DEFAULT_WIDTH;
                obj.Height = PPathwayNode.DEFAULT_HEIGHT;
            }

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
            // Check Errors
            if (string.IsNullOrEmpty(type))
                return ErrorType.Error_FigureNull;
            if (string.IsNullOrEmpty(argString))
                return ErrorType.Error_ArgsNull;

            type = type.ToLower();
            float[] values = StringToFloats(argString);
            ErrorType returnCode = ComponentSetting.ErrorType.No_Error;
            if(type.Equals("arc"))
            {
                returnCode = AddArc(values);
                return ErrorType.No_Error;
            }
            else if(type.Equals("bezier"))
            {
                returnCode = AddBezier(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("beziers"))
            {
                returnCode = AddBeziers(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("closedcurve"))
            {
                returnCode = AddClosedCurve(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("curve"))
            {
                returnCode = AddCurve(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("ellipse"))
            {
                returnCode = AddEllipse(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("line"))
            {
                returnCode = AddLine(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("lines"))
            {
                returnCode = AddLines(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("pie"))
            {
                returnCode = AddPie(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("polygon"))
            {
                returnCode = AddPolygon(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("rectangle"))
            {
                returnCode = AddRectangle(values);
                return ErrorType.No_Error;
            }
            else if (type.Equals("roundcornerrectangle"))
            {
                returnCode = AddRoundCornerRectangle(values);
                return ErrorType.No_Error;
            }
            else
            {
                return ErrorType.Error_NoSuchFigure;
            }
        }
        /// <summary>
        /// Change string to float array.
        /// </summary>
        /// <param name="argString"></param>
        /// <returns></returns>
        private float[] StringToFloats(string argString)
        {
            string[] args = argString.Split(new Char[] { ',', ' ' });
            float[] values = new float[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                values[i] = float.Parse(args[i]);
            }
            return values;
        }
        /// <summary>
        /// Add arc.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddArc(float[] args)
        {
            if (args.Length < 6)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddArc(args[0],
                            args[1],
                            args[2],
                            args[3],
                            args[4],
                            args[5]);
                return ComponentSetting.ErrorType.No_Error;
            }
            catch(FormatException)
            {
                return ComponentSetting.ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add bezier curve.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddBezier(float[] args)
        {
            if (args.Length < 8)
                return ErrorType.Error_LessArgs;

            try
            {
                PointF p1 = new PointF(args[0], args[1]);
                PointF p2 = new PointF(args[2], args[3]);
                PointF p3 = new PointF(args[4], args[5]);
                PointF p4 = new PointF(args[6], args[7]);
                m_gp.AddBezier(p1, p2, p3, p4);
                return ErrorType.No_Error;
            }
            catch(FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add bezier curves
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddBeziers(float[] args)
        {
            if(args.Length < 2)
                return ComponentSetting.ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(args[m], args[m + 1]);
                m_gp.AddBeziers(pArray);
                return ComponentSetting.ErrorType.No_Error;
            }
            catch(FormatException)
            {
                return ComponentSetting.ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add closed curve
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddClosedCurve(float[] args)
        {
            if (args.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(args[m], args[m + 1]);
                m_gp.AddClosedCurve(pArray);
                return ErrorType.No_Error;
            }
            catch(FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add curve
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddCurve(float[] args)
        {
            if (args.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(args[m], args[m + 1]);
                m_gp.AddCurve(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add ellipse
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddEllipse(float[] args)
        {
            if (args.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                RectangleF rect = new RectangleF( args[0],
                                                  args[1],
                                                  args[2],
                                                  args[3]);
                m_gp.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
                m_figureList.Add(new EllipseFigure(rect.X, rect.Y, rect.Width, rect.Height));
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add line
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddLine(float[] args)
        {
            if (args.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddLine(args[0],
                             args[1],
                             args[2],
                             args[3]);
                m_gp.CloseFigure();
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add lines
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddLines(float[] args)
        {
            if (args.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(args[m], args[m + 1]);
                m_gp.AddLines(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add pie
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddPie(float[] args)
        {
            if (args.Length < 6)
                return ErrorType.Error_LessArgs;

            try
            {
                m_gp.AddPie(args[0],
                            args[1],
                            args[2],
                            args[3],
                            args[4],
                            args[5]);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add polygon
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddPolygon(float[] args)
        {
            if (args.Length < 2)
                return ErrorType.Error_LessArgs;

            try
            {
                int numPoint = args.Length / 2;
                PointF[] pArray = new PointF[numPoint];
                for (int m = 0; m < numPoint; m++)
                    pArray[m] = new PointF(args[m], args[m + 1]);
                m_gp.AddPolygon(pArray);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add rectangle
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddRectangle(float[] args)
        {
            if (args.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                RectangleF rect = new RectangleF(args[0],
                                args[1],
                                args[2],
                                args[3]);
                m_figureList.Add(new RectangleFigure(rect.X, rect.Y, rect.Width, rect.Height));
                m_gp.AddRectangle(rect);
                return ErrorType.No_Error;
            }
            catch (FormatException)
            {
                return ErrorType.Error_IllegalFormat;
            }
        }
        /// <summary>
        /// Add RoundCornerRectangle
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ErrorType AddRoundCornerRectangle(float[] args)
        {
            if (args.Length < 4)
                return ErrorType.Error_LessArgs;

            try
            {
                RectangleF rect = new RectangleF(args[0],
                                args[1],
                                args[2],
                                args[3]);
                m_figureList.Add(new RoundCornerRectangle(rect.X, rect.Y, rect.Width, rect.Height));
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