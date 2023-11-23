using System;
using System.Collections.Generic;
using System.Text;


namespace EMRReport.DataContracts.Entities
{
    public sealed class EncounterTypeEntity : BaseEntity
    {
        public int ID { get; set; }
        public int EncounterTypeID { get; set; }
        public string EncounterType { get; set; }
        public Guid EncounterTypeGuid { get; set; }
    }
}
