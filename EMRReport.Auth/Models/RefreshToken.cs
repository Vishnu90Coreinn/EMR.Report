using System;

namespace EMRReport.Auth.Models
{
    public sealed class RefreshToken
    {
        public string UserName { get; set; }

        public string TokenString { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
