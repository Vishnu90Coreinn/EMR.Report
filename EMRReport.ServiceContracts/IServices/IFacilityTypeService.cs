using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IFacilityTypeService
    {
        Task<FacilityTypeServiceObject> GetFacilityTypeByNameAsync(string facilityTypeName, CancellationToken token);
        Task<ICollection<FacilityTypeServiceObject>> GetFacilityTypeDistinctListFromNameListAsync(ICollection<FacilityTypeServiceObject> facilityTypeServiceObjectList, CancellationToken token);
        Task<ICollection<FacilityTypeServiceObject>> GetFacilityTypeDDLAsync(CancellationToken token);
    }
}
