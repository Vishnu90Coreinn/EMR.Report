using EMRReport.ServiceContracts.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace EMRReport.Common.TokenManager
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly byte[] _secret;
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _secret = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:SecretKey"));
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var authorization = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (authorization != null)
                await attachUserToContext(context, userService, authorization);
            await _next(context);
        }
        private async Task attachUserToContext(HttpContext context, IUserService userService, string authorization)
        {
            try
            {

                bool IsRefreshToken = false;
                var controllerAction = context.GetRouteData().Values.ToArray();
                var actionrName = controllerAction[0].Value.ToString();
                var controllerName = controllerAction[1].Value.ToString();
                IsRefreshToken = controllerName.ToLower() == "user" && actionrName.ToLower() == "refreshtoken" ? true : false;
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(authorization, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = IsRefreshToken ? TimeSpan.FromHours(1) : TimeSpan.Zero,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userName = jwtToken.Claims.First(x => x.Type == "username").Value;
                var userAuth = await userService.GetUserNameWithRoleAuthentication(userName, controllerName, actionrName, IsRefreshToken);
                userAuth.Token = authorization;
                userAuth.RefreshToken = jwtToken.Claims.First(x => x.Type == "refreshToken").Value;
                context.Items["User"] = userAuth;
            }
            catch
            {
            }
        }
    }
}

