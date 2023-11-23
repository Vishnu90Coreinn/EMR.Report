using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IClaimActivityRepository
    {
        Task<ClaimActivityEntity> CreateClaimActivityAsync(ClaimActivityEntity claimActivityEntity, CancellationToken token);
        Task<List<ClaimActivityEntity>> CreateBulkClaimActivityAsync(List<ClaimActivityEntity> claimActivityEntityList, CancellationToken token);
    }
}
