using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class ValidatorTransactionServiceObject
    {
        public int ValidatorTransactionID { get; set; }
        public bool Status { get; set; }
    }
}
