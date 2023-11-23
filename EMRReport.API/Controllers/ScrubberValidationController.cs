using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Reporting;
using AutoMapper;
using ClosedXML.Excel;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EMRReport.Common.ExternalAttribute;
using EMRReport.Common.Models.User;
using EMRReport.Common.TokenManager;
using EMRReport.ServiceContracts.ServiceObjects;
using EMRReport.ServiceContracts.IServices;
using EMRReport.Common.Extensions;
using EMRReport.API.DataTranserObject.ScrubberError;

namespace EMRReport.API.Controllers
{
    [Route("api/[controller]")]
    public sealed class ScrubberValidationController : ApiController
    {
        private readonly ClaimsPrincipal _userService;
        private readonly ILogger<ScrubberValidationController> _logger;
        private readonly ILogPayLoadServiceService _logPayLoadServiceService;
        private readonly IScrubberErrorService _scrubberErrorService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IValidator<GetScrubberErrorRequestDto> _getScrubberErrorRequestDtoValidator;
        private int UserID = 0;

        public ScrubberValidationController(ILogger<ScrubberValidationController> logger, ILogPayLoadServiceService logPayLoadServiceService, IUserService userService, IScrubberErrorService scrubberErrorService, IWebHostEnvironment webHostEnvironment, IValidator<GetScrubberErrorRequestDto> getScrubberErrorRequestDtoValidator, IMapper mapper) : base(mapper)
        {
            UserID = 1;
            _userService = User;
            _logger = logger;
            _scrubberErrorService = scrubberErrorService;
            _logPayLoadServiceService = logPayLoadServiceService;
            _getScrubberErrorRequestDtoValidator = getScrubberErrorRequestDtoValidator;
            _webHostEnvironment = webHostEnvironment;
        }
        [JWTAuthorizeAttribute]
        [HttpPost("GetValidate")]
        [ProducesResponseType(201, Type = typeof(GetScrubberErrorGroupResponseDto))]
        public async Task<IActionResult> GetValidateAsync([FromForm] GetScrubberErrorRequestDto getScrubberErrorRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(getScrubberErrorRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/ScrubberValidation/GetValidate",
                FunctionName = "ScrubberValidationController/GetValidateAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                var validationResult = await _getScrubberErrorRequestDtoValidator.ValidateAsync(getScrubberErrorRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                GetScrubberErrorGroupResponseDto getScrubberErrorGroupResponseDto = new GetScrubberErrorGroupResponseDto();
                var getTuple = await _scrubberErrorService.GetScrubberErrorsFromFileCollectionAync(getScrubberErrorRequestDto.XMLfiles, token);
                if (!string.IsNullOrEmpty(getTuple.Item1))
                    return new BadRequestObjectResult(getTuple.Item1);
                getScrubberErrorGroupResponseDto.BasketGroupID = getTuple.Item2;
                var getValidatedTuple = await _scrubberErrorService.GetScrubberErrorsByBasketGroupIdAndTotalAync(getScrubberErrorGroupResponseDto.BasketGroupID, 1, token);
                getScrubberErrorGroupResponseDto.TotalRows = getValidatedTuple.Item1;
                getScrubberErrorGroupResponseDto.GetScrubberErrorResponseDtoList = _mapper.Map<List<GetScrubberErrorResponseDto>>(getValidatedTuple.Item2);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Success";
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new OkObjectResult(getScrubberErrorGroupResponseDto);
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
        [HttpGet("GetScrubberErrorsByBasketGroupId")]
        [ProducesResponseType(201, Type = typeof(GetScrubberErrorResponseDto))]
        public async Task<IActionResult> GetScrubberErrorsByBasketGroupIdAsync([FromQuery] int basketGroupID, [FromQuery] int page, CancellationToken token = default)
        {
            StringBuilder sb = new StringBuilder(10);
            string jsonSubmission = sb.Append("basketGroupID=").Append(basketGroupID).Append(",page=").Append(page).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "GET/api/ScrubberValidation/GetScrubberErrorsByBasketGroupId",
                FunctionName = "ScrubberValidationController/GetScrubberErrorsByBasketGroupIdAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                var ScrubberErrorsServiceObjectList = await _scrubberErrorService.GetScrubberErrorsByBasketGroupIdAync(basketGroupID, page, token);
                var getValidatorErrorResponceDtoList = _mapper.Map<List<GetScrubberErrorResponseDto>>(ScrubberErrorsServiceObjectList);
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
        [HttpGet("GetScrubberReportByBasketGroupId")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> GetScrubberReportByBasketGroupIdAsync([FromQuery] int basketGroupID, [FromQuery] int type, CancellationToken token = default)
        {
            StringBuilder sb = new StringBuilder(10);
            string jsonSubmission = sb.Append("basketGroupID=").Append(basketGroupID).Append(",type=").Append(type).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "GET/api/ScrubberValidation/GetScrubberReportByBasketGroupId",
                FunctionName = "ScrubberValidationController/GetScrubberReportByBasketGroupIdAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                bool IsDetail = type == 3 || type == 4 ? true : false;
                var scrubberErrorServiceObjectList = await _scrubberErrorService.GetScrubberReportByBasketGroupIdAync(basketGroupID, IsDetail, token);
                var getValidatorRepotResponceDtoList = _mapper.Map<List<GetScrubberReportResponseDto>>(scrubberErrorServiceObjectList);
                string mimetype = "";
                int extension = 1;
                var path = $"{_webHostEnvironment.WebRootPath}\\RDLCReports\\ScrubberValidationErrorReport1.rdlc";
                if (type == 1)
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\RDLCReports\\ScrubberValidationErrorReport1.rdlc";
                }
                if (type == 2)
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\RDLCReports\\ScrubberValidationErrorReport2.rdlc";
                }
                if (type == 3)
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\RDLCReports\\ScrubberValidationErrorReportNew1.rdlc";
                }
                if (type == 4)
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\RDLCReports\\ScrubberValidationErrorReportNew2.rdlc";
                }
                LocalReport lr = new LocalReport(path);
                lr.AddDataSource("ScrubberValidationErrorReportDataSet", getValidatorRepotResponceDtoList);
                var result = lr.Execute(RenderType.Excel, extension, null, mimetype);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Success";
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return File(result.MainStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ScrubberReport.xls");
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
        [ProducesResponseType(201, Type = typeof(GetScrubberErrorGroupResponseDto))]
        public async Task<IActionResult> GetExternalValidateAsync([FromForm] GetScrubberErrorExternalRequestDto getScrubberErrorExternalRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(getScrubberErrorExternalRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/ScrubberValidation/GetExternalValidate",
                FunctionName = "ScrubberValidationController/GetExternalValidateAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = UserID,
                ExceptionMessage = "",
            };
            try
            {
                if (string.IsNullOrEmpty(getScrubberErrorExternalRequestDto.XMLfiles.ValidateXMLFiles()))
                {
                    GetScrubberErrorGroupResponseDto getScrubberErrorGroupResponseDto = new GetScrubberErrorGroupResponseDto();
                    var getTuple = await _scrubberErrorService.GetScrubberErrorsFromFileCollectionAync(getScrubberErrorExternalRequestDto.XMLfiles, token);
                    if (!string.IsNullOrEmpty(getTuple.Item1))
                        return new BadRequestObjectResult(getTuple.Item1);
                    getScrubberErrorGroupResponseDto.BasketGroupID = getTuple.Item2;
                    var getValidatedTuple = await _scrubberErrorService.GetScrubberErrorsByBasketGroupIdAndTotalAync(getScrubberErrorGroupResponseDto.BasketGroupID, 1, token);
                    getScrubberErrorGroupResponseDto.TotalRows = getValidatedTuple.Item1;
                    getScrubberErrorGroupResponseDto.GetScrubberErrorResponseDtoList = _mapper.Map<List<GetScrubberErrorResponseDto>>(getValidatedTuple.Item2);
                    payLoad.RequestCompletedTime = DateTime.Now;
                    payLoad.RequestStatus = "Success";
                    await _logPayLoadServiceService.SavePayLoad(payLoad);
                    return new OkObjectResult(getScrubberErrorGroupResponseDto);
                }
                else
                    return new BadRequestObjectResult(getScrubberErrorExternalRequestDto.XMLfiles.ValidateXMLFiles());
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