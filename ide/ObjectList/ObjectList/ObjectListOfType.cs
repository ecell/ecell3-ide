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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

using EcellLib;

namespace EcellLib.ObjectList
{
    public class ObjectListOfType
    {
        #region Fields
        /// <summary>
        /// column string list.
        /// </summary>
        private List<string> column;
        /// <summary>
        /// Object list derived by data type.
        /// </summary>
        private DataGridView dgv;
        /// <summary>
        /// data set displayed on DataGridView.
        /// </summary>
        private DataSet ds;
        /// <summary>
        /// data type.
        /// </summary>
        private string type;
        #endregion

        /// <summary>
        /// constructor of ObjectListOfType with initial value.
        /// </summary>
        /// <param name="key">the initial key ID</param>
        /// <param name="data">the initial data</param>
        public ObjectListOfType(string type, List<string> column, DataGridView dgv, DataSet ds)
        {
            this.type = type;
            this.column = column;
            this.dgv = dgv;
            this.ds = ds;
        }

        /// <summary>
        /// get/set column.
        /// </summary>
        public List<string> Column
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// get/set dgv.
        /// </summary>
        public DataGridView Dgv
        {
            get { return dgv; }
            set { dgv = value; }
        }

        /// <summary>
        /// get/set ds.
        /// </summary>
        public DataSet Ds
        {
            get { return ds; }
            set { ds = value; }
        }

        /// <summary>
        /// add ecell data.
        /// </summary>
        /// <param name="ecellObject"></param>
        public void AddObject(EcellObject eo)
        {
            DataRow dataRow;
            if (ds.Tables[eo.type].Rows.Contains(eo.key)) return;

            dataRow = ds.Tables[eo.type].NewRow();
            if (column.Contains("modelID")) dataRow["modelID"] = eo.modelID;
            if (column.Contains("key")) dataRow["key"] = eo.key;
            if (column.Contains("classname")) dataRow["classname"] = eo.classname;

            if (eo.M_value != null)
            {
                foreach (EcellData data in eo.M_value)
                {
                    if (data.M_name != null && column.Contains(data.M_name))
                    {
                        string cellStr = data.M_value.ToString();
                        dataRow[data.M_name] = cellStr;
                    }
                }
            }
            ds.Tables[type].Rows.Add(dataRow);

            return;
        }

        /// <summary>
        /// Delete EcellObject from this ObjectListOfType.
        /// </summary>
        /// <param name="key">This key indicates an EcellObject to be deleted</param>
        public void DeleteObject(string key, string type)
        {
            if(key == null)
            {
                return;
            }

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataRow[] list1 = ds.Tables[i].Select();
                for (int j = 0; j < list1.Length; j++)
                {
                    if (list1[j]["key"].ToString().Equals(key) &&
                        type != this.type) continue;
                    if (type == "System")
                    {
//                        if (list1[j]["key"].ToString().StartsWith(key))
                        if (list1[j]["key"].ToString().Equals(key) ||
                            (list1[j]["key"].ToString().StartsWith(key) &&
                            list1[j]["key"].ToString()[key.Length] == '/'))
                        {
                            ds.Tables[i].Rows.Remove(list1[j]);
                        }
                    }
                    else
                    {
                        if (list1[j]["key"].ToString().Equals(key))
                        {
                            ds.Tables[i].Rows.Remove(list1[j]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Delete EcellObject from this ObjectListOfType with full text matching.
        /// </summary>
        /// <param name="key">This key indicates an EcellObject to be deleted</param>
        public void DeleteObjectWithFull(string key)
        {
            if (key == null)
            {
                return;
            }

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataRow[] list1 = ds.Tables[i].Select();
                for (int j = 0; j < list1.Length; j++)
                {
                    if (list1[j]["key"].ToString().Equals(key))
                    {
                        ds.Tables[i].Rows.Remove(list1[j]);
                    }
                }
            }
        }

        /// <summary>
        /// action of disposing this object.
        /// </summary>
        public void Kill()
        {
            dgv.Dispose();
            ds.Dispose();
        }
    }
}
