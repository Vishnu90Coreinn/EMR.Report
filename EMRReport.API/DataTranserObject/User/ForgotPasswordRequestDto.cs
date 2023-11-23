using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class ForgotPasswordRequestDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
