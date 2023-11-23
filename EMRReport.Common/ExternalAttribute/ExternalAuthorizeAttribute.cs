using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Reflection;
using System.Threading.Tasks;
using EMRReport.ServiceContracts.IServices;

namespace EMRReport.Common.ExternalAttribute
{
    public sealed class ExternalAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                string userName = "";
                string password = "";
                var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
                var Values = context.ActionArguments.Values;
                foreach (var obj in Values)
                {
                    var type = obj.GetType();
                    if (type.IsClass)
                    {
                        PropertyInfo propInfoUserName = obj.GetType().GetProperty("UserName");
                        object itemValueUserName = propInfoUserName.GetValue(obj, null);
                        userName = itemValueUserName.ToString();
                        PropertyInfo propInfoPassword = obj.GetType().GetProperty("Password");
                        object itemValuePassword = propInfoPassword.GetValue(obj, null);
                        password = itemValuePassword.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
                    var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

                    var getTuple = await userService.ExternalLoginUserAsync(userName, password, controllerName, actionName);
                    if (getTuple.Item1)
                        await next();
                    else
                        await Task.FromResult(context.Result = new UnauthorizedObjectResult(getTuple.Item2));
                }
                else
                {
                    await Task.FromResult(context.Result = new UnauthorizedObjectResult("Unauthorized"));
                }
            }
            catch (Exception ex)
            {
                await Task.FromResult(context.Result = new UnauthorizedObjectResult(ex.Message));
            }
            return;
        }
    }

}