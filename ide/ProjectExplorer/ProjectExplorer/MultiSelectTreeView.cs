//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.EntityListWindow
{
    /// <summary>
    /// TreeView to be able to select the multi objects.
    /// </summary>
    public class MultiSelectTreeView : System.Windows.Forms.TreeView
    {
        #region Fields
        private bool m_isUpdate = false;
        private ApplicationEnvironment m_env = ApplicationEnvironment.GetInstance();
        private List<TreeNode> m_selected = new List<TreeNode>();
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Constructor.
        /// </summary>
        public MultiSelectTreeView()
        {
            this.SelectedNode = null;
        }
        #endregion

        #region ACCESSORS
        /// <summary>
        /// Get the selected nodes.
        /// </summary>
        public List<TreeNode> SelNodes
        {
            get { return m_selected; }
        }
        #endregion

        /// <summary>
        /// Show the node by Highlight.
        /// </summary>
        /// <param name="tn">the selected node.</param>
        private void HighLight(TreeNode tn)
        {
            if (tn.BackColor != SystemColors.Highlight)
            {
                tn.BackColor = SystemColors.Highlight;
            }

            if (tn.ForeColor != SystemColors.HighlightText)
            {
                tn.ForeColor = SystemColors.HighlightText;
            }
        }

        /// <summary>
        /// Show the node by Lowlight.
        /// </summary>
        /// <param name="tn">the selected node.</param>
        private void LowlightNode(TreeNode tn)
        {
            if (tn.BackColor != this.BackColor)
            {
                tn.BackColor = this.BackColor;
            }

            if (tn.ForeColor != this.ForeColor)
            {
                tn.ForeColor = this.ForeColor;
            }
        }

        /// <summary>
        /// Clear the all selected information.
        /// </summary>
        public void ClearSelNode()
        {
            foreach (TreeNode tn in this.SelNodes)
            {
                LowlightNode(tn);
            }
            this.SelNodes.Clear();
        }

        /// <summary>
        /// Add the selected node.
        /// </summary>
        /// <param name="tn">the selected node.</param>
        public void SelectNodes(TreeNode tn)
        {
            if (m_isUpdate) return;
            if (!this.SelNodes.Contains(tn))
                this.SelNodes.Add(tn);

            HighLight(tn);
            if (tn.Tag != null)
            {
                TagData t = tn.Tag as TagData;
                if (t != null)
                {
                    m_isUpdate = true;
                    m_env.PluginManager.AddSelect(t.m_modelID, t.m_key, t.m_type);
                    m_isUpdate = false;
                }
            }
        }

        /// <summary>
        /// Set the selected node.
        /// </summary>
        /// <param name="tn">the selected node.</param>
        /// <param name="isChanged">the flag whether SelectChanged is executed.</param>
        /// <param name="isScroll">the flag whether EnsureVisible is executed.</param>
        public void SelectNode(TreeNode tn, bool isChanged, bool isScroll)
        {
            if (m_isUpdate) return;
            ClearSelNode();
            if (!this.SelNodes.Contains(tn))
                this.SelNodes.Add(tn);

            this.SelectedNode = null;
            HighLight(tn);

            if (isScroll) tn.EnsureVisible();
            if (isChanged) return;
            if (tn.Tag != null)
            {
                TagData t = tn.Tag as TagData;
                if (t != null)
                {
                    m_isUpdate = true;
                    m_env.PluginManager.SelectChanged(t.m_modelID, t.m_key, t.m_type);
                    m_isUpdate = false;
                }
            }
        }

        /// <summary>
        /// Deselect the selected node.
        /// </summary>
        /// <param name="tn">the selected node.</param>
        public void DeselectNode(TreeNode tn)
        {
            if (m_isUpdate) return;
            if (this.SelNodes.Contains(tn))
                this.SelNodes.Remove(tn);
            LowlightNode(tn);

            if (tn.Tag != null)
            {
                TagData t = tn.Tag as TagData;
                if (t != null)
                {
                    m_isUpdate = true;
                    m_env.PluginManager.RemoveSelect(t.m_modelID, t.m_key, t.m_type);
                    m_isUpdate = false;
                }
            }

            if (this.SelectedNode == tn)
                EnsureSelNodeNotNull();
        }

        /// <summary>
        /// Ensure the selected node.
        /// </summary>
        private void EnsureSelNodeNotNull()
        {
            if (this.SelNodes != null)
            {
                if (this.SelNodes.Count > 0)
                {
                    foreach (TreeNode tn in this.SelNodes)
                    {
                        SelectNodes(tn);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Check whether the node is selected.
        /// </summary>
        /// <param name="tn">the check node.</param>
        /// <returns>the flag whether the node is selected</returns>
        private bool IsTreeNodeSelected(TreeNode tn)
        {
            bool bRetVal = false;
            if (this.SelNodes != null)
            {
                if (this.SelNodes.Contains(tn))
                {
                    bRetVal = true;
                }
            }
            return bRetVal;
        }

        /// <summary>
        /// Before the node is selected, system check the current status.
        /// </summary>
        /// <param name="tn">the selected node.</param>
        private void BeforeSelectMethod(TreeNode tn)
        {
            bool isSystem = false;
            if (tn.Tag != null)
            {
                TagData td = tn.Tag as TagData;
                if (td != null)
                {
                    if (td.m_type.Equals(Constants.xpathSystem))
                        isSystem = true;
                }
            }
            if ((Control.ModifierKeys & Keys.Control) != 0 && !isSystem)
            {
                if (IsTreeNodeSelected(tn))
                {
                    DeselectNode(tn);
                }
                else
                {
                    SelectNodes(tn);
                }
            }
            else
            {
                SelectNode(tn, false, false);
            }
        }

        #region Events
        /// <summary>
        /// The event sequence when the node is selected.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown && e.Action != TreeViewAction.ByKeyboard)
            {
                BeforeSelectMethod(e.Node);
                e.Cancel = true;
                if (!IsTreeNodeSelected(e.Node))
                {
                    EnsureSelNodeNotNull();
                }
            }
            base.OnBeforeSelect(e);
        }

        /// <summary>
        /// The event sequence when mouse is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }
        #endregion
    }
}


