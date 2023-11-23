using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IGroupRepository
    {
        Task<IEnumerable<GroupEntity>> GetGroupDDLAsync(CancellationToken token);
    }
}