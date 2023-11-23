using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IGroupService
    {
        Task<ICollection<GroupServiceObject>> GetGroupDDLAsync(CancellationToken token);
    }
}
