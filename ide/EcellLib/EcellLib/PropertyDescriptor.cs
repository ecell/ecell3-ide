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
    public class PropertyDescriptor
    {
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

        public bool Dynamic
        {
            get
            {
                return m_dynamic;
            }
        }

        public EcellValue DefaultValue
        {
            get
            {
                return m_defaultValue != null ? (EcellValue)m_defaultValue.Clone() : null;
            }
        }

        public override bool Equals(object that)
        {
            if (that is PropertyDescriptor)
            {
                if (this == that)
                    return true;
                return m_dynamic == ((PropertyDescriptor)that).m_dynamic &&
                    m_isGettable == ((PropertyDescriptor)that).m_isGettable &&
                    m_isSettable == ((PropertyDescriptor)that).m_isSettable &&
                    m_isLoadable == ((PropertyDescriptor)that).m_isLoadable &&
                    m_isSavable == ((PropertyDescriptor)that).m_isSavable &&
                    m_name == ((PropertyDescriptor)that).m_name;
            }
            return base.Equals(that);
        }

        public override int GetHashCode()
        {
            return m_name.GetHashCode() ^ ((m_dynamic ? 0x01 : 0)
                | (m_isGettable ? 0x02 : 0)
                | (m_isSettable ? 0x04 : 0)
                | (m_isLoadable ? 0x08 : 0)
                | (m_isSavable ? 0x10 : 0));
        }

        public PropertyDescriptor(string name, bool settable, bool gettable,
                bool loadable, bool saveable, bool dynamic, bool logable,
                EcellValue defaultValue)
        {
            m_name = name;
            m_isGettable = gettable;
            m_isSettable = settable;
            m_isLoadable = loadable;
            m_isSavable = saveable;
            m_dynamic = dynamic;
            m_isLogable = logable;
            m_defaultValue = defaultValue;
        }
    }
}
