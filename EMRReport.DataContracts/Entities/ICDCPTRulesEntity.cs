namespace EMRReport.DataContracts.Entities
{
    public sealed class ICDCPTRulesEntity : BaseEntity
    {
        public int ICDCPTRulesID { get; set; }
        public string RuleCPT { get; set; }
        public string RuleICD { get; set; }
        public int RulesID { get; set; }
        public string Message { get; set; }
        public int MaxQuantity { get; set; }
    }
}