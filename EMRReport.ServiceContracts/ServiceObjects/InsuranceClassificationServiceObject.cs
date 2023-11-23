using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class InsuranceClassificationServiceObject
    {
        public int InsuranceClassificationID { get; set; }
        public string InsuranceClassificationName { get; set; }
        public bool Status { get; set; }
    }
}
