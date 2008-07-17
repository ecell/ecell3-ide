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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.Objects;

namespace Ecell.IDE
{
    public partial class VariableReferenceEditDialog : Form
    {
        #region Fields
        private string m_refStr = "";
        private string m_errMsg = "";
        /// <summary>
        /// variable select window for VariableReferenceList.
        /// </summary>
        VariableSelectDialog m_selectWindow;
        /// <summary>
        /// DataManager.
        /// </summary>
        DataManager m_dManager;
        /// <summary>
        /// PluginManager.
        /// </summary>
        PluginManager m_pManager;
        /// <summary>
        /// ResourceManager for VariableRefWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResources));
        #endregion

        public string ReferenceString
        {
            get { return m_refStr; }
            set { this.m_refStr = value; }
        }

        /// <summary>
        /// Constructor for VariableRefWindow.
        /// </summary>
        public VariableReferenceEditDialog(DataManager dManager, PluginManager pManager)
        {
            InitializeComponent();

            m_dManager = dManager;
            m_pManager = pManager;
        }

        /// <summary>
        /// Create the TreeView from data.
        /// </summary>
        void CopyTreeView()
        {
            Dictionary<string, TreeNode> dict = new Dictionary<string, TreeNode>();
            TreeNode m_prjNode = new TreeNode(Constants.defaultPrjID);
            m_selectWindow.selectTree.Nodes.Add(m_prjNode);
            List<EcellObject> objList = m_dManager.GetData(null, null);
            foreach (EcellObject obj in objList)
            {
                if (obj.Type == "Model")
                {
                    TreeNode node = new TreeNode(obj.ModelID);
                    node.ImageIndex = m_pManager.GetImageIndex(obj.Type);
                    node.SelectedImageIndex = node.ImageIndex;
                    m_prjNode.Nodes.Add(node);
                    dict.Add(obj.ModelID, node);
                }
                else if (obj.Type == "System")
                {
                    TreeNode node = null;
                    TreeNode current = dict[obj.ModelID];
                    TreeNode target = null;
                    if (obj.Key == "/")
                    {
                        node = new TreeNode(obj.Key);
                        node.ImageIndex = m_pManager.GetImageIndex(obj.Type);
                        node.SelectedImageIndex = node.ImageIndex;
                        current.Nodes.Add(node);
                    }
                    else
                    {
                        string path = "";
                        string[] elements;
                        elements = obj.Key.Split(new Char[] { '/' });
                        for (int i = 1; i < elements.Length - 1; i++)
                        {
                            path = path + "/" + elements[i];
                        }
                        target = GetTargetTreeNode(current, path);

                        if (target != null)
                        {
                            node = new TreeNode(elements[elements.Length - 1]);
                            node.ImageIndex = m_pManager.GetImageIndex(obj.Type);
                            node.SelectedImageIndex = node.ImageIndex;
                            target.Nodes.Add(node);
                        }
                    }
                    if (node != null)
                    {
                        if (obj.Children == null) continue;
                        foreach (EcellObject eo in obj.Children)
                        {
                            if (eo.Type != "Variable") continue;
                            string[] names = eo.Key.Split(new char[] { ':' });
                            IEnumerator iter = node.Nodes.GetEnumerator();
                            bool isHit = false;
                            while (iter.MoveNext())
                            {
                                TreeNode tmp = (TreeNode)iter.Current;
                                if (tmp.Text == names[names.Length - 1])
                                {
                                    isHit = true;
                                    break;
                                }
                            }
                            if (isHit == true) continue;

                            TreeNode childNode = new TreeNode(names[names.Length - 1]);
                            childNode.ImageIndex = m_pManager.GetImageIndex(eo.Type);
                            childNode.SelectedImageIndex = childNode.ImageIndex;
                            childNode.Tag = eo.Key;
                            node.Nodes.Add(childNode);
                        }
                    }
                }
            }
            m_selectWindow.selectTree.ExpandAll();

        }

