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
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib
{
    /// <summary>
    /// Interface of plugin.
    /// </summary>
    public interface PluginBase
    {
        /// <summary>
        /// Get menustrips for EntityListWindow plugin.
        /// </summary>
        /// <returns>null.</returns>
        List<ToolStripMenuItem> GetMenuStripItems();

        /// <summary>
        /// Get toolbar buttons for EntityListWindow plugin.
        /// </summary>
        /// <returns>null</returns>
        List<ToolStripItem> GetToolBarMenuStripItems();

        /// <summary>
        /// Get the window form for EntityListWindow.
        /// This user control add the NodeMouseClick event action.
        /// </summary>
        /// <returns>UserControl.</returns>
        List<UserControl> GetWindowsForms();

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        void SelectChanged(string modelID, string key, string type);

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        void DataChanged(string modelID, string key, string type, EcellObject data);

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        void DataAdd(List<EcellObject> data);

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        void DataDelete(string modelID, string key, string type);

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="data">Changed value of object.</param>
        void LogData(string modelID, string key, string type, string propName, List<LogData> data);

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
        void ChangeStatus(int type); // 0:initial 1:load 2:run 3:suspend

        /// <summary>
        /// Notify a plugin that it should save model-related information if necessary.
        /// </summary>
        /// <param name="modelID">ModelID of a model which is going to be saved</param>
        /// <param name="directory">A saved file must be under this directory </param>
        void SaveModel(string modelID, string directory);

        /// <summary>
        /// Set the panel that show this plugin in MainWindow.
        /// </summary>
        /// <param name="panel">The set panel.</param>
        void SetPanel(Panel panel);

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>        
        void Clear();

        /// <summary>
        /// cCeck whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false</returns>
        bool IsMessageWindow();

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>false.</returns>
        bool IsEnablePrint();

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>   
        Bitmap Print();

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>""</returns>
        string GetPluginName();

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        string GetVersionString();
    }
}
