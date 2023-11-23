using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.DashBoard
{
    public sealed class GetClaimCounterResponseDto
    {
        public string SenderID { get; set; }
        public string DateField { get; set; }
        public int ClaimCount { get; set; }
    }
}
