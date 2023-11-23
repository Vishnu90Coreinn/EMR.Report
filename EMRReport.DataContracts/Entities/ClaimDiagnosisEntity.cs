using System;
namespace EMRReport.DataContracts.Entities
{
    public sealed class ClaimDiagnosisEntity
    {
        public int ClaimDiagnosisID { get; set; }
        public int ClaimBasketID { get; set; }
        public int XMLClaimTagID { get; set; }
        public string DiagnosisCode { get; set; }
        public string Type { get; set; }
        public bool IsPrimary { get; set; }
        public bool ReasonForVisit { get; set; }
    }
}