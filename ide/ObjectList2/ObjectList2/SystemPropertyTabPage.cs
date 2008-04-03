using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using EcellLib.Objects;

namespace EcellLib.ObjectList2
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemPropertyTabPage : VPropertyTabPage
    {

        static private int s_columnNum = 7;
        /// <summary>
        /// 
        /// </summary>
        public override int ColumnNum
        {
            get
            {
                return SystemPropertyTabPage.s_columnNum;
            }
        }

        /// <summary>
        /// The property array of System.
        /// </summary>
        private String[] m_propertyArray = new string[] {
            VPropertyTabPage.s_indexType, 
            VPropertyTabPage.s_indexID, 
            VPropertyTabPage.s_indexModel, 
            VPropertyTabPage.s_indexClass,
            VPropertyTabPage.s_indexName, 
            VPropertyTabPage.s_indexStepper, 
            VPropertyTabPage.s_indexSize
        };

        /// <summary>
        /// 
        /// </summary>
        public SystemPropertyTabPage()
            : base()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void DataAdd(EcellObject obj)
        {
            int startpos = SearchHeaderPos();
            int index = SearchInsertIndex(startpos, obj.Key);
            int len = m_propertyArray.Length;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_propertyArray[i], obj);
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = data;
                rs.Cells.Add(c);
                c.ReadOnly = true;
                foreach (string name in m_notEditProp)
                {
                    if (name.Equals(m_propertyArray[i]))
                    {
                        c.ReadOnly = true;
                        break;
                    }
                }
                foreach (string name in m_simulationProp)
                {
                    if (name.Equals(m_propertyArray[i]))
                    {
                        string entPath = Util.ConvertSystemEntityPath(obj.Key, name);
                        m_propDic.Add(entPath, c);
                        break;
                    }
                }
            }
            for (int i = len; i < SystemPropertyTabPage.s_columnNum; i++)
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = "";
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }

            rs.Tag = obj;
            m_gridView.Rows.Insert(index, rs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="id"></param>
        /// <param name="isChanged"></param>
        /// <param name="dType"></param>
        public override void DataDelete(string modelID, string id, bool isChanged, Type dType)
        {
            base.DataDelete(modelID, id, isChanged, dType);
            if (isChanged)
            {
                int len = m_gridView.Rows.Count;
                for (int i = len - 1; i >= 0; i--)
                {
                    if (m_gridView[1, i].Value.ToString().StartsWith(id))
                    {
                        DeleteDictionary(i);
                        m_gridView.Rows.RemoveAt(i);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void CreateHeader()
        {
            int len = m_propertyArray.Length;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < SystemPropertyTabPage.s_columnNum; i++)
            {
                DataGridViewCell cs1 = new DataGridViewTextBoxCell();
                cs1.Style.BackColor = m_headerColor;
                if (i < len)
                    cs1.Value = m_propertyArray[i];
                else
                    cs1.Value = "";
                rs.Cells.Add(cs1);
                cs1.ReadOnly = true;
            }
            m_gridView.Rows.Add(rs);
        }

        /// <summary>
        /// Get tab name.
        /// </summary>
        /// <returns>name of tab.</returns>
        public override string GetTabPageName()
        {
            return "System";
        }

    }
}
