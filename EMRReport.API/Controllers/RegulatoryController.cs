using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EMRReport.API.DataTranserObject.Regulatory;
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
    public sealed class RegulatoryController : ApiController
    {
        private readonly ILogger<RegulatoryController> _logger;
        private readonly UserContext _userContext;
        private readonly IRegulatoryService _regulatoryService;
        private readonly IValidator<CreateRegulatoryRequestDto> _createRegulatoryRequestDtoValidator;
        private readonly IValidator<UpdateRegulatoryRequestDto> _updateRegulatoryRequestDtoValidator;

        public RegulatoryController(ILogger<RegulatoryController> logger, IRegulatoryService regulatoryService, IValidator<CreateRegulatoryRequestDto> createRegulatoryRequestDtoValidator, IValidator<UpdateRegulatoryRequestDto> updateRegulatoryRequestDtoValidator, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _regulatoryService = regulatoryService;
            _createRegulatoryRequestDtoValidator = createRegulatoryRequestDtoValidator;
            _updateRegulatoryRequestDtoValidator = updateRegulatoryRequestDtoValidator;
        }

        [HttpPost("Create")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(CreateRegulatoryResponseDto))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRegulatoryRequestDto createRegulatoryRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _createRegulatoryRequestDtoValidator.ValidateAsync(createRegulatoryRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<RegulatoryServiceObject>(createRegulatoryRequestDto);
                var responseServiceObject = await _regulatoryService.CreateRegulatoryAsync(userServiceObject, token);
                var response = _mapper.Map<CreateRegulatoryResponseDto>(responseServiceObject);
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
        [ProducesResponseType(201, Type = typeof(UpdateRegulatoryResponseDto))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateRegulatoryRequestDto updateRegulatoryRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updateRegulatoryRequestDtoValidator.ValidateAsync(updateRegulatoryRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<RegulatoryServiceObject>(updateRegulatoryRequestDto);
                var responseServiceObject = await _regulatoryService.UpdateRegulatoryAsync(userServiceObject, token);
                var response = _mapper.Map<UpdateRegulatoryResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("FindRegulatoryByID")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindRegulatoryResponceDto))]
        public async Task<IActionResult> FindRegulatoryByIDAsync([FromQuery] int facilityCategoryId, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _regulatoryService.GetRegulatoryByIdAsync(facilityCategoryId, token);
                var getOrganizationResponceDtoList = _mapper.Map<FindRegulatoryResponceDto>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetRegulatoryList")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetRegulatoryListResponseDto))]
        public async Task<IActionResult> GetRegulatoryListAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _regulatoryService.GetRegulatoryListAsync(token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetRegulatoryListResponseDto>>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetRegulatorySearch")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetRegulatoryListResponseDto))]
        public async Task<IActionResult> GetRegulatorySearchAsync([FromQuery] string RegulatoryName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _regulatoryService.GetRegulatoryListByNameAsync(RegulatoryName, token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetRegulatoryListResponseDto>>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetRegulatoryDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetRegulatoryDDLResponseDto))]
        public async Task<IActionResult> GetRegulatoryDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _regulatoryService.GetRegulatoryDDLAsync(token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetRegulatoryDDLResponseDto>>(responseServiceObjectDDL);
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