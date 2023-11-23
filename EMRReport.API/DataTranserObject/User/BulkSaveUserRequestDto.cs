using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class BulkSaveUserRequestDto
    {
        public IFormFile Excelfile { get; set; }
    }
}
