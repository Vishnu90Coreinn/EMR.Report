using AutoMapper;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public class EncounterTypeService : IEncounterTypeService
    {
        private readonly IEncounterTypeRepository _encounterTypeRepository;
        private readonly IMapper _mapper;
        public EncounterTypeService(IEncounterTypeRepository encounterTypeRepository, IMapper mapper)
        {
            _encounterTypeRepository = encounterTypeRepository;
            _mapper = mapper;
        }
        public async Task<EncounterTypeServiceObject> CreateEncounterTypeAsync(EncounterTypeServiceObject encounterTypeServiceObject, CancellationToken token)
        {
            var encounterTypeEntity = _mapper.Map<EncounterTypeEntity>(encounterTypeServiceObject);
            var encounterTypeEntityResponse = await _encounterTypeRepository.CreateEncounterTypeAsync(encounterTypeEntity, token);
            encounterTypeServiceObject = _mapper.Map<EncounterTypeServiceObject>(encounterTypeEntityResponse);
            return encounterTypeServiceObject;
        }

        public async Task<EncounterTypeServiceObject> FindEncounterTypeByIDAsync(int ID, CancellationToken token)
        {
            var encounterTypeEntity = await _encounterTypeRepository.FindEncounterTypeByIDAsync(ID, token);
            var encounterTypeServiceObject = _mapper.Map<EncounterTypeServiceObject>(encounterTypeEntity);
            return encounterTypeServiceObject;
        }

        public async Task<EncounterTypeServiceObject> GetEncounterTypeByNameAsync(string EncounterTypeName, CancellationToken token)
        {
            var encounterTypeEntityList = await _encounterTypeRepository.GetEncounterTypeByNameAsync(EncounterTypeName, token);
            var encounterTypeServiceObject = _mapper.Map<EncounterTypeServiceObject>(encounterTypeEntityList);
            return encounterTypeServiceObject;
        }

        public async Task<ICollection<EncounterTypeServiceObject>> GetEncounterTypeListAsync(CancellationToken token)
        {
            var encounterTypeEntityList = await _encounterTypeRepository.GetEncounterTypeListAsync(token);
            var organizationServiceObjectList = _mapper.Map<List<EncounterTypeServiceObject>>(encounterTypeEntityList);
            return organizationServiceObjectList;
        }

        public async Task<ICollection<EncounterTypeServiceObject>> GetEncounterTypeListByNameAsync(string EncounterTypeName, CancellationToken token)
        {
            var encounterTypeEntityList = await _encounterTypeRepository.GetEncounterTypeListByNameAsync(EncounterTypeName, token);
            var encounterTypeServiceObjectList = _mapper.Map<List<EncounterTypeServiceObject>>(encounterTypeEntityList);
            return encounterTypeServiceObjectList;
        }

        public async Task<ICollection<EncounterTypeServiceObject>> GetEncounterTypeListFromNameListAsync(ICollection<EncounterTypeServiceObject> encounterTypeServiceObjectList, CancellationToken token)
        {
            ICollection<EncounterTypeServiceObject> encounterTypeServiceObjectResultList = new List<EncounterTypeServiceObject>();
            var encounterTypeNameList = encounterTypeServiceObjectList.Select(x => x.EncounterType).Distinct().ToArray();
            for (int i = 0; i < encounterTypeNameList.Length; i++)
            {
                var data = encounterTypeNameList.Skip(i).Take(1).FirstOrDefault();
                if (data != null)
                {
                    EncounterTypeServiceObject item = new EncounterTypeServiceObject();
                    item = await GetEncounterTypeByNameAsync(data, token);
                    if (item != null)
                        encounterTypeServiceObjectResultList.Add(item);
                }
            }
            return encounterTypeServiceObjectResultList;
        }

        public async Task<EncounterTypeServiceObject> UpdateEncounterTypeAsync(EncounterTypeServiceObject encounterTypeServiceObject, CancellationToken token)
        {
            var encounterTypeData = await _encounterTypeRepository.FindEncounterTypeByIDAsync(encounterTypeServiceObject.ID, token);
            encounterTypeData.EncounterTypeID = encounterTypeServiceObject.EncounterTypeID;
            encounterTypeData.EncounterType = encounterTypeServiceObject.EncounterType;
            var encounterTypeEntityResponse = await _encounterTypeRepository.UpdateEncounterTypeAsync(encounterTypeData, token);
            encounterTypeServiceObject = _mapper.Map<EncounterTypeServiceObject>(encounterTypeEntityResponse);
            return encounterTypeServiceObject;

        }
    }
}
