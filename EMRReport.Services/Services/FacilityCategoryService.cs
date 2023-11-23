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
    public sealed class FacilityCategoryService : IFacilityCategoryService
    {
        private readonly IMapper _mapper;
        private readonly IFacilityCategoryRepository _facilityCategoryRepository;
        public FacilityCategoryService(IFacilityCategoryRepository facilityCategoryRepository, IMapper mapper)
        {
            _facilityCategoryRepository = facilityCategoryRepository;
            _mapper = mapper;
        }
        public async Task<FacilityCategoryServiceObject> FindFacilityCategoryByIdAsync(int facilityCategoryId, CancellationToken token)
        {
            var facilityCategoryrEntityList = await _facilityCategoryRepository.GetFacilityCategoryByIdAsync(facilityCategoryId, token);
            return _mapper.Map<FacilityCategoryServiceObject>(facilityCategoryrEntityList);
        }
        public async Task<ICollection<FacilityCategoryServiceObject>> GetFacilityCategoryListByNameAsync(string facilityCategoryName, CancellationToken token)
        {
            var facilityCategoryrEntityList = await _facilityCategoryRepository.GetFacilityCategoryListByNameAsync(facilityCategoryName, token);
            return _mapper.Map<ICollection<FacilityCategoryServiceObject>>(facilityCategoryrEntityList);
        }
        public async Task<ICollection<FacilityCategoryServiceObject>> GetFacilityCategoryListAsync(CancellationToken token)
        {
            var regulatoryrEntityList = await _facilityCategoryRepository.GetFacilityCategoryListAsync(token);
            return _mapper.Map<ICollection<FacilityCategoryServiceObject>>(regulatoryrEntityList);
        }
        public async Task<FacilityCategoryServiceObject> CreateFacilityCategoryAsync(FacilityCategoryServiceObject facilityCategoryServiceObject, CancellationToken token)
        {
            var facilityCategoryEntity = _mapper.Map<FacilityCategoryEntity>(facilityCategoryServiceObject);
            var facilityEntityResponce = await _facilityCategoryRepository.CreateFacilityCategoryAsync(facilityCategoryEntity, token);
            facilityCategoryServiceObject = _mapper.Map<FacilityCategoryServiceObject>(facilityEntityResponce);
            return facilityCategoryServiceObject;
        }
        public async Task<FacilityCategoryServiceObject> UpdateFacilityCategoryAsync(FacilityCategoryServiceObject facilityCategoryServiceObject, CancellationToken token)
        {
            var facilityCategoryData = await _facilityCategoryRepository.GetFacilityCategoryByIdAsync(facilityCategoryServiceObject.FacilityCategoryID, token);
            facilityCategoryData.FacilityCategory = facilityCategoryServiceObject.FacilityCategoryName;
            facilityCategoryData.Status = facilityCategoryServiceObject.Status;
            var regulatoryEntityResponce = await _facilityCategoryRepository.UpdateFacilityCategoryAsync(facilityCategoryData, token);
            facilityCategoryServiceObject = _mapper.Map<FacilityCategoryServiceObject>(regulatoryEntityResponce);
            return facilityCategoryServiceObject;
        }

        public async Task<ICollection<FacilityCategoryServiceObject>> GetFacilityCategoryDDLAsync(CancellationToken token)
        {
            var regulatoryrEntityDDL = await _facilityCategoryRepository.GetFacilityCategoryDDLAsync(token);
            return _mapper.Map<ICollection<FacilityCategoryServiceObject>>(regulatoryrEntityDDL);
        }
    }
}
