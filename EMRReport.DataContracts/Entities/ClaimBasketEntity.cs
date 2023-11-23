using System;
namespace EMRReport.DataContracts.Entities
{
    public partial class ClaimBasketEntity
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
        public virtual FacilityEntity facilityEntity { get; set; }
        public int? PayerReceiverID { get; set; }
        public virtual PayerReceiverEntity payerReceiverEntity { get; set; }
        public int? RegulatoryID { get; set; }
        public virtual RegulatoryEntity regulatoryEntity { get; set; }
        public int? CreatedBy { get; set; }
        public virtual UserEntity UserEntityCreated { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? BasketGroupID { get; set; }
        public virtual BasketGroupEntity basketGroupEntity { get; set; }

    }
}