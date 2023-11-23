using System;
using System.Collections.Generic;
namespace EMRReport.DataContracts.Entities
{
    public partial class FacilityEntity : BaseEntity
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public int FacilityTypeID { get; set; }
        public bool IsNurse { get; set; }
        public string FacilityWiseLandingCode { get; set; }
        public string WebServiceUserName { get; set; }
        public string WebServicePassword { get; set; }
        public string VatRegistrationNo { get; set; }
        public bool IsEnabledAutoDownload { get; set; }
        public bool IsEnabledAutoReconcile { get; set; }
        public DateTime? ProductivityReportDate { get; set; }
        public bool IsInsta { get; set; }
        public int? LinkedToFacilityID { get; set; }
        public int CompanyID { get; set; }
        public int VisCoreLisenceTypeID { get; set; }
        public int RegulatoryID { get; set; }
        public bool IsDOS { get; set; }
        public bool IsAbuDhabiDOS { get; set; }
        public int? OrganizationID { get; set; }
        public int? ClaimCount { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsUnlimited { get; set; }
        public Guid FacilityGuid { get; set; }
        public virtual CompanyEntity companyEntity { get; set; }
        public virtual FacilityEntity facilityEntityParent { get; set; }
        public virtual OrganizationEntity organizationEntity { get; set; }
        public virtual FacilityTypeEntity facilityTypeEntity { get; set; }
        public virtual RegulatoryEntity regulatoryEntity { get; set; }
        public virtual ICollection<FacilityEntity> facilityEntityParentList { get; set; }
        public virtual ICollection<ClaimBasketEntity> claimBasketEntityList { get; set; }
        public virtual ICollection<CompanyRoleFacilityEntity> companyRoleFacilityEntityList { get; set; }
        public virtual ICollection<PayerReceiverEntity> payerReceiverList { get; set; }
    }
}