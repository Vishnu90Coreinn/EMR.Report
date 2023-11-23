using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.PayerReceiver
{
    public sealed class BulkSavePayerReceiverRequestDto
    {
        public IFormFile Excelfile { get; set; }
    }
}
