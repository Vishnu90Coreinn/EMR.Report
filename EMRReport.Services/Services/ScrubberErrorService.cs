using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using EMRReport.DataContracts.IRepositories;
using EMRReport.DataContracts.Entities;
using EMRReport.Common.CodeEncryption;
using EMRReport.Common.Extensions;

namespace EMRReport.Services.Services
{
    public sealed class ScrubberErrorService : IScrubberErrorService
    {
        private readonly IScrubberErrorRepository _scrubberErrorRepository;
        private bool isScrubberDemo;
        private readonly IClaimBasketService _claimBasketService;
        private readonly IClaimService _claimService;
        private readonly IClaimActivityService _claimActivityService;
        private readonly IClaimDiagnosisService _claimDiagnosisService;
        private readonly IClaimEncounterService _claimEncounterService;
        private readonly IClaimActivityObservationService _claimActivityObservationService;
        private readonly IBasketGroupService _basketGroupService;
        private readonly ISettingsService _settingsService;

        private readonly IMapper _mapper;
        public ScrubberErrorService(IScrubberErrorRepository scrubberErrorRepository, IClaimService claimService, IClaimActivityService claimActivityService, IClaimBasketService claimBasketService, IClaimDiagnosisService claimDiagnosisService,
            IClaimEncounterService claimEncounterService, IClaimActivityObservationService claimActivityObservationService, IBasketGroupService basketGroupService, ISettingsService settingsService, IMapper mapper)
        {
            isScrubberDemo = false;
            _scrubberErrorRepository = scrubberErrorRepository;
            _claimBasketService = claimBasketService;
            _claimActivityService = claimActivityService;
            _claimDiagnosisService = claimDiagnosisService;
            _claimEncounterService = claimEncounterService;
            _claimService = claimService;
            _claimActivityObservationService = claimActivityObservationService;
            _basketGroupService = basketGroupService;
            _settingsService = settingsService;
            _mapper = mapper;
        }
        public async Task<List<ScrubberErrorServiceObject>> GetScrubberReportByBasketGroupIdAync(int basketGroupID, bool IsDetail, CancellationToken token)
        {
            var scrubberErrorEntityList = await _scrubberErrorRepository.GetScrubberReportByBasketGroupIdAync(basketGroupID, IsDetail, token);
            var scrubberErrorServiceObjectList = _mapper.Map<List<ScrubberErrorServiceObject>>(scrubberErrorEntityList);
            return scrubberErrorServiceObjectList;
        }
        public async Task<List<ScrubberErrorServiceObject>> GetScrubberErrorsByBasketGroupIdAync(int basketGroupID, int page, CancellationToken token)
        {
            var scrubberErrorEntityList = await _scrubberErrorRepository.GetScrubberErrorsByBasketGroupIdAync(basketGroupID, page, token);
            var scrubberErrorServiceObjectList = _mapper.Map<List<ScrubberErrorServiceObject>>(scrubberErrorEntityList);
            return scrubberErrorServiceObjectList;
        }
        public async Task<Tuple<int, List<ScrubberErrorServiceObject>>> GetScrubberErrorsByBasketGroupIdAndTotalAync(int basketGroupID, int page, CancellationToken token)
        {
            var gettuple = await _scrubberErrorRepository.GetScrubberErrorsByBasketGroupIdAndTotalAync(basketGroupID, page, token);
            var scrubberErrorServiceObjectList = _mapper.Map<List<ScrubberErrorServiceObject>>(gettuple.Item2);
            return Tuple.Create(gettuple.Item1, scrubberErrorServiceObjectList);
        }
        public async Task<Tuple<string, int>> GetScrubberErrorsFromFileCollectionAync(List<IFormFile> XMLfiles, CancellationToken token)
        {
            string fileName = "";
            BasketGroupServiceObject basketGroupServiceObject = new BasketGroupServiceObject();
            basketGroupServiceObject = await _basketGroupService.CreateBasketGroupAsync(basketGroupServiceObject, token);
            for (int i = 0; i < XMLfiles.Count; i++)
            {
                var xMLFile = XMLfiles.Skip(i).Take(1).FirstOrDefault();
                if (xMLFile != null)
                {
                    fileName = xMLFile.FileName;
                    string message = await GetScrubberErrorsAync(xMLFile, basketGroupServiceObject.BasketGroupID, token);
                    if (!string.IsNullOrEmpty(message))
                        return Tuple.Create("file :".Append(fileName, ", errorMessage :", message), basketGroupServiceObject.BasketGroupID);
                }
            }
            return Tuple.Create("", basketGroupServiceObject.BasketGroupID);
        }
        public async Task<string> GetScrubberErrorsAync(IFormFile xMLfile, int basketGroupID, CancellationToken token)
        {
            var getTuple = await _claimBasketService.CreateClaimBasketFromXMLFilesAsync(xMLfile, basketGroupID, isScrubberDemo, token);
            string message = getTuple.Item1;
            bool isDosFacility = getTuple.Item2;
            var isAbuDhabiDOS = getTuple.Item3;
            var claimBasketServiceObject = getTuple.Item4;
            if (string.IsNullOrEmpty(message))
                message = await SaveScrubberXMLFileDataAync(claimBasketServiceObject, isDosFacility, isAbuDhabiDOS, token);
            if (string.IsNullOrEmpty(message))
            {
                await SaveScrubberErrorsFromStoreProcedureAync(claimBasketServiceObject, token);
                return message;
            }
            return message;
        }

