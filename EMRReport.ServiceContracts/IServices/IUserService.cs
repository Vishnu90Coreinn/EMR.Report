using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IUserService
    {
        Task<UserServiceObject> CreateUserAsync(UserServiceObject userServiceObject, CancellationToken token);
        Task<UserServiceObject> UpdateUserAsync(UserServiceObject userServiceObject, CancellationToken token);
        Task<ICollection<UserServiceObject>> GetSignUpUserListAsync(int SignUpStatus, CancellationToken token);
        Task<ICollection<UserServiceObject>> GetSignUpUserListByNameAsync(int SignUpStatus, string userName, CancellationToken token);
        Task<ICollection<UserServiceObject>> GetSignUpUserListByEmailAsync(int SignUpStatus, string email, CancellationToken token);
        Task<ICollection<UserServiceObject>> GetUserListAsync(CancellationToken token);
        Task<ICollection<UserServiceObject>> GetUserListByNameAsync(string userName, CancellationToken token);
        Task<ICollection<UserServiceObject>> GetUserListByEmailAsync(string email, CancellationToken token);
        Task<UserServiceObject> FindUserByIdAsync(int userID, CancellationToken token);
        Task<UserServiceObject> FindUserByTokenAsync(string VerificationToken, CancellationToken token);
        Task<Tuple<DateTime, UserServiceObject>> LoginUserWithTokenAsync(UserServiceObject userServiceObject, CancellationToken token);
        Task<Tuple<DateTime, UserServiceObject>> LoginUserWithRefreshTokenAsync(UserServiceObject userServiceObject, string RefreshToken, CancellationToken token);
        Task<Tuple<bool, string>> ExternalLoginUserAsync(string UserName, string Password, string controllerName, string actionName, CancellationToken token = default);
        Task<UserServiceObject> GetUserNameWithRoleAuthentication(string userName, string ControllerName, string ActionName, bool IsRefreshToken, CancellationToken token = default);
        Task<UserServiceObject> ChangePasswordAsync(UserServiceObject userServiceObject, string newPassword, CancellationToken token);
        Task<string> BulkSaveUserAsync(IFormFile Excelfile, CancellationToken token);
        Task<ICollection<UserServiceObject>> GetUserDownloadAsync(CancellationToken token);
        Task<ICollection<UserServiceObject>> GetUserTypeDDLAsync(CancellationToken token);
        Task<UserServiceObject> UpdateUserApproveAsync(UserServiceObject userServiceObject, CancellationToken token);
        Task<UserServiceObject> UpdateUserRejecteAsync(UserServiceObject userServiceObject, CancellationToken token);
        Task<UserServiceObject> UpdateUserProfileAsync(UserServiceObject userServiceObject, CancellationToken token);
        Task<ICollection<UserServiceObject>> GetAuthorityTypeDDLAsync(CancellationToken token);
        Task<ICollection<UserServiceObject>> GetRuleVersionDDLAsync(CancellationToken token);
        Task<ICollection<UserServiceObject>> GetApplicationTypeDDLAsync(CancellationToken token);
        Task<int> GetUserIdLAsync(CancellationToken token);
        Task<Tuple<int, int>> GetUserAuthorityAndRuleVersionAsync(CancellationToken token, string userName = "");
        Task<UserServiceObject> SendEmailAsync(UserServiceObject emailServiceObject, string origin, CancellationToken token);
        Task<UserServiceObject> ForgotPasswordAsync(UserServiceObject userServiceObject, CancellationToken token);
        Task<string> LogOutUserWithSessionIDAsync(UserServiceObject userServiceObject, CancellationToken token);
        Task<bool> validateStaticTokenToContext(CancellationToken token);
    }
}
