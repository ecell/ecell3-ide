using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell
{
    /// <summary>
    /// Exception 
    /// </summary>
    public class IgnoreException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mes">exception message.</param>
        public IgnoreException(string mes):base(mes)
        {
            
        }
    }
}
