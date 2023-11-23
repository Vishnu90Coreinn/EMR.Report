using EMRReport.Common.Models.User;
using System;
using System.Collections.Generic;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class LoginUserResponseDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime? expiryDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

    }
}
