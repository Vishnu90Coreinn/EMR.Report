using AutoMapper;
using EMRReport.API.DataTranserObject.Organization;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EMRReport.Common.Models.User;
using EMRReport.Common.TokenManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.API.Controllers
{
    [Route("api/[controller]")]
    public sealed class OrganizationController : ApiController
    {
        private readonly IValidator<CreateOrganizationRequestDto> _organizationCreateRequestDtoValidator;
        private readonly IValidator<UpdateOrganizationRequestDto> _organizationUpdateRequestDtoValidator;
        private readonly ILogger<OrganizationController> _logger;
        private readonly IOrganizationService _organizationService;
        public OrganizationController(ILogger<OrganizationController> logger, IValidator<CreateOrganizationRequestDto> organizationCreateRequestDtoValidator, IValidator<UpdateOrganizationRequestDto> organizationUpdateRequestDtoValidator, IOrganizationService organizationService, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _organizationService = organizationService;
            _organizationCreateRequestDtoValidator = organizationCreateRequestDtoValidator;
            _organizationUpdateRequestDtoValidator = organizationUpdateRequestDtoValidator;
        }
        [HttpPost("Create")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(CreateOrganizationResponseDto))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrganizationRequestDto organizationCreateRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _organizationCreateRequestDtoValidator.ValidateAsync(organizationCreateRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var organizationServiceObject = _mapper.Map<OrganizationServiceObject>(organizationCreateRequestDto);
                var responseServiceObject = await _organizationService.CreateOrganizationAsync(organizationServiceObject, token);
                var response = _mapper.Map<CreateOrganizationResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("Update")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(UpdateOrganizationResponseDto))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateOrganizationRequestDto updateOrganizationRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _organizationUpdateRequestDtoValidator.ValidateAsync(updateOrganizationRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var organizationServiceObject = _mapper.Map<OrganizationServiceObject>(updateOrganizationRequestDto);
                var responseServiceObject = await _organizationService.UpdateOrganizationAsync(organizationServiceObject, token);
                var response = _mapper.Map<UpdateOrganizationResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetOrganizationByID")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetOrganizationResponseDto))]
        public async Task<IActionResult> GetOrganizationByIDAsync([FromQuery] int organizationID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _organizationService.GetOrganizationByIDAsync(organizationID, token);
                var response = _mapper.Map<GetOrganizationResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetOrganizationList")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetOrganizationResponseDto))]
        public async Task<IActionResult> GetOrganizationListAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _organizationService.GetOrganizationListAsync(token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetOrganizationResponseDto>>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetOrganizationListByName")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetOrganizationResponseDto))]
        public async Task<IActionResult> GetOrganizationListByNameAsync([FromQuery] string organizationName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _organizationService.GetOrganizationListByNameAsync(organizationName, token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetOrganizationResponseDto>>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
