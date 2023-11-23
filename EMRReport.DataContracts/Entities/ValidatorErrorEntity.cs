using System;
namespace EMRReport.DataContracts.Entities
{
    public partial class ValidatorErrorEntity
    {
        public int ValidatorErrorID { get; set; }
        public string CaseNumber { get; set; }
        public string ErrorCode1 { get; set; }
        public string ErrorCode2 { get; set; }
        public string Message { get; set; }
        public string CodingTips { get; set; }
        public bool Status { get; set; }
        public int ValidatedBy { get; set; }
        public DateTime ValidatedDate { get; set; }
        public int PrefixType { get; set; }
        public int ErrorCategory { get; set; }
        public int ValidatorTransactionID { get; set; }
        public virtual UserEntity userEntityValidated { get; set; }
    }
}