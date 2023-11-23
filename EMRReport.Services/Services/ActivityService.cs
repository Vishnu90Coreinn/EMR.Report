using AutoMapper;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class ActivityService : IActivityService
    {
        private readonly IMapper _mapper;
        private readonly IActivityRepository _activityRepository;
        public ActivityService(IActivityRepository activityRepository, IMapper mapper)
        {
            _activityRepository = activityRepository;
            _mapper = mapper;
        }
        public async Task<ActivityServiceObject> CreateActivityAsync(ActivityServiceObject activityServiceObject, CancellationToken token)
        {
            var activityEntity = _mapper.Map<ActivityEntity>(activityServiceObject);
            var activityEntityResponce = await _activityRepository.CreateActivityAsync(activityEntity, token);
            return _mapper.Map<ActivityServiceObject>(activityEntityResponce);
        }
        public async Task<ActivityServiceObject> FindActivityAsync(int ActivityID, CancellationToken token)
        {
            var activityEntity = await _activityRepository.FindActivityAsync(ActivityID, token);
            return _mapper.Map<ActivityServiceObject>(activityEntity);
        }

        public async Task<ICollection<ActivityServiceObject>> GetActivityListAsync(CancellationToken token)
        {
            var activityEntityList = await _activityRepository.GetActivityListAsync(token);
            return _mapper.Map<ICollection<ActivityServiceObject>>(activityEntityList);
        }

        public async Task<ActivityServiceObject> UpdateActivityAsync(ActivityServiceObject activityServiceObject, CancellationToken token)
        {
            var activityEntity = await _activityRepository.FindActivityAsync(activityServiceObject.ActivityID, token);
            activityEntity.ActivityName = activityServiceObject.ActivityName;
            activityEntity.ActivityNumber = activityServiceObject.ActivityNumber;
            var activityEntityResponce = await _activityRepository.UpdateActivityAsync(activityEntity, token);
            return _mapper.Map<ActivityServiceObject>(activityEntityResponce);
        }
    }
}