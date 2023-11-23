using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IClaimDiagnosisRepository
    {
        Task<ClaimDiagnosisEntity> CreateClaimDiagnosisAsync(ClaimDiagnosisEntity claimDiagnosisEntity, CancellationToken token);
        Task<List<ClaimDiagnosisEntity>> CreateBulkClaimDiagnosisAsync(List<ClaimDiagnosisEntity> claimDiagnosisEntityList, CancellationToken token);
    }
}
