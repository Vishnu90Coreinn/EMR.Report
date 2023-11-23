using AutoMapper;
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
    public sealed class FacilityTypeService : IFacilityTypeService
    {
        private readonly IMapper _mapper;
        private readonly IFacilityTypeRepository _facilityTypeRepository;
        public FacilityTypeService(IFacilityTypeRepository facilityTypeRepository, IMapper mapper)
        {
            _facilityTypeRepository = facilityTypeRepository;
            _mapper = mapper;
        }
        public async Task<FacilityTypeServiceObject> GetFacilityTypeByNameAsync(string facilityTypeName, CancellationToken token)
        {
            var facilityTypeEntity = await _facilityTypeRepository.GetFacilityTypeByNameAsync(facilityTypeName, token);
            return _mapper.Map<FacilityTypeServiceObject>(facilityTypeEntity);
        }
        public async Task<ICollection<FacilityTypeServiceObject>> GetFacilityTypeDistinctListFromNameListAsync(ICollection<FacilityTypeServiceObject> facilityTypeServiceObjectList, CancellationToken token)
        {
            ICollection<FacilityTypeServiceObject> facilityTypeServiceObjectResultList = new List<FacilityTypeServiceObject>();
            var facilityTypeNameList = facilityTypeServiceObjectList.Select(x => x.FacilityTypeName).Distinct().ToArray();
            for (int i = 0; i < facilityTypeNameList.Length; i++)
            {
                var data = facilityTypeNameList.Skip(i).Take(1).FirstOrDefault();
                if (data != null)
                {
                    FacilityTypeServiceObject item = new FacilityTypeServiceObject();
                    item = await GetFacilityTypeByNameAsync(data, token);
                    if (item != null)
                        facilityTypeServiceObjectResultList.Add(item);
                }
            }
            return facilityTypeServiceObjectResultList;
        }
        public async Task<ICollection<FacilityTypeServiceObject>> GetFacilityTypeDDLAsync(CancellationToken token)
        {
            var facilityTypeEntityList = await _facilityTypeRepository.GetFacilityTypeDDLAsync(token);
            return _mapper.Map<ICollection<FacilityTypeServiceObject>>(facilityTypeEntityList);
        }
    }
}
