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
    public sealed class RegulatoryService : IRegulatoryService
    {
        private readonly IMapper _mapper;
        private readonly IRegulatoryRepository _regulatoryRepository;
        public RegulatoryService(IRegulatoryRepository regulatoryRepository, IMapper mapper)
        {
            _regulatoryRepository = regulatoryRepository;
            _mapper = mapper;
        }
        public async Task<RegulatoryServiceObject> GetRegulatoryByIdAsync(int RegulatoryId, CancellationToken token)
        {
            var regulatoryrEntityList = await _regulatoryRepository.GetRegulatoryByIdAsync(RegulatoryId, token);
            return _mapper.Map<RegulatoryServiceObject>(regulatoryrEntityList);
        }
        public async Task<ICollection<RegulatoryServiceObject>> GetRegulatoryListByNameAsync(string RegulatoryName, CancellationToken token)
        {
            var regulatoryrEntityList = await _regulatoryRepository.GetRegulatoryListByNameAsync(RegulatoryName, token);
            return _mapper.Map<ICollection<RegulatoryServiceObject>>(regulatoryrEntityList);
        }
        public async Task<ICollection<RegulatoryServiceObject>> GetRegulatoryListAsync(CancellationToken token)
        {
            var regulatoryrEntityList = await _regulatoryRepository.GetRegulatoryListAsync(token);
            return _mapper.Map<ICollection<RegulatoryServiceObject>>(regulatoryrEntityList);
        }
        public async Task<RegulatoryServiceObject> GetRegulatoryByNameAsync(string regulatoryName, CancellationToken token)
        {
            var regulatoryrEntity = await _regulatoryRepository.GetRegulatoryByNameAsync(regulatoryName, token);
            return _mapper.Map<RegulatoryServiceObject>(regulatoryrEntity);
        }
        public async Task<RegulatoryServiceObject> CreateRegulatoryAsync(RegulatoryServiceObject regulatoryServiceObject, CancellationToken token)
        {
            var regulatoryEntity = _mapper.Map<RegulatoryEntity>(regulatoryServiceObject);
            var facilityEntityResponce = await _regulatoryRepository.CreateRegulatoryAsync(regulatoryEntity, token);
            regulatoryServiceObject = _mapper.Map<RegulatoryServiceObject>(facilityEntityResponce);
            return regulatoryServiceObject;
        }
        public async Task<RegulatoryServiceObject> UpdateRegulatoryAsync(RegulatoryServiceObject regulatoryServiceObject, CancellationToken token)
        {
            var regulatoryData = await _regulatoryRepository.GetRegulatoryByIdAsync(regulatoryServiceObject.RegulatoryID, token);
            regulatoryData.RegulatoryName = regulatoryServiceObject.RegulatoryName;
            regulatoryData.Status = regulatoryServiceObject.Status;
            var regulatoryEntityResponce = await _regulatoryRepository.UpdateRegulatoryAsync(regulatoryData, token);
            regulatoryServiceObject = _mapper.Map<RegulatoryServiceObject>(regulatoryEntityResponce);
            return regulatoryServiceObject;
        }
        public async Task<ICollection<RegulatoryServiceObject>> GetRegulatoryDistinctListFromNameListAsync(ICollection<RegulatoryServiceObject> regulatoryServiceObjectList, CancellationToken token)
        {
            ICollection<RegulatoryServiceObject> regulatoryServiceObjectResultList = new List<RegulatoryServiceObject>();
            var regulatoryNameList = regulatoryServiceObjectList.Select(x => x.RegulatoryName).Distinct().ToArray();
            for (int i = 0; i < regulatoryNameList.Length; i++)
            {
                var name = regulatoryNameList.Skip(i).Take(1).FirstOrDefault();
                if (name != null)
                {
                    RegulatoryServiceObject item = new RegulatoryServiceObject();
                    item = await GetRegulatoryByNameAsync(name, token);
                    if (item != null)
                        regulatoryServiceObjectResultList.Add(item);
                }
            }
            return regulatoryServiceObjectResultList;
        }
        public async Task<ICollection<RegulatoryServiceObject>> GetRegulatoryDDLAsync(CancellationToken token)
        {
            var regulatoryrEntityDDL = await _regulatoryRepository.GetRegulatoryDDLAsync(token);
            return _mapper.Map<ICollection<RegulatoryServiceObject>>(regulatoryrEntityDDL);
        }
    }
}
