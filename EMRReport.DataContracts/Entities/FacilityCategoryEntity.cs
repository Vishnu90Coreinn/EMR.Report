using System;
using System.Collections.Generic;

namespace EMRReport.DataContracts.Entities
{
    public partial class FacilityCategoryEntity : BaseEntity
    {
        public int FacilityCategoryId { get; set; }
        public string FacilityCategory { get; set; }
        public Guid FacilityCategoryGuid { get; set; }
        public virtual ICollection<EncounterTypeEntity> encounterTypeEntity { get; set; }
    }
}
