//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Ecell.Objects;

namespace Ecell.Action
{
    /// <summary>
    /// Class to manager actions.
    /// </summary>
    public class ActionManager
    {
        #region Fields
        /// <summary>
        /// DataManager
        /// </summary>
        private ApplicationEnvironment m_env;
        /// <summary>
        /// List of user action on IDE.
        /// </summary>
        private List<UserAction> m_list;
        /// <summary>
        /// An index of m_list for undoing and redoing.
        /// </summary>
        private int m_listIndex;
        /// <summary>
        /// Whether duaring load the actions or others.
        /// </summary>
        private bool m_isLoadAction = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for ActionManager.
        /// </summary>
        public ActionManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_list = new List<UserAction>();
        }

        #endregion

        #region Accessors
        /// <summary>
        /// Get the count of UserAction.
        /// </summary>
        public int Count
        {
            get { return m_list.Count; }
        }
        /// <summary>
        /// UndoStatus
        /// </summary>
        public UndoStatus UndoStatus
        {
            get
            {
                bool undoable = this.Undoable;
                bool redoable = this.Redoable;
                // Set status.
                UndoStatus status;
                if (undoable && redoable)
                    status = UndoStatus.UNDO_REDO;
                else if (undoable)
                    status = UndoStatus.UNDO_ONLY;
                else if (redoable)
                    status = UndoStatus.REDO_ONLY;
                else
                    status = UndoStatus.NOTHING;

                return status;
            }
        }

        /// <summary>
        /// Get the Undoable status
        /// </summary>
        public bool Undoable
        {
            get
            {
                if (0 < m_listIndex && m_list[m_listIndex - 1].IsUndoable)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Get the Redoable status
        /// </summary>
        public bool Redoable
        {
            get
            {
                if (m_listIndex < m_list.Count)
                    return true;
                else
                    return false;

            }
        }

        /// <summary>
        /// Whether duaring load the actions or others.
        /// </summary>
        public bool IsLoadAction
        {
            get { return m_isLoadAction; }
        }
        #endregion

        #region EventHandler for UndoStatusChanged
        private UndoStatusChangedEvent m_onUndoStatusChanged;
        /// <summary>
        /// Event on UndoStatus change.
        /// </summary>
        public event UndoStatusChangedEvent UndoStatusChanged
        {
            add { m_onUndoStatusChanged += value; }
            remove { m_onUndoStatusChanged -= value; }
        }
        /// <summary>
        /// Event on UndoStatus change.
        /// </summary>
        /// <param name="e">UndoStatusChangedEventArgs</param>
        protected virtual void OnUndoStatusChanged(UndoStatusChangedEventArgs e)
        {
            if (m_onUndoStatusChanged != null)
                m_onUndoStatusChanged(this, e);
        }
        /// <summary>
        /// Raise the UndoStatusChangedEvent.
        /// </summary>
        private void RaiseUndoStatusChanged()
        {
            UndoStatusChangedEventArgs e = new UndoStatusChangedEventArgs(UndoStatus);
            OnUndoStatusChanged(e);
        }
        #endregion

        /// <summary>
        /// Add the UserAction to ActionManager.
        /// </summary>
        /// <param name="u">The adding UserAction.</param>
        public void AddAction(UserAction u)
        {
            if (m_isLoadAction == true)
                return;

            u.Environment = m_env;

            if (m_list.Count > m_listIndex)
                m_list.RemoveRange(m_listIndex, m_list.Count - m_listIndex);
            
            m_list.Add(u);
            m_listIndex++;
            NotifyStatus();
        }

        /// <summary>
        /// Get the UserAction from ActionManager by index.
        /// </summary>
        /// <param name="index">The index of UserAction.</param>
        /// <returns>The target UserAction.</returns>
        public UserAction GetAction(int index)
        {
            if (index > m_list.Count || index < 0)
                return null;
            return m_list[index];
        }

        /// <summary>
        /// Clear the UserAction of ActionManager.
        /// </summary>
        public void Clear()
        {
            m_list.Clear();
            m_listIndex = 0;
            NotifyStatus();
        }

        /// <summary>
        /// Undo action.
        /// </summary>
        public void UndoAction()
        {
            if (m_listIndex == 0 || !m_list[m_listIndex - 1].IsUndoable)
                return;
            m_isLoadAction = true;
            NotifyStatus();
            do
            {
                m_list[m_listIndex - 1].UnExecute();
                //Console.WriteLine("index:" + (m_listIndex - 1) + " unexecute");
                m_listIndex--;

            } while (0 < m_listIndex && !m_list[m_listIndex - 1].IsAnchor);
            m_isLoadAction = false;
            NotifyStatus();
            m_env.PluginManager.RaiseRefreshEvent();
        }

        /// <summary>
        /// Redo action.
        /// </summary>
        public void RedoAction()
        {
            if (m_list.Count <= m_listIndex)
                return;
            m_isLoadAction = true;
            NotifyStatus();
            do
            {
                m_list[m_listIndex].Execute();
                //Console.WriteLine("index:" + m_listIndex + " execute");
                m_listIndex++;

            } while (m_listIndex < m_list.Count && !m_list[m_listIndex - 1].IsAnchor);
            m_isLoadAction = false;
            NotifyStatus();
            m_env.PluginManager.RaiseRefreshEvent();
        }

        /// <summary>
        /// Notify undoable/redoable status to PluginManager
        /// </summary>
        private void NotifyStatus()
        {
            RaiseUndoStatusChanged();
        }
    }
}