using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IStateService
    {
        Task<StateServiceObject> GetStateByNameAsync(string facilityTypeName, CancellationToken token);
        Task<ICollection<StateServiceObject>> GetStateListFromNameListAsync(ICollection<StateServiceObject> stateServiceObjectList, CancellationToken token);
    }
}
