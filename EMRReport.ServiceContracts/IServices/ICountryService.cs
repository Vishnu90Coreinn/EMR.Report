using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface ICountryService
    {
        Task<CountryServiceObject> GetCountryByNameAsync(string countryName, CancellationToken token);
        Task<ICollection<CountryServiceObject>> GetCountryListFromNameListAsync(ICollection<CountryServiceObject> countryTypeServiceObjectList, CancellationToken token);
    }
}