        /// <summary>
        /// Get the TreeNode using key.
        /// </summary>
        /// <param name="current">current model TreeNode.</param>
        /// <param name="key">searching key.</param>
        /// <returns>TreeNode</returns>
        public TreeNode GetTargetTreeNode(TreeNode current, string key)
        {
            string[] keydata;
            if (current.Nodes.Count <= 0) return null;

            keydata = key.Split(new Char[] { '/' });
            if (keydata.Length == 0) return null;

            if (keydata[0].Contains(":"))
            {
                string[] keys = keydata[0].Split(new char[] { ':' });
                if (keydata[0].StartsWith(":")) keydata[0] = keys[keys.Length - 1];
                else keydata = keydata[0].Split(new char[] { ':' });
            }

            IEnumerator nodes = current.Nodes.GetEnumerator();
            while (nodes.MoveNext())
            {
                TreeNode node = (TreeNode)nodes.Current;
                if (node.Text == keydata[0] || (keydata[0] == "" && node.Text == "/"))
                {
                    if (keydata.Length == 1 || key == "/") return node;
                    key = keydata[1];
                    for (int i = 2; i < keydata.Length; i++)
                    {
                        key = key + "/" + keydata[i];
                    }
                    return GetTargetTreeNode(node, key);
                }
            }
            return null;
        }

        public void AddReference(string key, string prefix)
        {
            int j = 0;
            int p = 0;
            if (prefix.Equals("P"))
                p = 1;
            else if (prefix.Equals("S"))
                p = -1;

            string id;
            while (true)
            {
                id = prefix + j;
                bool isHit = false;
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    if (id == (string)dgv[0, i].Value)
                    {
                        isHit = true;
                        break;
                    }
                }
                if (isHit == false)
                {
                    break;
                }
                j++;
            }

            dgv.Rows.Add(new object[] { id, key, p , true });
        }

        public void AddReference(string name, string key, int coeff, bool isAccessor)
        {
            string id = key;
            if (!key.StartsWith(":")) id = ":" + key;
            dgv.Rows.Add(new object[] { name, id, coeff, isAccessor });
        }

        #region Event
        /// <summary>
        /// Delete the selected variable reference.
        /// </summary>
        /// <param name="sender">Button(Delete)</param>
        /// <param name="e">EventArgs</param>
        public void DeleteVarReference(object sender, EventArgs e)
        {
            DataGridViewRow row = this.dgv.CurrentRow;
            if (row == null) return;
            this.dgv.Rows.Remove(row);
        }

        /// <summary>
        /// Add new variable reference.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddVarReference(object sender, EventArgs e)
        {
            m_selectWindow = new VariableSelectDialog(m_dManager, m_pManager);
            CopyTreeView();
            m_selectWindow.SetParentWindow(this);

            using (m_selectWindow)
            {
                m_selectWindow.ShowDialog();
            }
        }

        private void VariableReferenceEditDialogClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(m_errMsg))
            {
                Util.ShowWarningDialog(m_errMsg);
                e.Cancel = false;
                m_errMsg = "";
            }
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            List<String> nameList = new List<string>();
            string refStr = "(";

            for (int i = 0; i < this.dgv.RowCount; i++)
            {
                EcellReference v = new EcellReference();
                string name = (string)this.dgv[0, i].Value;
                if (nameList.Contains(name))
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrExistVariableRef,
                        new object[] { name }));
                    return;
                }
                nameList.Add(name);
                v.Name = (string)this.dgv[0, i].Value;
                v.FullID = (string)this.dgv[1, i].Value;
                try
                {
                    v.Coefficient = Convert.ToInt32(this.dgv[2, i].Value);
                    v.IsAccessor = 1;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    m_errMsg = MessageResources.ErrNoNumber;
                    return;
                }
                if (v.Name == "")
                {
                    m_errMsg = MessageResources.ErrInvalidID;
                    return;
                }
                if (v.FullID == "" || !v.FullID.StartsWith(":"))
                {
                    m_errMsg = MessageResources.ErrInvalidID;
                    return;
                }

                if (i == 0) refStr = refStr + v.ToString();
                else refStr = refStr + ", " + v.ToString();
            }
            refStr = refStr + ")";
            m_refStr = refStr;
        }
        #endregion
    }
}