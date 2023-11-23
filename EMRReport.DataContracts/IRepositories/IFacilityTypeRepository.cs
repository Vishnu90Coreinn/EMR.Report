using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IFacilityTypeRepository
    {
        Task<FacilityTypeEntity> GetFacilityTypeByNameAsync(string facilityTypeName, CancellationToken token);
        Task<IEnumerable<FacilityTypeEntity>> GetFacilityTypeDDLAsync(CancellationToken token);
    }
}