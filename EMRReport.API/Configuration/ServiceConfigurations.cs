using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;
using EMRReport.API.Controllers;
using EMRReport.API.DataTranserObject.Activity;
using EMRReport.API.DataTranserObject.Facility;
using EMRReport.API.DataTranserObject.FacilityCategory;
using EMRReport.API.DataTranserObject.Organization;
using EMRReport.API.DataTranserObject.PayerReceiver;
using EMRReport.API.DataTranserObject.Regulatory;
using EMRReport.API.DataTranserObject.ScrubberError;
using EMRReport.API.DataTranserObject.User;
using EMRReport.API.DataTranserObject.ValidatorError;
using EMRReport.API.Validators.Activity;
using EMRReport.API.Validators.Facility;
using EMRReport.API.Validators.FacilityCategory;
using EMRReport.API.Validators.Organization;
using EMRReport.API.Validators.PayerReceiver;
using EMRReport.API.Validators.Regulatory;
using EMRReport.API.Validators.ScrubberError;
using EMRReport.API.Validators.User;
using EMRReport.API.Validators.ValidatorError;
using EMRReport.Data;
using EMRReport.Data.Repositories;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.Services.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
namespace EMRReport.API.Configuration
{
    public static class ServiceConfigurations
    {
        public static void AddCrossDomainPolicy(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            serviceCollection.AddCors(options => options.AddPolicy("PolicyForAngular",
                builder => builder.WithOrigins("http://3.130.142.241:8081", "http://localhost:8081", "http://corestar.dyndns-ip.com:8037", "http://localhost:4200",
                "http://corestar.dyndns-ip.com:8032", "http://corestar.dyndns-ip.com:8018").AllowAnyHeader().AllowAnyMethod()));
        }
        public static void AddJWTAuthorization(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt: Issuer"],
                    ValidAudience = configuration["Jwt: Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt: SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            serviceCollection.AddTransient<IValidatorErrorService, ValidatorErrorService>();
            serviceCollection.AddTransient<IScrubberErrorService, ScrubberErrorService>();
            serviceCollection.AddTransient<IPayerReceiverService, PayerReceiverService>();
            serviceCollection.AddTransient<IFacilityService, FacilityService>();
            serviceCollection.AddTransient<IClaimService, ClaimService>();
            serviceCollection.AddTransient<IClaimEncounterService, ClaimEncounterService>();
            serviceCollection.AddTransient<IClaimDiagnosisService, ClaimDiagnosisService>();
            serviceCollection.AddTransient<IClaimBasketService, ClaimBasketService>();
            serviceCollection.AddTransient<IClaimActivityService, ClaimActivityService>();
            serviceCollection.AddTransient<IClaimActivityObservationService, ClaimActivityObservationService>();
            serviceCollection.AddTransient<IBasketGroupService, BasketGroupService>();
            serviceCollection.AddTransient<IValidatorErrorService, ValidatorErrorService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IDashBoardService, DashBoardService>();
            serviceCollection.AddTransient<IOrganizationService, OrganizationService>();
            serviceCollection.AddTransient<IFacilityTypeService, FacilityTypeService>();
            serviceCollection.AddTransient<IRegulatoryService, RegulatoryService>();
            serviceCollection.AddTransient<ICompanyRoleService, CompanyRoleService>();
            serviceCollection.AddTransient<ICountryService, CountryService>();
            serviceCollection.AddTransient<IStateService, StateService>();
            serviceCollection.AddTransient<IGroupService, GroupService>();
            serviceCollection.AddTransient<IControlService, ControlService>();
            serviceCollection.AddTransient<IEncounterTypeService, EncounterTypeService>();
            serviceCollection.AddTransient<IFacilityCategoryService, FacilityCategoryService>();
            serviceCollection.AddTransient<IInsuranceClassificationService, InsuranceClassificationService>();
            serviceCollection.AddTransient<IValidatorICDService, ValidatorICDService>();
            serviceCollection.AddTransient<IValidatorCPTService, ValidatorCPTService>();
            serviceCollection.AddTransient<IValidatorTransactionService, ValidatorTransactionService>();
            serviceCollection.AddTransient<IActivityService, ActivityService>();
            serviceCollection.AddTransient<ISettingsService, SettingsService>();
            serviceCollection.AddTransient<IUserHistoryService, UserHistoryService>();
            serviceCollection.AddTransient<ILogPayLoadServiceService, LogPayLoadServiceService>();
        }

        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            serviceCollection.AddScoped<IValidatorErrorRepository, ValidatorErrorRepository>();
            serviceCollection.AddScoped<IScrubberErrorRepository, ScrubberErrorRepository>();
            serviceCollection.AddScoped<IPayerReceiverRepository, PayerReceiverRepository>();
            serviceCollection.AddScoped<IFacilityRepository, FacilityRepository>();
            serviceCollection.AddScoped<IClaimRepository, ClaimRepository>();
            serviceCollection.AddScoped<IClaimEncounterRepository, ClaimEncounterRepository>();
            serviceCollection.AddScoped<IClaimDiagnosisRepository, ClaimDiagnosisRepository>();
            serviceCollection.AddScoped<IClaimBasketRepository, ClaimBasketRepository>();
            serviceCollection.AddScoped<IClaimActivityRepository, ClaimActivityRepository>();
            serviceCollection.AddScoped<IClaimActivityObservationRepository, ClaimActivityObservationRepository>();
            serviceCollection.AddScoped<IBasketGroupRepository, BasketGroupRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IDashBoardRepository, DashBoardRepository>();
            serviceCollection.AddScoped<IOrganizationRepository, OrganizationRepository>();
            serviceCollection.AddScoped<IFacilityTypeRepository, FacilityTypeRepository>();
            serviceCollection.AddScoped<IRegulatoryRepository, RegulatoryRepository>();
            serviceCollection.AddScoped<IAddressRepository, AddressRepository>();
            serviceCollection.AddScoped<ICompanyRoleRepository, CompanyRoleRepository>();
            serviceCollection.AddScoped<ICountryRepository, CountryRepository>();
            serviceCollection.AddScoped<IStateRepository, StateRepository>();
            serviceCollection.AddScoped<IGroupRepository, GroupRepository>();
            serviceCollection.AddScoped<IControlRepository, ControlRepository>();
            serviceCollection.AddScoped<IEncounterTypeRepository, EncounterTypeRepository>();
            serviceCollection.AddScoped<IFacilityCategoryRepository, FacilityCategoryRepository>();
            serviceCollection.AddScoped<IInsuranceClassificationRepository, InsuranceClassificationRepository>();
            serviceCollection.AddScoped<IValidatorICDRepository, ValidatorICDRepository>();
            serviceCollection.AddScoped<IValidatorCPTRepository, ValidatorCPTRepository>();
            serviceCollection.AddScoped<IValidatorTransactionRepository, ValidatorTransactionRepository>();
            serviceCollection.AddScoped<IActivityRepository, ActivityRepository>();
            serviceCollection.AddScoped<ISettingsRepository, SettingsRepository>();
            serviceCollection.AddScoped<IUserHistoryRepository, UserHistoryRepository>();
        }

