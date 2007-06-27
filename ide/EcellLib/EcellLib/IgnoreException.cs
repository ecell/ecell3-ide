using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib
{
    class IgnoreException : Exception
    {
        public IgnoreException(string mes):base(mes)
        {
            
        }
    }
}
