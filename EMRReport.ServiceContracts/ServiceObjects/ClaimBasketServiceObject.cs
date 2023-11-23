using System;
namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class ClaimBasketServiceObject
    {
        public int ClaimBasketID { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string TransactionDate { get; set; }
        public int RecordCount { get; set; }
        public string DispositionFlag { get; set; }
        public string XMLFileName { get; set; }
        public string FileName { get; set; }
        public bool Status { get; set; }
        public int? FacilityID { get; set; }
        public int? PayerReceiverID { get; set; }
        public int? RegulatoryID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? BasketGroupID { get; set; }
    }
}