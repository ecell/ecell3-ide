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
        /// <summary>
        /// The model ID
        /// </summary>
        private string m_modelID;
        /// <summary>
        /// The list of Drag object
        /// </summary>
        private List<EcellDragEntry> m_entries;
        /// <summary>
        /// The list of Drag data.
        /// </summary>
        private List<string> m_loglist;
        /// <summary>
        /// The flag whether this drag is enable to change ID.
        /// </summary>
        private bool m_isEnableChangeID = false;

        /// <summary>
        /// Constructor without initial parameters.
        /// </summary>
        public EcellDragObject()
            : this("")
        {
        }

        /// <summary>
        /// Constructor with initial parameters.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        public EcellDragObject(string modelID)
        {
            m_modelID = modelID;
            m_entries = new List<EcellDragEntry>();
            m_loglist = new List<string>();
        }

        /// <summary>
        /// get / set model ID.
        /// </summary>
        public string ModelID
        {
            get { return this.m_modelID; }
        }
        /// <summary>
        /// get / set list of EcellDragEntry.
        /// </summary>
        public List<EcellDragEntry> Entries
        {
            get { return this.m_entries; }
        }
        /// <summary>
        /// get / set a list of Log filenames.
        /// </summary>
        public List<string> LogList
        {
            get { return this.m_loglist; }
            set { this.m_loglist = value; }
        }
        /// <summary>
        /// get / set the flag whether this object is enable to change ID.
        /// </summary>
        public bool IsEnableChange
        {
            get { return this.m_isEnableChangeID; }
            set { this.m_isEnableChangeID = value; }
        }
    }

    /// <summary>
    /// EcellDragEntry
    /// </summary>
    public class EcellDragEntry
    {
        string m_key;
        string m_type;
        string m_path;
        bool m_isSettable;
        bool m_isLogable;
        /// <summary>
        /// 
        /// </summary>
        public EcellDragEntry()
        {
            m_key = "";
            m_type = "";
            m_path = "";
            m_isSettable = false;
            m_isLogable = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="path"></param>
        /// <param name="isSettable"></param>
        /// <param name="isLogable"></param>
        public EcellDragEntry(string key, string type,
            string path, bool isSettable, bool isLogable)
        {
            m_key = key;
            m_type = type;
            m_path = path;
            m_isSettable = isSettable;
            m_isLogable = isLogable;
        }

        /// <summary>
        /// get/set key of object.
        /// </summary>
        public string Key
        {
            get { return this.m_key; }
        }

        /// <summary>
        /// get/set type of object.
        /// </summary>
        public string Type
        {
            get { return this.m_type; }
        }

        /// <summary>
        /// get/set entity path of logger.
        /// </summary>
        public string Path
        {
            get { return this.m_path; }
        }

        /// <summary>
        /// get / set the flag whether this property is able to set.
        /// </summary>
        public bool IsSettable
        {
            get { return this.m_isSettable; }
        }

        /// <summary>
        /// get / set the flag whether this property is able to log.
        /// </summary>
        public bool IsLogable
        {
            get { return this.m_isLogable; }
        }
    }
}
