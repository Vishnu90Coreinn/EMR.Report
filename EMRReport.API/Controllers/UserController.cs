using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using EMRReport.Auth.Contracts;
using EMRReport.Common.TokenManager;
using EMRReport.Data;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using EMRReport.API.DataTranserObject.User;

namespace EMRReport.API.Controllers
{
    [Route("api/[controller]")]
    public sealed class UserController : ApiController
    {
        private readonly ILogger<UserController> _logger;
        //private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IUserService _userService;
        private readonly IValidator<ChangePasswordRequestDto> _changePasswordRequestDtoValidator;
        private readonly IValidator<LoginUserRequestDto> _loginUserRequestDtooValidator;
        private readonly IValidator<CreateUserRequestDto> _createUserRequestDtoValidator;
        private readonly IValidator<UpdateUserRequestDto> _updateUserRequestDtoValidator;
        private readonly IValidator<BulkSaveUserRequestDto> _bulkSaveUserRequestDtoValidator;
        private readonly IValidator<RegisterUserRequestDto> _registerUserRequestDtoValidator;
        private readonly IValidator<UpdateUserApproveRequestDto> _updateUserApproveRequestDtoValidator;
        private readonly IValidator<UpdateUserRejectRequestDto> _updateUserRejectRequestDtoValidator;
        private readonly IValidator<UpdateUserProfileRequestDto> _updateUserProfileRequestDtoValidator;
        private readonly IValidator<SendEmailRequestDto> _sendEmailRequestDtoValidator;
        private readonly IValidator<ForgotPasswordRequestDto> _forgotPasswordRequestDtoValidator;
        private int userID = 0;
        private readonly ILogPayLoadServiceService _logPayLoadServiceService;

