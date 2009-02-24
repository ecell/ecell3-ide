using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Plugin
{

    /// <summary>
    /// Data element displayed on plugins.
    /// </summary>
    public class PluginData
    {
        /// <summary>
        /// m_modelID (the model ID)
        /// </summary>
        private string m_modelID;
        /// <summary>
        /// m_key (the key ID)
        /// </summary>
        private string m_key;

        /// <summary>
        /// constructor of PluginData.
        /// </summary>
        public PluginData()
        {
            this.m_modelID = "model1";
            this.m_key = "key1";
        }

        /// <summary>
        /// constructir of PluginData with initial data.
        /// </summary>
        /// <param name="id">initial model ID</param>
        /// <param name="key">initial key ID</param>
        public PluginData(string id, string key)
        {
            this.m_modelID = id;
            this.m_key = key;
        }

        /// <summary>
        /// get/set m_modelID.
        /// </summary>
        public string ModelID
        {
            get { return this.m_modelID; }
            set { this.m_modelID = value; }
        }

        /// <summary>
        /// get/set m_key.
        /// </summary>
        public string Key
        {
            get { return this.m_key; }
            set { this.m_key = value; }
        }

        /// <summary>
        /// override equals method for PluginData.
        /// </summary>
        /// <param name="obj">the comparing object</param>
        /// <returns>if equal, return true</returns>
        public bool Equals(PluginData obj)
        {
            if (this.m_modelID == obj.m_modelID && this.m_key == obj.m_key)
            {
                return true;
            }
            return false;
        }
    }
}
