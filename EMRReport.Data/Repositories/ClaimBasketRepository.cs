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
    public sealed class ClaimBasketRepository : IClaimBasketRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public ClaimBasketRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<ClaimBasketEntity> CreateClaimBasketAsync(ClaimBasketEntity claimBasketEntity, CancellationToken token)
        {
            await _dbContext.ClaimBasket.AddAsync(claimBasketEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return claimBasketEntity;
        }
        public async Task<List<ClaimBasketEntity>> CreateBulkClaimBasketAsync(List<ClaimBasketEntity> claimBasketEntityList, CancellationToken token)
        {
            await _dbContext.ClaimBasket.AddRangeAsync(claimBasketEntityList, token);
            await _dbContext.SaveChangesAsync(token);
            return claimBasketEntityList;
        }
        public async Task<ClaimBasketEntity> GetClaimBasketByIdAsync(int ClaimBasketID, CancellationToken token)
        {
            return await _dbContext.ClaimBasket.SingleAsync(x => x.ClaimBasketID == ClaimBasketID, token);
        }
    }
}