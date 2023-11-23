using EMRReport.Common.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class UserLoginResponseDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public List<UserMenuDto> UserMenuList { get; set; }
    }
}
