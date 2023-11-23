using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.PayerReceiver
{
    public sealed class GetPayerReceiverDDLResponseDto
    {
        public int PayerReceiverID { get; set; }
        public string PayerReceiverName { get; set; }
    }
}
