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
    public sealed class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }
        public async Task<ICollection<GroupServiceObject>> GetGroupDDLAsync(CancellationToken token)
        {
            var groupEntityList = await _groupRepository.GetGroupDDLAsync(token);
            return _mapper.Map<ICollection<GroupServiceObject>>(groupEntityList);
        }
    }
}
