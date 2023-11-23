using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMRReport.DataContracts.Entities
{
    public partial class FacilityTypeEntity : BaseEntity
    {
        public int FacilityTypeID { get; set; }
        public string FacilityTypeName { get; set; }
        public int FacilityCategoryID { get; set; }
        public Guid FacilityTypeGuid { get; set; }
        public virtual ICollection<FacilityEntity> facilityEntityList { get; set; }
    }
}