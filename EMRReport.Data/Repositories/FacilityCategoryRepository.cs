using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class FacilityCategoryRepository : IFacilityCategoryRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public FacilityCategoryRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<FacilityCategoryEntity> GetFacilityCategoryByIdAsync(int facilityCategoryId, CancellationToken token)
        {
            return await _dbContext.FacilityCategory.FirstOrDefaultAsync(x => x.FacilityCategoryId == facilityCategoryId, token);
        }
        public async Task<IEnumerable<FacilityCategoryEntity>> GetFacilityCategoryListByNameAsync(string facilityCategoryName, CancellationToken token)
        {
            facilityCategoryName = !string.IsNullOrEmpty(facilityCategoryName) ? facilityCategoryName.ToLower() : facilityCategoryName;
            return await _dbContext.FacilityCategory.Where(x => x.FacilityCategory.ToLower().Contains(facilityCategoryName)).ToListAsync(token);
        }
        public async Task<IEnumerable<FacilityCategoryEntity>> GetFacilityCategoryListAsync(CancellationToken token)
        {
            return await _dbContext.FacilityCategory.ToListAsync(token);
        }
        public async Task<IEnumerable<FacilityCategoryEntity>> GetFacilityCategoryDDLAsync(CancellationToken token)
        {
            return await _dbContext.FacilityCategory.Select(x => new FacilityCategoryEntity { FacilityCategoryId = x.FacilityCategoryId, FacilityCategory = x.FacilityCategory }).ToListAsync(token);
        }
        public async Task<FacilityCategoryEntity> CreateFacilityCategoryAsync(FacilityCategoryEntity facilityCategoryEntity, CancellationToken token)
        {
            facilityCategoryEntity.Status = true;
            facilityCategoryEntity.FacilityCategoryGuid = Guid.NewGuid();
            facilityCategoryEntity.CreatedBy = UserID;
            facilityCategoryEntity.CreatedDate = DateTime.Now;
            await _dbContext.FacilityCategory.AddAsync(facilityCategoryEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return facilityCategoryEntity;
        }
        public async Task<FacilityCategoryEntity> UpdateFacilityCategoryAsync(FacilityCategoryEntity facilityCategoryEntity, CancellationToken token)
        {
            facilityCategoryEntity.ModifiedBy = UserID;
            facilityCategoryEntity.ModifiedDate = DateTime.Now;
            _dbContext.FacilityCategory.Update(facilityCategoryEntity);
            await _dbContext.SaveChangesAsync(token);
            return facilityCategoryEntity;
        }
    }
}