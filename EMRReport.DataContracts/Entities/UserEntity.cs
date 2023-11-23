using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EMRReport.DataContracts.Entities
{
    public partial class UserEntity
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddressID { get; set; }
        public int? CompanyRoleID { get; set; }
        public int UserTypeID { get; set; }
        public int LoginTried { get; set; }
        public bool LoginLocked { get; set; }
        public bool? EnableMultipleLogin { get; set; }
        public int AuthorityType { get; set; }
        public int RuleVersion { get; set; }
        public int ApplicationType { get; set; }
        public bool IsSignUp { get; set; }
        public int SignUpStatus { get; set; }
        public string VerificationToken { get; set; }
        public string ClientFacilityCode { get; set; }
        public bool Status { get; set; }
        public bool EmailVerified { get; set; }
        public string Reason { get; set; }
        public Guid UserGuid { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual AddressEntity addressEntity { get; set; }
        public virtual CompanyRoleEntity companyRoleEntity { get; set; }
        public virtual UserEntity userEntityParentCreated { get; set; }
        public virtual ICollection<UserEntity> userEntityParentCreatedList { get; set; }
        public virtual UserEntity userEntityParentModified { get; set; }
        public virtual ICollection<UserEntity> userEntityParentModifiedList { get; set; }
        public virtual ICollection<ValidatorErrorEntity> validatorErrorEntityList { get; set; }
        public virtual ICollection<ClaimBasketEntity> claimBasketEntityList { get; set; }
        public virtual ICollection<ControlEntity> controlEntityList { get; set; }
        public virtual ICollection<CompanyEntity> companyEntityCreatedList { get; set; }
        public virtual ICollection<CompanyEntity> companyEntityModifiedList { get; set; }
        public virtual ICollection<GroupControlEntity> GroupControlEntityCreatedList { get; set; }
        public virtual ICollection<GroupControlEntity> GroupControlEntityModifiedList { get; set; }
        public virtual ICollection<GroupEntity> groupEntityCreatedList { get; set; }
        public virtual ICollection<GroupEntity> groupEntityModifiedList { get; set; }
        public virtual ICollection<CompanyRoleFacilityEntity> companyRoleFacilityEntityCreatedList { get; set; }
        public virtual ICollection<CompanyRoleFacilityEntity> companyRoleFacilityEntityModifiedList { get; set; }
        public virtual ICollection<RoleGroupEntity> roleGroupEntityCreatedList { get; set; }
        public virtual ICollection<RoleGroupEntity> roleGroupEntityModifiedList { get; set; }
        public virtual ICollection<CompanyRoleEntity> companyRoleEntityCreatedList { get; set; }
        public virtual ICollection<CompanyRoleEntity> companyRoleEntityModifiedList { get; set; }
        public virtual ICollection<AddressEntity> addressEntityCreatedList { get; set; }
        public virtual ICollection<AddressEntity> addressEntityModifiedList { get; set; }
        public virtual ICollection<FacilityEntity> facilityEntityCreatedList { get; set; }
        public virtual ICollection<FacilityEntity> facilityEntityModifiedList { get; set; }
        public virtual ICollection<PayerReceiverEntity> payerReceiverCreatedList { get; set; }
        public virtual ICollection<PayerReceiverEntity> payerReceiverModifiedList { get; set; }
        public virtual ICollection<OrganizationEntity> organizationEntityCreatedList { get; set; }
        public virtual ICollection<OrganizationEntity> organizationEntityModifiedList { get; set; }
        public virtual ICollection<FacilityTypeEntity> facilityTypeEntitCreatedList { get; set; }
        public virtual ICollection<FacilityTypeEntity> facilityTypeEntityModifiedList { get; set; }
        public virtual ICollection<RegulatoryEntity> regulatoryEntityCreatedList { get; set; }
        public virtual ICollection<RegulatoryEntity> regulatoryEntityModifiedList { get; set; }
        public virtual ICollection<CountryEntity> countryEntityCreatedList { get; set; }
        public virtual ICollection<CountryEntity> countryEntityModifiedList { get; set; }
        public virtual ICollection<StateEntity> stateEntityCreatedList { get; set; }
        public virtual ICollection<StateEntity> stateEntityModifiedList { get; set; }
        public virtual ICollection<EncounterTypeEntity> encounterTypeEntityCreatedList { get; set; }
        public virtual ICollection<EncounterTypeEntity> encounterTypeEntityModifiedList { get; set; }
        public virtual ICollection<FacilityCategoryEntity> facilityCategoryEntityCreatedList { get; set; }
        public virtual ICollection<FacilityCategoryEntity> facilityCategoryEntityModifiedList { get; set; }
        public virtual ICollection<InsuranceClassificationEntity> insuranceClassificationEntityCreatedList { get; set; }
        public virtual ICollection<InsuranceClassificationEntity> insuranceClassificationEntityModifiedList { get; set; }
        public virtual ICollection<ActivityEntity> activityEntityCreatedList { get; set; }
        public virtual ICollection<ActivityEntity> activityEntityModifiedList { get; set; }
        public virtual ICollection<SettingsEntity> settingsEntityCreatedList { get; set; }
        public virtual ICollection<SettingsEntity> settingsEntityModifiedList { get; set; }
        public virtual ICollection<ActivityRulesEntity> activityRulesEntityCreatedList { get; set; }
        public virtual ICollection<ActivityRulesEntity> activityRulesEntityModifiedList { get; set; }
        public virtual ICollection<RulesEntity> rulesEntityCreatedList { get; set; }
        public virtual ICollection<RulesEntity> rulesEntityModifiedList { get; set; }
        public virtual ICollection<UserHistoryEntity> userHistoryEntityList { get; set; }

    }
}