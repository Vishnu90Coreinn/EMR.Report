using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class LoginUserRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LoginSessionID { get; set; }
        public string LoginIPAddress { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }
    }
}
