using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMRReport.DataContracts.Entities
{
    public partial class RulesEntity : BaseEntity
    {
        public int RulesID { get; set; }
        public string RuleName { get; set; }
        public int RuleType { get; set; }
        public string RulePrefix { get; set; }
        public string RulePXID { get; set; }
        public bool VStatus { get; set; }
        public bool IsDOSRule { get; set; }
        public bool IsAUHRule { get; set; }
        public bool IsBothRule { get; set; }
        public int ScrubberErrorCategory { get; set; }
        public int ScrubberPrefixType { get; set; }
        public int AuthorityType { get; set; }
        public int RuleVersion { get; set; }
        public string CodingTips { get; set; }
        public string PayerIDS { get; set; }
        public string Classification { get; set; }
        public virtual ICollection<ActivityRulesEntity> activityRulesEntityList { get; set; }
    }
}