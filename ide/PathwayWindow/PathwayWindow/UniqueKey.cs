using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Unique key in E-cell system
    /// </summary>
    public class UniqueKey
    {
        /// <summary>
        /// type of component
        /// </summary>
        private ComponentType m_type;

        /// <summary>
        /// key of E-cell core.
        /// </summary>
        private string m_key;

        /// <summary>
        /// Accessor for m_type
        /// </summary>
        public ComponentType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// Accessor for m_key
        /// </summary>
        public string Key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="key"></param>
        public UniqueKey(ComponentType ct, string key)
        {
            m_type = ct;
            m_key = key;
        }
    }
}
