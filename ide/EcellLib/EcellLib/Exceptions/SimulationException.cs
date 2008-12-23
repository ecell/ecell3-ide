using System;
using EcellCoreLib;

namespace Ecell.Exceptions
{
    public class SimulationException: EcellException
    {
        public new WrappedException InnerException
        {
            get { return (WrappedException)base.InnerException; }
        }

        public SimulationException(string message, WrappedException e):
            base(message, e)
        {
        }
    }
}
