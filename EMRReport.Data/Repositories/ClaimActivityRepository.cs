using EFCore.BulkExtensions;
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
    public sealed class ClaimActivityRepository : IClaimActivityRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;

        public ClaimActivityRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<ClaimActivityEntity> CreateClaimActivityAsync(ClaimActivityEntity claimActivityEntity, CancellationToken token)
        {
            await _dbContext.ClaimActivity.AddAsync(claimActivityEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return claimActivityEntity;
        }
        public async Task<List<ClaimActivityEntity>> CreateBulkClaimActivityAsync(List<ClaimActivityEntity> claimActivityEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(claimActivityEntityList);
            return claimActivityEntityList;
        }
    }
}