using System;
using System.Collections.Generic;

namespace EMRReport.DataContracts.Entities
{
    public partial class OrganizationEntity : BaseEntity
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int? ClaimCount { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsUnlimited { get; set; }
        public Guid OrganizationGuid { get; set; }
        public virtual ICollection<FacilityEntity> facilityEntityList { get; set; }
    }
}
