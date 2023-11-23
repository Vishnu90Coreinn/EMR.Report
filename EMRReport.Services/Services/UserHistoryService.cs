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
    public sealed class UserHistoryService : IUserHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IUserHistoryRepository _userHistoryRepository;
        public UserHistoryService(IUserHistoryRepository userHistoryRepository, IMapper mapper)
        {
            _userHistoryRepository = userHistoryRepository;
            _mapper = mapper;
        }
        public async Task<UserHistoryServiceObject> SaveLoginUserHistoryAsync(UserHistoryServiceObject userHistoryServiceObject, CancellationToken token)
        {
            userHistoryServiceObject.LogInTime = DateTime.Now;
            userHistoryServiceObject.Day = DateTime.Now.DayOfYear;
            var userHistoryEntity = _mapper.Map<UserHistoryEntity>(userHistoryServiceObject);
            var userHistoryEntityResponce = await _userHistoryRepository.SaveLoginUserHistoryAsync(userHistoryEntity, token);
            userHistoryServiceObject = _mapper.Map<UserHistoryServiceObject>(userHistoryEntityResponce);
            await SaveSessionOutUserHistoryAsync(token);// no need to await this fuction
            return userHistoryServiceObject;
        }

        public async Task<UserHistoryServiceObject> SaveLogOutUserHistoryAsync(UserHistoryServiceObject userHistoryServiceObject, CancellationToken token)
        {
            userHistoryServiceObject.LogOutTime = DateTime.Now;
            userHistoryServiceObject.IsLogOut = true;
            var userHistoryEntity = _mapper.Map<UserHistoryEntity>(userHistoryServiceObject);
            var userHistoryEntityResponce = await _userHistoryRepository.SaveLogOutUserHistoryAsync(userHistoryEntity, token);
            userHistoryServiceObject = _mapper.Map<UserHistoryServiceObject>(userHistoryEntityResponce);
            await SaveSessionOutUserHistoryAsync(token);// no need to wait this function
            return userHistoryServiceObject;
        }
        public async Task SaveSessionOutUserHistoryAsync(CancellationToken token)
        {
            var userHistoryEntityList = await _userHistoryRepository.GetSessionOutUserHistoryListAsync(token);
            await Task.Run(() =>
            {
                for (int i = 0; i < userHistoryEntityList.Count; i++)
                {
                    var data = userHistoryEntityList.Skip(i).Take(1).FirstOrDefault();
                    data.SessionOutTime = data.LogInTime.AddHours(3);
                    data.IsSessionOut = true;
                }
            });
            await _userHistoryRepository.SaveSessionOutUserHistoryAsync(userHistoryEntityList, token);
        }
    }
}
