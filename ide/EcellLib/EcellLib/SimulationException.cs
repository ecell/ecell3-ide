using System;
using EcellCoreLib;

namespace Ecell
{
    public class SimulationException: Exception
    {
        public new WrappedException InnerException
        {
            get { return (WrappedException)base.InnerException; }
        }

        public SimulationException(string message, WrappedException e): base(message, e)
        {
        }
    }
}
