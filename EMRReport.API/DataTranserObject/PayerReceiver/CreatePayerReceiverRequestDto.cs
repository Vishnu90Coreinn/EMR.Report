using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.PayerReceiver
{
    public sealed class CreatePayerReceiverRequestDto
    {
        public string PayerReceiverName { get; set; }
        public string PayerReceiverShortName { get; set; }
        public string PayerReceiverIdentification { get; set; }
        public int FacilityID { get; set; }
        public int RegulatoryID { get; set; }
        public int InsuranceClassificationID { get; set; }
    }
}
