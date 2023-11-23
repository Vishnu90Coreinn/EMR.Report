using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EMRReport.API.DataTranserObject.FacilityCategory;
using EMRReport.Common.Models.User;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EMRReport.Common.TokenManager;

namespace EMRReport.API.Controllers
{
    [Route("api/[controller]")]
    public sealed class FacilityCategoryController : ApiController
    {
        private readonly ILogger<FacilityCategoryController> _logger;
        private readonly IFacilityCategoryService _facilityCategoryService;
        private readonly IValidator<CreateFacilityCategoryRequestDto> _createFacilityCategoryRequestDtoValidator;
        private readonly IValidator<UpdateFacilityCategoryRequestDto> _updateFacilityCategoryRequestDtoValidator;

        public FacilityCategoryController(ILogger<FacilityCategoryController> logger, UserContext userContext, IHttpContextAccessor httpContextAccessor, IFacilityCategoryService regulatoryService, IValidator<CreateFacilityCategoryRequestDto> createFacilityCategoryRequestDtoValidator, IValidator<UpdateFacilityCategoryRequestDto> updateFacilityCategoryRequestDtoValidator, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _facilityCategoryService = regulatoryService;
            _createFacilityCategoryRequestDtoValidator = createFacilityCategoryRequestDtoValidator;
            _updateFacilityCategoryRequestDtoValidator = updateFacilityCategoryRequestDtoValidator;
        }
        [HttpPost("Create")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(CreateFacilityCategoryResponseDto))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateFacilityCategoryRequestDto createFacilityCategoryRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _createFacilityCategoryRequestDtoValidator.ValidateAsync(createFacilityCategoryRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<FacilityCategoryServiceObject>(createFacilityCategoryRequestDto);
                var responseServiceObject = await _facilityCategoryService.CreateFacilityCategoryAsync(userServiceObject, token);
                var response = _mapper.Map<CreateFacilityCategoryResponseDto>(responseServiceObject);
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
        [ProducesResponseType(201, Type = typeof(UpdateFacilityCategoryResponseDto))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateFacilityCategoryRequestDto updateFacilityCategoryRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updateFacilityCategoryRequestDtoValidator.ValidateAsync(updateFacilityCategoryRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<FacilityCategoryServiceObject>(updateFacilityCategoryRequestDto);
                var responseServiceObject = await _facilityCategoryService.UpdateFacilityCategoryAsync(userServiceObject, token);
                var response = _mapper.Map<UpdateFacilityCategoryResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("FindFacilityCategory")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindFacilityCategoryResponseDto))]
        public async Task<IActionResult> FindFacilityCategoryAsync([FromQuery] int facilityCategoryId, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _facilityCategoryService.FindFacilityCategoryByIdAsync(facilityCategoryId, token);
                var getOrganizationResponceDto = _mapper.Map<FindFacilityCategoryResponseDto>(responseServiceObjectList);
                return Ok(getOrganizationResponceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetFacilityCategoryList")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetFacilityCategoryListResponseDto))]
        public async Task<IActionResult> GetFacilityCategoryListAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _facilityCategoryService.GetFacilityCategoryListAsync(token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetFacilityCategoryListResponseDto>>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetFacilityCategorySearch")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetFacilityCategoryListResponseDto))]
        public async Task<IActionResult> GetFacilityCategorySearchAsync([FromQuery] string facilityCategoryName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _facilityCategoryService.GetFacilityCategoryListByNameAsync(facilityCategoryName, token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetFacilityCategoryListResponseDto>>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetFacilityCategoryDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetFacilityCategoryDDLResponseDto))]
        public async Task<IActionResult> GetFacilityCategoryDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _facilityCategoryService.GetFacilityCategoryDDLAsync(token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetFacilityCategoryDDLResponseDto>>(responseServiceObjectDDL);
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