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
    public sealed class RegulatoryRepository : IRegulatoryRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public RegulatoryRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<IEnumerable<RegulatoryEntity>> GetRegulatoryListByNameAsync(string RegulatoryName, CancellationToken token)
        {
            RegulatoryName = !string.IsNullOrEmpty(RegulatoryName) ? RegulatoryName.ToLower() : RegulatoryName;
            return await _dbContext.Regulatory.Where(x => x.RegulatoryName.ToLower().Contains(RegulatoryName)).ToListAsync(token);
        }
        public async Task<IEnumerable<RegulatoryEntity>> GetRegulatoryListAsync(CancellationToken token)
        {
            return await _dbContext.Regulatory.ToListAsync(token);
        }
        public async Task<RegulatoryEntity> GetRegulatoryByNameAsync(string regulatoryName, CancellationToken token)
        {
            regulatoryName = regulatoryName.ToLower();
            return await _dbContext.Regulatory.FirstOrDefaultAsync(x => x.RegulatoryName.ToLower().Equals(regulatoryName), token);
        }
        public async Task<RegulatoryEntity> GetRegulatoryByIdAsync(int regulatoryId, CancellationToken token)
        {
            return await _dbContext.Regulatory.FirstOrDefaultAsync(x => x.RegulatoryID == regulatoryId, token);
        }
        public async Task<IEnumerable<RegulatoryEntity>> GetRegulatoryDDLAsync(CancellationToken token)
        {
            return await _dbContext.Regulatory.Select(x => new RegulatoryEntity { RegulatoryID = x.RegulatoryID, RegulatoryName = x.RegulatoryName }).ToListAsync(token);
        }
        public async Task<RegulatoryEntity> CreateRegulatoryAsync(RegulatoryEntity regulatoryEntity, CancellationToken token)
        {
            regulatoryEntity.Status = true;
            regulatoryEntity.RegulatoryGuid = Guid.NewGuid();
            regulatoryEntity.CreatedBy = UserID;
            regulatoryEntity.CreatedDate = DateTime.Now;
            await _dbContext.Regulatory.AddAsync(regulatoryEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return regulatoryEntity;
        }
        public async Task<RegulatoryEntity> UpdateRegulatoryAsync(RegulatoryEntity regulatoryEntity, CancellationToken token)
        {
            regulatoryEntity.ModifiedBy = UserID;
            regulatoryEntity.ModifiedDate = DateTime.Now;
            _dbContext.Regulatory.Update(regulatoryEntity);
            await _dbContext.SaveChangesAsync(token);
            return regulatoryEntity;
        }
    }
}