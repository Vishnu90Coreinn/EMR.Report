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
    public sealed class ControlService : IControlService
    {
        private readonly IMapper _mapper;
        private readonly IControlRepository _controlRepository;
        public ControlService(IControlRepository controlRepository, IMapper mapper)
        {
            _controlRepository = controlRepository;
            _mapper = mapper;
        }
        public async Task<ICollection<ControlServiceObject>> GetControlDDLAsync(CancellationToken token)
        {
            var controlEntityList = await _controlRepository.GetControlDDLAsync(token);
            return _mapper.Map<ICollection<ControlServiceObject>>(controlEntityList);
        }
    }
}