        public static void AddSqlServerDbContext(this IServiceCollection serviceCollection, string connectionString)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddDbContext<ScrubberDbContext>(options => options.UseSqlServer(connectionString));
        }

        public static void AddAutoMapperProfiles(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            var autoMapperAssemblies = new List<Assembly>
            {
                typeof(CodingValidatorController).GetTypeInfo().Assembly,
                typeof(ValidatorErrorService).GetTypeInfo().Assembly
            };
            serviceCollection.AddAutoMapper(autoMapperAssemblies);
        }
        public static void AddValidators(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            serviceCollection.AddTransient<IValidator<GetValidatorErrorRequestDto>, GetValidatorErrorRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<LoginUserRequestDto>, LoginUserRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<CreateUserRequestDto>, CreateUserRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateUserRequestDto>, UpdateUserRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<BulkSaveUserRequestDto>, BulkSaveUserRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<ChangePasswordRequestDto>, ChangePasswordRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<CreateOrganizationRequestDto>, CreateOrganizationRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateOrganizationRequestDto>, UpdateOrganizationRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<CreateFacilityRequestDto>, CreateFacilityRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateFacilityRequestDto>, UpdateFacilityRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<BulkSaveFacilityRequestDto>, BulkSaveFacilityRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<CreateRegulatoryRequestDto>, CreateRegulatoryRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateRegulatoryRequestDto>, UpdateRegulatoryRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<CreateFacilityCategoryRequestDto>, CreateFacilityCategoryRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateFacilityCategoryRequestDto>, UpdateFacilityCategoryRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<CreatePayerReceiverRequestDto>, CreatePayerReceiverRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdatePayerReceiverRequestDto>, UpdatePayerReceiverRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<BulkSavePayerReceiverRequestDto>, BulkSavePayerReceiverRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<RegisterUserRequestDto>, RegisterUserRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateUserApproveRequestDto>, UpdateUserApproveRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateUserRejectRequestDto>, UpdateUserRejectRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateUserProfileRequestDto>, UpdateUserProfileRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<SendEmailRequestDto>, SendEmailRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<ForgotPasswordRequestDto>, ForgotPasswordRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<GetScrubberErrorRequestDto>, GetScrubberErrorRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<GetValidatorTransactionRequestDto>, GetValidatorTransactionRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<CreateActivityRequestDto>, CreateActivityRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<UpdateActivityRequestDto>, UpdateActivityRequestDtoValidator>();
            serviceCollection.AddTransient<IValidator<GetValidatorClassificationRequestDto>, GetValidatorClassificationRequestDtoValidator>();
        }
    }
}
