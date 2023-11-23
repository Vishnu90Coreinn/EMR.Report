using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class UpdateUserApproveRequestDto
    {
        public int UserID { get; set; }
        public int UserRoleId { get; set; }
        public int UserTypeId { get; set; }
        public int AuthorityType { get; set; }
        public int RuleVersion { get; set; }
        public int ApplicationType { get; set; }
    }
}
