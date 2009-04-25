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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;

using ZedGraph;

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Class object to manage the property of logger.
    /// </summary>
    class TraceEntry
    {
        #region  Fields
        private string m_path;
        private LineItem m_currentLineItem;
        private LineItem m_tmpLineItem;
        private bool m_isContinuous;
        private bool m_isLoaded;
        private bool m_isY2 = false;
        private PointD m_prevPoints;
        #endregion

        #region Accessors
        /// <summary>
        /// get the flag whether this data is shown in Y2.
        /// </summary>
        public bool IsY2
        {
            get { return this.m_isY2; }
        }

        /// <summary>
        /// get / set the fullPN of logger.
        /// </summary>
        public string Path
        {
            get { return m_path; }
            set { this.m_path = value; }
        }

        /// <summary>
        /// get / set the past line item.
        /// </summary>
        public LineItem CurrentLineItem
        {
            get { return m_currentLineItem; }
            set { m_currentLineItem = value; }
        }

        /// <summary>
        /// get / set the current line item.
        /// </summary>
        public LineItem TmpLineItem
        {
            get { return m_tmpLineItem; }
            set { m_tmpLineItem = value; }
        }

        /// <summary>
        /// get / set the flag whether this log is continuous.
        /// </summary>
        public bool IsContinuous
        {
            get { return m_isContinuous; }
            set { m_isContinuous = value; }
        }

        /// <summary>
        /// get / set the flag whether this log is loaded.
        /// </summary>
        public bool IsLoaded
        {
            get { return m_isLoaded; }
            set { m_isLoaded = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor with no options.
        /// </summary>
        public TraceEntry()
        {
            m_prevPoints = new PointD(0.0, 0.0);
        }

        /// <summary>
        /// Constructor with any options.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cItem"></param>
        /// <param name="tItem"></param>
        /// <param name="isCont"></param>
        /// <param name="isLoad"></param>
        public TraceEntry(string path, LineItem cItem, LineItem tItem, bool isCont, bool isLoad)
        {
            m_path = path;
            m_currentLineItem = cItem;
            m_tmpLineItem = tItem;
            m_isContinuous = isCont;
            m_isLoaded = isLoad;
            m_prevPoints = new PointD(0.0, 0.0);
        }
        #endregion

        #region SetViewer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        public void SetStyle(DashStyle style)
        {
            m_currentLineItem.Line.Style = style;
            m_tmpLineItem.Line.Style = style;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        public void SetColor(Color col)
        {
            m_currentLineItem.Color = col;
            m_tmpLineItem.Color = col;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visible"></param>
        public void SetVisible(bool visible)
        {
            m_currentLineItem.IsVisible = visible;
            m_tmpLineItem.IsVisible = visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        public void SetLineWidth(int width)
        {
            m_currentLineItem.Line.Width = (float)width;
            m_tmpLineItem.Line.Width = (float)width;
        }

        public void SetY2Axis(bool isY2)
        {
            m_currentLineItem.IsY2Axis = isY2;
            m_tmpLineItem.IsY2Axis = isY2;
            m_isY2 = isY2;
        }
        #endregion

        #region ManageData
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmax"></param>
        /// <param name="xmin"></param>
        /// <param name="ymax"></param>
        /// <param name="ymin"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public bool IsSmoothing(double xmax, double xmin, double ymax, double ymin, int width, int height)
        {
            double prex = -1.0;
            double prey = -1.0;
            double cx, cy;
            for (int i = 0; i < CurrentLineItem.Points.Count; i++)
            {
                cx = (CurrentLineItem.Points[i].X) / (xmax - xmin) * width;
                cy = (CurrentLineItem.Points[i].Y) / (ymax - ymin) * height;
                if (prex != -1.0)
                {
                    double dis = Math.Sqrt((cx - prex) * (cx - prex) + (cy - prey) * (cy - prey));
                    if (dis > 8) // 8ÇÕ3[ê¸],1[ãÛ],1[ì_],1[ãÛ],1[ì_],1[ãÛ]
                    {
                        return true;
                    }
                }
                prex = cx;
                prey = cy;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmax"></param>
        /// <param name="xmin"></param>
        /// <param name="ymax"></param>
        /// <param name="ymin"></param>
        /// <param name="isZoom"></param>
        /// <returns></returns>
        public bool AddPoint(List<LogValue> data, double xmax, double xmin,
            double ymax, double ymin, bool isZoom)
        {
            bool isAxis = false;

            foreach (LogValue v in data)
            {
                if (isZoom && (v.time < xmin && v.time > xmax))
                    continue;
                if (isAxis == false)
                {
                    if (ymax < v.value) isAxis = true;
                    if (ymin > v.value) isAxis = true;
                }
                if (IsContinuous)
                {
                    m_tmpLineItem.AddPoint(v.time, v.value);
                }
                else
                {
                    m_tmpLineItem.AddPoint(v.time, m_prevPoints.Y);
                    m_tmpLineItem.AddPoint(v.time, v.value);
                    m_prevPoints.X = v.time;
                    m_prevPoints.Y = v.value;
                }
            }
            return isAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ThinPoints()
        {
            if (IsContinuous)
            {
                ThinPointsForContinuous();
            }
            else
            {
                ThinPointsForNotContinuous();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThinPointsForContinuous()
        {
            for (int j = 0; j < m_tmpLineItem.Points.Count; j++)
            {
                m_currentLineItem.AddPoint(m_tmpLineItem.Points[j]);
            }
            m_tmpLineItem.Clear();

            if (m_currentLineItem.Points.Count > TracerWindow.s_count)
            {
                int i = m_currentLineItem.Points.Count - 2;
                while (i > 0)
                {
                    m_currentLineItem.RemovePoint(i);
                    i = i - 5;
                }
            }

            int l = m_currentLineItem.Points.Count;
            if (l > 0)
            {
                m_tmpLineItem.AddPoint(m_currentLineItem.Points[l - 1]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void ThinPointsForNotContinuous()
        {
            for (int j = 0; j < m_tmpLineItem.Points.Count; j++)
            {
                m_currentLineItem.AddPoint(m_tmpLineItem.Points[j]);
            }
            m_tmpLineItem.Clear();

            if (m_currentLineItem.Points.Count > TracerWindow.s_count)
            {
                int i = m_currentLineItem.Points.Count - 4;
                while (i < m_currentLineItem.Points.Count && i >= 0)
                {
                    m_currentLineItem.RemovePoint(i);
                    m_currentLineItem.RemovePoint(i);
                    i = i - 10;
                }
            }

            int l = m_currentLineItem.Points.Count;
            if (l > 0)
            {
                m_tmpLineItem.AddPoint(m_currentLineItem.Points[l - 1]);
                m_prevPoints.X = m_currentLineItem.Points[l - 1].X;
                m_prevPoints.Y = m_currentLineItem.Points[l - 1].Y;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ClearPoint()
        {
            m_currentLineItem.Clear();
            m_tmpLineItem.Clear();
            PrevPointInitialize();

        }
        /// <summary>
        /// 
        /// </summary>
        public void PrevPointInitialize()
        {
            m_prevPoints.X = 0.0;
            m_prevPoints.Y = 0.0;
        }
        #endregion
    }
}
