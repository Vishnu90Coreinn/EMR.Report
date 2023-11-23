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
    public sealed class ClaimEncounterRepository : IClaimEncounterRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public ClaimEncounterRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<ClaimEncounterEntity> CreateClaimEncounterAsync(ClaimEncounterEntity claimEncounterEntity, CancellationToken token)
        {
            await _dbContext.ClaimEncounter.AddAsync(claimEncounterEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return claimEncounterEntity;
        }
        public async Task<List<ClaimEncounterEntity>> CreateBulkClaimEncounterAsync(List<ClaimEncounterEntity> claimEncounterEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(claimEncounterEntityList);
            return claimEncounterEntityList;
        }
    }
}