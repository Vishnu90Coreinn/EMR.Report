using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IActivityRepository
    {
        Task<ActivityEntity> CreateActivityAsync(ActivityEntity basketGroupServiceObject, CancellationToken token);
        Task<ActivityEntity> UpdateActivityAsync(ActivityEntity basketGroupServiceObject, CancellationToken token);
        Task<ActivityEntity> FindActivityAsync(int ActivityID, CancellationToken token);
        Task<ICollection<ActivityEntity>> GetActivityListAsync(CancellationToken token);
    }
}
