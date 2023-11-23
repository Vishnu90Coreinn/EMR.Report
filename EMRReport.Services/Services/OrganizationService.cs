using AutoMapper;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public class OrganizationService : IOrganizationService
    {
        IOrganizationRepository _organizationRepository;
        IMapper _mapper;
        public OrganizationService(IOrganizationRepository organizationRepository, IMapper mapper)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
        }
        public async Task<OrganizationServiceObject> CreateOrganizationAsync(OrganizationServiceObject organizationServiceObject, CancellationToken token)
        {
            var organizationEntity = _mapper.Map<OrganizationEntity>(organizationServiceObject);
            var organizationEntityResponce = await _organizationRepository.CreateOrganizationAsync(organizationEntity, token);
            organizationServiceObject = _mapper.Map<OrganizationServiceObject>(organizationEntityResponce);
            return organizationServiceObject;
        }
        public async Task<OrganizationServiceObject> UpdateOrganizationAsync(OrganizationServiceObject organizationServiceObject, CancellationToken token)
        {
            var organizationData = await _organizationRepository.GetOrganizationByIDAsync(organizationServiceObject.OrganizationID, token);
            organizationData.IsUnlimited = organizationServiceObject.IsUnlimited;
            if (organizationData.IsUnlimited)
                organizationData.ClaimCount = 0;
            else
                organizationData.ClaimCount = organizationServiceObject.ClaimCount;
            organizationData.OrganizationName = organizationServiceObject.OrganizationName;
            organizationData.SubscriptionStartDate = organizationServiceObject.SubscriptionStartDate;
            organizationData.SubscriptionEndDate = organizationServiceObject.SubscriptionEndDate;
            organizationData.Status = organizationServiceObject.Status;
            var organizationEntityResponce = await _organizationRepository.UpdateOrganizationAsync(organizationData, token);
            organizationServiceObject = _mapper.Map<OrganizationServiceObject>(organizationEntityResponce);
            return organizationServiceObject;
        }
        public async Task<OrganizationServiceObject> GetOrganizationByIDAsync(int OrganizationID, CancellationToken token)
        {
            var organizationEntity = await _organizationRepository.GetOrganizationByIDAsync(OrganizationID, token);
            var organizationServiceObject = _mapper.Map<OrganizationServiceObject>(organizationEntity);
            return organizationServiceObject;
        }
        public async Task<ICollection<OrganizationServiceObject>> GetOrganizationListByNameAsync(string organizationName, CancellationToken token)
        {
            var organizationEntityList = await _organizationRepository.GetOrganizationListByNameAsync(organizationName, token);
            var organizationServiceObjectList = _mapper.Map<List<OrganizationServiceObject>>(organizationEntityList);
            return organizationServiceObjectList;
        }
        public async Task<OrganizationServiceObject> GetOrganizationByNameAsync(string organizationName, CancellationToken token)
        {
            var organizationEntityList = await _organizationRepository.GetOrganizationByNameAsync(organizationName, token);
            var organizationServiceObject = _mapper.Map<OrganizationServiceObject>(organizationEntityList);
            return organizationServiceObject;
        }
        public async Task<ICollection<OrganizationServiceObject>> GetOrganizationListAsync(CancellationToken token)
        {
            var organizationEntityList = await _organizationRepository.GetOrganizationListAsync(token);
            var organizationServiceObjectList = _mapper.Map<List<OrganizationServiceObject>>(organizationEntityList);
            return organizationServiceObjectList;
        }
        public async Task<ICollection<OrganizationServiceObject>> GetOrganizationDistinctListFromNameListAsync(ICollection<OrganizationServiceObject> organizationServiceObjectList, CancellationToken token)
        {
            ICollection<OrganizationServiceObject> organizationServiceObjectResultList = new List<OrganizationServiceObject>();
            var organizationNameList = organizationServiceObjectList.Select(x => x.OrganizationName).Distinct().ToArray();
            for (int i = 0; i < organizationNameList.Length; i++)
            {
                var data = organizationNameList.Skip(i).Take(1).FirstOrDefault();
                if (data != null)
                {
                    OrganizationServiceObject item = new OrganizationServiceObject();
                    item = await GetOrganizationByNameAsync(data, token);
                    if (item != null)
                        organizationServiceObjectResultList.Add(item);
                }
            }
            return organizationServiceObjectResultList;
        }
    }
}
