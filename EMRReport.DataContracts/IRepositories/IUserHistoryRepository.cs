using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IUserHistoryRepository
    {
        Task<List<UserHistoryEntity>> GetSessionOutUserHistoryListAsync(CancellationToken token);
        Task<UserHistoryEntity> GetUserHistoryByNameAndSessionIDAsync(int UserID, string SessionID, CancellationToken token);
        Task<UserHistoryEntity> SaveLoginUserHistoryAsync(UserHistoryEntity userHistoryEntity, CancellationToken token);
        Task<UserHistoryEntity> SaveLogOutUserHistoryAsync(UserHistoryEntity userHistoryEntity, CancellationToken token);
        Task SaveSessionOutUserHistoryAsync(List<UserHistoryEntity> userHistoryEntityList, CancellationToken token);
    }
}