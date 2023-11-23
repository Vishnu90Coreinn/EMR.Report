using AutoMapper;
using EMRReport.API.DataTranserObject.Group;
using EMRReport.ServiceContracts.IServices;
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
    public class GroupController : ApiController
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IGroupService _groupService;
        public GroupController(ILogger<GroupController> logger, IGroupService groupService, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _groupService = groupService;
        }
        [HttpGet("GetGroupDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetGroupDDLResponseDto))]
        public async Task<IActionResult> GetGroupDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _groupService.GetGroupDDLAsync(token);
                var getGroupResponceDtoList = _mapper.Map<ICollection<GetGroupDDLResponseDto>>(responseServiceObjectDDL);
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
