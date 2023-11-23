using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMRReport.DataContracts.Entities
{
    public sealed class CPTObservationRulesEntity : BaseEntity
    {
        public int CPTObservationRulesID { get; set; }
        public string RuleActivity { get; set; }
        public int RulesID { get; set; }
        public string Message { get; set; }
    }
}