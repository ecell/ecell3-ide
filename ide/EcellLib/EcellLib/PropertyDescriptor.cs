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
using Ecell.Objects;

namespace Ecell
{
    /// <summary>
    /// PropertyDescriptor for DM
    /// </summary>
    public class PropertyDescriptor
    {
        #region Fields
        /// <summary>
        /// The property name 
        /// </summary>
        private string m_name;
        /// <summary>
        /// The flag of gettable
        /// </summary>
        private bool m_isGettable;
        /// <summary>
        /// The flag of loadable
        /// </summary>
        private bool m_isLoadable;
        /// <summary>
        /// The flag of logable
        /// </summary>
        private bool m_isLogable;
        /// <summary>
        /// The flag of savable
        /// </summary>
        private bool m_isSavable;
        /// <summary>
        /// true if this property is dynamically added.
        /// </summary>
        private bool m_dynamic;
        /// <summary>
        /// The flag of settable
        /// </summary>
        private bool m_isSettable;
        /// <summary>
        /// default value for this slot
        /// </summary>
        private EcellValue m_defaultValue;
        #endregion

        #region Accessors
        /// <summary>
        /// get/set name.
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// get/set m_isGettable
        /// </summary>
        public bool Gettable
        {
            get { return m_isGettable; }
        }

        /// <summary>
        /// get/set m_isLoadable
        /// </summary>
        public bool Loadable
        {
            get { return m_isLoadable; }
        }

        /// <summary>
        /// get/set m_isLogable
        /// </summary>
        public bool Logable
        {
            get { return this.m_isLogable; }
        }

        /// <summary>
        /// get/set m_isSavable
        /// </summary>
        public bool Saveable
        {
            get { return m_isSavable; }
        }

        /// <summary>
        /// get/set m_isSettable
        /// </summary>
        public bool Settable
        {
            get { return m_isSettable; }
        }
        /// <summary>
        /// get / set whether DM is able to add or delete the property.
        /// </summary>
        public bool Dynamic
        {
            get { return m_dynamic; }
        }
        /// <summary>
        /// get the default value.
        /// </summary>
        public EcellValue DefaultValue
        {
            get
            {
                return m_defaultValue != null ? (EcellValue)m_defaultValue.Clone() : null;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">the property name.</param>
        /// <param name="settable">whether this property is settable.</param>
        /// <param name="gettable">whether this property is gettable</param>
        /// <param name="loadable">whether this property is loadable</param>
        /// <param name="saveable">whether this property is saveable</param>
        /// <param name="dynamic">whether this property is able to add or delete dynamically.</param>
        /// <param name="logable">whether this property is logable</param>
        /// <param name="defaultValue">the default value.</param>
        public PropertyDescriptor(string name, bool settable, bool gettable,
                bool loadable, bool saveable, bool dynamic, bool logable,
                EcellValue defaultValue)
        {
            m_name = name;
            // Set attributes.
            m_isSettable = settable;
            m_isGettable = gettable;
            m_isLoadable = loadable;
            m_isSavable = saveable;
            m_dynamic = dynamic;
            // Set default value.
            m_defaultValue = defaultValue;
            // Set Loggable
            m_isLogable = logable;
        }
        #endregion

        /// <summary>
        /// Equals override function.
        /// </summary>
        /// <param name="that">the compared object.</param>
        /// <returns>Return true, object is equal.</returns>
        public override bool Equals(object that)
        {
            if (!(that is PropertyDescriptor))
                return false;

            PropertyDescriptor pd = (PropertyDescriptor)that;
            return m_dynamic == ((PropertyDescriptor)that).m_dynamic &&
                m_isGettable == ((PropertyDescriptor)that).m_isGettable &&
                m_isSettable == ((PropertyDescriptor)that).m_isSettable &&
                m_isLoadable == ((PropertyDescriptor)that).m_isLoadable &&
                m_isSavable == ((PropertyDescriptor)that).m_isSavable &&
                m_name == ((PropertyDescriptor)that).m_name;
        }

        /// <summary>
        /// GetHashCode override function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_name.GetHashCode() ^ ((m_dynamic ? 0x01 : 0)
                | (m_isGettable ? 0x02 : 0)
                | (m_isSettable ? 0x04 : 0)
                | (m_isLoadable ? 0x08 : 0)
                | (m_isSavable ? 0x10 : 0));
        }
    }
}
