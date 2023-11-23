using System;

namespace EMRReport.Auth.Models
{
    public sealed class JwtAuthResult
    {
        public string AccessToken { get; set; }

        public RefreshToken RefreshToken { get; set; }

        public DateTime AccessTokenExpiry { get; set; }

    }
}
