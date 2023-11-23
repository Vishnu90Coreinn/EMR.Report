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
    public sealed class StateService : IStateService
    {
        private readonly IMapper _mapper;
        private readonly IStateRepository _stateRepository;
        public StateService(IStateRepository stateRepository, IMapper mapper)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
        }
        public async Task<StateServiceObject> GetStateByNameAsync(string stateName, CancellationToken token)
        {
            var stateEntity = await _stateRepository.GetStateByNameAsync(stateName, token);
            return _mapper.Map<StateServiceObject>(stateEntity);
        }
        public async Task<ICollection<StateServiceObject>> GetStateListFromNameListAsync(ICollection<StateServiceObject> stateServiceObjectList, CancellationToken token)
        {
            ICollection<StateServiceObject> stateServiceObjectResultList = new List<StateServiceObject>();
            var stateNameList = stateServiceObjectList.Select(x => x.State).Distinct().ToArray();
            for (int i = 0; i < stateNameList.Length; i++)
            {
                var data = stateNameList.Skip(i).Take(1).FirstOrDefault();
                if (data != null)
                {
                    StateServiceObject item = new StateServiceObject();
                    item = await GetStateByNameAsync(data, token);
                    if (item != null)
                        stateServiceObjectResultList.Add(item);
                }
            }
            return stateServiceObjectResultList;
        }
    }
}
