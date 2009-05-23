//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Ecell.Logger
{
    /// <summary>
    /// Logger entry object.
    /// </summary>
    public class LoggerEntry
    {
        #region Fields
        private string m_modelID;
        private string m_ID;
        private string m_Type;
        private string m_FullPN;
        private string m_filename;
        private Color m_color;
        private DashStyle m_lineStyle;
        private int m_lineWidth;
        private bool m_isShown;
        private bool m_isY2;
        private bool m_isLoaded;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor without the flag whether this property is logging.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="fullPN"></param>
        public LoggerEntry(string modelID, string id, string type, string fullPN)
        {
            this.m_modelID = modelID;
            this.m_ID = id;
            this.m_Type = type;
            this.m_FullPN = fullPN;
            this.m_color = Color.Black;
            this.m_lineStyle = DashStyle.Solid;
            this.m_lineWidth = 2;
            this.m_isShown = true;
            this.m_isY2 = false;
            this.m_isLoaded = false;
            this.m_filename = null; 
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set Model ID.
        /// </summary>
        public string ModelID
        {
            get { return this.m_modelID; }
            set { this.m_modelID = value; }
        }

        /// <summary>
        /// get / set ID.
        /// </summary>
        public string ID
        {
            get { return this.m_ID; }
            set { this.m_ID = value; }
        }

        /// <summary>
        /// get / set Type.
        /// </summary>
        public string Type
        {
            get { return this.m_Type; }
            set { this.m_Type = value; }
        }

        /// <summary>
        /// get / set FullPN.
        /// </summary>
        public string FullPN
        {
            get { return this.m_FullPN; }
            set { this.m_FullPN = value; }
        }

        /// <summary>
        /// get / set color.
        /// </summary>
        public Color Color
        {
            get { return this.m_color; }
            set { this.m_color = value; }
        }

        /// <summary>
        /// get / set the line style.
        /// </summary>
        public DashStyle LineStyle
        {
            get { return this.m_lineStyle; }
            set { this.m_lineStyle = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LineStyleInt
        {
            get
            {
                if (m_lineStyle == DashStyle.Solid)
                    return 0;
                else if (m_lineStyle == DashStyle.Dot)
                    return 1;
                else if (m_lineStyle == DashStyle.Dash)
                    return 2;
                else if (m_lineStyle == DashStyle.DashDot)
                    return 3;
                else
                    return 4;
            }
            set
            {
                if (value == 0)
                    m_lineStyle = DashStyle.Solid;
                else if (value == 1)
                    m_lineStyle = DashStyle.Dot;
                else if (value == 2)
                    m_lineStyle = DashStyle.Dash;
                else if (value == 3)
                    m_lineStyle = DashStyle.DashDot;
                else if (value == 4)
                    m_lineStyle = DashStyle.DashDotDot;
            }
        }

        /// <summary>
        /// get / set the line width.
        /// </summary>
        public int LineWidth
        {
            get { return this.m_lineWidth; }
            set { this.m_lineWidth = value; }
        }

        /// <summary>
        /// get / set the flag whether this log is shown.
        /// </summary>
        public bool IsShown
        {
            get { return this.m_isShown; }
            set { this.m_isShown = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int IsShowInt
        {
            get
            {
                if (this.m_isShown)
                    return 1;
                else
                    return 0;
            }
            set
            {
                if (value == 0)
                    this.m_isShown = false;
                else
                    this.m_isShown = true;
            }
        }

        /// <summary>
        /// get /set the flag whether this log is shown at Y2 axis.
        /// </summary>
        public bool IsY2Axis
        {
            get { return this.m_isY2; }
            set { this.m_isY2 = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int IsY2AxisInt
        {
            get
            {
                if (this.m_isY2)
                    return 1;
                else
                    return 0;
            }
            set
            {
                if (value == 0)
                    this.m_isY2 = false;
                else
                    this.m_isY2 = true;
            }
        }

        /// <summary>
        /// get / set the flag whether this property is loaded.
        /// </summary>
        public bool IsLoaded
        {
            get { return this.m_isLoaded; }
            set { this.m_isLoaded = value; }
        }

        /// <summary>
        /// get / set the loaded log file name.
        /// </summary>
        public string FileName
        {
            get { return this.m_filename; }
            set { this.m_filename = value; }
        }
        #endregion

        /// <summary>
        /// override the equal method on LoggerEntry.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is LoggerEntry))
                return false;

            LoggerEntry ent = (LoggerEntry)obj;
            if (ent.ModelID == this.ModelID &&
                ent.ID == this.ID &&
                ent.Type == this.Type &&
                ent.FullPN == this.FullPN &&
                ent.IsLoaded == this.IsLoaded &&
                (ent.IsLoaded == false ||
                (ent.IsLoaded == true && ent.FileName == this.FileName)))
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
