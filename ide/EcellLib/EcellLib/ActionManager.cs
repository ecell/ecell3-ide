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
//

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Ecell.Objects;

namespace Ecell
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
        private bool isLoadAction = false;
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
        #endregion

        #region EventHandler for UndoableChange
        private EventHandler m_onUndoableChange;
        /// <summary>
        /// Event on Undoable change.
        /// </summary>
        public event EventHandler UndoableChange
        {
            add { m_onUndoableChange += value; }
            remove { m_onUndoableChange -= value; }
        }
        /// <summary>
        /// Event on Undoable change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUndoableChange(EventArgs e)
        {
            if (m_onUndoableChange != null)
                m_onUndoableChange(this, e);
        }
        private void RaiseUndoableChange()
        {
            EventArgs e = new EventArgs();
            OnUndoableChange(e);
        }
        #endregion

        #region EventHandler for RedoableChange
        private EventHandler m_onRedoableChange;
        /// <summary>
        /// Event on Redoable change.
        /// </summary>
        public event EventHandler RedoableChange
        {
            add { m_onRedoableChange += value; }
            remove { m_onRedoableChange -= value; }
        }
        /// <summary>
        /// Event on Redoable change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRedoableChange(EventArgs e)
        {
            if (m_onRedoableChange != null)
                m_onRedoableChange(this, e);
        }
        private void RaiseRedoableChange()
        {
            EventArgs e = new EventArgs();
            OnRedoableChange(e);
        }
        #endregion

        /// <summary>
        /// Add the UserAction to ActionManager.
        /// </summary>
        /// <param name="u">The adding UserAction.</param>
        public void AddAction(UserAction u)
        {
            if (isLoadAction == true) return;

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
            if (index > m_list.Count || index < 0) return null;
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
        /// Load the information of UserAction and 
        /// execute the UserActions.
        /// </summary>
        /// <param name="fileName">File name of UserActions.</param>
        public void LoadActionFile(string fileName)
        {
            isLoadAction = true;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlNodeList topList = doc.SelectNodes("ActionList");
            foreach (XmlNode top in topList)
            {
                XmlNodeList commandList = top.SelectNodes("Action");

                foreach (XmlNode child in commandList)
                {
                    XmlNode cNode = child.Attributes.GetNamedItem("command");
                    if (cNode == null) continue;
                    string command = cNode.InnerText;
                    UserAction act = null;
                    if (command.Equals("DataAdd")) act = new DataAddAction();
                    else if (command.Equals("DataDelete")) act = new DataDeleteAction();
                    else if (command.Equals("DataChanged")) act = new DataChangeAction();
                    else if (command.Equals("ImportModel")) act = new ImportModelAction();
                    else if (command.Equals("NewProject")) act = new NewProjectAction();
                    else if (command.Equals("LoadProject")) act = new LoadProjectAction();
                    else if (command.Equals("AddStepper")) act = new AddStepperAction();
                    else if (command.Equals("DeleteStepper")) act = new DeleteStepperAction();
                    else if (command.Equals("UpdateStepper")) act = new UpdateStepperAction();
                    else if (command.Equals("NewSimParam")) act = new NewSimParamAction();
                    else if (command.Equals("DeleteSimParam")) act = new DeleteSimParamAction();
                    else if (command.Equals("SetSimParam")) act = new SetSimParamAction();

                    if (act == null)
                    {
                        continue;
                    }

                    act.Environment = m_env;
                    act.LoadScript(child);
                    m_list.Add(act);
                }
            }
            doc = null;

            foreach (UserAction u in m_list)
                u.Execute();
            isLoadAction = false;

            m_listIndex = m_list.Count;
        }

        /// <summary>
        /// Save the information of UserAction to the file.
        /// </summary>
        /// <param name="fileName">Saved file name.</param>
        public void SaveActionFile(string fileName)
        {
            TextWriter streamWriter =
                                new StreamWriter(fileName);
            XmlTextWriter w = new XmlTextWriter(streamWriter);
            w.Formatting = Formatting.Indented;

            w.WriteStartElement("ActionList");
            foreach (UserAction u in m_list)
                u.SaveScript(w);
            w.WriteEndElement();

            w.Close();
            streamWriter.Close();
        }

        /// <summary>
        /// Undo action.
        /// </summary>
        public void UndoAction()
        {
            if (m_listIndex == 0 || !m_list[m_listIndex - 1].IsUndoable)
                return;
            do
            {
                m_list[m_listIndex - 1].UnExecute();
                //Console.WriteLine("index:" + (m_listIndex - 1) + " unexecute");
                m_listIndex--;

            } while (0 < m_listIndex && !m_list[m_listIndex - 1].IsAnchor);
            NotifyStatus();
        }

        /// <summary>
        /// Redo action.
        /// </summary>
        public void RedoAction()
        {
            if (m_list.Count <= m_listIndex)
                return;
            do
            {
                m_list[m_listIndex].Execute();
                //Console.WriteLine("index:" + m_listIndex + " execute");
                m_listIndex++;

            } while (m_listIndex < m_list.Count && !m_list[m_listIndex - 1].IsAnchor);
            NotifyStatus();
        }

        /// <summary>
        /// Notify undoable/redoable status to PluginManager
        /// </summary>
        private void NotifyStatus()
        {
            bool undoable = Undoable;
            bool redoable = Redoable;

            if (undoable && redoable)
                m_env.PluginManager.ChangeUndoStatus(UndoStatus.UNDO_REDO);
            else if (undoable)
                m_env.PluginManager.ChangeUndoStatus(UndoStatus.UNDO_ONLY);
            else if (redoable)
                m_env.PluginManager.ChangeUndoStatus(UndoStatus.REDO_ONLY);
            else
                m_env.PluginManager.ChangeUndoStatus(UndoStatus.NOTHING);

            RaiseUndoableChange();
            RaiseRedoableChange();

        }
    }
}