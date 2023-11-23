using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.PayerReceiver
{
    public sealed class GetPayerReceiverResponseDto
    {
        public int PayerReceiverID { get; set; }
        public string PayerReceiverName { get; set; }
        public string PayerReceiverShortName { get; set; }
        public string PayerReceiverIdentification { get; set; }
        public string Facility { get; set; }
        public string Regulatory { get; set; }
        public string InsuranceClassification { get; set; }
        public string Status { get; set; }
    }
}
