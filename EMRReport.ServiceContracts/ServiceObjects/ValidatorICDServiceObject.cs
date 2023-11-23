using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class ValidatorICDServiceObject
    {
        public int ValidatorICDID { get; set; }
        public int ValidatorTransactionID { get; set; }
        public string ICD { get; set; }
        public bool Primary { get; set; }
        public bool Secondary { get; set; }
        public bool ReasonForVisit { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool Status { get; set; }
    }
}
