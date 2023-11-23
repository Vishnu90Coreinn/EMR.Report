using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using EMRReport.ServiceContracts.ServiceObjects;
using EMRReport.ServiceContracts.IServices;
using EMRReport.DataContracts.IRepositories;
using EMRReport.DataContracts.Entities;
using EMRReport.Common.Models.User;
using EMRReport.Common.ProjectEnums;
using EMRReport.Common.PwdEncryption;

namespace EMRReport.Services.Services
{
    public class UserService : IUserService
    {

        private readonly byte[] _secret;
        private readonly string _staticToken;
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICompanyRoleService _companyRoleService;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly IUserHistoryService _userHistoryService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly string _emailFrom;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _origin;
        private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;
        public UserService(IConfiguration configuration, IUserRepository userRepository, IAddressRepository addressRepository, ICompanyRoleService companyRoleService, ICountryService countryService, IStateService stateService, IUserHistoryService userHistoryService, IHttpContextAccessor httpcontext, IMapper mapper)
        {
            _secret = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:SecretKey"));
            _staticToken = configuration.GetValue<string>("Jwt:StaticToken");
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _companyRoleService = companyRoleService;
            _countryService = countryService;
            _stateService = stateService;
            _httpcontext = httpcontext;
            _userHistoryService = userHistoryService;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _mapper = mapper;
            _emailFrom = configuration.GetValue<string>("AppSettings:EmailFrom");
            _smtpHost = configuration.GetValue<string>("AppSettings:SmtpHost");
            _smtpPort = Convert.ToInt32(configuration.GetValue<string>("AppSettings:SmtpPort"));
            _smtpUser = configuration.GetValue<string>("AppSettings:SmtpUser");
            _smtpPass = configuration.GetValue<string>("AppSettings:SmtpPass");
            _origin = configuration.GetValue<string>("AppSettings:Origin");
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();

        }
        public async Task<Tuple<bool, string>> ExternalLoginUserAsync(string UserName, string Password, string ControllerName, string ActionName, CancellationToken token = default)
        {
            string message = "";
            var encyptedPassword = Password.EncryptUserString(UserName);
            UserEntity userEntity = new UserEntity();
            userEntity.UserName = UserName;
            userEntity.Password = encyptedPassword;
            var user = await _userRepository.UserLoginAsync(userEntity, token);
            if (user != null)
            {
                var userPermission = await _userRepository.GetUserNameWithRoleAuthentication(UserName, ControllerName, ActionName, token);
                if (userPermission != null)
                    return Tuple.Create(true, "");
                else
                    message = "Permission Denied.";
            }
            else
                message = "User Name or Password Mismatch.";
            return Tuple.Create(false, message);
        }
        public async Task<Tuple<DateTime, UserServiceObject>> LoginUserWithTokenAsync(UserServiceObject userServiceObject, CancellationToken token)
        {
            userServiceObject.EncyptedPassword = userServiceObject.EncyptedPassword.EncryptUserString(userServiceObject.UserName);
            var userEntity = _mapper.Map<UserEntity>(userServiceObject);
            var user = await _userRepository.UserLoginAsync(userEntity, token);
            if (user == null)
                return null;
            var gettupleRefresh = generateRefreshToken(user.UserName);
            userServiceObject.RefreshToken = gettupleRefresh.Item1;
            userServiceObject.RefreshTokenExpiry = gettupleRefresh.Item2;

            var gettupleAccess = generateJwtToken(user.UserName, userServiceObject.RefreshToken);
            userServiceObject.Token = gettupleAccess.Item1;
            userServiceObject.ExpiryDate = gettupleAccess.Item2;
            return Tuple.Create(gettupleAccess.Item2, userServiceObject);
        }
        private bool ValidateRefresh(UserServiceObject userServiceObject, string refreshToken)
        {
            return userServiceObject.RefreshToken == refreshToken ? true : false;
        }
        public async Task<Tuple<DateTime, UserServiceObject>> LoginUserWithRefreshTokenAsync(UserServiceObject userServiceObject, string RefreshToken, CancellationToken token)
        {
            var check = ValidateRefresh(userServiceObject, RefreshToken);
            if (!check)
                return null;
            var gettupleAccess = generateJwtToken(userServiceObject.UserName, userServiceObject.RefreshToken);
            userServiceObject.Token = gettupleAccess.Item1;
            userServiceObject.ExpiryDate = gettupleAccess.Item2;
            return Tuple.Create(gettupleAccess.Item2, userServiceObject);
        }
        public async Task<string> LogOutUserWithSessionIDAsync(UserServiceObject userServiceObject, CancellationToken token)
        {
            UserHistoryServiceObject userHistoryServiceObject = new UserHistoryServiceObject();
            userHistoryServiceObject.LoginSessionID = userServiceObject.LoginSessionID;
            var user = await _userRepository.FindUserByNameAsync(userServiceObject.UserName, token);
            if (user != null)
                userHistoryServiceObject.LoginUserID = user.UserID;
            await _userHistoryService.SaveLogOutUserHistoryAsync(userHistoryServiceObject, token);
            return "";
        }
        public async Task<UserServiceObject> GetUserNameWithRoleAuthentication(string UserName, string ControllerName, string ActionName, bool IsRefreshToken, CancellationToken token = default)
        {
            var userEntity = IsRefreshToken ? await _userRepository.FindUserByNameAsync(UserName, token) : await _userRepository.GetUserNameWithRoleAuthentication(UserName, ControllerName, ActionName, token);
            userEntity.addressEntity = await _addressRepository.FindAddressByIdAsync(userEntity.AddressID, token);
            return _mapper.Map<UserServiceObject>(userEntity);
        }
        private Tuple<string, DateTime> generateJwtToken(string userName, string refreshToken)
        {
            DateTime expiryDate = DateTime.Now.AddHours(12);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", userName), new Claim("refreshToken", refreshToken) }),
                NotBefore = DateTime.UtcNow,
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Tuple.Create(tokenHandler.WriteToken(token), expiryDate);
        }

