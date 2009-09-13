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

using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;

using Ecell;
using Ecell.Plugin;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.ProjectExplorer
{
    /// <summary>
    /// Plugin of ProjectExplorer.
    /// </summary>
    public class ProjectExplorer : PluginBase
    {
        #region Fields
        /// <summary>
        /// m_form (ProjectExplorerControl form) 
        /// </summary>
        private ProjectExplorerControl m_form = null;
        /// <summary>
        /// DM Editor
        /// </summary>
        private DMEditor m_editor = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for ProjectExplorer.
        /// </summary>
        public ProjectExplorer()
        {
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            m_form = new ProjectExplorerControl(this);
            m_form.Icon = Resources.ProjectExplorer;

            m_editor = new DMEditor();
            m_editor.Environment = m_env;
            m_editor.Text = MessageResources.NameDMEditor;
            m_editor.Name = MessageResources.NameDMEditor;
        }
        /// <summary>
        /// Get the window form for ProjectExplorer.
        /// This user control add the NodeMouseClick event action.
        /// </summary>
        /// <returns>UserControl.</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return new EcellDockContent[] { m_form, m_editor };
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            m_form.ChangeObject(modelID, key, type);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public override void AddSelect(string modelID, string key, string type)
        {
            m_form.AddSelect(modelID, key, type);
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public override void RemoveSelect(string modelID, string key, string type)
        {
            m_form.RemoveSelect(modelID, key, type);
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public override void ResetSelect()
        {
            m_form.treeView1.ClearSelNode();
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            m_form.DataAdd(data);
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public override void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            m_form.DataChanged(modelID, key, type, data);
        }


        /// <summary>
        /// Get the delegate function of plugin.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, Delegate> GetPublicDelegate()
        {
            Dictionary<string, Delegate> list = new Dictionary<string, Delegate>();
            list.Add(Constants.delegateSaveSimulationResult, 
                new SaveSimulationResultDelegate(this.SaveSimulationResult));
            list.Add(Constants.delegateAddDM,
                new AddDMDelegate(this.AddDM));
            return list;
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            m_form.DataDelete(modelID, key, type);
        }


        /// <summary>
        /// The event sequence when the simulation parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID.</param>
        public override void ParameterAdd(string projectID, string parameterID)
        {
            m_form.ParameterAdd(projectID, parameterID);
        }

        /// <summary>
        /// The event sequence when the parameter is deleted.
        /// </summary>
        /// <param name="projectID">the project id of deleted parameter.</param>
        /// <param name="parameterID">the id of deleted parameter.</param>
        public override void ParameterDelete(string projectID, string parameterID)
        {
            m_form.ParameterDelete(projectID, parameterID);
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            m_form.Clear();
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"ProjectExplorer"</returns>
        public override string GetPluginName()
        {
            return "ProjectExplorer";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Change the project status.
        /// </summary>
        /// <param name="type">the project status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            m_form.ChangeStatus(type);
            m_editor.ChangeStatus(type);
        }


        #endregion

        #region internal methods
        /// <summary>
        /// Get ProjectExplorerControl usercontrol.
        /// </summary>
        /// <returns>ProjectExplorerControl.</returns>
        internal ProjectExplorerControl GetForm()
        {
            return m_form;
        }        

        /// <summary>
        /// Check whether E-Cell SDK is already installed.
        /// </summary>
        /// <returns>if E-Cell SDK is installed, retur true.</returns>
        private bool CheckInstalledSDK()
        {
            return false;
        }

        /// <summary>
        /// Save the simulation result.
        /// </summary>
        /// <param name="list">the list of log.</param>
        private void SaveSimulationResult(List<string> list)
        {
            m_form.RefreshLogEntry();
        }

        /// <summary>
        /// Add the DM.
        /// </summary>
        /// <param name="dmName">the DM name.</param>
        /// <param name="path">the DM file path.</param>
        private void AddDM(string dmName, string path)
        {
            m_form.AddDM(dmName, path);
        }

        /// <summary>
        /// Show DM editor to display the file name.
        /// </summary>
        /// <param name="fileName">the file name.</param>
        public void ShowDMEditor(string fileName)
        {
            m_editor.path = fileName;
            m_editor.Activate();
        }

        /// <summary>
        /// Delete DM source file.
        /// </summary>
        /// <param name="fileName">the dm file path.</param>
        public void DeleteDM(string fileName)
        {
            if (fileName.Equals(m_editor.path))
                m_editor.DeleteSourceFile();
        }
        #endregion
    }
}
