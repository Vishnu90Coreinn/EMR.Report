using System;
using System.Collections.Generic;
namespace EMRReport.DataContracts.Entities
{
    public partial class PayerReceiverEntity : BaseEntity
    {
        public int PayerReceiverID { get; set; }
        public string PayerReceiverName { get; set; }
        public string PayerReceiverShortName { get; set; }
        public string PayerReceiverIdentification { get; set; }
        public string ReceiverID { get; set; }
        public int FacilityID { get; set; }
        public int RegulatoryID { get; set; }
        public Guid PayerReceiverGuid { get; set; }
        public int InsuranceClassificationID { get; set; }
        public string RecieverFacilityID { get; set; }
        public string PayerReceiverIdentificationValidate { get; set; }
        public bool IsTopUp { get; set; }
        public bool IsEnabledFactorSettings { get; set; }
        public bool IsEnabledMultipleDentalSettings { get; set; }
        public bool IsEnabledMultipleTariffSettings { get; set; }
        public string VatRegistrationNumber { get; set; }
        public virtual FacilityEntity facilityEntity { get; set; }
        public virtual InsuranceClassificationEntity insuranceClassificationEntity { get; set; }
        public virtual RegulatoryEntity regulatoryEntity { get; set; }
        public virtual ICollection<ClaimBasketEntity> claimBasketEntityList { get; set; }

    }
}