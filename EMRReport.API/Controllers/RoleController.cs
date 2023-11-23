using AutoMapper;
using EMRReport.API.DataTranserObject.Role;
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
    public sealed class RoleController : ApiController
    {
        private readonly ILogger<RoleController> _logger;
        private readonly ICompanyRoleService _companyRoleService;
        public RoleController(ILogger<RoleController> logger, UserContext userContext, IHttpContextAccessor httpContextAccessor, ICompanyRoleService companyRoleService, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _companyRoleService = companyRoleService;
        }
        [HttpGet("GetRoleDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetRoleDDLResponseDto))]
        public async Task<IActionResult> GetRoleDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _companyRoleService.GetCompanyRoleDDLAsync(token);
                var getCompanyRoleDDLResponceDtoList = _mapper.Map<ICollection<GetRoleDDLResponseDto>>(responseServiceObjectDDL);
                return Ok(getCompanyRoleDDLResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
