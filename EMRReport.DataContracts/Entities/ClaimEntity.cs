using System;
namespace EMRReport.DataContracts.Entities
{
    public sealed class ClaimEntity
    {
        public int ClaimEntityID { get; set; }
        public int ClaimBasketID { get; set; }
        public int XMLClaimTagID { get; set; }
        public string ClaimID { get; set; }
        public string PayerID { get; set; }
        public string MemberID { get; set; }
        public string EmiratesIDNumber { get; set; }
        public string MemberFormatID { get; set; }
        public decimal ClaimGross { get; set; }
        public decimal ClaimNet { get; set; }
        public decimal PatientShare { get; set; }
        public string ServiceCodes { get; set; }
        public string PrimaryICD { get; set; }
        public string SecondaryICDS { get; set; }
        public string ReasonForVisitICDS { get; set; }
    }
}