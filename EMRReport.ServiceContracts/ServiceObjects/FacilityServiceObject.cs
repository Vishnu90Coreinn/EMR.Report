using System;
namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class FacilityServiceObject
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public int FacilityTypeID { get; set; }
        public string FacilityType { get; set; }
        public int RegulatoryID { get; set; }
        public string Regulatory { get; set; }
        public bool IsDOS { get; set; }
        public bool IsAbuDhabiDOS { get; set; }
        public int? OrganizationID { get; set; }
        public string Organization { get; set; }
        public int? ClaimCount { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsUnlimited { get; set; }
        public bool Status { get; set; }
    }
}