using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.DashBoard
{
    public sealed class GetEncounterWiseResponseDto
    {
        public string SenderID { get; set; }
        public int EncounterType { get; set; }
        public int ClaimCountHavingErrors { get; set; }
        public int ClaimCountWithOutErrors { get; set; }
        public int TotalClaimCount { get; set; }
        public decimal ClaimAmountHavingErrors { get; set; }
        public decimal ClaimAmountWithOutErrors { get; set; }
        public decimal TotalClaimAmount { get; set; }
    }
}
