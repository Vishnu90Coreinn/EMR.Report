using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Facility
{
    public sealed class GetFacilityResponseDto
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string Regulatory { get; set; }
        public string IsDOS { get; set; }
        public string IsAbuDhabiDOS { get; set; }
        public string Organization { get; set; }
        public int? ClaimCount { get; set; }
        public string SubscriptionStartDate { get; set; }
        public string SubscriptionEndDate { get; set; }
        public string IsUnlimited { get; set; }
        public string Status { get; set; }
    }
}
