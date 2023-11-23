using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IClaimBasketRepository
    {
        Task<ClaimBasketEntity> GetClaimBasketByIdAsync(int ClaimBasketID, CancellationToken token);
        Task<ClaimBasketEntity> CreateClaimBasketAsync(ClaimBasketEntity claimBasketEntity, CancellationToken token);
        Task<List<ClaimBasketEntity>> CreateBulkClaimBasketAsync(List<ClaimBasketEntity> claimBasketEntityList, CancellationToken token);
    }
}