        //private Tuple<string, DateTime> generateStaticToken()
        //{
        //    DateTime expiryDate = DateTime.Now.AddMonths(3);
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        NotBefore = DateTime.UtcNow,
        //        Expires = expiryDate,
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return Tuple.Create(tokenHandler.WriteToken(token), expiryDate);
        //}

        private Tuple<string, DateTime> generateRefreshToken(string userName)
        {
            DateTime expiryDate = DateTime.Now.AddHours(12);
            var randomNumber = new byte[32];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            string RefreshToken = Convert.ToBase64String(randomNumber);
            var refreshToken = new RefreshToken
            {
                UserName = userName,
                TokenString = RefreshToken,
                ExpireAt = expiryDate
            };
            _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);
            return Tuple.Create(RefreshToken, expiryDate);
        }
        public async Task<UserServiceObject> ChangePasswordAsync(UserServiceObject userServiceObject, string newPassword, CancellationToken token)
        {
            userServiceObject.EncyptedPassword = userServiceObject.EncyptedPassword.EncryptUserString(userServiceObject.UserName);
            newPassword = newPassword.EncryptUserString(userServiceObject.UserName);
            var userEntity = _mapper.Map<UserEntity>(userServiceObject);
            var changedEntity = await _userRepository.ChangePasswordAsync(userEntity, newPassword, token);
            return _mapper.Map<UserServiceObject>(changedEntity);
        }
        public async Task<UserServiceObject> ForgotPasswordAsync(UserServiceObject userServiceObject, CancellationToken token)
        {

            var userData = await _userRepository.FindUserByNameAsync(userServiceObject.UserName, token);
            if (userData == null)
                return null;
            var AddressID = userData.AddressID;
            var addressData = await _addressRepository.FindAddressByIdAsync(AddressID, token);
            userData.addressEntity = addressData;

            string newPassword = Guid.NewGuid().ToString("N").ToLower()
                      .Replace("1", "").Replace("o", "").Replace("0", "")
                      .Substring(0, 10);
            string UserPassword = newPassword;
            newPassword = newPassword.EncryptUserString(userServiceObject.UserName);
            var userEntity = _mapper.Map<UserEntity>(userServiceObject);
            var changedEntity = await _userRepository.ChangePasswordAsync(userEntity, newPassword, token);
            string message;
            if (userData != null)
            {

                message = $@"<p>We have received a request to reset your fogotten password</p>
                             <p>Your UserName is  : {userServiceObject.UserName} </p>
                             <p>Resetted Password is  : {UserPassword} </p> 
                             <p>Kindly change the password of your choice after login for better security.</p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>api/User/VerifyEmail</code> api route:</p>
                             <p><code>{userData.VerificationToken}</code></p>";
            }
            Send(
                to: addressData.Email,
                subject: "Password Reset successful",
                html: $@"<h4>Dear {userServiceObject.UserName}</h4>
                         <p>Greetings!</p>
                         {message}"
            );
            return _mapper.Map<UserServiceObject>(userEntity);
        }
        public async Task<UserServiceObject> CreateUserAsync(UserServiceObject userServiceObject, CancellationToken token)
        {
            var userEntity = _mapper.Map<UserEntity>(userServiceObject);
            var createdUserEntity = await _userRepository.CreateUserAsync(userEntity, token);
            return _mapper.Map<UserServiceObject>(createdUserEntity);
        }

