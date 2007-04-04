using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.PathwayWindow
{
    public class PathwayException : Exception
    {
        public PathwayException(string message)
            : base(message)
        {
        }
    }
}
