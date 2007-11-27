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
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib
{
    public partial class VariableRefWindow : Form
    {
        #region Fields
        /// <summary>
        /// the parent windows form.
        /// </summary>
        public PropertyEditor m_editor;
        /// <summary>
        /// variable select window for VariableReferenceList.
        /// </summary>
        VariableSelectWindow m_selectWindow;
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
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResLib));
        #endregion

        /// <summary>
        /// Constructor for VariableRefWindow.
        /// </summary>
        public VariableRefWindow()
        {
            InitializeComponent();

            m_dManager = DataManager.GetDataManager();
            m_pManager = PluginManager.GetPluginManager();
        }

        /// <summary>
        /// Update variable reference list property.
        /// </summary>
        public String GetVarReference()
        {
            List<String> nameList = new List<string>();
            string refStr = "(";

            for (int i = 0; i < this.dgv.RowCount; i++)
            {
                EcellReference v = new EcellReference();
                string name = (string)this.dgv[0, i].Value;
                if (nameList.Contains(name))
                {
                    String errmes = m_resources.GetString("ErrExistID");
                    MessageBox.Show(name + errmes, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                nameList.Add(name);
                v.name = (string)this.dgv[0, i].Value;
                v.fullID = (string)this.dgv[1, i].Value;
                try
                {
                    v.coefficient = Convert.ToInt32(this.dgv[2, i].Value);
                    v.isAccessor = Convert.ToInt32(this.dgv[3, i].Value);
                }
                catch (Exception)
                {
                    String errmes = m_resources.GetString("ErrNoNumber");
                    MessageBox.Show(errmes,
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                if (v.name == "")
                {
                    String errmes = m_resources.GetString("ErrInvalidName");
                    MessageBox.Show(errmes + "(Name)",
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                if (v.fullID == "" || !v.fullID.StartsWith(":"))
                {
                    String errmes = m_resources.GetString("ErrInvalidName");
                    MessageBox.Show(errmes + "(FullID)",
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                if (i == 0) refStr = refStr + v.ToString();
                else refStr = refStr + ", " + v.ToString();
            }
            refStr = refStr + ")";
            return refStr;
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
                if (obj.type == "Model")
                {
                    TreeNode node = new TreeNode(obj.modelID);
                    node.ImageIndex = m_pManager.GetImageIndex(obj.type);
                    node.SelectedImageIndex = node.ImageIndex;
                    m_prjNode.Nodes.Add(node);
                    dict.Add(obj.modelID, node);
                }
                else if (obj.type == "System")
                {
                    TreeNode node = null;
                    TreeNode current = dict[obj.modelID];
                    TreeNode target = null;
                    if (obj.key == "/")
                    {
                        node = new TreeNode(obj.key);
                        node.ImageIndex = m_pManager.GetImageIndex(obj.type);
                        node.SelectedImageIndex = node.ImageIndex;
                        current.Nodes.Add(node);
                    }
                    else
                    {
                        string path = "";
                        string[] elements;
                        elements = obj.key.Split(new Char[] { '/' });
                        for (int i = 1; i < elements.Length - 1; i++)
                        {
                            path = path + "/" + elements[i];
                        }
                        target = GetTargetTreeNode(current, path);

                        if (target != null)
                        {
                            node = new TreeNode(elements[elements.Length - 1]);
                            node.ImageIndex = m_pManager.GetImageIndex(obj.type);
                            node.SelectedImageIndex = node.ImageIndex;
                            target.Nodes.Add(node);
                        }
                    }
                    if (node != null)
                    {
                        if (obj.Children == null) continue;
                        foreach (EcellObject eo in obj.Children)
                        {
                            if (eo.type != "Variable") continue;
                            string[] names = eo.key.Split(new char[] { ':' });
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
                            childNode.ImageIndex = m_pManager.GetImageIndex(eo.type);
                            childNode.SelectedImageIndex = childNode.ImageIndex;
                            childNode.Tag = eo.key;
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

        #region Event
        /// <summary>
        /// Close variable reference list window.
        /// </summary>
        /// <param name="sender">Button(Cancel)</param>
        /// <param name="e">EventArgs</param>
        public void CloseVarReference(object sender, EventArgs e)
        {
            this.Dispose();
        }

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
        /// Update variable reference list property and close this window.
        /// </summary>
        /// <param name="sender">Button(Update)</param>
        /// <param name="e">EventArgs</param>
        public void OKVarReference(object sender, EventArgs e)
        {
            List<String> nameList = new List<string>();
            string refStr = "(";

            for (int i = 0; i < this.dgv.RowCount; i++)
            {
                EcellReference v = new EcellReference();
                string name = (string)this.dgv[0, i].Value;
                if (nameList.Contains(name))
                {
                    String errmes = m_resources.GetString("ErrExistID");
                    MessageBox.Show(name + errmes, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                nameList.Add(name);
                v.name = (string)this.dgv[0, i].Value;
                v.fullID = (string)this.dgv[1, i].Value;
                try
                {
                    v.coefficient = Convert.ToInt32(this.dgv[2, i].Value);
                    v.isAccessor = Convert.ToInt32(this.dgv[3, i].Value);
                }
                catch (Exception)
                {
                    String errmes = m_resources.GetString("ErrNoNumber");
                    MessageBox.Show(errmes,
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (v.name == "")
                {
                    String errmes = m_resources.GetString("ErrInvalidName");
                    MessageBox.Show(errmes + "(Name)",
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (v.fullID == "" || !v.fullID.StartsWith(":"))
                {
                    String errmes = m_resources.GetString("ErrInvalidName");
                    MessageBox.Show(errmes + "(FullID)",
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (i == 0) refStr = refStr + v.ToString();
                else refStr = refStr + ", " + v.ToString();
            }
            refStr = refStr + ")";
            m_editor.m_refStr = refStr;

            this.Dispose();
        }

        /// <summary>
        /// Update variable reference list property.
        /// </summary>
        /// <param name="sender">Button(Update)</param>
        /// <param name="e">EventArgs</param>
        public void ApplyVarReference(object sender, EventArgs e)
        {
            List<String> nameList = new List<string>();
            string refStr = "(";

            for (int i = 0; i < this.dgv.RowCount; i++)
            {
                EcellReference v = new EcellReference();
                string name = (string)this.dgv[0, i].Value;
                if (nameList.Contains(name))
                {
                    String errmes = m_resources.GetString("ErrExistID");
                    MessageBox.Show(name + errmes, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                nameList.Add(name);
                v.name = (string)this.dgv[0, i].Value;
                v.fullID = (string)this.dgv[1, i].Value;
                try
                {
                    v.coefficient = Convert.ToInt32(this.dgv[2, i].Value);
                    v.isAccessor = Convert.ToInt32(this.dgv[3, i].Value);
                }
                catch (Exception)
                {
                    String errmes = m_resources.GetString("ErrNoNumber");
                    MessageBox.Show(errmes,
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (v.name == "")
                {
                    String errmes = m_resources.GetString("ErrInvalidName");
                    MessageBox.Show(errmes + "(Name)",
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (v.fullID == "" || !v.fullID.StartsWith(":"))
                {
                    String errmes = m_resources.GetString("ErrInvalidName");
                    MessageBox.Show(errmes + "(FullID)",
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (i == 0) refStr = refStr + v.ToString();
                else refStr = refStr + ", " + v.ToString();
            }
            refStr = refStr + ")";
            m_editor.m_refStr = refStr;
        }

        /// <summary>
        /// Add new variable reference.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddVarReference(object sender, EventArgs e)
        {
            m_selectWindow = new VariableSelectWindow();
            m_selectWindow.VSProductButton.Click += new EventHandler(m_selectWindow.ProductButtonClick);
            m_selectWindow.VSSourceButton.Click += new EventHandler(m_selectWindow.SourceButtonClick);
            m_selectWindow.VSConstantButton.Click += new EventHandler(m_selectWindow.ConstantButtonClick);
            m_selectWindow.VSCloseButton.Click += new EventHandler(m_selectWindow.SelectCancelButtonClick);

            CopyTreeView();
            m_selectWindow.SetParentWindow(this);

            m_selectWindow.ShowDialog();
        }
        #endregion
    }
}