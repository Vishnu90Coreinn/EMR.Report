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
    public sealed class EncounterTypeRepository : IEncounterTypeRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public EncounterTypeRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }

        public async Task<EncounterTypeEntity> CreateEncounterTypeAsync(EncounterTypeEntity encounterTypeEntity, CancellationToken token)
        {

            encounterTypeEntity.CreatedBy = UserID;
            encounterTypeEntity.CreatedDate = DateTime.Now;
            encounterTypeEntity.Status = true;
            encounterTypeEntity.EncounterTypeGuid = Guid.NewGuid();
            await _dbContext.EncounterType.AddAsync(encounterTypeEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return encounterTypeEntity;
        }

        public async Task<EncounterTypeEntity> FindEncounterTypeByIDAsync(int ID, CancellationToken token)
        {
            return await _dbContext.EncounterType.FirstOrDefaultAsync(x => x.ID == ID && x.Status, token);
        }

        public async Task<EncounterTypeEntity> GetEncounterTypeByNameAsync(string encounterTypeName, CancellationToken token)
        {
            encounterTypeName = encounterTypeName.ToLower();
            return await _dbContext.EncounterType.FirstOrDefaultAsync(x => x.EncounterType.ToLower().Equals(encounterTypeName), token);
        }

        public async Task<IEnumerable<EncounterTypeEntity>> GetEncounterTypeListAsync(CancellationToken token)
        {
            return await _dbContext.EncounterType.ToListAsync(token);
        }

        public async Task<IEnumerable<EncounterTypeEntity>> GetEncounterTypeListByNameAsync(string encounterTypeName, CancellationToken token)
        {
            encounterTypeName = encounterTypeName.ToLower();
            return await _dbContext.EncounterType.Where(x => x.EncounterType.ToLower().Contains(encounterTypeName)).ToListAsync(token);

        }

        public async Task<EncounterTypeEntity> UpdateEncounterTypeAsync(EncounterTypeEntity encounterTypeEntity, CancellationToken token)
        {
            encounterTypeEntity.ModifiedBy = UserID;
            encounterTypeEntity.ModifiedDate = DateTime.Now;
            _dbContext.EncounterType.Update(encounterTypeEntity);
            await _dbContext.SaveChangesAsync(token);
            return encounterTypeEntity;
        }
    }
}
