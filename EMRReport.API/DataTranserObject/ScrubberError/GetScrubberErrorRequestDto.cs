using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EMRReport.API.DataTranserObject.ScrubberError
{
    public sealed class GetScrubberErrorRequestDto
    {
        public List<IFormFile> XMLfiles { get; set; }
    }
}
