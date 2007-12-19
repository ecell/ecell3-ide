using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.PathwayWindow.Exceptions
{
    /// <summary>
    /// Exception for PathwayWindow.
    /// </summary>
    public class PathwayException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public PathwayException(string message)
            : base(message)
        {
        }
    }
}
