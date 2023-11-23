using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.DashBoard
{
    public sealed class GetErrorCategoryResponseDto
    {
        public string SenderID { get; set; }
        public string ScrubberErrorCategory { get; set; }
        public string ScrubberPrefixType { get; set; }
        public int ErrorCount { get; set; }
    }
}
