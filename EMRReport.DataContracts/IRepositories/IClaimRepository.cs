using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IClaimRepository
    {
        Task<ClaimEntity> CreateClaimAsync(ClaimEntity claimEntity, CancellationToken token);
        Task<List<ClaimEntity>> CreateBulkClaimAsync(List<ClaimEntity> claimEntityList, CancellationToken token);

    }
}
