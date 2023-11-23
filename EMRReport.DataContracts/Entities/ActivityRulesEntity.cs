using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMRReport.DataContracts.Entities
{
    public partial class ActivityRulesEntity : BaseEntity
    {
        public int ActivityRulesID { get; set; }
        public string RuleActivity { get; set; }
        public int RulesID { get; set; }
        public string Message { get; set; }
        public int? ActivityMax { get; set; }
        public int? ActivityType { get; set; }
        public int? ActivityClinicMax { get; set; }
        public int? GroupID { get; set; }
        public int? GroupCount { get; set; }
        public int? AgeMax { get; set; }
        public int? AgeMin { get; set; }
        public virtual RulesEntity rulesEntity { get; set; }

    }
}