using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Facility
{
    public sealed class UpdateFacilityRequestDto
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public int FacilityTypeID { get; set; }
        public int RegulatoryID { get; set; }
        public bool IsDOS { get; set; }
        public bool IsAbuDhabiDOS { get; set; }
        public int? OrganizationID { get; set; }
        public int? ClaimCount { get; set; }
        public string SubscriptionStartDate { get; set; }
        public string SubscriptionEndDate { get; set; }
        public bool IsUnlimited { get; set; }
    }
}
