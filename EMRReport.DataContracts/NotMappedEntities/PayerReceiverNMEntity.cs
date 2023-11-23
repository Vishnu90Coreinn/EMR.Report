using System;
namespace EMRReport.DataContracts.NotMappedEntities
{
    public sealed class PayerReceiverNMEntity
    {
        public int PayerReceiverID { get; set; }
        public string PayerReceiverName { get; set; }
        public string PayerReceiverShortName { get; set; }
        public string PayerReceiverIdentification { get; set; }
        public string Facility { get; set; }
        public string Regulatory { get; set; }
        public string InsuranceClassification { get; set; }
        public bool Status { get; set; }

    }
}