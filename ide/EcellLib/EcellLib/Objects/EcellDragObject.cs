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

namespace Ecell.Objects
{

    /// <summary>
    /// EcellObject to drag and drop.
    /// </summary>
    public class EcellDragObject
    {
        private string m_modelID;
        private List<EcellDragEntry> m_entries = new List<EcellDragEntry>();

        /// <summary>
        /// Constructor without initial parameters.
        /// </summary>
        public EcellDragObject()
        {
            m_modelID = "";
        }

        /// <summary>
        /// Constructor with initial parameters.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        /// <param name="key">the key of this object.</param>
        /// <param name="type">the type of this object.</param>
        /// <param name="path">the property path of this object.</param>
        /// <param name="isLogable">the flag whether this property is able to log.</param>
        /// <param name="isSettable">the flag whether this property is able to set.</param>
        public EcellDragObject(string modelID, string key, string type,
            string path, bool isSettable, bool isLogable)
        {
            m_modelID = modelID;
            m_entries.Add(new EcellDragEntry(key, type, path, isSettable, isLogable));
        }

        /// <summary>
        /// get/set model ID.
        /// </summary>
        public String ModelID
        {
            get { return this.m_modelID; }
            set { this.m_modelID = value; }
        }

        public List<EcellDragEntry> Entries
        {
            get { return this.m_entries; }
            set { this.m_entries = value; }
        }
    }

    public class EcellDragEntry
    {
        string m_key;
        string m_type;
        string m_path;
        bool m_isSettable;
        bool m_isLogable;

        public EcellDragEntry()
        {
            m_key = "";
            m_type = "";
            m_path = "";
            m_isLogable = false;
            m_isSettable = false;
        }

        public EcellDragEntry(string key, string type,
            string path, bool isSettable, bool isLogable)
        {
            m_key = key;
            m_type = type;
            m_path = path;
            m_isLogable = IsLogable;
            m_isSettable = isSettable;
        }

        /// <summary>
        /// get/set key of object.
        /// </summary>
        public String Key
        {
            get { return this.m_key; }
            set { this.m_key = value; }
        }

        /// <summary>
        /// get/set type of object.
        /// </summary>
        public String Type
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }

        /// <summary>
        /// get/set entity path of logger.
        /// </summary>
        public String Path
        {
            get { return this.m_path; }
            set { this.m_path = value; }
        }

        /// <summary>
        /// get / set the flag whether this property is able to set.
        /// </summary>
        public bool IsSettable
        {
            get { return this.m_isSettable; }
            set { this.m_isSettable = value; }
        }

        /// <summary>
        /// get / set the flag whether this property is able to log.
        /// </summary>
        public bool IsLogable
        {
            get { return this.m_isLogable; }
            set { this.m_isLogable = value; }
        }
    }
}
