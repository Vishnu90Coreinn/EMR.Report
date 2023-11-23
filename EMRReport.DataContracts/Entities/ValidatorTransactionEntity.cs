using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public sealed class ValidatorTransactionEntity
    {
        public int ValidatorTransactionID { get; set; }
        public bool Status { get; set; }
    }
}
