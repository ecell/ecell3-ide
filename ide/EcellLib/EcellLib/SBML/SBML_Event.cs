using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.SBML
{
    internal class SBML_Event
    {
        private SBML_Model Model;
        public int EventNumber;

        public SBML_Event(SBML_Model aModel)
        {
            this.Model = aModel;
            this.EventNumber = 0;
        }
        
        public string getEventID(string[] aEvent)
        {
            if( aEvent[0] != "" )
                return "Process:/:" + aEvent[0];
            else if( aEvent[1] != "" )
                return "Process:/:" + aEvent[1];
            else
            {
                string anID = "Process:/:Event" + this.EventNumber;
                this.EventNumber = this.EventNumber + 1;
                return anID;
            }

        }
    }
}
