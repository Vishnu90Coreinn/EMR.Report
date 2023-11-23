using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.NotMappedEntities
{
    public sealed class UserNMEntity
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRole { get; set; }
        public string UserType { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public bool Status { get; set; }
        public string FullAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Fax { get; set; }
        public int SignUpStatus { get; set; }
        public string AuthorityTypeName { get; set; }
        public string RuleVersionName { get; set; }
        public string ApplicationTypeName { get; set; }
    }
}
