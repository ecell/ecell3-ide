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
using Ecell.Exceptions;
using Ecell.Plugin;

namespace Ecell.IDE
{
    public partial class VariableCountDialog : Form, IVariableDialog
    {
        #region Fields
        /// <summary>
        /// Reference List.
        /// </summary>
        private List<EcellReference> m_refList = new List<EcellReference>();
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
        #endregion
        /// <summary>
        /// The List of EcellReference.
        /// </summary>
        public List<EcellReference> ReferenceList
        {
            get { return m_refList; }
        }

        /// <summary>
        /// Constructor for VariableRefWindow.
        /// </summary>
        public VariableCountDialog(DataManager dManager, PluginManager pManager, List<EcellReference> list)
        {
            m_dManager = dManager;
            m_pManager = pManager;
            InitializeComponent();
            m_refList = list;
            foreach (EcellReference er in list)
            {
                AddReference(er);
            }
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
                    node.ImageIndex = m_pManager.GetImageIndex(obj);
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
                        node.ImageIndex = m_pManager.GetImageIndex(obj);
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
                            node.ImageIndex = m_pManager.GetImageIndex(obj);
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
                            if (eo.LocalID == "SIZE") continue;
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
                            childNode.ImageIndex = m_pManager.GetImageIndex(eo);
                            childNode.SelectedImageIndex = childNode.ImageIndex;
                            childNode.Tag = eo.FullID;
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

        /// <summary>
        /// Create new Reference from key and prefix. 
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="prefix">prefix string</param>
        public void AddReference(string key, string prefix)
        {
            for (int i = 0; i < dgv.RowCount; i++)
            {
                if (((string)dgv[0, i].Value).EndsWith(key))
                {
                    Util.ShowWarningDialog(
                        string.Format(MessageResources.ErrExistVariableRef,
                        key));
                    return;
                }
            }

            dgv.Rows.Add(new object[] { key, 1});
        }

        /// <summary>
        /// Add new EcennReference.
        /// </summary>
        /// <param name="er">Reference object.</param>
        private void AddReference(EcellReference er)
        {
            dgv.Rows.Add(new object[] { er.FullID, er.Name });
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
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        public void AddVarButtonClick(object sender, EventArgs e)
        {
            m_selectWindow = new VariableSelectDialog(m_dManager, m_pManager);
            m_selectWindow.MassCalculation = true;
            CopyTreeView();
            m_selectWindow.SetParentWindow(this);

            using (m_selectWindow)
            {
                m_selectWindow.ShowDialog();
            }
        }
        /// <summary>
        /// Event on DialogClosing.
        /// </summary>
        /// <param name="sender">VariableReferenceEditDialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void VariableReferenceEditDialogClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                return;
            // Validate EcellReferences.
            try
            {
                ValidateReferences();
            }
            catch (EcellException ex)
            {
                Trace.WriteLine(ex);
                Util.ShowWarningDialog(ex.Message);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Validating the value of cell.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellValidatingEventArgs</param>
        private void DataCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string key;
                string fullID = (string)e.FormattedValue;
                if (fullID.StartsWith(Constants.xpathVariable))
                {
                    int ind = fullID.IndexOf(':');
                    key = fullID.Substring(ind + 1);
                }
                else
                {
                    key = fullID;
                }
            }
            else if (e.ColumnIndex == 1)
            {
                DataGridViewCell c = (DataGridViewCell)dgv[e.ColumnIndex, e.RowIndex];
                float dummy;
                if (!float.TryParse((string)e.FormattedValue, out dummy))
                {
                    e.Cancel = true;
                    dgv.CancelEdit();
                }
            }
        }

        /// <summary>
        /// Validatethe value of cell.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void DataCellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridViewCell c = (DataGridViewCell)dgv[e.ColumnIndex, e.RowIndex];
                float dummy;
                if (float.TryParse(c.Value.ToString(), out dummy))
                {
                    c.Value = dummy;
                }
            }
        }

        /// <summary>
        /// Load VariableReferenceEditDialog
        /// </summary>
        /// <param name="sender">VariableReferenceEditDialog</param>
        /// <param name="e">EventArgs</param>
        private void VariableReferenceEditDialog_Load(object sender, EventArgs e)
        {
            variableReferenceToolTip.SetToolTip(AddVarButton,
                MessageResources.DialogToolTipAddVar);
            variableReferenceToolTip.SetToolTip(DeleteVarButton,
                MessageResources.DialogToolTipDeleteVar);
        }
        #endregion

        /// <summary>
        /// Varidate Each EcellReference.
        /// </summary>
        private void ValidateReferences()
        {
            List<String> nameList = new List<string>();
            List<EcellReference> refList = new List<EcellReference>();
            for (int i = 0; i < this.dgv.RowCount; i++)
            {
                // Check FullID
                string fullID = (string)this.dgv[0, i].Value;
                if (string.IsNullOrEmpty(fullID))
                {
                    throw new EcellException(MessageResources.ErrInvalidID);
                }

                // Check Molar weight.
                float coef;
                try
                {
                    coef = (float)Convert.ToDouble(this.dgv[1, i].Value);
                }
                catch (Exception ex)
                {
                    throw new EcellException(MessageResources.ErrNoNumber, ex);
                }

                // Create EcellReference.
                EcellReference er = new EcellReference();
                er.Name = coef.ToString();
                er.FullID = fullID;
                er.Coefficient = 0;
                refList.Add(er);
            }
            // Set new list.
            this.m_refList = refList;
        }
    }
}