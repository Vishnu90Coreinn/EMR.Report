using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class FindUserSignUpStatusResponceDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserRoleId { get; set; }
        public int UserTypeId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public int SignUpStatus { get; set; }
        public int AuthorityType { get; set; }
        public int RuleVersion { get; set; }
        public int ApplicationType { get; set; }
    }
}
