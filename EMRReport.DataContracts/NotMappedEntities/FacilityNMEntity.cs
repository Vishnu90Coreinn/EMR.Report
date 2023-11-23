using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.NotMappedEntities
{
    public sealed class FacilityNMEntity
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string Regulatory { get; set; }
        public bool IsDOS { get; set; }
        public bool IsAbuDhabiDOS { get; set; }
        public string Organization { get; set; }
        public int? ClaimCount { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsUnlimited { get; set; }
        public bool Status { get; set; }
    }
}
