using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IClaimActivityObservationRepository
    {
        Task<ClaimActivityObservationEntity> CreateClaimObservationAsync(ClaimActivityObservationEntity claimObservationEntity, CancellationToken token);
        Task<List<ClaimActivityObservationEntity>> CreateBulkClaimActivityObservationAsync(List<ClaimActivityObservationEntity> claimObservationEntityList, CancellationToken token);
    }
}
