using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using EMRReport.API.DataTranserObject.Activity;
using EMRReport.Common.TokenManager;

namespace EMRReport.API.Controllers
{  // [Authorize]
    [Route("api/[controller]")]
    public sealed class ActivityController : ApiController
    {
        private readonly IValidator<CreateActivityRequestDto> _createActivityRequestDtoValidator;
        private readonly IValidator<UpdateActivityRequestDto> _updateActivityRequestDtoValidator;
        private readonly IActivityService _activityService;
        private readonly ILogger<ActivityController> _logger;
        public ActivityController(ILogger<ActivityController> logger, IMapper mapper, IValidator<CreateActivityRequestDto> createActivityRequestDtoValidator, IValidator<UpdateActivityRequestDto> updateActivityRequestDtoValidator, IActivityService activityService) : base(mapper)
        {
            _logger = logger;
            _activityService = activityService;
            _createActivityRequestDtoValidator = createActivityRequestDtoValidator;
            _updateActivityRequestDtoValidator = updateActivityRequestDtoValidator;
        }
        [HttpPost("Create")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(CreateActivityResponseDto))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateActivityRequestDto createRegulatoryRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _createActivityRequestDtoValidator.ValidateAsync(createRegulatoryRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<ActivityServiceObject>(createRegulatoryRequestDto);
                var responseServiceObject = await _activityService.CreateActivityAsync(userServiceObject, token);
                var response = _mapper.Map<CreateActivityResponseDto>(responseServiceObject);
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
        [ProducesResponseType(201, Type = typeof(UpdateActivityResponseDto))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateActivityRequestDto updatePayerReceiverRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updateActivityRequestDtoValidator.ValidateAsync(updatePayerReceiverRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<ActivityServiceObject>(updatePayerReceiverRequestDto);
                var responseServiceObject = await _activityService.UpdateActivityAsync(userServiceObject, token);
                var response = _mapper.Map<UpdateActivityResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("FindActivityByID")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindActivityResponseDto))]
        public async Task<IActionResult> FindActivityByIDAsync([FromQuery] int activityID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _activityService.FindActivityAsync(activityID, token);
                var response = _mapper.Map<FindActivityResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("GetActivityList")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetActivityResponseDto))]
        public async Task<IActionResult> GetActivityListAsync([FromQuery] string payerReceiverName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _activityService.GetActivityListAsync(token);
                var getActivityResponceDtoList = _mapper.Map<ICollection<GetActivityResponseDto>>(responseServiceObjectDDL);
                return Ok(getActivityResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
