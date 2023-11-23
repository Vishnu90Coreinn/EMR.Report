using AutoMapper;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class PayerReceiverService : IPayerReceiverService
    {
        private readonly IMapper _mapper;
        private readonly IPayerReceiverRepository _payerReceiverRepository;
        private readonly IFacilityService _facilityService;
        private readonly IInsuranceClassificationService _insuranceClassificationService;


        public PayerReceiverService(IPayerReceiverRepository payerReceiverRepository, IFacilityService facilityService, IInsuranceClassificationService insuranceClassificationService, IMapper mapper)
        {
            _payerReceiverRepository = payerReceiverRepository;
            _facilityService = facilityService;
            _insuranceClassificationService = insuranceClassificationService;
            _mapper = mapper;
        }
        public async Task<PayerReceiverServiceObject> GetPayerReceiverByIdAsync(int PayerReceiverID, CancellationToken token)
        {
            var payerReceiverEntity = await _payerReceiverRepository.GetPayerReceiverByIdAsync(PayerReceiverID, token);
            return _mapper.Map<PayerReceiverServiceObject>(payerReceiverEntity);
        }
        public async Task<PayerReceiverServiceObject> GetPayerReceiverByIdentificationAsync(int FacilityID, string Identification, CancellationToken token)
        {
            var payerReceiverEntity = await _payerReceiverRepository.GetPayerReceiverByIdentificationAsync(FacilityID, Identification, token);
            return _mapper.Map<PayerReceiverServiceObject>(payerReceiverEntity);
        }
        public async Task<ICollection<PayerReceiverServiceObject>> GetPayerReceiverDDLAsync(int facilityID, string payerReceiverName, CancellationToken token)
        {
            var payerReceiverEntityList = await _payerReceiverRepository.GetPayerReceiverDDLAsync(facilityID, payerReceiverName, token);
            return _mapper.Map<ICollection<PayerReceiverServiceObject>>(payerReceiverEntityList);
        }
        public async Task<PayerReceiverServiceObject> CreateRegulatoryAsync(PayerReceiverServiceObject payerReceiverServiceObject, CancellationToken token)
        {
            var payerReceiverEntity = _mapper.Map<PayerReceiverEntity>(payerReceiverServiceObject);
            payerReceiverEntity.ReceiverID = payerReceiverServiceObject.PayerReceiverIdentification;
            payerReceiverEntity.PayerReceiverIdentificationValidate = payerReceiverServiceObject.PayerReceiverIdentification;
            var payerReceiverEntityResponce = await _payerReceiverRepository.CreatePayerReceiverAsync(payerReceiverEntity, token);
            payerReceiverServiceObject = _mapper.Map<PayerReceiverServiceObject>(payerReceiverEntityResponce);
            return payerReceiverServiceObject;
        }
        public async Task<ICollection<PayerReceiverServiceObject>> GetPayerReceiverListAsync(int facilityID, CancellationToken token)
        {
            var apyerReceiverNMEntityList = await _payerReceiverRepository.GetPayerReceiverListAsync(facilityID, token);
            return _mapper.Map<ICollection<PayerReceiverServiceObject>>(apyerReceiverNMEntityList);
        }
        public async Task<ICollection<PayerReceiverServiceObject>> GetPayerReceiverListByNameAsync(int facilityID, string payerReceiverName, CancellationToken token)
        {
            var apyerReceiverNMEntityList = await _payerReceiverRepository.GetPayerReceiverListByNameAsync(facilityID, payerReceiverName, token);
            return _mapper.Map<ICollection<PayerReceiverServiceObject>>(apyerReceiverNMEntityList);
        }
        public async Task<PayerReceiverServiceObject> UpdateRegulatoryAsync(PayerReceiverServiceObject payerReceiverServiceObject, CancellationToken token)
        {
            var payerReceiverData = await _payerReceiverRepository.GetPayerReceiverByIdAsync(payerReceiverServiceObject.PayerReceiverID, token);
            payerReceiverData.PayerReceiverName = payerReceiverServiceObject.PayerReceiverName;
            payerReceiverData.PayerReceiverShortName = payerReceiverServiceObject.PayerReceiverShortName;
            payerReceiverData.PayerReceiverIdentification = payerReceiverServiceObject.PayerReceiverIdentification;
            payerReceiverData.ReceiverID = payerReceiverData.PayerReceiverIdentification;
            payerReceiverData.PayerReceiverIdentificationValidate = payerReceiverData.PayerReceiverIdentification;
            payerReceiverData.FacilityID = payerReceiverServiceObject.FacilityID;
            payerReceiverData.InsuranceClassificationID = payerReceiverServiceObject.InsuranceClassificationID;
            payerReceiverData.RegulatoryID = payerReceiverServiceObject.RegulatoryID;
            payerReceiverData.Status = payerReceiverServiceObject.Status;
            var payerReceiverEntityResponce = await _payerReceiverRepository.UpdatePayerReceiverAsync(payerReceiverData, token);
            payerReceiverServiceObject = _mapper.Map<PayerReceiverServiceObject>(payerReceiverEntityResponce);
            return payerReceiverServiceObject;
        }
        public async Task<string> BulkSavePayerReceiverAsync(IFormFile Excelfile, CancellationToken token)
        {
            ICollection<PayerReceiverServiceObject> payerReceiverServiceObjectList = new List<PayerReceiverServiceObject>();
            if (Excelfile.Length > 0)
            {
                string FileName = Excelfile.FileName;
                var extension = Path.GetExtension(FileName);
                if (extension.ToLower() != ".xlsx")
                    return "Excell File only Supported";
                var payerReceiverNMEntityList = await _payerReceiverRepository.ReadPayerReceiverFromExcelAsync(Excelfile, token);
                var facilityServiceObjectList = _mapper.Map<ICollection<FacilityServiceObject>>(payerReceiverNMEntityList);
                facilityServiceObjectList = await _facilityService.GetFacilityDistinctListFromCodeListAsync(facilityServiceObjectList, token);
                var insuranceClassificationServiceObjectList = _mapper.Map<ICollection<InsuranceClassificationServiceObject>>(payerReceiverNMEntityList);
                insuranceClassificationServiceObjectList = await _insuranceClassificationService.GetInsuranceClassificationDistinctListFromNameListAsync(insuranceClassificationServiceObjectList, token);
                await Task.Run(() =>
                {
                    for (int i = 0; i < facilityServiceObjectList.Count; i++)
                    {
                        var facility = facilityServiceObjectList.Skip(i).Take(1).FirstOrDefault();
                        for (int j = 0; j < payerReceiverNMEntityList.Count(); j++)
                        {
                            var data = payerReceiverNMEntityList.Skip(j).Take(1).FirstOrDefault();
                            if (data != null)
                            {
                                var insuranceClassification = insuranceClassificationServiceObjectList.FirstOrDefault(x => x.InsuranceClassificationName == data.InsuranceClassification);
                                PayerReceiverServiceObject item = new PayerReceiverServiceObject();
                                item.PayerReceiverID = data.PayerReceiverID;
                                item.PayerReceiverName = data.PayerReceiverName;
                                item.PayerReceiverShortName = data.PayerReceiverShortName;
                                item.PayerReceiverIdentification = data.PayerReceiverIdentification;
                                item.FacilityID = facility.FacilityID;
                                item.RegulatoryID = facility.RegulatoryID;
                                item.InsuranceClassificationID = insuranceClassification.InsuranceClassificationID;
                                item.Status = data.Status;
                                payerReceiverServiceObjectList.Add(item);
                            }
                        }
                    }
                });
                var createFacilityEntityList = _mapper.Map<List<PayerReceiverEntity>>(payerReceiverServiceObjectList.Where(x => x.PayerReceiverID < 1));
                await _payerReceiverRepository.BulkCreatePyaerReceiverAsync(createFacilityEntityList, token);
                var updateFacilityEntityList = _mapper.Map<List<PayerReceiverEntity>>(payerReceiverServiceObjectList.Where(x => x.PayerReceiverID > 0));
                await _payerReceiverRepository.BulkUpdatePyaerReceiverAsync(updateFacilityEntityList, token);
                return "";
            }
            else
                return "File not found";
        }
        public async Task<ICollection<PayerReceiverServiceObject>> GetPayerReceiverDownloadListAsync(int facilityID, CancellationToken token)
        {
            var facilityNMEntityList = await _payerReceiverRepository.GetPayerReceiverDownloadListAsync(facilityID, token);
            return _mapper.Map<ICollection<PayerReceiverServiceObject>>(facilityNMEntityList);
        }
    }
}
