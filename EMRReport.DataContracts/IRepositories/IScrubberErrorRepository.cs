using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IScrubberErrorRepository
    {
        Task<List<ScrubberErrorEntity>> CreateBulkScrubberErrorAsync(List<ScrubberErrorEntity> scrubberErrorEntityList, CancellationToken token);
        Task<List<ScrubberErrorEntity>> GetScrubberActivityErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token);
        Task<List<ScrubberErrorEntity>> GetScrubberICDErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token);
        Task<List<ScrubberErrorEntity>> GetScrubberICDCPTErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token);
        Task<List<ScrubberErrorEntity>> GetScrubberObservationErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token);
        Task<List<ScrubberErrorEntity>> GetScrubberNonHitErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token);
        Task<List<ScrubberErrorEntity>> GetScrubberErrorsByBasketGroupIdAync(int basketGroupID, int paage, CancellationToken token);
        Task<Tuple<int, List<ScrubberErrorEntity>>> GetScrubberErrorsByBasketGroupIdAndTotalAync(int basketGroupID, int page, CancellationToken token);
        Task<List<ScrubberErrorEntity>> GetScrubberReportByBasketGroupIdAync(int basketGroupID, bool IsDetail, CancellationToken token);
    }
}
