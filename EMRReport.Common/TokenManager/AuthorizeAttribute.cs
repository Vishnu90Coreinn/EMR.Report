using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace EMRReport.Common.TokenManager
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class JWTAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserServiceObject)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
                return;
            }
        }

    }

}