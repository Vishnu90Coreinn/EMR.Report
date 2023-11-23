using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IScrubberErrorService
    {
        Task<string> SaveScrubberXMLFileDataAync(ClaimBasketServiceObject claimBasketServiceObject, bool isDosFacility, bool isAbuDhabiDOS, CancellationToken token);
        Task<string> GetScrubberErrorsAync(IFormFile xMLfile, int basketGroupID, CancellationToken token);
        Task SaveScrubberErrorsFromStoreProcedureAync(ClaimBasketServiceObject claimBasketServiceObject, CancellationToken token);
        Task<Tuple<string, int>> GetScrubberErrorsFromFileCollectionAync(List<IFormFile> XMLfiles, CancellationToken token);
        Task<Tuple<int, List<ScrubberErrorServiceObject>>> GetScrubberErrorsByBasketGroupIdAndTotalAync(int basketGroupID, int page, CancellationToken token);
        Task<List<ScrubberErrorServiceObject>> GetScrubberErrorsByBasketGroupIdAync(int basketGroupID, int page, CancellationToken token);
        Task<List<ScrubberErrorServiceObject>> GetScrubberReportByBasketGroupIdAync(int basketGroupID, bool IsDetail, CancellationToken token);
    }
}
