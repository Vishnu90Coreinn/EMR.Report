using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMRReport.DataContracts.Entities
{
    public partial class BasketGroupEntity
    {
        public int BasketGroupID { get; set; }
        public virtual ICollection<ClaimBasketEntity> claimBasketEntityList { get; set; }
    }
}