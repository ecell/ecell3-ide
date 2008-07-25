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
// written by Motokazu Ishikawa<m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.ComponentModel;

using Ecell;
using Ecell.Plugin;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.EntityList
{
    /// <summary>
    /// Plugin class to display object by list.
    /// </summary>
    public class EntityList : PluginBase
    {
        #region Fields
        private ObjectListUserControl m_control;
        /// <summary>
        /// ComponentResourceManager for ObjectList.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResources));
        #endregion

        #region Constructor
        /// <summary>
        /// Construcotor.
        /// </summary>
        public EntityList()
        {
        }
        #endregion

        #region Initializer
        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            m_control = new ObjectListUserControl(this);
            m_control.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Deconstructor for ObjectList.
        /// </summary>
        ~EntityList()
        {
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get the window form for ObjectList.
        /// </summary>
        /// <returns>UserControl</returns>        
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            EcellDockContent win = new EcellDockContent();
            m_control.Dock = DockStyle.Fill;
            win.Controls.Add(m_control);
            win.Name = "EntityList";
            win.Text = MessageResources.ObjectList;
            win.Icon = MessageResources.objlist;
            win.TabText = win.Text;
            win.IsSavable = true;
            return new EcellDockContent[] { win };
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            m_control.SelectChanged(modelID, key, type);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public override void AddSelect(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            m_control.AddSelection(modelID, key, type);
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public override void RemoveSelect(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            m_control.RemoveSelection(modelID, key, type);

        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public override void ResetSelect()
        {
            m_control.ClearSelection();
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            if (data == null) return;
            
            foreach (EcellObject obj in data)
            {
                if (obj.Type != EcellObject.VARIABLE &&
                    obj.Type != EcellObject.PROCESS &&
                    obj.Type != EcellObject.SYSTEM)
                    continue;
                m_control.DataAdd(obj);
                if (obj.Children == null) continue;
                foreach (EcellObject cobj in obj.Children)
                {
                    m_control.DataAdd(cobj);
                }
            }
            return;
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
            m_control.DataChanged(modelID, key, type, data);

            return;
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            if (type.Equals(Constants.xpathStepper)) return;

            m_control.DataDelete(modelID, key, type);
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            m_control.Clear();
        }

        /// <summary>
        /// get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>bitmap data</returns>
        public override Bitmap Print(string names)
        {
            Bitmap b = new Bitmap(m_control.Width, m_control.Height);
            m_control.DrawToBitmap(b, m_control.ClientRectangle);

            return b;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"ObjectList"</returns>
        public override string GetPluginName()
        {
            return "EntityList";
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
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public override IEnumerable<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            names.Add(MessageResources.ObjectList);
            return names;
        }
        #endregion
    }
}
