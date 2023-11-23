using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EMRReport.API.DataTranserObject.ScrubberError
{
    public sealed class GetScrubberErrorExternalRequestDto
    {
        public List<IFormFile> XMLfiles { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
