//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
using System.Windows.Forms;
using System.Text;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.ObjectList2
{
    /// <summary>
    /// Interface class to manage the TabPage of ObjectList.
    /// </summary>
    public interface IObjectListTabPage
    {
        /// <summary>
        /// Event when object is selected.
        /// </summary>
        /// <param name="modelId">ModelID of the selected object.</param>
        /// <param name="id">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        void SelectChanged(string modelId, string id, string type);
        /// <summary>
        /// Event when object is deleted.
        /// </summary>
        /// <param name="modelID">ModelID of the deleted object.</param>
        /// <param name="id">ID of the deleted object.</param>
        /// <param name="type">Type of the deleted object.</param>
        /// <param name="isChanged">whether id is changed.</param>
        void DataDelete(string modelID, string id, string type, bool isChanged);
        /// <summary>
        /// Event when object is added.
        /// </summary>
        /// <param name="obj">the list of added object.</param>
        void DataAdd(List<EcellObject> obj);
        /// <summary>
        /// Event when the property of object is changed.
        /// </summary>
        /// <param name="modelID">ModelID of the changed object.</param>
        /// <param name="id">ID of the changed object.</param>
        /// <param name="type">Type of the changed object.</param>
        /// <param name="obj">The changed object.</param>
        void DataChanged(string modelID, string id, string type, EcellObject obj);
        /// <summary>
        /// Event when the project is closed.
        /// </summary>
        void Clear();
        /// <summary>
        /// Event when the selected object is added.
        /// </summary>
        /// <param name="modelId">ModelID of the selected object.</param>
        /// <param name="id">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        void AddSelection(string modelId, string id, string type);
        /// <summary>
        /// Event when the selected object is removed.
        /// </summary>
        /// <param name="modelId">ModelID of the removed object.</param>
        /// <param name="id">ID of the removed object.</param>
        /// <param name="type">Type of the removed object.</param>
        void RemoveSelection(string modelId, string id, string type);
        /// <summary>
        /// Event when the selected object is changed to no select.
        /// </summary>
        void ClearSelection();
        /// <summary>
        /// Event when the status of system is changed.
        /// </summary>
        /// <param name="status">the changed status.</param>
        void ChangeStatus(ProjectStatus status);
        /// <summary>
        /// Event when system search the object by text.
        /// </summary>
        /// <param name="text">search condition.</param>
        void SearchInstance(string text);
        /// <summary>
        /// Get tab name.
        /// </summary>
        /// <returns>name of tab.</returns>
        string GetTabPageName();
        /// <summary>
        /// Get TabPage.
        /// </summary>
        /// <returns>TabPage.</returns>
        TabPage GetTabPage();
    }
}
