using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;

using ZedGraph;

namespace Ecell.IDE.Plugins.TracerWindow
{
    class TraceEntry
    {
        private string m_path;
        private LineItem m_currentLineItem;
        private LineItem m_tmpLineItem;
        private bool m_isContinuous;
        private bool m_isLoaded;
        private PointD m_prevPoints;


        public string Path
        {
            get { return m_path; }
            set { this.m_path = value; }
        }

        public LineItem CurrentLineItem
        {
            get { return m_currentLineItem; }
            set { m_currentLineItem = value; }
        }

        public LineItem TmpLineItem
        {
            get { return m_tmpLineItem; }
            set { m_tmpLineItem = value; }
        }

        public bool IsContinuous
        {
            get { return m_isContinuous; }
            set { m_isContinuous = value; }
        }

        public bool IsLoaded
        {
            get { return m_isLoaded; }
            set { m_isLoaded = value; }
        }

        public TraceEntry()
        {
            m_prevPoints = new PointD(0.0, 0.0);
        }

        public TraceEntry(string path, LineItem cItem, LineItem tItem, bool isCont, bool isLoad)
        {
            m_path = path;
            m_currentLineItem = cItem;
            m_tmpLineItem = tItem;
            m_isContinuous = isCont;
            m_isLoaded = isLoad;
            m_prevPoints = new PointD(0.0, 0.0);
        }

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

        public bool AddPoint(List<LogValue> data, double max, double min)
        {
            bool isAxis = false;
            
            foreach (LogValue v in data)
            {
                if (isAxis == false)
                {
                    if (max < v.value) isAxis = true;
                    if (min > v.value) isAxis = true;
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

        public void SetStyle(DashStyle style)
        {
            m_currentLineItem.Line.Style = style;
            m_tmpLineItem.Line.Style = style;
        }

        public void SetColor(Color col)
        {
            m_currentLineItem.Color = col;
            m_tmpLineItem.Color = col;
        }

        public void SetVisible(bool visible)
        {
            m_currentLineItem.IsVisible = visible;
            m_tmpLineItem.IsVisible = visible;
        }


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

        public void ClearPoint()
        {
            m_currentLineItem.Clear();
            m_tmpLineItem.Clear();
            PrevPointInitialize();

        }

        public void PrevPointInitialize()
        {
            m_prevPoints.X = 0.0;
            m_prevPoints.Y = 0.0;
        }

    }
}
