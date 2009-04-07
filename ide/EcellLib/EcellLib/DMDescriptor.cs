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
// written by Moriyoshi Koizumi <mozo@sfc.keio.ac.jp>
//

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using EcellCoreLib;

namespace Ecell
{
    public class DMDescriptor: IEnumerable<PropertyDescriptor>
    {
        #region Fields
        /// <summary>
        /// Name of DM
        /// </summary>
        private string m_name;
        /// <summary>
        /// File path.
        /// </summary>
        private string m_path;
        /// <summary>
        /// Type of DM
        /// </summary>
        private string m_type;
        /// <summary>
        /// Description.
        /// </summary>
        private string m_desc;
        /// <summary>
        /// Properties
        /// </summary>
        private Dictionary<string, PropertyDescriptor> m_props;
        /// <summary>
        /// Can this DM have DynamicProperty or not.
        /// </summary>
        private bool m_canHaveDynamicProperties = false;
        #endregion

        #region Accessors
        /// <summary>
        /// Name of DM
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
        }

        /// <summary>
        /// File path.
        /// </summary>
        public string Path
        {
            get
            {
                return m_path;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            get
            {
                return m_type;
            }
        }

        /// <summary>
        /// Can this DM have DynamicProperty or not.
        /// </summary>
        public bool CanHaveDynamicProperties
        {
            get
            {
                return m_canHaveDynamicProperties;
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                return m_desc;
            }
        }

        /// <summary>
        /// Get PropertyDescriptor with name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertyDescriptor this[string name]
        {
            get
            {
                PropertyDescriptor retval = null;
                m_props.TryGetValue(name, out retval);
                return retval;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="canHaveDynamicProperties"></param>
        /// <param name="props"></param>
        public DMDescriptor(string name, string path, string type, bool canHaveDynamicProperties, Dictionary<string, PropertyDescriptor> props)
        {
            m_name = name;
            m_path = path;
            m_type = type;
            m_canHaveDynamicProperties = canHaveDynamicProperties;
            m_props = props;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<PropertyDescriptor> GetEnumerator()
        {
            return m_props.Values.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_props.Values.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public override bool Equals(object that)
        {
            if (that is DMDescriptor)
            {
                if (this == that)
                    return true;
                foreach (PropertyDescriptor p in m_props.Values)
                {
                    if (!p.Equals(that))
                        return false;
                }
                return m_name.Equals(((DMDescriptor)that).m_name) &&
                    m_path.Equals(((DMDescriptor)that).m_path) &&
                    m_type.Equals(((DMDescriptor)that).m_type) &&
                    m_canHaveDynamicProperties.Equals(((DMDescriptor)that).m_type);
            }
            return base.Equals(that);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (m_name.GetHashCode() ^ m_path.GetHashCode() ^ m_type.GetHashCode()) << (m_canHaveDynamicProperties ? 1 : 0);
        }
    }
}
