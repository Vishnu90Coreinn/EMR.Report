using System;
namespace EMRReport.DataContracts.Entities
{
    public sealed class ClaimActivityEntity
    {
        public int ClaimActivityID { get; set; }
        public int ClaimBasketID { get; set; }
        public int XMLClaimTagID { get; set; }
        public int ActivityJoinId { get; set; }
        public string ServiceCode { get; set; }
        public int ActivityType { get; set; }
        public decimal Quantity { get; set; }
        public string Start { get; set; }
        public string ActivityID { get; set; }
        public string PriorAuthorizationID { get; set; }
        public decimal Net { get; set; }
        public int? XMLActivityTagID { get; set; }
        public string ClinicianLicense { get; set; }
        public string ActivityClinicianLicense { get; set; }
        public string OrderingClinician { get; set; }
        public string ActivityClinician { get; set; }
        public decimal VAT { get; set; }
    }
}