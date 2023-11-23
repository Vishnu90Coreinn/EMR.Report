using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class CompanyEntity : BaseEntity
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLicenseToken { get; set; }
        public int ViscoreLicenseTypeId { get; set; }
        public DateTime? LicenseStartDate { get; set; }
        public DateTime? LicenseEndDate { get; set; }
        public int MrngenerationTypeId { get; set; }
        public string CompanyWiseLandingCode { get; set; }
        public string TopNotificationEmails { get; set; }
        public string NotificationFromEmailUserName { get; set; }
        public string NotificationFromEmailPassword { get; set; }
        public Guid CompanyGuid { get; set; }
        public virtual ICollection<CompanyRoleFacilityEntity> CompanyRoleFacilityEntityList { get; set; }
        public virtual ICollection<CompanyRoleEntity> companyRoleEntityList { get; set; }
        public virtual ICollection<FacilityEntity> facilityEntityList { get; set; }
        //public virtual ICollection<FacilitySingleMaster> FacilitySingleMaster { get; set; }
    }
}
