using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Facility
{
    public sealed class BulkSaveFacilityRequestDto
    {
        public IFormFile Excelfile { get; set; }
    }
}
