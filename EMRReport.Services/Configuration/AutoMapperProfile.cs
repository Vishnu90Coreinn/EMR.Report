using AutoMapper;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.NotMappedEntities;
using EMRReport.ServiceContracts.ServiceObjects;

namespace EMRReport.Services.Configuration
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapForValidatorError();
            MapForScrubberError();
            MapForClaim();
            MapForClaimBasket();
            MapForClaimEncounter();
            MapForClaimActivity();
            MapForClaimDiagnosis();
            MapForClaimActivityObservation();
            MapForFacility();
            MapForPayerReceiver();
            MapForBasketGroup();
            MapForUser();
            MapForDashboard();
            MapForOrganization();
            MapForRegulatory();
            MapForFacilityType();
            MapForCompanyRole();
            MapForCountry();
            MapForState();
            MapForGroup();
            MapForControl();
            MapForEncounterType();
            MapForFacilityCategory();
            MapForInsuranceClassification();
            MapForValidatorTransaction();
            MapForValidatorICD();
            MapForValidatorCPT();
            MapForActivity();
            MapForUserHistory();
        }

        private void MapForValidatorError()
        {
            CreateMap<ValidatorErrorEntity, ValidatorErrorServiceObject>();
            CreateMap<ValidatorErrorServiceObject, ValidatorErrorEntity>();
        }
        private void MapForScrubberError()
        {
            CreateMap<ScrubberErrorServiceObject, ScrubberErrorEntity>();
            CreateMap<ScrubberErrorEntity, ScrubberErrorServiceObject>();
        }
        private void MapForClaimBasket()
        {
            CreateMap<ClaimBasketServiceObject, ClaimBasketEntity>();
            CreateMap<ClaimBasketEntity, ClaimBasketServiceObject>();
        }
        private void MapForClaim()
        {
            CreateMap<ClaimServiceObject, ClaimEntity>();
            CreateMap<ClaimEntity, ClaimServiceObject>();
        }
        private void MapForClaimEncounter()
        {
            CreateMap<ClaimEncounterServiceObject, ClaimEncounterEntity>();
            CreateMap<ClaimEncounterEntity, ClaimEncounterServiceObject>();
        }
        private void MapForClaimActivity()
        {
            CreateMap<ClaimActivityServiceObject, ClaimActivityEntity>();
            CreateMap<ClaimActivityEntity, ClaimActivityServiceObject>();
        }
        private void MapForClaimDiagnosis()
        {
            CreateMap<ClaimDiagnosisServiceObject, ClaimDiagnosisEntity>();
            CreateMap<ClaimDiagnosisEntity, ClaimDiagnosisServiceObject>();
        }
        private void MapForClaimActivityObservation()
        {
            CreateMap<ClaimActivityObservationServiceObject, ClaimActivityObservationEntity>();
            CreateMap<ClaimActivityObservationEntity, ClaimActivityObservationServiceObject>();
        }
        private void MapForFacility()
        {
            CreateMap<FacilityEntity, FacilityServiceObject>();
            CreateMap<FacilityServiceObject, FacilityEntity>();
            CreateMap<FacilityNMEntity, OrganizationServiceObject>()
            .ForMember(des => des.OrganizationName, o => o.MapFrom(src => src.Organization));
            CreateMap<FacilityNMEntity, RegulatoryServiceObject>()
           .ForMember(des => des.RegulatoryName, o => o.MapFrom(src => src.Regulatory));
            CreateMap<FacilityNMEntity, FacilityTypeServiceObject>()
            .ForMember(des => des.FacilityTypeName, o => o.MapFrom(src => src.FacilityType));
            CreateMap<FacilityNMEntity, FacilityServiceObject>();
        }
        private void MapForPayerReceiver()
        {
            CreateMap<PayerReceiverServiceObject, PayerReceiverEntity>();
            CreateMap<PayerReceiverEntity, PayerReceiverServiceObject>();
            CreateMap<PayerReceiverNMEntity, PayerReceiverServiceObject>();
            CreateMap<PayerReceiverNMEntity, FacilityServiceObject>()
                .ForMember(des => des.FacilityCode, opt => opt.MapFrom(src => src.Facility));
            CreateMap<PayerReceiverNMEntity, InsuranceClassificationServiceObject>()
                .ForMember(des => des.InsuranceClassificationName, opt => opt.MapFrom(src => src.InsuranceClassification));
        }

        private void MapForBasketGroup()
        {
            CreateMap<BasketGroupEntity, BasketGroupServiceObject>();
            CreateMap<BasketGroupServiceObject, BasketGroupEntity>();
        }
        private void MapForUser()
        {
            CreateMap<UserEntity, UserServiceObject>().IncludeMembers(src => src.addressEntity)
                 .ForMember(des => des.EncyptedPassword, x => x.MapFrom(src => src.Password));
            CreateMap<AddressEntity, UserServiceObject>(MemberList.None)
              .ForMember(des => des.City, x => x.MapFrom(src => src.CityName))
              .ForMember(des => des.CountryId, x => x.MapFrom(src => src.CountryID > 0 ? src.CountryID : null))
              .ForMember(des => des.StateId, x => x.MapFrom(src => src.StateID > 0 ? src.StateID : null))
              .ForMember(des => des.Email, x => x.MapFrom(src => src.Email))
              .ForMember(des => des.Fax, x => x.MapFrom(src => src.Fax))
              .ForMember(des => des.FullAddress, x => x.MapFrom(src => src.FullAddress))
              .ForMember(des => des.Mobile, x => x.MapFrom(src => src.Mobile))
              .ForMember(des => des.Phone, x => x.MapFrom(src => src.Phone))
              .ForMember(des => des.Street, x => x.MapFrom(src => src.StreetName));
            CreateMap<UserServiceObject, AddressEntity>()
                .ForMember(des => des.CityName, x => x.MapFrom(src => src.City))
                .ForMember(des => des.CountryID, x => x.MapFrom(src => src.CountryId > 0 ? src.CountryId : null))
                .ForMember(des => des.StateID, x => x.MapFrom(src => src.StateId > 0 ? src.StateId : null))
                .ForMember(des => des.Email, x => x.MapFrom(src => src.Email))
                .ForMember(des => des.Fax, x => x.MapFrom(src => src.Fax))
                .ForMember(des => des.FullAddress, x => x.MapFrom(src => src.FullAddress))
                .ForMember(des => des.Mobile, x => x.MapFrom(src => src.Mobile))
                .ForMember(des => des.Phone, x => x.MapFrom(src => src.Phone))
                .ForMember(des => des.StreetName, x => x.MapFrom(src => src.Street));
            CreateMap<UserServiceObject, UserEntity>()
                .ForMember(des => des.Password, x => x.MapFrom(src => src.EncyptedPassword))
                .ForMember(des => des.addressEntity, opt => opt.MapFrom(src => src));
            CreateMap<UserNMEntity, CompanyRoleServiceObject>()
                 .ForMember(des => des.CompanyRole, x => x.MapFrom(src => src.UserRole));
            CreateMap<UserNMEntity, CountryServiceObject>()
                .ForMember(des => des.Country, x => x.MapFrom(src => src.Country));
            CreateMap<UserNMEntity, StateServiceObject>();
            CreateMap<UserNMEntity, UserServiceObject>()
                .ForMember(des => des.Active, x => x.MapFrom(src => src.Status))
                 .ForMember(des => des.EncyptedPassword, x => x.MapFrom(src => src.Password));

        }
        private void MapForDashboard()
        {
            CreateMap<DashBoardNMEntity, DashBoardServiceObject>();
        }
        private void MapForOrganization()
        {
            CreateMap<OrganizationEntity, OrganizationServiceObject>();
            CreateMap<OrganizationServiceObject, OrganizationEntity>();
        }
        private void MapForRegulatory()
        {
            CreateMap<RegulatoryEntity, RegulatoryServiceObject>();
            CreateMap<RegulatoryServiceObject, RegulatoryEntity>();
        }
        private void MapForFacilityType()
        {
            CreateMap<FacilityTypeEntity, FacilityTypeServiceObject>();
            CreateMap<FacilityTypeServiceObject, FacilityTypeEntity>();
        }
        private void MapForCompanyRole()
        {
            CreateMap<CompanyRoleEntity, CompanyRoleServiceObject>();
            CreateMap<CompanyRoleServiceObject, CompanyRoleEntity>();
        }
        private void MapForCountry()
        {
            CreateMap<CountryEntity, CountryServiceObject>();
            CreateMap<CountryServiceObject, CountryEntity>();
        }
        private void MapForState()
        {
            CreateMap<StateEntity, StateServiceObject>();
            CreateMap<StateServiceObject, StateEntity>();
        }
        private void MapForGroup()
        {
            CreateMap<GroupEntity, GroupServiceObject>();
            CreateMap<GroupServiceObject, GroupEntity>();
        }
        private void MapForControl()
        {
            CreateMap<ControlEntity, ControlServiceObject>();
            CreateMap<ControlServiceObject, ControlEntity>();
        }
        private void MapForEncounterType()
        {
            CreateMap<EncounterTypeEntity, EncounterTypeServiceObject>()
                .ForMember(des => des.EncounterType, opt => opt.MapFrom(src => src.EncounterType));
            CreateMap<EncounterTypeServiceObject, EncounterTypeEntity>()
                .ForMember(des => des.EncounterType, opt => opt.MapFrom(src => src.EncounterType));
        }
        private void MapForFacilityCategory()
        {
            CreateMap<FacilityCategoryEntity, FacilityCategoryServiceObject>()
                .ForMember(des => des.FacilityCategoryName, opt => opt.MapFrom(src => src.FacilityCategory));
            CreateMap<FacilityCategoryServiceObject, FacilityCategoryEntity>()
                 .ForMember(des => des.FacilityCategory, opt => opt.MapFrom(src => src.FacilityCategoryName));
        }

        private void MapForInsuranceClassification()
        {
            CreateMap<InsuranceClassificationServiceObject, InsuranceClassificationEntity>()
                .ForMember(des => des.InsuranceClassification, opt => opt.MapFrom(src => src.InsuranceClassificationName));
            CreateMap<InsuranceClassificationEntity, InsuranceClassificationServiceObject>()
                .ForMember(des => des.InsuranceClassificationName, opt => opt.MapFrom(src => src.InsuranceClassification));
        }
        private void MapForValidatorTransaction()
        {
            CreateMap<ValidatorTransactionServiceObject, ValidatorTransactionEntity>();
            CreateMap<ValidatorTransactionEntity, ValidatorTransactionServiceObject>();
        }
        private void MapForValidatorICD()
        {
            CreateMap<ValidatorICDServiceObject, ValidatorICDEntity>();
            CreateMap<ValidatorICDEntity, ValidatorICDServiceObject>();
        }
        private void MapForValidatorCPT()
        {
            CreateMap<ValidatorCPTServiceObject, ValidatorCPTEntity>();
            CreateMap<ValidatorCPTEntity, ValidatorCPTServiceObject>();
        }
        private void MapForActivity()
        {
            CreateMap<ActivityServiceObject, ActivityEntity>();
            CreateMap<ActivityEntity, ActivityServiceObject>();
        }
        private void MapForUserHistory()
        {
            CreateMap<UserHistoryServiceObject, UserHistoryEntity>();
            CreateMap<UserHistoryEntity, UserHistoryServiceObject>();
        }
    }
}