using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.ScrubberRule
{
    public sealed class ScrubberRulesMasterRequestDto
    {
        public int ScrubberRulesID { get; set; }
        public string RuleName { get; set; }
        public int RuleType { get; set; }
        public string RulePrefix { get; set; }
        public string RulePXID { get; set; }
        public bool Status { get; set; }
        public bool VStatus { get; set; }
        public int ScrubberErrorCategory { get; set; }
        public int ScrubberPrefixType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}