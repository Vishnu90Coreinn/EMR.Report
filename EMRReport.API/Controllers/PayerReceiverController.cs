using AutoMapper;
using ClosedXML.Excel;
using EMRReport.API.DataTranserObject.PayerReceiver;
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
    public class PayerReceiverController : ApiController
    {
        private readonly IPayerReceiverService _payerReceiverService;
        private readonly ILogger<PayerReceiverController> _logger;
        private readonly IValidator<CreatePayerReceiverRequestDto> _createPayerReceiverRequestDtoValidator;
        private readonly IValidator<UpdatePayerReceiverRequestDto> _updatePayerReceiverRequestDtoValidator;
        private readonly IValidator<BulkSavePayerReceiverRequestDto> _bulkSavePayerReceiverRequestDtoValidator;
        public PayerReceiverController(ILogger<PayerReceiverController> logger, IPayerReceiverService payerReceiverService, IValidator<CreatePayerReceiverRequestDto> createPayerReceiverRequestDtoValidator, IValidator<UpdatePayerReceiverRequestDto> updatePayerReceiverRequestDtoValidator,
            IValidator<BulkSavePayerReceiverRequestDto> bulkSavePayerReceiverRequestDtoValidator, IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _createPayerReceiverRequestDtoValidator = createPayerReceiverRequestDtoValidator;
            _updatePayerReceiverRequestDtoValidator = updatePayerReceiverRequestDtoValidator;
            _bulkSavePayerReceiverRequestDtoValidator = bulkSavePayerReceiverRequestDtoValidator;
            _payerReceiverService = payerReceiverService;
        }

        [HttpGet("GetPayerReceiverDDLByFacility")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetPayerReceiverDDLResponseDto))]
        public async Task<IActionResult> GetPayerReceiverDDLByFacilityAsync([FromQuery] int facilityID, [FromQuery] string payerReceiverName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _payerReceiverService.GetPayerReceiverDDLAsync(facilityID, payerReceiverName, token);
                var getPayerReceiverResponceDtoList = _mapper.Map<ICollection<GetPayerReceiverDDLResponseDto>>(responseServiceObjectDDL);
                return Ok(getPayerReceiverResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost("Create")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(CreatePayerReceiverResponseDto))]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePayerReceiverRequestDto createRegulatoryRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _createPayerReceiverRequestDtoValidator.ValidateAsync(createRegulatoryRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<PayerReceiverServiceObject>(createRegulatoryRequestDto);
                var responseServiceObject = await _payerReceiverService.CreateRegulatoryAsync(userServiceObject, token);
                var response = _mapper.Map<CreatePayerReceiverResponseDto>(responseServiceObject);
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
        [ProducesResponseType(201, Type = typeof(UpdatePayerReceiverResponseDto))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdatePayerReceiverRequestDto updatePayerReceiverRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _updatePayerReceiverRequestDtoValidator.ValidateAsync(updatePayerReceiverRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var userServiceObject = _mapper.Map<PayerReceiverServiceObject>(updatePayerReceiverRequestDto);
                var responseServiceObject = await _payerReceiverService.UpdateRegulatoryAsync(userServiceObject, token);
                var response = _mapper.Map<UpdatePayerReceiverResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("FindPayerReceiverByID")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindPayerReceiverResponseDto))]
        public async Task<IActionResult> FindPayerReceiverByIDAsync([FromQuery] int payerReceiverID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _payerReceiverService.GetPayerReceiverByIdAsync(payerReceiverID, token);
                var response = _mapper.Map<FindPayerReceiverResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("FindPayerReceiverByIdentification")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(FindPayerReceiverResponseDto))]
        public async Task<IActionResult> FindPayerReceiverByIdentificationAsync([FromQuery] int facilityID, [FromQuery] string payerIdentification, CancellationToken token = default)
        {
            try
            {
                var responseServiceObject = await _payerReceiverService.GetPayerReceiverByIdentificationAsync(facilityID, payerIdentification, token);
                var response = _mapper.Map<FindPayerReceiverResponseDto>(responseServiceObject);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetPayerReceiverListByFacility")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetPayerReceiverResponseDto))]
        public async Task<IActionResult> GetPayerReceiverListByFacilityAsync([FromQuery] int facilityID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _payerReceiverService.GetPayerReceiverListAsync(facilityID, token);
                var getPayerReceiverResponceDtoList = _mapper.Map<ICollection<GetPayerReceiverResponseDto>>(responseServiceObjectDDL);
                return Ok(getPayerReceiverResponceDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetPayerReceiverListByName")]
        [JWTAuthorizeAttribute]
        [ProducesResponseType(201, Type = typeof(GetPayerReceiverResponseDto))]
        public async Task<IActionResult> GetPayerReceiverListByNameAsync([FromQuery] int facilityID, [FromQuery] string payerReceiverName, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectDDL = await _payerReceiverService.GetPayerReceiverListByNameAsync(facilityID, payerReceiverName, token);
                var getPayerReceiverResponceDtoList = _mapper.Map<ICollection<GetPayerReceiverResponseDto>>(responseServiceObjectDDL);
                return Ok(getPayerReceiverResponceDtoList);
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
        public async Task<IActionResult> BulkSaveAsync([FromForm] BulkSavePayerReceiverRequestDto bulkSavePayerReceiverRequestDto, CancellationToken token = default)
        {
            try
            {
                var validationResult = await _bulkSavePayerReceiverRequestDtoValidator.ValidateAsync(bulkSavePayerReceiverRequestDto, token);
                if (!validationResult.IsValid)
                    return new BadRequestObjectResult(validationResult.Errors.ToList());
                var response = await _payerReceiverService.BulkSavePayerReceiverAsync(bulkSavePayerReceiverRequestDto.Excelfile, token);
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
        public async Task<IActionResult> BulkDownloadAsync([FromQuery] int facilityID, CancellationToken token = default)
        {
            try
            {
                var responseServiceObjectList = await _payerReceiverService.GetPayerReceiverDownloadListAsync(facilityID, token);
                var getFacilityDownloadResponceDtoList = _mapper.Map<ICollection<BulkDonloadPayerReceiverResponseDto>>(responseServiceObjectList);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("PayerReceiver");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "PayerReceiverID";
                    worksheet.Cell(currentRow, 2).Value = "PayerReceiverName";
                    worksheet.Cell(currentRow, 3).Value = "PayerReceiverShortName";
                    worksheet.Cell(currentRow, 4).Value = "PayerReceiverIdentification";
                    worksheet.Cell(currentRow, 5).Value = "InsuranceClassification";
                    worksheet.Cell(currentRow, 6).Value = "Status";
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < getFacilityDownloadResponceDtoList.Count; i++)
                        {
                            currentRow++;
                            var payerReceiver = getFacilityDownloadResponceDtoList.Skip(i).Take(1).FirstOrDefault();
                            worksheet.Cell(currentRow, 1).Value = payerReceiver.PayerReceiverID;
                            worksheet.Cell(currentRow, 2).Value = payerReceiver.PayerReceiverName;
                            worksheet.Cell(currentRow, 3).Value = payerReceiver.PayerReceiverShortName;
                            worksheet.Cell(currentRow, 4).Value = payerReceiver.PayerReceiverIdentification;
                            worksheet.Cell(currentRow, 5).Value = payerReceiver.InsuranceClassification;
                            worksheet.Cell(currentRow, 6).Value = payerReceiver.Status;
                        }
                    });
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayerReceivers.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
