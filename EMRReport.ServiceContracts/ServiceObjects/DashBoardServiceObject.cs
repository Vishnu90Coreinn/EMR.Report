using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class DashBoardServiceObject
    {
        //encounter
        public string DateRange { get; set; }
        public string SenderID { get; set; }
        public int EncounterType { get; set; }
        public int ClaimCountHavingErrors { get; set; }
        public int ClaimCountWithOutErrors { get; set; }
        public int TotalClaimCount { get; set; }
        public decimal ClaimAmountHavingErrors { get; set; }
        public decimal ClaimAmountWithOutErrors { get; set; }
        public decimal TotalClaimAmount { get; set; }
        //Categoty
        public string ScrubberErrorCategory { get; set; }
        public string ScrubberPrefixType { get; set; }
        public int ErrorCount { get; set; }
        //summary
        public decimal ValidateFiles { get; set; }
        public decimal TotalUploadedFiles { get; set; }
        public int ClaimErrorCount { get; set; }
        public int NotHitClaimCount { get; set; }
        public decimal ClaimErrorNet { get; set; }
        public decimal ClaimWithOutErrorNet { get; set; }
        public decimal TotalClaimNET { get; set; }
        public decimal NonValidatedFiles { get; set; }
        public decimal ClaimWithOutErrorCount { get; set; }
        // Claim Counter
        public string DateField { get; set; }
        public int ClaimCount { get; set; }

    }
}
