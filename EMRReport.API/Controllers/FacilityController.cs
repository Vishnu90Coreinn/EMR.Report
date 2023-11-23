using AutoMapper;
using ClosedXML.Excel;
using EMRReport.API.DataTranserObject.Facility;
using EMRReport.Common.TokenManager;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.API.Controllers
{
    [Route("api/[controller]")]
    public class FacilityController : ApiController
    {
        private readonly IValidator<CreateFacilityRequestDto> _createFacilityRequestDtoValidator;
        private readonly IValidator<UpdateFacilityRequestDto> _updateFacilityRequestDtoValidator;
        private readonly IValidator<BulkSaveFacilityRequestDto> _bulkSaveFacilityRequestDtoValidator;
        private readonly ILogger<FacilityController> _logger;
        private readonly IFacilityService _facilityService;
        public FacilityController(ILogger<FacilityController> logger, IValidator<CreateFacilityRequestDto> createFacilityRequestDtoValidator, IValidator<UpdateFacilityRequestDto> updateFacilityRequestDtoValidator, IValidator<BulkSaveFacilityRequestDto> bulkSaveFacilityRequestDtoValidator, IFacilityService facilityService, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _facilityService = facilityService;
            _createFacilityRequestDtoValidator = createFacilityRequestDtoValidator;
            _updateFacilityRequestDtoValidator = updateFacilityRequestDtoValidator;
            _bulkSaveFacilityRequestDtoValidator = bulkSaveFacilityRequestDtoValidator;
        }
        [HttpPost("Create")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(CreateFacilityResponseDto))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateFacilityRequestDto createFacilityRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _createFacilityRequestDtoValidator.ValidateAsync(createFacilityRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var organizationServiceObject = _mapper.Map<FacilityServiceObject>(createFacilityRequestDto);
                var responseServiceObject = await _facilityService.CreateFacilityAsync(organizationServiceObject, token);
                var response = _mapper.Map<CreateFacilityResponseDto>(responseServiceObject);
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
        [ProducesResponseType(201, Type = typeof(UpdateFacilityResponseDto))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateFacilityRequestDto updateFacilityRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updateFacilityRequestDtoValidator.ValidateAsync(updateFacilityRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var organizationServiceObject = _mapper.Map<FacilityServiceObject>(updateFacilityRequestDto);
                var responseServiceObject = await _facilityService.UpdateFacilityAsync(organizationServiceObject, token);
                var response = _mapper.Map<UpdateFacilityResponseDto>(responseServiceObject);
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
        public async Task<IActionResult> BulkSaveAsync([FromForm] BulkSaveFacilityRequestDto bulkSaveFacilityRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _bulkSaveFacilityRequestDtoValidator.ValidateAsync(bulkSaveFacilityRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var response = await _facilityService.BulkSaveFacilityAsync(bulkSaveFacilityRequestDto.Excelfile, token);
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
                var responseServiceObjectList = await _facilityService.GetFacilityDownloadListAsync(token);
                var getFacilityDownloadResponceDtoList = _mapper.Map<ICollection<GetFacilityDownloadResponseDto>>(responseServiceObjectList);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Facility");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "FacilityID";
                    worksheet.Cell(currentRow, 2).Value = "FacilityName";
                    worksheet.Cell(currentRow, 3).Value = "FacilityCode";
                    worksheet.Cell(currentRow, 4).Value = "SubscriptionStartDate";
                    worksheet.Cell(currentRow, 5).Value = "SubscriptionEndDate";
                    worksheet.Cell(currentRow, 6).Value = "IsUnlimited";
                    worksheet.Cell(currentRow, 7).Value = "ClaimCount";
                    worksheet.Cell(currentRow, 8).Value = "IsDOS";
                    worksheet.Cell(currentRow, 9).Value = "IsAbuDhabiDOS";
                    worksheet.Cell(currentRow, 10).Value = "FacilityType";
                    worksheet.Cell(currentRow, 11).Value = "Organization";
                    worksheet.Cell(currentRow, 12).Value = "Regulatory";
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < getFacilityDownloadResponceDtoList.Count; i++)
                        {
                            currentRow++;
                            var facility = getFacilityDownloadResponceDtoList.Skip(i).Take(1).FirstOrDefault();
                            worksheet.Cell(currentRow, 1).Value = facility.FacilityID;
                            worksheet.Cell(currentRow, 2).Value = facility.FacilityName;
                            worksheet.Cell(currentRow, 3).Value = facility.FacilityCode;
                            worksheet.Cell(currentRow, 4).Value = facility.FacilityCode;
                            worksheet.Cell(currentRow, 4).SetValue(facility.SubscriptionStartDate);
                            worksheet.Cell(currentRow, 5).Value = facility.FacilityCode;
                            worksheet.Cell(currentRow, 5).SetValue(facility.SubscriptionEndDate);
                            worksheet.Cell(currentRow, 6).Value = facility.IsUnlimited;
                            worksheet.Cell(currentRow, 7).Value = facility.ClaimCount;
                            worksheet.Cell(currentRow, 8).Value = facility.IsDOS;
                            worksheet.Cell(currentRow, 9).Value = facility.IsAbuDhabiDOS;
                            worksheet.Cell(currentRow, 10).Value = facility.FacilityType;
                            worksheet.Cell(currentRow, 11).Value = facility.Organization;
                            worksheet.Cell(currentRow, 12).Value = facility.Regulatory;
                        }
                    });
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Facility.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("FindFacilityByID")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindFacilityResponseDto))]
        public async Task<IActionResult> FindFacilityByIDAsync([FromQuery] int facilityID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _facilityService.GetFacilityByIdAsync(facilityID, token);
                var response = _mapper.Map<FindFacilityResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetFacilityList")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetFacilityResponseDto))]
        public async Task<IActionResult> GetFacilityListAsync(CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _facilityService.GetFacilityListAsync(token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetFacilityResponseDto>>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetFacilityListByName")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetFacilityResponseDto))]
        public async Task<IActionResult> GetFacilityListByNameAsync([FromQuery] string facilityName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _facilityService.GetFacilityListByNameAsync(facilityName, token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetFacilityResponseDto>>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetFacilityListByCode")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetFacilityResponseDto))]
        public async Task<IActionResult> GetFacilityListByCodeAsync([FromQuery] string facilityCode, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _facilityService.GetFacilityByCodeAsync(facilityCode, token);
                var getOrganizationResponceDtoList = _mapper.Map<GetFacilityResponseDto>(responseServiceObjectList);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetFacilityDDLByName")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetFacilityDDLResponseDto))]
        public async Task<IActionResult> GetFacilityDDLByNameAsync([FromQuery] string facilityName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _facilityService.GetFacilityDDLAsync(facilityName, token);
                var getOrganizationResponceDtoList = _mapper.Map<ICollection<GetFacilityDDLResponseDto>>(responseServiceObjectDDL);
                return Ok(getOrganizationResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
