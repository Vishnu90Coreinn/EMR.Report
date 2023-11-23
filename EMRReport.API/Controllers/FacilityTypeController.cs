using AutoMapper;
using EMRReport.API.DataTranserObject.FacilityType;
using EMRReport.Common.Models.User;
using EMRReport.ServiceContracts.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class FacilityTypeController : ApiController
    {
        private readonly ILogger<FacilityController> _logger;
        private readonly UserContext _userContext;
        private readonly IFacilityTypeService _facilityTypeService;
        public FacilityTypeController(ILogger<FacilityController> logger, IFacilityTypeService facilityTypeService, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _facilityTypeService = facilityTypeService;
        }
        [HttpGet("GetFacilityTypeDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetFacilityTypeDDLResponseDto))]
        public async Task<IActionResult> GetFacilityTypeDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _facilityTypeService.GetFacilityTypeDDLAsync(token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetFacilityTypeDDLResponseDto>>(responseServiceObjectDDL);
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
