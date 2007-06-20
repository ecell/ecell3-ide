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
        List<ToolStripMenuItem> GetMenuStripItems();
        List<ToolStripItem> GetToolBarMenuStripItems();
        List<UserControl> GetWindowsForms();
        void SelectChanged(string modelID, string key, string type);
        void DataChanged(string modelID, string key, string type, EcellObject data);
        void DataAdd(List<EcellObject> data);
        void DataDelete(string modelID, string key, string type);
        void LogData(string modelID, string key, string type, string propName, List<LogData> log);
        void WarnData(string modelID, string key, string type, string warntype);
        void LoggerAdd(string modelID, string type, string key, string path);
        void Message(string type, string message);
        void AdvancedTime(double time);
        void ChangeStatus(int type); // 0:initial 1:load 2:run 3:suspend

        /// <summary>
        /// Notify a plugin that it should save model-related information if necessary.
        /// </summary>
        /// <param name="modelID">ModelID of a model which is going to be saved</param>
        /// <param name="directory">A saved file must be under this directory </param>
        void SaveModel(string modelID, string directory);
        void SetPanel(Panel panel);
        void Clear();
        bool IsMessageWindow();
        bool IsEnablePrint();
        Bitmap Print();
        string GetPluginName();
        string GetVersionString();
    }
}
