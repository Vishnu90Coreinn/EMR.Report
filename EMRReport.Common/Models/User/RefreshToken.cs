using System;

namespace EMRReport.Common.Models.User
{
    public sealed class RefreshToken
    {
        public string UserName { get; set; }

        public string TokenString { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
