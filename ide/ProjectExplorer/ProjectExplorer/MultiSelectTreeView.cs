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

namespace Ecell.IDE.Plugins.ProjectExplorer
{
    /// <summary>
    /// TreeView to be able to select the multi objects.
    /// </summary>
    public class MultiSelectTreeView : System.Windows.Forms.TreeView
    {
        #region Fields
        private Point ptMouseDown;
        private TreeNode tnMouseDown = null;
        private bool m_isUpdate = false;
        private ApplicationEnvironment m_env;
        private List<TreeNode> m_selected = new List<TreeNode>();
        private bool m_isDrag = false;
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
        /// 
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            set { m_env = value; }
        }

        /// <summary>
        /// Get the selected nodes.
        /// </summary>
        public List<TreeNode> SelNodes
        {
            get { return m_selected; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDrag
        {
            get { return this.m_isDrag; }
            set { this.m_isDrag = value; }
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
        /// <param name="isdispatch"></param>
        public void SelectNodes(TreeNode tn, bool isdispatch)
        {
            if (m_isUpdate)
                return;
            if (!this.SelNodes.Contains(tn))
                this.SelNodes.Add(tn);

            HighLight(tn);
            if (!isdispatch)
                return;

            if (tn.Tag != null)
            {
                TagData t = tn.Tag as TagData;
                if (t != null)
                {
                    m_isUpdate = true;
                    m_env.PluginManager.AddSelect(t.ModelID, t.Key, t.Type);
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
            if (m_isUpdate)
                return;
            ClearSelNode();
            if (!this.SelNodes.Contains(tn))
                this.SelNodes.Add(tn);

            this.SelectedNode = null;
            HighLight(tn);

            if (isScroll)
                tn.EnsureVisible();

            m_RangeNode = null;
            if (isChanged)
                return;
            if (tn.Tag != null)
            {
                TagData t = tn.Tag as TagData;
                if (t != null)
                {
                    m_isUpdate = true;
                    m_env.PluginManager.SelectChanged(t.ModelID, t.Key, t.Type);
                    m_isUpdate = false;
                }
            }            
        }

        /// <summary>
        /// Deselect the selected node.
        /// </summary>
        /// <param name="tn">the selected node.</param>
        /// <param name="isdispatch"></param>
        public void DeselectNode(TreeNode tn, bool isdispatch)
        {
            if (m_isUpdate)
                return;
            if (this.SelNodes.Contains(tn))
                this.SelNodes.Remove(tn);
            LowlightNode(tn);

            if (!isdispatch)
                return;

            if (tn.Tag != null)
            {
                TagData t = tn.Tag as TagData;
                if (t != null)
                {
                    m_isUpdate = true;
                    m_env.PluginManager.RemoveSelect(t.ModelID, t.Key, t.Type);
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
                        SelectNodes(tn, true);
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
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                if (IsTreeNodeSelected(tn))
                {
                    DeselectNode(tn, true);
                }
                else
                {
                    SelectNodes(tn, true);
                }
            }
            else if ((Control.ModifierKeys & Keys.Shift) != 0)
            {
            }
            else
            {
                SelectNode(tn, false, false);
            }
        }

        private void SelectRange(TreeNode tn, bool bPrev)
        {
            m_env.PluginManager.ResetSelect();
            TreeNode tnTemp = tnMouseDown;
            if (tnTemp == null) return;
            SelectNodes(tnTemp, true);
            if (bPrev)
            {
                while (tnTemp.PrevVisibleNode != null && tnTemp != tn)
                {
                    SelectNodes(tnTemp.PrevVisibleNode, true);
                    tnTemp = tnTemp.PrevVisibleNode;
                }
            }
            else
            {
                while (tnTemp.NextVisibleNode != null && tnTemp != tn)
                {
                    SelectNodes(tnTemp.NextVisibleNode, true);
                    tnTemp = tnTemp.NextVisibleNode;
                }
            }
            this.Refresh();
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
            if ((Control.ModifierKeys & Keys.Shift) != 0)
            {
                bool bStartPainting = Math.Abs(ptMouseDown.Y - e.Y) > this.ItemHeight;
                TreeNode tn = this.GetNodeAt(e.X, e.Y);
                bool bPrev = ptMouseDown.Y - e.Y > 0;
                if (e.Button == MouseButtons.Left &&
                    (bStartPainting || (tn != tnMouseDown && tn != null)))
                {
                    SelectRange(tn, bPrev);
                }
                return;
            }
            m_isDrag = false;
            tnMouseDown = this.GetNodeAt(e.X, e.Y);
            ptMouseDown = new Point(e.X, e.Y);
            m_isCollapse = false;
            m_isExpand = false;
            SelectNode(tnMouseDown, false, false);
            base.OnMouseDown(e);
        }

        private bool m_isCollapse = false;
        private bool m_isExpand = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_isExpand = false;
            m_isCollapse = false;
            base.OnMouseUp(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
        {
            m_isCollapse = true; ;
            base.OnBeforeCollapse(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            base.OnAfterCollapse(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            m_isExpand = true;
            base.OnBeforeExpand(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_isDrag) return;
            bool bStartPainting = Math.Abs(ptMouseDown.Y - e.Y) > this.ItemHeight;
            TreeNode tn = this.GetNodeAt(e.X, e.Y);
            bool bPrev = ptMouseDown.Y - e.Y > 0;
            
            if (e.Button == MouseButtons.Left &&
                (bStartPainting || (tn != tnMouseDown && tn != null)))
            {
                if (!m_isExpand && !m_isCollapse)
                    SelectRange(tn, bPrev);
            }
            base.OnMouseMove(e);
        }

        private bool m_isDownEvent = false;
        private TreeNode m_RangeNode = null;
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (m_isDownEvent)
            {
                e.Handled = true;
                return;
            }
            if (e.Shift == true && e.KeyCode == Keys.Up)
            {
                if (tnMouseDown == null)
                {
                    e.Handled = true;
                    return;
                }
                if (m_RangeNode == null)
                {
                    if (tnMouseDown.PrevVisibleNode != null)
                        m_RangeNode = tnMouseDown.PrevVisibleNode;
                }
                else
                {
                    if (m_RangeNode.PrevVisibleNode != null)
                        m_RangeNode = m_RangeNode.PrevVisibleNode;
                }
                e.Handled = true;
                if (m_RangeNode == null)
                    return;
                bool isPrev = true;
                if (m_RangeNode.Bounds.Y > tnMouseDown.Bounds.Y)
                    isPrev = false;
                SelectRange(m_RangeNode, isPrev);
            }
            else if (e.Shift == true && e.KeyCode == Keys.Down)
            {
                if (tnMouseDown == null)
                {
                    e.Handled = true;
                    return;
                }
                if (m_RangeNode == null)
                {
                    if (tnMouseDown.NextVisibleNode != null)
                        m_RangeNode = tnMouseDown.NextVisibleNode;
                }
                else
                {
                    if (m_RangeNode.NextVisibleNode != null)
                        m_RangeNode = m_RangeNode.NextVisibleNode;
                }
                e.Handled = true;
                if (m_RangeNode == null)
                    return;
                bool isPrev = true;
                if (m_RangeNode.Bounds.Y > tnMouseDown.Bounds.Y)
                    isPrev = false;
                SelectRange(m_RangeNode, isPrev);
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (tnMouseDown == null)
                {
                    e.Handled = true;
                    return;
                }
                TreeNode unse = tnMouseDown;
                if (unse.Parent == null)
                {
                    e.Handled = true;
                    return;
                }
                else
                {
                    tnMouseDown = tnMouseDown.Parent;
                }

                m_isDownEvent = true;
                ptMouseDown = new Point(tnMouseDown.Bounds.X, tnMouseDown.Bounds.Y);
                DeselectNode(unse, true);
                SelectNode(tnMouseDown, false, false);
                e.Handled = true;
                m_isDownEvent = false;
                return;
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (tnMouseDown == null)
                {
                    e.Handled = true;
                    return;
                }
                TreeNode unse = tnMouseDown;
                if (unse.FirstNode == null)
                {
                    e.Handled = true; 
                    return;
                }
                else
                {
                    tnMouseDown = tnMouseDown.FirstNode;
                }

                m_isDownEvent = true;
                ptMouseDown = new Point(tnMouseDown.Bounds.X, tnMouseDown.Bounds.Y);
                DeselectNode(unse, true);
                SelectNode(tnMouseDown, false, false);
                e.Handled = true;
                m_isDownEvent = false;
                return;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (tnMouseDown == null)
                {
                    e.Handled = true;
                    return;
                }
                TreeNode unse = tnMouseDown;
                if (unse.PrevVisibleNode == null)
                {
                    e.Handled = true;
                    return;
                    //if (unse.PrevNode == null)
                    //{
                    //    if (unse.Parent == null)
                    //    {
                    //        e.Handled = true;
                    //        return;
                    //    }
                    //    tnMouseDown = unse.Parent;
                    //}
                    //else
                    //{
                    //    tnMouseDown = tnMouseDown.PrevNode;
                    //}
                }
                else
                {
                    tnMouseDown = unse.PrevVisibleNode;
                }

                m_isDownEvent = true;
                ptMouseDown = new Point(tnMouseDown.Bounds.X, tnMouseDown.Bounds.Y);
                DeselectNode(unse, true);
                SelectNode(tnMouseDown, false, false);
                e.Handled = true;
                m_isDownEvent = false;
                return;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (tnMouseDown == null)
                {
                    e.Handled = true;
                    return;
                }
                TreeNode unse = tnMouseDown;
                if (unse.NextVisibleNode == null)
                {
                    e.Handled = true;
                    return;
                    //if (unse.NextNode == null)
                    //{
                    //    if (unse.Parent == null)
                    //    {
                    //        e.Handled = true;
                    //        return;
                    //    }
                    //    if (unse.Parent.NextNode == null)
                    //    {
                    //        e.Handled = true;
                    //        return;
                    //    }
                    //    tnMouseDown = unse.Parent.NextNode;
                    //}
                    //else
                    //{
                    //    tnMouseDown = tnMouseDown.NextNode;
                    //}
                }
                else
                {
                    tnMouseDown = tnMouseDown.NextVisibleNode;
                }

                m_isDownEvent = true;
                ptMouseDown = new Point(tnMouseDown.Bounds.X, tnMouseDown.Bounds.Y);
                DeselectNode(unse, true);
                SelectNode(tnMouseDown, false, false);
                e.Handled = true;
                m_isDownEvent = false;
                return;
            }

            base.OnKeyDown(e);
        }
        #endregion
    }
}


