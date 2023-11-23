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
    public sealed class ControlRepository : IControlRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public ControlRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<IEnumerable<ControlEntity>> GetControlDDLAsync(CancellationToken token)
        {
            return await _dbContext.Control.Select(x => new ControlEntity { ControlId = x.ControlId, ControlName = x.ControlName }).ToListAsync(token);
        }
    }
}