        public async Task<UserServiceObject> UpdateUserAsync(UserServiceObject userServiceObject, CancellationToken token)
        {
            var userData = await _userRepository.FindUserByIdAsync(userServiceObject.UserID, token);
            var AddressID = userData.AddressID;
            var addressData = await _addressRepository.FindAddressByIdAsync(AddressID, token);
            userData.UserName = userServiceObject.UserName;
            userData.Password = userServiceObject.EncyptedPassword;
            userData.FirstName = userServiceObject.FirstName;
            userData.LastName = userServiceObject.LastName;
            userData.CompanyRoleID = userServiceObject.CompanyRoleID;
            userData.UserTypeID = userServiceObject.UserTypeID;
            userData.Status = userServiceObject.Status;
            userData.addressEntity = addressData;
            userData.addressEntity.CityName = userServiceObject.City;
            userData.addressEntity.CountryID = userServiceObject.CountryId > 0 ? userServiceObject.CountryId : null;
            userData.addressEntity.Email = userServiceObject.Email;
            userData.addressEntity.Fax = userServiceObject.Fax;
            userData.addressEntity.FullAddress = userServiceObject.FullAddress;
            userData.addressEntity.Mobile = userServiceObject.Mobile;
            userData.addressEntity.Phone = userServiceObject.Phone;
            userData.addressEntity.StateID = userServiceObject.StateId > 0 ? userServiceObject.StateId : null;
            userData.addressEntity.StreetName = userServiceObject.Street;
            userData.addressEntity.Status = userServiceObject.Status;
            var updatedUserEntity = await _userRepository.UpdateUserAsync(userData, token);
            return _mapper.Map<UserServiceObject>(updatedUserEntity);
        }
        public async Task<UserServiceObject> UpdateUserProfileAsync(UserServiceObject userServiceObject, CancellationToken token)
        {
            var userData = await _userRepository.FindUserByIdAsync(userServiceObject.UserID, token);
            var AddressID = userData.AddressID;
            var addressData = await _addressRepository.FindAddressByIdAsync(AddressID, token);
            userData.FirstName = userServiceObject.FirstName;
            userData.LastName = userServiceObject.LastName;
            userData.addressEntity = addressData;
            userData.addressEntity.Email = userServiceObject.Email;
            userData.addressEntity.Phone = userServiceObject.Phone;
            var updatedUserEntity = await _userRepository.UpdateUserAsync(userData, token);
            return _mapper.Map<UserServiceObject>(updatedUserEntity);
        }
        public async Task<ICollection<UserServiceObject>> GetSignUpUserListAsync(int SignUpStatus, CancellationToken token)
        {
            var getUserEntityList = await _userRepository.GetSignUpUserListAsync(SignUpStatus, token);
            return _mapper.Map<ICollection<UserServiceObject>>(getUserEntityList);
        }
        public async Task<ICollection<UserServiceObject>> GetSignUpUserListByNameAsync(int SignUpStatus, string userName, CancellationToken token)
        {
            var getUserEntityList = await _userRepository.GetSignUpUserListByNameAsync(SignUpStatus, userName, token);
            return _mapper.Map<ICollection<UserServiceObject>>(getUserEntityList);
        }
        public async Task<ICollection<UserServiceObject>> GetSignUpUserListByEmailAsync(int SignUpStatus, string email, CancellationToken token)
        {
            var getUserEntityList = await _userRepository.GetSignUpUserListByEmailAsync(SignUpStatus, email, token);
            return _mapper.Map<ICollection<UserServiceObject>>(getUserEntityList);
        }
        public async Task<ICollection<UserServiceObject>> GetUserListAsync(CancellationToken token)
        {
            var getUserEntityList = await _userRepository.GetUserListAsync(token);
            return _mapper.Map<ICollection<UserServiceObject>>(getUserEntityList);
        }
        public async Task<ICollection<UserServiceObject>> GetUserDownloadAsync(CancellationToken token)
        {
            var getUserEntityList = await _userRepository.GetUserDownloadAsync(token);
            return _mapper.Map<ICollection<UserServiceObject>>(getUserEntityList);
        }
        public async Task<ICollection<UserServiceObject>> GetUserListByNameAsync(string userName, CancellationToken token)
        {
            var getUserEntityList = await _userRepository.GetUserListByNameAsync(userName, token);
            return _mapper.Map<ICollection<UserServiceObject>>(getUserEntityList);
        }
        public async Task<ICollection<UserServiceObject>> GetUserListByEmailAsync(string email, CancellationToken token)
        {
            var getUserEntityList = await _userRepository.GetUserListByEmailAsync(email, token);
            return _mapper.Map<ICollection<UserServiceObject>>(getUserEntityList);
        }
        public async Task<UserServiceObject> FindUserByIdAsync(int userID, CancellationToken token)
        {
            var userEntity = await _userRepository.FindUserByIdAsync(userID, token);
            userEntity.addressEntity = await _addressRepository.FindAddressByIdAsync(userEntity.AddressID, token);
            return _mapper.Map<UserServiceObject>(userEntity);
        }
        public async Task<UserServiceObject> FindUserByTokenAsync(string VerificationToken, CancellationToken token)
        {
            var userEntity = await _userRepository.FindUserByTokenAsync(VerificationToken, token);
            var AddressID = userEntity.AddressID;
            var addressData = await _addressRepository.FindAddressByIdAsync(AddressID, token);
            userEntity.EmailVerified = true;
            userEntity.addressEntity = addressData;
            userEntity.addressEntity.Status = true;
            var updatedUserEntity = await _userRepository.UpdateUserAsync(userEntity, token);
            return _mapper.Map<UserServiceObject>(updatedUserEntity);
        }
        public async Task<string> BulkSaveUserAsync(IFormFile Excelfile, CancellationToken token)
        {
            ICollection<UserServiceObject> userServiceObjectList = new List<UserServiceObject>();
            if (Excelfile.Length > 0)
            {
                string FileName = Excelfile.FileName;
                var extension = Path.GetExtension(FileName);
                if (extension.ToLower() != ".xlsx")
                    return "Excell File only Supported";
                var userNMEntityList = await _userRepository.ReadExcelUserAsync(Excelfile, token);
                var companyRoleServiceObjectList = _mapper.Map<ICollection<CompanyRoleServiceObject>>(userNMEntityList);
                companyRoleServiceObjectList = await _companyRoleService.GetCompanyRoleListFromNameListAsync(companyRoleServiceObjectList, token);
                var countryServiceObjectList = _mapper.Map<ICollection<CountryServiceObject>>(userNMEntityList);
                countryServiceObjectList = await _countryService.GetCountryListFromNameListAsync(countryServiceObjectList, token);
                var stateServiceObjectList = _mapper.Map<ICollection<StateServiceObject>>(userNMEntityList);
                stateServiceObjectList = await _stateService.GetStateListFromNameListAsync(stateServiceObjectList, token);

                await Task.Run(() =>
                {
                    for (int i = 0; i < userNMEntityList.Count(); i++)
                    {
                        var data = userNMEntityList.Skip(i).Take(1).FirstOrDefault();
                        if (data != null)
                        {
                            UserServiceObject item = new UserServiceObject();
                            item.UserID = data.UserID;
                            item.UserName = data.UserName;
                            item.Status = data.Status;
                            item.City = data.City;
                            var country = countryServiceObjectList.FirstOrDefault(x => x.Country.ToLower() == data.Country.ToLower());
                            item.CountryId = country != null ? (int?)country.CountryId : null;
                            item.Email = data.Email;
                            item.Fax = data.Fax;
                            item.FirstName = data.FirstName;
                            item.FullAddress = data.FullAddress;
                            item.LastName = data.LastName;
                            item.Mobile = data.Mobile;
                            item.EncyptedPassword = data.Password.EncryptUserString(data.UserName);
                            item.Phone = data.Phone;
                            var state = stateServiceObjectList.FirstOrDefault(x => x.State.ToLower() == data.State.ToLower());
                            item.StateId = state != null ? (int?)state.StateId : null;
                            item.Street = data.Street;
                            var companyRole = companyRoleServiceObjectList.FirstOrDefault(x => x.CompanyRole.ToLower() == data.UserRole.ToLower());
                            item.CompanyRoleID = companyRole != null ? (int?)companyRole.CompanyRoleId : null;
                            item.UserTypeID = (int)(UserLoginTypeEnum)Enum.Parse(typeof(UserLoginTypeEnum), data.UserType);
                            userServiceObjectList.Add(item);
                        }
                    }
                });
                var userEntityCreateList = _mapper.Map<List<UserEntity>>(userServiceObjectList.Where(x => x.UserID < 1));
                if (userEntityCreateList.Count > 0)
                    await _userRepository.BulkCreateUserAsync(userEntityCreateList, token);
                var userEntityUpdateList = _mapper.Map<List<UserEntity>>(userServiceObjectList.Where(x => x.UserID > 0));
                if (userEntityUpdateList.Count > 0)
                    await _userRepository.BulkUpdateUserAsync(userEntityUpdateList, token);
                return "";
            }
            else
                return "File not found";
        }
        public async Task<UserServiceObject> UpdateUserApproveAsync(UserServiceObject userServiceObject, CancellationToken token)
        {
            var userData = await _userRepository.FindUserByIdAsync(userServiceObject.UserID, token);
            var AddressID = userData.AddressID;
            var addressData = await _addressRepository.FindAddressByIdAsync(AddressID, token);
            userData.AuthorityType = userServiceObject.AuthorityType;
            userData.RuleVersion = userServiceObject.RuleVersion;
            userData.CompanyRoleID = userServiceObject.CompanyRoleID;
            userData.UserTypeID = userServiceObject.UserTypeID;
            userData.ApplicationType = userServiceObject.ApplicationType;
            userData.Status = true;
            userData.SignUpStatus = (int)SignUpStatusEnum.Approved;
            userData.addressEntity = addressData;
            userData.addressEntity.Status = true;
            var updatedUserEntity = await _userRepository.UpdateUserAsync(userData, token);
            return _mapper.Map<UserServiceObject>(updatedUserEntity);
        }
        public async Task<UserServiceObject> UpdateUserRejecteAsync(UserServiceObject userServiceObject, CancellationToken token)
        {
            var userData = await _userRepository.FindUserByIdAsync(userServiceObject.UserID, token);
            var AddressID = userData.AddressID;
            var addressData = await _addressRepository.FindAddressByIdAsync(AddressID, token);
            userData.CompanyRoleID = null;
            userData.UserTypeID = 0;
            userData.Status = false;
            userData.SignUpStatus = (int)SignUpStatusEnum.Rejected;
            userData.Reason = userServiceObject.Reason;
            userData.addressEntity = addressData;
            userData.addressEntity.Status = false;
            var updatedUserEntity = await _userRepository.UpdateUserAsync(userData, token);
            return _mapper.Map<UserServiceObject>(updatedUserEntity);
        }
        public async Task<ICollection<UserServiceObject>> GetUserTypeDDLAsync(CancellationToken token)
        {
            ICollection<UserServiceObject> userServiceObjectList = new List<UserServiceObject>();
            await Task.Run(() =>
            {
                userServiceObjectList = Enum.GetValues(typeof(UserLoginTypeEnum))
                        .Cast<UserLoginTypeEnum>()
                        .Select(x => new UserServiceObject
                        {
                            UserTypeID = (int)x,
                            UserType = x.ToString()
                        }).Where(x => x.UserTypeID > 0).ToList();
            });
            return userServiceObjectList;
        }
        public async Task<ICollection<UserServiceObject>> GetAuthorityTypeDDLAsync(CancellationToken token)
        {
            ICollection<UserServiceObject> userServiceObjectList = new List<UserServiceObject>();
            await Task.Run(() =>
            {
                userServiceObjectList = Enum.GetValues(typeof(AuthorityTypeEnum))
                        .Cast<AuthorityTypeEnum>()
                        .Select(x => new UserServiceObject
                        {
                            AuthorityType = (int)x,
                            AuthorityTypeName = x.ToString()
                        }).Where(x => x.AuthorityType > 0).ToList();
            });
            return userServiceObjectList;
        }
        public async Task<ICollection<UserServiceObject>> GetRuleVersionDDLAsync(CancellationToken token)
        {
            ICollection<UserServiceObject> userServiceObjectList = new List<UserServiceObject>();
            await Task.Run(() =>
            {
                userServiceObjectList = Enum.GetValues(typeof(RuleVersionEnum))
                        .Cast<RuleVersionEnum>()
                        .Select(x => new UserServiceObject
                        {
                            RuleVersion = (int)x,
                            RuleVersionName = x.ToString()
                        }).Where(x => x.RuleVersion > 0).ToList();
            });
            return userServiceObjectList;
        }
        public async Task<ICollection<UserServiceObject>> GetApplicationTypeDDLAsync(CancellationToken token)
        {
            ICollection<UserServiceObject> userServiceObjectList = new List<UserServiceObject>();
            await Task.Run(() =>
            {
                userServiceObjectList = Enum.GetValues(typeof(ApplicationTypeEnum))
                        .Cast<ApplicationTypeEnum>()
                        .Select(x => new UserServiceObject
                        {
                            ApplicationType = (int)x,
                            ApplicationTypeName = x.ToString()
                        }).Where(x => x.ApplicationType > 0).ToList();
            });
            return userServiceObjectList;
        }
        private async Task<ClaimsIdentity> ValidateToken(string token)
        {
            var Result = await Task.FromResult(_jwtSecurityTokenHandler.ValidateToken(token,
                  new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(_secret),
                      ValidateLifetime = true,
                      ValidateAudience = false,
                      ValidateIssuer = false,
                      ClockSkew = TimeSpan.Zero
                  }, out SecurityToken securityToken));
            return Result.Identity as ClaimsIdentity;
        }
        public async Task<int> GetUserIdLAsync(CancellationToken token)
        {
            string userName = "";
            await Task.Run(() =>
            {
                string authorization = _httpcontext.HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();
                var claimsIdentity = ValidateToken(authorization);
                var taskStatus = claimsIdentity.Status;
                if (taskStatus == TaskStatus.RanToCompletion)
                    userName = claimsIdentity.Result.Claims.FirstOrDefault(x => x.Type == "username").Value;
            });
            var user = await _userRepository.FindUserByNameAsync(userName, token);
            return user.UserID;
        }
        public async Task<Tuple<int, int>> GetUserAuthorityAndRuleVersionAsync(CancellationToken token, string userName = "")
        {
            if (string.IsNullOrEmpty(userName))
            {
                await Task.Run(() =>
                {
                    string authorization = _httpcontext.HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();
                    var claimsIdentity = ValidateToken(authorization);
                    var taskStatus = claimsIdentity.Status;
                    if (taskStatus == TaskStatus.RanToCompletion)
                        userName = claimsIdentity.Result.Claims.FirstOrDefault(x => x.Type == "username").Value;
                });
            }
            var user = await _userRepository.FindUserByNameAsync(userName, token);
            return Tuple.Create(user.AuthorityType, user.RuleVersion);
        }

        public async Task<UserServiceObject> SendEmailAsync(UserServiceObject emailServiceObject, string origin, CancellationToken token)
        {
            string message;
            var userData = await _userRepository.FindUserByIdAsync(emailServiceObject.UserID, token);
            if (userData != null)
            {

                var verifyUrl = $"{_origin}api/User/VerifyEmail?Verificationtoken={userData.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">Click to Verify Email</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>api/User/VerifyEmail</code> api route:</p>
                             <p><code>{userData.VerificationToken}</code></p>";
            }
            var AddressID = userData.AddressID;
            var addressData = await _addressRepository.FindAddressByIdAsync(AddressID, token);
            Send(
                to: addressData.Email,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
            return _mapper.Map<UserServiceObject>(emailServiceObject);
        }
        private void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _emailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpUser, _smtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public Task<bool> validateStaticTokenToContext(CancellationToken token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(_staticToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken validatedToken);
                var staticTokenCheck = (JwtSecurityToken)validatedToken;
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