        public UserController(ILogger<UserController> logger, IUserService userService, ILogPayLoadServiceService logPayLoadServiceService, IValidator<LoginUserRequestDto> userLoginRequestDtoValidator, IValidator<ChangePasswordRequestDto> changePasswordRequestDtoValidator,
            IValidator<CreateUserRequestDto> createUserRequestDtoValidator, IValidator<UpdateUserRequestDto> updateUserRequestDtoValidator, IValidator<BulkSaveUserRequestDto> bulkSaveUserRequestDtoValidator,
            IValidator<RegisterUserRequestDto> registerUserRequestDtoValidator, IValidator<UpdateUserApproveRequestDto> updateUserApproveRequestDtoValidator, IValidator<UpdateUserRejectRequestDto> updateUserRejectRequestDtoValidator,
            IValidator<UpdateUserProfileRequestDto> updateUserProfileRequestDtoValidator, IValidator<SendEmailRequestDto> sendEmailRequestDtoValidator, IValidator<ForgotPasswordRequestDto> forgotPasswordRequestDtoValidator, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _userService = userService;
            userID = 1;
            _logPayLoadServiceService = logPayLoadServiceService;
            // this._jwtAuthManager = jwtAuthManager;
            _userService = userService;
            _loginUserRequestDtooValidator = userLoginRequestDtoValidator;
            _changePasswordRequestDtoValidator = changePasswordRequestDtoValidator;
            _createUserRequestDtoValidator = createUserRequestDtoValidator;
            _updateUserRequestDtoValidator = updateUserRequestDtoValidator;
            _bulkSaveUserRequestDtoValidator = bulkSaveUserRequestDtoValidator;
            _registerUserRequestDtoValidator = registerUserRequestDtoValidator;
            _updateUserApproveRequestDtoValidator = updateUserApproveRequestDtoValidator;
            _updateUserRejectRequestDtoValidator = updateUserRejectRequestDtoValidator;
            _updateUserProfileRequestDtoValidator = updateUserProfileRequestDtoValidator;
            _sendEmailRequestDtoValidator = sendEmailRequestDtoValidator;
            _forgotPasswordRequestDtoValidator = forgotPasswordRequestDtoValidator;
        }
        [HttpPost("Create")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(CreateUserResponseDto))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequestDto createUserRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _createUserRequestDtoValidator.ValidateAsync(createUserRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<UserServiceObject>(createUserRequestDto);
                var responseServiceObject = await _userService.CreateUserAsync(userServiceObject, token);
                var response = _mapper.Map<CreateUserResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("Update")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(UpdateUserResponseDto))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserRequestDto updateUserRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updateUserRequestDtoValidator.ValidateAsync(updateUserRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<UserServiceObject>(updateUserRequestDto);
                var responseServiceObject = await _userService.UpdateUserAsync(userServiceObject, token);
                var response = _mapper.Map<UpdateUserResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("Find")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindUserResponceDto))]
        public async Task<IActionResult> FindAsync([FromQuery] int userID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _userService.FindUserByIdAsync(userID, token);
                var response = _mapper.Map<FindUserResponceDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetUser")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetUserResponseDto))]
        public async Task<IActionResult> GetUserAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _userService.GetUserListAsync(token);
                var response = _mapper.Map<IEnumerable<GetUserResponseDto>>(responseServiceObjectList);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetUserByName")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetUserResponseDto))]
        public async Task<IActionResult> GetUserByNameAsync([FromQuery] string userName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _userService.GetUserListByNameAsync(userName, token);
                var response = _mapper.Map<ICollection<GetUserResponseDto>>(responseServiceObjectList);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetUserByEmail")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetUserResponseDto))]
        public async Task<IActionResult> GetUserByEmailAsync([FromQuery] string email, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _userService.GetUserListByEmailAsync(email, token);
                var response = _mapper.Map<ICollection<GetUserResponseDto>>(responseServiceObjectList);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("BulkSave")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201)]
        public async Task<IActionResult> BulkSaveAsync([FromForm] BulkSaveUserRequestDto bulkSaveUserRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _bulkSaveUserRequestDtoValidator.ValidateAsync(bulkSaveUserRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var response = await _userService.BulkSaveUserAsync(bulkSaveUserRequestDto.Excelfile, token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("BulkDownload")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201)]
        public async Task<IActionResult> BulkDownloadAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _userService.GetUserDownloadAsync(token);
                var getFacilityDownloadResponceDtoList = _mapper.Map<ICollection<BulkDownloadUserResponseDto>>(responseServiceObjectList);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("User");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "UserID";
                    worksheet.Cell(currentRow, 2).Value = "UserName";
                    worksheet.Cell(currentRow, 3).Value = "Password";
                    worksheet.Cell(currentRow, 4).Value = "FirstName";
                    worksheet.Cell(currentRow, 5).Value = "LastName";
                    worksheet.Cell(currentRow, 6).Value = "UserRole";
                    worksheet.Cell(currentRow, 7).Value = "UserType";
                    worksheet.Cell(currentRow, 8).Value = "Country";
                    worksheet.Cell(currentRow, 9).Value = "State";
                    worksheet.Cell(currentRow, 10).Value = "Email";
                    worksheet.Cell(currentRow, 11).Value = "Phone";
                    worksheet.Cell(currentRow, 12).Value = "Mobile";
                    worksheet.Cell(currentRow, 13).Value = "FullAddress";
                    worksheet.Cell(currentRow, 14).Value = "Street";
                    worksheet.Cell(currentRow, 15).Value = "City";
                    worksheet.Cell(currentRow, 16).Value = "Fax";
                    worksheet.Cell(currentRow, 17).Value = "Status";
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < getFacilityDownloadResponceDtoList.Count; i++)
                        {
                            currentRow++;
                            var facility = getFacilityDownloadResponceDtoList.Skip(i).Take(1).FirstOrDefault();
                            worksheet.Cell(currentRow, 1).Value = facility.UserID;
                            worksheet.Cell(currentRow, 2).Value = facility.UserName;
                            worksheet.Cell(currentRow, 3).Value = facility.Password;
                            worksheet.Cell(currentRow, 4).Value = facility.FirstName;
                            worksheet.Cell(currentRow, 5).Value = facility.LastName;
                            worksheet.Cell(currentRow, 6).Value = facility.UserRole;
                            worksheet.Cell(currentRow, 7).Value = facility.UserType;
                            worksheet.Cell(currentRow, 8).Value = facility.Country;
                            worksheet.Cell(currentRow, 9).Value = facility.State;
                            worksheet.Cell(currentRow, 10).Value = facility.Email;
                            worksheet.Cell(currentRow, 11).Value = facility.Phone;
                            worksheet.Cell(currentRow, 12).Value = facility.Mobile;
                            worksheet.Cell(currentRow, 13).Value = facility.FullAddress;
                            worksheet.Cell(currentRow, 14).Value = facility.Street;
                            worksheet.Cell(currentRow, 15).Value = facility.City;
                            worksheet.Cell(currentRow, 16).Value = facility.Fax;
                            worksheet.Cell(currentRow, 17).Value = facility.Status;
                        }
                    });
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "user.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }


        [HttpPost("Register")]
        [ProducesResponseType(201, Type = typeof(RegisterUserResponseDto))]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequestDto registerUserRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _registerUserRequestDtoValidator.ValidateAsync(registerUserRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<UserServiceObject>(registerUserRequestDto);
                var responseServiceObject = await _userService.CreateUserAsync(userServiceObject, token);
                var response = _mapper.Map<RegisterUserResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("SendEmail")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> SendEmailAsync([FromBody] SendEmailRequestDto sendEmailRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _sendEmailRequestDtoValidator.ValidateAsync(sendEmailRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var emailServiceObject = _mapper.Map<UserServiceObject>(sendEmailRequestDto);
                var responseServiceObject = await _userService.SendEmailAsync(emailServiceObject, "", token);
                var response = _mapper.Map<RegisterUserResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("VerifyEmail")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> VerifyEmailAsync([FromQuery] string Verificationtoken, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _userService.FindUserByTokenAsync(Verificationtoken, token);
                var response = _mapper.Map<FindUserSignUpStatusResponceDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetSignUpUserList")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetSignUpUserResponseDto))]
        public async Task<IActionResult> GetSignUpUserListAsync([FromQuery] int SignUpStatus, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _userService.GetSignUpUserListAsync(SignUpStatus, token);
                var response = _mapper.Map<ICollection<GetSignUpUserResponseDto>>(responseServiceObjectList);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetSignUpUserListByName")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetSignUpUserResponseDto))]
        public async Task<IActionResult> GetSignUpUserListByNameAsync([FromQuery] int SignUpStatus, [FromQuery] string userName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _userService.GetSignUpUserListByNameAsync(SignUpStatus, userName, token);
                var response = _mapper.Map<ICollection<GetSignUpUserResponseDto>>(responseServiceObjectList);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetSignUpUserListByEmail")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetSignUpUserResponseDto))]
        public async Task<IActionResult> GetSignUpUserListByEmailAsync([FromQuery] int SignUpStatus, [FromQuery] string email, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _userService.GetSignUpUserListByEmailAsync(SignUpStatus, email, token);
                var response = _mapper.Map<ICollection<GetSignUpUserResponseDto>>(responseServiceObjectList);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(201, Type = typeof(LoginUserResponseDto))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequestDto loginUserRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(loginUserRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/User/Login",
                FunctionName = "UserController/LoginAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = userID,
                ExceptionMessage = "",
            };
            try
            {
                var validationResult = await _loginUserRequestDtooValidator.ValidateAsync(loginUserRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                //var validateStaticToken = await _userService.validateStaticTokenToContext(token);
                //if (!validateStaticToken)
                //{
                //    payLoad.RequestCompletedTime = DateTime.Now;
                //    payLoad.RequestStatus = "Success";
                //    payLoad.RequestResponse = "InValid Access Token";
                //    await _logPayLoadServiceService.SavePayLoad(payLoad);
                //    return BadRequest(new { message = "InValid Access Token" });
                //}
                var userServiceObject = _mapper.Map<UserServiceObject>(loginUserRequestDto);
                var gettuple = await _userService.LoginUserWithTokenAsync(userServiceObject, token);
                if (gettuple == null)
                {
                    payLoad.RequestCompletedTime = DateTime.Now;
                    payLoad.RequestStatus = "Success";
                    payLoad.RequestResponse = "Wrong Password";
                //    await _logPayLoadServiceService.SavePayLoad(payLoad);
                    return BadRequest(new { message = "Username or password is incorrect" });
                }
                else
                {
                    var response = _mapper.Map<LoginUserResponseDto>(gettuple.Item2);
                    response.expiryDate = gettuple.Item1;
                    payLoad.RequestCompletedTime = DateTime.Now;
                    payLoad.RequestStatus = "Success";
                    payLoad.RequestResponse = JsonConvert.SerializeObject(response).ToString();
               //     await _logPayLoadServiceService.SavePayLoad(payLoad);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, payLoad.ApiEndPoint);
                payLoad.RequestCompletedTime = DateTime.Now;
                payLoad.RequestStatus = "Error";
                payLoad.ExceptionMessage = ex.Message;
                await _logPayLoadServiceService.SavePayLoad(payLoad);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [JWTAuthorizeAttribute]
        [HttpPost("RefreshToken")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken token = default)
        {
            string jsonSubmission = JsonConvert.SerializeObject(refreshTokenRequestDto).ToString();
            var payLoad = new LogPayLoadServiceObject
            {
                ApiEndPoint = "POST/api/User/RefreshToken",
                FunctionName = "UserController/RefreshTokenAsync",
                RequestData = jsonSubmission,
                RequestStartTime = DateTime.Now,
                CreatedBy = userID,
                ExceptionMessage = "",
            };
            try
            {
                var userServiceObject = (UserServiceObject)Request.HttpContext.Items["User"];
                var gettuple = await _userService.LoginUserWithRefreshTokenAsync(userServiceObject, refreshTokenRequestDto.refreshToken, token);
                if (gettuple == null)
                {
                    payLoad.RequestCompletedTime = DateTime.Now;
                    payLoad.RequestStatus = "Success";
                    payLoad.RequestResponse = "Refresh Token Expired";
                    return BadRequest(new { message = "Refresh Token Expired" });
                }
                else
                {
                    var response = _mapper.Map<LoginUserResponseDto>(gettuple.Item2);
                    response.expiryDate = gettuple.Item1;
                    payLoad.RequestCompletedTime = DateTime.Now;
                    payLoad.RequestStatus = "Success";
                    payLoad.RequestResponse = JsonConvert.SerializeObject(response).ToString();
                    await _logPayLoadServiceService.SavePayLoad(payLoad);
                    return Ok(response);
                }
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }
        [HttpGet("FindSignUpStatus")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindUserSignUpStatusResponceDto))]
        public async Task<IActionResult> FindSignUpStatusAsync([FromQuery] int userID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _userService.FindUserByIdAsync(userID, token);
                var response = _mapper.Map<FindUserSignUpStatusResponceDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("UpdateUserApprove")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindUserSignUpStatusResponceDto))]
        public async Task<IActionResult> UpdateUserApproveAsync([FromBody] UpdateUserApproveRequestDto approveUserRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updateUserApproveRequestDtoValidator.ValidateAsync(approveUserRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<UserServiceObject>(approveUserRequestDto);
                var createResponse = await _userService.UpdateUserApproveAsync(userServiceObject, token);
                var responseServiceObject = await _userService.FindUserByIdAsync(approveUserRequestDto.UserID, token);
                var response = _mapper.Map<FindUserSignUpStatusResponceDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("UpdateUserReject")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindUserSignUpStatusResponceDto))]
        public async Task<IActionResult> UpdateUserRejectAsync([FromBody] UpdateUserRejectRequestDto updateUserRejectRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updateUserRejectRequestDtoValidator.ValidateAsync(updateUserRejectRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<UserServiceObject>(updateUserRejectRequestDto);
                var createResponse = await _userService.UpdateUserRejecteAsync(userServiceObject, token);
                var responseServiceObject = await _userService.FindUserByIdAsync(updateUserRejectRequestDto.UserID, token);
                var response = _mapper.Map<FindUserSignUpStatusResponceDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("UpdateProfile")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(UpdateUserProfileResponseDto))]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateUserProfileRequestDto updateUserProfileRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updateUserProfileRequestDtoValidator.ValidateAsync(updateUserProfileRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<UserServiceObject>(updateUserProfileRequestDto);
                var responseServiceObject = await _userService.UpdateUserProfileAsync(userServiceObject, token);
                var response = _mapper.Map<UpdateUserProfileResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("ChangePassword")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(ChangePasswordResponseDto))]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDto changePasswordRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _changePasswordRequestDtoValidator.ValidateAsync(changePasswordRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<UserServiceObject>(changePasswordRequestDto);
                var responseServiceObject = await _userService.ChangePasswordAsync(userServiceObject, changePasswordRequestDto.NewPassword, token);
                var response = _mapper.Map<ChangePasswordResponseDto>(responseServiceObject);
                if (response == null)
                    return BadRequest(new { message = "Error on password update" });
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("ForgotPassword")]
        [ProducesResponseType(201, Type = typeof(ChangePasswordResponseDto))]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequestDto forgotPasswordRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _forgotPasswordRequestDtoValidator.ValidateAsync(forgotPasswordRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<UserServiceObject>(forgotPasswordRequestDto);
                var responseServiceObject = await _userService.ForgotPasswordAsync(userServiceObject, token);
                var response = _mapper.Map<ChangePasswordResponseDto>(responseServiceObject);
                if (response == null)
                    return BadRequest(new { message = "Error on password update" });
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("GetUserTypeDDL")]
        [JWTAuthorizeAttribute]

        [ProducesResponseType(201, Type = typeof(GetUserTypeDDLResponseDto))]
        public async Task<IActionResult> GetRoleDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _userService.GetUserTypeDDLAsync(token);
                var getuserTypeDDLResponceDtoList = _mapper.Map<ICollection<GetUserTypeDDLResponseDto>>(responseServiceObjectDDL);
                return Ok(getuserTypeDDLResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetAuthorityTypeDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetAuthorityTypeDDLResponseDto))]
        public async Task<IActionResult> GetAuthorityTypeDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _userService.GetAuthorityTypeDDLAsync(token);
                var getAuthorityTypeDDLResponseDtoList = _mapper.Map<ICollection<GetAuthorityTypeDDLResponseDto>>(responseServiceObjectDDL);
                return Ok(getAuthorityTypeDDLResponseDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetRuleVersionDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetRuleVersionDDLResponseDto))]
        public async Task<IActionResult> GetRuleVersionDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _userService.GetRuleVersionDDLAsync(token);
                var getRuleVersionDDLResponseDtoList = _mapper.Map<ICollection<GetRuleVersionDDLResponseDto>>(responseServiceObjectDDL);
                return Ok(getRuleVersionDDLResponseDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetApplicationTypeDDL")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetApplicationTypeDDLResponseDTO))]
        public async Task<IActionResult> GetApplicationTypeDDLAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _userService.GetApplicationTypeDDLAsync(token);
                var getApplicationTypeDDLResponseDtoList = _mapper.Map<ICollection<GetApplicationTypeDDLResponseDTO>>(responseServiceObjectDDL);
                return Ok(getApplicationTypeDDLResponseDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("Logout")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> LogoutAsync([FromBody] LoginUserRequestDto userRequestDto, CancellationToken token = default)
        {
            try
            {
                var userServiceObject = _mapper.Map<UserServiceObject>(userRequestDto);
                await _userService.LogOutUserWithSessionIDAsync(userServiceObject, token);
                return Ok(userRequestDto.UserName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }

        }


    }
}