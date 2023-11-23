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
    public sealed class CountryService : ICountryService
    {
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        public CountryService(ICountryRepository CountryRepository, IMapper mapper)
        {
            _countryRepository = CountryRepository;
            _mapper = mapper;
        }
        public async Task<CountryServiceObject> GetCountryByNameAsync(string countryeName, CancellationToken token)
        {
            var countryEntity = await _countryRepository.GetCountryByNameAsync(countryeName, token);
            return _mapper.Map<CountryServiceObject>(countryEntity);
        }
        public async Task<ICollection<CountryServiceObject>> GetCountryListFromNameListAsync(ICollection<CountryServiceObject> countryServiceObjectList, CancellationToken token)
        {
            ICollection<CountryServiceObject> countryServiceObjectResultList = new List<CountryServiceObject>();
            var countryNameList = countryServiceObjectList.Select(x => x.Country).Distinct().ToArray();
            for (int i = 0; i < countryNameList.Length; i++)
            {
                var data = countryNameList.Skip(i).Take(1).FirstOrDefault();
                if (data != null)
                {
                    CountryServiceObject item = new CountryServiceObject();
                    item = await GetCountryByNameAsync(data, token);
                    if (item != null)
                        countryServiceObjectResultList.Add(item);
                }
            }
            return countryServiceObjectResultList;
        }
    }
}
