using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public class UserServiceObject
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EncyptedPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? CompanyRoleID { get; set; }
        public string UserRole { get; set; }
        public int UserTypeID { get; set; }
        public string UserType { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public int? StateId { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string FullAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Fax { get; set; }
        public bool Status { get; set; }
        public string Active { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public List<Tuple<string, string>> UserMenuList { get; set; }
        public bool IsSignUp { get; set; }
        public int SignUpStatus { get; set; }
        public string SignUpStatusName { get; set; }
        public string Reason { get; set; }
        public int AuthorityType { get; set; }
        public int RuleVersion { get; set; }
        public int ApplicationType { get; set; }
        public string AuthorityTypeName { get; set; }
        public string RuleVersionName { get; set; }
        public string ApplicationTypeName { get; set; }
        public string VerificationToken { get; set; }
        public string To { get; set; }
        public bool EmailVerified { get; set; }
        public string LoginSessionID { get; set; }
        public string LoginIPAddress { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }
    }
}
