using System;
namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class ValidatorErrorServiceObject
    {
        public string CaseNumber { get; set; }
        public string ErrorCode1 { get; set; }
        public string ErrorCode2 { get; set; }
        public string CPTS { get; set; }
        public string ICDS { get; set; }
        public int PrefixType { get; set; }
        public int ErrorCategory { get; set; }
        public string Message { get; set; }
        public string CodingTips { get; set; }
        public bool Status { get; set; }
        public int ValidatedBy { get; set; }
        public DateTime ValidatedDate { get; set; }
    }
}
