using System;
using System.Collections.Generic;
namespace EMRReport.DataContracts.Entities
{
    public partial class InsuranceClassificationEntity : BaseEntity
    {
        public int InsuranceClassificationId { get; set; }
        public string InsuranceClassification { get; set; }
        public Guid InsuranceClassificationGuid { get; set; }
        public virtual ICollection<PayerReceiverEntity> payerReceiverEntity { get; set; }
    }
}
