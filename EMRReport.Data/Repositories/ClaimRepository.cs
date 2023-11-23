using EFCore.BulkExtensions;
using EMRReport.Data;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class ClaimRepository : IClaimRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;

        public ClaimRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<ClaimEntity> CreateClaimAsync(ClaimEntity claimEntity, CancellationToken token)
        {
            await _dbContext.Claim.AddAsync(claimEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return claimEntity;
        }
        public async Task<List<ClaimEntity>> CreateBulkClaimAsync(List<ClaimEntity> claimEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(claimEntityList);
            return claimEntityList;
        }
    }
}