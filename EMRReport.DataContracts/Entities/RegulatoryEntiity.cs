using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMRReport.DataContracts.Entities
{
    public partial class RegulatoryEntity : BaseEntity
    {
        public int RegulatoryID { get; set; }
        public string RegulatoryName { get; set; }
        public Guid RegulatoryGuid { get; set; }
        public virtual ICollection<ClaimBasketEntity> claimBasketEntityList { get; set; }
        public virtual ICollection<FacilityEntity> facilityEntityList { get; set; }
        public virtual ICollection<PayerReceiverEntity> payerReceiverList { get; set; }
    }
}