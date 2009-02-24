using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Logger
{
    public class LoggerEntry
    {
        #region Fields
        private string m_modelID;
        private string m_ID;
        private string m_Type;
        private string m_FullPN;
        private bool m_isLogging;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor without the flag whether this property is logging.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="fullPN"></param>
        public LoggerEntry(string modelID, string id, string type, string fullPN)
        {
            this.m_modelID = modelID;
            this.m_ID = id;
            this.m_Type = type;
            this.m_FullPN = fullPN;
            this.m_isLogging = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="fullPN"></param>
        /// <param name="isLogging"></param>
        public LoggerEntry(string modelID, string id, string type,
            string fullPN, bool isLogging)
        {
            this.m_modelID = modelID;
            this.m_ID = id;
            this.m_Type = type;
            this.m_FullPN = fullPN;
            this.m_isLogging = isLogging;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set Model ID.
        /// </summary>
        public String ModelID
        {
            get { return this.m_modelID; }
            set { this.m_modelID = value; }
        }

        /// <summary>
        /// get / set ID.
        /// </summary>
        public String ID
        {
            get { return this.m_ID; }
            set { this.m_ID = value; }
        }

        /// <summary>
        /// get / set Type.
        /// </summary>
        public String Type
        {
            get { return this.m_Type; }
            set { this.m_Type = value; }
        }

        /// <summary>
        /// get / set FullPN.
        /// </summary>
        public String FullPN
        {
            get { return this.m_FullPN; }
            set { this.m_FullPN = value; }
        }

        /// <summary>
        /// get / set the flag whether this property is logging.
        /// </summary>
        public bool IsLogging
        {
            get { return this.m_isLogging; }
            set { this.m_isLogging = value; }
        }
        #endregion

        /// <summary>
        /// override the equal method on LoggerEntry.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is LoggerEntry))
                return false;

            LoggerEntry ent = (LoggerEntry)obj;
            if (ent.ModelID == this.ModelID &&
                ent.ID == this.ID &&
                ent.Type == this.Type &&
                ent.FullPN == this.FullPN)
                return true;
            return false;
        }
    }
}
