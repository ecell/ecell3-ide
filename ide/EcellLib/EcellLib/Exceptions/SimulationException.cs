using System;
using EcellCoreLib;

namespace Ecell.Exceptions
{
    /// <summary>
    /// SimulationException
    /// </summary>
    public class SimulationException: EcellException
    {
        /// <summary>
        /// InnerException
        /// </summary>
        public new WrappedException InnerException
        {
            get { return (WrappedException)base.InnerException; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public SimulationException(string message, WrappedException e):
            base(message, e)
        {
        }
    }
}
