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
    /// EcellObject to drag and drop.
    /// </summary>
    public class EcellDragObject
    {
        string m_modelID;
        string m_key;
        string m_type;
        string m_path;

        /// <summary>
        /// Constructor without initial parameters.
        /// </summary>
        public EcellDragObject()
        {
            m_modelID = "";
            m_key = "";
            m_type = "";
            m_path = "";
        }

        /// <summary>
        /// Constructor with initial parameters.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="path"></param>
        public EcellDragObject(string modelID, string key, string type, string path)
        {
            m_modelID = modelID;
            m_key = key;
            m_type = type;
            m_path = path;
        }

        /// <summary>
        /// get/set model ID.
        /// </summary>
        public String ModelID
        {
            get { return this.m_modelID; }
            set { this.m_modelID = value; }
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
    }
}
