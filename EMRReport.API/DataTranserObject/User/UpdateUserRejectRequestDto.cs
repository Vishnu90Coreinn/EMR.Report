using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class UpdateUserRejectRequestDto
    {
        public int UserID { get; set; }
        public string Reason { get; set; }
    }
}
