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
    public sealed class FacilityService : IFacilityService
    {
        private readonly IMapper _mapper;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IOrganizationService _organizationService;
        private readonly IFacilityTypeService _facilityTypeService;
        private readonly IRegulatoryService _regulatoryService;

        public FacilityService(IFacilityRepository facilityRepository, IFacilityTypeService facilityTypeService, IRegulatoryService regulatoryService, IOrganizationService organizationService, IMapper mapper)
        {
            _facilityRepository = facilityRepository;
            _organizationService = organizationService;
            _facilityTypeService = facilityTypeService;
            _regulatoryService = regulatoryService;
            _mapper = mapper;
        }
        public async Task<FacilityServiceObject> GetFacilityByIdAsync(int facilityID, CancellationToken token)
        {
            var facilityEntity = await _facilityRepository.GetFacilityByIdAsync(facilityID, token);
            return _mapper.Map<FacilityServiceObject>(facilityEntity);
        }
        public async Task<FacilityServiceObject> GetFacilityByCodeAsync(string facilityCode, CancellationToken token)
        {
            var facilityEntity = await _facilityRepository.GetFacilityByCodeAsync(facilityCode, token);
            return _mapper.Map<FacilityServiceObject>(facilityEntity);
        }
        public async Task<FacilityServiceObject> FindFacilityByCodeAsync(string facilityCode, CancellationToken token)
        {
            var facilityEntity = await _facilityRepository.FindFacilityByCodeAsync(facilityCode, token);
            return _mapper.Map<FacilityServiceObject>(facilityEntity);
        }
        public async Task<ICollection<FacilityServiceObject>> GetFacilityDownloadListAsync(CancellationToken token)
        {
            var facilityNMEntityList = await _facilityRepository.GetFacilityDownloadListAsync(token);
            return _mapper.Map<ICollection<FacilityServiceObject>>(facilityNMEntityList);
        }
        public async Task<FacilityServiceObject> CreateFacilityAsync(FacilityServiceObject facilityServiceObject, CancellationToken token)
        {
            var facilityEntity = _mapper.Map<FacilityEntity>(facilityServiceObject);
            var facilityEntityResponce = await _facilityRepository.CreateFacilityAsync(facilityEntity, token);
            facilityServiceObject = _mapper.Map<FacilityServiceObject>(facilityEntityResponce);
            return facilityServiceObject;
        }
        public async Task<FacilityServiceObject> UpdateFacilityAsync(FacilityServiceObject facilityServiceObject, CancellationToken token)
        {
            var facilityData = await _facilityRepository.GetFacilityByIdAsync(facilityServiceObject.FacilityID, token);
            facilityData.IsUnlimited = facilityServiceObject.IsUnlimited;
            facilityData.IsAbuDhabiDOS = facilityServiceObject.IsAbuDhabiDOS;
            if (facilityData.IsUnlimited)
                facilityData.ClaimCount = 0;
            else
                facilityData.ClaimCount = facilityServiceObject.ClaimCount;
            if (facilityData.IsAbuDhabiDOS)
                facilityData.IsDOS = true;
            else
                facilityData.IsDOS = facilityServiceObject.IsDOS;
            facilityData.FacilityName = facilityServiceObject.FacilityName;
            facilityData.FacilityCode = facilityServiceObject.FacilityCode;
            facilityData.Status = facilityServiceObject.Status;
            facilityData.RegulatoryID = facilityServiceObject.RegulatoryID;
            facilityData.FacilityTypeID = facilityServiceObject.FacilityTypeID;
            facilityData.OrganizationID = facilityServiceObject.OrganizationID.HasValue ? facilityServiceObject.OrganizationID : null;
            facilityData.SubscriptionStartDate = facilityServiceObject.SubscriptionStartDate;
            facilityData.SubscriptionEndDate = facilityServiceObject.SubscriptionEndDate;
            var facilityEntityResponce = await _facilityRepository.UpdateFacilityAsync(facilityData, token);
            facilityServiceObject = _mapper.Map<FacilityServiceObject>(facilityEntityResponce);
            return facilityServiceObject;
        }
        public async Task<ICollection<FacilityServiceObject>> GetFacilityListByNameAsync(string facilityName, CancellationToken token)
        {
            var facilityEntityList = await _facilityRepository.GetFacilityListByNameAsync(facilityName, token);
            var facilityServiceObjectList = _mapper.Map<List<FacilityServiceObject>>(facilityEntityList);
            return facilityServiceObjectList;
        }
        public async Task<ICollection<FacilityServiceObject>> GetFacilityListAsync(CancellationToken token)
        {
            var facilityEntityList = await _facilityRepository.GetFacilityListAsync(token);
            var facilityServiceObjectList = _mapper.Map<List<FacilityServiceObject>>(facilityEntityList);
            return facilityServiceObjectList;
        }
        public async Task<string> BulkSaveFacilityAsync(IFormFile Excelfile, CancellationToken token)
        {
            ICollection<FacilityServiceObject> facilityServiceObjectList = new List<FacilityServiceObject>();
            if (Excelfile.Length > 0)
            {
                string FileName = Excelfile.FileName;
                var extension = Path.GetExtension(FileName);
                if (extension.ToLower() != ".xlsx")
                    return "Excell File only Supported";
                var facilityNMEntityList = await _facilityRepository.ReadExcelFacilityAsync(Excelfile, token);
                var regulatoryServiceObjectList = _mapper.Map<ICollection<RegulatoryServiceObject>>(facilityNMEntityList);
                regulatoryServiceObjectList = await _regulatoryService.GetRegulatoryDistinctListFromNameListAsync(regulatoryServiceObjectList, token);
                var organizationServiceObjectList = _mapper.Map<ICollection<OrganizationServiceObject>>(facilityNMEntityList);
                organizationServiceObjectList = await _organizationService.GetOrganizationDistinctListFromNameListAsync(organizationServiceObjectList, token);
                var facilityTypeServiceObjectList = _mapper.Map<ICollection<FacilityTypeServiceObject>>(facilityNMEntityList);
                facilityTypeServiceObjectList = await _facilityTypeService.GetFacilityTypeDistinctListFromNameListAsync(facilityTypeServiceObjectList, token);

                await Task.Run(() =>
                {
                    for (int i = 0; i < facilityNMEntityList.Count(); i++)
                    {
                        var data = facilityNMEntityList.Skip(i).Take(1).FirstOrDefault();
                        if (data != null)
                        {
                            FacilityServiceObject item = new FacilityServiceObject();
                            item.FacilityID = data.FacilityID;
                            item.FacilityName = data.FacilityName;
                            item.FacilityCode = data.FacilityCode;
                            var facilityType = facilityTypeServiceObjectList.FirstOrDefault(x => x.FacilityTypeName.ToLower() == data.FacilityType.ToLower());
                            item.FacilityTypeID = facilityType.FacilityTypeID;
                            var regulatory = regulatoryServiceObjectList.FirstOrDefault(x => x.RegulatoryName.ToLower() == data.Regulatory.ToLower());
                            item.RegulatoryID = regulatory.RegulatoryID;
                            if (data.IsAbuDhabiDOS)
                            {
                                item.IsAbuDhabiDOS = true;
                                item.IsDOS = true;
                            }
                            else
                                item.IsDOS = data.IsDOS;
                            var organization = organizationServiceObjectList.FirstOrDefault(x => x.OrganizationName.ToLower() == data.Organization.ToLower());
                            item.OrganizationID = organization != null && organization.OrganizationID > 0 ? (int?)organization.OrganizationID : null;
                            item.ClaimCount = data.ClaimCount;
                            item.SubscriptionStartDate = data.SubscriptionStartDate;
                            item.SubscriptionEndDate = data.SubscriptionStartDate;
                            item.IsUnlimited = data.IsUnlimited;
                            facilityServiceObjectList.Add(item);
                        }
                    }
                });
                var createFacilityEntityList = _mapper.Map<List<FacilityEntity>>(facilityServiceObjectList.Where(x => x.FacilityID < 1));
                await _facilityRepository.BulkCreateFacilityAsync(createFacilityEntityList, token);
                var updateFacilityEntityList = _mapper.Map<List<FacilityEntity>>(facilityServiceObjectList.Where(x => x.FacilityID > 0));
                await _facilityRepository.BulkUpdateFacilityAsync(updateFacilityEntityList, token);
                return "";
            }
            else
                return "File not found";
        }
        public async Task<ICollection<FacilityServiceObject>> GetFacilityDDLAsync(string facilityName, CancellationToken token)
        {
            var facilityEntityDDL = await _facilityRepository.GetFacilityDDLAsync(facilityName, token);
            var facilityServiceObjectDDL = _mapper.Map<List<FacilityServiceObject>>(facilityEntityDDL);
            return facilityServiceObjectDDL;
        }
        public async Task<ICollection<FacilityServiceObject>> GetFacilityDistinctListFromCodeListAsync(ICollection<FacilityServiceObject> facilityServiceObjectList, CancellationToken token)
        {
            ICollection<FacilityServiceObject> facilityServiceObjectResultList = new List<FacilityServiceObject>();
            var facilityCodeList = facilityServiceObjectList.Select(x => x.FacilityCode).Distinct().ToArray();
            for (int i = 0; i < facilityCodeList.Length; i++)
            {
                var code = facilityCodeList.Skip(i).Take(1).FirstOrDefault();
                if (code != null)
                {
                    FacilityServiceObject item = new FacilityServiceObject();
                    item = await GetFacilityByCodeAsync(code, token);
                    if (item != null)
                        facilityServiceObjectResultList.Add(item);
                }
            }
            return facilityServiceObjectResultList;
        }
    }
}
