using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class GetSignUpUserResponseDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRole { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string SignUpStatus { get; set; }
        public string AuthorityType { get; set; }
        public string RuleVersion { get; set; }
        public string ApplicationType { get; set; }
    }
}
