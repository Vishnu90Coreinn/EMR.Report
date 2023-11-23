using EMRReport.DataContracts.NotMappedEntities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IDashBoardRepository
    {
        Task<List<DashBoardNMEntity>> GetEncounterWiseAsync(DateTime? dateFrom, DateTime? dateTo, int type, CancellationToken token);
        Task<List<DashBoardNMEntity>> GeErrorCategoryWiseAsync(DateTime? dateFrom, DateTime? dateTo, int type, CancellationToken token);
        Task<List<DashBoardNMEntity>> GeErrorSummaryWiseAsync(DateTime? dateFrom, DateTime? dateTo, int type, CancellationToken token);
        Task<List<DashBoardNMEntity>> GeClaimCounterAsync(DateTime? dateFrom, DateTime? dateTo, int type, CancellationToken token);
    }
}
