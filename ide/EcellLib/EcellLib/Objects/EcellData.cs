//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Exceptions;

namespace Ecell.Objects
{
    /// <summary>
    /// Stores the property data.
    /// </summary>
    public class EcellData : ICloneable
    {
        #region Fields
        /// <summary>
        /// The property name 
        /// </summary>
        private string m_name;
        /// <summary>
        /// The property value
        /// </summary>
        private EcellValue m_value;
        /// <summary>
        /// The entity path 4 "EcellCoreLib"
        /// </summary>
        private string m_entityPath;
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
        /// The flag of logger
        /// </summary>
        private bool m_isLogged;
        /// <summary>
        /// The flag of savable
        /// </summary>
        private bool m_isSavable;
        /// <summary>
        /// The flag of settable
        /// </summary>
        private bool m_isSettable;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the new "EcellData" instance with no argument.
        /// </summary>
        public EcellData()
        {
            this.m_name = null;
            this.m_value = null;
            this.m_entityPath = null;
            this.m_isGettable = true;
            this.m_isSettable = true;
            this.m_isLoadable = true;
            this.m_isSavable = true;
            this.m_isLogable = false;
            this.m_isLogged = false;
        }

        /// <summary>
        /// Creates the new "EcellData" instance with some arguments.
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        /// <param name="entityPath">The entity path</param>
        public EcellData(string name, EcellValue value, string entityPath)
        {
            this.m_name = name;
            this.m_value = value;
            this.m_entityPath = entityPath;
            this.m_isGettable = true;
            this.m_isSettable = true;
            this.m_isLoadable = true;
            this.m_isSavable = true;
            this.m_isLogable = false;
            this.m_isLogged = false;
        }

        #endregion

        #region Accessors
        /// <summary>
        /// get/set name.
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// get/set value
        /// </summary>
        public EcellValue Value
        {
            get { return m_value; }
            set { this.m_value = value; }
        }

        /// <summary>
        /// get/set m_entityPath
        /// </summary>
        public string EntityPath
        {
            get { return m_entityPath; }
            set { this.m_entityPath = value; }
        }

        /// <summary>
        /// get/set m_isGettable
        /// </summary>
        public bool Gettable
        {
            get { return m_isGettable; }
            set { this.m_isGettable = value; }
        }

        /// <summary>
        /// get/set m_isLoadable
        /// </summary>
        public bool Loadable
        {
            get { return m_isLoadable; }
            set { this.m_isLoadable = value; }
        }

        /// <summary>
        /// get/set m_isLogable
        /// </summary>
        public bool Logable
        {
            get { return this.m_isLogable; }
            set { this.m_isLogable = value; }
        }

        /// <summary>
        /// get/set m_isLogger
        /// </summary>
        public bool Logged
        {
            get { return this.m_isLogged; }
            set { this.m_isLogged = value; }
        }

        /// <summary>
        /// get/set m_isSavable
        /// </summary>
        public bool Saveable
        {
            get { return m_isSavable; }
            set { this.m_isSavable = value; }
        }

        /// <summary>
        /// get/set m_isSettable
        /// </summary>
        public bool Settable
        {
            get { return m_isSettable; }
            set { this.m_isSettable = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "";
            if (m_entityPath != null)
                str += m_entityPath;
            if (m_value != null)
                str += ", " + m_value.ToString();
            return str;
        }

        /// <summary>
        /// override equal method on EcellData.
        /// </summary>
        /// <param name="obj">the comparing object</param>
        /// <returns>if equal, return true</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is EcellData))
                return false;
            EcellData data = (EcellData)obj;

            if (this.m_name != data.Name)
                return false;
            if (this.m_entityPath != data.EntityPath)
                return false;
            if ((this.m_value != null && data.Value == null)
                || (this.m_value == null && data.Value != null))
                return false;
            if ((this.m_value != null && data.Value != null)
                && !this.m_value.Equals(data.Value))
                return false;

            if ((this.m_isGettable != data.Gettable))
                return false;
            if ((this.m_isLoadable != data.Loadable))
                return false;
            if ((this.m_isLogable != data.Logable))
                return false;
            if ((this.m_isLogged != data.Logged))
                return false;
            if ((this.m_isSavable != data.Saveable))
                return false;
            if ((this.m_isSettable != data.Settable))
                return false;

            return true;
        }

        /// <summary>
        /// GetHashCode override function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = m_isGettable.GetHashCode()
                ^ m_isLoadable.GetHashCode()
                ^ m_isLogable.GetHashCode()
                ^ m_isLogged.GetHashCode()
                ^ m_isSavable.GetHashCode()
                ^ m_isSettable.GetHashCode();

            if (m_name != null)
                hash = hash ^ m_name.GetHashCode();
            if (m_entityPath != null)
                hash = hash ^ m_entityPath.GetHashCode();
            if (m_value != null)
                hash = hash ^ m_value.GetHashCode();

            return hash;
        }

        /// <summary>
        /// Tests whether it is possible to initialize this or not.
        /// </summary>
        /// <returns>true if it is possible to initialize; false otherwise</returns>
        public bool IsInitialized()
        {
            if (!this.m_isSettable)
            {
                return false;
            }
            if (this.m_value == null || (!this.m_value.IsInt && !this.m_value.IsDouble))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region ICloneable メンバ
        /// <summary>
        /// Create a copy of this EcellData object.
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Create a copy of this EcellData object.
        /// </summary>
        /// <returns>The copy "EcellData"</returns>
        public EcellData Clone()
        {
            EcellData newData = new EcellData();

            newData.Name = this.m_name;
            newData.EntityPath = this.m_entityPath;
            if (m_value == null)
                newData.Value = null;
            else
                newData.Value = this.m_value.Clone();

            newData.Gettable = this.m_isGettable;
            newData.Loadable = this.m_isLoadable;
            newData.Logable = this.m_isLogable;
            newData.Logged = this.m_isLogged;
            newData.Saveable = this.m_isSavable;
            newData.Settable = this.m_isSettable;

            return newData;
        }

        #endregion
    }
}
