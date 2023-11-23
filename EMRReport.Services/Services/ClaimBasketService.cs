using AutoMapper;
using EMRReport.Common.Extensions;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class ClaimBasketService : IClaimBasketService
    {
        private int userID;
        private readonly IMapper _mapper;
        private readonly IClaimBasketRepository _claimBasketRepository;
        private readonly IFacilityService _facilityService;
        private readonly IPayerReceiverService _payerReceiverService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ClaimBasketService(IClaimBasketRepository claimBasketRepository, IWebHostEnvironment webHostEnvironment, IFacilityService facilityService, IPayerReceiverService payerReceiverService, IMapper mapper)
        {
            _claimBasketRepository = claimBasketRepository;
            _mapper = mapper;
            _facilityService = facilityService;
            _payerReceiverService = payerReceiverService;
            _webHostEnvironment = webHostEnvironment;
            userID = 1;
        }
        public async Task<ClaimBasketServiceObject> CreateClaimBasketAsync(ClaimBasketServiceObject claimBasketServiceObject, CancellationToken token)
        {
            var claimBasketEntity = _mapper.Map<ClaimBasketEntity>(claimBasketServiceObject);
            claimBasketEntity = await _claimBasketRepository.CreateClaimBasketAsync(claimBasketEntity, token);
            return _mapper.Map<ClaimBasketServiceObject>(claimBasketEntity);
        }
        public async Task<List<ClaimBasketServiceObject>> CreateBulkClaimBasketAsync(List<ClaimBasketServiceObject> claimBasketServiceObjectList, CancellationToken token)
        {
            var claimBasketEntityList = _mapper.Map<List<ClaimBasketEntity>>(claimBasketServiceObjectList);
            claimBasketEntityList = await _claimBasketRepository.CreateBulkClaimBasketAsync(claimBasketEntityList, token);
            return _mapper.Map<List<ClaimBasketServiceObject>>(claimBasketEntityList);
        }
        public async Task<ClaimBasketServiceObject> GetClaimBasketByIdAsync(int ClaimBasketID, CancellationToken token)
        {
            var claimBasketEntity = await _claimBasketRepository.GetClaimBasketByIdAsync(ClaimBasketID, token);
            return _mapper.Map<ClaimBasketServiceObject>(claimBasketEntity);
        }
        public async Task<Tuple<string, bool, bool, ClaimBasketServiceObject>> CreateClaimBasketFromXMLFilesAsync(IFormFile XMLfile, int basketGroupID, bool IsScrubberDemo, CancellationToken token)
        {
            bool isDosFacility = false;
            bool isAbuDhabiDOS = false;
            string message = "";
            ClaimBasketServiceObject claimBasketServiceObject = new ClaimBasketServiceObject();
            string xmlFilePath = "ScrubberFiles".Append("\\", DateTime.Now.ConvertDateToPathString(), "\\");

            if (!Directory.Exists(xmlFilePath))
            {
                Directory.CreateDirectory(xmlFilePath);
            }
            if (XMLfile.Length > 0)
            {
                claimBasketServiceObject.FileName = XMLfile.FileName;
                var extension = Path.GetExtension(XMLfile.FileName);
                if (extension.ToLower() != ".xml")
                {
                    message = "XML File only Supported";
                    return Tuple.Create(message, isDosFacility, isAbuDhabiDOS, claimBasketServiceObject);
                }
                string uniqueString = DateTime.Now.Ticks.ToString();
                xmlFilePath = xmlFilePath.Append(uniqueString, "-", XMLfile.FileName);
                string xmlFileMapPath = _webHostEnvironment.ContentRootPath.Append("\\", xmlFilePath);
                using (Stream fileStream = new FileStream(xmlFileMapPath, FileMode.Create))
                {
                    await XMLfile.CopyToAsync(fileStream);
                }
            }
            else
            {
                message = "File not found";
                return Tuple.Create(message, isDosFacility, isAbuDhabiDOS, claimBasketServiceObject);
            }
            DataSet ds = new DataSet();
            await Task.Run(() => ds.ReadXml(xmlFilePath));
            DataTable claimBasketDataTable = ds.Tables["Header"];
            claimBasketServiceObject.SenderID = claimBasketDataTable.Rows[0]["SenderID"].ToString();
            claimBasketServiceObject.ReceiverID = claimBasketDataTable.Rows[0]["ReceiverID"].ToString();
            claimBasketServiceObject.TransactionDate = claimBasketDataTable.Rows[0]["TransactionDate"].ToString();
            claimBasketServiceObject.RecordCount = Convert.ToInt32(claimBasketDataTable.Rows[0]["RecordCount"]);
            claimBasketServiceObject.DispositionFlag = claimBasketDataTable.Rows[0]["DispositionFlag"].ToString();
            claimBasketServiceObject.XMLFileName = xmlFilePath;
            var facility = IsScrubberDemo ? await _facilityService.FindFacilityByCodeAsync("demo", token) : await _facilityService.FindFacilityByCodeAsync(claimBasketServiceObject.SenderID, token);
            if (facility != null)
            {
                claimBasketServiceObject.FacilityID = facility.FacilityID;
                claimBasketServiceObject.RegulatoryID = facility.RegulatoryID;
                isDosFacility = facility.IsDOS;
                isAbuDhabiDOS = facility.IsAbuDhabiDOS;
            }
            else
            {
                message = "Either facility with sernder code [" + claimBasketServiceObject.SenderID + "] Missing in DB or Wrong File";
                return Tuple.Create(message, isDosFacility, isAbuDhabiDOS, claimBasketServiceObject);
            }
            var PayerReceiver = IsScrubberDemo ? await _payerReceiverService.GetPayerReceiverByIdentificationAsync(facility.FacilityID, "demo", token) : await _payerReceiverService.GetPayerReceiverByIdentificationAsync(facility.FacilityID, claimBasketServiceObject.ReceiverID, token);
            if (PayerReceiver != null)
            {
                claimBasketServiceObject.PayerReceiverID = PayerReceiver.PayerReceiverID;
            }
            else
            {
                message = "Either Receiver code [" + claimBasketServiceObject.ReceiverID + "] Missing in DB or Wrong File";
                return Tuple.Create(message, isDosFacility, isAbuDhabiDOS, claimBasketServiceObject);
            }
            claimBasketServiceObject.CreatedBy = userID;
            claimBasketServiceObject.CreatedDate = DateTime.Now;
            claimBasketServiceObject.Status = true;
            claimBasketServiceObject.BasketGroupID = basketGroupID;
            claimBasketServiceObject = await CreateClaimBasketAsync(claimBasketServiceObject, token);
            return Tuple.Create(message, isDosFacility, isAbuDhabiDOS, claimBasketServiceObject);
        }
    }
}