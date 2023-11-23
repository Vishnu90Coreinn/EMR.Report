using EFCore.BulkExtensions;
using EMRReport.Data;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class ClaimActivityObservationRepository : IClaimActivityObservationRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;

        public ClaimActivityObservationRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<ClaimActivityObservationEntity> CreateClaimObservationAsync(ClaimActivityObservationEntity claimObservationEntity, CancellationToken token)
        {
            await _dbContext.ClaimObservation.AddAsync(claimObservationEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return claimObservationEntity;
        }
        public async Task<List<ClaimActivityObservationEntity>> CreateBulkClaimActivityObservationAsync(List<ClaimActivityObservationEntity> claimEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(claimEntityList);
            return claimEntityList;
        }
    }
}