using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IActivityService
    {
        Task<ActivityServiceObject> CreateActivityAsync(ActivityServiceObject basketGroupServiceObject, CancellationToken token);
        Task<ActivityServiceObject> UpdateActivityAsync(ActivityServiceObject basketGroupServiceObject, CancellationToken token);
        Task<ActivityServiceObject> FindActivityAsync(int ActivityID, CancellationToken token);
        Task<ICollection<ActivityServiceObject>> GetActivityListAsync(CancellationToken token);
    }
}
