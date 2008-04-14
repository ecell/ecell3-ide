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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EcellLib.Objects;

namespace EcellLib.Plugin
{
    /// <summary>
    /// Interface of plugin.
    /// </summary>
    public interface IEcellPlugin
    {
        #region Methods to return answer or objects.
        /// <summary>
        /// Get the name of this plugin.
        /// PluginName MUST BE unique.
        /// </summary>
        /// <returns>""</returns>
        string GetPluginName();

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        string GetVersionString();

        /// <summary>
        /// Get menustrips for each plugin.
        /// </summary>
        /// <returns>null.</returns>
        List<ToolStripMenuItem> GetMenuStripItems();

        /// <summary>
        /// Get toolbar buttons for each plugin.
        /// </summary>
        /// <returns>null</returns>
        List<ToolStripItem> GetToolBarMenuStripItems();

        /// <summary>
        /// Get the window forms of each plugin.
        /// DockContent is a docking window class of WeifenLuo.WinFormsUI plugin.
        /// </summary>
        /// <returns>UserControl.</returns>
        //List<UserControl> GetWindowsForms();
        List<EcellDockContent> GetWindowsForms();

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>false.</returns>
        List<String> GetEnablePrintNames();

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>   
        Bitmap Print(string name);

        /// <summary>
        /// cCeck whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false</returns>
        bool IsMessageWindow();
        #endregion

        #region Methods to receive events.
        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        void DataAdd(List<EcellObject> data);

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        void DataChanged(string modelID, string key, string type, EcellObject data);

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        void DataDelete(string modelID, string key, string type);

        /// <summary>
        /// Set the position of EcellObject.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        void SetPosition(EcellObject data);

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        void SelectChanged(string modelID, string key, string type);

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        void AddSelect(string modelID, string key, string type);

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        void RemoveSelect(string modelID, string key, string type);

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        void ResetSelect();

        /// <summary>
        /// The event sequence when the user add the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID/</param>
        void ParameterAdd(string projectID, string parameterID);

        /// <summary>
        /// The event sequence when the user delete the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        void ParameterDelete(string projectID, string parameterID);

        /// <summary>
        /// The event sequence when the user set the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The set parameter ID.</param>
        void ParameterSet(string projectID, string parameterID);

        /// <summary>
        /// The event sequence when the user add and change the observed data.
        /// </summary>
        /// <param name="data">the observed data.</param>
        void SetObservedData(EcellObservedData data);

        /// <summary>
        /// The event sequence when the user remove the data from the list of observed data.
        /// </summary>
        /// <param name="data">The removed observed data.</param>
        void RemoveObservedData(EcellObservedData data);

        /// <summary>
        /// The event sequence when the user add and change the parameter data.
        /// </summary>
        /// <param name="data">The parameter data.</param>
        void SetParameterData(EcellParameterData data);

        /// <summary>
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed parameter data.</param>
        void RemoveParameterData(EcellParameterData data);

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        void WarnData(string modelID, string key, string type, string warntype);

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        void LoggerAdd(string modelID, string type, string key, string path);

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        void Message(string type, string message);

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        void AdvancedTime(double time);

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        void ChangeStatus(ProjectStatus type); // 0:initial 1:load 2:run 3:suspend

        /// <summary>
        /// Change availability of undo/redo function.
        /// </summary>
        /// <param name="status"></param>
        void ChangeUndoStatus(UndoStatus status);

        /// <summary>
        /// Notify a plugin that it should save model-related information if necessary.
        /// </summary>
        /// <param name="modelID">ModelID of a model which is going to be saved</param>
        /// <param name="directory">A saved file must be under this directory </param>
        void SaveModel(string modelID, string directory);

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>        
        void Clear();
        #endregion
    }
}
