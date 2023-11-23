using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IDashBoardService
    {
        Task<List<DashBoardServiceObject>> GetEncounterWiseAsync(DashBoardServiceObject dashBoardServiceObject, CancellationToken token);
        Task<List<DashBoardServiceObject>> GeErrorCategoryWiseAsync(DashBoardServiceObject dashBoardServiceObject, CancellationToken token);
        Task<List<DashBoardServiceObject>> GeErrorSummaryWiseAsync(DashBoardServiceObject dashBoardServiceObject, CancellationToken token);
        Task<List<DashBoardServiceObject>> GeClaimCounterAsync(DashBoardServiceObject dashBoardServiceObject, CancellationToken token);

    }
}
