using System;
namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class ClaimEncounterServiceObject
    {
        public int ClaimEncounterID { get; set; }
        public int ClaimBasketID { get; set; }
        public int EncounterType { get; set; }
        public string PatientID { get; set; }
        public string Start { get; set; }
        public int XMLClaimTagID { get; set; }
        public string End { get; set; }
        public string EncounterStartType { get; set; }
        public string EncounterEndType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string Network { get; set; }
        public string PlanId { get; set; }
        public bool IsDOS { get; set; }
    }
}