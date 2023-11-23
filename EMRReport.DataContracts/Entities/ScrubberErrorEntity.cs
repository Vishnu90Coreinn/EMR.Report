using System;
namespace EMRReport.DataContracts.Entities
{
    public sealed class ScrubberErrorEntity
    {
        public int ScrubberErrorID { get; set; }
        public int ClaimBasketID { get; set; }
        public int XMLClaimTagID { get; set; }
        public int ScrubberPrefixType { get; set; }
        public int ScrubberErrorCategory { get; set; }
        public bool IsHit { get; set; }
        public int CreatedBy { get; set; }
        public int RoleID { get; set; }
        public bool IsValidated { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public string ClaimID { get; set; }
        public string ErrorHit { get; set; }
        public string ErrorCode1 { get; set; }
        public string ErrorCode2 { get; set; }
        public string Message { get; set; }
        public string CodingTips { get; set; }
        public string FileName { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string TransactionDate { get; set; }
        public int RecordCount { get; set; }
        public string DispositionFlag { get; set; }
        public decimal ClaimGross { get; set; }
        public decimal ClaimNet { get; set; }
        public decimal PatientShare { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public int EncounterType { get; set; }
        public string EncounterStartType { get; set; }
        public string EncounterEndType { get; set; }
        public string MemberID { get; set; }
        public string EmiratesID { get; set; }
        public string MRN { get; set; }
        public string Start { get; set; }
        public string EncounterEnd { get; set; }
        public string ActivityID { get; set; }
        public string ServiceCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Net { get; set; }
        public string ActivityStart { get; set; }
        public decimal VAT { get; set; }
        public int ActivityType { get; set; }
        public string PriorAuthorizationID { get; set; }
        public string OrderingClinician { get; set; }
        public string ActivityClinician { get; set; }
        public string CPTS { get; set; }
        public string PrimaryICD { get; set; }
        public string SecondaryICDS { get; set; }
        public string ReasonForVisitICDS { get; set; }
        public DateTime ErrorDate { get; set; }
    }
}