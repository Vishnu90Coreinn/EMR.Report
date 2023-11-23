using AutoMapper;
using System.Linq;
using EMRReport.ServiceContracts.ServiceObjects;
using EMRReport.Common.ProjectEnums;
using EMRReport.Common.PwdEncryption;
using EMRReport.Common.Extensions;
using EMRReport.API.DataTranserObject.FacilityCategory;
using EMRReport.API.DataTranserObject.Facility;
using EMRReport.API.DataTranserObject.Group;
using EMRReport.API.DataTranserObject.EncounterType;
using EMRReport.API.DataTranserObject.DashBoard;
using EMRReport.API.DataTranserObject.User;
using EMRReport.API.DataTranserObject.Activity;
using EMRReport.API.DataTranserObject.Regulatory;
using EMRReport.API.DataTranserObject.PayerReceiver;
using EMRReport.API.DataTranserObject.ScrubberError;
using EMRReport.API.DataTranserObject.Role;
using EMRReport.API.DataTranserObject.ValidatorError;
using EMRReport.API.DataTranserObject.Organization;
using EMRReport.API.DataTranserObject.Control;
using EMRReport.API.DataTranserObject.FacilityType;

namespace EMRReport.API.Configuration
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapForValidator();
            MapForScrubber();
            MapForUser();
            MapForDashboard();
            MapForOrganization();
            MapForFacility();
            MapForFacilityType();
            MapForRegulatory();
            MapForRole();
            MapForGroup();
            MapForControl();
            MapForPayerReceiver();
            MapForEncounterType();
            MapForFacilityCategory();
            MapForActivity();
        }
        private void MapForValidator()
        {
            CreateMap<GetValidatorErrorRequestDto, ValidatorErrorServiceObject>();
            CreateMap<ValidatorErrorServiceObject, GetValidatorErrorAppResponseDto>();
            CreateMap<GetExternalValidatorErrorRequestDto, GetValidatorErrorRequestDto>();
            CreateMap<ValidatorErrorServiceObject, GetValidatorErrorResponseDto>();
            CreateMap<GetValidatorCPTRequestDto, ValidatorCPTServiceObject>();
            CreateMap<GetValidatorICDRequestDto, ValidatorICDServiceObject>();
            CreateMap<ValidatorErrorServiceObject, GetValidatorErrorDetailResponseDto>();
            CreateMap<GetExternalClassRequestDto, GetValidatorClassificationRequestDto>();
        }
        private void MapForScrubber()
        {
            CreateMap<ScrubberErrorServiceObject, GetScrubberErrorResponseDto>()
                .ForMember(des => des.ErrorDate, opt => opt.MapFrom(src => src.ErrorDate.ConvertDateTimeToString()));
            CreateMap<ScrubberErrorServiceObject, GetScrubberReportResponseDto>()
               .ForMember(des => des.ExecutionDate, opt => opt.MapFrom(src => src.ErrorDate.ConvertDateTimeToString()))
               .ForMember(des => des.ErrorCPT1, opt => opt.MapFrom(src => src.ErrorCode1))
               .ForMember(des => des.ErrorCPT2, opt => opt.MapFrom(src => src.ErrorCode2))
               .ForMember(des => des.EncounterStart, opt => opt.MapFrom(src => src.Start));
        }
        private void MapForUser()
        {
            CreateMap<LoginUserRequestDto, UserServiceObject>()
            .ForMember(des => des.EncyptedPassword, opt => opt.MapFrom(src => src.Password));
            CreateMap<UserServiceObject, LoginUserResponseDto>();
                //.ForMember(des => des.UserMenuList, opt => opt.MapFrom(src => src.UserMenuList.MenuToDtoList()));
            CreateMap<ChangePasswordRequestDto, UserServiceObject>()
            .ForMember(des => des.EncyptedPassword, opt => opt.MapFrom(src => src.CurrentPassword));
            CreateMap<UserServiceObject, ChangePasswordResponseDto>();
            CreateMap<CreateUserRequestDto, UserServiceObject>()
            .ForMember(des => des.CompanyRoleID, opt => opt.MapFrom(src => src.UserRoleId))
           .ForMember(des => des.EncyptedPassword, opt => opt.MapFrom(src => src.Password.EncryptUserString(src.UserName)));
            CreateMap<UserServiceObject, CreateUserResponseDto>();
            CreateMap<UpdateUserRequestDto, UserServiceObject>()
            .ForMember(des => des.CompanyRoleID, opt => opt.MapFrom(src => src.UserRoleId))
            .ForMember(des => des.EncyptedPassword, opt => opt.MapFrom(src => src.Password.EncryptUserString(src.UserName)));
            CreateMap<UserServiceObject, UpdateUserResponseDto>();
            CreateMap<UserServiceObject, FindUserResponceDto>()
           .ForMember(des => des.Password, opt => opt.MapFrom(src => src.EncyptedPassword.DecryptUserString(src.UserName)))
           .ForMember(des => des.UserRoleId, opt => opt.MapFrom(src => src.CompanyRoleID.HasValue ? src.CompanyRoleID : 0));
            CreateMap<UserServiceObject, GetUserResponseDto>()
          .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ConvertBoolToActiveString()));
            CreateMap<UserServiceObject, BulkDownloadUserResponseDto>()
           .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ConvertBoolToActiveString()))
           .ForMember(des => des.Password, opt => opt.MapFrom(src => src.EncyptedPassword.DecryptUserString(src.UserName)));
            CreateMap<UserServiceObject, GetUserTypeDDLResponseDto>();
            CreateMap<RegisterUserRequestDto, UserServiceObject>()
            .ForMember(des => des.EncyptedPassword, opt => opt.MapFrom(src => src.Password.EncryptUserString(src.UserName)))
            .ForMember(des => des.IsSignUp, opt => opt.MapFrom(src => true))
            .ForMember(des => des.SignUpStatus, opt => opt.MapFrom(src => (int)SignUpStatusEnum.New));
            CreateMap<UserServiceObject, RegisterUserResponseDto>();
            CreateMap<UserServiceObject, GetSignUpUserResponseDto>()
            .ForMember(des => des.SignUpStatus, opt => opt.MapFrom(src => ((SignUpStatusEnum)src.SignUpStatus).ToString()));
            CreateMap<UserServiceObject, FindUserSignUpStatusResponceDto>()
            .ForMember(des => des.UserRoleId, opt => opt.MapFrom(src => src.CompanyRoleID));
            CreateMap<UpdateUserApproveRequestDto, UserServiceObject>()
            .ForMember(des => des.CompanyRoleID, opt => opt.MapFrom(src => src.UserRoleId));
            CreateMap<UpdateUserRejectRequestDto, UserServiceObject>();
            CreateMap<UpdateUserProfileRequestDto, UserServiceObject>();
            CreateMap<UserServiceObject, UpdateUserProfileResponseDto>();
            CreateMap<UserServiceObject, GetSignUpUserResponseDto>()
                .ForMember(des => des.RuleVersion, opt => opt.MapFrom(src => src.RuleVersionName))
                .ForMember(des => des.AuthorityType, opt => opt.MapFrom(src => src.AuthorityTypeName))
                .ForMember(des => des.ApplicationType, opt => opt.MapFrom(src => src.ApplicationTypeName));
            CreateMap<UserServiceObject, GetAuthorityTypeDDLResponseDto>();
            CreateMap<UserServiceObject, GetRuleVersionDDLResponseDto>();
            CreateMap<UserServiceObject, GetApplicationTypeDDLResponseDTO>();
            CreateMap<SendEmailRequestDto, UserServiceObject>();
            CreateMap<RefreshTokenRequestDto, UserServiceObject>();
        }
        private void MapForDashboard()
        {
            CreateMap<GetReportRequestDto, DashBoardServiceObject>();
            CreateMap<DashBoardServiceObject, GetEncounterWiseResponseDto>();
            CreateMap<DashBoardServiceObject, GetErrorCategoryResponseDto>();
            CreateMap<DashBoardServiceObject, GetErrorSummaryResponseDto>();
            CreateMap<DashBoardServiceObject, GetClaimCounterResponseDto>();
        }
        private void MapForOrganization()
        {
            CreateMap<CreateOrganizationRequestDto, OrganizationServiceObject>()
            .ForMember(des => des.SubscriptionStartDate, opt => opt.MapFrom(src => src.SubscriptionStartDate.ConvertStringToDateTime()))
            .ForMember(des => des.SubscriptionEndDate, opt => opt.MapFrom(src => src.SubscriptionEndDate.ConvertStringToDateTime()));
            CreateMap<OrganizationServiceObject, CreateOrganizationResponseDto>();
            CreateMap<UpdateOrganizationRequestDto, OrganizationServiceObject>()
           .ForMember(des => des.SubscriptionStartDate, opt => opt.MapFrom(src => src.SubscriptionStartDate.ConvertStringToDateTime()))
           .ForMember(des => des.SubscriptionEndDate, opt => opt.MapFrom(src => src.SubscriptionEndDate.ConvertStringToDateTime()));
            CreateMap<OrganizationServiceObject, UpdateOrganizationResponseDto>();
            CreateMap<OrganizationServiceObject, GetOrganizationResponseDto>()
          .ForMember(des => des.SubscriptionStartDate, opt => opt.MapFrom(src => src.SubscriptionStartDate.ConvertDateTimeToString()))
          .ForMember(des => des.SubscriptionEndDate, opt => opt.MapFrom(src => src.SubscriptionEndDate.ConvertDateTimeToString()))
          .ForMember(des => des.IsUnlimited, opt => opt.MapFrom(src => src.IsUnlimited.ConvertBoolYesString()))
          .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ConvertBoolToActiveString()));
        }
        private void MapForFacility()
        {
            CreateMap<CreateFacilityRequestDto, FacilityServiceObject>()
            .ForMember(des => des.SubscriptionStartDate, opt => opt.MapFrom(src => src.SubscriptionStartDate.ConvertStringToDateTime()))
            .ForMember(des => des.SubscriptionEndDate, opt => opt.MapFrom(src => src.SubscriptionEndDate.ConvertStringToDateTime()));
            CreateMap<FacilityServiceObject, CreateFacilityResponseDto>();
            CreateMap<UpdateFacilityRequestDto, FacilityServiceObject>()
           .ForMember(des => des.SubscriptionStartDate, opt => opt.MapFrom(src => src.SubscriptionStartDate.ConvertStringToDateTime()))
           .ForMember(des => des.SubscriptionEndDate, opt => opt.MapFrom(src => src.SubscriptionEndDate.ConvertStringToDateTime()));
            CreateMap<FacilityServiceObject, UpdateFacilityResponseDto>();
            CreateMap<FacilityServiceObject, FindFacilityResponseDto>();
            CreateMap<FacilityServiceObject, GetFacilityResponseDto>()
          .ForMember(des => des.SubscriptionStartDate, opt => opt.MapFrom(src => src.SubscriptionStartDate.ConvertDateTimeToString()))
          .ForMember(des => des.SubscriptionEndDate, opt => opt.MapFrom(src => src.SubscriptionEndDate.ConvertDateTimeToString()))
           .ForMember(des => des.IsDOS, opt => opt.MapFrom(src => src.IsDOS.ConvertBoolYesString()))
           .ForMember(des => des.IsAbuDhabiDOS, opt => opt.MapFrom(src => src.IsAbuDhabiDOS.ConvertBoolYesString()))
           .ForMember(des => des.IsUnlimited, opt => opt.MapFrom(src => src.IsUnlimited.ConvertBoolYesString()))
           .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ConvertBoolToActiveString()));
            CreateMap<FacilityServiceObject, GetFacilityDownloadResponseDto>()
          .ForMember(des => des.SubscriptionStartDate, opt => opt.MapFrom(src => src.SubscriptionStartDate.ConvertDateTimeToString()))
          .ForMember(des => des.SubscriptionEndDate, opt => opt.MapFrom(src => src.SubscriptionEndDate.ConvertDateTimeToString()))
          .ForMember(des => des.IsDOS, opt => opt.MapFrom(src => src.IsDOS.ConvertBoolYesString()))
          .ForMember(des => des.IsAbuDhabiDOS, opt => opt.MapFrom(src => src.IsAbuDhabiDOS.ConvertBoolYesString()))
           .ForMember(des => des.IsUnlimited, opt => opt.MapFrom(src => src.IsUnlimited.ConvertBoolYesString()));
            CreateMap<FacilityServiceObject, GetFacilityDDLResponseDto>()
                .ForMember(des => des.FacilityName, opt => opt.MapFrom(src => src.FacilityCode + " - " + src.FacilityName));
        }
        private void MapForFacilityType()
        {
            CreateMap<FacilityTypeServiceObject, GetFacilityTypeDDLResponseDto>();
        }
        private void MapForRegulatory()
        {
            CreateMap<RegulatoryServiceObject, GetRegulatoryDDLResponseDto>();
            CreateMap<CreateRegulatoryRequestDto, RegulatoryServiceObject>();
            CreateMap<RegulatoryServiceObject, CreateRegulatoryResponseDto>();
            CreateMap<UpdateRegulatoryRequestDto, RegulatoryServiceObject>();
            CreateMap<RegulatoryServiceObject, UpdateRegulatoryResponseDto>();
            CreateMap<RegulatoryServiceObject, GetRegulatoryListResponseDto>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ConvertBoolToActiveString()));
            CreateMap<RegulatoryServiceObject, FindRegulatoryResponceDto>();
        }
        private void MapForRole()
        {
            CreateMap<CompanyRoleServiceObject, GetRoleDDLResponseDto>()
                .ForMember(des => des.RoleId, opt => opt.MapFrom(src => src.CompanyRoleId))
             .ForMember(des => des.RoleName, opt => opt.MapFrom(src => src.CompanyRole));
        }
        private void MapForGroup()
        {
            CreateMap<GroupServiceObject, GetGroupDDLResponseDto>();
        }
        private void MapForControl()
        {
            CreateMap<ControlServiceObject, GetControlDDLResponseDto>();
        }
        private void MapForPayerReceiver()
        {
            CreateMap<PayerReceiverServiceObject, GetPayerReceiverDDLResponseDto>();
            CreateMap<CreatePayerReceiverRequestDto, PayerReceiverServiceObject>();
            CreateMap<PayerReceiverServiceObject, CreatePayerReceiverResponseDto>();
            CreateMap<UpdatePayerReceiverRequestDto, PayerReceiverServiceObject>();
            CreateMap<PayerReceiverServiceObject, UpdatePayerReceiverResponseDto>();
            CreateMap<PayerReceiverServiceObject, GetPayerReceiverResponseDto>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ConvertBoolToActiveString()));
            CreateMap<PayerReceiverServiceObject, BulkDonloadPayerReceiverResponseDto>()
            .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ConvertBoolToActiveString()));
            CreateMap<PayerReceiverServiceObject, FindPayerReceiverResponseDto>();

        }
        private void MapForEncounterType()
        {
            CreateMap<EncounterTypeServiceObject, GetEncounterTypeDDLResponseDto>();
        }
        private void MapForFacilityCategory()
        {
            CreateMap<FacilityCategoryServiceObject, GetFacilityCategoryDDLResponseDto>();
            CreateMap<CreateFacilityCategoryRequestDto, FacilityCategoryServiceObject>();
            CreateMap<FacilityCategoryServiceObject, CreateFacilityCategoryResponseDto>();
            CreateMap<UpdateFacilityCategoryRequestDto, FacilityCategoryServiceObject>();
            CreateMap<FacilityCategoryServiceObject, UpdateFacilityCategoryResponseDto>();
            CreateMap<FacilityCategoryServiceObject, GetFacilityCategoryListResponseDto>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.ConvertBoolToActiveString()));
            CreateMap<FacilityCategoryServiceObject, FindFacilityCategoryResponseDto>();
        }
        private void MapForActivity()
        {
            CreateMap<CreateActivityRequestDto, ActivityServiceObject>();
            CreateMap<ActivityServiceObject, CreateActivityResponseDto>();
            CreateMap<UpdateActivityRequestDto, ActivityServiceObject>();
            CreateMap<ActivityServiceObject, UpdateActivityResponseDto>();
            CreateMap<ActivityServiceObject, GetActivityResponseDto>();
            CreateMap<ActivityServiceObject, FindActivityResponseDto>();
        }
    }
}