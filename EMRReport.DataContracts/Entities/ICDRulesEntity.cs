using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMRReport.DataContracts.Entities
{
    public sealed class ICDRulesEntity : BaseEntity
    {
        public int ICDRulesID { get; set; }
        public string RuleICD { get; set; }
        public int RulesID { get; set; }
        public string MessageIndex { get; set; }
        public string Message { get; set; }
        public int? AgeMax { get; set; }
        public int? AgeMin { get; set; }
        public string Description { get; set; }
    }
}