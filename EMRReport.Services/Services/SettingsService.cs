using AutoMapper;
using EMRReport.Common.ProjectEnums;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class SettingsService : ISettingsService
    {
        private readonly IMapper _mapper;
        private readonly ISettingsRepository _settingsRepository;
        public SettingsService(ISettingsRepository settingsRepository, IMapper mapper)
        {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }
        public async Task<Tuple<DateTime?, DateTime?>> GetDubaiAndAbuDhabiDOS(CancellationToken token)
        {
            var tuple = await _settingsRepository.GetDubaiAndAbuDhabiDOS((int)DOSEnum.ScrubberDOS, (int)DOSEnum.ScrubberAbuDhabiDOS, token);
            return Tuple.Create(tuple.Item1, tuple.Item2);
        }
    }
}
