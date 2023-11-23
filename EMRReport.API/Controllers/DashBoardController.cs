using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EMRReport.API.DataTranserObject.DashBoard;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EMRReport.Common.TokenManager;

namespace EMRReport.API.Controllers
{
    [Route("api/[controller]")]
    public class DashBoardController : ApiController
    {
        private readonly ILogger<DashBoardController> _logger;
        IDashBoardService _dashBoardService;
        public DashBoardController(ILogger<DashBoardController> logger, IDashBoardService dashBoardService, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _dashBoardService = dashBoardService;
        }
        [HttpPost("GeEncounterWise")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetEncounterWiseResponseDto))]
        public async Task<IActionResult> GetEncounterWiseAsync([FromBody] GetReportRequestDto getReportRequestDto, CancellationToken token = default)
        {
            try
            {
                var requestServiceObject = _mapper.Map<DashBoardServiceObject>(getReportRequestDto);
                var responceServiceObject = await _dashBoardService.GetEncounterWiseAsync(requestServiceObject, token);
                var getEncounterWiseResponceDtoList = _mapper.Map<List<GetEncounterWiseResponseDto>>(responceServiceObject);
                return new OkObjectResult(getEncounterWiseResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("GeErrorCategoryWise")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetErrorCategoryResponseDto))]
        public async Task<IActionResult> GetErrorCategoryWiseAsync([FromBody] GetReportRequestDto getReportRequestDto, CancellationToken token = default)
        {
            try
            {
                var requestServiceObject = _mapper.Map<DashBoardServiceObject>(getReportRequestDto);
                var responceServiceObjectList = await _dashBoardService.GeErrorCategoryWiseAsync(requestServiceObject, token);
                var getErrorCategoryResponceDtoList = _mapper.Map<List<GetErrorCategoryResponseDto>>(responceServiceObjectList);
                return new OkObjectResult(getErrorCategoryResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("GeErrorSummaryWise")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetErrorSummaryResponseDto))]
        public async Task<IActionResult> GetErrorSummaryWiseAsync([FromBody] GetReportRequestDto getReportRequestDto, CancellationToken token = default)
        {
            try
            {
                var requestServiceObject = _mapper.Map<DashBoardServiceObject>(getReportRequestDto);
                var responceServiceObjectList = await _dashBoardService.GeErrorSummaryWiseAsync(requestServiceObject, token);
                var getErrorSummaryResponceDtoList = _mapper.Map<List<GetErrorSummaryResponseDto>>(responceServiceObjectList);
                return new OkObjectResult(getErrorSummaryResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("GeClaimCounter")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetClaimCounterResponseDto))]
        public async Task<IActionResult> GetClaimCounterAsync([FromBody] GetReportRequestDto getReportRequestDto, CancellationToken token = default)
        {
            try
            {
                var requestServiceObject = _mapper.Map<DashBoardServiceObject>(getReportRequestDto);
                var responceServiceObjectList = await _dashBoardService.GeClaimCounterAsync(requestServiceObject, token);
                var getErrorSummaryResponceDtoList = _mapper.Map<List<GetClaimCounterResponseDto>>(responceServiceObjectList);
                return new OkObjectResult(getErrorSummaryResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}