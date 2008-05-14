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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.Objects
{
    /// <summary>
    /// Stores the property data.
    /// </summary>
    public class EcellData
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
        private bool m_isLogger;
        /// <summary>
        /// The flag of savable
        /// </summary>
        private bool m_isSavable;
        /// <summary>
        /// The flag of settable
        /// </summary>
        private bool m_isSettable;
        /// <summary>
        /// Max value of this data.
        /// </summary>
        private double m_max;
        /// <summary>
        /// Min value of this data.
        /// </summary>
        private double m_min;
        /// <summary>
        /// Step value of this data.
        /// If this value is 0, this data is random parameter.
        /// </summary>
        private double m_step;
        #endregion

        #region Constractors
        /// <summary>
        /// Creates the new "EcellData" instance with no argument.
        /// </summary>
        public EcellData()
        {
            this.m_name = null;
            this.Value = null;
            this.m_entityPath = null;
            this.m_isGettable = true;
            this.m_isSettable = true;
            this.m_isLoadable = true;
            this.m_isSavable = true;
            this.m_isLogable = false;
            this.m_isLogger = false;
            this.m_max = 0.0;
            this.m_min = 0.0;
            this.m_step = 0.0;
        }

        /// <summary>
        /// Creates the new "EcellData" instance with some arguments.
        /// </summary>
        /// <param name="l_name">The property name</param>
        /// <param name="l_data">The property value</param>
        /// <param name="l_entityPath">The entity path</param>
        public EcellData(string l_name, EcellValue l_data, string l_entityPath)
        {
            this.m_name = l_name;
            this.Value = l_data;
            this.m_entityPath = l_entityPath;
            this.m_isGettable = true;
            this.m_isSettable = true;
            this.m_isLoadable = true;
            this.m_isSavable = true;
            this.m_isLogable = false;
            this.m_isLogger = false;
            this.m_max = 0.0;
            this.m_min = 0.0;
            this.m_step = 0.0;
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
            get { return this.m_isLogger; }
            set { this.m_isLogger = value; }
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

        /// <summary>
        /// get/set the max value of this data.
        /// </summary>
        public double Max
        {
            get { return this.m_max; }
            set { this.m_max = value; }
        }

        /// <summary>
        /// get/set the min value of this data.
        /// </summary>
        public double Min
        {
            get { return this.m_min; }
            set { this.m_min = value; }
        }

        /// <summary>
        /// get/set the step value of this data.
        /// </summary>
        public double Step
        {
            get { return this.m_step; }
            set { this.m_step = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_entityPath;
        }
        /// <summary>
        /// Create the copy "EcellData".
        /// </summary>
        /// <returns>The copy "EcellData"</returns>
        public EcellData Copy()
        {
            try
            {
                EcellData l_newData = new EcellData(this.m_name, this.Value.Copy(), this.m_entityPath);
                l_newData.Gettable = this.m_isGettable;
                l_newData.Loadable = this.m_isLoadable;
                l_newData.Logable = this.m_isLogable;
                l_newData.Logged = this.m_isLogger;
                l_newData.Saveable = this.m_isSavable;
                l_newData.Settable = this.m_isSettable;
                l_newData.Max = this.m_max;
                l_newData.Min = this.m_min;
                l_newData.Step = this.m_step;
                return l_newData;
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't copy the \"EcellData\". {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// override equal method on EcellData.
        /// </summary>
        /// <param name="l_obj">the comparing object</param>
        /// <returns>if equal, return true</returns>
        public bool Equals(EcellData l_obj)
        {
            if (this.m_name == l_obj.m_name && this.Value == l_obj.Value)
            {
                return true;
            }
            return false;
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
            if (this.Value == null || (!this.Value.IsInt() && !this.Value.IsDouble()))
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
