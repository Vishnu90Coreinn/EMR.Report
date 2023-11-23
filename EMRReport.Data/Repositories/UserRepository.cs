using Dapper;
using EFCore.BulkExtensions;
using EMRReport.Common.ProjectEnums;
using EMRReport.Data;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.DataContracts.NotMappedEntities;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EMRReport.Data.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private readonly IAddressRepository _addressRepository;
        private string connectionString;
        private int userID = 0;
        public UserRepository(ScrubberDbContext dbContext, IAddressRepository addressRepository, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _addressRepository = addressRepository;
            userID = 1;
            connectionString = configuration.GetValue<string>("ConnectionStrings:ScrubberDBConnectionString");
        }
        public async Task<UserEntity> UserLoginAsync(UserEntity userEntity, CancellationToken token)
        {
            return await _dbContext.User.FirstOrDefaultAsync(x => x.UserName == userEntity.UserName && x.Password == userEntity.Password, token);
        }
        public async Task<UserEntity> UserLoginDapperAsync(UserEntity userEntity, CancellationToken token)
        {
            var sql = "select UserName,Password from UserMaster where UserName='CoreAdmin'";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var users = connection.QuerySingleAsync<UserEntity>(sql);
                return users.Result;
            }
        }
        public async Task<UserEntity> GetUserNameWithRoleAuthentication(string userName, string ControllerName, string ActionName, CancellationToken token)
        {
            return await _dbContext.User.Where(x => x.UserName == userName).Join(_dbContext.CompanyRole, u => u.CompanyRoleID, r => r.CompanyRoleId, (u, r) => new { u, r })
                  .Join(_dbContext.RoleGroup.Where(x => x.Status), p0 => p0.r.CompanyRoleId, rg => rg.RoleId, (p0, rg) => new { p0, rg })
                          .Join(_dbContext.GroupControl.Where(x => x.Status), p1 => p1.rg.GroupId, gc => gc.GroupId, (p1, gc) => new { p1, gc })
                          .Join(_dbContext.Control.Where(x => x.ControlName == ControllerName), p2 => p2.gc.ControlId, c => c.ControlId, (p2, c) => new { p2, c })
                          .Select(x => new
                          {
                              x.p2.p1.p0.u,
                              x.c.ControlName,
                              Read = x.p2.gc.Read == true ? "Ge" : "",
                              Create = x.p2.gc.Create == true ? "Create" : "",
                              Update = x.p2.gc.Update == true ? "Update" : "",
                              Update2 = x.p2.gc.Update == true ? "Change" : "",
                              File = x.p2.gc.File == true ? "File" : "",
                              File2 = x.p2.gc.File == true ? "Bulk" : "",
                              Delete = x.p2.gc.Delete == true ? "Delete" : "",
                          }).Where(x => ActionName.Contains(x.Read) || ActionName.Contains(x.Create) || ActionName.Contains(x.Update) || ActionName.Contains(x.Update2) || ActionName.Contains(x.File) || ActionName.Contains(x.File2) || ActionName.Contains(x.Delete)).Select(x => x.u).FirstOrDefaultAsync(token);
        }

        public async Task<List<Tuple<string, string>>> GetUserMenu(string userName, CancellationToken token)
        {
            return await _dbContext.User.Where(x => x.UserName == userName).Join(_dbContext.CompanyRole, u => u.CompanyRoleID, r => r.CompanyRoleId, (u, r) => new { u, r })
            .Join(_dbContext.RoleGroup.Where(x => x.Status), p0 => p0.r.CompanyRoleId, rg => rg.RoleId, (p0, rg) => new { p0, rg })
            .Join(_dbContext.GroupControl.Where(x => x.Status), p1 => p1.rg.GroupId, gc => gc.GroupId, (p1, gc) => new { p1, gc })
            .Select(x => new
            {
                x.p1.rg.groupEntity.GroupName,
                x.gc.controlEntity.ControlName,
                x.gc.Menu
            }).Where(x => x.Menu).Select(x => Tuple.Create(x.GroupName, x.ControlName)).ToListAsync(token);
        }

        public async Task<UserEntity> ChangePasswordAsync(UserEntity userEntity, string newPassword, CancellationToken token)
        {
            var userData = await _dbContext.User.FirstOrDefaultAsync(x => x.UserName == userEntity.UserName && x.Password == userEntity.Password, token);
            if (userData != null)
            {
                userData.Password = newPassword;
                _dbContext.User.Update(userData);
                await _dbContext.SaveChangesAsync(token);
                return userEntity;
            }
            else
                return null;
        }
        public async Task<UserEntity> CreateUserAsync(UserEntity userEntity, CancellationToken token)
        {
            userEntity.VerificationToken = randomTokenString();
            userEntity.Status = true;
            userEntity.UserGuid = Guid.NewGuid();
            userEntity.CreatedBy = userID;
            userEntity.CreatedDate = DateTime.Now;
            userEntity.addressEntity.Status = true;
            userEntity.addressEntity.AddressGuid = userEntity.UserGuid;
            userEntity.addressEntity.CreatedBy = userID;
            userEntity.addressEntity.CreatedDate = userEntity.CreatedDate;
            await _dbContext.User.AddAsync(userEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return userEntity;
        }
        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
        public async Task<UserEntity> UpdateUserAsync(UserEntity userEntity, CancellationToken token)
        {

            userEntity.ModifiedBy = userID;
            userEntity.ModifiedDate = DateTime.Now;
            userEntity.addressEntity.AddressGuid = userEntity.UserGuid;
            userEntity.addressEntity.ModifiedBy = userID;
            userEntity.addressEntity.ModifiedDate = userEntity.ModifiedDate;
            _dbContext.User.Update(userEntity);
            await _dbContext.SaveChangesAsync(token);
            return userEntity;
        }
        public async Task<ICollection<UserNMEntity>> GetSignUpUserListAsync(int SignUpStatus, CancellationToken token)
        {
            return await _dbContext.User.Where(x => SignUpStatus == 0 ? x.IsSignUp : x.IsSignUp && x.SignUpStatus == SignUpStatus)
                .Join(_dbContext.Address, u => u.AddressID, a => a.AddressID, (u, a) => new { u, a })
                .Select(x => new UserNMEntity
                {
                    UserID = x.u.UserID,
                    UserName = x.u.UserName,
                    FirstName = x.u.FirstName,
                    LastName = x.u.LastName,
                    UserType = ((UserLoginTypeEnum)x.u.UserTypeID).ToString(),
                    Country = x.a.countryEntity != null ? x.a.countryEntity.Country : "",
                    UserRole = x.u.companyRoleEntity != null ? x.u.companyRoleEntity.CompanyRole : "",
                    State = x.a.stateEntity != null ? x.a.stateEntity.State : "",
                    Email = x.a.Email,
                    Mobile = x.a.Mobile,
                    Phone = x.a.Phone,
                    Status = x.u.Status,
                    SignUpStatus = x.u.SignUpStatus,
                    AuthorityTypeName = ((AuthorityTypeEnum)x.u.AuthorityType).ToString(),
                    RuleVersionName = ((RuleVersionEnum)x.u.RuleVersion).ToString(),
                    ApplicationTypeName = ((ApplicationTypeEnum)x.u.ApplicationType).ToString()

                }).OrderByDescending(x => x.UserID).Take(100).ToListAsync(token);
        }
        public async Task<ICollection<UserNMEntity>> GetSignUpUserListByNameAsync(int SignUpStatus, string userName, CancellationToken token)
        {
            userName = userName.ToLower();
            return await _dbContext.User.Where(x => x.UserName.ToLower().Contains(userName))
                .Where(x => SignUpStatus == 0 ? x.IsSignUp : x.IsSignUp && x.SignUpStatus == SignUpStatus)
                .Join(_dbContext.Address, u => u.AddressID, a => a.AddressID, (u, a) => new { u, a })
                .Select(x => new UserNMEntity
                {
                    Country = x.a.countryEntity.Country,
                    Email = x.a.Email,
                    FirstName = x.u.FirstName,
                    LastName = x.u.LastName,
                    Mobile = x.a.Mobile,
                    Phone = x.a.Phone,
                    Status = x.u.Status,
                    UserID = x.u.UserID,
                    UserRole = x.u.companyRoleEntity.CompanyRole,
                    UserType = ((UserLoginTypeEnum)x.u.UserTypeID).ToString(),
                    UserName = x.u.UserName,
                    SignUpStatus = x.u.SignUpStatus
                }).ToListAsync(token);
        }
        public async Task<ICollection<UserNMEntity>> GetSignUpUserListByEmailAsync(int SignUpStatus, string email, CancellationToken token)
        {
            email = email.ToLower();
            return await _dbContext.User.Where(x => SignUpStatus == 0 ? x.IsSignUp : x.IsSignUp && x.SignUpStatus == SignUpStatus)
                .Join(_dbContext.Address.Where(x => x.Email.ToLower().Contains(email)), u => u.AddressID, a => a.AddressID, (u, a) => new { u, a })
                .Select(x => new UserNMEntity
                {
                    Country = x.a.countryEntity.Country,
                    Email = x.a.Email,
                    FirstName = x.u.FirstName,
                    LastName = x.u.LastName,
                    Mobile = x.a.Mobile,
                    Phone = x.a.Phone,
                    Status = x.u.Status,
                    UserID = x.u.UserID,
                    UserRole = x.u.companyRoleEntity.CompanyRole,
                    UserType = ((UserLoginTypeEnum)x.u.UserTypeID).ToString(),
                    UserName = x.u.UserName,
                    SignUpStatus = x.u.SignUpStatus
                }).ToListAsync(token);
        }
        public async Task<ICollection<UserNMEntity>> GetUserListAsync(CancellationToken token)
        {
            return await _dbContext.User.Join(_dbContext.Address, u => u.AddressID, a => a.AddressID, (u, a) => new { u, a })
                .Select(x => new UserNMEntity
                {
                    UserID = x.u.UserID,
                    UserName = x.u.UserName,
                    FirstName = x.u.FirstName,
                    LastName = x.u.LastName,
                    UserType = ((UserLoginTypeEnum)x.u.UserTypeID).ToString(),
                    Country = x.a.countryEntity != null ? x.a.countryEntity.Country : "",
                    UserRole = x.u.companyRoleEntity != null ? x.u.companyRoleEntity.CompanyRole : "",
                    State = x.a.stateEntity != null ? x.a.stateEntity.State : "",
                    Email = x.a.Email,
                    Mobile = x.a.Mobile,
                    Phone = x.a.Phone,
                    Status = x.u.Status

                }).OrderByDescending(x => x.UserID).Take(100).ToListAsync(token);
        }
        public async Task<ICollection<UserNMEntity>> GetUserDownloadAsync(CancellationToken token)
        {
            return await _dbContext.User.Join(_dbContext.Address, u => u.AddressID, a => a.AddressID, (u, a) => new { u, a })
                .Select(x => new UserNMEntity
                {
                    UserID = x.u.UserID,
                    UserName = x.u.UserName,
                    Password = x.u.Password,
                    FirstName = x.u.FirstName,
                    LastName = x.u.LastName,
                    UserRole = x.u.companyRoleEntity != null ? x.u.companyRoleEntity.CompanyRole : "",
                    UserType = ((UserLoginTypeEnum)x.u.UserTypeID).ToString(),
                    Country = x.a.countryEntity != null ? x.a.countryEntity.Country : "",
                    State = x.a.stateEntity != null ? x.a.stateEntity.State : "",
                    Email = x.a.Email,
                    Mobile = x.a.Mobile,
                    Phone = x.a.Phone,
                    FullAddress = x.a.FullAddress,
                    Street = x.a.StreetName,
                    City = x.a.CityName,
                    Fax = x.a.Fax,
                    Status = x.u.Status,
                }).ToListAsync(token);
        }
        public async Task<ICollection<UserNMEntity>> GetUserListByNameAsync(string userName, CancellationToken token)
        {
            userName = userName.ToLower();
            return await _dbContext.User.Where(x => x.UserName.ToLower().Contains(userName))
                .Join(_dbContext.Address, u => u.AddressID, a => a.AddressID, (u, a) => new { u, a })
                .Select(x => new UserNMEntity
                {
                    Country = x.a.countryEntity.Country,
                    Email = x.a.Email,
                    FirstName = x.u.FirstName,
                    LastName = x.u.LastName,
                    Mobile = x.a.Mobile,
                    Phone = x.a.Phone,
                    Status = x.u.Status,
                    UserID = x.u.UserID,
                    UserRole = x.u.companyRoleEntity.CompanyRole,
                    UserType = ((UserLoginTypeEnum)x.u.UserTypeID).ToString(),
                    UserName = x.u.UserName
                }).ToListAsync(token);
        }
        public async Task<ICollection<UserNMEntity>> GetUserListByEmailAsync(string email, CancellationToken token)
        {
            email = email.ToLower();
            return await _dbContext.User
                .Join(_dbContext.Address.Where(x => x.Email.ToLower().Contains(email)), u => u.AddressID, a => a.AddressID, (u, a) => new { u, a })
                .Select(x => new UserNMEntity
                {
                    Country = x.a.countryEntity.Country,
                    Email = x.a.Email,
                    FirstName = x.u.FirstName,
                    LastName = x.u.LastName,
                    Mobile = x.a.Mobile,
                    Phone = x.a.Phone,
                    Status = x.u.Status,
                    UserID = x.u.UserID,
                    UserRole = x.u.companyRoleEntity.CompanyRole,
                    UserType = ((UserLoginTypeEnum)x.u.UserTypeID).ToString(),
                    UserName = x.u.UserName
                }).ToListAsync(token);
        }
        public async Task<UserEntity> FindUserByIdAsync(int userID, CancellationToken token)
        {
            return await _dbContext.User.FirstOrDefaultAsync(x => x.UserID == userID, token);
        }
        public async Task<UserEntity> FindUserByTokenAsync(string VerificationToken, CancellationToken token)
        {
            return await _dbContext.User.FirstOrDefaultAsync(x => x.VerificationToken == VerificationToken, token);
        }
        public async Task<UserEntity> FindUserByNameAsync(string userName, CancellationToken token)
        {
            return await _dbContext.User.FirstOrDefaultAsync(x => x.UserName == userName, token);
        }
        public async Task<ICollection<UserNMEntity>> ReadExcelUserAsync(IFormFile Excelfile, CancellationToken token)
        {
            ICollection<UserNMEntity> userNMEntityList = new List<UserNMEntity>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                Excelfile.CopyTo(stream);
                stream.Position = 0;
                await Task.Run(() =>
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(stream);
                    DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    userNMEntityList = (from DataRow dr in result.Tables[0].Rows
                                        select new UserNMEntity()
                                        {
                                            UserID = Convert.ToInt32(dr["UserID"]),
                                            UserName = dr["UserName"].ToString(),
                                            Password = dr["Password"].ToString(),
                                            FirstName = dr["FirstName"].ToString(),
                                            LastName = dr["LastName"].ToString(),
                                            UserRole = dr["UserRole"].ToString(),
                                            UserType = dr["UserType"].ToString(),
                                            Country = dr["Country"].ToString(),
                                            State = dr["State"].ToString(),
                                            FullAddress = dr["FullAddress"].ToString(),
                                            Street = dr["Street"].ToString(),
                                            City = dr["City"].ToString(),
                                            Fax = dr["Fax"].ToString(),
                                            Email = dr["Email"].ToString(),
                                            Phone = dr["Phone"].ToString(),
                                            Mobile = dr["Mobile"].ToString(),
                                            Status = dr["Status"].ToString() == "Active" ? true : false
                                        }).ToList();
                });
            }
            return userNMEntityList;
        }
        public async Task<List<UserEntity>> BulkCreateUserAsync(List<UserEntity> userEntityList, CancellationToken token)
        {
            for (int i = 0; i < userEntityList.Count(); i++)
            {
                var item = userEntityList.Skip(i).Take(1).FirstOrDefault();
                item.UserGuid = Guid.NewGuid();
                item.Status = item.Status;
                item.CreatedBy = userID;
                item.CreatedDate = DateTime.Now;
                item.addressEntity.AddressGuid = item.UserGuid;
                item.addressEntity.Status = item.Status;
                item.addressEntity.CreatedBy = userID;
                item.addressEntity.CreatedDate = item.CreatedDate;

            }
            await _dbContext.AddRangeAsync(userEntityList);
            await _dbContext.SaveChangesAsync(token);
            return userEntityList;
        }
        public async Task<List<UserEntity>> BulkUpdateUserAsync(List<UserEntity> userEntityList, CancellationToken token)
        {
            List<UserEntity> updateUserEntityList = new List<UserEntity>();
            List<AddressEntity> updateAddressEntityList = new List<AddressEntity>();
            for (int i = 0; i < userEntityList.Count(); i++)
            {
                var user = userEntityList.Skip(i).Take(1).FirstOrDefault();
                var address = user.addressEntity;
                var userData = await FindUserByIdAsync(user.UserID, token);
                var addressId = userData.AddressID;
                var addressData = await _addressRepository.FindAddressByIdAsync(addressId, token);
                userData.UserName = user.UserName;
                userData.CompanyRoleID = user.CompanyRoleID;
                userData.Password = user.Password;
                userData.FirstName = user.FirstName;
                userData.LastName = user.LastName;
                userData.Status = user.Status;
                userData.UserGuid = Guid.NewGuid();
                userData.ModifiedBy = userID;
                userData.ModifiedDate = DateTime.Now;
                userData.UserTypeID = user.UserTypeID;
                addressData.Mobile = address.Mobile;
                addressData.FullAddress = address.FullAddress;
                addressData.CityName = address.CityName;
                addressData.CountryID = address.CountryID;
                addressData.Email = address.Email;
                addressData.Fax = address.Fax;
                addressData.Phone = address.Phone;
                addressData.StateID = address.StateID;
                addressData.StreetName = address.StreetName;
                addressData.AddressGuid = userData.UserGuid;
                addressData.Status = userData.Status;
                addressData.ModifiedBy = userData.ModifiedBy;
                addressData.ModifiedDate = userData.ModifiedDate;
                updateAddressEntityList.Add(addressData);
                updateUserEntityList.Add(userData);
            }
            await _dbContext.BulkUpdateAsync(updateUserEntityList);
            await _dbContext.BulkUpdateAsync(updateAddressEntityList);
            return userEntityList;
        }

        /*public async Task<UserEntity> FindUserByNameAndEmailAsync(string userName, string email, CancellationToken token)
        {
            return await _dbContext.User.FirstOrDefaultAsync(x => x.UserName == userName && x.e, token);
        }*/
    }
}