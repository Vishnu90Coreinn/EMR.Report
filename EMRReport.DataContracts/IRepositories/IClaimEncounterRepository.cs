using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IClaimEncounterRepository
    {
        Task<ClaimEncounterEntity> CreateClaimEncounterAsync(ClaimEncounterEntity claimEncounterEntity, CancellationToken token);
        Task<List<ClaimEncounterEntity>> CreateBulkClaimEncounterAsync(List<ClaimEncounterEntity> claimEncounterEntityList, CancellationToken token);
    }
}
