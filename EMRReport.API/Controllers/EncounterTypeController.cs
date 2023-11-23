using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EMRReport.API.DataTranserObject.EncounterType;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EMRReport.Common.Models.User;
using EMRReport.Common.TokenManager;

namespace EMRReport.API.Controllers
{
    [Route("api/[controller]")]
    public sealed class EncounterTypeController : ApiController
    {
        private readonly IValidator<CreateEncounterTypeRequestDto> _encounterTypeCreateRequestDtoValidator;
        private readonly IValidator<UpdateEncounterTypeRequestDto> _encounterTypeUpdateRequestDtoValidator;

        private readonly ILogger<EncounterTypeController> _logger;
        private readonly IEncounterTypeService _encounterTypeService;
        public EncounterTypeController(ILogger<EncounterTypeController> logger, IValidator<CreateEncounterTypeRequestDto> encounterTypeCreateRequestDtoValidator, IValidator<UpdateEncounterTypeRequestDto> encounterTypeUpdateRequestDtoValidator, IEncounterTypeService encounterTypeService, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _encounterTypeService = encounterTypeService;
            _encounterTypeCreateRequestDtoValidator = encounterTypeCreateRequestDtoValidator;
            _encounterTypeUpdateRequestDtoValidator = encounterTypeUpdateRequestDtoValidator;
        }

        [HttpPost("Create")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(CreateEncounterTypeResponseDto))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateEncounterTypeRequestDto encounterTypeCreateRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _encounterTypeCreateRequestDtoValidator.ValidateAsync(encounterTypeCreateRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var encounterTypeServiceObject = _mapper.Map<EncounterTypeServiceObject>(encounterTypeCreateRequestDto);
                var responseServiceObject = await _encounterTypeService.CreateEncounterTypeAsync(encounterTypeServiceObject, token);
                var response = _mapper.Map<CreateEncounterTypeResponseDto>(responseServiceObject);
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
        [ProducesResponseType(201, Type = typeof(UpdateEncounterTypeResponseDto))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateEncounterTypeRequestDto updateEncounterTypeRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _encounterTypeUpdateRequestDtoValidator.ValidateAsync(updateEncounterTypeRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var encounterTypeServiceObject = _mapper.Map<EncounterTypeServiceObject>(updateEncounterTypeRequestDto);
                var responseServiceObject = await _encounterTypeService.UpdateEncounterTypeAsync(encounterTypeServiceObject, token);
                var response = _mapper.Map<UpdateEncounterTypeResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("FindEncounterTypeByID")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetEncounterTypeResponseDto))]
        public async Task<IActionResult> FindEncounterTypeByIDAsync([FromQuery] int ID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _encounterTypeService.FindEncounterTypeByIDAsync(ID, token);
                var response = _mapper.Map<GetEncounterTypeResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetEncounterTypeList")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetEncounterTypeResponseDto))]
        public async Task<IActionResult> GetEncounterTypeListAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _encounterTypeService.GetEncounterTypeListAsync(token);
                var getEncounterTypeResponseDtoList = _mapper.Map<ICollection<GetEncounterTypeResponseDto>>(responseServiceObjectList);
                return Ok(getEncounterTypeResponseDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetEncounterTypeListByName")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetEncounterTypeResponseDto))]
        public async Task<IActionResult> GetEncounterTypeListByNameAsync([FromQuery] string EncounterType, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _encounterTypeService.GetEncounterTypeListByNameAsync(EncounterType, token);
                var getEncounterTypeResponceDtoList = _mapper.Map<ICollection<GetEncounterTypeResponseDto>>(responseServiceObjectList);
                return Ok(getEncounterTypeResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }

        }
    }
}
