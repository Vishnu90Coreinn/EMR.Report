using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.ValidatorError
{
    public sealed class GetValidatorCPTRequestDto
    {
        public int ValidatorCPTID { get; set; }
        public int ValidatorTransactionID { get; set; }
        public string CPT { get; set; }
        public decimal Quantity { get; set; }
        public int ActivityType { get; set; }
        public decimal Net { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool Status { get; set; }
    }
}
