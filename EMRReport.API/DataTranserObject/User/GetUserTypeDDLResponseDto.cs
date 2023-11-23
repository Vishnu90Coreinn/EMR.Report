using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class GetUserTypeDDLResponseDto
    {
        public int UserTypeID { get; set; }
        public string UserType { get; set; }
    }
}
