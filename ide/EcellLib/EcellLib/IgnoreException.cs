using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib
{
    public class IgnoreException : Exception
    {
        public IgnoreException(string mes):base(mes)
        {
            
        }
    }
}
