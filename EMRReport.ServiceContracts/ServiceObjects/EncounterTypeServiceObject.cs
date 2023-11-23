using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class EncounterTypeServiceObject
    {
        public int ID { get; set; }
        public int EncounterTypeID { get; set; }
        public string EncounterType { get; set; }
    }
}