        public async Task<string> SaveScrubberXMLFileDataAync(ClaimBasketServiceObject claimBasketServiceObject, bool isDosFacility, bool isAbuDhabiDOS, CancellationToken token)
        {
            string message = "";
            var tuple = await _settingsService.GetDubaiAndAbuDhabiDOS(token);
            var dateOfService = tuple.Item1;
            var abuDhabiDOS = tuple.Item2;
            if (claimBasketServiceObject != null)
            {
                string filePath = claimBasketServiceObject.XMLFileName;
                DataSet ds = new DataSet();
                await Task.Run(() => ds.ReadXml(filePath));
                DataTable dtheader = ds.Tables["Header"];
                if (dtheader == null)
                    return "Invalid File";
                if (dtheader.Rows.Count > 1)
                    return "More Than One Header Tag Present";
                for (int k = 0; k < dtheader.Rows.Count; k++)
                {
                    var cardPattern = @"(^\d{1,2})/([a-z|A-Z][a-z|A-Z])/(\d{2,5})/+";
                    DataTable claimDataTable = ds.Tables["Claim"].AsEnumerable().Where(x => x.Field<string>("ProviderID").ToLower() == claimBasketServiceObject.SenderID.ToLower()).Any() ? ds.Tables["Claim"].AsEnumerable().Where(x => x.Field<string>("ProviderID").ToLower() == claimBasketServiceObject.SenderID.ToLower()).CopyToDataTable() : ds.Tables["Claim"].Clone();
                    var claimServiceObjectList = await _claimService.GetClaimFromDataTableAsync(claimDataTable, claimBasketServiceObject, cardPattern, token);
                    DataTable calimEncounterDataTable = ds.Tables["Encounter"].AsEnumerable().Where(x => x.Field<string>("FacilityID").ToLower() == claimBasketServiceObject.SenderID.ToLower()).Any() ? ds.Tables["Encounter"].AsEnumerable().Where(x => x.Field<string>("FacilityID").ToLower() == claimBasketServiceObject.SenderID.ToLower()).CopyToDataTable() : ds.Tables["Encounter"].Clone();
                    var DOS = isAbuDhabiDOS ? abuDhabiDOS : isDosFacility ? dateOfService : null;
                    await _claimEncounterService.bulkCreateClaimEncounterFromDataTableAndValidateDOS(calimEncounterDataTable, claimBasketServiceObject, isDosFacility, DOS, token);
                    DataTable claimDiagnosisDatatable = ds.Tables["Diagnosis"];
                    var claimDiagnosisServiceObjectList = await _claimDiagnosisService.BulkCreateClaimDaignosisFromDataTable(claimDiagnosisDatatable, claimBasketServiceObject, token);
                    DataTable claimActivityDataTable = ds.Tables["Activity"];
                    var claimActivityServiceObjectList = await _claimActivityService.BulkCreateClaimActivityFromDataTable(claimActivityDataTable, claimBasketServiceObject, token);
                    await _claimService.BulkCreateClaimWithClaimDetails(claimServiceObjectList, claimDiagnosisServiceObjectList, claimActivityServiceObjectList, token);
                    DataTable calimActivityObservationDataTable = ds.Tables["Observation"];
                    if (calimActivityObservationDataTable != null)
                        await _claimActivityObservationService.BulkCreateClaimActivityObservationFromDataTable(calimActivityObservationDataTable, claimBasketServiceObject, token);
                }
            }
            return message;
        }
        public async Task SaveScrubberErrorsFromStoreProcedureAync(ClaimBasketServiceObject claimBasketServiceObject, CancellationToken token)
        {
            List<ScrubberErrorServiceObject> scrubberErrorServiceObject = new List<ScrubberErrorServiceObject>();
            var claimBasketEntity = _mapper.Map<ClaimBasketEntity>(claimBasketServiceObject);
            var scrubberErrorEntityList = await _scrubberErrorRepository.GetScrubberActivityErrorsAsync(claimBasketEntity, token);
            var scrubberErrorEntityICDList = await _scrubberErrorRepository.GetScrubberICDErrorsAsync(claimBasketEntity, token);
            await Task.Run(() => scrubberErrorEntityList.AddRange(scrubberErrorEntityICDList));
            var scrubberErrorEntityICDCPTList = await _scrubberErrorRepository.GetScrubberICDCPTErrorsAsync(claimBasketEntity, token);
            await Task.Run(() => scrubberErrorEntityList.AddRange(scrubberErrorEntityICDCPTList));
            var scrubberErrorEntityObservationList = await _scrubberErrorRepository.GetScrubberObservationErrorsAsync(claimBasketEntity, token);
            await Task.Run(() => scrubberErrorEntityList.AddRange(scrubberErrorEntityObservationList));
            var scrubberErrorServiceObjectList = _mapper.Map<List<ScrubberErrorServiceObject>>(scrubberErrorEntityList);
            var scrubberErrorServiceObjectDecriptedList = await GetServiceValidarErrorDescriptAsync(scrubberErrorServiceObjectList, token);
            var scrubberErrorEntityDecriptedList = _mapper.Map<List<ScrubberErrorEntity>>(scrubberErrorServiceObjectDecriptedList);
            await _scrubberErrorRepository.CreateBulkScrubberErrorAsync(scrubberErrorEntityDecriptedList, token);

            var scrubberErrorEntityNonHitList = await _scrubberErrorRepository.GetScrubberNonHitErrorsAsync(claimBasketEntity, token);

            var scrubberErrorServiceObjectNonHitList = _mapper.Map<List<ScrubberErrorServiceObject>>(scrubberErrorEntityNonHitList);
            var scrubberErrorServiceObjectNonHitDecriptedList = await GetServiceValidarErrorDescriptAsync(scrubberErrorServiceObjectNonHitList, token);
            var scrubberErrorEntitytNonHitDecriptedList = _mapper.Map<List<ScrubberErrorEntity>>(scrubberErrorServiceObjectNonHitDecriptedList);
            await _scrubberErrorRepository.CreateBulkScrubberErrorAsync(scrubberErrorEntitytNonHitDecriptedList, token);
            //await Task.Run(() => scrubberErrorServiceObjectDecriptedList.AddRange(scrubberErrorServiceObjectNonHitDecriptedList));
            //return scrubberErrorServiceObjectDecriptedList;
        }
        public async Task<List<ScrubberErrorServiceObject>> GetServiceValidarErrorDescriptAsync(List<ScrubberErrorServiceObject> scrubberErrorServiceObjectList, CancellationToken token)
        {
            if (scrubberErrorServiceObjectList != null && scrubberErrorServiceObjectList.Count > 0)
            {
                await Task.Run(() =>
                {
                    for (int i = 0; i < scrubberErrorServiceObjectList.Count; i++)
                    {
                        var data = scrubberErrorServiceObjectList.Skip(i).Take(1).FirstOrDefault();
                        if (data != null)
                        {
                            data.Year = data.ErrorDate.GetYear();
                            data.Month = data.ErrorDate.GetMonth();
                            data.Day = data.ErrorDate.GetDate();
                            data.Week = data.ErrorDate.GetWeek();
                            if (!string.IsNullOrEmpty(data.ServiceCode) && data.ServiceCode.Length > 6)
                                data.ServiceCode = data.ServiceCode.DecriptCommaSeperatedCodes();
                            if (!string.IsNullOrEmpty(data.ErrorCode1) && data.ErrorCode1.Length > 6)
                                data.ErrorCode1 = data.ErrorCode1.DecriptCommaSeperatedCodes();
                            if (!string.IsNullOrEmpty(data.ErrorCode2) && data.ErrorCode2.Length > 6)
                            {
                                data.ErrorCode2 = data.ErrorCode2.DecriptCommaSeperatedCodes();
                                StringBuilder sb = new StringBuilder();
                                data.ErrorCode2 = data.ErrorCode2.Length > 399 ? sb.Append(data.ErrorCode2.Substring(1, 396)).Append("..").ToString() : data.ErrorCode2;
                            }
                            if (!string.IsNullOrEmpty(data.Message) && data.Message.IndexOf("COD-") == -1 && data.Message.IndexOf("SUB-") == -1 && data.Message.IndexOf("GD-") == -1 && !string.IsNullOrEmpty(data.Message))
                                data.Message = data.Message.DecriptCommaSeperatedMessage();
                            if (!string.IsNullOrEmpty(data.CodingTips) && data.CodingTips.Length > 6)
                                data.CodingTips = data.CodingTips.DecriptCommaSeperatedCodes();
                        }
                    }
                });
            }
            return scrubberErrorServiceObjectList;
        }

    }
}
