using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.NotMappedEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IUserRepository
    {
        Task<UserEntity> CreateUserAsync(UserEntity userEntity, CancellationToken token);
        Task<UserEntity> UpdateUserAsync(UserEntity userEntity, CancellationToken token);
        Task<ICollection<UserNMEntity>> GetSignUpUserListAsync(int SignUpStatus, CancellationToken token);
        Task<ICollection<UserNMEntity>> GetSignUpUserListByNameAsync(int SignUpStatus, string userName, CancellationToken token);
        Task<ICollection<UserNMEntity>> GetSignUpUserListByEmailAsync(int SignUpStatus, string email, CancellationToken token);
        Task<ICollection<UserNMEntity>> GetUserListAsync(CancellationToken token);
        Task<ICollection<UserNMEntity>> GetUserListByNameAsync(string userName, CancellationToken token);
        Task<ICollection<UserNMEntity>> GetUserListByEmailAsync(string email, CancellationToken token);
        Task<UserEntity> FindUserByIdAsync(int userID, CancellationToken token);
        Task<UserEntity> FindUserByTokenAsync(string VerificationToken, CancellationToken token);
        Task<UserEntity> FindUserByNameAsync(string userName, CancellationToken token);
        Task<UserEntity> UserLoginAsync(UserEntity userEntity, CancellationToken token);
        Task<UserEntity> UserLoginDapperAsync(UserEntity userEntity, CancellationToken token);
        
        Task<UserEntity> GetUserNameWithRoleAuthentication(string userName, string ControllerName, string ActionName, CancellationToken token);
        Task<List<Tuple<string, string>>> GetUserMenu(string userName, CancellationToken token);
        Task<UserEntity> ChangePasswordAsync(UserEntity userEntity, string newPassword, CancellationToken token);
        Task<ICollection<UserNMEntity>> ReadExcelUserAsync(IFormFile Excelfile, CancellationToken token);
        Task<List<UserEntity>> BulkCreateUserAsync(List<UserEntity> userEntityList, CancellationToken token);
        Task<List<UserEntity>> BulkUpdateUserAsync(List<UserEntity> userEntityList, CancellationToken token);
        Task<ICollection<UserNMEntity>> GetUserDownloadAsync(CancellationToken token);
    }
}