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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

using EcellLib.Objects;

namespace EcellLib.Message
{
    /// <summary>
    /// Message Entry Object for EcellData.
    /// </summary>
    public class EntityMessageEntry : MessageEntry
    {
        #region Fileds
        private EcellData m_entity;
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the entity of message.
        /// </summary>
        public EcellData Entity
        {
            get { return m_entity; }
            set
            {
                this.m_entity = value;
                m_location = ExtractLocationString();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityMessageEntry() 
        {
            m_entity = null;
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="entity"></param>
        public EntityMessageEntry(MessageType type, string message, EcellData entity)
        {
            m_type = type;
            m_message = message;
            m_entity = entity;
            m_location = ExtractLocationString();
        }
        #endregion

        /// <summary>
        /// Extract the location information from EcellData.
        /// </summary>
        /// <returns>the location string.</returns>
        private string ExtractLocationString()
        {
            if (m_entity == null) return "";
            return m_entity.EntityPath;
        }
    }
}
