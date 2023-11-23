using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.DashBoard
{
    public sealed class GetErrorSummaryResponseDto
    {
        public decimal ValidateFiles { get; set; }
        public decimal TotalUploadedFiles { get; set; }
        public int ClaimErrorCount { get; set; }
        public int ClaimWithOutErrorCount { get; set; }//public int NotHitClaimCount { get; set; }
        public int TotalClaimCount { get; set; }
        public decimal ClaimErrorNet { get; set; }
        public decimal ClaimWithOutErrorNet { get; set; }
        public decimal TotalClaimNET { get; set; }
        public decimal NonValidatedFiles { get; set; }

    }
}
