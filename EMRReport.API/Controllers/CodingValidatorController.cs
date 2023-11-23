using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EMRReport.API.DataTranserObject.ValidatorError;
using EMRReport.Common.ProjectEnums;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EMRReport.Common.ExternalAttribute;
using EMRReport.Common.Models.User;
using EMRReport.Common.TokenManager;

namespace EMRReport.API.Controllers
{

    [Route("api/[controller]")]
    public sealed class CodingValidatorController : ApiController
    {
        public int UserID = 0;

        private readonly IUserService _userService;
        private readonly ILogger<CodingValidatorController> _logger;
        private readonly IValidatorErrorService _validatorErrorService;
        private readonly ILogPayLoadServiceService _logPayLoadServiceService;
        private readonly IValidator<GetValidatorErrorRequestDto> _getValidatorErrorRequestDtoValidator;
        private readonly IValidator<GetValidatorTransactionRequestDto> _getValidatorTransactionRequestDtoValidator;
        private readonly IValidator<GetValidatorClassificationRequestDto> _getValidatorClassificationRequestDtoValidator;

        public CodingValidatorController(ILogger<CodingValidatorController> logger, IUserService userService, IValidatorErrorService validatorErrorService,
        IValidator<GetValidatorErrorRequestDto> getValidatorErrorRequestDtoValidator, IValidator<GetValidatorTransactionRequestDto> getValidatorTransactionRequestDtoValidator,
         IValidator<GetValidatorClassificationRequestDto> getValidatorClassificationRequestDtoValidator, ILogPayLoadServiceService logPayLoadServiceService, IMapper mapper) : base(mapper)
        {
            UserID = 1;
            _userService = userService;
            _logger = logger;
            _validatorErrorService = validatorErrorService;
            _logPayLoadServiceService = logPayLoadServiceService;
            _getValidatorErrorRequestDtoValidator = getValidatorErrorRequestDtoValidator;
            _getValidatorTransactionRequestDtoValidator = getValidatorTransactionRequestDtoValidator;
            _getValidatorClassificationRequestDtoValidator = getValidatorClassificationRequestDtoValidator;
        }
        [JWTAuthorizeAttribute]
        [HttpPost("GetValidate")]
        [ProducesResponseType(201, Type = typeof(GetValidatorErrorResponseDto))]
        public async Task<IActionResult> GetValidatorAsync([FromBody] GetValidatorErrorRequestDto getValidatorErrorRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(getValidatorErrorRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/CodingValidator/GetValidate",
                FunctionName = "CodingValidatorController/GetValidatorAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                var validationResult = await _getValidatorErrorRequestDtoValidator.ValidateAsync(getValidatorErrorRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var validatorErrorServiceObject = _mapper.Map<ValidatorErrorServiceObject>(getValidatorErrorRequestDto);
                var validatorErrorServiceObjectList = await _validatorErrorService.GetValidatorErrorAsync(validatorErrorServiceObject, token);
                var getValidatorErrorResponceDtoList = _mapper.Map<List<GetValidatorErrorResponseDto>>(validatorErrorServiceObjectList);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Success";
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new OkObjectResult(getValidatorErrorResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, payLoad.ApiEndPoint);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Error";
                payLoad.ExceptionMessage = ex.Message;
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [JWTAuthorizeAttribute]
        [HttpPost("GetValidateApp")]
        [ProducesResponseType(201, Type = typeof(GetValidatorErrorAppResponseDto))]
        public async Task<IActionResult> GetValidateAppAsync([FromBody] GetValidatorErrorRequestDto getValidatorErrorRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(getValidatorErrorRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/CodingValidator/GetValidateApp",
                FunctionName = "CodingValidatorController/GetValidateAppAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                string Classification = "0";
                var validationResult = await _getValidatorErrorRequestDtoValidator.ValidateAsync(getValidatorErrorRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var validatorErrorServiceObject = _mapper.Map<ValidatorErrorServiceObject>(getValidatorErrorRequestDto);
                var validatorErrorServiceObjectList = await _validatorErrorService.GetValidatorAPPErrorAsync(validatorErrorServiceObject, Classification, token);
                var getValidatorErrorResponceDtoList = _mapper.Map<List<GetValidatorErrorAppResponseDto>>(validatorErrorServiceObjectList);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Success";
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new OkObjectResult(getValidatorErrorResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Error";
                payLoad.ExceptionMessage = ex.Message;
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("GetExternalValidate")]
        [ExternalAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetValidatorErrorResponseDto))]
        public async Task<IActionResult> GetExternalValidatorAsync([FromBody] GetExternalValidatorErrorRequestDto getExternalValidatorErrorRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(getExternalValidatorErrorRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/CodingValidator/GetExternalValidate",
                FunctionName = "CodingValidatorController/GetExternalValidatorAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                var getValidatorErrorRequestDto = _mapper.Map<GetValidatorErrorRequestDto>(getExternalValidatorErrorRequestDto);
                var validationResult = await _getValidatorErrorRequestDtoValidator.ValidateAsync(getValidatorErrorRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var validatorErrorServiceObject = _mapper.Map<ValidatorErrorServiceObject>(getValidatorErrorRequestDto);
                var validatorErrorServiceObjectList = await _validatorErrorService.GetValidatorErrorAsync(validatorErrorServiceObject, token);
                var getValidatorErrorResponceDtoList = _mapper.Map<List<GetValidatorErrorResponseDto>>(validatorErrorServiceObjectList);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Success";
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new OkObjectResult(getValidatorErrorResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Error";
                payLoad.ExceptionMessage = ex.Message;
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("GetExternalClassValidate")]
        [ExternalAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetValidatorErrorDetailResponseDto))]
        public async Task<IActionResult> GetExternalClassValidateAsync([FromBody] GetExternalClassRequestDto getExternalClassRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(getExternalClassRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/CodingValidator/GetExternalClassValidate",
                FunctionName = "CodingValidatorController/GetExternalClassValidateAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                var getValidatorTransactionRequestDto = _mapper.Map<GetValidatorClassificationRequestDto>(getExternalClassRequestDto);
                var validationResult = await _getValidatorClassificationRequestDtoValidator.ValidateAsync(getValidatorTransactionRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var validatorCPTServiceObjectList = _mapper.Map<List<ValidatorCPTServiceObject>>(getValidatorTransactionRequestDto.ValidatorCPTList);
                var validatorICDServiceObjectList = _mapper.Map<List<ValidatorICDServiceObject>>(getValidatorTransactionRequestDto.ValidatorICDList);
                var validatorErrorServiceObjectList = await _validatorErrorService.GetDetialValidateAsync(validatorCPTServiceObjectList, validatorICDServiceObjectList, getValidatorTransactionRequestDto.DateOfBirth, getValidatorTransactionRequestDto.Gender, getValidatorTransactionRequestDto.sequenceCPT, getValidatorTransactionRequestDto.sequenceICD, getValidatorTransactionRequestDto.CPTS, getValidatorTransactionRequestDto.ICDS, getValidatorTransactionRequestDto.Classification, token, getExternalClassRequestDto.UserName);
                var getValidatorErrorDetailResponseDtoList = _mapper.Map<List<GetValidatorErrorDetailResponseDto>>(validatorErrorServiceObjectList);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Success";
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new OkObjectResult(getValidatorErrorDetailResponseDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Error";
                payLoad.ExceptionMessage = ex.Message;
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [JWTAuthorizeAttribute]
        [HttpPost("GetDetailValidate")]
        [ProducesResponseType(201, Type = typeof(GetValidatorErrorDetailResponseDto))]
        public async Task<IActionResult> GetDetailValidateAsync([FromBody] GetValidatorTransactionRequestDto getValidatorTransactionRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(getValidatorTransactionRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/CodingValidator/GetDetailValidate",
                FunctionName = "CodingValidatorController/GetDetailValidateAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                string Classification = ((int)ClassificationEnum.All).ToString();
                var validationResult = await _getValidatorTransactionRequestDtoValidator.ValidateAsync(getValidatorTransactionRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var validatorCPTServiceObjectList = _mapper.Map<List<ValidatorCPTServiceObject>>(getValidatorTransactionRequestDto.ValidatorCPTList);
                var validatorICDServiceObjectList = _mapper.Map<List<ValidatorICDServiceObject>>(getValidatorTransactionRequestDto.ValidatorICDList);
                var validatorErrorServiceObjectList = await _validatorErrorService.GetDetialValidateAsync(validatorCPTServiceObjectList, validatorICDServiceObjectList, getValidatorTransactionRequestDto.DateOfBirth, getValidatorTransactionRequestDto.Gender, getValidatorTransactionRequestDto.sequenceCPT, getValidatorTransactionRequestDto.sequenceICD, getValidatorTransactionRequestDto.CPTS, getValidatorTransactionRequestDto.ICDS, Classification, token);
                var getValidatorErrorDetailResponseDtoList = _mapper.Map<List<GetValidatorErrorDetailResponseDto>>(validatorErrorServiceObjectList);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Success";
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new OkObjectResult(getValidatorErrorDetailResponseDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Error";
                payLoad.ExceptionMessage = ex.Message;
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [JWTAuthorizeAttribute]
        [HttpPost("GetClassificationValidate")]
        [ProducesResponseType(201, Type = typeof(GetValidatorErrorDetailResponseDto))]
        public async Task<IActionResult> GetClassificationValidateAsync([FromBody] GetValidatorClassificationRequestDto getValidatorTransactionRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(getValidatorTransactionRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/CodingValidator/GetClassificationValidate",
                FunctionName = "CodingValidatorController/GetClassificationValidateAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                var validationResult = await _getValidatorClassificationRequestDtoValidator.ValidateAsync(getValidatorTransactionRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var validatorCPTServiceObjectList = _mapper.Map<List<ValidatorCPTServiceObject>>(getValidatorTransactionRequestDto.ValidatorCPTList);
                var validatorICDServiceObjectList = _mapper.Map<List<ValidatorICDServiceObject>>(getValidatorTransactionRequestDto.ValidatorICDList);
                var validatorErrorServiceObjectList = await _validatorErrorService.GetDetialValidateAsync(validatorCPTServiceObjectList, validatorICDServiceObjectList, getValidatorTransactionRequestDto.DateOfBirth, getValidatorTransactionRequestDto.Gender, getValidatorTransactionRequestDto.sequenceCPT, getValidatorTransactionRequestDto.sequenceICD, getValidatorTransactionRequestDto.CPTS, getValidatorTransactionRequestDto.ICDS, getValidatorTransactionRequestDto.Classification, token);
                var getValidatorErrorDetailResponseDtoList = _mapper.Map<List<GetValidatorErrorDetailResponseDto>>(validatorErrorServiceObjectList);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Success";
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new OkObjectResult(getValidatorErrorDetailResponseDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Error";
                payLoad.ExceptionMessage = ex.Message;
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}