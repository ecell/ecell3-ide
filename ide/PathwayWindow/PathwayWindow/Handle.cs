using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Attached to tag of ToolStripMenuItem
    /// </summary>
    public class Handle
    {
        /// <summary>
        /// This handle's mode
        /// </summary>
        private Mode m_mode;

        /// <summary>
        /// ID of this handle
        /// </summary>
        private int m_handleID;

        /// <summary>
        /// ID of component setting
        /// </summary>
        private int m_csID;

        /// <summary>
        /// Accessor for mode of this handle.
        /// </summary>
        public Mode Mode
        {
            get { return this.m_mode; }
        }

        /// <summary>
        /// Accessor for ID.
        /// </summary>
        public int HandleID
        {
            get { return this.m_handleID; }
        }

        /// <summary>
        /// Accessor for component setting's ID
        /// </summary>
        public int CsID
        {
            get { return this.m_csID; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mode">Mode of this handle (select, pan, etc.)</param>
        /// <param name="handleID">ID of this handle</param>
        /// <param name="csID">ID of component setting</param>
        public Handle(Mode mode, int handleID, int csID)
        {
            this.m_mode = mode;
            this.m_handleID = handleID;
            this.m_csID = csID;
        }
    }
}