using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IUserHistoryService
    {
        Task<UserHistoryServiceObject> SaveLoginUserHistoryAsync(UserHistoryServiceObject userHistoryServiceObject, CancellationToken token);
        Task<UserHistoryServiceObject> SaveLogOutUserHistoryAsync(UserHistoryServiceObject userHistoryServiceObject, CancellationToken token);
        Task SaveSessionOutUserHistoryAsync(CancellationToken token);
    }
}
