using System;
namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class PayerReceiverServiceObject
    {
        public int PayerReceiverID { get; set; }
        public string PayerReceiverName { get; set; }
        public string PayerReceiverShortName { get; set; }
        public string PayerReceiverIdentification { get; set; }
        public int FacilityID { get; set; }
        public string Facility { get; set; }
        public int RegulatoryID { get; set; }
        public string Regulatory { get; set; }
        public int InsuranceClassificationID { get; set; }
        public string InsuranceClassification { get; set; }
        public bool Status { get; set; }

    }
}