using EMRReport.Data;
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
    public sealed class StateRepository : IStateRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public StateRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<StateEntity> GetStateByNameAsync(string stateName, CancellationToken token)
        {
            stateName = stateName.ToLower();
            return await _dbContext.State.FirstOrDefaultAsync(x => x.State.ToLower().Equals(stateName), token);
        }
    }
}