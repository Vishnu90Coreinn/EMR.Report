using AutoMapper;
using EMRReport.API.DataTranserObject.Control;
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
    public class ControlController : ApiController
    {
        private readonly ILogger<ControlController> _logger;
        private readonly UserContext _userContext;
        private readonly IControlService _controlService;
        public ControlController(ILogger<ControlController> logger, UserContext userContext, IHttpContextAccessor httpContextAccessor, IControlService controlService, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _controlService = controlService;
        }
        [HttpGet("GetControlDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetControlDDLResponseDto))]
        public async Task<IActionResult> GetGroupDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _controlService.GetControlDDLAsync(token);
                var getGroupResponceDtoList = _mapper.Map<ICollection<GetControlDDLResponseDto>>(responseServiceObjectDDL);
                return Ok(getGroupResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
